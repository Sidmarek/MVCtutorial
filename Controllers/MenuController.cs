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
                if ((Int32.TryParse(Session["id"].ToString(), out id)) == true)
                {
                    //out int id = int id 
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            int i = 0;

            FileController FC = new FileController();
            List<String> values = FC.readXML("plc", id);
            List<String> items = FC.readNodesNameXML("plc", id, 3);
            List<String> plc = FC.readNodesNameXML("plc", id, 1);
            List<String> names = FC.readNodesNameXML("plc", id, 2);
            List<String> types = FC.XMLgetTypes("plc", id);

            i = 0;
            foreach (String value in values)
            {
                //String name = names[i] + items[i];
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
    }
}