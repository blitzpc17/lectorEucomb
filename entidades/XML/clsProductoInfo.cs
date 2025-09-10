using entidades.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entidades.XML
{
    public class clsProductoInfo
    {
        public string ClaveProducto { get; set; }
        public string ClaveSubProducto { get; set; }
        public decimal VolumenExistenciasMes { get; set; }
        public int TotalRecepcionesMes { get; set; }
        public decimal SumaVolumenRecepcionMes { get; set; }
        public List<clsPartidaInfo> Partidas { get; set; } = new List<clsPartidaInfo>();
        public decimal SumaVolumenEntregadoMes { get; set; }
    }
}
