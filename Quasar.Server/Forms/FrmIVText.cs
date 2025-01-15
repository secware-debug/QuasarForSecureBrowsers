using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace Quasar.Server.Forms
{
    public partial class FrmIVText : Form
    {
        private const string RegistryKeyPath = @"SOFTWARE\Quasar\IVTextSettings";
        private const string FontSizeKey = "FontSize";

        public FrmIVText()
        {
            InitializeComponent();
            LoadFontSizeFromRegistry();
        }

        private void LoadFontSizeFromRegistry()
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    // Load FontSize
                    object fontSizeValue = key.GetValue(FontSizeKey);
                    if (fontSizeValue != null)
                    {
                        txtFontSize.Text = fontSizeValue.ToString();
                    }
                }
            }
        }

        private void SaveFontSizeToRegistry()
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    key.SetValue(FontSizeKey, txtFontSize.Text);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int number;
            if(int.TryParse(txtFontSize.Text, out number)) { 
                if(number < 5 || number > 149) {
                    MessageBox.Show("Please enter font size value between 5 and 149");
                    return;
                }
            }
            SaveFontSizeToRegistry();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string DialogText
        {
            get { return txtContent.Text; } // Replace 'myTextBox' with your TextBox's name
        }

        public int FontSize
        {
            get { return int.Parse(txtFontSize.Text); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFontSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the key pressed is a control key (like Backspace)
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
    }
}
