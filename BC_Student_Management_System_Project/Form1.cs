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
        private FileHandler studentFile;
        private FileHandler summaryFile;

        public frmStudentManagementSystem()
        {
            InitializeComponent();

            studentFile = new FileHandler("students.txt");
            summaryFile = new FileHandler("summary.txt");
            studentBindingSource = new BindingSource();

            studentFile.CheckOrCreateFile(); // Ensure students file exists at startup
            summaryFile.CheckOrCreateFile(); //Ensure summary file exists at startup

            students = LinesToStudents(studentFile.ReadAllLines()); //load all students from textFile
        }

        //converts list of strings objects to list of student objects
        //used to convert textFile content to student objects
        private static List<Student> LinesToStudents(List<string> lines)
        {
            List<Student> students = new List<Student>();

            foreach (string line in lines)
            {
                string[] details = line.Split(',');
                students.Add(new Student(details[0], details[1], int.Parse(details[2]), details[3]));
            }

            return students;
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

            MessageBox.Show("Student added successfully!");
            ClearFields(); // Reset input fields after adding a student
            studentBindingSource.ResetBindings(false); // Refresh the BindingSource to update DataGridView
        }
        //====================================================================================================================================
        
        // Loads all students from students.txt and displays in DataGridView
        private void btnViewAllStudents_Click(object sender, EventArgs e)
        {
            students = LinesToStudents(studentFile.ReadAllLines());

            dgvStudents.DataSource = students;
        }
        //====================================================================================================================================
        
        //check if updated values are valid
        public (string, string) checkUpdates(System.Windows.Forms.TextBox textBox, string text, string studentProperty)
        {
            string edit = string.Empty;
            string change;

            //check if value entered and not the same
            if (!string.IsNullOrWhiteSpace(textBox.Text) && (textBox.Text != text))
            {
                //check if age is number
                if (text == "Age" && !(int.TryParse(textBox.Text, out int yes)))
                {
                    change = studentProperty;
                    return ($"{studentProperty} --> (No Change, Invalid Number)\n", change);
                }

                //check if StudentID already exists
                if (text == "StudentId")
                {
                    if (studentFile.SearchID(txtStudentID.Text))
                    {
                        change = studentProperty;
                        return ($"{studentProperty} --> (No Change, Duplicate ID)\n", change);
                    }
                }

                edit += $"{studentProperty} --> {textBox.Text}\n";
                change = textBox.Text;
            }
            else
            {
                edit += $"{studentProperty} --> (No Change)\n";
                change = studentProperty;
            }

            return (edit, change);
        }
        //====================================================================================================================================
        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            //get selected record from DataGridView
            int rowIndex = dgvStudents.CurrentCell.RowIndex;

            string edit = string.Empty;
            string StudentID = string.Empty;
            string Name = string.Empty;
            int Age;
            string Course = string.Empty;

            var updates = checkUpdates(txtStudentID, "StudentId", students[rowIndex].StudentID);
            edit += updates.Item1;
            StudentID = updates.Item2;

            updates = checkUpdates(txtName, "Name", students[rowIndex].Name);
            edit += updates.Item1;
            Name = updates.Item2;

            updates = checkUpdates(txtAge, "Age", students[rowIndex].Age.ToString());
            edit += updates.Item1;
            Age = int.Parse(updates.Item2);

            updates = checkUpdates(txtCourse, "Course", students[rowIndex].Course);
            edit += updates.Item1;
            Course = updates.Item2;

            //confirm message
            var result = MessageBox.Show($"Are you sure you want to CHANGE:\n\n{edit}", "Are you sure?", MessageBoxButtons.YesNo);

            //check user response
            if (result == DialogResult.Yes)
            {
                //confirmed
                students[rowIndex].StudentID = StudentID;
                students[rowIndex].Name = Name;
                students[rowIndex].Age = Age;
                students[rowIndex].Course = Course;

                studentFile.ReWrite(students);

                //update student info on DataGridView
                btnViewAllStudents_Click(sender, e);

                MessageBox.Show("Successfully updated Student Information!", "Success");

                ClearFields();
            }
            else if (result == DialogResult.No)
            {
                //canceled
                MessageBox.Show("Canceled", "Canceled");
            }
            
        }
        //====================================================================================================================================
        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            //get record from DataGridView
            int rowIndex = dgvStudents.CurrentCell.RowIndex;

            string deleted = $"ID: {students[rowIndex].StudentID}\n" +
                             $"Name: {students[rowIndex].Name}\n" +
                             $"Age: {students[rowIndex].Age}\n" +
                             $"Course: {students[rowIndex].Course}";

            //confirm message
            var result = MessageBox.Show($"Are you sure you want to DELETE:\n\n{deleted}", "Are you sure?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                //confirmed
                students.Remove(students[rowIndex]);
                studentFile.ReWrite(students);
                btnViewAllStudents_Click(sender, e);

                MessageBox.Show("Successfully DELETED Student Information!", "Success");

                ClearFields();
            }
            else if (result == DialogResult.No)
            {
                //canceled
                MessageBox.Show("Canceled", "Canceled");
            }

        }
        //====================================================================================================================================
        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {
            //get students from textFile
            students = LinesToStudents(studentFile.ReadAllLines());
            
            //check if students exists
            if(students.Count == 0)
            {
                //no students to display summary
                MessageBox.Show("No student data available");
                summaryFile.ReWrite(new List<string>());
            }
            else
            {
                //student records exist
                int totalStudents, totalAge = 0;
                double avgAge = 0;

                totalStudents = students.Count;

                //go through all records
                foreach (Student student in students)
                {
                    totalAge += student.Age;
                }

                avgAge = totalAge / totalStudents;

                //ReWrite method uses list as parameter
                List<string> lines = new List<string>()
                {
                    "Total Students:" + totalStudents.ToString(),
                    "Average Age:" + avgAge.ToString()
                };

                //write summary to textFile
                summaryFile.ReWrite(lines);
                // Update label text properties with values
                lblTotalStudents.Text = lines[0];
                lblAverageAge.Text = lines[1] ;
                
            }
            
        }
        //====================================================================================================================================
        // Clears input fields on the form for easier data entry
        private void ClearFields()
        {
            txtStudentID.Clear();
            txtLeave("StudentId", txtStudentID);
            txtName.Clear();
            txtLeave("Name", txtName);
            txtAge.Clear();
            txtLeave("Age", txtAge);
            txtCourse.Clear();
            txtLeave("Course", txtCourse);
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

        private void btnMembers_Click(object sender, EventArgs e)
        {
            // Format the student information as a single string
            string studentInfo = "Petrus Human - ID: 577842\n" +
                                 "Mark Brown - ID: 600190\n" +
                                 "Ken Aspeling - ID: 600551";

            // Display the student data in a MessageBox
            MessageBox.Show(studentInfo, "Student Members Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
    
