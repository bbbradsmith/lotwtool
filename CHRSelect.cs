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
    public partial class CHRSelect : Form, RomRefresh
    {
        bool sprite = false;
        int zoom = 2;
        Main mp;
        Bitmap bmp = null;
        uint[] chr_cache = null;
        int highlight = -1;

        static readonly uint[] GREY =
        {
            0xFF000000,
            0xFF555555,
            0xFFAAAAAA,
            0xFFFFFFFF,
        };

        static readonly uint[] RED =
        {
            0xFF331111,
            0xFF663333,
            0xFFCC4444,
            0xFFFFCCCC,
        };

        void cache()
        {
            int cc_tiles = 64 * mp.chr_count;
            chr_cache = new uint[2 * cc_tiles * 64];
            for (int i = 0; i < (mp.chr_count * 64); ++i)
            {
                mp.chr_cache(i, i, chr_cache, GREY);
                mp.chr_cache(i, cc_tiles + i, chr_cache, RED);
            }
        }

        void redraw_page(BitmapData d, int page, bool highlight)
        {
            if (page < 0 || page >= mp.chr_count) return;

            int yo = page * 32;
            int to = page * 64;
            if (highlight) to += (64 * mp.chr_count); // use RED version
            for (int y=0; y<4; ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    int t = x + (y * 16);
                    if (sprite)
                    {
                        t = (((x * 2) & 31) ^ (y & 1)) + ((y & (~1)) * 16);
                    }
                    mp.chr_blit(d, chr_cache, to + t, x * 8, yo + y * 8, zoom);
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

            bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData d = draw_lock();
            for (int page = 0; page < mp.chr_count; ++page)
            {
                redraw_page(d, page, page == highlight);
            }
            draw_unlock(d);
        }

        public void refresh_all() { } // TODO
        public void refresh_chr(int tile) { } // TODO
        public void refresh_metatile(int page) { }
        public void refresh_close() { this.Close(); }

        public CHRSelect(Main parent)
        {
            mp = parent;
            InitializeComponent();
            cache();
            redraw();
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
            zoom1xToolStripMenuItem.Checked = zoom == 1;
            zoom2xToolStripMenuItem.Checked = zoom == 2;
            zoom3xToolStripMenuItem.Checked = zoom == 3;
            zoom4xToolStripMenuItem.Checked = zoom == 4;
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
            toolStripStatusLabel.Text = string.Format("CHR ${0:X2} {1:D}",page,t&63);
            if (page != highlight)
            {
                BitmapData d = draw_lock();
                redraw_page(d, highlight, false);
                redraw_page(d, page, true);
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
    }
}
