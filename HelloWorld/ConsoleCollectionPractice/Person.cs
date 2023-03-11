using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCollectionPractice
{
    public class Person
    {
        public Person()//no paremeter constructor /Default Construtor 
        {

        }
        //parameter constructor with 3 type
        public Person(int id,string name,string address)
        {
            Id = id;
            Name = name;
            Address = address;
        }
        //Member variable of Person Class
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return $"Id:{Id}\nName:{Name}\nAddress:{Address}";
        }

    }
}
