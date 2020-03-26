using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lotwtool
{
    public partial class Main : Form
    {
        // ROM state

        bool changed = false;
        byte[] rom = { };
        string filename = "";

        // Common code

        bool changePrevent(string caption)
        {
            if (!changed) return false;
            DialogResult result = MessageBox.Show("You have unsaved changes. Proceed?", caption, MessageBoxButtons.OKCancel);
            return result != DialogResult.OK;
        }

        bool openFile(string path)
        {
            if (changePrevent("Open...")) return false;

            byte[] read_rom;
            try
            {
                read_rom = File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open file:\n" + path + "\n\n" + ex.ToString(), "File open error!");
                return false;
            }
            rom = read_rom;
            filename = path; textBoxFilename.Text = filename;
            changed = false;
            return true;
        }

        bool saveFile(string path)
        {
            try
            {
                File.WriteAllBytes(path,rom);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open file:\n" + path + "\n\n" + ex.ToString(), "File open error!");
                return false;
            }
            changed = false;
            return true;
        }

        // Form

        public Main()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Open NES ROM";
            if (d.ShowDialog() == DialogResult.OK)
            {
                openFile(d.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(filename);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Save NES ROM";
            if (d.ShowDialog() == DialogResult.OK)
            {
                saveFile(d.FileName);
                filename = d.FileName; textBoxFilename.Text = filename;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = changePrevent("Exit...");
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // open file from the command line
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                openFile(args[1]);
            }
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ?
                DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in files) openFile(file);
        }
    }
}
