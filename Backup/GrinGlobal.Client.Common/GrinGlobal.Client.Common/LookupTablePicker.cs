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
    public partial class LookupTablePicker : Form
    {
        SharedUtils _sharedUtils;
        private DataTable _boundTable;
        private Dictionary<string, string> _filters;
        private string _currentKey = "";
        private string _newKey = "";
        private string _newValue = "";
        private Timer textChangeDelayTimer = new Timer();
        private string _lookupTableName = "";

        public LookupTablePicker(SharedUtils sharedUtils, string columnNameToLookup, DataRow parentRow, string currentValue)
        {
            InitializeComponent();
            // Initialize new objects to access the local lookup tables...
            _sharedUtils = sharedUtils;
            // Save the key for the cell that is being edited...
            _currentKey = parentRow[columnNameToLookup].ToString();
            _newKey = _currentKey;
            // Create new filter dictionary...
            _filters = new Dictionary<string, string>();
            // Get the table for the datarow being edited...
            DataTable dt = parentRow.Table;
            // Get the column for the cell being edited...
            DataColumn dc = parentRow.Table.Columns[columnNameToLookup];
            // Make sure this is an FK column and if so inspect the fields available
            // in the lookup table that might match fields in the parentRow's table
            // If there are matches - wire them up as filters to restrict the number
            // of rows returned to the dialog box that the user chooses from...
            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                //_lookupTableName = dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim();
                _lookupTableName = dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim();
                if (_sharedUtils.LocalDatabaseTableExists(_lookupTableName))
                {
                    // This is a query to 1) get data for the current FK and also 2) get the schema for the lookup table...
                    _boundTable = _sharedUtils.GetLocalData("SELECT TOP 1000 * FROM " + _lookupTableName + " WHERE value_member = @valuemember" + " ORDER BY display_member ASC", "@valuemember=" + _currentKey);
                }
                else
                {
//MessageBox.Show("Warning!!!\n\nYour computer does not have a local copy of this lookup table.  Until this lookup table is downloaded, the Curator Tool will connect to the central database to retrieve lookup values.\n\nTIP:  To maximize performance of the Curator Tool please download all lookup tables to your local computer.", "Missing Lookup Table", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Warning!!!\n\nYour computer does not have a local copy of this lookup table.  Until this lookup table is downloaded, the Curator Tool will connect to the central database to retrieve lookup values.\n\nTIP:  To maximize performance of the Curator Tool please download all lookup tables to your local computer.", "Missing Lookup Table", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "LookupTablePicker_LookupTablePickerMessage1";
if (_sharedUtils != null) _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
//_boundTable = _lookupTables.GetFilteredLookupTableRows(_lookupTableName, ux_textboxFind.Text, 1000);
_boundTable = _sharedUtils.LookupTablesGetMatchingRows(_lookupTableName, currentValue, 1000);
                    ux_buttonRefresh.Enabled = false;
                }

                if (_boundTable != null)
                {
                    foreach (DataColumn luColumn in _boundTable.Columns)
                    {
                        if (luColumn.ColumnName != "value_member" &&
                            luColumn.ColumnName != "display_member" &&
                            luColumn.ColumnName != "created_date" &&
                            luColumn.ColumnName != "modified_date")
                        {
                            if (dt.Columns.Contains(luColumn.ColumnName) &&
                                parentRow[luColumn.ColumnName] != null &&
                                !luColumn.ColumnName.StartsWith("is_")) _filters.Add(luColumn.ColumnName, parentRow[luColumn.ColumnName].ToString().Trim());
                            if (luColumn.ColumnName.StartsWith("is_") &&
                                !_filters.ContainsKey(luColumn.ColumnName)) _filters.Add(luColumn.ColumnName, "Y");
                        }
                    }
                }
            }
            textChangeDelayTimer.Tick += new EventHandler(timerDelay_Tick);
            ux_textboxFind.Text = currentValue;
            ux_textboxFind.Focus();
            ux_textboxFind.SelectionStart = ux_textboxFind.Text.Length;
sharedUtils.UpdateControls(this.Controls, this.Name);
        }

        public string NewKey
        {
            get
            {
                return _newKey;
            }
        }

        public string NewValue
        {
            get
            {
                return _newValue;
            }
        }

        private void LookupPicker_Load(object sender, EventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            ux_labelListMustMatch.Visible = false;
            ux_checkedlistboxFilters.Visible = false;
            ux_listboxLookupDisplayMember.Width = this.FindForm().Width - 32;
            foreach (string filterKey in _filters.Keys)
            {
                // NOTE: Each time a new item is added to the checkedListBox with checked = true
                //       the ux_checkedlistboxFilters_ItemCheck event modifies the _lookupTable RowFilter
                ux_checkedlistboxFilters.Items.Add(filterKey, true);
                ux_labelListMustMatch.Visible = true;
                ux_checkedlistboxFilters.Visible = true;
                ux_listboxLookupDisplayMember.Width = this.FindForm().Width - ux_checkedlistboxFilters.Width - 6 - 32;
            }

            ux_listboxLookupDisplayMember.DisplayMember = "display_member";
            ux_listboxLookupDisplayMember.ValueMember = "value_member";
            _boundTable.DefaultView.Sort = "display_member ASC";
            ux_listboxLookupDisplayMember.DataSource = _boundTable;  // NOTE: this could be null if lookup tables do not exist yet.
            if (ux_listboxLookupDisplayMember.GetItemText(_currentKey) != null && ux_listboxLookupDisplayMember.GetItemText(_currentKey).Length > 0)
            {
                ux_listboxLookupDisplayMember.SelectedValue = _currentKey;
            }
            ux_textboxFind.Focus();
            ux_textboxFind.SelectionStart = ux_textboxFind.Text.Length;

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void ux_textboxFind_TextChanged(object sender, EventArgs e)
        {
            textChangeDelayTimer.Stop();
            textChangeDelayTimer.Interval = 1000;
            textChangeDelayTimer.Start();
        }

        void timerDelay_Tick(object sender, EventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            textChangeDelayTimer.Stop();
            RefreshList();

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void RefreshList()
        {
            DataTable dt = new DataTable();
            string rowFilter = "";

            if(_boundTable != null) rowFilter = _boundTable.DefaultView.RowFilter;

            // Break the row filter up into tokens...
            string[] tokens = rowFilter.Split(new string[1] { " AND " }, StringSplitOptions.RemoveEmptyEntries);

            // Start rebuilding the row filter for all criteria except display_member...
            rowFilter = "";
            foreach (string token in tokens)
            {
                if (!token.Contains("display_member")) rowFilter += token + " AND ";
            }
            // Now add back in the display_member row filter...
            rowFilter += "display_member like @displaymember + '%'";


            if (_sharedUtils.LocalDatabaseTableExists(_lookupTableName))
            {
                // Query the local copy of the lookup...
                // Get rows from the full lookup table that match the 'Find Filter' text...
                dt = _sharedUtils.GetLocalData("SELECT TOP 1000 * FROM " + _boundTable.TableName + " WHERE " + rowFilter + " ORDER BY display_member ASC", "@displaymember=" + ux_textboxFind.Text);
            }
            else
            {
                dt = _sharedUtils.LookupTablesGetMatchingRows(_lookupTableName, ux_textboxFind.Text, 1000);
            }

            // First clear the bound table...
            _boundTable.Clear();
            _boundTable.Load(dt.CreateDataReader(), LoadOption.Upsert);
            // Apply the row filter to the returned data...
            _boundTable.DefaultView.RowFilter = rowFilter.Replace("@displaymember + '%'", "'" + ux_textboxFind.Text.Replace("'", "''") + "%'");

            ux_textboxFind.Focus();
            ux_textboxFind.SelectionStart = ux_textboxFind.Text.Length;
        }

        private void ux_checkedlistboxFilters_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            //if (((ListBox)sender).SelectedItem == null) return;
            string rowFilter = _boundTable.DefaultView.RowFilter;

            // Break the row filter up into tokens...
            string[] tokens = rowFilter.Split(new string[1] { " AND " }, StringSplitOptions.RemoveEmptyEntries);

            // Start rebuilding the row filter for all criteria EXCEPT the criterion for
            // the display_member or the checkbox item currently being changed...
            rowFilter = "";
            foreach (string token in tokens)
            {
                if (!token.Contains("display_member") && 
                    !token.Contains(_filters.Keys.ElementAt(e.Index).Trim())) rowFilter += token + " AND ";
            }

            // If the current item is being checked add it to the row filter too...
            if (e.NewValue == CheckState.Checked)
            {
                rowFilter += _filters.Keys.ElementAt(e.Index).Trim() + "='" + _filters.Values.ElementAt(e.Index).Trim() + "' AND ";
            }

            // Now add back in the display_member row filter...
            rowFilter += "display_member like @displaymember + '%'";

            if (_sharedUtils.LocalDatabaseTableExists(_boundTable.TableName))
            {
                // First clear the table...
                _boundTable.Clear();
                _boundTable.DefaultView.RowFilter = "";

                // Get rows from the full lookup table that match the 'Find Filter' text...
                DataTable dt = _sharedUtils.GetLocalData("SELECT TOP 1000 * FROM " + _boundTable.TableName + " WHERE " + rowFilter + " ORDER BY display_member ASC", "@displaymember=" + ux_textboxFind.Text);
                _boundTable.Load(dt.CreateDataReader(), LoadOption.Upsert);
            }
            _boundTable.DefaultView.RowFilter = rowFilter.Replace("@displaymember + '%'", "'" + ux_textboxFind.Text.Replace("'", "''") + "%'");

            ux_textboxFind.Focus();
            ux_textboxFind.SelectionStart = ux_textboxFind.Text.Length;

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void ux_buttonOK_Click(object sender, EventArgs e)
        {
            if (ux_listboxLookupDisplayMember.SelectedValue != null)
            {
                _newKey = ux_listboxLookupDisplayMember.SelectedValue.ToString();
                _newValue = ux_listboxLookupDisplayMember.Text;
            }
            else
            {
                _newKey = null;
                _newValue = ux_listboxLookupDisplayMember.Text;
            }
            ux_listboxLookupDisplayMember.DataSource = null;
            _boundTable.DefaultView.RowFilter = "";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            ux_listboxLookupDisplayMember.DataSource = null;
            _boundTable.DefaultView.RowFilter = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ux_buttonRefresh_Click(object sender, EventArgs e)
        {
            if (_sharedUtils.LocalDatabaseTableExists(_boundTable.TableName))
            {
                _sharedUtils.LookupTablesUpdateTable(_boundTable.TableName, false);
                RefreshList();
            }
        }

        private void ux_listboxLookupDisplayMember_DoubleClick(object sender, EventArgs e)
        {
            ux_buttonOK.PerformClick();
        }
    }
}
