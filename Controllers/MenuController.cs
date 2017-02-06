using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCtutorial.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu\
        [Authorize]
        public ActionResult Index()
        {
            int i = 0;
            getMenu();


            db db = new db();
            object s = db.singleItemSelect("DefaultView", "AspNetUsers", "UserName = '" + User.Identity.Name.ToString() + "'");
            string name = s.ToString();

            foreach (String key in Session.Keys)
            {
                if (key.Contains(name))
                {
                    ViewBag.url = Session[key];
                    i++;
                }
            }

            Session["SchemeURLImage"] = ViewBag.url;


            return View();
        }

        /*
         * @param void, @return redirect 
         * Method to get Menu o
         * to Menu on Index view method
         */
        public ActionResult getMenu()
        {
            String preId;
            int id = 00000;
            if (Request.QueryString["id"] != null)
            {
                preId = Request.QueryString["id"].ToString();
                if (User.IsInRole(preId))
                {
                    id = Int32.Parse(preId);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                if ((Int32.TryParse(Session["id"].ToString(), out id)) != true)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            int i = 0;

            XMLController XC = new XMLController();
            List<String> values = XC.readXML("plc", id);
            List<String> items = XC.readNodesNameXML("plc", id, 3);
            List<String> plc = XC.readNodesNameXML("plc", id, 1);
            List<String> names = XC.readNodesNameXML("plc", id, 2);
            List<String> types = XC.XMLgetTypes("plc", id);
            if (types.Count != 0)
            {
                i = 0;
                foreach (String value in values)
                {
                    Session.Add(items[i], value);
                    i++;
                }
                Session.Add("values", values);

                Session.Add("id", id);
                Session.Add("names", names);
                Session.Add("plc", plc);
                Session.Add("types", types);
                ViewBag.id = id;

                return RedirectToAction("Index", "Menu");
            }
            else {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}