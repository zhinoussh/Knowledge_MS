namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_question_solutions
    {
        public tbl_question_solutions()
        {
            tbl_solution_uploads = new HashSet<tbl_solution_uploads>();
        }
        
        [Key]
        public long pkey { get; set; }

        public long? fk_question { get; set; }

        public string solution { get; set; }


        public int? fk_employee { get; set; }

        [ForeignKey("fk_question")]
        public virtual tbl_questions tbl_questions { get; set; }

        [ForeignKey("fk_employee")]
        public virtual tbl_employee tbl_employee { get; set; }

        public virtual ICollection<tbl_solution_uploads> tbl_solution_uploads { get; set; }

    }
}

