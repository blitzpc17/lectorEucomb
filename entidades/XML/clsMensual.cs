using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entidades.XML
{
    public class clsMensual
    {
        public string RfcClienteOProveedor { get; set; }
        public string NombreClienteOProveedor { set; get; }
        public string CFDI { get; set; }
        public DateTime FechaYHoraTransaccion { get; set; }
        public Decimal ValorNumerico { get; set; }
    }
}
