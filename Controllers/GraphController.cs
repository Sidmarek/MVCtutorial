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
            Iniparser ini = new Iniparser(ViewData["pathConfig"].ToString(), ViewData["pathNames"].ToString());
            ini.ParseNames(Const.Separ_Space, );
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

    public class Const
    {
        public static readonly string[] Separ_Space = {":", "=", "  ", "             ", ";;", "\n"};
        public static readonly string[] Separ_equate = { "=" };
    }


    /// <summary>
    /// Class for parsing .ini configs using reading all lines. -- Bad written ini files (without sections)
    /// </summary>
    class Iniparser
    {
        private static string Path;
        private static string NamePath;
        public Iniparser(string apath, string aNamePath) {
            Path = apath;
            NamePath = aNamePath;
        }
        public void ParseNames (string[] separators)
        {
            string[] lines = System.IO.File.ReadAllLines(NamePath);

            string[] separeted_string = null ;
            string[] multitext_line = null, line_with_id = null;
            List<string[]> multitext_lines = new List<string[]>();

            string multitext_name = null;
            string line_without_id = null;
            string name_line = null;
            string name_help_line = null;

            List<int> Idxs = new List<int>();
            int i = 0;
            int position;
            

            foreach (string line in lines)
            {
                separeted_string = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (!(separeted_string.Length==0))
                {
                    switch (separeted_string[0])
                    {
                        case "DefineMultitext":
                           multitext_name = separeted_string[1];
                            for (int j=(i+1); j < lines.Length; j++)
                            {
                                if (lines[j].Length == 0)
                                {
                                    j = lines.Length;
                                    TextlistDefinition.Add(multitext_name,multitext_lines, Idxs);
                                }
                                else
                                {
                                    line_with_id = lines[j].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    Idxs.Add(int.Parse(line_with_id[0]));
                                    position = lines[j].LastIndexOf(';');
                                    line_without_id = lines[j].Substring(position+1);
                                    multitext_line = line_without_id.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    multitext_lines.Add(multitext_line);
                                }
                            }
                        break;
                        case "# =":
                            for (int k = i; k < lines.Length; k++)
                                if (!(lines[k].Length == 0))
                                {
                                    name_line = lines[k].Split(Const.Separ_equate, StringSplitOptions.RemoveEmptyEntries);
                                }
                        break;
                    }
                }               
                i++;
            }
        }

        public string[][] ParseCfgNames(string[] separators)
        {
            string[] lines = System.IO.File.ReadAllLines(Path);
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

        /// <summary>
        /// Reads all lines from config files and gives them to array (2D)
        /// </summary>
        /// <param name="separators">Parameter defines seperators as '=', '#', '$' etc.</param>
        /// <returns> Two dimensional Array where first diemension is for structure of config and second for rows</returns>
        public string[][] readAllLines(string[] separators)
        {
            string[] lines = System.IO.File.ReadAllLines(Path);
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