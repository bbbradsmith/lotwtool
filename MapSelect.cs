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
    public partial class MapSelect : Form, RomRefresh
    {
        int zoom = -8;
        static int default_zoom = -8;
        int secret = 2;
        bool items = false;
        bool collision = false;
        bool auto_refresh = false;
        Main mp;
        Bitmap bmp;

        void redraw()
        {
            int z = (zoom > 0) ? zoom : 1;
            int maph = (mp.map_count + 3) / 4;
            int rw = 256 * 4 * z;
            int rh = 192 * z;
            int w = rw * 4;
            int h = rh * maph;
            bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            MapEdit temp_map = new MapEdit(mp, 0);
            for (int r=0; r<mp.map_count; ++r)
            {
                int x = r & 3;
                int y = r / 4;
                BitmapData d = bmp.LockBits(new Rectangle(x*rw, y*rh, rw, rh), ImageLockMode.WriteOnly, bmp.PixelFormat);
                temp_map.render_select(d, r, z, secret, items, collision);
                bmp.UnlockBits(d);
            }
            if (zoom < 0) // downscale
            {
                Bitmap sbmp = new Bitmap(w/(-zoom),h/(-zoom), bmp.PixelFormat);
                using (Graphics g = Graphics.FromImage(sbmp))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    g.DrawImage(bmp,0,0,sbmp.Width,sbmp.Height);
                }
                bmp = sbmp;
            }
            pictureBox.Image = bmp;
        }

        public void refresh_all() { if (auto_refresh) redraw(); }
        public void refresh_chr(int tile) { if (auto_refresh) redraw(); }
        public void refresh_metatile(int page) { if (auto_refresh) redraw(); }
        public void refresh_map(int map) { if (auto_refresh) redraw(); }
        public void refresh_close() { this.Close(); }

        public MapSelect(Main parent)
        {
            mp = parent;
            zoom = default_zoom;
            InitializeComponent();
            toolStripTipLabel.Text = "Double Click = Open Map";
            updateZoom();
            //redraw(); // drawn by updateZoom
        }

        private void updateZoom()
        {
            default_zoom = zoom;
            zoom1xToolStripMenuItem.Checked = zoom == 1;
            zoom2xToolStripMenuItem.Checked = zoom == 2;
            zoom3xToolStripMenuItem.Checked = zoom == 3;
            zoom4xToolStripMenuItem.Checked = zoom == 4;
            zoomr2xToolStripMenuItem.Checked = zoom == -2;
            zoomr4xToolStripMenuItem.Checked = zoom == -4;
            zoomr8xToolStripMenuItem.Checked = zoom == -8;
            zoomr16xToolStripMenuItem.Checked = zoom == -16;

            int m = (zoom >= 0) ? zoom : 1;
            int d = (zoom <  0) ? (-zoom) : 1;
            
            const int dspan = (1024 * 4 * 1) / 4;
            int span = (1024 * 4 * m) / d;
            int w = (1065-dspan)+span;
            if (w > 1065) w = 1065;

            const int dhspan = 614;
            int hspan = (192 * (72/4) * m) / d;
            int h = (706 - dhspan) + hspan;
            if (h > 706) h = 706;
            else w -= 16; // no vertical scrollbar needed

            Size = new Size(w,h);

            redraw();
        }

        private void zoomX1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 1; updateZoom();
        }

        private void zoomX2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 2; updateZoom();
        }

        private void zoomX3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 3; updateZoom();
        }

        private void zoomX4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = 4; updateZoom();
        }

        private void zoomXr2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = -2; updateZoom();
        }

        private void zoomXr4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = -4; updateZoom();
        }

        private void zoomXr8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = -8; updateZoom();
        }

        private void zoomXr16ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zoom = -16; updateZoom();
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Title = "Save Image";
            d.DefaultExt = "png";
            d.Filter = "PNG Image (*.png)|*.png|All files (*.*)|*.*";
            d.FileName = System.IO.Path.GetFileNameWithoutExtension(mp.filename) + ".png";
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bmp.Save(d.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save image:\n" + d.FileName + "\n\n" + ex.ToString(), "Image save error!");
                }
            }
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int z = (zoom > 0) ? zoom : 1;
            int x = e.X / z;
            int y = e.Y / z;
            if (zoom < 0)
            {
                x *= -zoom;
                y *= -zoom;
            }
            x /= (256 * 4);
            y /= 192;
            toolStripStatusLabel.Text = string.Format("Map {0},{1} ({2})",x,y,(y*4)+x);
        }

        private void pictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int z = (zoom > 0) ? zoom : 1;
            int x = e.X / z;
            int y = e.Y / z;
            if (zoom < 0)
            {
                x *= -zoom;
                y *= -zoom;
            }
            x /= (256 * 4);
            y /= 192;

            int room = (y * 4) + x;
            mp.add_map_edit(room);
        }

        private void MapSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            mp.map_select = null;
            mp.remove_refresh(this);
        }

        private void updateSecret()
        {
            showSecretToolStripMenuItem.Checked = secret == 1;
            halfSecretToolStripMenuItem.Checked = secret == 2;
            redraw();
        }

        private void showSecretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (secret != 1) secret = 1;
            else             secret = 0;
            updateSecret();
        }

        private void halfSecretToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (secret != 2) secret = 2;
            else             secret = 0;
            updateSecret();
        }

        private void showItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            items = !items;
            showItemsToolStripMenuItem.Checked = items;
            redraw();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.undo();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            redraw();
        }

        private void autoRefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            auto_refresh = !auto_refresh;
            autoRefreshToolStripMenuItem.Checked = auto_refresh;
            redraw();
        }

        private void showCollisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            collision = !collision;
            showCollisionToolStripMenuItem.Enabled = collision;
            redraw();
        }

        private void saveAndRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mp.saveAndTestToolStripMenuItem_Click(sender,e);
        }
    }
}
