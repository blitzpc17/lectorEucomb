using entidades.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace entidades.XML.Diario
{
    [XmlRoot(ElementName = "ControlesVolumetricos", Namespace = "https://repositorio.cloudb.sat.gob.mx/Covol/xml/Diarios")]
    public class clsCdfiDiario
    {
        [XmlElement("Version")]
        public string Version { get; set; }

        [XmlElement("RfcContribuyente")]
        public string RfcContribuyente { get; set; }

        [XmlElement("RfcRepresentanteLegal")]
        public string RfcRepresentanteLegal { get; set; }

        [XmlElement("RfcProveedor")]
        public string RfcProveedor { get; set; }

        [XmlElement("Caracter")]
        public Caracter Caracter { get; set; }

        [XmlElement("ClaveInstalacion")]
        public string ClaveInstalacion { get; set; }

        [XmlElement("DescripcionInstalacion")]
        public string DescripcionInstalacion { get; set; }

        [XmlElement("NumeroPozos")]
        public int NumeroPozos { get; set; }

        [XmlElement("NumeroTanques")]
        public int NumeroTanques { get; set; }

        [XmlElement("NumeroDuctosEntradaSalida")]
        public int NumeroDuctosEntradaSalida { get; set; }

        [XmlElement("NumeroDuctosTransporteDistribucion")]
        public int NumeroDuctosTransporteDistribucion { get; set; }

        [XmlElement("NumeroDispensarios")]
        public int NumeroDispensarios { get; set; }

        [XmlElement("FechaYHoraCorte")]
        public DateTime FechaYHoraCorte { get; set; }

        [XmlElement("PRODUCTO")]
        public List<Producto> Productos { get; set; }
    }

    public class Caracter
    {
        [XmlElement("TipoCaracter")]
        public string TipoCaracter { get; set; }

        [XmlElement("ModalidadPermiso")]
        public string ModalidadPermiso { get; set; }

        [XmlElement("NumPermiso")]
        public string NumPermiso { get; set; }
    }

    public class Producto
    {
        [XmlElement("ClaveProducto")]
        public string ClaveProducto { get; set; }

        [XmlElement("ClaveSubProducto")]
        public string ClaveSubProducto { get; set; }

        [XmlElement("Gasolina")]
        public Gasolina Gasolina { get; set; }

        [XmlElement("MarcaComercial")]
        public string MarcaComercial { get; set; }

        [XmlElement("TANQUE")]
        public List<Tanque> Tanques { get; set; }

        [XmlElement("DISPENSARIO")]
        public List<Dispensario> Dispensarios { get; set; }
    }

    public class Gasolina
    {
        [XmlElement("ComposOctanajeGasolina")]
        public int ComposOctanajeGasolina { get; set; }

        [XmlElement("GasolinaConCombustibleNoFosil")]
        public string GasolinaConCombustibleNoFosil { get; set; }
    }

    public class Tanque
    {
        [XmlElement("ClaveIdentificacionTanque")]
        public string ClaveIdentificacionTanque { get; set; }

        [XmlElement("LocalizacionYODescripcionTanque")]
        public string LocalizacionYODescripcionTanque { get; set; }

        [XmlElement("VigenciaCalibracionTanque")]
        public string VigenciaCalibracionTanque { get; set; }

        [XmlElement("CapacidadTotalTanque")]
        public Capacidad CapacidadTotalTanque { get; set; }

        [XmlElement("CapacidadOperativaTanque")]
        public Capacidad CapacidadOperativaTanque { get; set; }

        [XmlElement("CapacidadUtilTanque")]
        public Capacidad CapacidadUtilTanque { get; set; }

        [XmlElement("VolumenMinimoOperacion")]
        public Capacidad VolumenMinimoOperacion { get; set; }

        [XmlElement("EstadoTanque")]
        public string EstadoTanque { get; set; }

        [XmlElement("MedicionTanque")]
        public MedicionTanque MedicionTanque { get; set; }

        [XmlElement("EXISTENCIAS")]
        public Existencias Existencias { get; set; }

        [XmlElement("RECEPCIONES")]
        public Recepciones Recepciones { get; set; }

        [XmlElement("ENTREGAS")]
        public Entregas Entregas { get; set; }
    }

    public class Capacidad
    {
        [XmlElement("ValorNumerico")]
        public decimal ValorNumerico { get; set; }

        [XmlElement("UM")]
        public string UM { get; set; }
    }

    public class MedicionTanque
    {
        [XmlElement("SistemaMedicionTanque")]
        public string SistemaMedicionTanque { get; set; }

        [XmlElement("LocalizODescripSistMedicionTanque")]
        public string LocalizODescripSistMedicionTanque { get; set; }

        [XmlElement("VigenciaCalibracionSistMedicionTanque")]
        public string VigenciaCalibracionSistMedicionTanque { get; set; }

        [XmlElement("IncertidumbreMedicionSistMedicionTanque")]
        public decimal IncertidumbreMedicionSistMedicionTanque { get; set; }
    }

    public class Existencias
    {
        [XmlElement("VolumenExistenciasAnterior")]
        public Volumen VolumenExistenciasAnterior { get; set; }

        [XmlElement("VolumenAcumOpsRecepcion")]
        public Capacidad VolumenAcumOpsRecepcion { get; set; }

        [XmlElement("HoraRecepcionAcumulado")]
        public string HoraRecepcionAcumulado { get; set; }

        [XmlElement("VolumenAcumOpsEntrega")]
        public Capacidad VolumenAcumOpsEntrega { get; set; }

        [XmlElement("HoraEntregaAcumulado")]
        public string HoraEntregaAcumulado { get; set; }

        [XmlElement("VolumenExistencias")]
        public Volumen VolumenExistencias { get; set; }

        [XmlElement("FechaYHoraEstaMedicion")]
        public DateTime FechaYHoraEstaMedicion { get; set; }

        [XmlElement("FechaYHoraMedicionAnterior")]
        public DateTime FechaYHoraMedicionAnterior { get; set; }
    }

    public class Volumen
    {
        [XmlElement("ValorNumerico")]
        public decimal ValorNumerico { get; set; }
    }

    public class Recepciones
    {
        [XmlElement("TotalRecepciones")]
        public int TotalRecepciones { get; set; }

        [XmlElement("SumaVolumenRecepcion")]
        public Capacidad SumaVolumenRecepcion { get; set; }

        [XmlElement("TotalDocumentos")]
        public int TotalDocumentos { get; set; }
    }

    public class Entregas
    {
        [XmlElement("TotalEntregas")]
        public int TotalEntregas { get; set; }

        [XmlElement("SumaVolumenEntregado")]
        public Capacidad SumaVolumenEntregado { get; set; }

        [XmlElement("TotalDocumentos")]
        public int TotalDocumentos { get; set; }
    }

    public class Dispensario
    {
        [XmlElement("ClaveDispensario")]
        public string ClaveDispensario { get; set; }

        [XmlElement("MedicionDispensario")]
        public MedicionDispensario MedicionDispensario { get; set; }

        [XmlElement("MANGUERA")]
        public List<Manguera> Mangueras { get; set; }
    }

    public class MedicionDispensario
    {
        [XmlElement("SistemaMedicionDispensario")]
        public string SistemaMedicionDispensario { get; set; }

        [XmlElement("LocalizODescripSistMedicionDispensario")]
        public string LocalizODescripSistMedicionDispensario { get; set; }

        [XmlElement("VigenciaCalibracionSistMedicionDispensario")]
        public string VigenciaCalibracionSistMedicionDispensario { get; set; }

        [XmlElement("IncertidumbreMedicionSistMedicionDispensario")]
        public decimal IncertidumbreMedicionSistMedicionDispensario { get; set; }
    }

    public class Manguera
    {
        [XmlElement("IdentificadorManguera")]
        public string IdentificadorManguera { get; set; }

        [XmlElement("ENTREGAS")]
        public EntregasManguera Entregas { get; set; }
    }
    [XmlRoot(ElementName = "ENTREGAS", Namespace = "https://repositorio.cloudb.sat.gob.mx/Covol/xml/Diarios")]
    public class EntregasManguera
    {
        [XmlElement("TotalEntregas")]
        public int TotalEntregas { get; set; }

        [XmlElement("SumaVolumenEntregado")]
        public Capacidad SumaVolumenEntregado { get; set; }

        [XmlElement("TotalDocumentos")]
        public int TotalDocumentos { get; set; }

        [XmlElement("ENTREGA")]
        public List<Entrega> Entregas { get; set; }
    }
    [XmlRoot(ElementName = "ENTREGA", Namespace = "https://repositorio.cloudb.sat.gob.mx/Covol/xml/Diarios")]
    public class Entrega
    {
        [XmlElement("NumeroDeRegistro")]
        public int NumeroDeRegistro { get; set; }

        [XmlElement("TipoDeRegistro")]
        public string TipoDeRegistro { get; set; }

        [XmlElement("VolumenEntregadoTotalizadorAcum")]
        public Capacidad VolumenEntregadoTotalizadorAcum { get; set; }

        [XmlElement("VolumenEntregadoTotalizadorInsta")]
        public Capacidad VolumenEntregadoTotalizadorInsta { get; set; }

        [XmlElement("FechaYHoraEntrega")]
        public DateTime FechaYHoraEntrega { get; set; }

        [XmlElement("Complemento")]
        public Complemento Complemento { get; set; }
    }

    public class Complemento
    {
        [XmlElement("Complemento_Expendio")]
        public ComplementoExpendio ComplementoExpendio { get; set; }
    }
    [XmlType(Namespace = "Complemento_Expendio")]
    public class ComplementoExpendio
    {
        [XmlElement("NACIONAL", Namespace = "Complemento_Expendio")]
        public Nacional Nacional { get; set; }
    }

    [XmlType(Namespace = "Complemento_Expendio")]
    public class Nacional
    {
        [XmlElement("RfcClienteOProveedor")]
        public string RfcClienteOProveedor { get; set; }

        [XmlElement("NombreClienteOProveedor")]
        public string NombreClienteOProveedor { get; set; }

        [XmlElement("PermisoProveedor")]
        public string PermisoProveedor { get; set; }

        [XmlElement("CFDIs")]
        public CFDIs CFDIs { get; set; }
    }

    public class CFDIs
    {
        [XmlElement("CFDI")]
        public string CFDI { get; set; }

        [XmlElement("TipoCFDI")]
        public string TipoCFDI { get; set; }

        [XmlElement("PrecioCompra")]
        public decimal PrecioCompra { get; set; }

        [XmlElement("PrecioDeVentaAlPublico")]
        public decimal PrecioDeVentaAlPublico { get; set; }

        [XmlElement("PrecioVenta")]
        public decimal PrecioVenta { get; set; }

        [XmlElement("FechaYHoraTransaccion")]
        public DateTime FechaYHoraTransaccion { get; set; }

        [XmlElement("VolumenDocumentado")]
        public VolumenDocumentado VolumenDocumentado { get; set; }
    }

    public class VolumenDocumentado
    {
        [XmlElement("ValorNumerico")]
        public decimal ValorNumerico { get; set; }

        [XmlElement("UM")]
        public string UM { get; set; }
    }


}
