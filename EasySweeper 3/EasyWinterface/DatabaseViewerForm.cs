using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EasyWinterface {
    internal class DatabaseViewerForm : Form {
        private TextBox textBox1;
        private Button button1;
        private BindingSource ewintDataSetBindingSource;
        private System.ComponentModel.IContainer components;
        private TextBox textBox2;
        private Label label2;
        private TextBox textBox3;
        private Label label3;
        private ComboBox comboBox1;
        private Label label1;

        public DatabaseViewerForm() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ewintDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ewintDataSetBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lower Bound:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(70, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(297, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_onClick);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Location = new System.Drawing.Point(7, 55);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(367, 195);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(83, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Upper Bound:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(86, 29);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(73, 20);
            this.textBox3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Category:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "C1",
            "Solo small",
            "Solo med",
            "Solo",
            "Duo",
            "Trio",
            "4s1l",
            "5 man"});
            this.comboBox1.Location = new System.Drawing.Point(165, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 7;
            // 
            // DatabaseViewerForm
            // 
            this.ClientSize = new System.Drawing.Size(384, 261);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "DatabaseViewerForm";
            ((System.ComponentModel.ISupportInitialize)(this.ewintDataSetBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private async void button1_onClick(object sender, EventArgs e) {
            TimeSpan startTimeSpan, endTimeSpan;
            try {
                startTimeSpan = TimeSpan.Parse(textBox1.Text);
                endTimeSpan = TimeSpan.Parse(textBox3.Text);
            } catch (Exception ex ) when (ex is FormatException || ex is OverflowException){
                MessageBox.Show("Invalid query");
                return;
            }
            var matchedFloors = await Tasks.Api.SearchFloor(ids: "" /*, start: startTimeSpan, end: endTimeSpan*/);
            Console.WriteLine("Lower: " + startTimeSpan.ToString());
            Console.WriteLine("Upper: " + endTimeSpan.ToString());
            Console.WriteLine("Search successful! Floor count =  " + matchedFloors.Count);
            textBox2.Clear();
            foreach(var floor in matchedFloors) {
                string playerString = "";
                foreach(var player in floor.Players) {
                    playerString += player.User + " ";
                }
                textBox2.AppendText(floor.Time + " " + floor.Size + " " + Tasks.CategoryString(Tasks.DetermineCategory(floor)) + " " + playerString + "\n");
                //TODO implement category
            }
        }
    }
}