using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class DetailQuestionViewModel
    {
        public long question_id { get; set; }

        public int dep_id { get; set; }
        public SelectList lst_dep { get; set; }

        public int job_id { get; set; }

        public SelectList lst_job { get; set; }
        public int dep_obj_id { get; set; }

        [Display(Name = "هدف واحد سازمانی")]
        public SelectList lst_dep_objective { get; set; }

        public long job_desc_id { get; set; }

        [Display(Name = "شرح شغل")]
        public SelectList lst_job_desc { get; set; }

        public int strategy_id { get; set; }
        
        [Display(Name = "استراتژی")]
        public SelectList lst_strategy { get; set; }
    }
}