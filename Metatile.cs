using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lotwtool
{
    public partial class Metatile : Form
    {
        Main mp;
        Bitmap tbmp;
        Bitmap mbmp;
        uint[] chr_cache;
        public int[] chr; // 2 CHR pages
        public int[] mt; // 4 tiles of metatile
        int mti = 0; // selected metatile
        const int TZOOM = 2;
        const int MZOOM = 8;
        void cache_tile(int rom_index, int cache_index)
        {
            mp.chr_cache(rom_index, cache_index, chr_cache, Main.GREY);
            mp.chr_cache(rom_index, cache_index+256, chr_cache, Main.HIGHLIGHT);
        }

        void cache_page(int slot, int page)
        {
            for (int i = 0; i < 64; ++i)
            {
                cache_tile((page*64)+i,(slot*64)+i);
            }
        }

        void cache()
        {
            chr_cache = new uint[2 * 256 * 64];
            cache_page(0,chr[0] & (~1));
            cache_page(1,chr[0] | 1);
            cache_page(2,chr[1] & (~1));
            cache_page(3,chr[1] | 1);
        }

        void redraw()
        {
            int zoom = TZOOM;
            int w = 16 * 8 * zoom;
            int h = 16 * 8 * zoom;
            if (tbmp == null || tbmp.Width != w || tbmp.Height != h)
                tbmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData d = tbmp.LockBits(new Rectangle(0, 0, tbmp.Width, tbmp.Height), ImageLockMode.WriteOnly, tbmp.PixelFormat);
            for (int i=0; i<256; ++i)
            {
                int t = i;
                if (i == mt[mti]) t += 256; // assigned tile to metatile
                int x = i % 16;
                int y = i / 16;
                Main.chr_blit(d, chr_cache, t, x*8, y*8, zoom);
            }
            tbmp.UnlockBits(d);
            tileBox.Image = tbmp;

            zoom = MZOOM;
            w = 2 * 8 * zoom;
            h = 2 * 8 * zoom;
            if (mbmp == null || mbmp.Width != w || mbmp.Height != h)
                mbmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            d = mbmp.LockBits(new Rectangle(0, 0, mbmp.Width, mbmp.Height), ImageLockMode.WriteOnly, mbmp.PixelFormat);
            for (int i=0; i<4; ++i)
            {
                int t = mt[i];
                if (i == mti) t += 256; // selected metatile quadrant
                int x = i / 2; // X/Y swapped for metatile
                int y = i % 2;
                Main.chr_blit(d, chr_cache, t, x*8, y*8, zoom);
            }
            mbmp.UnlockBits(d);
            metatileBox.Image = mbmp;
        }

        public Metatile(Main parent, int mt_index, int chr0, int chr1)
        {
            mp = parent;
            chr = new int[2];
            mt = new int[4];
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;

            chr[0] = chr0;
            chr[1] = chr1;
            int mto = mp.map_offset + (1024 * 8 * 9) + (mt_index * 4);
            if ((mto + 4) <= mp.rom.Length)
            {
                for (int i=0; i<4; ++i) mt[i] = mp.rom[mto+i];
            }

            cache();
            redraw();
            Text = string.Format("Metatile {0:X1}:{0:X2}",mt_index/64,mt_index%64);
        }

        private void tileBox_MouseDown(object sender, MouseEventArgs e)
        {
            int c = e.Y / (TZOOM * 8 * 8);

            if (e.Button == MouseButtons.Right && c >= 0 && c < 2)
            {
                CHRSelect cs = new CHRSelect(mp,true);
                cs.StartPosition = FormStartPosition.CenterParent;
                cs.preselect = chr[c];
                cs.dualpage = true;
                cs.sprite = false;
                if (cs.ShowDialog() == DialogResult.OK)
                {
                    if (cs.highlight >= 0 && cs.highlight < mp.chr_count)
                    {
                        chr[c] = cs.highlight & (~1);
                        cache();
                        redraw();
                    }
                }
            }
            tileBox_MouseMove(sender,e);
        }

        private void tileBox_MouseMove(object sender, MouseEventArgs e)
        {
            int tx = e.X / (TZOOM * 8);
            int ty = e.Y / (TZOOM * 8);
            int t = tx + (ty * 16);

            if (tx >= 0 && tx < 16 && ty >= 0 && ty < 16)
            {
                if (e.Button == MouseButtons.Left)
                {
                    mt[mti] = t;
                    redraw();
                }
                toolStripStatusLabel.Text = string.Format("{0:X2}",t);
            }
            toolStripTipLabel.Text = "RMB = Select CHR";
        }

        private void metatileBox_MouseDown(object sender, MouseEventArgs e)
        {
            metatileBox_MouseMove(sender,e);
        }

        private void metatileBox_MouseMove(object sender, MouseEventArgs e)
        {
            int tx = e.Y / (MZOOM * 8); // X/Y swapped for metatile
            int ty = e.X / (MZOOM * 8);
            int t = tx + (ty * 2);

            if (tx >= 0 && tx < 2 && ty >= 0 && ty < 2)
            {
                if (e.Button != MouseButtons.None)
                {
                    mti = t;
                    redraw();
                }
                toolStripStatusLabel.Text = string.Format("{0:X2}",mt[t]);
            }
            toolStripTipLabel.Text = "";
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Metatile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                buttonCancel_Click(sender,e);
            }
            else if (e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Return)
            {
                buttonOK_Click(sender, e);
            }
        }
    }
}
