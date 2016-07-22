using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.ViewModels
{
    public class StrategyViewModel
    {

        [Display(Name="شرح استراتژی: ")]
        [MaxLength(500, ErrorMessage = "شرح استراتژی بیش از حد مجاز وارد شده است")]
        public string strategy_name { get; set; }

        public int strategy_id { get; set; }


    }
}