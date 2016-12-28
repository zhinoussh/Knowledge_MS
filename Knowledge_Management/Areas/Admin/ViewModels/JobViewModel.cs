using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Knowledge_Management.Models;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class JobViewModel
    {
        public int job_id { get; set; }

        [Display(Name = "Job Title: ")]
        [MaxLength(50, ErrorMessage = "Max length exceeded.")]
        [Required]
        public string job_name { get; set; }
     
        public string selected_dep { get; set; }

        public SelectList lst_dep { get; set; }
        
    }
}