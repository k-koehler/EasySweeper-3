using System;
using System.Drawing;
using System.Windows.Forms;

namespace EasyWinterface
{
    class EWAppContext : ApplicationContext {
        //Component declarations
        public NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;
        private ToolStripMenuItem _dbQueryItem;

        public EWAppContext() {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            InitializeComponent();
            TrayIcon.Visible = true;
        }

        private void InitializeComponent() {
            TrayIcon = new NotifyIcon();

            //The icon is added to the project resources.
            //Here I assume that the name of the file is 'TrayIcon.ico'
            TrayIcon.Icon = Properties.Resources.TrayIcon;


            //Optional - Add a context menu to the TrayIcon:
            TrayIconContextMenu = new ContextMenuStrip();
            CloseMenuItem = new ToolStripMenuItem();
            _dbQueryItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
                this.CloseMenuItem, this._dbQueryItem});
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new Size(153, 70);
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = "Close EasyWinterface";
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);
            //
            // _dbQueryItem
            //
            this._dbQueryItem.Name = "_dbQueryItem";
            this._dbQueryItem.Size = new Size(152, 22);
            this._dbQueryItem.Text = "Explore DB";
            this._dbQueryItem.Click += new EventHandler(this.db_queryItem_click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;
        }

        private void db_queryItem_click(object sender, EventArgs e)
        {
            var form = new DatabaseViewerForm();
            form.Show();
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            //Cleanup so that the icon will be removed when the application is closed
            TrayIcon.Visible = false;
        }

        private void CloseMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}