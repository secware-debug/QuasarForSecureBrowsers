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
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(614, 160);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(614, 189);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
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
            this.txtContent.Size = new System.Drawing.Size(589, 235);
            this.txtContent.TabIndex = 1;
            // 
            // txtFontSize
            // 
            this.txtFontSize.Location = new System.Drawing.Point(623, 39);
            this.txtFontSize.Name = "txtFontSize";
            this.txtFontSize.Size = new System.Drawing.Size(75, 20);
            this.txtFontSize.TabIndex = 2;
            this.txtFontSize.Text = "36";
            this.txtFontSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(608, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Font Size";
            // 
            // chkTextSave
            // 
            this.chkTextSave.AutoSize = true;
            this.chkTextSave.Location = new System.Drawing.Point(624, 223);
            this.chkTextSave.Name = "chkTextSave";
            this.chkTextSave.Size = new System.Drawing.Size(51, 17);
            this.chkTextSave.TabIndex = 4;
            this.chkTextSave.Text = "Save";
            this.chkTextSave.UseVisualStyleBackColor = true;
            this.chkTextSave.CheckedChanged += new System.EventHandler(this.chkTextSave_CheckedChanged);
            // 
            // txtStartX
            // 
            this.txtStartX.Location = new System.Drawing.Point(616, 84);
            this.txtStartX.Name = "txtStartX";
            this.txtStartX.Size = new System.Drawing.Size(42, 20);
            this.txtStartX.TabIndex = 2;
            this.txtStartX.Text = "0";
            this.txtStartX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(609, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Start X";
            // 
            // txtStartY
            // 
            this.txtStartY.Location = new System.Drawing.Point(673, 84);
            this.txtStartY.Name = "txtStartY";
            this.txtStartY.Size = new System.Drawing.Size(41, 20);
            this.txtStartY.TabIndex = 2;
            this.txtStartY.Text = "0";
            this.txtStartY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(666, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Start Y";
            // 
            // txtEndX
            // 
            this.txtEndX.Location = new System.Drawing.Point(616, 125);
            this.txtEndX.Name = "txtEndX";
            this.txtEndX.Size = new System.Drawing.Size(42, 20);
            this.txtEndX.TabIndex = 2;
            this.txtEndX.Text = "0";
            this.txtEndX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // txtEndY
            // 
            this.txtEndY.Location = new System.Drawing.Point(673, 125);
            this.txtEndY.Name = "txtEndY";
            this.txtEndY.Size = new System.Drawing.Size(41, 20);
            this.txtEndY.TabIndex = 2;
            this.txtEndY.Text = "0";
            this.txtEndY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFontSize_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(666, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "End Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "End X";
            // 
            // FrmIVText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 260);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkTextSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtEndY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEndX);
            this.Controls.Add(this.txtStartY);
            this.Controls.Add(this.txtStartX);
            this.Controls.Add(this.txtFontSize);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "FrmIVText";
            this.Text = "Red Text Content";
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
    }
}