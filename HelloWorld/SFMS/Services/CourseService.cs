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
        public void Create(CourseViewModel viewModel) {
            //Dto process here 
            //Data Transfer Object from videModel to Model 
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
            //saving the record into the database by applying repository 
            _courseRepository.Create(model);
        }

        public void Delete(string id) {
          _courseRepository.Delete(id);
        }

        public CourseViewModel FindById(string id) {
            var s = _courseRepository.FindById(id);
            var viewModel =new CourseViewModel
            {
                Id = s.Id,
                Description = s.Description,
                Name = s.Name,
                OpeningDate = s.OpeningDate,
                DurationInHour = s.DurationInHour,
                Fees = s.Fees,
            };
            return viewModel;
        }

        public IList<CourseViewModel> ReteriveActive() {
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

        public void Update(CourseViewModel viewModel) {
            var model = new Course();
            //audit columns
            model.Id = viewModel.Id;
             //ui columns
            model.Name = viewModel.Name;
            model.Description = viewModel.Description;
            model.OpeningDate = viewModel.OpeningDate;
            model.DurationInHour = viewModel.DurationInHour;
            model.Fees = viewModel.Fees;
            _courseRepository.Update(model);
        }
    }
}
