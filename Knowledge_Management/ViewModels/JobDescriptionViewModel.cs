using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Knowledge_Management.ViewModels
{
    public class JobDescriptionViewModel
    {
        [Display(Name = "شرح شغل: ")]
        public string jobDesc { get; set; }
       
        public int jobDesc_id { get; set; }

        public string dep_id { get; set; }

        public SelectList lst_dep { get; set; }

        public string job_id { get; set; }

        public SelectList lst_job { get; set; }
        
       
    }
}