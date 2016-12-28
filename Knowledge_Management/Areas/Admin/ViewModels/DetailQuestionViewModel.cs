using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class DetailQuestionViewModel
    {
        public long question_id { get; set; }

        public int dep_id { get; set; }
        
        [Display(Name="Department: ")]
        public SelectList lst_dep { get; set; }

        public int job_id { get; set; }

        [Display(Name = "Job Title: ")]
        public SelectList lst_job { get; set; }
        public int dep_obj_id { get; set; }

        [Display(Name = "Department Objective: ")]
        public SelectList lst_dep_objective { get; set; }

        public long job_desc_id { get; set; }

        [Display(Name = "Job Description: ")]
        public SelectList lst_job_desc { get; set; }

        public int strategy_id { get; set; }
        
        [Display(Name = "Strategy Description: ")]
        public SelectList lst_strategy { get; set; }
    }
}