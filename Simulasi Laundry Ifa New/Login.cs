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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using(var conn = Properti.conn())
            {
                try
                {

                    if (Properti.validasi(this.Controls))
                    {
                        MessageBox.Show("Data yang ingin diinput tidak boleh kosong!");
                    }
                    else
                    {
                        var mess = MessageBox.Show("Apakah data yang ingin anda input sudah benar?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (mess == DialogResult.Yes)
                        {
                            SqlCommand cmd = new SqlCommand("select * from [User] where email = @email and password = @password", conn);                            
                            cmd.CommandType = CommandType.Text;
                            conn.Open();
                            cmd.Parameters.AddWithValue("@email", textBox1.Text);
                            cmd.Parameters.AddWithValue("@password",Properti.enkripsi(textBox2.Text));
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {
                                this.Hide();
                                string namauser = dr["namauser"].ToString();
                                utama utama = new utama(namauser);
                                utama.Show();
                            } else
                            {
                                MessageBox.Show("User tidak ditemukan!");
                            }
                            
                            conn.Close();
                        }
                    }

                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
