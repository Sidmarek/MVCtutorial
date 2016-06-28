using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCtutorial.CMS.Models;
using System;

namespace MVCtutorial.Controllers
{
    public class CMSController : Controller
    {
        /// <summary>
        /// Actually Index mean list of articles
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Index()
        {
            db db = new db();
            List<object> objects = new List<object>();
            List<string> test = new List<string>();
            objects = db.multipleItemSelect("Date,Author,Header,Text", "Articles", "bakeryId=" + Session["id"]);
            foreach (object o in objects)
            {
                test.Add(o.ToString());
            }
            ViewBag.data = test;
            ViewBag.count = test.Count();
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// Add informations for project
        /// </summary>
        /// <param name="model">Method to evaluate form</param>
        /// <returns>Redirect back to action Add</returns>
        [HttpPost]
        public ActionResult AddArticle(AddProjectArticle model)
        {
            int bakeryId;
            if (model.bakeryId == null)
            {
                bakeryId = Int32.Parse(Session["id"].ToString());
            }
            else
            {
                bakeryId = Int32.Parse(model.bakeryId.ToString());
            }
            db db = new db();
            string text = model.Text.ToString();
            text = text.Replace("\r\n", "<br>");
            
            db.singleItemInsertAsync("Articles", "bakeryId,Author,Header,Text", bakeryId + ",'" + User.Identity.Name.ToString() + "','" + model.Header + "','" + text + "'"); //TODO solve potential SQL injections
            Session["tempforview"] = "Your status informations has been successfully added to bakery: " + model.bakeryId + ".";
            return RedirectToAction("Add", "CMS");
        }
    }
}