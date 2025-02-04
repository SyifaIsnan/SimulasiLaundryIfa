using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
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
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";
            textBox6.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;

            tampildata();
            using (var conn = Properti.conn())
            {
                SqlCommand cmd = new SqlCommand("select kodepetugas, namapetugas from [PetugasAntar]", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "namapetugas";
                comboBox2.ValueMember = "kodepetugas";
                comboBox2.SelectedIndex = -1;
                conn.Close();
            }

        }

        private void tampildata()
        {
            using (var conn = Properti.conn())
            {
                SqlCommand cmd = new SqlCommand("select * from [Order]", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                DataTable dt = new DataTable(); 
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                dataGridView1.DataSource = dt;


                DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                combo.Name = "Status";
                combo.HeaderText = "Status";
                combo.DataSource = new string[] { "PENDING", "DICUCI", "DIANTAR" };
                combo.DataPropertyName = "Status";

                DataGridViewLinkColumn link = new DataGridViewLinkColumn();
                link.Name = "Layanan";
                link.HeaderText = "Layanan";
                link.Text = "Layanan";
                link.UseColumnTextForLinkValue = true;

                dataGridView1.Columns.Add(combo);
                dataGridView1.Columns.Add(link);

                conn.Close();
            }           
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

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            using (var conn = Properti.conn())
            {
                DateTime order = dateTimePicker1.Value;
                DateTime selesai = dateTimePicker2.Value;

                TimeSpan selisih = selesai - order;
                int lamahari = selisih.Days;
                textBox3.Text = lamahari.ToString();

                if (lamahari > 3)
                {
                    SqlCommand cmd = new SqlCommand("SELECT biaya FROM BiayaTambahan WHERE kodebiaya = 1", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox4.Text = dr["biaya"].ToString();
                    }
                }
                else
                {
                    textBox4.Text = "0";
                }
                conn.Close();

                hitungtotal();


            }

        }

         private void hitungtotal()
        {
            int jemput = Convert.ToInt32(textBox6.Text);
            int antar = Convert.ToInt32(textBox7.Text);
            int hari = Convert.ToInt32(textBox4.Text);

            int total = jemput + antar + hari;
            textBox5.Text = total.ToString("C", CultureInfo.GetCultureInfo("id-ID"));

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            using (var conn = Properti.conn())
            {
                if (checkBox1.Checked)
                {
                    SqlCommand cmd = new SqlCommand("select biaya from [biayatambahan] where kodebiaya = 3", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox6.Text = dr["biaya"].ToString();
                    }

                }
                else
                    {
                        textBox6.Text = "0";
                    }
                conn.Close();
                hitungtotal();

            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            using (var conn = Properti.conn())
            {
                if (checkBox2.Checked)
                {
                    SqlCommand cmd = new SqlCommand("SELECT biaya FROM BiayaTambahan WHERE kodebiaya = 2", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        textBox7.Text = dr["biaya"].ToString();
                    }

                    label8.Enabled = true;
                    comboBox2.Enabled = true;
                }
                else
                {
                    textBox7.Text = "0";
                    label8.Enabled = false;
                    comboBox2.Enabled = false;
                }
                conn.Close();
                hitungtotal();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var conn = Properti.conn()) 
            { 
                try
                {
                    if (Properti.validasi(this.Controls, textBox8))
                    {
                        MessageBox.Show("Inputan tidak boleh kosong!");
                    }
                    else
                    {
                        SqlCommand pelanggan = new SqlCommand("select count(*) from [Pelanggan] where nomortelepon = @nomortelepon", conn);
                        pelanggan.CommandType = CommandType.Text;
                        conn.Open();
                        pelanggan.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                        SqlDataReader dr = pelanggan.ExecuteReader();
                        int datapelanggan = (int)pelanggan.ExecuteScalar();
                        conn.Close();
                        if (datapelanggan == 0)
                        {
                            SqlCommand pel = new SqlCommand("insert into [Pelanggan] values (@nomortelepon, @nama, @alamat)", conn);
                            pel.CommandType = CommandType.Text;
                            conn.Open();
                            pel.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                            pel.Parameters.AddWithValue("@nama", textBox2.Text);
                            pel.Parameters.AddWithValue("@alamat", richTextBox1.Text);
                            pel.ExecuteNonQuery();
                            conn.Close();
                        }

                        SqlCommand cmd = new SqlCommand("insert into [Order] values (@nomortelepon, @tanggalorder, @tanggalselesai, @biayaantar, @biayajemput, @biayahari, @petugasantar, @statusorder) where kodeorder=@kodeorder", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                        cmd.Parameters.AddWithValue("@tanggalorder", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@tanggalselesai", dateTimePicker2.Value);
                        cmd.Parameters.AddWithValue("@biayaantar", textBox7);
                        cmd.Parameters.AddWithValue("@biayajemput", textBox6);
                        cmd.Parameters.AddWithValue("@biayahari", textBox4);
                        cmd.Parameters.AddWithValue("@petugasantar", comboBox2.SelectedValue);
                        cmd.Parameters.AddWithValue("@statusorder", "PENDING");
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Berhasil menambahkan data!");
                        hitungtotal();
                        tampildata();
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.CurrentRow.Cells;
            textBox1.Text = row["nomortelepon"].Value.ToString();
            textBox6.Text = row["biayajemput"].Value.ToString();
            textBox7.Text = row["biayaantar"].Value.ToString();
            textBox4.Text = row["biayahari"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(row["tanggalorder"].Value.ToString());
            dateTimePicker2.Value = Convert.ToDateTime(row["tanggalselesai"].Value.ToString());
            comboBox2.SelectedValue = row["petugasantar"].Value.ToString();
        }
    }
}
