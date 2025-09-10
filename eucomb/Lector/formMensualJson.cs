using DocumentFormat.OpenXml.Spreadsheet;
using entidades.JSON;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace eucomb.Lector
{
    public partial class formMensualJson: Form
    {

        private List<clsExVentaPorFecha> LstExcel;

        private List<clsMensual> ListaJson;

        private List<KeyValuePair<string, string>> LstCfdisErrores;

        private clsCfdiMensual facturaDetalle;

        private string argumentoBackground = "";
        private string rutaExcel = "";
        private string nombreExcel = "";

        private int progreso = 0;

        //resultados
        List<clsProductoInfo> LstProductos;
        List<clsResultadosMensual> LstResultados;

        public formMensualJson()
        {
            InitializeComponent();
        }

        private void Inicializar()
        {
            rutaExcel = "";
            argumentoBackground = "";
            nombreExcel = "";
            panelCarga.Visible = false;
            progreso = 0;
            panelCargaArchivo.Visible = false;
        }

        private void btnImportarLayout_Click(object sender, EventArgs e)
        {
            // Crear el OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx|Todos los archivos (*.*)|*.*";
            openFileDialog.Title = "Selecciona un archivo Excel";

            // Mostrar el diálogo y verificar si se seleccionó un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                rutaExcel = openFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            progreso = 0;
            lblEstado.Text = "Importando archivo...";
            panelCargaArchivo.Visible = true;
            backgroundWorker1.RunWorkerAsync("importarExcel");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {           
            argumentoBackground = e.Argument.ToString();
            backgroundWorker1.ReportProgress(progreso);
            switch (argumentoBackground)
            {
                case "nodos":

                    ListaJson = new List<clsMensual>();
                    int acumulador = 0;
                    foreach (var prod in facturaDetalle.Producto)
                    {
                        foreach (var complemento in prod.ReporteDeVolumenMensual.Entregas.Complemento)
                        {
                            foreach (var entrega in complemento.Nacional)
                            {
                                foreach (var factura in entrega.CFDIs)
                                    ListaJson.Add(new clsMensual
                                    {
                                        RfcClienteOProveedor = entrega.RfcClienteOProveedor,
                                        NombreClienteOProveedor = entrega.NombreClienteOProveedor,
                                        CFDI = factura.Cfdi,
                                        FechaYHoraTransaccion = Utilidades.CambiarFormatoFecha(factura.FechaYHoraTransaccion),
                                        ValorNumerico = Utilidades.RedondearCantidad(factura.VolumenDocumentado.ValorNumerico)
                                    });
                            }
                        }
                        acumulador++;
                        progreso = (int)Math.Round((double)((acumulador * 45) / facturaDetalle.Producto.Count));
                        backgroundWorker1.ReportProgress(progreso);
                    }

                    MostrarInventarios();

                    break;

                case "importarExcel":
                    LeerExcel(rutaExcel);                    
                    break;

                case "comparar":
                    ComparacionAivic();
                    break;
                case "exportar":
                    ExportacionExcel(nombreExcel);
                    break;



            }

            backgroundWorker1.ReportProgress(100);
        }

        private void ExportacionExcel(string nombreExcel)
        {
            progreso = 0;
            SLDocument sl = new SLDocument();
            sl.SetCellValue("A1", "RfcClienteOProveedor");
            sl.SetCellValue("B1", "NombreClienteOProveedor");
            sl.SetCellValue("C1", "CFDI");
            sl.SetCellValue("D1", "FechaYHoraTransaccion");
            sl.SetCellValue("E1", "ValorNumerico");

            int row = 2;
            
            foreach(var item in ListaJson)
            {
                sl.SetCellValue("A"+row, item.RfcClienteOProveedor);
                sl.SetCellValue("B"+row, item.NombreClienteOProveedor);
                sl.SetCellValue("C"+row, item.CFDI);
                sl.SetCellValue("D"+row, item.FechaYHoraTransaccion);
                sl.SetCellValue("E"+row, item.ValorNumerico);

                row++;


                progreso = (int)Math.Ceiling((double)( (row - 1) * 90 / ListaJson.Count));
                backgroundWorker1.ReportProgress(progreso);
            }

            sl.SaveAs(nombreExcel);


        }

        private void ComparacionAivic()
        {
            // LstControlVolumetricoMensual
            List<clsResultadosMensual> LstCvNoExisten = new List<clsResultadosMensual>();
            //   List<Entidades.AIVIC.EXCEL.FACTURADETALLE> LstFactDetalleNoExisten = new List<Entidades.AIVIC.EXCEL.FACTURADETALLE>();
            LstResultados = new List<clsResultadosMensual>();


            //buscar cuales estan en XML y no en AIVIC
            foreach (var objFact in LstExcel)
            {
                string observacion = "";
                var objNe = objFact;
                bool comparaNombre = false;
                bool comparaCfdi = false;
                bool comparaLts = false;
                clsMensual ObjRegistroCV;

                if (ListaJson.Any(x => x.CFDI == objFact.UUID))
                {
                    ObjRegistroCV = ListaJson.First(x => x.CFDI == objFact.UUID); // LstControlVolumetricoMensual .First(x => x.CFDI == objFact.UUID);
                    DateTime fechaRegistroCv = DateTime.Parse(ObjRegistroCV.FechaYHoraTransaccion.ToShortDateString());
                    if (fechaRegistroCv != objFact.FechaVenta)
                    {
                        observacion = "EXISTE EN ARCHIVO C.V., PERO LAS FECHAS NO COICIDEN.";
                    }
                    else if (Math.Round(ObjRegistroCV.ValorNumerico, 2) != objFact.Cantidad)
                    {
                        observacion = "EXISTE EN ARCHIVO C.V PERO LAS CANTIDADES NO COINCIDEN";
                        comparaLts = false;
                    }

                }
                else
                {
                    ObjRegistroCV = null;
                    observacion = "NO EXISTE EN ARCHIVO C.V.";
                    comparaCfdi = false;

                }

                if (!string.IsNullOrEmpty(observacion) && ObjRegistroCV == null)
                {
                    //LstFactDetalleNoExisten.Add(objNe);
                    LstResultados.Add(new clsResultadosMensual
                    {
                        RfcClienteOProveedor = null,
                        NombreCliente = null,
                        CFDI = null,
                        FechaYHoraTransaccion = null,
                        VolumenNumerico = 0,
                        Existe = ">",
                        folio_Imp = objFact.NumeroFactura,
                        clavecli = "",
                        NombreClienteOPRoveedor = objFact.Cliente,
                        serie = "",
                        docto = "",
                        fecha_reg = objFact.FechaFactura,
                        nombrep = "",
                        Cant = objFact.Cantidad,
                        precio = 0,
                        imported = objFact.TotalVenta,
                        UUID = objFact.UUID,
                        ComparaNombre = comparaNombre,
                        ComparaCfdi = comparaCfdi,
                        ComparaLts = comparaLts,
                        Observacion = observacion,
                        DiferenciaCantidades = objFact.Cantidad
                    });
                }
                else
                {
                    if (ObjRegistroCV == null) return;
                    LstResultados.Add(new clsResultadosMensual
                    {
                        RfcClienteOProveedor = ObjRegistroCV.RfcClienteOProveedor,
                        NombreCliente = ObjRegistroCV.NombreClienteOProveedor,
                        CFDI = ObjRegistroCV.CFDI,
                        FechaYHoraTransaccion = ObjRegistroCV.FechaYHoraTransaccion,
                        VolumenNumerico = ObjRegistroCV.ValorNumerico,
                        Existe = "<>",
                        folio_Imp = objFact.UUID,
                        clavecli = "",
                        NombreClienteOPRoveedor = objFact.Cliente,
                        serie = "",
                        docto = "",
                        fecha_reg = objFact.FechaFactura,
                        nombrep = "",
                        Cant = Math.Round(objFact.Cantidad, 2),
                        precio = 0,
                        imported = objFact.TotalVenta,
                        UUID = objFact.UUID,
                        ComparaNombre = comparaNombre,
                        ComparaCfdi = comparaCfdi,
                        ComparaLts = comparaLts,
                        Observacion = observacion,
                        DiferenciaCantidades = (ObjRegistroCV.ValorNumerico - objFact.Cantidad) < 0 ? (ObjRegistroCV.ValorNumerico - objFact.Cantidad) * -1 : (ObjRegistroCV.ValorNumerico - objFact.Cantidad)
                    });
                }


            }

            //buscar cuales no estan del xml  en el excel de AIVIC
            foreach (var objCv in ListaJson)
            {
                string observacion = "";
                var objNe = objCv;
                bool comparaNombre = false;
                bool comparaCfdi = false;
                bool comparaLts = false;
                clsExVentaPorFecha ObjRegistroFactura;

                if (LstExcel.Any(x => x.UUID == objCv.CFDI))
                {
                    ObjRegistroFactura = LstExcel.First(x => x.UUID == objCv.CFDI);
                    DateTime fechaRegistroFactura = DateTime.Parse(objCv.FechaYHoraTransaccion.ToShortDateString());
                    if (ObjRegistroFactura.FechaFactura != fechaRegistroFactura)
                    {
                        observacion = "EXISTE EN FACTURA ATIO PERO LAS FECHAS NO COINCIDEN";

                    }
                    else if (Math.Round(objCv.ValorNumerico, 2) == ObjRegistroFactura.Cantidad)
                    {
                        observacion = "EXISTE EN FACTURA ATIO PERO LAS CANTIDADES NO COINCIDEN";
                        comparaLts = false;
                    }
                }
                else
                {
                    ObjRegistroFactura = null;
                    observacion = "NO EXISTE EN FACTURA ATIO.";
                    comparaCfdi = false;
                }


                if (!string.IsNullOrEmpty(observacion) && ObjRegistroFactura == null)
                {
                    //  LstFactDetalleNoExisten.Add(objNe);
                    LstResultados.Add(new clsResultadosMensual
                    {
                        RfcClienteOProveedor = objCv.RfcClienteOProveedor,
                        NombreCliente = objCv.NombreClienteOProveedor,
                        CFDI = objCv.CFDI,
                        FechaYHoraTransaccion = objCv.FechaYHoraTransaccion,
                        VolumenNumerico = 0,
                        Existe = "<",
                        folio_Imp = null,
                        clavecli = null,
                        NombreClienteOPRoveedor = null,
                        serie = null,
                        docto = null,
                        fecha_reg = null,
                        nombrep = null,
                        Cant = 0,
                        precio = 0,
                        imported = 0,
                        UUID = null,
                        ComparaNombre = comparaNombre,
                        ComparaCfdi = comparaCfdi,
                        ComparaLts = comparaLts,
                        Observacion = observacion,
                        DiferenciaCantidades = Math.Round(objCv.ValorNumerico, 2)
                    });
                }
                else
                {
                    if (ObjRegistroFactura == null) return;
                    LstResultados.Add(new clsResultadosMensual
                    {
                        RfcClienteOProveedor = objCv.RfcClienteOProveedor,
                        NombreCliente = objCv.NombreClienteOProveedor,
                        CFDI = objCv.CFDI,
                        FechaYHoraTransaccion = objCv.FechaYHoraTransaccion,
                        VolumenNumerico = objCv.ValorNumerico,
                        Existe = "<>",
                        folio_Imp = ObjRegistroFactura.UUID,
                        clavecli = "",
                        NombreClienteOPRoveedor = ObjRegistroFactura.Cliente,
                        serie = "",
                        docto = "",
                        fecha_reg = ObjRegistroFactura.FechaFactura,
                        nombrep = "",
                        Cant = Math.Round(ObjRegistroFactura.Cantidad, 2),
                        precio = 0,
                        imported = ObjRegistroFactura.TotalVenta,
                        UUID = ObjRegistroFactura.UUID,
                        ComparaNombre = comparaNombre,
                        ComparaCfdi = comparaCfdi,
                        ComparaLts = comparaLts,
                        Observacion = observacion,
                        DiferenciaCantidades = (objCv.ValorNumerico - ObjRegistroFactura.Cantidad) < 0 ? (objCv.ValorNumerico - ObjRegistroFactura.Cantidad) * -1 : (objCv.ValorNumerico - ObjRegistroFactura.Cantidad)
                    });
                }

            }


        }

        private void LeerExcel(string ruta)
        {
            // Obtener la ruta del archivo seleccionado
            rutaExcel = ruta;

            // Abrir el archivo Excel para obtener las hojas
            using (SLDocument sl = new SLDocument(rutaExcel))
            {
                // Obtener la lista de nombres de las hojas
                List<string> hojas = sl.GetSheetNames();

                // Mostrar las hojas al usuario
                Console.WriteLine("Hojas disponibles:");
                for (int i = 0; i < hojas.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {hojas[i]}");
                }

                int indiceHoja = 0;

                if (indiceHoja >= 0 && indiceHoja < hojas.Count)
                {
                    // Seleccionar la hoja por su índice
                    sl.SelectWorksheet(hojas[indiceHoja]);

                    // Fila desde la cual comenzar a leer (ejemplo: fila 9)
                    int rowInicio = 9;

                    // Obtener el número de columnas
                    int colFinal = sl.GetWorksheetStatistics().EndColumnIndex;
                    int rowFinal = sl.GetWorksheetStatistics().EndRowIndex;

                    LstExcel = new List<clsExVentaPorFecha>();


                    //recorrer rows                    
                    for (int row = rowInicio; row <= rowFinal; row++)
                    {
                        clsExVentaPorFecha ObjRegistro = new clsExVentaPorFecha();

                        // Leer celda por celda, columna por columna, a partir de la fila especificada
                        for (int col = 1; col <= colFinal; col++)
                        {
                            Console.WriteLine(col);
                            //en la col 8 se va a valdiar si tiene encabezado
                            string valor = sl.GetCellValueAsString(8, col);
                            if (string.IsNullOrEmpty(valor)) continue;


                            switch (valor)
                            {
                                case "Codigo Cliente":
                                    ObjRegistro.CodigoCliente = sl.GetCellValueAsString(row, col).PadLeft(4, '0');
                                    break;
                                case "Cliente":
                                    ObjRegistro.Cliente = sl.GetCellValueAsString(row, col);
                                    break;
                                case "Cred":
                                    ObjRegistro.Credito = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "Id Fac":
                                    ObjRegistro.IdFactura = sl.GetCellValueAsInt32(row, col);
                                    break;
                                case "Factura":
                                    ObjRegistro.NumeroFactura = sl.GetCellValueAsString(row, col);
                                    break;
                                case "Fecha Venta":
                                    ObjRegistro.FechaVenta = sl.GetCellValueAsDateTime(row, col);
                                    break;
                                case "Fecha Factura":
                                    ObjRegistro.FechaFactura = sl.GetCellValueAsDateTime(row, col);
                                    break;
                                case "Fecha Timbrado":
                                    ObjRegistro.FechaTimbrado = sl.GetCellValueAsDateTime(row, col);
                                    break;
                                case "UUID":
                                    ObjRegistro.UUID = sl.GetCellValueAsString(row, col);
                                    break;
                                case "Venta":
                                    ObjRegistro.Venta = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "Tipo Venta":
                                    ObjRegistro.TipoVenta = sl.GetCellValueAsString(row, col);
                                    break;
                                case "Tipo Factura":
                                    ObjRegistro.TipoFactura = sl.GetCellValueAsString(row, col);
                                    break;
                                case "Producto":
                                    ObjRegistro.Producto = sl.GetCellValueAsString(row, col);
                                    break;
                                case "Cantidad":
                                    ObjRegistro.Cantidad = Utilidades.RedondearCantidad(sl.GetCellValueAsDecimal(row, col));
                                    break;
                                case "IVA Factura":
                                    ObjRegistro.IvaFactura = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "Total Factura":
                                    ObjRegistro.TotalFactura = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "SubTotal S/I Venta":
                                    ObjRegistro.SubTotalSinIva = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "SubTotalVenta":
                                    ObjRegistro.SubTotalVenta = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "IVA Venta":
                                    ObjRegistro.IvaVenta = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "IEPS Venta":
                                    ObjRegistro.IepsVenta = sl.GetCellValueAsDecimal(row, col);
                                    break;
                                case "Total Venta":
                                    ObjRegistro.TotalVenta = sl.GetCellValueAsDecimal(row, col);
                                    break;
                            }

                        }

                        LstExcel.Add(ObjRegistro);

                        progreso = (int)Math.Ceiling((double)((row*70)/rowFinal));
                        backgroundWorker1.ReportProgress(progreso);
                    }
                    LstExcel = LstExcel.Where(x => x.Producto != null &&
                        (
                            x.Producto.StartsWith("Magna", StringComparison.OrdinalIgnoreCase) ||
                            x.Producto.StartsWith("Premium", StringComparison.OrdinalIgnoreCase) ||
                            x.Producto.StartsWith("Diésel", StringComparison.OrdinalIgnoreCase) ||
                            x.Producto.StartsWith("Gasolina", StringComparison.OrdinalIgnoreCase) ||
                            x.Producto.StartsWith("Diesel", StringComparison.OrdinalIgnoreCase) ||
                            x.Producto.StartsWith("Combustible", StringComparison.OrdinalIgnoreCase)
                    )
                    ).ToList();

                    progreso = 90;
                    backgroundWorker1.ReportProgress(progreso);


                }
                else
                {
                    Console.WriteLine("Índice de hoja inválido.");
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (argumentoBackground)
            {
                case "nodos":
                    progressBar2.Value = e.ProgressPercentage;
                    break;
                case "importarExcel":
                    progressBar1.Value = e.ProgressPercentage;
                    break;
                case "exportar":
                    progressBar1.Value = e.ProgressPercentage;
                    break;
            }
            
        }

        private void btnComparar_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy) return;
           
            lblEstado.Text = "Comparando archivos...";
            backgroundWorker1.RunWorkerAsync("comparar");
        }

        private void btnXmlJson_Click(object sender, EventArgs e)
        {
            ImportarJson();
        }

        private void ImportarJson()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos JSON (*.json)|*.json|Todos los archivos (*.*)|*.*";
            openFileDialog.Title = "Selecciona un archivo JSON";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string rutaArchivo = openFileDialog.FileName;
                string contenidoJson = File.ReadAllText(rutaArchivo);
                facturaDetalle = JsonConvert.DeserializeObject<clsCfdiMensual>(contenidoJson);

                RecorrerNodosJson();             
          
            }
        }

        private void RecorrerNodosJson()
        {
            lblCarga.Text = @"Leyendo JSON...";
            panelCarga.Visible = true;
            progreso = 0;
            backgroundWorker1.RunWorkerAsync("nodos");
        }

        private void MostrarInventarios()
        {           
            var productosOrdenados = facturaDetalle.Producto.OrderByDescending(x => x.ClaveProducto).ToList();
            LstCfdisErrores = new List<KeyValuePair<string, string>>();
            LstProductos = new List<clsProductoInfo>();

            for (int i = 0; i < productosOrdenados.Count; i++)
            {
                var pro = productosOrdenados[i];
                var productoInfo = new clsProductoInfo
                {
                    ClaveProducto = pro.ClaveProducto,
                    ClaveSubProducto = pro.ClaveSubProducto,
                    VolumenExistenciasMes = pro.ReporteDeVolumenMensual.ControlDeExistencias.VolumenExistenciasMes,
                    TotalRecepcionesMes = pro.ReporteDeVolumenMensual.Recepciones.TotalRecepcionesMes,
                    SumaVolumenRecepcionMes = pro.ReporteDeVolumenMensual.Recepciones.SumaVolumenRecepcionMes.ValorNumerico,
                    SumaVolumenEntregadoMes = pro.ReporteDeVolumenMensual.Entregas.SumaVolumenEntregadoMes.ValorNumerico
                };

                if (pro.ReporteDeVolumenMensual.Recepciones.Complemento != null)
                {
                    int numeral = 1;
                    foreach (var comp in pro.ReporteDeVolumenMensual.Recepciones.Complemento)
                    {
                        if (comp.Nacional == null) continue;

                        foreach (var nac in comp.Nacional)
                        {
                            foreach (var part in nac.CFDIs)
                            {
                                if (part.Cfdi.Length != 36)
                                {
                                    LstCfdisErrores.Add(new KeyValuePair<string, string>(
                                        part.Cfdi,
                                        $"Error de longitud: {part.Cfdi.Length:N0}"));
                                }

                                productoInfo.Partidas.Add(new clsPartidaInfo
                                {
                                    Numero = numeral++,
                                    NombreClienteProveedor = nac.NombreClienteOProveedor,
                                    RfcClienteProveedor = nac.RfcClienteOProveedor,
                                    Cfdi = part.Cfdi,
                                    LongitudCfdi = part.Cfdi.Length,
                                    FechaYHoraTransaccion = part.FechaYHoraTransaccion,
                                    PrecioCompra = part.PrecioCompra,
                                    PrecioVentaPublico = part.PrecioDeVentaAlPublico,
                                    VolumenDocumentado = part.VolumenDocumentado.ValorNumerico
                                });
                            }
                        }
                    }
                }
                LstProductos.Add(productoInfo);
                progreso += ((int)Math.Round((double)((i+1 * 45) / productosOrdenados.Count)));
                backgroundWorker1.ReportProgress(progreso);

            }

        }

        private void MostrarEnPantalla()
        {         

            if (ListaJson.Count >= 1)
            {
                dgvRegistrosDiario.DataSource = ListaJson;
                tsTotalRegistros.Text = ListaJson.Count.ToString("N0");
            }

            

            if (LstProductos.Count > 0)
            {
                int posicionDgvInventariosY = 0;
                foreach (var producto in LstProductos.OrderByDescending(x => x.ClaveProducto).ToList())
                {
                    DataGridView dgvEncabezado = new DataGridView();

                    dgvEncabezado.Columns.Add("Texto", "");
                    dgvEncabezado.Columns.Add("Valor", "");

                    dgvEncabezado.Rows.Add("Producto:", $"{producto.ClaveSubProducto} {producto.ClaveProducto}");
                    dgvEncabezado.Rows.Add("INVENTARIO EN TANQUE AL FINALIZAR EL MES:", producto.VolumenExistenciasMes.ToString("N2"));
                    dgvEncabezado.Rows.Add("NÚMERO DE VECES QUE ENTRO PRODUCTO AL TANQUE:", producto.TotalRecepcionesMes.ToString("N0"));
                    dgvEncabezado.Rows.Add("TOTAL DE LITROS QUE MUESTRA LA FACTURA:", producto.SumaVolumenRecepcionMes.ToString("N2"));
                    panelInventarios.Controls.Add(dgvEncabezado);
                    dgvEncabezado.Location = new System.Drawing.Point(10, posicionDgvInventariosY + 30);
                    dgvEncabezado.Width = 1107;
                    posicionDgvInventariosY = dgvEncabezado.Location.Y + dgvEncabezado.Size.Height + 15;
                    dgvEncabezado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)))));
                    dgvEncabezado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

                    panelInventarios.Controls.Add(dgvPartidas);
                    dgvPartidas.Location = new System.Drawing.Point(10, posicionDgvInventariosY + 30);
                    dgvPartidas.Width = 1107;
                    dgvPartidas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top)))));
                    dgvPartidas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    decimal sumaRecepciones = 0;
                    foreach (var partida in producto.Partidas)
                    {
                        dgvPartidas.Rows.Add(
                            partida.Numero,
                            partida.NombreClienteProveedor,
                            partida.RfcClienteProveedor,
                            partida.Cfdi,
                            partida.LongitudCfdi,
                            partida.FechaYHoraTransaccion,
                            partida.PrecioCompra,
                            partida.PrecioVentaPublico,
                            partida.VolumenDocumentado);

                        sumaRecepciones += partida.VolumenDocumentado;
                    }

                    // Agregar filas resumen
                    decimal diferencia = producto.SumaVolumenRecepcionMes - sumaRecepciones;
                    dgvPartidas.Rows.Add(null, null, null, null, null, null, "TOTAL:", sumaRecepciones);
                    dgvPartidas.Rows.Add(null, null, null, "VENTA LTS. POR MES:", producto.SumaVolumenEntregadoMes, null, null, null);
                    dgvPartidas.Rows.Add(null, null, null, "DIF- FACT. VS PIPAS:", diferencia, null, null, null);
                    dgvPartidas.Rows.Add(null, null, null, "LA FACTURA TRAE", diferencia >= 0 ? " MÁS" : " MENOS");
                    posicionDgvInventariosY = dgvPartidas.Location.Y + dgvPartidas.Size.Height + 15;

                    txtVersion.Text = facturaDetalle.Version.ToString("N2");
                    txtRfcRepresentante.Text = facturaDetalle.RfcRepresentanteLegal;
                    txtRfcProveedor.Text = facturaDetalle.RfcProveedor;
                    txtRfcContrib.Text = facturaDetalle.RfcContribuyente;
                    txtNoPermiso.Text = facturaDetalle.NumPermiso;
                    txtSucursal.Text = Utilidades.CatalogSucursales().First(x => x.Value == facturaDetalle.NumPermiso).Key;
                    txtCaracter.Text = facturaDetalle.Caracter;
                    txtModPermiso.Text = facturaDetalle.ModalidadPermiso;
                    txtPeriodo.Text = facturaDetalle.FechaYHoraReporteMes.ToString();
                }

                lblCarga.Text = @"¡Listo!";
                MessageBox.Show("Archivo JSON cargado y deserializado correctamente.");
               

            }       

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (argumentoBackground)
            {
                case "nodos":
                    lblCarga.Text = @"Mostrando en pantalla...";
                    MostrarEnPantalla();
                    break;

                case "importarExcel":
                    if (LstExcel.Count >= 1)
                    {
                        lblCarga.Text = @"Mostrando en pantalla...";
                        dgvManuales.DataSource = LstExcel;
                        tsManuales.Text = LstExcel.Count.ToString("N0");
                        MessageBox.Show("¡Información lista!.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;

                case "comparar":

                    if (!chkTodo.Checked)
                    {
                        LstResultados = LstResultados.Where(x => x.DiferenciaCantidades >= 0.01M).ToList();
                    }
                    progressBar1.Value = 95;

                    dgvErrores.DataSource = LstResultados.OrderBy(x => x.NombreCliente).ThenBy(x => x.UUID).ThenBy(x => x.Cant).ToList(); ;
                    tsErrores.Text = dgvErrores.RowCount.ToString();
                    progressBar1.Value = 100;
                    lblEstado.Text = "¡Listo!";
                    MessageBox.Show("Comparación realizada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    progressBar1.Value = 0;
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
            panelCargaArchivo .Visible = false;


        }

        private void formMensualJson_Load(object sender, EventArgs e)
        {
            Inicializar();
        }

        private void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            ExportarExcel();
        }

        private void ExportarExcel()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Rev Inf Arch Mensual";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                nombreExcel = dlg.FileName + ".xlsx";
                lblEstado.Text = "Exportando registros.";
                panelCargaArchivo.Visible = true;
                backgroundWorker1.RunWorkerAsync("exportar");
            }
        }
    }
}
