namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_login
    {
        [Key]
        public int pk { get; set; }

        public int? fk_emp { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(100)]
        public string pass { get; set; }

        [StringLength(50)]
        public string role { get; set; }

        public virtual tbl_employee tbl_employee { get; set; }
    }
}
