using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class DepartmentObjectiveViewModel
    {
        public int obj_id { get; set; }

        [Display(Name = "Objective Description: ")]
        [MaxLength(200, ErrorMessage = "Max length exceeded.")]
        [Required]
        public string obj_name { get; set; }
        public string dep_name { get; set; }
        public int dep_id { get; set; }
    }
}