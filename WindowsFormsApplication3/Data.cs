using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Irama
{
    public partial class Data : Form
    {
        public Data()
        {
            InitializeComponent();
            CenterToParent();
        }

        DataGridView dg1 = new DataGridView();
        DataGridView dg2 = new DataGridView();

        private void Data_Load(object sender, EventArgs e)
        {
            tabPage1.Text = "Siswa";
            tabPage2.Text = "Pembayaran";

            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=;database=iramamusik;";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();

                // load data and bind it to gridview
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select id,name as Nama,no_telp as 'no. telp',alamat,jenis_kursus as 'jenis kursus', grade from siswa";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                dg1.Location = new Point(13, 13);
                dg1.DataSource = ds.Tables[0].DefaultView;

                // size control
                    int height = tabPage1.Height-100;
                    foreach (DataGridViewRow row in dg1.Rows) {
                        height += row.Height;
                    }
                    height += dg1.ColumnHeadersHeight;

                    int width = tabPage1.Width-50;
                    foreach (DataGridViewColumn col in dg1.Columns)
                    {
                        width += col.Width;
                    }
                    width += dg1.RowHeadersWidth;
                
                    tabPage1.Controls.Add(dg1); // add datagridview to tabPage1 aka tab Siswa
                    
                    dg1.Columns[0].Width = 30;
                    dg1.ClientSize = new Size(width-17, height + 2);

                // ----------------------------------------
                // ----------- TAB PEMBAYARAN -------------
                // ----------------------------------------

                    cmd.CommandText = "select parent_id, name as Nama, lastpaid_bulanan as 'bulan terakhir', lastpaid_booklevel as 'buku terakhir' from pembayaran, siswa where siswa.id = pembayaran.parent_id";
                MySqlDataAdapter adap1 = new MySqlDataAdapter(cmd);
                DataSet ds1 = new DataSet();
                adap.Fill(ds1);
                DataGridView dg2 = new DataGridView();
                dg2.Location = new Point(13, 13);
                dg2.DataSource = ds1.Tables[0].DefaultView;

                // size control
                foreach (DataGridViewRow row in dg2.Rows)
                {
                    height += row.Height;
                }
                height += dg2.ColumnHeadersHeight;

                foreach (DataGridViewColumn col in dg2.Columns)
                {
                    width += col.Width;
                }
                width += dg2.RowHeadersWidth;

                tabPage2.Controls.Add(dg2); // add datagridview to tabPage2
                dg2.ClientSize = new Size(width - 58, height - 21);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Data_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }

        private void Data_FormClosed(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=;database=iramamusik;";

            string criteria = textBox2.Text;

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select id,name as Nama,no_telp as 'no. telp',alamat,jenis_kursus as 'jenis kursus', grade from siswa where name like '%" + criteria + "%'";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                dg1.DataSource = ds.Tables[0].DefaultView;
                tabPage1.Controls.Add(dg1);

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSiswaBaru fSiswa = new FormSiswaBaru();
            fSiswa.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormPembayaran fPemb = new FormPembayaran();
            fPemb.Show();
        }

        
    }
}
