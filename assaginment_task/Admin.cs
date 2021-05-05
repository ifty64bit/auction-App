using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assaginment_task
{
    public partial class Admin : Form
    {
        public string userName;
        public Log_In login;
        public MemoryStream ms;
        public int id;
        public Admin()
        {
            InitializeComponent();
        }

        public Admin(Log_In login, string userName, int id) //Constractor
        {
            InitializeComponent();
            this.login = login;
            this.userName = userName;
            label2.Text = userName;
            this.id = id;
            getProfit();
            loadPhoto();
        }

        //Methods
        public void getProfit()
        {
            if (login.conn.State == ConnectionState.Closed)
            {
                string querry = "SELECT SUM(Ammount) as Ammount from Inc";
                SqlCommand cmd = new SqlCommand(querry, login.conn);
                login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    label6.Text = Convert.ToString((int)reader["Ammount"]);
                }
                login.conn.Close();
            }
        }

        void loadPhoto()
        {
            string query = "Select picture from [User] where userName=@userName";
            SqlCommand cmd = new SqlCommand(query, login.conn);
            cmd.Parameters.AddWithValue("@userName", userName);
            if (login.conn.State == ConnectionState.Closed)
            {
                login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                try
                {
                    byte[] bytes = (byte[])reader["picture"];
                    ms = new MemoryStream(bytes);

                }
                catch (Exception e)
                {
                    pictureBox3.Image = Properties.Resources.pngwing_com__1_;
                    pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                login.conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Bid bid = new Bid();
            bid.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Information info = new Information(this);
            info.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Payment_Admin payment_Admin = new Payment_Admin(this);
            payment_Admin.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //Add_Property add_Property = new Add_Property();
            //add_Property.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log_In log_In = new Log_In();
            log_In.Show();
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {

        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button8_MouseHover(object sender, EventArgs e)
        {

        }

        private void panel4_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button6_MouseHover(object sender, EventArgs e)
        {
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button10_MouseHover(object sender, EventArgs e)
        {
        }

        private void button10_DragLeave(object sender, EventArgs e)
        {
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Admin_Sold_History his = new Admin_Sold_History(this);
            his.Show();
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            Admin_sett sett = new Admin_sett(this);
            sett.Show();
        }
    }
}
