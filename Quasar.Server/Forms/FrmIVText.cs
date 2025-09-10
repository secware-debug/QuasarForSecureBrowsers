using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace Quasar.Server.Forms
{
    public partial class FrmIVText : Form
    {
        private const string RegistryKeyPath = @"SOFTWARE\Quasar\IVTextSettings";
        private const string FontSizeKey = "FontSize";
        private const string IsSavedKey = "TextSaved";
        private const string SavedTextKey = "SavedText";
        private const string StartXKey = "StartXPos";
        private const string StartYKey = "StartYPos";

        public FrmIVText()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
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

                    // Load StartX value
                    object startXValue = key.GetValue(StartXKey);
                    if (startXValue != null)
                    {
                        txtStartX.Text = startXValue.ToString();
                    }

                    // Load StartY value
                    object startYValue = key.GetValue(StartYKey);
                    if (startYValue != null)
                    {
                        txtStartY.Text = startYValue.ToString();
                    }

                    // Load IsSaved state
                    object isSavedValue = key.GetValue(IsSavedKey);
                    if (isSavedValue != null && bool.TryParse(isSavedValue.ToString(), out bool isSaved))
                    {
                        chkTextSave.Checked = isSaved;

                        if (isSaved)
                        {
                            object savedTextValue = key.GetValue(SavedTextKey);
                            if (savedTextValue != null)
                            {
                                txtContent.Text = savedTextValue.ToString();
                            }
                        }
                        else
                        {
                            txtContent.Text = string.Empty;
                        }
                    }
                }
            }
        }

        private void SaveSettings()
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
            {
                if (key != null)
                {
                    key.SetValue(FontSizeKey, txtFontSize.Text);
                    key.SetValue(StartXKey, txtStartX.Text);
                    key.SetValue(StartYKey, txtStartY.Text);
                    if (chkTextSave.Checked) { 
                        key.SetValue(IsSavedKey, true);
                        key.SetValue(SavedTextKey, txtContent.Text);
                    }
                    else
                    {
                        key.SetValue(IsSavedKey, false);
                        key.SetValue(SavedTextKey, "");
                    }
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
            SaveSettings();
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

        public int StartX
        {
            get { return int.Parse(txtStartX.Text); }
        }
        public int StartY
        {
            get { return int.Parse(txtStartY.Text); }
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

        private void chkTextSave_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
