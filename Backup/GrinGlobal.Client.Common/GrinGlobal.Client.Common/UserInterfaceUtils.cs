using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace GRINGlobal.Client.Common
{
    class UserInterfaceUtils
    {
        WebServices _webServices;
        public UserInterfaceUtils(WebServices webServices)
        {
            _webServices = webServices;
        }

        #region Dataview TabControl methods...

        public void buildDataviewTabControl(TabControl ux_tabcontrolDataview, SharedUtils sharedUtils)
        {
            int numDataviewTabs = -1;
            
            // Clear the dataview tabs...
            ux_tabcontrolDataview.TabPages.Clear();

            // Now add back in the tabpage for adding new tabpages...
            if (!ux_tabcontrolDataview.TabPages.ContainsKey("ux_tabpageDataviewNewTab"))
            {
                ux_tabcontrolDataview.TabPages.Add("ux_tabpageDataviewNewTab", "...");
            }

            // Get the number of tabpages saved in the current user's settings...
            int.TryParse(sharedUtils.GetUserSetting(ux_tabcontrolDataview.Name, "TabPages.Count", "-1"), out numDataviewTabs);

            // Create the user's saved dataview tabs...
            if (numDataviewTabs > 0)
            {
                for (int i = 0; i < numDataviewTabs; i++)
                {
                    DataviewProperties dp = new DataviewProperties();
                    dp.TabName = sharedUtils.GetUserSetting(ux_tabcontrolDataview.Name, "TabPages[" + i.ToString() + "].TabName", "");
                    dp.DataviewName = sharedUtils.GetUserSetting(ux_tabcontrolDataview.Name, "TabPages[" + i.ToString() + "].DataviewName", "");
                    dp.StrongFormName = sharedUtils.GetUserSetting(ux_tabcontrolDataview.Name, "TabPages[" + i.ToString() + "].FormName", "");
                    dp.ViewerStyle = sharedUtils.GetUserSetting(ux_tabcontrolDataview.Name, "TabPages[" + i.ToString() + "].ViewerStyle", "");
                    dp.AlwaysOnTop = sharedUtils.GetUserSetting(ux_tabcontrolDataview.Name, "TabPages[" + i.ToString() + "].AlwaysOnTop", "");
                    ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(i, ux_tabcontrolDataview.TabPages.IndexOfKey("ux_tabpageDataviewNewTab")));
                }
            }
            else
            {
                // Make the default Accession dataview tab...
                DataviewProperties dp = new DataviewProperties();
                dp.TabName = "Accessions";
                dp.DataviewName = "get_accession";
                dp.StrongFormName = "";
                dp.ViewerStyle = "Spreadsheet";
                dp.AlwaysOnTop = "false";
                ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(0, ux_tabcontrolDataview.TabPages.IndexOfKey("ux_tabpageDataviewNewTab")));
                // Make the default Inventory dataview tab...
                dp = new DataviewProperties();
                dp.TabName = "Inventory";
                dp.DataviewName = "get_inventory";
                dp.StrongFormName = "";
                dp.ViewerStyle = "Spreadsheet";
                dp.AlwaysOnTop = "false";
                ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(1, ux_tabcontrolDataview.TabPages.IndexOfKey("ux_tabpageDataviewNewTab")));
                // Make the default Orders dataview tab...
                dp = new DataviewProperties();
                dp.TabName = "Orders";
                dp.DataviewName = "get_order_request";
                dp.StrongFormName = "";
                dp.ViewerStyle = "Spreadsheet";
                dp.AlwaysOnTop = "false";
                ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(2, ux_tabcontrolDataview.TabPages.IndexOfKey("ux_tabpageDataviewNewTab")));
                // Make the default Cooperators dataview tab...
                dp = new DataviewProperties();
                dp.TabName = "Cooperators";
                dp.DataviewName = "get_cooperator";
                dp.StrongFormName = "";
                dp.ViewerStyle = "Spreadsheet";
                dp.AlwaysOnTop = "false";
                ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(3, ux_tabcontrolDataview.TabPages.IndexOfKey("ux_tabpageDataviewNewTab")));
            }
            // Make the first tab active...
            ux_tabcontrolDataview.SelectedTab = ux_tabcontrolDataview.TabPages[0];
        }

        public void ux_tabcontrolCreateNewTab(TabControl ux_tabcontrolDataview, int indexOfNewTab)
        {
            DataviewProperties dvp = new DataviewProperties();
            dvp.TabName = "";
            dvp.DataviewName = "";
            dvp.StrongFormName = "";
            dvp.ViewerStyle = "Spreadsheet";
            dvp.AlwaysOnTop = "false";

            DataviewTabProperties newTabDialog = new DataviewTabProperties(_webServices, dvp);
            newTabDialog.StartPosition = FormStartPosition.CenterParent;
            if (newTabDialog.ShowDialog() == DialogResult.OK)
            {
                ux_tabcontrolAddTab(ux_tabcontrolDataview, newTabDialog.TabProperties.TabName, newTabDialog.TabProperties, indexOfNewTab);
                ux_tabcontrolDataview.SelectedIndex = indexOfNewTab;
                //SetDGVMainDataviewUserSettings();
            }
            else
            {
                ux_tabcontrolDataview.DeselectTab(indexOfNewTab);
            }
        }

        public void ux_tabcontrolAddTab(TabControl ux_tabcontrolDataview, string text, DataviewProperties tag, int position)
        {
            foreach (TabPage tp in ux_tabcontrolDataview.TabPages)
            {
                // If this tab page is already in the collection - bail out...
                if (tp.Text.ToUpper() == text.ToUpper() && ((DataviewProperties)tp.Tag).DataviewName == tag.DataviewName) return;
            }
            TabPage newTabPage = new TabPage();
            newTabPage.Text = text;
            newTabPage.Tag = tag;
            ux_tabcontrolDataview.TabPages.Insert(position, newTabPage);
        }

        public void ux_tabcontrolShowProperties(TabControl ux_tabcontrolDataview, int indexOfCurrentTab)
        {
            DataviewProperties dvp;
            if (ux_tabcontrolDataview.SelectedTab != null &&
                ux_tabcontrolDataview.SelectedTab.Tag != null &&
                ux_tabcontrolDataview.SelectedTab.Tag.GetType() == typeof(DataviewProperties) &&
                !string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).TabName))
            {
                dvp = (DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag;
            }
            else
            {
                dvp = new DataviewProperties();
                dvp.TabName = ux_tabcontrolDataview.SelectedTab.Text;
                dvp.DataviewName = (string)ux_tabcontrolDataview.SelectedTab.Tag;
                dvp.StrongFormName = "";
                dvp.ViewerStyle = "Spreadsheet";
                dvp.AlwaysOnTop = "false";
            }

            DataviewTabProperties dataviewTabPropertiesDialog = new DataviewTabProperties(_webServices, dvp);
            dataviewTabPropertiesDialog.StartPosition = FormStartPosition.CenterParent;
            if (dataviewTabPropertiesDialog.ShowDialog() == DialogResult.OK)
            {
                ux_tabcontrolDataview.TabPages[indexOfCurrentTab].Tag = dataviewTabPropertiesDialog.TabProperties;
                ux_tabcontrolDataview.TabPages[indexOfCurrentTab].Text = ((DataviewProperties)ux_tabcontrolDataview.TabPages[indexOfCurrentTab].Tag).TabName;
            }
        }

        public void ux_tabcontrolRemoveTab(TabControl ux_tabcontrolDataview, int indexOfTabToDelete)
        {
            // Deselect the tab before deleting it...
            ux_tabcontrolDataview.SelectedIndex = -1;
            // Delete the tab (unless this is the last tab remaining)...
            if (ux_tabcontrolDataview.TabPages.Count >= 3) ux_tabcontrolDataview.TabPages.RemoveAt(indexOfTabToDelete);
            // Attempt to select another tab (to auto-refresh the DGV)...
            int newSelectedTabIndex = Math.Max(0, indexOfTabToDelete - 1);
            ux_tabcontrolDataview.SelectedIndex = newSelectedTabIndex;
        }

        public void ux_tabcontrolMouseDownEvent(TabControl ux_tabcontrolDataview, MouseEventArgs e)
        {
            int clickedTabPage = ux_tabcontrolDataview.SelectedIndex;

            // Attempt to find the tabpage that was clicked...
            for (int i = 0; i < ux_tabcontrolDataview.TabPages.Count; i++)
            {
                if (ux_tabcontrolDataview.GetTabRect(i).Contains(e.Location)) clickedTabPage = i; //MessageBox.Show(ux_tabcontrolDataview.TabPages[i].Text + " : " + e.Location.ToString());
            }

            if (e.Button == MouseButtons.Left)
            {
                // Begin tabpage drag and drop move (if the clicked tab is not 
                // the "ux_tabpageDataviewNewTab" tabpage - which is use to add new dataviews)...
                if (ux_tabcontrolDataview.TabPages[clickedTabPage] != ux_tabcontrolDataview.TabPages["ux_tabpageDataviewNewTab"])
                {
                    ux_tabcontrolDataview.DoDragDrop(ux_tabcontrolDataview.TabPages[clickedTabPage], DragDropEffects.Move);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Make the right clicked tabpage the selected tabpage for the control...
                ux_tabcontrolDataview.SelectTab(clickedTabPage);
            }
        }

        public void ux_tabcontrolDragOverEvent(TabControl ux_tabcontrolDataview, DragEventArgs e)
        {
            // Convert the mouse coordinates from screen to client...
            System.Drawing.Point ptClientCoord = ux_tabcontrolDataview.PointToClient(new System.Drawing.Point(e.X, e.Y));
            int destinationTabPage = ux_tabcontrolDataview.TabPages.IndexOf((TabPage)e.Data.GetData(typeof(TabPage)));

            // Attempt to find the tabpage that is being dragged over...
            for (int i = 0; i < ux_tabcontrolDataview.TabPages.Count; i++)
            {
                if (ux_tabcontrolDataview.GetTabRect(i).Contains(ptClientCoord)) destinationTabPage = i;
            }

            if (e.Data.GetDataPresent(typeof(TabPage)) &&
                ux_tabcontrolDataview.TabPages[destinationTabPage] != (TabPage)e.Data.GetData(typeof(TabPage)) /* &&
                destinationTabPage != ux_tabcontrolDataview.TabPages.IndexOfKey("ux_tabpageDataviewNewTab")*/
                                                                                                                      )
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void ux_tabcontrolDragDropEvent(TabControl ux_tabcontrolDataview, DragEventArgs e)
        {
            if (e.AllowedEffect == e.Effect)
            {
                // Convert the mouse coordinates from screen to client...
                System.Drawing.Point ptClientCoord = ux_tabcontrolDataview.PointToClient(new System.Drawing.Point(e.X, e.Y));

                int destinationTabPageIndex = -1;
                int originalTabPageIndex = -1;

                // Attempt to find where the tabpage should be dropped...
                for (int i = 0; i < ux_tabcontrolDataview.TabPages.Count; i++)
                {
                    if (ux_tabcontrolDataview.TabPages[i] == e.Data.GetData(typeof(TabPage))) originalTabPageIndex = i;
                    if (ux_tabcontrolDataview.GetTabRect(i).Contains(ptClientCoord)) destinationTabPageIndex = i;
                }

                // Now create a copy of the tabpage that is being moved so that 
                // you can remove the orginal and insert the copy at the right spot...
                TabPage newTabPage = new TabPage();
                newTabPage.Text = ((TabPage)e.Data.GetData(typeof(TabPage))).Text;
                newTabPage.Tag = ((TabPage)e.Data.GetData(typeof(TabPage))).Tag;
                ux_tabcontrolDataview.TabPages.Insert(destinationTabPageIndex, newTabPage);
                ux_tabcontrolDataview.SelectTab(destinationTabPageIndex);
                if (originalTabPageIndex < destinationTabPageIndex)
                {
                    ux_tabcontrolDataview.TabPages.RemoveAt(originalTabPageIndex);
                }
                else
                {
                    ux_tabcontrolDataview.TabPages.RemoveAt(originalTabPageIndex + 1);
                }
            }
        }

        #endregion

        public string GetFriendlyFieldName(DataColumn dc, string defaultName)
        {
            string friendlyFieldName = defaultName;
            // Try to find the friendly_field_name first...
            if (dc.ExtendedProperties.Contains("friendly_field_name") &&
                dc.ExtendedProperties["friendly_field_name"].ToString().Length > 0)
            {
                friendlyFieldName = dc.ExtendedProperties["friendly_field_name"].ToString();
            }
            // Then try to find the title next...
            if (dc.ExtendedProperties.Contains("title") &&
                dc.ExtendedProperties["title"].ToString().Length > 0)
            {
                friendlyFieldName = dc.ExtendedProperties["title"].ToString();
            }
            // Otherwise the caption property should have the friendly name
            else if (dc.Caption.Length > 0)
            {
                friendlyFieldName = dc.Caption;
            }
            // Fallback to the ColumnName if all else fails...
            else
            {
                friendlyFieldName = dc.ColumnName;
            }
            // If everything else has failed use the default name passed in...
            if (friendlyFieldName.Length == 0) friendlyFieldName = defaultName;

            return friendlyFieldName;
        }

        #region Building the Read-Only version of the DataGridView based on the DataTable...

        public void buildReadOnlyDataGridView(DataGridView dataGridView, DataTable dataTable, LookupTables lookupTables)
        {
            DataTable newDataTable = new DataTable(dataTable.TableName);
            System.Collections.Generic.List<DataColumn> pKeys = new System.Collections.Generic.List<DataColumn>();

            // Clear the DGV columns and set the autogenerate columns property...
            dataGridView.Columns.Clear();
            dataGridView.AutoGenerateColumns = true;


            // First build the columns of the new table...
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                string newColumnName = dataColumn.ColumnName;
                newDataTable.Columns.Add(dataColumn.ColumnName);
                // Set the column header...
                newDataTable.Columns[newColumnName].Caption = GetFriendlyFieldName(dataColumn, dataColumn.ColumnName);

                // Add the extended properties from the source table...
                foreach (string key in dataColumn.ExtendedProperties.Keys)
                {
                    newDataTable.Columns[newColumnName].ExtendedProperties.Add(key, dataColumn.ExtendedProperties[key]);
                }
                // Add this column to the primary keys list if extended properties indicate it is a primary key column
                if (newDataTable.Columns[newColumnName].ExtendedProperties.Contains("is_primary_key") &&
                    newDataTable.Columns[newColumnName].ExtendedProperties["is_primary_key"].ToString() == "Y")
                {
                    pKeys.Add(newDataTable.Columns[newColumnName]);
                }

                if (dataColumn.ExtendedProperties.Contains("gui_hint"))
                {
                    switch (dataColumn.ExtendedProperties["gui_hint"].ToString())
                    {
                        case "LARGE_SINGLE_SELECT_CONTROL":
                            break;
                        case "SMALL_SINGLE_SELECT_CONTROL":
                            break;
                        default:
                            // This column is not a Code/Value or FK lookup column, so set the datatype for the column (to facilitate sorting)...
                            newDataTable.Columns[newColumnName].DataType = dataColumn.DataType;
                            break;
                    }
                }
                // Set the readonly property...
                newDataTable.Columns[newColumnName].ReadOnly = dataColumn.ReadOnly;
            }

            // Set the datatable's primary key...
            newDataTable.PrimaryKey = pKeys.ToArray();

            // Before populating the rows in the table, pre-load the FKey LU dictionaries with all needed values...
            foreach (DataColumn dc in dataTable.Columns)
            {
                if (lookupTables.IsValidFKField(dc))
                {
                    string lookupTable = dc.ExtendedProperties["foreign_key_dataview_name"].ToString();
                    int[] fkeys = new int[dataTable.Rows.Count];
                    // Build the list of FKeys that need to be resolved in the dicitonary...
                    for(int i=0; i<dataTable.Rows.Count; i++)
                    {
                        //fkeys[i] = (int)dataTable.Rows[i][dc];
                        int fkey = 0;
                        if(int.TryParse(dataTable.Rows[i][dc].ToString(), out fkey))
                        {
                            fkeys[i] = fkey;
                        }
                    }
                    lookupTables.PreLoadPKeyLUTDictionary(lookupTable, fkeys);
                }
            }

            // Now populate the rows of the new table...
            foreach (DataRow dr in dataTable.Rows)
            {
                if (dr.RowState != DataRowState.Deleted &&
                    dr.RowState != DataRowState.Detached)
                {
                    DataRow newDataRow = newDataTable.NewRow();
                    foreach (DataColumn dc in newDataTable.Columns)
                    {
                        if (lookupTables.IsValidFKField(dc))
                        {
                            string lookupTable = dc.ExtendedProperties["foreign_key_dataview_name"].ToString();
                            if (dr[dc.ColumnName] != DBNull.Value)
                            {
                                //newDataRow[dc.ColumnName] = lookupTables.GetDisplayMember(lookupTable, dr[dc.ColumnName].ToString(), "", dr[dc.ColumnName].ToString());
                                newDataRow[dc.ColumnName] = lookupTables.GetPKeyDisplayMember(lookupTable, (int)dr[dc.ColumnName], dr[dc.ColumnName].ToString());
                            }
                        }
                        else if (lookupTables.IsValidCodeValueField(dc))
                        {
                            if (dr[dc.ColumnName] != DBNull.Value)
                            {
                                //newDataRow[dc.ColumnName] = lookupTables.GetDisplayMember("code_value_lookup", dr[dc.ColumnName].ToString(), "group_name='" + dc.ExtendedProperties["group_name"].ToString() + "'", dr[dc.ColumnName].ToString());
                                newDataRow[dc.ColumnName] = lookupTables.GetCodeValueDisplayMember(dc.ExtendedProperties["group_name"].ToString() + dr[dc.ColumnName].ToString(), dr[dc.ColumnName].ToString());
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
                            {
                                newDataRow[dc.ColumnName] = dr[dc.ColumnName].ToString();
                            }
                        }
                    }
                    // Add the row to the new data table...
                    newDataTable.Rows.Add(newDataRow.ItemArray);
                }
            }

            // Set the sort and filter properties in the new datatable...
            newDataTable.DefaultView.Sort = dataTable.DefaultView.Sort;
            newDataTable.DefaultView.RowFilter = dataTable.DefaultView.RowFilter;

            newDataTable.DefaultView.AllowDelete = false;
            newDataTable.DefaultView.AllowEdit = false;
            newDataTable.DefaultView.AllowNew = false;

            // Bind the DataGridView to the datasource passed into this procedure...
            newDataTable.AcceptChanges();
            // NOTE: Sometimes the datagridview's datasource is actually a bindingsource (to wire up the binding navigator)
            // so binding must be handled differently...
            if (dataGridView != null &&
                dataGridView.DataSource != null &&
                dataGridView.DataSource.GetType() == typeof(BindingSource))
            {
                // Bind the new table to the default binding source (the bindingNavigator and mainDGV are both bound to this bindingSource)...
                ((BindingSource)dataGridView.DataSource).DataSource = newDataTable;
            }
            else
            {
                dataGridView.DataSource = newDataTable;
            }
            // Turn on wrap mode for all columns set as varchar(max) and set the gridview columne text...
            foreach (DataGridViewColumn dgvc in dataGridView.Columns)
            {
                if (newDataTable.Columns[dgvc.Name].ExtendedProperties.Contains("max_length") &&
                    newDataTable.Columns[dgvc.Name].ExtendedProperties["max_length"].ToString().Trim() == "-1")
                {
                    dgvc.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                // Get the text for the column header...
                dgvc.HeaderText = GetFriendlyFieldName(newDataTable.Columns[dgvc.Name], newDataTable.Columns[dgvc.Name].ColumnName);

            }
        }

        #endregion

        #region Building the Editable version of the DataGridView based on the DataTable...

        public void buildEditDataGridView(DataGridView dataGridView, DataTable dataTable, LookupTables lookupTables, string cno)
        {
            dataGridView.Columns.Clear();
            dataGridView.AutoGenerateColumns = false;

            DataGridViewColumn newDGVColumn = new DataGridViewColumn();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                if (dataColumn.ExtendedProperties.Contains("gui_hint"))
                {
                    switch (dataColumn.ExtendedProperties["gui_hint"].ToString())
                    {
                        case "LARGE_SINGLE_SELECT_CONTROL":
                            newDGVColumn = buildDGVUnboundTextBoxColumn(dataColumn);
                            break;
                        case "SMALL_SINGLE_SELECT_CONTROL":
                            newDGVColumn = buildDGVComboBoxColumn(dataColumn, lookupTables);
                            break;
                        case "TOGGLE_CONTROL":
                            newDGVColumn = buildDGVCheckBoxColumn(dataColumn);
                            break;
                        case "DATE_CONTROL":
                        case "INTEGER_CONTROL":
                        case "TEXT_CONTROL":
                        default:
                            newDGVColumn = buildDGVTextBoxColumn(dataColumn);
                            break;
                    }
                }
                else
                {
                    newDGVColumn = buildDGVTextBoxColumn(dataColumn);
                }

                // Get the text for the column header...
                newDGVColumn.HeaderText = GetFriendlyFieldName(dataColumn, dataColumn.ColumnName);

                // Set the properties of the new column...
                newDGVColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
                if (dataColumn.ExtendedProperties.Contains("is_autoincrement") && 
                    dataColumn.ExtendedProperties["is_autoincrement"].ToString() == "Y")
                {
                    dataColumn.AutoIncrement = true;
                    dataColumn.AutoIncrementSeed = -1;
                    dataColumn.AutoIncrementStep = -1;
                }
                if (dataColumn.ExtendedProperties.Contains("is_nullable") &&
                    dataColumn.ExtendedProperties["is_nullable"].ToString() == "N")
                {
                    //dataColumn.AllowDBNull = false;
                    if (dataColumn.ColumnName.StartsWith("is_")) dataColumn.DefaultValue = "N";
                    if (dataColumn.ColumnName == "created_by") dataColumn.DefaultValue = cno;
                    if (dataColumn.ColumnName == "created_date") dataColumn.DefaultValue = DateTime.Now;
                    if (dataColumn.ColumnName == "owned_by") dataColumn.DefaultValue = cno;
                    if (dataColumn.ColumnName == "owned_date") dataColumn.DefaultValue = DateTime.Now;
                }

                // Add the new column to the DataGridView...
                dataGridView.Columns.Add(newDGVColumn);
            }
            
            // NOTE: Sometimes the datagridview's datasource is actually a bindingsource (to wire up the binding navigator)
            // so binding must be handled differently...
            if (dataGridView != null &&
                dataGridView.DataSource != null &&
                dataGridView.DataSource.GetType() == typeof(BindingSource))
            {
                // Bind the new table to the default binding source (the bindingNavigator and mainDGV are both bound to this bindingSource)...
                ((BindingSource)dataGridView.DataSource).DataSource = dataTable;
            }
            else
            {
                dataGridView.DataSource = dataTable;
            }
        }

        private DataGridViewCheckBoxColumn buildDGVCheckBoxColumn(DataColumn dataColumn)
        {
            DataGridViewCheckBoxColumn newCheckBoxColumn = new DataGridViewCheckBoxColumn(false);
            newCheckBoxColumn.DataPropertyName = dataColumn.ColumnName;
            newCheckBoxColumn.Name = dataColumn.ColumnName;
            newCheckBoxColumn.ReadOnly = dataColumn.ReadOnly;
            newCheckBoxColumn.ValueType = dataColumn.DataType;
            newCheckBoxColumn.TrueValue = "Y";
            newCheckBoxColumn.FalseValue = "N";
            return newCheckBoxColumn;
        }

        private DataGridViewComboBoxColumn buildDGVComboBoxColumn(DataColumn dataColumn, LookupTables lookupTables)
        {
            DataGridViewComboBoxColumn newComboBoxColumn = new DataGridViewComboBoxColumn();
            newComboBoxColumn.DataPropertyName = dataColumn.ColumnName;
            newComboBoxColumn.Name = dataColumn.ColumnName;
            newComboBoxColumn.ReadOnly = dataColumn.ReadOnly;
            newComboBoxColumn.ValueType = dataColumn.DataType;
            newComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            if (dataColumn.ExtendedProperties.Contains("group_name"))
            {
                string groupName = dataColumn.ExtendedProperties["group_name"].ToString();
                DataTable dt = lookupTables.GetCodeValueDataTableByGroupName(groupName);
                if (dataColumn.ExtendedProperties.Contains("is_nullable") && dataColumn.ExtendedProperties["is_nullable"].ToString() == "Y")
                {
                    DataRow dr = dt.NewRow();
                    dr["display_member"] = "[Null]";
                    dr["value_member"] = DBNull.Value;
                    dt.Rows.InsertAt(dr, 0);
                    dt.AcceptChanges();
                }

                newComboBoxColumn.DisplayMember = "display_member";
                newComboBoxColumn.ValueMember = "value_member";
                newComboBoxColumn.DataSource = dt;
                newComboBoxColumn.DefaultCellStyle.DataSourceNullValue = DBNull.Value;
                newComboBoxColumn.DefaultCellStyle.NullValue = "[Null]";
            }
            return newComboBoxColumn;
        }

        private DataGridViewTextBoxColumn buildDGVTextBoxColumn(DataColumn dataColumn)
        {
            int maxTextLength = 0;
            DataGridViewTextBoxColumn newTextBoxColumn = new DataGridViewTextBoxColumn();
            newTextBoxColumn.DataPropertyName = dataColumn.ColumnName;
            newTextBoxColumn.Name = dataColumn.ColumnName;
            newTextBoxColumn.ReadOnly = dataColumn.ReadOnly;
            newTextBoxColumn.ValueType = dataColumn.DataType;
            if (dataColumn.ExtendedProperties.Contains("max_length"))
            {
                Int32.TryParse(dataColumn.ExtendedProperties["max_length"].ToString(), out maxTextLength);
                if (maxTextLength > 0)
                {
                    newTextBoxColumn.MaxInputLength = maxTextLength;
                }
            }
            return newTextBoxColumn;
        }

        private DataGridViewTextBoxColumn buildDGVUnboundTextBoxColumn(DataColumn dataColumn)
        {
            int maxTextLength = 0;
            DataGridViewTextBoxColumn newUnboundTextBoxColumn = new DataGridViewTextBoxColumn();
            //newTextBoxColumn.DataPropertyName = dataColumn.ColumnName;
            newUnboundTextBoxColumn.Name = dataColumn.ColumnName;
            newUnboundTextBoxColumn.ReadOnly = dataColumn.ReadOnly;
            newUnboundTextBoxColumn.ValueType = dataColumn.DataType;
            if (dataColumn.ExtendedProperties.Contains("max_length"))
            {
                Int32.TryParse(dataColumn.ExtendedProperties["max_length"].ToString(), out maxTextLength);
                if (maxTextLength > 0)
                {
                    newUnboundTextBoxColumn.MaxInputLength = maxTextLength;
                }
            }
            return newUnboundTextBoxColumn;
        }

        #endregion

        #region Keyboard Shortcut processing...
        public bool ProcessDGVEditShortcutKeys(DataGridView dgv, KeyEventArgs e, string cno, LookupTables lookupTables)
        {
            bool keyProcessed = false;

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            if (e.KeyCode == Keys.D && e.Control)
            {
                Dictionary<int, int> selectedColumnMinRow = new Dictionary<int, int>();

                // Processing keystroke...
                keyProcessed = true;

                foreach (DataGridViewCell cell in dgv.SelectedCells)
                {
                    // If the min selected row has not been found for this column - find it now...
                    if (!selectedColumnMinRow.ContainsKey(cell.ColumnIndex))
                    {
                        int minRow = dgv.Rows.Count; ;
                        // Find the minimum row index in this column's selected cells...
                        minRow = cell.RowIndex;
                        for (int i = minRow; i > -1; i--)
                        {
                            if (dgv.SelectedCells.Contains(dgv[cell.ColumnIndex, i])) minRow = i;
                        }
                        // If the user is trying to perform a copy down (CTRL+D) using the row for adding a new row as the source row - bail out now...
                        if (dgv.Rows[minRow].IsNewRow) return false;

                        //Save the min row for this column in the dictionary
                        selectedColumnMinRow.Add(cell.ColumnIndex, minRow);
                    }

                    //
                    object newValue = ((DataRowView)dgv.Rows[selectedColumnMinRow[cell.ColumnIndex]].DataBoundItem)[cell.ColumnIndex];
                    DataRowView dr = (DataRowView)cell.OwningRow.DataBoundItem;
                    if (dr == null) //if (dgv.Rows[row].IsNewRow)
                    {
                        // Couldn't find a bound row so this must be the 'new row' row in the datagrid...
                        dgv[cell.ColumnIndex, cell.RowIndex].Value = newValue;
                        dgv.UpdateCellValue(cell.ColumnIndex, cell.RowIndex);
                    }
                    else
                    {
                        if (!dr[cell.ColumnIndex].Equals(newValue))
                        {
                            // Edit the DataRow (not the DataRowView) so that row state is changed...
                            dr.Row[cell.ColumnIndex] = newValue;
                        }
                    }
                }
            }

            if (e.KeyCode == Keys.N && e.Control)
            {
                if (dgv.CurrentRow != null &&
                    dgv.CurrentRow.Selected &&
                    !dgv.CurrentRow.IsNewRow)
                {
                    DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                    DataRow sourceRow = null;
                    DataRow destRow = null;

                    // Processing keystroke...
                    keyProcessed = true;

                    if (dt != null)
                    {
                        sourceRow = dt.DefaultView[dgv.CurrentRow.Index].Row;
                        destRow = dt.NewRow();
                    }
                    if (sourceRow != null)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (!dt.PrimaryKey.Contains(dc))
                            {
                                switch (dc.ColumnName)
                                {
                                    case "created_by":
                                    case "owned_by":
                                        if (string.IsNullOrEmpty(cno))
                                        {
                                            destRow[dc] = sourceRow[dc];
                                        }
                                        else
                                        {
                                            destRow[dc] = cno;
                                        }
                                        break;
                                    case "created_date":
                                    case "owned_date":
                                        destRow[dc] = DateTime.Now;
                                        break;
                                    case "modified_by":
                                    case "modified_date":
                                        break;
                                    default:
                                        // Column is not a required field (or is a boolean field that only allows Y or N)
                                        destRow[dc] = sourceRow[dc];
                                        break;
                                }
                            }
                        }
                        dt.Rows.InsertAt(destRow, dgv.CurrentRow.Index + 1);
//RefreshDGVRowFormatting(dgv.Rows[dgv.CurrentRow.Index + 1], ux_checkboxHighlightChanges.Checked);
                    }
                }
            }

            if (e.KeyCode == Keys.OemQuotes && e.Control)
            {
                if (dgv.CurrentRow != null &&
                    dgv.CurrentRow.Index > 0)
                {
                    int sourceRowIndex;
                    DataRow sourceRow;
                    DataRow destinationRow;

                    // Processing keystroke...
                    keyProcessed = true;

                    if (dgv.CurrentRow.IsNewRow)
                    {
                        dgv.BeginEdit(true);
                        sourceRowIndex = dgv.CurrentRow.Index - 1;
                        sourceRow = ((DataRowView)dgv.Rows[sourceRowIndex].DataBoundItem).Row;
                        destinationRow = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                    }
                    else
                    {
                        sourceRowIndex = dgv.CurrentRow.Index - 1;
                        sourceRow = ((DataRowView)dgv.Rows[sourceRowIndex].DataBoundItem).Row;
                        destinationRow = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                    }

                    if (sourceRow != null && destinationRow != null)
                    {
                        if (!destinationRow[dgv.CurrentCell.ColumnIndex].Equals(sourceRow[dgv.CurrentCell.ColumnIndex]))
                        {
                            if (!dgv.Columns[dgv.CurrentCell.ColumnIndex].ReadOnly)
                            {
                                destinationRow[dgv.CurrentCell.ColumnIndex] = sourceRow[dgv.CurrentCell.ColumnIndex];
                            }
                        }
                    }
//RefreshDGVRowFormatting(dgv.CurrentCell.OwningRow, ux_checkboxHighlightChanges.Checked);
                }
            }

            if (e.KeyCode == Keys.V && e.Control)
            {
                IDataObject dataObj = Clipboard.GetDataObject();
                string pasteText = "";
                //string[] junk = dataObj.GetFormats();
                if (dataObj.GetDataPresent(System.Windows.Forms.DataFormats.UnicodeText))
                {
                    char[] rowDelimiters = new char[] { '\r', '\n' };
                    char[] columnDelimiters = new char[] { '\t' };
                    int badRows = 0;
                    int missingRows = 0;
                    bool importSuccess = false;

                    // Processing keystroke...
                    keyProcessed = true;

                    pasteText = dataObj.GetData(DataFormats.UnicodeText).ToString();
                    DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                    importSuccess = ImportTextToDataTableUsingKeys(pasteText, dt, rowDelimiters, columnDelimiters, out badRows, out missingRows, lookupTables);
                    if (!importSuccess)
                    {
                        // Paste the text into the DGV in 'block style'
                        importSuccess = ImportTextToDataTableUsingBlockStyle(pasteText, dgv, rowDelimiters, columnDelimiters, out badRows, out missingRows, lookupTables);
                    }
//RefreshMainDGVFormatting();
//RefreshForm();
                }
            }

            if (e.KeyCode == Keys.C && e.Control)
            {
                string copyString = "";
                // First we need to get the min/max rows and columns for the selected cells...
                int minCol = dgv.Columns.Count;
                int maxCol = -1;
                int minRow = dgv.Rows.Count;
                int maxRow = -1;

                // Processing keystroke...
                keyProcessed = true;

                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    if (dgvc.ColumnIndex < minCol) minCol = dgvc.ColumnIndex;
                    if (dgvc.ColumnIndex > maxCol) maxCol = dgvc.ColumnIndex;
                    if (dgvc.RowIndex < minRow) minRow = dgvc.RowIndex;
                    if (dgvc.RowIndex > maxRow) maxRow = dgvc.RowIndex;
                }

                // First, gather the column headers (but only if the entire row was selected)...
                if (dgv.SelectedRows.Count != 0)
                {
                    for (int i = minCol; i <= maxCol; i++)
                    {
                        copyString += dgv.Columns[i].HeaderText + '\t';
                        //copyString += dgv.Columns[i].Name + '\t';
                    }

                    // Strip the last tab and insert a newline...
                    copyString = copyString.TrimEnd('\t');
                    copyString += "\r\n";
                }
                // Now build the string to pass to the clipboard...
                for (int i = minRow; i <= maxRow; i++)
                {
                    for (int j = minCol; j <= maxCol; j++)
                    {
                        switch (dgv[j, i].FormattedValueType.Name)
                        {
                            case "Boolean":
                                copyString += dgv[j, i].Value.ToString() + '\t';
                                break;
                            default:
                                if (dgv[j, i].FormattedValue == null || dgv[j, i].FormattedValue.ToString().ToLower() == "[null]")
                                {
                                    copyString += "" + '\t';
                                }
                                else
                                {
                                    copyString += dgv[j, i].FormattedValue.ToString() + '\t';
                                }
                                break;
                        }
                    }
                    copyString = copyString.TrimEnd('\t');
                    copyString += "\r\n";
                }
                copyString = copyString.TrimEnd('\n');
                copyString = copyString.TrimEnd('\r');

                // Pass the new string to the clipboard...
                Clipboard.SetDataObject(copyString, false, 1, 1000);

//RefreshMainDGVFormatting();
//RefreshForm();
            }

            if (e.KeyCode == Keys.Delete)
            {
                // Processing keystroke...
                keyProcessed = true;

                if (dgv.SelectedRows.Count == 0)
                {
                    // The user is deleting values from individual selected cells (not entire rows)...
                    foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                    {
                        DataRowView drv = (DataRowView)dgvc.OwningRow.DataBoundItem;
                        if (drv == null) //if (dgv.Rows[row].IsNewRow)
                        {
                            dgvc.Value = "";
                            dgv.UpdateCellValue(dgvc.ColumnIndex, dgvc.RowIndex);
                            //dgv[dgvc.ColumnIndex, dgvc.RowIndex].Style.BackColor = Color.Yellow;
                        }
                        else
                        {
                            if (!drv[dgvc.OwningColumn.Index].Equals(DBNull.Value))
                            {
                                if (!dgvc.ReadOnly)
                                {
                                    // Edit the DataRow (not the DataRowView) so that row state is changed...
                                    drv.Row[dgvc.OwningColumn.Index] = DBNull.Value;
                                    // For unbound text cells we have to manually clear the cell's text...
                                    if (string.IsNullOrEmpty(dgvc.OwningColumn.DataPropertyName)) dgvc.Value = "";
                                    dgv.UpdateCellValue(dgvc.ColumnIndex, dgvc.RowIndex);
                                    //dgv[dgvc.ColumnIndex, dgvc.RowIndex].Style.BackColor = Color.Yellow;
                                }
                            }
                        }
//RefreshDGVRowFormatting(dgvc.OwningRow, ux_checkboxHighlightChanges.Checked);
                    }
                }
                else
                {
                    // The user is attempting to delete entire rows from the datagridview...
//SharedUtils sharedUtils = new SharedUtils(lookupTables.WebServiceURL, lookupTables.Username, lookupTables.Password_ClearText, true);
SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("WARNING!!!  You are about to permanently delete {0} records from the central database!\n\nAre you sure you want to do this?", "Record Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2);
ggMessageBox.Name = "UserInterfaceUtils_ProcessDGVEditShortcutKeysMessage1";
if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dgv.SelectedRows.Count.ToString());
//if (DialogResult.OK == MessageBox.Show("WARNING!!!  You are about to permanently delete " + dgv.SelectedRows.Count.ToString() + " records from the central database!\n\nAre you sure you want to do this?", "Record delete confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
if (DialogResult.OK == ggMessageBox.ShowDialog())
                    {
                        foreach (DataGridViewRow dgvr in dgv.SelectedRows)
                        {
                            dgv.Rows.Remove(dgvr);
                        }
                    }
e.Handled = true;
                }
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;

            return keyProcessed;
        }
        #endregion

        #region Import Text Data to DGV/DataTables...
        public bool ImportTextToDataTableUsingKeys(string rawImportText, DataTable destinationTable, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows, LookupTables lookupTables)
        {
            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] uniqueKeyColumnNames = null;
            bool primaryKeyFound = false;
            System.Collections.Generic.List<DataColumn> uniqueKeys = new System.Collections.Generic.List<DataColumn>();
            bool processedImportSuccessfully = false;
            badRows = 0;
            missingRows = 0;
            // Make sure there is text to process - if not bail out now...
            if (rawImportRows == null || rawImportRows.Length <= 0) return false;
            // Begin looking for a row of raw text that contains the column headers for the destination datatable...
            // This is a 2 phase approach that first looks for a row that contains all of the primary key column names
            // But if that is not found - try again to find a row of raw text that contains all of the column names for the unique compound key
            int columnHeaderRowIndex = -1;
            // PHASE 1:
            // Look for a raw text line that contains the full text name of the primary key columns (they must all be on the same line of raw text)...
            if (destinationTable.PrimaryKey.Length > 0)
            {
                // Look through all of the rows of raw text for a single row that contains all of the primary key column names
                for (int i = 0; i < rawImportRows.Length && columnHeaderRowIndex == -1; i++)
                {
                    columnHeaderRowIndex = i; // Start out ASSUMING this is the 'right' row...
                    foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                    {
                        if (!FindText(GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName), rawImportRows[i], false, rowDelimiters, columnDelimiters))
                        {
                            // If the column header was not matched using case sensitive - try matching again (case insensitive)...
                            if (!FindText(GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName), rawImportRows[i], true, rowDelimiters, columnDelimiters))
                            {
                                // If the column header was still not matched - try the raw table field name...
                                if (!FindText(pKeyColumn.ColumnName, rawImportRows[i], true, rowDelimiters, columnDelimiters))
                                {
                                    // The ASSUMPTION was wrong because the header text for one of the required primary key columns is missing in this raw text row...
                                    columnHeaderRowIndex = -1;
                                }
                            }
                        }
                    }
                }
                if (columnHeaderRowIndex != -1) primaryKeyFound = true;
                // Check to see if we need to move on to PHASE 2...
                if (!primaryKeyFound)
                {
                    // PHASE 2:
                    // Didn't find the primary key column in any text row in the import data - so try again, but this time looking for the alternate unique key...
                    if (destinationTable.PrimaryKey[0].ExtendedProperties.Contains("alternate_key_fields") &&
                        destinationTable.PrimaryKey[0].ExtendedProperties["alternate_key_fields"].ToString().Length > 0)
                    {
                        uniqueKeyColumnNames = destinationTable.PrimaryKey[0].ExtendedProperties["alternate_key_fields"].ToString().Split(',');
                        // Make sure the destination datatable has all of the columns specified in the alternate_key_fields ext. prop...
                        foreach (string uniqueColumnName in uniqueKeyColumnNames)
                        {
                            if (destinationTable.Columns.Contains(uniqueColumnName.Trim().ToLower()))
                            {
                                uniqueKeys.Add(destinationTable.Columns[uniqueColumnName.Trim().ToLower()]);
                            }
                        }
                        // The destination datatable does not have all of the columns specified in the compound unique key so bail out now...
                        if (uniqueKeys.Count != uniqueKeyColumnNames.Length) return false;
                        // Look through all of the rows of raw text for a single row that contains all of the unique key column names
                        for (int i = 0; i < rawImportRows.Length && columnHeaderRowIndex == -1; i++)
                        {
                            columnHeaderRowIndex = i; // Start out assuming the row has all of the unique key column headers...
                            foreach (DataColumn uKeyColumn in uniqueKeys)
                            {
                                if (!FindText(GetFriendlyFieldName(uKeyColumn, uKeyColumn.ColumnName), rawImportRows[i], false, rowDelimiters, columnDelimiters))
                                {
                                    // If the column header was not matched using case sensitive - try matching again (case insensitive)...
                                    if (!FindText(GetFriendlyFieldName(uKeyColumn, uKeyColumn.ColumnName), rawImportRows[i], true, rowDelimiters, columnDelimiters))
                                    {
                                        // If the column header was still not matched - try the raw table field name...
                                        if (!FindText(uKeyColumn.ColumnName, rawImportRows[i], true, rowDelimiters, columnDelimiters))
                                        {
                                            // The ASSUMPTION was wrong because the header text for one of the required unique key columns is missing in this raw text row...
                                            columnHeaderRowIndex = -1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                // Check to see if a column header was found for the psuedo-primary key of the destinationTable...
                if (columnHeaderRowIndex == -1)
                {
                    // Still cannot find an import row with column text that contains a collection of unique key columns - ask the user if they want to bail out now...
string uniqueKeyColumnFriendlyNames = "";
foreach (DataColumn dc in uniqueKeys)
{
    uniqueKeyColumnFriendlyNames += GetFriendlyFieldName(dc, dc.ColumnName) + ", ";
}
uniqueKeyColumnFriendlyNames = uniqueKeyColumnFriendlyNames.Trim().TrimEnd(',');
//SharedUtils sharedUtils = new SharedUtils(lookupTables.WebServiceURL, lookupTables.Username, lookupTables.Password_ClearText, true);
SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("WARNING!!!  You are pasting data in to this dataview without column headers that include: \n   1) The primary key column ({0}) OR \n   2) A combination of all of the columns ({1}) that will uniquely identify a single record in this dataview.\n\nWould you like to paste the data directly to the dataview starting at the selected cell?", "Missing Columns", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "UserInterfaceUtils_ImportTextToDataTableUsingKeysMessage1";
if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}") &&
    ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, GetFriendlyFieldName(destinationTable.PrimaryKey[0], destinationTable.PrimaryKey[0].ColumnName), uniqueKeyColumnFriendlyNames);
}
else if (ggMessageBox.MessageText.Contains("{0}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, GetFriendlyFieldName(destinationTable.PrimaryKey[0], destinationTable.PrimaryKey[0].ColumnName));
}
else if (ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, uniqueKeyColumnFriendlyNames);
}
// Ask the user if they wish to perform a 'block paste' of the data...
if (DialogResult.Yes == ggMessageBox.ShowDialog())
{
    return false;
}
else
{
    return true;
}
                }

                // Since we made it here, it looks like we found a row in the import text that contains the column names for the destination tables primary/unique key...
                string[] importColumnNames = rawImportRows[columnHeaderRowIndex].Split(columnDelimiters, StringSplitOptions.None);
                System.Collections.Generic.Dictionary<string, int> columnNameMap = new System.Collections.Generic.Dictionary<string, int>();
                // So now we need to build a map of datatable columns in import text columns (because they may not be in the same order)...
                for (int i = 0; i < importColumnNames.Length; i++)
                {
                    // Map the friendly field name from the incoming text to the matching column in the datatable (case sensitive)...
                    foreach (DataColumn dc in destinationTable.Columns)
                    {
                        if (GetFriendlyFieldName(dc, dc.ColumnName) == importColumnNames[i])
                        {
                            columnNameMap.Add(dc.ColumnName, i);
                        }
                    }
                    // If the column header was not matched - try matching again (case insensitive)...
                    if (!columnNameMap.ContainsValue(i))
                    {
                        // Map the friendly field name from the incoming text to the matching column in the datatable (case insensitive)...
                        foreach (DataColumn dc in destinationTable.Columns)
                        {
                            if (GetFriendlyFieldName(dc, dc.ColumnName).ToLower() == importColumnNames[i].ToLower())
                            {
                                columnNameMap.Add(dc.ColumnName, i);
                            }
                        }
                    }
                    // If the column header was still not matched - try the raw table field name...
                    if (!columnNameMap.ContainsValue(i))
                    {
                        // Map the friendly field name from the incoming text to the matching column in the datatable (case insensitive)...
                        foreach (DataColumn dc in destinationTable.Columns)
                        {
                            if (dc.ColumnName.ToLower() == importColumnNames[i].ToLower())
                            {
                                columnNameMap.Add(dc.ColumnName, i);
                            }
                        }
                    }
                }

                // Now that we have the column map, start processing the rows (starting with the one right after the column header row)...
                for (int i = columnHeaderRowIndex + 1; i < rawImportRows.Length; i++)
                {
                    DataRow dr = null;
                    string[] rawFieldData = rawImportRows[i].Split(columnDelimiters, StringSplitOptions.None);
                    if (primaryKeyFound)
                    {
                        System.Collections.Generic.List<object> rowKeys = new System.Collections.Generic.List<object>();
                        // Build the primary key to get the row to edit...
                        foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                        {
                            object keyValue;
                            if (string.IsNullOrEmpty(rawFieldData[columnNameMap[pKeyColumn.ColumnName]].ToString()))
                            {
                                keyValue = DBNull.Value;
                            }
                            else
                            {
                                keyValue = rawFieldData[columnNameMap[pKeyColumn.ColumnName]];
                            }
                            rowKeys.Add(keyValue);
                        }
                        // Get the row to update (or create a new one for insert if an existing one is not found)...
                        // First - attempt to find a row in the DataTable that matches the primary key(s)...
                        dr = destinationTable.Rows.Find(rowKeys.ToArray());
                        if (dr == null)
                        {
                            // No row exists in this DataTable for the given primary key(s), so create a new blank row to fill...
                            dr = destinationTable.NewRow();
                            // and add it to the DataTable...
                            destinationTable.Rows.Add(dr);
                        }
                    }
                    else // Find the row using the unique keys...
                    {
                        DataRow[] matchingRows = null;
                        string rowFilter = "";
                        foreach (DataColumn uKeyColumn in uniqueKeys)
                        {
                            if (columnNameMap[uKeyColumn.ColumnName] >= 0 &&
                                columnNameMap[uKeyColumn.ColumnName] <= (rawFieldData.Length - 1) &&
                                !string.IsNullOrEmpty(rawFieldData[columnNameMap[uKeyColumn.ColumnName]]))
                            {
                                string newValue = "";
                                // Perform a reverse lookup to get the key if this is a ForeignKey field...
                                if (lookupTables.IsValidFKField(uKeyColumn))
                                {
                                    if (!string.IsNullOrEmpty(rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString().Trim()))
                                    {
                                        newValue = lookupTables.GetPKeyValueMember(dr, uKeyColumn.ExtendedProperties["foreign_key_dataview_name"].ToString(),
                                                                                   rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString().Trim(),
                                                                                    -1).ToString();
                                        // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
                                        if (newValue.Equals("-1"))
                                        {
                                            dr.SetColumnError(uKeyColumn.ColumnName, "\tCould not find lookup value: " + rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString());
                                        }
                                    }
                                }
                                // Perform a reverse lookup to get the value if this is a Code_Value field...
                                else if (lookupTables.IsValidCodeValueField(uKeyColumn))
                                {
                                    if (!string.IsNullOrEmpty(rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString().Trim()))
                                    {
                                        newValue = lookupTables.GetCodeValueValueMember(rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString(),
                                                                                        uKeyColumn.ExtendedProperties["group_name"].ToString(),
                                                                                        "!Error! - GetValueMember method failed to find display member");
                                        // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
                                        if (newValue.Equals("!Error! - GetValueMember method failed to find display member"))
                                        {
                                            dr.SetColumnError(uKeyColumn.ColumnName, "\tCould not find lookup value: " + rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString());
                                        }
                                    }
                                }
                                // Doesn't require a lookup...
                                else
                                {
                                    newValue = rawFieldData[columnNameMap[uKeyColumn.ColumnName]];
                                }

                                if (uKeyColumn.DataType == typeof(string))
                                {
                                    rowFilter += uKeyColumn.ColumnName + "='" + newValue + "' AND ";
                                }
                                else
                                {
                                    rowFilter += uKeyColumn.ColumnName + "=" + newValue + " AND ";
                                }
                            }
                            else
                            {
                                rowFilter += uKeyColumn.ColumnName + " IS NULL AND ";
                            }
                        }
                        rowFilter = rowFilter.Substring(0, rowFilter.LastIndexOf(" AND "));
                        try
                        {
                            matchingRows = destinationTable.Select(rowFilter);
                        }
                        catch
                        {
                            matchingRows = new DataRow[] { };
                        }

                        if (matchingRows.Length > 0)
                        {
                            dr = matchingRows[0];
                        }
                        else
                        {
                            // Could not find a matching row, so set the dr to null (this will effectively ignore this import record)
                            //dr = null;
                            // No row exists in this DataTable for the given primary key(s), so create a new blank row to fill...
                            dr = destinationTable.NewRow();
                            // and add it to the DataTable...
                            destinationTable.Rows.Add(dr);
                        }
                    }
                    if (dr != null)
                    {
                        populateRowWithImportData(dr, rawFieldData, columnNameMap, lookupTables);
                    }
                    else
                    {
                        missingRows++;
                    }
                }
            }
            processedImportSuccessfully = true;

            return processedImportSuccessfully;
        }

        public bool ImportTextToDataTableUsingBlockStyle(string rawImportText, DataGridView dgv, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows, LookupTables lookupTables)
        {
            bool processedImportSuccessfully = true;
            DataTable destinationTable = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] tempColumns = null;
            string newImportText = "";
            string newImportRowText = "";
            badRows = 0;
            missingRows = 0;

            // If the DGV does not have a currently active cell bail out now...
            if (dgv.CurrentCell == null) return false;
            // If the import string is empty bail out now...
            if (string.IsNullOrEmpty(rawImportText) || rawImportRows.Length < 1) return false;

            // Okay we need to build a new importText string that has column headers that include the friendly names for the primary key columns
            // and the friendly names for the dgv columns starting at the currenly active cell in the dgv...  Why are we doing this?  Because
            // we are going to pass this new importText string off to the 'ImportTextToDataTableUsingKeys' method, and since that method
            // requires a primary key or alternate pkey we are going to get them from the dgv starting at the current row of the current cell...

            // Step 1 - Determine the number of rows and columns in the incoming rawImportText (to use later for building the new ImportText string)...
            int rawImportRowCount = 0;
            int rawImportColCount = 0;
            // Estimate the number of rows and columns in the import text (assumes a rectangular shape)
            if (rawImportRows != null && rawImportRows.Length > 0)
            {
                rawImportRowCount = rawImportRows.Length;
                tempColumns = rawImportRows[0].Split(columnDelimiters, StringSplitOptions.None);
                if (tempColumns != null && tempColumns.Length > 0)
                {
                    rawImportColCount = tempColumns.Length;
                }
            }

            int minSelectedCol = dgv.Columns.Count;
            int maxSelectedCol = -1;
            int minSelectedRow = dgv.Rows.Count;
            int maxSelectedRow = -1;
            // Check to see if the datagridview's selected cells contains the CurrentCell
            // and if so use the selected cells as the destination cells...

            // If no cells were selected for pasting the data bail out now...
if (dgv.SelectedCells.Count == 0)
{
//SharedUtils sharedUtils = new SharedUtils(lookupTables.WebServiceURL, lookupTables.Username, lookupTables.Password_ClearText, true);
    SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true);
    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("WARNING!!!  You must select the destination cell(s) in this dataview before pasting data without column headers", "No Destination Cells Selected", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
    ggMessageBox.Name = "UserInterfaceUtils_ImportTextToDataTableUsingBlockStyleMessage1";
    if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
    ggMessageBox.ShowDialog();
    return false;
}
            // Find the bounding rectangle for the selected cells...
            if (dgv.SelectedCells.Count == 1)
            {
                minSelectedCol = dgv.CurrentCell.ColumnIndex;
                maxSelectedCol = dgv.CurrentCell.ColumnIndex + rawImportColCount - 1;
                minSelectedRow = dgv.CurrentCell.RowIndex;
                maxSelectedRow = dgv.CurrentCell.RowIndex + rawImportRowCount - 1;
            }
            else
            {
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    if (dgvc.ColumnIndex < minSelectedCol) minSelectedCol = dgvc.ColumnIndex;
                    if (dgvc.ColumnIndex > maxSelectedCol) maxSelectedCol = dgvc.ColumnIndex;
                    if (dgvc.RowIndex < minSelectedRow) minSelectedRow = dgvc.RowIndex;
                    if (dgvc.RowIndex > maxSelectedRow) maxSelectedRow = dgvc.RowIndex;
                }
                if ((maxSelectedCol - minSelectedCol) < (rawImportColCount - 1)) maxSelectedCol = minSelectedCol + rawImportColCount - 1;
                if ((maxSelectedRow - minSelectedRow) < (rawImportRowCount - 1)) maxSelectedRow = minSelectedRow + rawImportRowCount - 1;
            }

            string modifiedImportText = "";
            // Now fill (or clip) the import data to fit the selected cells...
            for (int iSelectedRow = 0; iSelectedRow <= (maxSelectedRow - minSelectedRow); iSelectedRow++)
            {
                // 
                tempColumns = rawImportRows[iSelectedRow % rawImportRowCount].Split(columnDelimiters, StringSplitOptions.None);
                for (int iSelectedCol = 0; iSelectedCol <= (maxSelectedCol - minSelectedCol); iSelectedCol++)
                {
                    //
                    modifiedImportText += tempColumns[iSelectedCol % rawImportColCount] + "\t";
                }
                // Strip the last tab character and add a CR LF...
                modifiedImportText = modifiedImportText.Substring(0, modifiedImportText.Length - 1) + "\r\n";
            }

            // Step 2 - Get the primary key column names for the new column header row text...
            if (destinationTable.PrimaryKey.Length > 0)
            {
                foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                {
                    newImportText += GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName) + "\t";
                }
            }

            // Step 3 - Continue adding friendly column names to the import text (starting with the column name of the current cell's column HeaderText)...
            //DataGridViewColumn currColumn = dgv.CurrentCell.OwningColumn;
            DataGridViewColumn currColumn = dgv.Columns[minSelectedCol];
            // Step 4 - Now repeat this process for each additional column in the rawImportText...
            //foreach(string tempCol in tempColumns)
            for (int i = 0; i < Math.Max(rawImportColCount, maxSelectedCol - minSelectedCol + 1); i++)
            {
                if (currColumn != null)
                {
                    newImportText += currColumn.HeaderText + "\t";
                }
                else
                {
                    newImportText += "\t";
                }
                // Try to find the next visible column...
                currColumn = dgv.Columns.GetNextColumn(currColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.Frozen);
            }
            // Strip the last tab character and add a CR LF...
            newImportText = newImportText.Substring(0, newImportText.Length - 1) + "\r\n";

            // Step 5 - Get the primary key for each row receiving pasted text and prepend it to the orginal import raw text...
            string[] modifiedImportRows = modifiedImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            ////DataGridViewRow currRow = dgv.CurrentCell.OwningRow;
            DataGridViewRow currRow = dgv.Rows[minSelectedRow];
            int nextRowIndex = currRow.Index;
            for (int i = 0; i < modifiedImportRows.Length; i++)
            {
                newImportRowText = "";
                if (currRow != null)
                {
                    if (destinationTable.PrimaryKey.Length > 0)
                    {
                        foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                        {
                            newImportRowText += ((DataRowView)currRow.DataBoundItem).Row[pKeyColumn].ToString() + "\t";
                        }
                    }
                    // Now add the original import row text to the new import row text...
                    //newImportRowText += rawImportRows[i] + "\r\n";
                    newImportRowText += modifiedImportRows[i] + "\r\n";
                    // And now add it to the new import text string...
                    newImportText += newImportRowText;
                }

                // Finally, try to find the next visible row...
                nextRowIndex = dgv.Rows.GetNextRow(currRow.Index, DataGridViewElementStates.Visible);
                if (nextRowIndex != -1 &&
                    !dgv.Rows[nextRowIndex].IsNewRow &&
                    nextRowIndex >= minSelectedRow &&
                    nextRowIndex <= maxSelectedRow)
                {
                    currRow = dgv.Rows[nextRowIndex];
                }
                else
                {
                    // Looks like we hit the end of the rows in the DGV - bailout now...
                    //currRow = null;
                    break;
                }
            }

            // Step 6 - Now that we have built a new ImportText string that contains pkeys, we can pass it off to the 'ImportTextToDataTableUsingKeys' 
            processedImportSuccessfully = ImportTextToDataTableUsingKeys(newImportText, destinationTable, rowDelimiters, columnDelimiters, out badRows, out missingRows, lookupTables);

            return processedImportSuccessfully;
        }

        private void populateRowWithImportData(DataRow dr, string[] fieldValues, System.Collections.Generic.Dictionary<string, int> columnNameMap, LookupTables lookupTables)
        {
            DataTable dt = dr.Table;
            foreach (string tableColumnName in columnNameMap.Keys)
            {
                // Only update the write enabled columns in this row...
                if (!dt.Columns[tableColumnName].ReadOnly)
                {
                    string newValue = "";
                    DataColumn dc = dt.Columns[tableColumnName];
                    int fieldIndex = columnNameMap[tableColumnName];
                    if (fieldValues.Length <= fieldIndex) continue;
                    // Perform a reverse lookup to get the key if this is a ForeignKey field...
                    if (lookupTables.IsValidFKField(dc))
                    {
                        if (!string.IsNullOrEmpty(fieldValues[fieldIndex].ToString().Trim()))
                        {
                            newValue = lookupTables.GetPKeyValueMember(dr, dc.ExtendedProperties["foreign_key_dataview_name"].ToString(),
                                                                            fieldValues[fieldIndex].ToString().Trim(),
                                                                            -1).ToString();
                            // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
                            if (newValue.Equals("-1"))
                            {
                                dr.SetColumnError(tableColumnName, "\tCould not find lookup value: " + fieldValues[fieldIndex].ToString());
                            }
                        }
                    }
                    // Perform a reverse lookup to get the value if this is a Code_Value field...
                    else if (lookupTables.IsValidCodeValueField(dc))
                    {
                        if (!string.IsNullOrEmpty(fieldValues[fieldIndex].ToString().Trim()))
                        {
                            newValue = lookupTables.GetCodeValueValueMember(fieldValues[fieldIndex].ToString(),
                                                                            dc.ExtendedProperties["group_name"].ToString(),
                                                                            "!Error! - GetValueMember method failed to find display member");
                            // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
                            if (newValue.Equals("!Error! - GetValueMember method failed to find display member"))
                            {
                                dr.SetColumnError(tableColumnName, "\tCould not find lookup value: " + fieldValues[fieldIndex].ToString());
                            }
                        }
                    }
                    // Doesn't require a lookup...
                    else
                    {
                        newValue = fieldValues[fieldIndex];
                    }

                    // If the newValue is null attempt to retrieve the default value before further processing...
                    if (string.IsNullOrEmpty(newValue) &&
                        dt.Columns[tableColumnName].ExtendedProperties.Contains("default_value") &&
                        dt.Columns[tableColumnName].ExtendedProperties["default_value"].ToString().Length > 0)
                    {
                        newValue = dt.Columns[tableColumnName].ExtendedProperties["default_value"].ToString();
                    }

                    // Set the newValue to a default value if it is empty or null...
                    if (string.IsNullOrEmpty(newValue) || newValue == "{DBNull.Value}")
                    {
                        if (dt.Columns[tableColumnName].ExtendedProperties.Contains("is_nullable") &&
                            dt.Columns[tableColumnName].ExtendedProperties["is_nullable"].ToString() == "Y")
                        {
                            if (!dr[tableColumnName].Equals(DBNull.Value) && !dr[tableColumnName].Equals(newValue)) dr[tableColumnName] = DBNull.Value;
                        }
                        else
                        {
                            dr.SetColumnError(tableColumnName, "\tThis value cannot be empty (null)");
                        }
                    }
                    // Convert the newValue string to the datatype for this column...
                    else if ((dt.Columns[tableColumnName].DataType == typeof(int) ||
                                dt.Columns[tableColumnName].DataType == typeof(Int16) ||
                                dt.Columns[tableColumnName].DataType == typeof(Int32) ||
                                dt.Columns[tableColumnName].DataType == typeof(Int64)) &&
                                !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
                    {
                        int tempValue = 0;
                        if (Int32.TryParse(newValue, out tempValue))
                        {
                            if (!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
                        }
                        else
                        {
                            dr.SetColumnError(tableColumnName, "\tValue '" + newValue + "' cannot be converted to an integer");
                        }
                    }
                    else if (dt.Columns[tableColumnName].DataType == typeof(Decimal) && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
                    {
                        Decimal tempValue = new Decimal();
                        if (Decimal.TryParse(newValue, out tempValue))
                        {
                            if (!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
                        }
                        else
                        {
                            dr.SetColumnError(tableColumnName, "\tValue '" + newValue + "' cannot be converted to a decimal");
                        }
                    }
                    else if (dt.Columns[tableColumnName].DataType == typeof(DateTime) && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
                    {
                        DateTime tempValue = new DateTime();
                        if (DateTime.TryParse(newValue, out tempValue))
                        {
                            if (!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
                        }
                        else
                        {
                            // Basic DateTime conversion failed - look to see if the user provided a hint about how to interpret this date value...
                            // Look to see if there is a column provided that matches this current column name + "_code"...
                            if (dt.Columns.Contains(tableColumnName + "_code"))
                            {
                                string dateFormat = "MM/dd/yyyy";
                                dateFormat = lookupTables.GetCodeValueValueMember(fieldValues[columnNameMap[tableColumnName + "_code"]].ToString().Trim(), dr.Table.Columns[tableColumnName + "_code"].ExtendedProperties["group_name"].ToString(), dateFormat);
                                if (DateTime.TryParseExact(newValue, dateFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out tempValue))
                                {
                                    if (!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
                                }
                                else
                                {
                                    dr.SetColumnError(tableColumnName, "\tValue '" + newValue + "' cannot be converted to a Date/Time");
                                }
                            }
                        }
                    }
                    else if (dt.Columns[tableColumnName].DataType == typeof(string) && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
                    {
                        if (dc.ExtendedProperties.Contains("max_length") && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
                        {
                            int maxLength = 0;
                            if (Int32.TryParse(dc.ExtendedProperties["max_length"].ToString(), out maxLength))
                            {
                                if (newValue.Length <= maxLength ||
                                    maxLength == -1)
                                {
                                    if (!dr[tableColumnName].Equals(newValue)) dr[tableColumnName] = newValue;
                                }
                                else
                                {
                                    dr.SetColumnError(tableColumnName, "\tValue exceeds maximum length - truncated to " + maxLength.ToString() + " characters");
                                    dr[tableColumnName] = newValue.Substring(0, maxLength); // Truncate the value (so the user can see what is legal to be pasted in)
                                }
                            }
                        }
                    }
                    else
                    {
                        // Not sure what datatype got us here - bailout...
                    }
                }
            }
        }

        private bool FindText(string textToFind, string textToSearch, bool ignoreCase, char[] rowDelimiters, char[] columnDelimters)
        {
            bool foundText = false;
            string[] textLinesToSearch = textToSearch.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            foreach (string lineText in textLinesToSearch)
            {
                string[] columnsOfText = lineText.Split(columnDelimters, StringSplitOptions.None);
                foreach (string columnText in columnsOfText)
                {
                    if (ignoreCase)
                    {
                        if (columnText.ToLower().Trim().Equals(textToFind.ToLower().Trim()))
                        {
                            foundText = true;
                            break;
                        }
                    }
                    else
                    {
                        if (columnText.Trim().Equals(textToFind.Trim()))
                        {
                            foundText = true;
                            break;
                        }
                    }
                }
                if (foundText)
                {
                    break;
                }
            }
            return foundText;
        }

        #endregion
    }
}
