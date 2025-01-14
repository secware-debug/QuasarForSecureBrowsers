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
    public partial class FrmAutoTypeSetting : Form
    {
        public FrmAutoTypeSetting()
        {
            InitializeComponent();
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
