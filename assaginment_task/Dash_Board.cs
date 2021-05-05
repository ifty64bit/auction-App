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
    public partial class Dash_Board : Form
    {
        Log_In login;
        int i;
        List<byte[]> images = new List<byte[]>();
        List<string> title = new List<string>();
        List<string> userNames = new List<string>();
        List<string> locations = new List<string>();
        List<int> soldPrice = new List<int>();
        public Dash_Board(Log_In login)
        {
            InitializeComponent();
            this.login = login;
            timer1.Start();
            getImage();
            setData();
            i = 0;
        }

        void getImage()
        {
            string querry = "Select Prop.img, Prop.Title, Prop.location, [User].userName, SOLD.Ammount from((Prop Inner Join[User] On Prop.userId =[User].userId) Inner Join SOLD On Prop.propId = SOLD.propId) where status = @status";
            SqlCommand cmd = new SqlCommand(querry, login.conn);
            cmd.Parameters.AddWithValue("@status", "Inactive");
            if (login.conn.State == ConnectionState.Closed)
            {
                login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    images.Add((byte[])reader["img"]);
                    title.Add((string)reader["Title"]);
                    locations.Add((string)reader["location"]);
                    userNames.Add((string)reader["userName"]);
                    soldPrice.Add((int)reader["Ammount"]);
                }
                login.conn.Close();
            }
        }

        void setData(int i=0)
        {
            MemoryStream ms = new MemoryStream(images[i]);
            pictureBox2.Image = new Bitmap(ms);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            label5.Text = title[i];
            label6.Text = locations[i];
            label7.Text = Convert.ToString(soldPrice[i]);
            label9.Text = userNames[i];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Log_In log_In = new Log_In();
            log_In.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (i + 1 >= images.Count)
            {
                i = 0;
            }
            else
            {
                i++;
            }
            setData(i);
        }

        private void Timer1_Tick_1(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
            label2.Text = DateTime.Now.ToLongDateString();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (i - 1 == -1)
            {
                i = images.Count-1;
            }
            else
            {
                i--;
            }
            setData(i);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            US uS = new US();
            uS.Show();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("For Any Querry Contact: +8801309077861");
        }
    }
}
