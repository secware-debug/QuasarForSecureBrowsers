namespace Quasar.Server.Forms
{
    partial class FrmIVText
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.txtFontSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkTextSave = new System.Windows.Forms.CheckBox();
            this.txtStartX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStartY = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEndX = new System.Windows.Forms.TextBox();
            this.txtEndY = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBkColor = new System.Windows.Forms.Button();
            this.btnTextColor = new System.Windows.Forms.Button();
            this.textBox_Opacity = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_SetBk = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoKill = new System.Windows.Forms.CheckBox();
            this.textBox_AutoKillTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(620, 288);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(66, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(693, 288);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(13, 13);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(589, 298);
            this.txtContent.TabIndex = 1;
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(635, 42);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(48, 20);
            this.txtFontSize.TabIndex = 2;
            this.txtFontSize.Text = "36";
            this.txtFontSize.TextChanged += new System.EventHandler(this.txtFontSize_TextChanged);
            this.txtFontSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(617, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Font Size";
            // 
            // chkTextSave
            // 
            this.chkTextSave.AutoSize = true;
            this.chkTextSave.Location = new System.Drawing.Point(626, 265);
            this.chkTextSave.Name = "chkTextSave";
            this.chkTextSave.Size = new System.Drawing.Size(51, 17);
            this.chkTextSave.TabIndex = 4;
            this.chkTextSave.Text = "Save";
            this.chkTextSave.UseVisualStyleBackColor = true;
            this.chkTextSave.CheckedChanged += new System.EventHandler(this.chkTextSave_CheckedChanged);
            // 
            // txtStartX
            // 
            this.txtStartX.Location = new System.Drawing.Point(636, 189);
            this.txtStartX.Name = "txtStartX";
            this.txtStartX.Size = new System.Drawing.Size(42, 20);
            this.txtStartX.TabIndex = 2;
            this.txtStartX.Text = "0";
            this.txtStartX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(629, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Start X";
            // 
            // txtStartY
            // 
            this.txtStartY.Location = new System.Drawing.Point(700, 189);
            this.txtStartY.Name = "txtStartY";
            this.txtStartY.Size = new System.Drawing.Size(41, 20);
            this.txtStartY.TabIndex = 2;
            this.txtStartY.Text = "0";
            this.txtStartY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(693, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Start Y";
            // 
            // txtEndX
            // 
            this.txtEndX.Location = new System.Drawing.Point(636, 230);
            this.txtEndX.Name = "txtEndX";
            this.txtEndX.Size = new System.Drawing.Size(42, 20);
            this.txtEndX.TabIndex = 2;
            this.txtEndX.Text = "0";
            this.txtEndX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // txtEndY
            // 
            this.txtEndY.Location = new System.Drawing.Point(700, 230);
            this.txtEndY.Name = "txtEndY";
            this.txtEndY.Size = new System.Drawing.Size(41, 20);
            this.txtEndY.TabIndex = 2;
            this.txtEndY.Text = "0";
            this.txtEndY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(693, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "End Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(629, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "End X";
            // 
            // btnBkColor
            // 
            this.btnBkColor.Location = new System.Drawing.Point(684, 81);
            this.btnBkColor.Name = "btnBkColor";
            this.btnBkColor.Size = new System.Drawing.Size(66, 22);
            this.btnBkColor.TabIndex = 5;
            this.btnBkColor.Text = "BkColor";
            this.btnBkColor.UseVisualStyleBackColor = true;
            this.btnBkColor.Click += new System.EventHandler(this.btnBkColor_Click);
            // 
            // btnTextColor
            // 
            this.btnTextColor.Location = new System.Drawing.Point(620, 108);
            this.btnTextColor.Name = "btnTextColor";
            this.btnTextColor.Size = new System.Drawing.Size(132, 22);
            this.btnTextColor.TabIndex = 5;
            this.btnTextColor.Text = "TextColor";
            this.btnTextColor.UseVisualStyleBackColor = true;
            this.btnTextColor.Click += new System.EventHandler(this.btnTextColor_Click);
            // 
            // textBox_Opacity
            // 
            this.textBox_Opacity.Location = new System.Drawing.Point(704, 42);
            this.textBox_Opacity.Name = "textBox_Opacity";
            this.textBox_Opacity.Size = new System.Drawing.Size(48, 20);
            this.textBox_Opacity.TabIndex = 2;
            this.textBox_Opacity.Text = "0";
            this.textBox_Opacity.TextChanged += new System.EventHandler(this.txtFontSize_TextChanged);
            this.textBox_Opacity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(686, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Opacity";
            // 
            // checkBox_SetBk
            // 
            this.checkBox_SetBk.AutoSize = true;
            this.checkBox_SetBk.Location = new System.Drawing.Point(624, 85);
            this.checkBox_SetBk.Name = "checkBox_SetBk";
            this.checkBox_SetBk.Size = new System.Drawing.Size(58, 17);
            this.checkBox_SetBk.TabIndex = 4;
            this.checkBox_SetBk.Text = "Set Bk";
            this.checkBox_SetBk.UseVisualStyleBackColor = true;
            this.checkBox_SetBk.CheckedChanged += new System.EventHandler(this.chkSetBk_CheckedChanged);
            // 
            // checkBox_AutoKill
            // 
            this.checkBox_AutoKill.AutoSize = true;
            this.checkBox_AutoKill.Location = new System.Drawing.Point(625, 142);
            this.checkBox_AutoKill.Name = "checkBox_AutoKill";
            this.checkBox_AutoKill.Size = new System.Drawing.Size(61, 17);
            this.checkBox_AutoKill.TabIndex = 4;
            this.checkBox_AutoKill.Text = "AutoKill";
            this.checkBox_AutoKill.UseVisualStyleBackColor = true;
            this.checkBox_AutoKill.CheckedChanged += new System.EventHandler(this.chkAutoKill_CheckedChanged);
            // 
            // textBox_AutoKillTime
            // 
            this.textBox_AutoKillTime.Location = new System.Drawing.Point(695, 139);
            this.textBox_AutoKillTime.Name = "textBox_AutoKillTime";
            this.textBox_AutoKillTime.Size = new System.Drawing.Size(55, 20);
            this.textBox_AutoKillTime.TabIndex = 2;
            this.textBox_AutoKillTime.Text = "0";
            this.textBox_AutoKillTime.TextChanged += new System.EventHandler(this.txtFontSize_TextChanged);
            this.textBox_AutoKillTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // FrmIVText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 323);
            this.Controls.Add(this.btnTextColor);
            this.Controls.Add(this.btnBkColor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_AutoKill);
            this.Controls.Add(this.checkBox_SetBk);
            this.Controls.Add(this.chkTextSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEndY);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEndX);
            this.Controls.Add(this.txtStartY);
            this.Controls.Add(this.textBox_AutoKillTime);
            this.Controls.Add(this.textBox_Opacity);
            this.Controls.Add(this.txtStartX);
            this.Controls.Add(this.txtFontSize);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "FrmIVText";
            this.Text = "Red Text Content";
            this.Load += new System.EventHandler(this.FrmIVText_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.TextBox txtFontSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkTextSave;
        private System.Windows.Forms.TextBox txtStartX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStartY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEndX;
        private System.Windows.Forms.TextBox txtEndY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBkColor;
        private System.Windows.Forms.Button btnTextColor;
        private System.Windows.Forms.TextBox textBox_Opacity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox_SetBk;
        private System.Windows.Forms.CheckBox checkBox_AutoKill;
        private System.Windows.Forms.TextBox textBox_AutoKillTime;
    }
}