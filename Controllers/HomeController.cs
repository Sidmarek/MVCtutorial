using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.SqlClient;
using System.Configuration;

namespace MVCtutorial.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        [Authorize]
        public ActionResult Homepage()
        {
            string[] existingRolesForUser = Roles.GetRolesForUser();
            int id;
            int projectsCount = 0;

            foreach (String role in existingRolesForUser)
            {
                if ((Int32.TryParse(role, out id)) == true)
                {
                    Session.Add("id", id);
                    projectsCount++;
                }
                if (projectsCount >= 2)
                {
                    var exist = Roles.GetAllRoles();
                    String some = User.Identity.Name.ToString();
                    ViewBag.some = some;

                    XMLController XC = new XMLController();

                    List<int> Numbers = XC.GetAllConfigsProjectNumbers(existingRolesForUser);
                    ViewBag.Numbers = Numbers;
                    ViewBag.Count = Numbers.Count();
                    List<string> Texts = XC.GetAllConfigsNames(existingRolesForUser);
                    ViewBag.Text = Texts;

                    return View();
                }
            }
            if (projectsCount <= 1)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Menu");
        }
    }
}