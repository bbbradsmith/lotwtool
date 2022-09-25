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
        Bitmap bbit = null;
        int[] color_select = {0,15};
        int bit_select = 1;
        const uint SELECT = 0xFFFFFF00;
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
            {
                if (!sprite)
                    mp.chr_cache(tiles[i],i,chr_cache);
                else
                    mp.spr_cache(tiles[i],i,chr_cache);
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
                Main.chr_blit(d, chr_cache, i, XO[i], YO[i], zoom,15);

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

            // bit selector
            if (bbit == null || bbit.Width != w || bbit.Height != span)
                bbit = new Bitmap(w,span,PixelFormat.Format32bppArgb);
            d = bbit.LockBits(new Rectangle(0,0,bbit.Width,bbit.Height), ImageLockMode.WriteOnly, bbit.PixelFormat);
            for (int i=0; i<2; ++i)
            {
                int ix = i * (w/2);
                int iy = 0;
                Main.draw_box(d,ix,iy*span,w/2,span,(i>0)?0xFFFFFFFF:0xFF000000);
                if (i == bit_select)
                {
                    Main.draw_outbox(d,ix,iy*span,w/2,span,SELECT);
                    Main.draw_outbox(d,ix+1,iy+1,(w/2)-2,span-2,SELECT);
                }
            }
            bbit.UnlockBits(d);
            bitBox.Image = bbit;

            // palette selector
            // width/height same as pictureBox
            h = w / 4;
            if (bpal == null || bpal.Width != w || bpal.Height != h)
                bpal = new Bitmap(w, h*4, PixelFormat.Format32bppArgb);
            d = bpal.LockBits(new Rectangle(0, 0, bpal.Width, bpal.Height), ImageLockMode.WriteOnly, bpal.PixelFormat);
            for (int i=0; i<16; ++i)
            {
                int ix = i%4;
                int iy = i/4;
                Main.draw_box(d,ix*h,iy*h,h,h,Main.MSX_PALETTE[i]);
                if (i == color_select[bit_select])
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
                color_select[bit_select] = cx + (cy * 4);
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
            int[] pc = { -1, -1 };

            string status = "...";

            int co = 0;
            if (!sprite)
            {
                co = mp.chr_offset;
                co += (t >> 8) * 4096;
                co += (t & 0xFF) * 8;
            }
            else
            {
                co = mp.spr_offset;
                co += (t >> 2) * 32;
                co += (t &  2) * 8;
                co += (t &  1) * 8;
            }

            if (valid)
            {
                int bo0 = 0;
                int bo1 = 0;
                int sh = 0;

                bo0 = co + py;
                sh = (7 - px);
                p = (mp.rom[bo0] >> sh) & 1;
                if (!sprite)
                {
                    bo1 = co + py + 2048;
                    pc[0] = mp.rom[bo1] & 0xF;
                    pc[1] = mp.rom[bo1] >> 4;
                }

                if (e.Button == MouseButtons.Left && drawing && q == 4)
                {
                    bool changed = false;

                    byte mask = (byte)~(0x01 << sh);
                    byte bm0 = (byte)((mp.rom[bo0] & mask) | (bit_select << sh));
                    changed |= mp.rom_modify(bo0,bm0,true);
                    if (!sprite)
                    {
                        byte cmask = (byte)~(0x0F << (4*bit_select));
                        byte bm1 = (byte)((mp.rom[bo1] & cmask) | (color_select[bit_select] << (4 * bit_select)));
                        changed |= mp.rom_modify(bo1,bm1,true);
                    }

                    if (changed)
                    {
                        //redraw(); // refresh_chr will do this
                        mp.refresh_chr(t);
                    }
                    p = bit_select;
                    pc[bit_select] = color_select[bit_select];
                }
                else if (e.Button == MouseButtons.Right)
                {
                    bit_select = p;
                    if (!sprite)
                    {
                        color_select[bit_select] = pc[bit_select];
                    }
                    redraw();
                }

                status = string.Format("{0:X2}:{1:X2} {2:D},{3:D}={4:D}",t/64,t%64,px,py,p);
                if (!sprite)
                    status += string.Format(" {0:X},{1:X}",pc[0],pc[1]);
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
                case Keys.D1: bit_select = 0; break;
                case Keys.D2: bit_select = 1; break;
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
                "1-2 = Bit Colour";
            MessageBox.Show(HELPTEXT,"CHR Edit Help");
        }

        private void updateZoom()
        {
            default_zoom = zoom;
            zoom4xToolStripMenuItem.Checked  = zoom == 4;
            zoom8xToolStripMenuItem.Checked  = zoom == 8;
            zoom12xToolStripMenuItem.Checked = zoom == 12;
            zoom16xToolStripMenuItem.Checked = zoom == 16;

            int span = zoom * 24;
            int bspan = zoom * 8;
            bitBox.Top = pictureBox.Top + span + 6;
            paletteBox.Top = bitBox.Top + bspan + 6;
            int w = (208-bspan)+span;
            if (w < 137) w = 137; // keep Options menu visible
            Size = new Size(w, 103 + span + bspan + span + 12);
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
        private void bitBox_MouseClick(object sender, MouseEventArgs e)
        {
            int w = 24 * zoom;
            bit_select = (e.X * 2 / w) & 1;
            redraw();
        }
    }
}
