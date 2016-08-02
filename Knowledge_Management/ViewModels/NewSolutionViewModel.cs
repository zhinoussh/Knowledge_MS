using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.ViewModels
{
    public class NewSolutionViewModel
    {
        public int question_id { get; set; }

        [Display(Name = "شرح مسئله: ")]
        public string question { get; set; }

        [Display(Name = "راهکار جدید: ")]
        [Required(ErrorMessage = "شرح راهکار را وارد نمایید")]
        public string new_solution { get; set; }

        public long new_solution_id { get; set; }

    }
}