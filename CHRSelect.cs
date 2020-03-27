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
    public partial class CHRSelect : Form
    {
        bool sprite = false;
        int zoom = 2;
        Main mp;
        Bitmap bmp = null;
        uint[] chr_cache = null;

        static readonly uint[] GREY =
        {
            0xFF000000,
            0xFF555555,
            0xFFAAAAAA,
            0xFFFFFFFF
        };

        void cache_tile(int rom_index, int cache_index, uint[] palette)
        {
            int ro = mp.chr_offset + (rom_index * 16);
            int co = cache_index * 64;
            for (int y=0; y<8; ++y)
            {
                byte p0 = mp.rom[ro + y];
                byte p1 = mp.rom[ro + y + 8];
                for (int x = 0; x < 8; ++x)
                {
                    int p = ((p0 >> 7) & 1) | ((p1 >> 6) & 2);
                    chr_cache[co + x + (y * 8)] = palette[p];
                    p0 <<= 1;
                    p1 <<= 1;
                }
            }
        }

        void cache()
        {
            chr_cache = new uint[64 * mp.chr_count * 64];
            for (int i = 0; i < (mp.chr_count * 64); ++i)
            {
                cache_tile(i, i, GREY);
            }
        }

        void blit(BitmapData bd, int tile, int x, int y)
        {
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                fixed (uint* fcc = chr_cache)
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
            BitmapData bmpd = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, bmp.PixelFormat);
            for (int y = 0; y < (mp.chr_count * 4); ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    int t = x + (y * 16);
                    if (sprite)
                    {
                        t = (((x * 2) & 31) ^ (y & 1)) + ((y & (~1)) * 16);
                    }
                    blit(bmpd, t, x * 8 * zoom, y * 8 * zoom);
                }
            }
            bmp.UnlockBits(bmpd);
            pictureBox.Image = bmp;
            pictureBox.Width = w;
            pictureBox.Height = h;
        }

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
        }
    }
}
