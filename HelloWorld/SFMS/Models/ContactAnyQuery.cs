using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models {
    [Table("ContactAnyQuery")]
    public class ContactAnyQuery : BaseEntity {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
