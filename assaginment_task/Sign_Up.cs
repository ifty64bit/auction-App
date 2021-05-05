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
using System.IO;

namespace assaginment_task
{
    public partial class Sign_Up : Form
    {
        Log_In login;
        string imgName;
        public Sign_Up(Log_In login)
        {
            InitializeComponent();
            this.login = login;
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.Icon = Properties.Resources.error;
                errorProvider1.SetError(this.textBox1, "Enter your name please!!");
            }
            else
            {
                errorProvider1.Icon = Properties.Resources.check;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Focus();
                errorProvider2.Icon = Properties.Resources.error;
                errorProvider2.SetError(this.textBox2, "Enter your Password please!!");
            }
            else
            {
                errorProvider2.Icon = Properties.Resources.check;
            }
        }

        private void TextBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                textBox3.Focus();
                errorProvider3.Icon = Properties.Resources.error;
                errorProvider3.SetError(this.textBox3, "Re-Enter your Password please!!");
            }
            else if (textBox2.Text != textBox3.Text)
            {
                errorProvider3.Icon = Properties.Resources.error;
                errorProvider3.SetError(this.textBox3, "Password Did not Matched");
            }
            else
            {
                errorProvider3.Icon = Properties.Resources.check;
            }

        }

        private void TextBox4_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                textBox4.Focus();
                errorProvider4.Icon = Properties.Resources.error;
                errorProvider4.SetError(this.textBox4, "Enter your valid email please!!");
            }
            else
            {
                errorProvider4.Icon = Properties.Resources.check;
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool status = checkBox1.Checked;
            switch (status)
            {
                case true:
                    textBox2.UseSystemPasswordChar = false;
                    break;
                default:
                    textBox2.UseSystemPasswordChar = true;
                    break; 
            }

        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            bool status = checkBox2.Checked;
            switch (status)
            {
                case true:
                    textBox3.UseSystemPasswordChar = false;
                    break;
                default:
                    textBox3.UseSystemPasswordChar = true;
                    break;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && !string.IsNullOrEmpty(imgName))
            {
                int a;
                
                FileStream stream = new FileStream(imgName, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
                stream.Close();
                string query = "insert into [User](userName,pass,email,access,Mobile,picture) values(@u,@p,@e,@a,@M,@Phone)";
                SqlCommand cmd = new SqlCommand(query, login.conn);
                cmd.Parameters.AddWithValue("@u", textBox1.Text);
                cmd.Parameters.AddWithValue("@p", textBox2.Text);
                cmd.Parameters.AddWithValue("@a", comboBox1.SelectedItem);
                cmd.Parameters.AddWithValue("@e", textBox4.Text);
                cmd.Parameters.AddWithValue("@M", Convert.ToInt32(textBox5.Text));
                cmd.Parameters.AddWithValue("@Phone", bytes);
                if (login.conn.State == ConnectionState.Closed)
                {
                    login.conn.Open();
                    a = cmd.ExecuteNonQuery();
                    login.conn.Close();
                }
                else
                {
                    a = -1;
                }
                if (a > 0)
                {
                    MessageBox.Show("Successfully Registered. Welcome to SINK. Please click BACK and login your Account", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Sorry not added", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            else 
            {
                MessageBox.Show("Please Enter infomation", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
            login.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
          //  Dash_Board dash_Board = new Dash_Board(this);
           // dash_Board.Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Title = "Choce youre picture";
            OFD.Filter = "Image File(*.png;*.jpg) |*.png;*.jpg";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = new Bitmap(OFD.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                imgName = OFD.FileName.ToString();
            }
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
    }
}

