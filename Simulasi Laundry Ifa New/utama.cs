using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulasi_Laundry_Ifa_New
{
    public partial class utama : Form
    {
        public utama(string namauser)
        {
            InitializeComponent();
            if (namauser == "Admin")
            {
                lOGINToolStripMenuItem.Enabled = false;
                dATAToolStripMenuItem.Enabled = false;
                bIAYATAMBAHANToolStripMenuItem.Enabled = false;
            }
            else if (namauser == "kasir")
            {
                lOGINToolStripMenuItem.Enabled = false;
            }
        }

        private void lOGINToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void uSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataUser dataUser = new DataUser();
            dataUser.ShowDialog();
        }
    }
}
