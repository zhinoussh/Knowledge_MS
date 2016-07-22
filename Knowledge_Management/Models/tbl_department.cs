namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_department
    {
        public tbl_department()
        {
            tbl_department_objectives = new HashSet<tbl_department_objectives>();
            tbl_employee = new HashSet<tbl_employee>();
            tbl_job = new HashSet<tbl_job>();
        }

        [Key]
        public int pkey { get; set; }

        [StringLength(200)]
        public string department_name { get; set; }

        public virtual ICollection<tbl_department_objectives> tbl_department_objectives { get; set; }

        public virtual ICollection<tbl_employee> tbl_employee { get; set; }

        public virtual ICollection<tbl_job> tbl_job { get; set; }
    }
}
