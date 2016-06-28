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

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Subject")]
        public string Header { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Please write what is on your mind...")]
        public string Text { get; set; }
    }
}