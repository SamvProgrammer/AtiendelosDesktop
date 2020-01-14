using AtiendelosDestktop.forms;
using AtiendelosDestktop.herramientas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtiendelosDestktop
{
    public partial class login : Form
    {
        string id;
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
            string query = "SELECT nombre,id FROM empresas;";
            List<Dictionary<string, object>> res = globales.consulta(query);
            foreach(var item in res)
            {
                string nombre_empresa = Convert.ToString(item["nombre"]);
                comboBox1.Items.Add(nombre_empresa);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string login = $"SELECT * FROM users where login='{txtUsr.Text}' and password='{txtPasword.Text}' and id_empresa={this.id};";
            List<Dictionary<string, object>> resultado = globales.consulta(login);
            if (resultado.Count <= 0) return;

            frmMenu menu = new frmMenu();
            menu.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string valida = $"select id from empresas where nombre='{comboBox1.Text}'";
            List<Dictionary<string, object>> valid = globales.consulta(valida);
                this.id = string.Empty;
            id = Convert.ToString(valid[0]["id"]);
        }
    }
}
