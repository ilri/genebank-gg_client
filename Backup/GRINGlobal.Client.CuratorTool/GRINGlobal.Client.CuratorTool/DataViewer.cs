using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using GRINGlobal.Client.Common;

namespace GRINGlobal.Client.CuratorTool
{
    public partial class GRINGlobalClientCuratorTool
    {
        #region TabControl for Dataview logic...

        #region Code relocated to SharedUtils...
        //        private void ux_tabcontrolCreateNewTab(TabControl ux_tabcontrolDataview, int indexOfNewTab)
//        {
//            DataviewProperties dvp = new DataviewProperties();
//            dvp.TabName = "";
//            dvp.DataviewName = "";
//            dvp.StrongFormName = "";
//            dvp.ViewerStyle = "Spreadsheet";
//            dvp.AlwaysOnTop = "false";

//            DataviewTabProperties newTabDialog = new DataviewTabProperties(_sharedUtils, dvp, localFormsAssemblies);
//            newTabDialog.StartPosition = FormStartPosition.CenterParent;
//            if (newTabDialog.ShowDialog(this) == DialogResult.OK)
//            {
//                ux_tabcontrolAddTab(newTabDialog.TabProperties.TabName, newTabDialog.TabProperties, indexOfNewTab);
//                ux_tabcontrolDataview.SelectedIndex = indexOfNewTab;
////SetDGVMainDataviewUserSettings();
//            }
//            else
//            {
//                ux_tabcontrolDataview.DeselectTab(indexOfNewTab);
//            }
//        }

//        private void ux_tabcontrolAddTab(string text, DataviewProperties tag, int position)
//        {
//            foreach (TabPage tp in ux_tabcontrolDataview.TabPages)
//            {
//                // If this tab page is already in the collection - bail out...
//                if (tp.Text.ToUpper() == text.ToUpper() && ((DataviewProperties)tp.Tag).DataviewName == tag.DataviewName) return;
//            }
//            TabPage newTabPage = new TabPage();
//            newTabPage.Text = text;
//            newTabPage.Tag = tag;
//            ux_tabcontrolDataview.TabPages.Insert(position, newTabPage);
//        }

//        private void ux_tabcontrolShowProperties(TabControl ux_tabcontrolDataview, int indexOfCurrentTab)
//        {
//            DataviewProperties dvp;
//            if (ux_tabcontrolDataview.SelectedTab != null &&
//                ux_tabcontrolDataview.SelectedTab.Tag != null &&
//                ux_tabcontrolDataview.SelectedTab.Tag.GetType() == typeof(DataviewProperties) &&
//                !string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).TabName))
//            {
//                dvp = (DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag;
//            }
//            else
//            {
//                dvp = new DataviewProperties();
//                dvp.TabName = ux_tabcontrolDataview.SelectedTab.Text;
//                dvp.DataviewName = (string)ux_tabcontrolDataview.SelectedTab.Tag;
//                dvp.StrongFormName = "";
//                dvp.ViewerStyle = "Spreadsheet";
//                dvp.AlwaysOnTop = "false";
//            }

//            DataviewTabProperties dataviewTabPropertiesDialog = new DataviewTabProperties(_sharedUtils, dvp, localFormsAssemblies);
//            dataviewTabPropertiesDialog.StartPosition = FormStartPosition.CenterParent;
//            if (dataviewTabPropertiesDialog.ShowDialog(this) == DialogResult.OK)
//            {
//                ux_tabcontrolDataview.TabPages[indexOfCurrentTab].Tag = dataviewTabPropertiesDialog.TabProperties;
//                ux_tabcontrolDataview.TabPages[indexOfCurrentTab].Text = ((DataviewProperties)ux_tabcontrolDataview.TabPages[indexOfCurrentTab].Tag).TabName;
//            }
//        }

//        private void ux_tabcontrolRemoveTab(TabControl ux_tabcontrolDataview, int indexOfTabToDelete)
//        {
//            // Deselect the tab before deleting it...
//            //            ux_tabcontrolDataview.DeselectTab(indexOfTabToDelete);
//            ux_tabcontrolDataview.SelectedIndex = -1;
//            // Delete the tab (unless this is the last tab remaining)...
//            if (ux_tabcontrolDataview.TabPages.Count >= 3) ux_tabcontrolDataview.TabPages.RemoveAt(indexOfTabToDelete);
//            // Attempt to select another tab (to auto-refresh the DGV)...
//            int newSelectedTabIndex = Math.Max(0, indexOfTabToDelete - 1);
//            ux_tabcontrolDataview.SelectedIndex = newSelectedTabIndex;
//        }

//        private void ux_tabcontrolMouseDownEvent(TabControl ux_tabcontrolDataview, MouseEventArgs e)
//        {
//            int clickedTabPage = ux_tabcontrolDataview.SelectedIndex;

//            // Attempt to find the tabpage that was clicked...
//            for (int i = 0; i < ux_tabcontrolDataview.TabPages.Count; i++)
//            {
//                if (ux_tabcontrolDataview.GetTabRect(i).Contains(e.Location)) clickedTabPage = i; //MessageBox.Show(ux_tabcontrolDataview.TabPages[i].Text + " : " + e.Location.ToString());
//            }

//            if (e.Button == MouseButtons.Left)
//            {
//                // Begin tabpage drag and drop move (if the clicked tab is not 
//                // the "ux_tabpageDataviewNewTab" tabpage - which is use to add new dataviews)...
//                if (ux_tabcontrolDataview.TabPages[clickedTabPage] != ux_tabcontrolDataview.TabPages["ux_tabpageDataviewNewTab"])
//                {
//                    ux_tabcontrolDataview.DoDragDrop(ux_tabcontrolDataview.TabPages[clickedTabPage], DragDropEffects.Move);
//                }
//            }
//            else if (e.Button == MouseButtons.Right)
//            {
//                // Make the right clicked tabpage the selected tabpage for the control...
//                ux_tabcontrolDataview.SelectTab(clickedTabPage);
//            }
//        }

//        private void ux_tabcontrolDragOverEvent(TabControl ux_tabcontrolDataview, DragEventArgs e)
//        {
//            // Convert the mouse coordinates from screen to client...
//            Point ptClientCoord = ux_tabcontrolDataview.PointToClient(new Point(e.X, e.Y));
//            int destinationTabPage = ux_tabcontrolDataview.TabPages.IndexOf((TabPage)e.Data.GetData(typeof(TabPage)));

//            // Attempt to find the tabpage that is being dragged over...
//            for (int i = 0; i < ux_tabcontrolDataview.TabPages.Count; i++)
//            {
//                if (ux_tabcontrolDataview.GetTabRect(i).Contains(ptClientCoord)) destinationTabPage = i;
//            }

//            if (e.Data.GetDataPresent(typeof(TabPage)) &&
//                ux_tabcontrolDataview.TabPages[destinationTabPage] != (TabPage)e.Data.GetData(typeof(TabPage)) /* &&
//                destinationTabPage != ux_tabcontrolDataview.TabPages.IndexOfKey("ux_tabpageDataviewNewTab")*/
//                                                                                                                      )
//            {
//                e.Effect = DragDropEffects.Move;
//            }
//            else
//            {
//                e.Effect = DragDropEffects.None;
//            }
//        }

//        private void ux_tabcontrolDragDropEvent(TabControl ux_tabcontrolDataview, DragEventArgs e)
//        {
//            if (e.AllowedEffect == e.Effect)
//            {
//                // Convert the mouse coordinates from screen to client...
//                Point ptClientCoord = ux_tabcontrolDataview.PointToClient(new Point(e.X, e.Y));

//                int destinationTabPageIndex = -1;
//                int originalTabPageIndex = -1;

//                // Attempt to find where the tabpage should be dropped...
//                for (int i = 0; i < ux_tabcontrolDataview.TabPages.Count; i++)
//                {
//                    if (ux_tabcontrolDataview.TabPages[i] == e.Data.GetData(typeof(TabPage))) originalTabPageIndex = i;
//                    if (ux_tabcontrolDataview.GetTabRect(i).Contains(ptClientCoord)) destinationTabPageIndex = i;
//                }

//                // Now create a copy of the tabpage that is being moved so that 
//                // you can remove the orginal and insert the copy at the right spot...
//                TabPage newTabPage = new TabPage();
//                newTabPage.Text = ((TabPage)e.Data.GetData(typeof(TabPage))).Text;
//                newTabPage.Tag = ((TabPage)e.Data.GetData(typeof(TabPage))).Tag;
//                ux_tabcontrolDataview.TabPages.Insert(destinationTabPageIndex, newTabPage);
//                ux_tabcontrolDataview.SelectTab(destinationTabPageIndex);
//                if (originalTabPageIndex < destinationTabPageIndex)
//                {
//                    ux_tabcontrolDataview.TabPages.RemoveAt(originalTabPageIndex);
//                }
//                else
//                {
//                    ux_tabcontrolDataview.TabPages.RemoveAt(originalTabPageIndex + 1);
//                }
//            }
        //        }
        #endregion

        private void newDataviewTabPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int indexOfNewTab = ux_tabcontrolCTDataviews.SelectedIndex;
            // Add the tab to the tabcontrol...
            _sharedUtils.ux_tabcontrolCreateNewTab(ux_tabcontrolCTDataviews, indexOfNewTab);
#region Old Code...
//            DataviewProperties dvp = new DataviewProperties();
////dvp.TabName = "New Tab";
//            dvp.TabName = "";
//            dvp.DataviewName = "";
//            dvp.StrongFormName = "";
//            dvp.ViewerStyle = "Spreadsheet";
//            dvp.AlwaysOnTop = "false";
////DataviewTabProperties newTabPropertiesDialog = new DataviewTabProperties(username, password, GUIWebServices.Url, dvp, localFormsAssemblies);
////DataviewTabProperties newTabPropertiesDialog = new DataviewTabProperties(GRINGlobalWebServices, dvp, localFormsAssemblies);
//            DataviewTabProperties newTabPropertiesDialog = new DataviewTabProperties(_sharedUtils, dvp, localFormsAssemblies);
//            AddTabControlDataviewTab(dvp.TabName, dvp, indexOfNewTab);
//            ux_tabcontrolDataview.SelectedIndex = indexOfNewTab;
//            if (newTabPropertiesDialog.ShowDialog(this) == DialogResult.OK)
//            {
////ux_tabcontrolDataview.TabPages[indexOfNewTab].Text = newTabPropertiesDialog.TabName;
////ux_tabcontrolDataview.TabPages[indexOfNewTab].Tag = newTabPropertiesDialog.TabDataviewName;
//                ux_tabcontrolDataview.TabPages[indexOfNewTab].Tag = newTabPropertiesDialog.TabProperties;
//                ux_tabcontrolDataview.TabPages[indexOfNewTab].Text = ((DataviewProperties)ux_tabcontrolDataview.TabPages[indexOfNewTab].Tag).TabName;
//                //SetDGVMainDataviewUserSettings();
//                SetAllUserSettings();
//                // Refresh the data view...
//                RefreshMainDGVData();
//                RefreshMainDGVFormatting();
//            }
//            else
//            {
//                ux_tabcontrolDataview.TabPages.RemoveAt(indexOfNewTab);
//            }
#endregion
        }

        private void propertiesDataviewTabPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int indexOfCurrentTab = ux_tabcontrolCTDataviews.SelectedIndex;

            _sharedUtils.ux_tabcontrolShowProperties(ux_tabcontrolCTDataviews, indexOfCurrentTab);

            SetAllUserSettings();
            // Refresh the data view...
            RefreshMainDGVData();
            RefreshMainDGVFormatting();
            RefreshForm();
        }

        private void deleteDataviewTabPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int indexOfTabToDelete = ux_tabcontrolCTDataviews.SelectedIndex;
            _sharedUtils.ux_tabcontrolRemoveTab(ux_tabcontrolCTDataviews, indexOfTabToDelete);

            // Save the changes to tabcontrol in the usersettings...
            SetAllUserSettings();
        }

        private void ux_tabcontrolDataview_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            // Update all TabControl settings on the userSettings dataset...
            SetDGVMainDataviewUserSettings();
        }

        private void ux_tabcontrolDataview_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ux_tabcontrolCTDataviews.SelectedIndex > -1)
            {
                if (ux_tabcontrolCTDataviews.SelectedTab.Name == "ux_tabpageDataviewNewTab")
                {
                    int indexOfNewTab = ux_tabcontrolCTDataviews.SelectedIndex;
                    // Add the tab to the tabcontrol...
                    _sharedUtils.ux_tabcontrolCreateNewTab(ux_tabcontrolCTDataviews, indexOfNewTab);
                }
            }

            // If there is a form still attached to the global variable dataviewForm - dispose of it properly...
            if (dataviewForm != null)
            {
                dataviewForm.Close();
                dataviewForm.Dispose();
                dataviewForm = null;
            }
            
            // Refresh the data view...
            RefreshMainDGVData();
            RefreshMainDGVFormatting();
            // Display a form if the tab property indicates...
            RefreshForm();
       }
 
        private void ux_tabcontrolDataview_MouseDown(object sender, MouseEventArgs e)
        {
            _sharedUtils.ux_tabcontrolMouseDownEvent(ux_tabcontrolCTDataviews, e);
        }

        private void ux_tabcontrolDataview_DragOver(object sender, DragEventArgs e)
        {
            _sharedUtils.ux_tabcontrolDragOverEvent(ux_tabcontrolCTDataviews, e);
        }

        private void ux_tabcontrolDataview_DragDrop(object sender, DragEventArgs e)
        {
            _sharedUtils.ux_tabcontrolDragDropEvent(ux_tabcontrolCTDataviews, e);
        }

        #endregion

        #region Vertical TabControl for Dataview Options logic...

        private void ux_tabcontrolDataDataviewOptions_DrawItem(object sender, DrawItemEventArgs e)
        {
            Rectangle clipRectangle = ux_tabcontrolDataviewOptions.GetTabRect(e.Index);
            float offsetX = (float)(clipRectangle.Left + ((float)clipRectangle.Width / 2.0));
            float offsetY = (float)(clipRectangle.Top + ((float)clipRectangle.Height / 2.0));
            Font tabFont;
            Brush tabBrush;
            if (ux_tabcontrolDataviewOptions.SelectedIndex == e.Index)
            {
                tabFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point);
                tabBrush = Brushes.Black;
                e.Graphics.FillRectangle(Brushes.White, clipRectangle);
                e.Graphics.FillRectangle(Brushes.Orange, clipRectangle.X, clipRectangle.Y, 3, clipRectangle.Height);
            }
            else
            {
                tabFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point);
                tabBrush = Brushes.Black;
                e.Graphics.FillRectangle(Brushes.WhiteSmoke, clipRectangle);
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.DirectionVertical;

            e.Graphics.ResetTransform();
            e.Graphics.TranslateTransform(-offsetX, -offsetY, System.Drawing.Drawing2D.MatrixOrder.Append);
            e.Graphics.RotateTransform(180f, System.Drawing.Drawing2D.MatrixOrder.Append);
            e.Graphics.TranslateTransform(offsetX, offsetY, System.Drawing.Drawing2D.MatrixOrder.Append);
            e.Graphics.DrawString(ux_tabcontrolDataviewOptions.TabPages[e.Index].Text,
                //new Font("Microsoft Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point), 
                //new System.Drawing.SolidBrush(e.ForeColor),
                                    tabFont,
                                    tabBrush,
                                    ux_tabcontrolDataviewOptions.GetTabRect(e.Index),
                                    new StringFormat(sf));

        }

        private void ux_tabcontrolDataviewOptions_Enter(object sender, EventArgs e)
        {
            ux_tabcontrolDataviewOptions.Left -= ux_tabcontrolDataviewOptions.TabPages[0].Width;
        }

        private void ux_tabcontrolDataviewOptions_Leave(object sender, EventArgs e)
        {
            ux_tabcontrolDataviewOptions.Left += ux_tabcontrolDataviewOptions.TabPages[0].Width;
        }

        private void ux_checkboxHighlightChanges_CheckedChanged(object sender, EventArgs e)
        {
            RefreshMainDGVFormatting();
            ux_datagridviewMain.Focus();
        }

        private void ux_checkboxHideUnchangedRows_CheckedChanged(object sender, EventArgs e)
        {
            // Hide the unchanged rows if the checkbox indicates...
            if (ux_checkboxHideUnchangedRows.Checked && ux_buttonSaveData.Enabled)
            {
                ux_datagridviewMain.CurrentCell = null; // You can't hide a row that the CurrentCell property is pointing at
                ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowStateFilter = DataViewRowState.ModifiedCurrent;
            }
            else
            {
                ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
            }
            RefreshMainDGVFormatting();
            ux_datagridviewMain.Focus();
        }

        private void ux_buttonDefaultCellColor_Click(object sender, EventArgs e)
        {
            ux_colordialog.Color = ux_datagridviewMain.DefaultCellStyle.BackColor;
            DialogResult diagResult = ux_colordialog.ShowDialog();
            ux_datagridviewMain.DefaultCellStyle.BackColor = ux_colordialog.Color;
        }

        private void ux_buttonAlternatingRowColor_Click(object sender, EventArgs e)
        {
            ux_colordialog.Color = ux_datagridviewMain.AlternatingRowsDefaultCellStyle.BackColor;
            DialogResult diagResult = ux_colordialog.ShowDialog();
            ux_datagridviewMain.AlternatingRowsDefaultCellStyle.BackColor = ux_colordialog.Color;
        }

        #endregion

        #region CheckedListBox for selecting Viewed Columns in the DataGridView logic...

        private void ux_checkedlistboxViewedColumns_Leave(object sender, EventArgs e)
        {
            // Remember all user settings...
            SetDGVMainDataviewUserSettings();
            //SetAllUserSettings();
        }

        private void ux_checkedlistboxViewedColumns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // This method uses a foreach loop because the 'friendly name' text in the checkbox list is not the datatable field name
            // so finding the correct column in the dgv would be difficult (because we would need to inspect each dgv column Caption 
            // to get the 'friendly name' text to match to the checkbox
            int clbItem = -1;
            foreach (DataGridViewColumn dgvc in ux_datagridviewMain.Columns)
            {
                clbItem = ux_checkedlistboxViewedColumns.FindString(dgvc.HeaderText);
                if (clbItem > -1) dgvc.Visible = ux_checkedlistboxViewedColumns.GetItemChecked(clbItem);
                // This next line of code is necessary because the checkbox that has been changed
                // has not had it's new value assigned yet, so you must inspect the e.NewValue state
                if (clbItem == e.Index) dgvc.Visible = (e.NewValue == CheckState.Checked);
            }
            // Now we need to iterate through each item in the checklistbox collection to determine 
            // if all checkboxes are uniformly checked or unchecked so that we can set the 
            // checkedstate of the 'Select/Deselect All' checkbox...
            bool allAreChecked = true;
            bool allAreUnchecked = true;
            for (int i = 0; i < ux_checkedlistboxViewedColumns.Items.Count; i++)
            {
                bool checkedState = ux_checkedlistboxViewedColumns.GetItemChecked(i);
                if(i == e.Index) checkedState = (e.NewValue == CheckState.Checked);

                if (checkedState)
                {
                    allAreUnchecked = false;
                }
                else
                {
                    allAreChecked = false;
                }
            }
            if (allAreChecked) ux_checkboxSelectAll.CheckState = CheckState.Checked;
            else if (allAreUnchecked) ux_checkboxSelectAll.CheckState = CheckState.Unchecked;
            else ux_checkboxSelectAll.CheckState = CheckState.Indeterminate;
        }

        private void ux_checkboxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ux_checkboxSelectAll.CheckState == CheckState.Checked)
            {
                for (int i = 0; i < ux_checkedlistboxViewedColumns.Items.Count; i++) ux_checkedlistboxViewedColumns.SetItemChecked(i, true);
            }
            else if (ux_checkboxSelectAll.CheckState == CheckState.Unchecked)
            {
                for (int i = 0; i < ux_checkedlistboxViewedColumns.Items.Count; i++) ux_checkedlistboxViewedColumns.SetItemChecked(i, false);
            }
        }

        #endregion

        #region Edit, Save, and Cancel buttons and edit-specific checkboxes logic...

        private void ux_buttonEditData_Click(object sender, EventArgs e)
        {
            // Remember the first visible column to restore later...
            int firstVisibleColumn = ux_datagridviewMain.FirstDisplayedScrollingColumnIndex;
            int currentRow = 0;
            if (ux_datagridviewMain.CurrentRow != null) currentRow = ux_datagridviewMain.CurrentRow.Index;

//string currentColName = ux_datagridviewMain.Columns[ux_datagridviewMain.FirstDisplayedScrollingColumnIndex].Name;

            // Remember all user settings...
            SetAllUserSettings();

            // Update the controls on the interface...
            ux_checkboxHighlightChanges.Enabled = true;
            ux_checkboxHideUnchangedRows.Enabled = true;
            ux_buttonSaveData.Enabled = true;
            ux_buttonEditData.Enabled = false;
            ux_buttonCancelEditData.Enabled = true;
            ux_splitcontainerMain.Panel1.Enabled = false;
            ux_tabcontrolCTDataviews.Enabled = false;

            // Bail if there is no data to process...
            if (ux_tabcontrolCTDataviews.SelectedTab == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag.GetType() != typeof(DataviewProperties) ||
                string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName))
            {
                return;
            }

            // Create a filtered and sorted copy of the data to be edited...
            // First get the filtered and sorted rows in the readonly datatable copied to a new temp table...
            DataTable filteredSortedTable = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.ToTable();
            // Create a new empty table that will have just the rows in the filtered and sorted rows in the temp table...
            DataTable editTable = grinData.Tables[((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName].Clone();
            // Add each row in the readonly view to the edit table...
            System.Collections.Generic.List<object> pKeyVals = new System.Collections.Generic.List<object>();
            foreach (DataRow dr in filteredSortedTable.Rows)
            {
                pKeyVals.Clear();
                foreach (DataColumn pKeyCol in editTable.PrimaryKey)
                {
                    pKeyVals.Add(dr[pKeyCol.ColumnName]);
                }
                DataRow newRow = editTable.NewRow();

                if (pKeyVals.Count > 0 && grinData.Tables[((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName].Rows.Contains(pKeyVals.ToArray()))
                {
                    newRow.ItemArray = grinData.Tables[((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName].Rows.Find(pKeyVals.ToArray()).ItemArray;
                    editTable.Rows.Add(newRow);
                }
            }

            editTable.AcceptChanges();

            // Drop the original datatable and add the new one...
            grinData.Tables.Remove(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName);
            grinData.Tables.Add(editTable);
            //ux_datagridviewMain.ResumeLayout();

            // Create the editable DGV...
//            buildEditDataGridView(ux_datagridviewMain, grinData.Tables[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName]);
            _sharedUtils.BuildEditDataGridView(ux_datagridviewMain, grinData.Tables[((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName]);
//ux_datagridviewMain.DataSource = grinData.Tables[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName];
            // Refresh formatting on the new DGV...
            RefreshMainDGVFormatting();

            if (ux_datagridviewMain.Rows.Count > 0)
            {
                ux_datagridviewMain.FirstDisplayedScrollingColumnIndex = firstVisibleColumn;
                ux_datagridviewMain.CurrentCell = ux_datagridviewMain[firstVisibleColumn, currentRow];
                ux_datagridviewMain.CurrentCell.Selected = true;
            }

            // Prepare the grid view for handling cell edits...
            ux_datagridviewMain.Enabled = true;
            ux_datagridviewMain.Focus();
            ux_datagridviewMain.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            ux_datagridviewMain.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            //ux_datagridviewMain.AllowUserToAddRows = true;
            ux_datagridviewMain.AllowUserToDeleteRows = true;
            ux_datagridviewMain.AllowDrop = true;

            // Set the context menu for the columns...
            foreach (DataGridViewColumn dgvc in ux_datagridviewMain.Columns)
            {
                dgvc.ContextMenuStrip = ux_contextmenustripEditDGVCell;
            }

            // Un-wire the event handler for formatting cells in readonly mode...
            ux_datagridviewMain.CellFormatting -= new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_ReadOnlyDGVCellFormatting);

            // Wire up the event handler for formatting cells in edit mode (populating the FK lookup values)...
            ux_datagridviewMain.CellFormatting += new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_EditDGVCellFormatting);
            ux_datagridviewMain.CellParsing += new DataGridViewCellParsingEventHandler(ux_datagridviewMain_EditDGVCellParsing);
            ux_datagridviewMain.DataError += new DataGridViewDataErrorEventHandler(dataGridView_DataError);
            
            // If a Form is being shown, enable its controls...
            if (dataviewForm != null)
            {
                System.Reflection.PropertyInfo pi = dataviewForm.GetType().GetProperty("EditMode");
                if(pi != null) pi.SetValue(dataviewForm, true, null);
            }
        }

        private void ux_buttonSaveData_Click(object sender, EventArgs e)
        {
            DataSet modifiedRecords = new DataSet();
            DataSet errorRecords;
            int errorCount = 0;
            bool origAllowUsersToAddRows = ux_datagridviewMain.AllowUserToAddRows;

            foreach (DataRowView drv in ((BindingSource)ux_datagridviewMain.DataSource).List)
            {
                if (drv.IsEdit || 
                    drv.Row.RowState == DataRowState.Added ||
                    drv.Row.RowState == DataRowState.Deleted ||
                    drv.Row.RowState == DataRowState.Detached ||
                    drv.Row.RowState == DataRowState.Modified)
                {
                    drv.EndEdit();
                    drv.Row.ClearErrors();
                }
            }
            // Get just the rows that have changed and put them in to a new dataset...
            if (((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).GetChanges() != null)
            {
                modifiedRecords.Tables.Add(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).GetChanges());
            }

            if (modifiedRecords.Tables.Contains(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).TableName) && 
                modifiedRecords.Tables[((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).TableName].GetErrors().Length > 0)
            {
//if (DialogResult.Yes == MessageBox.Show("The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to abort saving the data and review the errors now.\n(Click No to continue saving the data with errors).", "Data Errors Need Review", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)) return;
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to abort saving the data and review the errors now.\n(Click No to continue saving the data with errors).", "Data Errors Need Review", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ux_buttonSaveDataMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (DialogResult.Yes == ggMessageBox.ShowDialog()) return;
            }

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Temp. supress the AllowUserToAddRows property to allow processing of the save results...
            ux_datagridviewMain.AllowUserToAddRows = false;

            // If there are changes in this datagridview - save them now...
            if (modifiedRecords.Tables.Contains(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).TableName))
            {
                // Save the data...
                errorRecords = SaveDGVData(modifiedRecords);

                // Update/refresh the treeview items with changes that were just saved...
                errorCount = SyncSavedRecordsWithTreeViewAndDGV(errorRecords.Tables[((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).TableName]);
            }

            if (errorCount == 0)
            {
                // Restore the grid view for handling row selections...
                ux_datagridviewMain.Enabled = true;
                ux_datagridviewMain.Focus();
                ux_datagridviewMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                ux_datagridviewMain.EditMode = DataGridViewEditMode.EditProgrammatically;
                ux_datagridviewMain.AllowUserToAddRows = false;
                ux_datagridviewMain.AllowUserToDeleteRows = false;
                ux_datagridviewMain.AllowDrop = false;

                // Update the controls on the interface...
                ux_checkboxHighlightChanges.Enabled = false;
                ux_checkboxHideUnchangedRows.Checked = false;
                ux_checkboxHideUnchangedRows.Enabled = false;
                ux_buttonSaveData.Enabled = false;
                ux_buttonEditData.Enabled = true;
                ux_buttonCancelEditData.Enabled = false;
                ux_splitcontainerMain.Panel1.Enabled = true;
                ux_tabcontrolCTDataviews.Enabled = true;
// Un-wire the event handler for formatting cells in edit mode (populating the FK lookup values)...
ux_datagridviewMain.CellFormatting -= new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_EditDGVCellFormatting);
ux_datagridviewMain.CellParsing -= new DataGridViewCellParsingEventHandler(ux_datagridviewMain_EditDGVCellParsing);
ux_datagridviewMain.DataError -= new DataGridViewDataErrorEventHandler(dataGridView_DataError);

// Wire up the event handler for formatting cells in readonly mode...
ux_datagridviewMain.CellFormatting += new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_ReadOnlyDGVCellFormatting);

                //// NOTE: removing the table forces RefreshMainDataViewer to retrieve the data from the DB...
                //if (((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DataSet != null)
                //{
                //    ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DataSet.Tables.Remove((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource);
                //}

                // Remember the first visible column to restore later...
                int firstVisibleColumn = ux_datagridviewMain.FirstDisplayedScrollingColumnIndex;

                // Now refresh the data and a new copy of the table should be retrieved...
                // Resetting these two global variables will force a refresh of the DGV data...
                lastFullPath = "";
                lastTabName = "";
                RefreshMainDGVData();
                // Is the next line needed anymore???
                RefreshMainDGVFormatting();

                // Restore the dataview left-most column (so that the user's perceived transition from edit to readonly is more seamless)...
                if (ux_datagridviewMain.Columns[firstVisibleColumn].Visible) ux_datagridviewMain.FirstDisplayedScrollingColumnIndex = firstVisibleColumn;

// Moved the following code up above (before the refresh data and format calls)...
//// Un-wire the event handler for populating the FK lookup values...
//ux_datagridviewMain.CellFormatting -= new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_CellFormatting);
//ux_datagridviewMain.DataError -= new DataGridViewDataErrorEventHandler(dataGridView_DataError);

                // If a Form is being shown, disable its controls since we are done editing...
                if (dataviewForm != null)
                {
                    System.Reflection.PropertyInfo pi = dataviewForm.GetType().GetProperty("EditMode");
                    if (pi != null) pi.SetValue(dataviewForm, false, null);
                }
            }
            else
            {
                if (errorCount == 1)
                {
//MessageBox.Show("There was an error encountered during the Save operation.");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an error encountered during the Save operation.", "Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ux_buttonSaveDataMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                }
                else
                {
//MessageBox.Show("There were " + errorCount.ToString() + " errors encountered during the Save operation.");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were {0} errors encountered during the Save operation.", "Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ux_buttonSaveDataMessage3";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
ggMessageBox.ShowDialog();
                }

                // Restore the state of the AllowUserToAddRows property...
                ux_datagridviewMain.AllowUserToAddRows = origAllowUsersToAddRows;
                // If you are here there were errors in some of the changed rows so
                // update the datagridview with the results of the attempted save...
                RefreshMainDGVFormatting();
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }


        private void ux_buttonCancelEditData_Click(object sender, EventArgs e)
        {
            int intRowEdits = 0;
            bool bCancel = true;

            if (((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).GetChanges() != null)
            {
                intRowEdits = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).GetChanges().Rows.Count;
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s), are you sure you want to cancel?", "Cancel Edits", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ux_buttonCancelEditDataMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
//if (DialogResult.No == MessageBox.Show("You have " + intRowEdits + " unsaved row change(s), are you sure you want to cancel?", "Cancel Edits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
if (DialogResult.No == ggMessageBox.ShowDialog())
                {
                    bCancel = false;
                }
            }
            if (bCancel)
            {
                // Restore the grid view for handling row selections...
                ux_datagridviewMain.Enabled = true;
                ux_datagridviewMain.Focus();
                ux_datagridviewMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                ux_datagridviewMain.EditMode = DataGridViewEditMode.EditProgrammatically;
                ux_datagridviewMain.AllowUserToAddRows = false;
                ux_datagridviewMain.AllowUserToDeleteRows = false;
                ux_datagridviewMain.AllowDrop = false;

                // Update the controls on the interface...
                ux_checkboxHighlightChanges.Enabled = false;
                ux_checkboxHideUnchangedRows.Checked = false;
                ux_checkboxHideUnchangedRows.Enabled = false;
                ux_buttonSaveData.Enabled = false;
                ux_buttonEditData.Enabled = true;
                ux_buttonCancelEditData.Enabled = false;
                ux_splitcontainerMain.Panel1.Enabled = true;
                ux_tabcontrolCTDataviews.Enabled = true;

//// NOTE: removing the table forces RefreshMainDataViewer to retrieve the data from the DB...
//((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DataSet.Tables.Remove((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource);
                // Now refresh the data and a new copy of the table should be retrieved...
                // Resetting these two global variables will force a refresh of the DGV data...
                lastFullPath = "";
                lastTabName = "";

                // Remember the first visible column to restore later...
                int firstVisibleColumn = ux_datagridviewMain.FirstDisplayedScrollingColumnIndex;
                // Update the formatting for the datagridview cells (to remove highlighting of cells)
                RefreshMainDGVData();
                RefreshMainDGVFormatting();
                if (ux_datagridviewMain.Columns.Count > 0 &&
                    firstVisibleColumn > -1 &&
                    ux_datagridviewMain.Columns[firstVisibleColumn].Visible) ux_datagridviewMain.FirstDisplayedScrollingColumnIndex = firstVisibleColumn;

                // If a Form is being shown, enable its controls...
                if (dataviewForm != null)
                {
                    System.Reflection.PropertyInfo pi = dataviewForm.GetType().GetProperty("EditMode");
                    if (pi != null) pi.SetValue(dataviewForm, false, null);
                }

                // Un-wire the event handler for formatting cells in edit mode (populating the FK lookup values)...
                ux_datagridviewMain.CellFormatting -= new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_EditDGVCellFormatting);
                ux_datagridviewMain.CellParsing -= new DataGridViewCellParsingEventHandler(ux_datagridviewMain_EditDGVCellParsing);
                ux_datagridviewMain.DataError -= new DataGridViewDataErrorEventHandler(dataGridView_DataError);

                // Wire up the event handler for formatting cells in readonly mode...
                ux_datagridviewMain.CellFormatting += new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_ReadOnlyDGVCellFormatting);
            }
        }

        #endregion

        #region DataGridView logic...

        private void ux_datagridviewMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataView dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;

            // Reset the last char pressed global variable...
            _lastDGVCharPressed = (char)0;

            // If we are: 
            //  1) in edit mode, 
            //  2) the current cell is parked on a cell that is a FK lookup 
            //  3) and the Alt and Ctrl keys are not down
            // Then remember the keypress so that it can be passed into the Lookup Picker dialog...
            if (ux_buttonEditData.Enabled == false &&
                dv != null && 
                dgv.CurrentCell != null &&
                dgv.CurrentCell.ColumnIndex > -1 &&
                dgv.CurrentCell.RowIndex > -1)
            {
                DataColumn dc = dv.Table.Columns[dgv.CurrentCell.ColumnIndex];
                if (_sharedUtils.LookupTablesIsValidFKField(dc) && 
                    dgv.CurrentCell.RowIndex < dv.Count &&
                    dv[dgv.CurrentCell.RowIndex].Row.RowState != DataRowState.Deleted)
                {
                    if (!e.Alt && !e.Control)
                    {
                        string lastChar = e.KeyCode.ToString();
                        if (e.Shift)
                        {
                            _lastDGVCharPressed = lastChar.ToUpper()[0];
                        }
                        else
                        {
                            _lastDGVCharPressed = lastChar.ToLower()[0];
                        }
                    }
                }
            }

        }

        private void ux_datagridviewMain_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // Keyboard shortcuts for edit mode...
            if (!ux_buttonEditData.Enabled)
            {
                #region old_code...
                //if (e.KeyCode == Keys.D && e.Control)
                //{
                //    int minRow = dgv.Rows.Count; ;
                //    int maxRow = 0;
                //    //int minCol = dgv.Columns.Count;
                //    //int maxCol = 0;
                //    //foreach (DataGridViewCell cell in dgv.SelectedCells)
                //    //{
                //    //    if (cell.ColumnIndex < minCol) minCol = cell.ColumnIndex;
                //    //    if (cell.ColumnIndex > maxCol) maxCol = cell.ColumnIndex;
                //    //    if (cell.RowIndex < minRow) minRow = cell.RowIndex;
                //    //    if (cell.RowIndex > maxRow) maxRow = cell.RowIndex;
                //    //}
                //    foreach (DataGridViewCell cell in dgv.SelectedCells)
                //    {
                //        // Find the minimum row index in this column's selected cells...
                //        minRow = cell.RowIndex;
                //        for (minRow = cell.RowIndex; minRow > 0 && dgv.SelectedCells.Contains(dgv[cell.ColumnIndex, minRow - 1]); minRow--)
                //        {
                //        }
                //        // If the user is trying to perform a copy down (CTRL+D) using the row for adding a new row as the source row - bail out now...
                //        if (dgv.Rows[minRow].IsNewRow) return;

                //        // Now find the maximum row index for this column's selected cells...
                //        maxRow = cell.RowIndex;
                //        for (maxRow = cell.RowIndex; maxRow + 1 < dgv.Rows.Count && dgv.SelectedCells.Contains(dgv[cell.ColumnIndex, maxRow + 1]); maxRow++)
                //        {
                //        }
                //        // if the user is trying to perform a copy down (CTRL+D) with only one row selected - bail out now...
                //        if (minRow == maxRow) return;

                //        //object newValue = dgv[cell.ColumnIndex, minRow].Value;
                //        object newValue = ((DataRowView)dgv.Rows[minRow].DataBoundItem)[cell.ColumnIndex];
                //        for (int row = minRow + 1; row <= maxRow; row++)
                //        {
                //            if (dgv.SelectedCells.Contains(dgv[cell.ColumnIndex, row]) &&
                //                !dgv[cell.ColumnIndex, row].ReadOnly)
                //            {
                //                DataRowView dr = (DataRowView)dgv.Rows[row].DataBoundItem;
                //                if (dr == null) //if (dgv.Rows[row].IsNewRow)
                //                {
                //                    dgv[cell.ColumnIndex, row].Value = newValue;
                //                    dgv.UpdateCellValue(cell.ColumnIndex, row);
                //                    //dgv[cell.ColumnIndex, row].Style.BackColor = Color.Yellow;
                //                    RefreshDGVRowFormatting(dgv.Rows[row], ux_checkboxHighlightChanges.Checked);
                //                }
                //                else
                //                {
                //                    if (!dr[cell.ColumnIndex].Equals(newValue))
                //                    {
                //                        // Edit the DataRow (not the DataRowView) so that row state is changed...
                //                        dr.Row[cell.ColumnIndex] = newValue;
                //                        //dgv[cell.ColumnIndex, row].Style.BackColor = Color.Yellow;
                //                        RefreshDGVRowFormatting(dgv.Rows[row], ux_checkboxHighlightChanges.Checked);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                //if (e.KeyCode == Keys.N && e.Control)
                //{
                //    if (dgv.CurrentRow != null &&
                //        dgv.CurrentRow.Selected &&
                //        !dgv.CurrentRow.IsNewRow)
                //    {
                //        DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                //        DataRow sourceRow = null;
                //        DataRow destRow = null;
                //        if (dt != null)
                //        {
                //            sourceRow = dt.Rows[dgv.CurrentRow.Index];
                //            destRow = dt.NewRow();
                //        }
                //        if (sourceRow != null)
                //        {
                //            foreach (DataColumn dc in dt.Columns)
                //            {
                //                if (!dt.PrimaryKey.Contains(dc))
                //                {
                //                    switch (dc.ColumnName)
                //                    {
                //                        case "created_by":
                //                        case "owned_by":
                //                            destRow[dc] = cno;
                //                            break;
                //                        case "created_date":
                //                        case "owned_date":
                //                            destRow[dc] = DateTime.Now;
                //                            break;
                //                        case "modified_by":
                //                        case "modified_date":
                //                            break;
                //                        default:
                //                            //if (dc.ExtendedProperties.Contains("is_nullable") &&
                //                            //    dc.ExtendedProperties["is_nullable"].ToString() == "Y" ||
                //                            //    dc.ColumnName.StartsWith("is_"))
                //                            //{
                //                                // Column is not a required field (or is a boolean field that only allows Y or N)
                //                                destRow[dc] = sourceRow[dc];
                //                            //}
                //                            break;
                //                    }
                //                }
                //            }
                //            dt.Rows.InsertAt(destRow, dgv.CurrentRow.Index + 1);
                //            RefreshDGVRowFormatting(dgv.Rows[dgv.CurrentRow.Index + 1], ux_checkboxHighlightChanges.Checked);
                //        }
                //    }
                //}

                //if (e.KeyCode == Keys.OemQuotes && e.Control)
                //{
                //    if (dgv.CurrentRow != null &&
                //        dgv.CurrentRow.Index > 0)
                //    {
                //        int sourceRowIndex;
                //        DataRow sourceRow;
                //        DataRow destinationRow;
                //        if (dgv.CurrentRow.IsNewRow)
                //        {
                //            // The user is copying the values into the new row which doesn't have a datatable row yet - so create one using this HACK...
                //            // BEGIN HACK...
                //            //DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                //            //DataRow dr = dt.NewRow();
                //            //dt.Rows.Add(dr);
                //            //dt.Rows.Remove(dr);
                //            // END HACK...
                //            dgv.BeginEdit(true);
                //            sourceRowIndex = dgv.CurrentRow.Index - 1;
                //            sourceRow = ((DataRowView)dgv.Rows[sourceRowIndex].DataBoundItem).Row;
                //            destinationRow = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                //        }
                //        else
                //        {
                //            sourceRowIndex = dgv.CurrentRow.Index - 1;
                //            sourceRow = ((DataRowView)dgv.Rows[sourceRowIndex].DataBoundItem).Row;
                //            destinationRow = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                //        }

                //        if (sourceRow != null && destinationRow != null)
                //        {
                //            if (!destinationRow[dgv.CurrentCell.ColumnIndex].Equals(sourceRow[dgv.CurrentCell.ColumnIndex]))
                //            {
                //                if (!dgv.Columns[dgv.CurrentCell.ColumnIndex].ReadOnly)
                //                {
                //                    destinationRow[dgv.CurrentCell.ColumnIndex] = sourceRow[dgv.CurrentCell.ColumnIndex];
                //                }
                //            }
                //        }
                //        RefreshDGVRowFormatting(dgv.CurrentCell.OwningRow, ux_checkboxHighlightChanges.Checked);
                //    }
                //}

                //if (e.KeyCode == Keys.V && e.Control)
                //{
                //    IDataObject dataObj = Clipboard.GetDataObject();
                //    string pasteText = "";
                //    string[] junk = dataObj.GetFormats();
                //    if (dataObj.GetDataPresent(System.Windows.Forms.DataFormats.UnicodeText))
                //    {
                //        char[] rowDelimiters = new char[] { '\r', '\n' };
                //        char[] columnDelimiters = new char[] { '\t' };
                //        int badRows = 0;
                //        int missingRows = 0;
                //        bool importSuccess = false;
                //        pasteText = dataObj.GetData(DataFormats.UnicodeText).ToString();
                //        DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                //        importSuccess = ImportTextToDataTableUsingKeys(pasteText, dt, rowDelimiters, columnDelimiters, out badRows, out missingRows);
                //        if (!importSuccess)
                //        {
                //            // Paste the text into the DGV in 'block style'
                //            importSuccess = ImportTextToDataTableUsingBlockStyle(pasteText, dgv, rowDelimiters, columnDelimiters, out badRows, out missingRows);
                //        }
                //        RefreshMainDGVFormatting();
                //        RefreshForm();
                //    }
                //}

                //if (e.KeyCode == Keys.C && e.Control)
                //{
                //    string copyString = "";
                //    // First we need to get the min/max rows and columns for the selected cells...
                //    int minCol = dgv.Columns.Count;
                //    int maxCol = -1;
                //    int minRow = dgv.Rows.Count;
                //    int maxRow = -1;
                //    foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                //    {
                //        if (dgvc.ColumnIndex < minCol) minCol = dgvc.ColumnIndex;
                //        if (dgvc.ColumnIndex > maxCol) maxCol = dgvc.ColumnIndex;
                //        if (dgvc.RowIndex < minRow) minRow = dgvc.RowIndex;
                //        if (dgvc.RowIndex > maxRow) maxRow = dgvc.RowIndex;
                //    }

                //    // Now build the string to pass to the clipboard...
                //    for (int i = minRow; i <= maxRow; i++)
                //    {
                //        for (int j = minCol; j <= maxCol; j++)
                //        {
                //            switch (dgv[j, i].FormattedValueType.Name)
                //            {
                //                case "Boolean":
                //                    copyString += dgv[j, i].Value.ToString() + '\t';
                //                    break;
                //                default:
                //                    if (dgv[j, i].FormattedValue == null || dgv[j, i].FormattedValue.ToString().ToLower() == "[null]")
                //                    {
                //                        copyString += "" + '\t';
                //                    }
                //                    else
                //                    {
                //                        copyString += dgv[j, i].FormattedValue.ToString() + '\t';
                //                    }
                //                    break;
                //            }
                //        }
                //        copyString = copyString.TrimEnd('\t');
                //        copyString += "\r\n";
                //    }
                //    copyString = copyString.TrimEnd('\n');
                //    copyString = copyString.TrimEnd('\r');

                //    // Pass the new string to the clipboard...
                //    Clipboard.SetDataObject(copyString, false, 1, 1000);

                //    RefreshMainDGVFormatting();
                //    RefreshForm();
                //}

                //if (e.KeyCode == Keys.Delete)
                //{
                //    if (dgv.SelectedRows.Count == 0)
                //    {
                //        // The user is deleting values from individual selected cells (not entire rows)...
                //        foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                //        {
                //            DataRowView drv = (DataRowView)dgvc.OwningRow.DataBoundItem;
                //            if (drv == null) //if (dgv.Rows[row].IsNewRow)
                //            {
                //                dgvc.Value = "";
                //                dgv.UpdateCellValue(dgvc.ColumnIndex, dgvc.RowIndex);
                //                //dgv[dgvc.ColumnIndex, dgvc.RowIndex].Style.BackColor = Color.Yellow;
                //            }
                //            else
                //            {
                //                if (!drv[dgvc.OwningColumn.Index].Equals(DBNull.Value))
                //                {
                //                    if (!dgvc.ReadOnly)
                //                    {
                //                        // Edit the DataRow (not the DataRowView) so that row state is changed...
                //                        drv.Row[dgvc.OwningColumn.Index] = DBNull.Value;
                //                        // For unbound text cells we have to manually clear the cell's text...
                //                        if(string.IsNullOrEmpty(dgvc.OwningColumn.DataPropertyName)) dgvc.Value = "";
                //                        dgv.UpdateCellValue(dgvc.ColumnIndex, dgvc.RowIndex);
                //                        //dgv[dgvc.ColumnIndex, dgvc.RowIndex].Style.BackColor = Color.Yellow;
                //                    }
                //                }
                //            }
                //            RefreshDGVRowFormatting(dgvc.OwningRow, ux_checkboxHighlightChanges.Checked);
                //        }
                //    }
                //    else
                //    {
                //        // The user is attempting to delete entire rows from the datagridview...
                //        if (DialogResult.OK == MessageBox.Show("WARNING!!!  You are about to permanently delete " + dgv.SelectedRows.Count.ToString() + " records from the central database!\n\nAre you sure you want to do this?",
                //            "Record delete confirmation",
                //            MessageBoxButtons.OKCancel,
                //            MessageBoxIcon.Warning,
                //            MessageBoxDefaultButton.Button2))
                //        {
                //            foreach (DataGridViewRow dgvr in dgv.SelectedRows)
                //            {
                //                dgv.Rows.Remove(dgvr);
                //            }
                //            e.Handled = true;
                //        }
                //    }
                //}
                #endregion
                //_sharedUtils.ProcessDGVEditShortcutKeys(dgv, e, cno, ImportTextToDataTableUsingKeys, ImportTextToDataTableUsingBlockStyle);
                if (_sharedUtils.ProcessDGVEditShortcutKeys(dgv, e, _usernameCooperatorID))
                {
                    RefreshMainDGVFormatting();
// Commented out the RefreshForm() call to prevent the Form from popping up in front of the CT...
//RefreshForm();
                }
            }
        }


//private void ux_datagridviewMain_CellEnter(object sender, DataGridViewCellEventArgs e)
//{
//    if (ux_datagridviewMain.CurrentCell != null)
//    {
//        if (dataviewForm != null)
//        {
//            System.Reflection.MethodInfo methInfo = dataviewForm.GetType().GetMethod("CurrentRowChanged", new Type[] { typeof(DataRow), typeof(int), typeof(int) });
//            DataRow dr = ((DataRowView)ux_datagridviewMain.CurrentCell.OwningRow.DataBoundItem).Row;
//            if (methInfo != null && dr != null) methInfo.Invoke(dataviewForm, new object[] { dr, ux_datagridviewMain.CurrentCell.RowIndex, ux_datagridviewMain.Rows.Count });
//        }
//    }
//}

        private void ux_datagridviewMain_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                RefreshDGVRowFormatting(ux_datagridviewMain.CurrentCell.OwningRow, ux_checkboxHighlightChanges.Checked);
////if (ux_checkboxHighlightChanges.Checked)
////{
////    if(ux_datagridviewMain.CurrentCell != null) ux_datagridviewMain.CurrentCell.Style.BackColor = Color.Yellow;
////}
////else
////{
////    if (ux_datagridviewMain.CurrentCell != null) ux_datagridviewMain.CurrentCell.Style.BackColor = Color.Empty;
////}
//////if (ux_datagridviewMain.CurrentCell.Value.ToString() == "-99999999")
//////{
//////    if(ux_datagridviewMain.CurrentCell != null) ux_datagridviewMain.CurrentCell.Style.BackColor = Color.Red;
//////}
            }
        }

        private void ux_datagridviewMain_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.Rows[e.RowIndex].IsNewRow && dgv.CurrentCell.GetType() != typeof(DataGridViewCheckBoxCell))
            {
                DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                dgv.DataSource = null;
                dgv.CancelEdit();
                e.Cancel = true;
                // NOTE: If this is the DGV NewRow, then it is not bound to a row in the DataTable, so the following code is a HACK to force a new row into the bound DataTable...
                // BEGIN HACK...
        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        dgv.DataSource = dt;
        dgv.CurrentCell = dgv[e.ColumnIndex, e.RowIndex];
                //dt.Rows.Remove(dr);
                // END HACK...
                //foreach (DataColumn dc in dt.Columns)
                //{
                //    if ((dc.ExtendedProperties.Contains("is_nullable") && dc.ExtendedProperties["is_nullable"].ToString() == "N") &&
                //        (dc.ExtendedProperties.Contains("is_readonly") && dc.ExtendedProperties["is_readonly"].ToString() == "N"))
                //    {
                //        dgv.Rows[e.RowIndex].Cells[dc.ColumnName].Style.BackColor = Color.Violet;
                //    }
                //    if ((dc.ExtendedProperties.Contains("is_nullable") && dc.ExtendedProperties["is_nullable"].ToString() == "N") &&
                //        dc.ColumnName.StartsWith("is_"))
                //    {
                //        // Column is a required field (and is a boolean field that only allows Y or N)
                //        // TODO: Get the default value from the extended properties when it becomes available in the next version...
                //        dgv.Rows[e.RowIndex].Cells[dc.ColumnName].Value = "N";
                //        dgv.Rows[e.RowIndex].Cells[dc.ColumnName].Style.BackColor = Color.Empty;
                //    }
                //}
                //dgv.AllowUserToAddRows = true;
            }
            //ux_datagridviewMain.CurrentCell = ux_datagridviewMain[e.ColumnIndex, e.RowIndex];
            //RefreshMainDGVFormatting();
        }

        private void ux_datagridviewMain_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (!dgv.CurrentRow.IsNewRow)
            {
                DataRow dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                if (dr.GetColumnsInError().Contains(dr.Table.Columns[e.ColumnIndex]) && dr[e.ColumnIndex] != DBNull.Value) dr.SetColumnError(e.ColumnIndex, null);
            }
            RefreshDGVRowFormatting(dgv.CurrentRow, ux_checkboxHighlightChanges.Checked);
            //if (!ux_datagridviewMain.AllowUserToAddRows) ux_datagridviewMain.AllowUserToAddRows = true;
        }

        private void ux_datagridviewMain_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataTable dt = null;
            if (dgv.DataSource.GetType() == typeof(BindingSource))
            {
                dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            }
            else
            {
                dt = (DataTable)dgv.DataSource;
            }
            string columnName = dgv.CurrentCell.OwningColumn.Name;
            DataColumn dc = dt.Columns[columnName];
            DataRow dr;

            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                //string luTableName = dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim();
                string luTableName = dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim();
                dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                //GrinGlobal.Client.Data.LookupTablePicker ltp = new GrinGlobal.Client.Data.LookupTablePicker(lookupTables, localDBInstance, columnName, dr, dgv.CurrentCell.EditedFormattedValue.ToString());
                string suggestedFilter = dgv.CurrentCell.EditedFormattedValue.ToString();
                if (_lastDGVCharPressed > 0) suggestedFilter = _lastDGVCharPressed.ToString();
//GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(lookupTables, columnName, dr, suggestedFilter);
                GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(_sharedUtils, columnName, dr, suggestedFilter);
                _lastDGVCharPressed = (char)0;
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
                        else if(ltp.NewKey == null)
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

//        private void ux_datagridviewMain_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
//        {
//            DataGridView dgv = (DataGridView)sender;
//            System.Collections.Generic.Dictionary<string, string> lookupFilters = new System.Collections.Generic.Dictionary<string, string>();
//            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
//            string columnName = dgv.CurrentCell.OwningColumn.Name;
//            DataColumn dc = dt.Columns[columnName];

//            if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "LARGE_SINGLE_SELECT_CONTROL" &&
//                dc.ExtendedProperties.Contains("foreign_key_field_name") && dc.ExtendedProperties["foreign_key_field_name"].ToString().Length > 0 &&
//                dc.ExtendedProperties.Contains("foreign_key_resultset_name") && dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Length > 0)
//            {
//                string luTableName = dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim();
//                    // Check to see if this is a hybrid combobox - if not start the lookup picker dialog...
//                    if (e.Control.GetType() != typeof(DataGridViewComboBoxEditingControl))
//                    {
//                        //string dataSourceCellValue = "";
//                        //System.Collections.Generic.List<object> pKeyVals = new System.Collections.Generic.List<object>();

//                        DataRow dr;
//                        //DataRowView drv = ((DataRowView)dgv.CurrentRow.DataBoundItem);
//                        // Check to see if this is a DGV NewRow and if so, create a new row in the bound table (because
//                        // the NewRow row in a DGV is not bound to anything...
//                        if (dgv.CurrentRow.IsNewRow)
//                        {
////int rowIndex = dgv.CurrentCell.RowIndex;
////int colIndex = dgv.CurrentCell.ColumnIndex;
////dgv.CancelEdit(); // This will take the NewRow's CurrentCell out of edit mode (otherwise two rows will be created)
////dr = ((DataTable)((BindingSource)dgv.DataSource).DataSource).NewRow();
////((DataTable)((BindingSource)dgv.DataSource).DataSource).Rows.Add(dr);
////dgv.CurrentCell = dgv[colIndex, rowIndex];

//                            // The user is copying the values into the new row which doesn't have a datatable row yet - so create one using this HACK...
//                            // BEGIN HACK...
//                            int rowIndex = dgv.CurrentCell.RowIndex;
//                            int colIndex = dgv.CurrentCell.ColumnIndex;
//                            ux_datagridviewMain.AllowUserToAddRows = false;
//                            ux_datagridviewMain.AllowUserToAddRows = true;
//                            //dgv.CancelEdit();
//                    dr = dt.NewRow();
//                    dt.Rows.Add(dr);
//                    //dt.Rows.Remove(dr);
//                            dgv.CurrentCell = dgv[colIndex, rowIndex];
//                            //ux_datagridviewMain.AllowUserToAddRows = true;
//                            // END HACK...

//                        }
//                        else
//                        {
//                            //dr = ((DataTable)((BindingSource)dgv.DataSource).DataSource).NewRow();
//                            dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
//                        }

//                        // If the current row is bound to a data source (new rows may not be yet) get the suggested lookup value...
//                        //if(dr != null) dataSourceCellValue = dr[dgv.CurrentCell.ColumnIndex].ToString();
//                        //LookupTablePicker ltp = new LookupTablePicker(dataSourceCellValue, luTable, lookupFilters);
//                        LookupTablePicker ltp = new LookupTablePicker(lookupTables, localDBInstance, columnName, dr, dgv.CurrentCell.EditedFormattedValue.ToString());
//                        if (DialogResult.OK == ltp.ShowDialog())
//                        {
//                            dgv.CurrentCell.Value = ltp.NewValue.Trim();
//                            if (dr != null)
//                            {
//                                if (ltp.NewKey != null && dr[dgv.CurrentCell.ColumnIndex].ToString().Trim() != ltp.NewKey.Trim())
//                                {
//                                    dr[dgv.CurrentCell.ColumnIndex] = ltp.NewKey.Trim();
//                                }
//                                else
//                                {
//                                    dr[dgv.CurrentCell.ColumnIndex] = DBNull.Value;
//                                }
//                            }
//                        }
//                        //dgv.EndEdit();
//                    }
//                    else // This is a hybrid combobox - attempt to refine the list to just choices associated with the current row...
//                    {
//                        string rowFilter = "";
//                        foreach (string filterKey in lookupFilters.Keys)
//                        {
//                            if (!string.IsNullOrEmpty(lookupFilters[filterKey]))
//                            {
//                                rowFilter += filterKey + "=" + lookupFilters[filterKey] + " AND ";
//                            }
//                        }
//                        if(!string.IsNullOrEmpty(rowFilter))rowFilter = rowFilter.Remove(rowFilter.LastIndexOf(" AND "));
//                        DataGridViewComboBoxEditingControl editControl = (DataGridViewComboBoxEditingControl)e.Control;
//                        DataTable editControlDataSource = (DataTable)((BindingSource)dgv.DataSource).DataSource;

//                        editControlDataSource.DefaultView.RowFilter = rowFilter;
//                        editControl.SelectedValue = dgv.CurrentCell.Value;
//                    }
//                //}
//            }
//        }

        void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
//string errorMessage = e.Exception.Message;
//int columnWithError = -1;

//// Find the cell the error belongs to (don't use e.ColumnIndex because it points to the current cell *NOT* the offending cell)...
//foreach (DataGridViewColumn col in dgv.Columns)
//{
//    if (errorMessage.Contains(col.Name))
//    {
//        dgv[col.Name, e.RowIndex].ErrorText = errorMessage;
//        columnWithError = col.Index;
//    }
//}
            // Display a warning message to the user that invalid data is entered for integer and datetime fields (but not FK lookups)...
            if ((dgv[e.ColumnIndex, e.RowIndex].ValueType == typeof(int) ||
                dgv[e.ColumnIndex, e.RowIndex].ValueType == typeof(DateTime)) &&
                !string.IsNullOrEmpty(dgv[e.ColumnIndex, e.RowIndex].OwningColumn.DataPropertyName))
            {
                // The code commented out below does not show properly because the edit control is overlaying the dgvc!!!
                //dgv[e.ColumnIndex, e.RowIndex].ErrorText = e.Exception.Message;
                //dgv.UpdateCellErrorText(e.ColumnIndex, e.RowIndex);
                //dgv.UpdateCellValue(e.ColumnIndex, e.RowIndex);
                // So we use this code instead...
//MessageBox.Show(e.Exception.Message);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox(e.Exception.Message, "Cancel Edits", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "dataGridView_DataErrorMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
            }
        }

        void ux_datagridviewMain_EditDGVCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            // Check to see if this cell is in a column that needs a FK lookup...
// NOTE: took out the following line to properly 'refresh' the cells below a combox that was exposing its list...
//            if (string.IsNullOrEmpty(dgv.Columns[e.ColumnIndex].DataPropertyName))
//            {
            DataView dv = null;
            if (dgv.DataSource.GetType() == typeof(BindingSource))
            {
                dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            }
            else
            {
                dv = ((DataTable)dgv.DataSource).DefaultView;
            }
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

//            }
                    else if (e.Value != null &&
                        e.Value != DBNull.Value && 
                        dc.DataType == typeof(DateTime))
                    {
                        if (dgv.Columns.Contains(dc.ColumnName + "_code"))
                        {
                            string dateFormat = dv[e.RowIndex][dc.ColumnName + "_code"].ToString().Trim();
                            e.Value = ((DateTime)dv[e.RowIndex][e.ColumnIndex]).ToString(dateFormat);
                            e.FormattingApplied = true;
                        }

                    }
                    else if (e.Value != null &&
                        e.Value != DBNull.Value &&
                        (dc.DataType == typeof(int) ||
                        dc.DataType == typeof(Int16) ||
                        dc.DataType == typeof(Int32) ||
                        dc.DataType == typeof(Int64)))
                        //(dc.DataType.Name.ToLower() == "int" ||
                        //dc.DataType.Name.ToLower() == "int16" ||
                        //dc.DataType.Name.ToLower() == "int32" ||
                        //dc.DataType.Name.ToLower() == "int64"))
                    {
                        int junk;
                        if (!int.TryParse(e.Value.ToString(), out junk))
                        {
                            dgv[e.ColumnIndex, e.RowIndex].ErrorText = dv[e.RowIndex].Row.GetColumnError(dc);
                        }
                    }

                    if (dc.ReadOnly)
                    {
                        e.CellStyle.BackColor = Color.LightGray;
                    }
                    else if (dv.Table.Columns[e.ColumnIndex].ExtendedProperties.Contains("is_nullable") &&
                        dv.Table.Columns[e.ColumnIndex].ExtendedProperties["is_nullable"].ToString() == "N" &&
                        !dv.Table.Columns[e.ColumnIndex].ColumnName.StartsWith("is_") &&
                        dv[e.RowIndex][e.ColumnIndex] == DBNull.Value)
                    {
                        e.CellStyle.BackColor = Color.Plum;
                    }
                }
        }

        void ux_datagridviewMain_EditDGVCellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataView dv = null;
            if (dgv.DataSource.GetType() == typeof(BindingSource))
            {
                dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            }
            else
            {
                dv = ((DataTable)dgv.DataSource).DefaultView;
            }
            if (dv != null && e.ColumnIndex > -1)
            {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                if (e.Value != null &&
                    e.Value != DBNull.Value &&
                    dc.DataType == typeof(DateTime))
                {
                    if (dgv.Columns.Contains(dc.ColumnName + "_code"))
                    {
                        string dateFormat = dv[e.RowIndex][dc.ColumnName + "_code"].ToString().Trim();
                        DateTime formattedDate;
                        //e.Value = ((DateTime)dv[e.RowIndex][e.ColumnIndex]).ToString(dateFormat);
                        if (DateTime.TryParseExact(e.Value.ToString(), dateFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out formattedDate))
                        {
                            e.Value = formattedDate;
                            e.ParsingApplied = true;
                        }
                    }

                }
            }
        }

        void ux_datagridviewMain_ReadOnlyDGVCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            DataView dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            if (dv != null && e.ColumnIndex > -1)
            {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                // Format the date fields if a format code is available...
                if (dc.DataType == typeof(DateTime))
                {
                    if (dgv.Columns.Contains(dc.ColumnName + "_code"))
                    {
                        string dateFormatDisplayMember = dv[e.RowIndex][dc.ColumnName + "_code"].ToString().Trim();
                        string dateFormatValueMember = "";
//if (!string.IsNullOrEmpty(dateFormatDisplayMember)) dateFormatValueMember = _sharedUtils.GetLookupValueMember("code_value_lookup", dateFormatDisplayMember, "group_name='" + dv.Table.Columns[dc.ColumnName + "_code"].ExtendedProperties["group_name"].ToString() + "'", "");
                        if (!string.IsNullOrEmpty(dateFormatDisplayMember)) dateFormatValueMember = _sharedUtils.GetLookupValueMember(dv[e.RowIndex].Row, "code_value_lookup", dateFormatDisplayMember, dv.Table.Columns[dc.ColumnName + "_code"].ExtendedProperties["group_name"].ToString(), "");
                        if (e.Value != null &&
                            e.Value != DBNull.Value)
                        {
                            e.Value = ((DateTime)dv[e.RowIndex][e.ColumnIndex]).ToString(dateFormatValueMember);
                            e.FormattingApplied = true;
                        }
                    }
                }

                if (dc.ReadOnly)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }
            }
        }
        
        private void ux_datagridviewMain_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            //if (e.Row.IsNewRow)
            //{
            //    DataTable dt = (DataTable)e.Row.DataGridView.DataSource;

            //    foreach (DataColumn dc in dt.Columns)
            //    {
            //    }
            //}
        }

        private void ux_datagridviewMain_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (ux_checkboxHotSyncTreeview.Checked)
            {
                DataGridView dgv = (DataGridView)sender;

                // If this is the new row format the cells to give the user visual cues about what is a required field...
                if (dgv.Rows[e.RowIndex].IsNewRow)
                {
                    RefreshDGVRowFormatting(dgv.Rows[e.RowIndex], ux_checkboxHighlightChanges.Checked);
                }

                if (!dgv.Rows[e.RowIndex].IsNewRow &&
                    _ux_NavigatorTabControl.SelectedTab != null &&
                    _ux_NavigatorTabControl.SelectedTab.Name != "ux_tabpageGroupListNavigatorNewTab")
                {
                    DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                    TreeView tv = (TreeView)_ux_NavigatorTabControl.SelectedTab.Controls[0];
                    TreeNode[] linkedNodes;
                    DataRowView drv = ((DataRowView)dgv[e.ColumnIndex, e.RowIndex].OwningRow.DataBoundItem);

                    // Reset each node to the default font for the control...
ResetTreeviewNodeFormatting(tv.Nodes);
//foreach (TreeNode tn in tv.Nodes)
//{
//    ResetNodeFormat(tn);
//}

                    // Iterate through each column in the datatable to see if any columns point at items in the treeview...
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string colName = dc.ColumnName.Trim().ToUpper();

                        // Treat the primary key column different than other FK lookups...
                        if (dt.PrimaryKey.Length == 1 && colName == dt.PrimaryKey[0].ColumnName.Trim().ToUpper()) colName = "THIS_DATAVIEWS_PRIMARY_KEY";

                        switch (colName)
                        {
                            case "ACCESSION_ID":
                            case "INVENTORY_ID":
                            case "PARENT_INVENTORY_ID":
                            case "BACKUP_INVENTORY_ID":
                            case "ORDER_REQUEST_ID":
                                //linkedNodes = tv.Nodes.Find(dgv[colName, e.RowIndex].Value.ToString(), true);
                                //linkedNodes = tv.Nodes.Find(dt.Rows[e.RowIndex][colName].ToString(), true);
                                string fKey = "";
                                if (ux_buttonEditData.Enabled)
                                {
                                    //string lookupTableName = dt.Columns[colName].ExtendedProperties["foreign_key_resultset_name"].ToString();
                                    string lookupTableName = dt.Columns[colName].ExtendedProperties["foreign_key_dataview_name"].ToString();
                                    //fKey = lookupTables.GetValueMember(lookupTableName, drv[colName].ToString(), "", drv[colName].ToString());
                                    fKey = _sharedUtils.GetLookupValueMember(drv.Row, lookupTableName, drv[colName].ToString(), "", drv[colName].ToString());
                                }
                                else
                                {
                                    fKey = drv[colName].ToString();
                                }
                                //linkedNodes = tv.Nodes.Find(fKey, true);
                                linkedNodes = tv.SelectedNode.Nodes.Find(fKey, false);
                                foreach (TreeNode tn in linkedNodes)
                                {
                                    // Get the font used by the TreeView because the NodeFont property is set to the default null...
                                    Font italicFont = new Font(tv.Font, FontStyle.Italic | FontStyle.Underline);
                                    // Set the font for each node...
                                    tn.NodeFont = italicFont;
                                    // KLUDGE: Set the node text to itself to force a recalc of the clipping rectangle (otherwise some
                                    // of the text could get clipped because the new font takes more space than the original font for the same text)...
                                    tn.Text = tn.Text;
                                }
                                break;
                            case "THIS_DATAVIEWS_PRIMARY_KEY":
                                //linkedNodes = tv.Nodes.Find(dgv[dt.PrimaryKey[0].ColumnName, e.RowIndex].Value.ToString(), true);
                                //linkedNodes = tv.Nodes.Find(dt.Rows[e.RowIndex][dt.PrimaryKey[0].ColumnName].ToString(), true);
                                //linkedNodes = tv.Nodes.Find(drv[dt.PrimaryKey[0].ColumnName].ToString(), true);
                                linkedNodes = tv.SelectedNode.Nodes.Find(drv[dt.PrimaryKey[0].ColumnName].ToString(), false);
                                foreach (TreeNode tn in linkedNodes)
                                {
                                    // Get the font used by the TreeView because the NodeFont property is set to the default null...
                                    Font boldFont = new Font(tv.Font, FontStyle.Bold);
                                    // Set the font for each node...
                                    tn.NodeFont = boldFont;
                                    // KLUDGE: Set the node text to itself to force a recalc of the clipping rectangle (otherwise some
                                    // of the text could get clipped because the new font takes more space than the original font for the same text)...
                                    tn.Text = tn.Text;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void ux_datagridviewMain_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // If this is the new row reset the format of the cells to give the user visual cues that they are no longer editing the new row...
            if (dgv.Rows[e.RowIndex].IsNewRow)
            {
                // Reset the rows cells to the default color scheme...
                foreach (DataGridViewCell dgvc in dgv.Rows[e.RowIndex].Cells)
                {
                    dgvc.Style.BackColor = Color.Empty;
                    dgvc.Style.SelectionBackColor = Color.Empty;
                }
            }
        }

        private void ux_datagridviewMain_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //if (!e.Row.IsNewRow && DialogResult.Cancel == MessageBox.Show("WARNING!!!  You are permanently deleting this record from the central database!\n\nAre you sure you want to do this?",
            //                                                            "Record delete confirmation",
            //                                                            MessageBoxButtons.OKCancel,
            //                                                            MessageBoxIcon.Warning,
            //                                                            MessageBoxDefaultButton.Button2))
            //{
            //    e.Cancel = true;
            //}
        }
        
        private void ux_datagridviewMain_Leave(object sender, EventArgs e)
        {
            // Remember all user settings...
            if (ux_buttonEditData.Enabled)
            {
                SetAllUserSettings();
            }
        }

        private void ux_datagridviewMain_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!ux_buttonEditData.Enabled && (e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                DataTable dt = null;
                if (ux_datagridviewMain.DataSource.GetType() == typeof(BindingSource) &&
                    ((BindingSource)ux_datagridviewMain.DataSource).DataSource.GetType() == typeof(DataTable))
                {
                    dt = (DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource;
                }
                else if (ux_datagridviewMain.DataSource.GetType() == typeof(DataTable))
                {
                    dt = (DataTable)ux_datagridviewMain.DataSource;
                }
                if (dt != null &&
                    !dt.Columns[e.ColumnIndex].ReadOnly &&
                    dt.Columns[e.ColumnIndex].ExtendedProperties.Contains("gui_hint") &&
                    dt.Columns[e.ColumnIndex].ExtendedProperties["gui_hint"].ToString() == "LARGE_SINGLE_SELECT_CONTROL")
                {
                    //ux_datagridviewMain.Cursor = Cursors.PanNE;
                    ux_datagridviewMain.Cursor = _cursorLUT;
                }
                else
                {
                    //ux_datagridviewMain.Cursor = Cursors.Default; 
                    ux_datagridviewMain.Cursor = _cursorGG;
                }
            }
            //else if (ux_datagridviewMain.Cursor == Cursors.PanNE && (e.RowIndex > -1) && (e.ColumnIndex > -1))
            else if (ux_datagridviewMain.Cursor == _cursorLUT && (e.RowIndex > -1) && (e.ColumnIndex > -1))
            {
                //ux_datagridviewMain.Cursor = Cursors.Default; 
                ux_datagridviewMain.Cursor = _cursorGG;
            }
        }

        private void ux_datagridviewMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && (ux_datagridviewMain.Rows[e.RowIndex].Selected) && (e.Button == MouseButtons.Left))
            {
                // If there were selected rows lets start the drag-drop operation...
                if (ux_datagridviewMain.SelectedRows.Count > 0)
                {
                    // Change cursor to the wait cursor...
                    Cursor origCursor = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    
                    DataObject doDragDropData;
                    // Get the string of selected rows extracted from the GridView...
                    doDragDropData = BuildDragAndDropDGVData(ux_datagridviewMain);
                    ux_datagridviewMain.DoDragDrop(doDragDropData, DragDropEffects.Copy);

                    // Restore cursor to default cursor...
                    Cursor.Current = origCursor;
                }
            }
            //if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && (e.Button == MouseButtons.Right) && ux_buttonEditData.Enabled)
            //{
            //    // Change the color of the cell background so that the user
            //    // knows what cell the context menu applies to...
            //    ux_datagridviewMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
            //    ux_datagridviewMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Red;
            //}
        }

        private void ux_datagridviewMain_DragOver(object sender, DragEventArgs e)
        {
            // Okay we are in the middle of a Drag and Drop operation and the mouse is in 
            // the DGV control so lets handle this event...

            // This code will change the cursor icon to give the user feedback about whether or not
            // the drag-drop operation is allowed...
            //

            // Get the DGV object...
            DataGridView dgv = (DataGridView)sender;

            // Convert the mouse coordinates from screen to client...
            Point ptClientCoord = dgv.PointToClient(new Point(e.X, e.Y));

            // Is this a string being dragged to a node...
            if (e.Data.GetDataPresent(typeof(string)) && !dgv.ReadOnly)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ux_datagridviewMain_DragDrop(object sender, DragEventArgs e)
        {
            // The drag-drop event is coming to a close process this event to handle the dropping of
            // data into the treeview...

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Get the DGV object...
            DataGridView dgv = (DataGridView)sender;

            // Is this an allowed drop???
            if (e.Effect != DragDropEffects.None)
            {
                // Is this a collection of dataset rows being dragged to the DGV...
                if (e.Data.GetDataPresent(typeof(string)))
                {
                    char[] rowDelimiters = new char[] { '\r', '\n' };
                    char[] columnDelimiters = new char[] { '\t' };
                    string rawText = (string)e.Data.GetData(typeof(string));
                    DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
                    int badRows = 0;
                    int missingRows = 0;
                    bool importSuccess = false;
                    importSuccess = _sharedUtils.ImportTextToDataTableUsingKeys(rawText, dt, rowDelimiters, columnDelimiters, out badRows, out missingRows);
                    if (!importSuccess)
                    {
                        Point clientCoord = ux_datagridviewMain.PointToClient(new Point(e.X, e.Y));
                        System.Windows.Forms.DataGridView.HitTestInfo hti = ux_datagridviewMain.HitTest(clientCoord.X, clientCoord.Y);
                        ux_datagridviewMain.CurrentCell = ux_datagridviewMain[hti.ColumnIndex, hti.RowIndex];
                        importSuccess = _sharedUtils.ImportTextToDataTableUsingBlockStyle(rawText, dgv, rowDelimiters, columnDelimiters, out badRows, out missingRows);
                    }

                    RefreshMainDGVFormatting();

                }
                // Or is this image files being dragged in from the File Explorer...
                else if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] fullPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                    Point clientCoord = ux_datagridviewMain.PointToClient(new Point(e.X, e.Y));
                    System.Windows.Forms.DataGridView.HitTestInfo hti = ux_datagridviewMain.HitTest(clientCoord.X, clientCoord.Y);
                    DataTable dt = (DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource;
                    if (dt.PrimaryKey.Length == 1 &&
                        (dt.PrimaryKey[0].ColumnName == "inventory_id" || dt.PrimaryKey[0].ColumnName == "accession_id") &&
                        hti.RowIndex > -1)
                    {
                        string inventoryIDs = "";
                        // If the user dragged image file on to a dataview that has inventory_id at the primary key...
                        if (dt.PrimaryKey[0].ColumnName == "inventory_id")
                        {
                            if (ux_datagridviewMain.Rows[hti.RowIndex].Selected)
                            {
                                foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
                                {
                                    inventoryIDs += dgvr.Cells["inventory_id"].Value.ToString() + ",";
                                }
                                inventoryIDs = inventoryIDs.TrimEnd(',');
                            }
                            else if (hti.RowIndex > -1)
                            {
                                // The drop was not on a DGV row that was selected so default to the row under the mouse icon...
                                inventoryIDs = ux_datagridviewMain.Rows[hti.RowIndex].Cells["inventory_id"].Value.ToString();
                            }
                            else
                            {
                                // Don't have a clue which inventory_id to associate the images with - so do nothing...
                            }
                        }
                        // If the user dragged image file on to a dataview that has accession_id at the primary key...
                        else if (dt.PrimaryKey[0].ColumnName == "accession_id")
                        {
                            // First gather up the Accession IDs...
                            string accessionIDs = "";
                            if (ux_datagridviewMain.Rows[hti.RowIndex].Selected)
                            {
                                foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
                                {
                                    accessionIDs += ((DataRowView)dgvr.DataBoundItem).Row["accession_id"].ToString() + ",";
                                }
                                accessionIDs = accessionIDs.TrimEnd(',');
                            }
                            else if (hti.RowIndex > -1)
                            {
                                // The drop was not on a DGV row that was selected so default to the row under the mouse icon...
                                accessionIDs = ((DataRowView)ux_datagridviewMain.Rows[hti.RowIndex].DataBoundItem).Row["accession_id"].ToString();
                            }
                            else
                            {
                                // Don't have a clue which accession_id to associate the images with - so do nothing...
                            }
                            // Now convert the Accession IDs to Inventory IDs...
                            if (!string.IsNullOrEmpty(accessionIDs))
                            {
                                //DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=; :accessionid=" + accessionIDs + "; :orderrequestid=; :cooperatorid=;", 0, 0);
                                DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + accessionIDs, 0, 0);
                                if (ds.Tables.Contains("get_inventory"))
                                {
//DataRow[] drs = ds.Tables["get_inventory"].Select("inventory_type_code='**'");
                                    DataRow[] drs = ds.Tables["get_inventory"].Select("FORM_TYPE_CODE='**'");
                                    foreach (DataRow dr in drs)
                                    {
                                        inventoryIDs += dr["INVENTORY_ID"].ToString() + ",";
                                    }
                                    inventoryIDs = inventoryIDs.TrimEnd(',');
                                }
                            }
                        }

                        // We are done gathering data - now call the wizard to load the inventory images...
                        LoadInventoryImages(fullPaths, inventoryIDs);
                    }
                    else if (((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).PrimaryKey.Length == 1 &&
                        ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).PrimaryKey[0].ColumnName == "order_request_id" &&
                        hti.RowIndex > -1)
                    {
                    }
                }
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

//                bool pkeysPresent = true;
//                foreach (DataColumn pkey in dt.PrimaryKey)
//                {
//                    if(!FindText(_sharedUtils.GetFriendlyFieldName(pkey, pkey.ColumnName), rawText, false, rowDelimiters, columnDelimiters))
//                    {
//                        pkeysPresent = false;
//                    }
//                }
//                if (pkeysPresent)
//                {
//                    ImportTextToDataTable(rawText, dt, rowDelimiters, columnDelimiters, out badRows, out missingRows);
//                    string errorMessage = "NOTE: skipped " + badRows.ToString() + " row(s) because the row content could not be parsed properly.";
//                    if (badRows > 0) MessageBox.Show(errorMessage);
//                    errorMessage = "NOTE: skipped " + missingRows.ToString() + " row(s) because a matching row could not be found in current data viewer (spreadsheet).";
//                    if (missingRows > 0) MessageBox.Show(errorMessage);
//                    RefreshMainDGVFormatting();
//                }
//                else
//                {
//                    bool uniqueKeysPresent = true;
//                    string[] uniqueKeyColumnNames = null;
//                    if (dt.PrimaryKey.Length > 0 &&
//                        dt.PrimaryKey[0].ExtendedProperties.Contains("alternate_key_fields") &&
//                        dt.PrimaryKey[0].ExtendedProperties["alternate_key_fields"].ToString().Length > 0)
//                    {
//                        uniqueKeyColumnNames = dt.PrimaryKey[0].ExtendedProperties["alternate_key_fields"].ToString().Split(',');
//                    }
//                    // Attempt to find any column headers...
//                    DataColumn dc = null;
//                    foreach (string uniqueColumnName in uniqueKeyColumnNames)
//                    {
//                        dc = dt.Columns[uniqueColumnName.Trim().ToLower()];
//                        if (!FindText(_sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName), rawText, false, rowDelimiters, columnDelimiters))
//                        {
//                            uniqueKeysPresent = false;
//                        }
//                    }
////////ux_datagridviewMain.Rows[ux_datagridviewMain.Rows.GetLastRow(DataGridViewElementStates.None)]
////////Point screenCoord = new Point(e.X, e.Y);
//////Point clientCoord = ux_datagridviewMain.PointToClient(new Point(e.X, e.Y));
//////if (uniqueKeysPresent && ux_datagridviewMain.Rows[ux_datagridviewMain.HitTest(clientCoord.X, clientCoord.Y).RowIndex].IsNewRow)
//////{

//////}
////////MessageBox.Show("Screen: " + ux_datagridviewMain.HitTest(screenCoord.X, screenCoord.Y).RowIndex.ToString() + " Client: " + ux_datagridviewMain.HitTest(clientCoord.X, clientCoord.Y).RowIndex.ToString());
////////if (ux_datagridviewMain.Rows[0].Cells.Contains(dropControl)) MessageBox.Show("Test");
//                }

        //public bool FindText(string textToFind, string textToSearch, bool ignoreCase, char[] rowDelimiters, char[] columnDelimters)
        //{
        //    bool foundText = false;
        //    string[] textLinesToSearch = textToSearch.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (string lineText in textLinesToSearch)
        //    {
        //        string[] columnsOfText = lineText.Split(columnDelimters, StringSplitOptions.None);
        //        foreach (string columnText in columnsOfText)
        //        {
        //            if (ignoreCase)
        //            {
        //                if (columnText.ToLower().Trim().Equals(textToFind.ToLower().Trim()))
        //                {
        //                    foundText = true;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                if (columnText.Trim().Equals(textToFind.Trim()))
        //                {
        //                    foundText = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (foundText)
        //        {
        //            break;
        //        }
        //    }
        //    return foundText;
        //}

//        public bool ImportTextToDataTableUsingKeys(string rawImportText, DataTable destinationTable, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
//        {
//            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
//            string[] uniqueKeyColumnNames = null;
//            bool primaryKeyFound = false;
//            System.Collections.Generic.List<DataColumn> uniqueKeys = new System.Collections.Generic.List<DataColumn>();
//            bool processedImportSuccessfully = false;
//            badRows = 0;
//            missingRows = 0;
//            // Make sure there is text to process - if not bail out now...
//            if (rawImportRows == null || rawImportRows.Length <= 0) return false;
//            // Begin looking for a row of raw text that contains the column headers for the destination datatable...
//            // This is a 2 phase approach that first looks for a row that contains all of the primary key column names
//            // But if that is not found - try again to find a row of raw text that contains all of the column names for the unique compound key
//            int columnHeaderRowIndex = -1;
//            // PHASE 1:
//            // Look for a raw text line that contains the full text name of the primary key columns (they must all be on the same line of raw text)...
//            if (destinationTable.PrimaryKey.Length > 0)
//            {
//                // Look through all of the rows of raw text for a single row that contains all of the primary key column names
//                for (int i = 0; i < rawImportRows.Length && columnHeaderRowIndex == -1; i++)
//                {
//                    columnHeaderRowIndex = i; // Start out ASSUMING this is the 'right' row...
//                    foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
//                    {
//                        if (!FindText(_sharedUtils.GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName), rawImportRows[i], false, rowDelimiters, columnDelimiters))
//                        {
//                            // If the column header was not matched using case sensitive - try matching again (case insensitive)...
//                            if (!FindText(_sharedUtils.GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName), rawImportRows[i], true, rowDelimiters, columnDelimiters))
//                            {
//                                // If the column header was still not matched - try the raw table field name...
//                                if (!FindText(pKeyColumn.ColumnName, rawImportRows[i], true, rowDelimiters, columnDelimiters))
//                                {
//                                    // The ASSUMPTION was wrong because the header text for one of the required primary key columns is missing in this raw text row...
//                                    columnHeaderRowIndex = -1;
//                                }
//                            }
//                        }
//                    }
//                }
//                if (columnHeaderRowIndex != -1) primaryKeyFound = true;
//                // Check to see if we need to move on to PHASE 2...
//                if (!primaryKeyFound)
//                {
//                    // PHASE 2:
//                    // Didn't find the primary key column in any text row in the import data - so try again, but this time looking for the alternate unique key...
//                    if (destinationTable.PrimaryKey[0].ExtendedProperties.Contains("alternate_key_fields") &&
//                        destinationTable.PrimaryKey[0].ExtendedProperties["alternate_key_fields"].ToString().Length > 0)
//                    {
//                        uniqueKeyColumnNames = destinationTable.PrimaryKey[0].ExtendedProperties["alternate_key_fields"].ToString().Split(',');
//                        // Make sure the destination datatable has all of the columns specified in the alternate_key_fields ext. prop...
//                        foreach (string uniqueColumnName in uniqueKeyColumnNames)
//                        {
//                            if (destinationTable.Columns.Contains(uniqueColumnName.Trim().ToLower()))
//                            {
//                                uniqueKeys.Add(destinationTable.Columns[uniqueColumnName.Trim().ToLower()]);
//                            }
//                        }
//                        // The destination datatable does not have all of the columns specified in the compound unique key so bail out now...
//                        if (uniqueKeys.Count != uniqueKeyColumnNames.Length) return false;
//                        // Look through all of the rows of raw text for a single row that contains all of the unique key column names
//                        for (int i = 0; i < rawImportRows.Length && columnHeaderRowIndex == -1; i++)
//                        {
//                            columnHeaderRowIndex = i; // Start out assuming the row has all of the unique key column headers...
//                            foreach (DataColumn uKeyColumn in uniqueKeys)
//                            {
//                                if (!FindText(_sharedUtils.GetFriendlyFieldName(uKeyColumn, uKeyColumn.ColumnName), rawImportRows[i], false, rowDelimiters, columnDelimiters))
//                                {
//                                    // If the column header was not matched using case sensitive - try matching again (case insensitive)...
//                                    if (!FindText(_sharedUtils.GetFriendlyFieldName(uKeyColumn, uKeyColumn.ColumnName), rawImportRows[i], true, rowDelimiters, columnDelimiters))
//                                    {
//                                        // If the column header was still not matched - try the raw table field name...
//                                        if (!FindText(uKeyColumn.ColumnName, rawImportRows[i], true, rowDelimiters, columnDelimiters))
//                                        {
//                                            // The ASSUMPTION was wrong because the header text for one of the required primary key columns is missing in this raw text row...
//                                            columnHeaderRowIndex = -1;
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                // Check to see if a column header was found for the psuedo-primary key of the destinationTable...
//                if (columnHeaderRowIndex == -1)
//                {
//                    // Still cannot find an import row with column text that contains a collection of unique key columns - bail out now...
//                    return false;
//                }

//                // Since we made it here, it looks like we found a row in the import text that contains the column names for the destination tables primary/unique key...
//                string[] importColumnNames = rawImportRows[columnHeaderRowIndex].Split(columnDelimiters, StringSplitOptions.None);
//                System.Collections.Generic.Dictionary<string, int> columnNameMap = new System.Collections.Generic.Dictionary<string, int>();
//                // So now we need to build a map of datatable columns in import text columns (because they may not be in the same order)...
//                for (int i = 0; i < importColumnNames.Length; i++)
//                {
//                    // Map the friendly field name from the incoming text to the matching column in the datatable (case sensitive)...
//                    foreach (DataColumn dc in destinationTable.Columns)
//                    {
//                        if (_sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName) == importColumnNames[i])
//                        {
//                            columnNameMap.Add(dc.ColumnName, i);
//                        }
//                    }
//                    // If the column header was not matched - try matching again (case insensitive)...
//                    if (!columnNameMap.ContainsValue(i))
//                    {
//                        // Map the friendly field name from the incoming text to the matching column in the datatable (case insensitive)...
//                        foreach (DataColumn dc in destinationTable.Columns)
//                        {
//                            if (_sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName).ToLower() == importColumnNames[i].ToLower())
//                            {
//                                columnNameMap.Add(dc.ColumnName, i);
//                            }
//                        }
//                    }
//                    // If the column header was still not matched - try the raw table field name...
//                    if (!columnNameMap.ContainsValue(i))
//                    {
//                        // Map the friendly field name from the incoming text to the matching column in the datatable (case insensitive)...
//                        foreach (DataColumn dc in destinationTable.Columns)
//                        {
//                            if (dc.ColumnName.ToLower() == importColumnNames[i].ToLower())
//                            {
//                                columnNameMap.Add(dc.ColumnName, i);
//                            }
//                        }
//                    }
//                }

//                // Now that we have the column map, start processing the rows (starting with the one right after the column header row)...
//                for (int i = columnHeaderRowIndex + 1; i < rawImportRows.Length; i++)
//                {
//                    DataRow dr = null;
//                    string[] rawFieldData = rawImportRows[i].Split(columnDelimiters, StringSplitOptions.None);
//                    if (primaryKeyFound)
//                    {
//                        System.Collections.Generic.List<object> rowKeys = new System.Collections.Generic.List<object>();
//                        // Build the primary key to get the row to edit...
//                        foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
//                        {
//                            object keyValue;
//                            if (string.IsNullOrEmpty(rawFieldData[columnNameMap[pKeyColumn.ColumnName]].ToString()))
//                            {
//                                keyValue = DBNull.Value;
//                            }
//                            else
//                            {
//                                keyValue = rawFieldData[columnNameMap[pKeyColumn.ColumnName]];
//                            }
//                            rowKeys.Add(keyValue);
//                        }
//                        // Get the row to update (or create a new one for insert if an existing one is not found)...
//                        // First - attempt to find a row in the DataTable that matches the primary key(s)...
//                        dr = destinationTable.Rows.Find(rowKeys.ToArray());
//                        if (dr == null)
//                        {
//                            // No row exists in this DataTable for the given primary key(s), so create a new blank row to fill...
//                            dr = destinationTable.NewRow();
//                            // and add it to the DataTable...
//                            destinationTable.Rows.Add(dr);
//                        }
//                    }
//                    else // Find the row using the unique keys...
//                    {
//                        DataRow[] matchingRows = null;
//                        string rowFilter = "";
//                        foreach (DataColumn uKeyColumn in uniqueKeys)
//                        {
//                            if (!string.IsNullOrEmpty(rawFieldData[columnNameMap[uKeyColumn.ColumnName]]))
//                            {

//                                //rowFilter += uKeyColumn.ColumnName + "='" + rawFieldData[columnNameMap[uKeyColumn.ColumnName]] + "' AND ";

//                                string newValue = "";
//                                // Perform a reverse lookup to get the key if this is a ForeignKey field...
//                                if (_sharedUtils.LookupTablesIsValidFKField(uKeyColumn))
//                                {
//                                    if (!string.IsNullOrEmpty(rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString().Trim()))
//                                    {
////newValue = lookupTables.GetValueMember(uKeyColumn.ExtendedProperties["foreign_key_resultset_name"].ToString(),
//                                        //newValue = _sharedUtils.GetLookupValueMember(uKeyColumn.ExtendedProperties["foreign_key_resultset_name"].ToString(),
//                                        newValue = _sharedUtils.GetLookupValueMember(uKeyColumn.ExtendedProperties["foreign_key_dataview_name"].ToString(),
//                                                                                        rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString().Trim(),
//                                                                                        null,
//                                                                                        "!Error! - GetValueMember method failed to find display member");
//                                        // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
//                                        if (newValue.Equals("!Error! - GetValueMember method failed to find display member"))
//                                        {
//                                            dr.SetColumnError(uKeyColumn.ColumnName, "\tCould not find lookup value: " + rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString());
//                                        }
//                                    }
//                                }
//                                // Perform a reverse lookup to get the value if this is a Code_Value field...
////else if (uKeyColumn.ExtendedProperties.Contains("gui_hint") && uKeyColumn.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "SMALL_SINGLE_SELECT_CONTROL" &&
////    //uKeyColumn.ExtendedProperties.Contains("code_group_id") && uKeyColumn.ExtendedProperties["code_group_id"].ToString().Length > 0)
////    uKeyColumn.ExtendedProperties.Contains("group_name") && uKeyColumn.ExtendedProperties["group_name"].ToString().Length > 0)
//                                else if (_sharedUtils.LookupTablesIsValidCodeValueField(uKeyColumn))
//                                {
//                                    if (!string.IsNullOrEmpty(rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString().Trim()))
//                                    {
////newValue = lookupTables.GetValueMember("code_value_lookup",
//                                        newValue = _sharedUtils.GetLookupValueMember("code_value_lookup",
//                                                                                        rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString(),
//                                                                                        //"code_group_id='" + uKeyColumn.ExtendedProperties["code_group_id"].ToString() + "'",
//                                                                                        "group_name='" + uKeyColumn.ExtendedProperties["group_name"].ToString() + "'",
//                                                                                        "!Error! - GetValueMember method failed to find display member");
//                                        // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
//                                        if (newValue.Equals("!Error! - GetValueMember method failed to find display member"))
//                                        {
//                                            dr.SetColumnError(uKeyColumn.ColumnName, "\tCould not find lookup value: " + rawFieldData[columnNameMap[uKeyColumn.ColumnName]].ToString());
//                                        }
//                                    }
//                                }
//                                // Doesn't require a lookup...
//                                else
//                                {
//                                    newValue = rawFieldData[columnNameMap[uKeyColumn.ColumnName]];
//                                }

//                                if (uKeyColumn.DataType == typeof(string))
//                                {
//                                    rowFilter += uKeyColumn.ColumnName + "='" + newValue + "' AND ";
//                                }
//                                else
//                                {
//                                    rowFilter += uKeyColumn.ColumnName + "=" + newValue + " AND ";
//                                }
//                            }
//                            else
//                            {
//                                rowFilter += uKeyColumn.ColumnName + " IS NULL AND ";
//                            }
//                        }
//                        rowFilter = rowFilter.Substring(0, rowFilter.LastIndexOf(" AND "));
//                        try
//                        {
//                            matchingRows = destinationTable.Select(rowFilter);
//                        }
//                        catch
//                        {
//                            matchingRows = new DataRow[] {};
//                        }

//                        if (matchingRows.Length > 0)
//                        {
//                            dr = matchingRows[0];
//                        }
//                        else
//                        {
//// Could not find a matching row, so set the dr to null (this will effectively ignore this import record)
////dr = null;
//                            // No row exists in this DataTable for the given primary key(s), so create a new blank row to fill...
//                            dr = destinationTable.NewRow();
//                            // and add it to the DataTable...
//                            destinationTable.Rows.Add(dr);
//                        }
//                    }
//                    if (dr != null)
//                    {
//                        populateRowWithImportData(dr, rawFieldData, columnNameMap);
//                    }
//                    else
//                    {
//                        missingRows++;
//                    }
//                }
//            }
//            processedImportSuccessfully = true;

//            return processedImportSuccessfully;
//        }

//        private bool ImportTextToDataTableUsingBlockStyle(string rawImportText, DataGridView dgv, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
//        {
//            bool processedImportSuccessfully = true;
//            DataTable destinationTable = (DataTable)((BindingSource)dgv.DataSource).DataSource;
//            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
//            string[] tempColumns = null;
//            string newImportText = "";
//            string newImportRowText = "";
//            badRows = 0;
//            missingRows = 0;

//            // If the DGV does not have a currently active cell bail out now...
//            if (dgv.CurrentCell == null) return false;
//            // If the import string is empty bail out now...
//            if (string.IsNullOrEmpty(rawImportText) || rawImportRows.Length < 1) return false;

//            // Okay we need to build a new importText string that has column headers that include the friendly names for the primary key columns
//            // and the friendly names for the dgv columns starting at the currenly active cell in the dgv...  Why are we doing this?  Because
//            // we are going to pass this new importText string off to the 'ImportTextToDataTableUsingKeys' method, and since that method
//            // requires a primary key or alternate pkey we are going to get them from the dgv starting at the current row of the current cell...

//            // Step 1 - Determine the number of rows and columns in the incoming rawImportText (to use later for building the new ImportText string)...
//            int rawImportRowCount = 0;
//            int rawImportColCount = 0;
//            // Estimate the number of rows and columns in the import text (assumes a rectangular shape)
//            if (rawImportRows != null && rawImportRows.Length > 0)
//            {
//                rawImportRowCount = rawImportRows.Length;
//                tempColumns = rawImportRows[0].Split(columnDelimiters, StringSplitOptions.None);
//                if (tempColumns != null && tempColumns.Length > 0)
//                {
//                    rawImportColCount = tempColumns.Length;
//                }
//            }

//            int minSelectedCol = dgv.Columns.Count;
//            int maxSelectedCol = -1;
//            int minSelectedRow = dgv.Rows.Count;
//            int maxSelectedRow = -1;
//            // Check to see if the datagridview's selected cells contains the CurrentCell
//            // and if so use the selected cells as the destination cells...

//            // Find the bounding rectangle for the selected cells...
//            if (dgv.SelectedCells.Count == 1)
//            {
//                minSelectedCol = dgv.CurrentCell.ColumnIndex;
//                maxSelectedCol = dgv.CurrentCell.ColumnIndex + rawImportColCount - 1;
//                minSelectedRow = dgv.CurrentCell.RowIndex;
//                maxSelectedRow = dgv.CurrentCell.RowIndex + rawImportRowCount - 1;
//            }
//            else
//            {
//                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
//                {
//                    if (dgvc.ColumnIndex < minSelectedCol) minSelectedCol = dgvc.ColumnIndex;
//                    if (dgvc.ColumnIndex > maxSelectedCol) maxSelectedCol = dgvc.ColumnIndex;
//                    if (dgvc.RowIndex < minSelectedRow) minSelectedRow = dgvc.RowIndex;
//                    if (dgvc.RowIndex > maxSelectedRow) maxSelectedRow = dgvc.RowIndex;
//                }
//                if ((maxSelectedCol - minSelectedCol) < (rawImportColCount - 1)) maxSelectedCol = minSelectedCol + rawImportColCount - 1;
//                if ((maxSelectedRow - minSelectedRow) < (rawImportRowCount - 1)) maxSelectedRow = minSelectedRow + rawImportRowCount - 1;
//            }

//            string modifiedImportText = "";
//            // Now fill (or clip) the import data to fit the selected cells...
//            for (int iSelectedRow = 0; iSelectedRow <= (maxSelectedRow - minSelectedRow); iSelectedRow++)
//            {
//                // 
//                tempColumns = rawImportRows[iSelectedRow % rawImportRowCount].Split(columnDelimiters, StringSplitOptions.None);
//                for (int iSelectedCol = 0; iSelectedCol <= (maxSelectedCol - minSelectedCol); iSelectedCol++)
//                {
//                    //
//                    modifiedImportText += tempColumns[iSelectedCol % rawImportColCount] + "\t";
//                }
//                // Strip the last tab character and add a CR LF...
//                modifiedImportText = modifiedImportText.Substring(0, modifiedImportText.Length - 1) + "\r\n";
//            }

//            // Step 2 - Get the primary key column names for the new column header row text...
//            if (destinationTable.PrimaryKey.Length > 0)
//            {
//                foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
//                {
//                    newImportText += _sharedUtils.GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName) + "\t";
//                }
//            }
            
//            // Step 3 - Continue adding friendly column names to the import text (starting with the column name of the current cell's column HeaderText)...
////DataGridViewColumn currColumn = dgv.CurrentCell.OwningColumn;
//            DataGridViewColumn currColumn = dgv.Columns[minSelectedCol];
//            // Step 4 - Now repeat this process for each additional column in the rawImportText...
//            //foreach(string tempCol in tempColumns)
//            for (int i = 0; i < Math.Max(rawImportColCount, maxSelectedCol - minSelectedCol + 1); i++)
//            {
//                if (currColumn != null)
//                {
//                    newImportText += currColumn.HeaderText + "\t";
//                }
//                else
//                {
//                    newImportText += "\t";
//                }
//                // Try to find the next visible column...
//                currColumn = dgv.Columns.GetNextColumn(currColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.Frozen);
//            }
//            // Strip the last tab character and add a CR LF...
//            newImportText = newImportText.Substring(0, newImportText.Length - 1) + "\r\n";

//            // Step 5 - Get the primary key for each row receiving pasted text and prepend it to the orginal import raw text...
//            string[] modifiedImportRows = modifiedImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
//////DataGridViewRow currRow = dgv.CurrentCell.OwningRow;
//            DataGridViewRow currRow = dgv.Rows[minSelectedRow];
//            int nextRowIndex = currRow.Index;
//            for (int i = 0; i < modifiedImportRows.Length; i++)
//            {
//                newImportRowText = "";
//                if (currRow != null)
//                {
//                    if (destinationTable.PrimaryKey.Length > 0)
//                    {
//                        foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
//                        {
//                            newImportRowText += ((DataRowView)currRow.DataBoundItem).Row[pKeyColumn].ToString() + "\t";
//                        }
//                    }
//                    // Now add the original import row text to the new import row text...
////newImportRowText += rawImportRows[i] + "\r\n";
//                    newImportRowText += modifiedImportRows[i] + "\r\n";
//                    // And now add it to the new import text string...
//                    newImportText += newImportRowText;
//                }

//                // Finally, try to find the next visible row...
//                nextRowIndex = dgv.Rows.GetNextRow(currRow.Index, DataGridViewElementStates.Visible);
//                if (nextRowIndex != -1 &&
//                    !dgv.Rows[nextRowIndex].IsNewRow &&
//                    nextRowIndex >= minSelectedRow &&
//                    nextRowIndex <= maxSelectedRow)
//                {
//                    currRow = dgv.Rows[nextRowIndex];
//                }
//                else
//                {
//                    // Looks like we hit the end of the rows in the DGV - bailout now...
//                    //currRow = null;
//                    break;
//                }
//            }

//            // Step 6 - Now that we have built a new ImportText string that contains pkeys, we can pass it off to the 'ImportTextToDataTableUsingKeys' 
//            processedImportSuccessfully = ImportTextToDataTableUsingKeys(newImportText, destinationTable, rowDelimiters, columnDelimiters, out badRows, out missingRows);

//            return processedImportSuccessfully;
//        }

//        public void populateRowWithImportData(DataRow dr, string[] fieldValues, System.Collections.Generic.Dictionary<string, int> columnNameMap)
//        {
//            DataTable dt = dr.Table;
//            foreach (string tableColumnName in columnNameMap.Keys)
//            {
//                // Only update the write enabled columns in this row...
//                if (!dt.Columns[tableColumnName].ReadOnly)
//                {
//                    string newValue = "";
//                    DataColumn dc = dt.Columns[tableColumnName];
//                    int fieldIndex = columnNameMap[tableColumnName];
//                    if (fieldValues.Length <= fieldIndex) continue;
//                    // Perform a reverse lookup to get the key if this is a ForeignKey field...
//                    if (_sharedUtils.LookupTablesIsValidFKField(dc))
//                    {
//                        if (!string.IsNullOrEmpty(fieldValues[fieldIndex].ToString().Trim()))
//                        {
////newValue = lookupTables.GetValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(),
//                            //newValue = _sharedUtils.GetLookupValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(),
//                            newValue = _sharedUtils.GetLookupValueMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString(),
//                                                                            fieldValues[fieldIndex].ToString().Trim(),
//                                                                            null,
//                                                                            "!Error! - GetValueMember method failed to find display member");
//                            // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
//                            if (newValue.Equals("!Error! - GetValueMember method failed to find display member"))
//                            {
//                                dr.SetColumnError(tableColumnName, "\tCould not find lookup value: " + fieldValues[fieldIndex].ToString());
//                            }
//                        }
//                    }
//                    // Perform a reverse lookup to get the value if this is a Code_Value field...
////else if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "SMALL_SINGLE_SELECT_CONTROL" &&
////    //dc.ExtendedProperties.Contains("code_group_id") && dc.ExtendedProperties["code_group_id"].ToString().Length > 0)
////    dc.ExtendedProperties.Contains("group_name") && dc.ExtendedProperties["group_name"].ToString().Length > 0)
//                    else if(_sharedUtils.LookupTablesIsValidCodeValueField(dc))
//                    {
//                        if (!string.IsNullOrEmpty(fieldValues[fieldIndex].ToString().Trim()))
//                        {
////newValue = lookupTables.GetValueMember("code_value_lookup",
//                            newValue = _sharedUtils.GetLookupValueMember("code_value_lookup",
//                                                                            fieldValues[fieldIndex].ToString(),
//                                                                            //"code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "'",
//                                                                            "group_name='" + dc.ExtendedProperties["group_name"].ToString() + "'",
//                                                                            "!Error! - GetValueMember method failed to find display member");
//                            // If the lookup attempt returned the default value - indicate to the user that the lookup failed...
//                            if (newValue.Equals("!Error! - GetValueMember method failed to find display member"))
//                            {
//                                dr.SetColumnError(tableColumnName, "\tCould not find lookup value: " + fieldValues[fieldIndex].ToString());
//                            }
//                        }
//                    }
//                    // Doesn't require a lookup...
//                    else
//                    {
//                        newValue = fieldValues[fieldIndex];
//                    }

//                    // If the newValue is null attempt to retrieve the default value before further processing...
//                    if (string.IsNullOrEmpty(newValue) &&
//                        dt.Columns[tableColumnName].ExtendedProperties.Contains("default_value") &&
//                        dt.Columns[tableColumnName].ExtendedProperties["default_value"].ToString().Length > 0)
//                    {
//                        newValue = dt.Columns[tableColumnName].ExtendedProperties["default_value"].ToString();
//                    }
                    
//                    // Set the newValue to a default value if it is empty or null...
//                    if (string.IsNullOrEmpty(newValue) || newValue == "{DBNull.Value}")
//                    {
//                        if (dt.Columns[tableColumnName].ExtendedProperties.Contains("is_nullable") &&
//                            dt.Columns[tableColumnName].ExtendedProperties["is_nullable"].ToString() == "Y")
//                        {
//                            if (!dr[tableColumnName].Equals(DBNull.Value) && !dr[tableColumnName].Equals(newValue)) dr[tableColumnName] = DBNull.Value;
//                        }
//                        else
//                        {
//                            dr.SetColumnError(tableColumnName, "\tThis value cannot be empty (null)");
//                        }
//                    }
//                    // Convert the newValue string to the datatype for this column...
//                    else if ((dt.Columns[tableColumnName].DataType == typeof(int) ||
//                                dt.Columns[tableColumnName].DataType == typeof(Int16) ||
//                                dt.Columns[tableColumnName].DataType == typeof(Int32) ||
//                                dt.Columns[tableColumnName].DataType == typeof(Int64)) &&
//                                !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
//                    {
//                        int tempValue = 0;
//                        if (Int32.TryParse(newValue, out tempValue))
//                        {
//                            if(!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
//                        }
//                        else
//                        {
//                            dr.SetColumnError(tableColumnName, "\tValue '" + newValue + "' cannot be converted to an integer");
//                            //newValue = "-99999999";
//                        }
//                    }
//                    else if (dt.Columns[tableColumnName].DataType == typeof(Decimal) && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
//                    {
//                        Decimal tempValue = new Decimal();
//                        if (Decimal.TryParse(newValue, out tempValue))
//                        {
//                            if (!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
//                        }
//                        else
//                        {
//                            dr.SetColumnError(tableColumnName, "\tValue '" + newValue + "' cannot be converted to a decimal");
//                            //newValue = "-99999999";
//                        }
//                    }
//                    else if (dt.Columns[tableColumnName].DataType == typeof(DateTime) && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
//                    {
//                        DateTime tempValue = new DateTime();
//                        if (DateTime.TryParse(newValue, out tempValue))
//                        {
//                            if (!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
//                        }
//                        else
//                        {
//                            // Basic DateTime conversion failed - look to see if the user provided a hint about how to interpret this date value...
//                            // Look to see if there is a column provided that matches this current column name + "_code"...
//                            if (dt.Columns.Contains(tableColumnName + "_code"))
//                            {
//                                string dateFormat = "MM/dd/yyyy";
//                                dateFormat = _sharedUtils.GetLookupValueMember("code_value_lookup", fieldValues[columnNameMap[tableColumnName + "_code"]].ToString().Trim(), "group_name='" + dr.Table.Columns[tableColumnName + "_code"].ExtendedProperties["group_name"].ToString() + "'", dateFormat);
//                                if (DateTime.TryParseExact(newValue, dateFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out tempValue))
//                                {
//                                    if (!dr[tableColumnName].Equals(tempValue)) dr[tableColumnName] = tempValue;
//                                }
//                                else
//                                {
//                                    dr.SetColumnError(tableColumnName, "\tValue '" + newValue + "' cannot be converted to a Date/Time");
//                                    //newValue = "-99999999";
//                                }
//                            }
//                        }
//                    }
//                    else if (dt.Columns[tableColumnName].DataType == typeof(string) && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
//                    {
//                        if (dc.ExtendedProperties.Contains("max_length") && !dr.GetColumnsInError().Contains(dt.Columns[tableColumnName]))
//                        {
//                            int maxLength = 0;
//                            if (Int32.TryParse(dc.ExtendedProperties["max_length"].ToString(), out maxLength))
//                            {
//                                if (newValue.Length <= maxLength ||
//                                    maxLength == -1)
//                                {
//                                    if (!dr[tableColumnName].Equals(newValue)) dr[tableColumnName] = newValue;
//                                }
//                                else
//                                {
//                                    dr.SetColumnError(tableColumnName, "\tValue exceeds maximum length - truncated to " + maxLength.ToString() + " characters");
//                                    dr[tableColumnName] = newValue.Substring(0, maxLength); // Truncate the value (so the user can see what is legal to be pasted in)
//                                    //newValue = "-99999999";
//                                }
//                            }
//                        }
//                    }
//                    else
//                    {
//                        // Not sure what datatype got us here - bailout...
//                    }
//                }
//            }
//        }

        //public bool ImportTextToDataTable(string rawText, DataTable destinationTable, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        //{
        //    string[] rawTextRows = rawText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
        //    DataColumn[] datatablePrimaryKeys = destinationTable.PrimaryKey;
        //    bool processedSuccessfully = false;
        //    badRows = 0;
        //    missingRows = 0;

        //    // Make sure there is text to process and the destinationTable has the primary key(s) defined
        //    if (rawTextRows != null && rawTextRows.Length > 0 && destinationTable.PrimaryKey.Length > 0)
        //    {
        //        // Find the row of text data that contains the column headers
        //        int headerRowIndex = -1;
        //        for (int i = 0; i < rawTextRows.Length || headerRowIndex > 0; i++)
        //        {
        //            foreach (DataColumn dc in destinationTable.Columns)
        //            {
        //                if (FindText(_sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName), rawTextRows[i], false, rowDelimiters, columnDelimiters))
        //                {
        //                    headerRowIndex = i;
        //                }
        //            }
        //        }
        //        string[] friendlyColumnNames = rawTextRows[headerRowIndex].Split(columnDelimiters, StringSplitOptions.None);
        //        System.Collections.Generic.Dictionary<string, int> columnNameMap = new System.Collections.Generic.Dictionary<string, int>();
        //        System.Collections.Generic.List<string> pKeysList = new System.Collections.Generic.List<string>();
        //        for (int i = 0; i < friendlyColumnNames.Length; i++)
        //        {
        //            // Map the friendly field name from the incoming text to the matching column in the datatable...
        //            foreach (DataColumn dc in destinationTable.Columns)
        //            {
        //                if (_sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName) == friendlyColumnNames[i])
        //                {
        //                    columnNameMap.Add(dc.ColumnName, i);
        //                }
        //            }
        //            // Build the list of primary key columns in the row header for the incoming text...
        //            foreach (DataColumn pkey in destinationTable.PrimaryKey)
        //            {
        //                if (_sharedUtils.GetFriendlyFieldName(pkey, pkey.ColumnName) == friendlyColumnNames[i])
        //                {
        //                    pKeysList.Add(pkey.ColumnName);
        //                }
        //            }
        //        }

        //        if (pKeysList.Count == destinationTable.PrimaryKey.Length)
        //        {
        //            // Found all the primary key columns so treat the incoming text as fully qualified text containing DataRows...
        //            for (int i = headerRowIndex + 1; i < rawTextRows.Length; i++)
        //            {
        //                DataRow dr = null;
        //                string[] rawData = rawTextRows[i].Split(columnDelimiters, StringSplitOptions.None);
        //                System.Collections.Generic.List<object> rowKeys = new System.Collections.Generic.List<object>();
        //                // Build the primary key to get the row to edit...
        //                foreach (string key in pKeysList)
        //                {
        //                    object keyValue;
        //                    if (string.IsNullOrEmpty(rawData[columnNameMap[key]].ToString()))
        //                    {
        //                        keyValue = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        keyValue = rawData[columnNameMap[key]];
        //                    }
        //                    rowKeys.Add(keyValue);
        //                }
        //                // Get the row to update...
        //                try
        //                {
        //                    // Attempt to find a row in the DataTable that matches the primary key(s)...
        //                    dr = destinationTable.Rows.Find(rowKeys.ToArray());
        //                    if (dr == null)
        //                    {
        //                        // No row exists in this DataTable for the given primary key(s), so create a new blank row to fill...
        //                        dr = destinationTable.NewRow();
        //                        // and add it to the DataTable...
        //                        destinationTable.Rows.Add(dr);
        //                    }
        //                }
        //                catch (Exception errorException)
        //                {
        //                }
        //                // Make sure the text row was parsed properly...
        //                if (rawData.Length == friendlyColumnNames.Length)
        //                {
        //                    // Update the row (if one was found)...
        //                    if (dr != null)
        //                    {
        //                        foreach (string tableColumnName in columnNameMap.Keys)
        //                        {
        //                            // Only update the write enabled columns in this row...
        //                            if (!destinationTable.Columns[tableColumnName].ReadOnly)
        //                            {
        //                                string newValue = "";
        //                                DataColumn dc = destinationTable.Columns[tableColumnName];
        //                                // Perform a reverse lookup to get the key if this is a ForeignKey field...
        //                                if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "LARGE_SINGLE_SELECT_CONTROL" &&
        //                                    dc.ExtendedProperties.Contains("foreign_key_field_name") && dc.ExtendedProperties["foreign_key_field_name"].ToString().Length > 0 &&
        //                                    dc.ExtendedProperties.Contains("foreign_key_resultset_name") && dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Length > 0)
        //                                {
        //                                    newValue = lookupTables.GetValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(),
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString(),
        //                                                                            null,
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString());
        //                                }
        //                                // Perform a reverse lookup to get the value if this is a Code_Value field...
        //                                else if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "SMALL_SINGLE_SELECT_CONTROL" &&
        //                                    dc.ExtendedProperties.Contains("code_group_id") && dc.ExtendedProperties["code_group_id"].ToString().Length > 0)
        //                                {
        //                                    newValue = lookupTables.GetValueMember("code_value_lookup",
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString(),
        //                                                                            "code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "'",
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString());
        //                                }
        //                                // Doesn't require a lookup...
        //                                else
        //                                {
        //                                    newValue = rawData[columnNameMap[tableColumnName]];
        //                                }
        //                                // Set the value...
        //                                if (string.IsNullOrEmpty(newValue))
        //                                {
        //                                    if (destinationTable.Columns[tableColumnName].ExtendedProperties.Contains("is_nullable") &&
        //                                    destinationTable.Columns[tableColumnName].ExtendedProperties["is_nullable"].ToString() == "Y")
        //                                    {
        //                                        dr[tableColumnName] = DBNull.Value;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (destinationTable.Columns[tableColumnName].ExtendedProperties.Contains("default_value") &&
        //                                        destinationTable.Columns[tableColumnName].ExtendedProperties["default_value"].ToString().Length > 0)
        //                                        {
        //                                            newValue = destinationTable.Columns[tableColumnName].ExtendedProperties["default_value"].ToString();
        //                                        }
        //                                        else
        //                                        {
        //                                            dr.SetColumnError(tableColumnName, "\tThis value cannot be empty (null)");
        //                                        }
        //                                    }
        //                                }
        //                                else if (destinationTable.Columns[tableColumnName].DataType == typeof(int))
        //                                {
        //                                    int tempValue = 0;
        //                                    if (Int32.TryParse(newValue, out tempValue))
        //                                    {
        //                                        dr[tableColumnName] = tempValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        dr.SetColumnError(tableColumnName, "\tValue cannot be converted to an integer");
        //                                        //newValue = "-99999999";
        //                                    }
        //                                }
        //                                else if (destinationTable.Columns[tableColumnName].DataType == typeof(DateTime))
        //                                {
        //                                    DateTime tempValue = new DateTime();
        //                                    if (DateTime.TryParse(newValue, out tempValue))
        //                                    {
        //                                        dr[tableColumnName] = tempValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        dr.SetColumnError(tableColumnName, "\tValue cannot be converted to a Date/Time");
        //                                        //newValue = "-99999999";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (dc.ExtendedProperties.Contains("max_length"))
        //                                    {
        //                                        int maxLength = 0;
        //                                        if (Int32.TryParse(dc.ExtendedProperties["max_length"].ToString(), out maxLength))
        //                                        {
        //                                            if (newValue.Length <= maxLength)
        //                                            {
        //                                                dr[tableColumnName] = newValue;
        //                                            }
        //                                            else
        //                                            {
        //                                                dr.SetColumnError(tableColumnName, "\tValue exceeds maximum length - truncated to " + maxLength.ToString() + " characters");
        //                                                dr[tableColumnName] = newValue.Substring(0, maxLength); // Truncate the value (so the user can see what is legal to be pasted in)
        //                                                //newValue = "-99999999";
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Could not find a row in the datatable matching the values of the primary key(s)
        //                        missingRows++;
        //                    }
        //                }
        //                else
        //                {
        //                    // Number of objects in the header row do not match the number
        //                    // of objects in this DataRow - so skip it...
        //                    badRows++;
        //                }
        //            }
        //            processedSuccessfully = true;
        //        }
        //        else
        //        {
        //            processedSuccessfully = false;
        //        }
        //    }
        //    return processedSuccessfully;
        //}


        //public bool ImportTextWithPKey(string rawText, DataTable destinationTable, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        //{
        //    //char[] rowDelimiters = new char[] { '\r', '\n' };
        //    //char[] columnDelimiters = new char[] { '\t' };
        //    //string rawText = (string)e.Data.GetData(typeof(string));
        //    string[] rawTextLines = rawText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
        //    //DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
        //    DataColumn[] datatablePrimaryKeys = destinationTable.PrimaryKey;
        //    bool processedSuccessfully = false;
        //    badRows = 0;
        //    missingRows = 0;

        //    if (rawTextLines != null && rawTextLines.Length > 0 && datatablePrimaryKeys != null && datatablePrimaryKeys.Length > 0)
        //    {
        //        // Find the row of text data that contains the column headers
        //        int headerRowIndex = 0;
        //        for (int i = 0; i < rawTextLines.Length; i++)
        //        {
        //            if (FindText(_sharedUtils.GetFriendlyFieldName(destinationTable.PrimaryKey[0], destinationTable.PrimaryKey[0].ColumnName), rawTextLines[i], false, rowDelimiters, columnDelimiters))
        //            {
        //                headerRowIndex = i;
        //            }
        //        }
        //        string[] friendlyColumnNames = rawTextLines[headerRowIndex].Split(columnDelimiters, StringSplitOptions.None);
        //        System.Collections.Generic.Dictionary<string, int> columnNameMap = new System.Collections.Generic.Dictionary<string, int>();
        //        System.Collections.Generic.List<string> primaryKeyList = new System.Collections.Generic.List<string>();
        //        for (int i = 0; i < friendlyColumnNames.Length; i++)
        //        {
        //            // Map the friendly field name from the incoming text to the matching column in the datatable...
        //            foreach (DataColumn dc in destinationTable.Columns)
        //            {
        //                if (_sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName) == friendlyColumnNames[i])
        //                {
        //                    columnNameMap.Add(dc.ColumnName, i);
        //                }
        //            }
        //            // Make sure all of the primary key columns are in the row header for the incoming text...
        //            foreach (DataColumn pkey in datatablePrimaryKeys)
        //            {
        //                if (_sharedUtils.GetFriendlyFieldName(pkey, pkey.ColumnName) == friendlyColumnNames[i])
        //                {
        //                    primaryKeyList.Add(pkey.ColumnName);
        //                }
        //            }
        //        }

        //        if (primaryKeyList.Count == datatablePrimaryKeys.Length)
        //        {
        //            // Found all the primary key columns so treat the incoming text as DataRows...
        //            for (int i = headerRowIndex + 1; i < rawTextLines.Length; i++)
        //            {
        //                DataRow dr = destinationTable.NewRow();
        //                string[] rawData = rawTextLines[i].Split(columnDelimiters, StringSplitOptions.None);
        //                System.Collections.Generic.List<object> rowKeys = new System.Collections.Generic.List<object>();
        //                // Build the primary key to get the row to edit...
        //                foreach (string key in primaryKeyList)
        //                {
        //                    object keyValue;
        //                    if (string.IsNullOrEmpty(rawData[columnNameMap[key]].ToString()))
        //                    {
        //                        keyValue = DBNull.Value;
        //                    }
        //                    else
        //                    {
        //                        keyValue = rawData[columnNameMap[key]];
        //                    }
        //                    rowKeys.Add(keyValue);
        //                }
        //                // Get the row to update...
        //                try
        //                {
        //                    dr = destinationTable.Rows.Find(rowKeys.ToArray());
        //                }
        //                catch (Exception errorException)
        //                {
        //                }
        //                // Make sure the text row was parsed properly...
        //                if (rawData.Length == friendlyColumnNames.Length)
        //                {
        //                    // Update the row (if one was found)...
        //                    if (dr != null)
        //                    {
        //                        foreach (string tableColumnName in columnNameMap.Keys)
        //                        {
        //                            // Only update the write enabled columns in this row...
        //                            if (!destinationTable.Columns[tableColumnName].ReadOnly)
        //                            {
        //                                string newValue = "";
        //                                DataColumn dc = destinationTable.Columns[tableColumnName];
        //                                // Perform a reverse lookup to get the key if this is a ForeignKey field...
        //                                if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "LARGE_SINGLE_SELECT_CONTROL" &&
        //                                    dc.ExtendedProperties.Contains("foreign_key_field_name") && dc.ExtendedProperties["foreign_key_field_name"].ToString().Length > 0 &&
        //                                    dc.ExtendedProperties.Contains("foreign_key_resultset_name") && dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Length > 0)
        //                                {
        //                                    newValue = lookupTables.GetValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(),
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString(),
        //                                                                            null,
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString());
        //                                }
        //                                // Perform a reverse lookup to get the value if this is a Code_Value field...
        //                                else if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "SMALL_SINGLE_SELECT_CONTROL" &&
        //                                    dc.ExtendedProperties.Contains("code_group_id") && dc.ExtendedProperties["code_group_id"].ToString().Length > 0)
        //                                {
        //                                    newValue = lookupTables.GetValueMember("code_value_lookup",
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString(),
        //                                                                            "code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "'",
        //                                                                            rawData[columnNameMap[tableColumnName]].ToString());
        //                                }
        //                                // Doesn't require a lookup...
        //                                else
        //                                {
        //                                    newValue = rawData[columnNameMap[tableColumnName]];
        //                                }
        //                                // Set the value...
        //                                if (string.IsNullOrEmpty(newValue))
        //                                {
        //                                    if (destinationTable.Columns[tableColumnName].ExtendedProperties.Contains("is_nullable") &&
        //                                    destinationTable.Columns[tableColumnName].ExtendedProperties["is_nullable"].ToString() == "Y")
        //                                    {
        //                                        dr[tableColumnName] = DBNull.Value;
        //                                    }
        //                                    else
        //                                    {
        //                                        dr.SetColumnError(tableColumnName, "\tThis value cannot be empty (null)");
        //                                    }
        //                                }
        //                                else if (destinationTable.Columns[tableColumnName].DataType == typeof(int))
        //                                {
        //                                    int tempValue = 0;
        //                                    if (Int32.TryParse(newValue, out tempValue))
        //                                    {
        //                                        dr[tableColumnName] = tempValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        dr.SetColumnError(tableColumnName, "\tValue cannot be converted to an integer");
        //                                        //newValue = "-99999999";
        //                                    }
        //                                }
        //                                else if (destinationTable.Columns[tableColumnName].DataType == typeof(DateTime))
        //                                {
        //                                    DateTime tempValue = new DateTime();
        //                                    if (DateTime.TryParse(newValue, out tempValue))
        //                                    {
        //                                        dr[tableColumnName] = tempValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        dr.SetColumnError(tableColumnName, "\tValue cannot be converted to a Date/Time");
        //                                        //newValue = "-99999999";
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    if (dc.ExtendedProperties.Contains("max_length"))
        //                                    {
        //                                        int maxLength = 0;
        //                                        if (Int32.TryParse(dc.ExtendedProperties["max_length"].ToString(), out maxLength))
        //                                        {
        //                                            if (newValue.Length <= maxLength)
        //                                            {
        //                                                dr[tableColumnName] = newValue;
        //                                            }
        //                                            else
        //                                            {
        //                                                dr.SetColumnError(tableColumnName, "\tValue exceeds maximum length - truncated to " + maxLength.ToString() + " characters");
        //                                                dr[tableColumnName] = newValue.Substring(0, maxLength); // Truncate the value (so the user can see what is legal to be pasted in)
        //                                                //newValue = "-99999999";
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Could not find a row in the datatable matching the values of the primary key(s)
        //                        missingRows++;
        //                    }
        //                }
        //                else
        //                {
        //                    // Number of objects in the header row do not match the number
        //                    // of objects in this DataRow - so skip it...
        //                    badRows++;
        //                }
        //            }
        //            processedSuccessfully = true;
        //        }
        //        else
        //        {
        //            processedSuccessfully = false;
        //        }
        //    }
        //    return processedSuccessfully;
        //}

        public DataObject BuildDragAndDropDGVData(DataGridView dgv)
        {
            DataObject doReturn = new DataObject();

            // Write the rows out in text format first...
            //string strData = "";
            //// Write out the column headers...
            //foreach (DataGridViewColumn dgvCol in dgv.Columns)
            //{
            //    if (dgvCol.Visible) strData += dgvCol.HeaderText + "\t";
            //}
            //// Write out the values in the visible cells of the selected rows...
            //foreach (DataGridViewRow dgvr in dgv.SelectedRows)
            //{
            //    strData += "\n";
            //    foreach (DataGridViewCell dgvCell in dgvr.Cells)
            //    {
            //        if (dgvCell.Visible) strData += dgvCell.FormattedValue.ToString() + "\t";
            //    }
            //}
            System.Text.StringBuilder sb = new System.Text.StringBuilder("");

            string columnHeader = "";
            string columnNames = "";
            // First, gather the column headers...
            foreach (DataGridViewColumn dgvCol in dgv.Columns)
            {
                if (dgvCol.Visible)
                {
                    columnHeader += dgvCol.HeaderText + "\t";
                    columnNames += dgvCol.Name + "\t";
                }
            }
            // And write it to the string builder...
//if ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            if ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                sb.Append(columnNames);
            }
            else
            {
                sb.Append(columnHeader);
            }
            // Build the array of visible column names...
            string[] columnList = columnNames.Trim().Split('\t');
            // And use that list for writing the values to the string builder...
            // NOTE: because of the way the DGV adds rows to the selectedRows collection
            //       we have to process the rows in the opposite direction they were selected in...
            int rowStart = 0;
            int rowStop = dgv.SelectedRows.Count;
            int stepValue = 1;
            if (dgv.SelectedRows.Count > 1 && dgv.SelectedRows[0].Index > dgv.SelectedRows[1].Index)
            {
                rowStart = dgv.SelectedRows.Count - 1;
                rowStop = -1;
                stepValue = -1;
            }

            DataGridViewRow dgvrow = null;
            // Process the rows in the opposite direction they were selected by the user...
            for (int i = rowStart; i != rowStop; i += stepValue)
            {
                dgvrow = dgv.SelectedRows[i];
                //if (!dgv.SelectedRows[i].IsNewRow)
                if (!dgvrow.IsNewRow)
                {
                    sb.Append("\n");
                    foreach (string columnname in columnList)
                    {
                        switch (dgvrow.Cells[columnname].FormattedValueType.Name)
                        {
                            case "Boolean":
                                sb.Append(dgvrow.Cells[columnname].Value.ToString());
                                break;
                            default:
                                if (dgvrow.Cells[columnname].FormattedValue == null || dgvrow.Cells[columnname].FormattedValue.ToString().ToLower() == "[null]")
                                {
                                    sb.Append("");
                                }
                                else
                                {
                                    sb.Append(dgvrow.Cells[columnname].FormattedValue.ToString());
                                }
                                break;
                        }
                        sb.Append("\t");
                    }
    
                    //////foreach (string columnName in columnList)
                    //////{
                    //////    switch (dgv.SelectedRows[i].Cells[columnName].FormattedValueType.Name)
                    //////    {
                    //////        case "Boolean":
                    //////            sb.Append(dgv.SelectedRows[i].Cells[columnName].Value.ToString());
                    //////            break;
                    //////        default:
                    //////            if (dgv.SelectedRows[i].Cells[columnName].FormattedValue.ToString() == "[Null]")
                    //////            {
                    //////                sb.Append("");
                    //////            }
                    //////            else
                    //////            {
                    //////                sb.Append(dgv.SelectedRows[i].Cells[columnName].FormattedValue.ToString());
                    //////            }
                    //////            break;
                    //////    }
                    //////    sb.Append("\t");
                    //////}
                }
            }
            //foreach (DataGridViewRow dgvr in dgv.SelectedRows)
            //{
            //    sb.Append("\n");
            //    foreach (string columnName in columnList)
            //    {
            //        sb.Append(dgvr.Cells[columnName].FormattedValue.ToString());
            //        sb.Append("\t");
            //    }
            //}

            // Now write it out as a collection of data table rows (for tree view drag and drop).
            DataSet dsData = new DataSet();
            dsData.Tables.Add(((DataTable)((BindingSource)dgv.DataSource).DataSource).Clone());
            foreach (DataGridViewRow dgvr in dgv.SelectedRows)
            {
                if (!dgvr.IsNewRow) dsData.Tables[0].Rows.Add(((DataRowView)dgvr.DataBoundItem).Row.ItemArray);
            }

            // Set the data types into the data object and return...
            //doReturn.SetData(typeof(string), strData);
            doReturn.SetData(typeof(string), sb.ToString());
            doReturn.SetData(typeof(DataSet), dsData);

            return doReturn;
        }

        //public string GetFriendlyFieldName(DataColumn dc, string defaultName)
        //{
        //    string friendlyFieldName = defaultName;
        //    // Try to find the friendly_field_name first...
        //    if (dc.ExtendedProperties.Contains("friendly_field_name") && 
        //        dc.ExtendedProperties["friendly_field_name"].ToString().Length > 0)
        //    {
        //        friendlyFieldName = dc.ExtendedProperties["friendly_field_name"].ToString();
        //    }
        //    // Otherwise the caption property should have the friendly name
        //    else if (dc.Caption.Length > 0)
        //    {
        //        friendlyFieldName = dc.Caption;
        //    }
        //    // Fallback to the ColumnName if all else fails...
        //    else
        //    {
        //        friendlyFieldName = dc.ColumnName;
        //    }
        //    // If everything else has failed use the default name passed in...
        //    if (friendlyFieldName.Length == 0) friendlyFieldName = defaultName;

        //    return friendlyFieldName;
        //}

        #endregion

        #region DGV cell context menu logic...

        private void ux_datagridviewMain_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            // Set the global variables so that later processing will know where to apply the command from the context menu...
            mouseClickDGVColumnIndex = e.ColumnIndex;
            mouseClickDGVRowIndex = e.RowIndex;

            // Change the color of the cell background so that the user
            // knows what cell the context menu applies to...
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                ux_datagridviewMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                ux_datagridviewMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Red;
            }

            // Clear the reports tool strip menu items and get a fresh list from the Reports directory...
            ux_dgvcellmenuReports.DropDownItems.Clear();

            // There are lots of ways to get the Reports directory location (the cleanest is to find the CT directory and go from there)...
            // So... Here are some of the better ways to find the CT's EXE path:
            //      System.Environment.GetFolderPath(System.Reflection.Assembly.GetExecutingAssembly().Location)
            //      System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)
            //      System.Windows.Forms.Application.StartupPath
            //      System.IO.Directory.GetCurrentDirectory()  <-- this one changes if the user saves an export of a report from CR Viewer

////System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory()); //System.Environment.GetFolderPath(Environment.SpecialFolder. System.Reflection.Assembly.GetExecutingAssembly()..Location);
System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\Reports");
System.Collections.Generic.Dictionary<string, string> reportsMap = _sharedUtils.GetReportsMapping();
string dataviewName = ((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName.Trim().ToUpper();
foreach (System.IO.FileInfo fi in di.GetFiles("*.rpt", System.IO.SearchOption.AllDirectories))
{
    if (reportsMap.ContainsKey(fi.Name.Trim().ToUpper()) &&
        reportsMap[fi.Name.Trim().ToUpper()].Contains(" " + dataviewName + " "))
    {
        ToolStripItem tsi = ux_dgvcellmenuReports.DropDownItems.Add(fi.Name, null, ux_DGVCellReport_Click);
        tsi.Tag = fi.FullName;
    }
}
if (ux_dgvcellmenuReports.DropDownItems.Count > 0)
{
    ux_dgvcellmenuReports.Enabled = true;
}
else
{
    ux_dgvcellmenuReports.Enabled = false;
}
            // Enable/Disable the Change Ownership menu selection based on if the user owns all of the rows...
            bool userOwnsAllRows = false;
            string cooperator = _usernameCooperatorID;
            // If the DGV is in read-only mode use the display value - not the cooperator_id for comparison...
            if (ux_buttonEditData.Enabled)
            {
                cooperator = _sharedUtils.GetLookupDisplayMember("cooperator_lookup", _usernameCooperatorID, "", _usernameCooperatorID);
            }
            // Now iterate through the selected rows to make sure the current user owns all of them...
            if (ux_datagridviewMain.Columns.Contains("owned_by"))
            {
                // Assume the user owns all the rows until proven otherwise...
                userOwnsAllRows = true;
                foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
                {
                    if (((DataRowView)dgvr.DataBoundItem).Row["owned_by"].ToString() != cooperator) userOwnsAllRows = false;
                }
            }
            ux_dgvcellmenuChangeOwner.Enabled = userOwnsAllRows;
        }

//        private void showOnlyRowsWithThisDataToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            string DataGridViewFilter = "";
//            string ColName = "Error!";
//            string CellValue = "Error!";

//            if (grinData.Tables.Contains(ux_tabcontrolDataview.SelectedTab.Tag.ToString()))
//            {
//                DataGridViewFilter = grinData.Tables[ux_tabcontrolDataview.SelectedTab.Tag.ToString()].DefaultView.RowFilter;
//            }

//            if (mouseClickDGVColumnIndex >= 0 && mouseClickDGVRowIndex >= 0)
//            {
//                ColName = ux_datagridviewMain.Columns[mouseClickDGVColumnIndex].Name;
//                CellValue = "'" + ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex].Value.ToString() + "'";
//                if (DataGridViewFilter.Length > 0)
//                {
//                    DataGridViewFilter += " AND " + ColName + " = " + CellValue;
//                }
//                else
//                {
//                    DataGridViewFilter = ColName + " = " + CellValue;
//                }
//            }
//            // Set the row filter for the table currently viewed in the DGV (this is a copy of the grinData original)...
//            ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter = DataGridViewFilter;
////// Set the row filter for the original table (this one only gets reset when logic forces a GetData from the webservice)...
////grinData.Tables[ux_tabcontrolDataview.SelectedTab.Tag.ToString()].DefaultView.RowFilter = DataGridViewFilter;
//            SetAllUserSettings();
//            // Refresh the formatting for the datagridview to show the sort glyphs...
//            RefreshMainDGVFormatting();
//            //uxlblRowsFound.Text = "Showing " + uxdgvSearchResults.Rows.Count.ToString() + " rows (of " + GridViewData.Tables["SearchResults"].Rows.Count.ToString() + " retrieved)";
//        }

//        private void hideRowsWithThisDataToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            string DataGridViewFilter = "";
//            string ColName = "Error!";
//            string CellValue = "Error!";
//            string TableName = ux_tabcontrolDataView.SelectedTab.Tag.ToString();

//            if (grinData.Tables.Contains(TableName))
//            {
////DataGridViewFilter = grinData.Tables[ux_tabcontrolDataView.SelectedTab.Tag.ToString()].DefaultView.RowFilter;
//                DataGridViewFilter = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter;
//                if (mouseClickDGVColumnIndex >= 0 && mouseClickDGVRowIndex >= 0)
//                {
//                    // Get the column name for the bound datatable (NOTE: convert the dbnulls to '' using the isnull(,) function so that they are filtered properly)...
//                    ColName = "isnull(" + ux_datagridviewMain.Columns[mouseClickDGVColumnIndex].Name + ", '')";
////// Get the cell value in the bound datatable for the DGV cell that was clicked...
////CellValue = ((DataRowView)ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex].OwningRow.DataBoundItem).Row[ColName].ToString();
//                    CellValue = "'" + ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex].Value.ToString() + "'";
//                    if (DataGridViewFilter.Length > 0)
//                    {
//                        DataGridViewFilter += " AND " + ColName + " <> " + CellValue;
//                    }
//                    else
//                    {
//                        DataGridViewFilter = ColName + " <> " + CellValue;
//                    }
//                }
//                // Set the row filter for the table currently viewed in the DGV (this is a copy of the grinData original)...
//                ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter = DataGridViewFilter;
////// Set the row filter for the original table (this one only gets reset when logic forces a GetData from the webservice)...
////grinData.Tables[TableName].DefaultView.RowFilter = DataGridViewFilter;
//                //SetDGVMainResultsetUserSettings();
//                SetAllUserSettings();
//                // Refresh the formatting for the datagridview to show the sort glyphs...
//                RefreshMainDGVFormatting();
//                //uxlblRowsFound.Text = "Showing " + uxdgvSearchResults.Rows.Count.ToString() + " rows (of " + GridViewData.Tables["SearchResults"].Rows.Count.ToString() + " retrieved)";
//            }
//        }

        private void ux_dgvcellmenuSecurityWizard_Click(object sender, EventArgs e)
        {
            string pkeyColumnName = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).PrimaryKey[0].ColumnName;
            string tableName = "";
            if (((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).PrimaryKey[0].ExtendedProperties.Contains("table_name") &&
                !string.IsNullOrEmpty(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).PrimaryKey[0].ExtendedProperties["table_name"].ToString()))
            {
                tableName = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).PrimaryKey[0].ExtendedProperties["table_name"].ToString();
            }
            else
            {
                tableName = pkeyColumnName.Remove(pkeyColumnName.LastIndexOf("_id"));
            }
            string pkeyCollection = "";
            foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
            {
                pkeyCollection += dgvr.Cells[pkeyColumnName].Value + ",";
            }
//MessageBox.Show("Please note that the Security Wizard is temporarily disabled for schema changes.");
SecurityWizard newSecurityWizardDialog = new SecurityWizard(tableName, pkeyColumnName, pkeyCollection.TrimEnd(','), _sharedUtils);
if (newSecurityWizardDialog.ShowDialog(this) == DialogResult.OK)
{
}
else
{
}
RefreshMainDGVFormatting();
        }

        private void ux_dgvcellmenuChangeOwner_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Clone());
            foreach(DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
            {
                ds.Tables[0].LoadDataRow(((DataRowView)dgvr.DataBoundItem).Row.ItemArray, LoadOption.Upsert);
            }
            GRINGlobal.Client.Common.ChangeOwnership newChangeOwnershipDialog = new GRINGlobal.Client.Common.ChangeOwnership(ds, _sharedUtils);
            if (newChangeOwnershipDialog.ShowDialog(this) == DialogResult.OK)
            {
                ux_buttonRefreshData.PerformClick();
            }
            else
            {
            }
            //RefreshMainDGVFormatting();
        }

        private void ux_contextmenustripDGVCell_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            RefreshMainDGVFormatting();
        }

        private void ux_dgvcellmenuShowOnlyRowsWithThisDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DataGridViewFilter = "";
            string ColName = "Error!";
            string CellValue = "Error!";

            if ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource != null)
            {
                DataGridViewFilter = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter;

                if (mouseClickDGVColumnIndex >= 0 && mouseClickDGVRowIndex >= 0)
                {
                    // Get the column name for the bound datatable (NOTE: convert the dbnulls to '' using the isnull(,) function so that they are filtered properly)...
                    ColName = "convert(isnull(" + ux_datagridviewMain.Columns[mouseClickDGVColumnIndex].Name + ", ''), 'System.String')";
                    CellValue = "'" + ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex].Value.ToString().Replace("'", "''") + "'";
                    if (DataGridViewFilter.Length > 0)
                    {
                        DataGridViewFilter += " AND " + ColName + " = " + CellValue;
                    }
                    else
                    {
                        DataGridViewFilter = ColName + " = " + CellValue;
                    }
                }
                // Set the row filter for the table currently viewed in the DGV (this is a copy of the grinData original)...
                ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter = DataGridViewFilter;
                // Save the users settings (which may or may not include this filter)...
                SetAllUserSettings();
                // Refresh the formatting for the datagridview to show the sort glyphs...
                RefreshMainDGVFormatting();
                //uxlblRowsFound.Text = "Showing " + uxdgvSearchResults.Rows.Count.ToString() + " rows (of " + GridViewData.Tables["SearchResults"].Rows.Count.ToString() + " retrieved)";
            }
        }

        private void ux_dgvcellmenuHideRowsWithThisDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DataGridViewFilter = "";
            string ColName = "Error!";
            string CellValue = "Error!";

            if ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource != null)
            {
                DataGridViewFilter = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter;
                if (mouseClickDGVColumnIndex >= 0 && mouseClickDGVRowIndex >= 0)
                {
                    // Get the column name for the bound datatable (NOTE: convert the dbnulls to '' using the isnull(,) function so that they are filtered properly)...
                    ColName = "convert(isnull(" + ux_datagridviewMain.Columns[mouseClickDGVColumnIndex].Name + ", ''), 'System.String')";
                    CellValue = "'" + ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex].Value.ToString().Replace("'", "''") + "'";
                    if (DataGridViewFilter.Length > 0)
                    {
                        DataGridViewFilter += " AND " + ColName + " <> " + CellValue;
                    }
                    else
                    {
                        DataGridViewFilter = ColName + " <> " + CellValue;
                    }
                }
                // Set the row filter for the table currently viewed in the DGV (this is a copy of the grinData original)...
                ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter = DataGridViewFilter;
                // Save the users settings (which may or may not include this filter)...
                SetAllUserSettings();
                // Refresh the formatting for the datagridview to show the sort glyphs...
                RefreshMainDGVFormatting();
                //uxlblRowsFound.Text = "Showing " + uxdgvSearchResults.Rows.Count.ToString() + " rows (of " + GridViewData.Tables["SearchResults"].Rows.Count.ToString() + " retrieved)";
            }
        }

        //private void resetRowFilterToolStripMenuItem_Click(object sender, EventArgs e)
        private void ux_dgvcellmenuResetRowFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.RowFilter = "";
            //SetDGVMainDataviewUserSettings();
            SetAllUserSettings();
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshMainDGVFormatting();
            //uxlblRowsFound.Text = "Showing " + uxdgvSearchResults.Rows.Count.ToString() + " rows (of " + GridViewData.Tables["SearchResults"].Rows.Count.ToString() + " retrieved)";
        }

        private void sortAscendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fieldName = "";
//string sortOrder = userSettings[ux_tabcontrolDataview.SelectedTab.Tag.ToString(), "DefaultView.Sort"];
//string sortOrder = userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "DefaultView.Sort"];
            string sortOrder = _sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "DefaultView.Sort", "");

            fieldName = ux_datagridviewMain.Columns[mouseClickDGVColumnIndex].Name;
            if (sortOrder.Contains(fieldName + " DESC"))
            {
                sortOrder = sortOrder.Replace(fieldName + " DESC", fieldName + " ASC");
            }
            else if (!sortOrder.Contains(fieldName + " ASC"))
            {
                if (sortOrder.Length > 0)
                {
                    sortOrder += "," + fieldName + " ASC";
                }
                else
                {
                    sortOrder += fieldName + " ASC";
                }
            }
            sortOrder = sortOrder.Replace(",,", ",");
            // Save the new sort order to userSettings...
            //SetDGVMainDataviewUserSettings();
            //SetAllUserSettings();
            ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.Sort = sortOrder;
            //SetDGVMainDataviewUserSettings();
            SetAllUserSettings();
            RefreshMainDGVFormatting();
        }

        private void sortDescendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fieldName = "";
//string sortOrder = userSettings[ux_tabcontrolDataview.SelectedTab.Tag.ToString(), "DefaultView.Sort"];
//string sortOrder = userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "DefaultView.Sort"];
            string sortOrder = _sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "DefaultView.Sort", "");

            fieldName = ux_datagridviewMain.Columns[mouseClickDGVColumnIndex].Name;
            if (sortOrder.Contains(fieldName + " ASC"))
            {
                sortOrder = sortOrder.Replace(fieldName + " ASC", fieldName + " DESC");
            }
            else if (!sortOrder.Contains(fieldName + " DESC"))
            {
                if (sortOrder.Length > 0)
                {
                    sortOrder += "," + fieldName + " DESC";
                }
                else
                {
                    sortOrder += fieldName + " DESC";
                }
            }
            sortOrder = sortOrder.Replace(",,", ",");
            // Save the new sort order to userSettings...
            //SetDGVMainDataviewUserSettings();
            //SetAllUserSettings();
            ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.Sort = sortOrder;
            //SetDGVMainDataviewUserSettings();
            SetAllUserSettings();
            RefreshMainDGVFormatting();
        }

        private void noSortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fieldName = "";
//string sortOrder = userSettings[ux_tabcontrolDataview.SelectedTab.Tag.ToString(), "DefaultView.Sort"];
//string sortOrder = userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "DefaultView.Sort"];
            string sortOrder = _sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "DefaultView.Sort", "");

            fieldName = ux_datagridviewMain.Columns[mouseClickDGVColumnIndex].Name;
            if (sortOrder.Contains(fieldName + " DESC"))
            {
                sortOrder = sortOrder.Replace(fieldName + " DESC", "");
            }
            else if (sortOrder.Contains(fieldName + " ASC"))
            {
                sortOrder = sortOrder.Replace(fieldName + " ASC", "");
            }
            // Remove all commas at the start and end of the string...
            sortOrder = sortOrder.TrimStart(',').TrimEnd(',');
            // Remove any double commas from the string...
            sortOrder = sortOrder.Replace(",,", ",");

            // Save the new sort order to userSettings...
            ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.Sort = sortOrder;
            //SetDGVMainDataviewUserSettings();
            SetAllUserSettings();
            RefreshMainDGVFormatting();
        }

        private void resetAllSortingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sortOrder = "";
            // Save the new sort order to userSettings...
//userSettings[ux_tabcontrolDataview.SelectedTab.Tag.ToString(), "DefaultView.Sort"] = sortOrder;
//userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "DefaultView.Sort"] = sortOrder;
            _sharedUtils.SaveUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "DefaultView.Sort", sortOrder);
            ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.Sort = sortOrder;
            //SetDGVMainDataviewUserSettings();
            SetAllUserSettings();
            RefreshMainDGVFormatting();
        }

        private void ux_dgvcellmenuEditCopy_Click(object sender, EventArgs e)
        {
            if (!ux_datagridviewMain.SelectedCells.Contains(ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex]))
            {
                ux_datagridviewMain.CurrentCell = ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex];
            }
            KeyEventArgs sendKeys = new KeyEventArgs(Keys.C | Keys.Control);
            ux_datagridviewMain_KeyDown(ux_datagridviewMain, sendKeys);
        }

        private void ux_dgvcellmenuEditPaste_Click(object sender, EventArgs e)
        {
            if (!ux_datagridviewMain.SelectedCells.Contains(ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex]))
            {
                ux_datagridviewMain.CurrentCell = ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex];
            }
            KeyEventArgs sendKeys = new KeyEventArgs(Keys.V | Keys.Control);
            ux_datagridviewMain_KeyDown(ux_datagridviewMain, sendKeys);
        }

        private void ux_dgvcellmenuEditDelete_Click(object sender, EventArgs e)
        {
            if (!ux_datagridviewMain.SelectedCells.Contains(ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex]))
            {
                ux_datagridviewMain.CurrentCell = ux_datagridviewMain[mouseClickDGVColumnIndex, mouseClickDGVRowIndex];
            }
            KeyEventArgs sendKeys = new KeyEventArgs(Keys.Delete);
            ux_datagridviewMain_KeyDown(ux_datagridviewMain, sendKeys);
        }

        #endregion

        #region Building the Read-Only version of the DataGridView based on the DataTable...

//        private void buildReadOnlyDataGridView(DataGridView dataGridView, DataTable dataTable)
//        {
//            DataTable newDataTable = new DataTable(dataTable.TableName);
//            System.Collections.Generic.List<DataColumn> pKeys = new System.Collections.Generic.List<DataColumn>();
////            dataGridView.DataSource = null;
//            if (dataGridView != null && dataGridView.DataSource != null)
//            {
////                ((BindingSource)dataGridView.DataSource).DataSource = null;
//            }
//            dataGridView.Columns.Clear();
//            dataGridView.AutoGenerateColumns = true;


//            // First build the columns of the new table...
//            foreach (DataColumn dataColumn in dataTable.Columns)
//            {
//                string newColumnName = dataColumn.ColumnName;
//                newDataTable.Columns.Add(dataColumn.ColumnName);
//                // Set the column header...
//                newDataTable.Columns[newColumnName].Caption = _sharedUtils.GetFriendlyFieldName(dataColumn, dataColumn.ColumnName);

//                // Add the extended properties from the source table...
//                foreach (string key in dataColumn.ExtendedProperties.Keys)
//                {
//                    newDataTable.Columns[newColumnName].ExtendedProperties.Add(key, dataColumn.ExtendedProperties[key]);
//                }
//                // Add this column to the primary keys list if extended properties indicate it is a primary key
//                if (newDataTable.Columns[newColumnName].ExtendedProperties.Contains("is_primary_key") &&
//                    newDataTable.Columns[newColumnName].ExtendedProperties["is_primary_key"].ToString() == "Y")
//                {
//                    pKeys.Add(newDataTable.Columns[newColumnName]);
//                }

//                if (dataColumn.ExtendedProperties.Contains("gui_hint"))
//                {
//                    switch (dataColumn.ExtendedProperties["gui_hint"].ToString())
//                    {
//                        case "LARGE_SINGLE_SELECT_CONTROL":
//                            string fkRSName = "";
//                            if (dataColumn.ExtendedProperties.Contains("foreign_key_resultset_name"))
//                            {
//                                fkRSName = dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString();
//                            }
//                            if (fkRSName.Length > 0)
//                            {
////MessageBox.Show("Starting load of " + fkRSName + " Lookup Table");
////newDataTable.Columns[newColumnName].ExtendedProperties["foreign_key_resultset_name"] = fkRSName;
//                                if (!lookupTables.Contains(fkRSName))
//                                {
////                                    new System.Threading.Thread(lookupTables.LoadTableFromDatabase).Start(fkRSName);
////lookupTables.LoadTableFromDatabase(fkRSName, ":createddate=; :modifieddate=");
////                                    DataSet fkResultsetLookup = new DataSet();
////                                    fkResultsetLookup = GUIWebServices.GetData(false, username, password, fkRSName, ":createddate=; :modifieddate=", 0, 5000);
////                                    lookupTables.Add(fkResultsetLookup.Tables[fkRSName].Copy());
//                                }
////MessageBox.Show("Finished loading " + fkRSName + " Lookup Table");
//                            }
//                            break;
//                        case "SMALL_SINGLE_SELECT_CONTROL":
//                            string codeValueGroup = "";
//                            if (dataColumn.ExtendedProperties.Contains("code_group_id"))
//                            {
//                                codeValueGroup = dataColumn.ExtendedProperties["code_group_id"].ToString();
//                            }
//                            if (codeValueGroup.Length > 0)
//                            {
////newDataTable.Columns[newColumnName].ExtendedProperties["code_group_id"] = codeValueGroup;
//                                if (!lookupTables.Contains("code_value_lookup"))
//                                {
////                                    new System.Threading.Thread(lookupTables.LoadTableFromDatabase).Start("code_value_lookup");
////lookupTables.LoadTableFromDatabase("code_value_lookup", ":createddate=; :modifieddate=");
////                                    DataSet codeValueLookup = new DataSet();
////                                    codeValueLookup = GUIWebServices.GetData(false, username, password, "code_value_lookup", ":createddate=; :modifieddate=", 0, 0);
////                                    lookupTables.Add(codeValueLookup.Tables["code_value_lookup"].Copy());
//                                }
//                            }
//                            break;
//                        default:
//                            // This column is not a FK lookup, so set the datatype for the column (to facilitate sorting)...
//                            newDataTable.Columns[newColumnName].DataType = dataColumn.DataType;
//                            break;
//                    }
//                }
//            }

//            // Set the datatable's primary key...
//            newDataTable.PrimaryKey = pKeys.ToArray();

//            // Now populate the rows of the new table...
//            foreach (DataRow dr in dataTable.Rows)
//            {
//                DataRow newDataRow = newDataTable.NewRow();
//                foreach (DataColumn dc in newDataTable.Columns)
//                {
//                    if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "LARGE_SINGLE_SELECT_CONTROL" &&
//                        dc.ExtendedProperties.Contains("foreign_key_field_name") && dc.ExtendedProperties["foreign_key_field_name"].ToString().Length > 0 &&
//                        dc.ExtendedProperties.Contains("foreign_key_resultset_name") && dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Length > 0)
//                    {
//                        string lookupTable = dc.ExtendedProperties["foreign_key_resultset_name"].ToString();
//                        //DataRow[] lookupRows = null;
//                        //if(dr[dc.ColumnName] != DBNull.Value) lookupRows = lookupTables.Tables[lookupTable].Select("value_member='" + dr[dc.ColumnName].ToString() + "'");
//                        //if (lookupRows != null && lookupRows.Length > 0)
//                        //{
//                        //    newDataRow[dc.ColumnName] = lookupRows[0]["display_member"].ToString();
//                        //}
//                        //else
//                        //{
//                        //    newDataRow[dc.ColumnName] = dr[dc.ColumnName].ToString();
//                        //}
//                        if (dr[dc.ColumnName] != DBNull.Value)
//                        {
//                            newDataRow[dc.ColumnName] = lookupTables.GetDisplayMember(lookupTable, dr[dc.ColumnName].ToString(), "", dr[dc.ColumnName].ToString());
//                        }
//                    }
//                    else if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "SMALL_SINGLE_SELECT_CONTROL" &&
//                        dc.ExtendedProperties.Contains("code_group_id") && dc.ExtendedProperties["code_group_id"].ToString().Length > 0)
//                    {
//                        //DataRow[] lookupRows = lookupTables.Tables["code_value_lookup"].Select("code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "' AND value='" + dr[dc.ColumnName].ToString() + "'");
//                        //if (lookupRows != null && lookupRows.Length > 0)
//                        //{
//                        //    newDataRow[dc.ColumnName] = lookupRows[0]["name"].ToString();
//                        //}
//                        //else
//                        //{
//                        //    newDataRow[dc.ColumnName] = dr[dc.ColumnName].ToString();
//                        //}
//                        if (dr[dc.ColumnName] != DBNull.Value)
//                        {
//                            newDataRow[dc.ColumnName] = lookupTables.GetDisplayMember("code_value_lookup", dr[dc.ColumnName].ToString(), "code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "'", dr[dc.ColumnName].ToString());
//                        }
//                    }
//                    else
//                    {
//                        if (!string.IsNullOrEmpty(dr[dc.ColumnName].ToString()))
//                        {
//                            newDataRow[dc.ColumnName] = dr[dc.ColumnName].ToString();
//                        }
//                    }
//                }
//                // Add the row to the new data table...
//                newDataTable.Rows.Add(newDataRow.ItemArray);
//            }

//            // Set the sort and filter properties in the new datatable...
//            newDataTable.DefaultView.Sort = dataTable.DefaultView.Sort;
//            newDataTable.DefaultView.RowFilter = dataTable.DefaultView.RowFilter;

//            newDataTable.DefaultView.AllowDelete = false;
//            newDataTable.DefaultView.AllowEdit = false;
//            newDataTable.DefaultView.AllowNew = false;

//            // Bind the DataGridView to the datasource passed into this procedure...
//            newDataTable.AcceptChanges();
////dataGridView.DataSource = newDataTable;
//            // NOTE: The datagridview's datasource is actually a bindingsource (to wire up the binding navigator)
//            if (dataGridView != null && dataGridView.DataSource != null)
//            {
//                // Bind the new table to the default binding source (the bindingNavigator and mainDGV are both bound to this bindingSource)...
//                ((BindingSource)dataGridView.DataSource).DataSource = newDataTable;
//            }
//        }

        #endregion

        #region Building the Editable version of the DataGridView based on the DataTable...

//        private void buildEditDataGridView(DataGridView dataGridView, DataTable dataTable)
//        {
////            dataGridView.DataSource = null;
//            if (dataGridView != null && dataGridView.DataSource != null)
//            {
////                ((BindingSource)dataGridView.DataSource).DataSource = null;
//            }
//            dataGridView.Columns.Clear();
//            dataGridView.AutoGenerateColumns = false;

//            DataGridViewColumn newDGVColumn = new DataGridViewColumn();
//            foreach (DataColumn dataColumn in dataTable.Columns)
//            {
//                if (dataColumn.ExtendedProperties.Contains("gui_hint"))
//                {
//                    switch (dataColumn.ExtendedProperties["gui_hint"].ToString())
//                    {
//                        case "LARGE_SINGLE_SELECT_CONTROL":
//                            //if (dataColumn.ReadOnly)
//                            //{
//                            //    newDGVColumn = buildDGVUnboundTextBoxColumn(dataColumn);
//                            //}
//                            //else
//                            //{
//                            //    newDGVColumn = buildDGVHybridComboBoxColumn(dataColumn);
//                            //}
//                            //// If buildHybridComboboxColumn returns null then there are too many
//                            //// rows in the lookup table to make a combobox pratical so 
//                            //// make it an unbound textbox column instead...
//                            //if(newDGVColumn == null) newDGVColumn = buildDGVUnboundTextBoxColumn(dataColumn);
//                            newDGVColumn = buildDGVUnboundTextBoxColumn(dataColumn);
//                            break;
//                        case "SMALL_SINGLE_SELECT_CONTROL":
//                            newDGVColumn = buildDGVComboBoxColumn(dataColumn);
//                            break;
//                        case "TOGGLE_CONTROL":
//                            newDGVColumn = buildDGVCheckBoxColumn(dataColumn);
//                            break;
//                        case "DATE_CONTROL":
//                        case "INTEGER_CONTROL":
//                        case "TEXT_CONTROL":
//                        default:
//                            newDGVColumn = buildDGVTextBoxColumn(dataColumn);
//                            break;
//                    }
//                }
//                else
//                {
//                    newDGVColumn = buildDGVTextBoxColumn(dataColumn);
//                }

//                // Set the datatype...


//                // Get the text for the column header...
//                newDGVColumn.HeaderText = _sharedUtils.GetFriendlyFieldName(dataColumn, dataColumn.ColumnName);

//                // Set the properties of the new column...
//                newDGVColumn.SortMode = DataGridViewColumnSortMode.Programmatic;
//                if (dataColumn.ExtendedProperties.Contains("is_autoincrement") && 
//                    dataColumn.ExtendedProperties["is_autoincrement"].ToString() == "Y")
//                {
//                    dataColumn.AutoIncrement = true;
//                    dataColumn.AutoIncrementSeed = -1;
//                    dataColumn.AutoIncrementStep = -1;
//                }
//                if (dataColumn.ExtendedProperties.Contains("is_nullable") &&
//                    dataColumn.ExtendedProperties["is_nullable"].ToString() == "N")
//                {
//                    //dataColumn.AllowDBNull = false;
//                    if (dataColumn.ColumnName.StartsWith("is_")) dataColumn.DefaultValue = "N";
//                    if (dataColumn.ColumnName == "created_by") dataColumn.DefaultValue = cno;
//                    if (dataColumn.ColumnName == "created_date") dataColumn.DefaultValue = DateTime.Now;
//                    if (dataColumn.ColumnName == "owned_by") dataColumn.DefaultValue = cno;
//                    if (dataColumn.ColumnName == "owned_date") dataColumn.DefaultValue = DateTime.Now;
//                }

//                // Add the new column to the DataGridView...
//                dataGridView.Columns.Add(newDGVColumn);
//            }
            
////// Create a new datatable based on the current datasource's default view...
////DataTable newDataTable = dataTable.DefaultView.ToTable();
////newDataTable.AcceptChanges();

//            // Bind the DataGridView to the filtered and sorted copy of the datasource passed into this procedure...
////dataGridView.DataSource = newDataTable;

////dataGridView.DataSource = dataTable;
//            // NOTE: The datagridview's datasource is actually a bindingsource (to wire up the binding navigator)
//            if (dataGridView != null && dataGridView.DataSource != null)
//            {
//                ((BindingSource)dataGridView.DataSource).DataSource = dataTable;
//            }
//        }

//        void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
//        {
//            DataGridView dgv = (DataGridView)sender;
//            string errorMessage = e.Exception.Message;
//            int columnWithError = -1;
////            bool alertUser = true;
//            //object o = dgv[4, 3].ParseFormattedValue(dgv[4,3].FormattedValue, dgv[4, 3].Style, null, null);
//            // Find the cell the error belongs to (don't use e.ColumnIndex because it points to the current cell *NOT* the offending cell)...
//            foreach (DataGridViewColumn col in dgv.Columns)
//            {
//                if (errorMessage.Contains(col.Name))
//                {
//                    dgv[col.Name, e.RowIndex].ErrorText = errorMessage;
//                    columnWithError = col.Index;
//                }
//            }

//            //if (e.Exception.GetType().Equals(typeof(NoNullAllowedException)))
//            //{
//            //    DataRowView drv = (DataRowView)dgv.CurrentRow.DataBoundItem;
//            //    DataRow dr = null;
//            //    if (drv != null) dr = drv.Row;
//            //    if (dr != null && dr.IsNull(columnWithError))
//            //    {
//            //        // The offending cell has a value but the underlying datatable's row/column value is DBNull
//            //        // (this is most likely because the cell was not bound to the table before it was populated - which
//            //        //  is probably because the new row was added with a CTRL+D command)
//            //        // Try to get the key for this cell and populate the datatable directly...
//            //        DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
//            //        DataColumn dc = dr.Table.Columns[columnWithError];
//            //        if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "LARGE_SINGLE_SELECT_CONTROL" &&
//            //            dc.ExtendedProperties.Contains("foreign_key_field_name") && dc.ExtendedProperties["foreign_key_field_name"].ToString().Length > 0 &&
//            //            dc.ExtendedProperties.Contains("foreign_key_resultset_name") && dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Length > 0)
//            //        {
//            //            object newValue = lookupTables.GetValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(),
//            //                                                    dgv[columnWithError, e.RowIndex].Value.ToString(),
//            //                                                    "",
//            //                                                    "!ERROR - value in cell did not resolve to a valid code!");
//            //            if (newValue.ToString() == "!ERROR - value in cell did not resolve to a valid code!")
//            //            {
//            //                newValue = lookupTables.GetDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(),
//            //                                                        dgv[columnWithError, e.RowIndex].Value.ToString(),
//            //                                                        "",
//            //                                                        dc.ExtendedProperties["foreign_key_resultset_name"].ToString());
//            //                dr[columnWithError] = dgv[columnWithError, e.RowIndex].Value.ToString();
//            //                dgv[columnWithError, e.RowIndex].Value = newValue;
//            //            }
//            //            else
//            //            {
//            //                dr[e.ColumnIndex] = newValue;
//            //            }
//            //            //alertUser = false;
//            //        }
//            //    }
//            //}
//            //if (alertUser && DialogResult.OK == MessageBox.Show("There appear to be error(s) in the data you entered - would you like to continue editing this cell?\n\nError Text: " + errorMessage, "Data entry error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1))
//            //{
//            //    e.Cancel = true;
//            //}
//        }

//        private DataGridViewCheckBoxColumn buildDGVCheckBoxColumn(DataColumn dataColumn)
//        {
//            DataGridViewCheckBoxColumn newCheckBoxColumn = new DataGridViewCheckBoxColumn(false);
//            newCheckBoxColumn.DataPropertyName = dataColumn.ColumnName;
//            newCheckBoxColumn.Name = dataColumn.ColumnName;
//            newCheckBoxColumn.ReadOnly = dataColumn.ReadOnly;
//            newCheckBoxColumn.ValueType = dataColumn.DataType;
//            newCheckBoxColumn.TrueValue = "Y";
//            newCheckBoxColumn.FalseValue = "N";
//            return newCheckBoxColumn;
//        }

//        private DataGridViewComboBoxColumn buildDGVComboBoxColumn(DataColumn dataColumn)
//        {
//            DataTable lookupTable = null;
//            DataGridViewComboBoxColumn newComboBoxColumn = new DataGridViewComboBoxColumn();
//            newComboBoxColumn.DataPropertyName = dataColumn.ColumnName;
//            newComboBoxColumn.Name = dataColumn.ColumnName;
//            newComboBoxColumn.ReadOnly = dataColumn.ReadOnly;
//            newComboBoxColumn.ValueType = dataColumn.DataType;
//            newComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
////if (lookupTables.Contains("MRU_code_value_lookup"))
////{
////    lookupTable = lookupTables["MRU_code_value_lookup"];
////}
//            if (_sharedUtils.LookupTablesContains("MRU_code_value_lookup"))
//            {
//                lookupTable = _sharedUtils.LookupTablesGetMRUTable("MRU_code_value_lookup");
//            }
//            else
//            {
//                lookupTable = null;
//            }

//            if (dataColumn.ExtendedProperties.Contains("code_group_id") && lookupTable != null)
//            {
//                DataView dv = new DataView(lookupTable, "code_group_id='" + dataColumn.ExtendedProperties["code_group_id"].ToString() + "'", "display_member ASC", DataViewRowState.CurrentRows);
//                DataTable dt = dv.ToTable();
//                if (dataColumn.ExtendedProperties.Contains("is_nullable") && dataColumn.ExtendedProperties["is_nullable"].ToString() == "Y")
//                {
//                    DataRow dr = dt.NewRow();
//                    foreach (DataColumn dc in lookupTable.Columns)
//                    {
//                        // If there are any non-nullable fields - set them now...
//                        if (!dc.AllowDBNull)
//                        {
//                            dr[dc.ColumnName] = -1;
//                        }
//                    }
//                    dr["display_member"] = "[Null]";
//                    dr["value_member"] = DBNull.Value;
//                    dt.Rows.InsertAt(dr, 0);
//                    dt.AcceptChanges();
//                }
//                newComboBoxColumn.DisplayMember = "display_member";
//                newComboBoxColumn.ValueMember = "value_member";
//                newComboBoxColumn.DataSource = dt;
//                newComboBoxColumn.DefaultCellStyle.DataSourceNullValue = DBNull.Value;
//                newComboBoxColumn.DefaultCellStyle.NullValue = "[Null]";
//            }
//            return newComboBoxColumn;
//        }

//        private DataGridViewTextBoxColumn buildDGVTextBoxColumn(DataColumn dataColumn)
//        {
//            int maxTextLength = 0;
//            DataGridViewTextBoxColumn newTextBoxColumn = new DataGridViewTextBoxColumn();
//            newTextBoxColumn.DataPropertyName = dataColumn.ColumnName;
//            newTextBoxColumn.Name = dataColumn.ColumnName;
//            newTextBoxColumn.ReadOnly = dataColumn.ReadOnly;
//            newTextBoxColumn.ValueType = dataColumn.DataType;
//            if (dataColumn.ExtendedProperties.Contains("max_length"))
//            {
//                Int32.TryParse(dataColumn.ExtendedProperties["max_length"].ToString(), out maxTextLength);
//                if (maxTextLength > 0)
//                {
//                    newTextBoxColumn.MaxInputLength = maxTextLength;
//                }
//            }
//            return newTextBoxColumn;
//        }

//        private DataGridViewTextBoxColumn buildDGVUnboundTextBoxColumn(DataColumn dataColumn)
//        {
//            int maxTextLength = 0;
//            DataGridViewTextBoxColumn newUnboundTextBoxColumn = new DataGridViewTextBoxColumn();
//            //newTextBoxColumn.DataPropertyName = dataColumn.ColumnName;
//            newUnboundTextBoxColumn.Name = dataColumn.ColumnName;
//            newUnboundTextBoxColumn.ReadOnly = dataColumn.ReadOnly;
//            newUnboundTextBoxColumn.ValueType = dataColumn.DataType;
//            if (dataColumn.ExtendedProperties.Contains("max_length"))
//            {
//                Int32.TryParse(dataColumn.ExtendedProperties["max_length"].ToString(), out maxTextLength);
//                newUnboundTextBoxColumn.MaxInputLength = maxTextLength;
//            }
//            return newUnboundTextBoxColumn;
//        }

//        private DataGridViewComboBoxColumn buildDGVHybridComboBoxColumn(DataColumn dataColumn)
//        {
//            DataGridViewComboBoxColumn newHybridComboBoxColumn = new DataGridViewComboBoxColumn();
//            newHybridComboBoxColumn.DataPropertyName = dataColumn.ColumnName;
//            newHybridComboBoxColumn.Name = dataColumn.ColumnName;
//            newHybridComboBoxColumn.ReadOnly = dataColumn.ReadOnly;
//            newHybridComboBoxColumn.ValueType = dataColumn.DataType;
//            newHybridComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

//            // Build the temp table containing the best-guess entries most likely to be needed for this column...
//            if (dataColumn.ExtendedProperties.Contains("foreign_key_field_name") && dataColumn.ExtendedProperties["foreign_key_field_name"].ToString().Length > 0 &&
//                dataColumn.ExtendedProperties.Contains("foreign_key_resultset_name") && dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString().Length > 0)
//            {
////if (lookupTables.Contains(dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim()))
//                if (_sharedUtils.LookupTablesContains(dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim()))
//                {
////DataTable luTable = lookupTables[dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim()];
//                    DataTable luTable = _sharedUtils.LookupTablesGetMRUTable(dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim());
//                    DataTable bindTable = luTable.Clone();
//                    string rowFilter = "";
//                    string sort = "";
//                    // Create a list of 'filters' to help narrow down the choices in the dialogbox...
//                    foreach (DataColumn luColumn in luTable.Columns)
//                    {
//                        if (luColumn.ColumnName != "value_member" &&
//                            luColumn.ColumnName != "display_member" &&
//                            luColumn.ColumnName != "created_date" &&
//                            luColumn.ColumnName != "modified_date")
//                        {
//                            if (luColumn.ColumnName.StartsWith("is_"))
//                            {
//                                rowFilter += luColumn.ColumnName.Trim() + "='Y' AND ";
//                            }
//                            else
//                            {
//                                if (dataColumn.Table.Columns.Contains(luColumn.ColumnName))
//                                {
//                                    sort += luColumn.ColumnName + " ASC, ";
//                                    foreach (DataRow dr in dataColumn.Table.Rows)
//                                    {
//                                        DataRow[] luRows = luTable.Select(luColumn.ColumnName.Trim() + "='" + dr[luColumn.ColumnName] + "'");
//                                        foreach (DataRow luRow in luRows)
//                                        {
//                                            bindTable.LoadDataRow(luRow.ItemArray, LoadOption.Upsert);
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                    if (rowFilter.Contains(" AND "))
//                    {
//                        rowFilter = rowFilter.Trim().Remove(rowFilter.LastIndexOf(" AND "));
//                    }
//                    sort += "display_member ASC";
////                    DataView dv = new DataView(lookupTables[dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim()], rowFilter, sortOrder, DataViewRowState.CurrentRows);
//                    bindTable.DefaultView.RowFilter = rowFilter;
//                    bindTable.DefaultView.Sort = sort;
//                    newHybridComboBoxColumn.DisplayMember = "display_member";
//                    newHybridComboBoxColumn.ValueMember = "value_member";
//                    if (bindTable.Rows.Count > 5)
//                    {
//                        // Too many rows in the created lookup table 
//                        // so don't use a combobox control for this column...
//                        newHybridComboBoxColumn = null;
//                    }
//                    else if (bindTable.Rows.Count > 0)
//                    {
//                        // Successfully built a lookup table with less than 500 rows
//                        // so bind the combobox to it...
//                        newHybridComboBoxColumn.DataSource = bindTable;
//                    }
//                    else if (luTable.Rows.Count > 5000)
//                    {
//                        // Did not find any columns in the raw lookup table to help restrict
//                        // the choices in a combobox and the raw table has more than 500 rows
//                        // so don't use a combobox...
//                        newHybridComboBoxColumn = null;
//                    }
//                    else
//                    {
//                        // The raw lookup table is less than 500 rows, so use it to 
//                        // bind the combobox to...
//                        newHybridComboBoxColumn.DataSource = luTable;
//                    }
//                }
//            }
//            return newHybridComboBoxColumn;
//        }
///*

//            DataGridView dgv = (DataGridView)sender;
//            System.Collections.Generic.Dictionary<string, string> lookupFilters = new System.Collections.Generic.Dictionary<string, string>(); 
//            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
//            string columnName = dgv.CurrentCell.OwningColumn.Name;
//            DataColumn dc = dt.Columns[columnName];
//            if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "LARGE_SINGLE_SELECT_CONTROL" &&
//                dc.ExtendedProperties.Contains("foreign_key_field_name") && dc.ExtendedProperties["foreign_key_field_name"].ToString().Length > 0 &&
//                dc.ExtendedProperties.Contains("foreign_key_resultset_name") && dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Length > 0)
//            {
//                if (lookupTables.Contains(dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim()))
//                {
//                    DataTable luTable = lookupTables[dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim()];
//                    // Create a list of 'filters' to help narrow down the choices in the dialogbox...
//                    foreach (DataColumn luColumn in luTable.Columns)
//                    {
//                        if (luColumn.ColumnName != "value_member" && 
//                            luColumn.ColumnName != "display_member" &&
//                            luColumn.ColumnName != "created_date" &&
//                            luColumn.ColumnName != "modified_date")
//                        {
//                            if (dgv.Columns.Contains(luColumn.ColumnName)) lookupFilters.Add(luColumn.ColumnName, dgv.CurrentCell.OwningRow.Cells[luColumn.ColumnName].Value.ToString().Trim());
//                            if (luColumn.ColumnName.StartsWith("is_")) lookupFilters.Add(luColumn.ColumnName, "Y");
//                        }
//                    }
//                    string dataSourceCellValue = dt.Rows[dgv.CurrentCell.RowIndex][dgv.CurrentCell.ColumnIndex].ToString();
////                    LookupTablePicker ltp = new LookupTablePicker(dgv.CurrentCell.Value.ToString().Trim(), lookupTables[dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim()], lookupFilters);
//                    LookupTablePicker ltp = new LookupTablePicker(dataSourceCellValue, luTable, lookupFilters);
//                    if (DialogResult.OK == ltp.ShowDialog())
//                    {
////                        dgv.CurrentCell.Value = ltp.NewKey;
//                        dgv.CurrentCell.Value = ltp.NewValue.Trim();
//                        if(dt.Rows[dgv.CurrentCell.RowIndex][dgv.CurrentCell.ColumnIndex].ToString().Trim() != ltp.NewKey.Trim())
//                            dt.Rows[dgv.CurrentCell.RowIndex][dgv.CurrentCell.ColumnIndex] = ltp.NewKey.Trim();
//                    }
//                    dgv.EndEdit();
//                }
//            }

//*/
//        //private DataGridViewComboBoxColumn buildDGVAutoCompleteTextBoxColumn(DataColumn dataColumn)
//        //{
//        //    DataGridViewComboBoxColumn newComboBoxColumn = new DataGridViewComboBoxColumn();
//        //    newComboBoxColumn.DataPropertyName = dataColumn.ColumnName;
//        //    newComboBoxColumn.Name = dataColumn.ColumnName;
//        //    newComboBoxColumn.ValueType = dataColumn.DataType;
//        //    if (dataColumn.ExtendedProperties.Contains("foreign_key_resultset_name"))
//        //    {
//        //        string resultsetName = dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString();
//        //        if (resultsetName.Length > 0)
//        //        {
//        //            if (!grinLookups.Tables.Contains(resultsetName))
//        //            {
//        //                DataSet fkResultsetLookup = new DataSet();
//        //                fkResultsetLookup = GUIWebServices.GetData(false, username, password, resultsetName, "", 0);
//        //                grinLookups.Tables.Add(fkResultsetLookup.Tables[resultsetName].Copy());
//        //            }
//        //            DataView dv = new DataView(grinLookups.Tables[resultsetName], "", "display_member ASC", DataViewRowState.CurrentRows);
//        //            newComboBoxColumn.AutoComplete = true;
//        //            newComboBoxColumn.DisplayMember = "display_member";
//        //            newComboBoxColumn.ValueMember = "value_member";
//        //            newComboBoxColumn.DataSource = dv;
//        //        }
//        //    }
//        //    newComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
//        //    return newComboBoxColumn;
//        //}

//        //private DataGridViewButtonColumn newbuildDGVAutoCompleteTextBoxColumn(DataColumn dataColumn)
//        //{
//        //    DataGridViewButtonColumn newButtonColumn = new DataGridViewButtonColumn();
//        //    newButtonColumn.DataPropertyName = dataColumn.ColumnName;
//        //    newButtonColumn.Name = dataColumn.ColumnName;
//        //    newButtonColumn.ValueType = dataColumn.DataType;
//        //    if (dataColumn.ExtendedProperties.Contains("foreign_key_resultset_name"))
//        //    {
//        //        string resultsetName = dataColumn.ExtendedProperties["foreign_key_resultset_name"].ToString();
//        //        if (!grinLookups.Tables.Contains(resultsetName) && resultsetName.Length > 0)
//        //        {
//        //            DataSet fkResultsetLookup = new DataSet();
//        //            fkResultsetLookup = GUIWebServices.GetData(false, username, password, resultsetName, "", 0);
//        //            grinLookups.Tables.Add(fkResultsetLookup.Tables[resultsetName].Copy());
//        //            DataView dv = new DataView(grinLookups.Tables[resultsetName], "", "display_member ASC", DataViewRowState.CurrentRows);
//        //        }
//        //    }
//        //    return newButtonColumn;
//        //}
        
        #endregion

    }
}