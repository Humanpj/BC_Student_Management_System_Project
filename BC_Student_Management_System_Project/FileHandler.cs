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
        private string path;

        public string Path { get => path; set => path = value; }

        public FileHandler(string filePath) 
        {
            Path = filePath;
        }

        public void CheckOrCreateFile()
        {
            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }
        }

        //used to write all students to textFile
        //replaces existing file
        public void ReWrite(List<Student> students)
        {
            FileStream fs = new FileStream(Path, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (Student student in students)
                {
                    sw.WriteLine(student.ToString());
                }
            }

            fs.Close();
        }

        //Read all lines in textFile
        //Returns list of all lines (string)
        public List<string> ReadAllLines()
        {
            return File.Exists(Path) ? new List<string>(File.ReadAllLines(Path)) : new List<string>();
        }

        //Write single line
        //Appends line to textFile
        public void Write(string line)
        {
            using (StreamWriter writer = new StreamWriter(Path, true))
            {
                writer.WriteLine(line);
            }
        }

        //Checks if studentID in textfile
        //returns true of found else returns false
        public bool SearchID(string studentID)
        {
            if (File.Exists(Path))
            {
                using (StreamReader reader = new StreamReader(Path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        // Check if the first element (StudentID) matches the input
                        if (data.Length > 0 && data[0] == studentID)
                        {
                            return true; // Duplicate found
                        }
                    }
                }
            }
            return false; // No duplicate found
        }

        /*public static void Write(string path, int totalStudents, double avgAge)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(fs))
            {

                sw.WriteLine("Total number of students: " + totalStudents.ToString());
                sw.WriteLine("Average student age: " + avgAge.ToString());

                sw.WriteLine($"{totalStudents},{averageAge}");

            }

            fs.Close();
            Console.WriteLine("Data saved successfully");
            Console.ReadLine();
        }*/

        /*public static List<Student> Read(string path)
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
        }*/
    }
}
