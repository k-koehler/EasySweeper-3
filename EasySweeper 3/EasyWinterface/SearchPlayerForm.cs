using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EasyAPI;
using System.Threading;

namespace EasyWinterface {
    internal class SearchPlayerForm : Form {
        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private Label label2;
        private ComboBox comboBox1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private Label label3;
        private TextBox textBox3;
        private TextBox textBox2;

        public SearchPlayerForm() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
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
            this.textBox2.Location = new System.Drawing.Point(0, 69);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(584, 192);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(357, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sort by:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Date",
            "Time"});
            this.comboBox1.Location = new System.Drawing.Point(406, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(17, 24);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(56, 17);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "5 Man";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(95, 24);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(51, 17);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "4S1L";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(181, 24);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(44, 17);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "Trio";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(267, 24);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(46, 17);
            this.checkBox4.TabIndex = 9;
            this.checkBox4.Text = "Duo";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Location = new System.Drawing.Point(344, 23);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(47, 17);
            this.checkBox5.TabIndex = 10;
            this.checkBox5.Text = "Solo";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.Location = new System.Drawing.Point(430, 23);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(95, 17);
            this.checkBox6.TabIndex = 11;
            this.checkBox6.Text = "Med/Small/C1";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Average Times:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(95, 43);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(432, 20);
            this.textBox3.TabIndex = 13;
            // 
            // SearchPlayerForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 261);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox6);
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "SearchPlayerForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void _sortByDate(List<Floor> list)
        {
            list.Sort((x, y) => DateTime.Compare(y.Date, x.Date));
        }

        private void _sortByTime(List<Floor> list) {
            list.Sort((x, y) => TimeSpan.Compare(x.Time, y.Time));
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

        private List<Tasks.CATEGORY> _checkCategories()
        {
            var retList = new List<Tasks.CATEGORY>();
            if (this.checkBox1.Checked) retList.Add(Tasks.CATEGORY._5s);
            if (this.checkBox2.Checked) retList.Add(Tasks.CATEGORY._4s);
            if (this.checkBox3.Checked) retList.Add(Tasks.CATEGORY._3s);
            if (this.checkBox4.Checked) retList.Add(Tasks.CATEGORY.Duo);
            if (this.checkBox5.Checked) retList.Add(Tasks.CATEGORY.Solo);
            if (this.checkBox6.Checked)
            {
                retList.Add(Tasks.CATEGORY.Small);
                retList.Add(Tasks.CATEGORY.Med);
                retList.Add(Tasks.CATEGORY.C1);
            }
            Console.WriteLine("checked: " + retList.Count);
            return retList;
        }

        private List<string> _calculateAverages(ICollection<Floor> list)
        {
            List<List<TimeSpan>> averages = new List<List<TimeSpan>>(5);
            for (int i = 0; i < 5; i++)
            {
                averages.Add(new List<TimeSpan>());
            }
            foreach (var floor in list)
            {
                var cat = Tasks.DetermineCategory(floor);
                switch (cat)
                {
                    case Tasks.CATEGORY._5s:
                    averages[0].Add(floor.Time);
                    break;
                    case Tasks.CATEGORY._4s:
                    averages[1].Add(floor.Time);
                    break;
                    case Tasks.CATEGORY._3s:
                    averages[2].Add(floor.Time);
                    break;
                    case Tasks.CATEGORY.Duo:
                    averages[3].Add(floor.Time);
                    break;
                    case Tasks.CATEGORY.Solo:
                    averages[4].Add(floor.Time);
                    break;
                    default:
                    //do nothing
                    break;
                }
            }

            List<TimeSpan> averageTimes = new List<TimeSpan>();
            if (averages[0].Count > 0) averageTimes.Add(_calculateAverageTimeSpan(averages[0]));
            else averageTimes.Add(new TimeSpan(0));
            if (averages[1].Count > 0) averageTimes.Add(_calculateAverageTimeSpan(averages[1]));
            else averageTimes.Add(new TimeSpan(0));
            if (averages[2].Count > 0) averageTimes.Add(_calculateAverageTimeSpan(averages[2]));
            else averageTimes.Add(new TimeSpan(0));
            if (averages[3].Count > 0) averageTimes.Add(_calculateAverageTimeSpan(averages[3]));
            else averageTimes.Add(new TimeSpan(0));
            if (averages[4].Count > 0) averageTimes.Add(_calculateAverageTimeSpan(averages[4]));
            else averageTimes.Add(new TimeSpan(0));

            return new List<string>
            {
                averageTimes[0].ToString(), averageTimes[1].ToString(),
                averageTimes[2].ToString(),averageTimes[3].ToString(),
                averageTimes[4].ToString()
            };
        }

        private TimeSpan _calculateAverageTimeSpan(List<TimeSpan> sourceList)
        {
            double doubleAverageTicks = sourceList.Average(timeSpan => timeSpan.Ticks);
            long longAverageTicks = Convert.ToInt64(doubleAverageTicks);
            return new TimeSpan(longAverageTicks);
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            var validCategories = _checkCategories();
            textBox2.Clear();
            var counter = 1;
            var playerQuery = new List<Tuple<Player, int>>();
            playerQuery.Add(new Tuple<Player, int>(new Player(textBox1.Text), 0));
            var matchedFloors = await Tasks.Api.SearchFloor(participants: playerQuery, ignorePosition: true);

            //clear wrong categories
            for (int i = 0; i < matchedFloors.Count; ++i)
            {
                var category = Tasks.DetermineCategory(matchedFloors[i]);
                Console.WriteLine(Tasks.CategoryString(category));
                if (!validCategories.Contains(category))
                {
                    matchedFloors.RemoveAt(i);
                    --i;
                }
            }

            var listAvgTimes = _calculateAverages(matchedFloors);
            this.textBox3.Text = listAvgTimes[0];

            if (comboBox1.SelectedIndex==0)
                _sortByDate((List<Floor>)matchedFloors);
            else _sortByTime((List<Floor>)matchedFloors);
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