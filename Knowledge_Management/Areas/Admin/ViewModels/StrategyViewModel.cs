using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class StrategyViewModel
    {

        [Display(Name = "Strategy Description: ")]
        [MaxLength(500, ErrorMessage = "Max length exceeded.")]
        public string strategy_name { get; set; }

        public int strategy_id { get; set; }


    }
}