using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Knowledge_Management.Models;
using System.Web.Mvc;

namespace Knowledge_Management.ViewModels
{
    public class JobViewModel
    {
        public int job_id { get; set; }

        [Display(Name = "نام شغل: ")]
        [MaxLength(50, ErrorMessage = "تعداد کاراکتر وارد شده، بیش از حد مجاز است")]
        public string job_name { get; set; }
     
        public string selected_dep { get; set; }

        public SelectList lst_dep { get; set; }
        
    }
}