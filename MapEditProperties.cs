using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
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

        // TODO ranges, validations, dropdown lists, visual representations, pickers, etc...

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
        [Description("$0F unused")]    VF = 0x0F,
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
    public class TypeCHR2kEditor : TypeCHREditor
    {
        public TypeCHR2kEditor() : base(true) { }
    }
    public class TypeCHR1kEditor : TypeCHREditor
    {
        public TypeCHR1kEditor() : base(false) { }
    }
}
