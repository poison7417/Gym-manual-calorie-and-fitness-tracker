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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace gym_management
{
    public partial class dashboard : Form
    {

        public string user_name { get;}

        public DateTime current_date { get; }
        public dashboard(string Username)
        {
            user_name = Username;
            current_date = System.DateTime.Today;

            InitializeComponent();

            label1.Text = "Welcome " + user_name + "";
            Fetch_UserData();

            

        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source = user_database.mdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void label2_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }


        private void Fetch_UserData()
        {

            int calorieGoal = 0;
            int sum_walk = 0;
            int sum_swim = 0;
            int sum_cyc = 0;
            int sum_yoga = 0;
            int sum_stren = 0;
            int sum_jump = 0;

            double calorieBurn_per = 0.0;
            try
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM tbl_user_profile INNER JOIN user_login ON tbl_user_profile.u_name = user_login.u_name WHERE user_login.u_name = @username";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@username", user_name);
                        using (OleDbDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                Namelabel.Text = "Name: " + rd["u_fname"].ToString();
                                Agelabel.Text = "Age: " + rd["u_age"].ToString();
                                weightlabel.Text = "Weight: " + rd["u_weight"].ToString() + " kg";
                                Heightlabel.Text = "Height: " + rd["u_height"].ToString() + " cm";
                                Genderlabel.Text = "Gender: " + rd["u_gender"].ToString();
                                calorie_goal.Text = "Calorie goal: " + rd["cal_goal"].ToString();

                                calorieGoal = Convert.ToInt32(rd["cal_goal"]);
                            }
                        }
                    }

                    
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Data Fetching Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }



                try
                {
                    con.Open();
                    string walk_cal = "SELECT SUM(calorie_burn) FROM tbl_walking WHERE u_name = @username AND FORMAT(date_time, 'MM/dd/yyyy') = FORMAT(@dateTime, 'MM/dd/yyyy')";
                    using (OleDbCommand cmd = new OleDbCommand(walk_cal, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user_name);
                        cmd.Parameters.AddWithValue("@dateTime", current_date);

                        object Cal = cmd.ExecuteScalar();
                        if (Cal != null && Cal != DBNull.Value)
                        {
                            int cal_burn = Convert.ToInt32(Cal);
                            Walk_Cal.Text = "Walking: " + Cal.ToString();

                            sum_walk = cal_burn;

                        }
                      
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving total calorie burn: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }


                try
                {
                    con.Open();
                    string swim_cal = "SELECT SUM(calorie_burn) FROM tbl_swimming WHERE u_name = @username AND FORMAT(date_time, 'MM/dd/yyyy') = FORMAT(@dateTime, 'MM/dd/yyyy')";
                    using (OleDbCommand cmd = new OleDbCommand(swim_cal, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user_name);
                        cmd.Parameters.AddWithValue("@dateTime", current_date);

                        object Cal = cmd.ExecuteScalar();
                        if (Cal != null && Cal != DBNull.Value)
                        {
                            int cal_burn = Convert.ToInt32(Cal);
                            swimming_cal.Text = "Swimming: " + Cal.ToString();

                            sum_swim = cal_burn;

                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving total calorie burn: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }


                try
                {
                    con.Open();
                    string cycl_cal = "SELECT SUM(calorie_burn) FROM tbl_cycling WHERE u_name = @username AND FORMAT(date_time, 'MM/dd/yyyy') = FORMAT(@dateTime, 'MM/dd/yyyy')";
                    using (OleDbCommand cmd = new OleDbCommand(cycl_cal, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user_name);
                        cmd.Parameters.AddWithValue("@dateTime", current_date);

                        object Cal = cmd.ExecuteScalar();
                        if (Cal != null && Cal != DBNull.Value)
                        {
                            int cal_burn = Convert.ToInt32(Cal);
                            cycling_cal.Text = "Cycling: " + Cal.ToString();

                            sum_cyc = cal_burn;

                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving total calorie burn: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }


                try
                {
                    con.Open();
                    string yo_cal = "SELECT SUM(calorie_burn) FROM tbl_yoga WHERE u_name = @username AND FORMAT(date_time, 'MM/dd/yyyy') = FORMAT(@dateTime, 'MM/dd/yyyy')";
                    using (OleDbCommand cmd = new OleDbCommand(yo_cal, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user_name);
                        cmd.Parameters.AddWithValue("@dateTime", current_date);

                        object Cal = cmd.ExecuteScalar();
                        if (Cal != null && Cal != DBNull.Value)
                        {
                            int cal_burn = Convert.ToInt32(Cal);
                            yoga_cal.Text = "Yoga: " + Cal.ToString();

                            sum_yoga = cal_burn;

                        }
                      
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving total calorie burn: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }

                try
                {
                    con.Open();
                    string str_cal = "SELECT SUM(calorie_burn) FROM tbl_strength WHERE u_name = @username AND FORMAT(date_time, 'MM/dd/yyyy') = FORMAT(@dateTime, 'MM/dd/yyyy')";
                    using (OleDbCommand cmd = new OleDbCommand(str_cal, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user_name);
                        cmd.Parameters.AddWithValue("@dateTime", current_date);

                        object Cal = cmd.ExecuteScalar();
                        if (Cal != null && Cal != DBNull.Value)
                        {
                            int cal_burn = Convert.ToInt32(Cal);
                            WeightLift_cal.Text = "Strength training: " + Cal.ToString();

                            sum_stren = cal_burn;

                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving total calorie burn: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }

                try
                {
                    con.Open();
                    string rj_cal = "SELECT SUM(calorie_burn) FROM tbl_ropeJump WHERE u_name = @username AND FORMAT(date_time, 'MM/dd/yyyy') = FORMAT(@dateTime, 'MM/dd/yyyy')";
                    using (OleDbCommand cmd = new OleDbCommand(rj_cal, con))
                    {
                        cmd.Parameters.AddWithValue("@username", user_name);
                        cmd.Parameters.AddWithValue("@dateTime", current_date);

                        object Cal = cmd.ExecuteScalar();
                        if (Cal != null && Cal != DBNull.Value)
                        {
                            int cal_burn = Convert.ToInt32(Cal);
                            rope_jump.Text = "Rope jumping: " + Cal.ToString();

                            sum_jump = cal_burn;

                        }
                       
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving total calorie burn: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }

                int total_cal = sum_walk + sum_swim + sum_cyc + sum_yoga + sum_stren + sum_jump;

                if (calorieGoal > 0)
                {
                    calorieBurn_per = (total_cal  / (double)calorieGoal)*100;
                }
               
                circularProgressBar1.Value = (int)calorieBurn_per;
                circularProgressBar1.Text = calorieBurn_per.ToString("0.00") + " %";

                to_cal.Text = "Total cal burn: " + total_cal.ToString();

            } catch (Exception ex)
            {
               MessageBox.Show("Something went wrong" + ex.Message);
            }


        }

        private void edit_profile_Click(object sender, EventArgs e)
        {
            string username = user_name;
            edit_profile sForm = new edit_profile(username);
            sForm.Show();
            this.Hide();
        }

        private void walking_Click(object sender, EventArgs e)
        {
            string username = user_name;
            walking sForm = new walking(username);
            sForm.Show();
            this.Hide();
        }

        private void swimming_Click(object sender, EventArgs e)
        {
            string username = user_name;
            swimming sForm = new swimming(username);
            sForm.Show();
            this.Hide();
        }

        private void cycling_Click(object sender, EventArgs e)
        {
            string username = user_name;
            cycling sForm = new cycling(username);
            sForm.Show();
            this.Hide();
        }

        private void yoga_Click(object sender, EventArgs e)
        {
            string username = user_name;
            yoga sForm = new yoga(username);
            sForm.Show();
            this.Hide();
        }

        private void strength_training_Click(object sender, EventArgs e)
        {
            string username = user_name;
            strength_training sForm = new strength_training(username);
            sForm.Show();
            this.Hide();
        }

        private void cross_fit_Click(object sender, EventArgs e)
        {
            string username = user_name;
            RopeJumping sForm = new RopeJumping(username);
            sForm.Show();
            this.Hide();
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {

            Form1 sForm  = new Form1();
            sForm.Show();
            this.Close();

        }
    }
}
