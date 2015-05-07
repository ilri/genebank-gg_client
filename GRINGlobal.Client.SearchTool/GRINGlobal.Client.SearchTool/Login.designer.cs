namespace GRINGlobal.Client.SearchTool
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.ux_textboxUsername = new System.Windows.Forms.TextBox();
            this.ux_textboxPassword = new System.Windows.Forms.TextBox();
            this.ux_labelUsername = new System.Windows.Forms.Label();
            this.ux_labelPassword = new System.Windows.Forms.Label();
            this.ux_buttonOk = new System.Windows.Forms.Button();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_textboxLoginMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ux_textboxUsername
            // 
            this.ux_textboxUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxUsername.Location = new System.Drawing.Point(76, 13);
            this.ux_textboxUsername.Name = "ux_textboxUsername";
            this.ux_textboxUsername.Size = new System.Drawing.Size(182, 20);
            this.ux_textboxUsername.TabIndex = 0;
            this.ux_textboxUsername.TextChanged += new System.EventHandler(this.ux_textboxUsername_TextChanged);
            // 
            // ux_textboxPassword
            // 
            this.ux_textboxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxPassword.Location = new System.Drawing.Point(76, 40);
            this.ux_textboxPassword.Name = "ux_textboxPassword";
            this.ux_textboxPassword.PasswordChar = '*';
            this.ux_textboxPassword.Size = new System.Drawing.Size(182, 20);
            this.ux_textboxPassword.TabIndex = 1;
            this.ux_textboxPassword.TextChanged += new System.EventHandler(this.ux_textboxPassword_TextChanged);
            // 
            // ux_labelUsername
            // 
            this.ux_labelUsername.AutoSize = true;
            this.ux_labelUsername.Location = new System.Drawing.Point(12, 16);
            this.ux_labelUsername.Name = "ux_labelUsername";
            this.ux_labelUsername.Size = new System.Drawing.Size(58, 13);
            this.ux_labelUsername.TabIndex = 2;
            this.ux_labelUsername.Text = "Username:";
            // 
            // ux_labelPassword
            // 
            this.ux_labelPassword.AutoSize = true;
            this.ux_labelPassword.Location = new System.Drawing.Point(12, 43);
            this.ux_labelPassword.Name = "ux_labelPassword";
            this.ux_labelPassword.Size = new System.Drawing.Size(56, 13);
            this.ux_labelPassword.TabIndex = 3;
            this.ux_labelPassword.Text = "Password:";
            // 
            // ux_buttonOk
            // 
            this.ux_buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOk.Location = new System.Drawing.Point(102, 101);
            this.ux_buttonOk.Name = "ux_buttonOk";
            this.ux_buttonOk.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonOk.TabIndex = 4;
            this.ux_buttonOk.Text = "OK";
            this.ux_buttonOk.UseVisualStyleBackColor = true;
            this.ux_buttonOk.Click += new System.EventHandler(this.ux_buttonOk_Click);
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.Location = new System.Drawing.Point(183, 101);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 5;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // ux_textboxLoginMessage
            // 
            this.ux_textboxLoginMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxLoginMessage.BackColor = System.Drawing.SystemColors.Control;
            this.ux_textboxLoginMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ux_textboxLoginMessage.Location = new System.Drawing.Point(15, 66);
            this.ux_textboxLoginMessage.Multiline = true;
            this.ux_textboxLoginMessage.Name = "ux_textboxLoginMessage";
            this.ux_textboxLoginMessage.ReadOnly = true;
            this.ux_textboxLoginMessage.Size = new System.Drawing.Size(243, 29);
            this.ux_textboxLoginMessage.TabIndex = 6;
            this.ux_textboxLoginMessage.Text = "Error: Username/Password are not valid.  Please correct and try again.";
            this.ux_textboxLoginMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 130);
            this.Controls.Add(this.ux_textboxLoginMessage);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOk);
            this.Controls.Add(this.ux_labelPassword);
            this.Controls.Add(this.ux_labelUsername);
            this.Controls.Add(this.ux_textboxPassword);
            this.Controls.Add(this.ux_textboxUsername);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ux_textboxUsername;
        private System.Windows.Forms.TextBox ux_textboxPassword;
        private System.Windows.Forms.Label ux_labelUsername;
        private System.Windows.Forms.Label ux_labelPassword;
        private System.Windows.Forms.Button ux_buttonOk;
        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.TextBox ux_textboxLoginMessage;
    }
}