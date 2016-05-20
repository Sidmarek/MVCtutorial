using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCtutorial.Alarms.Models
{
    public class AlarmFormModel
    {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "PageNumber")]
            public int PageNumber { get; set; }

            public int someId { get; set; }
            public int NumberOfRecords { get; set; }
    }

    public class AlarmViewModel {
        
        public string Id { get; set; }
        public string Label { get; set; }
        public string originTime { get; set; }
        public string expTime { get; set; }
        
    }
}