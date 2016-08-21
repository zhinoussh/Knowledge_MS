using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knowledge_Management.ViewModels
{
    public class ViewQuestionViewModel
    {
        public int dep_id { get; set; }
        public int depObj_id { get; set; }
        public int strategy_id { get; set; }
        public int jobDesc_id { get; set; }
        public int emp_id { get; set; }
        public int job_id { get; set; }

        public string Description { get; set; }
    }
}