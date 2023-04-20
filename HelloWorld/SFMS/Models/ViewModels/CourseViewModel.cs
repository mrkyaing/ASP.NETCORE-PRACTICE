using System;

namespace SFMS.Models.ViewModels {
    public class CourseViewModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OpeningDate { get; set; }
        public int DurationInHour { get; set; }
        public double Fees { get; set; }
    }
}
