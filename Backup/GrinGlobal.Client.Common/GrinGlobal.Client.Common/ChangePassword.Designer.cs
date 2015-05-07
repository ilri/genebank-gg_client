namespace GRINGlobal.Client.Common
{
    partial class ChangePassword
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
            this.ux_labelPassword = new System.Windows.Forms.Label();
            this.ux_labelUsername = new System.Windows.Forms.Label();
            this.ux_textboxCurrentPassword = new System.Windows.Forms.TextBox();
            this.ux_textboxUsername = new System.Windows.Forms.TextBox();
            this.ux_labelNewPassword = new System.Windows.Forms.Label();
            this.ux_textboxNewPassword = new System.Windows.Forms.TextBox();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ux_labelPassword
            // 
            this.ux_labelPassword.Location = new System.Drawing.Point(1, 41);
            this.ux_labelPassword.Name = "ux_labelPassword";
            this.ux_labelPassword.Size = new System.Drawing.Size(101, 13);
            this.ux_labelPassword.TabIndex = 7;
            this.ux_labelPassword.Text = "Current Password:";
            this.ux_labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ux_labelUsername
            // 
            this.ux_labelUsername.Location = new System.Drawing.Point(1, 15);
            this.ux_labelUsername.Name = "ux_labelUsername";
            this.ux_labelUsername.Size = new System.Drawing.Size(103, 13);
            this.ux_labelUsername.TabIndex = 6;
            this.ux_labelUsername.Text = "Username:";
            this.ux_labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ux_textboxCurrentPassword
            // 
            this.ux_textboxCurrentPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxCurrentPassword.Location = new System.Drawing.Point(108, 38);
            this.ux_textboxCurrentPassword.Name = "ux_textboxCurrentPassword";
            this.ux_textboxCurrentPassword.PasswordChar = '*';
            this.ux_textboxCurrentPassword.Size = new System.Drawing.Size(157, 20);
            this.ux_textboxCurrentPassword.TabIndex = 5;
            this.ux_textboxCurrentPassword.TextChanged += new System.EventHandler(this.ux_textboxCurrentPassword_TextChanged);
            // 
            // ux_textboxUsername
            // 
            this.ux_textboxUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxUsername.Location = new System.Drawing.Point(108, 12);
            this.ux_textboxUsername.Name = "ux_textboxUsername";
            this.ux_textboxUsername.Size = new System.Drawing.Size(157, 20);
            this.ux_textboxUsername.TabIndex = 4;
            this.ux_textboxUsername.TextChanged += new System.EventHandler(this.ux_textboxUsername_TextChanged);
            // 
            // ux_labelNewPassword
            // 
            this.ux_labelNewPassword.Location = new System.Drawing.Point(1, 67);
            this.ux_labelNewPassword.Name = "ux_labelNewPassword";
            this.ux_labelNewPassword.Size = new System.Drawing.Size(101, 13);
            this.ux_labelNewPassword.TabIndex = 9;
            this.ux_labelNewPassword.Text = "New Password:";
            this.ux_labelNewPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ux_textboxNewPassword
            // 
            this.ux_textboxNewPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxNewPassword.Location = new System.Drawing.Point(108, 64);
            this.ux_textboxNewPassword.Name = "ux_textboxNewPassword";
            this.ux_textboxNewPassword.PasswordChar = '*';
            this.ux_textboxNewPassword.Size = new System.Drawing.Size(157, 20);
            this.ux_textboxNewPassword.TabIndex = 8;
            this.ux_textboxNewPassword.TextChanged += new System.EventHandler(this.ux_textboxNewPassword_TextChanged);
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.Location = new System.Drawing.Point(190, 98);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 11;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // ux_buttonOk
            // 
            this.ux_buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOk.Location = new System.Drawing.Point(109, 98);
            this.ux_buttonOk.Name = "ux_buttonOk";
            this.ux_buttonOk.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonOk.TabIndex = 10;
            this.ux_buttonOk.Text = "OK";
            this.ux_buttonOk.UseVisualStyleBackColor = true;
            this.ux_buttonOk.Click += new System.EventHandler(this.ux_buttonOk_Click);
            // 
            // ChangePassword
            // 
            this.AcceptButton = this.ux_buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 133);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOk);
            this.Controls.Add(this.ux_labelNewPassword);
            this.Controls.Add(this.ux_textboxNewPassword);
            this.Controls.Add(this.ux_labelPassword);
            this.Controls.Add(this.ux_labelUsername);
            this.Controls.Add(this.ux_textboxCurrentPassword);
            this.Controls.Add(this.ux_textboxUsername);
            this.Name = "ChangePassword";
            this.Text = "Change Password";
            this.Load += new System.EventHandler(this.ChangePassword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ux_labelPassword;
        private System.Windows.Forms.Label ux_labelUsername;
        private System.Windows.Forms.TextBox ux_textboxCurrentPassword;
        private System.Windows.Forms.TextBox ux_textboxUsername;
        private System.Windows.Forms.Label ux_labelNewPassword;
        private System.Windows.Forms.TextBox ux_textboxNewPassword;
        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.Button ux_buttonOk;
    }
}