using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knowledge_Management.Areas.Admin.ViewModels
{
    public class EmployeeQuestionViewModel
    {
        public int emp_id { get; set; }
        public int question_id { get; set; }

        public string Description { get; set; }
    }
}