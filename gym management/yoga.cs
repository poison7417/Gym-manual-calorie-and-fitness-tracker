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
    public partial class yoga : Form
    {
        public string user_name { get; }
        public yoga(string username)
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

            else if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Intensity cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else if (string.IsNullOrWhiteSpace(comboBox2.Text))
            {
                MessageBox.Show("Yoga type cannot be empty", "fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            else
            {
                int time_min = Convert.ToInt32(time.Text);
                string intensity = comboBox1.Text;
                string yogaType = comboBox2.Text;


                double calorieBurn = calculate_calorieBurn(time_min, intensity, yogaType);

                try
                {
                    con.Open();
                    string yoga = "INSERT INTO tbl_yoga (duration_min, intensity, yoga_type, calorie_burn, u_name, date_time) VALUES(?,?,?,?,?,?)";
                    using (OleDbCommand cmd = new OleDbCommand(yoga, con))
                    {
                        cmd.Parameters.AddWithValue("@p1", time_min);
                        cmd.Parameters.AddWithValue("@p2", intensity);
                        cmd.Parameters.AddWithValue("@p3", yogaType);
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


        private double calculate_calorieBurn(int time_min, string intensity, string yogaType)
        {
            double metValue = GetMETValue(yogaType);
            
            double durationHours = time_min / 60.0; 
            double calorieBurn = metValue * weight * durationHours; 

            return calorieBurn;
        }

        private double GetMETValue(string yogaType)
        {
           
            Dictionary<string, double> metValues = new Dictionary<string, double>
            {
                { "Cardio or Sculpt Yoga", 5.0 },
                { "Vinyasa", 4.0 },
                { "Bikram", 3.5 },
                { "Jivamukti", 4.5 },
                { "Power Yoga", 4.2 },
        
            };

            if (metValues.ContainsKey(yogaType))
            {
                return metValues[yogaType];
            }
            else
            {
                return 3.0;
            }
        }




    }
}

