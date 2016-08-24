using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Knowledge_Management.ViewModels
{
    public class SolutionEmployeeViewModel
    {
        public long question_id { get; set; }

        public string question { get; set; }


        public string solution { get; set; }

        public long solution_id { get; set; }

        public int count_upload { get; set; }
        public bool confirm { get; set; }
    }
}