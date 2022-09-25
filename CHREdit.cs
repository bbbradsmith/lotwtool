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
    public partial class CHREdit : Form, RomRefresh
    {
        Main mp;
        int[] tiles;
        uint[] chr_cache;
        bool sprite;
        Bitmap bmp = null;
        Bitmap bpal = null;
        int color_select = 0;
        const uint SELECT = 0xFFFFFF00;
        bool drawing = false;
        int zoom = 12;
        static int default_zoom = 12;
        bool grid = false;
        static bool default_grid = false;
        bool map_palette = false;
        static bool default_map_palette = false;

        void find_tiles(int tile)
        {
            int[] FLAT = { -17,-16,-15, -1, 0, 1,  15,16,17 };
            int[] S0 =   { -17,-15,-13, -2, 0, 2,  -1, 1, 3 };
            int[] S1 =   {  -3, -1,  1, -2, 0, 2,  13,15,17 };

            int[] adjacent = FLAT;
            if (sprite)
                adjacent = ((tile & 1)==0) ? S0 : S1;

            tiles = new int[9];
            for (int i=0; i<9; ++i)
                tiles[i] = tile + adjacent[i];

            Text = string.Format("CHR {0:X2}:{1:X2}",tile/64,tile%64);
        }

        void redraw()
        {
            uint[] palette = new uint[16];
            if (!sprite)
            {
                if (map_palette) mp.last_map_palette.CopyTo(palette,0);
                else             Main.CHREDIT.CopyTo(palette,0);
            }
            else
            {
                Main.GREY.CopyTo(palette,0);
            }

            for (int i=0; i<9; ++i)
            {
                if (!sprite)
                    mp.chr_cache(tiles[i],i,chr_cache,palette);
                else
                    mp.spr_cache(tiles[i],i,chr_cache,palette);
            }

            // CHR view
            int span = 8 * zoom;
            int w = 24 * zoom;
            int h = 24 * zoom;
            if (bmp == null || bmp.Width != w || bmp.Height != h)
                bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData d = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);

            // CHR tiles
            int[] XO = { 0, 8, 16, 0, 8, 16, 0, 8, 16 };
            int[] YO = { 0, 0, 0, 8, 8, 8, 16, 16, 16 };
            for (int i=0; i<9; ++i)
                Main.chr_blit(d, chr_cache, i, XO[i], YO[i], zoom);

            // grid
            if (grid)
            {
                for (int i=0; i<9; ++i)
                {
                    Main.draw_hline(d,span,span+(i*zoom),span+1,Main.GRID);
                    Main.draw_vline(d,span+(i*zoom),span,span+1,Main.GRID);
                }
            }
            else
            {
                Main.draw_outbox(d,span-1,span-1,span+2,span+2,Main.GRID);
            }

            bmp.UnlockBits(d);
            pictureBox.Image = bmp;

            // palette selector
            // width same as pictureBox
            h = w / 4;
            if (bpal == null || bpal.Width != w || bpal.Height != h)
                bpal = new Bitmap(w, h*4, PixelFormat.Format32bppArgb);
            d = bpal.LockBits(new Rectangle(0, 0, bpal.Width, bpal.Height), ImageLockMode.WriteOnly, bpal.PixelFormat);

            for (int i=0; i<16; ++i)
            {
                int ix = i%4;
                int iy = i/4;
                Main.draw_box(d,ix*h,iy*h,h,h,palette[i]);
                if (i == color_select)
                {
                    Main.draw_outbox(d,ix*h,iy*h,h,h,SELECT);
                    Main.draw_outbox(d,(ix*h)+1,(iy*h)+1,h-2,h-2,SELECT);
                }
            }

            bpal.UnlockBits(d);
            paletteBox.Image = bpal;
        }

        public void refresh_all() {  redraw(); }

        public void refresh_chr(int tile)
        {
            bool changed = false;
            foreach (int t in tiles)
                changed |= (tile == t);
            if (changed) redraw();
        }

        public void refresh_metatile(int page) { } // ignore

        public void refresh_map(int map) { } // ignore

        public void refresh_close() { this.Close(); }

        public CHREdit(Main parent, int tile, bool sprite_)
        {
            mp = parent;
            sprite = sprite_;
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;
            find_tiles(tile);
            chr_cache = new uint[9 * 64];
            zoom = default_zoom;
            grid = default_grid;
            gridToolStripMenuItem.Checked = grid;
            map_palette = default_map_palette;
            lastMapPaletteToolStripMenuItem.Checked = map_palette;
            updateZoom();
            //redraw(); // handled by updateZoom
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
            redraw();
        }

        private void paletteBox_MouseClick(object sender, MouseEventArgs e)
        {
            paletteBox_MouseMove(sender, e);
        }

        private void paletteBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                int cx = e.X / (zoom * 6);
                int cy = e.Y / (zoom * 6);
                if (cx < 0 || cx > 3) return;
                if (cy < 0 || cy > 3) return;
                color_select = cx + (cy * 4);
                redraw();
            }
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = false;

            int span = zoom * 8;
            int xq = e.X / span;
            int yq = e.Y / span;
            int q = (yq*3)+xq;

            if (xq < 0 || xq >= 3) return;
            if (yq < 0 || yq >= 3) return;
            int t = tiles[q];

            if (e.Button == MouseButtons.Left)
            {
                if (q == 4)
                {
                    mp.rom_modify_start(); // start drawing
                    drawing = true;
                }
                else // switch tile
                {
                    if (t >= 0 && t < (mp.chr_count * 64))
                    {
                        find_tiles(t);
                        redraw();
                    }
                }
            }

            pictureBox_MouseMove(sender, e);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int span = zoom * 8;
            int xq = e.X / span;
            int yq = e.Y / span;
            int q = (yq*3)+xq;

            if (xq < 0 || xq >= 3) return;
            if (yq < 0 || yq >= 3) return;
            int t = tiles[q];

            bool valid = (t >= 0 && t < (mp.chr_count * 64));
            int px = (e.X % span) / zoom;
            int py = (e.Y % span) / zoom;
            int p = -1;

            string status = "...";

            int co = mp.chr_offset + (t*32); // 4bpp chunky CHR
            if (sprite) // 2bpp packed SPR
            {
                co = mp.spr_offset;
                co += (t >> 2) * 64;
                co += (t &  2) * 8;
                co += (t &  1) * 8;
            }

            if (valid)
            {
                int bo0 = 0;
                int bo1 = 0;
                int sh = 0;

                if (!sprite)
                {
                    bo0 = co + (py*4) + (px/2);
                    sh = (4-((px&1)*4));
                    p = (mp.rom[bo0] >> sh) & 0x0F;
                }
                else
                {
                    bo0 = co+ 0+py+(px/8);
                    bo1 = co+32+py+(px/8);
                    sh = (7-(px&7));
                    byte pb0 = (byte)((mp.rom[bo0] >> sh) & 0x01);
                    byte pb1 = (byte)((mp.rom[bo1] >> sh) & 0x01);
                    p = pb0 | (pb1 << 1);
                }

                if (e.Button == MouseButtons.Left && drawing && q == 4)
                {
                    bool changed = false;

                    if (!sprite)
                    {
                        byte mask = (byte)~(0x0F << sh);
                        byte bm = (byte)((mp.rom[bo0] & mask) | ((color_select & 0xF) << sh));
                        changed |= mp.rom_modify(bo0,bm,true);
                    }
                    else
                    {
                        byte mask = (byte)~(0x01 << sh);
                        byte bm0 = (byte)((mp.rom[bo0] & mask) | ((color_select & 1) << sh));
                        byte bm1 = (byte)((mp.rom[bo1] & mask) | (((color_select & 2) >> 1) << sh));
                        changed |= mp.rom_modify(bo0,bm0,true);
                        changed |= mp.rom_modify(bo1,bm1,true);
                    }

                    if (changed)
                    {
                        //redraw(); // refresh_chr will do this
                        mp.refresh_chr(t);
                    }
                    p = color_select;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    color_select = p;
                    redraw();
                }

                status = string.Format("{0:X2}:{1:X2} {2:D},{3:D}={4:D}",t/64,t%64,px,py,p);
            }

            toolStripStatusLabel.Text = status;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }

        private void CHREdit_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1: color_select =  0; break;
                case Keys.D2: color_select = 13; break;
                case Keys.D3: color_select = 14; break;
                case Keys.D4: color_select = 15; break;
                default:
                    return;
            }
            redraw();
        }

        private void CHREdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            mp.remove_refresh(this);
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string HELPTEXT =
                "LMB = Draw\n" +
                "RMB = Pick\n" +
                "1-4 = Colour";
            MessageBox.Show(HELPTEXT,"CHR Edit Help");
        }

        private void updateZoom()
        {
            default_zoom = zoom;
            zoom4xToolStripMenuItem.Checked  = zoom == 4;
            zoom8xToolStripMenuItem.Checked  = zoom == 8;
            zoom12xToolStripMenuItem.Checked = zoom == 12;
            zoom16xToolStripMenuItem.Checked = zoom == 16;

            int dspan = 8 * 24;
            int span = zoom * 24;
            paletteBox.Top = (225-dspan)+span;
            int w = (208-dspan)+span;
            if (w < 137) w = 137; // keep Options menu visible
            Size = new Size(w, (336-(dspan+dspan/4))+(span+span));
            redraw();
        }

        private void zoom4xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 4; updateZoom();
        }

        private void zoom8xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 8; updateZoom();
        }

        private void zoom12xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 12; updateZoom();
        }

        private void zoom16xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 16; updateZoom();
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = !grid;
            default_grid = grid;
            gridToolStripMenuItem.Checked = grid;
            redraw();
        }

        private void lastMapPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map_palette = !map_palette;
            default_map_palette = map_palette;
            lastMapPaletteToolStripMenuItem.Checked = map_palette;
            redraw();
        }
    }
}
