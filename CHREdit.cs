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
        const uint SELECT = 0xFFFF0000;
        bool drawing = false;
        int zoom = 12;
        static int default_zoom = 12;
        bool grid = false;
        static bool default_grid = false;

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
            for (int i=0; i<9; ++i)
                mp.chr_cache(tiles[i],i,chr_cache,Main.GREY);

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
                bpal = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            d = bpal.LockBits(new Rectangle(0, 0, bpal.Width, bpal.Height), ImageLockMode.WriteOnly, bpal.PixelFormat);

            for (int i=0; i<4; ++i)
            {
                Main.draw_box(d,i*h,0,h,h,Main.GREY[i]);
                if (i == color_select)
                {
                    Main.draw_outbox(d,i*h,0,h,h,SELECT);
                    Main.draw_outbox(d,(i*h)+1,1,h-2,h-2,SELECT);
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
                color_select = e.X / (zoom * 6);
                if (color_select < 0) color_select = 0;
                if (color_select > 3) color_select = 3;
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

            int co = mp.chr_offset + (t*16) + py;
            if (valid)
            {
                int sh = 7-px;
                byte p0 = mp.rom[co+0];
                byte p1 = mp.rom[co+8];
                p = (  (p0 >> sh)        & 1) |
                    ((((p1 >> sh)) << 1) & 2);

                if (e.Button == MouseButtons.Left && drawing && q == 4)
                {
                    byte bit = (byte)(1 << sh);
                    byte mask = (byte)~bit;
                    byte np0 = (byte)(p0 & mask);
                    byte np1 = (byte)(p1 & mask);
                    if ((color_select & 1) != 0) np0 |= bit;
                    if ((color_select & 2) != 0) np1 |= bit;

                    bool changed = false;
                    changed |= mp.rom_modify(co+0,np0,true);
                    changed |= mp.rom_modify(co+8,np1,true);
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
                case Keys.D1: color_select = 0; break;
                case Keys.D2: color_select = 1; break;
                case Keys.D3: color_select = 2; break;
                case Keys.D4: color_select = 3; break;
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
            Size = new Size(w, (336-(dspan+dspan/4))+(span+span/4));
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
    }
}
