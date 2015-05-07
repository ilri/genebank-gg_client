namespace GRINGlobal.Client
{
    partial class Splash
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
            this.ux_labelProgressMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ux_labelProgressMessage
            // 
            this.ux_labelProgressMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelProgressMessage.BackColor = System.Drawing.Color.Transparent;
            this.ux_labelProgressMessage.Location = new System.Drawing.Point(0, 138);
            this.ux_labelProgressMessage.Name = "ux_labelProgressMessage";
            this.ux_labelProgressMessage.Size = new System.Drawing.Size(149, 13);
            this.ux_labelProgressMessage.TabIndex = 0;
            this.ux_labelProgressMessage.Text = "System data loading...";
            // 
            // Splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GRINGlobal.Client.Properties.Resources.GG_logo;
            this.ClientSize = new System.Drawing.Size(152, 152);
            this.Controls.Add(this.ux_labelProgressMessage);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Splash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splash";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ux_labelProgressMessage;

    }
}