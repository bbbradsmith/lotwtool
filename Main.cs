using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace lotwtool
{
    public partial class Main : Form, RomRefresh
    {
        const string VERSION = "0"; // TODO set this on first release

        // ROM state

        public byte[] rom = { };
        byte[] rom_unchanged = null; // last saved file
        byte[] rom_original = null; // as first opened
        public string filename = "";
        public int chr_offset = 0;
        public int chr_count = 0;
        public int map_count = 0;

        List<RomRefresh> refreshers = new List<RomRefresh>();
        List<MapEdit> mapedits = new List<MapEdit>();
        public MapSelect map_select = null;
        public MiscCheat misc_cheat = null;
        public Nametable title_screen = null;
        Stack<List<int>> undo_stack = new Stack<List<int>>();
        public bool misc_errors_shown = false;

        public static readonly uint[] NES_PALETTE =
        {
            0xFF656665, 0xFF001E9C, 0xFF200DAC, 0xFF44049C,
            0xFF6A036F, 0xFF71021D, 0xFF651000, 0xFF461E00,
            0xFF232D00, 0xFF003900, 0xFF003C00, 0xFF003720,
            0xFF003266, 0xFF000000, 0xFF000000, 0xFF000000,
            0xFFB0B1B0, 0xFF0955EB, 0xFF473DFF, 0xFF7730FE,
            0xFFAE2CCE, 0xFFBC2964, 0xFFB43900, 0xFF8F4B00,
            0xFF635F00, 0xFF1B7000, 0xFF007700, 0xFF00733B,
            0xFF006D99, 0xFF000000, 0xFF000000, 0xFF000000,
            0xFFFFFFFF, 0xFF4DADFF, 0xFF8694FF, 0xFFB885FF,
            0xFFF17FFF, 0xFFFF79D4, 0xFFFF865F, 0xFFF19710,
            0xFFC8AB00, 0xFF7EBE00, 0xFF47C81F, 0xFF2BC86F,
            0xFF2EC4CC, 0xFF50514D, 0xFF000000, 0xFF000000,
            0xFFFFFFFF, 0xFFB9E5FF, 0xFFD0DBFF, 0xFFE6D5FF,
            0xFFFDD1FF, 0xFFFFCEF5, 0xFFFFD4C5, 0xFFFFDAA3,
            0xFFEEE290, 0xFFD0EB8E, 0xFFB9EFA5, 0xFFAEEFC7,
            0xFFAEEEEE, 0xFFBABCB9, 0xFF000000, 0xFF000000,
        };

        public static readonly uint[] GREY =
        {
            0xFF000000,
            0xFF555555,
            0xFFAAAAAA,
            0xFFFFFFFF,
        };

        public static readonly uint[] HIGHLIGHT =
        {
            0xFF662244,
            0xFF883366,
            0xFFCC4488,
            0xFFFFCCDD,
        };

        public static readonly uint[] PRESELECT =
        {
            0xFF446622,
            0xFF668833,
            0xFF88CC44,
            0xFFDDFFCC,
        };

        public const uint GRID = 0xFFFFFF00; // yellow
        public const uint GRID2 = 0xFFAAAA00; // dimmer yellow
        public const uint PAL_INNER = 0xFF000000;
        public const uint PAL_OUTER = 0xFFFFFFFF;


        // Common code

        bool changed()
        {
            if (rom.Length < 1) return false;
            return !rom.SequenceEqual(rom_unchanged);
        }

        bool changePrevent(string caption)
        {
            if (!changed()) return false;
            DialogResult result = MessageBox.Show("You have unsaved changes. Proceed?", caption, MessageBoxButtons.OKCancel);
            return result != DialogResult.OK;
        }

        bool openFile(string path)
        {
            if (changePrevent("Open...")) return false;

            close_children();
            misc_errors_shown = false;

            byte[] read_rom;
            try
            {
                read_rom = File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open file:\n" + path + "\n\n" + ex.ToString(), "File open error!");
                return false;
            }

            // successful load: acquire the ROM
            rom = read_rom;
            filename = path; textBoxFilename.Text = filename;
            rom_unchanged = (byte[])rom.Clone();
            rom_original = (byte[])rom.Clone();
            undo_stack.Clear();

            // set file boundaries
            chr_offset = 0;
            chr_count = 0;
            map_count = 0;
            if (rom.Length >= 16)
            {
                int ines_prg = rom[4];
                int ines_chr = rom[5];
                int prg_max = 16 + (ines_prg * 16 * 1024);
                int chr_max = prg_max + (ines_chr * 8 * 1024);
                if (prg_max > rom.Length) prg_max = rom.Length;
                if (chr_max > rom.Length) chr_max = rom.Length;
                map_count = 4 * 18;
                chr_count = ines_chr * 8;
                chr_offset = 16 + (ines_prg * 16 * 1024);
                if (16 + (map_count * 1024) > prg_max)
                    map_count = (prg_max - 16) / 1024;
                if (16 + chr_offset + (chr_count * 1024) > chr_max)
                    chr_max = (chr_max - chr_offset) / 1024;
            }

            labelCHRCountValue.Text = string.Format("{0}", chr_count);
            labelMapCountValue.Text = string.Format("{0}", map_count);

            buttonMapEdit.Enabled = map_count > 0;
            buttonCHREdit.Enabled = chr_count > 1;
            buttonTitleScreen.Enabled = rom.Length >= 16+0x1B000;
            buttonUnusedScreen.Enabled = rom.Length >= 16+0x19000;
            buttonCredits.Enabled = rom.Length >= 16+0x1C000;
            buttonDragon.Enabled = rom.Length >= 16+0x1C000;
            buttonMisc.Enabled = rom.Length >= 16+0x20000;

            mapsToolStripMenuItem.Enabled = buttonMapEdit.Enabled;
            CHRToolStripMenuItem.Enabled = buttonCHREdit.Enabled;
            titleScreenToolStripMenuItem.Enabled = buttonTitleScreen.Enabled;
            unusedScreenToolStripMenuItem.Enabled = buttonUnusedScreen.Enabled;
            creditsToolStripMenuItem.Enabled = buttonCredits.Enabled;
            dragonToolStripMenuItem.Enabled = buttonDragon.Enabled;
            miscToolStripMenuItem.Enabled = buttonMisc.Enabled;

            return true;
        }

        public bool saveFile(string path)
        {
            try
            {
                File.WriteAllBytes(path,rom);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open file:\n" + path + "\n\n" + ex.ToString(), "File open error!");
                return false;
            }
            rom_unchanged = (byte[])rom.Clone();
            return true;
        }

        public void chr_cache(int rom_index, int cache_index, uint[] cache, uint[] palette)
        {
            // decodes a tile from the ROM, and stores it in cache with given palette
            int ro = chr_offset + (rom_index * 16);
            int co = cache_index * 64;
            if ((ro + 16) > rom.Length || ro < chr_offset)
            {
                for (int i=0; i<16; ++i) cache[co+i] = 0;
                return;
            }
            for (int y = 0; y < 8; ++y)
            {
                byte p0 = rom[ro + y];
                byte p1 = rom[ro + y + 8];
                for (int x = 0; x < 8; ++x)
                {
                    int p = ((p0 >> 7) & 1) | ((p1 >> 6) & 2);
                    cache[co + x + (y * 8)] = palette[p];
                    p0 <<= 1;
                    p1 <<= 1;
                }
            }
        }

        public static void chr_blit(BitmapData bd, uint[] cache, int tile, int x, int y, int zoom)
        {
            // draws a cached tile on a locked bitmap data
            x *= zoom;
            y *= zoom;
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                fixed (uint* fcc = cache)
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

        public static void chr_half(BitmapData bd, uint[] cache, int tile0, int tile1, int x, int y, int zoom)
        {
            // draws a blend of two cached tiles
            x *= zoom;
            y *= zoom;
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                fixed (uint* fcc = cache)
                {
                    uint* scanline = braw + (stride * y) + x;
                    uint* chrline0 = fcc + (tile0 * 64);
                    uint* chrline1 = fcc + (tile1 * 64);
                    for (int py = 0; py < 8; ++py)
                    {
                        for (int yz = 0; yz < zoom; ++yz)
                        {
                            int sx = 0;
                            for (int px = 0; px < 8; ++px)
                            {
                                for (int xz = 0; xz < zoom; ++xz)
                                {
                                    uint c0 = chrline0[px];
                                    uint c1 = chrline1[px];
                                    scanline[sx] = (((c0>>1)&0x7F7F7F) + ((c1>>1)&0x7F7F7F)) | 0xFF000000;
                                    ++sx;
                                }
                            }
                            scanline += stride;
                        }
                        chrline0 += 8;
                        chrline1 += 8;
                    }
                }
            }
        }

        public static void chr_blit_mask(BitmapData bd, uint[] cache, int tile, int x, int y, int zoom)
        {
            x *= zoom;
            y *= zoom;
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                fixed (uint* fcc = cache)
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
                                    uint c = chrline[px];
                                    if (c != 0)
                                        scanline[sx] = c;
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

        public static void draw_hline(BitmapData bd, int x, int y, int w, uint c) // draws a horizontal line
        {
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                int o = (stride * y) + x;
                for (int i=0; i<w; ++i)
                {
                    braw[o] = c;
                    ++o;
                }
            }
        }

        public static void draw_vline(BitmapData bd, int x, int y, int h, uint c) // draws a vertical line
        {
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                int o = (stride * y) + x;
                for (int i=0; i<h; ++i)
                {
                    braw[o] = c;
                    o += stride;
                }
            }
        }

        public static void draw_box(BitmapData bd, int x, int y, int w, int h, uint c) // draws a box
        {
            unsafe
            {
                uint* braw = (uint*)bd.Scan0.ToPointer();
                int stride = bd.Stride / 4;
                int o = (stride * y) + x;
                for (int j=0; j<h; ++j)
                {
                    int ol = o;
                    for (int i=0; i<w; ++i)
                    {
                        braw[ol] = c;
                        ++ol;
                    }
                    o += stride;
                }
            }
        }

        public static void draw_outbox(BitmapData bd, int x, int y, int w, int h, uint c) // draws a box outline
        {
            draw_hline(bd,x,y,w,c);
            draw_vline(bd,x,y,h,c);
            draw_hline(bd,x,y+h-1,w,c);
            draw_vline(bd,x+w-1,y,h,c);
        }

        public string romhex(int start, int length)
        {
            string s = "";
            for (int i=0; i<length; ++i)
            {
                if (i!=0) s += " ";
                if (start+i < rom.Length)
                    s += string.Format("{0:X2}",rom[start+i]);
            }
            return s;
        }

        // ROM modification and undo

        public void undo()
        {
            if (undo_stack.Count < 1) return;
            List<int> a = undo_stack.Pop();
            if (a.Count < 1) { undo(); return; } // skip any null undo

            for (int i=a.Count-2; i>=0; i-=2)
            {
                int addr = a[i+0];
                byte value =  (byte)a[i+1];
                rom[addr] = value;
            }
            refresh_all();
        }

        public bool rom_modify(int address, byte value, bool append = false) // true if a byte was changed
        {
            byte old = rom[address];
            List<int> a;
            if (!append || undo_stack.Count < 1)
            {
                if (undo_stack.Count < 1 || undo_stack.Peek().Count > 0)
                    a = new List<int>();
                else // append anyway if we've got an empty step
                    a = undo_stack.Pop();
            }
            else // append adds to last undo step
            {
                a = undo_stack.Pop();
            }
            if (old != value)
            {
                a.Add(address);
                a.Add(old);
            }
            undo_stack.Push(a);
            rom[address] = value;
            return old != value;
        }

        public void rom_modify_start() // begins a new undo step
        {
            rom_modify(0,rom[0],false); // dummy non-change to start an empty step
        }

        public bool rom_modify_range(int address, byte[] c, bool append=false) // change a block of ROM in one step
        {
            bool modified = false;
            if (!append) rom_modify_start();
            for (int i=0; i<c.Length; ++i)
                modified |= rom_modify(address+i,c[i],true);
            return modified;
        }

        public bool rom_modify_bit(int address, int bit, bool v, bool append=false) // write a single bit (LSB=0, MSB=7)
        {
            byte m = (byte)(1 << bit);
            byte b = (byte)(rom[address] & (~m));
            if (v) b |= m;
            return rom_modify(address, b, append);
        }

        public bool rom_bit(int address, int bit) // read a single bit (LSB=0, MSB=7)
        {
            byte m = (byte)(1 << bit);
            return (rom[address] & m) != 0;
        }

        public bool rom_modify_hex32(int address, uint v, bool append=false) // writes big endian 32 bit
        {
            return rom_modify_range(address, new byte[] {
                    (byte)((v>>24)&0xFF),
                    (byte)((v>>16)&0xFF),
                    (byte)((v>> 8)&0xFF),
                    (byte)((v>> 0)&0xFF)}, append);
        }
        
        public uint rom_hex32(int address) // reads big endian 32 bit
        {
            uint v = 0;
            for (int i=0; i<4; ++i)
            {
                v <<= 8;
                if ((address+i) < rom.Length)
                    v |= rom[address+i];
            }
            return v;
        }

        public bool rom_compare(int address, byte[] c) // check ROM for a specific string
        {
            if ((address + c.Length) > rom.Length) return false;
            for (int i=0; i<c.Length; ++i)
                if (rom[address+i] != c[i]) return false;
            return true;
        }

        // Children management

        public void refresh_all()
        {
            foreach (RomRefresh r in refreshers) r.refresh_all();
            if (misc_cheat != null) misc_cheat.redraw();
        }

        public void refresh_chr(int tile)
        {
            foreach (RomRefresh r in refreshers) r.refresh_chr(tile);
        }

        public void refresh_metatile(int page)
        {
            foreach (RomRefresh r in refreshers) r.refresh_metatile(page);
        }

        public void refresh_map(int map)
        {
            foreach (RomRefresh r in refreshers) r.refresh_map(map);
        }

        public void refresh_close()
        {
            this.Close(); // shouldn't be used?
        }

        public void add_refresh(RomRefresh r)
        {
            refreshers.Add(r);
        }

        public void remove_refresh(RomRefresh r)
        {
            bool removed;
            do
            {
                removed = refreshers.Remove(r);
            } while (removed);
        }

        public void refresh_misc()
        {
            if (misc_cheat != null) misc_cheat.redraw();
        }

        public void add_map_edit(int room)
        {
            if (room >= map_count && room != 0x4E) return;
            foreach (MapEdit m in mapedits)
            {
                if (m.room == room)
                {
                    if (raise_child(m)) return;
                }
            }
            MapEdit me = new MapEdit(this, room);
            me.Show();
            mapedits.Add(me);
            add_refresh(me);
        }

        public void remove_map_edit(MapEdit m)
        {
            bool removed;
            do
            {
                removed = mapedits.Remove(m);
            } while (removed);
        }

        public void add_chr_edit(int tile, bool sprite)
        {
            CHREdit ce = new CHREdit(this, tile, sprite);
            ce.Show();
            add_refresh(ce);
        }

        public void close_children()
        {
            while (refreshers.Count > 0)
                refreshers[0].refresh_close();
            if (map_select != null) map_select.Close();
            if (misc_cheat != null) misc_cheat.Close();
            if (title_screen != null) title_screen.Close();
        }

        public static bool raise_child(Form c) // convenient way to wake a child
        {
            if (c == null) return false;
            if (c.WindowState == FormWindowState.Minimized)
            {
                // can't seem to easily check if it was maximized before minimize, unfortunately?
                c.WindowState = FormWindowState.Normal;
            }
            c.Activate();
            return true;
        }

        // Form

        public Main()
        {
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;

            // open file from the command line
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                openFile(args[1]);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Title = "Open NES ROM";
            if (d.ShowDialog() == DialogResult.OK)
            {
                openFile(d.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(filename);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Save NES ROM";
            if (d.ShowDialog() == DialogResult.OK)
            {
                saveFile(d.FileName);
                filename = d.FileName; textBoxFilename.Text = filename;
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = changePrevent("Exit...");
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop)) ?
                DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (string file in files) openFile(file);
        }

        private void buttonMapEdit_Click(object sender, EventArgs e)
        {
            if (raise_child(map_select)) return;
            map_select = new MapSelect(this);
            map_select.Show();
            add_refresh(map_select);
        }

        private void buttonCHREdit_Click(object sender, EventArgs e)
        {
            CHRSelect chr_select = new CHRSelect(this);
            chr_select.Show();
            add_refresh(chr_select);
        }

        private void buttonTitleScreen_Click(object sender, EventArgs e)
        {
            int ADDRESS = 16+0x19EC9;
            if (title_screen != null && title_screen.no != ADDRESS) title_screen.Close();
            if (raise_child(title_screen)) return;
            title_screen = new Nametable(this, ADDRESS);
            title_screen.Show();
            add_refresh(title_screen);
        }

        private void buttonUnusedScreen_Click(object sender, EventArgs e)
        {
            int ADDRESS = 16+0x17BCA;
            if (title_screen != null && title_screen.no != ADDRESS) title_screen.Close();
            if (raise_child(title_screen)) return;
            title_screen = new Nametable(this, ADDRESS);
            title_screen.Show();
            add_refresh(title_screen);
        }

        private void buttonCredits_Click(object sender, EventArgs e)
        {
            // use the presence of code that loads the credits pointer into $0C to identify ROM type
            //   A9 .. = LDA #..
            //   85 0C = STA $0C
            //   A9 .. = LDA #..
            //   85 0D = STA $0D
            int credits_loc = 16 + 0x1B79C;
            int credits_pal = 16 + 0x1B2CD;
            if      (rom_compare(16 + 0x1B183, new byte[] { 0xA9,0x9C,0x85,0x0C,0xA9,0xB7,0x85,0x0D }))
            { } // Legacy of the Wizard (default)
            else if (rom_compare(16 + 0x1B191, new byte[] { 0xA9,0xAA,0x85,0x0C,0xA9,0xB7,0x85,0x0D }))
            {   // Dragon Slayer 4
                credits_loc = 16 + 0x1B7AA;
                credits_pal = 16 + 0x1B2DB;
            }
            else
            {
                MessageBox.Show("Could not identify credits location. Corrupt ROM?", "Credits error!");
                return;
            }

            byte[] cpal = {
                rom[credits_pal+ 0],
                rom[credits_pal+ 5],
                rom[credits_pal+10],
                rom[credits_pal+15]
            };

            // credits are an ASCII string
            string credits_text = "";
            int pos = 0;
            for (; (credits_loc+pos)<(16+0x1C000); ++pos)
            {
                byte b = rom[credits_loc+pos];
                if (b == 0) break;
                if (b == 13)
                    credits_text += "\r\n";
                else
                    credits_text += ((char)b).ToString();
            }
            if ((credits_loc + pos) >= (16+0x1C000))
            {
                MessageBox.Show("Credits not terminated with 0. Corrupt ROM?", "Credits error!");
                return;
            }
            // following credits are 0 padding up to 1BFA3 or 1BF00,
            // scanning to figure out how much total space we have to use for credits
            for (; (credits_loc+pos)<(16+0x1C000); ++pos)
            {
                if (rom[credits_loc+pos] != 0) break;
            }
            int pos_max = pos;

            Credits c = new Credits(this,credits_text,cpal);
            while (c.ShowDialog() == DialogResult.OK)
            {
                int count = 1; // +1 for terminal 0
                foreach (char t in c.result)
                    if (t != '\r') ++count;
                if (count >= pos_max)
                {
                    MessageBox.Show(string.Format("Credits too long by {0} characters!",(count+1)-pos_max), "Credits error!");
                    // dialog will be re-shown
                }
                else
                {
                    // return credits to ROM
                    rom_modify_start();
                    pos = 0;
                    foreach (char t in c.result)
                    {
                        if (t == '\n') continue;
                        rom_modify(credits_loc+pos, (byte)t, true);
                        pos += 1;
                    }
                    for (; pos < pos_max; ++pos)
                        rom_modify(credits_loc+pos, 0, true);
                    break;
                }
            }
        }

        private void buttonDragon_Click(object sender, EventArgs e)
        {
            add_map_edit(0x4E); // seems to just be a special map at 13800
        }

        private void buttonMisc_Click(object sender, EventArgs e)
        {
            if (raise_child(misc_cheat)) return;
            misc_cheat = new MiscCheat(this);
            misc_cheat.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ABOUT_TEXT =
                "LotW Tool\n" +
                "\n" +
                "An editor for Legacy of the Wizard (NES)\n" +
                "and Dragon Slayer IV (Famicom).\n" +
                "\n" +
                "Brad Smith\n" +
                "Version: " + VERSION;
            MessageBox.Show(ABOUT_TEXT, "About the LotW Tool");
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            undo();
        }

        public void saveAndTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFile(filename))
            {
                System.Diagnostics.Process.Start(filename); // run the .NES file
            }
        }

        private void mapsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonMapEdit_Click(sender, e);
        }

        private void CHRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonCHREdit_Click(sender, e);
        }

        private void miscToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonMisc_Click(sender,e);
        }

        private void revertAllWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rom.SequenceEqual(rom_original))
            {
                MessageBox.Show("No changes since opening.", "Revert to Opened", MessageBoxButtons.OK);
                return;
            }

            if (DialogResult.OK == MessageBox.Show(
                "Undo all changes since opening the file?",
                "Revert to Opened",
                MessageBoxButtons.OKCancel))
            {
                rom = (byte[])rom_original.Clone();
                rom_unchanged = (byte[])rom.Clone();
                undo_stack.Clear();
                refresh_all();
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close_children();
        }
    }

    public interface RomRefresh // for forms that need to redraw after ROM changes
    {
        void refresh_all();
        void refresh_chr(int tile);
        void refresh_metatile(int page);
        void refresh_map(int map);
        void refresh_close();
    }

    public class HexByteConverter : TypeConverter // for hex bytes in property grid
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() != typeof(string))
                return base.ConvertFrom(context, culture, value);

            int v = 0;
            int b = 10;
            string s = (string)value;
            s.Trim();
            if      (s.StartsWith("$" )) { b = 16; s = s.Substring(1); }
            else if (s.StartsWith("0x")) { b = 16; s = s.Substring(2); }
            else if (s.StartsWith("%" )) { b = 2;  s = s.Substring(1); }
            try
            {
                v = Convert.ToInt32(s,b);
            }
            catch (Exception)
            {
                //v = 0; // this might be nicer for debugging, custom exceptions raise the debugger
                throw new CustomIgnorableException(s + " is not a valid number.");
            }

            if (v < 0) v = 0;
            if (v > 255) v = 255;
            return v;
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return string.Format("${0:X2}", value);
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class BinaryByteConverter : HexByteConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return string.Format("%{0}{1}{2}{3}{4}{5}{6}{7}",
                    (((int)value)>>7)&1,
                    (((int)value)>>6)&1,
                    (((int)value)>>5)&1,
                    (((int)value)>>4)&1,
                    (((int)value)>>3)&1,
                    (((int)value)>>2)&1,
                    (((int)value)>>1)&1,
                    (((int)value)>>0)&1);
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class IntByteConverter : HexByteConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return string.Format("{0}", value);
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class Hex32ByteConverter : HexByteConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() != typeof(string))
                return base.ConvertFrom(context, culture, value);

            uint v = 0;
            int b = 10;
            string s = (string)value;
            s.Trim();
            if      (s.StartsWith("$" )) { b = 16; s = s.Substring(1); }
            else if (s.StartsWith("0x")) { b = 16; s = s.Substring(2); }
            else if (s.StartsWith("%" )) { b = 2;  s = s.Substring(1); }
            try
            {
                v = Convert.ToUInt32(s,b);
            }
            catch (Exception)
            {
                //v = 0; // this might be nicer for debugging, custom exceptions raise the debugger
                throw new CustomIgnorableException(s + " is not a valid number.");
            }
            return v;
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return string.Format("${0:X8}", value);
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class EnumDescriptionConverter : EnumConverter // allows Description property on enums
    {
        protected Type enum_type;
        public static string GetEnumDescription(System.Type value, string name)
        {
            FieldInfo fi = value.GetField(name);
            if (fi == null) return name;
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),false);
            return (attributes.Length>0) ? attributes[0].Description : name;
        }
        public static string GetEnumDescription(Enum value)
        {
            return GetEnumDescription(value.GetType(), value.ToString());
        }
        public static object GetEnumValue(System.Type value, string description)
        {
            FieldInfo[] fis = value.GetFields();
            foreach (FieldInfo fi in fis)
            {
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute),false);
                if (attributes.Length>0 && attributes[0].Description == description)
                    return fi.GetValue(fi.Name);
                if (fi.Name == description)
                    return fi.GetValue(fi.Name);
            }
            return description;
        }
        public EnumDescriptionConverter(Type t) : base(t.GetType())
        {
            enum_type = t;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<object> values = new List<object>();
            foreach (FieldInfo fi in enum_type.GetFields())
            {
                if (fi.IsLiteral)
                    values.Add(fi.GetValue(null));
            }
            values.Sort();
            return new StandardValuesCollection(values);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return GetEnumValue(enum_type, (string)value);
            if (value is Enum)
                return GetEnumDescription((Enum)value);
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is Enum && destinationType == typeof(string))
                return GetEnumDescription((Enum)value);
            if (value is string && destinationType == typeof(string))
                return GetEnumDescription(enum_type, (string)value);
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class TypePaletteEditor : UITypeEditor
    {
        TypePaletteEditor() { }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context) { return true; }
        public override void PaintValue(PaintValueEventArgs e)
        {
            int hw = e.Bounds.Width / 2;
            int hh = e.Bounds.Height / 2;
            for (int i=0; i<4; ++i)
            {
                uint p = (((uint)e.Value) >> (8*(3-i))) & 0xFF;
                int x = e.Bounds.X;
                int y = e.Bounds.Y;
                int w = hw;
                int h = hh;
                if ((i&1)!=0) { x += hw; w = e.Bounds.Width - hw; }
                if ((i&2)!=0) { y += hh; h = e.Bounds.Height - hh; }
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)Main.NES_PALETTE[p])), new Rectangle(x,y,w,h));
            }
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() != typeof(uint)) return value;
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if( edSvc != null )
            {
                PropertyPaletteControl prop = new PropertyPaletteControl((uint)value);
                edSvc.DropDownControl(prop);
                return prop.result;
            }
            return value;
        }
    }

    public class PropertyPaletteControl : UserControl
    {
        const int zoom = 32;
        public uint result;
        public PropertyPaletteControl(uint value)
        {
            result = value;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            Width = zoom * 4;
            Height = zoom;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            for (int i=0; i<4; ++i)
            {
                uint p = (result >> (8*(3-i))) & 0xFF;
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)Main.NES_PALETTE[p])), new Rectangle(i*zoom,0,zoom,zoom));
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            int i = e.X / zoom;
            if (i < 0 || i >= 4) return;
            int s = (8*(3-i));
            uint old = (result >> s) & 0xFF;
            PalettePick p = new PalettePick((int)(old & 63));
            p.StartPosition = FormStartPosition.CenterParent;
            if (p.ShowDialog() == DialogResult.OK)
            {
                result &= ~(uint)(0x000000FF << s);
                result |= (uint)p.picked << s;
                Refresh();
            }
        }
    }

    public class BoxlessSquareIconEditor : UITypeEditor
    {
        public override bool GetPaintValueSupported(ITypeDescriptorContext context) { return true; }
        public virtual void PaintIcon(PaintValueEventArgs e, Rectangle r) { } // override this and draw image to rectangle
        public override void PaintValue(PaintValueEventArgs e)
        {
            // remove black border (more legible without it)
            e.Graphics.ExcludeClip(new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, 1));
            e.Graphics.ExcludeClip(new Rectangle(e.Bounds.X, e.Bounds.Y, 1, e.Bounds.Height));
            e.Graphics.ExcludeClip(new Rectangle(e.Bounds.Width, e.Bounds.Y, 1, e.Bounds.Height));
            e.Graphics.ExcludeClip(new Rectangle(e.Bounds.X, e.Bounds.Height, e.Bounds.Width, 1));
            
            // create square image rectangle (unfortunately will be squished, can't make it taller)
            Rectangle r = new Rectangle(e.Bounds.X+1, e.Bounds.Y+1, e.Bounds.Width-2, e.Bounds.Height-2);
            if (r.Width > r.Height)
            {
                r.X = r.X + ((r.Width - r.Height) / 2);
                r.Width = r.Height;
            }
            if (r.Height > r.Width)
            {
                r.Y = r.Y + ((r.Height - r.Width) / 2);
                r.Height = r.Width;
            }

            // virtual member to fill in the now borderless rectangle
            PaintIcon(e,r);
        }
    }

    public class CustomIgnorableException : Exception // just something that makes it easy to ignore in the debugger
    {
        public CustomIgnorableException(string s) : base(s) {}
    }
}
