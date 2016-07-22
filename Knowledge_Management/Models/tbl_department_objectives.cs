namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_department_objectives
    {

        public tbl_department_objectives()
        {
            //tbl_employee_job = new HashSet<tbl_employee_job>();
            tbl_questions = new HashSet<tbl_questions>();
        }
        [Key]
        public int pkey { get; set; }

        public int? fk_department { get; set; }

        [StringLength(200)]
        public string objective { get; set; }

        public virtual tbl_department tbl_department { get; set; }

        public virtual ICollection<tbl_questions> tbl_questions { get; set; }

    }
}
