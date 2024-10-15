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
    public partial class strength_training : Form
    {
        public string user_name { get; }
        public strength_training(string username)
        {
            user_name = username;
            InitializeComponent();
            weight = GetUserWeight(username);

            
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        public double weight;
        public double user_weight;

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

        public void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(time.Text))
            {
                MessageBox.Show("Time cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Intensity cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Exercise type cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                int time_min = Convert.ToInt32(time.Text);
                string intensity = comboBox1.Text;
                string exerType = comboBox2.Text;
                double userweight = weight;


                double calorieBurn = calculate_calorieBurn(time_min, intensity, exerType, userweight);

                try
                {
                    con.Open();
                    string yoga = "INSERT INTO tbl_strength (duration_min, intensity, type_exer, calorie_burn, u_name, date_time) VALUES(?,?,?,?,?,?)";
                    using (OleDbCommand cmd = new OleDbCommand(yoga, con))
                    {
                        cmd.Parameters.AddWithValue("@p1", time_min);
                        cmd.Parameters.AddWithValue("@p2", intensity);
                        cmd.Parameters.AddWithValue("@p3", exerType);
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


        public double calculate_calorieBurn(int time_min, string intensity, string exerType, double userweight)
        {
            double metValue = GetMETValue(exerType);
            double setMetValue = METValue_Intensity(metValue, intensity);
            double durationHours = time_min / 60.0; // Convert minutes to hours
            double calorieBurn = setMetValue * userweight * durationHours; // Weight is in kg

            return calorieBurn;
        }


        public double GetMETValue(string exerType)
        {
            Dictionary<string, double> metValues = new Dictionary<string, double>
                {
                    { "Squats", 3.5 },
                    { "Deadlifts", 3.8 },
                    { "Bench Presses", 4.0 },
                    { "Bicep Curls", 3.0 },
                };

            if (metValues.ContainsKey(exerType))
            {
                return metValues[exerType];
            }
            else
            {
                return 3.0; // Default MET value
            }
        }

        public double METValue_Intensity(double metValue, string intensity)
        {
            Dictionary<string, double> intensityFactors = new Dictionary<string, double>
                {
                    { "Low", 0.9 },
                    { "Moderate", 1.0 },
                    { "High", 1.1 },
                };

            if (intensityFactors.ContainsKey(intensity))
            {
                return metValue * intensityFactors[intensity];
            }
            else
            {
                return metValue * intensityFactors["Moderate"]; // Default intensity factor
            }
        }

    }

}
