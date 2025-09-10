using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entidades.XML.Diario
{
    public class clsInventarioDiario
    {
        public string Sucursal { get; set; }
        public string Version { get; set; }
        public string RfcContribuyente { get; set; }
        public string RfcProveedor { get; set; }
        public string RfcRepresentante { get; set; }
        public DateTime Periodo { get; set; } // dd/MM/yyyy H:m:s
        public string Caracter { get; set; }
        public string ModPermiso { get; set; }
        public string NoPermiso { get; set; }
        public List<ProductoData> Productos  { get; set; }
    }

    public class ProductoData
    {
        public string ClaveProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal InventarioTanqueFinMes { get; set; }
        public int NumeroEntradasProducto { get; set; }
        public decimal TotalLitrosFactura { get; set; }

        public List<ProductoPartida> Partidas { get; set; }

    }

    public class ProductoPartida
    {
        public string NombreCliente { get; set; }
        public string RfcCliente { get; set; }
        public string Cfdi { get; set; }
        public int LongitudCfdi { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal ValorNumerico { get; set; }

    }



}
