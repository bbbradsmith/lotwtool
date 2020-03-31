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
            toolStripTipLabel.Text = "RMB = Edit";
            updateTileStatus(me.draw_tile);
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
            paletteBox.Top = (283-dspan)+span;
            Size = new Size((272-dspan)+span, (360-(dspan+dpspan))+(span+pspan));
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

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int tx = e.X / (16 * zoom);
            int ty = e.Y / (16 * zoom);
            int t = (ty * 8) + tx;

            if (e.Button == MouseButtons.Right && tx >= 0 && tx < 8 && ty >= 0 && ty < 8)
            {
                int ro = 16 + (1024 * me.room);
                byte mt_page = mp.rom[ro+0x300];
                byte chr0 = mp.rom[ro+0x305];
                byte chr1 = mp.rom[ro+0x306];
                Metatile m = new Metatile(mp,(mt_page*64)+t,chr0,chr1);
                m.StartPosition = FormStartPosition.CenterParent;
                if (m.ShowDialog() == DialogResult.OK)
                {
                    int mto = 16 + (1024 * 8 * 9) + (mt_page * 256) + (t * 4);

                    bool chrchgd = false;
                    mp.rom_modify_start();
                    chrchgd |= mp.rom_modify(ro+0x305,(byte)m.chr[0],true);
                    chrchgd |= mp.rom_modify(ro+0x306,(byte)m.chr[1],true);
                    bool changed = chrchgd;
                    changed |= mp.rom_modify(mto+0,(byte)m.mt[0],true);
                    changed |= mp.rom_modify(mto+1,(byte)m.mt[1],true);
                    changed |= mp.rom_modify(mto+2,(byte)m.mt[2],true);
                    changed |= mp.rom_modify(mto+3,(byte)m.mt[3],true);
                    if (chrchgd)
                        me.reload_chr();
                    if (changed)
                        mp.refresh_metatile(mt_page);
                }
            }
            else pictureBox_MouseMove(sender,e);
        }

        public void updateTileStatus(byte t, int p = -1)
        {
            string s = string.Format("{0:X2} ({1:X1}:{2:X2})",t,t>>6,t&63);
            if (p >= 0 && p < 16)
            {
                int ro = 16 + (1024 * me.room);
                s += string.Format(" {0:X2}",mp.rom[ro+0x3E0+p]);
            }
            s += " " + me.get_tile_type(t);
            toolStripStatusLabel.Text = s;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int tx = e.X / (16 * zoom);
            int ty = e.Y / (16 * zoom);
            int t = (ty * 8) + tx;

            byte new_tile = (byte)((me.draw_tile & 0xC0) | t);

            if (e.Button == MouseButtons.Left)
            {
                me.draw_tile = new_tile;
                redraw();
            }

            updateTileStatus(new_tile);
        }

        private void paletteBox_MouseDown(object sender, MouseEventArgs e)
        {
            int px = e.X / (8 * zoom);
            if (e.Button == MouseButtons.Right && px >= 0 && px < 16)
            {
                int ro = 16 + (1024 * me.room) + 0x3E0 + px;
                byte old = mp.rom[ro];
                PalettePick p = new PalettePick(old & 63);
                p.StartPosition = FormStartPosition.CenterParent;
                if (p.ShowDialog() == DialogResult.OK)
                {
                    byte np = (byte)p.picked;
                    if (mp.rom_modify(ro, np))
                    {
                        mp.refresh_map(me.room);
                        me.cache();
                        me.redraw(); // not covered by refresh_map
                        me.redraw_info();
                        redraw();
                    }
                }
            }
            else paletteBox_MouseMove(sender,e);
        }

        private void paletteBox_MouseMove(object sender, MouseEventArgs e)
        {
            int px = e.X / (8 * zoom);
            int p = px / 4;
            byte new_tile = (byte)((me.draw_tile & 63) | (p<<6));

            if (e.Button == MouseButtons.Left)
            {
                me.draw_tile = new_tile;
                redraw();
            }

            updateTileStatus(new_tile,px);
        }

        private void MapEditTile_KeyDown(object sender, KeyEventArgs e)
        {
            me.MapEdit_KeyDown(sender,e);
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            updateTileStatus(me.draw_tile); // revert when mouse leaves
        }
    }
}
