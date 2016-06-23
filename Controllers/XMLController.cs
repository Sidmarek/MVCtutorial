using MVCtutorial.File.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Xml;

namespace MVCtutorial.Controllers
{
    public class XMLController : Controller
    {
        public static String path = @"C:\Users\ADMIN\Documents\Visual Studio 2015\Projects\MVCtutorial\MVCtutorial\Config\";
        List<String> ProjectNames = new List<string>();
        List<int> ProjectNumbers = new List<int>();
        public string[] absoulte_path;

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

            foreach (XmlNode xn in xnList)
            {
                XmlNodeList nList = xn.ChildNodes;
                foreach (XmlNode n in nList)
                {
                    XmlNodeList nodeList = n.ChildNodes;
                    foreach (XmlNode node in nodeList)
                    {
                        XMLcontentList.Add(node.InnerText);
                    }
                }


            }

            return XMLcontentList;
        }

        /*
         * @param String tag, @return string text
         * Method to read exact XML node inner text 
         */
        public string readNodeXML(String tag, int ProjectNumber)
        {
            XmlDocument xml = new XmlDocument();
            String search_pattern = "config_" + ProjectNumber + "*";
            string[] absoulte_path = Directory.GetFiles(path, search_pattern);
            xml.Load(absoulte_path[0]);

            XmlNodeList xnList = xml.SelectNodes("//" + tag);
            string text = xnList[0].InnerText;

            return text;
        }

        /* @param String tag, @return List<String> XMLcontentList
         * Method to parse XML nodes from config
         * XMLcontentList is List indexed by integer
         */
        public List<String> readNodesNameXML(String tag, int ProjectNumber, int xmlDeepLevel)
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
                if (xmlDeepLevel == 1)
                {
                    XmlAttribute attributeNamePlc = xn.Attributes["name"];
                    if (attributeNamePlc == null)
                    {
                        XMLNodesName.Add(xn.Name);
                    }
                    else
                    {
                        XMLNodesName.Add(xn.Attributes["name"].Value);
                    }
                }

                XmlNodeList nList = xn.ChildNodes;

                foreach (XmlNode n in nList)
                {
                    if (xmlDeepLevel == 2)
                    {
                        XmlAttribute attributeNameOther = n.Attributes["name"];
                        if (attributeNameOther == null)
                        {
                            XMLNodesName.Add(n.Name);
                        }
                        else
                        {
                            XMLNodesName.Add(n.Attributes["name"].Value);
                        }
                    }
                    if (xmlDeepLevel >= 3)
                    {
                        XmlNodeList nodeList = n.ChildNodes;
                        foreach (XmlNode node in nodeList)
                        {
                            String parentNode;
                            String grandParentNode;
                            XmlAttribute attributeNameOther = n.Attributes["name"];
                            if (attributeNameOther == null)
                            {
                                grandParentNode = node.ParentNode.ParentNode.Name;
                                parentNode = node.ParentNode.Name;

                            }
                            else
                            {
                                grandParentNode = node.ParentNode.ParentNode.Attributes["name"].Value;
                                parentNode = n.Attributes["name"].Value;
                            }
                            XMLNodesName.Add(node.Name + parentNode + grandParentNode);
                        }
                    }
                }
            }
            return XMLNodesName;
        }

        /* DONT USE ANY UNDERBRACKET IN THE PATH
         * GetAllConfigsProjectNumbers()
         * @param void, @return List<int> ProjectNumbers
         * Method to return all files (project number) from the path (folder on the end of path)
         */

        public List<int> GetAllConfigsProjectNumbers(string[] IDs)
        {
            foreach (String ID in IDs)
            {
                string[] fileArray = Directory.GetFiles(path, "config_" + ID + "*.xml");
                foreach (String fileName in fileArray)
                {
                    int length = fileName.Length;
                    int start = fileName.IndexOf("_");
                    int end = fileName.LastIndexOf("_");

                    String projectNumber = fileName.Substring(start + 1, end - start - 1);
                    int temp = Int32.Parse(projectNumber);
                    if (!ProjectNumbers.Contains(temp))
                    {
                        ProjectNumbers.Add(temp);
                    }
                }
            }
            return ProjectNumbers;
        }

        /* DONT USE ANY UNDERBRACKET IN THE PATH
         * GetAllConfigsNames()
         * @param void, @return List<int> ProjectNames
         * Method to return all names of files from the path (folder on the end of path)
         */
        public List<String> GetAllConfigsNames(string[] IDs)
        {

            List<String> XMLcontentList = new List<string>();
            foreach (String ID in IDs)
            {
                string[] fileArray = Directory.GetFiles(path, "config_" + ID + "*.xml");
                foreach (String fileName in fileArray)
                {
                    int length = fileName.Length;
                    int start = fileName.IndexOf("_");
                    int end = fileName.LastIndexOf("_");

                    String projectNumber = fileName.Substring(start + 1, end - start - 1);
                    int temp = Int32.Parse(projectNumber);

                    String Name = readNodeXML("name", temp);
                    if (!ProjectNames.Contains(Name))
                    {
                        ProjectNames.Add(Name);
                    }
                }
            }
            return ProjectNames;
        }

        /* @param String tag, @return List<String> XMLcontentList
         * Method to count nuber of some tags
         * XMLcontentList is List indexed by integer
        */
        public int XMLtagCount(String tag, int ProjectNumber)
        {

            XmlDocument xml = new XmlDocument();
            String search_pattern = "config_" + ProjectNumber + "*";
            string[] absoulte_path = Directory.GetFiles(path, search_pattern);
            xml.Load(absoulte_path[0]);

            XmlNodeList xnList = xml.SelectNodes("//" + tag);

            int count = xnList.Count;
            return count;
        }

        /* @param String tag, int ProjectNumber 
         * @return List<String> XMLNodesType
         * Get types of Nodes in 2 levels - one perent level one child level
         * type = (ex. alarm, graph, plc, scheme, etc.)
         */
        public List<String> XMLgetTypes(String tag, int ProjectNumber)
        {

            XmlDocument xml = new XmlDocument();
            String search_pattern = "config_" + ProjectNumber + "*";
            string[] absoulte_path = Directory.GetFiles(path, search_pattern);
            xml.Load(absoulte_path[0]);
            List<String> XMLNodesType = new List<String>();
            XmlNodeList xnList = xml.SelectNodes("//" + tag);

            foreach (XmlNode xn in xnList)
            {
                XMLNodesType.Add(xn.Name);
                XmlNodeList nList = xn.ChildNodes;

                foreach (XmlNode n in nList)
                {
                    XMLNodesType.Add(n.Name);
                }
            }
            return XMLNodesType;
        }

        /* Not prepared for building config file
         * @param FormCollection formCollection, FileFormModel model, @return String Path (full path)
         * Method to create XML File 
        */
        public String CreateXMLFile(FormCollection formCollection, FileFormModel model)
        {
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
            using (XmlWriter writer = XmlWriter.Create(full_path, settings))
            {

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
        // GET: XML
        public ActionResult Index()
        {
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