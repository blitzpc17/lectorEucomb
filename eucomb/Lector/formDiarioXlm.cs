using entidades.XML.Diario;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace eucomb.Lector
{
    public partial class formDiarioXlm: Form
    {
        private clsCdfiDiario facturaDetalle;
        private List<KeyValuePair<string, string>> LstCfdisErrores;
        private List<clsContenidoControlVolumetrico> ListaXlm;
       // private List<clsInventarioDiario> LstProductos;

        private clsInventarioDiario Inventario;
        private clsInventarioDiario InventarioFinal;


        private string argumentoBackground = "";
        private string rutaArchivo = "";

        private int progreso = 0;

        private XmlSerializer serializer;




        public formDiarioXlm()
        {
            InitializeComponent();
        }

        private void InicializarModulo()
        {
            facturaDetalle = null;
            LstCfdisErrores = null;
            argumentoBackground = "";
            rutaArchivo = "";
            progreso = 0;
            serializer = null;
           
        }

        private void formMensualXlm_Shown(object sender, EventArgs e)
        {
            InicializarModulo();
        }

        private void btnXmlJson_Click(object sender, EventArgs e)
        {
            ImportarXLM();
        }

        private void ImportarXLM()
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();

           
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                rutaArchivo = fbd.SelectedPath;               

                RecorrerNodosXML();
            

            }
        }

        private void RecorrerNodosXML()
        {
            lblCarga.Text = @"Leyendo XLM...";
            panelCarga.Visible = true;
            progreso = 0;
            backgroundWorker1.RunWorkerAsync("nodos");
        }

        private void MostrarInventarios()
        {
            // var productosOrdenados = facturaDetalle.Productos.OrderByDescending(x => x.ClaveProducto).ToList();
            List<string> productosOrdenados = Inventario.Productos.Select(x => x.ClaveProducto).Distinct().OrderByDescending(y=>y).ToList();
            LstCfdisErrores = new List<KeyValuePair<string, string>>();

            //aqui se va a trabajar ya con la lista de inventario
            InventarioFinal = new clsInventarioDiario();
            InventarioFinal.Productos = new List<ProductoData>();
            
            int i = 0;
            foreach(var prod in productosOrdenados)
            {
                i++;
                ProductoData data = new ProductoData
                {
                    ClaveProducto = Inventario.Productos.FirstOrDefault(x=>x.ClaveProducto == prod).ClaveProducto,
                    NombreProducto = Inventario.Productos.FirstOrDefault(x => x.ClaveProducto == prod).NombreProducto,
                    InventarioTanqueFinMes = Inventario.Productos.Where(x=>x.ClaveProducto == prod).Sum(x=>x.InventarioTanqueFinMes),
                    NumeroEntradasProducto = Inventario.Productos.Where(x => x.ClaveProducto == prod).Sum(x=>x.NumeroEntradasProducto),
                    TotalLitrosFactura = Inventario.Productos.Where(x => x.ClaveProducto == prod).Sum(x=>x.TotalLitrosFactura)

                };

                data.Partidas = new List<ProductoPartida>();

                foreach(var partidas in Inventario.Productos.Where(x=>x.ClaveProducto == prod ))
                {

                    foreach(var partida in partidas.Partidas)
                    {
                        ProductoPartida pp = new ProductoPartida
                        {
                            NombreCliente = partida.NombreCliente,
                            RfcCliente = partida.RfcCliente,    
                            Cfdi = partida.Cfdi,
                            LongitudCfdi = partida.LongitudCfdi,
                            FechaHora = partida.FechaHora,
                            PrecioCompra = partida.PrecioCompra,
                            PrecioVenta = partida.PrecioVenta,
                            ValorNumerico = partida.ValorNumerico,
                        };

                        data.Partidas.Add(pp);  
                    }

                    
                }

                InventarioFinal.Productos.Add(data);    

                progreso = ((int)Math.Round( (i * 100 / productosOrdenados.Count) * .4)) + 50;
                backgroundWorker1.ReportProgress(progreso);

            }


          

           
                
              

            

        }

        private void MostrarEnPantalla()
        {

            if (ListaXlm.Count >= 1)
            {
                dgvRegistrosDiario.DataSource = ListaXlm;
                tsTotalRegistros.Text = ListaXlm.Count.ToString("N0");
            }

            //set encabezado del modulo
            txtSucursal.Text = Inventario.Sucursal;
            txtVersion.Text = Inventario.Version;
            txtRfcContrib.Text = Inventario.RfcContribuyente;
            txtRfcProveedor.Text = Inventario.RfcProveedor;
            txtRfcRepresentante.Text = Inventario.RfcRepresentante;
            txtPeriodo.Text = Inventario.Periodo.ToString();
            txtCaracter.Text = Inventario.Caracter;
            txtModPermiso.Text = Inventario.ModPermiso;
            txtNoPermiso.Text = Inventario.NoPermiso;



            if (InventarioFinal.Productos!=null && InventarioFinal.Productos.Count>0) //LstProductos.Count > 0)
            {
                int posicionDgvInventariosY = 0;
                foreach (var producto in InventarioFinal.Productos.OrderByDescending(x=>x.ClaveProducto))//Inventario.Productos.OrderByDescending(x=>x.ClaveProducto))//LstProductos.OrderByDescending(x => x.).ToList())
                {
                    DataGridView dgvEncabezado = new DataGridView();

                    dgvEncabezado.Columns.Add("Texto", "");
                    dgvEncabezado.Columns.Add("Valor", "");
                    dgvEncabezado.Name = "dgvEncabezado" + producto.ClaveProducto;
                    dgvEncabezado.Rows.Add("Producto: "+producto.NombreProducto);
                    dgvEncabezado.Rows.Add("INVENTARIO EN TANQUE AL FINALIZAR EL MES:", producto.InventarioTanqueFinMes);//producto.VolumenExistenciasMes.ToString("N2"));
                    dgvEncabezado.Rows.Add("NÚMERO DE VECES QUE ENTRO PRODUCTO AL TANQUE:", producto.NumeroEntradasProducto); //producto.TotalRecepcionesMes.ToString("N0"));
                    dgvEncabezado.Rows.Add("TOTAL DE LITROS QUE MUESTRA LA FACTURA:", producto.TotalLitrosFactura);// producto.SumaVolumenRecepcionMes.ToString("N2"));
                    panelInventarios.Controls.Add(dgvEncabezado);
                    dgvEncabezado.Location = new System.Drawing.Point(10, posicionDgvInventariosY + 30);
                    dgvEncabezado.Width = 1107;
                    dgvEncabezado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)))));
                    dgvEncabezado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    posicionDgvInventariosY = dgvEncabezado.Location.Y + dgvEncabezado.Size.Height + 15;
                    
                  

                    panelInventarios.Controls.Add(dgvEncabezado);
                    // Grid de partidas
                    var dgvPartidas = new DataGridView();

                    dgvPartidas.Columns.Add("numero", "No.");
                    dgvPartidas.Columns.Add("nombreCliente", "Nombre Cliente Proveedor");
                    dgvPartidas.Columns.Add("rfcCliente", "Rfc Cliente Proveedor");
                    dgvPartidas.Columns.Add("cfdi", "CFDI");
                    dgvPartidas.Columns.Add("long", "Long.");
                    dgvPartidas.Columns.Add("fechaHora", "Fecha y Hora");
                    dgvPartidas.Columns.Add("precioCompra", "Precio Compra");
                    dgvPartidas.Columns.Add("precioVenta", "Precio Venta Púb.");
                    dgvPartidas.Columns.Add("valorNumerico", "ValorNumerico");

    
                    dgvPartidas.Location = new System.Drawing.Point(10, posicionDgvInventariosY + 30);
                    dgvPartidas.Width = 1107;
                    dgvPartidas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)))));
                    dgvPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    posicionDgvInventariosY = dgvPartidas.Location.Y + dgvPartidas.Size.Height + 15;

                    decimal sumaRecepciones = 0;
                    int ip = 0;
                    foreach(var part in producto.Partidas)
                    {
                        ip++;
                        dgvPartidas.Rows.Add(ip, part.NombreCliente, part.RfcCliente, part.Cfdi, part.LongitudCfdi, part.FechaHora, part.PrecioCompra, part.PrecioVenta, part.ValorNumerico);
                       
                    }

                    panelInventarios.Controls.Add(dgvPartidas);





                }

                lblCarga.Text = @"¡Listo!";
                MessageBox.Show("Archivo JSON cargado y deserializado correctamente.");


            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            argumentoBackground = e.Argument.ToString();
            backgroundWorker1.ReportProgress(progreso);
            switch (argumentoBackground)
            {
                case "nodos":

                    Inventario = new clsInventarioDiario();
                    Inventario.Productos = new List<ProductoData>();

                    // Obtener todos los archivos XML en la carpeta
                    string[] archivosXml = Directory.GetFiles(rutaArchivo, "*.xml");
                    int totalArchivos = archivosXml.Length;                  

            

                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("Covol", "https://repositorio.cloudb.sat.gob.mx/Covol/xml/Diarios");
                    namespaces.Add("exp", "Complemento_Expendio");
                    //ns.Add("controlesvolumetricos", "http://www.sat.gob.mx/esquemas/controlesvolumetricos");

                    // Configurar los atributos adicionales para el serializador
                    var attr = new XmlAttributes();
                    attr.Xmlns = true;

                    var overrides = new XmlAttributeOverrides();
                    overrides.Add(typeof(clsCdfiDiario), attr);

                    serializer = new XmlSerializer(typeof(clsCdfiDiario), "https://repositorio.cloudb.sat.gob.mx/Covol/xml/Diarios");
                   

                    ListaXlm = new List<clsContenidoControlVolumetrico>();
                    int docRecorridos = 0;
                    string nombreArchivo = "" ;
                    foreach (string path in archivosXml)
                    {
                        facturaDetalle = null;
                        nombreArchivo = Path.GetFileName(path);

                        using (StreamReader reader = new StreamReader(path))//XmlReader reader = XmlReader.Create(fileStream))
                        {
                            facturaDetalle = (clsCdfiDiario)serializer.Deserialize(reader);
                        }

                        if (string.IsNullOrEmpty(Inventario.RfcRepresentante))
                        {
                            Inventario.Sucursal = "PENDIENTE";
                            Inventario.Version = facturaDetalle.Version;
                            Inventario.RfcContribuyente = facturaDetalle.RfcContribuyente;
                            Inventario.RfcProveedor = facturaDetalle.RfcRepresentanteLegal;
                            Inventario.Periodo = Utilidades.CambiarFormatoFecha(facturaDetalle.FechaYHoraCorte);
                            Inventario.Caracter = facturaDetalle.Caracter.TipoCaracter;
                            Inventario.ModPermiso = facturaDetalle.Caracter.ModalidadPermiso;
                            Inventario.NoPermiso = facturaDetalle.Caracter.NumPermiso;                            
                           
                        }                      

                        foreach (var pro in facturaDetalle.Productos)
                        {
                           
                            ProductoData product = new ProductoData
                            {
                                ClaveProducto = pro.ClaveSubProducto,
                                NombreProducto = pro.ClaveSubProducto + " "+ pro.MarcaComercial,
                                InventarioTanqueFinMes = pro.Tanques.Sum(x => x.Existencias.VolumenAcumOpsEntrega.ValorNumerico),
                                NumeroEntradasProducto = pro.Tanques.Sum(x => x.Recepciones.TotalRecepciones),
                                TotalLitrosFactura = pro.Tanques.Sum(x => x.Recepciones.SumaVolumenRecepcion.ValorNumerico),
                                Partidas = new List<ProductoPartida>()
                            };

                            foreach(var tan in pro.Tanques)
                            {

                                if (tan.Recepciones.TotalRecepciones <= 0) continue;

                         

                                product.Partidas.Add(new ProductoPartida
                                {
                                    NombreCliente = "",//obj.NombreClienteOProveedor,
                                    RfcCliente = "",//obj.RfcClienteOProveedor,
                                    Cfdi = nombreArchivo, //obj.CFDIs.CFDI,  
                                    LongitudCfdi = 0,//obj.CFDIs.CFDI.Length,
                                    FechaHora = Utilidades.CambiarFormatoFecha(tan.Existencias.FechaYHoraEstaMedicion),
                                    PrecioCompra = 0,
                                    PrecioVenta = 0,//pregutnar que va aqui si preceioventa o rpecio al publico
                                    ValorNumerico = tan.Recepciones.SumaVolumenRecepcion.ValorNumerico

                                });

                                Console.WriteLine(pro.MarcaComercial);
                            }

                            Inventario.Productos.Add(product);

                            foreach (var dispensario in pro.Dispensarios)
                            {
                                foreach (var manguera in dispensario.Mangueras)
                                {
                                    foreach(var entrega in manguera.Entregas.Entregas)
                                    {                                        

                                        if (entrega.Complemento == null || 
                                            entrega.Complemento.ComplementoExpendio == null|| 
                                            entrega.Complemento.ComplementoExpendio.Nacional ==null) continue;

                                        Nacional obj = entrega.Complemento.ComplementoExpendio.Nacional;                                        

                                        ListaXlm.Add(new clsContenidoControlVolumetrico
                                        {                                          
                                            RfcClienteOProveedor = obj.RfcClienteOProveedor,
                                            NombreClienteOProveedor = obj.NombreClienteOProveedor,
                                            CFDI = obj.CFDIs.CFDI,
                                            FechaYHoraTransaccion = entrega.FechaYHoraEntrega,
                                            ValorNumerico = obj.CFDIs.VolumenDocumentado.ValorNumerico
                                        });                                      

                                    }
                                }
                            }

                            

                        }

                        docRecorridos++;
                        Console.WriteLine(docRecorridos);
                        double subpro = (docRecorridos * 100 / totalArchivos) * .5;
                        Console.WriteLine(subpro);
                        progreso = (int)Math.Round(subpro);             
                        Console.WriteLine(progreso);
                        Console.WriteLine(nombreArchivo);
                        backgroundWorker1.ReportProgress(progreso);



                    }
                   

                    MostrarInventarios();

                    break;

               
                case "exportar":
                 //   ExportacionExcel(nombreExcel);
                    break;



            }

            backgroundWorker1.ReportProgress(100);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (argumentoBackground)
            {
                case "nodos":
                    lblCarga.Text = @"Mostrando en pantalla...";
                    MostrarEnPantalla();
                    progreso = 0;
                    break; 
               

                case "exportar":
                    lblEstado.Text = "¡Listo!";
                    MessageBox.Show("Exportación realizada, revise su archivo excel.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar1.Value = 0;
                    break;
            }


            lblCarga.Text = @"";
            panelCarga.Visible = false;

            lblEstado.Text = @"";
            panelCargaArchivo.Visible = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (argumentoBackground)
            {
                case "nodos":
                    progressBar2.Value = e.ProgressPercentage;
                    break;
               
                case "exportar":
                    progressBar1.Value = e.ProgressPercentage;
                    break;
            }
        }

        private void btnImportarLayout_Click(object sender, EventArgs e)
        {

        }
    }
}
