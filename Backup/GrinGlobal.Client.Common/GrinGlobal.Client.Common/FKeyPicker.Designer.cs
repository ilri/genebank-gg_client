namespace GRINGlobal.Client.Common
{
    partial class FKeyPicker
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ux_datagridviewInventory = new System.Windows.Forms.DataGridView();
            this.ux_buttonOK = new System.Windows.Forms.Button();
            this.ux_buttonCancel = new System.Windows.Forms.Button();
            this.ux_labelAccessionNumber = new System.Windows.Forms.Label();
            this.ux_textboxAccessionNumber = new System.Windows.Forms.TextBox();
            this.ux_textboxAccessionName = new System.Windows.Forms.TextBox();
            this.ux_labelAccessionName = new System.Windows.Forms.Label();
            this.ux_textboxTaxonomy = new System.Windows.Forms.TextBox();
            this.ux_labelTaxonomy = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewInventory)).BeginInit();
            this.SuspendLayout();
            // 
            // ux_datagridviewInventory
            // 
            this.ux_datagridviewInventory.AllowUserToOrderColumns = true;
            this.ux_datagridviewInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ux_datagridviewInventory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.ux_datagridviewInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ux_datagridviewInventory.DefaultCellStyle = dataGridViewCellStyle8;
            this.ux_datagridviewInventory.Location = new System.Drawing.Point(3, 88);
            this.ux_datagridviewInventory.MultiSelect = false;
            this.ux_datagridviewInventory.Name = "ux_datagridviewInventory";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ux_datagridviewInventory.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.ux_datagridviewInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ux_datagridviewInventory.Size = new System.Drawing.Size(775, 200);
            this.ux_datagridviewInventory.TabIndex = 0;
            this.ux_datagridviewInventory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ux_datagridviewInventory_KeyDown);
            // 
            // ux_buttonOK
            // 
            this.ux_buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonOK.Location = new System.Drawing.Point(613, 294);
            this.ux_buttonOK.Name = "ux_buttonOK";
            this.ux_buttonOK.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonOK.TabIndex = 1;
            this.ux_buttonOK.Text = "OK";
            this.ux_buttonOK.UseVisualStyleBackColor = true;
            this.ux_buttonOK.Click += new System.EventHandler(this.ux_buttonOK_Click);
            // 
            // ux_buttonCancel
            // 
            this.ux_buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ux_buttonCancel.Location = new System.Drawing.Point(694, 294);
            this.ux_buttonCancel.Name = "ux_buttonCancel";
            this.ux_buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.ux_buttonCancel.TabIndex = 2;
            this.ux_buttonCancel.Text = "Cancel";
            this.ux_buttonCancel.UseVisualStyleBackColor = true;
            this.ux_buttonCancel.Click += new System.EventHandler(this.ux_buttonCancel_Click);
            // 
            // ux_labelAccessionNumber
            // 
            this.ux_labelAccessionNumber.AutoSize = true;
            this.ux_labelAccessionNumber.Location = new System.Drawing.Point(3, 13);
            this.ux_labelAccessionNumber.Name = "ux_labelAccessionNumber";
            this.ux_labelAccessionNumber.Size = new System.Drawing.Size(99, 13);
            this.ux_labelAccessionNumber.TabIndex = 3;
            this.ux_labelAccessionNumber.Text = "Accession Number:";
            // 
            // ux_textboxAccessionNumber
            // 
            this.ux_textboxAccessionNumber.Location = new System.Drawing.Point(108, 10);
            this.ux_textboxAccessionNumber.Name = "ux_textboxAccessionNumber";
            this.ux_textboxAccessionNumber.Size = new System.Drawing.Size(191, 20);
            this.ux_textboxAccessionNumber.TabIndex = 4;
            this.ux_textboxAccessionNumber.TextChanged += new System.EventHandler(this.ux_textboxAccessionNumber_TextChanged);
            // 
            // ux_textboxAccessionName
            // 
            this.ux_textboxAccessionName.Location = new System.Drawing.Point(108, 36);
            this.ux_textboxAccessionName.Name = "ux_textboxAccessionName";
            this.ux_textboxAccessionName.Size = new System.Drawing.Size(191, 20);
            this.ux_textboxAccessionName.TabIndex = 6;
            this.ux_textboxAccessionName.TextChanged += new System.EventHandler(this.ux_textboxAccessionName_TextChanged);
            // 
            // ux_labelAccessionName
            // 
            this.ux_labelAccessionName.AutoSize = true;
            this.ux_labelAccessionName.Location = new System.Drawing.Point(3, 39);
            this.ux_labelAccessionName.Name = "ux_labelAccessionName";
            this.ux_labelAccessionName.Size = new System.Drawing.Size(90, 13);
            this.ux_labelAccessionName.TabIndex = 5;
            this.ux_labelAccessionName.Text = "Accession Name:";
            // 
            // ux_textboxTaxonomy
            // 
            this.ux_textboxTaxonomy.Location = new System.Drawing.Point(108, 62);
            this.ux_textboxTaxonomy.Name = "ux_textboxTaxonomy";
            this.ux_textboxTaxonomy.Size = new System.Drawing.Size(191, 20);
            this.ux_textboxTaxonomy.TabIndex = 8;
            this.ux_textboxTaxonomy.TextChanged += new System.EventHandler(this.ux_textboxTaxonomy_TextChanged);
            // 
            // ux_labelTaxonomy
            // 
            this.ux_labelTaxonomy.AutoSize = true;
            this.ux_labelTaxonomy.Location = new System.Drawing.Point(3, 65);
            this.ux_labelTaxonomy.Name = "ux_labelTaxonomy";
            this.ux_labelTaxonomy.Size = new System.Drawing.Size(59, 13);
            this.ux_labelTaxonomy.TabIndex = 7;
            this.ux_labelTaxonomy.Text = "Taxonomy:";
            // 
            // FKeyPicker
            // 
            this.AcceptButton = this.ux_buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ux_buttonCancel;
            this.ClientSize = new System.Drawing.Size(781, 320);
            this.Controls.Add(this.ux_textboxTaxonomy);
            this.Controls.Add(this.ux_labelTaxonomy);
            this.Controls.Add(this.ux_textboxAccessionName);
            this.Controls.Add(this.ux_labelAccessionName);
            this.Controls.Add(this.ux_textboxAccessionNumber);
            this.Controls.Add(this.ux_labelAccessionNumber);
            this.Controls.Add(this.ux_buttonCancel);
            this.Controls.Add(this.ux_buttonOK);
            this.Controls.Add(this.ux_datagridviewInventory);
            this.Name = "FKeyPicker";
            this.Text = "Inventory Picker";
            ((System.ComponentModel.ISupportInitialize)(this.ux_datagridviewInventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView ux_datagridviewInventory;
        private System.Windows.Forms.Button ux_buttonOK;
        private System.Windows.Forms.Button ux_buttonCancel;
        private System.Windows.Forms.Label ux_labelAccessionNumber;
        private System.Windows.Forms.TextBox ux_textboxAccessionNumber;
        private System.Windows.Forms.TextBox ux_textboxAccessionName;
        private System.Windows.Forms.Label ux_labelAccessionName;
        private System.Windows.Forms.TextBox ux_textboxTaxonomy;
        private System.Windows.Forms.Label ux_labelTaxonomy;
    }
}