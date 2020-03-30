namespace lotwtool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAndTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CHRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxFilename = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelCHRCountValue = new System.Windows.Forms.Label();
            this.labelMapCount = new System.Windows.Forms.Label();
            this.buttonMapEdit = new System.Windows.Forms.Button();
            this.labelCHRCount = new System.Windows.Forms.Label();
            this.buttonCHREdit = new System.Windows.Forms.Button();
            this.labelMapCountValue = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(559, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAndTestToolStripMenuItem,
            this.toolStripMenuItem1,
            this.undoToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAndTestToolStripMenuItem
            // 
            this.saveAndTestToolStripMenuItem.Name = "saveAndTestToolStripMenuItem";
            this.saveAndTestToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.saveAndTestToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.saveAndTestToolStripMenuItem.Text = "Save and &Run";
            this.saveAndTestToolStripMenuItem.Click += new System.EventHandler(this.saveAndTestToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(192, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.undoToolStripMenuItem.Text = "&Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(192, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapsToolStripMenuItem,
            this.CHRToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // mapsToolStripMenuItem
            // 
            this.mapsToolStripMenuItem.Enabled = false;
            this.mapsToolStripMenuItem.Name = "mapsToolStripMenuItem";
            this.mapsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mapsToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.mapsToolStripMenuItem.Text = "&Maps";
            this.mapsToolStripMenuItem.Click += new System.EventHandler(this.mapsToolStripMenuItem_Click);
            // 
            // CHRToolStripMenuItem
            // 
            this.CHRToolStripMenuItem.Enabled = false;
            this.CHRToolStripMenuItem.Name = "CHRToolStripMenuItem";
            this.CHRToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.CHRToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.CHRToolStripMenuItem.Text = "&CHR";
            this.CHRToolStripMenuItem.Click += new System.EventHandler(this.CHRToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxFilename, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(559, 119);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // textBoxFilename
            // 
            this.textBoxFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilename.Location = new System.Drawing.Point(3, 3);
            this.textBoxFilename.Name = "textBoxFilename";
            this.textBoxFilename.ReadOnly = true;
            this.textBoxFilename.Size = new System.Drawing.Size(553, 20);
            this.textBoxFilename.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Controls.Add(this.labelCHRCountValue, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelMapCount, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonMapEdit, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelCHRCount, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonCHREdit, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelMapCountValue, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 29);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(553, 87);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // labelCHRCountValue
            // 
            this.labelCHRCountValue.AutoSize = true;
            this.labelCHRCountValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelCHRCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCHRCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCHRCountValue.Location = new System.Drawing.Point(417, 43);
            this.labelCHRCountValue.Name = "labelCHRCountValue";
            this.labelCHRCountValue.Size = new System.Drawing.Size(133, 44);
            this.labelCHRCountValue.TabIndex = 7;
            this.labelCHRCountValue.Text = "0";
            this.labelCHRCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMapCount
            // 
            this.labelMapCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMapCount.Location = new System.Drawing.Point(279, 0);
            this.labelMapCount.Name = "labelMapCount";
            this.labelMapCount.Size = new System.Drawing.Size(132, 43);
            this.labelMapCount.TabIndex = 2;
            this.labelMapCount.Text = "Map Count:";
            this.labelMapCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonMapEdit
            // 
            this.buttonMapEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonMapEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMapEdit.Enabled = false;
            this.buttonMapEdit.Location = new System.Drawing.Point(3, 3);
            this.buttonMapEdit.Name = "buttonMapEdit";
            this.buttonMapEdit.Size = new System.Drawing.Size(270, 37);
            this.buttonMapEdit.TabIndex = 4;
            this.buttonMapEdit.Text = "Maps";
            this.buttonMapEdit.UseVisualStyleBackColor = true;
            this.buttonMapEdit.Click += new System.EventHandler(this.buttonMapEdit_Click);
            // 
            // labelCHRCount
            // 
            this.labelCHRCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCHRCount.Location = new System.Drawing.Point(279, 43);
            this.labelCHRCount.Name = "labelCHRCount";
            this.labelCHRCount.Size = new System.Drawing.Size(132, 44);
            this.labelCHRCount.TabIndex = 3;
            this.labelCHRCount.Text = "CHR Count:";
            this.labelCHRCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonCHREdit
            // 
            this.buttonCHREdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCHREdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCHREdit.Enabled = false;
            this.buttonCHREdit.Location = new System.Drawing.Point(3, 46);
            this.buttonCHREdit.Name = "buttonCHREdit";
            this.buttonCHREdit.Size = new System.Drawing.Size(270, 38);
            this.buttonCHREdit.TabIndex = 5;
            this.buttonCHREdit.Text = "CHR";
            this.buttonCHREdit.UseVisualStyleBackColor = true;
            this.buttonCHREdit.Click += new System.EventHandler(this.buttonCHREdit_Click);
            // 
            // labelMapCountValue
            // 
            this.labelMapCountValue.AutoSize = true;
            this.labelMapCountValue.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelMapCountValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelMapCountValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMapCountValue.Location = new System.Drawing.Point(417, 0);
            this.labelMapCountValue.Name = "labelMapCountValue";
            this.labelMapCountValue.Size = new System.Drawing.Size(133, 43);
            this.labelMapCountValue.TabIndex = 6;
            this.labelMapCountValue.Text = "0";
            this.labelMapCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 143);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.Text = "LotW Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Main_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Main_DragEnter);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxFilename;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelCHRCount;
        private System.Windows.Forms.Label labelMapCount;
        private System.Windows.Forms.Button buttonMapEdit;
        private System.Windows.Forms.Button buttonCHREdit;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem saveAndTestToolStripMenuItem;
        private System.Windows.Forms.Label labelCHRCountValue;
        private System.Windows.Forms.Label labelMapCountValue;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CHRToolStripMenuItem;
    }
}

