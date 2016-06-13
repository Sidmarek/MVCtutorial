using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class GraphController : Controller
    {
        // GET: Graph
        public ActionResult Index()
        {
            int i = 0;
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

            ViewBag.name = name;
            return View();
        }
    }
}