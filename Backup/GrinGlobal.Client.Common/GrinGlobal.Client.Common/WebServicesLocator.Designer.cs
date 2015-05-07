namespace GRINGlobal.Client.Common
{
    partial class WebServicesLocator
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
            this.ux_textboxListName = new System.Windows.Forms.TextBox();
            this.ux_textboxServerHint = new System.Windows.Forms.TextBox();
            this.ux_groupboxServerName = new System.Windows.Forms.GroupBox();
            this.ux_labelServerName = new System.Windows.Forms.Label();
            this.ux_textboxServerName = new System.Windows.Forms.TextBox();
            this.ux_lableListName = new System.Windows.Forms.Label();
            this.ux_buttonTestServer = new System.Windows.Forms.Button();
            this.ux_groupboxListName = new System.Windows.Forms.GroupBox();
            this.ux_buttonDeleteListItem = new System.Windows.Forms.Button();
            this.ux_buttonMoveDown = new System.Windows.Forms.Button();
            this.ux_buttonMoveUp = new System.Windows.Forms.Button();
            this.ux_listboxWebserviceURLNames = new System.Windows.Forms.ListBox();
            this.ux_buttonAddNewListItem = new System.Windows.Forms.Button();
            this.ux_buttonOK = new System.Windows.Forms.Button();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_groupboxServerName.SuspendLayout();
            this.ux_groupboxListName.SuspendLayout();
            this.SuspendLayout();
            // 
            // ux_textboxListName
            // 
            this.ux_textboxListName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxListName.Location = new System.Drawing.Point(6, 36);
            this.ux_textboxListName.Name = "ux_textboxListName";
            this.ux_textboxListName.Size = new System.Drawing.Size(197, 20);
            this.ux_textboxListName.TabIndex = 3;
            this.ux_textboxListName.TextChanged += new System.EventHandler(this.ux_textboxListName_TextChanged);
            // 
            // ux_textboxServerHint
            // 
            this.ux_textboxServerHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxServerHint.Enabled = false;
            this.ux_textboxServerHint.Location = new System.Drawing.Point(6, 104);
            this.ux_textboxServerHint.Multiline = true;
            this.ux_textboxServerHint.Name = "ux_textboxServerHint";
            this.ux_textboxServerHint.Size = new System.Drawing.Size(197, 78);
            this.ux_textboxServerHint.TabIndex = 6;
            this.ux_textboxServerHint.Text = "Examples:\r\n  grin-global-test1.agron.iastate.edu\r\n  ncrpis-arwen.agron.iastate.ed" +
                "u\r\n  129.186.234.51\r\n  129.186.234.4\r\n";
            // 
            // ux_groupboxServerName
            // 
            this.ux_groupboxServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_groupboxServerName.Controls.Add(this.ux_labelServerName);
            this.ux_groupboxServerName.Controls.Add(this.ux_textboxServerName);
            this.ux_groupboxServerName.Controls.Add(this.ux_lableListName);
            this.ux_groupboxServerName.Controls.Add(this.ux_buttonTestServer);
            this.ux_groupboxServerName.Controls.Add(this.ux_textboxServerHint);
            this.ux_groupboxServerName.Controls.Add(this.ux_textboxListName);
            this.ux_groupboxServerName.Location = new System.Drawing.Point(280, 12);
            this.ux_groupboxServerName.Name = "ux_groupboxServerName";
            this.ux_groupboxServerName.Size = new System.Drawing.Size(209, 225);
            this.ux_groupboxServerName.TabIndex = 2;
            this.ux_groupboxServerName.TabStop = false;
            this.ux_groupboxServerName.Text = "Properties";
            // 
            // ux_labelServerName
            // 
            this.ux_labelServerName.AutoSize = true;
            this.ux_labelServerName.Location = new System.Drawing.Point(7, 62);
            this.ux_labelServerName.Name = "ux_labelServerName";
            this.ux_labelServerName.Size = new System.Drawing.Size(144, 13);
            this.ux_labelServerName.TabIndex = 9;
            this.ux_labelServerName.Text = "Server Name (or IP Address):";
            // 
            // ux_textboxServerName
            // 
            this.ux_textboxServerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxServerName.Location = new System.Drawing.Point(6, 78);
            this.ux_textboxServerName.Name = "ux_textboxServerName";
            this.ux_textboxServerName.Size = new System.Drawing.Size(197, 20);
            this.ux_textboxServerName.TabIndex = 8;
            // 
            // ux_lableListName
            // 
            this.ux_lableListName.AutoSize = true;
            this.ux_lableListName.Location = new System.Drawing.Point(7, 20);
            this.ux_lableListName.Name = "ux_lableListName";
            this.ux_lableListName.Size = new System.Drawing.Size(38, 13);
            this.ux_lableListName.TabIndex = 7;
            this.ux_lableListName.Text = "Name:";
            // 
            // ux_buttonTestServer
            // 
            this.ux_buttonTestServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonTestServer.Location = new System.Drawing.Point(41, 192);
            this.ux_buttonTestServer.Name = "ux_buttonTestServer";
            this.ux_buttonTestServer.Size = new System.Drawing.Size(133, 23);
            this.ux_buttonTestServer.TabIndex = 6;
            this.ux_buttonTestServer.Text = "Test Server Address";
            this.ux_buttonTestServer.UseVisualStyleBackColor = true;
            this.ux_buttonTestServer.Click += new System.EventHandler(this.ux_buttonTestServer_Click);
            // 
            // ux_groupboxListName
            // 
            this.ux_groupboxListName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_groupboxListName.Controls.Add(this.ux_buttonDeleteListItem);
            this.ux_groupboxListName.Controls.Add(this.ux_buttonMoveDown);
            this.ux_groupboxListName.Controls.Add(this.ux_buttonMoveUp);
            this.ux_groupboxListName.Controls.Add(this.ux_listboxWebserviceURLNames);
            this.ux_groupboxListName.Controls.Add(this.ux_buttonAddNewListItem);
            this.ux_groupboxListName.Location = new System.Drawing.Point(12, 12);
            this.ux_groupboxListName.Name = "ux_groupboxListName";
            this.ux_groupboxListName.Size = new System.Drawing.Size(262, 225);
            this.ux_groupboxListName.TabIndex = 0;
            this.ux_groupboxListName.TabStop = false;
            this.ux_groupboxListName.Text = "List (Friendly) Name";
            // 
            // ux_buttonDeleteListItem
            // 
            this.ux_buttonDeleteListItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonDeleteListItem.Location = new System.Drawing.Point(100, 194);
            this.ux_buttonDeleteListItem.Name = "ux_buttonDeleteListItem";
            this.ux_buttonDeleteListItem.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonDeleteListItem.TabIndex = 11;
            this.ux_buttonDeleteListItem.Text = "Delete";
            this.ux_buttonDeleteListItem.UseVisualStyleBackColor = true;
            this.ux_buttonDeleteListItem.Click += new System.EventHandler(this.ux_buttonDeleteListItem_Click);
            // 
            // ux_buttonMoveDown
            // 
            this.ux_buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonMoveDown.Location = new System.Drawing.Point(181, 135);
            this.ux_buttonMoveDown.Name = "ux_buttonMoveDown";
            this.ux_buttonMoveDown.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonMoveDown.TabIndex = 10;
            this.ux_buttonMoveDown.Text = "Move Down";
            this.ux_buttonMoveDown.UseVisualStyleBackColor = true;
            this.ux_buttonMoveDown.Click += new System.EventHandler(this.ux_buttonMoveDown_Click);
            // 
            // ux_buttonMoveUp
            // 
            this.ux_buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonMoveUp.Location = new System.Drawing.Point(181, 47);
            this.ux_buttonMoveUp.Name = "ux_buttonMoveUp";
            this.ux_buttonMoveUp.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonMoveUp.TabIndex = 9;
            this.ux_buttonMoveUp.Text = "Move Up";
            this.ux_buttonMoveUp.UseVisualStyleBackColor = true;
            this.ux_buttonMoveUp.Click += new System.EventHandler(this.ux_buttonMoveUp_Click);
            // 
            // ux_listboxWebserviceURLNames
            // 
            this.ux_listboxWebserviceURLNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_listboxWebserviceURLNames.FormattingEnabled = true;
            this.ux_listboxWebserviceURLNames.Location = new System.Drawing.Point(6, 22);
            this.ux_listboxWebserviceURLNames.Name = "ux_listboxWebserviceURLNames";
            this.ux_listboxWebserviceURLNames.ScrollAlwaysVisible = true;
            this.ux_listboxWebserviceURLNames.Size = new System.Drawing.Size(169, 160);
            this.ux_listboxWebserviceURLNames.TabIndex = 8;
            // 
            // ux_buttonAddNewListItem
            // 
            this.ux_buttonAddNewListItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ux_buttonAddNewListItem.Location = new System.Drawing.Point(6, 194);
            this.ux_buttonAddNewListItem.Name = "ux_buttonAddNewListItem";
            this.ux_buttonAddNewListItem.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonAddNewListItem.TabIndex = 7;
            this.ux_buttonAddNewListItem.Text = "Add New...";
            this.ux_buttonAddNewListItem.UseVisualStyleBackColor = true;
            this.ux_buttonAddNewListItem.Click += new System.EventHandler(this.ux_buttonAddNewListItem_Click);
            // 
            // ux_buttonOK
            // 
            this.ux_buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ux_buttonOK.Location = new System.Drawing.Point(327, 243);
            this.ux_buttonOK.Name = "ux_buttonOK";
            this.ux_buttonOK.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonOK.TabIndex = 4;
            this.ux_buttonOK.Text = "OK";
            this.ux_buttonOK.UseVisualStyleBackColor = true;
            this.ux_buttonOK.Click += new System.EventHandler(this.ux_buttonOK_Click);
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ux_buttonCancel.Location = new System.Drawing.Point(408, 243);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 5;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // WebServicesLocator
            // 
            this.AcceptButton = this.ux_buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ux_buttonCancel;
            this.ClientSize = new System.Drawing.Size(501, 271);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOK);
            this.Controls.Add(this.ux_groupboxListName);
            this.Controls.Add(this.ux_groupboxServerName);
            this.Name = "WebServicesLocator";
            this.Text = "Web Services List Editor";
            this.ux_groupboxServerName.ResumeLayout(false);
            this.ux_groupboxServerName.PerformLayout();
            this.ux_groupboxListName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox ux_textboxListName;
        private System.Windows.Forms.TextBox ux_textboxServerHint;
        private System.Windows.Forms.GroupBox ux_groupboxServerName;
        private System.Windows.Forms.GroupBox ux_groupboxListName;
        private System.Windows.Forms.Button ux_buttonOK;
        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.Button ux_buttonTestServer;
        private System.Windows.Forms.Button ux_buttonAddNewListItem;
        private System.Windows.Forms.ListBox ux_listboxWebserviceURLNames;
        private System.Windows.Forms.Button ux_buttonMoveDown;
        private System.Windows.Forms.Button ux_buttonMoveUp;
        private System.Windows.Forms.Label ux_labelServerName;
        private System.Windows.Forms.TextBox ux_textboxServerName;
        private System.Windows.Forms.Label ux_lableListName;
        private System.Windows.Forms.Button ux_buttonDeleteListItem;
    }
}