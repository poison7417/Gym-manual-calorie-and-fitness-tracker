using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gym_management
{
    public partial class ForgotPassword : Form
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter dr = new OleDbDataAdapter();

        private void forgot_login_Click(object sender, EventArgs e)
        {
            string entered_username = username.Text.Trim();

            string query = "SELECT * FROM user_login WHERE u_name = @username";
            using (OleDbCommand cmd = new OleDbCommand(query, con))
            {
                cmd.Parameters.AddWithValue("username", entered_username);
                con.Open();
                using (OleDbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        string security_question = dr["u_Squestion"].ToString();
                        string security_answer = dr["u_Sanswer"].ToString();

                        string entered_question = Security_question.Text.Trim();
                        string entered_answer = Security_answer.Text.Trim();

                        if (security_question == entered_question && security_answer == entered_answer)
                        {
                            string password = dr["u_password"].ToString();
                            UnlockAccount(username.Text);
                            MessageBox.Show($"Your psaaword is: {password}");
                        }

                        else
                        {
                            MessageBox.Show("The security question or answer is incorrect. ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username not found.", "Incorrect username", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                con.Close();
            }

            Form1 sForm = new Form1();
            sForm.Show();
            this.Hide();


        }

        private void UnlockAccount(string username)
        {
            string lockAccountQuery = "UPDATE user_login SET u_locked = @locked WHERE u_name = @username";
            cmd = new OleDbCommand(lockAccountQuery, con);
            cmd.Parameters.AddWithValue("@locked", false);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
