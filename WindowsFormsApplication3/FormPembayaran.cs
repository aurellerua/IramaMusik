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
    public partial class FormPembayaran : Form
    {
        public FormPembayaran()
        {
            InitializeComponent();
            CenterToParent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=;database=iramamusik;";

            string nama = textNama.Text;
            string idsiswa = textID.Text;
            string bulan = textBoxBayarBulan.Text;
            string buku = textBoxBukuLevel.Text;
            string tanggal = textTanggal.Text;

            if (nama != "" & idsiswa != "")
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    MySql.Data.MySqlClient.MySqlCommand cmd = conn.CreateCommand();
                    long id = cmd.LastInsertedId;
                    cmd.CommandText = "insert into pembayaran values ('id', '" + tanggal + "', '" + nama + "', '" + idsiswa + "', '" + bulan + "', '" + buku + "')";
                    MySql.Data.MySqlClient.MySqlDataAdapter adap = new MySql.Data.MySqlClient.MySqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adap.Fill(ds);

                    MessageBox.Show("Data berhasil disimpan.");
                    this.Hide();
                    Data dataform = new Data();
                    dataform.Show();
                }

                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Data siswa harus lengkap!");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            FormPembayaran fPemb = new FormPembayaran();
            fPemb.Show();
        }
        
    }

}
