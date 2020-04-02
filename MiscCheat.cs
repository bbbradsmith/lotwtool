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
    public partial class MiscCheat : Form
    {
        Main mp;

        public void redraw()
        {
            propertyGrid.Refresh();
        }

        private void collapseCategory(string name)
        {
            GridItem root = propertyGrid.SelectedGridItem;
            while (root.Parent != null) root = root.Parent;
            foreach (GridItem cat in root.GridItems)
            {
                if (cat.Label == name)
                    cat.Expanded = false;
            }
        }

        public MiscCheat(Main parent)
        {
            mp = parent;
            InitializeComponent();
            MiscCheatProperties p = new MiscCheatProperties(mp);
            propertyGrid.SelectedObject = p;
            collapseCategory("Items Extra"); // nicer if this is collapsed by default

            // errors locating family data
            if (p.errors.Length > 0 && mp.misc_errors_shown == false)
            {
                MessageBox.Show(p.errors, "Miscellany / Cheat errors!");
                mp.misc_errors_shown = true;
                collapseCategory("Stats 0 Xemn");
                collapseCategory("Stats 1 Meyna");
                collapseCategory("Stats 2 Roas");
                collapseCategory("Stats 3 Lyll");
                collapseCategory("Stats 4 Pochi");
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
        }

        private void saveAndRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.saveAndTestToolStripMenuItem_Click(sender,e);
        }

        private void MiscCheat_FormClosing(object sender, FormClosingEventArgs e)
        {
            mp.misc_cheat = null;
        }
    }

    public class MiscCheatProperties
    {
        Main mp;
        int family_offset;
        string rom_type = "Unknown. Corrupt?";
        public string errors = "";

        public MiscCheatProperties(Main parent)
        {
            mp = parent;

            // find starting item table
            family_offset   = 16 + 0x1FFA7; // default = Legacy of the Wizard
            if      (mp.rom_compare(16+0x1E1E7,new byte[]{0xB9,0xA7,0xFF}))
            {
                rom_type = "Legacy of the Wizard (NES)";
            }
            else if (mp.rom_compare(16+0x1E1F1,new byte[]{0xB9,0xB6,0xFF}))
            {
                rom_type = "Dragon Slayer IV (Famicom)";
                family_offset = 16 + 0x1FFB6;
            }
            else
            {
                errors += "Starting family stats location could not be detected. Corrupt ROM?\n";
            }
        }

        // (ROM)

        [DisplayName("Region Type")]
        [Category("(ROM)")]
        [Description("Detected base ROM version.")]
        public string ROMType
        {
            get { return rom_type; }
        }

        [DisplayName("Start location patch removed?")]
        [Category("(ROM)")]
        [Description("Set true to remove the \"run from here\" patch if accidentally present.")]
        public bool ROMRFHPatch
        {
            get { return !MapEdit.detect_run_from_here(mp); }
            set { if (value) MapEdit.remove_run_from_here(mp); }
        }

        // Items

        [DisplayName("All Items = 4")]
        [Category("Items")]
        [Description("19BFF-19C0F")]
        public bool AllItems
        {
            get { return mp.rom_compare(16+0x19BFF, new byte[]{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}); }
            set { mp.rom_modify_range(  16+0x19BFF, value ?
                  (new byte[]{4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4}):
                  (new byte[]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}));
                  mp.refresh_misc(); }
        }

        [DisplayName("Gold")]
        [Category("Items")]
        [Description("19BF9")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Gold
        {
            get { return mp.rom[16+0x19BF9]; }
            set { mp.rom_modify(16+0x19BF9,(byte)value); }
        }

        [DisplayName("Keys")]
        [Category("Items")]
        [Description("19BFA")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Keys
        {
            get { return mp.rom[16+0x19BFA]; }
            set { mp.rom_modify(16+0x19BFA,(byte)value); }
        }

        // Items Extra

        [DisplayName("Wings")]
        [Category("Items Extra")]
        [Description("19BFF+0")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Wings
        {
            get { return mp.rom[16+0x19BFF+0]; }
            set { mp.rom_modify(16+0x19BFF+0,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Armor")]
        [Category("Items Extra")]
        [Description("19BFF+1")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Armor
        {
            get { return mp.rom[16+0x19BFF+1]; }
            set { mp.rom_modify(16+0x19BFF+1,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Mattock")]
        [Category("Items Extra")]
        [Description("19BFF+2")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Mattock
        {
            get { return mp.rom[16+0x19BFF+2]; }
            set { mp.rom_modify(16+0x19BFF+2,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Glove")]
        [Category("Items Extra")]
        [Description("19BFF+3")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Glove
        {
            get { return mp.rom[16+0x19BFF+3]; }
            set { mp.rom_modify(16+0x19BFF+3,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Rod")]
        [Category("Items Extra")]
        [Description("19BFF+4")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Rod
        {
            get { return mp.rom[16+0x19BFF+4]; }
            set { mp.rom_modify(16+0x19BFF+4,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Power Boots")]
        [Category("Items Extra")]
        [Description("19BFF+5")]
        [TypeConverter(typeof(IntByteConverter))]
        public int PowerBoots
        {
            get { return mp.rom[16+0x19BFF+5]; }
            set { mp.rom_modify(16+0x19BFF+5,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Jump Shoes")]
        [Category("Items Extra")]
        [Description("19BFF+6")]
        [TypeConverter(typeof(IntByteConverter))]
        public int JumpShoes
        {
            get { return mp.rom[16+0x19BFF+6]; }
            set { mp.rom_modify(16+0x19BFF+6,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Key Stick")]
        [Category("Items Extra")]
        [Description("19BFF+7")]
        [TypeConverter(typeof(IntByteConverter))]
        public int KeyStick
        {
            get { return mp.rom[16+0x19BFF+7]; }
            set { mp.rom_modify(16+0x19BFF+7,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Power Knuckle")]
        [Category("Items Extra")]
        [Description("19BFF+8")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Knuckle
        {
            get { return mp.rom[16+0x19BFF+8]; }
            set { mp.rom_modify(16+0x19BFF+8,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Fire Rod")]
        [Category("Items Extra")]
        [Description("19BFF+9")]
        [TypeConverter(typeof(IntByteConverter))]
        public int FireRod
        {
            get { return mp.rom[16+0x19BFF+9]; }
            set { mp.rom_modify(16+0x19BFF+9,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Shield")]
        [Category("Items Extra")]
        [Description("19BFF+10")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Shield
        {
            get { return mp.rom[16+0x19BFF+10]; }
            set { mp.rom_modify(16+0x19BFF+10,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Magic Bottle")]
        [Category("Items Extra")]
        [Description("19BFF+11")]
        [TypeConverter(typeof(IntByteConverter))]
        public int MagicBottle
        {
            get { return mp.rom[16+0x19BFF+11]; }
            set { mp.rom_modify(16+0x19BFF+11,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Elixer")]
        [Category("Items Extra")]
        [Description("19BFF+12")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Elixer
        {
            get { return mp.rom[16+0x19BFF+12]; }
            set { mp.rom_modify(16+0x19BFF+12,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Crystal")]
        [Category("Items Extra")]
        [Description("19BFF+13")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Crystal
        {
            get { return mp.rom[16+0x19BFF+13]; }
            set { mp.rom_modify(16+0x19BFF+13,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Crowns")]
        [Category("Items Extra")]
        [Description("19BFF+14")]
        [TypeConverter(typeof(IntByteConverter))]
        public int Crowns
        {
            get { return mp.rom[16+0x19BFF+14]; }
            set { mp.rom_modify(16+0x19BFF+14,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("DragonSlayer")]
        [Category("Items Extra")]
        [Description("19BFF+15")]
        [TypeConverter(typeof(IntByteConverter))]
        public int DragonSlayer
        {
            get { return mp.rom[16+0x19BFF+15]; }
            set { mp.rom_modify(16+0x19BFF+15,(byte)value); mp.refresh_misc(); }
        }

        // Dungeon Exit

        [DisplayName("Dungeon Exit Map X")]
        [Category("Dungeon Exit")]
        [Description("1D86B - Dungeon exit teleport map horizontal coordinate.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int DungeonExitMapX
        {
            get { return mp.rom[16+0x1D86B]; }
            set { mp.rom_modify(16+0x1D86B,(byte)value); }
        }

        [DisplayName("Dungeon Exit Map Y")]
        [Category("Dungeon Exit")]
        [Description("1D867 - Dungeon exit teleport map vertical coordinate.")]
        [TypeConverter(typeof(IntByteConverter))]
        public int DungeonExitMapY
        {
            get { return mp.rom[16+0x1D867]; }
            set { mp.rom_modify(16+0x1D867,(byte)value); }
        }

        [DisplayName("Dungeon Exit Player X")]
        [Category("Dungeon Exit")]
        [Description("1D877 - Dungeon exit teleport location horizontal grid coordinate.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int DungeonExitPlayerX
        {
            get { return mp.rom[16+0x1D877]; }
            set { mp.rom_modify(16+0x1D877,(byte)value); }
        }

        [DisplayName("Dungeon Exit Scroll X")]
        [Category("Dungeon Exit")]
        [Description("1D86F - Dungeon exit scroll grid position. Min $00, Max $30, should be 0-15 less than player X.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int DungeonExitScrollX
        {
            get { return mp.rom[16+0x1D86F]; }
            set { mp.rom_modify(16+0x1D86F,(byte)value); }
        }

        [DisplayName("Dungeon Exit Player Y")]
        [Category("Dungeon Exit")]
        [Description("1D873 - Dungeon exit teleport location vertical pixel coordinate.")]
        [TypeConverter(typeof(HexByteConverter))]
        public int DungeonExitPlayerY
        {
            get { return mp.rom[16+0x1D873]; }
            set { mp.rom_modify(16+0x1D873,(byte)value); }
        }

        // Family Stats
        // Stats 0 Xemn, Stats 1 Meyna, Stats 2 Roas, Stats 3 Lyll, Stats 4 Pochi

        [DisplayName("Xemn Jump")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+0")]
        [TypeConverter(typeof(IntByteConverter))]
        public int XemnJump
        {
            get { return mp.rom[family_offset+0]; }
            set { mp.rom_modify(family_offset+0,(byte)value); }
        }

        [DisplayName("Xemn Strength")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+1")]
        [TypeConverter(typeof(IntByteConverter))]
        public int XemnStrength
        {
            get { return mp.rom[family_offset+1]; }
            set { mp.rom_modify(family_offset+1,(byte)value); }
        }

        [DisplayName("Xemn Shots")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+2")]
        [TypeConverter(typeof(IntByteConverter))]
        public int XemnShots
        {
            get { return mp.rom[family_offset+2]; }
            set { mp.rom_modify(family_offset+2,(byte)value); }
        }

        [DisplayName("Xemn Range")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+3")]
        [TypeConverter(typeof(IntByteConverter))]
        public int XemnRange
        {
            get { return mp.rom[family_offset+3]; }
            set { mp.rom_modify(family_offset+3,(byte)value); }
        }

        [DisplayName("Xemn Equip 0")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+20")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int XemnEquip0
        {
            get { return mp.rom[family_offset+20]; }
            set { mp.rom_modify(family_offset+20,(byte)value); }
        }

        [DisplayName("Xemn Equip 1")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+21")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int XemnEquip1
        {
            get { return mp.rom[family_offset+21]; }
            set { mp.rom_modify(family_offset+21,(byte)value); }
        }

        // TODO these palette entries should use a custom palette picker/editor control
        [DisplayName("Xemn Palette")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+30")]
        [TypeConverter(typeof(Hex32ByteConverter))]
        public uint XemnPalette
        {
            get { return mp.rom_hex32(family_offset+30); }
            set { mp.rom_modify_hex32(family_offset+30,value); }
        }

        [DisplayName("Meyna Jump")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+4")]
        [TypeConverter(typeof(IntByteConverter))]
        public int MeynaJump
        {
            get { return mp.rom[family_offset+4]; }
            set { mp.rom_modify(family_offset+4,(byte)value); }
        }

        [DisplayName("Meyna Strength")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+5")]
        [TypeConverter(typeof(IntByteConverter))]
        public int MeynaStrength
        {
            get { return mp.rom[family_offset+5]; }
            set { mp.rom_modify(family_offset+5,(byte)value); }
        }

        [DisplayName("Meyna Shots")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+6")]
        [TypeConverter(typeof(IntByteConverter))]
        public int MeynaShots
        {
            get { return mp.rom[family_offset+6]; }
            set { mp.rom_modify(family_offset+6,(byte)value); }
        }

        [DisplayName("Meyna Range")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+7")]
        [TypeConverter(typeof(IntByteConverter))]
        public int MeynaRange
        {
            get { return mp.rom[family_offset+7]; }
            set { mp.rom_modify(family_offset+7,(byte)value); }
        }

        [DisplayName("Meyna Equip 0")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+22")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int MeynaEquip0
        {
            get { return mp.rom[family_offset+22]; }
            set { mp.rom_modify(family_offset+22,(byte)value); }
        }

        [DisplayName("Meyna Equip 1")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+23")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int MeynaEquip1
        {
            get { return mp.rom[family_offset+23]; }
            set { mp.rom_modify(family_offset+23,(byte)value); }
        }

        [DisplayName("Meyna Palette")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+34")]
        [TypeConverter(typeof(Hex32ByteConverter))]
        public uint MeynaPalette
        {
            get { return mp.rom_hex32(family_offset+34); }
            set { mp.rom_modify_hex32(family_offset+34,value); }
        }

        [DisplayName("Roas Jump")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+8")]
        [TypeConverter(typeof(IntByteConverter))]
        public int RoasJump
        {
            get { return mp.rom[family_offset+8]; }
            set { mp.rom_modify(family_offset+8,(byte)value); }
        }

        [DisplayName("Roas Strength")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+9")]
        [TypeConverter(typeof(IntByteConverter))]
        public int RoasStrength
        {
            get { return mp.rom[family_offset+9]; }
            set { mp.rom_modify(family_offset+9,(byte)value); }
        }

        [DisplayName("Roas Shots")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+10")]
        [TypeConverter(typeof(IntByteConverter))]
        public int RoasShots
        {
            get { return mp.rom[family_offset+10]; }
            set { mp.rom_modify(family_offset+10,(byte)value); }
        }

        [DisplayName("Roas Range")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+11")]
        [TypeConverter(typeof(IntByteConverter))]
        public int RoasRange
        {
            get { return mp.rom[family_offset+11]; }
            set { mp.rom_modify(family_offset+11,(byte)value); }
        }

        [DisplayName("Roas Equip 0")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+24")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int RoasEquip0
        {
            get { return mp.rom[family_offset+24]; }
            set { mp.rom_modify(family_offset+24,(byte)value); }
        }

        [DisplayName("Roas Equip 1")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+25")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int RoasEquip1
        {
            get { return mp.rom[family_offset+25]; }
            set { mp.rom_modify(family_offset+25,(byte)value); }
        }

        [DisplayName("Roas Palette")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+38")]
        [TypeConverter(typeof(Hex32ByteConverter))]
        public uint RoasPalette
        {
            get { return mp.rom_hex32(family_offset+38); }
            set { mp.rom_modify_hex32(family_offset+38,value); }
        }

        [DisplayName("Lyll Jump")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+12")]
        [TypeConverter(typeof(IntByteConverter))]
        public int LyllJump
        {
            get { return mp.rom[family_offset+12]; }
            set { mp.rom_modify(family_offset+12,(byte)value); }
        }

        [DisplayName("Lyll Strength")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+13")]
        [TypeConverter(typeof(IntByteConverter))]
        public int LyllStrength
        {
            get { return mp.rom[family_offset+13]; }
            set { mp.rom_modify(family_offset+13,(byte)value); }
        }

        [DisplayName("Lyll Shots")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+14")]
        [TypeConverter(typeof(IntByteConverter))]
        public int LyllShots
        {
            get { return mp.rom[family_offset+14]; }
            set { mp.rom_modify(family_offset+14,(byte)value); }
        }

        [DisplayName("Lyll Range")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+15")]
        [TypeConverter(typeof(IntByteConverter))]
        public int LyllRange
        {
            get { return mp.rom[family_offset+15]; }
            set { mp.rom_modify(family_offset+15,(byte)value); }
        }

        [DisplayName("Lyll Equip 0")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+26")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int LyllEquip0
        {
            get { return mp.rom[family_offset+26]; }
            set { mp.rom_modify(family_offset+26,(byte)value); }
        }

        [DisplayName("Lyll Equip 1")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+27")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int LyllEquip1
        {
            get { return mp.rom[family_offset+27]; }
            set { mp.rom_modify(family_offset+27,(byte)value); }
        }

        [DisplayName("Lyll Palette")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+42")]
        [TypeConverter(typeof(Hex32ByteConverter))]
        public uint LyllPalette
        {
            get { return mp.rom_hex32(family_offset+42); }
            set { mp.rom_modify_hex32(family_offset+42,value); }
        }

        [DisplayName("Pochi Jump")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+16")]

        [TypeConverter(typeof(IntByteConverter))]
        public int PochiJump
        {
            get { return mp.rom[family_offset+16]; }
            set { mp.rom_modify(family_offset+16,(byte)value); }
        }

        [DisplayName("Pochi Strength")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+17")]
        [TypeConverter(typeof(IntByteConverter))]
        public int PochiStrength
        {
            get { return mp.rom[family_offset+17]; }
            set { mp.rom_modify(family_offset+17,(byte)value); }
        }

        [DisplayName("Pochi Shots")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+18")]
        [TypeConverter(typeof(IntByteConverter))]
        public int PochiShots
        {
            get { return mp.rom[family_offset+18]; }
            set { mp.rom_modify(family_offset+18,(byte)value); }
        }

        [DisplayName("Pochi Range")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+19")]
        [TypeConverter(typeof(IntByteConverter))]
        public int PochiRange
        {
            get { return mp.rom[family_offset+19]; }
            set { mp.rom_modify(family_offset+19,(byte)value); }
        }

        [DisplayName("Pochi Equip 0")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+28")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int PochiEquip0
        {
            get { return mp.rom[family_offset+28]; }
            set { mp.rom_modify(family_offset+28,(byte)value); }
        }

        [DisplayName("Pochi Equip 1")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+29")]
        [TypeConverter(typeof(BinaryByteConverter))]
        public int PochiEquip1
        {
            get { return mp.rom[family_offset+29]; }
            set { mp.rom_modify(family_offset+29,(byte)value); }
        }

        [DisplayName("Pochi Palette")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+46")]
        [TypeConverter(typeof(Hex32ByteConverter))]
        public uint PochiPalette
        {
            get { return mp.rom_hex32(family_offset+46); }
            set { mp.rom_modify_hex32(family_offset+46,value); }
        }

    }
}
