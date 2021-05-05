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
    public partial class Buyer : Form
    {
        public Log_In login;
        public string userName;
        public int id;
        public MemoryStream ms;
        List<string> values = new List<string>();
        public Buyer()
        {
            InitializeComponent();
        }
        public Buyer(Log_In login, string userName, int id)     //Constractor
        {
            InitializeComponent();
            this.login = login;
            this.userName = userName;
            label2.Text = userName;
            this.id = id;
            loadData();
            listBox1.DataSource = values;
            loadPhoto();
        }
        void loadData()
        {
            string query = "select * from Prop where status=@active";
            SqlCommand cmd = new SqlCommand(query,login.conn);
            cmd.Parameters.AddWithValue("@active", "Active");
            if(login.conn.State == ConnectionState.Closed)
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

        void loadPhoto()
        {
            string query = "Select picture from [User] where userName=@userName and access=@access";
            SqlCommand cmd = new SqlCommand(query, login.conn);
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@access", "Buyer");
            if (login.conn.State == ConnectionState.Closed)
            {
                login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                try
                {
                    byte[] bytes = (byte[])reader["picture"];
                    ms = new MemoryStream(bytes);
                    pictureBox3.Image = new Bitmap(ms);
                    pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception e)
                {
                    pictureBox3.Image = Properties.Resources.pngwing_com__1_;
                    pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                
                login.conn.Close();
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
        void getBidder()
        {
            string querry = "Select Top 1 [User].userName, Bids.Ammount from [User] INNER JOIN Bids ON [User].userId = Bids.userId where [User].userId = (SELECT userId from Bids where Ammount = (SELECT MAX(Ammount) from Bids)) AND propId=(SELECT propId From Prop Where Title=@title) ORDER BY Ammount DESC";
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
                        label15.Text = (string)reader["userName"];
                        label16.Text = Convert.ToString((int)reader["Ammount"]);
                    }
                }
                else
                {
                    label16.Text = "You are first";
                    label15.Text = "";
                }
                login.conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ammout;
            int bid = textBox1.Text == "" ? 0 : Convert.ToInt32(textBox1.Text);
            if (!int.TryParse(textBox1.Text,out ammout))
            {
                MessageBox.Show("Only Number Are Allowed");
            }
            else if ((ammout <= bid) && (ammout<= Convert.ToInt32(label11.Text)))
            {
                MessageBox.Show("Value Must be Greater Then MAX Bidder or Starting Price");
            }
            else
            {
                string query = "INSERT INTO Bids(userId,propId,Ammount) VALUES(@userId,@propId,@ammount)";
                int propId=getPropId();
                SqlCommand cmd = new SqlCommand(query,login.conn);
                cmd.Parameters.AddWithValue("@userId", id);
                cmd.Parameters.AddWithValue("@propId", propId);
                cmd.Parameters.AddWithValue("@ammount", Convert.ToInt32(ammout));
                if (login.conn.State == ConnectionState.Closed)
                {
                    login.conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        MessageBox.Show("Bid Placed Successfuly");
                        textBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Some Error Happend");
                    }
                    login.conn.Close();
                    getBidder();
                }
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Add_Fund fund = new Add_Fund(this);
            fund.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
            login.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            values.Clear();
            loadData();
        }

        private void ListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(listBox1.SelectedItem);
            if (listBox1.SelectedValue != null)
            {
                string value = listBox1.SelectedItem.ToString();
                label7.Text = value;
                string query = "Select * from Prop where Title=@title";
                SqlCommand cmd = new SqlCommand(query, login.conn);
                cmd.Parameters.AddWithValue("@title", value);
                if (login.conn.State == ConnectionState.Closed)
                {
                    login.conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        byte[] img = (byte[])reader["img"];
                        MemoryStream ms = new MemoryStream(img);
                        pictureBox1.Image = new Bitmap(ms);
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        label11.Text = Convert.ToString((int)reader["startPrice"]);
                        label9.Text = (string)reader["location"];
                        label13.Text = (string)reader["Dec"];
                    }
                    login.conn.Close();
                    getBidder();
                }
                
                
            }
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            buyer_info buyerInfo = new buyer_info(this);
            buyerInfo.Show();
        }

    }
}
