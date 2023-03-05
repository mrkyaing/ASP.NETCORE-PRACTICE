using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCollectionPractice
{
    public class GenericPractice
    {
       
       public void ListDemo()
        {
            IList<int> ExamMarks = new List<int>();//create IList Generic Collection
            ExamMarks.Add(40);
            //ExamMarks.Add("Mg Mg");
            ExamMarks.Add(70);
            ExamMarks.Add(80);
            
            foreach(int i in ExamMarks) {
                Console.WriteLine(i);
            }

        }

        public void GetPersonListDemo()
        {
            IList<Person> Persons = new List<Person>();
            
            Person p1=new Person();
            p1.Id= 1;
            p1.Name = "U Ba";
            p1.Address = "MDY";

            Person p2 = new Person();////passing the data to Person Class with . Operator
            p2.Id = 2;
            p2.Name = "Daw Mya";
            p2.Address = "YGN";
            
            Persons.Add(p1);
            Persons.Add(p2);

            Person p3 = new Person()//passing the data to Person Class  with anonymous style 
            {
                Id = 3,
                Name = "Ko Mya Gyi",
                Address = "YGN"
            };
            Persons.Add(p3);

            Person p4 = new Person(4,"Su Su","NPT");//passing the data to Person Class  with parameter value
            Persons.Add(p4);

            Console.WriteLine("=============person recrod as below==============");
            
            foreach (Person p in Persons) {
                Console.WriteLine(p);
            }

        }
        
    }
}
