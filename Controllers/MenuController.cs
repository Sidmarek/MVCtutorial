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
            foreach (String item in values) {
                Session.Add(item, item);
            }
            List<String> names = FC.readNodesNameXML("plc", id);
            Session.Add("id", id);
            Session.Add("names", names);

            List<String> temp = FC.readXML("name",id);
            String Name = temp[0];

            ViewBag.Name = Name;
            ViewBag.id = id;

            return View();
        }
    }
}