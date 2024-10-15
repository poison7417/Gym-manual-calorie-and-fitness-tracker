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
    public partial class registration_form : Form
    {
        public registration_form()
        {
            InitializeComponent();

            password.PasswordChar = '*';
            Com_password.PasswordChar = '*';
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void Back_to_login_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();

        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Register_Click(object sender, EventArgs e)
        {

            if (Special_Characters(username.Text))
            {
                MessageBox.Show("Username cannot contain special characters", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text) || string.IsNullOrWhiteSpace(Com_password.Text))
            {
                MessageBox.Show("Username and Password fields cannot be empty", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (string.IsNullOrWhiteSpace(Security_question.Text))
            {
                MessageBox.Show("Please select security question.", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (string.IsNullOrWhiteSpace(Security_answer.Text))
            {

                MessageBox.Show("Please enter security question answer.");
            }

            else if (!ValidatePass(password.Text))
            {
                MessageBox.Show("Password must has 12 charector long and Lowercase and Uppercase latter", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                password.Text = "";
                Com_password.Text = "";
            }

            else if (password.Text != Com_password.Text)
            {
                MessageBox.Show("Passwords do not match, please re-enter", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                password.Text = "";
                Com_password.Text = "";
                password.Focus();
                return;
            }
            else
            {

                try
                {
                    con.Open();

                    string checkUserQuery = "SELECT COUNT(*) FROM user_login WHERE u_name = @username";
                    cmd = new OleDbCommand(checkUserQuery, con);
                    cmd.Parameters.AddWithValue("@username", username.Text);
                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Username already exists. Please choose a different username.", "Registration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    string register = "INSERT INTO user_login (u_name, [u_password], u_Squestion, u_Sanswer, u_locked) VALUES (@p1, @p2, @p3, @p4, @p5)";
                    using (OleDbCommand cmd = new OleDbCommand(register, con))
                    {
                        cmd.Parameters.AddWithValue("@p1", username.Text);
                        cmd.Parameters.AddWithValue("@p2", password.Text);
                        cmd.Parameters.AddWithValue("@p3", Security_question.Text);
                        cmd.Parameters.AddWithValue("@p4", Security_answer.Text);
                        cmd.Parameters.AddWithValue("@p5", false);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Your account created", "Registration Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string User_name = username.Text;
                    CreateProfile sForm = new CreateProfile(User_name);
                    sForm.Show();
                    this.Hide();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
                
            }
        }

        public  bool ValidatePass(string pass)
        {
            if(pass.Length < 12)
            {
                return false;
            }

            bool has_Lower = pass.Any(char.IsLower);
            bool has_Upper = pass.Any(char.IsUpper);
            bool has_Digit = pass.Any(char.IsDigit);

            return has_Lower && has_Upper && has_Digit;

        }


        public bool Special_Characters(string str)
        {
            return str.Any(c => !char.IsLetterOrDigit(c));
        }


        public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                password.PasswordChar = '\0';
                Com_password.PasswordChar = '\0';
            }
            else
            {
                password.PasswordChar = '*';
                Com_password.PasswordChar = '*';
            }
        }
}   }
