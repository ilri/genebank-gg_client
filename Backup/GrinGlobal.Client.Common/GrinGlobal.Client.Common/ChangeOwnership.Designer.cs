namespace GRINGlobal.Client.Common
{
    partial class ChangeOwnership
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
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_buttonOk = new System.Windows.Forms.Button();
            this.ux_comboboxNewOwner = new System.Windows.Forms.ComboBox();
            this.ux_labelNewOwner = new System.Windows.Forms.Label();
            this.ux_radiobuttonSelectedRowsOnly = new System.Windows.Forms.RadioButton();
            this.ux_radiobuttonSelectedRowsAndChildren = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.Location = new System.Drawing.Point(196, 105);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 13;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // ux_buttonOk
            // 
            this.ux_buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOk.Location = new System.Drawing.Point(115, 105);
            this.ux_buttonOk.Name = "ux_buttonOk";
            this.ux_buttonOk.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonOk.TabIndex = 12;
            this.ux_buttonOk.Text = "OK";
            this.ux_buttonOk.UseVisualStyleBackColor = true;
            this.ux_buttonOk.Click += new System.EventHandler(this.ux_buttonOk_Click);
            // 
            // ux_comboboxNewOwner
            // 
            this.ux_comboboxNewOwner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_comboboxNewOwner.FormattingEnabled = true;
            this.ux_comboboxNewOwner.Location = new System.Drawing.Point(85, 13);
            this.ux_comboboxNewOwner.Name = "ux_comboboxNewOwner";
            this.ux_comboboxNewOwner.Size = new System.Drawing.Size(183, 21);
            this.ux_comboboxNewOwner.TabIndex = 14;
            this.ux_comboboxNewOwner.SelectedIndexChanged += new System.EventHandler(this.ux_comboboxNewOwner_SelectedIndexChanged);
            // 
            // ux_labelNewOwner
            // 
            this.ux_labelNewOwner.AutoSize = true;
            this.ux_labelNewOwner.Location = new System.Drawing.Point(12, 16);
            this.ux_labelNewOwner.Name = "ux_labelNewOwner";
            this.ux_labelNewOwner.Size = new System.Drawing.Size(66, 13);
            this.ux_labelNewOwner.TabIndex = 15;
            this.ux_labelNewOwner.Text = "New Owner:";
            // 
            // ux_radiobuttonSelectedRowsOnly
            // 
            this.ux_radiobuttonSelectedRowsOnly.AutoSize = true;
            this.ux_radiobuttonSelectedRowsOnly.Location = new System.Drawing.Point(15, 50);
            this.ux_radiobuttonSelectedRowsOnly.Name = "ux_radiobuttonSelectedRowsOnly";
            this.ux_radiobuttonSelectedRowsOnly.Size = new System.Drawing.Size(114, 17);
            this.ux_radiobuttonSelectedRowsOnly.TabIndex = 16;
            this.ux_radiobuttonSelectedRowsOnly.Text = "Selected rows only";
            this.ux_radiobuttonSelectedRowsOnly.UseVisualStyleBackColor = true;
            this.ux_radiobuttonSelectedRowsOnly.CheckedChanged += new System.EventHandler(this.ux_radiobuttonSelectedRowsOnly_CheckedChanged);
            // 
            // ux_radiobuttonSelectedRowsAndChildren
            // 
            this.ux_radiobuttonSelectedRowsAndChildren.AutoSize = true;
            this.ux_radiobuttonSelectedRowsAndChildren.Location = new System.Drawing.Point(15, 73);
            this.ux_radiobuttonSelectedRowsAndChildren.Name = "ux_radiobuttonSelectedRowsAndChildren";
            this.ux_radiobuttonSelectedRowsAndChildren.Size = new System.Drawing.Size(191, 17);
            this.ux_radiobuttonSelectedRowsAndChildren.TabIndex = 17;
            this.ux_radiobuttonSelectedRowsAndChildren.Text = "Selected rows and all children rows";
            this.ux_radiobuttonSelectedRowsAndChildren.UseVisualStyleBackColor = true;
            this.ux_radiobuttonSelectedRowsAndChildren.CheckedChanged += new System.EventHandler(this.ux_radiobuttonSelectedRowsAndChildren_CheckedChanged);
            // 
            // ChangeOwnership
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 140);
            this.Controls.Add(this.ux_radiobuttonSelectedRowsAndChildren);
            this.Controls.Add(this.ux_radiobuttonSelectedRowsOnly);
            this.Controls.Add(this.ux_labelNewOwner);
            this.Controls.Add(this.ux_comboboxNewOwner);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOk);
            this.Name = "ChangeOwnership";
            this.Text = "Change Ownership";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.Button ux_buttonOk;
        private System.Windows.Forms.ComboBox ux_comboboxNewOwner;
        private System.Windows.Forms.Label ux_labelNewOwner;
        private System.Windows.Forms.RadioButton ux_radiobuttonSelectedRowsOnly;
        private System.Windows.Forms.RadioButton ux_radiobuttonSelectedRowsAndChildren;
    }
}