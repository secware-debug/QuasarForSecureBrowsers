using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Quasar.Client.Interception
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Boolean Predicate(Int32 device);

    public static class InterceptionDriver
    {
        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_create_context")]
        public static extern IntPtr CreateContext();

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_destroy_context")]
        public static extern void DestroyContext(IntPtr context);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_get_precedence")]
        public static extern Int32 GetPrecedence(IntPtr context, Int32 device);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_set_precedence")]
        public static extern void SetPrecedence(IntPtr context, Int32 device, Int32 Precedence);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_get_filter")]
        public static extern UInt16 GetFilter(IntPtr context, Int32 device);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_set_filter")]
        public static extern void SetFilter(IntPtr context, Predicate predicate, UInt16 keyboardFilterMode);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_wait")]
        public static extern Int32 Wait(IntPtr context);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_wait_with_timeout")]
        public static extern Int32 WaitWithTimeout(IntPtr context, UInt64 milliseconds);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_send")]
        public static extern Int32 Send(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_receive")]
        public static extern Int32 Receive(IntPtr context, Int32 device, ref Stroke stroke, UInt32 numStrokes);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_get_hardware_id")]
        public static extern UInt32 GetHardwareId(IntPtr context, Int32 device, IntPtr hardwareIdentifier, UInt32 sizeOfString);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_is_invalid")]
        public static extern Int32 IsInvalid(Int32 device);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_is_keyboard")]
        public static extern Int32 IsKeyboard(Int32 device);

        [DllImport("int3rc3ption.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "interception_is_mouse")]
        public static extern Int32 IsMouse(Int32 device);
    }

}
