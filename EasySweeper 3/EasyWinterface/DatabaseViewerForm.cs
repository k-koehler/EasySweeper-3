using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EasyWinterface {
    internal class DatabaseViewerForm : Form {
        private TextBox textBox1;
        private Button button1;
        private DataGridView dataGridView1;
        private BindingSource ewintDataSetBindingSource;
        private System.ComponentModel.IContainer components;
        private ewintDataSet ewintDataSet;
        private Label label1;

        public DatabaseViewerForm() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ewintDataSet = new EasyWinterface.ewintDataSet();
            this.ewintDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ewintDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ewintDataSetBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter username: ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(97, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(203, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(button1_onClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.DataSource = this.ewintDataSetBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 37);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(260, 221);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ewintDataSet
            // 
            this.ewintDataSet.DataSetName = "ewintDataSet";
            this.ewintDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ewintDataSetBindingSource
            // 
            this.ewintDataSetBindingSource.DataSource = this.ewintDataSet;
            this.ewintDataSetBindingSource.Position = 0;
            // 
            // DatabaseViewerForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "DatabaseViewerForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ewintDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ewintDataSetBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_onClick(object sender, EventArgs e) {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }
    }
}