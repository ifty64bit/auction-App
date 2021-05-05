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
    public partial class Add_Property : Form
    {
        public string userName;
        Seller seller;
        string imgName;
        public Log_In login;
        public int userId;

        public Add_Property(Seller seller)
        {
            InitializeComponent();
            this.seller = seller;
            this.userName = seller.userName;
            label1.Text = userName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            seller.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select image to be upload.";
            openFileDialog1.Filter = "Image Only(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (openFileDialog1.CheckFileExists)
                {
                    imgName = openFileDialog1.FileName.ToString();
                    pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            else
            {
                MessageBox.Show("Please Upload image.");
            }
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            if ( String.IsNullOrEmpty(textBox1.Text) && String.IsNullOrEmpty(textBox4.Text) && String.IsNullOrEmpty(textBox3.Text) && String.IsNullOrEmpty(textBox2.Text) && String.IsNullOrEmpty(imgName))
            {
                MessageBox.Show("Insert All Fields");
            }
            else
            {
                FileStream stream = new FileStream(imgName, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
                stream.Close();
                string query = "INSERT INTO Prop(Title, Dec, userId, img, location, startPrice) VALUES(@title,@dec,@userId,@img,@location,@startprice)";
                seller.login.conn.Open();
                SqlCommand cmd = new SqlCommand(query, seller.login.conn);
                cmd.Parameters.AddWithValue("@title", textBox1.Text);
                cmd.Parameters.AddWithValue("@dec", textBox2.Text);
                cmd.Parameters.AddWithValue("@userId", seller.userId);
                cmd.Parameters.AddWithValue("@img", bytes);
                cmd.Parameters.AddWithValue("@location", textBox4.Text);
                cmd.Parameters.AddWithValue("@startprice", textBox3.Text);
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Property Added Successfully");
                    seller.login.conn.Close();
                    this.Close();
                    seller.Show();
                }
                else
                {
                    MessageBox.Show("Error Happend");
                    seller.login.conn.Close();
                }
            }
            seller.refresh();
        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }
    }
}
