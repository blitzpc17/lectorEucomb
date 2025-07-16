using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace entidades.JSON
{
    public class clsMensual
    {
        public String RfcClienteOProveedor { get; set; }
        public String NombreClienteOProveedor { set; get; }
        public String CFDI { get; set; }
        public DateTime FechaYHoraTransaccion { get; set; }
        public Decimal ValorNumerico { get; set; }
    }
}
