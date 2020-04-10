namespace lotwtool
{
    partial class MapEditTile
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTipLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom1xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom2xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom3xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom4xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.paletteBox = new System.Windows.Forms.PictureBox();
            this.statusStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripTipLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 300);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(256, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(28, 17);
            this.toolStripStatusLabel.Text = "...";
            // 
            // toolStripTipLabel
            // 
            this.toolStripTipLabel.Name = "toolStripTipLabel";
            this.toolStripTipLabel.Size = new System.Drawing.Size(213, 17);
            this.toolStripTipLabel.Spring = true;
            this.toolStripTipLabel.Text = "...";
            this.toolStripTipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(256, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // actionToolStripMenuItem
            // 
            this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem});
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.actionToolStripMenuItem.Text = "&Action";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoom1xToolStripMenuItem,
            this.zoom2xToolStripMenuItem,
            this.zoom3xToolStripMenuItem,
            this.zoom4xToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // zoom1xToolStripMenuItem
            // 
            this.zoom1xToolStripMenuItem.Name = "zoom1xToolStripMenuItem";
            this.zoom1xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.zoom1xToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.zoom1xToolStripMenuItem.Text = "Zoom 1x";
            this.zoom1xToolStripMenuItem.Click += new System.EventHandler(this.zoom1xToolStripMenuItem_Click);
            // 
            // zoom2xToolStripMenuItem
            // 
            this.zoom2xToolStripMenuItem.Name = "zoom2xToolStripMenuItem";
            this.zoom2xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.zoom2xToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.zoom2xToolStripMenuItem.Text = "Zoom 2x";
            this.zoom2xToolStripMenuItem.Click += new System.EventHandler(this.zoom2xToolStripMenuItem_Click);
            // 
            // zoom3xToolStripMenuItem
            // 
            this.zoom3xToolStripMenuItem.Name = "zoom3xToolStripMenuItem";
            this.zoom3xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.zoom3xToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.zoom3xToolStripMenuItem.Text = "Zoom 3x";
            this.zoom3xToolStripMenuItem.Click += new System.EventHandler(this.zoom3xToolStripMenuItem_Click);
            // 
            // zoom4xToolStripMenuItem
            // 
            this.zoom4xToolStripMenuItem.Name = "zoom4xToolStripMenuItem";
            this.zoom4xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.zoom4xToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.zoom4xToolStripMenuItem.Text = "Zoom 4x";
            this.zoom4xToolStripMenuItem.Click += new System.EventHandler(this.zoom4xToolStripMenuItem_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(0, 27);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(256, 256);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // paletteBox
            // 
            this.paletteBox.Location = new System.Drawing.Point(0, 283);
            this.paletteBox.Name = "paletteBox";
            this.paletteBox.Size = new System.Drawing.Size(256, 16);
            this.paletteBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.paletteBox.TabIndex = 3;
            this.paletteBox.TabStop = false;
            this.paletteBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.paletteBox_MouseDown);
            this.paletteBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.paletteBox_MouseMove);
            // 
            // MapEditTile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 322);
            this.Controls.Add(this.paletteBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MapEditTile";
            this.Text = "Tiles";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapEditTile_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MapEditTile_KeyDown);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.PictureBox paletteBox;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom1xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom2xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom3xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom4xToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTipLabel;
    }
}