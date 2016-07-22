using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Knowledge_Management.ViewModels
{
    public class EmployeeViewModel
    {
        public int emp_id { get; set; }

        [Display(Name = "نام: ")]
        [MaxLength(50, ErrorMessage = "تعداد کاراکتر وارد شده، بیش از حد مجاز است")]
        public string first_name { get; set; }

        [Display(Name = "نام خانوادگی: ")]
        [MaxLength(50, ErrorMessage = "تعداد کاراکتر وارد شده، بیش از حد مجاز است")]
        public string last_name { get; set; }

        [Display(Name = "کد ملی: ")]
        [MaxLength(10, ErrorMessage = "تعداد کاراکتر وارد شده، بیش از حد مجاز است")]
        public string personel_code { get; set; }

        [Display(Name = "کلمه عبور: ")]
        public string pass { get; set; }

        public string dep_id { get; set; }

        public SelectList lst_dep { get; set; }

        public string job_id { get; set; }

        [Display(Name = "ورود اطلاعات")]
        public bool data_entry { get; set; }

        [Display(Name = "مشاهده اطلاعات")]
        public bool data_view { get; set; }


        public SelectList lst_job { get; set; }
    }
}