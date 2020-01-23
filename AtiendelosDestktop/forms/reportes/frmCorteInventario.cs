using AtiendelosDestktop.herramientas;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtiendelosDestktop.forms.reportes
{


    public partial class frmCorteInventario : Form
    {


        string id_empresaPrincipal;
        List<Dictionary<string, object>> lista;
        List<Dictionary<string, object>> bodegaslista;
        string id_bodega;

        string id_sucursal;

        public frmCorteInventario(string id_empresa)
        {
            InitializeComponent();
            this.id_empresaPrincipal = id_empresa;
        }

        private void Ventas_Load(object sender, EventArgs e)
        {
            string query = $"SELECT nombre,id FROM sucursales WHERE id_empresa={this.id_empresaPrincipal}";
            this.lista = globales.consulta(query);
            foreach (var item in lista)
            {
                string nombre = Convert.ToString(item["nombre"]);
                string id = Convert.ToString(item["id"]);
                comboBox1.Items.Add(nombre);
            }
            comboBox1.SelectedIndex = 0;

            string bodegas = $"SELECT nombre_bodega, id_bodega FROM bodegas where id_sucursal={this.id_sucursal} and id_empresa={this.id_empresaPrincipal} ;";
            this.bodegaslista = globales.consulta(bodegas);
            foreach(var che in bodegaslista)
            {
                string nombre = Convert.ToString(che["nombre_bodega"]);
                comboBox2.Items.Add(nombre);
            }
            comboBox2.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {




        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string query = "SELECT DISTINCT(id_inventario) , descripcion, unidad_medida FROM inventario;";
            List<Dictionary<string, object>> resultado = globales.consulta(query);

            object[] aux1 = new object[resultado.Count];
            int contador1 = 0;

            foreach (var item in resultado)
            {
                string id_inventario = Convert.ToString(item["id_inventario"]);
                string descripcion = Convert.ToString(item["descripcion"]);
                string unidad_medida = Convert.ToString(item["unidad_medida"]);

                string corte = $"SELECT SUM (cantidad) AS cantidad  FROM ( SELECT COALESCE (SUM(cantidad), 0) AS cantidad	FROM control_movimientos WHERE	tipo_mov IN ('E', 'OC') AND id_inventario = {id_inventario} AND id_sucursal = {this.id_sucursal} UNION SELECT((COALESCE(SUM(cantidad), 0))*- 1) AS cantidad	FROM control_movimientos WHERE	tipo_mov IN ('S', 'V')AND id_inventario = {id_inventario} AND id_sucursal ={this.id_sucursal} AND id_empresa={this.id_empresaPrincipal} and ubicacion={this.id_bodega}) AS A1;";
                List<Dictionary<string, object>> res = globales.consulta(corte);
                double total = 0.00;
                if (res.Count>=0)
                {

                    continue;
                }
                else
                {
                     total = Convert.ToDouble(res[0]["cantidad"]);
                }

                object[] tt1 = { id_inventario, descripcion, comboBox2.Text, total };

                aux1[contador1] = tt1;
                contador1++;
            }

            object[] parametros = { "sucursal" };
            object[] valor = { comboBox1.Text};
            object[][] enviarParametros = new object[2][];

            enviarParametros[0] = parametros;
            enviarParametros[1] = valor;





            ReportViewer reporte = globales.reportesParaPanel("ventas", "ventas", aux1, "", false, enviarParametros);
            reporte.Dock = DockStyle.Fill;
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(reporte);


            this.Cursor = Cursors.Default;
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            foreach (var item in this.lista)
            {
                string nombre = Convert.ToString(item["nombre"]);

                if (nombre == comboBox1.Text)
                {
                    this.id_sucursal = Convert.ToString(item["id"]);
                    continue;

                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var item in this.bodegaslista)
            {
                string nombre = Convert.ToString(item["nombre_bodega"]);

                if (nombre == comboBox2.Text)
                {
                    this.id_bodega = Convert.ToString(item["id_bodega"]);
                    continue;

                }
            }
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            string query = $"SELECT DISTINCT(id_inventario) , descripcion, unidad_medida FROM inventario where id_empresa ={this.id_empresaPrincipal} ";
            List<Dictionary<string, object>> resultado = globales.consulta(query);

            object[] aux1 = new object[resultado.Count];
            int contador1 = 0;

            foreach (var item in resultado)
            {
                string id_inventario = Convert.ToString(item["id_inventario"]);
                string descripcion = Convert.ToString(item["descripcion"]);
                string unidad_medida = Convert.ToString(item["unidad_medida"]);

                string corte = $"SELECT SUM (cantidad) AS cantidad  FROM ( SELECT COALESCE (SUM(cantidad), 0) AS cantidad	FROM control_movimientos WHERE	tipo_mov IN ('E', 'OC') AND id_inventario = {id_inventario} AND id_sucursal = {this.id_sucursal}  and ubicacion='{this.id_bodega}' UNION SELECT((COALESCE(SUM(cantidad), 0))*- 1) AS cantidad	FROM control_movimientos WHERE	tipo_mov IN ('S', 'V')AND id_inventario = {id_inventario} AND id_sucursal ={this.id_sucursal} AND id_empresa={this.id_empresaPrincipal} and ubicacion='{this.id_bodega}') AS A1;";
                List<Dictionary<string, object>> corte1 = globales.consulta(corte);
                double total = 0.0;
                if (corte1.Count <= 0)
                {

                    continue;
                }
                else
                {
                    total = Convert.ToDouble(corte1[0]["cantidad"]);
                }

                object[] tt1 = { id_inventario, descripcion, comboBox2.Text, total, unidad_medida };

                aux1[contador1] = tt1;
                contador1++;
            }

            object[] parametros = { "sucursal" , "titulo" };
            object[] valor = { comboBox1.Text , "REPORTE FÍSICO DE INVENTARIO"};
            object[][] enviarParametros = new object[2][];

            enviarParametros[0] = parametros;
            enviarParametros[1] = valor;





            ReportViewer reporte = globales.reportesParaPanel("corteInventario", "corte_inv", aux1, "", false, enviarParametros);
            reporte.Dock = DockStyle.Fill;
            this.panel2.Controls.Clear();
            this.panel2.Controls.Add(reporte);


            this.Cursor = Cursors.Default;
        }
    }
}
