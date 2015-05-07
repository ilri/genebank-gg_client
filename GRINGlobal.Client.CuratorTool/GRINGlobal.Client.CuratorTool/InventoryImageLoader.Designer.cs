namespace GRINGlobal.Client
{
    partial class InventoryImageLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryImageLoader));
            this.ux_datagridviewImages = new System.Windows.Forms.DataGridView();
            this.ux_buttonLoad = new System.Windows.Forms.Button();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_progressbarLoading = new System.Windows.Forms.ProgressBar();
            this.ux_labelCommonFilePath = new System.Windows.Forms.Label();
            this.ux_textboxCommonFilePath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewImages)).BeginInit();
            this.SuspendLayout();
            // 
            // ux_datagridviewImages
            // 
            this.ux_datagridviewImages.AllowUserToAddRows = false;
            this.ux_datagridviewImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_datagridviewImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ux_datagridviewImages.Location = new System.Drawing.Point(12, 49);
            this.ux_datagridviewImages.Name = "ux_datagridviewImages";
            this.ux_datagridviewImages.Size = new System.Drawing.Size(377, 187);
            this.ux_datagridviewImages.TabIndex = 0;
            // 
            // ux_buttonLoad
            // 
            this.ux_buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonLoad.Location = new System.Drawing.Point(233, 242);
            this.ux_buttonLoad.Name = "ux_buttonLoad";
            this.ux_buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonLoad.TabIndex = 1;
            this.ux_buttonLoad.Text = "Load";
            this.ux_buttonLoad.UseVisualStyleBackColor = true;
            this.ux_buttonLoad.Click += new System.EventHandler(this.ux_buttonLoad_Click);
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.Location = new System.Drawing.Point(314, 242);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 2;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // ux_progressbarLoading
            // 
            this.ux_progressbarLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_progressbarLoading.Location = new System.Drawing.Point(12, 242);
            this.ux_progressbarLoading.Name = "ux_progressbarLoading";
            this.ux_progressbarLoading.Size = new System.Drawing.Size(215, 23);
            this.ux_progressbarLoading.TabIndex = 3;
            this.ux_progressbarLoading.Visible = false;
            // 
            // ux_labelCommonFilePath
            // 
            this.ux_labelCommonFilePath.AutoSize = true;
            this.ux_labelCommonFilePath.Location = new System.Drawing.Point(12, 7);
            this.ux_labelCommonFilePath.Name = "ux_labelCommonFilePath";
            this.ux_labelCommonFilePath.Size = new System.Drawing.Size(91, 13);
            this.ux_labelCommonFilePath.TabIndex = 4;
            this.ux_labelCommonFilePath.Text = "Common file path:";
            // 
            // ux_textboxCommonFilePath
            // 
            this.ux_textboxCommonFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxCommonFilePath.Location = new System.Drawing.Point(12, 23);
            this.ux_textboxCommonFilePath.Name = "ux_textboxCommonFilePath";
            this.ux_textboxCommonFilePath.Size = new System.Drawing.Size(377, 20);
            this.ux_textboxCommonFilePath.TabIndex = 5;
            this.ux_textboxCommonFilePath.TextChanged += new System.EventHandler(this.ux_textboxCommonFilePath_TextChanged);
            // 
            // InventoryImageLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 277);
            this.Controls.Add(this.ux_textboxCommonFilePath);
            this.Controls.Add(this.ux_labelCommonFilePath);
            this.Controls.Add(this.ux_progressbarLoading);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonLoad);
            this.Controls.Add(this.ux_datagridviewImages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InventoryImageLoader";
            this.Text = "ImageLoader";
            this.Load += new System.EventHandler(this.InventoryImageLoader_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewImages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ux_datagridviewImages;
        private System.Windows.Forms.Button ux_buttonLoad;
        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.ProgressBar ux_progressbarLoading;
        private System.Windows.Forms.Label ux_labelCommonFilePath;
        private System.Windows.Forms.TextBox ux_textboxCommonFilePath;
    }
}