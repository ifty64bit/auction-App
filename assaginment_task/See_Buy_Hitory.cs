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
    public partial class See_Buy_Hitory : Form
    {
        Seller seller;
        SqlDataAdapter adapt;
        DataTable dt;
        public See_Buy_Hitory(Seller seller)
        {
            InitializeComponent();
            this.seller = seller;
            string query = "Select Prop.Title, Prop.Dec,Prop.img, SOLD.Ammount, [User].userName from((Prop Inner Join SOLD on SOLD.propId = Prop.propId) Inner Join[User] on[User].userId = SOLD.userId) where Prop.userId = @userId and status = @status";
            dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query,seller.login.conn);
            seller.login.conn.Open();
            cmd.Parameters.AddWithValue("@userId", seller.userId);
            cmd.Parameters.AddWithValue("@status", "Inactive");
            adapt = new SqlDataAdapter(cmd);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            seller.login.conn.Close();
            label2.Text = seller.userName;
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
