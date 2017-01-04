using System.Web.Mvc;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;
using MVCtutorial.Graph.Models;
using System.Net;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class GraphController : Controller
    {
        // GET: Graph
        public ActionResult Index()
        {
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
            ToJSON ToJSON = new ToJSON();
            ini.ParseNames(Const.separators);
            ini.ParseCfg(Const.separators);
            CIniFile iniFile = new CIniFile();
            string json = iniFile.toJSON(iniFile);
            Response.ContentType = "application/json";
            Response.Write(json);
            Response.Flush();
            /*
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                result = client.UploadString(url, "POST", json);
            }*/
            return View();
        }

    }
}
