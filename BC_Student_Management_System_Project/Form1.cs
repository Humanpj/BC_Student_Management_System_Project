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
        private string filePath = "students.txt";  // Path for storing student data
        private BindingSource studentBindingSource = new BindingSource(); // Binding source for DataGridView
        private List<Student> students = new List<Student>();  // List to hold student records

        public frmStudentManagementSystem()
        {
            InitializeComponent();
            CheckOrCreateFile(); // Ensure the file exists at startup
         //   LoadAllStudents();    //load all students from file at startup
        }
//====================================================================================================================================
        // Ensure the students.txt file exists, create if not
        private void CheckOrCreateFile()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }
//====================================================================================================================================
        //add a new student to the list and saves to textfile with name students.txt
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            // Call the IsInputValid method to check input
            if (!IsInputValid())
            {
                return; // Stop execution if validation fails
            }

            // Gather user input from form fields
            string studentId = txtStudentID.Text;
            string name = txtName.Text;
            string ageText = txtAge.Text;
            string course = txtCourse.Text;

            // Parse age as an integer (already validated in IsInputValid)
            int age = int.Parse(ageText);

            // Create a new student object and add to the list
            Student newStudent = new Student { StudentID = studentId, Name = name, Age = age, Course = course };
            students.Add(newStudent);

            // Append new student data to students.txt file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{studentId},{name},{age},{course}");
            }

            MessageBox.Show("Student added successfully!");
            ClearFields(); // Reset input fields after adding a student
            studentBindingSource.ResetBindings(false); // Refresh the BindingSource to update DataGridView
        }
//====================================================================================================================================
        // Loads all students from students.txt and displays in DataGridView
        private void btnViewAllStudents_Click(object sender, EventArgs e)
        {
              LoadAllStudents();
        }
//====================================================================================================================================
        // Reads all student records from students.txt and populates the student list
        private void LoadAllStudents()
        {
            students.Clear(); // Clear the current list before loading

            // Check if file exists and read each line to populate students list
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

            // Bind the list to DataGridView for display
            studentBindingSource.DataSource = students;
            dgvStudents.DataSource = studentBindingSource;
        }
//====================================================================================================================================
        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
        

        }
//====================================================================================================================================
        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
           
        }
        //====================================================================================================================================
        //====================================================================================================================================
        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {

        }
        //====================================================================================================================================
        // Saves all student records from the list to students.txt, overwriting existing file content
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
//====================================================================================================================================
        // Clears input fields on the form for easier data entry
        private void ClearFields()
        {
            txtStudentID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtCourse.Clear();
        }
//====================================================================================================================================
        // Method to validate that all required fields are filled
        private bool IsInputValid()
        {
            // Check if any textbox is empty or only contains whitespace
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtCourse.Text))
            {
                MessageBox.Show("All fields are required.");
                return false; // Return false if any field is empty
            }

            // Additional validation for Age to ensure it’s an integer
            if (!int.TryParse(txtAge.Text, out _))
            {
                MessageBox.Show("Please enter a valid age.");
                return false;
            }
            // Check for duplicate StudentID in the text file
            if (IsDuplicateStudentID(txtStudentID.Text))
            {
                MessageBox.Show("Student ID already exists. Please enter a unique ID.");
                return false;
            }

            return true; // Return true if all validations pass
        }
        //====================================================================================================================================
        private bool IsDuplicateStudentID(string studentId)
        {
            // Ensure file exists before reading
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        // Check if the first element (StudentID) matches the input
                        if (data.Length > 0 && data[0] == studentId)
                        {
                            return true; // Duplicate found
                        }
                    }
                }
            }
            return false; // No duplicate found
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Optional: Save all current student records to the file before logout
            SaveAllStudentsToFile();

            // Clear the in-memory list of students (if needed)
            students.Clear();

            // Show a message to confirm logout
            MessageBox.Show("You have been logged out.");

            // Close the form (or perform your desired logout action)
            this.Close();
        }
    }
}
    
