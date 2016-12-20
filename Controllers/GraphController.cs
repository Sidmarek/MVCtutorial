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
            ini.ParseNames(Const.separators);
            ini.ParseCfg(Const.separators);

            ViewData["tableDef"] = TableDefinition.TableDefList;
            ViewData["views"] = CIniFile.ViewList;
            ViewData["fields"] = CView.FieldList;
            ViewData["signal"] = CField.SigList;
            ViewData["sigMultitext"] = CField.SigMultiList;
            CField.SigList[0].ToJson();


            return View();
        }

    }

    public class Const
    {
        public static readonly string[] separators = { ":", "=", "  ", "             ", ";;", "\n" };
        public static readonly string[] separators_signal = { ":", "=", " ", ",", "  ", "             ", ";;", "\n" };
        public static readonly string[] separ_equate = { "=" };
        public static readonly string[] separ_dollar = { "$" };
        public static readonly string[] separ_names = { "$", "             ", ";" };
        public static readonly string[] separ_backslash = {@"\", "$", "             ", ";" };
    }
}
