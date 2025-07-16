using eucomb.Lector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eucomb.Sistema
{
    public partial class MDIMain : Form
    {
        private int childFormNumber = 0;

        public MDIMain()
        {
            InitializeComponent();
        }       

        private void ventasPorMesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formVentaPorFecha form = new formVentaPorFecha();
            form.MdiParent = this;
            form.Show();
        }

        private void jSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMensualJson form = new formMensualJson();
            form.MdiParent = this;
            form.Show();
        }
    }
}
