using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCollectionPractice
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()=> $"Id:{Id}\nName:{Name}\n Age:{Age}";

    }
}
