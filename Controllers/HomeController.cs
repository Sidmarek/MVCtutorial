using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;

namespace MVCtutorial.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            string userName = User.Identity.Name;
            //Roles.AddUserToRole(userName, "15021");
            string[] some = Roles.GetAllRoles();
            ViewBag.some = some;
            
            FileController FC = new FileController();
            
            List<int> Numbers = FC.GetAllConfigsProjectNumbers();
            ViewBag.Numbers = Numbers;
            ViewBag.Count = Numbers.Count();
            List<string> Texts = FC.GetAllConfigsNames();
            ViewBag.Text = Texts;
            return View();
        }
        public ActionResult News()
        {
            return View();
        }
        public ActionResult Order()
        {
            return View();
        }
    }
}