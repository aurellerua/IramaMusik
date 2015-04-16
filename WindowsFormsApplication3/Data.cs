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

        DataGridView dg1 = new DataGridView(); //siswa
        DataGridView dg2 = new DataGridView(); //pembayaran
        MySqlDataAdapter adap;
        DataSet ds;
        MySql.Data.MySqlClient.MySqlConnection conn;

        private void Data_Load(object sender, EventArgs e)
        {
            tabPage1.Text = "Siswa";
            tabPage2.Text = "Pembayaran";

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
                cmd.CommandText = "select id as 'ID Siswa',name as Nama,no_telp as 'No. Telp',alamat as Alamat,jenis_kursus as 'Jenis Kursus', grade as Grade from siswa";
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

                    cmd.CommandText = "select id_pemb as 'ID Pembayaran', tanggal as Tanggal, name as Nama, idsiswa as 'ID Siswa', lastpaid_bulanan as 'Bulan Terakhir', lastpaid_booklevel as 'Level Buku' from pembayaran, siswa where siswa.id = pembayaran.idsiswa";
                adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);
                DataGridView dg2 = new DataGridView();
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

        private void button5_Click(object sender, EventArgs e) // update siswa
        {
            int columnidx = dg1.CurrentCell.ColumnIndex;
            int rowidx = dg1.CurrentCell.RowIndex;
            string selectedCellVal = dg1[columnidx, rowidx].Value.ToString();
            string getIdVal = dg1[0, rowidx].Value.ToString();

            MySqlCommand cmd = conn.CreateCommand();
            string column = "";
            switch (columnidx)
            {
                case 1:
                    column = "name";
                    break;
                case 2:
                    column = "no_telp";
                    break;
                case 3:
                    column = "alamat";
                    break;
                case 4:
                    column = "jenis_kursus";
                    break;
                case 5:
                    column = "grade";
                    break;
            }

            cmd.CommandText = "update siswa set " + column + "='" + dg1.SelectedCells[0].Value + "' where id=" + getIdVal;
            adap = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            adap.Fill(ds);
            MessageBox.Show("Perubahan tersimpan.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Data terhapus.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Can't delete record constrained by foreign key", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e) // update pembayaran
        {
            //int columnidx = dg2.CurrentCell.ColumnIndex;
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

            cmd.CommandText = "update pembayaran set " + column + "='" + dg2.SelectedCells[0].Value + "' where id_pemb=" + getIdVal;
            adap = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            adap.Fill(ds);
            MessageBox.Show("Perubahan tersimpan.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void deleteRecPembayaran_Click(object sender, EventArgs e)
        {
            int rowidx = dg2.CurrentCell.RowIndex;
            //int rowidx = 1;
            string getIdVal = dg2[0, rowidx].Value.ToString();

            try
            {
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete from pembayaran where id_pemb=" + getIdVal;
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                cmd.CommandText = "select id_pemb, tanggal, nama, idsiswa, lastpaid_bulanan as 'Bulan Terakhir', lastpaid_booklevel as 'Level Buku' from pembayaran, siswa where siswa.id = pembayaran.id_pemb";
                adap = new MySqlDataAdapter(cmd);
                ds = new DataSet();
                adap.Fill(ds);

                dg2.DataSource = ds.Tables[0].DefaultView;
                tabPage2.Controls.Add(dg2);
                MessageBox.Show("Data terhapus.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Can't delete record constrained by foreign key", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        
    }
}
