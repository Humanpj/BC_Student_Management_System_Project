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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BC_Student_Management_System_Project
{
    public partial class frmStudentManagementSystem : Form
    {
        private BindingSource studentBindingSource = new BindingSource(); // Binding source for DataGridView
        private List<Student> students = new List<Student>();  // List to hold student records
        private Transactions transactions;
        private FileHandler studentFile;
        private FileHandler summaryFile;

        public frmStudentManagementSystem()
        {
            InitializeComponent();

            //   LoadAllStudents();    //load all students from file at startup

            transactions = new Transactions();
            studentFile = new FileHandler("students.txt");
            studentBindingSource = new BindingSource();
            //LoadAllStudents();

            studentFile.CheckOrCreateFile(); // Ensure the file exists at startup
            students = Transactions.LinesToStudents(studentFile.ReadAllLines());
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
            Student newStudent = new Student(studentId, name, age, course);
            students.Add(newStudent);

            // Append new student data to students.txt file
            studentFile.Write(newStudent.ToString());
            /*using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{studentId},{name},{age},{course}");
            }*/

            //Transactions.AddStudent(newStudent);
            MessageBox.Show("Student added successfully!");
            ClearFields(); // Reset input fields after adding a student
            studentBindingSource.ResetBindings(false); // Refresh the BindingSource to update DataGridView
        }
        //====================================================================================================================================
        // Loads all students from students.txt and displays in DataGridView

        //Ken -> implement
        private void btnViewAllStudents_Click(object sender, EventArgs e)
        {
            students = Transactions.LinesToStudents(studentFile.ReadAllLines());

            dgvStudents.DataSource = students;

            //LoadAllStudents();
        }
        //====================================================================================================================================
        //REPLACED WITH Transactions.LinesToStudents(FileHandler.ReadAllLines);

        // Reads all student records from students.txt and populates the student list
        /*private void LoadAllStudents()
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
        }*/
        //====================================================================================================================================
        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            int rowIndex = dgvStudents.CurrentCell.RowIndex;

            if (!string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                students[rowIndex].StudentID = txtStudentID.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                students[rowIndex].Name = txtName.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtAge.Text) && int.TryParse(txtAge.Text, out int age))
            {
                students[rowIndex].Age = age;
            }

            if (!string.IsNullOrWhiteSpace(txtCourse.Text))
            {
                students[rowIndex].Course = txtCourse.Text;
            }

            studentFile.ReWrite(students);

            btnViewAllStudents_Click(sender, e);
        }
        //====================================================================================================================================
        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            int rowIndex = dgvStudents.CurrentCell.RowIndex;

            students.Remove(students[rowIndex]);

            studentFile.ReWrite(students);

            btnViewAllStudents_Click(sender, e);
        }
        //====================================================================================================================================
        //====================================================================================================================================
        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {
            int totalStudents, totalAge = 0;
            double avgAge = 0;

            students = Transactions.LinesToStudents(studentFile.ReadAllLines());

            totalStudents = students.Count;

            foreach (Student student in students)
            {
                totalAge += student.Age;
            }

            avgAge = totalAge / totalStudents;

            lblTotalStudents.Text = "Total Students: " + totalStudents.ToString();
            lblAverageAge.Text = "Average Age: " + avgAge.ToString();
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
                string.IsNullOrWhiteSpace(txtCourse.Text) ||
                txtStudentID.Text == "StudentId" ||
                txtName.Text == "Name" ||
                txtAge.Text == "Age" ||
                txtCourse.Text == "Course")
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
            if (studentFile.SearchID(txtStudentID.Text))
            {
                MessageBox.Show("Student ID already exists. Please enter a unique ID.");
                return false;
            }

            return true; // Return true if all validations pass
        }
        //====================================================================================================================================

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Optional: Save all current student records to the file before logout
            studentFile.ReWrite(students);

            // Clear the in-memory list of students (if needed)
            students.Clear();

            // Show a message to confirm logout
            MessageBox.Show("You have been logged out.");

            // Close the form (or perform your desired logout action)
            this.Close();
        }

        private void frmStudentManagementSystem_Load(object sender, EventArgs e)
        {
            // Show the LoginForm as a modal dialog
            LoginForm loginForm = new LoginForm();

            // If login fails (DialogResult not OK), close the main form
            if (loginForm.ShowDialog() != DialogResult.OK)
            {
                // Close the main form if login is not successful
                this.Close();
            }
        }

        public void txtEnter(string text, System.Windows.Forms.TextBox textBox)
        {
            if (textBox.Text == text)
            {
                textBox.Text = "";

                textBox.ForeColor = Color.Black;
            }
        }

        public void txtLeave(string text, System.Windows.Forms.TextBox textBox)
        {
            if (textBox.Text == "")
            {
                textBox.Text = text;

                textBox.ForeColor = Color.Gray;
            }
        }

        private void txtStudentID_Enter(object sender, EventArgs e)
        {
            txtEnter("StudentId", txtStudentID);
        }

        private void txtStudentID_Leave(object sender, EventArgs e)
        {
            txtLeave("StudentId", txtStudentID);
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtEnter("Name", txtName);
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            txtLeave("Name", txtName);
        }

        private void txtAge_Enter(object sender, EventArgs e)
        {
            txtEnter("Age", txtAge);
        }

        private void txtAge_Leave(object sender, EventArgs e)
        {
            txtLeave("Age", txtAge);
        }

        private void txtCourse_Enter(object sender, EventArgs e)
        {
            txtEnter("Course", txtCourse);
        }

        private void txtCourse_Leave(object sender, EventArgs e)
        {
            txtLeave("Course", txtCourse);
        }
    }
}
    
