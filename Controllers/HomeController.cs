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
            int i = 0;

            foreach (String role in existingRolesForUser)
            {
                if ((Int32.TryParse(role, out id)) == true)
                {
                    Session.Add("id", id);
                    i++;
                }
                if (i >= 2)
                {
                    var exist = Roles.GetAllRoles();
                    String some = User.Identity.Name.ToString();
                    ViewBag.some = some;

                    FileController FC = new FileController();

                    List<int> Numbers = FC.GetAllConfigsProjectNumbers(existingRolesForUser);
                    ViewBag.Numbers = Numbers;
                    ViewBag.Count = Numbers.Count();
                    List<string> Texts = FC.GetAllConfigsNames(existingRolesForUser);
                    ViewBag.Text = Texts;

                    return View();
                }
            }
            if (i == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Menu");
        }
    }
}