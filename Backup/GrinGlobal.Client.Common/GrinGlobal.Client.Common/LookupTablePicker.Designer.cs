namespace GRINGlobal.Client.Common
{
    partial class LookupTablePicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookupTablePicker));
            this.ux_buttonOK = new System.Windows.Forms.Button();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_listboxLookupDisplayMember = new System.Windows.Forms.ListBox();
            this.ux_textboxFind = new System.Windows.Forms.TextBox();
            this.ux_labelFilter = new System.Windows.Forms.Label();
            this.ux_checkedlistboxFilters = new System.Windows.Forms.CheckedListBox();
            this.ux_labelListMustMatch = new System.Windows.Forms.Label();
            this.ux_buttonRefresh = new System.Windows.Forms.Button();
            this.ux_labelFilterHint = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ux_buttonOK
            // 
            this.ux_buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOK.Location = new System.Drawing.Point(213, 231);
            this.ux_buttonOK.Name = "ux_buttonOK";
            this.ux_buttonOK.Size = new System.Drawing.Size(55, 23);
            this.ux_buttonOK.TabIndex = 2;
            this.ux_buttonOK.Text = "OK";
            this.ux_buttonOK.UseVisualStyleBackColor = true;
            this.ux_buttonOK.Click += new System.EventHandler(this.ux_buttonOK_Click);
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ux_buttonCancel.Location = new System.Drawing.Point(275, 231);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.ux_buttonCancel.TabIndex = 3;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // ux_listboxLookupDisplayMember
            // 
            this.ux_listboxLookupDisplayMember.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_listboxLookupDisplayMember.FormattingEnabled = true;
            this.ux_listboxLookupDisplayMember.IntegralHeight = false;
            this.ux_listboxLookupDisplayMember.Location = new System.Drawing.Point(12, 53);
            this.ux_listboxLookupDisplayMember.Name = "ux_listboxLookupDisplayMember";
            this.ux_listboxLookupDisplayMember.Size = new System.Drawing.Size(192, 172);
            this.ux_listboxLookupDisplayMember.TabIndex = 1;
            this.ux_listboxLookupDisplayMember.DoubleClick += new System.EventHandler(this.ux_listboxLookupDisplayMember_DoubleClick);
            // 
            // ux_textboxFind
            // 
            this.ux_textboxFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxFind.Location = new System.Drawing.Point(59, 27);
            this.ux_textboxFind.Name = "ux_textboxFind";
            this.ux_textboxFind.Size = new System.Drawing.Size(271, 20);
            this.ux_textboxFind.TabIndex = 0;
            this.ux_textboxFind.TextChanged += new System.EventHandler(this.ux_textboxFind_TextChanged);
            // 
            // ux_labelFilter
            // 
            this.ux_labelFilter.AutoSize = true;
            this.ux_labelFilter.Location = new System.Drawing.Point(12, 30);
            this.ux_labelFilter.Name = "ux_labelFilter";
            this.ux_labelFilter.Size = new System.Drawing.Size(41, 13);
            this.ux_labelFilter.TabIndex = 4;
            this.ux_labelFilter.Text = "Filter ->";
            // 
            // ux_checkedlistboxFilters
            // 
            this.ux_checkedlistboxFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_checkedlistboxFilters.CheckOnClick = true;
            this.ux_checkedlistboxFilters.FormattingEnabled = true;
            this.ux_checkedlistboxFilters.IntegralHeight = false;
            this.ux_checkedlistboxFilters.Location = new System.Drawing.Point(210, 88);
            this.ux_checkedlistboxFilters.Name = "ux_checkedlistboxFilters";
            this.ux_checkedlistboxFilters.Size = new System.Drawing.Size(120, 137);
            this.ux_checkedlistboxFilters.TabIndex = 4;
            this.ux_checkedlistboxFilters.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ux_checkedlistboxFilters_ItemCheck);
            // 
            // ux_labelListMustMatch
            // 
            this.ux_labelListMustMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelListMustMatch.Location = new System.Drawing.Point(210, 53);
            this.ux_labelListMustMatch.Name = "ux_labelListMustMatch";
            this.ux_labelListMustMatch.Size = new System.Drawing.Size(119, 32);
            this.ux_labelListMustMatch.TabIndex = 6;
            this.ux_labelListMustMatch.Text = "Show Only Choices Valid For This:";
            // 
            // ux_buttonRefresh
            // 
            this.ux_buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ux_buttonRefresh.Location = new System.Drawing.Point(12, 231);
            this.ux_buttonRefresh.Name = "ux_buttonRefresh";
            this.ux_buttonRefresh.Size = new System.Drawing.Size(79, 23);
            this.ux_buttonRefresh.TabIndex = 7;
            this.ux_buttonRefresh.Text = "Refresh List";
            this.ux_buttonRefresh.UseVisualStyleBackColor = true;
            this.ux_buttonRefresh.Click += new System.EventHandler(this.ux_buttonRefresh_Click);
            // 
            // ux_labelFilterHint
            // 
            this.ux_labelFilterHint.AutoSize = true;
            this.ux_labelFilterHint.Location = new System.Drawing.Point(12, 9);
            this.ux_labelFilterHint.Name = "ux_labelFilterHint";
            this.ux_labelFilterHint.Size = new System.Drawing.Size(295, 13);
            this.ux_labelFilterHint.TabIndex = 8;
            this.ux_labelFilterHint.Text = "HINT: For big lists, use the text filter to shorten the list search.";
            // 
            // LookupTablePicker
            // 
            this.AcceptButton = this.ux_buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ux_buttonCancel;
            this.ClientSize = new System.Drawing.Size(342, 266);
            this.Controls.Add(this.ux_labelFilterHint);
            this.Controls.Add(this.ux_buttonRefresh);
            this.Controls.Add(this.ux_labelListMustMatch);
            this.Controls.Add(this.ux_checkedlistboxFilters);
            this.Controls.Add(this.ux_labelFilter);
            this.Controls.Add(this.ux_textboxFind);
            this.Controls.Add(this.ux_listboxLookupDisplayMember);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LookupTablePicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LookupPicker";
            this.Load += new System.EventHandler(this.LookupPicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ux_buttonOK;
        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.ListBox ux_listboxLookupDisplayMember;
        private System.Windows.Forms.TextBox ux_textboxFind;
        private System.Windows.Forms.Label ux_labelFilter;
        private System.Windows.Forms.CheckedListBox ux_checkedlistboxFilters;
        private System.Windows.Forms.Label ux_labelListMustMatch;
        private System.Windows.Forms.Button ux_buttonRefresh;
        private System.Windows.Forms.Label ux_labelFilterHint;
    }
}