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
    public partial class Payment_Admin : Form
    {
        List<string> props = new List<string>();
        Admin admin;
        int propId;
        SqlConnection conn;
        public Payment_Admin(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            populateList();
            listBox1.DataSource = props;
            conn = admin.login.conn;
        }

        void populateList()
        {
            string query = "SELECT Prop.Title, SOLD.propId from Prop INNER Join SOLD on Prop.propId = SOLD.PropId where SOLD.Payment = 'Unpaid'";
            SqlCommand cmd = new SqlCommand(query, admin.login.conn);
            if (admin.login.conn.State == ConnectionState.Closed)
            {
                admin.login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    props.Add((string)reader["Title"]);
                }
                admin.login.conn.Close();
            }
        }

        void refresh()
        {
            listBox1.DataSource = null;
            props.Clear();
            populateList();
            listBox1.DataSource = props;
        }

        int getPropId()
        {
            int propID;
            SqlCommand cmd = new SqlCommand("Select propId from Prop where Title=@title", admin.login.conn);
            cmd.Parameters.AddWithValue("@title", listBox1.SelectedItem.ToString());
            if (admin.login.conn.State == ConnectionState.Closed)
            {
                admin.login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                propID = (int)reader["propId"];
                admin.login.conn.Close();
                return propID;
            }
            return -1;
        }

        int getSellerId()
        {
            int sellerId;
            SqlCommand cmd = new SqlCommand("Select [User].userName from (([User] inner join SOLD on SOLD.userId =[User].userId) inner join Prop on Prop.propId = SOLD.propId) where Prop.Title = @title", admin.login.conn);
            cmd.Parameters.AddWithValue("@title", listBox1.SelectedItem.ToString());
            if (admin.login.conn.State == ConnectionState.Closed)
            {
                admin.login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                sellerId = (int)reader["userId"];
                admin.login.conn.Close();
                return sellerId;
            }
            return -1;
        }

        string getSellerName()
        {
            string buyer;
            SqlCommand cmd = new SqlCommand("Select userName from [User] where userId=(select userId from Prop where Title=@Title)", admin.login.conn);
            cmd.Parameters.AddWithValue("@Title", listBox1.SelectedItem.ToString());
            if (admin.login.conn.State == ConnectionState.Closed)
            {
                admin.login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                buyer = (string)reader["userName"];
                admin.login.conn.Close();
                return buyer;
            }
            return null;
        }

        int getBuyerId()
        {
            int sellerId;
            SqlCommand cmd = new SqlCommand("select userID from SOLD where propId = @propId", admin.login.conn);
            cmd.Parameters.AddWithValue("@propId", getPropId());
            if (conn.State == ConnectionState.Closed)
            {
                admin.login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                sellerId = (int)reader["userId"];
                admin.login.conn.Close();
                return sellerId;
            }
            return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            admin.Show();
            admin.getProfit();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void ListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                label10.Text = "";
                label14.Text = "";
                label16.Text = "";
                label7.Text = "";
                label9.Text = "";
                label13.Text = "";
                return;
            }
            string title =listBox1.SelectedItem.ToString();
            propId = getPropId();
            string sellerName=getSellerName();
            string query = "Select [User].userName as Buyer, Prop.location, prop.Dec, Prop.img, SOLD.Ammount, SOLD.propId from (([User] inner join SOLD on SOLD.userId =[User].userId) inner join Prop on Prop.propId = SOLD.propId) where Prop.propId = @propId";
            SqlCommand cmd = new SqlCommand(query, admin.login.conn);
            cmd.Parameters.AddWithValue("@propId", getPropId());
            if (admin.login.conn.State == ConnectionState.Closed)
            {
                admin.login.conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    label14.Text = (string)reader["Buyer"];
                    label9.Text = (string)reader["location"];
                    label13.Text = (string)reader["Dec"];
                    byte[] img = (byte[])reader["img"];
                    MemoryStream ms = new MemoryStream(img);
                    pictureBox1.Image = new Bitmap(ms);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    label16.Text = Convert.ToString((int)reader["Ammount"]);
                    propId = (int)reader["propId"];
                }
                admin.login.conn.Close();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            int ammount = Convert.ToInt32(label16.Text);
            int profit = Convert.ToInt32(Math.Floor(ammount * 0.5));
            int sellerAmmount = ammount - profit;
            string query = "Insert Into Inc(propId,Ammount) Values(@propId,@profit); Update SOLD SET Payment = 'Paid' where propId = @propId; Update [User] SET Balance = @userProfit where userId = (SELECT userId from SOLD where propId=@propId); ";
            SqlCommand cmd = new SqlCommand(query, admin.login.conn);
            cmd.Parameters.AddWithValue("@propId", propId);
            cmd.Parameters.AddWithValue("@profit", profit);
            cmd.Parameters.AddWithValue("@userProfit", sellerAmmount);
            if (admin.login.conn.State == ConnectionState.Closed)
            {
                admin.login.conn.Open();
                int count = cmd.ExecuteNonQuery();
                admin.login.conn.Close();
                if (count > 0)
                {
                    MessageBox.Show("Successfull");
                    refresh();
                }
                else
                {
                    MessageBox.Show("Some Error Happend");
                }
            }  
        }
    }
}
