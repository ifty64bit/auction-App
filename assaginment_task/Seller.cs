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
using System.Configuration;

namespace assaginment_task
{
    public partial class Seller : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        public SqlConnection conn;


        public string userName;
        public Log_In login;
        public int userId;
        List<string> values = new List<string>();
        string imgName;
        string location;
        string description;
        int maxBidderId;
        public Seller()
        {
            InitializeComponent();
        }
        public Seller(Log_In login,string userName, int id)
        {
            InitializeComponent();
            this.login = login;
            this.userName = userName;
            label2.Text = userName;
            this.userId = id;
            loadList();
            listBox1.DataSource = values;
            getBidder();
            loadPhoto();
            loadData();
        }

        void loadList()
        {
            string query = "select * from Prop where status=@active AND userId=@userId";
            SqlCommand cmd = new SqlCommand(query,login.conn);
            cmd.Parameters.AddWithValue("@active", "Active");
            cmd.Parameters.AddWithValue("@userId", userId);
            if (login.conn.State == ConnectionState.Closed)
            {
               login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    values.Add((string)reader["Title"]);
                }
              login.conn.Close();
            }
            
        }

        void loadData()
        {
            string query = "SELECT picture,Balance From [user] where userName=@UserName And access=@access";
            conn = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@access", "Seller");
            if (login.conn.State == ConnectionState.Closed)
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //textBox1.Text = dr.GetValue(1).ToString();
                        //label2.Text = dr.GetValue(1).ToString();
                        label14.Text =Convert.ToString((int)dr["Balance"]);
                        //MessageBox.Show((string)dr["Balance"]);
                        try
                        {
                            Byte[] pic;
                            pic = (byte[])dr["picture"];
                            MemoryStream ms = new MemoryStream(pic);
                            pictureBox3.Image = Image.FromStream(ms);
                            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        catch (Exception e)
                        {
                            pictureBox3.Image = Properties.Resources.pngwing_com__1_;
                            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                }
                conn.Close();
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
                    MemoryStream ms = new MemoryStream(bytes);
                    
                }
                catch(Exception e)
                {
                    pictureBox3.Image = Properties.Resources.pngwing_com__1_;
                    pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                login.conn.Close();
            }
        }

        void getBidder()
        {
            if (listBox1.SelectedItem != null)
            {
                string querry = "Select Top 1 [User].userName, [User].userId, Bids.Ammount from [User] INNER JOIN Bids ON [User].userId = Bids.userId where [User].userId = (SELECT userId from Bids where Ammount = (SELECT MAX(Ammount) from Bids)) AND propId=(SELECT propId From Prop Where Title=@title) ORDER BY Ammount DESC";
                SqlCommand cmd = new SqlCommand(querry, login.conn);
                cmd.Parameters.AddWithValue("@title", listBox1.SelectedItem.ToString());
                if (login.conn.State == ConnectionState.Closed)
                {
                    login.conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            label7.Text = (string)reader["userName"];
                            label9.Text = Convert.ToString((int)reader["Ammount"]);
                            maxBidderId = (int)reader["userId"];
                        }
                    }
                    else
                    {
                        label7.Text = "No One Bidded Yet";
                        label9.Text = "";
                    }
                    login.conn.Close();
                }
            }
            
        }

        int getPropId()
        {
            SqlCommand cmd = new SqlCommand("Select propId from Prop where Title=@title", login.conn);
            cmd.Parameters.AddWithValue("@title", listBox1.SelectedItem.ToString());
            if (login.conn.State == ConnectionState.Closed)
            {
                login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                int propId = (int)reader["propId"];
                login.conn.Close();
                return propId;
            }
            return -1;
        }

        public void refresh()
        {
            listBox1.DataSource = null;
            values.Clear();
            loadList();
            listBox1.DataSource = values;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Add_Property add_Property = new Add_Property(this);
            add_Property.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            this.Close();
            login.Show();
        }

        private void ListBox1_SelectedValueChanged(object sender, EventArgs e)      //Listbox Value Change Event
        {
            if (listBox1.SelectedValue != null)
            {
                string value = listBox1.SelectedItem.ToString();
                textBox2.Text = value;
                string query = "Select * from Prop where Title=@title";
                if (login.conn.State == ConnectionState.Closed)
                {
                    login.conn.Open();
                    SqlCommand cmd = new SqlCommand(query, login.conn);
                    cmd.Parameters.AddWithValue("@title", value);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        byte[] img = (byte[])reader["img"];
                        MemoryStream ms = new MemoryStream(img);
                        pictureBox1.Image = new Bitmap(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        label11.Text = Convert.ToString((int)reader["startPrice"]);
                        textBox3.Text = (string)reader["location"];
                        location = textBox3.Text;
                        textBox4.Text = (string)reader["Dec"];
                        description = textBox4.Text;
                    }
                    login.conn.Close();
                }
                getBidder();

            }
            else
            {
                pictureBox1.Image = null;
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                label7.Text = "";
                label9.Text = "";
                label11.Text = "";
            }

        }

        private void Button3_Click(object sender, EventArgs e)  //Image Select BTN
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
        }

        private void Button2_Click(object sender, EventArgs e)  //Update
        {
            if ((string.IsNullOrEmpty(imgName) && (textBox2.Text.Equals(listBox1.SelectedItem.ToString()))) && (textBox3.Text.Equals(location)) && (textBox4.Text.Equals(description)))
            {
                MessageBox.Show("Nothe To Change");
            }
            else if(string.IsNullOrEmpty(imgName))
            {
                string query = "UPDATE Prop SET Title = @title, Dec = @dec, location=@location WHERE propId = @propId";
                SqlCommand cmd = new SqlCommand(query, login.conn);
                cmd.Parameters.AddWithValue("@title", textBox2.Text);
                cmd.Parameters.AddWithValue("@dec", textBox4.Text);
                cmd.Parameters.AddWithValue("@location", textBox3.Text);
                cmd.Parameters.AddWithValue("@propId", getPropId());
                if (login.conn.State == ConnectionState.Closed)
                {
                    login.conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        MessageBox.Show("Updated");
                        login.conn.Close();
                        refresh();
                    }
                    else
                    {
                        MessageBox.Show("Some Error Happend");
                        login.conn.Close();
                    }
                }
                
            }
            else
            {
                FileStream stream = new FileStream(imgName, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
                stream.Close();
                string query = "UPDATE Prop SET Title = @title, Dec = @dec, img=@img, location=@location WHERE propId = @propId";
                SqlCommand cmd = new SqlCommand(query, login.conn);
                cmd.Parameters.AddWithValue("@title", textBox2.Text);
                cmd.Parameters.AddWithValue("@dec", textBox4.Text);
                cmd.Parameters.AddWithValue("@img", bytes);
                cmd.Parameters.AddWithValue("@location", textBox3.Text);
                cmd.Parameters.AddWithValue("@propId", getPropId());
                if (login.conn.State == ConnectionState.Closed)
                {
                    login.conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        MessageBox.Show("Updated");
                        values.Clear();
                        loadList();
                    }
                    else
                    {
                        MessageBox.Show("Some Error Happend");
                    }
                    login.conn.Close();
                }
            }
        }

        private void Button5_Click(object sender, EventArgs e)  //Delete
        {
            string query = "Delete from Prop where propId=@propId";
            SqlCommand cmd = new SqlCommand(query, login.conn);
            cmd.Parameters.AddWithValue("@propId", getPropId());
            if (login.conn.State == ConnectionState.Closed)
            {
                login.conn.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Deleted");
                    login.conn.Close();
                    values.Clear();
                    loadList();
                }
                else
                {
                    MessageBox.Show("Some Error Happend");
                    login.conn.Close();
                }
            }
            refresh();
        }

        private void Button6_Click(object sender, EventArgs e) //Sold
        {
            string query = "Insert Into SOLD(propId, userId, Ammount) Values(@propId,@userId,@Ammount); Update Prop Set status=@status where propId=@propId;";
            SqlCommand cmd = new SqlCommand(query, login.conn);
            cmd.Parameters.AddWithValue("@propId", getPropId());
            cmd.Parameters.AddWithValue("@userId", maxBidderId);
            cmd.Parameters.AddWithValue("@status", "Inactive");
            cmd.Parameters.AddWithValue("@Ammount", Convert.ToInt32(label9.Text));
            if (login.conn.State == ConnectionState.Closed)
            {
                login.conn.Open();
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    MessageBox.Show("Sold");
                    refresh();
                }
                else
                {
                    MessageBox.Show("Some Error Happend");
                }
                login.conn.Close();
            }
            
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            See_Buy_Hitory history = new See_Buy_Hitory(this);
            history.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Seller_info seller_Buyer_Info = new Seller_info(login,userName,userId);
            seller_Buyer_Info.Show();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
    }
}
