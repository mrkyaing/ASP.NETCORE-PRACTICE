
using System;

namespace SFMS.Models.ViewModels {
    public class AttendanceViewModel {
        public string Id { get; set; }
        public DateTime AttendaceDate { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string IsLate { get; set; }
        public string IsLeave { get; set; }
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
