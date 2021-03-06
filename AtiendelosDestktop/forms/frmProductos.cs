﻿using AtiendelosDestktop.herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtiendelosDestktop.forms
{
    public partial class frmProductos : Form
    {
        string id_empresaPrincipal;
        int r;
        int c;
        private bool teclaEnter;
        string id_subcategoria;
        string id_categ;
        string subc;
        string id_combocateg;


        public frmProductos(string id_empresa)
        {
            InitializeComponent();
            this.id_empresaPrincipal = id_empresa;

        }

        private void bunifuiOSSwitch1_OnValueChange(object sender, EventArgs e)
        {

        }

        private void frmProductos_Shown(object sender, EventArgs e)
        {
            rellenaProductos();
        }

        public void rellenaProductos()
        {
            string query = $"select * from productos where id_empresa={this.id_empresaPrincipal} order by nombre ";
            List<Dictionary<string, object>> resultado = globales.consulta(query);
            if (resultado.Count<=0)
            {
                DialogResult dialogo = globales.MessageBoxExclamation("NO EXISTEN PRODUCTOS REGISTRADOS", "AVISO", globales.menuPrincipal);
                return;
            }

            dataProductos.Rows.Clear();
            foreach(var item in resultado)
            {
                string nombre = Convert.ToString(item["nombre"]);
                string id_producto = Convert.ToString(item["id_producto"]);
                dataProductos.Rows.Add(nombre, id_producto);
            }

        }

        private void dataProductos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            r = e.RowIndex;
            if (r == -1) return;
            DataGridViewRow row = dataProductos.Rows[r];
            c = e.ColumnIndex;
        }

        private void dataProductos_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (this.teclaEnter)
            {
                var x = this.r + 1;
                var y = dataProductos.Rows.Count;
                if (x != y)
                    SendKeys.Send("{UP}");
                SendKeys.Send("{TAB}");

                this.teclaEnter = false;
            }
        }

        private void viendoEdicion(object sender, PreviewKeyDownEventArgs e)
        {
            this.teclaEnter = e.KeyCode == Keys.Enter;
        }

        private void dataProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          string id = Convert.ToString(dataProductos.Rows[r].Cells[1].Value);
            rellenaProductos(id);
            btnOk.Text = "ACTUALIZAR";
        }

        private void rellenaProductos (string id_obtenido)
        {
            rbCocina.Checked = false;
            rbBarra.Checked=false;

            string query = $"select * from productos where id_producto={id_obtenido}";
            List<Dictionary<string, object>> resultado = globales.consulta(query);
            txtNombre.Text = Convert.ToString(resultado[0]["nombre"]);
            int cost = Convert.ToInt32(resultado[0]["precio"]);
            txtCosto.Text = cost.ToString("C");
            string id_categoria = Convert.ToString(resultado[0]["id_categoria"]);
            this. id_subcategoria = Convert.ToString(resultado[0]["subcategoria"]);
            rellenaCategs(id_categoria, id_subcategoria);
            string tipo = Convert.ToString(resultado[0]["notificacion"]);
            if (tipo == "1") rbBarra.Checked = true;
            if (tipo == "2") rbCocina.Checked = true;
            


        }

        private  void rellenaCategs( string id_categoria, string id_subcategoria)
        {
            ComboCateg.Clear();
            ComboSubcateg.Clear();
            string query = $"SELECT nombre FROM categoria where id={id_categoria};";
            if (string.IsNullOrWhiteSpace(id_categoria)) return;
            List<Dictionary<string, object>> res = globales.consulta(query);

                string nombre = Convert.ToString(res[0]["nombre"]);
            ComboCateg.AddItem(nombre);
            ComboCateg.selectedIndex=0;

            string subcateg = $"SELECT nombre FROM subcategoria where id ={id_subcategoria}";
            if (String.IsNullOrWhiteSpace(id_subcategoria)) return;
            List<Dictionary<string, object>> sus = globales.consulta(subcateg);
            if (sus.Count <= 0) return;
            string sub = Convert.ToString(sus[0]["nombre"]);
            ComboSubcateg.AddItem(sub);
            ComboSubcateg.selectedIndex = 0;
        }

        private void bunifuTextbox1_OnTextChange(object sender, EventArgs e)
        {
            string query = $"SELECT * FROM productos where nombre like '{txtBusqueda.text}%' AND id_empresa={this.id_empresaPrincipal};";
            List<Dictionary<string, object>> busqueda = globales.consulta(query);
            dataProductos.Rows.Clear();
            foreach (var item in busqueda)
            {
                string nombre = Convert.ToString(item["nombre"]);
                string id_producto = Convert.ToString(item["id_producto"]);
                dataProductos.Rows.Add(nombre, id_producto);
            }
        }

        private void ComboCateg_onItemSelected(object sender, EventArgs e)
        {
        }

        private void ComboCateg_Click(object sender, EventArgs e)
        {
        }

        private void ComboCateg_Enter(object sender, EventArgs e)
        {
            string query = $"select * from categoria where id_empresa={this.id_empresaPrincipal}";
            List<Dictionary<string, object>> resultado = globales.consulta(query);
            foreach(var item in resultado)
            {
                string nombre = Convert.ToString(item["nombre"]);
               string id_combocateg = Convert.ToString(item["id"]);
                ComboCateg.AddItem(nombre);
            }

        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            ComboCateg.Clear();
            ComboSubcateg.Clear();
            txtCosto.Text = "";
            rbCocina.Checked = false;
            rbBarra.Checked = false;
            btnOk.Text = "INSERTAR";
        }

        private void ComboCateg_Leave(object sender, EventArgs e)
        {

            string subcateg = $"SELECT id FROM categoria where nombre ='{ComboCateg.selectedValue}'";
            if (String.IsNullOrWhiteSpace(ComboCateg.selectedValue)) return;
            List<Dictionary<string, object>> sus = globales.consulta(subcateg);
            
                string id = Convert.ToString(sus[0]["id"]);

            string busca = $"select nombre from subcategoria where id_categoria={id}";
            List<Dictionary<string, object>> hecha = globales.consulta(busca);
            if (hecha.Count>=1)
            {
                foreach (var item in hecha)
                {
                    string nombre = Convert.ToString(item["nombre"]);
                    ComboSubcateg.AddItem(nombre);
                }
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
             id_categ = string.Empty;
            string categ = $"select id from categoria where nombre='{ComboCateg.selectedValue}' ";
            List<Dictionary<string, object>> res = globales.consulta(categ);
            if (res.Count>=1)
            {
                 id_categ = Convert.ToString(res[0]["id"]);
            }
            if (string.IsNullOrWhiteSpace(id_categ)) id_categ = "null";
            string sub = $"select id from subcategoria where nombre={ComboSubcateg.selectedValue}";
            List<Dictionary<string, object>> subs = globales.consulta(sub);
                if (subs.Count<=1)
            {
                this.subc = Convert.ToString(subs[0]["nombre"]);

            }
            if (string.IsNullOrWhiteSpace(this.subc)) this.subc = "null";
            string valor = string.Empty;
            if (rbBarra.Checked) valor = "1";
            if (rbCocina.Checked) valor = "2";


                if (btnOk.Text=="INSERTAR")
            {
                string inserta = $" insert into productos(nombre, descripcion, precio, id_categoria, notificacion, subcategoria, id_empresa) values    ('{txtNombre.Text}','',{txtCosto.Text}),{this.id_categ}),{valor},{this.subc},{this.id_empresaPrincipal};";
                globales.consulta(inserta, true);
                DialogResult dialogo = globales.MessageBoxSuccess("SE INSERTO CORRECTAMENTE", "AVISO", globales.menuPrincipal);
            }
                


            if (btnOk.Text== "ACTUALIZAR")
            {
                string query = $"UPDATE productos set nombre='{txtNombre.Text}',precio={txtCosto.Text},id_categoria={this.id_categ},subcategoria={this.subc},{this.id_empresaPrincipal};";
                globales.consulta(query);
            }
        }
    }
}
