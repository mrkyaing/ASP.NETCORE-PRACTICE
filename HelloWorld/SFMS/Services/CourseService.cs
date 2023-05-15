using SFMS.Models;
using SFMS.Models.ViewModels;
using SFMS.Repository;
using System.Collections.Generic;
using System.Linq;

namespace SFMS.Services {
    public class CourseService : ICourseService {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public void Entry(CourseViewModel viewModel) {
            var model = new Course()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                OpeningDate = viewModel.OpeningDate,
                DurationInHour = viewModel.DurationInHour,
                Fees = viewModel.Fees,
                IsPromotion = viewModel.IsPromotion,
                Fixed = viewModel.Fixed,
                Percetance = viewModel.Percetance
            };
            _courseRepository.Create(model);
        }

        public IList<CourseViewModel> GetAll() {
            var courses =_courseRepository.ReteriveActive().Select(s => new CourseViewModel
            {
                Name = s.Name,
                Description = s.Description,
                Id = s.Id,
                OpeningDate = s.OpeningDate,
                DurationInHour = s.DurationInHour,
                Fees = s.Fees,
                IsPromotion = s.IsPromotion,
                Fixed = s.Fixed,
                Percetance = s.Percetance,
                FeesAfterPromo = (s.Fees - ((s.Fees * s.Percetance) / 100) + s.Fixed)
            }).ToList();
            return courses;
        }
    }
}
