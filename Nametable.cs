using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lotwtool
{
    public partial class Nametable : Form, RomRefresh
    {
        Main mp;
        public int no; // offset to data
        uint[] chr_cache = null;
        Bitmap bnmt = null;
        Bitmap btil = null;
        Bitmap bpal = null;
        int pal_select = 0;
        int tile_select = 0;
        int zoom = 2;
        bool grid = false;

        int[] chr_set()
        {
            byte chr0 = mp.rom[no+0x420];
            byte chr1 = mp.rom[no+0x421];
            return new int[] { chr0 & (~1), chr0 | 1, chr1 & (~1), chr1 | 1};
        }

        uint[][] palette_set()
        {
            uint[][] palettes = new uint[8][];
            for (int i=0; i<8; ++i)
            {
                palettes[i] = new uint[4];
                for (int j=0; j<4; ++j)
                {
                    int p = mp.rom[no+0x400+(i*4)+j] & 63;
                    palettes[i][j] = Main.NES_PALETTE[p]; // TODO this isn't the format anymore
                }
            }
            return palettes;
        }

        void cache()
        {
            int[] chr = chr_set();
            uint[][] palettes = palette_set();
            for (int i=0; i<4; ++i) // 1K CHR page
            {
                for (int j=0; j<64; ++j) // tile in page
                {
                    for (int k=0; k<4; ++k) // colour
                    {
                        mp.chr_cache(chr[i]*64+j, (256*k)+(i*64)+j, chr_cache, palettes[k]);
                    }
                    mp.chr_cache(chr[i]*64+j, (256*4)+(i*64)+j, chr_cache, Main.GREY);
                    mp.chr_cache(chr[i]*64+j, (256*5)+(i*64)+j, chr_cache, Main.HIGHLIGHT);
                }
            }
        }

        void redraw_nametable()
        {
            int w = 256 * zoom;
            int h = 240 * zoom;
            if (bnmt == null || bnmt.Width != w || bnmt.Height != h)
            {
                bnmt = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            }
            BitmapData d = bnmt.LockBits(new Rectangle(0, 0, bnmt.Width, bnmt.Height), ImageLockMode.WriteOnly, bnmt.PixelFormat);

            for (int y=0; y<30; ++y)
            {
                for (int x=0; x<32; ++x)
                {
                    int t = mp.rom[no+(y*32)+x];
                    byte ra = mp.rom[no+0x3C0+(x/4)+((y/4)*8)];
                    int a = (ra >> ((x&2) | ((y<<1)&4))) & 3;
                    Main.chr_blit(d, chr_cache, t+(256*a), x*8, y*8, zoom);
                }
            }

            if (grid)
            {
                for (int y=1; y<30; y += 2) Main.draw_hline(d,0,y*8*zoom,w,Main.GRID2);
                for (int x=1; x<32; x += 2) Main.draw_vline(d,x*8*zoom,0,h,Main.GRID2);
                for (int y=0; y<30; y += 2) Main.draw_hline(d,0,y*8*zoom,w,Main.GRID);
                for (int x=0; x<32; x += 2) Main.draw_vline(d,x*8*zoom,0,h,Main.GRID);
            }

            bnmt.UnlockBits(d);
            pictureBox.Image = bnmt;
        }

        void redraw_tiles()
        {
            int w = 128 * zoom;
            int h = 128 * zoom;
            if (btil == null || btil.Width != w || btil.Height != h)
            {
                btil = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            }
            BitmapData d = btil.LockBits(new Rectangle(0, 0, btil.Width, btil.Height), ImageLockMode.WriteOnly, btil.PixelFormat);

            for (int y=0; y<16; ++y)
            {
                for (int x=0; x<16; ++x)
                {
                    int t = x+(y*16);
                    if (t == tile_select) t += (256*5); // highlight
                    else                  t += (256*4); // grey
                    Main.chr_blit(d, chr_cache, t, x*8, y*8, zoom);
                }
            }

            btil.UnlockBits(d);
            pictureTile.Image = btil;
        }

        void redraw_palettes()
        {
            uint[][] palettes = palette_set();

            int w = 128 * zoom;
            int h = 32 * zoom;
            if (bpal == null || bpal.Width != w || bpal.Height != h)
            {
                bpal = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            }
            BitmapData d = bpal.LockBits(new Rectangle(0, 0, bpal.Width, bpal.Height), ImageLockMode.WriteOnly, bpal.PixelFormat);

            int pw = w / 16;
            int ph = h / 2;

            for (int i=0; i<8; ++i)
            {
                int px = (i % 4) * (pw * 4);
                int py = (i / 4) * ph;
                for (int j=0; j<4; ++j)
                {
                    Main.draw_box(d,px+(j*pw),py,pw,ph,palettes[i][j]);
                }
                if (i == pal_select)
                {
                    Main.draw_outbox(d,px,py,pw*4,ph,Main.PAL_OUTER);
                    Main.draw_outbox(d,px+1,py+1,(pw*4)-2,ph-2,Main.PAL_INNER);
                }
            }

            bpal.UnlockBits(d);
            picturePalette.Image = bpal;
        }

        void redraw()
        {
            redraw_nametable();
            redraw_tiles();
            redraw_palettes();
        }

        public void refresh_all()
        {
            cache();
            redraw();
        }

        public void refresh_chr(int tile)
        {
            int page = tile / 64;
            if (chr_set().Contains(page))
            {
                cache();
                redraw();
            }
        }

        public void refresh_metatile(int page) { }

        public void refresh_map(int map) { }

        public void refresh_close() { this.Close(); }

        public Nametable(Main parent, int offset)
        {
            mp = parent;
            no = offset;
            chr_cache = new uint[256*6*64];
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;
            this.Text = string.Format("Nametable {0:X}",no-16);
            cache();
            redraw();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mp.rom_modify_start(); // set undo point
            }
            pictureBox_MouseMove(sender,e);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int tx = e.X / (8 * zoom);
            int ty = e.Y / (8 * zoom);
            if (tx < 0 || tx >= 32 || ty < 0 || ty >= 30) return;


            int na = no+(ty*32)+tx;
            int aa = no+0x3C0+(tx/4)+((ty/4)*8);
            int ms = (tx&2) | ((ty<<1)&4);

            byte t = mp.rom[na];
            byte ra = mp.rom[aa];
            int a = (ra >> ms) & 3;

            if (e.Button == MouseButtons.Left)
            {
                if (!ModifierKeys.HasFlag(Keys.Shift)) // hold shift for just attribute
                    t = (byte)tile_select;
                a = pal_select;
                ra = (byte)((ra & (~(3<<ms))) | (a<<ms));
                bool changed = false;
                changed |= mp.rom_modify(na,t,true);
                changed |= mp.rom_modify(aa,ra,true);
                if (changed)
                    redraw_nametable();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (tile_select != t)
                {
                    tile_select = t;
                    redraw_tiles();
                }
                if (pal_select != a)
                {
                    pal_select = a;
                    redraw_palettes();
                }
            }

            toolStripStatusLabel.Text = string.Format("{0,2},{1,2} = {2:X2}:{3}",tx,ty,t,a);
            toolStripTipLabel.Text = "LMB = Draw, Shift + LMB = Draw Attribute, RMB = Pick";
        }

        private void pictureTile_MouseDown(object sender, MouseEventArgs e)
        {
            int c = (e.Y * 2) / (16 * 8 * zoom);
            if (c < 0 || c >= 2) return;

            if (e.Button == MouseButtons.Right)
            {
                CHRSelect cs = new CHRSelect(mp,true);
                cs.StartPosition = FormStartPosition.CenterParent;
                cs.preselect = chr_set()[c*2];
                cs.dualpage = true;
                cs.sprite = false;
                if (cs.ShowDialog() == DialogResult.OK)
                {
                    if (cs.highlight >= 0 && cs.highlight < mp.chr_count)
                    {
                        if (mp.rom_modify(no+0x420+c,(byte)cs.highlight))
                        {
                            cache();
                            redraw();
                        }
                    }
                }
            }
            else pictureTile_MouseMove(sender, e);
        }

        private void pictureTile_MouseMove(object sender, MouseEventArgs e)
        {
            int tx = e.X / (8 * zoom);
            int ty = e.Y / (8 * zoom);
            if (tx < 0 || tx >= 16 || ty < 0 || ty >=16) return;
            int t = tx + (ty * 16);

            if (e.Button == MouseButtons.Left)
            {
                if (tile_select != t)
                {
                    tile_select = t;
                    redraw_tiles();
                }
            }

            toolStripStatusLabel.Text = string.Format("{0:X2}",t);
            toolStripTipLabel.Text = "RMB = Select CHR";
        }

        private void picturePalette_MouseDown(object sender, MouseEventArgs e)
        {
            int px = (e.X * 4) / picturePalette.Width;
            int py = (e.Y * 2) / picturePalette.Height;
            int p = (py * 4) + px;
            int c = ((e.X * 16) / picturePalette.Width) % 4;
            int a = no+0x400+((p*4)+c);
            if (px < 0 || px >= 4 || py < 0 || py >= 2) return;

            if (e.Button == MouseButtons.Right)
            {
                byte old = mp.rom[a]; // TODO MSX palettes?
                PalettePick pp = new PalettePick(old & 63);
                pp.StartPosition = FormStartPosition.CenterParent;
                if (pp.ShowDialog() == DialogResult.OK)
                {
                    byte np = (byte)pp.picked;
                    if (mp.rom_modify(a, np))
                    {
                        cache();
                        redraw();
                    }
                }
            }
            else picturePalette_MouseMove(sender,e);
        }

        private void picturePalette_MouseMove(object sender, MouseEventArgs e)
        {
            int px = (e.X * 4) / picturePalette.Width;
            int py = (e.Y * 2) / picturePalette.Height;
            int p = (py * 4) + px;
            int c = ((e.X * 16) / picturePalette.Width) % 4;
            int a = no+0x400+((p*4)+c);
            if (px < 0 || px >= 4 || py < 0 || py >= 2) return;

            byte v = mp.rom[a];
            if (e.Button == MouseButtons.Left && p < 4) // only select first 4
            {
                if (pal_select != p)
                {
                    pal_select = p;
                    redraw_palettes();
                }
            }

            toolStripStatusLabel.Text = string.Format("{0}:{1} = {2:X2}",p,c,v);
            toolStripTipLabel.Text = "RMB = Edit";
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
        }

        private void Nametable_FormClosing(object sender, FormClosingEventArgs e)
        {
            mp.remove_refresh(this);
            mp.title_screen = null;
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid = !grid;
            gridToolStripMenuItem.Checked = grid;
            redraw_nametable();
        }

        private void importNamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Import Nametable Binary";
            d.DefaultExt = "nam";
            d.Filter = "Nametable Binary (*.nam;*.bin)|*.nam;*.bin|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + ".nam";
            if (d.ShowDialog() == DialogResult.OK)
            {
                byte[] nam;
                try
                {
                    nam = File.ReadAllBytes(d.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to open NAM:\n\n" + ex.ToString(),"Nametable import error!");
                    return;
                }

                mp.rom_modify_start(); // begin a single undo step
                int len = nam.Length;
                if (len > 1024) len = 1024;
                for (int i=0; i<len; ++i)
                    mp.rom_modify(no+i,nam[i],true);
                mp.refresh_all();
            }
        }

        private void exportNamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Export Nametable Binary";
            d.DefaultExt = "nam";
            d.Filter = "Nametable Binary (*.nam;*.bin)|*.nam;*.bin|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + ".nam";
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] nam = new byte[1024];
                    Array.Copy(mp.rom,no,nam,0,1024);
                    File.WriteAllBytes(d.FileName,nam);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save NAM:\n" + d.FileName + "\n\n" + ex.ToString(), "Nametable save error!");
                }
            }

        }
    }
}
