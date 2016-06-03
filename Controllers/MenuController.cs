using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace MVCtutorial.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu\
        [Authorize]
        public ActionResult Index()
        {
            int id = int.Parse(Request.QueryString["id"]);
            FileController FC = new FileController();

            List<String> values = FC.readXML("plc",id);
            List<String> items = FC.readNodesNameXML("plc", id, 3);

            int i = 0;
            foreach (String value in values) {
                Session.Add(items[i], value);
                i++;
            }

            List<String> names = FC.readNodesNameXML("plc", id, 2);
            Session.Add("id", id);
            Session.Add("names", names);
                        
            //ViewBag.Name = Name;
            ViewBag.id = id;

            return View();
        }
    }
}