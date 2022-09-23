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
    public partial class Credits : Form
    {
        Main mp;
        Bitmap bmp;
        uint[] chr_cache = null;
        readonly int[] CHR = { 0x21, 0x21, 0x30, 0x31 };
        public string result;

        void cache()
        {
            chr_cache = new uint[256*64];
            for (int i=0; i<4; ++i)
            {
                for (int j=0; j<64; ++j)
                {
                    mp.chr_cache((CHR[i]*64)+j,(i*64)+j,chr_cache,Main.GREY);
                }
            }
        }

        void redraw()
        {
            int lines = 1;
            foreach (char c in textBox.Text)
                if (c == '\r') ++lines;

            int zoom = 1;
            int w = 256 * zoom;
            int h = 16 * lines * zoom;
            if (bmp == null || bmp.Width != w || bmp.Height != h)
            {
                bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            }
            BitmapData d = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Main.draw_box(d,0,0,w,h,Main.GREY[0]);

            int x = 0;
            int y = 0;
            foreach (char c in textBox.Text)
            {
                if (c == '\n') continue;
                if (c == '\r')
                {
                    x = 0;
                    y += 1;
                    continue;
                }

                // tile is ASCII character code with the high nibble shifted left one bit
                // (each character is 2 tiles tall, in interleaved rows of 16 tiles)
                int tile = ((c<<1) & 0xE0) | (c & 0x0F);
                if (x < 32)
                {
                    Main.chr_blit(d,chr_cache,tile+ 0,x*8,((y*2)+0)*8,zoom);
                    Main.chr_blit(d,chr_cache,tile+16,x*8,((y*2)+1)*8,zoom);
                }
                x += 1;
            }

            bmp.UnlockBits(d);
            pictureBox.Image = bmp;
        }

        public Credits(Main parent, string start_text)
        {
            mp = parent;
            InitializeComponent();
            this.Icon = lotwtool.Properties.Resources.Icon;
            cache();
            textBox.Text = start_text;
            //redraw(); // already done by assigning textBot.Text
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            result = textBox.Text;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            redraw();
        }
    }
}
