using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models { 
    [Table("Course")]
    public class Course:BaseEntity{       
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime  OpeningDate { get; set; }
        public int DurationInHour { get; set; }
        public decimal Fees { get; set; }
        public bool IsPromotion { get; set; }
        public decimal Percetance { get; set; }
        public int Fixed { get; set; }
        public virtual IList<Batch> Batches { get; set; }
    }
}
