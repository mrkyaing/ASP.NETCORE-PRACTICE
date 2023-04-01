
using System;
using System.Collections.Generic;

namespace SFMS.Models.ViewModels {
    public class AttendanceDayEndProcessViewModel {
        public string Id { get; set; }
        public DateTime FromDayEndDate { get; set; }
        public DateTime ToDayEndDate { get; set; }
        public IList<BatchViewModel>   batches { get; set; }
        public IList<StudentViewModel> students { get; set; }
        public string StudentId { get; set; }
        public string BathId { get; set; }
    }
}
