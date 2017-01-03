using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.Areas.User.ViewModels
{
    public class NewSolutionViewModel
    {
        public long question_id { get; set; }

        [Display(Name = "Question Description")]
        public string question { get; set; }

        [Display(Name ="Solution Description")]
        [Required(ErrorMessage="Solution description is not valid.")]
        public string new_solution { get; set; }

        public long new_solution_id { get; set; }

        [Display(Name = "Upload Description")]
        public string file_description { get; set; }

        public long upload_id { get; set; }


    }
}