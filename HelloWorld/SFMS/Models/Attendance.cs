using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models { 
    [Table("FineTransaction")]
    public class FineTransaction : BaseEntity{
        public DateTime FinedDate { get; set; }
        public string FinePolicyId { get; set; }
        [ForeignKey("FinePolicyId")]
        public virtual FinePolicy FinePolicy { get; set; } 
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student   Student { get; set; }
        public int FineAmount { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
    }
}
