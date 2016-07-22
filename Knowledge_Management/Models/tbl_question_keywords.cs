namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_question_keywords
    {
        [Key]
        public long pkey { get; set; }

        public long? fk_question { get; set; }

        [StringLength(100)]
        public string keyword { get; set; }

        public virtual tbl_questions tbl_questions { get; set; }
    }
}
