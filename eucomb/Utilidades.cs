using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eucomb
{
    public static class Utilidades
    {
        public static void FastExportToExcel<T>(List<T> data, string filePath)
        {
            using (SLDocument sl = new SLDocument())
            {
                // Obtener las propiedades de la clase T
                PropertyInfo[] properties = typeof(T).GetProperties();

                // Escribir encabezados en la primera fila
                for (int i = 0; i < properties.Length; i++)
                {
                    sl.SetCellValue(1, i + 1, properties[i].Name);
                }

                // Escribir datos en las filas siguientes
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < properties.Length; j++)
                    {
                        object value = properties[j].GetValue(data[i], null);
                        sl.SetCellValue(i + 2, j + 1, value?.ToString());
                    }
                }

                // Guardar el archivo en la ruta especificada
                sl.SaveAs(filePath);
            }
        }

        public static decimal RedondearCantidad(decimal valor)
        {
            decimal valorAjustado = Math.Round(valor, 4, MidpointRounding.AwayFromZero);
            decimal cuartoDecimal = (valorAjustado * 10000) % 10;

            if (cuartoDecimal >= 6)
            {
                return Math.Round(valor, 3, MidpointRounding.AwayFromZero);
            }
            else
            {
                return Math.Floor(valor * 1000) / 1000;
            }
        }

        public static DateTime CambiarFormatoFecha(DateTime fechaInglesa)
        {
            try
            {
                string fechaOriginal = fechaInglesa.ToString("MM/dd/yyyy hh:mm:ss tt", new CultureInfo("es-ES"));
                DateTime fecha = DateTime.ParseExact(fechaOriginal, "dd/MM/yyyy hh:mm:ss tt", new CultureInfo("es-ES"));

                return fecha;
            }
            catch (Exception ex)
            {
                string fechaOriginal = fechaInglesa.ToString("dd/MM/yyyy hh:mm:ss tt", new CultureInfo("es-ES"));
                DateTime fecha = DateTime.ParseExact(fechaOriginal, "dd/MM/yyyy hh:mm:ss tt", new CultureInfo("es-ES"));
                return fecha;
            }

        }

        public static List<KeyValuePair<String, String>> CatalogSucursales()
        {
            List<KeyValuePair<String, String>> LstSucursales = new List<KeyValuePair<string, string>>();

            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Suc. Doncellas)", "PL/2916/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS DE CORDOBA, S.A. DE C.V.", "PL/3022/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS DE CORDOBA, S.A. DE C.V.", "PL/3744/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Suc. Tehuacán)", "PL/3267/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Matríz)", "PL/3449/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("GASOLINERIA ELE, S.A. DE C.V.", "PL/3664/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("COMBUSTIBLES Y SERVICIOS ESMERALDA, S.A. DE C.V.", "PL/3219/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO CUAUTLAPAN, S.A. DE C.V. (Suc. Chapulco)", "PL/3456/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS SOLE, S.A. DE C.V.", "PL/3461/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS SANTA MARIA DEL MONTE, S.A. DE C.V.", "PL/3656/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("ENERGETICOS AHUACATLAN, S.A. DE C.V.", "PL/2614/EXP/ES/2015"));
            LstSucursales.Add(new KeyValuePair<String, String>("GASOLINERA ZAVALETA, S.A. DE C.V.", "PL/19419/EXP/ES/2016"));
            LstSucursales.Add(new KeyValuePair<String, String>("SERVICIO ALFA BRAVO COCA, SA DE CV", "PL/21370/EXP/ES/2018"));
            LstSucursales.Add(new KeyValuePair<String, String>("LITRO EXACTO OCOTLAN, S. DE R.L. DE C.V.", "PL/23036/EXP/ES/2019"));
            LstSucursales.Add(new KeyValuePair<string, string>("OCOTLAN DE MORELOS OAXACA", "PL/25070/EXP/ES/2023"));
            return LstSucursales;
        }


    }
}
