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
    public partial class Main : Form, RomRefresh
    {
        const string VERSION = "0"; // TODO set this

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
            buttonTitleScreen.Enabled = false; // TODO (romsize check)
            buttonCredits.Enabled = false; // TODO (romsize check)
            buttonMisc.Enabled = rom.Length >= 0x20010;

            mapsToolStripMenuItem.Enabled = buttonMapEdit.Enabled;
            CHRToolStripMenuItem.Enabled = buttonCHREdit.Enabled;
            titleScreenToolStripMenuItem.Enabled = buttonTitleScreen.Enabled;
            creditsToolStripMenuItem.Enabled = buttonCredits.Enabled;
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

        public void rom_modify_range(int address, byte[] c, bool append=false) // change a block of ROM in one step
        {
            if (!append) rom_modify_start();
            for (int i=0; i<c.Length; ++i)
                rom_modify(address+i,c[i],true);
        }

        public void rom_modify_hex32(int address, uint v, bool append=false) // writes big endian 32 bit
        {
            rom_modify_range(address, new byte[] {
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
            if (room >= map_count) return;
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
            // TODO
            // title screen editor
        }

        private void buttonCredits_Click(object sender, EventArgs e)
        {
            // TODO
            // credits editor
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
        protected virtual bool byterange() { return true; }
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

            if (byterange())
            {
                if (v < 0) v = 0;
                if (v > 255) v = 255;
            }
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
        protected override bool byterange() { return false; } // no clamping
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return string.Format("${0:X8}", value);
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class CustomIgnorableException : Exception // just something that makes it easy to ignore in the debugger
    {
        public CustomIgnorableException(string s) : base(s) {}
    }
}
