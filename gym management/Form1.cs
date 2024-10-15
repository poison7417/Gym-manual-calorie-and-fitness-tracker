using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
namespace gym_management
{
    public partial class Form1 : Form
    {

        private int loginAttempts = 0;
        private const int maxLoginAttempts = 3;

        public Form1()
        {
            InitializeComponent();

            password.PasswordChar = '*';
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter dr = new OleDbDataAdapter();

        private void register_here_Click(object sender, EventArgs e)
        {
            registration_form sForm =  new registration_form();
            sForm.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                con.Open();
                string login = "SELECT * FROM user_login WHERE u_name = @username ";
                cmd = new OleDbCommand(login, con);
                cmd.Parameters.AddWithValue("@username", username.Text);
                OleDbDataReader dr = cmd.ExecuteReader();
                string user_name = username.Text;

                if (dr.Read())
                {
                    bool isLocked = (bool)dr["u_locked"];
                    if (isLocked)
                    {
                        MessageBox.Show("Account is locked. Please use the 'Forgot Password' option to recover password.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Move the password check inside the block where the username is found
                    dr.Close();
                    login = "SELECT * FROM user_login WHERE u_name = @username AND [u_password] = @password";
                    cmd = new OleDbCommand(login, con);
                    cmd.Parameters.AddWithValue("@username", username.Text);
                    cmd.Parameters.AddWithValue("@password", password.Text);
                    dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        string Username = username.Text;
                        MessageBox.Show("Login successful");
                        ResetLoginAttempts();
                        dashboard sForm = new dashboard(Username);
                        sForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Handle incorrect password here
                        IncrementLoginAttempts(); // Increment the login attempts counter
                        if (loginAttempts >= maxLoginAttempts)
                        {
                               string lockAccountQuery = "UPDATE user_login SET u_locked = @locked WHERE u_name = @username";
                               cmd = new OleDbCommand(lockAccountQuery, con);
                               cmd.Parameters.AddWithValue("@locked", true);
                               cmd.Parameters.AddWithValue("@username", username.Text);
                               cmd.ExecuteNonQuery();

                            MessageBox.Show("Maximum login attempts exceeded. Account locked. Please use the 'Forgot Password' option recover password.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password. Attempts remaining: " + (maxLoginAttempts - loginAttempts), "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            password.Text = "";
                            
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username. Please enter a valid username.", "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                
            }

        }



        private void ResetLoginAttempts()
        {
            loginAttempts = 0;
        }

        private void IncrementLoginAttempts()
        {
            loginAttempts++;
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                password.PasswordChar = '\0';
               
            }
            else
            {
                password.PasswordChar = '*';
                
            }
        }

        private void forgot_password_Click(object sender, EventArgs e)
        {
            ForgotPassword sForm = new ForgotPassword();
            sForm.Show();
            this.Hide();
        }
    }
}
