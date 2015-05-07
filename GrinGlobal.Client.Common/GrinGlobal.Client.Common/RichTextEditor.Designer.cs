namespace GRINGlobal.Client.Common
{
    partial class RichTextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextEditor));
            this.ux_buttonOK = new System.Windows.Forms.Button();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_richtextboxMessage = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ux_buttonOK
            // 
            this.ux_buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ux_buttonOK.Location = new System.Drawing.Point(116, 227);
            this.ux_buttonOK.Name = "ux_buttonOK";
            this.ux_buttonOK.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonOK.TabIndex = 0;
            this.ux_buttonOK.Text = "OK";
            this.ux_buttonOK.UseVisualStyleBackColor = true;
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ux_buttonCancel.Location = new System.Drawing.Point(197, 227);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 1;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            // 
            // ux_richtextboxMessage
            // 
            this.ux_richtextboxMessage.AcceptsTab = true;
            this.ux_richtextboxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_richtextboxMessage.Location = new System.Drawing.Point(13, 13);
            this.ux_richtextboxMessage.Name = "ux_richtextboxMessage";
            this.ux_richtextboxMessage.Size = new System.Drawing.Size(259, 208);
            this.ux_richtextboxMessage.TabIndex = 2;
            this.ux_richtextboxMessage.Text = "";
            // 
            // RichTextEditor
            // 
            this.AcceptButton = this.ux_buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ux_buttonCancel;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ux_richtextboxMessage);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RichTextEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ux_buttonOK;
        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.RichTextBox ux_richtextboxMessage;
    }
}