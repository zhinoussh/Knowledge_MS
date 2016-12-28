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

        [Display(Name = "Solution Description")]
        public string full_solution { get; set; }

        [Display(Name = "Question Description")]
        public string question { get; set; }

        public long question_id { get; set; }

        [Display(Name = "Uploads")]
        public string uploads { get; set; }

    }
}