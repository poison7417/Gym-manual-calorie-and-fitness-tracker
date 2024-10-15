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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace gym_management
{
    public partial class walking : Form
    {
        public string user_name { get; }
        public walking(string username)
        {
            user_name = username;
            InitializeComponent();

            weight = GetUserWeight(username);

            double user_weight = weight;
        }


        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();


        internal double weight;
        internal double user_weight;

        private void label1_Click(object sender, EventArgs e)
        {
            string Username = user_name;
            dashboard sForm = new dashboard(Username);
            sForm.Show();
            this.Hide();
        }

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

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(steps.Text))
            {
                MessageBox.Show("Steps cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(distance.Text))
            {
                MessageBox.Show("Diatance cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(time.Text))
            {
                MessageBox.Show("Time cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                int stepsCount = Convert.ToInt32(steps.Text);
                double distanceKm = (Convert.ToDouble(distance.Text)/1000); 
                int durationMinutes = Convert.ToInt32(time.Text);
                double userweight = user_weight;


                double speedKmph = distanceKm / (durationMinutes / 60.0);


                double calorieBurn = CalculateCalorieBurn(stepsCount,  distanceKm, durationMinutes,  userweight);
                try
                {
                    con.Open();
                    string walking = "INSERT INTO tbl_walking (steps_taken, distance_cover, time_taken, calorie_burn, u_name, date_time) VALUES(?,?,?,?,?,?)";
                    using (OleDbCommand cmd = new OleDbCommand(walking, con))
                    {
                        string users_name = user_name;


                        cmd.Parameters.AddWithValue("@p1", stepsCount);
                        cmd.Parameters.AddWithValue("@p2", distanceKm);
                        cmd.Parameters.AddWithValue("@p3", durationMinutes);
                        cmd.Parameters.AddWithValue("@p4", calorieBurn);
                        cmd.Parameters.AddWithValue("@p5", users_name);
                        cmd.Parameters.AddWithValue("@p6", dateTimePicker1.Value);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Calories burned: {calorieBurn}"  , "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: something went wrong" + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public double CalculateCalorieBurn(int stepsCount, double distanceKm, int durationMinutes, double userweight)
        {
            double speedKmph = distanceKm / (durationMinutes / 60.0);

            double metValue;
            if (speedKmph < 2.0)
            {
                metValue = 2.0;
            }
            else if (speedKmph >= 2.0 && speedKmph < 4.0)
            {
                metValue = 3.0;
            }
            else
            {
                metValue = 4.0;
            }

            double calorieBurn = metValue * userweight * (durationMinutes / 60.0);
            return calorieBurn;
        }

    }
}
