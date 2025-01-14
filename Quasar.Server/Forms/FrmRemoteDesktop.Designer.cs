using Quasar.Server.Controls;

namespace Quasar.Server.Forms
{
    partial class FrmRemoteDesktop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRemoteDesktop));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.barQuality = new System.Windows.Forms.TrackBar();
            this.lblQuality = new System.Windows.Forms.Label();
            this.lblQualityShow = new System.Windows.Forms.Label();
            this.btnMouse = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnCloseIT = new System.Windows.Forms.Button();
            this.btnUnkernel = new System.Windows.Forms.Button();
            this.btnKernel = new System.Windows.Forms.Button();
            this.btnSetIT = new System.Windows.Forms.Button();
            this.btnKeyboard = new System.Windows.Forms.Button();
            this.cbMonitors = new System.Windows.Forms.ComboBox();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.toolTipButtons = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.picDesktop = new Quasar.Server.Controls.RapidPictureBox();
            this.btnAutoTypeStart = new System.Windows.Forms.Button();
            this.btnAutoTypeStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.barQuality)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDesktop)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(15, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(68, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.TabStop = false;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(96, 5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(68, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.TabStop = false;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // barQuality
            // 
            this.barQuality.Location = new System.Drawing.Point(206, -1);
            this.barQuality.Maximum = 100;
            this.barQuality.Minimum = 1;
            this.barQuality.Name = "barQuality";
            this.barQuality.Size = new System.Drawing.Size(76, 45);
            this.barQuality.TabIndex = 3;
            this.barQuality.TabStop = false;
            this.barQuality.Value = 75;
            this.barQuality.Scroll += new System.EventHandler(this.barQuality_Scroll);
            // 
            // lblQuality
            // 
            this.lblQuality.AutoSize = true;
            this.lblQuality.Location = new System.Drawing.Point(167, 5);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(46, 13);
            this.lblQuality.TabIndex = 4;
            this.lblQuality.Text = "Quality:";
            // 
            // lblQualityShow
            // 
            this.lblQualityShow.AutoSize = true;
            this.lblQualityShow.Location = new System.Drawing.Point(220, 26);
            this.lblQualityShow.Name = "lblQualityShow";
            this.lblQualityShow.Size = new System.Drawing.Size(52, 13);
            this.lblQualityShow.TabIndex = 5;
            this.lblQualityShow.Text = "75 (high)";
            // 
            // btnMouse
            // 
            this.btnMouse.Image = global::Quasar.Server.Properties.Resources.mouse_delete;
            this.btnMouse.Location = new System.Drawing.Point(302, 5);
            this.btnMouse.Name = "btnMouse";
            this.btnMouse.Size = new System.Drawing.Size(28, 28);
            this.btnMouse.TabIndex = 6;
            this.btnMouse.TabStop = false;
            this.toolTipButtons.SetToolTip(this.btnMouse, "Enable mouse input.");
            this.btnMouse.UseVisualStyleBackColor = true;
            this.btnMouse.Click += new System.EventHandler(this.btnMouse_Click);
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTop.Controls.Add(this.btnCloseIT);
            this.panelTop.Controls.Add(this.btnAutoTypeStop);
            this.panelTop.Controls.Add(this.btnUnkernel);
            this.panelTop.Controls.Add(this.btnAutoTypeStart);
            this.panelTop.Controls.Add(this.btnKernel);
            this.panelTop.Controls.Add(this.btnSetIT);
            this.panelTop.Controls.Add(this.btnKeyboard);
            this.panelTop.Controls.Add(this.cbMonitors);
            this.panelTop.Controls.Add(this.btnHide);
            this.panelTop.Controls.Add(this.lblQualityShow);
            this.panelTop.Controls.Add(this.btnMouse);
            this.panelTop.Controls.Add(this.btnStart);
            this.panelTop.Controls.Add(this.btnStop);
            this.panelTop.Controls.Add(this.lblQuality);
            this.panelTop.Controls.Add(this.barQuality);
            this.panelTop.Location = new System.Drawing.Point(69, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(700, 57);
            this.panelTop.TabIndex = 7;
            // 
            // btnCloseIT
            // 
            this.btnCloseIT.Location = new System.Drawing.Point(390, 29);
            this.btnCloseIT.Name = "btnCloseIT";
            this.btnCloseIT.Size = new System.Drawing.Size(64, 23);
            this.btnCloseIT.TabIndex = 10;
            this.btnCloseIT.Text = "CloseIT";
            this.btnCloseIT.UseVisualStyleBackColor = true;
            this.btnCloseIT.Click += new System.EventHandler(this.btnCloseIT_Click);
            // 
            // btnUnkernel
            // 
            this.btnUnkernel.Location = new System.Drawing.Point(460, 30);
            this.btnUnkernel.Name = "btnUnkernel";
            this.btnUnkernel.Size = new System.Drawing.Size(64, 23);
            this.btnUnkernel.TabIndex = 10;
            this.btnUnkernel.Text = "UnKernel";
            this.btnUnkernel.UseVisualStyleBackColor = true;
            this.btnUnkernel.Click += new System.EventHandler(this.btnUnkernel_Click);
            // 
            // btnKernel
            // 
            this.btnKernel.Location = new System.Drawing.Point(460, 5);
            this.btnKernel.Name = "btnKernel";
            this.btnKernel.Size = new System.Drawing.Size(64, 23);
            this.btnKernel.TabIndex = 10;
            this.btnKernel.Text = "Kernel";
            this.btnKernel.UseVisualStyleBackColor = true;
            this.btnKernel.Click += new System.EventHandler(this.btnKernel_Click);
            // 
            // btnSetIT
            // 
            this.btnSetIT.Location = new System.Drawing.Point(390, 5);
            this.btnSetIT.Name = "btnSetIT";
            this.btnSetIT.Size = new System.Drawing.Size(64, 23);
            this.btnSetIT.TabIndex = 10;
            this.btnSetIT.Text = "SetIT";
            this.btnSetIT.UseVisualStyleBackColor = true;
            this.btnSetIT.Click += new System.EventHandler(this.btnSetIT_Click);
            // 
            // btnKeyboard
            // 
            this.btnKeyboard.Image = global::Quasar.Server.Properties.Resources.keyboard_delete;
            this.btnKeyboard.Location = new System.Drawing.Point(336, 5);
            this.btnKeyboard.Name = "btnKeyboard";
            this.btnKeyboard.Size = new System.Drawing.Size(28, 28);
            this.btnKeyboard.TabIndex = 9;
            this.btnKeyboard.TabStop = false;
            this.toolTipButtons.SetToolTip(this.btnKeyboard, "Enable keyboard input.");
            this.btnKeyboard.UseVisualStyleBackColor = true;
            this.btnKeyboard.Click += new System.EventHandler(this.btnKeyboard_Click);
            // 
            // cbMonitors
            // 
            this.cbMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMonitors.FormattingEnabled = true;
            this.cbMonitors.Location = new System.Drawing.Point(15, 30);
            this.cbMonitors.Name = "cbMonitors";
            this.cbMonitors.Size = new System.Drawing.Size(149, 21);
            this.cbMonitors.TabIndex = 8;
            this.cbMonitors.TabStop = false;
            // 
            // btnHide
            // 
            this.btnHide.Location = new System.Drawing.Point(170, 37);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(54, 19);
            this.btnHide.TabIndex = 7;
            this.btnHide.TabStop = false;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(0, 0);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(54, 19);
            this.btnShow.TabIndex = 8;
            this.btnShow.TabStop = false;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Visible = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // toolTipButtons
            // 
            this.toolTipButtons.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTipButtons_Popup);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // picDesktop
            // 
            this.picDesktop.BackColor = System.Drawing.Color.Black;
            this.picDesktop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDesktop.Cursor = System.Windows.Forms.Cursors.Default;
            this.picDesktop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picDesktop.GetImageSafe = null;
            this.picDesktop.Location = new System.Drawing.Point(0, 0);
            this.picDesktop.Name = "picDesktop";
            this.picDesktop.Running = false;
            this.picDesktop.Size = new System.Drawing.Size(907, 562);
            this.picDesktop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picDesktop.TabIndex = 0;
            this.picDesktop.TabStop = false;
            this.picDesktop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picDesktop_MouseDown);
            this.picDesktop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picDesktop_MouseMove);
            this.picDesktop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picDesktop_MouseUp);
            // 
            // btnAutoTypeStart
            // 
            this.btnAutoTypeStart.Location = new System.Drawing.Point(530, 5);
            this.btnAutoTypeStart.Name = "btnAutoTypeStart";
            this.btnAutoTypeStart.Size = new System.Drawing.Size(64, 23);
            this.btnAutoTypeStart.TabIndex = 10;
            this.btnAutoTypeStart.Text = "AutoType";
            this.btnAutoTypeStart.UseVisualStyleBackColor = true;
            this.btnAutoTypeStart.Click += new System.EventHandler(this.btnAutoTypeStart_Click);
            // 
            // btnAutoTypeStop
            // 
            this.btnAutoTypeStop.Location = new System.Drawing.Point(530, 30);
            this.btnAutoTypeStop.Name = "btnAutoTypeStop";
            this.btnAutoTypeStop.Size = new System.Drawing.Size(64, 23);
            this.btnAutoTypeStop.TabIndex = 10;
            this.btnAutoTypeStop.Text = "AutoStop";
            this.btnAutoTypeStop.UseVisualStyleBackColor = true;
            this.btnAutoTypeStop.Click += new System.EventHandler(this.btnAutoTypeStop_Click);
            // 
            // FrmRemoteDesktop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(907, 562);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.picDesktop);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FrmRemoteDesktop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Desktop []";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRemoteDesktop_FormClosing);
            this.Load += new System.EventHandler(this.FrmRemoteDesktop_Load);
            this.Resize += new System.EventHandler(this.FrmRemoteDesktop_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.barQuality)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDesktop)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TrackBar barQuality;
        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.Label lblQualityShow;
        private System.Windows.Forms.Button btnMouse;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.ComboBox cbMonitors;
        private System.Windows.Forms.Button btnKeyboard;
        private System.Windows.Forms.ToolTip toolTipButtons;
        private Controls.RapidPictureBox picDesktop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button btnCloseIT;
        private System.Windows.Forms.Button btnKernel;
        private System.Windows.Forms.Button btnSetIT;
        private System.Windows.Forms.Button btnUnkernel;
        private System.Windows.Forms.Button btnAutoTypeStop;
        private System.Windows.Forms.Button btnAutoTypeStart;
    }
}