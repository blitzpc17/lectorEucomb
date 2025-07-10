namespace eucomb.Sistema
{
    partial class MDIMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.sATGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ventasPorMesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sATGToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1584, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // sATGToolStripMenuItem
            // 
            this.sATGToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ventasPorMesToolStripMenuItem});
            this.sATGToolStripMenuItem.Name = "sATGToolStripMenuItem";
            this.sATGToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.sATGToolStripMenuItem.Text = "SAT";
            // 
            // ventasPorMesToolStripMenuItem
            // 
            this.ventasPorMesToolStripMenuItem.Name = "ventasPorMesToolStripMenuItem";
            this.ventasPorMesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ventasPorMesToolStripMenuItem.Text = "Ventas Por Mes";
            this.ventasPorMesToolStripMenuItem.Click += new System.EventHandler(this.ventasPorMesToolStripMenuItem_Click);
            // 
            // MDIMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MDIMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EUCOMB";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripMenuItem sATGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ventasPorMesToolStripMenuItem;
    }
}



