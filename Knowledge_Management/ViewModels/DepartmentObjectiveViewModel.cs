using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.ViewModels
{
    public class DepartmentObjectiveViewModel
    {
        public int obj_id { get; set; }

        [Display(Name = "شرح هدف: ")]
        [MaxLength(200, ErrorMessage = "تعداد کاراکتر وارد شده، بیش از حد مجاز است")]
        public string obj_name { get; set; }
        public string dep_name { get; set; }
        public int dep_id { get; set; }
    }
}