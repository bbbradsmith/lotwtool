﻿using System;
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
    public partial class MapEditTile : Form
    {
        MapEdit me;
        Main mp;
        Bitmap bmp;
        Bitmap bpal;
        uint[] chr_cache;
        const uint BLACK = 0xFF000000;
        const uint SELECT = 0xFFFFFFFF;
        int zoom = 2;
        static int default_zoom = 2;

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

        public void cache()
        {
            chr_cache = new uint[2 * 256 * 64];
            int[] chri = me.chr_set();
            for (int i=0; i<4; ++i)
            {
                cache_page(i,chri[i]);
            }
        }

        public void redraw()
        {
            int ro = 16 + (1024 * me.room);
            byte metatile_page = mp.rom[ro+0x300];
            int mto = 16 + (1024 * 8 * 9) + (metatile_page * 256);
            if ((mto+256) > mp.rom.Length) return;

            // tile selector
            int w = 16 * 8 * zoom;
            int h = 16 * 8 * zoom;
            if (bmp == null || bmp.Width != w || bmp.Height != h)
                bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData d = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            int[] XO = { 0, 0, 8, 8 };
            int[] YO = { 0, 8, 0, 8 };

            int ts = me.draw_tile & 63;
            for (int t=0; t<64; ++t)
            {
                int tx = t % 8;
                int ty = t / 8;
                for (int i=0; i<4; ++i)
                {
                    int mt = mp.rom[mto+(t*4)+i];
                    if (t == ts) mt += 256; // highlight
                    Main.chr_blit(d, chr_cache,mt,(tx*16)+XO[i],(ty*16)+YO[i],zoom);
                }
            }

            bmp.UnlockBits(d);
            pictureBox.Image = bmp;

            // palette selector
            // width same as pictureBox
            h = 8 * zoom;
            if (bpal == null || bpal.Width != w || bpal.Height != h)
                bpal = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            d = bpal.LockBits(new Rectangle(0, 0, bpal.Width, bpal.Height), ImageLockMode.WriteOnly, bpal.PixelFormat);

            int pw = w / 16;
            for (int i=0; i<16; ++i)
            {
                Main.draw_box(d,i*pw,0,pw,h,me.palette[i/4][i%4]);
            }
            int ps = me.draw_tile >> 6;
            int pw4 = pw*4;
            Main.draw_outbox(d,ps*pw4,0,pw4,h,SELECT);
            Main.draw_outbox(d,ps*pw4+1,1,pw4-2,h-2,BLACK);

            bpal.UnlockBits(d);
            paletteBox.Image = bpal;
        }

        public MapEditTile(MapEdit me_, Main parent)
        {
            mp = parent;
            me = me_;
            InitializeComponent();
            int x = me.room % 4;
            int y = me.room / 4;
            Text = string.Format("Tiles {0},{1} ({2})",x,y,(y*4)+x);
            zoom = default_zoom;
            cache();
            updateZoom();
            //redraw(); // done by updateZoom
        }

        private void MapEditTile_FormClosing(object sender, FormClosingEventArgs e)
        {
            me.tilepal = null;
        }

        private void updateZoom()
        {
            default_zoom = zoom;
            zoom1xToolStripMenuItem.Checked = zoom == 1;
            zoom2xToolStripMenuItem.Checked = zoom == 2;
            zoom3xToolStripMenuItem.Checked = zoom == 3;
            zoom4xToolStripMenuItem.Checked = zoom == 4;

            const int dspan = 256;
            const int dpspan = 16;
            int span = zoom * 128;
            int pspan = zoom * 8;
            paletteBox.Top = (287-dspan)+span;
            Size = new Size((272-dspan)+span, (364-(dspan+dpspan))+(span+pspan));
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

    }
}