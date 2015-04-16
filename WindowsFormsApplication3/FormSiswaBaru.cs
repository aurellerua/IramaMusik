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
    public partial class FormSiswaBaru : Form
    {
        public FormSiswaBaru()
        {
            InitializeComponent();
            CenterToParent();
        }

        private void FormSiswaBaru_Load(object sender, EventArgs e)
        {

        }

        private void buttonTambah_Click(object sender, EventArgs e)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            string myConnectionString;

            myConnectionString = "server=127.0.0.1;uid=root;" +
                "pwd=;database=iramamusik;";

            string nama = textNama.Text;
            string alamat = textAlamat.Text;
            string notelp = textNotelp.Text;
            string jenis = textJenis.Text;

            if (nama != "" & alamat != "" & notelp != "" & jenis != "")
            {
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection();
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    MySql.Data.MySqlClient.MySqlCommand cmd = conn.CreateCommand();
                    long id = cmd.LastInsertedId;
                    cmd.CommandText = "insert into siswa values ('id', '" + nama + "', '" + notelp + "', '" + alamat + "', '" + jenis + "', '1')";
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
                MessageBox.Show("Data harus lengkap!");
            }
        }

        
    }
}
