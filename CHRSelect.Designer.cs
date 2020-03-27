namespace lotwtool
{
    partial class CHRSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CHRSelect));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
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
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(279, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundToolStripMenuItem,
            this.spriteToolStripMenuItem,
            this.toolStripMenuItem,
            this.zoom1xToolStripMenuItem,
            this.zoom2xToolStripMenuItem,
            this.zoom3xToolStripMenuItem,
            this.zoom4xToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // backgroundToolStripMenuItem
            // 
            this.backgroundToolStripMenuItem.Checked = true;
            this.backgroundToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.backgroundToolStripMenuItem.Name = "backgroundToolStripMenuItem";
            this.backgroundToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.backgroundToolStripMenuItem.Text = "Background";
            this.backgroundToolStripMenuItem.Click += new System.EventHandler(this.backgroundToolStripMenuItem_Click);
            // 
            // spriteToolStripMenuItem
            // 
            this.spriteToolStripMenuItem.Name = "spriteToolStripMenuItem";
            this.spriteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.spriteToolStripMenuItem.Text = "Sprite";
            this.spriteToolStripMenuItem.Click += new System.EventHandler(this.spriteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem
            // 
            this.toolStripMenuItem.Name = "toolStripMenuItem";
            this.toolStripMenuItem.Size = new System.Drawing.Size(135, 6);
            // 
            // zoom1xToolStripMenuItem
            // 
            this.zoom1xToolStripMenuItem.Name = "zoom1xToolStripMenuItem";
            this.zoom1xToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.zoom1xToolStripMenuItem.Text = "Zoom 1x";
            this.zoom1xToolStripMenuItem.Click += new System.EventHandler(this.zoom1xToolStripMenuItem_Click);
            // 
            // zoom2xToolStripMenuItem
            // 
            this.zoom2xToolStripMenuItem.Checked = true;
            this.zoom2xToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zoom2xToolStripMenuItem.Name = "zoom2xToolStripMenuItem";
            this.zoom2xToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.zoom2xToolStripMenuItem.Text = "Zoom 2x";
            this.zoom2xToolStripMenuItem.Click += new System.EventHandler(this.zoom2xToolStripMenuItem_Click);
            // 
            // zoom3xToolStripMenuItem
            // 
            this.zoom3xToolStripMenuItem.Name = "zoom3xToolStripMenuItem";
            this.zoom3xToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.zoom3xToolStripMenuItem.Text = "Zoom 3x";
            this.zoom3xToolStripMenuItem.Click += new System.EventHandler(this.zoom3xToolStripMenuItem_Click);
            // 
            // zoom4xToolStripMenuItem
            // 
            this.zoom4xToolStripMenuItem.Name = "zoom4xToolStripMenuItem";
            this.zoom4xToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.zoom4xToolStripMenuItem.Text = "Zoom 4x";
            this.zoom4xToolStripMenuItem.Click += new System.EventHandler(this.zoom4xToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 420);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(279, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
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
            this.flowLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel.Controls.Add(this.pictureBox);
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(279, 396);
            this.flowLayoutPanel.TabIndex = 3;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(100, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            // 
            // CHRSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 442);
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "CHRSelect";
            this.Text = "CHR Select";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CHRSelect_FormClosing);
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
        private System.Windows.Forms.ToolStripMenuItem backgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spriteToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom1xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom2xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom3xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom4xToolStripMenuItem;
    }
}