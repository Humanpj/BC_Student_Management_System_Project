using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Student_Management_System_Project
{
    internal class Transactions
    {
        public static List<Student> LinesToStudents(List<string> lines)
        {
            List<Student> students = new List<Student>();

            foreach (string line in lines)
            {
                string[] details = line.Split(',');
                students.Add(new Student(details[0], details[1], int.Parse(details[2]), details[3]));
            }

            return students;
        }

        //Ken -> implement
        public static bool UpdateStudent(string studentId, string name, int age, string course)
        {
            /*var student = students.FirstOrDefault(s => s.StudentID == studentId);
            if (student == null) return false;

            student.Name = name;
            student.Age = age;
            student.Course = course;*/
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //fileHandler.SaveAllStudents(students);
            return true;
        }

        //Ken -> implement
        public static bool DeleteStudent(string studentId)
        {
            /*var student = students.FirstOrDefault(s => s.StudentID == studentId);
            if (student == null) return false;

            students.Remove(student);
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //fileHandler.SaveAllStudents(students);*/
            return true;
        }

        //Delete?
        /*private void LoadAllStudents()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            fileHandler.CheckOrCreateFile();
            var lines = fileHandler.ReadAllLines();
            foreach (var line in lines)
            {
                var data = line.Split(',');
                if (data.Length == 4 && int.TryParse(data[2], out int age))
                {
                    students.Add(new Student(data[0], data[1], age, data[3]));
                }

            }

            avgAge = totAge/count;


        }*/
    }
}
