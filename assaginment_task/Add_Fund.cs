using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assaginment_task
{
    public partial class Add_Fund : Form
    {
        Buyer buyer;
        public Add_Fund(Buyer buyer)
        {
            InitializeComponent();
            this.buyer = buyer;
            label2.Text = buyer.userName;
            try
            {
                pictureBox3.Image = new Bitmap(buyer.ms);
            }
            catch (Exception e)
            {
                pictureBox3.Image = Properties.Resources.pngwing_com__1_;
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Enter All Field");
            }
            else
            {
                string querry = "Update [User] Set Balance = @balance where userName=@userName and access=@access";
                SqlCommand cmd = new SqlCommand(querry, buyer.login.conn);
                cmd.Parameters.AddWithValue("@balance", Convert.ToInt32(textBox1.Text));
                cmd.Parameters.AddWithValue("@userName", buyer.userName);
                cmd.Parameters.AddWithValue("@access", "Buyer");
                if (buyer.login.conn.State == ConnectionState.Closed)
                {
                    buyer.login.conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        MessageBox.Show("Money Added Successfuly");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Some Error Happend");
                    }
                    buyer.login.conn.Close();
                }
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
