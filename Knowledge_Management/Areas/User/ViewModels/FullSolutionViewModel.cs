using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.Areas.User.ViewModels
{
    public class FullSolutionViewModel
    {
        public long solution_id { get; set; }

        [Display(Name = "شرح راهکار")]
        public string full_solution { get; set; }

        [Display(Name = "شرح مسئله")]
        public string question { get; set; }

        public long question_id { get; set; }

        [Display(Name = "اسناد بارگذاری شده")]
        public string uploads { get; set; }

    }
}