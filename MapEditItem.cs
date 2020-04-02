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
    public partial class MapEditItem : Form
    {
        Main mp;
        MapEdit me;
        Bitmap bmp;
        int ro;

        public void redraw()
        {
            propertyGrid.Refresh();

            // sprite palette display
            int w = 256;
            int h = 16;
            if (bmp == null || bmp.Width != w || bmp.Height != h)
                bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            BitmapData d = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            int pw = w / 16;
            for (int i=0; i<16; ++i)
            {
                Main.draw_box(d,i*pw,0,pw,h,Main.NES_PALETTE[mp.rom[ro+0x3F0+i]]);
            }
            bmp.UnlockBits(d);
            pictureBox.Image = bmp;
        }

        public void set_item(int item)
        {
            if (item < 0 || item >= 16) return;
            me.last_item = item;
            comboBox.SelectedIndex = item;
            int eo = ro + 0x320 + (item*16);
            propertyGrid.SelectedObject = new MapItemProperties(mp,me,eo);
        }

        public MapEditItem(MapEdit parent, Main mp_)
        {
            me = parent;
            mp = mp_;
            ro = 16 + (1024 * me.room);
            InitializeComponent();
            int x = me.room % 4;
            int y = me.room / 4;
            Text = string.Format("Map Items {0},{1} ({2})",x,y,me.room);
            set_item(0);
            redraw();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
        }

        private void MapEditItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            me.itemedit = null;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            set_item(comboBox.SelectedIndex);
        }

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int item = comboBox.SelectedIndex;
            if (item < 0 || item >= 12) return;

            int eo = 16 + (1024 * me.room) + 0x320 + (item*16);
            mp.rom_modify_start();
            for (int i=0; i<16; ++i)
            {
                mp.rom_modify(eo+i,0,true);
            }
            redraw();
            me.redraw();
            mp.refresh_map(me.room);
            me.redraw_info();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int p = e.X / 16;
            if (p >= 0 && p < 16)
            {
                int po = ro + 0x3F0 + p;
                byte old = mp.rom[po];
                PalettePick pp = new PalettePick(old & 63);
                pp.StartPosition = FormStartPosition.CenterParent;
                if (pp.ShowDialog() == DialogResult.OK)
                {
                    byte np = (byte)pp.picked;
                    if (mp.rom_modify(po, np))
                    {
                        mp.refresh_map(me.room);
                        me.cache();
                        me.redraw(); // not covered by refresh_map
                        me.redraw_info();
                        redraw();
                    }
                }
            }
            pictureBox_MouseMove(sender,e);
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int p = e.X / 16;
            if (p >= 0 && p < 16)
                toolStripStatusLabel.Text = string.Format("{0}:{1} = {2:X2}",p/4,p%4,mp.rom[ro+0x3F0+p]);
        }
    }

    public class MapItemProperties
    {
        Main mp;
        MapEdit me;
        int eo;

        public MapItemProperties(Main mp_, MapEdit me_, int eo_)
        {
            mp = mp_;
            me = me_;
            eo = eo_;
        }

        // TODO ranges, validations, dropdown lists, visual representations, pickers, etc...

        [DisplayName("Sprite Main")]
        [Category("Appearance")]
        [Description("0 - Selects the first sprite in the item's animation set.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int SpriteMain
        {
            get { return mp.rom[eo+0x0]; }
            set {
                mp.rom_modify(eo+0x0,(byte)value);
                me.redraw();
                mp.refresh_map(me.room);
                me.redraw_info();
            }
        }

        [DisplayName("Sprite Dead")]
        [Category("Appearance")]
        [Description("6 - Selects the dead sprite.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int SpriteDead
        {
            get { return mp.rom[eo+0x6]; }
            set {
                mp.rom_modify(eo+0x6,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Sprite Other")]
        [Category("Appearance")]
        [Description("7 - Selects the other sprite?")]
        [TypeConverter(typeof(HexByteConverter))]
        public int SpriteOther
        {
            get { return mp.rom[eo+0x7]; }
            set {
                mp.rom_modify(eo+0x7,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Palette")]
        [Category("Appearance")]
        [Description("1 - Selects the colour palette to use.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Palette
        {
            get { return mp.rom[eo+0x1]; }
            set {
                mp.rom_modify(eo+0x1,(byte)value);
                me.redraw();
                mp.refresh_map(me.room);
                me.redraw_info();
            }
        }

        [DisplayName("Position X")]
        [Category("Position")]
        [Description("3 - The horizontal grid position.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int PositionX
        {
            get { return mp.rom[eo+0x2]; }
            set {
                mp.rom_modify(eo+0x2,(byte)value);
                me.redraw();
                mp.refresh_map(me.room);
                me.redraw_info();
            }
        }

        [DisplayName("Position Y")]
        [Category("Position")]
        [Description("3 - The vertical pixel position.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int PositionY
        {
            get { return mp.rom[eo+0x3]; }
            set {
                mp.rom_modify(eo+0x3,(byte)value);
                me.redraw();
                mp.refresh_map(me.room);
                me.redraw_info();
            }
        }

        [DisplayName("Hit points")]
        [Category("Attributes")]
        [Description("4 - Damge received before death. (Death takes one extra point.)")]
        [TypeConverter(typeof(IntByteConverter))]
        public int HitPoints
        {
            get { return mp.rom[eo+0x4]; }
            set {
                mp.rom_modify(eo+0x4,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Damage")]
        [Category("Attributes")]
        [Description("5 - Damage to player on contact.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Damage
        {
            get { return mp.rom[eo+0x5]; }
            set {
                mp.rom_modify(eo+0x5,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Behaviour")]
        [Category("Attributes")]
        [Description("8 - Behaviour type.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int Behave
        {
            get { return mp.rom[eo+0x8]; }
            set {
                mp.rom_modify(eo+0x8,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Speed")]
        [Category("Attributes")]
        [Description("9 - Movement speed.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Speed
        {
            get { return mp.rom[eo+0x9]; }
            set {
                mp.rom_modify(eo+0x9,(byte)value);
                me.redraw_info();
            }
        }
    }
}
