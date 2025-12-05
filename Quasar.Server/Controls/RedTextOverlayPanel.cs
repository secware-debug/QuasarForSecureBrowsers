using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quasar.Server.Controls
{
    public class RedTextOverlayPanel : Panel
    {
        public Rectangle DrawRect = Rectangle.Empty;
        public bool IsDrawMode = false;

        public RedTextOverlayPanel()
        {
            this.SetStyle(ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.SupportsTransparentBackColor,
                          true);

            this.BackColor = Color.Transparent;

            this.MouseDown += Frm_MouseDown;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Prevent flicker AND ensure parent shows through
            // Do NOT call base.OnPaintBackground()
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            if (IsDrawMode && DrawRect != Rectangle.Empty)
            {
                using (Pen p = new Pen(Color.Red, 2))
                {
                    e.Graphics.DrawRectangle(p, DrawRect);

                    MessageBox.Show("overlay pannel onpaint is fired!");
                }
            }

            base.OnPaint(e);
        }

        private void Frm_MouseDown(object sender, MouseEventArgs e)
        {
            MessageBox.Show("mouse down on overlay pannel");
        }
    }
}
