using MVCtutorial.Alarms.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace MVCtutorial.Controllers
{
    [Authorize]
    public class AlarmController : Controller
    {
        //define variables
        public int i = 0;
        string DB;
        //Predefiened arrays

        public List<string> titles = new List<string>();
        public List<int> alarm_ids = new List<int>();
        public int[] id = new int[32];
        public int[] alarm_id = new int[32];
        public int[] originTime = new int[32];
        public int[] expTime = new int[32];
        public string[] labels = new string[32];
        public DateTime[] datetimeOrigin = new DateTime[32];
        public DateTime[] datetimeExp = new DateTime[32];

        public DateTime pkTimeToDateTime(long timeForFormat)
        {
            long timeInNanoSeconds = (timeForFormat * 10000000);
            //TimeSpan converted = TimeSpan.FromSeconds(timeForFormat);
            DateTime DateTime = new DateTime(((630836424000000000 - 13608000000000) + timeInNanoSeconds));
            //DateTime DateTime = new DateTime(years, months, days, hours, minutes, seconds);
            return DateTime;
         }

        public string DBConnnection()
        {

                // PostgeSQL-style connection string            
                foreach (string key in Session.Keys)
                {
                    if (key.Contains("dbName" + Request.QueryString["name"] + Request.QueryString["plc"]))
                    {
                        DB = Session[key.ToString()].ToString();
                    }
                }
            string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
              "192.168.2.12", 5432, "postgres", "Nordit0276", DB);
            return connstring;
        }

        public void SelectAlarms(int NumberOfRecords, int PageNumber)
        {
            if (NumberOfRecords == 0)
            {
                NumberOfRecords = 20;
            }
            string connstring = DBConnnection();
            NpgsqlConnection conn = new NpgsqlConnection(connstring);
            conn.Open();
            int plcID = 1;
            string sql = "SELECT * FROM alarm_history WHERE plc_id=" + plcID + " ORDER BY origin_pktime DESC LIMIT " + NumberOfRecords + " OFFSET " + (PageNumber * NumberOfRecords);
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            //Prepare DataReader
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                id[i] = Int16.Parse(dr["plc_id"].ToString());
                alarm_id[i] = Int16.Parse(dr["alarm_id"].ToString());
                //small improvment beacause alarm_id in table alarm_texts and alarm_id in table alarm_history are bind
                labels[i] = titles[alarm_id[i]];

                originTime[i] = Int32.Parse(dr["origin_pktime"].ToString());
                datetimeOrigin[i] = pkTimeToDateTime(originTime[i]);
                expTime[i] = Int32.Parse(dr["expiry_pktime"].ToString());
                datetimeExp[i] = pkTimeToDateTime(expTime[i]);
                i++;
            }
            //cmd.Dispose();
            conn.Close();

            //Give page number and number of records on page to view
            ViewBag.PageNumber = PageNumber;
            ViewBag.NumberOfRecords = NumberOfRecords;

            //Give data to view
            ViewBag.Id = alarm_id;
            ViewBag.Label = labels;
            ViewBag.originTime = datetimeOrigin;
            ViewBag.expTime = datetimeExp;
        }

        public void SelectAlarmsTexts()
        {
            // Making connection with Npgsql provider
            string connstring = DBConnnection();
            NpgsqlConnection conn = new NpgsqlConnection(connstring);
            conn.Open();

            // Execute the query and obtain a result set                
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT title,alarm_id FROM alarm_texts", conn);
            //Prepare DataReader
            NpgsqlDataReader dataReader = cmd.ExecuteReader();

            //Cycle reads data from result set 
            while (dataReader.Read())
            {
                titles.Add(dataReader["title"].ToString());
                alarm_ids.Add(Int16.Parse(dataReader["alarm_id"].ToString()));

            }
            //We need to close connection to select texts
            //cmd.Dispose();
            conn.Close();

        }
        public ActionResult Index()
        {

            try
            {

                SelectAlarmsTexts();
                //--------------------------------------------------------------------------

                //Select Alarms using SelectAlarms(int count) method (defined upper )
                int count = 20;
                SelectAlarms(count, 0);

            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why

                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        /*

        public ActionResult Form()
        {
            return View();
        }
        */

        // POST: Alarm
        [HttpPost]
        public ActionResult Form(AlarmFormModel model, string returnUrl)
        {

            try
            {
                SelectAlarmsTexts();

                //--------------------------------------------------------------------------


                int PageNumber = model.someId;
                int NumberOfRecords = model.NumberOfRecords;
                SelectAlarms(NumberOfRecords, PageNumber);

            }
            catch (Exception msg)
            {
                // something went wrong, and you wanna know why

                throw msg;
            }

            return View(model);
        }
    }
}