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

        DataGridView dg1 = new DataGridView(); // siswa
        DataGridView dg2 = new DataGridView(); // pembayaran
        MySqlDataAdapter adap;
        DataSet ds;
        MySql.Data.MySqlClient.MySqlConnection conn;

        private void Data_Load(object sender, EventArgs e)
        {
            tabPage1.Text = "Siswa";
            tabPage2.Text = "Pembayaran";

            
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=;database=irama;Convert Zero Datetime=True";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();

                // load data and bind it to gridview
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select id,name as Nama,no_telp as 'no. telp',alamat,jenis_kursus as 'jenis kursus' from siswa";
                adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
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

                    cmd.CommandText = "select parent_id, tanggal, nama, idsiswa, lastpaid_bulanan as 'bulan terakhir', lastpaid_booklevel as 'buku terakhir' from pembayaran, siswa where siswa.id = pembayaran.parent_id";
                adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);
                dg2 = new DataGridView();
                dg2.Location = new Point(13, 13);
                dg2.DataSource = ds.Tables[0].DefaultView;

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

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            dg1.BeginEdit(false);
            this.Validate();
        }

        private void Data_FormClosed(object sender, FormClosingEventArgs e)
        {
            
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e) // search siswa
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=;database=irama;";

            string criteria = textBox2.Text;

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select id,name as Nama,no_telp as 'no. telp',alamat,jenis_kursus as 'jenis kursus' from siswa where name like '%" + criteria + "%'";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                dg1.DataSource = ds.Tables[0].DefaultView;
                tabPage1.Controls.Add(dg1);
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e) // update button (siswa)
        {
            int columnidx = dg1.CurrentCell.ColumnIndex;
            int rowidx = dg1.CurrentCell.RowIndex;
            string selectedCellVal = dg1[columnidx, rowidx].Value.ToString();
            string getIdVal = dg1[0,rowidx].Value.ToString();

            MySqlCommand cmd = conn.CreateCommand();
            string column = "";
            switch (columnidx)
            {
                case 1:
                    column = "name";
                    break;
                case 2:
                    column="no_telp";
                    break;
                case 3:
                    column="alamat";
                    break;
                case 4:
                    column="jenis_kursus";
                    break;
                case 5:
                    column = "grade";
                    break;
            }

            cmd.CommandText = "update siswa set " + column + "='" + dg1.SelectedCells[0].Value + "' where id=" + getIdVal;
            adap = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            adap.Fill(ds);
            MessageBox.Show("Update saved", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button6_Click(object sender, EventArgs e) // update pembayaran
        {
            int columnidx = dg2.CurrentCell.ColumnIndex;
            int rowidx = dg2.CurrentCell.RowIndex;
            string selectedCellVal = dg2[columnidx, rowidx].Value.ToString();
            string getIdVal = dg2[0, rowidx].Value.ToString();

            MySqlCommand cmd = conn.CreateCommand();
            string column = "";
            switch (columnidx)
            {
                case 2:
                    column = "nama";
                    break;
                case 3:
                    column = "idsiswa";
                    break;
                case 4:
                    column = "lastpaid_bulanan";
                    break;
                case 5:
                    column = "lastpaid_booklevel";
                    break;
            }

            cmd.CommandText = "update pembayaran set " + column + "='" + dg2.SelectedCells[0].Value + "' where parent_id=" + getIdVal;
            adap = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            adap.Fill(ds);
            MessageBox.Show("Update saved", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e) // search pembayaran
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=;database=irama;";

            string criteria = textBox1.Text;

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "select parent_id, nama, idsiswa, lastpaid_bulanan as 'bulan terakhir', lastpaid_booklevel as 'buku terakhir' from pembayaran, siswa where parent_id = siswa.id and nama like '%" + criteria + "%'";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                dg2.DataSource = ds.Tables[0].DefaultView;
                tabPage2.Controls.Add(dg2);

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteRecSiswa_Click(object sender, EventArgs e)
        {
            int rowidx = dg1.CurrentCell.RowIndex;
            string getIdVal = dg1[0, rowidx].Value.ToString();

            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete from siswa where id=" + getIdVal;
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                cmd.CommandText = "select id,name as Nama,no_telp as 'no. telp',alamat,jenis_kursus as 'jenis kursus' from siswa";
                adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                dg1.DataSource = ds.Tables[0].DefaultView;
                tabPage1.Controls.Add(dg1);
                MessageBox.Show("Record deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Can't delete record constrained by foreign key","error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void deleteRecPembayaran_Click(object sender, EventArgs e)
        {
            int rowidx = dg2.CurrentCell.RowIndex;
            string getIdVal = dg2[0, rowidx].Value.ToString();

            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete from pembayaran where parent_id=" + getIdVal;
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                cmd.CommandText = "select parent_id, tanggal, nama, idsiswa, lastpaid_bulanan as 'bulan terakhir', lastpaid_booklevel as 'buku terakhir' from pembayaran, siswa where siswa.id = pembayaran.parent_id";
                adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                dg2.DataSource = ds.Tables[0].DefaultView;
                tabPage2.Controls.Add(dg2);
                MessageBox.Show("Record deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Can't delete record constrained by foreign key", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
