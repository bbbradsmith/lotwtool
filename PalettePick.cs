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
    public partial class PalettePick : Form
    {
        int zoom = 12;
        public int picked = -1;

        public PalettePick(int select)
        {
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;

            Bitmap bmp = new Bitmap(16*zoom, 32*zoom, PixelFormat.Format32bppArgb);
            BitmapData d = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            for (int y=0; y<32; ++y)
            {
                for (int x=0; x<16; ++x)
                {
                    uint p = (uint)(x+(y*16));
                    uint r = ((p >> 0) & 0x7) * 255 / 7;
                    uint g = ((p >> 3) & 0x7) * 255 / 7;
                    uint b = ((p >> 6) & 0x7) * 255 / 7;
                    uint c = 0xFF000000 | (r << 16) | (g << 8) | b;
                    Main.draw_box(d,x*zoom,y*zoom,zoom,zoom,c);
                    if (p == select)
                    {
                        Main.draw_outbox(d,x*zoom,y*zoom,zoom,zoom,Main.PAL_OUTER);
                        Main.draw_outbox(d,x*zoom+1,y*zoom+1,zoom-2,zoom-2,Main.PAL_INNER);
                    }
                }
            }
            bmp.UnlockBits(d);
            pictureBox.Image = bmp;

            if (select > 0 && select < 512)
                toolStripStatusLabel.Text = string.Format("{0:X1}",select);
        }

        private void PalettePick_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape ||
                e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Return)
                Close();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            int px = e.X / zoom;
            int py = e.Y / zoom;
            int p = px + (py*16);
            if (px >= 0 && px < 16 && py >= 0 && py < 32)
            {
                picked = -1;
                if (p>=0) picked = p;
                Close();
                DialogResult = DialogResult.OK;
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int px = e.X / zoom;
            int py = e.Y / zoom;
            int p = px + (py*16);
            toolStripStatusLabel.Text = string.Format("{0:X1}",p);
        }
    }
}
