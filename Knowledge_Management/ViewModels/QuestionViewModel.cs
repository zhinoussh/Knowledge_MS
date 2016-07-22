using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Knowledge_Management.ViewModels
{
    public class QuestionViewModel
    {
        [Display(Name = "شرح مسئله: ")]
        [MaxLength(500, ErrorMessage = "تعداد کاراکتر وارد شده، بیش از حد مجاز است")]
        public string question { get; set; }

        public long question_id { get; set; }

        [Display(Name = "کلیدواژه: ")]
        //seprated with ,
        public String lst_keywords { get; set; }

        [Display(Name = "راهکار: ")]        
        public string solution { get; set; }
        public int? dep_obj_id { get; set; }

        [Display(Name = "هدف واحد سازمانی")]
        public SelectList lst_dep_objective { get; set; }

        public long? job_desc_id { get; set; }

        [Display(Name = "شرح شغل")]
        public SelectList lst_job_desc { get; set; }

        public int? strategy_id { get; set; }
        
        [Display(Name = "استراتژی")]
        public SelectList lst_strategy { get; set; }
    }
}