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
        private DataHandler dataHandler;
        private BindingSource studentBindingSource;

        public frmStudentManagementSystem()
        {
            InitializeComponent();
            dataHandler = new DataHandler();
            studentBindingSource = new BindingSource();
            LoadAllStudents();
        }

        // Add a new student
        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            if (!IsInputValid())
            {
                return;
            }

            string studentId = txtStudentID.Text;
            string name = txtName.Text;
            int age = int.Parse(txtAge.Text);
            string course = txtCourse.Text;

            Student newStudent = new Student { StudentID = studentId, Name = name, Age = age, Course = course };
            dataHandler.AddStudent(newStudent);

            MessageBox.Show("Student added successfully!");
            ClearFields();
            LoadAllStudents();
        }

        // Load all students and bind to DataGridView
        private void LoadAllStudents()
        {
            studentBindingSource.DataSource = dataHandler.GetStudents();
            dgvStudents.DataSource = studentBindingSource;
        }

        // Update selected student
        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
           
        }

        // Delete selected student
        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
          
        }

        // Generate summary of students
        private void btnGenerateSummary_Click(object sender, EventArgs e)
        {
          
        }

        // Validate input fields
        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                string.IsNullOrWhiteSpace(txtCourse.Text))
            {
                MessageBox.Show("All fields are required.");
                return false;
            }

            if (!int.TryParse(txtAge.Text, out _))
            {
                MessageBox.Show("Please enter a valid age.");
                return false;
            }

            if (dataHandler.GetStudents().Any(s => s.StudentID == txtStudentID.Text))
            {
                MessageBox.Show("Student ID already exists. Please enter a unique ID.");
                return false;
            }

            return true;
        }

        // Clear input fields
        private void ClearFields()
        {
            txtStudentID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtCourse.Clear();
        }

        private void frmStudentManagementSystem_Load(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            if (loginForm.ShowDialog() != DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You have been logged out.");
            this.Close();
        }

        private void btnViewAllStudents_Click(object sender, EventArgs e)
        {
            LoadAllStudents();
        }
    }
}