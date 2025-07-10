using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entidades.VentaPorFecha
{
    public class clsReporteVentaPorFecha
    {
        public string Factura { get; set; }
        public DateTime Fecha { get; set; }
        public string Uuid { get; set; }
        public string Clave { get; set; }
        public string Producto { get; set; }
        public decimal Litros { get; set; }
        public decimal Importe { get; set; }
        public string Rfc { get; set; }
        public string CodigoPostal { get; set; }
    }
}
