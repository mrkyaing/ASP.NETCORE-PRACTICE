using System;
using System.ComponentModel.DataAnnotations;

namespace SFMS.Models{
    public class BaseEntity{
        [Key]
        public string Id { get; set; }
        public DateTime CreatedDte { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string IP { get; set; }
    }
}
