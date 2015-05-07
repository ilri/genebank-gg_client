namespace GRINGlobal.Client.Common
{
    partial class GGMessageBox
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
            this.ux_textboxMessage = new System.Windows.Forms.TextBox();
            this.ux_button1 = new System.Windows.Forms.Button();
            this.ux_button2 = new System.Windows.Forms.Button();
            this.ux_button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ux_textboxMessage
            // 
            this.ux_textboxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxMessage.Location = new System.Drawing.Point(12, 12);
            this.ux_textboxMessage.Multiline = true;
            this.ux_textboxMessage.Name = "ux_textboxMessage";
            this.ux_textboxMessage.ReadOnly = true;
            this.ux_textboxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ux_textboxMessage.Size = new System.Drawing.Size(312, 152);
            this.ux_textboxMessage.TabIndex = 0;
            this.ux_textboxMessage.Text = "Sample Text";
            // 
            // ux_button1
            // 
            this.ux_button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_button1.Location = new System.Drawing.Point(87, 170);
            this.ux_button1.Name = "ux_button1";
            this.ux_button1.Size = new System.Drawing.Size(75, 23);
            this.ux_button1.TabIndex = 1;
            this.ux_button1.Text = "ux_button1";
            this.ux_button1.UseVisualStyleBackColor = true;
            // 
            // ux_button2
            // 
            this.ux_button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_button2.Location = new System.Drawing.Point(168, 170);
            this.ux_button2.Name = "ux_button2";
            this.ux_button2.Size = new System.Drawing.Size(75, 23);
            this.ux_button2.TabIndex = 2;
            this.ux_button2.Text = "ux_button2";
            this.ux_button2.UseVisualStyleBackColor = true;
            // 
            // ux_button3
            // 
            this.ux_button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_button3.Location = new System.Drawing.Point(249, 170);
            this.ux_button3.Name = "ux_button3";
            this.ux_button3.Size = new System.Drawing.Size(75, 23);
            this.ux_button3.TabIndex = 3;
            this.ux_button3.Text = "ux_button3";
            this.ux_button3.UseVisualStyleBackColor = true;
            // 
            // GGMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 205);
            this.Controls.Add(this.ux_button3);
            this.Controls.Add(this.ux_button2);
            this.Controls.Add(this.ux_button1);
            this.Controls.Add(this.ux_textboxMessage);
            this.Name = "GGMessageBox";
            this.Text = "GGMessageBox";
            this.Load += new System.EventHandler(this.GGMessageBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ux_textboxMessage;
        private System.Windows.Forms.Button ux_button1;
        private System.Windows.Forms.Button ux_button2;
        private System.Windows.Forms.Button ux_button3;
    }
}