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
        public int Gold
        {
            get { return mp.rom[16+0x19BF9]; }
            set { mp.rom_modify(16+0x19BF9,(byte)value); }
        }

        [DisplayName("Keys")]
        [Category("Items")]
        [Description("19BFA")]
        public int Keys
        {
            get { return mp.rom[16+0x19BFA]; }
            set { mp.rom_modify(16+0x19BFA,(byte)value); }
        }

        // Items Extra

        [DisplayName("Wings")]
        [Category("Items Extra")]
        [Description("19BFF+0")]
        public int Wings
        {
            get { return mp.rom[16+0x19BFF+0]; }
            set { mp.rom_modify(16+0x19BFF+0,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Armor")]
        [Category("Items Extra")]
        [Description("19BFF+1")]
        public int Armor
        {
            get { return mp.rom[16+0x19BFF+1]; }
            set { mp.rom_modify(16+0x19BFF+1,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Mattock")]
        [Category("Items Extra")]
        [Description("19BFF+2")]
        public int Mattock
        {
            get { return mp.rom[16+0x19BFF+2]; }
            set { mp.rom_modify(16+0x19BFF+2,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Glove")]
        [Category("Items Extra")]
        [Description("19BFF+3")]
        public int Glove
        {
            get { return mp.rom[16+0x19BFF+3]; }
            set { mp.rom_modify(16+0x19BFF+3,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Rod")]
        [Category("Items Extra")]
        [Description("19BFF+4")]
        public int Rod
        {
            get { return mp.rom[16+0x19BFF+4]; }
            set { mp.rom_modify(16+0x19BFF+4,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Power Boots")]
        [Category("Items Extra")]
        [Description("19BFF+5")]
        public int PowerBoots
        {
            get { return mp.rom[16+0x19BFF+5]; }
            set { mp.rom_modify(16+0x19BFF+5,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Jump Shoes")]
        [Category("Items Extra")]
        [Description("19BFF+6")]
        public int JumpShoes
        {
            get { return mp.rom[16+0x19BFF+6]; }
            set { mp.rom_modify(16+0x19BFF+6,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Key Stick")]
        [Category("Items Extra")]
        [Description("19BFF+7")]
        public int KeyStick
        {
            get { return mp.rom[16+0x19BFF+7]; }
            set { mp.rom_modify(16+0x19BFF+7,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Power Knuckle")]
        [Category("Items Extra")]
        [Description("19BFF+8")]
        public int Knuckle
        {
            get { return mp.rom[16+0x19BFF+8]; }
            set { mp.rom_modify(16+0x19BFF+8,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Fire Rod")]
        [Category("Items Extra")]
        [Description("19BFF+9")]
        public int FireRod
        {
            get { return mp.rom[16+0x19BFF+9]; }
            set { mp.rom_modify(16+0x19BFF+9,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Shield")]
        [Category("Items Extra")]
        [Description("19BFF+10")]
        public int Shield
        {
            get { return mp.rom[16+0x19BFF+10]; }
            set { mp.rom_modify(16+0x19BFF+10,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Magic Bottle")]
        [Category("Items Extra")]
        [Description("19BFF+11")]
        public int MagicBottle
        {
            get { return mp.rom[16+0x19BFF+11]; }
            set { mp.rom_modify(16+0x19BFF+11,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Elixer")]
        [Category("Items Extra")]
        [Description("19BFF+12")]
        public int Elixer
        {
            get { return mp.rom[16+0x19BFF+12]; }
            set { mp.rom_modify(16+0x19BFF+12,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Crystal")]
        [Category("Items Extra")]
        [Description("19BFF+13")]
        public int Crystal
        {
            get { return mp.rom[16+0x19BFF+13]; }
            set { mp.rom_modify(16+0x19BFF+13,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("Crowns")]
        [Category("Items Extra")]
        [Description("19BFF+14")]
        public int Crowns
        {
            get { return mp.rom[16+0x19BFF+14]; }
            set { mp.rom_modify(16+0x19BFF+14,(byte)value); mp.refresh_misc(); }
        }

        [DisplayName("DragonSlayer")]
        [Category("Items Extra")]
        [Description("19BFF+15")]
        public int DragonSlayer
        {
            get { return mp.rom[16+0x19BFF+15]; }
            set { mp.rom_modify(16+0x19BFF+15,(byte)value); mp.refresh_misc(); }
        }

        // Family Stats
        // Stats 0 Xemn, Stats 1 Meyna, Stats 2 Roas, Stats 3 Lyll, Stats 4 Pochi

        [DisplayName("Xemn Jump")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+0")]
        public int XemnJump
        {
            get { return mp.rom[family_offset+0]; }
            set { mp.rom_modify(family_offset+0,(byte)value); }
        }

        [DisplayName("Xemn Strength")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+1")]
        public int XemnStrength
        {
            get { return mp.rom[family_offset+1]; }
            set { mp.rom_modify(family_offset+1,(byte)value); }
        }

        [DisplayName("Xemn Shots")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+2")]
        public int XemnShots
        {
            get { return mp.rom[family_offset+2]; }
            set { mp.rom_modify(family_offset+2,(byte)value); }
        }

        [DisplayName("Xemn Range")]
        [Category("Stats 0 Xemn")]
        [Description("1FFA7/1FFB6+3")]
        public int XemnRange
        {
            get { return mp.rom[family_offset+3]; }
            set { mp.rom_modify(family_offset+3,(byte)value); }
        }

        [DisplayName("Meyna Jump")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+4")]
        public int MeynaJump
        {
            get { return mp.rom[family_offset+4]; }
            set { mp.rom_modify(family_offset+4,(byte)value); }
        }

        [DisplayName("Meyna Strength")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+5")]
        public int MeynaStrength
        {
            get { return mp.rom[family_offset+5]; }
            set { mp.rom_modify(family_offset+5,(byte)value); }
        }

        [DisplayName("Meyna Shots")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+6")]
        public int MeynaShots
        {
            get { return mp.rom[family_offset+6]; }
            set { mp.rom_modify(family_offset+6,(byte)value); }
        }

        [DisplayName("Meyna Range")]
        [Category("Stats 1 Meyna")]
        [Description("1FFA7/1FFB6+7")]
        public int MeynaRange
        {
            get { return mp.rom[family_offset+7]; }
            set { mp.rom_modify(family_offset+7,(byte)value); }
        }

        [DisplayName("Roas Jump")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+8")]
        public int RoasJump
        {
            get { return mp.rom[family_offset+8]; }
            set { mp.rom_modify(family_offset+8,(byte)value); }
        }

        [DisplayName("Roas Strength")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+9")]
        public int RoasStrength
        {
            get { return mp.rom[family_offset+9]; }
            set { mp.rom_modify(family_offset+9,(byte)value); }
        }

        [DisplayName("Roas Shots")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+10")]
        public int RoasShots
        {
            get { return mp.rom[family_offset+10]; }
            set { mp.rom_modify(family_offset+10,(byte)value); }
        }

        [DisplayName("Roas Range")]
        [Category("Stats 2 Roas")]
        [Description("1FFA7/1FFB6+11")]
        public int RoasRange
        {
            get { return mp.rom[family_offset+11]; }
            set { mp.rom_modify(family_offset+11,(byte)value); }
        }

        [DisplayName("Lyll Jump")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+12")]
        public int LyllJump
        {
            get { return mp.rom[family_offset+12]; }
            set { mp.rom_modify(family_offset+12,(byte)value); }
        }

        [DisplayName("Lyll Strength")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+13")]
        public int LyllStrength
        {
            get { return mp.rom[family_offset+13]; }
            set { mp.rom_modify(family_offset+13,(byte)value); }
        }

        [DisplayName("Lyll Shots")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+14")]
        public int LyllShots
        {
            get { return mp.rom[family_offset+14]; }
            set { mp.rom_modify(family_offset+14,(byte)value); }
        }

        [DisplayName("Lyll Range")]
        [Category("Stats 3 Lyll")]
        [Description("1FFA7/1FFB6+15")]
        public int LyllRange
        {
            get { return mp.rom[family_offset+15]; }
            set { mp.rom_modify(family_offset+15,(byte)value); }
        }

        [DisplayName("Pochi Jump")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+16")]

        public int PochiJump
        {
            get { return mp.rom[family_offset+16]; }
            set { mp.rom_modify(family_offset+16,(byte)value); }
        }

        [DisplayName("Pochi Strength")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+17")]
        public int PochiStrength
        {
            get { return mp.rom[family_offset+17]; }
            set { mp.rom_modify(family_offset+17,(byte)value); }
        }

        [DisplayName("Pochi Shots")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+18")]
        public int PochiShots
        {
            get { return mp.rom[family_offset+18]; }
            set { mp.rom_modify(family_offset+18,(byte)value); }
        }

        [DisplayName("Pochi Range")]
        [Category("Stats 4 Pochi")]
        [Description("1FFA7/1FFB6+19")]
        public int PochiRange
        {
            get { return mp.rom[family_offset+19]; }
            set { mp.rom_modify(family_offset+19,(byte)value); }
        }
    }
}
