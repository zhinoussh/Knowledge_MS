using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class tbl_solution_uploads
    {
        [Key]
        public long pkey { get; set; }

        public long? fk_solution { get; set; }

        public string file_path { get; set; }

        public string file_desc { get; set; }

        public bool confirm { get; set; } 

        [ForeignKey("fk_solution")]
        public virtual tbl_question_solutions tbl_question_solutions { get; set; }

    }
}