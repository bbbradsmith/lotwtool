using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
            this.Icon = lotwtool.Properties.Resources.Icon;
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
        public Main mp;
        public MapEdit me;

        [Browsable(false)]
        public int eo;

        public MapItemProperties(Main mp_, MapEdit me_, int eo_)
        {
            mp = mp_;
            me = me_;
            eo = eo_;
        }

        [DisplayName("Sprite Main")]
        [Category("Appearance")]
        [Description("0 - Selects the first sprite in the item's animation set.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(PropertySpriteEditor),typeof(UITypeEditor))]
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
        [Editor(typeof(PropertySpriteEditor),typeof(UITypeEditor))]
        public int SpriteDead
        {
            get { return mp.rom[eo+0x6]; }
            set {
                mp.rom_modify(eo+0x6,(byte)value);
                me.redraw_info();
            }
        }

        // TODO find out what this does
        [DisplayName("Unknown 7")]
        [Category("Unknown")]
        [Description("7 - Affects selected sprite appearance?")]
        [TypeConverter(typeof(HexByteConverter))]
        public int SpriteOther
        {
            get { return mp.rom[eo+0x7]; }
            set {
                mp.rom_modify(eo+0x7,(byte)value);
                me.redraw_info();
            }
        }

        // TODO click a palette?
        [DisplayName("Draw Attribute")]
        [Category("Appearance")]
        [Description("1 - Low 2 bits select the colour palette to use." +
                     " Bit 5 ($20) makes the enemy 'hide' behind the background until touched.")]
        [TypeConverter(typeof(HexByteConverter))]
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

        // TODO dropdown list
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

    public class PropertySpriteEditor : BoxlessSquareIconEditor
    {
        public PropertySpriteEditor() { }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }
        public override void PaintIcon(PaintValueEventArgs e, Rectangle r)
        {
            MapItemProperties mip = (MapItemProperties)e.Context.Instance;
            byte sprite = (byte)(int)e.Value;
            int palette = mip.mp.rom[mip.eo+0x1] & 3;
            Bitmap b = mip.me.make_icon(sprite,palette,true,1);
            e.Graphics.DrawImage(b,r);
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() != typeof(int)) return value;
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if( edSvc != null )
            {
                int iv = (int)value;
                int i = (((iv & 1) ^ 1) << 6) | (iv >> 2);
                MapItemProperties mip = (MapItemProperties)context.Instance;
                SpriteControl prop = new SpriteControl(mip.me, i);
                edSvc.DropDownControl(prop);
                iv = prop.result;
                if (prop.valid)
                {
                    return ((iv>>6)^1) | ((iv & 63) << 2);
                }
            }
            return value;
        }
    }

    public class SpriteControl : UserControl
    {
        const int zoom = 2;
        public int result;
        public int hover = -1;
        public bool valid = false;
        MapEdit me;
        Bitmap b = null;
        public SpriteControl(MapEdit me_, int value)
        {
            me = me_;
            result = value;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            Width = zoom * 16 * 8;
            Height = zoom * 16 * 8;
        }
        BitmapData draw_lock()
        {
            return b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, b.PixelFormat);
        }
        void draw_unlock(BitmapData d)
        {
            b.UnlockBits(d);
        }
        void draw_tile(BitmapData d, int i)
        { 
            if (i<0 || i>=64) return;
            int x = i % 8;
            int y = i / 8;
            int palette = 4;
            if (i == result) palette = 6;
            if (i == hover) palette = 5;
            int s = ((i>>6)^1) | ((i & 63) << 2);
            me.draw_icon(d,(byte)s,palette,x*16,y*16,true,zoom);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (b == null)
            {
                b = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                BitmapData d = draw_lock();
                for (int i=0; i<64;++i)
                    draw_tile(d,i);
                draw_unlock(d);
            }
            e.Graphics.DrawImage(b,0,0);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int y = e.Y / (16 * zoom);
            int x = e.X / (16 * zoom);
            if (x>=0 && x<8 && y>=0 && y < 16)
            {
                int new_hover = (y*8)+x;
                if (new_hover != hover)
                {
                    BitmapData d = draw_lock();
                    int old_hover = hover;
                    hover = new_hover;
                    draw_tile(d,old_hover);
                    draw_tile(d,hover);
                    draw_unlock(d);
                    Refresh();
                }
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            int y = e.Y / (16 * zoom);
            int x = e.X / (16 * zoom);
            if (x>=0 && x<8 && y>=0 && y <16)
            {
                result = (y*8)+x;
                valid = true;
                ParentForm.Close();
            }
        }
    }
}
