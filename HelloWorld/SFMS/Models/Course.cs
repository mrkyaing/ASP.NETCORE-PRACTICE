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
        public double Fees { get; set; }
        public virtual IList<Batch> Batches { get; set; }
    }
}
