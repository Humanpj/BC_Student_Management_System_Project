using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BC_Student_Management_System_Project
{
    public partial class LoginForm : Form
    {
        private string credentialsFilePath = "logincredentials.txt"; // Path for login credentials file

        public LoginForm()
        {
            InitializeComponent();
        }

       

        private bool IsUsernameDuplicate(string username)
        {
            if (File.Exists(credentialsFilePath))
            {
                foreach (var line in File.ReadLines(credentialsFilePath))
                {
                    var data = line.Split(',');
                    if (data[0] == username) // Username already exists
                        return true;
                }
            }
            return false;
        }

        private void RegisterUser(string username, string password)
        {
            
            // Save the new user with encrypted password
            using (StreamWriter writer = new StreamWriter(credentialsFilePath, true))
            {
                writer.WriteLine($"{username},{password}");
            }
            MessageBox.Show("Registration successful!");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }

            // Check credentials
            if (File.Exists(credentialsFilePath))
            {
                foreach (var line in File.ReadLines(credentialsFilePath))
                {
                    var data = line.Split(',');
                    if (data[0] == username && (data[1]) == password)
                    {
                        MessageBox.Show("Login successful!");
                        this.DialogResult = DialogResult.OK; // Successful login
                        this.Close();
                        return;
                    }
                }
            }
            MessageBox.Show("Invalid username or password.");
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.");
                return;
            }

            if (IsUsernameDuplicate(username))
            {
                MessageBox.Show("Username is already taken. Please choose another one.");
                return;
            }

            RegisterUser(username, password);
            ClearFields();
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
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

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            txtEnter("Username", txtUsername);
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            txtLeave("Username", txtUsername);
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtEnter("Password", txtPassword);
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            txtLeave("Password", txtPassword);
        }
    }
}
