using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCtutorial.CMS.Models
{
    public class AddProjectArticle
    {
        /// <summary>
        /// Model for add project status informations
        /// </summary>

        [DataType(DataType.Text)]
        [Display(Name = "For bakery (id - integer)")]
        public string bakeryId { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [DataType(DataType.Text)]
        [Display(Name = "Subject")]
        public string Header { get; set; }

        [Required(ErrorMessage = "Project info is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Project info status")]
        public string Text { get; set; }
    }
}