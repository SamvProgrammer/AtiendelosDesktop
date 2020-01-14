using AtiendelosDestktop.Resources.codigo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtiendelosDestktop.herramientas
{
    class globales
    {

        internal static string id_usuario;
        internal static string nombre;
        internal static bool esReporte;
        internal static bool sinPagos;
        internal static bool leftButton = false;

        public static dynamic consulta(string consulta, bool tipoSelect = false, bool eliminando = false)
        {
            return baseDatos.consulta(consulta, tipoSelect, eliminando);
        }

        public static void reportes(string nombreReporte, string tablaSetNombre, object[] objeto, string mensaje = "", bool imprimir = false, object[] parametros = null, bool espdf = false, string nombrePdf = "")
        {
            herramientas.reportes(nombreReporte, tablaSetNombre, objeto, mensaje, imprimir, parametros, espdf, nombrePdf);
        }



    }
}
