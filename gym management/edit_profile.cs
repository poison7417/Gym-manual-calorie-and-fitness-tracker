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
    public partial class edit_profile : Form
    {
        public string user_name { get; }
        public edit_profile(string username)
        {
            user_name = username;
            InitializeComponent();
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void label1_Click(object sender, EventArgs e)
        {
            string Username = user_name;
            dashboard sForm = new dashboard(Username);
            sForm.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (  string.IsNullOrWhiteSpace(weight.Text) || string.IsNullOrWhiteSpace(height.Text)  || dateTimePicker1.Value == null || string.IsNullOrWhiteSpace(calorie_goal.Text) )
            {
                MessageBox.Show("Weight, Height, Calorie Goal and Date is compulsory", "Update failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                con.Open();

                string update_query;

                if (!string.IsNullOrWhiteSpace(name.Text))
                {
                    update_query = "UPDATE tbl_user_profile SET u_fname = @FullName WHERE u_name = @Username";
                    using (OleDbCommand cmd = new OleDbCommand(update_query, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", name.Text);
                        cmd.Parameters.AddWithValue("@Username", user_name);
                        cmd.ExecuteNonQuery();
                    }
                }

                
                update_query = "UPDATE tbl_user_profile SET u_age = @Age WHERE u_name = @Username";
                using (OleDbCommand cmd = new OleDbCommand(update_query, con))
                {
                    cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(age.Text));
                    cmd.Parameters.AddWithValue("@Username", user_name);
                    cmd.ExecuteNonQuery();
                }

               
                update_query = "UPDATE tbl_user_profile SET u_weight = @Weight WHERE u_name = @Username";
                using (OleDbCommand cmd = new OleDbCommand(update_query, con))
                {
                    cmd.Parameters.AddWithValue("@Weight", Convert.ToDouble(weight.Text));
                    cmd.Parameters.AddWithValue("@Username", user_name);
                    cmd.ExecuteNonQuery();
                }

                
                update_query = "UPDATE tbl_user_profile SET u_height = @Height WHERE u_name = @Username";
                using (OleDbCommand cmd = new OleDbCommand(update_query, con))
                {
                    cmd.Parameters.AddWithValue("@Height", Convert.ToDouble(height.Text));
                    cmd.Parameters.AddWithValue("@Username", user_name);
                    cmd.ExecuteNonQuery();
                }


                update_query = "UPDATE tbl_user_profile SET update_date = @UpdateDate WHERE u_name = @Username";
                using (OleDbCommand cmd = new OleDbCommand(update_query, con))
                {
                    cmd.Parameters.AddWithValue("@UpdateDate", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@Username", user_name);
                    cmd.ExecuteNonQuery();
                }


                update_query = "UPDATE tbl_user_profile SET cal_goal = @CalorieGoal WHERE u_name = @Username";
                using (OleDbCommand cmd = new OleDbCommand(update_query, con))
                {
                    cmd.Parameters.AddWithValue("@CalorieGoal", Convert.ToInt32(calorie_goal.Text));
                    cmd.Parameters.AddWithValue("@Username", user_name);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Profile updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                string Username = user_name;
                dashboard sForm = new dashboard(Username);
                sForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
                con.Close();
            }


        }
    }
}
