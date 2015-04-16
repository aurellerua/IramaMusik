using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Irama
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("test") == true && textBox2.Text.Equals("test") == true )
            {
                //MessageBox.Show("<logged in>");
                this.Hide();
                Data dataform = new Data();
                dataform.Show();
            }
            else
            {
                MessageBox.Show("wrong input");
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
