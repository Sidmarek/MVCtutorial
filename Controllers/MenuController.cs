using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            List<String> values = FC.readNodesNameXML("plc",id);
            List<String> names = FC.readNodesNameXML("plc", id);
            ViewBag.names = names;
            List<String> temp = FC.readXML("name",id);
            String Name = temp[0];
            ViewBag.Name = Name;
            ViewBag.id = id;
            return View();
        }
    }
}