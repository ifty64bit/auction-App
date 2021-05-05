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
    public partial class Admin_sett : Form
    {
        public SqlConnection conn;
        Admin admin;
        public Admin_sett(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            string query = "SELECT * From [user] where userName=@UserName And access=@access";
            SqlCommand cmd = new SqlCommand(query, admin.login.conn);
            cmd.Parameters.AddWithValue("@UserName", admin.userName);
            cmd.Parameters.AddWithValue("@access", "Admin");
            admin.login.conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    textBox1.Text = dr.GetValue(1).ToString();
                    textBox2.Text = dr.GetValue(3).ToString();
                    textBox3.Text = dr.GetValue(6).ToString();
                    textBox4.Text = dr.GetValue(2).ToString();
                    label9.Text = dr.GetValue(1).ToString();

                    try
                    {
                        Byte[] pic;
                        pic = (byte[])dr.GetValue(7);
                        MemoryStream ms = new MemoryStream(pic);
                        pictureBox1.Image = Image.FromStream(ms);
                        pictureBox3.Image = Image.FromStream(ms);
                    }
                    catch (Exception e)
                    {
                        pictureBox1.Image = Properties.Resources.pngwing_com__1_;
                        pictureBox3.Image = Properties.Resources.pngwing_com__1_;
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            admin.login.conn.Close();

        }
        private byte[] SavedPhoto(Image picture)
        {
            System.IO.MemoryStream ms = new MemoryStream();
            picture.Save(ms, picture.RawFormat);
            return ms.GetBuffer();
        }
        private void button12_Click(object sender, EventArgs e)
        {

            string quary = "update [user] set userName=@UserName,pass=@Password,email=@email,Mobile=@mobile,picture=@img where userName=@UserName";
            SqlCommand cmd = new SqlCommand(quary, admin.login.conn);
            cmd.Parameters.AddWithValue("@UserName", textBox1.Text);
            cmd.Parameters.AddWithValue("@Password", textBox2.Text);
            cmd.Parameters.AddWithValue("@mobile", textBox3.Text);
            cmd.Parameters.AddWithValue("@email", textBox4.Text);
            cmd.Parameters.AddWithValue("@img", SavedPhoto(pictureBox1.Image));
            admin.login.conn.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Data Updated Successfully ! ");

            }
            else
            {
                MessageBox.Show("Data not Updated ! ");
            }
            admin.login.conn.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Choce youre picture";
            OFD.Filter = "Image File(*.png;*.jpg) |*.png;*.jpg";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(OFD.FileName);

            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }    
}
