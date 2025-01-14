using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quasar.Server.Forms
{
    public partial class FrmAudioSetting : Form
    {
        public FrmAudioSetting()
        {
            InitializeComponent();
            cmbSampleRate.SelectedIndex = 1;
            cmbBitDepth.SelectedIndex = 1;
            cmbChannels.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
