namespace SFMS.Models.ViewModels {
    public class BatchViewModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CourseId { get; set; }//for create process when batch record is created.
        public string CourseName { get; set; }//for reterive process when batch is reterived from db.
    }
}
