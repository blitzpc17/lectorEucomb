using entidades.ReporteLitros;
using entidades.VentaPorFecha;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eucomb.Lector
{
    public partial class formVentaPorFecha: Form
    {

        private List<clsReporteVentaPorFecha> ListaRegistros;
        private List<clsReporteVentaPorFecha> ListaRegistrosAux;

        private string entrada;
        private List<KeyValuePair<string, int>> ListaOpcionesFiltrado;

        private string folderPath;
        private string[] xmlFiles;
        private string argumento;

        private string filePath;

        private formVentaPorFechaLogica processor;

        public formVentaPorFecha()
        {
            InitializeComponent();
        }

        private void InicializarFormulario()
        {

            ListaOpcionesFiltrado = new List<KeyValuePair<string, int>>();

            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("FACTURA", 0));
            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("FECHA", 1));
            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("UUID", 2));
            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("PRODUCTO", 3));
            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("RFC", 4));
            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("CODIGOPOSTAL", 5));
            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("LITROS", 6));
            ListaOpcionesFiltrado.Add(new KeyValuePair<string, int>("IMPORTE", 7));

            cbxFiltro.DataSource = ListaOpcionesFiltrado;
            cbxFiltro.DisplayMember = "key";
            cbxFiltro.ValueMember = "value";

            cbxFiltro.SelectedIndex = -1;

            tsProgressbar.Visible = false;
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = "Selecciona la carpeta que contiene los archivos XML"
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    processor = new formVentaPorFechaLogica();

                    folderPath = folderBrowserDialog.SelectedPath;

                    xmlFiles = Directory.GetFiles(folderPath, "*.xml");

                    ListaRegistros = new List<clsReporteVentaPorFecha>();

                    tsProgressbar.Visible = true;

                    backgroundWorker1.RunWorkerAsync("leer");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void LLenarDgv()
        {
            dgvRegistros.DataSource = ListaRegistrosAux;
            Apariencias();
            tsTotalRegistros.Text = dgvRegistros.RowCount.ToString("N0");
        }

        private void Apariencias()
        {
            dgvRegistros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvRegistros.Columns[0].Width = 100;
            dgvRegistros.Columns[0].HeaderText = "FACTURA";
            dgvRegistros.Columns[0].Frozen = true;

            dgvRegistros.Columns[1].Width = 150;
            dgvRegistros.Columns[1].HeaderText = "FECHA";
            dgvRegistros.Columns[1].Frozen = true;

            dgvRegistros.Columns[2].Width = 210;
            dgvRegistros.Columns[2].HeaderText = "UUID";
            dgvRegistros.Columns[2].Frozen = true;

            dgvRegistros.Columns[3].Width = 280;
            dgvRegistros.Columns[3].HeaderText = "PRODUCTO";

            dgvRegistros.Columns[4].Width = 130;
            dgvRegistros.Columns[4].HeaderText = "LITROS";

            dgvRegistros.Columns[5].Width = 130;
            dgvRegistros.Columns[5].HeaderText = "IMPORTE";

            dgvRegistros.Columns[6].Width = 130;
            dgvRegistros.Columns[6].HeaderText = "RFC";

            dgvRegistros.Columns[7].Width = 100;
            dgvRegistros.Columns[7].HeaderText = "C.P.";

        }

        private void ReportarProgressBar(int valor)
        {
            Console.WriteLine(valor.ToString("N0"));
            backgroundWorker1.ReportProgress(valor);        
        }

        private void txtBusqueda_KeyUp(object sender, KeyEventArgs e)
        {
            entrada = txtBusqueda.Text;

            switch (cbxFiltro.SelectedValue)
            {
                case 0:

                    ListaRegistrosAux = ListaRegistros.Where(x => x.Factura.ToUpper().Contains(entrada)).ToList();
                    break;

                case 1:

                    ListaRegistrosAux = ListaRegistros.Where(x => x.Fecha.ToString("dd/MM/yyyy HH:mm:ss").Contains(entrada)).ToList();
                    break;

                case 2:
                    ListaRegistrosAux = ListaRegistros.Where(x => x.Uuid.ToUpper().Contains(entrada)).ToList();
                    break;

                case 3: 
                    ListaRegistrosAux = ListaRegistros.Where(x => x.Producto.ToUpper().Contains(entrada)).ToList();
                    break;

                case 4:
                    ListaRegistrosAux = ListaRegistros.Where(x => x.Rfc.ToUpper().Contains(entrada)).ToList();
                    break;

                case 5:
                    ListaRegistrosAux = ListaRegistros.Where(x => x.CodigoPostal.Contains(entrada)).ToList();
                    break;

                case 6:
                    ListaRegistrosAux = ListaRegistros.Where(x => x.Litros.ToString().Contains(entrada)).ToList();
                    break;

                case 7:
                    ListaRegistrosAux = ListaRegistros.Where(x => x.Importe.ToString().Contains(entrada)).ToList();
                    break;
            }

            ListaRegistrosAux = ListaRegistrosAux.OrderBy(x => x.Rfc).ThenBy(x => x.CodigoPostal).ThenBy(x => x.Fecha).ToList();



            LLenarDgv();
        }

        private void formVentaPorFecha_Load(object sender, EventArgs e)
        {
            InicializarFormulario();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            argumento = e.Argument.ToString();
            switch (argumento)
            {
                case "leer":
                    int pos = 0;
                    foreach (string filePath in xmlFiles)
                    {
                        var cfdi = processor.DeserializarCFDI(filePath);                      

                        foreach (var item in cfdi.Conceptos)
                        {
                            ListaRegistros.Add(new clsReporteVentaPorFecha
                            {
                                Factura = cfdi.Folio,
                                Rfc = cfdi.Receptor.Rfc,
                                CodigoPostal = cfdi.Receptor.DomicilioFiscalReceptor,
                                Fecha = cfdi.Fecha,
                                Uuid = cfdi.Complemento.TimbreFiscalDigital.UUID,
                                Producto = item.Descripcion,
                                Litros = item.Cantidad,
                                Importe = item.Importe

                            });                           
                        }

                        pos++;
                        ReportarProgressBar((int)Math.Round((double)pos * 80/ xmlFiles.Length));


                    }

                    ListaRegistros = ListaRegistros.OrderBy(x => x.Rfc).ThenBy(x => x.CodigoPostal).ThenBy(x => x.Fecha).ToList();
                    ListaRegistrosAux = ListaRegistros;

                    ReportarProgressBar(90);

                    break;

                case "exportar":
                    Utilidades.FastExportToExcel(ListaRegistrosAux, filePath);
                    ReportarProgressBar(90);
                    break;
            }


            ReportarProgressBar(100);
            



           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (argumento)
            {
                case "leer":
                    LLenarDgv();                   
                    break;

                case "exportar":
                    MessageBox.Show("Archivo generado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
              
            }
            tsProgressbar.Visible = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tsProgressbar.Value = e.ProgressPercentage;
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog sf = new FolderBrowserDialog())
            {
                sf.Description = "Selecciona la carpeta de destino para el archivo Excel";
                sf.ShowNewFolderButton = true;
                if (sf.ShowDialog() == DialogResult.OK)
                {

                    string carpetaSeleccionada = sf.SelectedPath;
                    filePath = System.IO.Path.Combine(carpetaSeleccionada, "_ventas_por_fecha.xlsx");
                    tsProgressbar.Visible = true;
                    backgroundWorker1.RunWorkerAsync("exportar");

                }
            }
        }
    }
}
