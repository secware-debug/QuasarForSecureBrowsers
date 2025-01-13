using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Quasar.Server.Forms
{
    public partial class FrmIVText : Form
    {
        public FrmIVText()
        {
            InitializeComponent();
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
