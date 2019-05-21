namespace CheckView
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.checkCơmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bổSungBáoCơmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.đồngBộToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkCơmToolStripMenuItem,
            this.bổSungBáoCơmToolStripMenuItem,
            this.đồngBộToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1049, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // checkCơmToolStripMenuItem
            // 
            this.checkCơmToolStripMenuItem.Name = "checkCơmToolStripMenuItem";
            this.checkCơmToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.checkCơmToolStripMenuItem.Text = "Check Cơm";
            this.checkCơmToolStripMenuItem.Click += new System.EventHandler(this.checkCơmToolStripMenuItem_Click);
            // 
            // bổSungBáoCơmToolStripMenuItem
            // 
            this.bổSungBáoCơmToolStripMenuItem.Name = "bổSungBáoCơmToolStripMenuItem";
            this.bổSungBáoCơmToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.bổSungBáoCơmToolStripMenuItem.Text = "Bổ sung báo cơm";
            this.bổSungBáoCơmToolStripMenuItem.Click += new System.EventHandler(this.bổSungBáoCơmToolStripMenuItem_Click);
            // 
            // đồngBộToolStripMenuItem
            // 
            this.đồngBộToolStripMenuItem.Name = "đồngBộToolStripMenuItem";
            this.đồngBộToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.đồngBộToolStripMenuItem.Text = "Đồng bộ";
            this.đồngBộToolStripMenuItem.Click += new System.EventHandler(this.đồngBộToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 681);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem checkCơmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bổSungBáoCơmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem đồngBộToolStripMenuItem;
    }
}