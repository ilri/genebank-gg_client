namespace CooperatorWizard
{
    partial class CooperatorWizard
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
            this.ux_tabcontrolMain = new System.Windows.Forms.TabControl();
            this.CooperatorPage = new System.Windows.Forms.TabPage();
            this.ux_buttonNewCooperator = new System.Windows.Forms.Button();
            this.ux_textboxFirstName = new System.Windows.Forms.TextBox();
            this.ux_textboxLastName = new System.Windows.Forms.TextBox();
            this.ux_labelSearch = new System.Windows.Forms.Label();
            this.ux_labelFirstName = new System.Windows.Forms.Label();
            this.ux_labelLastName = new System.Windows.Forms.Label();
            this.ux_datagridviewCooperator = new System.Windows.Forms.DataGridView();
            this.WebCooperatorPage = new System.Windows.Forms.TabPage();
            this.ux_buttonCreateNewCooperator = new System.Windows.Forms.Button();
            this.ux_textboxWebFirstName = new System.Windows.Forms.TextBox();
            this.ux_textboxWebLastName = new System.Windows.Forms.TextBox();
            this.ux_labelWebSearch = new System.Windows.Forms.Label();
            this.ux_labelWebFirstName = new System.Windows.Forms.Label();
            this.ux_labelWebLastName = new System.Windows.Forms.Label();
            this.ux_datagridviewWebCooperator = new System.Windows.Forms.DataGridView();
            this.ux_buttonSave = new System.Windows.Forms.Button();
            this.ux_buttonSaveAndExit = new System.Windows.Forms.Button();
            this.ux_tabcontrolMain.SuspendLayout();
            this.CooperatorPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewCooperator)).BeginInit();
            this.WebCooperatorPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewWebCooperator)).BeginInit();
            this.SuspendLayout();
            // 
            // ux_tabcontrolMain
            // 
            this.ux_tabcontrolMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_tabcontrolMain.Controls.Add(this.CooperatorPage);
            this.ux_tabcontrolMain.Controls.Add(this.WebCooperatorPage);
            this.ux_tabcontrolMain.Location = new System.Drawing.Point(3, 34);
            this.ux_tabcontrolMain.Name = "ux_tabcontrolMain";
            this.ux_tabcontrolMain.SelectedIndex = 0;
            this.ux_tabcontrolMain.Size = new System.Drawing.Size(776, 272);
            this.ux_tabcontrolMain.TabIndex = 0;
            // 
            // CooperatorPage
            // 
            this.CooperatorPage.Controls.Add(this.ux_buttonNewCooperator);
            this.CooperatorPage.Controls.Add(this.ux_textboxFirstName);
            this.CooperatorPage.Controls.Add(this.ux_textboxLastName);
            this.CooperatorPage.Controls.Add(this.ux_labelSearch);
            this.CooperatorPage.Controls.Add(this.ux_labelFirstName);
            this.CooperatorPage.Controls.Add(this.ux_labelLastName);
            this.CooperatorPage.Controls.Add(this.ux_datagridviewCooperator);
            this.CooperatorPage.Location = new System.Drawing.Point(4, 22);
            this.CooperatorPage.Name = "CooperatorPage";
            this.CooperatorPage.Padding = new System.Windows.Forms.Padding(3);
            this.CooperatorPage.Size = new System.Drawing.Size(768, 246);
            this.CooperatorPage.TabIndex = 0;
            this.CooperatorPage.Text = "Cooperator";
            this.CooperatorPage.UseVisualStyleBackColor = true;
            // 
            // ux_buttonNewCooperator
            // 
            this.ux_buttonNewCooperator.Location = new System.Drawing.Point(6, 20);
            this.ux_buttonNewCooperator.Name = "ux_buttonNewCooperator";
            this.ux_buttonNewCooperator.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonNewCooperator.TabIndex = 6;
            this.ux_buttonNewCooperator.Text = "New...";
            this.ux_buttonNewCooperator.UseVisualStyleBackColor = true;
            this.ux_buttonNewCooperator.Click += new System.EventHandler(this.ux_buttonNewCooperator_Click);
            // 
            // ux_textboxFirstName
            // 
            this.ux_textboxFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxFirstName.Location = new System.Drawing.Point(662, 23);
            this.ux_textboxFirstName.Name = "ux_textboxFirstName";
            this.ux_textboxFirstName.Size = new System.Drawing.Size(100, 20);
            this.ux_textboxFirstName.TabIndex = 5;
            this.ux_textboxFirstName.TextChanged += new System.EventHandler(this.ux_textboxNameFilter_TextChanged);
            // 
            // ux_textboxLastName
            // 
            this.ux_textboxLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxLastName.Location = new System.Drawing.Point(556, 23);
            this.ux_textboxLastName.Name = "ux_textboxLastName";
            this.ux_textboxLastName.Size = new System.Drawing.Size(100, 20);
            this.ux_textboxLastName.TabIndex = 4;
            this.ux_textboxLastName.TextChanged += new System.EventHandler(this.ux_textboxNameFilter_TextChanged);
            // 
            // ux_labelSearch
            // 
            this.ux_labelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelSearch.AutoSize = true;
            this.ux_labelSearch.Location = new System.Drawing.Point(506, 26);
            this.ux_labelSearch.Name = "ux_labelSearch";
            this.ux_labelSearch.Size = new System.Drawing.Size(44, 13);
            this.ux_labelSearch.TabIndex = 3;
            this.ux_labelSearch.Text = "Search:";
            // 
            // ux_labelFirstName
            // 
            this.ux_labelFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelFirstName.AutoSize = true;
            this.ux_labelFirstName.Location = new System.Drawing.Point(659, 7);
            this.ux_labelFirstName.Name = "ux_labelFirstName";
            this.ux_labelFirstName.Size = new System.Drawing.Size(57, 13);
            this.ux_labelFirstName.TabIndex = 2;
            this.ux_labelFirstName.Text = "First Name";
            // 
            // ux_labelLastName
            // 
            this.ux_labelLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelLastName.AutoSize = true;
            this.ux_labelLastName.Location = new System.Drawing.Point(553, 7);
            this.ux_labelLastName.Name = "ux_labelLastName";
            this.ux_labelLastName.Size = new System.Drawing.Size(58, 13);
            this.ux_labelLastName.TabIndex = 1;
            this.ux_labelLastName.Text = "Last Name";
            // 
            // ux_datagridviewCooperator
            // 
            this.ux_datagridviewCooperator.AllowUserToAddRows = false;
            this.ux_datagridviewCooperator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_datagridviewCooperator.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ux_datagridviewCooperator.Location = new System.Drawing.Point(0, 49);
            this.ux_datagridviewCooperator.Name = "ux_datagridviewCooperator";
            this.ux_datagridviewCooperator.Size = new System.Drawing.Size(768, 197);
            this.ux_datagridviewCooperator.TabIndex = 0;
            this.ux_datagridviewCooperator.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ux_datagridview_CellFormatting);
            this.ux_datagridviewCooperator.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.ux_datagridview_EditingControlShowing);
            this.ux_datagridviewCooperator.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.ux_datagridview_DataError);
            this.ux_datagridviewCooperator.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ux_datagridview_KeyDown);
            // 
            // WebCooperatorPage
            // 
            this.WebCooperatorPage.Controls.Add(this.ux_buttonCreateNewCooperator);
            this.WebCooperatorPage.Controls.Add(this.ux_textboxWebFirstName);
            this.WebCooperatorPage.Controls.Add(this.ux_textboxWebLastName);
            this.WebCooperatorPage.Controls.Add(this.ux_labelWebSearch);
            this.WebCooperatorPage.Controls.Add(this.ux_labelWebFirstName);
            this.WebCooperatorPage.Controls.Add(this.ux_labelWebLastName);
            this.WebCooperatorPage.Controls.Add(this.ux_datagridviewWebCooperator);
            this.WebCooperatorPage.Location = new System.Drawing.Point(4, 22);
            this.WebCooperatorPage.Name = "WebCooperatorPage";
            this.WebCooperatorPage.Padding = new System.Windows.Forms.Padding(3);
            this.WebCooperatorPage.Size = new System.Drawing.Size(768, 246);
            this.WebCooperatorPage.TabIndex = 1;
            this.WebCooperatorPage.Text = "Web Cooperator";
            this.WebCooperatorPage.UseVisualStyleBackColor = true;
            // 
            // ux_buttonCreateNewCooperator
            // 
            this.ux_buttonCreateNewCooperator.Location = new System.Drawing.Point(6, 17);
            this.ux_buttonCreateNewCooperator.Name = "ux_buttonCreateNewCooperator";
            this.ux_buttonCreateNewCooperator.Size = new System.Drawing.Size(230, 23);
            this.ux_buttonCreateNewCooperator.TabIndex = 13;
            this.ux_buttonCreateNewCooperator.Text = "Create New Cooperator from Selection";
            this.ux_buttonCreateNewCooperator.UseVisualStyleBackColor = true;
            this.ux_buttonCreateNewCooperator.Click += new System.EventHandler(this.ux_buttonCreateNewCooperator_Click);
            // 
            // ux_textboxWebFirstName
            // 
            this.ux_textboxWebFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxWebFirstName.Location = new System.Drawing.Point(662, 20);
            this.ux_textboxWebFirstName.Name = "ux_textboxWebFirstName";
            this.ux_textboxWebFirstName.Size = new System.Drawing.Size(100, 20);
            this.ux_textboxWebFirstName.TabIndex = 12;
            this.ux_textboxWebFirstName.TextChanged += new System.EventHandler(this.ux_textboxNameFilter_TextChanged);
            // 
            // ux_textboxWebLastName
            // 
            this.ux_textboxWebLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxWebLastName.Location = new System.Drawing.Point(556, 20);
            this.ux_textboxWebLastName.Name = "ux_textboxWebLastName";
            this.ux_textboxWebLastName.Size = new System.Drawing.Size(100, 20);
            this.ux_textboxWebLastName.TabIndex = 11;
            this.ux_textboxWebLastName.TextChanged += new System.EventHandler(this.ux_textboxNameFilter_TextChanged);
            // 
            // ux_labelWebSearch
            // 
            this.ux_labelWebSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelWebSearch.AutoSize = true;
            this.ux_labelWebSearch.Location = new System.Drawing.Point(506, 23);
            this.ux_labelWebSearch.Name = "ux_labelWebSearch";
            this.ux_labelWebSearch.Size = new System.Drawing.Size(44, 13);
            this.ux_labelWebSearch.TabIndex = 10;
            this.ux_labelWebSearch.Text = "Search:";
            // 
            // ux_labelWebFirstName
            // 
            this.ux_labelWebFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelWebFirstName.AutoSize = true;
            this.ux_labelWebFirstName.Location = new System.Drawing.Point(659, 4);
            this.ux_labelWebFirstName.Name = "ux_labelWebFirstName";
            this.ux_labelWebFirstName.Size = new System.Drawing.Size(57, 13);
            this.ux_labelWebFirstName.TabIndex = 9;
            this.ux_labelWebFirstName.Text = "First Name";
            // 
            // ux_labelWebLastName
            // 
            this.ux_labelWebLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelWebLastName.AutoSize = true;
            this.ux_labelWebLastName.Location = new System.Drawing.Point(553, 4);
            this.ux_labelWebLastName.Name = "ux_labelWebLastName";
            this.ux_labelWebLastName.Size = new System.Drawing.Size(58, 13);
            this.ux_labelWebLastName.TabIndex = 8;
            this.ux_labelWebLastName.Text = "Last Name";
            // 
            // ux_datagridviewWebCooperator
            // 
            this.ux_datagridviewWebCooperator.AllowUserToAddRows = false;
            this.ux_datagridviewWebCooperator.AllowUserToDeleteRows = false;
            this.ux_datagridviewWebCooperator.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_datagridviewWebCooperator.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ux_datagridviewWebCooperator.Location = new System.Drawing.Point(0, 46);
            this.ux_datagridviewWebCooperator.Name = "ux_datagridviewWebCooperator";
            this.ux_datagridviewWebCooperator.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ux_datagridviewWebCooperator.Size = new System.Drawing.Size(768, 197);
            this.ux_datagridviewWebCooperator.TabIndex = 7;
            // 
            // ux_buttonSave
            // 
            this.ux_buttonSave.Location = new System.Drawing.Point(553, 5);
            this.ux_buttonSave.Name = "ux_buttonSave";
            this.ux_buttonSave.Size = new System.Drawing.Size(110, 23);
            this.ux_buttonSave.TabIndex = 1;
            this.ux_buttonSave.Text = "Save";
            this.ux_buttonSave.UseVisualStyleBackColor = true;
            this.ux_buttonSave.Click += new System.EventHandler(this.ux_buttonSave_Click);
            // 
            // ux_buttonSaveAndExit
            // 
            this.ux_buttonSaveAndExit.Location = new System.Drawing.Point(669, 5);
            this.ux_buttonSaveAndExit.Name = "ux_buttonSaveAndExit";
            this.ux_buttonSaveAndExit.Size = new System.Drawing.Size(110, 23);
            this.ux_buttonSaveAndExit.TabIndex = 2;
            this.ux_buttonSaveAndExit.Text = "Save and Exit";
            this.ux_buttonSaveAndExit.UseVisualStyleBackColor = true;
            this.ux_buttonSaveAndExit.Click += new System.EventHandler(this.ux_buttonSaveAndExit_Click);
            // 
            // CooperatorWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 306);
            this.Controls.Add(this.ux_buttonSaveAndExit);
            this.Controls.Add(this.ux_buttonSave);
            this.Controls.Add(this.ux_tabcontrolMain);
            this.Name = "CooperatorWizard";
            this.Text = "Cooperator Wizard";
            this.Load += new System.EventHandler(this.CooperatorWizard_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CooperatorWizard_FormClosing);
            this.ux_tabcontrolMain.ResumeLayout(false);
            this.CooperatorPage.ResumeLayout(false);
            this.CooperatorPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewCooperator)).EndInit();
            this.WebCooperatorPage.ResumeLayout(false);
            this.WebCooperatorPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewWebCooperator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ux_tabcontrolMain;
        private System.Windows.Forms.TabPage CooperatorPage;
        private System.Windows.Forms.DataGridView ux_datagridviewCooperator;
        private System.Windows.Forms.TabPage WebCooperatorPage;
        private System.Windows.Forms.Button ux_buttonNewCooperator;
        private System.Windows.Forms.TextBox ux_textboxFirstName;
        private System.Windows.Forms.TextBox ux_textboxLastName;
        private System.Windows.Forms.Label ux_labelSearch;
        private System.Windows.Forms.Label ux_labelFirstName;
        private System.Windows.Forms.Label ux_labelLastName;
        private System.Windows.Forms.Button ux_buttonSave;
        private System.Windows.Forms.Button ux_buttonSaveAndExit;
        private System.Windows.Forms.Button ux_buttonCreateNewCooperator;
        private System.Windows.Forms.TextBox ux_textboxWebFirstName;
        private System.Windows.Forms.TextBox ux_textboxWebLastName;
        private System.Windows.Forms.Label ux_labelWebSearch;
        private System.Windows.Forms.Label ux_labelWebFirstName;
        private System.Windows.Forms.Label ux_labelWebLastName;
        private System.Windows.Forms.DataGridView ux_datagridviewWebCooperator;
    }
}

