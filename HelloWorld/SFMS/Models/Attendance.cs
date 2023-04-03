using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models { 
    [Table("Attendance")]
    public class Attendance:BaseEntity{
        public DateTime AttendaceDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public bool IsLate { get; set; }
        public bool IsLeave { get; set; }
        public string StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student   Student { get; set; }
    }
}
