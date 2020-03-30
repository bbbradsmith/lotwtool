namespace lotwtool
{
    partial class MapEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapEdit));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terrainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSecretToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.halfSecretToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.zoom1xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom2xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom3xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom4xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoHexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTipLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.adjacentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionToolStripMenuItem,
            this.modeToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.adjacentToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1030, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // actionToolStripMenuItem
            // 
            this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.saveImageToolStripMenuItem});
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
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.saveImageToolStripMenuItem.Text = "Save &Image...";
            this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.saveImageToolStripMenuItem_Click);
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terrainToolStripMenuItem,
            this.itemsToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.modeToolStripMenuItem.Text = "&Mode";
            // 
            // terrainToolStripMenuItem
            // 
            this.terrainToolStripMenuItem.Checked = true;
            this.terrainToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.terrainToolStripMenuItem.Name = "terrainToolStripMenuItem";
            this.terrainToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.terrainToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.terrainToolStripMenuItem.Text = "&Terrain";
            this.terrainToolStripMenuItem.Click += new System.EventHandler(this.terrainToolStripMenuItem_Click);
            // 
            // itemsToolStripMenuItem
            // 
            this.itemsToolStripMenuItem.Name = "itemsToolStripMenuItem";
            this.itemsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.itemsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.itemsToolStripMenuItem.Text = "&Items";
            this.itemsToolStripMenuItem.Click += new System.EventHandler(this.itemsToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSecretToolStripMenuItem,
            this.halfSecretToolStripMenuItem,
            this.showItemsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.zoom1xToolStripMenuItem,
            this.zoom2xToolStripMenuItem,
            this.zoom3xToolStripMenuItem,
            this.zoom4xToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // showSecretToolStripMenuItem
            // 
            this.showSecretToolStripMenuItem.Name = "showSecretToolStripMenuItem";
            this.showSecretToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showSecretToolStripMenuItem.Text = "Show &Secret Walls";
            this.showSecretToolStripMenuItem.Click += new System.EventHandler(this.showSecretToolStripMenuItem_Click);
            // 
            // halfSecretToolStripMenuItem
            // 
            this.halfSecretToolStripMenuItem.Checked = true;
            this.halfSecretToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.halfSecretToolStripMenuItem.Name = "halfSecretToolStripMenuItem";
            this.halfSecretToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.halfSecretToolStripMenuItem.Text = "&Half Secret Walls";
            this.halfSecretToolStripMenuItem.Click += new System.EventHandler(this.halfSecretToolStripMenuItem_Click);
            // 
            // showItemsToolStripMenuItem
            // 
            this.showItemsToolStripMenuItem.Checked = true;
            this.showItemsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showItemsToolStripMenuItem.Name = "showItemsToolStripMenuItem";
            this.showItemsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showItemsToolStripMenuItem.Text = "Show &Items";
            this.showItemsToolStripMenuItem.Click += new System.EventHandler(this.showItemsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // zoom1xToolStripMenuItem
            // 
            this.zoom1xToolStripMenuItem.Checked = true;
            this.zoom1xToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zoom1xToolStripMenuItem.Name = "zoom1xToolStripMenuItem";
            this.zoom1xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.zoom1xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom1xToolStripMenuItem.Text = "Zoom &1x";
            this.zoom1xToolStripMenuItem.Click += new System.EventHandler(this.zoom1xToolStripMenuItem_Click);
            // 
            // zoom2xToolStripMenuItem
            // 
            this.zoom2xToolStripMenuItem.Name = "zoom2xToolStripMenuItem";
            this.zoom2xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.zoom2xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom2xToolStripMenuItem.Text = "Zoom &2x";
            this.zoom2xToolStripMenuItem.Click += new System.EventHandler(this.zoom2xToolStripMenuItem_Click);
            // 
            // zoom3xToolStripMenuItem
            // 
            this.zoom3xToolStripMenuItem.Name = "zoom3xToolStripMenuItem";
            this.zoom3xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.zoom3xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom3xToolStripMenuItem.Text = "Zoom &3x";
            this.zoom3xToolStripMenuItem.Click += new System.EventHandler(this.zoom3xToolStripMenuItem_Click);
            // 
            // zoom4xToolStripMenuItem
            // 
            this.zoom4xToolStripMenuItem.Name = "zoom4xToolStripMenuItem";
            this.zoom4xToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.zoom4xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom4xToolStripMenuItem.Text = "Zoom &4x";
            this.zoom4xToolStripMenuItem.Click += new System.EventHandler(this.zoom4xToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tilesToolStripMenuItem,
            this.infoHexToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // tilesToolStripMenuItem
            // 
            this.tilesToolStripMenuItem.Name = "tilesToolStripMenuItem";
            this.tilesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.tilesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.tilesToolStripMenuItem.Text = "Tile &Palette";
            this.tilesToolStripMenuItem.Click += new System.EventHandler(this.tilesToolStripMenuItem_Click);
            // 
            // infoHexToolStripMenuItem
            // 
            this.infoHexToolStripMenuItem.Name = "infoHexToolStripMenuItem";
            this.infoHexToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.infoHexToolStripMenuItem.Text = "Info &Hex";
            this.infoHexToolStripMenuItem.Click += new System.EventHandler(this.infoHexToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripTipLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 222);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1030, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
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
            this.toolStripTipLabel.Size = new System.Drawing.Size(987, 17);
            this.toolStripTipLabel.Spring = true;
            this.toolStripTipLabel.Text = "...";
            this.toolStripTipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Controls.Add(this.pictureBox);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(1030, 198);
            this.flowLayoutPanel.TabIndex = 2;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // adjacentToolStripMenuItem
            // 
            this.adjacentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upToolStripMenuItem,
            this.downToolStripMenuItem,
            this.leftToolStripMenuItem,
            this.rightToolStripMenuItem});
            this.adjacentToolStripMenuItem.Name = "adjacentToolStripMenuItem";
            this.adjacentToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.adjacentToolStripMenuItem.Text = "Ad&jacent";
            // 
            // upToolStripMenuItem
            // 
            this.upToolStripMenuItem.Name = "upToolStripMenuItem";
            this.upToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.upToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.upToolStripMenuItem.Text = "&Up";
            this.upToolStripMenuItem.Click += new System.EventHandler(this.upToolStripMenuItem_Click);
            // 
            // downToolStripMenuItem
            // 
            this.downToolStripMenuItem.Name = "downToolStripMenuItem";
            this.downToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.downToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.downToolStripMenuItem.Text = "&Down";
            this.downToolStripMenuItem.Click += new System.EventHandler(this.downToolStripMenuItem_Click);
            // 
            // leftToolStripMenuItem
            // 
            this.leftToolStripMenuItem.Name = "leftToolStripMenuItem";
            this.leftToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Left)));
            this.leftToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.leftToolStripMenuItem.Text = "&Left";
            this.leftToolStripMenuItem.Click += new System.EventHandler(this.leftToolStripMenuItem_Click);
            // 
            // rightToolStripMenuItem
            // 
            this.rightToolStripMenuItem.Name = "rightToolStripMenuItem";
            this.rightToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Right)));
            this.rightToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.rightToolStripMenuItem.Text = "&Right";
            this.rightToolStripMenuItem.Click += new System.EventHandler(this.rightToolStripMenuItem_Click);
            // 
            // MapEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 244);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MapEdit";
            this.Text = "Map Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapEdit_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom1xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom2xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom3xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom4xToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSecretToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoHexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terrainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem halfSecretToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTipLabel;
        private System.Windows.Forms.ToolStripMenuItem tilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjacentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rightToolStripMenuItem;
    }
}