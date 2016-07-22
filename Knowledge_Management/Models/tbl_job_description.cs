namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_job_description
    {

        public tbl_job_description()
        {
            //tbl_employee_job = new HashSet<tbl_employee_job>();
            tbl_questions = new HashSet<tbl_questions>();
        }
        [Key]
        public long pkey { get; set; }

        public string job_desc { get; set; }

        public int? fk_job { get; set; }

        public double? job_desc_score { get; set; }

        public virtual tbl_job tbl_job { get; set; }

        public virtual ICollection<tbl_questions> tbl_questions { get; set; }

    }
}
