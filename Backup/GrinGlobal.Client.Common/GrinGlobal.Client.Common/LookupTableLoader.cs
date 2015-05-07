using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRINGlobal.Client.Common
{
    public partial class LookupTableLoader : Form
    {
        SharedUtils _sharedUtils;

        private enum CtrlPosition
        {
            autoUpdate,
            tableName,
            progressBar,
            loadTableNow,
            activityProgressBar
        }

        public LookupTableLoader(string localDBInstance, SharedUtils sharedUtils)
        {
            _sharedUtils = sharedUtils;

            InitializeComponent();
        }
        
        private void LookupTableLoader_Load(object sender, EventArgs e)
        {
            // Indicate that things are working and the user should be patient...
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

_sharedUtils.UpdateControls(this.Controls, this.Name);
toolStripStatusLabel1.Tag = toolStripStatusLabel1.Text;

            DataTable lookupTables = _sharedUtils.LookupTablesGetSynchronizationStats();
            tableLayoutPanel1.Visible = true;

            tableLayoutPanel1.Show();

            // Set the style for the header row...
            tableLayoutPanel1.RowCount = lookupTables.Rows.Count + 2;
            tableLayoutPanel1.RowStyles[0].Height = 52;
            tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;

            // Create the column headers...
            CheckBox autoUpdateCheckBox = new CheckBox();
            autoUpdateCheckBox.CheckAlign = ContentAlignment.MiddleRight;
            autoUpdateCheckBox.TextImageRelation = TextImageRelation.ImageAboveText;
            autoUpdateCheckBox.Image = Image.FromFile(@"Images\GG_LUTableAutoUpdate.ico");// Icon.ExtractAssociatedIcon(@"Images\GG-LUTableAutoUpdate.ico").ToBitmap();
            autoUpdateCheckBox.Text = ux_labelAutoUpdateCheckbox.Text;
            autoUpdateCheckBox.Tag = "_AUTOUPDATEALL_";
            autoUpdateCheckBox.TextAlign = ContentAlignment.MiddleCenter;
            autoUpdateCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            autoUpdateCheckBox.CheckedChanged += new EventHandler(autoUpdate_CheckedChanged);
            tableLayoutPanel1.Controls.Add(autoUpdateCheckBox, (int)CtrlPosition.autoUpdate, 0);
            Label tableNameLabel = new Label();
            tableNameLabel.Text = ux_labelTableNameLabel.Text;
            tableNameLabel.TextAlign = ContentAlignment.MiddleCenter;
            tableNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            tableLayoutPanel1.Controls.Add(tableNameLabel, (int)CtrlPosition.tableName, 0);
            Label progressBarLabel = new Label();
            progressBarLabel.Text = ux_labelProgressBarLabel.Text;
            progressBarLabel.TextAlign = ContentAlignment.MiddleCenter;
            progressBarLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            tableLayoutPanel1.Controls.Add(progressBarLabel, (int)CtrlPosition.progressBar, 0);
            Button loadAllTablesButton = new Button();
            loadAllTablesButton.Text = ux_labelLoadAllTablesLabel.Text;
            loadAllTablesButton.Tag = "_LOADALL_";
            loadAllTablesButton.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            loadAllTablesButton.Click += new EventHandler(loadButton_Click);
            tableLayoutPanel1.Controls.Add(loadAllTablesButton, (int)CtrlPosition.loadTableNow, 0);
            Label activityBarLabel = new Label();
            activityBarLabel.Text = ux_labelActivityBarLabel.Text;
            activityBarLabel.TextAlign = ContentAlignment.MiddleCenter;
            activityBarLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            tableLayoutPanel1.Controls.Add(activityBarLabel, (int)CtrlPosition.activityProgressBar, 0);

            for (int i = 0; i < lookupTables.Rows.Count; i++)
            {
                CheckBox autoUpdate = new CheckBox();
                autoUpdate.Text = "";
                autoUpdate.CheckAlign = ContentAlignment.MiddleRight;
                autoUpdate.Tag = lookupTables.Rows[i]["dataview_name"].ToString();
                autoUpdate.CheckedChanged += new EventHandler(autoUpdate_CheckedChanged);
                autoUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
                autoUpdate.TextAlign = ContentAlignment.MiddleCenter;
                tableLayoutPanel1.Controls.Add(autoUpdate, (int)CtrlPosition.autoUpdate, i + 1);
                Label dataviewName = new Label();
                if (!string.IsNullOrEmpty(lookupTables.Rows[i]["title"].ToString()))
                {
                    dataviewName.Text = lookupTables.Rows[i]["title"].ToString();
                }
                else
                {
                    dataviewName.Text = lookupTables.Rows[i]["dataview_name"].ToString();
                }
                dataviewName.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
                dataviewName.Tag = lookupTables.Rows[i]["dataview_name"].ToString();
                dataviewName.TextAlign = ContentAlignment.MiddleCenter;
                tableLayoutPanel1.Controls.Add(dataviewName, (int)CtrlPosition.tableName, i + 1);
                ProgressBar progressBar = new ProgressBar();
                progressBar.Value = 0;
                progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
                progressBar.Tag = lookupTables.Rows[i]["dataview_name"].ToString();
                tableLayoutPanel1.Controls.Add(progressBar, (int)CtrlPosition.progressBar, i + 1);
                Button loadTableNow = new Button();
                loadTableNow.Text = ux_labelLoadTableNowLabel.Text;
                loadTableNow.Tag = lookupTables.Rows[i]["dataview_name"].ToString();
                loadTableNow.Width = 100;
                loadTableNow.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
                loadTableNow.Click += new EventHandler(loadButton_Click);
                tableLayoutPanel1.Controls.Add(loadTableNow, (int)CtrlPosition.loadTableNow, i + 1);
                ProgressBar activityProgressBar = new ProgressBar();
                activityProgressBar.Style = ProgressBarStyle.Blocks;
                activityProgressBar.MarqueeAnimationSpeed = 100;
                activityProgressBar.Minimum = 0;
                activityProgressBar.Maximum = 100;
                activityProgressBar.Value = 0;
                activityProgressBar.Anchor = AnchorStyles.Right | AnchorStyles.Left;
                activityProgressBar.Height = 10;
                activityProgressBar.Tag = lookupTables.Rows[i]["dataview_name"].ToString();
                tableLayoutPanel1.Controls.Add(activityProgressBar, (int)CtrlPosition.activityProgressBar, i + 1);
            }

            // Set the checkboxes for Auto Updating LU Tables...
            if (_sharedUtils.LocalDatabaseTableExists("lookup_table_status"))
            {
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM lookup_table_status", "");
                for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                {
                    string tableName = "";
                    if (tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i) != null &&
                        tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i).GetType() == typeof(CheckBox))
                    {
                        tableName = ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i)).Tag.ToString().Trim();
                        DataRow dr = dt.Rows.Find(tableName);
                        if (dr != null)
                        {
                            CheckState cs = dr["auto_update"].ToString().Trim().ToUpper() == "Y" ? CheckState.Checked : CheckState.Unchecked;
                            ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i)).CheckState = cs;
                        }
                    }                                        
                }
            }

            // Update the dialog box controls with current database information...
            UpdateControls();
            // Start the timer to update the interface...
            timer1.Start();
            
            // Indicate that the dialog is built and the user can navigate normally...
            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        void autoUpdate_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.Tag.ToString().Trim().ToUpper() == "_AUTOUPDATEALL_")
            {
                if (cb.CheckState == CheckState.Checked)
                {
                    for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                    {
                        if (tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i) != null &&
                            tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i).GetType() == typeof(CheckBox))
                        {
                            ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i)).CheckState = CheckState.Checked;
                        }
                    }
                }
                else if (cb.CheckState == CheckState.Unchecked)
                {
                    for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                    {
                        if (tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i) != null &&
                            tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i).GetType() == typeof(CheckBox))
                        {
                            ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i)).CheckState = CheckState.Unchecked;
                        }
                    }
                }
            }
            else
            {
                bool allSameCheckState = true;
                for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                {
                    if (tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i) != null &&
                        tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i).GetType() == typeof(CheckBox) &&
                        ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i)).CheckState != cb.CheckState)
                    {
                        allSameCheckState = false;
                    }
                }
                if (!allSameCheckState)
                {
                    if (tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, 0) != null &&
                        tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, 0).GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, 0)).CheckState = CheckState.Indeterminate;
                    }
                }
                else
                {
                    if (tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, 0) != null &&
                        tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, 0).GetType() == typeof(CheckBox))
                    {
                        ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, 0)).CheckState = cb.CheckState;
                    }
                }
            }
        }

        void loadButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            // Set the pagesize of the lookup table loader...
            if (ux_radiobuttonHighDemand.Checked)
            {
                _sharedUtils.LookupTablesLoadingPageSize = 100000;
            }
            else if (ux_radiobuttonMediumDemand.Checked)
            {
                _sharedUtils.LookupTablesLoadingPageSize = 10000;
            }
            else
            {
                _sharedUtils.LookupTablesLoadingPageSize = 1000;
            }

            if (btn.Tag.ToString() == "_LOADALL_")
            {
                for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                {
                    // Get the next rows start loading button...
                    Control ctrl = tableLayoutPanel1.GetControlFromPosition(2, i);
                    // If the row is empty - ignore it...
                    if (ctrl != null)
                    {
                        // Now disable it...
                        ctrl.Enabled = false;
                        // Enable the marquee progress bar...
                        ((ProgressBar)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.activityProgressBar, i)).Style = ProgressBarStyle.Marquee;
                        ((ProgressBar)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.activityProgressBar, i)).MarqueeAnimationSpeed = 3000000 / _sharedUtils.LookupTablesLoadingPageSize;

                        // If this table has been completely loaded, clear it and reload from Empty...
                        if (_sharedUtils.LookupTablesIsUpdated(ctrl.Tag.ToString().Trim()))
                        {
                            _sharedUtils.LookupTablesClearLookupTable(ctrl.Tag.ToString().Trim());
                        }

                        // Background thread the loading of the lookup table...
                        //_lookupTables.LoadTableFromDatabase(ctrl.Tag.ToString());
                        new System.Threading.Thread(_sharedUtils.LookupTablesLoadTableFromDatabase).Start(ctrl.Tag.ToString());
                        DateTime start = DateTime.Now;
                        // Wait 1/2 second before processing the next row (lookup table)...
                        while (DateTime.Now < start.AddSeconds(0.5))
                        {
                            // Let messages get processed...
                            Application.DoEvents();
                        }
                    }
                }
            }
            else
            {
                // Disable the start loading button...
                btn.Enabled = false;
                // Enable the marquee progress bar...
                TableLayoutPanelCellPosition progressBarPosition = tableLayoutPanel1.GetCellPosition(btn);
                ((ProgressBar)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.activityProgressBar, progressBarPosition.Row)).Style = ProgressBarStyle.Marquee;
                ((ProgressBar)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.activityProgressBar, progressBarPosition.Row)).MarqueeAnimationSpeed = 3000000 / _sharedUtils.LookupTablesLoadingPageSize;

                // If this table has been completely loaded, clear it and reload from Empty...
                if (_sharedUtils.LookupTablesIsUpdated(btn.Tag.ToString().Trim()))
                {
                    _sharedUtils.LookupTablesClearLookupTable(btn.Tag.ToString().Trim());
                }

                // Now thread the LU table load...
                new System.Threading.Thread(_sharedUtils.LookupTablesLoadTableFromDatabase).Start(btn.Tag.ToString());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            DataTable dt = new DataTable("lookup_table_status");
            try
            {
                dt = _sharedUtils.GetLocalData("SELECT * FROM lookup_table_status", "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (Control ctrl in tableLayoutPanel1.Controls)
                        {
                            // Set background color to cue the user whether or not this table needs updating...
                            if (ctrl.GetType() == typeof(Label) &&
                                ctrl.Tag != null &&
                                ctrl.Tag.ToString() == dr["dataview_name"].ToString())
                            {
                                if (((int)(dr["max_pk"]) > 0 &&
                                    (int)(dr["current_pk"]) < (int)(dr["max_pk"])) ||
                                    (dr["status"].ToString().ToUpper() != "COMPLETED" &&
                                    dr["status"].ToString().ToUpper() != "UPDATED"))
                                {
                                    ctrl.BackColor = Color.SandyBrown;
                                }
                                else
                                {
                                    ctrl.BackColor = Color.Empty;
                                }
                            }

                            // Update the progress bar for the LU Table...
                            if (ctrl.GetType() == typeof(ProgressBar) &&
                                ctrl.Tag.ToString() == dr["dataview_name"].ToString() &&
                                tableLayoutPanel1.GetColumn(ctrl) == (int)CtrlPosition.progressBar)
                            {
                                    Int64 min = 0;
                                    Int64 max = 0;
                                    Int64 current = 0;
                                    if (Int64.TryParse(dr["min_pk"].ToString(), out min) &&
                                        Int64.TryParse(dr["max_pk"].ToString(), out max) &&
                                        Int64.TryParse(dr["current_pk"].ToString(), out current))
                                    {
                                        if ((max - min) > 0)
                                        {
                                            ((ProgressBar)ctrl).Value = (int)Math.Min(100, (100 * (current - min) / (max - min)));
                                        }
                                        else if (current > 0 &&
                                            (dr["status"].ToString().ToUpper() == "COMPLETED" ||
                                            dr["status"].ToString().ToUpper() == "UPDATED"))
                                        {
                                            // This is the special case where only one row exists in the LUT...
                                            ((ProgressBar)ctrl).Value = 100;
                                        }
                                    }
                            }

                            // Update the activity progress bar...
                            if (ctrl.GetType() == typeof(ProgressBar) &&
                                ctrl.Tag.ToString() == dr["dataview_name"].ToString() &&
                                tableLayoutPanel1.GetColumn(ctrl) == (int)CtrlPosition.activityProgressBar)
                            {
                                if (dr["status"].ToString().ToUpper() == "COMPLETED")
                                {
                                    ((ProgressBar)ctrl).Style = ProgressBarStyle.Blocks;
                                    ((ProgressBar)ctrl).Visible = false;
                                }
                                else if (dr["status"].ToString().ToUpper() == "FAILED")
                                {
                                    ((ProgressBar)ctrl).Style = ProgressBarStyle.Blocks;
                                    ((ProgressBar)ctrl).Visible = false;
                                }
                                else if (dr["status"].ToString().ToUpper() == "PARTIAL")
                                {
                                    ((ProgressBar)ctrl).Style = ProgressBarStyle.Blocks;
                                    ((ProgressBar)ctrl).Visible = false;
                                }
                                else if (dr["status"].ToString().ToUpper() == "LOADING")
                                {
                                    ((ProgressBar)ctrl).Style = ProgressBarStyle.Marquee;
                                    ((ProgressBar)ctrl).Visible = true;
                                }
                                else if (dr["status"].ToString().ToUpper() == "UPDATED")
                                {
                                    ((ProgressBar)ctrl).Style = ProgressBarStyle.Blocks;
                                    ((ProgressBar)ctrl).Visible = false;
                                }
                                else
                                {
                                    ((ProgressBar)ctrl).Style = ProgressBarStyle.Blocks;
                                    ((ProgressBar)ctrl).Visible = false;
                                }
                            }

                            // Update the Load button for this table...
                            if (ctrl.GetType() == typeof(Button) &&
                                ctrl.Tag.ToString() == dr["dataview_name"].ToString())
                            {
                                if (dr["status"].ToString().ToUpper() == "COMPLETED")
                                {
                                    ctrl.Enabled = true;
                                    if (_sharedUtils.LookupTablesIsUpdated(ctrl.Tag.ToString().Trim()))
                                    {
                                        ctrl.Text = ux_labelReloadLabel.Text;
                                    }
                                    else
                                    {
                                        ctrl.Text = ux_labelUpdateLabel.Text;
                                    }
                                }
                                else if (dr["status"].ToString().ToUpper() == "FAILED")
                                {
                                    ctrl.Enabled = true;
                                }
                                else if (dr["status"].ToString().ToUpper() == "PARTIAL")
                                {
                                    ctrl.Enabled = true;
                                }
                                else if (dr["status"].ToString().ToUpper() == "LOADING")
                                {
                                    ctrl.Text = ux_labelLoadingLabel.Text;
                                    ctrl.Enabled = false;
                                }
                                else if (dr["status"].ToString().ToUpper() == "UPDATED")
                                {
                                    ctrl.Enabled = true;
                                    if (_sharedUtils.LookupTablesIsUpdated(ctrl.Tag.ToString().Trim()))
                                    {
                                        ctrl.Text = ux_labelReloadLabel.Text;
                                    }
                                    else
                                    {
                                        ctrl.Text = ux_labelUpdateLabel.Text;
                                    }
                                }
                                else
                                {
                                    ctrl.Enabled = true;
                                }
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch
            {
            }

//toolStripStatusLabel1.Text = "Connected to " + _sharedUtils.Url;
if (toolStripStatusLabel1.Tag != null)
{
    if (toolStripStatusLabel1.Tag.ToString().Contains("{0}"))
    {
        toolStripStatusLabel1.Text = string.Format(toolStripStatusLabel1.Tag.ToString(), _sharedUtils.Url);
    }
    else if (!toolStripStatusLabel1.Tag.ToString().Contains(_sharedUtils.Url))
    {
        toolStripStatusLabel1.Text = toolStripStatusLabel1.Tag.ToString() + " (" + _sharedUtils.Url + ")";
    }
    else
    {
        toolStripStatusLabel1.Text = toolStripStatusLabel1.Tag.ToString();
    }
}
else
{
    if (!toolStripStatusLabel1.Text.Contains(_sharedUtils.Url))
    {
        toolStripStatusLabel1.Text = toolStripStatusLabel1.Text + " (" + _sharedUtils.Url + ")";
    }
}
        }

        private void LookupTableLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Update the local LUT status table...
            if (_sharedUtils.LocalDatabaseTableExists("lookup_table_status"))
            {
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM lookup_table_status", "");
                for (int i = 1; i < tableLayoutPanel1.RowCount; i++)
                {
                    string tableName = "";
                    string autoUpdateSetting = "";
                    if (tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i) != null &&
                        tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i).GetType() == typeof(CheckBox))
                    {
                        tableName = ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i)).Tag.ToString().Trim();
                        autoUpdateSetting = ((CheckBox)tableLayoutPanel1.GetControlFromPosition((int)CtrlPosition.autoUpdate, i)).CheckState == CheckState.Checked ? "Y" : "N";
                        DataRow dr = dt.Rows.Find(tableName);
                        if (dr != null) dr["auto_update"] = autoUpdateSetting;
                    }
                }
                _sharedUtils.SaveLocalData(dt);
            }
        }
    }
}
