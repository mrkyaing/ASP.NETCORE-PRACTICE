using System.Reflection;

namespace WorkOutMVC.Models
{
    public class Student
    {
        //properties 
        public int Id { get; set; }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        //HAS-A Relationship
        public Address HomeAddress { get; set; }
    }
}
