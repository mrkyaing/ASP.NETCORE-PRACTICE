using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace SFMS.Models.ViewModels {
    public class FineTransactionViewModel {
        public string Id { get; set; } 
        public DateTime FinedDate { get; set; }
        public string FinePolicyId { get; set; }
        public virtual FinePolicy FinePolicy { get; set; }
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int FineAmount { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
    }
}
