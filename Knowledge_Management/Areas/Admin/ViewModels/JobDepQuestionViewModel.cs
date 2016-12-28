using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class JobDepQuestionViewModel
    {
        public int question_id { get; set; }

        public string dep_id { get; set; }

        [Display(Name="Department: ")]
        public SelectList lst_dep { get; set; }

        public string job_id { get; set; }

        [Display(Name = "Job Title: ")]
        public SelectList lst_job { get; set; }
    }
}