using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleCollectionPractice
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //non-generic demo with ArrayList
            ArrayList marks=new ArrayList();
            //adding the record to the arrayList
            marks.Add(1);//0
            marks.Add("Hello");//1
            marks.Add(3.2f);
            marks.Add(200);
            marks.Add('h');
            //to print out by using for loop
            for(int i=0;i<marks.Count;i++) {
                Console.WriteLine(marks[i]);
            }
            Console.WriteLine("=======================================");
            //to print out by using for-each loop
            foreach(var i in marks) {
                Console.WriteLine(i);
            }
            ArrayList mark2 = new ArrayList();
            mark2.Add(100);
            mark2.Add(200);

            marks.InsertRange(2, mark2);//adding the recrod with range 
            //to print out by using for-each loop
            Console.WriteLine("=======================================");
            foreach (var i in marks) {
                Console.WriteLine(i);
            }
            ArrayList students=new ArrayList();
            students.Add("Mg Mg");
            students.Add("Mya Mya");
            students.Add("Aye Aye");
            //to print out by using for-each loop
            Console.WriteLine("===============Original Student Data=================");
            foreach (var s in students) {
                Console.WriteLine(s);
            }
            students.Reverse();
            Console.WriteLine("===============Reversing Student Data=================");
            foreach (var s in students) {
                Console.WriteLine(s);
            }
            students.Sort();
            Console.WriteLine("===============Soring(ASC Order) Student Data=================");
            foreach (var s in students) {
                Console.WriteLine(s);
            }
            Hashtable hs=new Hashtable();
            hs.Add(1, "Ko ko");
            hs.Add(2, "Min Min");
            hs.Add(3, 200);
            hs.Add(4, false);
            hs.Add(5, 300.5);
            Console.WriteLine("========printing out from hashTable===========");
           for(int i=1; i <=hs.Count; i++) {
                Console.WriteLine(hs[i]);
            }

            GenericPractice gp = new GenericPractice();//create the object for GenericPractice class
            gp.ListDemo();//method invoke for IListDemo 

            gp.GetPersonListDemo();
            Console.WriteLine("==============================");
            gp.GetTeenAgerStudent();
            Console.WriteLine("program finished");
        }
    }
}
