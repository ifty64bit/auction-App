using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace assaginment_task
{
    public partial class Log_In : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        public SqlConnection conn;
        public Log_In()
        {
            InitializeComponent();
            conn = new SqlConnection(cs);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Admin")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
            }
            else if (comboBox1.Text == "Seller")
            {
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
                pictureBox3.Visible = false;
            }
            else if (comboBox1.Text == "Buyer")
            {
                pictureBox3.Visible = true;
                pictureBox2.Visible = false;
                pictureBox1.Visible = false;
            }
            else
            {
                
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool status = checkBox1.Checked;
            if (status == true)
            {
                textBox2.UseSystemPasswordChar = false;

            }
            else
            {
                textBox2.UseSystemPasswordChar = true;

            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) == true)
            {
                errorProvider1.Icon = Properties.Resources.error;
                textBox1.Focus();
                errorProvider1.SetError(this.textBox1, "Enter UserName");
            }
            else
            {
                errorProvider1.Icon = Properties.Resources.check;
                //errorProvider1.Clear();
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) == true)
            {
                errorProvider2.Icon = Properties.Resources.error;
                textBox2.Focus();
                errorProvider2.SetError(this.textBox2, "Enter Password");
            }
            else
            {
                errorProvider2.Icon = Properties.Resources.check;
                //errorProvider2.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                string quary = "select userId from [User] where userName=@UserName and pass=@Password and access=@AccessLevel";
                SqlCommand cmd = new SqlCommand(quary,conn);
                cmd.Parameters.AddWithValue("@AccessLevel",comboBox1.SelectedItem);
                cmd.Parameters.AddWithValue("@UserName",textBox1.Text);
                cmd.Parameters.AddWithValue("@Password", textBox2.Text);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        dr.Read();
                        int id = (int)dr["userId"];
                        conn.Close();
                        MessageBox.Show("Login Successfull", "Log In", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (comboBox1.SelectedItem.ToString() == "Admin")
                        {
                            Admin admin = new Admin(this, textBox1.Text, id);
                            admin.Show();
                            this.Hide();
                        }
                        else if (comboBox1.SelectedItem.ToString() == "Seller")
                        {
                            Seller seller = new Seller(this, textBox1.Text, id);
                            seller.Show();
                            this.Hide();
                        }
                        else if (comboBox1.SelectedItem.ToString() == "Buyer")
                        {
                            Buyer buyer = new Buyer(this, textBox1.Text, id);
                            buyer.Show();
                            this.Hide();
                        }
                        textBox1.Clear();
                        textBox2.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Login Failed", "Log In", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Please Enter infomation", "Log In", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin admin = new Admin();
            admin.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Seller seller = new Seller();
            seller.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Buyer buyer = new Buyer();
            buyer.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Sign_Up sign_Up = new Sign_Up(this);
            sign_Up.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dash_Board dash_Board = new Dash_Board(this);
            dash_Board.Show();
        }
    }
}
