using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
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
            
        

            Person p3 = new Person()//passing the data to Person Class  with anonymous style 
            {
                Id = 3,
                Name = "Ko Mya Gyi",
                Address = "YGN"
            };
           

            Person p4 = new Person(4,"Su Su","NPT");//passing the data to Person Class  with parameter value


            Persons.Add(p1);
            Persons.Add(p2);
            Persons.Add(p3);
            Persons.Add(p4);

            Console.WriteLine("=============person recrod as below==============");
            
            foreach (Person p in Persons) {
                Console.WriteLine(p);
            }

        }
      
        public void GetTeenAgerStudent()
        {
            IList<Student> students = new List<Student>() 
            {
                new Student() { Id=1,Name="Su Su",Age=18},
                 new Student() { Id=2,Name="Aye Aye",Age=19},
                 new Student() { Id=3,Name="Min Min",Age=20},
                 new Student() { Id=4,Name="Kaung Kaung",Age=18},
                 new Student() { Id=5,Name="Aun Zaw",Age=25},
                new Student() { Id=6,Name="Min Chit",Age=18},
                new Student() { Id=7,Name="Aung Aung Oo",Age=22},
            };
            IList<Student> TeenAgerstudents = new List<Student>();
            int count = 0;
            foreach (Student s in students) {
                if(s.Age>12 && s.Age < 20) {
                    count++;
                    TeenAgerstudents.Add(s);//copying from students list to new TeenAgerstudents
                }
            }
            Console.WriteLine("Total TeenAger Student :"+count);//4
            foreach(var s in TeenAgerstudents) {
                Console.WriteLine(s);
            }

            Console.WriteLine("applying linq");

            IList<Student> teenAgerStudentLinq = students.Where(s => s.Age > 12 && s.Age < 20).OrderBy(o=>o.Name).ToList();

            IList<Student> teenAgerStudentQuery=(from s in students 
                                                                            where s.Age> 12 && s.Age< 20
                                                                            select s).ToList();

            int studentCount= teenAgerStudentLinq.Count();
            foreach (var s in teenAgerStudentLinq) {
                Console.WriteLine(s);   
            }
            Student student = students.Where(s => s.Name == "Su Su").FirstOrDefault();
            Console.WriteLine(student);//1 Su Su 18
        }
    }
}
