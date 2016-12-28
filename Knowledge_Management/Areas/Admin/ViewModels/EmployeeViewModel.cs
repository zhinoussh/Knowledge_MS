using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class EmployeeViewModel
    {
        public int emp_id { get; set; }

        [Display(Name = "First Name: ")]
        [MaxLength(50, ErrorMessage = "Max length exceeded.")]
        [Required]
        public string first_name { get; set; }

        [Display(Name = "Last Name: ")]
        [MaxLength(50, ErrorMessage = "Max length exceeded.")]
        [Required]
        public string last_name { get; set; }

        [Display(Name = "Personel code: ")]
        [MaxLength(10, ErrorMessage = "Max length exceeded.")]
        [Required]
        public string personel_code { get; set; }

        [Display(Name = "Password: ")]
        [Required]
        public string pass { get; set; }

        public string dep_id { get; set; }

        [Display(Name = "Department: ")]
        public SelectList lst_dep { get; set; }

        public string job_id { get; set; }

        [Display(Name = "Access to Data Entry")]
        public bool data_entry { get; set; }

        [Display(Name = "Access to View Information")]
        public bool data_view { get; set; }

        [Display(Name = "Job Title: ")]
        public SelectList lst_job { get; set; }
    }
}