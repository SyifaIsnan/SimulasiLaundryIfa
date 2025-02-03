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

namespace Simulasi_Laundry_Ifa_New
{
    public partial class DataUser : Form
    {
        public DataUser()
        {
            InitializeComponent();

            tampidata();
        }

        private void tampidata()
        {
            using (var conn = Properti.conn())
            {
                SqlCommand cmd = new SqlCommand("select * from [User]", conn);
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
                conn.Close();

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var conn = Properti.conn())
            {
                if (Properti.validasi(this.Controls))
                {
                    MessageBox.Show("Data yang ingin ditambahkan tidak boleh kosong!");
                }
                else
                {
                    try
                    {
                        var mess = MessageBox.Show("Apakah data yang ingin anda tambahkan sudah benar?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (mess == DialogResult.Yes)
                        {
                            SqlCommand cmd = new SqlCommand("INSERT INTO [User] VALUES (@namauser, @email, @password)", conn);
                            conn.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@namauser", textBox1.Text);
                            cmd.Parameters.AddWithValue("@email", textBox2.Text);
                            cmd.Parameters.AddWithValue("@password", Properti.enkripsi(textBox3.Text));
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data berhasil ditambahkan!");
                            tampidata();

                        }
                    } catch(Exception ex) 
                    {
                        conn.Close();
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }
    }
}
