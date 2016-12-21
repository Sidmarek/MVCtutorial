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
            CIniFile iniFile = new CIniFile();
            for(int i = 0; i < CIniFile.ViewList.Count; i++) {
                CView view = CIniFile.ViewList[i];
                for (int j = 0; j < view.FieldList.Count; j++)
                {
                    CField field = view.FieldList[j];

                    for (int k = 0; k < field.SigList.Count; k++)
                    {
                        ViewData["json"] = field.SigList[k].ToJson<CSignal>();
                    }
                    for (int l = 0; l < field.SigMultiList.Count; l++)
                    {
                        //ViewData["jsonMul"] = field.SigMultiList[l]
                    }
                }
            }

            return View();
        }

    }
}
