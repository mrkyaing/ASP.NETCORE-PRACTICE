using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFMS.Models.ViewModels
{
    public class StudentViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string NRC { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string FatherName { get; set; }
        public string BathId { get; set; }
        public string BathName { get; set; }
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

    }
}
