using MVCtutorial.File.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MVCtutorial.Controllers
{
    public class FileController : Controller
    {
        public String path = @"C:\Users\ADMIN\Documents\Visual Studio 2015\Projects\MVCtutorial\MVCtutorial\Config\";
        
        List<String> ProjectNames = new List<string>();
        List<int> ProjectNumbers = new List<int>();

        /* @param String tag, @return List<String> XMLcontentList
         * Method to parse XML nodes from config
         * XMLcontentList is List indexed by integer
        */

        public List<String> readXML(String tag, int ProjectNumber)
        {
            List<String> XMLcontentList = new List<string>();
            XmlDocument xml = new XmlDocument();
            String search_pattern = "config_" + ProjectNumber + "*";
            string[] absoulte_path = Directory.GetFiles(path, search_pattern);
            xml.Load(absoulte_path[0]);
            XmlNodeList xnList = xml.SelectNodes("//" + tag);
            int coutn = xnList.Count;
            
            foreach (XmlNode xn in xnList)
            {
                XmlNodeList nList = xn.ChildNodes;
                foreach (XmlNode n in nList)
                {
                    XMLcontentList.Add(n.InnerText);
                }
            }
            return XMLcontentList;
        }

        public List<String> readNodesNameXML(String tag, int ProjectNumber)
        {
            List<String> XMLNodesName = new List<string>();
            XmlDocument xml = new XmlDocument();
            String search_pattern = "config_" + ProjectNumber + "*";
            string[] absoulte_path = Directory.GetFiles(path, search_pattern);
            xml.Load(absoulte_path[0]);
            XmlNodeList xnList = xml.SelectNodes("//" + tag);
            int coutn = xnList.Count;

            foreach (XmlNode xn in xnList)
            {
                XMLNodesName.Add(xn.Attributes["name"].Value);
                XmlNodeList nList = xn.ChildNodes;
                foreach (XmlNode n in nList)
                {
                    XMLNodesName.Add(n.Name);
                }
            }
            return XMLNodesName;
        }
        /* DONT USE ANY UNDERBRACKET IN THE PATH
         * GetAllConfigsProjectNumbers()
         * @param void, @return List<int> ProjectNumbers
         * Method to return all files (project number) from the path (folder on the end of path)
         */

        public List<int> GetAllConfigsProjectNumbers() {
            
            string[] fileArray = Directory.GetFiles(path, "*.xml");
            foreach (String fileName in fileArray)
            {
                /*
                int start = fileName.LastIndexOf("\\");
                int length = fileName.Length;
                int remainer = fileName.IndexOf("_");
                int substract = length - remainer;
                String projectNumber = fileName.Substring(start+1, length-start-substract-1);
                */
                int length = fileName.Length;
                int start = fileName.IndexOf("_");
                int end = fileName.LastIndexOf("_");
                
                String projectNumber = fileName.Substring(start + 1, end-start-1);
                int temp = Int32.Parse(projectNumber);
                if (!ProjectNumbers.Contains(temp))
                {
                    ProjectNumbers.Add(temp);
                }
            }            
            return ProjectNumbers;
        }
        /* DONT USE ANY UNDERBRACKET IN THE PATH
         * GetAllConfigsNames()
         * @param void, @return List<int> ProjectNames
         * Method to return all names of files from the path (folder on the end of path)
         */
        public List<String> GetAllConfigsNames()
        {
            List<String> XMLcontentList = new List<string>();
            string[] fileArray = Directory.GetFiles(path, "*.xml");
            foreach (String fileName in fileArray)
            {
                int length = fileName.Length;
                int start = fileName.IndexOf("_");
                int end = fileName.LastIndexOf("_");
                
                String projectNumber = fileName.Substring(start + 1, end-start-1);
                int temp = Int32.Parse(projectNumber);
                    XMLcontentList = readXML("name", temp);
                    String Name = XMLcontentList[0];
                    if(!ProjectNames.Contains(Name))
                    {
                        ProjectNames.Add(Name);
                    }
                
            }
            return ProjectNames;
        }

        /* @param String tag, @return List<String> XMLcontentList
         * Method to parse XML nodes from config
         * XMLcontentList is List indexed by integer
        */
        public int XMLtagCount(String tag, int ProjectNumber)
        {
            
            XmlDocument xml = new XmlDocument();
            String search_pattern = "_config_" + ProjectNumber + "*";
            string[] absoulte_path = Directory.GetFiles(path, search_pattern);
            xml.Load(absoulte_path[0]);

            XmlNodeList xnList = xml.SelectNodes("//" + tag);

            int count = xnList.Count;
            return count;
        }

        /* Not prepared for building config file
         * @param FormCollection formCollection, FileFormModel model, @return String Path (full path)
         * Method to create XML File 
        */
        public String CreateXMLFile(FormCollection formCollection, FileFormModel model) {
            //TODO Universal XML creator to prepare XML
            
            String fileName = model.fileName;

            int id = model.id;
            String prefix = model.prefix;
            String country = model.country;
            String name = model.name;
            String description = model.description;

            String full_path = path + id + "_config.xml";
            /*
             * Think about dynamic XML creating
            */
            XmlWriterSettings settings = new XmlWriterSettings();
            using (XmlWriter writer = XmlWriter.Create(full_path, settings)) {
                
                   writer.WriteStartDocument();
                   writer.WriteStartElement("configuration");
                       writer.WriteStartElement("contract");
                           writer.WriteStartElement("id");
                               writer.WriteValue(id);
                           writer.WriteEndElement();
                           writer.WriteStartElement("prefix");
                               writer.WriteValue(prefix);
                           writer.WriteEndElement();
                           writer.WriteStartElement("country");
                               writer.WriteValue(country);
                           writer.WriteEndElement();
                           writer.WriteStartElement("name");
                               writer.WriteValue(name);
                           writer.WriteEndElement();
                           writer.WriteStartElement("description");
                               writer.WriteValue(description);
                           writer.WriteEndElement();
                       writer.WriteEndElement();
                            foreach (string _formData in formCollection)
                            {
                                if (_formData.Contains("alarmDbName"))
                                {
                                    writer.WriteStartElement("alarm");
                                        writer.WriteStartElement(_formData);
                                            writer.WriteValue(formCollection[_formData]);
                                        writer.WriteEndElement();
                                    writer.WriteEndElement();
                                }

                                if (_formData.Contains("scheme"))
                                {
                                    writer.WriteStartElement("scheme");
                                    writer.WriteStartElement(_formData);
                                    writer.WriteValue(formCollection[_formData]);
                                    writer.WriteEndElement();
                                    writer.WriteEndElement();
                                }

                                if (_formData.Contains("report"))
                                {
                                    writer.WriteStartElement("report");
                                    writer.WriteStartElement(_formData);
                                    writer.WriteValue(formCollection[_formData]);
                                    writer.WriteEndElement();
                                    writer.WriteEndElement();
                                }
                            }
                        writer.WriteStartElement("graph");
                            foreach (string _formData in formCollection)
                            {
                                if (_formData.Contains("graph"))
                                {
                                    writer.WriteStartElement(_formData);
                                    writer.WriteValue(formCollection[_formData]);
                                    writer.WriteEndElement();
                                }
                            }
                       writer.WriteEndElement();
                   writer.WriteEndElement();
                   writer.WriteEndDocument();
            }
            return full_path;
        }



        public ActionResult Index()
        {
            List<String> XMLcontentList = new List<string>();
            ProjectNames = GetAllConfigsNames();
            XMLcontentList = readXML("contract", 15021);
            ViewBag.Numbers = ProjectNames;
            return View();
        }

        public ActionResult preXMLForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateXMLForm(preCreateXmlForm model)
        {
            int i = model.alarmsNumber;
            int j = model.graphsNumber;
            int k = model.schemesNumber;
            int l = model.reportsNumber;
            ViewBag.i = i;
            ViewBag.j = j;
            ViewBag.k = k;
            ViewBag.l = l;
            return View();
        }


        [HttpPost]
        public ActionResult CreateXML(FormCollection formCollection, FileFormModel model)
        {
            String test = CreateXMLFile(formCollection, model);
            ViewBag.debug = test;
            return View(model);
        }

    }
}