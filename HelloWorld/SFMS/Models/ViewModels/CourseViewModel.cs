﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SFMS.Models.ViewModels {
    public class CourseViewModel {
        public string Id { get; set; }
        [Required]
        [Remote(action: "IsCourseAlreadyExists", controller: "Course")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime OpeningDate { get; set; }
        public int DurationInHour { get; set; }
        public decimal Fees { get; set; }
        public bool IsPromotion { get; set; }
        public decimal Percetance { get; set; }
        public int Fixed { get; set; }
        public decimal FeesAfterPromo { get; set; }
    }
}
