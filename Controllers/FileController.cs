using MVCtutorial.File.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MVCtutorial.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public String parsexmL(String tag)
        {
            String XMLString = "Not defined";
            return XMLString;
        }


        public String CreateXMLFile(FormCollection formCollection, FileFormModel model) {
            //TODO Universal XML creator to prepare XML
            
            String fileName = model.fileName;

            int id = model.id;
            String prefix = model.prefix;
            String country = model.country;
            String name = model.name;
            String description = model.description;

            String Path = @"C:\Users\ADMIN\Documents\Visual Studio 2015\Projects\MVCtutorial\MVCtutorial\Config\"
                + id + "_"+  fileName + country + "_" + name + ".xml";
            /*
             * Think about dynamic XML creating
            */
            XmlWriterSettings settings = new XmlWriterSettings();
            using (XmlWriter writer = XmlWriter.Create(Path, settings)) {
                
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
                       
                       writer.WriteStartElement("alarms");
                            foreach (string _formData in formCollection)
                            {
                                if (_formData.Contains("alarmDbName"))
                                {
                                    writer.WriteStartElement(_formData);
                                    writer.WriteValue(formCollection[_formData]);
                                    writer.WriteEndElement();
                                }
                            }
                       writer.WriteEndElement();
                       writer.WriteStartElement("graphs");
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
                       writer.WriteStartElement("schemes");
                        foreach (string _formData in formCollection) {
                            if (_formData.Contains("scheme"))
                            {
                                writer.WriteStartElement(_formData);
                                writer.WriteValue(formCollection[_formData]);
                                writer.WriteEndElement();
                            }
                        }   
                       writer.WriteEndElement();
                       writer.WriteStartElement("reports");
                            foreach (string _formData in formCollection)
                            {
                                if (_formData.Contains("report"))
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
            return Path;
        }



        public ActionResult  Index()
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