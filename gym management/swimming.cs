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

namespace gym_management
{
    public partial class swimming : Form
    {
        public string user_name { get; }
        public swimming(string username)
        {
            user_name = username;
            InitializeComponent();

            weight = GetUserWeight(username);
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private double weight;



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

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(num_laps.Text))
            {
                MessageBox.Show("Number of laps cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if(string.IsNullOrWhiteSpace(time.Text))
            {
                MessageBox.Show("Time taken cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if(string.IsNullOrWhiteSpace(heart_rate.Text))
            {
                MessageBox.Show("Heart rate cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                int number_Laps = Convert.ToInt32(num_laps.Text);
                int time_Taken = Convert.ToInt32(time.Text);
                int avg_HeartRate = Convert.ToInt32(heart_rate.Text);


                double calorieBurn = calculate_calorieBurn(number_Laps, time_Taken, avg_HeartRate);

                try
                {
                    con.Open();
                    string swimming = "INSERT INTO tbl_swimming (num_laps, time_taken, avg_heartRate, calorie_burn, u_name, date_time) VALUES(?,?,?,?,?,?)";
                    using(OleDbCommand cmd = new OleDbCommand(swimming, con))
                    {
                        cmd.Parameters.AddWithValue("@p1", number_Laps);
                        cmd.Parameters.AddWithValue("@p2", time_Taken);
                        cmd.Parameters.AddWithValue("@p3", avg_HeartRate);
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

        private double calculate_calorieBurn(int number_Laps, int time_Taken, int avg_HeartRate)
        {
            double met_value = 5.8;
            double user_weight = weight;

            double time = time_Taken/60;


            double calorieBurned = met_value * user_weight * time;

            return calorieBurned;

        }
    }
}
