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

        public bool changed = false;
        public byte[] rom = { };
        public string filename = "";
        public int chr_offset = 0;
        public int chr_count = 0;
        public int map_count = 0;

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

            // successful load: acquire the ROM
            rom = read_rom;
            filename = path; textBoxFilename.Text = filename;
            changed = false;

            // set file boundaries
            chr_offset = 0;
            chr_count = 0;
            map_count = 0;
            if (rom.Length >= 16)
            {
                int ines_prg = rom[4];
                int ines_chr = rom[5];
                int prg_max = 16 + (ines_prg * 16 * 1024);
                int chr_max = prg_max + (ines_chr * 8 * 1024);
                if (prg_max > rom.Length) prg_max = rom.Length;
                if (chr_max > rom.Length) chr_max = rom.Length;
                map_count = 4 * 18;
                chr_count = ines_chr * 8;
                chr_offset = 16 + (ines_prg * 16 * 1024);
                if (16 + (map_count * 1024) > prg_max)
                    map_count = (prg_max - 16) / 1024;
                if (16 + chr_offset + (chr_count * 1024) > chr_max)
                    chr_max = (chr_max - chr_offset) / 1024;
            }

            textBoxCHRCount.Text = string.Format("{0}", chr_count);
            textBoxMapCount.Text = string.Format("{0}", map_count);

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

        private void buttonMapEdit_Click(object sender, EventArgs e)
        {
            // TODO this is just test editing the first map, should open selector
            MapEdit map_edit = new MapEdit(this, 0); // HACK test on room 0 first
            map_edit.Show();
        }

        private void buttonCHREdit_Click(object sender, EventArgs e)
        {
            CHRSelect chr_select = new CHRSelect(this);
            chr_select.Show();
            // TODO keep track of children, have children notify on close
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
        }
    }
}
