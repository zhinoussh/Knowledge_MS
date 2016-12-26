using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class DepartmentViewModel
    {
        [Display(Name = "Department Name: ")]
        [MaxLength(200, ErrorMessage = "Max length exceeded.")]
        public string dep_name { get; set; }

        public int dep_id { get; set; }
    }
}
