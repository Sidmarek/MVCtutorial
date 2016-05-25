using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCtutorial.File.Models
{
    public class FileFormModel
    {
            
            //XML file name given by user


            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Name of XML File")]
            public string fileName { get; set; }
            
            //Identifiers and description for contracts    

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "ID")]
            public int id { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Prefix")]
            public string prefix { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Country")]
            public string country { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            public string name { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Description")]
            public string description { get; set; }

    }
    public class preCreateXmlForm {

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Number of alarms")]
            public int alarmsNumber { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Number of graphs")]
            public int graphsNumber { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Number of schemes")]
            public int schemesNumber { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Number of reports")]
            public int reportsNumber { get; set; }

    }
}