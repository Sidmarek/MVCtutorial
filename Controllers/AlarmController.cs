using MVCtutorial.Alarms.Models;
using Npgsql;
using System;
using System.Web.Mvc;


namespace MVCtutorial.Controllers
{
    [Authorize]
    public class AlarmController : Controller
    {
        //define int i
        public int i = 0;
        //Predefiened arrays
        public string[] title = new string[1000];
        public int[] alarm_id = new int[1000];
        public int[] id = new int[50];
        public int[] label = new int[50];
        public int[] originTime = new int[50];
        public int[] expTime = new int[50];
        public string[] labels = new string[50];
        public string[] datetimeOrigin = new string[50];
        public string[] datetimeExp = new string[50];

        public String JulToDateTime(int timeForFormat) {
            int date, time, fact, reminder;
            int years, months, days, hours, minutes, seconds;

            date = timeForFormat / 86400;
            time = timeForFormat % 86400;

            seconds = time % 60;
            time = (time - seconds) / 60;
            minutes = time % 60;
            hours = time / 60;

            fact = date * 4;
            fact -= 233;
            years = fact / 1461;

            reminder = (((fact%1461)/4)*5) + 2;
            months = reminder / 153;
            days = (reminder % 153) / 5;

            if (months < 10) {
                months += 3;
            }
            else {
                months -= 9;
                years++;
            }
            years += 2000;
            hours -= 0;

            String DateTime = days + "." + months + "." + years + "      " + hours + ":" + minutes + ":" + seconds;
            return DateTime; 
        }

        public string DBConnnection() {
            // PostgeSQL-style connection string
            string DB = System.IO.File.ReadAllText(@"C:\0\wwwroot\MVCtutorial\Config\config.txt");
            string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
              "192.168.2.12", 5432, "postgres", "Nordit0276", DB);
            return connstring;
        }

        public void SelectAlarms(int NumberOfRecords, int PageNumber) {
            if (NumberOfRecords == 0) {
                NumberOfRecords = 20;
            }
            string connstring = DBConnnection();
            NpgsqlConnection conn = new NpgsqlConnection(connstring);
            conn.Open();
            string DB = System.IO.File.ReadAllText(@"C:\0\wwwroot\MVCtutorial\Config\config.txt");
            string sql = "SELECT * FROM alarm_history ORDER BY origin_pktime DESC LIMIT " + NumberOfRecords + " OFFSET " + (PageNumber * NumberOfRecords);
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            //Prepare DataReader
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                id[i] = Int16.Parse(dr["plc_id"].ToString());
                label[i] = Int16.Parse(dr["alarm_id"].ToString());
                for (int j = 0; j < 1000; j++)
                {
                    if (alarm_id[j] == label[i])
                    {
                        labels[i] = title[j];
                        j = 1100;
                    }
                }
                originTime[i] = Int32.Parse(dr["origin_pktime"].ToString());
                datetimeOrigin[i] = JulToDateTime(originTime[i]);
                expTime[i] = Int32.Parse(dr["expiry_pktime"].ToString());
                datetimeExp[i] = JulToDateTime(expTime[i]);
                i++;
            }
            cmd.Dispose();
            conn.Close();

            //Give page number and number of records on page to view
            ViewBag.PageNumber = PageNumber;
            ViewBag.NumberOfRecords = NumberOfRecords;

            //Give data to view
            ViewBag.Id = label;
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
            NpgsqlCommand command = new NpgsqlCommand("SELECT title,alarm_id FROM alarm_texts LIMIT 1000", conn);
            //Prepare DataReader
            NpgsqlDataReader dataReader = command.ExecuteReader();

            //Cycle reads data from result set 
            while (dataReader.Read())
            {
                title[i] = (dataReader["title"].ToString());
                alarm_id[i] = Int16.Parse(dataReader["alarm_id"].ToString());
                i++;
            }
            //We need to close connection to select texts
            i = 0;
            command.Dispose();
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

                throw msg;
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