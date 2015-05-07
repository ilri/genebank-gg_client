using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace CooperatorWizard
{

    interface IGRINGlobalDataWizard
    {
        string FormName { get; }
        DataTable ChangedRecords { get; }
        string PKeyName { get; }
    }

    public partial class CooperatorWizard : Form, IGRINGlobalDataWizard
    {
        SharedUtils _sharedUtils;
        string _originalPKeys = "";
        string _cooperatorPKeys = "";
        DataTable _cooperator;
        BindingSource _cooperatorBindingSource;
        DataTable _webCooperator;
        BindingSource _webCooperatorBindingSource;
        DataSet _changedRecords = new DataSet();
        private Timer textChangeDelayTimer = new Timer();

        public CooperatorWizard(string pKeys, SharedUtils sharedUtils)
        {
            InitializeComponent();

            _cooperatorBindingSource = new BindingSource();
            _webCooperatorBindingSource = new BindingSource();
            _sharedUtils = sharedUtils;
            _originalPKeys = pKeys;
        }

        private void CooperatorWizard_Load(object sender, EventArgs e)
        {
            BuildCooperatorPage();
            BuildWebCooperatorPage();
            
            // Wire up the event handler for timer tick events...
            textChangeDelayTimer.Tick += new EventHandler(timerDelay_Tick);
        }

        public string FormName
        {
            get
            {
                return "Cooperator Wizard";
            }
        }

        public DataTable ChangedRecords
        {
            get
            {
                DataTable dt = new DataTable();
                if (_changedRecords.Tables.Contains(_cooperator.TableName))
                {
                    dt = _changedRecords.Tables[_cooperator.TableName].Copy();
                }
                return dt;
            }
        }

        public string PKeyName
        {
            get
            {
                return "cooperator_id";
            }
        }

        private void CooperatorWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // The user might be closing the form during the middle of edit changes in the datagridview - if so ask the
            // user if they would like to save their data...
            int intRowEdits = 0;

            _cooperatorBindingSource.EndEdit();
            if (_cooperator.GetChanges() != null) intRowEdits = _cooperator.GetChanges().Rows.Count;

            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s), are you sure you want to cancel your edits and close this window?", "Cancel Edits and Close", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "CooperatorWizard_FormClosingMessage1";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
            // Show the warning dialog message if there are unsaved edits...
            if (intRowEdits > 0 && DialogResult.No == ggMessageBox.ShowDialog())
            {
                e.Cancel = true;
            }
        }

        #region DGV control logic...
        private void ux_datagridview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            DataView dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            if (dv != null && e.ColumnIndex > -1)
            {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                if (_sharedUtils.LookupTablesIsValidFKField(dc) &&
                    e.RowIndex < dv.Count &&
                    dv[e.RowIndex].Row.RowState != DataRowState.Deleted)
                {
                    if (dv[e.RowIndex][e.ColumnIndex] != DBNull.Value)
                    {
                        e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                    }
                    dgv[e.ColumnIndex, e.RowIndex].ErrorText = dv[e.RowIndex].Row.GetColumnError(dc);
                    e.FormattingApplied = true;
                }

                if (dc.ReadOnly)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }

                if (dc.ExtendedProperties.Contains("is_nullable") &&
                    dc.ExtendedProperties["is_nullable"].ToString() == "N" &&
                    string.IsNullOrEmpty(dv[e.RowIndex][e.ColumnIndex].ToString()))
                {
                    e.CellStyle.BackColor = Color.Plum;
                }
            }
        }

        private void ux_datagridview_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string columnName = dgv.CurrentCell.OwningColumn.Name;
            DataColumn dc = dt.Columns[columnName];
            DataRow dr;

            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                string luTableName = dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim();
                dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                string suggestedFilter = dgv.CurrentCell.EditedFormattedValue.ToString();
                GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(_sharedUtils, columnName, dr, suggestedFilter);
                ltp.StartPosition = FormStartPosition.CenterParent;
                if (DialogResult.OK == ltp.ShowDialog())
                {
                    if (dr != null)
                    {
                        if (ltp.NewKey != null && dr[dgv.CurrentCell.ColumnIndex].ToString().Trim() != ltp.NewKey.Trim())
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = ltp.NewKey.Trim();
                            dgv.CurrentCell.Value = ltp.NewValue.Trim();
                        }
                        else if (ltp.NewKey == null)
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = DBNull.Value;
                            dgv.CurrentCell.Value = "";
                        }
                        dr.SetColumnError(dgv.CurrentCell.ColumnIndex, null);
                    }
                }
                dgv.EndEdit();
            }
        }

        private void ux_datagridview_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            string errorMessage = e.Exception.Message;
            int columnWithError = -1;

            // Find the cell the error belongs to (don't use e.ColumnIndex because it points to the current cell *NOT* the offending cell)...
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (errorMessage.Contains(col.Name))
                {
                    dgv[col.Name, e.RowIndex].ErrorText = errorMessage;
                    columnWithError = col.Index;
                }
            }
        }

        private void ux_datagridview_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (_sharedUtils.ProcessDGVEditShortcutKeys(dgv, e, _sharedUtils.UserCooperatorID))
            {
                RefreshDGVFormatting(dgv);
            }
        }

        private void RefreshDGVFormatting(DataGridView dgv)
        {
            // Refresh the format for each row in the DGV...
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                RefreshDGVRowFormatting(dgvr);
            }
            // Now force a re-paint of the DGV...
            dgv.Refresh();
        }

        private void RefreshDGVRowFormatting(DataGridViewRow dgvr)
        {
            foreach (DataGridViewCell dgvc in dgvr.Cells)
            {
                // Reset the background and foreground color...
                dgvc.Style.BackColor = Color.Empty;
                dgvc.Style.ForeColor = Color.Empty;
                dgvc.Style.SelectionBackColor = Color.Empty;
                dgvc.Style.SelectionForeColor = Color.Empty;
            }
            // If the row has changes make each changed cell yellow...
            DataRow dr = ((DataRowView)dgvr.DataBoundItem).Row;
            if (dr.RowState == DataRowState.Modified)
            {
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    // If the cell has been changed make it yellow...
                    if (!dr[dgvc.ColumnIndex, DataRowVersion.Original].Equals(dr[dgvc.ColumnIndex, DataRowVersion.Current]))
                    {
                        dgvc.Style.BackColor = Color.Yellow;
                        dr.SetColumnError(dgvc.ColumnIndex, null);
                    }
                    // Use default background color for this cell...
                    else
                    {
                        dgvc.Style.BackColor = Color.Empty;
                    }
                }
            }
        }

        #endregion
        
        private void BuildCooperatorPage()
        {
            // Get a refreshed list of the cooperators for the cooperator table...
            RefreshCooperatorList();

            // Now bind it to the DGV on the first tabpage...
            if (_cooperator != null)
            {
                // Bind the DGV to the binding source...
                ux_datagridviewCooperator.DataSource = _cooperatorBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewCooperator, _cooperator);
            }
        }

        private void BuildWebCooperatorPage()
        {
            // Get a refreshed list of the cooperators for the cooperator table...
            RefreshWebCooperatorList();

            // Now bind it to the DGV on the first tabpage...
            if (_webCooperator != null)
            {
                // Bind the DGV to the binding source...
                ux_datagridviewWebCooperator.DataSource = _webCooperatorBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewWebCooperator, _webCooperator);
            }
        }

        private void ux_textboxNameFilter_TextChanged(object sender, EventArgs e)
        {
            textChangeDelayTimer.Stop();
            textChangeDelayTimer.Interval = 1000;
            textChangeDelayTimer.Start();
        }

        void timerDelay_Tick(object sender, EventArgs e)
        {
            int intRowEdits = 0;
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            textChangeDelayTimer.Stop();

            // If the user has changed the _cooperator filter text and there are unsaved row changes in the _cooperator table - so warn the user...
            if (ux_tabcontrolMain.SelectedTab == CooperatorPage)
            {
                _cooperatorBindingSource.EndEdit();
                if (_cooperator.GetChanges() != null) intRowEdits = _cooperator.GetChanges().Rows.Count;

                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s) that will be lost if you refresh the data with new filters.\n\nAre you sure you want to lose your edit changes?", "Refreshing Data Will Lose Edits", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                ggMessageBox.Name = "CooperatorWizard_timerDelayMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
                // Show the warning dialog message if there are unsaved edits...
                if (intRowEdits == 0 || DialogResult.Yes == ggMessageBox.ShowDialog())
                {
                    // User has chosen to refresh _cooperator data and lose edit changes...
                    BuildCooperatorPage();
                }
            }

            if (ux_tabcontrolMain.SelectedTab == WebCooperatorPage) BuildWebCooperatorPage();

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void RefreshCooperatorList()
        {
            DataSet ds = new DataSet();
            string findText = "";
            string searchPKeys = "";

            // First get the cooperator_ids for the orginal search criteria (_originalPKeys)...
            ds = _sharedUtils.GetWebServiceData("get_cooperator", _originalPKeys, 0, 0);
            searchPKeys = ":cooperatorid=";
            if (ds.Tables.Contains("get_cooperator"))
            {
                foreach (DataRow dr in ds.Tables["get_cooperator"].Rows)
                {
                    searchPKeys += dr["cooperator_id"].ToString() + ",";
                }
            }

            // Now get the cooperator_ids from the local LU table that match the lastname and first name...
            if (!string.IsNullOrEmpty(ux_textboxLastName.Text)) findText += ux_textboxLastName.Text + "%";
            if (!string.IsNullOrEmpty(ux_textboxFirstName.Text)) findText += ux_textboxFirstName.Text + "%";
            if(!string.IsNullOrEmpty(findText))
            {
                DataTable dt = new DataTable();

                dt = _sharedUtils.LookupTablesGetMatchingRows("cooperator_lookup", findText, 1000);
                foreach (DataRow dr in dt.Rows)
                {
                    searchPKeys += dr["value_member"].ToString() + ",";
                }
            }

            // Remove the last trailing comma...
            searchPKeys = searchPKeys.TrimEnd(',');

            // Finally go get the full collection of cooperator_ids...
            ds = _sharedUtils.GetWebServiceData("get_cooperator", searchPKeys, 0, 0);
            if (ds.Tables.Contains("get_cooperator"))
            {
                _cooperator = ds.Tables["get_cooperator"].Copy();
            }
            else if (_cooperator == null)
            {
                _cooperator = new DataTable();
            }
            // And then update the binding source's data...
            _cooperatorBindingSource.DataSource = _cooperator;
        }

        private void RefreshWebCooperatorList()
        {
            DataSet ds = new DataSet();
            string findText = "";
            string searchPKeys = "";

            // First get the cooperator_ids for the orginal search criteria (_originalPKeys)...
            ds = _sharedUtils.GetWebServiceData("get_web_cooperator", _originalPKeys, 0, 0);
            searchPKeys = ":webcooperatorid=";
            if (ds.Tables.Contains("get_web_cooperator"))
            {
                foreach (DataRow dr in ds.Tables["get_web_cooperator"].Rows)
                {
                    searchPKeys += dr["web_cooperator_id"].ToString() + ",";
                }
            }

            // Now get the web_cooperator_ids from the local LU table that match the web_lastname and web_firstname...
            if (!string.IsNullOrEmpty(ux_textboxWebLastName.Text)) findText += ux_textboxWebLastName.Text + "%";
            if (!string.IsNullOrEmpty(ux_textboxWebFirstName.Text)) findText += ux_textboxWebFirstName.Text + "%";
            if (!string.IsNullOrEmpty(findText))
            {
                DataTable dt = new DataTable();

                dt = _sharedUtils.LookupTablesGetMatchingRows("web_cooperator_lookup", findText, 1000);
                foreach (DataRow dr in dt.Rows)
                {
                    searchPKeys += dr["value_member"].ToString() + ",";
                }
            }

            // Remove the last trailing comma...
            searchPKeys = searchPKeys.TrimEnd(',');

            // Finally go get the full collection of web_cooperator_ids...
            ds = _sharedUtils.GetWebServiceData("get_web_cooperator", searchPKeys, 0, 0);
            if (ds.Tables.Contains("get_web_cooperator"))
            {
                _webCooperator = ds.Tables["get_web_cooperator"].Copy();
            }
            else if (_webCooperator == null)
            {
                _webCooperator = new DataTable();
            }
            // And then update the binding source's data...
            _webCooperatorBindingSource.DataSource = _webCooperator;
        }

        private int SaveCooperatorData()
        {
            int errorCount = 0;
            DataSet cooperatorChanges = new DataSet();
            DataSet cooperatorSaveResults = new DataSet();

            // Process COOPERATOR...
            // Make sure the last edited row in the Accessions Form has been commited to the datatable...
            _cooperatorBindingSource.EndEdit();

            // Make sure the navigator is not currently editing a cell...
            foreach (DataRowView drv in _cooperatorBindingSource.List)
            {
                if (drv.IsEdit ||
                    drv.Row.RowState == DataRowState.Added ||
                    drv.Row.RowState == DataRowState.Deleted ||
                    drv.Row.RowState == DataRowState.Detached ||
                    drv.Row.RowState == DataRowState.Modified)
                {
                    drv.EndEdit();
                    //drv.Row.ClearErrors();
                }
            }

            // Get the changes (if any) for the accession table and commit them to the remote database...
            if (_cooperator.GetChanges() != null)
            {
                cooperatorChanges.Tables.Add(_cooperator.GetChanges());
                ScrubData(cooperatorChanges);
                // Save the changes to the remote server...
                cooperatorSaveResults = _sharedUtils.SaveWebServiceData(cooperatorChanges);
                if (cooperatorSaveResults.Tables.Contains(_cooperator.TableName))
                {
                    errorCount += SyncSavedResults(_cooperator, cooperatorSaveResults.Tables[_cooperator.TableName]);
                }
            }

            // Now add the new changes to the _changedRecords dataset (this data will be passed back to the calling program)...
            if (cooperatorSaveResults != null && cooperatorSaveResults.Tables.Contains(_cooperator.TableName))
            {
                string pkeyName = cooperatorSaveResults.Tables[_cooperator.TableName].PrimaryKey[0].ColumnName;
                bool origColumnReadOnlyValue = cooperatorSaveResults.Tables[_cooperator.TableName].Columns[pkeyName].ReadOnly;
                foreach (DataRow dr in cooperatorSaveResults.Tables[_cooperator.TableName].Rows)
                {
                    dr.Table.Columns[pkeyName].ReadOnly = false;
                    dr[pkeyName] = dr["NewPrimaryKeyID"];
                    dr.AcceptChanges();
                }
                cooperatorSaveResults.Tables[_cooperator.TableName].Columns[pkeyName].ReadOnly = origColumnReadOnlyValue;

                if (_changedRecords.Tables.Contains(_cooperator.TableName))
                {
                    // If the saved results table exists - update or insert the new records...
                    _changedRecords.Tables[_cooperator.TableName].Load(cooperatorSaveResults.Tables[_cooperator.TableName].CreateDataReader(), LoadOption.Upsert);
                    _changedRecords.Tables[_cooperator.TableName].AcceptChanges();

                }
                else
                {
                    // If the saved results table doesn't exist - create it (and include the new records)...
                    _changedRecords.Tables.Add(cooperatorSaveResults.Tables[_cooperator.TableName].Copy());
                    _changedRecords.AcceptChanges();
                }
            }

            return errorCount;
        }

        private void ScrubData(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ExtendedProperties.Contains("is_nullable") &&
                            dc.ExtendedProperties["is_nullable"].ToString().Trim().ToUpper() == "N" &&
                            dr[dc] == DBNull.Value)
                        {
                            if (dc.ExtendedProperties.Contains("default_value") &&
                                !string.IsNullOrEmpty(dc.ExtendedProperties["default_value"].ToString()) &&
                                dc.ExtendedProperties["default_value"].ToString().Trim().ToUpper() != "{DBNULL.VALUE}")
                            {
                                dr[dc] = dc.ExtendedProperties["default_value"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private int SyncSavedResults(DataTable originalTable, DataTable savedResults)
        {
            int errorCount = 0;

            if (savedResults != null && savedResults.PrimaryKey.Length == 1)
            {
                string pKeyCol = savedResults.PrimaryKey[0].ColumnName.Trim().ToUpper();
                savedResults.Columns[pKeyCol].ReadOnly = false;
                foreach (DataRow dr in savedResults.Rows)
                {
                    DataRow originalRow = originalTable.Rows.Find(dr["OriginalPrimaryKeyID"]);

                    switch (dr["SavedAction"].ToString())
                    {
                        case "Insert":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Set the originalTable row's status for this new row to committed (and update the pkey with the int returned from the server DB)...
                                if (originalRow != null)
                                {
                                    bool origColumnReadOnlyValue = originalRow.Table.Columns[pKeyCol].ReadOnly;
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = false;
                                    originalRow[pKeyCol] = dr["NewPrimaryKeyID"];
                                    originalRow.AcceptChanges();
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = origColumnReadOnlyValue;
                                    originalRow.ClearErrors();
                                }
                            }
                            else
                            {
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Update":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                originalRow.AcceptChanges();
                                originalRow.ClearErrors();
                            }
                            else
                            {
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Delete":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Set the row's status for this deleted row to committed...
                                if (originalRow != null)
                                {
                                    originalRow.AcceptChanges();
                                    originalRow.ClearErrors();
                                }
                            }
                            else
                            {
                                errorCount++;
                                // Find the deleted row (NOTE: datatable.rows.find() method does not work on deleted rows)...
                                foreach (DataRow deletedRow in originalTable.Rows)
                                {
                                    if (deletedRow[0, DataRowVersion.Original].Equals(dr["OriginalPrimaryKeyID"]))
                                    {
                                        deletedRow.RejectChanges();
                                        deletedRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return errorCount;
        }

        private void ux_buttonNewCooperator_Click(object sender, EventArgs e)
        {
            // Create a new cooperator table row...
            DataRow newCooperator = _cooperator.NewRow();
            // Add it to the cooperator table...
            _cooperator.Rows.Add(newCooperator);
            // Navigate to the first editable cell in the DGV's new row...
            int newRowIndex = ux_datagridviewCooperator.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewCooperator.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            // First find the DGV row that contains the new table row created in the code above...
            for (int i = 0; i < ux_datagridviewCooperator.Rows.Count; i++)
            {
                if (ux_datagridviewCooperator["cooperator_id", i].Value.Equals(newCooperator["cooperator_id"])) newRowIndex = i;
            }
            // Now iterate through all of the columns until you find the first displayed column in the DGV...
            foreach (DataGridViewColumn dgvc in ux_datagridviewCooperator.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            // Set the DGV current cell to the first editable cell in the new row...
            ux_datagridviewCooperator.CurrentCell = ux_datagridviewCooperator[newColIndex, newRowIndex];
        }

        private void ux_buttonSave_Click(object sender, EventArgs e)
        {
            int errorCount = 0;
            errorCount = SaveCooperatorData();
            if (errorCount == 0)
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Cooperator Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "CooperatorWizard_ux_buttonSaveMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                ggMessageBox.ShowDialog();
            }
            else
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\n  Error Count: {0}", "Cooperator Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "CooperatorWizard_ux_buttonSaveMessage2";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
                ggMessageBox.ShowDialog();
            }
            // Refresh the formatting of the cells in the DGV on the Cooperator tab page...
            RefreshDGVFormatting(ux_datagridviewCooperator);
        }

        private void ux_buttonSaveAndExit_Click(object sender, EventArgs e)
        {
            int errorCount = 0;
            errorCount = SaveCooperatorData();
            if (errorCount == 0)
            {
                //MessageBox.Show(this, "All data was saved successfully", "Accession Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Cooperator Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "CooperatorWizard_ux_buttonSaveMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                ggMessageBox.ShowDialog();
                this.Close();
            }
            else
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Cooperator Wizard).\n\n  Error Count: {0}", "Cooperator Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "CooperatorWizard_ux_buttonSaveMessage3";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
                //if (DialogResult.No == MessageBox.Show(this, "The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Accession Wizard).\n\n  Error Count: " + errorCount, "Accession Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
                if (DialogResult.No == ggMessageBox.ShowDialog())
                {
                    this.Close();
                }
                else
                {
                    // Update the row error message for this cooperator row...
                    //if (string.IsNullOrEmpty(((DataRowView)_cooperatorBindingSource.Current).Row.RowError))
                    //{
                    //    ux_textboxCooperatorRowError.Visible = false;
                    //    ux_textboxCooperatorRowError.Text = "";
                    //}
                    //else
                    //{
                    //    ux_textboxCooperatorRowError.Visible = true;
                    //    ux_textboxCooperatorRowError.ReadOnly = false;
                    //    ux_textboxCooperatorRowError.Enabled = true;
                    //    ux_textboxCooperatorRowError.Text = ((DataRowView)_cooperatorBindingSource.Current).Row.RowError;
                    //}
                }
            }
            // Refresh the formatting of the cells in the DGV on the Cooperator tab page...
            RefreshDGVFormatting(ux_datagridviewCooperator);
        }

        private void ux_buttonCreateNewCooperator_Click(object sender, EventArgs e)
        {
            int firstNewRowIndex = int.MaxValue;
            foreach (DataGridViewRow dgvr in ux_datagridviewWebCooperator.SelectedRows)
            {
                // Create a new cooperator table row...
                DataRow newCooperator = _cooperator.NewRow();
                // Populate the row with data from the web_cooperator table's selected row...
                DataRow webCooperator = _webCooperator.Rows.Find(((DataRowView)dgvr.DataBoundItem).Row["web_cooperator_id"]);
                if (webCooperator != null)
                {
                    if (_cooperator.Columns.Contains("last_name") && _webCooperator.Columns.Contains("last_name")) newCooperator["last_name"] = webCooperator["last_name"];
                    if (_cooperator.Columns.Contains("title") && _webCooperator.Columns.Contains("title")) newCooperator["title"] = webCooperator["title"];
                    if (_cooperator.Columns.Contains("first_name") && _webCooperator.Columns.Contains("first_name")) newCooperator["first_name"] = webCooperator["first_name"];
                    if (_cooperator.Columns.Contains("organization") && _webCooperator.Columns.Contains("organization")) newCooperator["organization"] = webCooperator["organization"];
                    if (_cooperator.Columns.Contains("address_line1") && _webCooperator.Columns.Contains("address_line1")) newCooperator["address_line1"] = webCooperator["address_line1"];
                    if (_cooperator.Columns.Contains("address_line2") && _webCooperator.Columns.Contains("address_line2")) newCooperator["address_line2"] = webCooperator["address_line2"];
                    if (_cooperator.Columns.Contains("address_line3") && _webCooperator.Columns.Contains("address_line3")) newCooperator["address_line3"] = webCooperator["address_line3"];
                    if (_cooperator.Columns.Contains("city") && _webCooperator.Columns.Contains("city")) newCooperator["city"] = webCooperator["city"];
                    if (_cooperator.Columns.Contains("postal_index") && _webCooperator.Columns.Contains("postal_index")) newCooperator["postal_index"] = webCooperator["postal_index"];
                    if (_cooperator.Columns.Contains("geography_id") && _webCooperator.Columns.Contains("geography_id")) newCooperator["geography_id"] = webCooperator["geography_id"];
                    if (_cooperator.Columns.Contains("primary_phone") && _webCooperator.Columns.Contains("primary_phone")) newCooperator["primary_phone"] = webCooperator["primary_phone"];
                    if (_cooperator.Columns.Contains("email") && _webCooperator.Columns.Contains("email")) newCooperator["email"] = webCooperator["email"];
                }
                // Add it to the cooperator table...
                _cooperator.Rows.Add(newCooperator);
                // Remember the first new row in the cooperator DGV...
                if (firstNewRowIndex == int.MaxValue) firstNewRowIndex = (int)newCooperator["cooperator_id"];
            }

            if (firstNewRowIndex != int.MaxValue)
            {
                // Navigate to the first editable cell in the DGV's new row...
                int newRowIndex = ux_datagridviewCooperator.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
                int newColIndex = 0; //ux_datagridviewCooperator.Columns.gGetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
                // First find the DGV row that contains the new table row created in the code above...
                for (int i = 0; i < ux_datagridviewCooperator.Rows.Count; i++)
                {
                    if (ux_datagridviewCooperator["cooperator_id", i].Value.Equals(firstNewRowIndex)) newRowIndex = i;
                }
                // Now iterate through all of the columns until you find the first displayed column in the DGV...
                foreach (DataGridViewColumn dgvc in ux_datagridviewCooperator.Columns)
                {
                    if (dgvc.DisplayIndex == 0)
                    {
                        newColIndex = dgvc.Index;
                        break;
                    }
                }
                // Set the DGV current cell to the first editable cell in the new row...
                ux_datagridviewCooperator.CurrentCell = ux_datagridviewCooperator[newColIndex, newRowIndex];
                ux_tabcontrolMain.SelectedTab = CooperatorPage;
                ux_datagridviewCooperator.Focus();
            }
        }

    }
}
