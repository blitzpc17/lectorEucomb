using SpreadsheetLight;
using System;
using System.Collections.Generic;
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
    }
}
