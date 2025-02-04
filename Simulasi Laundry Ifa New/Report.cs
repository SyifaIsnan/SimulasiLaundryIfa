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
using System.Windows.Forms.DataVisualization.Charting;

namespace Simulasi_Laundry_Ifa_New
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
            string[] from = new string[12];
            string[] to = new string[12];

            for(int i = 0 ; i < from.Length; i++)
            {
                from[i] = new DateTime(DateTime.Now.Year, i+1, 1).ToString("MMMM");
                to[i] = new DateTime(DateTime.Now.Year, i+1, 1).ToString("MMMM");
                
            }

            for(int i = 0; i < from.Length; i++ )
            {
                comboBox1.Items.Add(from[i].ToString());
                comboBox2.Items.Add(to[i].ToString());
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var conn = Properti.conn())
            {
                string from = comboBox1.SelectedItem.ToString();
                string to = comboBox2.SelectedItem.ToString();
                SqlCommand cmd = new SqlCommand("select month([Order].tanggalorder) as 'Bulan', sum(jumlahunit*biaya) as 'Income' from [Order] inner join [Detailorder] on [Order].kodeorder = [Detailorder].kodeorder where tanggalorder >= @from and tanggalorder <= @to group by month(tanggalorder)", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@from", new DateTime(DateTime.Now.Year, DateTime.ParseExact(from, "MMMM", CultureInfo.CurrentCulture).Month, 1));
                cmd.Parameters.AddWithValue("@to", new DateTime(DateTime.Now.Year, DateTime.ParseExact(to, "MMMM", CultureInfo.CurrentCulture).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.ParseExact(to, "MMMM", CultureInfo.CurrentCulture).Month)));
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);

                dataGridView1.DataSource = dt.AsEnumerable().Select(d => new
                {
                    Bulan = new DateTime(DateTime.Now.Year, d.Field<int>("Bulan"), 1).ToString("MMMM"),
                    Income = d.Field<int>("Income").ToString("C", CultureInfo.GetCultureInfo("id-ID")),
                }).ToList();

                chart1.Series.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string Bulan = new DateTime(DateTime.Now.Year, row.Field<int>("Bulan"), 1).ToString("MMMM");
                    int Income = row.Field<int>("Income"); 
                    if(chart1.Series.IndexOf("Pendapatan") == -1)
                    {
                        chart1.Series.Add("Pendapatan");
                        chart1.Series["Pendapatan"].ChartType = SeriesChartType.Column;
                    }

                    chart1.Series["Pendapatan"].Points.AddXY(Bulan, Income);
                }

                conn.Close();
            }
        }
    }
}
