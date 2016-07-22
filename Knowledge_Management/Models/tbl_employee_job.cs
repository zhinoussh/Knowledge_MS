namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_employee_job
    {
        [Key]
        public long pkey { get; set; }

        public int? fk_employee { get; set; }

        public int? fk_job { get; set; }

     //   public virtual tbl_employee tbl_employee { get; set; }

        public virtual tbl_job tbl_job { get; set; }
    }
}
