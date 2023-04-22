using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models { 
    [Table("FinePolicy")]
    public class FinePolicy : BaseEntity{       
        public string Name { get; set; }
        public string Rule { get; set; }
        public int FineAmount { get; set; }
        public int FineAfterMinutes { get; set; }
        public string BatchId { get; set; }
        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }
        public bool IsEnable { get; set; }
    }
}
