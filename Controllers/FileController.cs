using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Net;

namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FileController :Controller
    {
        public static String path = @"C:\Users\ADMIN\Documents\Visual Studio 2015\Projects\MVCtutorial\MVCtutorial\Config\";
        public static string network_path = @"\\192.168.2.20\Public\0\Marek\db\";
        public static string local_path = @"C:\0\00\db\";
        List <String> ProjectNames = new List<string>();
        List<int> ProjectNumbers = new List<int>();
        public string[] absoulte_path;


        /*
         * @param Stirng NetworkPath, string maskfile = null, String extension = "*", @return string[] absoulte_path
         * Method to get all files from server with exact params as mask of the file
         */ 
        public string[] findFilesOnServer(String networkPath, String maskFile = null, String extension = "*")
        {
            string maskPath;
            int iHelper;
            if (maskFile == null)
            {
                absoulte_path = Directory.GetFiles(networkPath, "*."+ extension);
            }
            else
            {
                iHelper = maskFile.LastIndexOf(@"\");
                maskPath = maskFile.Substring(0, maskFile.Length - (maskFile.Length - iHelper));
                maskFile = maskFile.Substring(iHelper+1);
                absoulte_path = Directory.GetFiles(networkPath+maskPath, maskFile + "*." + extension);
            }
            int count = absoulte_path.Count();

            ViewBag.count = count;
            Session["absoulte_path"]  = absoulte_path;
            return absoulte_path;
        }

        /*
         * @param void, @return List<string> 
         * Method to select all mask for current bakery
         */ 
        public List<string> selectMasks() {
            db db = new db();
            string mask;
            List<string> masks = new List<string>();
            List<object> objectList = db.multipleItemSelect("maskFile", "mask_directory", "bakeryId='" + Session["id"] + "'");
            foreach (object o in objectList)
            {
                mask = o.ToString();
                masks.Add(mask);
            }
            return masks;
        }

        /*
         * @param void, @return void
         * Method for download a file
         */
        public void downloadFile() {
                WebClient client = new WebClient();
                String absoultePathToFile = null;
                String nameFile = Request.QueryString["nameFile"].ToString();

                ViewBag.s = Session["absoulte_path"];
                foreach (string path in ViewBag.s) {
                    if (path.Contains(network_path + nameFile)) {
                        absoultePathToFile = path;
                    }
                }
                Response.ContentType = "application/octet-stream";


                if (absoultePathToFile == null)
                {
                    Session["tempforview"] = "Error: Your file has been not found";
                }
                else
                {

                    if (Request.QueryString["View"] != null) 
                    {
                        if (absoultePathToFile.Contains(".pdf"))
                        {
                            Response.ContentType = "application/pdf"; //change content type for pdf files
                        }
                        else
                        {
                            Response.ContentType = "text/plain"; //change content type for txt files
                        }
                        Response.TransmitFile(absoultePathToFile);//For View the file
                    }
                    else {                    
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + nameFile);
                        Response.TransmitFile(absoultePathToFile); //For download file
                        Response.Flush(); //For download file
                    }
            }
        }

        public ActionResult Index()
        {
            List<string> filesToView = new List<string>();
            string fileName;
            int iHelper;
            List<string> masks = new List<string>();
            masks = selectMasks();
            foreach (string mask in masks)
            {
                string[] files = findFilesOnServer(network_path, mask);
                foreach (string file in files) {
                    iHelper = file.LastIndexOf(@"\");
                    fileName = file.Substring(iHelper+1);
                    filesToView.Add(fileName);
                }

            }
            ViewBag.files = filesToView;
            ViewBag.masks = masks;
            return View();
        }
    }
}