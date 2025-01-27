using Gma.System.MouseKeyHook;
using Quasar.Common.Enums;
using Quasar.Common.Helpers;
using Quasar.Common.Messages;
using Quasar.Server.Helper;
using Quasar.Server.Messages;
using Quasar.Server.Networking;
using Quasar.Server.Properties;
using Quasar.Server.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Win32;

namespace Quasar.Server.Forms
{
    public partial class FrmRemoteDesktop : Form
    {
        /// <summary>
        /// States whether remote mouse input is enabled.
        /// </summary>
        private bool _enableMouseInput;

        /// <summary>
        /// States whether remote keyboard input is enabled.
        /// </summary>
        private bool _enableKeyboardInput;

        /// <summary>
        /// Holds the state of the local keyboard hooks.
        /// </summary>
        private IKeyboardMouseEvents _keyboardHook;

        /// <summary>
        /// Holds the state of the local mouse hooks.
        /// </summary>
        private IKeyboardMouseEvents _mouseHook;

        /// <summary>
        /// A list of pressed keys for synchronization between key down & -up events.
        /// </summary>
        private readonly List<Keys> _keysPressed;

        /// <summary>
        /// The client which can be used for the remote desktop.
        /// </summary>
        private readonly Client _connectClient;

        /// <summary>
        /// The client's remote desktop mode(normal/kernel)
        /// </summary>
        private readonly RemoteDesktopMode _remoteDesktopMode;       

        /// <summary>
        /// The message handler for handling the communication with the client.
        /// </summary>
        private readonly RemoteDesktopHandler _remoteDesktopHandler;

        private static Process audioListenProcess = null;

        /// <summary>
        /// Holds the opened remote desktop form for each client.
        /// </summary>
        private static readonly Dictionary<Client, FrmRemoteDesktop> OpenedForms = new Dictionary<Client, FrmRemoteDesktop>();


        /// <summary>
        /// variables for customized context menu
        /// </summary>
        private const uint TPM_LEFTALIGN = 0x0000;
        private const uint TPM_RETURNCMD = 0x0100;
        private const uint TPM_RIGHTBUTTON = 0x0002;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int WM_NCRBUTTONDOWN = 0x00A4;
        private const int HTCAPTION = 0x02;
        private bool isMenuCustomized = false;

        private const int USER_CMD_ONVUE = 1001;
        private const int USER_CMD_RESPONDUS = 1002;
        private const int USER_CMD_ETS = 1003;
        private const int USER_CMD_GMAT = 1004;
        private const int USER_CMD_PROPROCTOR = 1005;
        private const int USER_CMD_BLUEBOOK_SAT = 1006;
        private const int USER_CMD_UNKNOWN7 = 1007;
        private const int USER_CMD_UNKNOWN8 = 1008;
        private const int USER_CMD_UNKNOWN9 = 1009;
        private const int USER_CMD_UNKNOWN10 = 1010;
        private const int USER_CMD_UNKNOWN11 = 1011;
        private const int USER_CMD_UNKNOWN12 = 1012;
        private const int USER_CMD_UNKNOWN13 = 1013;
        private const int USER_CMD_UNKNOWN14 = 1014;
        private const int USER_CMD_UNKNOWN15 = 1015;
        private const int USER_CMD_PATCH_ETS = 1016;
        private const int USER_CMD_RESTORE_ETS = 1017;
        private const int USER_CMD_INSTALL_KBM_DRIVER = 1018;
        private const int USER_CMD_UNINSTALL_KBM_DRIVER = 1019;
        private const int USER_CMD_START_AUDIO = 1020;
        private const int USER_CMD_STOP_AUDIO = 1021;

        private const int USER_CMD_ETS_CHILD_FOR_TEST = 1031;

        private static long tempTime = 0;

        /// <summary>
        /// Creates a new remote desktop form for the client or gets the current open form, if there exists one already.
        /// </summary>
        /// <param name="client">The client used for the remote desktop form.</param>
        /// <returns>
        /// Returns a new remote desktop form for the client if there is none currently open, otherwise creates a new one.
        /// </returns>
        public static FrmRemoteDesktop CreateNewOrGetExisting(Client client, RemoteDesktopMode mode)
        {
            if (OpenedForms.ContainsKey(client))
            {
                return OpenedForms[client];
            }
            FrmRemoteDesktop r = new FrmRemoteDesktop(client, mode);
            r.Disposed += (sender, args) => OpenedForms.Remove(client);
            OpenedForms.Add(client, r);
            return r;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmRemoteDesktop"/> class using the given client.
        /// </summary>
        /// <param name="client">The client used for the remote desktop form.</param>
        public FrmRemoteDesktop(Client client, RemoteDesktopMode mode)
        {
            _connectClient = client;
            _remoteDesktopMode = mode;
            _remoteDesktopHandler = new RemoteDesktopHandler(client);
            _keysPressed = new List<Keys>();

            RegisterMessageHandler();
            InitializeComponent();
        }

        // Override the WndProc method to handle right-click on the title bar
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCRBUTTONDOWN && m.WParam.ToInt32() == HTCAPTION)
            {
                // Get the mouse position
                int x = Cursor.Position.X;
                int y = Cursor.Position.Y;

                // Get the default system menu
                IntPtr systemMenu = GetSystemMenu(this.Handle, false);

                if (systemMenu != IntPtr.Zero && !isMenuCustomized)
                {
                    // Add a custom separator and menu items to the system menu
                    if (_remoteDesktopMode == RemoteDesktopMode.Kernel)
                    {
                        AppendCustomItems(systemMenu);
                        isMenuCustomized = true;
                    }
                }
                // Show the system menu at the mouse position
                int command = TrackPopupMenu(systemMenu, TPM_LEFTALIGN | TPM_RETURNCMD | TPM_RIGHTBUTTON, x, y, 0, this.Handle, IntPtr.Zero);

                // If a command is selected, process it
                if (command != 0)
                {
                    SendMessage(this.Handle, WM_SYSCOMMAND, new IntPtr(command), IntPtr.Zero);
                }

                return; // Prevent default behavior
            }

            base.WndProc(ref m);
        }

        private void AppendCustomItems(IntPtr systemMenu)
        {
            // Add a separator
            AppendMenu(systemMenu, 0x800, 0, string.Empty);

            // Add custom menu items
            AppendMenu(systemMenu, 0x0, USER_CMD_ONVUE, "OnVue");
            AppendMenu(systemMenu, 0x0, USER_CMD_RESPONDUS, "Respondus");
            AppendMenu(systemMenu, 0x0, USER_CMD_ETS, "ETS");
            AppendMenu(systemMenu, 0x0, USER_CMD_ETS_CHILD_FOR_TEST, "ETS child for dev");
            AppendMenu(systemMenu, 0x0, USER_CMD_GMAT, "GMAT");
            AppendMenu(systemMenu, 0x0, USER_CMD_PROPROCTOR, "ProProctor");
            AppendMenu(systemMenu, 0x0, USER_CMD_BLUEBOOK_SAT, "Bluebook (SAT)");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN7, "Unknown7");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN8, "Unknown8");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN9, "Unknown9");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN10, "Unknown10");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN11, "Unknown11");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN12, "Unknown12");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN13, "Unknown13");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN14, "Unknown14");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNKNOWN15, "Unknown15");

            AppendMenu(systemMenu, 0x800, 0, string.Empty);
            
            AppendMenu(systemMenu, 0x0, USER_CMD_PATCH_ETS, "Patch ETS");
            AppendMenu(systemMenu, 0x0, USER_CMD_RESTORE_ETS, "Restore ETS");

            AppendMenu(systemMenu, 0x800, 0, string.Empty);

            AppendMenu(systemMenu, 0x0, USER_CMD_INSTALL_KBM_DRIVER, "Install KB/M driver");
            AppendMenu(systemMenu, 0x0, USER_CMD_UNINSTALL_KBM_DRIVER, "Uninstall KB/M driver");

            AppendMenu(systemMenu, 0x800, 0, string.Empty);

            AppendMenu(systemMenu, 0x0, USER_CMD_START_AUDIO, "Start audio");
            AppendMenu(systemMenu, 0x0, USER_CMD_STOP_AUDIO, "Stop audio");
        }

        // Handle the custom menu commands
        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                if (_connectClient != null)
                {
                    switch ((int)m.WParam)
                    {
                        case USER_CMD_ONVUE:
                            _connectClient.Send(new DoUnblockScreenCapture { browserType = BrowserTypeToUnblock.ONVUE });
                            return;
                        case USER_CMD_RESPONDUS:
                            _connectClient.Send(new DoUnblockScreenCapture { browserType = BrowserTypeToUnblock.RESPONDUS });
                            return;
                        case USER_CMD_ETS:
                            _connectClient.Send(new DoUnblockScreenCapture { browserType = BrowserTypeToUnblock.ETS, args="unblock" });
                            return;
                        case USER_CMD_PATCH_ETS:
                            _connectClient.Send(new DoUnblockScreenCapture { browserType = BrowserTypeToUnblock.ETS, args = "patch" });
                            return;
                        case USER_CMD_RESTORE_ETS:
                            _connectClient.Send(new DoUnblockScreenCapture { browserType = BrowserTypeToUnblock.ETS, args = "restore" });
                            return;
                        case USER_CMD_ETS_CHILD_FOR_TEST:
                            _connectClient.Send(new DoUnblockScreenCapture { browserType = BrowserTypeToUnblock.ETS, args = "unblock_child" });
                            return;
                        case USER_CMD_INSTALL_KBM_DRIVER:
                            _connectClient.Send(new KBMDriverCommand
                            {
                                action = "install"
                            });
                            return;
                        case USER_CMD_UNINSTALL_KBM_DRIVER:
                            _connectClient.Send(new KBMDriverCommand
                            {
                                action = "uninstall"
                            });
                            return;
                        case USER_CMD_START_AUDIO:

                            AudioStartItemClicked();                          
                            break;
                        case USER_CMD_STOP_AUDIO:
                            _connectClient.Send(new DoAudioTransfer
                            {
                                Action = "stop"
                            });
                            return;

                    }
                }
            }
            base.DefWndProc(ref m);
        }

        private void StartAudioListener(int port, int sampleRate, int bitDepth, int channels)
        {
            string text = Path.Combine(new string[]
            {
                Path.GetTempPath(),
                Guid.NewGuid().ToString() + ".exe"
            });
            try
            {
                File.WriteAllBytes(text, Resources.AudioListener);
            }
            catch (Exception ex)
            {
                MessageBox.Show("AudioListener writing error: ", ex.Message);
            }
            string stdout, stderr;
            CheckOutput("\"" + text + "\"", string.Format("{0} {1} {2} {3}", port, sampleRate, bitDepth, channels));
        }

        private void CheckOutput(string fileName, string cmd)
        {
            audioListenProcess = new Process();
            audioListenProcess.StartInfo.UseShellExecute = false;
            //audioListenProcess.StartInfo.RedirectStandardOutput = true;
            //audioListenProcess.StartInfo.RedirectStandardError = true;
            audioListenProcess.StartInfo.CreateNoWindow = false;
            audioListenProcess.StartInfo.FileName = fileName;
            audioListenProcess.StartInfo.Arguments = cmd;
            audioListenProcess.Start();

            //stdout = audioListenProcess.StandardOutput.ReadToEnd();
            //stderr = audioListenProcess.StandardError.ReadToEnd();
            //audioListenProcess.WaitForExit();
            
        }

        private static bool IsPortListening(int port)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var tcpConnections = properties.GetActiveTcpListeners();

            return tcpConnections.Any(endpoint => endpoint.Port == port);
        }

        private void AudioStartItemClicked()
        {
            FrmAudioSetting audioSetting = new FrmAudioSetting();
            if (audioSetting.ShowDialog(this) == DialogResult.OK)
            {
                int sampleRate = audioSetting.SampleRate;
                int bitDepth = audioSetting.BitDepth;
                int channels = audioSetting.Channels;

                _connectClient.Send(new DoAudioTransfer
                {
                    Action = "start",
                    BitDepth = bitDepth,
                    Channels = channels,
                    SampleRate = sampleRate,
                    Port = 10000
                });
                if(!IsPortListening(10000))
                {
                    StartAudioListener(10000, sampleRate, bitDepth, channels);
                }
            }
        }

        /// <summary>
        /// Called whenever a client disconnects.
        /// </summary>
        /// <param name="client">The client which disconnected.</param>
        /// <param name="connected">True if the client connected, false if disconnected</param>
        private void ClientDisconnected(Client client, bool connected)
        {
            if (!connected)
            {
                this.Invoke((MethodInvoker)this.Close);
            }
        }

        /// <summary>
        /// Registers the remote desktop message handler for client communication.
        /// </summary>
        private void RegisterMessageHandler()
        {
            _connectClient.ClientState += ClientDisconnected;
            _remoteDesktopHandler.DisplaysChanged += DisplaysChanged;
            _remoteDesktopHandler.ProgressChanged += UpdateImage;
            _remoteDesktopHandler.ClientDriverStateChanged += KBMDriverStateChanged;
            MessageHandler.Register(_remoteDesktopHandler);

            //MessageHandler.Register(_clientDebugMessageHandler);
        }

        /// <summary>
        /// Unregisters the remote desktop message handler.
        /// </summary>
        private void UnregisterMessageHandler()
        {
            //MessageHandler.Unregister(_clientDebugMessageHandler);
            MessageHandler.Unregister(_remoteDesktopHandler);
            _remoteDesktopHandler.DisplaysChanged -= DisplaysChanged;
            _remoteDesktopHandler.ProgressChanged -= UpdateImage;
            _remoteDesktopHandler.ClientDriverStateChanged -= KBMDriverStateChanged;
            _connectClient.ClientState -= ClientDisconnected;
        }

        /// <summary>
        /// Subscribes to local mouse and keyboard events for remote desktop input.
        /// </summary>
        private void SubscribeEvents()
        {
            // TODO: Check Hook.GlobalEvents vs Hook.AppEvents below
            // TODO: Maybe replace library with .NET events like on Linux
            if (PlatformHelper.RunningOnMono) // Mono/Linux
            {
                this.KeyDown += OnKeyDown;
                this.KeyUp += OnKeyUp;
            }
            else // Windows
            {
                _keyboardHook = Hook.GlobalEvents();
                _keyboardHook.KeyDown += OnKeyDown;
                _keyboardHook.KeyUp += OnKeyUp;

                _mouseHook = Hook.AppEvents();
                _mouseHook.MouseWheel += OnMouseWheelMove;
            }
        }

        /// <summary>
        /// Unsubscribes from local mouse and keyboard events.
        /// </summary>
        private void UnsubscribeEvents()
        {
            if (PlatformHelper.RunningOnMono) // Mono/Linux
            {
                this.KeyDown -= OnKeyDown;
                this.KeyUp -= OnKeyUp;
            }
            else // Windows
            {
                if (_keyboardHook != null)
                {
                    _keyboardHook.KeyDown -= OnKeyDown;
                    _keyboardHook.KeyUp -= OnKeyUp;
                    _keyboardHook.Dispose();
                }
                if (_mouseHook != null)
                {
                    _mouseHook.MouseWheel -= OnMouseWheelMove;
                    _mouseHook.Dispose();
                }
            }
        }

        /// <summary>
        /// Starts the remote desktop stream and begin to receive desktop frames.
        /// </summary>
        private void StartStream()
        {
            ToggleConfigurationControls(true);

            picDesktop.Start();
            // Subscribe to the new frame counter.
            picDesktop.SetFrameUpdatedEvent(frameCounter_FrameUpdated);

            this.ActiveControl = picDesktop;

            _remoteDesktopHandler.BeginReceiveFrames(barQuality.Value, cbMonitors.SelectedIndex);
        }

        /// <summary>
        /// Stops the remote desktop stream.
        /// </summary>
        private void StopStream()
        {
            ToggleConfigurationControls(false);

            picDesktop.Stop();
            // Unsubscribe from the frame counter. It will be re-created when starting again.
            picDesktop.UnsetFrameUpdatedEvent(frameCounter_FrameUpdated);

            this.ActiveControl = picDesktop;

            _remoteDesktopHandler.EndReceiveFrames();
        }

        /// <summary>
        /// Toggles the activatability of configuration controls in the status/configuration panel.
        /// </summary>
        /// <param name="started">When set to <code>true</code> the configuration controls get enabled, otherwise they get disabled.</param>
        private void ToggleConfigurationControls(bool started)
        {
            btnStart.Enabled = !started;
            btnStop.Enabled = started;
            barQuality.Enabled = !started;
            cbMonitors.Enabled = !started;
        }

        /// <summary>
        /// Toggles the visibility of the status/configuration panel.
        /// </summary>
        /// <param name="visible">Decides if the panel should be visible.</param>
        private void TogglePanelVisibility(bool visible)
        {
            panelTop.Visible = visible;
            btnShow.Visible = !visible;
            this.ActiveControl = picDesktop;
        }


        private void KBMDriverStateChanged(object sender, int state)
        {
            if (state == 15)
            {
                SubscribeEvents();
                StartStream();
            }
            else if(state == 7)
            {
                MessageBox.Show("KB/M driver is not installed or reboot isn't done!");
            }
        }

        /// <summary>
        /// Called whenever the remote displays changed.
        /// </summary>
        /// <param name="sender">The message handler which raised the event.</param>
        /// <param name="displays">The currently available displays.</param>
        private void DisplaysChanged(object sender, int displays)
        {
            cbMonitors.Items.Clear();
            for (int i = 0; i < displays; i++)
                cbMonitors.Items.Add($"Display {i + 1}");
            cbMonitors.SelectedIndex = 0;
        }

        /// <summary>
        /// Updates the current desktop image by drawing it to the desktop picturebox.
        /// </summary>
        /// <param name="sender">The message handler which raised the event.</param>
        /// <param name="bmp">The new desktop image to draw.</param>
        private void UpdateImage(object sender, Bitmap bmp)
        {
            picDesktop.UpdateImage(bmp, false);
        }

        private void FrmRemoteDesktop_Load(object sender, EventArgs e)
        {
            this.Text = WindowHelper.GetWindowTitle("Remote Desktop", _connectClient);

            OnResize(EventArgs.Empty); // trigger resize event to align controls 

            _remoteDesktopHandler.RefreshDisplays();
        }

        /// <summary>
        /// Updates the title with the current frames per second.
        /// </summary>
        /// <param name="e">The new frames per second.</param>
        private void frameCounter_FrameUpdated(FrameUpdatedEventArgs e)
        {
            this.Text = string.Format("{0} - FPS: {1}", WindowHelper.GetWindowTitle("Remote Desktop", _connectClient), e.CurrentFramesPerSecond.ToString("0.00"));
        }

        private void FrmRemoteDesktop_FormClosing(object sender, FormClosingEventArgs e)
        {
            // all cleanup logic goes here
            UnsubscribeEvents();
            if (_remoteDesktopHandler.IsStarted) StopStream();
            UnregisterMessageHandler();
            _remoteDesktopHandler.Dispose();
            picDesktop.Image?.Dispose();
        }

        private void FrmRemoteDesktop_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                return;

            _remoteDesktopHandler.LocalResolution = picDesktop.Size;
            panelTop.Left = (this.Width - panelTop.Width) / 2;
            btnShow.Left = (this.Width - btnShow.Width) / 2;
            btnHide.Left = (panelTop.Width - btnHide.Width) / 2;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_remoteDesktopMode == RemoteDesktopMode.Kernel)
            {
                _connectClient.Send(new KBMDriverCommand
                {
                    action = "driverinit"
                });
            }
            else if (_remoteDesktopMode == RemoteDesktopMode.Normal)
            {
                SubscribeEvents();
                StartStream();
            }

            if (cbMonitors.Items.Count == 0)
            {
                MessageBox.Show("No remote display detected.\nPlease wait till the client sends a list with available displays.",
                    "Starting failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            UnsubscribeEvents();
            StopStream();

        }

        #region Remote Desktop Input

        private void LocalSendMouseEvent(MouseAction mouseAction, bool isMouseDown, int x, int y, int displayIndex)
        {
            if(_remoteDesktopMode == RemoteDesktopMode.Normal)
            {
                _remoteDesktopHandler.SendMouseEvent(mouseAction, isMouseDown, x, y, displayIndex, RemoteDesktopMode.Normal);
            }
            else
                _remoteDesktopHandler.SendMouseEvent(mouseAction, isMouseDown, x, y, displayIndex, RemoteDesktopMode.Kernel);

        }

        private void picDesktop_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                MouseAction action = MouseAction.None;

                if (e.Button == MouseButtons.Left)
                    action = MouseAction.LeftDown;
                if (e.Button == MouseButtons.Right)
                    action = MouseAction.RightDown;

                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                LocalSendMouseEvent(action, true, e.X, e.Y, selectedDisplayIndex);
            }
        }

        private void picDesktop_MouseUp(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                MouseAction action = MouseAction.None;

                if (e.Button == MouseButtons.Left)
                    action = MouseAction.LeftUp;
                if (e.Button == MouseButtons.Right)
                    action = MouseAction.RightUp;

                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                LocalSendMouseEvent(action, false, e.X, e.Y, selectedDisplayIndex);
            }
        }

        private void picDesktop_MouseMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                int selectedDisplayIndex = cbMonitors.SelectedIndex;

                LocalSendMouseEvent(MouseAction.MoveCursor, false, e.X, e.Y, selectedDisplayIndex);
            }
        }

        private void OnMouseWheelMove(object sender, MouseEventArgs e)
        {
            if (picDesktop.Image != null && _enableMouseInput && this.ContainsFocus)
            {
                LocalSendMouseEvent(e.Delta == 120 ? MouseAction.ScrollUp : MouseAction.ScrollDown,
                    false, 0, 0, cbMonitors.SelectedIndex);
            }
        }

        private void LocalSendKeyboardEvent(Keys keyCode, bool isDown)
        {
            if(this._remoteDesktopMode== RemoteDesktopMode.Normal)
            {
                _remoteDesktopHandler.SendKeyboardEvent((byte)keyCode, isDown, RemoteDesktopMode.Normal);
            }
            else
                _remoteDesktopHandler.SendKeyboardEvent((byte)keyCode, isDown, RemoteDesktopMode.Kernel);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (picDesktop.Image != null && _enableKeyboardInput && this.ContainsFocus)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;
                long curTime = (long)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                long elapsedTime = curTime - tempTime;
                
                if (_keysPressed.Contains(e.KeyCode))
                {
                    if (elapsedTime > 100 && !(e.Shift || e.Control || e.Alt))
                    {
                        LocalSendKeyboardEvent(e.KeyCode, true);
                        tempTime = curTime;
                        return;
                    }
                    return;
                }
                _keysPressed.Add(e.KeyCode);
                LocalSendKeyboardEvent(e.KeyCode, true);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (picDesktop.Image != null && _enableKeyboardInput && this.ContainsFocus)
            {
                if (!IsLockKey(e.KeyCode))
                    e.Handled = true;

               _keysPressed.Remove(e.KeyCode);

                LocalSendKeyboardEvent(e.KeyCode, false);
            }
        }

        private bool IsLockKey(Keys key)
        {
            return ((key & Keys.CapsLock) == Keys.CapsLock)
                   || ((key & Keys.NumLock) == Keys.NumLock)
                   || ((key & Keys.Scroll) == Keys.Scroll);
        }

        #endregion

        #region Remote Desktop Configuration

        private void barQuality_Scroll(object sender, EventArgs e)
        {
            int value = barQuality.Value;
            lblQualityShow.Text = value.ToString();

            if (value < 25)
                lblQualityShow.Text += " (low)";
            else if (value >= 85)
                lblQualityShow.Text += " (best)";
            else if (value >= 75)
                lblQualityShow.Text += " (high)";
            else if (value >= 25)
                lblQualityShow.Text += " (mid)";

            this.ActiveControl = picDesktop;
        }

        private void btnMouse_Click(object sender, EventArgs e)
        {
            if (_enableMouseInput)
            {
                this.picDesktop.Cursor = Cursors.Default;
                btnMouse.Image = Properties.Resources.mouse_delete;
                toolTipButtons.SetToolTip(btnMouse, "Enable mouse input.");
                _enableMouseInput = false;
            }
            else
            {
                this.picDesktop.Cursor = Cursors.Hand;
                btnMouse.Image = Properties.Resources.mouse_add;
                toolTipButtons.SetToolTip(btnMouse, "Disable mouse input.");
                _enableMouseInput = true;
            }

            this.ActiveControl = picDesktop;
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            if (_enableKeyboardInput)
            {
                this.picDesktop.Cursor = Cursors.Default;
                btnKeyboard.Image = Properties.Resources.keyboard_delete;
                toolTipButtons.SetToolTip(btnKeyboard, "Enable keyboard input.");
                _enableKeyboardInput = false;
            }
            else
            {
                this.picDesktop.Cursor = Cursors.Hand;
                btnKeyboard.Image = Properties.Resources.keyboard_add;
                toolTipButtons.SetToolTip(btnKeyboard, "Disable keyboard input.");
                _enableKeyboardInput = true;
            }

            this.ActiveControl = picDesktop;
        }

        #endregion

        private void btnHide_Click(object sender, EventArgs e)
        {
            TogglePanelVisibility(false);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            TogglePanelVisibility(true);
        }

        private void toolTipButtons_Popup(object sender, PopupEventArgs e)
        {

        }

        // Import the necessary Windows API functions
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        // Import additional Windows API functions
        [DllImport("user32.dll")]
        private static extern bool AppendMenu(IntPtr hMenu, uint uFlags, uint uIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void btnCloseIT_Click(object sender, EventArgs e)
        {
            if (_connectClient.Connected)
            {
                _connectClient.Send(new DoShowRedText
                {
                    Action = "close"
                });
            }
        }

        private void btnSetIT_Click(object sender, EventArgs e)
        {
            try
            {
                if (_connectClient.Connected)
                {
                    FrmIVText ivTextDlg = new FrmIVText();

                    DialogResult result = ivTextDlg.ShowDialog();
                    if(result == DialogResult.OK)
                    {
                        _connectClient.Send(new DoShowRedText
                        {
                            Action = "show",
                            Text = ivTextDlg.DialogText,
                            FontSize = ivTextDlg.FontSize
                        });
                    }                    
                }
                else
                {
                    MessageBox.Show("This client is not connected to a server.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnKernel_Click(object sender, EventArgs e)
        {
            if (_connectClient.Connected)
            {
                _connectClient.Send(new DoRTAction { Action = "install" });
            }
        }

        private void btnUnkernel_Click(object sender, EventArgs e)
        {

            if (_connectClient.Connected)
            {
                _connectClient.Send(new DoRTAction { Action = "uninstall" });
            }
        }

        private void btnAutoTypeStart_Click(object sender, EventArgs e)
        {
            FrmAutoTypeSetting dlgAuto = new FrmAutoTypeSetting();

            DialogResult result = dlgAuto.ShowDialog();

            if (result == DialogResult.OK)
            {
                //MessageBox.Show(dlgAuto.AutotypeContent);
                if (_connectClient != null)
                {
                    _connectClient.Send(new DoAutoType
                    {
                        Action = "start",
                        Text = dlgAuto.AutotypeContent,
                        MinDelay = dlgAuto.MinDelayValue,
                        MaxDelay = dlgAuto.MaxDelayValue
                    });
                }
            }
        }

        private void btnAutoTypeStop_Click(object sender, EventArgs e)
        {
            if (_connectClient != null)
            {
                _connectClient.Send(new DoAutoType
                {
                    Action = "stop"
                });
            }
        }

    }
}
