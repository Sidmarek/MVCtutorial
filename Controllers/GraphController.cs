using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCtutorial.Controllers
{
    public class GraphController : Controller
    {
        // GET: Graph
        public ActionResult Index()
        {
            int i = 0;
            int id = int.Parse(Request.QueryString["id"]);
            String name = Request.QueryString["name"];
            String plc = Request.QueryString["plc"];

            foreach (String key in Session.Keys)
            {
                if (key.Contains("graphplc"))
                {
                    ViewData[key] = Session[key];
                    i++;
                }
            }

            ViewBag.id = id;
            ViewBag.name = name;
            return View();
        }
    }
}