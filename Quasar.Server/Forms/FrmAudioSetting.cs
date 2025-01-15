using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Quasar.Server.Forms
{
    public partial class FrmAudioSetting : Form
    {
        private const string RegistryKeyPath = @"Software\Quasar\AudioSettings";
        public FrmAudioSetting()
        {
            InitializeComponent();
            LoadSettings();

            cmbSampleRate.SelectedIndex = 1;
            cmbBitDepth.SelectedIndex = 1;
            cmbChannels.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadSettings()
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    cmbSampleRate.SelectedIndex = (int)(key.GetValue("SampleRateIndex", 1));
                    cmbBitDepth.SelectedIndex = (int)(key.GetValue("BitDepthIndex", 1));
                    cmbChannels.SelectedIndex = (int)(key.GetValue("ChannelsIndex", 0));
                }
            }
        }

        private void SaveSettings()
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    key.SetValue("SampleRateIndex", cmbSampleRate.SelectedIndex);
                    key.SetValue("BitDepthIndex", cmbBitDepth.SelectedIndex);
                    key.SetValue("ChannelsIndex", cmbChannels.SelectedIndex);
                }
            }
        }

        public int SampleRate
        {
            get { return int.Parse(cmbSampleRate.SelectedItem as string); }
        }

        public int BitDepth
        {
            get { return int.Parse(cmbBitDepth.SelectedItem as string); }
        }

        public int Channels
        {
            get { return int.Parse(cmbChannels.SelectedItem as string); }
        }
    }
}
