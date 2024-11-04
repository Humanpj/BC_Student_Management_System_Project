using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Student_Management_System_Project
{
    internal class FileHandler
    {
        public static void Write(string path, List<Student> students)
        {
            //string path = @"students.txt";

            FileStream fs = new FileStream(path, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                string text;
                foreach (Student student in students)
                {
                    //Change below line into Student.ToString();
                    //text = student.StudentID + "," + student.Name + "," + student.Age +","+ student.Course;
                    sw.WriteLine(student.ToString());
                }
            }

            fs.Close();
            Console.WriteLine("Data saved successfully");
            Console.ReadLine();
        }

        public static void Write(string path, int totalStudents, double avgAge)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine("Total number of students: " + totalStudents.ToString());
                sw.WriteLine("Average student age: " + avgAge.ToString());
            }

            fs.Close();
            Console.WriteLine("Data saved successfully");
            Console.ReadLine();
        }

        public static List<Student> Read(string path)
        {
            List<Student> students = new List<Student>();

            //string path = @"students.txt";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs);
            string text;
            while ((text = sr.ReadLine()) != null)
            {
                string[] strings = text.Split(',');
                Student newstudent = new Student(int.Parse(strings[0]), strings[1], strings[2]);
                students.Add(newstudent);
            }
            fs.Close();
            sr.Close();

            return students;
        }
    }
}
