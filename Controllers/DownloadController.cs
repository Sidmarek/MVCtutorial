using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Net;
using System.Web.Security;


namespace MVCtutorial.Controllers
{
    [Authorize(Roles = "Download,Admin")]
    public class DownloadController :Controller
    {
        public static String path = @"C:\Users\ADMIN\Documents\Visual Studio 2015\Projects\MVCtutorial\MVCtutorial\Config\";
        //public static string network_path = @"\\192.168.2.20\Public\0\Marek\db\";
        public static string local_path = @"C:\0\00\db\";
        List <String> ProjectNames = new List<string>();
        List<int> ProjectNumbers = new List<int>();
        public string[] absoulte_path;


        /*
         * @param Stirng NetworkPath, string maskfile = null, String extension = "*", @return string[] absoulte_path
         * Method to get all files from server with exact params as mask of the file
         */ 
        public string[] findFilesOnServer(String networkPath, String maskFile = null)
        {
            string maskPath;
            int iHelper;
            if (maskFile == null)
            {
                absoulte_path = Directory.GetFiles(networkPath, "*.*");
            }
            else
            {
                iHelper = maskFile.LastIndexOf(@"\");
                maskPath = maskFile.Substring(0, maskFile.Length - (maskFile.Length - iHelper));
                maskFile = maskFile.Substring(iHelper+1);
                absoulte_path = Directory.GetFiles(networkPath+maskPath, maskFile);
            }
            int count = absoulte_path.Count();

            ViewBag.count = count;
            Session["absoulte_path"]  = absoulte_path;
            return absoulte_path;
        }

        /*
         * @param void, @return List<string> 
         * 
         */
        /// <summary>
        /// Method to select all mask for current bakery
        /// </summary>
        /// <returns>List<string> masks</returns>
        public List<string> selectMasks() {
            db db = new db();
            string mask;
            List<string> masks = new List<string>();
            string[] roles = Roles.GetRolesForUser();
            string someName = "maskRole='" + roles[0] + "'";
            for (int i = 1;i<=(roles.Count()-1);i++) {
                someName += " OR maskRole='" + roles[i] + "'";
            }
            string where = "bakeryId='" + Session["id"] + "'AND " + someName;
            List<object> objectList = db.multipleItemSelect("maskFile", "mask_directory", where);
            foreach (object o in objectList)
            {
                mask = o.ToString();
                masks.Add(mask);
            }
            return masks;
        }
        /// <summary>
        /// method to select roles for all masks
        /// </summary>
        /// <returns>List<string> maskRoles</returns>
        public List<string> selectMasksRoles() {
            db db = new db();
            string maskRole;
            List<string> masksRoles = new List<string>();
            List<object> objectList = db.multipleItemSelect("maskRole", "mask_directory", "bakeryId='" + Session["id"] + "'");
            foreach (object o in objectList)
            {
                maskRole = o.ToString();
                masksRoles.Add(maskRole);
            }
            return masksRoles;
        }
        /// <summary>
        /// Method to select all masks names
        /// </summary>
        /// <returns>List<string> masksNames</returns>
        public List<string> selectMasksNames()
        {
            db db = new db();
            string maskName;
            List<string> masksNames = new List<string>();
            List<object> objectList = db.multipleItemSelect("maskName", "mask_directory", "bakeryId='" + Session["id"] + "'");
            foreach (object o in objectList)
            {
                maskName = o.ToString();
                masksNames.Add(maskName);
            }
            return masksNames;
        }

        /*
         * @param void, @return void
         * Method for download a file
         */
        public void downloadFile() {
                WebClient client = new WebClient();
                String absoultePathToFile = null;
                string nameFile = Request.QueryString["nameFile"].ToString();
                string network_path = Session["pathdownloadplc"].ToString();

                absoultePathToFile = network_path+nameFile;

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
            string network_path = Session["pathdownloadplc"].ToString();
            int network_path_length = network_path.Length;
            List <string> filesToView = new List<string>();
            int i = 0;
            List<string> masks = new List<string>();
            List<string> masksNames = new List<string>();
            masksNames = selectMasksNames();
            masks = selectMasks();
            foreach (string mask in masks)
            {
                if (masksNames[i] != "") {
                    filesToView.Add(masksNames[i]);
                }
                string[] files = findFilesOnServer(network_path, mask);
                if (files.Count() != 0)
                {
                    foreach (string file in files)
                    {
                        string fileToView = file.Substring(network_path_length); 
                        filesToView.Add(fileToView);
                    }
                }
                else
                {
                    filesToView.Add("No files has been found");
                }
                i++;
            }
            ViewBag.files = filesToView;
            ViewBag.masks = masks;
            return View();
        }
    }
}