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
    public partial class MapEdit : Form, RomRefresh
    {
        int zoom = 1;
        int mode = 0; // 0 terrain edit, 1 item edit
        bool secret = true;
        public int room = 0;
        Main mp;
        Bitmap bmp;
        uint[] chr_cache;
        uint[][] palette;
        public MapEditHex infohex = null;

        void cache_page(int slot, int page)
        {
            for (int i = 0; i < 64; ++i)
            {
                for (int p = 0; p < 4; ++p)
                {
                    mp.chr_cache((page * 64) + i, (slot * 64) + i + (p * 512), chr_cache, palette[p + (slot & 4)]);
                }
            }
        }

        void cache()
        {
            int ro = 16 + (1024 * room);

            palette = new uint[8][];
            for (int i = 0; i < 8; ++i)
            {
                palette[i] = new uint[4];
                for (int j=0; j<4; ++j)
                {
                    int p = mp.rom[ro+0x3E0+(i*4)+j];
                    palette[i][j] = Main.NES_PALETTE[p] | 0xFF000000;
                    if (i>=4 && j==0) palette[i][j] = 0x00000000;
                }
            }

            chr_cache = new uint[4 * 8 * 64 * 64];
            int[] chri = new int[8];
            chri[0] = mp.rom[ro + 0x305];
            chri[2] = mp.rom[ro + 0x306];
            chri[5] = mp.rom[ro + 0x301];
            chri[1] = chri[0] + 1;
            chri[3] = chri[2] + 1;
            chri[4] = 0x3A; // always Roas
            chri[6] = 0x3E; // always items 0
            chri[7] = 0x3F; // always items 0
            for (int i=0; i<8; ++i)
            {
                cache_page(i, chri[i]);
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

        void draw_bg_tile(BitmapData d, int x, int y)
        {
            int ro = 16 + (1024 * room);
            byte t = mp.rom[ro+(x*12)+y];
            int attribute = t >> 6;
            int metatile = t & 63;
            int st = mp.rom[ro+0x302];
            if (!secret) st = -1;

            // metatile tables are in 256 byte pages in bank 9
            byte metatile_page = mp.rom[ro+0x300];
            int mto = 16 + (1024 * 8 * 9) + (metatile_page * 256);
            if ((mto+4) > mp.rom.Length) return;

            int[] XO = { 0, 0, 8, 8 };
            int[] YO = { 0, 8, 0, 8 };

            for (int i=0; i<4; ++i)
            {
                int mt = mp.rom[mto+(metatile*4)+i];
                if (metatile != st)
                    mp.chr_blit(d, chr_cache, mt + (attribute * 512), (x*16)+XO[i], (y*16)+YO[i], zoom);
                else
                    mp.chr_blit_dark(d, chr_cache, mt + (attribute * 512), (x*16)+XO[i], (y*16)+YO[i], zoom);
            }
        }

        void draw_bg(BitmapData d)
        {
            for (int y=0; y<12; ++y)
            {
                for (int x=0; x<64; ++x)
                {
                    draw_bg_tile(d,x,y);
                }
            }
        }

        void redraw()
        {
            int w = 256 * 4 * zoom;
            int h = 192 * zoom;
            bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData d = draw_lock();
            draw_bg(d);
            draw_unlock(d);
        }

        public void render_select(BitmapData d, int room_, int zoom_, bool secret_)
        {
            room = room_;
            zoom = zoom_;
            secret = secret_;
            cache();
            draw_bg(d);
        }

        public void refresh_all() { } // TODO
        public void refresh_chr(int tile) { } // TODO
        public void refresh_metatile(int page) { } // TODO
        public void refresh_close() { this.Close(); }

        public MapEdit(Main parent, int room_)
        {
            mp = parent;
            room = room_;
            InitializeComponent();
            int x = room % 4;
            int y = room / 4;
            Text = string.Format("Map {0},{1} ({2})",x,y,(y*4)+x);
            cache();
            redraw();
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

        private void pictureBox_Click(object sender, EventArgs e)
        {
            // TODO editing
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            //toolStripStatusLabel.Text = string.Format("{0}, {1}",this.Width,this.Height);
            // TODO
        }

        private void MapEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (infohex != null) infohex.Close();
            mp.remove_refresh(this);
            mp.remove_map_edit(this);
        }

        private void showSecretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            secret = !secret;
            showSecretToolStripMenuItem.Checked = secret;
            redraw();
        }

        private void infoHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Main.raise_child(infohex)) return;
            infohex = new MapEditHex(this, mp);
            infohex.Show();
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Save Image";
            d.DefaultExt = "png";
            d.Filter = "PNG Image (*.png)|*.png|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + string.Format(".map.{0}.png",room);
            if (d.ShowDialog() == DialogResult.OK)
            {
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

        private void updateMode()
        {
            terrainToolStripMenuItem.Checked = mode == 0;
            itemsToolStripMenuItem.Checked   = mode == 1;
        }

        private void terrainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 0; updateMode();
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 1; updateMode();
        }
    }
}
