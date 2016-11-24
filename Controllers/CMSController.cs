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
            List<object> objects_id = new List<object>();
            List<string> strings = new List<string>();
            List<int> ids = new List<int>();
            objects = db.multipleItemSelect("Date,Author,Header,Text", "Articles", "bakeryId=" + Session["id"].ToString());
            foreach (object o in objects)
            {
                strings.Add(o.ToString());
            }
            objects_id = db.multipleItemSelect("Id", "Articles", "bakeryId=" + Session["id"].ToString());
            foreach (object o in objects_id)
            {
                ids.Add(int.Parse(o.ToString()));
            }
            if (Session["tempforview"] != null)
            {
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }
            ViewBag.Id = ids;
            ViewBag.data = strings;
            ViewBag.count = strings.Count();
            return View();
        }
        public ActionResult Delete()
        {
            if (User.IsInRole("programmer"))
            {
                string PostId = Request.QueryString["PostId"].ToString();
                db db = new db();
                db.singleItemDeleteAsync("Articles", "Id=" + PostId);
            }
            Session["tempforview"] = "Post has been deleted";
            return RedirectToAction("Index", "CMS");
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
            if (ModelState.IsValid)
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
            else {
                Session["tempforview"] = "Empty Subject or Project info status";
                return RedirectToAction("Add", "CMS");
            }
        }
    }
}