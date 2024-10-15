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
    public partial class cycling : Form
    {
        public string user_name { get; }
        public cycling(string username)
        {
            user_name = username;
            InitializeComponent();

            weight = GetUserWeight(username);
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private double weight;

        private double GetUserWeight(string username)
        {
            double userweight = 0;

            try
            {
                con.Open();
                string query = "SELECT u_weight FROM tbl_user_profile WHERE u_name = ?";
                using (OleDbCommand cmd = new OleDbCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        userweight = Convert.ToDouble(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving user weight: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }

            return userweight;

        }



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
            if (string.IsNullOrWhiteSpace(time.Text))
            {
                MessageBox.Show("Time cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(distance.Text))
            {
                MessageBox.Show("Distance cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Intensity cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                int time_min = Convert.ToInt32(time.Text);
                double distance_km = Convert.ToDouble(distance.Text);
                string intensity = comboBox1.Text;


                double calorieBurn = calculate_calorieBurn(time_min, distance_km, intensity);

                try
                {
                    con.Open();
                    string cycling = "INSERT INTO tbl_cycling (duration_min, intensity, distance, calorie_burn, u_name, date_time) VALUES(?,?,?,?,?,?)";
                    using (OleDbCommand cmd = new OleDbCommand(cycling, con))
                    {
                        cmd.Parameters.AddWithValue("@p1", time_min);
                        cmd.Parameters.AddWithValue("@p2", intensity);
                        cmd.Parameters.AddWithValue("@p3", distance_km);
                        cmd.Parameters.AddWithValue("@p4", calorieBurn);
                        cmd.Parameters.AddWithValue("@p5", user_name);
                        cmd.Parameters.AddWithValue("@p6", dateTimePicker1.Value);
                        cmd.ExecuteNonQuery();


                    }
                    MessageBox.Show($"Calories burned: {calorieBurn}", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting data into database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }

                string Username = user_name;
                dashboard sForm = new dashboard(Username);
                sForm.Show();
                this.Hide();
            }
        }

            private double calculate_calorieBurn(int time_min, double distance_km, string intensity)
            {
                double metValue;
                switch (intensity)
                {
                    case "Low":
                        metValue = 4.0;
                        break;
                    case "Moderate":
                        metValue = 6.0;
                        break;
                    case "High":
                        metValue = 8.0;
                        break;
                    default:
                        // Default to moderate intensity if intensity is not specified
                        metValue = 6.0;
                        break;
                }

            double calorieBurn = metValue * weight * (time_min / 60.0);
            return calorieBurn;

            }
    }
}
