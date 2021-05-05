using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace assaginment_task
{
    public partial class Bid : Form
    {
        Buyer buyer;
        string userName;
        public Bid()
        {
            InitializeComponent();
        }
        public Bid(Buyer buyer,string userName)
        {
            InitializeComponent();
            this.userName = userName;
            label1.Text = userName;
            this.buyer = buyer;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            buyer.Show();
        }

       
    }
}
