using System.Web.Mvc;
using MVCtutorial.Graph.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Web.Script.Serialization;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class GraphController : Controller
    {
        // GET: Graph
        private static CIniFile config = new CIniFile();
        private static List<db> openDbList = new List<db>();
        private static string dbConfigPath = @"C:\0\00\grafy.ini";
        private static List<DatabaseDef> dbDefList = new List<DatabaseDef>();

        [HttpPost]
        public void getConfig()
        {
            ViewData["pathConfig"] = Session["pathConfig"];
            ViewData["pathNames"] = Session["pathNames"];
            Iniparser ini = new Iniparser(ViewData["pathConfig"].ToString(), ViewData["pathNames"].ToString());
            ini.ParseNames(config, Const.separators);
            ini.ParseCfg(config, Const.separators);            
            string json = config.toJSON(config);


            Response.ContentType= "application/json";
            Response.Write(json);            
        }

        public ActionResult Index()
        {
            object pathConfig = null;
            object pathNames = null;
            foreach (string key in Session.Keys)
            {
                if (key.Contains("pathConfig") && key.Contains(Request.QueryString["plc"].ToString()))
                {
                   pathConfig = Session[key];
                }
                if (key.Contains("pathNames") && key.Contains(Request.QueryString["plc"].ToString()))
                {                    
                   pathNames = Session[key];
                }
            }
            Session.Add("pathConfig", pathConfig);
            Session.Add("pathNames", pathNames);
            /*
            if (config.ViewList.Count == 0)
            {
                Iniparser ini = new Iniparser(pathConfig.ToString(), pathNames.ToString());
                ini.ParseNames(config, Const.separators);
                ini.ParseCfg(config, Const.separators);
                string json = config.toJSON(config);
            }*/
            return View();
        }
        public string pkTimeToUTC(double time)
        {
            double utcTime = (time / 86400) + 2451544.5;
            string utc = utcTime.ToString();
            if (utc.Contains(",")) {
                utc = utc.Replace(",", ".");
            }
            return Convert.ToString(utcTime);
        }

        public long utcToPkTime(string time)
        {
            try
            {
                double utcTime;
                //time = time.Replace(".", ",");

                //zakrácení času v utc na fixní délku 'od ":" až do konce odmažeme'
                if (time.IndexOf(":") >= 0)
                {
                    int idx = time.IndexOf(":");
                    time = time.Substring(0, idx - 1);
                }
                //fjikosdjfgdk
                utcTime = double.Parse(time);
                utcTime = Math.Round((utcTime - (24515445E-1)) * 86400);
                return (long)utcTime;
            }
            catch (Exception e) {
                string k = e.Message.ToString(); // TODO to txt file
                return 0;
            }
        }
        [HttpPost]
        public async Task<JsonResult> getData()
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string json = stream.ReadToEnd();
            if (json != "")
            {
                object data = new object();
                DataRequest dataRequest = new JavaScriptSerializer().Deserialize<DataRequest>(json);
                try
                {
                    DataRequest dataResponse = await proceedSQLquery(dataRequest);
                    data = dataResponse;
                    return Json(data, "application/json", JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    data = dataRequest;
                    string k = e.Message.ToString(); // TODO to txt file
                    return Json(data, "application/json", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return null;
            }
        }
        private async Task<DataRequest> proceedSQLquery(DataRequest dataRequest) {

            getDbConfig();
            openDBconnections();

            int period = int.MinValue;
            string columns = null;
            string[] conditions1 = { "\"UTC\"", "\"UTC\"" };
            string[] Operators = { ">=", "<=" };
            string[] conditions2 = { "'" + pkTimeToUTC(dataRequest.beginTime) + "'", "'" + pkTimeToUTC(dataRequest.beginTime+dataRequest.timeAxisLength) + "'" };
            List<int> tagsPos = new List<int>();
            List<object[]> objects = new List<object[]>();
            foreach (TableDef tabledef in config.TableDefList) {
                foreach (Tag tag in dataRequest.tags) {
                    if (tabledef.shortName.Contains(tag.table)) {
                        columns += " \"" + tag.column + "\",";
                        tagsPos.Add(dataRequest.tags.IndexOf(tag));
                        period = tag.period;
                    }
                }
                if (columns != null) {
                    columns = columns.Substring(0, columns.Length - 1);
                    db opennedDbConn = openDbList.Find(x => x.dbIdx == tabledef.dbIdx);
                    string where = db.whereMultiple(conditions1, Operators, conditions2);
                    string order = db.order("\"UTC\"", "ASC");
                    objects = await opennedDbConn.multipleItemSelectPostgresAsync("\"UTC\"," + columns, "\"" + tabledef.tabName  + "\"", where, null, order);
                    readResponseforTable(objects, tagsPos, period, dataRequest, tabledef);
                }
                columns = null;
                tagsPos.Clear();
            }
            foreach (db connection in openDbList) {
                connection.connection.Close();
            }
            return dataRequest;
        }

        private void readResponse(List<object[]> rstObjects,DataRequest dataRequest, List<int> tagsPos, TableDef tabledef) {
            int rstPos = 0, buffPos = 0;
            List <double> vals_agreg = new List<double>();
            long time, startTime, endTime, low_buff_time, high_buff_time;
            startTime = dataRequest.beginTime;
            endTime = dataRequest.beginTime + dataRequest.timeAxisLength;
                
                for (int i = 1; i < rstObjects[0].Length; i++)
                {
                    double[] vals_buffer = new double[(dataRequest.timeAxisLength)/dataRequest.tags[tagsPos[i-1]].period]; //prepare values buffer
                    for (int j=0; j<rstObjects.Count; j++) {
                        object[] objectsArray = rstObjects[j];
                        low_buff_time = (startTime + (rstPos * dataRequest.tags[tagsPos[i-1]].period));
                        high_buff_time = (startTime + ((rstPos + 1) * dataRequest.tags[tagsPos[i-1]].period));
                        time = utcToPkTime(objectsArray[0].ToString());
                        if (low_buff_time <= time && high_buff_time >= time)
                        {
                            vals_agreg.Add(Convert.ToDouble(objectsArray[i]));
                            if ((time + dataRequest.tags[tagsPos[i - 1]].period) >= high_buff_time)
                            {
                                if (vals_agreg.Count != 0)
                                {
                                    vals_agreg.Reverse();
                                    vals_buffer[buffPos] = vals_agreg[0];
                                    buffPos++;
                                    vals_agreg.Clear();
                                }
                                else
                                {
                                    if (buffPos < vals_buffer.Length)
                                    {
                                        vals_buffer[buffPos] = double.NaN;
                                        buffPos++;
                                    }
                                }
                            }
                        rstPos++;
                        }
                        else
                        {
                            if (vals_agreg.Count != 0)
                            {
                                vals_agreg.Reverse();
                                vals_buffer[buffPos] = vals_agreg[0];
                                buffPos++;
                                vals_agreg.Clear();
                            }
                            else
                            {
                                if (buffPos < vals_buffer.Length)
                                {
                                    vals_buffer[buffPos] = double.NaN;
                                    buffPos++;
                                    rstPos++;
                                    j--;
                                }
                            } 
                        }
                }

                    rstPos = 0;
                    buffPos = 0; 
                //    dataRequest.tags[tagsPos[i-1]].vals = vals_buffer;
                }
                
            }

        /// <summary>
        /// Method to read response and prepare vals 
        /// </summary>
        /// <param name="rstObjects">result of SQL query</param>
        /// <param name="period">Period of signals in table</param>
        /// <param name="dataRequest">DataRequest</param>
        private void readResponseforTable(List<object[]> rstObjects, List<int> tagsPos, int period, DataRequest dataRequest, TableDef tabledef)
        {
            int rstPos = 0, buffPos = 0;
            object[] objectsArray;
            List<object[]> vals_agreg = new List<object[]>();
            double[][] vals_buffers = new double[(dataRequest.timeAxisLength / period)][];
            long time, startTime, endTime, low_buff_time, high_buff_time;
            startTime = dataRequest.beginTime;
            endTime = dataRequest.beginTime + dataRequest.timeAxisLength;
            foreach (Tag tag in dataRequest.tags)
            {
                tag.vals = new double[dataRequest.timeAxisLength / period];
            }
            for (int i = 0; i < rstObjects.Count; i++)
            {
                objectsArray = rstObjects[i];
                low_buff_time = (startTime + (i * period));
                high_buff_time = (startTime + ((i + 1) * period));
                time = utcToPkTime(objectsArray[0].ToString());

                if (low_buff_time <= time && time <= high_buff_time)
                {
                    vals_agreg.Add(objectsArray);
                    if ((time + tabledef.period) >= high_buff_time)
                    {
                        if (vals_agreg.Count != 0)
                        {
                            vals_agreg.Reverse();
                            for (int j = 1; j < (objectsArray.Length - 1); j++)
                            {
                                double value = Convert.ToDouble(vals_agreg[0][j]);
                                dataRequest.tags[tagsPos[j - 1]].vals[buffPos] = value;
                            }
                            buffPos++;

                            vals_agreg.Clear();
                        }
                        else
                        {
                            if (buffPos <= vals_buffers.Length)
                            {
                                for (int j = 1; j < (objectsArray.Length - 1); j++)
                                {
                                    dataRequest.tags[tagsPos[j - 1]].vals[buffPos] = double.NaN; // missing data adding NaN value to response
                                }
                                buffPos++;
                            }
                        }
                    }
                }
                else
                {
                    if (vals_agreg.Count != 0)
                    {
                        vals_agreg.Reverse();
                        for (int j = 1; j < (objectsArray.Length - 1); j++)
                        {
                            double value = Convert.ToDouble(vals_agreg[0][j]);
                            dataRequest.tags[tagsPos[j - 1]].vals[buffPos] = value; //Adding value to response
                        }
                        buffPos++;
                        vals_agreg.Clear();
                    }
                    else
                    {
                        if (buffPos < vals_buffers.Length)
                        {
                            for (int j = 1; j < (objectsArray.Length - 1); j++)
                            {
                                dataRequest.tags[tagsPos[j - 1]].vals[buffPos] = double.NaN;// missing data adding NaN value to response
                            }
                            buffPos++;
                        }
                    }
                }
            }
        }

        // WARNING: very fast to transform but not very secure way
        private void getDbConfig(){
            int i = 0;
            int dataserverNumber, dbIndex;
            string databaseName = null;
            string[] separeted_string = null;
            string[] separators = {"URL=",",jdbc", ".2.", ":5432/"}; //possible problem
            string[] lines = System.IO.File.ReadAllLines(dbConfigPath, Encoding.Default);
            foreach (string line in lines)
            {
                separeted_string = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (!(lines[i].StartsWith("#")) && (lines[i].Length != 0 && separeted_string.Length > 1))
                {
                        dbIndex = int.Parse(separeted_string[1]);
                        dataserverNumber = int.Parse(separeted_string[3]);
                        databaseName = separeted_string[4];
                        dbDefList.Add(new DatabaseDef() {dbIdx= dbIndex, database = databaseName, dataserverNumber = dataserverNumber });
                }
                i++;
            }            
        }
        private void openDBconnections()
        {
            string database = null;
            int dbIndex, serverNumber = 0;

            foreach (TableDef TableDef in config.TableDefList) { 
                dbIndex = TableDef.dbIdx;
                foreach (DatabaseDef DatabaseDef in dbDefList) {
                    if (DatabaseDef.dbIdx == dbIndex)
                    {
                        database = DatabaseDef.database;
                        serverNumber = DatabaseDef.dataserverNumber;
                    }
                }
                db db = new db(database, serverNumber, dbIndex);
                if (!(openDbList.Exists(x => x.dbIdx == dbIndex)))
                {
                    openDbList.Add(db);
                }
            }
        }

        [HttpPost]
        public JsonResult Config()
        {
            if (config.ViewList.Count == 0)
            {
                ViewData["pathConfig"] = Session["pathConfig"];
                ViewData["pathNames"] = Session["pathNames"];
                Iniparser ini = new Iniparser(ViewData["pathConfig"].ToString(), ViewData["pathNames"].ToString());
                ini.ParseNames(config, Const.separators);
                ini.ParseCfg(config, Const.separators);                
            }
            object data = new object();
            data = config;
            return Json(data, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
