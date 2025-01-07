using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InputInterceptorNS;

namespace Quasar.Client.Helper
{
    public static class InputIntercepterHelper
    {
        static MouseHook mouseHook;
        static KeyboardHook keyboardHook;
        public static Boolean InstallKMDriver()
        {
            //if (InitializeDriver())
            //{
            //    mouseHook = new MouseHook();
            //    keyboardHook = new KeyboardHook();
            //    return true;
            //}
            //else
            //{
                InstallDriver();
           // }
            return false;
        }

        public static Boolean UninstallKMDriver() { return false; }

        public static Boolean InputSimulateKeyDown(KeyCode code)
        {
            return keyboardHook.SimulateKeyDown(code);
            
        }

        public static Boolean InputSimulateKeyUp(KeyCode code)
        {
            return keyboardHook.SimulateKeyUp(code);
        }

        static Boolean InitializeDriver()
        {
            if (InputInterceptor.CheckDriverInstalled())
            {
                if (InputInterceptor.Initialize())
                {
                    MessageBox.Show("Input interceptor successfully initialized.");
                    return true;
                }
            }
            MessageBox.Show("Input interceptor initialization failed.");
            return false;
        }

        static void InstallDriver()
        {
            if (InputInterceptor.CheckAdministratorRights())
            {
                if (InputInterceptor.InstallDriver())
                {
                    Console.WriteLine("Done! Restart your computer.");
                }
                else
                {
                    Console.WriteLine("Something... gone... wrong... :(");
                }
            }
            else
            {
                Console.WriteLine("Restart program with administrator rights so it will be installed.");
            }
        }
    }
}
