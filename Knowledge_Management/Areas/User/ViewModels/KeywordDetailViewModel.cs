using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Knowledge_Management.Areas.User.ViewModels
{
    public class KeywordDetailViewModel
    {
        [Display(Name = "Keyword:")]
        public string keyword { get; set; }

        public int strategyId { get; set; }

        [Display(Name = "Strategy:")]
        public string strategy { get; set; }

        public int jobDescId { get; set; }

        [Display(Name = "Job description:")]
        public string job_desc { get; set; }

        public int depObjId { get; set; }

        [Display(Name = "Department Objective:")]
        public string dep_obj { get; set; }
    }
}