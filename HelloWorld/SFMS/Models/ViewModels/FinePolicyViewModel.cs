namespace SFMS.Models.ViewModels {
    public class FinePolicyViewModel {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Rule { get; set; }
        public int FineAmount { get; set; }
        public int FineAfterMinutes { get; set; }
    }
}
