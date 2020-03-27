using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lotwtool
{
    public interface RomRefresh
    {
        void refresh_all();
        void refresh_chr(int tile);
        void refresh_metatile(int page);
        void refresh_close();
    }

    public partial class Main : Form, RomRefresh
    {
        // ROM state

        public bool changed = false;
        public byte[] rom = { };
        public string filename = "";
        public int chr_offset = 0;
        public int chr_count = 0;
        public int map_count = 0;

        List<RomRefresh> refreshers = new List<RomRefresh>();
        List<MapEdit> mapedits = new List<MapEdit>();
        public MapSelect map_select = null;

        public static readonly uint[] NES_PALETTE =
        {
            0x656665, 0x001E9C, 0x200DAC, 0x44049C,
            0x6A036F, 0x71021D, 0x651000, 0x461E00,
            0x232D00, 0x003900, 0x003C00, 0x003720,
            0x003266, 0x000000, 0x000000, 0x000000,
            0xB0B1B0, 0x0955EB, 0x473DFF, 0x7730FE,
            0xAE2CCE, 0xBC2964, 0xB43900, 0x8F4B00,
            0x635F00, 0x1B7000, 0x007700, 0x00733B,
            0x006D99, 0x000000, 0x000000, 0x000000,
            0xFFFFFF, 0x4DADFF, 0x8694FF, 0xB885FF,
            0xF17FFF, 0xFF79D4, 0xFF865F, 0xF19710,
            0xC8AB00, 0x7EBE00, 0x47C81F, 0x2BC86F,
            0x2EC4CC, 0x50514D, 0x000000, 0x000000,
            0xFFFFFF, 0xB9E5FF, 0xD0DBFF, 0xE6D5FF,
            0xFDD1FF, 0xFFCEF5, 0xFFD4C5, 0xFFDAA3,
            0xEEE290, 0xD0EB8E, 0xB9EFA5, 0xAEEFC7,
            0xAEEEEE, 0xBABCB9, 0x000000, 0x000000,
        };

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

            close_children();

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

        public void chr_cache(int rom_index, int cache_index, uint[] cache, uint[] palette)
        {
            // decodes a tile from the ROM, and stores it in cache with given palette
            int ro = chr_offset + (rom_index * 16);
            int co = cache_index * 64;
            if ((ro + 16) > rom.Length)
            {
                for (int i=0; i<16; ++i) cache[co+i] = 0;
                return;
            }
            for (int y = 0; y < 8; ++y)
            {
                byte p0 = rom[ro + y];
                byte p1 = rom[ro + y + 8];
                for (int x = 0; x < 8; ++x)
                {
                    int p = ((p0 >> 7) & 1) | ((p1 >> 6) & 2);
                    cache[co + x + (y * 8)] = palette[p];
                    p0 <<= 1;
                    p1 <<= 1;
                }
            }
        }

        public void chr_blit(BitmapData bd, uint[] cache, int tile, int x, int y, int zoom)
        {
            x *= zoom;
            y *= zoom;
            // draws a cached tile on a locked bitmap data
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                fixed (uint* fcc = cache)
                {
                    uint* scanline = braw + (stride * y) + x;
                    uint* chrline = fcc + (tile * 64);
                    for (int py = 0; py < 8; ++py)
                    {
                        for (int yz = 0; yz < zoom; ++yz)
                        {
                            int sx = 0;
                            for (int px = 0; px < 8; ++px)
                            {
                                for (int xz = 0; xz < zoom; ++xz)
                                {
                                    scanline[sx] = chrline[px];
                                    ++sx;
                                }
                            }
                            scanline += stride;
                        }
                        chrline += 8;
                    }
                }
            }
        }

        public void chr_blit_dark(BitmapData bd, uint[] cache, int tile, int x, int y, int zoom)
        {
            x *= zoom;
            y *= zoom;
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                fixed (uint* fcc = cache)
                {
                    uint* scanline = braw + (stride * y) + x;
                    uint* chrline = fcc + (tile * 64);
                    for (int py = 0; py < 8; ++py)
                    {
                        for (int yz = 0; yz < zoom; ++yz)
                        {
                            int sx = 0;
                            for (int px = 0; px < 8; ++px)
                            {
                                for (int xz = 0; xz < zoom; ++xz)
                                {
                                    //scanline[sx] = ((chrline[px] >> 1) & 0x7F7F7F7F) | 0xFF000000; // 1/2 bright
                                    scanline[sx] = ((chrline[px] >> 2) & 0x3F3F3F3F) | 0xFF000000; // 1/4 bright
                                    ++sx;
                                }
                            }
                            scanline += stride;
                        }
                        chrline += 8;
                    }
                }
            }
        }

        public void chr_blit_mask(BitmapData bd, uint[] cache, int tile, int x, int y, int zoom)
        {
            x *= zoom;
            y *= zoom;
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                fixed (uint* fcc = cache)
                {
                    uint* scanline = braw + (stride * y) + x;
                    uint* chrline = fcc + (tile * 64);
                    for (int py = 0; py < 8; ++py)
                    {
                        for (int yz = 0; yz < zoom; ++yz)
                        {
                            int sx = 0;
                            for (int px = 0; px < 8; ++px)
                            {
                                for (int xz = 0; xz < zoom; ++xz)
                                {
                                    uint c = chrline[px];
                                    if (c != 0)
                                        scanline[sx] = c;
                                    ++sx;
                                }
                            }
                            scanline += stride;
                        }
                        chrline += 8;
                    }
                }
            }
        }

        // Children management

        public void refresh_all() { } // TODO

        public void refresh_chr(int tile)
        {
            foreach (RomRefresh r in refreshers) r.refresh_chr(tile);
        }

        public void refresh_metatile(int page)
        {
            foreach (RomRefresh r in refreshers) r.refresh_metatile(page);
        }

        public void refresh_close()
        {
            this.Close(); // shouldn't be used?
        }

        public void add_refresh(RomRefresh r)
        {
            refreshers.Add(r);
        }

        public void remove_refresh(RomRefresh r)
        {
            bool removed;
            do
            {
                removed = refreshers.Remove(r);
            } while (removed);
        }

        public void add_map_edit(int room)
        {
            if (room >= map_count) return;
            foreach (MapEdit m in mapedits)
            {
                if (m.room == room)
                {
                    if (raise_child(m)) return;
                }
            }
            MapEdit me = new MapEdit(this, room);
            me.Show();
            mapedits.Add(me);
            add_refresh(me);
        }

        public void remove_map_edit(MapEdit m)
        {
            bool removed;
            do
            {
                removed = mapedits.Remove(m);
            } while (removed);
        }

        public void close_children()
        {
            while (refreshers.Count > 0)
                refreshers[0].refresh_close();
        }

        public static bool raise_child(Form c) // convenient way to wake a child
        {
            if (c == null) return false;
            if (c.WindowState == FormWindowState.Minimized)
            {
                // can't seem to easily check if it was maximized before minimize, unfortunately?
                c.WindowState = FormWindowState.Normal;
            }
            c.Activate();
            return true;
        }

        // Form

        public Main()
        {
            InitializeComponent();
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
            if (map_count < 1) return;
            if (raise_child(map_select)) return;
            map_select = new MapSelect(this);
            map_select.Show();
            add_refresh(map_select);
        }

        private void buttonCHREdit_Click(object sender, EventArgs e)
        {
            if (chr_count < 1) return;
            CHRSelect chr_select = new CHRSelect(this);
            chr_select.Show();
            add_refresh(chr_select);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
        }
    }
}
