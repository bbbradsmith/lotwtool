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
    public partial class MapEditProperties : Form
    {
        Main mp;
        MapEdit me;

        public void redraw()
        {
            propertyGrid.Refresh();
        }

        public MapEditProperties(MapEdit parent, Main mp_)
        {
            me = parent;
            mp = mp_;
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;
            int x = me.room % 4;
            int y = me.room / 4;
            Text = string.Format("Map Properties {0},{1} ({2})",x,y,me.room);
            int ro = 16 + (1024 * me.room);
            propertyGrid.SelectedObject = new MapProperties(mp, me, ro);
        }

        private void MapEditProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            me.props = null;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
        }
    }

    public class MapProperties
    {
        public Main mp;
        public MapEdit me;
        int ro;

        public MapProperties(Main mp_, MapEdit me_, int ro_)
        {
            ro = ro_;
            mp = mp_;
            me = me_;
        }

        [DisplayName("Metatile Page")]
        [Category("Tileset")]
        [Description("300 - Selects the page of metatile definitions to use.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int MetatileSet
        {
            get { return mp.rom[ro+0x300]; }
            set {
                mp.rom_modify(ro+0x300,(byte)value);
                mp.refresh_metatile(value);
                me.redraw_info();
            }
        }

        [DisplayName("Terrain CHR 0")]
        [Category("Tileset")]
        [Description("305 - Selects the first 2K page of CHR to use for the background.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(TypeCHR2kEditor),typeof(UITypeEditor))]
        public int CHR0
        {
            get { return mp.rom[ro+0x305]; }
            set
            {
                mp.rom_modify(ro+0x305,(byte)value);
                me.reload_chr();
                me.redraw_info();
            }
        }

        [DisplayName("Terrain CHR 1")]
        [Category("Tileset")]
        [Description("306 - Selects the second 2K page of CHR to use for the background.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(TypeCHR2kEditor),typeof(UITypeEditor))]
        public int CHR1
        {
            get { return mp.rom[ro+0x306]; }
            set
            {
                mp.rom_modify(ro+0x306,(byte)value);
                me.reload_chr();
                me.redraw_info();
            }
        }

        [DisplayName("Enemy CHR")]
        [Category("Tileset")]
        [Description("301 - Selects the 1K page of CHR to use for enemies.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(TypeCHR1kEditor),typeof(UITypeEditor))]
        public int CHREnemy
        {
            get { return mp.rom[ro+0x301]; }
            set
            {
                mp.rom_modify(ro+0x301,(byte)value);
                me.reload_chr();
                me.redraw_info();
            }
        }

        [DisplayName("Treasure Active")]
        [Category("Treasure")]
        [Description("307 - Whether the treasure chest appears.")]
        public bool TreasureActive
        {
            get { return mp.rom[ro+0x307] == 1; }
            set
            {
                mp.rom_modify(ro+0x307,(byte)(value?1:0));
                mp.refresh_map(me.room);
                me.redraw();
                me.redraw_info();
            }
        }

        [DisplayName("Treasure Contents")]
        [Category("Treasure")]
        [Description("30A - Contents of the treasure chest.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(TreasureEditor),typeof(UITypeEditor))]
        public int TreasureContents
        {
            get { return mp.rom[ro+0x30A]; }
            set
            {
                mp.rom_modify(ro+0x30A,(byte)value);
                mp.refresh_map(me.room);
                me.redraw();
                me.redraw_info();
            }
        }

        [DisplayName("Treasure X")]
        [Category("Treasure")]
        [Description("308 - Horizontal grid location of treasure.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int TreasureX
        {
            get { return mp.rom[ro+0x308]; }
            set
            {
                mp.rom_modify(ro+0x308,(byte)value);
                mp.refresh_map(me.room);
                me.redraw();
                me.redraw_info();
            }
        }

        [DisplayName("Treasure Y")]
        [Category("Treasure")]
        [Description("309 - Vertical pixel location of treasure.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int TreasureY
        {
            get { return mp.rom[ro+0x309]; }
            set
            {
                mp.rom_modify(ro+0x309,(byte)value);
                mp.refresh_map(me.room);
                me.redraw();
                me.redraw_info();
            }
        }

        [DisplayName("Shop Item 0")]
        [Category("Shop")]
        [Description("310 - First shop item.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(ShopEditor),typeof(UITypeEditor))]
        public int ShopItem0
        {
            get { return mp.rom[ro+0x310]; }
            set
            {
                mp.rom_modify(ro+0x310,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Shop Price 0")]
        [Category("Shop")]
        [Description("311 - First shop price.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int ShopPrice0
        {
            get { return mp.rom[ro+0x311]; }
            set
            {
                mp.rom_modify(ro+0x311,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Shop Item 1")]
        [Category("Shop")]
        [Description("312 - Second shop item.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(ShopEditor),typeof(UITypeEditor))]
        public int ShopItem1
        {
            get { return mp.rom[ro+0x312]; }
            set
            {
                mp.rom_modify(ro+0x312,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Shop Price 1")]
        [Category("Shop")]
        [Description("313 - Second shop price.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int ShopPrice1
        {
            get { return mp.rom[ro+0x313]; }
            set
            {
                mp.rom_modify(ro+0x313,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Teleport Map X")]
        [Category("Teleport")]
        [Description("30C - Celina teleport map horizontal coordinate.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int TeleportMapX
        {
            get { return mp.rom[ro+0x30C]; }
            set
            {
                mp.rom_modify(ro+0x30C,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Teleport Map Y")]
        [Category("Teleport")]
        [Description("30D - Celina teleport map vertical coordinate.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int TeleportMapY
        {
            get { return mp.rom[ro+0x30D]; }
            set
            {
                mp.rom_modify(ro+0x30D,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Teleport Player X")]
        [Category("Teleport")]
        [Description("30E - Celina teleport location horizontal grid coordinate.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int TeleportPlayerX
        {
            get { return mp.rom[ro+0x30E]; }
            set
            {
                mp.rom_modify(ro+0x30E,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Teleport Player Y")]
        [Category("Teleport")]
        [Description("30F - Celina teleport location vertical pixel coordinate.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int TeleportPlayerY
        {
            get { return mp.rom[ro+0x30F]; }
            set
            {
                mp.rom_modify(ro+0x30F,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Secret Wall Tile")]
        [Category("Secret Wall")]
        [Description("302 - The tile which will secretly be replaced when touched.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(GreyPropertyTileEditor),typeof(UITypeEditor))]
        public int SecretWallTile
        {
            get { return mp.rom[ro+0x302]; }
            set
            {
                mp.rom_modify(ro+0x302,(byte)value);
                mp.refresh_map(me.room);
                me.redraw();
                me.redraw_info();
            }
        }

        [DisplayName("Secret Wall Replace")]
        [Category("Secret Wall")]
        [Description("303 - The tile which will replace the secret wall when touched.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(PropertyTileEditor),typeof(UITypeEditor))]
        public int SecretWallReplace
        {
            get { return mp.rom[ro+0x303]; }
            set
            {
                mp.rom_modify(ro+0x303,(byte)value);
                mp.refresh_map(me.room);
                me.redraw();
                me.redraw_info();
            }
        }

        [DisplayName("Block Replace")]
        [Category("Secret Wall")]
        [Description("304 - The tile which will replace a block when moved or destroyed.")]
        [TypeConverter(typeof(HexByteConverter))]
        [Editor(typeof(PropertyTileEditor),typeof(UITypeEditor))]
        public int BlockReplace
        {
            get { return mp.rom[ro+0x304]; }
            set
            {
                mp.rom_modify(ro+0x304,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Music Track")]
        [Category("Music")]
        [Description("30B - Music to play on this map.")]
        [TypeConverter(typeof(EnumDescriptionConverter))]
        public MusicEnum MusicTrack
        {
            get { return (MusicEnum)mp.rom[ro+0x30B]; }
            set
            {
                mp.rom_modify(ro+0x30B,(byte)value);
                mp.rom_modify(ro+0x315,(byte)(1<<(int)value));
                me.redraw_info();
            }
        }

        [DisplayName("Music Control")]
        [Category("Music")]
        [Description("315 - Bitfield to prevent switching music. If (1 << current track) is set, will keep current music instead of switching. 0 to always switch." +
            " Also used in strange way by shop map (vestigial tile lookup, similar to 'Unknown 316').")]
        [TypeConverter(typeof(HexByteConverter))]
        public int MusicControl
        {
            get { return mp.rom[ro+0x315]; }
            set
            {
                mp.rom_modify(ro+0x315,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Demo Xemn")]
        [Category("Title Demo")]
        [Description("314:0 - Xemn may appear in demonstrations of this room.")]
        public bool DemoXemn
        {
            get { return mp.rom_bit(ro+0x314,0); }
            set { mp.rom_modify_bit(ro+0x314,0,value); me.redraw_info(); }
        }

        [DisplayName("Demo Meyna")]
        [Category("Title Demo")]
        [Description("314:1 - Meyna may appear in demonstrations of this room.")]
        public bool DemoMeyna
        {
            get { return mp.rom_bit(ro+0x314,1); }
            set { mp.rom_modify_bit(ro+0x314,1,value); me.redraw_info(); }
        }

        [DisplayName("Demo Roas")]
        [Category("Title Demo")]
        [Description("314:2 - Roas may appear in demonstrations of this room.")]
        public bool DemoRoas
        {
            get { return mp.rom_bit(ro+0x314,2); }
            set { mp.rom_modify_bit(ro+0x314,2,value); me.redraw_info(); }
        }

        [DisplayName("Demo Lyll")]
        [Category("Title Demo")]
        [Description("314:3 - Lyll may appear in demonstrations of this room.")]
        public bool DemoLyll
        {
            get { return mp.rom_bit(ro+0x314,3); }
            set { mp.rom_modify_bit(ro+0x314,3,value); me.redraw_info(); }
        }

        [DisplayName("Demo Pochi")]
        [Category("Title Demo")]
        [Description("314:4 - Pochi may appear in demonstrations of this room.")]
        public bool DemoPochi
        {
            get { return mp.rom_bit(ro+0x314,4); }
            set { mp.rom_modify_bit(ro+0x314,4,value); me.redraw_info(); }
        }

        [DisplayName("Unknown 316")]
        [Category("Unknown")]
        [Description("316 - Rarely not 0? Seems unused by dungeon maps," +
            " but on the shop map used as a tile lookup when loading which renders uselessly offscreen? Possibly vestigial.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int Unknown316
        {
            get { return mp.rom[ro+0x316]; }
            set
            {
                mp.rom_modify(ro+0x316,(byte)value);
                me.redraw_info();
            }
        }
    }

    public enum MusicEnum
    {
        [Description("$00 dungeon")]   V0 = 0x00,
        [Description("$01 xemn")]      V1 = 0x01,
        [Description("$02 meyna")]     V2 = 0x02,
        [Description("$03 lyll")]      V3 = 0x03,
        [Description("$04 pochi")]     V4 = 0x04,
        [Description("$05 dragon")]    V5 = 0x05,
        [Description("$06 inn")]       V6 = 0x06,
        [Description("$07 shop")]      V7 = 0x07,
        [Description("$08 death")]     V8 = 0x08,
        [Description("$09 title")]     V9 = 0x09,
        [Description("$0A credits")]   VA = 0x0A,
        [Description("$0B boss")]      VB = 0x0B,
        [Description("$0C home")]      VC = 0x0C,
        [Description("$0D inventory")] VD = 0x0D,
        [Description("$0E pickup")]    VE = 0x0E,
        [Description("$0F roas?")]     VF = 0x0F,
    }

    public class TypeCHREditor : UITypeEditor
    {
        protected bool dualpage;
        public TypeCHREditor(bool dualpage_) { dualpage = dualpage_; }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.Modal; }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() != typeof(int)) return value;
            MapProperties mep = (MapProperties)context.Instance;
            CHRSelect cs = new CHRSelect(mep.mp,true);
            cs.StartPosition = FormStartPosition.CenterParent;
            cs.preselect = (int)value;
            cs.dualpage = dualpage;
            cs.sprite = !dualpage;
            if (cs.ShowDialog() == DialogResult.OK)
            {
                if (cs.highlight >= 0 && cs.highlight < mep.mp.chr_count)
                {
                    value = cs.highlight;
                    if (dualpage) value = (int)value & ~1;
                }
            }
            return value;
        }
    }
    public class TypeCHR2kEditor : TypeCHREditor { public TypeCHR2kEditor() : base(true ) { } }
    public class TypeCHR1kEditor : TypeCHREditor { public TypeCHR1kEditor() : base(false) { } }

    public class CollectibleItemEditor : BoxlessSquareIconEditor
    {
        protected bool shop;
        public CollectibleItemEditor(bool shop_) { shop = shop_; }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.DropDown; }
        public override void PaintIcon(PaintValueEventArgs e, Rectangle r)
        {
            MapProperties mep = (MapProperties)e.Context.Instance;
            byte sprite = (byte)(0x81 + ((int)e.Value*4));
            if (shop) sprite += (8*4);
            Bitmap b = mep.me.make_icon(sprite,1,true,1);
            e.Graphics.DrawImage(b,r);
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() != typeof(int)) return value;
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if( edSvc != null )
            {
                MapProperties mep = (MapProperties)context.Instance;
                CollectibleItemControl prop = new CollectibleItemControl(mep.me, shop, (int)value);
                edSvc.DropDownControl(prop);
                return prop.result;
            }
            return value;
        }
    }
    public class TreasureEditor : CollectibleItemEditor { public TreasureEditor() : base(false) { } }
    public class ShopEditor     : CollectibleItemEditor { public ShopEditor()     : base(true ) { } }

    public class CollectibleItemControl : UserControl
    {
        const int zoom = 2;
        public int result;
        public int hover = -1;
        MapEdit me;
        bool shop;
        Bitmap b = null;
        public CollectibleItemControl(MapEdit me_, bool shop_, int value)
        {
            me = me_;
            shop = shop_;
            result = value;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            Width = zoom * 16 * 8;
            Height = zoom * 16 * (shop ? 2 : 3);
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
            if (i<0 || i>=(shop?16:24)) return;
            int x = i % 8;
            int y = i / 8;
            int palette = 4;
            if (i == result) palette = 6;
            if (i == hover) palette = 5;
            if (shop) i += 8;
            me.draw_icon(d,(byte)(0x81+(i*4)),palette,x*16,y*16,true,zoom);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (b == null)
            {
                b = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                BitmapData d = draw_lock();
                for (int i=0; i<(shop?16:24);++i)
                    draw_tile(d,i);
                draw_unlock(d);
            }
            e.Graphics.DrawImage(b,0,0);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            int y = e.Y / (16 * zoom);
            int x = e.X / (16 * zoom);
            if (x>=0 && x<8 && y>=0 && y <(shop?2:3))
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
            if (x>=0 && x<8 && y>=0 && y <(shop?2:3))
            {
                result = (y*8)+x;
                ParentForm.Close();
            }
        }
    }

    public class PropertyTileEditor : BoxlessSquareIconEditor
    {
        protected bool grey = false;
        public PropertyTileEditor() { }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return UITypeEditorEditStyle.Modal; }
        public override void PaintIcon(PaintValueEventArgs e, Rectangle r)
        {
            MapProperties mep = (MapProperties)e.Context.Instance;
            byte tile = (byte)(int)e.Value;
            Bitmap b = mep.me.make_icon(tile,grey?4:(tile>>6),false,1);
            e.Graphics.DrawImage(b,r);
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value.GetType() != typeof(int)) return value;
            MapProperties mep = (MapProperties)context.Instance;
            MapEditTile et = new MapEditTile(mep.me, mep.mp, true);
            et.StartPosition = FormStartPosition.CenterParent;
            et.show_palselect = !grey;
            et.set_modal_tile(-1,-1,(int)value);
            if (et.ShowDialog() == DialogResult.OK)
            {
                value = (int)et.result;
                if (grey) value = (int)(et.result & 0x3F);
            }
            return value;
        }
    }
    public class GreyPropertyTileEditor : PropertyTileEditor
    {
        public GreyPropertyTileEditor() {  grey = true; }
    }
}
