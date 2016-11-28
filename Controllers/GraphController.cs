using System.Web.Mvc;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;
using MVCtutorial.Graph.Models;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class GraphController : Controller
    {
        // GET: Graph
        public ActionResult Index()
        {
            /*
            String name = Request.QueryString["name"];
            String plc = Request.QueryString["plc"];

            

            ViewBag.name = name;
            
            <applet code="GPane.class" archive="gc13.jar" codebase="/java" width="@ViewData["widthgraphplc"]" height="@ViewData["heightgraphplc"]">
                <param name="ViewFile" value="@ViewData["viewFilegraphplc"]">
                <param name="NameFile" value="@ViewData["nameFilegraphplc"]">
                <param name="MessageLevel" value="@ViewData["messageLevelgraphplc"]">
                <param name="Language" value="@ViewData["languagegraphplc"]">
                <param name="LangEnb" value="@ViewData["langEnbgraphplc"]">
                <param name="Timezone" value="@ViewData["timezonegraphplc"]">
            </applet>
            */
            foreach (string key in Session.Keys)
            {
                if (key.Contains("pathConfig"))
                {
                    ViewData["pathConfig"] = Session[key];
                }
                if (key.Contains("pathNames"))
                {
                    ViewData["pathNames"] = Session[key];
                }
            }
            IniConfig ini = new IniConfig(ViewData["pathConfig"].ToString());
            /*
            IniFile MyIni = new IniFile(ViewData["pathConfig"].ToString());
            if (System.IO.File.Exists(ViewData["pathConfig"].ToString()))
            {
                string signal = MyIni.Read("Something");

                ViewData["signal"] = signal;
            }
            else
            {
                Session["tempforview"] = "File has not been found. Please check config file for bakery.";
            }
            */
            return View();
        }

    }

    /// <summary>
    /// Class for parsing .ini configs using reading all lines. -- Bad written ini files (without sections)
    /// </summary>
    class IniConfig
    {
        private string path;
        public IniConfig(string path) {

        }
        public void writeAllLinesCfg()
        {

        }

        /// <summary>
        /// Reads all lines from config files and gives them to array (2D)
        /// </summary>
        /// <param name="separators">Parameter defines seperators as '=', '#', '$' etc.</param>
        /// <returns> Two dimensional Array where first diemension is for structure of config and second for rows</returns>
        public string[][] readAllLines(string[] separators)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            string[] separated_string = null;
            string[][] separated_lines = null;
            int i = 0;
            foreach (string line in lines)
            {
                separated_string = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                separated_lines[i] = separated_string;
                i++;
            }
            return separated_lines;
        }

    }
}