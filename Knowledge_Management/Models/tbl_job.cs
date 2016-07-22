namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_job
    {
        public tbl_job()
        {
            tbl_employee = new HashSet<tbl_employee>();
            tbl_job_description = new HashSet<tbl_job_description>();
        }

        [Key]
        public int pkey { get; set; }

        [StringLength(50)]
        public string job_name { get; set; }

        public int? fk_department { get; set; }

        public virtual tbl_department tbl_department { get; set; }

        public virtual ICollection<tbl_employee> tbl_employee { get; set; }

        public virtual ICollection<tbl_job_description> tbl_job_description { get; set; }
    }
}
