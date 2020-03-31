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
    public partial class CHRSelect : Form, RomRefresh
    {
        public bool sprite = false;
        int zoom = 2;
        static int default_zoom = 2;
        Main mp;
        Bitmap bmp = null;
        uint[] chr_cache = null;
        public int highlight = -1;
        public int preselect = -1;
        public bool dualpage = false; // for selecting 2k CHR pages
        bool modal = false;

        void cache_tile(int i)
        {
            int cc_tiles = 64 * mp.chr_count;
            mp.chr_cache(i, i, chr_cache, Main.GREY);
            mp.chr_cache(i, cc_tiles + i, chr_cache, Main.HIGHLIGHT);
            mp.chr_cache(i,( cc_tiles * 2) + i, chr_cache, Main.PRESELECT);
        }

        void cache()
        {
            int cc_tiles = 64 * mp.chr_count;
            chr_cache = new uint[3 * cc_tiles * 64];
            for (int i = 0; i < (mp.chr_count * 64); ++i)
            {
                cache_tile(i);
            }
        }

        void redraw_page(BitmapData d, int page, bool highlight, bool export=false)
        {
            if (page < 0 || page >= mp.chr_count) return;

            int z = zoom;
            int yo = page * 32;
            int to = page * 64;
            if (highlight)
                to += (64 * mp.chr_count); // use HIGHLIGHT version
            else if (page == preselect || (dualpage && page == (preselect ^ 1)))
                to += (2 * 64 * mp.chr_count); // use PRESELECT version

            if (export)
            {
                yo = 0;
                z = 1;
            }

            for (int y=0; y<4; ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    int t = x + (y * 16);
                    if (sprite)
                    {
                        t = (((x * 2) & 31) ^ (y & 1)) + ((y & (~1)) * 16);
                    }
                    Main.chr_blit(d, chr_cache, to + t, x * 8, yo + y * 8, z);
                }
            }
        }

        BitmapData draw_lock()
        {
            return bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
        }

        void draw_unlock(BitmapData d)
        {
            bmp.UnlockBits(d);
            pictureBox.Image = bmp;
        }

        void redraw()
        {
            if (mp.chr_count < 1)
            {
                bmp = null;
                return;
            }

            int w = 128 * zoom;
            int h = 32 * mp.chr_count * zoom;

            if (bmp == null || bmp.Width != w || bmp.Height != h)
            {
                bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            }
            BitmapData d = draw_lock();
            for (int page = 0; page < mp.chr_count; ++page)
            {
                redraw_page(d, page, page == highlight || (dualpage && page == (highlight^1)));
            }
            draw_unlock(d);
        }

        public void refresh_all()
        {
            cache();
            redraw();
        }

        public void refresh_chr(int tile)
        {
            cache_tile(tile);
            redraw();
        }

        public void refresh_metatile(int page) { } // ignore

        public void refresh_map(int map) { } // ignore

        public void refresh_close() { this.Close(); }

        public CHRSelect(Main parent, bool modal_=false)
        {
            zoom = default_zoom;
            mp = parent;
            modal = modal_;
            InitializeComponent();
            if (modal)
            {
                actionToolStripMenuItem.Enabled = false;
                modeToolStripMenuItem.Enabled = false;
                undoToolStripMenuItem.Enabled = false; // just in case action doesn't disable it too
                pictureBox.ContextMenuStrip = null;
            }
        }

        private void CHRSelect_Shown(object sender, EventArgs e)
        {
            cache();
            updateZoom();
            //redraw(); // updateZoom redraws
            if (preselect >= 0)
                flowLayoutPanel.AutoScrollPosition = new Point(0,(preselect * 32 * zoom)-64);
        }

        private void updateSpriteMode()
        {
            backgroundToolStripMenuItem.Checked = !sprite;
            spriteToolStripMenuItem.Checked = sprite;
            redraw();
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sprite = false; updateSpriteMode();
        }

        private void spriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sprite = true; updateSpriteMode();
        }

        private void updateZoom()
        {
            default_zoom = zoom;
            zoom1xToolStripMenuItem.Checked = zoom == 1;
            zoom2xToolStripMenuItem.Checked = zoom == 2;
            zoom3xToolStripMenuItem.Checked = zoom == 3;
            zoom4xToolStripMenuItem.Checked = zoom == 4;

            const int dspan = 2 * 128;
            int span = zoom * 128;
            int w = (301-dspan)+span;
            if (w < 187) w = 187; // make sure options are visible
            Width = w;

            redraw();
        }

        private void zoom1xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 1; updateZoom();
        }

        private void zoom2xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 2; updateZoom();
        }

        private void zoom3xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 3; updateZoom();
        }

        private void zoom4xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 4; updateZoom();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / (zoom * 8);
            int y = e.Y / (zoom * 8);
            int page = y / 4;
            int t = x + (y * 16);
            if (sprite)
            {
                t = (((x * 2) & 31) ^ (y & 1)) + ((y & (~1)) * 16);
            }
            toolStripStatusLabel.Text = string.Format("CHR {0:X2}:{1:X2}",page,t&63);
            if (page != highlight)
            {
                BitmapData d = draw_lock();
                redraw_page(d, highlight, false);
                if (dualpage) redraw_page(d, highlight^1, false);
                redraw_page(d, page, true);
                if (dualpage) redraw_page(d, page^1, true);
                draw_unlock(d);
                highlight = page;
            }
        }

        private void CHRSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            mp.remove_refresh(this);
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Save Image";
            d.DefaultExt = "png";
            d.Filter = "PNG Image (*.png)|*.png|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + ".chr.png";
            if (d.ShowDialog() == DialogResult.OK)
            {
                highlight = -1;
                redraw();
                try
                {
                    bmp.Save(d.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save image:\n" + d.FileName + "\n\n" + ex.ToString(), "Image save error!");
                }
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
        }

        private void importPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (highlight < 0 || highlight >= mp.chr_count)
            {
                MessageBox.Show("No CHR selected.","CHR Import Error!");
                return;
            }

            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Import CHR Image";
            d.DefaultExt = "png";
            d.Filter = "PNG Image (*.png)|*.png|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + string.Format(".chr.{0:X2}.png",highlight);
            if (d.ShowDialog() == DialogResult.OK)
            {
                Image img;
                Bitmap b;
                try
                {
                    img = Image.FromFile(d.FileName);
                    b = new Bitmap(img);
                    b = new Bitmap(b); // does this need an extra copy to work around a .NET bug?
                    b = b.Clone(new Rectangle(0,0,b.Width,b.Height),PixelFormat.Format32bppArgb); // convert to ARGB32
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open image:\n\n" + ex.ToString(),"CHR Import Error!");
                    return;
                }

                if (b.Width != 128 || b.Height != 32)
                {
                    MessageBox.Show("Image must be 128 x 32 pixels.", "CHR Import Error!");
                    return;
                }

                mp.rom_modify_start(); // begin a single undo step
                int co = mp.chr_offset + (highlight * 1024);
                for (int t=0; t<64; ++t)
                {
                    int tx = t % 16;
                    int ty = t / 16;
                    if (sprite)
                    {
                        tx = (t >> 1) % 16;
                        ty = (t & 1) | ((t>>4) & 2);
                    }
                    int ox = tx * 8;
                    int oy = ty * 8;
                    for (int y=0; y<8; ++y)
                    {
                        byte p0 = 0;
                        byte p1 = 0;
                        for (int x=0; x<8; ++x)
                        {
                            // take average of RGB and use top 2 bits as greyscale colour index
                            Color c = b.GetPixel(ox+x,oy+y);
                            int a = (((int)c.R + (int)c.G + (int)c.B) / 3) >> 6;
                            p0 <<= 1;
                            p1 <<= 1;
                            p0 |= (byte)((a >> 0) & 1);
                            p1 |= (byte)((a >> 1) & 1);
                        }
                        mp.rom_modify(co+(t*16)+0+y, p0, true);
                        mp.rom_modify(co+(t*16)+8+y, p1, true);
                    }
                }
                mp.refresh_all();
            }
        }

        private void exportPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (highlight < 0 || highlight >= mp.chr_count)
            {
                MessageBox.Show("No CHR selected.","CHR Export Error!");
                return;
            }

            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Export CHR Image";
            d.DefaultExt = "png";
            d.Filter = "PNG Image (*.png)|*.png|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + string.Format(".chr.{0:X2}.png",highlight);
            if (d.ShowDialog() == DialogResult.OK)
            {
                Bitmap b = new Bitmap(128, 32, PixelFormat.Format32bppArgb);
                BitmapData bd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                redraw_page(bd,highlight,false,true);
                b.UnlockBits(bd);
                try
                {
                    b.Save(d.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save image:\n" + d.FileName + "\n\n" + ex.ToString(), "Image save error!");
                }
            }
        }

        private void importCHRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (highlight < 0 || highlight >= mp.chr_count)
            {
                MessageBox.Show("No CHR selected.","CHR Import Error!");
                return;
            }

            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Import CHR Binary";
            d.DefaultExt = "chr";
            d.Filter = "CHR Binary (*.chr;*.bin)|*.chr;*.bin|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + string.Format(".{0:X2}.chr",highlight);
            if (d.ShowDialog() == DialogResult.OK)
            {
                byte[] chr;
                try
                {
                    chr = File.ReadAllBytes(d.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open CHR:\n\n" + ex.ToString(),"CHR Import Error!");
                    return;
                }

                mp.rom_modify_start(); // begin a single undo step
                int co = mp.chr_offset + (highlight * 1024);
                int len = chr.Length;
                if (len > 1024) len = 1024;
                for (int i=0; i<len; ++i)
                    mp.rom_modify(co+i,chr[i],true);
                mp.refresh_all();
            }
        }

        private void exportCHRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (highlight < 0 || highlight >= mp.chr_count)
            {
                MessageBox.Show("No CHR selected.","CHR Export Error!");
                return;
            }

            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Export CHR Binary";
            d.DefaultExt = "chr";
            d.Filter = "CHR Binary (*.chr;*.bin)|*.chr;*.bin|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + string.Format(".{0:X2}.chr",highlight);
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int co = mp.chr_offset + (1024 * highlight);
                    byte[] chr = new byte[1024];
                    Array.Copy(mp.rom,co,chr,0,1024);
                    File.WriteAllBytes(d.FileName,chr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save CHR:\n" + d.FileName + "\n\n" + ex.ToString(), "Image save error!");
                }
            }
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / (zoom * 8);
            int y = e.Y / (zoom * 8);
            int page = y / 4;
            int t = x + (y * 16);
            if (sprite)
            {
                t = (((x * 2) & 31) ^ (y & 1)) + ((y & (~1)) * 16);
            }
            if (t >= 0 && t < (mp.chr_count * 64))
            {
                if (!modal)
                {
                    mp.add_chr_edit(t,sprite);
                }
                else
                {
                    highlight = t / 64;
                    Close();
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void CHRSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape ||
                e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Return)
                Close();
        }
    }
}
