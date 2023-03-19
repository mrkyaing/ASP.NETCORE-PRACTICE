using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models
{
    [Table("Student")]
    public class Student:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string NRC { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string FatherName { get; set; }
        public string BathId { get; set; }
        [ForeignKey("BathId")]
        public Batch Batch { get; set; }
    }
}
