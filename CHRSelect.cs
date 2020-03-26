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
        Main parent;
        Bitmap bmp = null;

        static readonly uint[] GREY =
        {
            0xFF000000,
            0xFF555555,
            0xFFAAAAAA,
            0xFFFFFFFF
        };

        void redraw()
        {
            if (parent.chr_count < 1)
            {
                bmp = null;
                return;
            }

            const int w = 128;
            int h = 32 * parent.chr_count;

            bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData bmpd = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, bmp.PixelFormat);
            unsafe
            {
                uint* braw = (uint*)bmpd.Scan0.ToPointer();
                int stride = bmpd.Stride / 4;

                fixed (byte* rom = parent.rom)
                {
                    for (int page = 0; page < parent.chr_count; ++page)
                    {
                        for (int ty = 0; ty < 4; ++ty)
                        {
                            for (int tx = 0; tx < 16; ++tx)
                            {
                                int ti = tx + (ty * 16);
                                if (sprite) // rearrange tile pairs vertically for sprite mode
                                {
                                    ti = (((tx << 1) & 31) ^ (ty & 1)) + ((ty & 2) * 16);
                                }

                                byte* chr = rom + parent.chr_offset + (ti * 16) + (page * 1024);
                                int ox = tx * 8;
                                int oy = (ty * 8) + (page * 32);

                                uint* pix = braw + (stride * oy) + ox;
                                for (int y=0; y<8; ++y)
                                {
                                    byte p0 = chr[y];
                                    byte p1 = chr[y + 8];
                                    for (int x=0; x<8; ++x)
                                    {
                                        int p = ((p0 >> 7) & 1) | ((p1 >> 6) & 2);
                                        pix[x] = GREY[p];
                                        p0 <<= 1;
                                        p1 <<= 1;
                                    }
                                    pix += stride;
                                }
                            }
                        }
                    }
                }
            }
            bmp.UnlockBits(bmpd);
            pictureBox.Image = bmp;
            // TODO how do we make this scrolly?
        }

        public CHRSelect(Main parent_)
        {
            parent = parent_;
            InitializeComponent();
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
    }
}
