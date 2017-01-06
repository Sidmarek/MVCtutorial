using System.Web.Mvc;
using MVCtutorial.Graph.Models;
using System.Net;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class GraphController : Controller
    {

        // GET: Graph
        public ContentResult getConfig()
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
            CIniFile config = new CIniFile();
            ini.ParseNames(config, Const.separators);
            ini.ParseCfg(config, Const.separators, config);
            string json = config.toJSON(config);
            string url = Request.Url.AbsoluteUri;
            
            
            return Content(json, "application/json");
        }
        /*
        public async void asyncConfig(string json, string url) {
            using (var client = new WebClient())
            {
                var response = client.UploadValues()

                var responseString = System.Text.Encoding.Default.GetString(response);
            }

        }*/

        public ActionResult Index()
        {
            getConfig();
            return View();
        }
    }
}
