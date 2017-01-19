using System.Web.Mvc;
using MVCtutorial.Graph.Models;
using System.Net;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Net.Http;
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
            if (config.ViewList.Count == 0)
            {
                ViewData["pathConfig"] = Session["pathConfig"];
                ViewData["pathNames"] = Session["pathNames"];
                Iniparser ini = new Iniparser(ViewData["pathConfig"].ToString(), ViewData["pathNames"].ToString());
                ini.ParseNames(config, Const.separators);
                ini.ParseCfg(config, Const.separators, config);
                ini.ParseCfg(config, Const.separators, config);
            }
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
            
            Iniparser ini = new Iniparser(pathConfig.ToString(), pathNames.ToString());
            ini.ParseNames(config, Const.separators);
            ini.ParseCfg(config, Const.separators, config);
            ini.ParseCfg(config, Const.separators, config);
            string json = config.toJSON(config);

            return View();
        }
        public string pkTimeToUTC(double time)
        {
            double utcTime = (time / 86400) + 2451544.5;
            return Convert.ToString(utcTime);
        }

        public long utcToPkTime(string time)
        {
            double utcTime;
            //time = time.Replace(".", ",");

            //zakrácení času v utc na fixní délku 'od ":" až do konce odmažeme'
            if (time.IndexOf(":") >= 0)
            {
                int idx = time.IndexOf(":");
                time = time.Substring(0, idx - 1);
            }

            utcTime = double.Parse(time);
            utcTime = Math.Round((utcTime - (24515445E-1)) * 86400);
            return (long)utcTime;
        }
        [HttpPost]
        public async Task<JsonResult> getData()
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string json = await stream.ReadToEndAsync();
            object data = new object();
            DataRequest dataRequest = new JavaScriptSerializer().Deserialize<DataRequest>(json);
            proceedSQLquery(dataRequest);
            data = json;
            
            return Json(data, "application/json", JsonRequestBehavior.AllowGet);
        }
        private async void proceedSQLquery(DataRequest dataRequest) {

            getDbConfig();
            openDBconnections();

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
                    }
                }
                if (columns != null) {
                    columns = columns.Substring(0, columns.Length - 1);
                    db opennedDbConn = openDbList.Find(x => x.dbIdx == tabledef.dbIdx);
                    string where = db.whereMultiple(conditions1, Operators, conditions2);
                    objects = await opennedDbConn.multipleItemSelectPostgresAsync("\"UTC\"," + columns, "\"" + tabledef.tabName  + "\"", where);
                    readResponse(objects, dataRequest, tagsPos);
                }
                columns = null;
                tagsPos.Clear();
            }
            foreach (db connection in openDbList) {
                connection.connection.Close();
            }
        }

        private void readResponse(List<object[]> rstObjects,DataRequest dataRequest, List<int> tagsPos) {
            int rstPos = 0, buffPos = 0;
            List <long> vals_agreg = new List<long>();
            long time, startTime, endTime;            
            startTime = dataRequest.beginTime;
            endTime = dataRequest.beginTime + dataRequest.timeAxisLength;
            
            for (int i = 1; i < rstObjects[0].Length; i++)
            {
                string[] vals_buffer = new string[(dataRequest.timeAxisLength)/dataRequest.tags[tagsPos[i]].period]; //prepare values buffer
                foreach (object[] objectsArray in rstObjects) {
                    time = utcToPkTime(objectsArray[0].ToString());
                    if ((startTime + (rstPos * dataRequest.tags[tagsPos[i]].period)) < time && (startTime + ((rstPos + 1) * dataRequest.tags[tagsPos[i]].period)) > time)
                    {
                        vals_agreg.Add(Convert.ToInt64(objectsArray[i]));
                    }
                    else {
                        if (vals_agreg.Count != 0)
                        {
                            vals_agreg.Reverse();
                            vals_buffer[buffPos] = vals_agreg[0].ToString();
                            buffPos++;
                            rstPos++;
                            vals_agreg.Clear();
                        }
                        else {
                            if (buffPos < vals_buffer.Length)
                            {
                                vals_buffer[buffPos] = null;
                                buffPos++;
                                rstPos++;
                            }
                        }
                    }
                }
                dataRequest.tags[i].vals = vals_buffer;
            }
        }
        
        // WARNING: very fast to transform but not very secure way
        private void getDbConfig(){
            int i =0;
            int dataserverNumber, dbIndex;
            string databaseName = null;
            string[] separeted_string = null;
            string[] separators = {"URL=",",jdbc", ".2.", ":5432/"}; 
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
                ini.ParseCfg(config, Const.separators, config);
                ini.ParseCfg(config, Const.separators, config);
            }
            object data = new object();
            data = config;
            return Json(data, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
