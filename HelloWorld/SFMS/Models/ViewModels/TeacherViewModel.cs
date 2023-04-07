using System;
using System.Collections;
using System.Collections.Generic;

namespace SFMS.Models.ViewModels
{
    public class TeacherViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string NRC { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Position { get; set; }
        public virtual IList<BathViewModel> Courses { get; set; }
    }
}
