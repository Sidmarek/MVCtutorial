﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "View")]
    public class SchemeController : Controller
    {
        // GET: Scheme
        //[Authorize]
        public ActionResult Index()
        {
            int i = 0;
            String name = Request.QueryString["name"];
            String plc = Request.QueryString["plc"];
            foreach (String key in Session.Keys) {
                if (key.Contains(name+plc)) {
                    ViewBag.url = Session[key];
                    i++;
                }
            }

            Session["SchemeURLImage"] = ViewBag.url;
            ViewBag.name = name;
            return View();
        }

        public void getImage() {
            string url = Session["SchemeURLImage"].ToString();

            WebClient client = new WebClient();
            byte[] data = client.DownloadData(url);
            //MemoryStream mem = new MemoryStream(data);

            //var yourImage = Image.FromStream(mem);
            //yourImage.                        
            //yourImage.Save(path, ImageFormat.Png);
            Response.BinaryWrite(data);
            Response.ContentType = "image/png";
        }
    }
}