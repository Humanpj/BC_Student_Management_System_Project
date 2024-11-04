using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BC_Student_Management_System_Project
{
    internal class DataHandler
    {
        private List<Student> students;
        private FileHandler fileHandler;

        public DataHandler()
        {
            students = new List<Student>();
            fileHandler = new FileHandler();
            LoadAllStudents();
        }

        public List<Student> GetStudents() => students;

        public void AddStudent(Student student)
        {
            students.Add(student);
            fileHandler.WriteStudent($"{student.StudentID},{student.Name},{student.Age},{student.Course}");
        }

        public bool UpdateStudent(string studentId, string name, int age, string course)
        {
            var student = students.FirstOrDefault(s => s.StudentID == studentId);
            if (student == null) return false;

            student.Name = name;
            student.Age = age;
            student.Course = course;
            fileHandler.SaveAllStudents(students);
            return true;
        }

        public bool DeleteStudent(string studentId)
        {
            var student = students.FirstOrDefault(s => s.StudentID == studentId);
            if (student == null) return false;

            students.Remove(student);
            fileHandler.SaveAllStudents(students);
            return true;
        }

        public void GenerateSummary()
        {
            int totalStudents = students.Count;
            double averageAge = students.Any() ? students.Average(s => s.Age) : 0;
            fileHandler.WriteSummary(totalStudents, averageAge);
        }

        private void LoadAllStudents()
        {
            fileHandler.CheckOrCreateFile();
            var lines = fileHandler.ReadAllLines();
            foreach (var line in lines)
            {
                var data = line.Split(',');
                if (data.Length == 4 && int.TryParse(data[2], out int age))
                {
                    students.Add(new Student
                    {
                        StudentID = data[0],
                        Name = data[1],
                        Age = age,
                        Course = data[3]
                    });
                }
            }
        }
    }
}
