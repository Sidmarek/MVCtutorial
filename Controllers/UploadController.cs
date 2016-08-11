using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCtutorial.Upload.Models;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Web.UI.WebControls.WebParts;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "Upload,Admin")]
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            string ServerPath = Session["pathUploadplc"].ToString();
            string[] files = Directory.GetFiles(ServerPath, "*.*");
            List<string> fileList = new List<string>();
            List<string> fileNames = new List<string>();
            int index;
            string help_string_file;
            string help_string_file_path;
            foreach (string file in files) {
                index = file.LastIndexOf(@"\");
                help_string_file = file.Substring(index+1);
                help_string_file_path = @"9_public\" + help_string_file;
                fileList.Add(help_string_file);
                fileNames.Add(help_string_file_path);
            }
            if (Session["tempforview"] != null)
            {
                ViewBag.message = Session["tempforview"];
                Session["tempforview"] = null;
            }
            ViewBag.fileName = fileList;
            ViewBag.files = fileNames;
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(UploadFile model)
        {
            
            if (ModelState.IsValid)
            {
                try {
                    string filename = model.File.FileName;
                    if (filename.Contains(@"\"))
                    {
                        int index = model.File.FileName.LastIndexOf(@"\");
                        filename = model.File.FileName.Substring(index + 1);
                    }
                    string network_path = Session["pathdownloadplc"].ToString();
                    // Use your file here
                    MemoryStream memoryStream = new MemoryStream();
                    model.File.SaveAs(network_path + @"\9_public\" + filename);
                    //Message for user
                    Session["tempforview"] = "File: " + filename + " has been successfullly uploaded.";
                } catch(Exception ex)
                {
                    Session["tempforview"] = "An error has occured in file uploading:" + ex;
                }
                
            }

            return RedirectToAction("Index", "Upload");
        }
    }

} 