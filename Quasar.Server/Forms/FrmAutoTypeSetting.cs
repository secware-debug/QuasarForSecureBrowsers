using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace Quasar.Server.Forms
{
    public partial class FrmAutoTypeSetting : Form
    {
        private const string RegistryKeyPath = @"SOFTWARE\Quasar\AutoTypeSettings";
        private const string MinDelayKey = "MinDelay";
        private const string MaxDelayKey = "MaxDelay";

        public FrmAutoTypeSetting()
        {
            InitializeComponent();
            LoadSettingsFromRegistry();
        }
        private void LoadSettingsFromRegistry()
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    // Load MinDelay
                    object minDelayValue = key.GetValue(MinDelayKey);
                    if (minDelayValue != null)
                    {
                        txtMinDelay.Text = minDelayValue.ToString();
                    }

                    // Load MaxDelay
                    object maxDelayValue = key.GetValue(MaxDelayKey);
                    if (maxDelayValue != null)
                    {
                        txtMaxDelay.Text = maxDelayValue.ToString();
                    }
                }
            }
        }

        private void SaveSettingsToRegistry()
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    key.SetValue(MinDelayKey, txtMinDelay.Text);
                    key.SetValue(MaxDelayKey, txtMaxDelay.Text);
                }
            }
        }

        private void textBoxMinDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                // Check if the key pressed is a digit
                if (!char.IsDigit(e.KeyChar))
                {
                    // If it's not a digit, suppress the key press
                    e.Handled = true;
                }
            }
        }

        private void textBoxMaxDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar))
            {
                // Check if the key pressed is a digit
                if (!char.IsDigit(e.KeyChar))
                {
                    // If it's not a digit, suppress the key press
                    e.Handled = true;
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(txtContent.Text);
            if (txtContent.Text == "")
            {
                MessageBox.Show("Please fill the auto type content.");
                return;
            }
            else
            {
                SaveSettingsToRegistry();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string AutotypeContent
        {
            get {
                if (txtContent.Text.Contains("\r\n"))
                {
                    string modifiedText = txtContent.Text.Replace("\r\n", "\n");
                    return modifiedText;
                }
                return txtContent.Text; 
            }
        }

        public int MinDelayValue
        {
            get { return int.Parse(txtMinDelay.Text) > 10 ? int.Parse(txtMinDelay.Text) : 10; }
        }
        public int MaxDelayValue
        {
            get { return int.Parse(txtMaxDelay.Text); }
        }

    }
}
