using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Student_Management_System_Project
{
    internal class Student
    {
        private string name, course, studentID;
        private int age;

        public Student(string studentID, string name, int age, string course)
        {
            this.Name = name;
            this.Course = course;
            this.StudentID = studentID;
            this.Age = age;
        }

        public string Name { get => name; set => name = value; }
        public string Course { get => course; set => course = value; }
        public string StudentID { get => studentID; set => studentID = value; }
        public int Age { get => age; set => age = value; }

        public override string ToString()
        {
            return $"{StudentID},{Name},{Age},{Course}";
        }
    }
}
