﻿using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtiendelosDestktop.herramientas
{
    public partial class frmReporte : Form
    {
        private bool cargando;
        private bool imprimir;
        private bool pdf;
        private string mensaje;
        private string nombrePdf;
        public frmReporte(string nombreReporte, string tablaDataSet)
        {
            InitializeComponent();
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.p_quirogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tablas = new AtiendelosDestktop.reportes.tablas();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);


            ((System.ComponentModel.ISupportInitialize)(this.p_quirogBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablas)).BeginInit();
            this.SuspendLayout();
            this.p_quirogBindingSource.DataMember = tablaDataSet;
            this.p_quirogBindingSource.DataSource = this.tablas;
            this.tablas.DataSetName = "tablas";
            this.tablas.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.p_quirogBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = string.Format("AtiendelosDestktop.reportes{0}.rdlc", nombreReporte);
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(768, 499); 
            this.reportViewer1.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font; 
            this.ClientSize = new System.Drawing.Size(768, 499);
            this.Controls.Add(this.reportViewer1);
            this.Name = "frmReporte";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmReporte_Load);

            ((System.ComponentModel.ISupportInitialize)(this.tablas)).EndInit();
            this.ResumeLayout(false);
            WindowState = FormWindowState.Maximized;
            this.Text = "Reporte";
            this.ShowInTaskbar = false;
        }

      
        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void frmReporte_Load(object sender, EventArgs e)
        {





        }


        internal void cargarDatos(string tablaNombre, object[] objeto, string mensaje, bool imprimir = false, object[] parametros = null)
        {

            DataTable tabla = tablas.Tables[tablaNombre];

            this.mensaje = mensaje;

            if (tabla != null)
            {

                foreach (var item in objeto)
                {
                    tabla.Rows.Add((object[])item);
                }
            }

            if (parametros != null)
            {
                object[][] elemento = (object[][])parametros;
                ReportParameter[] auxParametros = new ReportParameter[elemento[0].Length];
                for (int x = 0; x < elemento[0].Length; x++)
                {
                    ReportParameter p1 = new ReportParameter((string)elemento[0][x], (string)elemento[1][x]);
                    auxParametros[x] = p1;
                }
                reportViewer1.LocalReport.SetParameters(auxParametros);
            }
            this.reportViewer1.RefreshReport();
            this.imprimir = imprimir;
        }
        public void setParametrosExtra(bool pdf, string nombre)
        {
            this.pdf = pdf;
            this.nombrePdf = nombre;
        }

        private void frmReporte_Load_1(object sender, EventArgs e)
        {

        }
    }
}
