using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyMap {

    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        public bool ChangeMainMapPicture(Bitmap bmp) {
            return mapPictureBox1.changePicture(bmp);
        }
    }

    public class MapPictureBox : PictureBox {
        public bool changePicture(Bitmap bmp) {
            try {
                this.Image = bmp;
                return true;
            } catch (Exception) {
                return false;
            }
        }

        private void InitializeComponent() {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
