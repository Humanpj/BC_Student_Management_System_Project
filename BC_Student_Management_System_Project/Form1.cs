using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BC_Student_Management_System_Project
{
    public partial class frmStudentManagementSystem : Form
    {
        private string filePath = "students.txt";
        private BindingSource studentBindingSource = new BindingSource();
        private List<Student> students = new List<Student>();

        public frmStudentManagementSystem()
        {
            InitializeComponent();
            CheckOrCreateFile(); // Ensure the file exists at startup
            LoadAllStudents();
        }
        // Ensure the students.txt file exists, create if not
        private void CheckOrCreateFile()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            string studentId = txtStudentID.Text;
            string name = txtName.Text;
            string ageText = txtAge.Text;
            string course = txtCourse.Text;

            if (string.IsNullOrWhiteSpace(studentId) || string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(ageText) || string.IsNullOrWhiteSpace(course))
            {
                MessageBox.Show("All fields are required.");
                return;
            }

            if (!int.TryParse(ageText, out int age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            Student newStudent = new Student { StudentID = studentId, Name = name, Age = age, Course = course };
            students.Add(newStudent);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{studentId},{name},{age},{course}");
            }

            MessageBox.Show("Student added successfully!");
            ClearFields();
            studentBindingSource.ResetBindings(false); // Refresh the BindingSource to update DataGridView
        }


        private void btnViewAllStudents_Click(object sender, EventArgs e)
        {
              LoadAllStudents();
        }

        private void LoadAllStudents()
        {
            students.Clear(); // Clear the current list

            // Ensure file exists before reading
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
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

            studentBindingSource.DataSource = students;
            dgvStudents.DataSource = studentBindingSource;
        }
        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
        

        }

        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
           
        }


        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {
        }
        private void SaveAllStudentsToFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.StudentID},{student.Name},{student.Age},{student.Course}");
                }
            }
        }
        private void ClearFields()
        {
            txtStudentID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtCourse.Clear();
        }
    }
}
    
