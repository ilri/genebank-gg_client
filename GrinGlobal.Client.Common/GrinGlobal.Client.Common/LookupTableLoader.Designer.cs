namespace GRINGlobal.Client.Common
{
    partial class LookupTableLoader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookupTableLoader));
            this.ux_labelUserHintText = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ux_labelActivityBarLabel = new System.Windows.Forms.Label();
            this.ux_radiobuttonHighDemand = new System.Windows.Forms.RadioButton();
            this.ux_labelLoadAllTablesLabel = new System.Windows.Forms.Label();
            this.ux_radiobuttonMediumDemand = new System.Windows.Forms.RadioButton();
            this.ux_radiobuttonLowDemand = new System.Windows.Forms.RadioButton();
            this.ux_labelAutoUpdateCheckbox = new System.Windows.Forms.Label();
            this.ux_labelTableNameLabel = new System.Windows.Forms.Label();
            this.ux_labelProgressBarLabel = new System.Windows.Forms.Label();
            this.ux_labelLoadTableNowLabel = new System.Windows.Forms.Label();
            this.ux_labelReloadLabel = new System.Windows.Forms.Label();
            this.ux_labelUpdateLabel = new System.Windows.Forms.Label();
            this.ux_labelLoadingLabel = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ux_labelUserHintText
            // 
            this.ux_labelUserHintText.AutoSize = true;
            this.ux_labelUserHintText.Location = new System.Drawing.Point(12, 9);
            this.ux_labelUserHintText.Name = "ux_labelUserHintText";
            this.ux_labelUserHintText.Size = new System.Drawing.Size(307, 13);
            this.ux_labelUserHintText.TabIndex = 2;
            this.ux_labelUserHintText.Text = "Click any Load button (or the Load All button) to begin loading...";
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetDouble;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 55);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(595, 236);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 294);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(622, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Enabled = false;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(607, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ux_labelActivityBarLabel);
            this.groupBox1.Controls.Add(this.ux_radiobuttonHighDemand);
            this.groupBox1.Controls.Add(this.ux_labelLoadAllTablesLabel);
            this.groupBox1.Controls.Add(this.ux_radiobuttonMediumDemand);
            this.groupBox1.Controls.Add(this.ux_radiobuttonLowDemand);
            this.groupBox1.Location = new System.Drawing.Point(426, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 40);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resource Demand";
            // 
            // ux_labelActivityBarLabel
            // 
            this.ux_labelActivityBarLabel.AutoSize = true;
            this.ux_labelActivityBarLabel.Location = new System.Drawing.Point(97, 32);
            this.ux_labelActivityBarLabel.Name = "ux_labelActivityBarLabel";
            this.ux_labelActivityBarLabel.Size = new System.Drawing.Size(41, 13);
            this.ux_labelActivityBarLabel.TabIndex = 11;
            this.ux_labelActivityBarLabel.Text = "Activity";
            this.ux_labelActivityBarLabel.Visible = false;
            // 
            // ux_radiobuttonHighDemand
            // 
            this.ux_radiobuttonHighDemand.AutoSize = true;
            this.ux_radiobuttonHighDemand.Checked = true;
            this.ux_radiobuttonHighDemand.Location = new System.Drawing.Point(120, 17);
            this.ux_radiobuttonHighDemand.Name = "ux_radiobuttonHighDemand";
            this.ux_radiobuttonHighDemand.Size = new System.Drawing.Size(47, 17);
            this.ux_radiobuttonHighDemand.TabIndex = 2;
            this.ux_radiobuttonHighDemand.TabStop = true;
            this.ux_radiobuttonHighDemand.Text = "High";
            this.ux_radiobuttonHighDemand.UseVisualStyleBackColor = true;
            // 
            // ux_labelLoadAllTablesLabel
            // 
            this.ux_labelLoadAllTablesLabel.AutoSize = true;
            this.ux_labelLoadAllTablesLabel.Location = new System.Drawing.Point(-3, 32);
            this.ux_labelLoadAllTablesLabel.Name = "ux_labelLoadAllTablesLabel";
            this.ux_labelLoadAllTablesLabel.Size = new System.Drawing.Size(45, 13);
            this.ux_labelLoadAllTablesLabel.TabIndex = 10;
            this.ux_labelLoadAllTablesLabel.Text = "Load All";
            this.ux_labelLoadAllTablesLabel.Visible = false;
            // 
            // ux_radiobuttonMediumDemand
            // 
            this.ux_radiobuttonMediumDemand.AutoSize = true;
            this.ux_radiobuttonMediumDemand.Location = new System.Drawing.Point(68, 17);
            this.ux_radiobuttonMediumDemand.Name = "ux_radiobuttonMediumDemand";
            this.ux_radiobuttonMediumDemand.Size = new System.Drawing.Size(46, 17);
            this.ux_radiobuttonMediumDemand.TabIndex = 1;
            this.ux_radiobuttonMediumDemand.Text = "Med";
            this.ux_radiobuttonMediumDemand.UseVisualStyleBackColor = true;
            // 
            // ux_radiobuttonLowDemand
            // 
            this.ux_radiobuttonLowDemand.AutoSize = true;
            this.ux_radiobuttonLowDemand.Location = new System.Drawing.Point(7, 17);
            this.ux_radiobuttonLowDemand.Name = "ux_radiobuttonLowDemand";
            this.ux_radiobuttonLowDemand.Size = new System.Drawing.Size(45, 17);
            this.ux_radiobuttonLowDemand.TabIndex = 0;
            this.ux_radiobuttonLowDemand.Text = "Low";
            this.ux_radiobuttonLowDemand.UseVisualStyleBackColor = true;
            // 
            // ux_labelAutoUpdateCheckbox
            // 
            this.ux_labelAutoUpdateCheckbox.AutoSize = true;
            this.ux_labelAutoUpdateCheckbox.Location = new System.Drawing.Point(15, 41);
            this.ux_labelAutoUpdateCheckbox.Name = "ux_labelAutoUpdateCheckbox";
            this.ux_labelAutoUpdateCheckbox.Size = new System.Drawing.Size(67, 13);
            this.ux_labelAutoUpdateCheckbox.TabIndex = 7;
            this.ux_labelAutoUpdateCheckbox.Text = "Auto Update";
            this.ux_labelAutoUpdateCheckbox.Visible = false;
            // 
            // ux_labelTableNameLabel
            // 
            this.ux_labelTableNameLabel.AutoSize = true;
            this.ux_labelTableNameLabel.Location = new System.Drawing.Point(102, 41);
            this.ux_labelTableNameLabel.Name = "ux_labelTableNameLabel";
            this.ux_labelTableNameLabel.Size = new System.Drawing.Size(104, 13);
            this.ux_labelTableNameLabel.TabIndex = 8;
            this.ux_labelTableNameLabel.Text = "Lookup Table Name";
            this.ux_labelTableNameLabel.Visible = false;
            // 
            // ux_labelProgressBarLabel
            // 
            this.ux_labelProgressBarLabel.AutoSize = true;
            this.ux_labelProgressBarLabel.Location = new System.Drawing.Point(299, 41);
            this.ux_labelProgressBarLabel.Name = "ux_labelProgressBarLabel";
            this.ux_labelProgressBarLabel.Size = new System.Drawing.Size(48, 13);
            this.ux_labelProgressBarLabel.TabIndex = 9;
            this.ux_labelProgressBarLabel.Text = "Progress";
            this.ux_labelProgressBarLabel.Visible = false;
            // 
            // ux_labelLoadTableNowLabel
            // 
            this.ux_labelLoadTableNowLabel.AutoSize = true;
            this.ux_labelLoadTableNowLabel.Location = new System.Drawing.Point(181, 26);
            this.ux_labelLoadTableNowLabel.Name = "ux_labelLoadTableNowLabel";
            this.ux_labelLoadTableNowLabel.Size = new System.Drawing.Size(31, 13);
            this.ux_labelLoadTableNowLabel.TabIndex = 10;
            this.ux_labelLoadTableNowLabel.Text = "Load";
            this.ux_labelLoadTableNowLabel.Visible = false;
            // 
            // ux_labelReloadLabel
            // 
            this.ux_labelReloadLabel.AutoSize = true;
            this.ux_labelReloadLabel.Location = new System.Drawing.Point(218, 26);
            this.ux_labelReloadLabel.Name = "ux_labelReloadLabel";
            this.ux_labelReloadLabel.Size = new System.Drawing.Size(41, 13);
            this.ux_labelReloadLabel.TabIndex = 11;
            this.ux_labelReloadLabel.Text = "Reload";
            this.ux_labelReloadLabel.Visible = false;
            // 
            // ux_labelUpdateLabel
            // 
            this.ux_labelUpdateLabel.AutoSize = true;
            this.ux_labelUpdateLabel.Location = new System.Drawing.Point(265, 26);
            this.ux_labelUpdateLabel.Name = "ux_labelUpdateLabel";
            this.ux_labelUpdateLabel.Size = new System.Drawing.Size(42, 13);
            this.ux_labelUpdateLabel.TabIndex = 12;
            this.ux_labelUpdateLabel.Text = "Update";
            this.ux_labelUpdateLabel.Visible = false;
            // 
            // ux_labelLoadingLabel
            // 
            this.ux_labelLoadingLabel.AutoSize = true;
            this.ux_labelLoadingLabel.Location = new System.Drawing.Point(313, 26);
            this.ux_labelLoadingLabel.Name = "ux_labelLoadingLabel";
            this.ux_labelLoadingLabel.Size = new System.Drawing.Size(54, 13);
            this.ux_labelLoadingLabel.TabIndex = 13;
            this.ux_labelLoadingLabel.Text = "Loading...";
            this.ux_labelLoadingLabel.Visible = false;
            // 
            // LookupTableLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 316);
            this.Controls.Add(this.ux_labelLoadingLabel);
            this.Controls.Add(this.ux_labelUpdateLabel);
            this.Controls.Add(this.ux_labelReloadLabel);
            this.Controls.Add(this.ux_labelLoadTableNowLabel);
            this.Controls.Add(this.ux_labelProgressBarLabel);
            this.Controls.Add(this.ux_labelTableNameLabel);
            this.Controls.Add(this.ux_labelAutoUpdateCheckbox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ux_labelUserHintText);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LookupTableLoader";
            this.Text = "Lookup Table Loader";
            this.Load += new System.EventHandler(this.LookupTableLoader_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LookupTableLoader_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ux_labelUserHintText;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton ux_radiobuttonLowDemand;
        private System.Windows.Forms.RadioButton ux_radiobuttonHighDemand;
        private System.Windows.Forms.RadioButton ux_radiobuttonMediumDemand;
        private System.Windows.Forms.Label ux_labelAutoUpdateCheckbox;
        private System.Windows.Forms.Label ux_labelTableNameLabel;
        private System.Windows.Forms.Label ux_labelProgressBarLabel;
        private System.Windows.Forms.Label ux_labelLoadAllTablesLabel;
        private System.Windows.Forms.Label ux_labelActivityBarLabel;
        private System.Windows.Forms.Label ux_labelLoadTableNowLabel;
        private System.Windows.Forms.Label ux_labelReloadLabel;
        private System.Windows.Forms.Label ux_labelUpdateLabel;
        private System.Windows.Forms.Label ux_labelLoadingLabel;
    }
}