using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assaginment_task
{
    public partial class Information : Form
    {
        Admin admin;
        DataTable dt;
        SqlDataAdapter adapt;
        public Information(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            loadData();
            label2.Text = admin.userName;
        }
        void loadData()
        {
            string query = "Select userName, Balance,email,Mobile from [User] where access=@buyer OR access=@seller";
            dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, admin.login.conn);
            admin.login.conn.Open();
            cmd.Parameters.AddWithValue("@buyer", "Buyer");
            cmd.Parameters.AddWithValue("@seller", "Seller");
            adapt = new SqlDataAdapter(cmd);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            admin.login.conn.Close();
        }
    }
}
