namespace lotwtool
{
    partial class MapSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSelect));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSecretToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.halfSecretToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomr16xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomr8xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomr4xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomr2xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom1xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom2xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom3xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom4xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1047, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveImageToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveImageToolStripMenuItem.Text = "Save Image...";
            this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.saveImageToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSecretToolStripMenuItem,
            this.halfSecretToolStripMenuItem,
            this.showItemsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.zoomr16xToolStripMenuItem,
            this.zoomr8xToolStripMenuItem,
            this.zoomr4xToolStripMenuItem,
            this.zoomr2xToolStripMenuItem,
            this.zoom1xToolStripMenuItem,
            this.zoom2xToolStripMenuItem,
            this.zoom3xToolStripMenuItem,
            this.zoom4xToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // showSecretToolStripMenuItem
            // 
            this.showSecretToolStripMenuItem.Name = "showSecretToolStripMenuItem";
            this.showSecretToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showSecretToolStripMenuItem.Text = "Show Secret Walls";
            this.showSecretToolStripMenuItem.Click += new System.EventHandler(this.showSecretToolStripMenuItem_Click);
            // 
            // halfSecretToolStripMenuItem
            // 
            this.halfSecretToolStripMenuItem.Checked = true;
            this.halfSecretToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.halfSecretToolStripMenuItem.Name = "halfSecretToolStripMenuItem";
            this.halfSecretToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.halfSecretToolStripMenuItem.Text = "Half Secret Walls";
            this.halfSecretToolStripMenuItem.Click += new System.EventHandler(this.halfSecretToolStripMenuItem_Click);
            // 
            // showItemsToolStripMenuItem
            // 
            this.showItemsToolStripMenuItem.Name = "showItemsToolStripMenuItem";
            this.showItemsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showItemsToolStripMenuItem.Text = "Show Items";
            this.showItemsToolStripMenuItem.Click += new System.EventHandler(this.showItemsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // zoomr16xToolStripMenuItem
            // 
            this.zoomr16xToolStripMenuItem.Name = "zoomr16xToolStripMenuItem";
            this.zoomr16xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoomr16xToolStripMenuItem.Text = "Zoom 1/16x";
            this.zoomr16xToolStripMenuItem.Click += new System.EventHandler(this.zoomXr16ToolStripMenuItem_Click);
            // 
            // zoomr8xToolStripMenuItem
            // 
            this.zoomr8xToolStripMenuItem.Name = "zoomr8xToolStripMenuItem";
            this.zoomr8xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoomr8xToolStripMenuItem.Text = "Zoom 1/8x";
            this.zoomr8xToolStripMenuItem.Click += new System.EventHandler(this.zoomXr8ToolStripMenuItem_Click);
            // 
            // zoomr4xToolStripMenuItem
            // 
            this.zoomr4xToolStripMenuItem.Checked = true;
            this.zoomr4xToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zoomr4xToolStripMenuItem.Name = "zoomr4xToolStripMenuItem";
            this.zoomr4xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoomr4xToolStripMenuItem.Text = "Zoom 1/4x";
            this.zoomr4xToolStripMenuItem.Click += new System.EventHandler(this.zoomXr4ToolStripMenuItem_Click);
            // 
            // zoomr2xToolStripMenuItem
            // 
            this.zoomr2xToolStripMenuItem.Name = "zoomr2xToolStripMenuItem";
            this.zoomr2xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoomr2xToolStripMenuItem.Text = "Zoom 1/2x";
            this.zoomr2xToolStripMenuItem.Click += new System.EventHandler(this.zoomXr2ToolStripMenuItem_Click);
            // 
            // zoom1xToolStripMenuItem
            // 
            this.zoom1xToolStripMenuItem.Name = "zoom1xToolStripMenuItem";
            this.zoom1xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom1xToolStripMenuItem.Text = "Zoom 1x";
            this.zoom1xToolStripMenuItem.Click += new System.EventHandler(this.zoomX1ToolStripMenuItem_Click);
            // 
            // zoom2xToolStripMenuItem
            // 
            this.zoom2xToolStripMenuItem.Name = "zoom2xToolStripMenuItem";
            this.zoom2xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom2xToolStripMenuItem.Text = "Zoom 2x";
            this.zoom2xToolStripMenuItem.Click += new System.EventHandler(this.zoomX2ToolStripMenuItem_Click);
            // 
            // zoom3xToolStripMenuItem
            // 
            this.zoom3xToolStripMenuItem.Name = "zoom3xToolStripMenuItem";
            this.zoom3xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom3xToolStripMenuItem.Text = "Zoom 3x";
            this.zoom3xToolStripMenuItem.Click += new System.EventHandler(this.zoomX3ToolStripMenuItem_Click);
            // 
            // zoom4xToolStripMenuItem
            // 
            this.zoom4xToolStripMenuItem.Name = "zoom4xToolStripMenuItem";
            this.zoom4xToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zoom4xToolStripMenuItem.Text = "Zoom 4x";
            this.zoom4xToolStripMenuItem.Click += new System.EventHandler(this.zoomX4ToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 644);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1047, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel.Text = "...";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.Controls.Add(this.pictureBox);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(1047, 620);
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
            this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseClick);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            // 
            // MapSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 666);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MapSelect";
            this.Text = "Map Select";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MapSelect_FormClosing);
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
        private System.Windows.Forms.ToolStripMenuItem zoomr16xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomr8xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomr4xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomr2xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom1xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom2xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom3xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom4xToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSecretToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem showItemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem halfSecretToolStripMenuItem;
    }
}