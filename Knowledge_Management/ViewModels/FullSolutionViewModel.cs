using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.ViewModels
{
    public class FullSolutionViewModel
    {
        [Display(Name = "شرح راهکار: ")]
        public string full_solution { get; set; }

        [Display(Name = "شرح مسئله: ")]
        public string question { get; set; }

    }
}