using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BC_Student_Management_System_Project
{
    internal class FileHandler
    {
        private string studentFilePath = "students.txt";
        private string summaryFilePath = "summary.txt";

        public void CheckOrCreateFile()
        {
            if (!File.Exists(studentFilePath))
            {
                File.Create(studentFilePath).Close();
            }
        }

        public List<string> ReadAllLines()
        {
            return File.Exists(studentFilePath) ? new List<string>(File.ReadAllLines(studentFilePath)) : new List<string>();
        }

        public void WriteStudent(string studentRecord)
        {
            using (StreamWriter writer = new StreamWriter(studentFilePath, true))
            {
                writer.WriteLine(studentRecord);
            }
        }

        public void SaveAllStudents(List<Student> students)
        {
            using (StreamWriter writer = new StreamWriter(studentFilePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.StudentID},{student.Name},{student.Age},{student.Course}");
                }
            }
        }

        public void WriteSummary(int totalStudents, double averageAge)
        {
            using (StreamWriter writer = new StreamWriter(summaryFilePath))
            {
                writer.WriteLine($"Total Students: {totalStudents}");
                writer.WriteLine($"Average Age: {averageAge}");
            }
        }
    }
}
