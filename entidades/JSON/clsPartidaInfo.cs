using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entidades.JSON
{
    public class clsPartidaInfo
    {
        public int Numero { get; set; }
        public string NombreClienteProveedor { get; set; }
        public string RfcClienteProveedor { get; set; }
        public string Cfdi { get; set; }
        public int LongitudCfdi { get; set; }
        public DateTime FechaYHoraTransaccion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVentaPublico { get; set; }
        public decimal VolumenDocumentado { get; set; }
    }
}
