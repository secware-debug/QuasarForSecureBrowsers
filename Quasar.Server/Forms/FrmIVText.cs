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
        private const string EndXKey = "EndXPos";
        private const string EndYKey = "EndYPos";
        private const string BkColorValueKey = "BackgroundColor";
        private const string TxtColorValueKey = "TextColor";
        private const string BkColorExistKey = "BackgroundExist";
        private const string TimerExistKey = "AutoKillTimerExist";
        private const string TimerValueKey = "AutoKillTimerValue";
        private const string AlphaValueKey = "AlphaValue";
        private const string BkCustomColorsKey = "BackgroundCustomColors";
        private const string TextCustomColorsKey = "TextCustomColors";
        private uint bkColorValue = 0xFFFF0080;
        private uint txtColorValue = 0xFFFF0000;
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

                    // Load EndX value
                    object endXValue = key.GetValue(EndXKey);
                    if (endXValue != null)
                    {
                        txtEndX.Text = endXValue.ToString();
                    }

                    // Load EndY value
                    object endYValue = key.GetValue(EndYKey);
                    if (endYValue != null)
                    {
                        txtEndY.Text = endYValue.ToString();
                    }

                    // Load Opacity value
                    object opacityValue = key.GetValue(AlphaValueKey);
                    if (opacityValue != null)
                    {
                        textBox_Opacity.Text = opacityValue.ToString();
                    }

                    // Load Text color value
                    if (key.GetValue(TxtColorValueKey) is int txtcolor)
                    {
                        txtColorValue = unchecked((uint)txtcolor);
                    }

                    // Load Background Color existance
                    object IsBkExist = key.GetValue(BkColorExistKey);
                    if (IsBkExist != null && bool.TryParse(IsBkExist.ToString(), out bool result1))
                    {
                        checkBox_SetBk.Checked = result1;
                        if (result1)
                        {
                            // Load Background color value
                            if (key.GetValue(BkColorValueKey) is int color)
                            {
                                bkColorValue = unchecked((uint)color);
                            }
                        }
                        else
                        {
                            btnBkColor.Enabled = false;
                        }
                    }

                    // Load AutoKill Timer Existance
                    object ExistAutoKill = key.GetValue(TimerExistKey);
                    if (ExistAutoKill != null && bool.TryParse(ExistAutoKill.ToString(), out bool result2))
                    {
                        checkBox_AutoKill.Checked = result2;
                        if (result2)
                        {
                            object timerValue = key.GetValue(TimerValueKey);
                            if (timerValue != null)
                            {
                                textBox_AutoKillTime.Text = timerValue.ToString();
                            }
                        }
                        else
                        {
                            textBox_AutoKillTime.Enabled = false;
                        }
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

        private void SaveCustomColors(int[] colors, string regKey)
        {
            if (colors == null) return;

            byte[] data = new byte[colors.Length * 4];
            Buffer.BlockCopy(colors, 0, data, 0, data.Length);

            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistryKeyPath))
            {
                key.SetValue(regKey, data, RegistryValueKind.Binary);
            }
        }

        private int[] LoadCustomColors(string regKey)
        {
            using (RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                if (key == null)
                    return null;

                byte[] data = key.GetValue(regKey) as byte[];
                if (data == null || data.Length == 0 || data.Length % 4 != 0)
                    return null;

                int[] colors = new int[data.Length / 4];
                Buffer.BlockCopy(data, 0, colors, 0, data.Length);
                return colors;
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
                    key.SetValue(EndXKey, txtEndX.Text);
                    key.SetValue(EndYKey, txtEndY.Text);
                    
                    key.SetValue(TxtColorValueKey, (int)txtColorValue, RegistryValueKind.DWord);
                    key.SetValue(AlphaValueKey, textBox_Opacity.Text);
                    key.SetValue(BkColorExistKey, checkBox_SetBk.Checked);
                    key.SetValue(TimerExistKey, checkBox_AutoKill.Checked);

                    if (checkBox_SetBk.Checked)
                    {
                        key.SetValue(BkColorValueKey, (int)bkColorValue, RegistryValueKind.DWord);
                    }

                    if (checkBox_AutoKill.Checked)
                    {
                        key.SetValue(TimerValueKey, textBox_AutoKillTime.Text);
                    }

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
        public int EndX
        {
            get { return int.Parse(txtEndX.Text); }
        }
        public int EndY
        {
            get { return int.Parse(txtEndY.Text); }
        }

        public uint TextColor
        {
            get { return txtColorValue; }
        }

        public bool IsAutoKill
        {
            get { return checkBox_AutoKill.Checked; }
        }

        public int AutoKillTime
        {
            get { return int.Parse(textBox_AutoKillTime.Text); }
        }

        public bool IsBackgroundExist
        {
            get { return checkBox_SetBk.Checked; }
        }

        public int OpacityValue
        {
            get { return int.Parse(textBox_Opacity.Text); }
        }

        public uint BkColor
        {
            get { return bkColorValue; }
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

        private void chkSetBk_CheckedChanged(object sender, EventArgs e)
        {
            btnBkColor.Enabled = checkBox_SetBk.Checked;
        }
        private void chkAutoKill_CheckedChanged(object sender, EventArgs e)
        {
            textBox_AutoKillTime.Enabled = checkBox_AutoKill.Checked;
        }

        private void FrmIVText_Load(object sender, EventArgs e)
        {

        }

        private void btnBkColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.FullOpen = true;
                colorDialog.AnyColor = true;
                int[] customColors = LoadCustomColors(BkCustomColorsKey);

                if (customColors != null)
                {
                    colorDialog.CustomColors = customColors;
                }

                if(colorDialog.ShowDialog() == DialogResult.OK)
                {
                    //MessageBox.Show(colorDialog.Color.ToArgb().ToString());
                    bkColorValue = (uint)colorDialog.Color.ToArgb();
                    SaveCustomColors(colorDialog.CustomColors, BkCustomColorsKey);
                }
            }
        }

        private void btnTextColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.FullOpen = true;
                colorDialog.AnyColor = true;

                int[] customColors = LoadCustomColors(TextCustomColorsKey);
                if (customColors != null)
                {
                    colorDialog.CustomColors = customColors;
                }

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    //MessageBox.Show(colorDialog.Color.ToArgb().ToString());
                    txtColorValue = (uint)colorDialog.Color.ToArgb();
                    SaveCustomColors(colorDialog.CustomColors, TextCustomColorsKey);
                }
            }
        }

        private void txtFontSize_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
