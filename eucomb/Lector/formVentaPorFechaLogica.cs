using entidades.ReporteLitros;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace eucomb.Lector
{
    public class formVentaPorFechaLogica
    {


        public Comprobante DeserializarCFDI(string xmlFilePath)
        {
            try
            {
                // Configurar el serializador con los namespaces necesarios
                var serializer = new XmlSerializer(typeof(Comprobante));
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("cfdi", "http://www.sat.gob.mx/cfd/4");
                namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                namespaces.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                // Configuración adicional para manejar namespaces
                var settings = new XmlReaderSettings
                {
                    IgnoreWhitespace = true,
                    IgnoreComments = true
                };

                using (var reader = XmlReader.Create(xmlFilePath, settings))
                {
                    return (Comprobante)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al deserializar el CFDI: " + ex.Message, ex);
            }
        }

    }
}
