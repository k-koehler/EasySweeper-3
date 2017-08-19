using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using EasyAPI;

namespace EasyWinterface {
    internal class SearchPlayerForm : Form {
        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private TextBox textBox2;

        public SearchPlayerForm() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search Player:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(95, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 20);
            this.textBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(275, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox2.Location = new System.Drawing.Point(0, 31);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(584, 230);
            this.textBox2.TabIndex = 3;
            // 
            // SearchPlayerForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "SearchPlayerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private string _sexyToString(Floor f)
        {
            return "Category: " + Tasks.CategoryString(Tasks.DetermineCategory(f))
            + " | Theme: " + f.Theme 
            + " | Time: "  + f.Time.ToString()
            + " | Players: " + _playersToString(f);
        }

        private string _playersToString(Floor f)
        {
            var str = "";
            foreach (var p in f.Players)
            {
                str += p.User + ", ";
            }
            return str;
        }

        private async void button1_Click(object sender, System.EventArgs e) {
            int counter = 1;
            var playerQuery = new List<Tuple<Player, int>>();
            playerQuery.Add(new Tuple<Player, int>(new Player(textBox1.Text), 0));
            var matchedFloors = await Tasks.Api.SearchFloor(participants: playerQuery, ignorePosition: true);
            foreach (var floor in matchedFloors)
            {
                if (Tasks.DetermineCategory(floor) == Tasks.CATEGORY.InvalidFloor)
                    continue;
                textBox2.AppendText(counter + "." + _sexyToString(floor) + "\n");
                ++counter;
            }

        }
    }
}