using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        Main mp;
        MapEdit me;
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
        public int MusicTrack
        {
            get { return mp.rom[ro+0x30B]; }
            set
            {
                mp.rom_modify(ro+0x30B,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Music Control")]
        [Category("Music")]
        [Description("315 - Music behaviour for this map.")]
        public int MusicControl
        {
            get { return mp.rom[ro+0x315]; }
            set
            {
                mp.rom_modify(ro+0x315,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Unknown 314")]
        [Category("Unknown")]
        [Description("314 - ?")]
        public int Unknown314
        {
            get { return mp.rom[ro+0x314]; }
            set
            {
                mp.rom_modify(ro+0x314,(byte)value);
                me.redraw_info();
            }
        }

        [DisplayName("Unknown 316")]
        [Category("Unknown")]
        [Description("316 - Rarely not 0?")]
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
}
