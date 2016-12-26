using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knowledge_Management.Areas.User.ViewModels
{
    public class JobDepQuestionViewModel
    {
        public int question_id { get; set; }

        public string dep_id { get; set; }

        public SelectList lst_dep { get; set; }

        public string job_id { get; set; }

        public SelectList lst_job { get; set; }
    }
}