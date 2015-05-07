namespace AccessionForm
{
    partial class AccessionForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccessionForm));
            this.ux_labelPrefix = new System.Windows.Forms.Label();
            this.ux_labelNumber = new System.Windows.Forms.Label();
            this.ux_textboxNumber = new System.Windows.Forms.TextBox();
            this.ux_labelSuffix = new System.Windows.Forms.Label();
            this.ux_textboxSuffix = new System.Windows.Forms.TextBox();
            this.ux_groupbox1 = new System.Windows.Forms.GroupBox();
            this.ux_textboxReceivedDate = new System.Windows.Forms.TextBox();
            this.ux_comboboxReceivedDatePrecision = new System.Windows.Forms.ComboBox();
            this.ux_textboxPrefix = new System.Windows.Forms.TextBox();
            this.ux_labelReceivedDatePrecision = new System.Windows.Forms.Label();
            this.ux_labelReceivedDate = new System.Windows.Forms.Label();
            this.ux_textboxTaxonomy = new System.Windows.Forms.TextBox();
            this.ux_labelTaxonomy = new System.Windows.Forms.Label();
            this.ux_checkboxCore = new System.Windows.Forms.CheckBox();
            this.ux_checkboxBackedUp = new System.Windows.Forms.CheckBox();
            this.ux_textboxBackupLocation = new System.Windows.Forms.TextBox();
            this.ux_labelBackupLocation = new System.Windows.Forms.Label();
            this.ux_textboxActiveSite = new System.Windows.Forms.TextBox();
            this.ux_labelActiveSite = new System.Windows.Forms.Label();
            this.ux_groupbox3 = new System.Windows.Forms.GroupBox();
            this.ux_comboboxReproductiveUniformity = new System.Windows.Forms.ComboBox();
            this.ux_comboboxLifeForm = new System.Windows.Forms.ComboBox();
            this.ux_comboboxImprovementStatus = new System.Windows.Forms.ComboBox();
            this.ux_comboboxInitialMaterialType = new System.Windows.Forms.ComboBox();
            this.ux_labelInitialMaterialType = new System.Windows.Forms.Label();
            this.ux_labelReproductiveUniformity = new System.Windows.Forms.Label();
            this.ux_labelLifeForm = new System.Windows.Forms.Label();
            this.ux_labelImprovementStatus = new System.Windows.Forms.Label();
            this.ux_groupbox2 = new System.Windows.Forms.GroupBox();
            this.ux_labelBackupLocation2 = new System.Windows.Forms.Label();
            this.ux_textboxBackupLocation2 = new System.Windows.Forms.TextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.ux_bindingnavigatorForm = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ux_labelAccessionID = new System.Windows.Forms.Label();
            this.ux_labelAccessionName = new System.Windows.Forms.Label();
            this.ux_textboxAccessionName = new System.Windows.Forms.TextBox();
            this.ux_textboxAccessionID = new System.Windows.Forms.TextBox();
            this.ux_groupbox4 = new System.Windows.Forms.GroupBox();
            this.ux_labelNote = new System.Windows.Forms.Label();
            this.ux_textboxNote = new System.Windows.Forms.TextBox();
            this.ux_groupbox1.SuspendLayout();
            this.ux_groupbox3.SuspendLayout();
            this.ux_groupbox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ux_bindingnavigatorForm)).BeginInit();
            this.ux_bindingnavigatorForm.SuspendLayout();
            this.ux_groupbox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ux_labelPrefix
            // 
            this.ux_labelPrefix.AutoSize = true;
            this.ux_labelPrefix.Location = new System.Drawing.Point(15, 21);
            this.ux_labelPrefix.Name = "ux_labelPrefix";
            this.ux_labelPrefix.Size = new System.Drawing.Size(33, 13);
            this.ux_labelPrefix.TabIndex = 1;
            this.ux_labelPrefix.Tag = "accession_number_part1";
            this.ux_labelPrefix.Text = "Prefix";
            // 
            // ux_labelNumber
            // 
            this.ux_labelNumber.AutoSize = true;
            this.ux_labelNumber.Location = new System.Drawing.Point(167, 21);
            this.ux_labelNumber.Name = "ux_labelNumber";
            this.ux_labelNumber.Size = new System.Drawing.Size(44, 13);
            this.ux_labelNumber.TabIndex = 3;
            this.ux_labelNumber.Tag = "accession_number_part2";
            this.ux_labelNumber.Text = "Number";
            // 
            // ux_textboxNumber
            // 
            this.ux_textboxNumber.Location = new System.Drawing.Point(170, 37);
            this.ux_textboxNumber.Name = "ux_textboxNumber";
            this.ux_textboxNumber.Size = new System.Drawing.Size(139, 20);
            this.ux_textboxNumber.TabIndex = 1;
            this.ux_textboxNumber.Tag = "accession_number_part2";
            // 
            // ux_labelSuffix
            // 
            this.ux_labelSuffix.AutoSize = true;
            this.ux_labelSuffix.Location = new System.Drawing.Point(319, 21);
            this.ux_labelSuffix.Name = "ux_labelSuffix";
            this.ux_labelSuffix.Size = new System.Drawing.Size(33, 13);
            this.ux_labelSuffix.TabIndex = 5;
            this.ux_labelSuffix.Tag = "accession_number_part3";
            this.ux_labelSuffix.Text = "Suffix";
            // 
            // ux_textboxSuffix
            // 
            this.ux_textboxSuffix.Location = new System.Drawing.Point(322, 37);
            this.ux_textboxSuffix.Name = "ux_textboxSuffix";
            this.ux_textboxSuffix.Size = new System.Drawing.Size(139, 20);
            this.ux_textboxSuffix.TabIndex = 2;
            this.ux_textboxSuffix.Tag = "accession_number_part3";
            // 
            // ux_groupbox1
            // 
            this.ux_groupbox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_groupbox1.Controls.Add(this.ux_textboxReceivedDate);
            this.ux_groupbox1.Controls.Add(this.ux_comboboxReceivedDatePrecision);
            this.ux_groupbox1.Controls.Add(this.ux_textboxPrefix);
            this.ux_groupbox1.Controls.Add(this.ux_textboxSuffix);
            this.ux_groupbox1.Controls.Add(this.ux_labelReceivedDatePrecision);
            this.ux_groupbox1.Controls.Add(this.ux_labelSuffix);
            this.ux_groupbox1.Controls.Add(this.ux_labelPrefix);
            this.ux_groupbox1.Controls.Add(this.ux_labelReceivedDate);
            this.ux_groupbox1.Controls.Add(this.ux_textboxNumber);
            this.ux_groupbox1.Controls.Add(this.ux_labelNumber);
            this.ux_groupbox1.Controls.Add(this.ux_textboxTaxonomy);
            this.ux_groupbox1.Controls.Add(this.ux_labelTaxonomy);
            this.ux_groupbox1.Location = new System.Drawing.Point(12, 58);
            this.ux_groupbox1.Name = "ux_groupbox1";
            this.ux_groupbox1.Size = new System.Drawing.Size(534, 150);
            this.ux_groupbox1.TabIndex = 100;
            this.ux_groupbox1.TabStop = false;
            // 
            // ux_textboxReceivedDate
            // 
            this.ux_textboxReceivedDate.Location = new System.Drawing.Point(16, 120);
            this.ux_textboxReceivedDate.Multiline = true;
            this.ux_textboxReceivedDate.Name = "ux_textboxReceivedDate";
            this.ux_textboxReceivedDate.Size = new System.Drawing.Size(102, 20);
            this.ux_textboxReceivedDate.TabIndex = 4;
            this.ux_textboxReceivedDate.Tag = "initial_received_date";
            // 
            // ux_comboboxReceivedDatePrecision
            // 
            this.ux_comboboxReceivedDatePrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ux_comboboxReceivedDatePrecision.FormattingEnabled = true;
            this.ux_comboboxReceivedDatePrecision.Location = new System.Drawing.Point(144, 120);
            this.ux_comboboxReceivedDatePrecision.Name = "ux_comboboxReceivedDatePrecision";
            this.ux_comboboxReceivedDatePrecision.Size = new System.Drawing.Size(99, 21);
            this.ux_comboboxReceivedDatePrecision.TabIndex = 5;
            this.ux_comboboxReceivedDatePrecision.Tag = "initial_received_date_code";
            // 
            // ux_textboxPrefix
            // 
            this.ux_textboxPrefix.Location = new System.Drawing.Point(18, 37);
            this.ux_textboxPrefix.Multiline = true;
            this.ux_textboxPrefix.Name = "ux_textboxPrefix";
            this.ux_textboxPrefix.Size = new System.Drawing.Size(139, 20);
            this.ux_textboxPrefix.TabIndex = 0;
            this.ux_textboxPrefix.Tag = "accession_number_part1";
            // 
            // ux_labelReceivedDatePrecision
            // 
            this.ux_labelReceivedDatePrecision.AutoSize = true;
            this.ux_labelReceivedDatePrecision.Location = new System.Drawing.Point(141, 104);
            this.ux_labelReceivedDatePrecision.Name = "ux_labelReceivedDatePrecision";
            this.ux_labelReceivedDatePrecision.Size = new System.Drawing.Size(76, 13);
            this.ux_labelReceivedDatePrecision.TabIndex = 11;
            this.ux_labelReceivedDatePrecision.Tag = "initial_received_date_code";
            this.ux_labelReceivedDatePrecision.Text = "Date Precision";
            // 
            // ux_labelReceivedDate
            // 
            this.ux_labelReceivedDate.AutoSize = true;
            this.ux_labelReceivedDate.Location = new System.Drawing.Point(14, 104);
            this.ux_labelReceivedDate.Name = "ux_labelReceivedDate";
            this.ux_labelReceivedDate.Size = new System.Drawing.Size(79, 13);
            this.ux_labelReceivedDate.TabIndex = 9;
            this.ux_labelReceivedDate.Tag = "initial_received_date";
            this.ux_labelReceivedDate.Text = "Received Date";
            // 
            // ux_textboxTaxonomy
            // 
            this.ux_textboxTaxonomy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxTaxonomy.Location = new System.Drawing.Point(18, 81);
            this.ux_textboxTaxonomy.Name = "ux_textboxTaxonomy";
            this.ux_textboxTaxonomy.Size = new System.Drawing.Size(495, 20);
            this.ux_textboxTaxonomy.TabIndex = 3;
            this.ux_textboxTaxonomy.Tag = "taxonomy_species_id";
            // 
            // ux_labelTaxonomy
            // 
            this.ux_labelTaxonomy.AutoSize = true;
            this.ux_labelTaxonomy.Location = new System.Drawing.Point(15, 65);
            this.ux_labelTaxonomy.Name = "ux_labelTaxonomy";
            this.ux_labelTaxonomy.Size = new System.Drawing.Size(56, 13);
            this.ux_labelTaxonomy.TabIndex = 7;
            this.ux_labelTaxonomy.Tag = "taxonomy_species_id";
            this.ux_labelTaxonomy.Text = "Taxonomy";
            // 
            // ux_checkboxCore
            // 
            this.ux_checkboxCore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_checkboxCore.Location = new System.Drawing.Point(371, 34);
            this.ux_checkboxCore.Name = "ux_checkboxCore";
            this.ux_checkboxCore.Size = new System.Drawing.Size(157, 17);
            this.ux_checkboxCore.TabIndex = 11;
            this.ux_checkboxCore.Tag = "is_core";
            this.ux_checkboxCore.Text = "Is this a Core Accession?";
            this.ux_checkboxCore.UseVisualStyleBackColor = true;
            // 
            // ux_checkboxBackedUp
            // 
            this.ux_checkboxBackedUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_checkboxBackedUp.Location = new System.Drawing.Point(371, 73);
            this.ux_checkboxBackedUp.Name = "ux_checkboxBackedUp";
            this.ux_checkboxBackedUp.Size = new System.Drawing.Size(157, 17);
            this.ux_checkboxBackedUp.TabIndex = 13;
            this.ux_checkboxBackedUp.Tag = "is_backed_up";
            this.ux_checkboxBackedUp.Text = "Is this Backed up?";
            this.ux_checkboxBackedUp.UseVisualStyleBackColor = true;
            // 
            // ux_textboxBackupLocation
            // 
            this.ux_textboxBackupLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxBackupLocation.Location = new System.Drawing.Point(16, 71);
            this.ux_textboxBackupLocation.Name = "ux_textboxBackupLocation";
            this.ux_textboxBackupLocation.Size = new System.Drawing.Size(341, 20);
            this.ux_textboxBackupLocation.TabIndex = 12;
            this.ux_textboxBackupLocation.Tag = "backup_location1_site_id";
            // 
            // ux_labelBackupLocation
            // 
            this.ux_labelBackupLocation.AutoSize = true;
            this.ux_labelBackupLocation.Location = new System.Drawing.Point(13, 55);
            this.ux_labelBackupLocation.Name = "ux_labelBackupLocation";
            this.ux_labelBackupLocation.Size = new System.Drawing.Size(88, 13);
            this.ux_labelBackupLocation.TabIndex = 13;
            this.ux_labelBackupLocation.Tag = "backup_location1_site_id";
            this.ux_labelBackupLocation.Text = "Backup Location";
            // 
            // ux_textboxActiveSite
            // 
            this.ux_textboxActiveSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxActiveSite.Location = new System.Drawing.Point(16, 32);
            this.ux_textboxActiveSite.Name = "ux_textboxActiveSite";
            this.ux_textboxActiveSite.Size = new System.Drawing.Size(341, 20);
            this.ux_textboxActiveSite.TabIndex = 10;
            this.ux_textboxActiveSite.Tag = "owner_site_id";
            // 
            // ux_labelActiveSite
            // 
            this.ux_labelActiveSite.AutoSize = true;
            this.ux_labelActiveSite.Location = new System.Drawing.Point(13, 16);
            this.ux_labelActiveSite.Name = "ux_labelActiveSite";
            this.ux_labelActiveSite.Size = new System.Drawing.Size(58, 13);
            this.ux_labelActiveSite.TabIndex = 15;
            this.ux_labelActiveSite.Tag = "owner_site_id";
            this.ux_labelActiveSite.Text = "Active Site";
            // 
            // ux_groupbox3
            // 
            this.ux_groupbox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_groupbox3.Controls.Add(this.ux_comboboxReproductiveUniformity);
            this.ux_groupbox3.Controls.Add(this.ux_comboboxLifeForm);
            this.ux_groupbox3.Controls.Add(this.ux_comboboxImprovementStatus);
            this.ux_groupbox3.Controls.Add(this.ux_comboboxInitialMaterialType);
            this.ux_groupbox3.Controls.Add(this.ux_labelInitialMaterialType);
            this.ux_groupbox3.Controls.Add(this.ux_labelReproductiveUniformity);
            this.ux_groupbox3.Controls.Add(this.ux_labelLifeForm);
            this.ux_groupbox3.Controls.Add(this.ux_labelImprovementStatus);
            this.ux_groupbox3.Location = new System.Drawing.Point(12, 214);
            this.ux_groupbox3.Name = "ux_groupbox3";
            this.ux_groupbox3.Size = new System.Drawing.Size(534, 83);
            this.ux_groupbox3.TabIndex = 101;
            this.ux_groupbox3.TabStop = false;
            // 
            // ux_comboboxReproductiveUniformity
            // 
            this.ux_comboboxReproductiveUniformity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ux_comboboxReproductiveUniformity.FormattingEnabled = true;
            this.ux_comboboxReproductiveUniformity.Location = new System.Drawing.Point(409, 43);
            this.ux_comboboxReproductiveUniformity.Name = "ux_comboboxReproductiveUniformity";
            this.ux_comboboxReproductiveUniformity.Size = new System.Drawing.Size(115, 21);
            this.ux_comboboxReproductiveUniformity.TabIndex = 9;
            this.ux_comboboxReproductiveUniformity.Tag = "reproductive_uniformity_code";
            // 
            // ux_comboboxLifeForm
            // 
            this.ux_comboboxLifeForm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ux_comboboxLifeForm.FormattingEnabled = true;
            this.ux_comboboxLifeForm.Location = new System.Drawing.Point(147, 43);
            this.ux_comboboxLifeForm.Name = "ux_comboboxLifeForm";
            this.ux_comboboxLifeForm.Size = new System.Drawing.Size(115, 21);
            this.ux_comboboxLifeForm.TabIndex = 7;
            this.ux_comboboxLifeForm.Tag = "life_form_code";
            // 
            // ux_comboboxImprovementStatus
            // 
            this.ux_comboboxImprovementStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ux_comboboxImprovementStatus.FormattingEnabled = true;
            this.ux_comboboxImprovementStatus.Location = new System.Drawing.Point(278, 43);
            this.ux_comboboxImprovementStatus.Name = "ux_comboboxImprovementStatus";
            this.ux_comboboxImprovementStatus.Size = new System.Drawing.Size(115, 21);
            this.ux_comboboxImprovementStatus.TabIndex = 8;
            this.ux_comboboxImprovementStatus.Tag = "improvement_status_code";
            // 
            // ux_comboboxInitialMaterialType
            // 
            this.ux_comboboxInitialMaterialType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ux_comboboxInitialMaterialType.FormattingEnabled = true;
            this.ux_comboboxInitialMaterialType.Location = new System.Drawing.Point(16, 43);
            this.ux_comboboxInitialMaterialType.Name = "ux_comboboxInitialMaterialType";
            this.ux_comboboxInitialMaterialType.Size = new System.Drawing.Size(115, 21);
            this.ux_comboboxInitialMaterialType.TabIndex = 6;
            this.ux_comboboxInitialMaterialType.Tag = "initial_received_form_code";
            // 
            // ux_labelInitialMaterialType
            // 
            this.ux_labelInitialMaterialType.AutoSize = true;
            this.ux_labelInitialMaterialType.Location = new System.Drawing.Point(13, 27);
            this.ux_labelInitialMaterialType.Name = "ux_labelInitialMaterialType";
            this.ux_labelInitialMaterialType.Size = new System.Drawing.Size(98, 13);
            this.ux_labelInitialMaterialType.TabIndex = 7;
            this.ux_labelInitialMaterialType.Tag = "initial_received_form_code";
            this.ux_labelInitialMaterialType.Text = "Initial Material Type";
            // 
            // ux_labelReproductiveUniformity
            // 
            this.ux_labelReproductiveUniformity.AutoSize = true;
            this.ux_labelReproductiveUniformity.Location = new System.Drawing.Point(406, 27);
            this.ux_labelReproductiveUniformity.Name = "ux_labelReproductiveUniformity";
            this.ux_labelReproductiveUniformity.Size = new System.Drawing.Size(120, 13);
            this.ux_labelReproductiveUniformity.TabIndex = 11;
            this.ux_labelReproductiveUniformity.Tag = "reproductive_uniformity_code";
            this.ux_labelReproductiveUniformity.Text = "Reproductive Uniformity";
            // 
            // ux_labelLifeForm
            // 
            this.ux_labelLifeForm.AutoSize = true;
            this.ux_labelLifeForm.Location = new System.Drawing.Point(144, 27);
            this.ux_labelLifeForm.Name = "ux_labelLifeForm";
            this.ux_labelLifeForm.Size = new System.Drawing.Size(50, 13);
            this.ux_labelLifeForm.TabIndex = 7;
            this.ux_labelLifeForm.Tag = "life_form_code";
            this.ux_labelLifeForm.Text = "Life Form";
            // 
            // ux_labelImprovementStatus
            // 
            this.ux_labelImprovementStatus.AutoSize = true;
            this.ux_labelImprovementStatus.Location = new System.Drawing.Point(275, 27);
            this.ux_labelImprovementStatus.Name = "ux_labelImprovementStatus";
            this.ux_labelImprovementStatus.Size = new System.Drawing.Size(101, 13);
            this.ux_labelImprovementStatus.TabIndex = 9;
            this.ux_labelImprovementStatus.Tag = "improvement_status_code";
            this.ux_labelImprovementStatus.Text = "Improvement Status";
            // 
            // ux_groupbox2
            // 
            this.ux_groupbox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_groupbox2.Controls.Add(this.ux_labelBackupLocation2);
            this.ux_groupbox2.Controls.Add(this.ux_textboxBackupLocation2);
            this.ux_groupbox2.Controls.Add(this.ux_textboxActiveSite);
            this.ux_groupbox2.Controls.Add(this.ux_checkboxCore);
            this.ux_groupbox2.Controls.Add(this.ux_checkboxBackedUp);
            this.ux_groupbox2.Controls.Add(this.ux_labelBackupLocation);
            this.ux_groupbox2.Controls.Add(this.ux_labelActiveSite);
            this.ux_groupbox2.Controls.Add(this.ux_textboxBackupLocation);
            this.ux_groupbox2.Location = new System.Drawing.Point(12, 303);
            this.ux_groupbox2.Name = "ux_groupbox2";
            this.ux_groupbox2.Size = new System.Drawing.Size(534, 143);
            this.ux_groupbox2.TabIndex = 102;
            this.ux_groupbox2.TabStop = false;
            // 
            // ux_labelBackupLocation2
            // 
            this.ux_labelBackupLocation2.AutoSize = true;
            this.ux_labelBackupLocation2.Location = new System.Drawing.Point(13, 93);
            this.ux_labelBackupLocation2.Name = "ux_labelBackupLocation2";
            this.ux_labelBackupLocation2.Size = new System.Drawing.Size(97, 13);
            this.ux_labelBackupLocation2.TabIndex = 17;
            this.ux_labelBackupLocation2.Tag = "backup_location2_site_id";
            this.ux_labelBackupLocation2.Text = "Backup Location 2";
            // 
            // ux_textboxBackupLocation2
            // 
            this.ux_textboxBackupLocation2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxBackupLocation2.Location = new System.Drawing.Point(16, 109);
            this.ux_textboxBackupLocation2.Name = "ux_textboxBackupLocation2";
            this.ux_textboxBackupLocation2.Size = new System.Drawing.Size(341, 20);
            this.ux_textboxBackupLocation2.TabIndex = 16;
            this.ux_textboxBackupLocation2.Tag = "backup_location2_site_id";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "DataContainer_MoveFirst.bmp");
            this.imageList1.Images.SetKeyName(1, "DataContainer_MoveLast.bmp");
            this.imageList1.Images.SetKeyName(2, "DataContainer_MoveNext.bmp");
            this.imageList1.Images.SetKeyName(3, "DataContainer_MovePrevious.bmp");
            // 
            // ux_bindingnavigatorForm
            // 
            this.ux_bindingnavigatorForm.AddNewItem = this.bindingNavigatorAddNewItem;
            this.ux_bindingnavigatorForm.CountItem = this.bindingNavigatorCountItem;
            this.ux_bindingnavigatorForm.DeleteItem = this.bindingNavigatorDeleteItem;
            this.ux_bindingnavigatorForm.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ux_bindingnavigatorForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.ux_bindingnavigatorForm.Location = new System.Drawing.Point(0, 0);
            this.ux_bindingnavigatorForm.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.ux_bindingnavigatorForm.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.ux_bindingnavigatorForm.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.ux_bindingnavigatorForm.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.ux_bindingnavigatorForm.Name = "ux_bindingnavigatorForm";
            this.ux_bindingnavigatorForm.PositionItem = this.bindingNavigatorPositionItem;
            this.ux_bindingnavigatorForm.Size = new System.Drawing.Size(563, 25);
            this.ux_bindingnavigatorForm.TabIndex = 23;
            this.ux_bindingnavigatorForm.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Enabled = false;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Enabled = false;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 21);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ux_labelAccessionID
            // 
            this.ux_labelAccessionID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_labelAccessionID.Location = new System.Drawing.Point(385, 40);
            this.ux_labelAccessionID.Name = "ux_labelAccessionID";
            this.ux_labelAccessionID.Size = new System.Drawing.Size(94, 15);
            this.ux_labelAccessionID.TabIndex = 24;
            this.ux_labelAccessionID.Tag = "accession_id";
            this.ux_labelAccessionID.Text = "ID:";
            this.ux_labelAccessionID.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ux_labelAccessionName
            // 
            this.ux_labelAccessionName.Location = new System.Drawing.Point(12, 40);
            this.ux_labelAccessionName.Name = "ux_labelAccessionName";
            this.ux_labelAccessionName.Size = new System.Drawing.Size(93, 15);
            this.ux_labelAccessionName.TabIndex = 25;
            this.ux_labelAccessionName.Tag = "accession_name_id";
            this.ux_labelAccessionName.Text = "Accession Name:";
            this.ux_labelAccessionName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ux_textboxAccessionName
            // 
            this.ux_textboxAccessionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxAccessionName.Location = new System.Drawing.Point(111, 37);
            this.ux_textboxAccessionName.Multiline = true;
            this.ux_textboxAccessionName.Name = "ux_textboxAccessionName";
            this.ux_textboxAccessionName.Size = new System.Drawing.Size(261, 20);
            this.ux_textboxAccessionName.TabIndex = 200;
            this.ux_textboxAccessionName.Tag = "plant_name";
            // 
            // ux_textboxAccessionID
            // 
            this.ux_textboxAccessionID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxAccessionID.Location = new System.Drawing.Point(485, 37);
            this.ux_textboxAccessionID.Name = "ux_textboxAccessionID";
            this.ux_textboxAccessionID.Size = new System.Drawing.Size(61, 20);
            this.ux_textboxAccessionID.TabIndex = 201;
            this.ux_textboxAccessionID.Tag = "accession_id";
            // 
            // ux_groupbox4
            // 
            this.ux_groupbox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_groupbox4.Controls.Add(this.ux_labelNote);
            this.ux_groupbox4.Controls.Add(this.ux_textboxNote);
            this.ux_groupbox4.Location = new System.Drawing.Point(12, 453);
            this.ux_groupbox4.Name = "ux_groupbox4";
            this.ux_groupbox4.Size = new System.Drawing.Size(534, 101);
            this.ux_groupbox4.TabIndex = 202;
            this.ux_groupbox4.TabStop = false;
            // 
            // ux_labelNote
            // 
            this.ux_labelNote.AutoSize = true;
            this.ux_labelNote.Location = new System.Drawing.Point(14, 10);
            this.ux_labelNote.Name = "ux_labelNote";
            this.ux_labelNote.Size = new System.Drawing.Size(33, 13);
            this.ux_labelNote.TabIndex = 16;
            this.ux_labelNote.Tag = "note";
            this.ux_labelNote.Text = "Note:";
            // 
            // ux_textboxNote
            // 
            this.ux_textboxNote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ux_textboxNote.Location = new System.Drawing.Point(11, 27);
            this.ux_textboxNote.Multiline = true;
            this.ux_textboxNote.Name = "ux_textboxNote";
            this.ux_textboxNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ux_textboxNote.Size = new System.Drawing.Size(513, 68);
            this.ux_textboxNote.TabIndex = 0;
            this.ux_textboxNote.Tag = "note";
            // 
            // AccessionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 565);
            this.Controls.Add(this.ux_groupbox4);
            this.Controls.Add(this.ux_textboxAccessionID);
            this.Controls.Add(this.ux_textboxAccessionName);
            this.Controls.Add(this.ux_labelAccessionName);
            this.Controls.Add(this.ux_labelAccessionID);
            this.Controls.Add(this.ux_bindingnavigatorForm);
            this.Controls.Add(this.ux_groupbox2);
            this.Controls.Add(this.ux_groupbox3);
            this.Controls.Add(this.ux_groupbox1);
            this.Name = "AccessionForm";
            this.Text = "Accession Form";
            this.Load += new System.EventHandler(this.AccessionForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AccessionForm_FormClosed);
            this.ux_groupbox1.ResumeLayout(false);
            this.ux_groupbox1.PerformLayout();
            this.ux_groupbox3.ResumeLayout(false);
            this.ux_groupbox3.PerformLayout();
            this.ux_groupbox2.ResumeLayout(false);
            this.ux_groupbox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ux_bindingnavigatorForm)).EndInit();
            this.ux_bindingnavigatorForm.ResumeLayout(false);
            this.ux_bindingnavigatorForm.PerformLayout();
            this.ux_groupbox4.ResumeLayout(false);
            this.ux_groupbox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ux_labelPrefix;
        private System.Windows.Forms.Label ux_labelNumber;
        private System.Windows.Forms.TextBox ux_textboxNumber;
        private System.Windows.Forms.Label ux_labelSuffix;
        private System.Windows.Forms.TextBox ux_textboxSuffix;
        private System.Windows.Forms.GroupBox ux_groupbox1;
        private System.Windows.Forms.TextBox ux_textboxTaxonomy;
        private System.Windows.Forms.Label ux_labelTaxonomy;
        private System.Windows.Forms.CheckBox ux_checkboxCore;
        private System.Windows.Forms.CheckBox ux_checkboxBackedUp;
        private System.Windows.Forms.TextBox ux_textboxBackupLocation;
        private System.Windows.Forms.Label ux_labelBackupLocation;
        private System.Windows.Forms.TextBox ux_textboxActiveSite;
        private System.Windows.Forms.Label ux_labelActiveSite;
        private System.Windows.Forms.GroupBox ux_groupbox3;
        private System.Windows.Forms.Label ux_labelReproductiveUniformity;
        private System.Windows.Forms.Label ux_labelLifeForm;
        private System.Windows.Forms.Label ux_labelImprovementStatus;
        private System.Windows.Forms.Label ux_labelReceivedDatePrecision;
        private System.Windows.Forms.Label ux_labelInitialMaterialType;
        private System.Windows.Forms.Label ux_labelReceivedDate;
        private System.Windows.Forms.TextBox ux_textboxPrefix;
        private System.Windows.Forms.GroupBox ux_groupbox2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox ux_comboboxLifeForm;
        private System.Windows.Forms.ComboBox ux_comboboxInitialMaterialType;
        private System.Windows.Forms.ComboBox ux_comboboxImprovementStatus;
        private System.Windows.Forms.ComboBox ux_comboboxReproductiveUniformity;
        private System.Windows.Forms.ComboBox ux_comboboxReceivedDatePrecision;
        private System.Windows.Forms.BindingNavigator ux_bindingnavigatorForm;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.Label ux_labelAccessionID;
        private System.Windows.Forms.Label ux_labelAccessionName;
        private System.Windows.Forms.TextBox ux_textboxAccessionName;
        private System.Windows.Forms.TextBox ux_textboxAccessionID;
        private System.Windows.Forms.TextBox ux_textboxReceivedDate;
        private System.Windows.Forms.Label ux_labelBackupLocation2;
        private System.Windows.Forms.TextBox ux_textboxBackupLocation2;
        private System.Windows.Forms.GroupBox ux_groupbox4;
        private System.Windows.Forms.Label ux_labelNote;
        private System.Windows.Forms.TextBox ux_textboxNote;
    }
}