using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.Areas.User.ViewModels
{
    public class SolutionViewModel
    {
        public int question_id { get; set; }

        [Display(Name = "شرح مسئله")]
         public string question { get; set; }

        [Display(Name = "راهکارهای مسئله")]
        public string[][] solutions { get; set; }


    }
}