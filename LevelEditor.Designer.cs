namespace itp380
{
    partial class LevelEditor
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BrushCombo = new System.Windows.Forms.ToolStripComboBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.loadLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.BrushCombo});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 27);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.exportXMLToolStripMenuItem,
            this.loadLevelToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 23);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // exportXMLToolStripMenuItem
            // 
            this.exportXMLToolStripMenuItem.Name = "exportXMLToolStripMenuItem";
            this.exportXMLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportXMLToolStripMenuItem.Text = "Export XML";
            this.exportXMLToolStripMenuItem.Click += new System.EventHandler(this.exportXMLToolStripMenuItem_Click);
            // 
            // BrushCombo
            // 
            this.BrushCombo.Items.AddRange(new object[] {
            "Erase",
            "Spawn",
            "King",
            "Horizontal Wall",
            "Vertical Wall"});
            this.BrushCombo.Name = "BrushCombo";
            this.BrushCombo.Size = new System.Drawing.Size(121, 23);
            this.BrushCombo.Tag = "Brushes";
            this.BrushCombo.Text = "Erase";
            this.BrushCombo.SelectedIndexChanged += new System.EventHandler(this.BrushCombo_SelectedIndexChanged);
            this.BrushCombo.Click += new System.EventHandler(this.BrushCombo_Click);
            // 
            // loadLevelToolStripMenuItem
            // 
            this.loadLevelToolStripMenuItem.Name = "loadLevelToolStripMenuItem";
            this.loadLevelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadLevelToolStripMenuItem.Text = "Load Level";
            this.loadLevelToolStripMenuItem.Click += new System.EventHandler(this.loadLevelToolStripMenuItem_Click);
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "LevelEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LevelEditor";
            this.Load += new System.EventHandler(this.LevelEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportXMLToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripComboBox BrushCombo;
        private System.Windows.Forms.ToolStripMenuItem loadLevelToolStripMenuItem;

    }
}