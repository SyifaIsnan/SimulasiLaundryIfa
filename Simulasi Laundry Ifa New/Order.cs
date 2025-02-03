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
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();


            tampildata();


        }

        private void tampildata()
        {
            throw new NotImplementedException();
           
        }

        private void tampilkanpelanggan(string nama, string alamat)
        {
            textBox2.Text = nama;
            richTextBox1.Text = alamat;
        }

        private void nomortelepon(string nomortelepon)
        {
           using (var conn = Properti.conn())
           {
                SqlCommand cmd = new SqlCommand("select * from [Pelanggan] where nomortelepon = @nomortelepon", conn);
                conn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string nama = dr["nama"].ToString();
                    string alamat = dr["alamat"].ToString();
                    tampilkanpelanggan(nama, alamat);
                }
                conn.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string nomortelepo = textBox1.Text;
                nomortelepon(nomortelepo);
            }
        }
    }
}
