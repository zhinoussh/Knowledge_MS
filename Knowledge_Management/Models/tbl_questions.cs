namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_questions
    {
        public tbl_questions()
        {
            tbl_question_keywords = new HashSet<tbl_question_keywords>();
            tbl_question_solutions = new HashSet<tbl_question_solutions>();
        }

        [Key]
        public long pkey { get; set; }

        [StringLength(500)]
        public string subject { get; set; }

        public int? fk_employee { get; set; }
        public int? fk_depObj { get; set; }
        public int? fk_strategy { get; set; }
        public long? fk_jobDesc { get; set; }

        public virtual tbl_employee tbl_employee { get; set; }

        [ForeignKey("fk_depObj")]
        public virtual tbl_department_objectives tbl_department_objectives { get; set; }

        [ForeignKey("fk_strategy")]
        public virtual tbl_strategy tbl_strategy { get; set; }

        [ForeignKey("fk_jobDesc")]
        public virtual tbl_job_description tbl_job_description { get; set; }

        public virtual ICollection<tbl_question_keywords> tbl_question_keywords { get; set; }
        public virtual ICollection<tbl_question_solutions> tbl_question_solutions { get; set; }
    }
}
