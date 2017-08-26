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
        private ToolStripMenuItem _searchPlayer;
        private ToolStripMenuItem HiScores;

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
            _searchPlayer = new ToolStripMenuItem();
            HiScores      = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
                this.CloseMenuItem, _searchPlayer, HiScores});
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
            // _searchPlater
            //
            this._searchPlayer.Name = "_searchPlayer";
            this._searchPlayer.Size = new Size(152, 22);
            this._searchPlayer.Text = "Search Player";
            this._searchPlayer.Click += new EventHandler(this._searchPlayer_click);
            //
            // HiScores
            //
            this.HiScores.Name = "HiScores";
            this.HiScores.Size = new Size(152,22);
            this.HiScores.Text = "Browse HiScores";
            this.HiScores.Click += new EventHandler(this.Hiscores_click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;
        }

        private void Hiscores_click(object sender, EventArgs e)
        {
            var form = new HiScoresForm();
            form.Show();
        }

        private void _searchPlayer_click(object sender, EventArgs e)
        {
            var form = new SearchPlayerForm();
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