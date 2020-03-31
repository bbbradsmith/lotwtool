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
        static int default_zoom = 1;
        int mode = 0; // 0 terrain edit, 1 item edit
        int secret = 2; // 0 none, 1 replace, 2 blend
        bool items = true;
        public int room = 0;
        Main mp;
        Bitmap bmp = null;
        uint[] chr_cache;
        public uint[][] palette;
        public MapEditHex infohex = null;
        public MapEditTile tilepal = null;
        public MapEditProperties props = null;
        public MapEditItem itemedit = null;

        public byte draw_tile = 0;
        int drag_item = -1;
        int drag_y;
        int drag_item_y;
        int last_status_x = 0;
        int last_status_y = 0;
        public int last_item = 0;

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

        public int[] chr_set()
        {
            int ro = 16 + (1024 * room);
            int[] chri = new int[8];
            chri[0] = mp.rom[ro + 0x305];
            chri[2] = mp.rom[ro + 0x306];
            chri[5] = mp.rom[ro + 0x301];
            chri[1] = chri[0] + 1;
            chri[3] = chri[2] + 1;
            chri[4] = 0x3A; // always Roas
            chri[6] = 0x3E; // always items 0
            chri[7] = 0x3F; // always items 0
            return chri;
        }

        public void cache()
        {
            int ro = 16 + (1024 * room);

            palette = new uint[8][];
            for (int i = 0; i < 8; ++i)
            {
                palette[i] = new uint[4];
                for (int j=0; j<4; ++j)
                {
                    int p = mp.rom[ro+0x3E0+(i*4)+j] & 63;
                    palette[i][j] = Main.NES_PALETTE[p] | 0xFF000000;
                    if (i>=4 && j==0) palette[i][j] = 0x00000000;
                }
            }

            chr_cache = new uint[4 * 8 * 64 * 64];
            int[] chri = chr_set();
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

            int st = mp.rom[ro+0x302]; // secret tile (no palette)
            int str = mp.rom[ro+0x303]; // secret tile replacement (with palette)
            int t = mp.rom[ro+(x*12)+y];
            bool replace = (t & 63) == st;

            if (replace && secret == 1) t = str;
            int attribute = t >> 6;
            int metatile = t & 63;

            // metatile tables are in 256 byte pages in bank 9
            byte metatile_page = mp.rom[ro+0x300];
            int mto = 16 + (1024 * 8 * 9) + (metatile_page * 256);
            if ((mto+256) > mp.rom.Length) return;

            int[] XO = { 0, 0, 8, 8 };
            int[] YO = { 0, 8, 0, 8 };

            for (int i=0; i<4; ++i)
            {
                int mt = mp.rom[mto+(metatile*4)+i] + (attribute * 512);
                if (secret != 2 || !replace)
                {
                    Main.chr_blit(d, chr_cache, mt, (x*16)+XO[i], (y*16)+YO[i], zoom);
                }
                else // blend original and secret replacement
                {
                    int smt = mp.rom[mto+((str&63)*4)+i] + ((str>>6) * 512);
                    Main.chr_half(d, chr_cache, mt, smt, (x*16)+XO[i], (y*16)+YO[i], zoom);
                }
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

        void draw_sprite(BitmapData d, int s, int a, int x, int y)
        {
            if (s>=256) return;
            if ((x+16) > 1024) return;
            if ((y+16) > 192) return;

            int t = ((s & 1)<<8) | (s & 0xFE); // NES 16px sprite tile selector
            t |= (512 * (a & 3)); // select palette

            Main.chr_blit_mask(d, chr_cache, t+0x00, x+0, y+0, zoom);
            Main.chr_blit_mask(d, chr_cache, t+0x01, x+0, y+8, zoom);
            Main.chr_blit_mask(d, chr_cache, t+0x02, x+8, y+0, zoom);
            Main.chr_blit_mask(d, chr_cache, t+0x03, x+8, y+8, zoom);
        }

        void draw_items(BitmapData d)
        {
            if (!items) return;
            int ro = 16 + (1024 * room);

            for (int i=11; i>=0; --i)
            {
                int eo = ro + 0x320 + (i*16);
                int s = mp.rom[eo+0]; // sprite
                int a = mp.rom[eo+1]; // palette
                int x = mp.rom[eo+2]; // x grid
                int y = mp.rom[eo+3]; // y pixel
                x *= 16;
                if (s == 0) continue;
                draw_sprite(d,s,a,x,y);
            }

            // treasure
            if (mp.rom[ro+0x307] != 0) // 1 = active
            {
                int x = mp.rom[ro+0x308]; // x grid
                int y = mp.rom[ro+0x309]; // y pixel
                int s = mp.rom[ro+0x30A]; // contents
                //int a = (s >= 8) ? 1 : 0; // palette is selected by type
                // (using palette 1 always instead, colour 0 is replaced by player anyway and not really valid in map data)
                x *= 16;
                s = 0x81 + (s*4);
                draw_sprite(d,s,1,x,y);
            }
        }

        int pick_item(int x, int y) // pick item under pixel
        {
            int ro = 16 + (1024 * room);

            // treasure
            if (mp.rom[ro+0x307] != 0)
            {
                int ox = mp.rom[ro+0x308] * 16;
                int oy = mp.rom[ro+0x309];
                if (x >= ox && x < (ox+16) && y >= oy && y < (oy+16)) return 12;
            }

            // items
            for (int i=0; i<12; ++i)
            {
                int eo = ro + 0x320 + (i*16);
                if (mp.rom[eo+0] == 0) continue;
                int ox = mp.rom[eo+2] * 16;
                int oy = mp.rom[eo+3];
                if (x >= ox && x < (ox+16) && y >= oy && y < (oy+16)) return i;
            }

            return -1;
        }

        Tuple<int,int> pos_item(int item)
        {
            int ro = 16 + (1024 * room);
            int tx,ty;
            if (item < 0 || item > 12)
            {
                tx = -1;
                ty = -1;
            }
            else if (item == 12) // treasure
            {
                tx = mp.rom[ro+0x308] * 16;
                ty = mp.rom[ro+0x309];
            }
            else
            {
                int eo = ro + 0x320 + (item*16);
                tx = mp.rom[eo+2] * 16;
                ty = mp.rom[eo+3];
            }
            return new Tuple<int,int>(tx,ty);
        }

        public void redraw() // redraws just the map window
        {
            int w = 256 * 4 * zoom;
            int h = 192 * zoom;
            if (bmp == null || bmp.Width != w || bmp.Height != h)
            {
                bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            }
            BitmapData d = draw_lock();
            draw_bg(d);
            draw_items(d);
            draw_unlock(d);
        }

        public void redraw_info() // redraws info windows when stuff outside the terrain data changes
        {
            if (infohex != null) infohex.redraw();
            if (props != null) props.redraw();
            if (itemedit != null) itemedit.redraw();
        }

        public void reload_chr()
        {
            cache();
            redraw();
            if (tilepal != null)
            {
                tilepal.cache();
                tilepal.redraw();
            }
            mp.refresh_map(room);
        }

        public void render_select(BitmapData d, int room_, int zoom_, int secret_, bool items_)
        {
            room = room_;
            zoom = zoom_;
            secret = secret_;
            items = items_;
            cache();
            draw_bg(d);
            draw_items(d);
            //debug_room(); // easy way to query all the rooms
        }

        public void debug_room()
        {
            // for investigating a few questions about the format

            int ro = 16 + (1024 * room);
            int rx = room % 4;
            int ry = room / 4;
            string h = string.Format("debug_room {0:D2},{1:D2} {2:D2} ",rx,ry,room);

            string s = "";

            // 300 possible metatile pages
            /*s += string.Format("metatile: {0:X2}",mp.rom[ro+0x300]);*/

            // 302/303 secret tile possible values
            /*s += string.Format("secret tile {0:X2} -> {1:X2}",mp.rom[ro+0x302],mp.rom[ro+0x303]);*/

            // 314,316 what are these?
            //s += string.Format("= {0:X2} {1:X2}",mp.rom[ro+0x314],mp.rom[ro+0x316]);

            // Is 307 treasure chest active = 1, inactive = 0?
            /*if (mp.rom[ro+0x307] != 0x01) s += string.Format("\nTreasure 307: {0:X2}",mp.rom[ro+0x307]);*/

            // querying unused item data fields
            /*
            for (int i=0; i<12; ++i)
            {
                int eo = ro + 0x320 + (i*16);
                bool tail0 = true; // is 6 byte tail always empty? Yes.
                bool all0 = true; // is everything empty if byte 0 is empty? No, boss rooms are an exception.
                for (int j=0; j<16; ++j)
                {
                    bool now0 = mp.rom[eo+j] == 0;
                    all0 &= now0;
                    if (j >= 0xA) tail0 &= now0;
                }
                if ((mp.rom[eo+0] == 0 && !all0) || !tail0)
                    s += string.Format("\nEnemy {0:D2}: ",i) + mp.romhex(eo,16);
            }*/

            // unused space in control data 0x300 - 0x31F
            /*{
                bool room0 = true;
                //for (int i=0x30C; i<=0x30F; ++i) room0 &= mp.rom[ro+i] == 0; // used for princess portrait teleportZ
                for (int i=0x317; i<=0x31F; ++i) room0 &= mp.rom[ro+i] == 0;
                if (!room0) s += "\nNon-empty room control?\n" + mp.romhex(ro+0x300,16) + "\n" + mp.romhex(ro+0x310,16);
            }*/

            if (s.Length > 0) Console.WriteLine(h+s);
        }

        void set_draw_tile(int tile, int palette, int combo=-1) // -1 not to set one or the other
        {
            if (tile >= 0)    draw_tile = (byte)((draw_tile & 0xC0) | (tile & 0x3F));
            if (palette >= 0) draw_tile = (byte)((draw_tile & 0x3F) | (palette<<6));
            if (combo >= 0)   draw_tile = (byte)combo;
            updateStatus();
            if (tilepal != null)
            {
                tilepal.updateTileStatus(draw_tile);
                tilepal.redraw();
            }
        }

        public string get_tile_type(int t)
        {
            t = t & 0x3F;

            string s = "open";
            if (t >= 0x30) s = "solid";
            switch (t)
            {
            case 0:    s = "ladder"; break;
            case 1:    s = "enter";  break;
            case 2:    s = "door";   break;
            case 3:    s = "celina"; break;
            case 4:    s = "shop";   break;
            case 5:    s = "inn";    break;
            case 0x30: s = "spike";  break;
            case 0x3E: s = "block";  break;
            }

            int ro = 16 + (1024 * room);
            if (mp.rom[ro+0x302] == t) s += "*";

            return s;
        }

        public void refresh_all()
        {
            cache();
            redraw();
            redraw_info();
            if (tilepal != null)
            {
                tilepal.cache();
                tilepal.redraw();
            }
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

        public void refresh_metatile(int page)
        {
            int ro = 16 + (1024 * room);
            if (page == mp.rom[ro+0x300])
            {
                redraw();
                if (tilepal != null) tilepal.redraw();
            }
        }

        public void refresh_map(int map) { } // ignore, a map edit always comes from here

        public void refresh_close() { this.Close(); }

        public MapEdit(Main parent, int room_)
        {
            mp = parent;
            room = room_;
            zoom = default_zoom;
            InitializeComponent();

            int x = room % 4;
            int y = room / 4;
            Text = string.Format("Map {0},{1} ({2})",x,y,(y*4)+x);
            upToolStripMenuItem.Enabled = y > 0;
            downToolStripMenuItem.Enabled = (room+4) < mp.map_count;
            leftToolStripMenuItem.Enabled = x > 0;
            rightToolStripMenuItem.Enabled = (x < 3) && ((room+1) < mp.map_count);

            cache();
            updateZoom();
            //redraw(); // updateZoom does this
            updateStatus();
        }

        private void updateZoom()
        {
            default_zoom = zoom;
            zoom1xToolStripMenuItem.Checked = zoom == 1;
            zoom2xToolStripMenuItem.Checked = zoom == 2;
            zoom3xToolStripMenuItem.Checked = zoom == 3;
            zoom4xToolStripMenuItem.Checked = zoom == 4;

            int dspan = 192;
            int span = 192 * zoom;
            int h = (282-dspan)+span;
            if (zoom > 1) // scrollbar
                h += 18;
            Height = h;

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

        private void MapEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tilepal != null) tilepal.Close();
            if (infohex != null) infohex.Close();
            if (props != null) props.Close();
            if (itemedit != null) itemedit.Close();
            mp.remove_refresh(this);
            mp.remove_map_edit(this);
        }

        private void updateSecret()
        {
            showSecretToolStripMenuItem.Checked = secret == 1;
            halfSecretToolStripMenuItem.Checked = secret == 2;
            redraw();
        }

        private void showSecretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (secret != 1) secret = 1;
            else             secret = 0;
            updateSecret();
        }

        private void halfSecretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (secret != 2) secret = 2;
            else             secret = 0;
            updateSecret();
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
            if (mode == 1 && items == false)
                showItemsToolStripMenuItem_Click(null, null);
            updateStatus();
        }

        private void terrainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 0; updateMode();
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mode = 1; updateMode();
        }

        private void showItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // note: sender/e may be null, see updateMode
            items = !items;
            showItemsToolStripMenuItem.Checked = items;
            redraw();
        }

        private void updateStatus(int x=~0, int y=~0)
        {
            if (x == ~0) x = last_status_x;
            if (y == ~0) y = last_status_y;
            last_status_x = x;
            last_status_y = y;

            int ro = 16 + (1024 * room);
            int tx = x / 16;
            int ty = y / 16;

            string modeinfo = "";
            string tileinfo = "";
            string iteminfo = "";
            string modetips = "";

            if (mode == 0)
            {
                modeinfo = string.Format("TERRAIN {0:X2} ({1:X1}:{2:X2}) => ",draw_tile,draw_tile>>6,draw_tile&63);
                if (tx >= 0 && tx < 64 && ty >= 0 && ty < 16)
                {
                    byte tile = mp.rom[ro+(tx*12)+ty];
                    tileinfo = string.Format("{0,2:D},{1,2:D} = {2:X2} ({3:X1}:{4:X2}) {5}",tx,ty,tile,tile>>6,tile&63,get_tile_type(tile));
                }
                modetips = "LMB = Draw, RMB = Pick, Ctrl+RMB = Tiles";
            }
            else if (mode == 1)
            {
                modeinfo = string.Format("ITEMS");
                iteminfo = " None";
            }

            int item = pick_item(x,y);
            if (item == 12) // treasure
            {
                iteminfo = " Treasure";
                if (mode == 1) iteminfo += ": " + mp.romhex(ro + 0x307, 4);
            }
            else if (item >= 0) // item
            {
                iteminfo = string.Format(" Item {0:D}",item);
                if(mode == 1) iteminfo += ": " + mp.romhex(ro + 0x320 + (item*16),10);
            }

            if (mode == 0 && iteminfo.Length > 0)
            {
                iteminfo = " +" + iteminfo;
            }
            else if (mode == 1)
            {
                if (drag_item >= 0)
                    modetips = "Shift = Free Y";
                else if (item >= 0)
                    modetips = "LMB = Drag, Ctrl+LMB = Create, RMB = Edit";
                else
                    modetips = "Ctrl+LMB = Create";
            }

            toolStripStatusLabel.Text = modeinfo + tileinfo + iteminfo;
            toolStripTipLabel.Text = modetips;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X / zoom;
            int y = e.Y / zoom;
            int ro = 16 + (1024 * room);

            drag_item = -1;

            if (mode == 0) // terrain
            {
                if (e.Button == MouseButtons.Left) // start a draw
                {
                    mp.rom_modify_start();
                }
                else if (e.Button == MouseButtons.Right && ModifierKeys.HasFlag(Keys.Control))
                {
                    tilesToolStripMenuItem_Click(sender, e);
                }
            }
            else if (mode == 1) // item
            {
                if (e.Button == MouseButtons.Right) // edit
                {
                    int item = pick_item(x,y);
                    if (item == 12) // treasure
                    {
                        propertiesToolStripMenuItem_Click(sender,e);
                    }
                    else if (item >= 0 && item < 12)
                    {
                        itemEditorToolStripMenuItem_Click(sender,e);
                        itemedit.set_item(item);
                        last_item = item;
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (ModifierKeys.HasFlag(Keys.Control)) // try to create default item
                    {
                        for (int i=0; i<12; ++i)
                        {
                            bool empty = true;
                            for (int j=0; j<16; ++j) empty &= mp.rom[ro+0x320+(i*16)+j] == 0;
                            if (empty)
                            {
                                drag_item = i;
                                break;
                            }
                        }
                        if (drag_item >= 0)
                        {
                            mp.rom_modify_start();
                            byte[] monster = { 0x51, 0x03, 0x00, 0x00, 0x0D, 0x01, 0x5D, 0x02, 0x02, 0x01 }; // 0,0 default Meta Black
                            monster[2] = (byte)(x / 16);
                            monster[3] = (byte)(y & (~15));
                            for (int i=0; i<monster.Length; ++i)
                                mp.rom_modify(ro+0x320+(drag_item*16)+i, monster[i], true);
                            redraw();
                            redraw_info();
                            mp.refresh_map(room);
                        }
                    }
                    else // try to pick up an item
                    {
                        drag_item = pick_item(x,y);
                        if (drag_item >= 0) mp.rom_modify_start();
                    }

                    if (drag_item >= 0)
                    {
                        Tuple<int,int> ipos = pos_item(drag_item);
                        drag_y = y;
                        drag_item_y = ipos.Item2;
                        last_item = drag_item;
                    }
                }
            }
            pictureBox_MouseMove(sender, e);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X / zoom;
            int y = e.Y / zoom;
            int tx = x / 16;
            int ty = y / 16;
            int ro = 16 + (1024 * room);

            if (e.Button != MouseButtons.None)
            {
                int ti = ro+(tx*12)+ty;
                if (mode == 0)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (tx >= 0 && tx < 64 && ty >= 0 && ty < 12)
                        {
                            if (mp.rom_modify(ti,draw_tile,true))
                            {
                                redraw();
                                mp.refresh_map(room);
                            }
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (tx >= 0 && tx < 64 && ty >= 0 && ty < 12)
                        {
                            byte ndt = mp.rom[ti];
                            if (draw_tile != ndt)
                                set_draw_tile(-1,-1,ndt);
                        }
                    }
                }
                else if (mode == 1 && drag_item >= 0)
                {
                    int nx = x & (~15); // snap to grid
                    int ny = y & (~15);
                    if (ModifierKeys.HasFlag(Keys.Shift)) // Y can move freely with shift
                        ny = drag_item_y + (y - drag_y);

                    byte bx = (byte)(nx >> 4);
                    byte by = (byte)(ny);
                    bool changed = false;

                    int romx = ro + 0x320 + (drag_item*16) + 2; // item
                    if (drag_item == 12) romx = ro + 0x308; // treasure
                    int romy = romx + 1;

                    changed |= mp.rom_modify(romx,bx,true);
                    changed |= mp.rom_modify(romy,by,true);
                    if (changed)
                    {
                        redraw();
                        redraw_info();
                        mp.refresh_map(room);
                    }
                }
            }

            updateStatus(x,y);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            drag_item = -1;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Main.raise_child(props)) return;
            props = new MapEditProperties(this, mp);
            props.Show();
        }

        private void tilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Main.raise_child(tilepal)) return;
            tilepal = new MapEditTile(this, mp);
            tilepal.Show();
        }

        private void infoHexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Main.raise_child(infohex)) return;
            infohex = new MapEditHex(this, mp);
            infohex.Show();
        }

        private void itemEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Main.raise_child(itemedit))
            {
                itemedit = new MapEditItem(this, mp);
                itemedit.Show();
            }
            itemedit.set_item(last_item);
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.add_map_edit(room-4);
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.add_map_edit(room+4);
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.add_map_edit(room-1);
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.add_map_edit(room+1);
        }

        public void MapEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Modifiers.HasFlag(Keys.Control) && !e.Modifiers.HasFlag(Keys.Shift))
            {
                switch (e.KeyCode)
                {
                    // set palette
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                    case Keys.D4:
                        set_draw_tile(-1,e.KeyCode - Keys.D1); break;

                    // select tile
                    case Keys.OemOpenBrackets:
                        set_draw_tile((draw_tile-1)&0x3F,-1); break; // decrement tile
                    case Keys.OemCloseBrackets:
                        set_draw_tile((draw_tile+1)&0x3F,-1); break; // increment tile
                    case Keys.Left:
                        set_draw_tile((draw_tile & 0x38) | ((draw_tile-1) & 0x07), -1); break; // wrapping select
                    case Keys.Right:
                        set_draw_tile((draw_tile & 0x38) | ((draw_tile+1) & 0x07), -1); break;
                    case Keys.Up:
                        set_draw_tile((draw_tile & 0x07) | ((draw_tile-8) & 0x38), -1); break;
                    case Keys.Down:
                        set_draw_tile((draw_tile & 0x07) | ((draw_tile+8) & 0x38), -1); break;

                    // select mode
                    case Keys.T:
                        terrainToolStripMenuItem_Click(sender,e); break;
                    case Keys.I:
                        itemsToolStripMenuItem_Click(sender,e); break;
                    case Keys.Space:
                        mode = mode ^ 1; updateMode(); break;

                    // tile palette
                    case Keys.P:
                        tilesToolStripMenuItem_Click(sender,e); break;

                    default:
                        break;
                }
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string HELPTEXT =
                "T = Terrain mode\n" +
                "I = Item mode\n" +
                "P = Open tile palette\n" +
                "Space = Toggle mode\n" +
                "\n"+
                "Terrain mode:\n" +
                "LMB = Draw tile\n" +
                "RMB = Pick tile\n" +
                "Ctrl + RMB = Open tile palette\n" +
                "1-4 = Palette\n" +
                "[/] = Cycle tile\n" +
                "Cursor Keys = Select tile\n" +
                "\n" +
                "Item mode:\n" +
                "LMB = Move item\n" +
                "Ctrl + LMB = Create item\n" +
                "Shift + LMB = Move with free Y position\n" +
                "RMB = Edit item\n";
            MessageBox.Show(HELPTEXT,"Map Edit Help");
        }
    }
}
