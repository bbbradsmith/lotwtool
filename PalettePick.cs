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
        int zoom = 16;
        public int picked = -1;
        const uint BLACK = 0xFF000000;
        const uint SELECT = 0xFFFFFFFF;

        public PalettePick(int select)
        {
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;

            Bitmap bmp = new Bitmap(16*zoom, 4*zoom, PixelFormat.Format32bppArgb);
            BitmapData d = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            for (int y=0; y<4; ++y)
            {
                for (int x=0; x<16; ++x)
                {
                    int p = x+(y*16);
                    uint c = Main.NES_PALETTE[p];
                    Main.draw_box(d,x*zoom,y*zoom,zoom,zoom,c);
                    if (p == select)
                    {
                        Main.draw_outbox(d,x*zoom,y*zoom,zoom,zoom,SELECT);
                        Main.draw_outbox(d,x*zoom+1,y*zoom+1,zoom-2,zoom-2,BLACK);
                    }
                }
            }
            bmp.UnlockBits(d);
            pictureBox.Image = bmp;
            if (select >0 && select < 64)
                toolStripStatusLabel.Text = string.Format("{0:X2}",select);
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
            int p = px + (py*zoom);
            if (px >= 0 && px < 16 && py >= 0 && py < 4)
            {
                picked = p;
                Close();
                DialogResult = DialogResult.OK;
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int px = e.X / zoom;
            int py = e.Y / zoom;
            int p = px + (py*zoom);
            toolStripStatusLabel.Text = string.Format("{0:X2}",p);
        }
    }
}
