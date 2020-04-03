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
    public partial class MapEditHex : Form
    {
        MapEdit me;
        Main mp;

        public void redraw()
        {
            int ro = 16 + (1024 * me.room);
            string s = "";
            for (int l=0; l<16; ++l)
            {
                int o = 0x300 + (l * 16);
                s += string.Format("{0:X3}: ",o) + mp.romhex(ro+o,16);
                if (l != 15) s += "\r\n";
            }
            textBox.Text = s;
        }

        public MapEditHex(MapEdit me_, Main parent)
        {
            mp = parent;
            me = me_;
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;
            int x = me.room % 4;
            int y = me.room / 4;
            Text = string.Format("Info Hex {0},{1} ({2})",x,y,me.room);
            redraw();
        }

        private void MapEditHex_FormClosing(object sender, FormClosingEventArgs e)
        {
            me.infohex = null;
        }
    }
}
