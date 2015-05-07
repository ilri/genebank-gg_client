namespace GRINGlobal.Client
{
    partial class NavigatorTabProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavigatorTabProperties));
            this.ux_labelTabText = new System.Windows.Forms.Label();
            this.ux_textboxTabText = new System.Windows.Forms.TextBox();
            this.ux_buttonOK = new System.Windows.Forms.Button();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ux_labelTabText
            // 
            this.ux_labelTabText.AutoSize = true;
            this.ux_labelTabText.Location = new System.Drawing.Point(13, 13);
            this.ux_labelTabText.Name = "ux_labelTabText";
            this.ux_labelTabText.Size = new System.Drawing.Size(60, 13);
            this.ux_labelTabText.TabIndex = 0;
            this.ux_labelTabText.Text = "Tab Name:";
            // 
            // ux_textboxTabText
            // 
            this.ux_textboxTabText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxTabText.Location = new System.Drawing.Point(80, 13);
            this.ux_textboxTabText.Name = "ux_textboxTabText";
            this.ux_textboxTabText.Size = new System.Drawing.Size(187, 20);
            this.ux_textboxTabText.TabIndex = 1;
            this.ux_textboxTabText.TextChanged += new System.EventHandler(this.ux_textboxTabText_TextChanged);
            // 
            // ux_buttonOK
            // 
            this.ux_buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOK.Location = new System.Drawing.Point(110, 49);
            this.ux_buttonOK.Name = "ux_buttonOK";
            this.ux_buttonOK.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonOK.TabIndex = 2;
            this.ux_buttonOK.Text = "OK";
            this.ux_buttonOK.UseVisualStyleBackColor = true;
            this.ux_buttonOK.Click += new System.EventHandler(this.ux_buttonOK_Click);
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.Location = new System.Drawing.Point(192, 49);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 3;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // NavigatorTabProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 84);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOK);
            this.Controls.Add(this.ux_textboxTabText);
            this.Controls.Add(this.ux_labelTabText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NavigatorTabProperties";
            this.Text = "NavigatorTabProperties";
            this.Load += new System.EventHandler(this.NavigatorTabProperties_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ux_labelTabText;
        private System.Windows.Forms.TextBox ux_textboxTabText;
        private System.Windows.Forms.Button ux_buttonOK;
        private System.Windows.Forms.Button ux_buttonCancel;
    }
}