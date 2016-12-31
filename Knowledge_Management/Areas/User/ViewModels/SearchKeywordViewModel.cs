using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.User.ViewModels
{
    public class SearchKeywordViewModel
    {
        public long key_id { get; set; }

        [Display(Name = "Keyword:")]
        public string keyword { get; set; }

        public int? dep_obj_id { get; set; }

        [Display(Name = "Department Objective:")]
        public SelectList lst_dep_objective { get; set; }

        public long? job_desc_id { get; set; }

        [Display(Name = "Job description:")]
        public SelectList lst_job_desc { get; set; }

        public int? strategy_id { get; set; }

        [Display(Name = "Strategy:")]
        public SelectList lst_strategy { get; set; }
    }
}