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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace gym_management
{
    public partial class RopeJumping : Form
    {
        public string user_name { get; }
        public RopeJumping(string username)
        {
            user_name = username;
            InitializeComponent();
            weight = GetUserWeight(username);
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        public double weight;

        public double GetUserWeight(string username)
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
            if (string.IsNullOrWhiteSpace(act_time.Text))
            {
                MessageBox.Show("Time cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Intensity cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(number_jump.Text))
            {
                MessageBox.Show("Number of jumps cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {



                int time_min = Convert.ToInt32(act_time.Text);
                string intensity = comboBox1.Text;
                int num_jump = Convert.ToInt32(number_jump.Text);


                double calorieBurn = calculate_calorieBurn(time_min, intensity, num_jump);

                try
                {
                    con.Open();
                    string yoga = "INSERT INTO tbl_ropeJump (duration_min, intensity, jump_num, calorie_burn, u_name, date_time) VALUES(?,?,?,?,?,?)";
                    using (OleDbCommand cmd = new OleDbCommand(yoga, con))
                    {
                        cmd.Parameters.AddWithValue("@p1", time_min);
                        cmd.Parameters.AddWithValue("@p2", intensity);
                        cmd.Parameters.AddWithValue("@p3", num_jump);
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

        public double calculate_calorieBurn(int time_min, string intensity, int num_jump)
        {

            if (time_min <= 0)
            {
                throw new ArgumentException("Time must be greater than zero.", nameof(time_min));
            }

            double jumpsPerMinute = num_jump / time_min;


            double energyPerJump = EnergyPerJump(intensity);


            double totalEnergy = jumpsPerMinute * energyPerJump;

            return totalEnergy;
        }

        public double EnergyPerJump(string intensity)
        {
            double energyPerJump;

            switch (intensity)
            {
                case "Low":
                    energyPerJump = 0.05;
                    break;
                case "Moderate":
                    energyPerJump = 0.1;
                    break;
                case "High":
                    energyPerJump = 0.15;
                    break;
                default:
                    energyPerJump = 0.1; // Default to moderate intensity
                    break;
            }

            return energyPerJump;

        }
    }
}
