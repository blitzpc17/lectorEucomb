using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace entidades.ReporteLitros
{
    [XmlRoot(ElementName = "Comprobante", Namespace = "http://www.sat.gob.mx/cfd/4")]
    public class Comprobante
    {
        [XmlAttribute("Version")]
        public string Version { get; set; }

        [XmlAttribute("Serie")]
        public string Serie { get; set; }

        [XmlAttribute("Folio")]
        public string Folio { get; set; }

        [XmlAttribute("Fecha")]
        public DateTime Fecha { get; set; }

        [XmlAttribute("Sello")]
        public string Sello { get; set; }

        [XmlAttribute("FormaPago")]
        public string FormaPago { get; set; }

        [XmlAttribute("NoCertificado")]
        public string NoCertificado { get; set; }

        [XmlAttribute("Certificado")]
        public string Certificado { get; set; }

        [XmlAttribute("CondicionesDePago")]
        public string CondicionesDePago { get; set; }

        [XmlAttribute("SubTotal")]
        public decimal SubTotal { get; set; }

        [XmlAttribute("Moneda")]
        public string Moneda { get; set; }

        [XmlAttribute("TipoCambio")]
        public decimal TipoCambio { get; set; }

        [XmlAttribute("Total")]
        public decimal Total { get; set; }

        [XmlAttribute("TipoDeComprobante")]
        public string TipoDeComprobante { get; set; }

        [XmlAttribute("Exportacion")]
        public string Exportacion { get; set; }

        [XmlAttribute("MetodoPago")]
        public string MetodoPago { get; set; }

        [XmlAttribute("LugarExpedicion")]
        public string LugarExpedicion { get; set; }

        [XmlElement("Emisor")]
        public Emisor Emisor { get; set; }

        [XmlElement("Receptor")]
        public Receptor Receptor { get; set; }

        [XmlArray("Conceptos")]
        [XmlArrayItem("Concepto")]
        public List<Concepto> Conceptos { get; set; }

        [XmlElement("Impuestos")]
        public Impuestos Impuestos { get; set; }

        [XmlElement("Complemento")]
        public Complemento Complemento { get; set; }




    }



    public class Emisor
    {
        [XmlAttribute("Rfc")]
        public string Rfc { get; set; }

        [XmlAttribute("Nombre")]
        public string Nombre { get; set; }

        [XmlAttribute("RegimenFiscal")]
        public string RegimenFiscal { get; set; }
    }

    public class Receptor
    {
        [XmlAttribute("Rfc")]
        public string Rfc { get; set; }

        [XmlAttribute("Nombre")]
        public string Nombre { get; set; }

        [XmlAttribute("DomicilioFiscalReceptor")]
        public string DomicilioFiscalReceptor { get; set; }

        [XmlAttribute("RegimenFiscalReceptor")]
        public string RegimenFiscalReceptor { get; set; }

        [XmlAttribute("UsoCFDI")]
        public string UsoCFDI { get; set; }
    }

    public class Concepto
    {
        [XmlAttribute("ClaveProdServ")]
        public string ClaveProdServ { get; set; }

        [XmlAttribute("NoIdentificacion")]
        public string NoIdentificacion { get; set; }

        [XmlAttribute("Cantidad")]
        public decimal Cantidad { get; set; }

        [XmlAttribute("ClaveUnidad")]
        public string ClaveUnidad { get; set; }

        [XmlAttribute("Unidad")]
        public string Unidad { get; set; }

        [XmlAttribute("Descripcion")]
        public string Descripcion { get; set; }

        [XmlAttribute("ValorUnitario")]
        public decimal ValorUnitario { get; set; }

        [XmlAttribute("Importe")]
        public decimal Importe { get; set; }

        [XmlAttribute("ObjetoImp")]
        public string ObjetoImp { get; set; }

        [XmlElement("Impuestos")]
        public ConceptoImpuestos Impuestos { get; set; }
    }

    public class ConceptoImpuestos
    {
        [XmlArray("Traslados")]
        [XmlArrayItem("Traslado")]
        public List<Traslado> Traslados { get; set; }
    }

    public class Impuestos
    {
        [XmlAttribute("TotalImpuestosTrasladados")]
        public decimal TotalImpuestosTrasladados { get; set; }

        [XmlArray("Traslados")]
        [XmlArrayItem("Traslado")]
        public List<Traslado> Traslados { get; set; }
    }

    public class Traslado
    {
        [XmlAttribute("Base")]
        public decimal Base { get; set; }

        [XmlAttribute("Impuesto")]
        public string Impuesto { get; set; }

        [XmlAttribute("TipoFactor")]
        public string TipoFactor { get; set; }

        [XmlAttribute("TasaOCuota")]
        public decimal TasaOCuota { get; set; }

        [XmlAttribute("Importe")]
        public decimal Importe { get; set; }
    }

    public class Complemento
    {
        [XmlElement("TimbreFiscalDigital", Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
        public TimbreFiscalDigital TimbreFiscalDigital { get; set; }
    }

    public class TimbreFiscalDigital
    {
        [XmlAttribute("Version")]
        public string Version { get; set; }

        [XmlAttribute("UUID")]
        public string UUID { get; set; }

        [XmlAttribute("FechaTimbrado")]
        public DateTime FechaTimbrado { get; set; }

        [XmlAttribute("RfcProvCertif")]
        public string RfcProvCertif { get; set; }

        [XmlAttribute("SelloCFD")]
        public string SelloCFD { get; set; }

        [XmlAttribute("NoCertificadoSAT")]
        public string NoCertificadoSAT { get; set; }

        [XmlAttribute("SelloSAT")]
        public string SelloSAT { get; set; }
    }









}
