namespace EasyMap {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mapPictureBox1 = new EasyMap.MapPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mapPictureBox1
            // 
            this.mapPictureBox1.BackgroundImage = global::EasyMap.Properties.Resources.background;
            this.mapPictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mapPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.mapPictureBox1.Name = "mapPictureBox1";
            this.mapPictureBox1.Size = new System.Drawing.Size(284, 262);
            this.mapPictureBox1.TabIndex = 0;
            this.mapPictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.mapPictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MapPictureBox mapPictureBox1;
    }
}

