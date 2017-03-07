using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCtutorial.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Form() {
            return View();
        }
    }
}