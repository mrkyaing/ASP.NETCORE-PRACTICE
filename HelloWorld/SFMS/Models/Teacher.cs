using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models
{
    [Table("Teacher")]
    public class Teacher:BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string NRC { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public string FacebookUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string TwitterUrl { get; set; }
        public virtual IList<Course> Courses { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }
        //public string? ProfileImageUrl { get; set; }
    }
}
