using System;
using System.ComponentModel.DataAnnotations;

namespace SFMS.Models{
    public class BaseEntity{
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string IP { get; set; }
    }
}
