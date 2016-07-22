namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_employee
    {
        public tbl_employee()
        {
            tbl_login = new HashSet<tbl_login>();
            tbl_questions = new HashSet<tbl_questions>();
            tbl_questions_solution = new HashSet<tbl_question_solutions>();
        }

        [Key]
        public int pkey { get; set; }

        [StringLength(50)]
        public string fname { get; set; }

        [StringLength(50)]
        public string lname { get; set; }

        [StringLength(50)]
        public string personel_code { get; set; }

        public int? fk_department { get; set; }
        public int? fk_job { get; set; }
        public bool data_entry { get; set; }
        public bool data_view { get; set; }

        [ForeignKey("fk_department")]
        public virtual tbl_department tbl_department { get; set; }

        [ForeignKey("fk_job")]
        public virtual tbl_job tbl_job { get; set; }

        public virtual ICollection<tbl_login> tbl_login { get; set; }
        public virtual ICollection<tbl_questions> tbl_questions { get; set; }
        public virtual ICollection<tbl_question_solutions> tbl_questions_solution { get; set; }
    }
}
