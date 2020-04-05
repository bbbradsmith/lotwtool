namespace lotwtool
{
    partial class Nametable
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripTipLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pictureTile = new System.Windows.Forms.PictureBox();
            this.picturePalette = new System.Windows.Forms.PictureBox();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importNamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportNamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePalette)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // actionToolStripMenuItem
            // 
            this.actionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.importNamToolStripMenuItem,
            this.exportNamToolStripMenuItem});
            this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
            this.actionToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.actionToolStripMenuItem.Text = "&Action";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripTipLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 519);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
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
            this.toolStripTipLabel.Size = new System.Drawing.Size(741, 17);
            this.toolStripTipLabel.Spring = true;
            this.toolStripTipLabel.Text = "...";
            this.toolStripTipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(5, 33);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(512, 480);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            // 
            // pictureTile
            // 
            this.pictureTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureTile.Location = new System.Drawing.Point(523, 33);
            this.pictureTile.Name = "pictureTile";
            this.pictureTile.Size = new System.Drawing.Size(256, 256);
            this.pictureTile.TabIndex = 3;
            this.pictureTile.TabStop = false;
            this.pictureTile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureTile_MouseDown);
            this.pictureTile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureTile_MouseMove);
            // 
            // picturePalette
            // 
            this.picturePalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picturePalette.Location = new System.Drawing.Point(523, 295);
            this.picturePalette.Name = "picturePalette";
            this.picturePalette.Size = new System.Drawing.Size(256, 64);
            this.picturePalette.TabIndex = 4;
            this.picturePalette.TabStop = false;
            this.picturePalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picturePalette_MouseDown);
            this.picturePalette.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picturePalette_MouseMove);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gridToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // gridToolStripMenuItem
            // 
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.gridToolStripMenuItem.Text = "&Grid";
            this.gridToolStripMenuItem.Click += new System.EventHandler(this.gridToolStripMenuItem_Click);
            // 
            // importNamToolStripMenuItem
            // 
            this.importNamToolStripMenuItem.Name = "importNamToolStripMenuItem";
            this.importNamToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importNamToolStripMenuItem.Text = "&Import NAM...";
            this.importNamToolStripMenuItem.Click += new System.EventHandler(this.importNamToolStripMenuItem_Click);
            // 
            // exportNamToolStripMenuItem
            // 
            this.exportNamToolStripMenuItem.Name = "exportNamToolStripMenuItem";
            this.exportNamToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportNamToolStripMenuItem.Text = "&Export NAM...";
            this.exportNamToolStripMenuItem.Click += new System.EventHandler(this.exportNamToolStripMenuItem_Click);
            // 
            // Nametable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 541);
            this.Controls.Add(this.picturePalette);
            this.Controls.Add(this.pictureTile);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Nametable";
            this.Text = "Nametable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Nametable_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePalette)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripTipLabel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.PictureBox pictureTile;
        private System.Windows.Forms.PictureBox picturePalette;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importNamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportNamToolStripMenuItem;
    }
}