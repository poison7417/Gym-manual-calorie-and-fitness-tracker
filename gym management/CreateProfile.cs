using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace gym_management
{
    public partial class CreateProfile : Form
    {
        public string username { get; }
        public CreateProfile(string User_name)
        {
            username = User_name;

            InitializeComponent();
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(name.Text) || string.IsNullOrWhiteSpace(age.Text) || string.IsNullOrWhiteSpace(weight.Text) || string.IsNullOrWhiteSpace(height.Text) || string.IsNullOrWhiteSpace(gender.Text) || dateTimePicker1.Value == null)
            {
                MessageBox.Show("All fields are required", "Registration failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(age.Text, out int ageValue) || !double.TryParse(weight.Text, out double weightValue) || !double.TryParse(height.Text, out double heightValue))
            {
                MessageBox.Show("Invalid age, weight, or height", "Registration failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                con.Open();
                string profile = "INSERT INTO tbl_user_profile (u_fname, u_age, u_weight, u_height, u_gender, date_time, u_name, cal_goal) VALUES(?,?,?,?,?,?,?,?)";
                using (OleDbCommand cmd = new OleDbCommand(profile, con))
                {
                    string users_name  = username;




                    cmd.Parameters.AddWithValue("@p1", name.Text);
                    cmd.Parameters.AddWithValue("@p2", age.Text);
                    cmd.Parameters.AddWithValue("@p3", weight.Text);
                    cmd.Parameters.AddWithValue("@p4", height.Text);
                    cmd.Parameters.AddWithValue("@p5", gender.Text);
                    cmd.Parameters.AddWithValue("@p6", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@p8", users_name);
                    cmd.Parameters.AddWithValue("@p9", textBox1.Text);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Your Profile Created", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: something went wrong" + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            string Username = username;
            dashboard sForm = new dashboard(Username);
            sForm.Show();
            this.Hide();
            
        }

      

        private void label12_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}