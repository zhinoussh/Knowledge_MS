using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.ViewModels
{
    public class DepartmentViewModel
    {
        [Display(Name = "نام واحد سازمانی: ")]
        [MaxLength(200, ErrorMessage = "تعداد کاراکتر واحد سازمانی، بیش از حد مجاز وارد شده است")]
        public string dep_name { get; set; }

        public int dep_id { get; set; }
    }
}
