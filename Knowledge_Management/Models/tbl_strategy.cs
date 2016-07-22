

namespace Knowledge_Management.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class tbl_strategy
    {
        public tbl_strategy()
        {
            tbl_questions = new HashSet<tbl_questions>();
        }
        [Key]
        public int pkey { get; set; }

        [StringLength(500)]
        public string strategy_name { get; set; }

        public virtual ICollection<tbl_questions> tbl_questions { get; set; }

    }
}