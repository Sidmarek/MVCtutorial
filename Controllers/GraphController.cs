using System.Web.Mvc;
using MVCtutorial.Graph.Models;
using System.Net;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class GraphController : Controller
    {

        // GET: Graph
        public void getConfig()
        {

            ViewData["pathConfig"] = Session["pathConfig"];
            ViewData["pathNames"] = Session["pathNames"];
            Iniparser ini = new Iniparser(ViewData["pathConfig"].ToString(), ViewData["pathNames"].ToString());
            ToJSON ToJSON = new ToJSON();
            CIniFile config = new CIniFile();
            ini.ParseNames(config, Const.separators);
            ini.ParseCfg(config, Const.separators, config);
            ini.ParseCfg(config, Const.separators, config);
            
            string json = config.toJSON(config);

            //string jsonSerialized = JsonConvert.SerializeObject(config);
            Response.ContentType= "application/json";
            Response.Write(json);
            RedirectToAction("Index");
            //CIniFile deserializedIniFile = JsonConvert.DeserializeObject<CIniFile>(json);
            //return Content(json, "application/json", System.Text.Encoding.Default);
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
            //ContentResult Configjson = getConfig();
            Iniparser ini = new Iniparser(pathConfig.ToString(), pathNames.ToString());
            ToJSON ToJSON = new ToJSON();
            CIniFile config = new CIniFile();
            ini.ParseNames(config, Const.separators);
            ini.ParseCfg(config, Const.separators, config);
            ini.ParseCfg(config, Const.separators, config);
            string json = config.toJSON(config);
            Config(config);
            getData("Gida");

            return View();
        }

        public void getData(string database)
        {
            db db = new db(database);
            //return Json(data, "application/json", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Config(CIniFile config)
        {
            object data = new object();
            data = config;
            return Json(data, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
