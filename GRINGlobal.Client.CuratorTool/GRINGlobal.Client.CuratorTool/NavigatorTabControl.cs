using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections;
using GRINGlobal.Client.Common;

namespace GRINGlobal.Client.CuratorTool
{
    public partial class GRINGlobalClientCuratorTool
    {
        DateTime _dtMouseHoverStartTime = new DateTime();
        TreeNode _tnMouseHoveringOverNode = new TreeNode();
        Point _ptTreeNodeInsertLineStart = new Point(0, 0);
        Point _ptTreeNodeInsertLineStop = new Point(0, 0);
        TabControl _ux_NavigatorTabControl = null;
        ContextMenuStrip _ux_tab_cms_Navigator = null;
        ContextMenuStrip _ux_treeview_cms_Navigator = null;
        int _maxPathLength = 300;  // This is the column size of LIST_NAME in GG v1.0
        List<int> _deletedTreeNodes = new List<int>();
        ImageList _treeviewImageList;
        ImageList _tabControlImageList;

        private void ux_comboboxCNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            
            if (ux_comboboxCNO.SelectedIndex != -1)
            {
                string tabOrder = "";
                DataTable userListItems;

                if (_currentCooperatorID.Trim() == _usernameCooperatorID.Trim())
                {
                    // Save user settings...
                    SetAllUserSettings();
                    _sharedUtils.SaveAllUserSettings();
                    // Save the user's list (if the user owns that list)...
                    SaveNavigatorTabControlData(_ux_NavigatorTabControl, int.Parse(_usernameCooperatorID));
                }
                
                // Get the treeview lists for the currently selected cooperator...
                userListItems = GetUserItemList((int)ux_comboboxCNO.SelectedValue);

                //// If the user is switching back to their own list - go get the tabOrder property...
                //if (cno.ToLower().Trim() == ux_comboboxCNO.SelectedValue.ToString().ToLower().Trim())
                //{
                //    //tabOrder = userSettings[ux_tabcontrolGroupListNavigator.Name, "TabPages.Order"];
                //    tabOrder = _sharedUtils.GetUserSetting(ux_tabcontrolGroupListNavigator.Name, "TabPages.Order", "");

                //    // This is code to handle legacy user settings - pull this out after RC2...
                //    if (!tabOrder.Contains(_pathSeparator))
                //    {
                //        tabOrder = "";
                //    }

                //}
                //else
                //{
                //    tabOrder = "";
                //}

                // Rebuild the Navigator Tab Control with the new user item list...
                _ux_NavigatorTabControl = BuildTabControl(userListItems);
                //_ux_NavigatorTabControl.Select();
                //_ux_NavigatorTabControl.Focus();
                //_ux_NavigatorTabControl.SelectTab("");
                _ux_NavigatorTabControl.SelectTab(0);
                TreeView tv = (TreeView)_ux_NavigatorTabControl.SelectedTab.Controls[_ux_NavigatorTabControl.SelectedTab.Name + "TreeView"];
                if (tv.SelectedNode == null)
                {
                    RestoreActiveNode(tv);
                }

                //if (tv.SelectedNode == null)
                //{
                //    tv.SelectedNode = tv.Nodes[0];
                //}
                //else
                //{
                //    TreeNode currentSelectedNode = tv.SelectedNode;
                //    tv.SelectedNode = null;
                //    tv.SelectedNode = currentSelectedNode;
                //}
                // Save the value of the new currentCooperatorID
                _currentCooperatorID = ux_comboboxCNO.SelectedValue.ToString().Trim();
            }
            else
            {
                _ux_NavigatorTabControl.TabPages.Clear();
            }

// Now that selecting the CNO above has triggered the creation of the Navigator TabControl, place it in the left pane...
ux_panelNavigator.Controls.Clear();
ux_panelNavigator.Controls.Add(_ux_NavigatorTabControl);
_ux_NavigatorTabControl.Size = ux_panelNavigator.Size;
_ux_NavigatorTabControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void LoadCooperators(string localDBInstance)
        {
            DataTable cooperatorTable = null;
            LocalDatabase localDB = new LocalDatabase(localDBInstance);

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // If the cooperator_lookup table does not exist in the local DB 
            // create it using the GetDisplayMember method which will do three things...
            if (!localDB.TableExists("cooperator_lookup"))
            {
                // This will force the lookupTables class to:
                // 1) retrieve the record for the current cno from the remote DB
                // 2) create a new cooperator_lookup table on the local DB with one row of data
                // 3) copy that table to the in memory MRU table...
                //lookupTables.GetDisplayMember("cooperator_lookup", cno, "", cno);
                _sharedUtils.GetLookupDisplayMember("cooperator_lookup", _usernameCooperatorID, "", _usernameCooperatorID);

                // Now we will background thread the cooperator_lookup table loading - because it is really needed
                // and we can't count on the user to load it manually...
                //new System.Threading.Thread(lookupTables.LoadTableFromDatabase).Start("cooperator_lookup");
                //new System.Threading.Thread(lookupTables.LoadTableFromDatabase).Start("cooperator");
                new System.Threading.Thread(_sharedUtils.LookupTablesLoadTableFromDatabase).Start("cooperator_lookup");
            }
            else
            {
                // Since the cooperator_lookup table exists - make sure the selected cno is one of the rows...
                //lookupTables.GetDisplayMember("cooperator_lookup", cno, "", cno);
                _sharedUtils.GetLookupDisplayMember("cooperator_lookup", _usernameCooperatorID, "", _usernameCooperatorID);
            }
            // Now that we are sure the cooperator lookup table exists use it...
            //cooperatorTable = localDB.GetData("SELECT * FROM cooperator_lookup WHERE is_account_enabled = 'Y'");
            cooperatorTable = localDB.GetData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", new string[1] { "@accountisenabled=Y" });
            if (cooperatorTable.Columns.Contains("display_member")) cooperatorTable.DefaultView.Sort = "display_member ASC";
            if (cooperatorTable.Columns.Contains("site")) cooperatorTable.DefaultView.RowFilter = "site = '" + site + "'";
            //if (cooperatorTable.Columns.Contains("site_code")) cooperatorTable.DefaultView.RowFilter = "site_code = '" + site + "'";

            // Bind the control to the data in grinLookups...
            // WARNING!!!: You must set DisplayMember and ValueMember properties BEFORE setting 
            //             DataSource - otherwise the cbCooperators.SelectedValue.ToString() method 
            //             will return an object of DataRowView instead of the CNO value
            ux_comboboxCNO.DisplayMember = "display_member";
            ux_comboboxCNO.ValueMember = "value_member";
            ux_comboboxCNO.DataSource = cooperatorTable;

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private DataTable GetUserItemList(int cooperatorID)
        {
            DataTable returnList = new DataTable();

            DataSet UserListItems = _sharedUtils.GetWebServiceData("get_lists", ":cooperatorid=" + cooperatorID.ToString(), 0, 0);
            if (UserListItems != null &&
                UserListItems.Tables.Contains("get_lists"))
            {
                // First get and save the max path length allowed for a user list item...
                if (UserListItems.Tables["get_lists"].Columns.Contains("LIST_NAME") &&
                    UserListItems.Tables["get_lists"].Columns["LIST_NAME"].ExtendedProperties.Contains("max_length"))
                {
                    int maxPathLength = -1;
                    if (int.TryParse(UserListItems.Tables["get_lists"].Columns["LIST_NAME"].ExtendedProperties["max_length"].ToString(), out maxPathLength))
                    {
                        _maxPathLength = maxPathLength;
                    }
                    else
                    {
                        _maxPathLength = 300;  // This is the column size of LIST_NAME in GG v1.0
                    }
                }

                returnList = UserListItems.Tables["get_lists"].Copy();

                foreach (DataRow dr in returnList.Rows)
                {
                    string[] propertyTokens = dr["PROPERTIES"].ToString().Split(';');
                    if (propertyTokens == null ||
                        propertyTokens.Length < 3 ||
                        !Regex.Match(propertyTokens[0], @"\s*\S+(?:_ID|_id)\s*").Success)
                    {
                        // Convert legacy sys_user_item_list records to new format...
                        switch (dr["ID_TYPE"].ToString().Trim().ToUpper())
                        {
                            case "ACCESSION_ID":
                                dr["PROPERTIES"] = "ACCESSION_ID; :accessionid=" + dr["ID_NUMBER"].ToString() + "; @accession.accession_id=" + dr["ID_NUMBER"].ToString() + "; " + dr["PROPERTIES"].ToString();
                                break;
                            case "INVENTORY_ID":
                                dr["PROPERTIES"] = "INVENTORY_ID; :inventoryid=" + dr["ID_NUMBER"].ToString() + "; @inventory.inventory_id=" + dr["ID_NUMBER"].ToString() + "; " + dr["PROPERTIES"].ToString();
                                break;
                            case "ORDER_REQUEST_ID":
                                dr["PROPERTIES"] = "ORDER_REQUEST_ID; :orderrequestid=" + dr["ID_NUMBER"].ToString() + "; @order_request.order_request_id=" + dr["ID_NUMBER"].ToString() + "; " + dr["PROPERTIES"].ToString();
                                break;
                            case "COOPERATOR_ID":
                                dr["PROPERTIES"] = "COOPERATOR_ID; :cooperatorid=" + dr["ID_NUMBER"].ToString() + "; @cooperator.cooperator_id=" + dr["ID_NUMBER"].ToString() + "; " + dr["PROPERTIES"].ToString();
                                break;
                            case "GEOGRAPHY_ID":
                                dr["PROPERTIES"] = "GEOGRAPHY_ID; :geographyid=" + dr["ID_NUMBER"].ToString() + "; @geography.geography_id=" + dr["ID_NUMBER"].ToString() + "; " + dr["PROPERTIES"].ToString();
                                break;
                            case "TAXONOMY_GENUS_ID":
                                dr["PROPERTIES"] = "TAXONOMY_GENUS_ID; :taxonomygenusid=" + dr["ID_NUMBER"].ToString() + "; @taxonomy_genus.taxonomy_genus_id=" + dr["ID_NUMBER"].ToString() + "; " + dr["PROPERTIES"].ToString();
                                break;
                            case "CROP_ID":
                                dr["PROPERTIES"] = "CROP_ID; :cropid=" + dr["ID_NUMBER"].ToString() + "; @crop.crop_id=" + dr["ID_NUMBER"].ToString() + "; " + dr["PROPERTIES"].ToString();
                                break;
                            case "FOLDER":
                                //if (!dr["PROPERTIES"].ToString().Contains("FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=;") &&
                                //    !dr["PROPERTIES"].ToString().Contains("QUERY; DYNAMIC_FOLDER_SEARCH_CRITERIA="))
                                if (!dr["PROPERTIES"].ToString().Contains("FOLDER; DYNAMIC_FOLDER_") &&
                                    !dr["PROPERTIES"].ToString().Contains("QUERY; DYNAMIC_FOLDER_"))
                                {
                                    dr["PROPERTIES"] = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; DYNAMIC_FOLDER_RESOLVE_TO=; " + dr["PROPERTIES"].ToString();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            UserListItems.Dispose();

            return returnList;
        }

        public TabControl BuildTabControl(DataTable userItemList)
        {
            TabControl newTabControl = new TabControl();

            // Set the properties for the tab control...
            newTabControl.Name = "ux_tabcontrolGroupListNavigator";
            newTabControl.ContextMenuStrip = BuildTabControlContextMenuStrip();
            newTabControl.AllowDrop = true;
            newTabControl.DragOver += new DragEventHandler(ux_tabControl_DragOver);
            newTabControl.DragDrop += new DragEventHandler(ux_tabControl_DragDrop);
            newTabControl.MouseDown += new MouseEventHandler(ux_tabControl_MouseDown);
            newTabControl.SelectedIndexChanged += new EventHandler(ux_TabControl_SelectedIndexChanged);
            newTabControl.ImageList = BuildTabControlImageList();

            // Create sorted list of user items...
            userItemList.DefaultView.Sort = "tab_name ASC, list_name ASC, sort_order ASC";
//            userItemList.DefaultView.Sort = "app_user_item_list_id ASC";
            DataTable sortedUserItemList = userItemList.DefaultView.ToTable();

            // Get the names of the tabs from the user items list and iterate through them to create the tabs...
            // This is a neat trick to get the distinct values from a table (taking advantage of the distinct param of the ToTable method)...
            DataTable distinctTABNAME = userItemList.DefaultView.ToTable(true, "TAB_NAME");
            if (distinctTABNAME.Rows.Count > 0)
            {
                foreach (DataRow dr in distinctTABNAME.Rows)
                {
                    sortedUserItemList.DefaultView.RowFilter = "TAB_NAME='" + dr["TAB_NAME"].ToString().Replace("'", "''") + "'";
                    TabPage newTabPage = BuildTabPageAndTreeView(dr["TAB_NAME"].ToString(), sortedUserItemList.DefaultView.ToTable());
                    newTabControl.TabPages.Add(newTabPage);
                }
            }
            else
            {
                sortedUserItemList.DefaultView.RowFilter = "TAB_NAME='Tab 1'";
                TabPage newTabPage = BuildTabPageAndTreeView("Tab 1", sortedUserItemList.DefaultView.ToTable());
                newTabControl.TabPages.Add(newTabPage);
            }

            // Step 4 - cache a dictionary of tabpages to the tab control (for enabling tab page hide/show)
            //          and add each tab to the context menu 'Show Tab' list...
            Dictionary<string, TabPage> userTabPages = new Dictionary<string,TabPage>();
            foreach (TabPage tp in newTabControl.TabPages)
            {
                userTabPages.Add(tp.Name, tp);
                // Create the new tool strip menu item and bind it to the click event handler...
                ToolStripMenuItem tsmiNew = new ToolStripMenuItem(tp.Text, null, ux_tab_tsmi_NavigatorShowTabsItem_Click, tp.Name);
                tsmiNew.Checked = false;
                ((ToolStripMenuItem)newTabControl.ContextMenuStrip.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems.Add(tsmiNew);
            }
            newTabControl.Tag = userTabPages;

            // Step 5 - remove all tabpages and re-add them in the order the user prefers...
            // First get the user setting for visible tabs (and their order)...
            string userTabSettings = _sharedUtils.GetUserSetting("", newTabControl.Name, "TabPages.Order", "");
            // Then split the space delimited tabpage order list into an array...
            if (userTabSettings.Length > 0 &&
                ux_comboboxCNO.SelectedValue.ToString() == _usernameCooperatorID)
            {
                string[] tabOrder = userTabSettings.Split(new string[] { _pathSeparator }, StringSplitOptions.None);
                // Remove all tabpages from the tab control...
                newTabControl.TabPages.Clear();
                // Now re-add the ones the user wants to see (in the preferred order)...
                foreach (string tabName in tabOrder)
                {
                    if (userTabPages.ContainsKey(tabName))
                    {
                        newTabControl.TabPages.Add(userTabPages[tabName]);
                        ((ToolStripMenuItem)((ToolStripMenuItem)newTabControl.ContextMenuStrip.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems[tabName]).Checked = true;
                    }
                }
                // If for some reason the user's visible tabs preference setting does not reference any tabs in the collection
                // there will be no tabs present in the control - so re-add all tabs to the control as a fail-safe measure...
                if (newTabControl.TabCount == 0)
                {
                    foreach (string tabName in userTabPages.Keys)
                    {
                        newTabControl.TabPages.Add(userTabPages[tabName]);
                        ((ToolStripMenuItem)((ToolStripMenuItem)newTabControl.ContextMenuStrip.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems[tabName]).Checked = true;
                    }
                }
            }
            else
            {
                // Since there is no user preference for hidden/shown tabs - leave them all as shown and update the checkmark on the list...
                foreach (TabPage tp in newTabControl.TabPages)
                {
                    ((ToolStripMenuItem)((ToolStripMenuItem)newTabControl.ContextMenuStrip.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems[tp.Name]).Checked = true;
                }
            }

            // Step 6 - create the 'New Tab...' tabpage and add it to the tabcontrol...
            if (!newTabControl.TabPages.ContainsKey("ux_tabpageGroupListNavigatorNewTab"))
            {
                TabPage tp = new TabPage();
                tp.Name = "ux_tabpageGroupListNavigatorNewTab";
                tp.Text = "...";
                tp.ToolTipText = "New Tab...";
                newTabControl.TabPages.Add(tp);
                // For some reason you have to add the tabpage to the tabcontrol *before* setting the imagekey
                // in order for the binding of the image to occur properly...
                tp.ImageKey = "new_tab";
                // Wire up the click event handler for this tab...
                tp.Click += new System.EventHandler(this.ux_tab_cms_NavigatorNewTab_Click);
            }

            return newTabControl;
        }

        private void SyncTreeNodesWithUserItemListTable(TreeNode treeNode, DataTable syncedItemList)
        {
            DataRow syncDR;
            int pkey;

            // Attempt to find a row in the database that matches this node...
            if (int.TryParse(treeNode.ToolTipText, out pkey))
            {
                // This node was built using an existing database record (tooltip had a pkey) - so find the row in the collection of app_user_item_list records...
                syncDR = syncedItemList.Rows.Find(pkey);

                if (syncDR != null)
                {
                    // There is a record in the database that matches this node (so sync it up)...
                    if (isFolder(treeNode))
                    {
                        // First sync this folder node itself...
                        if (syncDR["TAB_NAME"].ToString() != treeNode.TreeView.Parent.Text) syncDR["TAB_NAME"] = treeNode.TreeView.Parent.Text;
                        if (treeNode.Parent == null)
                        {
                            if (syncDR["LIST_NAME"].ToString() != "{DBNull.Value}") syncDR["LIST_NAME"] = "{DBNull.Value}";
                        }
                        else
                        {
                            if (syncDR["LIST_NAME"].ToString() != treeNode.Parent.FullPath) syncDR["LIST_NAME"] = treeNode.Parent.FullPath;
                        }
                        if (syncDR["ID_TYPE"].ToString() != "FOLDER") syncDR["ID_TYPE"] = "FOLDER";
                        if (syncDR["SORT_ORDER"].ToString() != treeNode.Index.ToString()) syncDR["SORT_ORDER"] = treeNode.Index.ToString();
                        if (syncDR["TITLE"].ToString() != treeNode.Text) syncDR["TITLE"] = treeNode.Text;
                        //if (dr["DESCRIPTION"].ToString() != dr["DESCRIPTION"].ToString()) syncDR["DESCRIPTION"] = dr["DESCRIPTION"];
                        if (syncDR["PROPERTIES"].ToString() != treeNode.Tag.ToString()) syncDR["PROPERTIES"] = treeNode.Tag.ToString();

                        // Now iterate through all of the nodes in this folder...
                        foreach (TreeNode tn in treeNode.Nodes)
                        {
                            SyncTreeNodesWithUserItemListTable(tn, syncedItemList);
                        }
                    }
                    else
                    {
                        // Sync the item node...
                        if (syncDR["TAB_NAME"].ToString() != treeNode.TreeView.Parent.Text) syncDR["TAB_NAME"] = treeNode.TreeView.Parent.Text;
                        if (syncDR["LIST_NAME"].ToString() != treeNode.Parent.FullPath) syncDR["LIST_NAME"] = treeNode.Parent.FullPath;
                        if (syncDR["ID_NUMBER"].ToString() != treeNode.Name.Split('=')[1]) syncDR["ID_NUMBER"] = treeNode.Name.Split('=')[1];
                        if (syncDR["ID_TYPE"].ToString() != ((string)treeNode.Tag).Split(';')[0]) syncDR["ID_TYPE"] = ((string)treeNode.Tag).Split(';')[0];
                        if (syncDR["SORT_ORDER"].ToString() != treeNode.Index.ToString()) syncDR["SORT_ORDER"] = treeNode.Index.ToString();
                        if (syncDR["TITLE"].ToString() != treeNode.Text) syncDR["TITLE"] = treeNode.Text;
                        //if (dr["DESCRIPTION"].ToString() != dr["DESCRIPTION"].ToString()) syncDR["DESCRIPTION"] = dr["DESCRIPTION"];
                        if (syncDR["PROPERTIES"].ToString() != treeNode.Tag.ToString()) syncDR["PROPERTIES"] = treeNode.Tag.ToString();
                    }
                }
                else
                {
                    // Since this treenode was built using an existing database record - but the record can't be found
                    // assume that someone else (antother CT running) deleted the node - so delete the node in this CT...
                    treeNode.Remove();
                }
            }
            else
            {
                // This treenode was just built and has never been saved to the database - so create a new record for it in the app_user_item_list collection...
                DataRow newAppUserItemListRow = syncedItemList.NewRow();

                if (isFolder(treeNode))
                {
                    // First sync this folder node itself...
                    newAppUserItemListRow["COOPERATOR_ID"] = _usernameCooperatorID;
                    newAppUserItemListRow["TAB_NAME"] = treeNode.TreeView.Parent.Text;
                    if (treeNode.Parent == null)
                    {
                        newAppUserItemListRow["LIST_NAME"] = "{DBNull.Value}";
                    }
                    else
                    {
                        newAppUserItemListRow["LIST_NAME"] = treeNode.Parent.FullPath;
                    }
                    newAppUserItemListRow["ID_TYPE"] = "FOLDER";
                    newAppUserItemListRow["SORT_ORDER"] = treeNode.Index.ToString();
                    newAppUserItemListRow["TITLE"] = treeNode.Text;
                    //newAppUserItemListRow["DESCRIPTION"] = dr["DESCRIPTION"];
                    newAppUserItemListRow["PROPERTIES"] = treeNode.Tag.ToString();
                    // Now add it to the collection...
                    syncedItemList.Rows.Add(newAppUserItemListRow);

                    // Now iterate through all of the nodes in this folder...
                    foreach (TreeNode tn in treeNode.Nodes)
                    {
                        SyncTreeNodesWithUserItemListTable(tn, syncedItemList);
                    }
                }
                else
                {
                    newAppUserItemListRow["COOPERATOR_ID"] = _usernameCooperatorID;
                    newAppUserItemListRow["TAB_NAME"] = treeNode.TreeView.Parent.Text;
                    newAppUserItemListRow["LIST_NAME"] = treeNode.Parent.FullPath;
                    newAppUserItemListRow["ID_NUMBER"] = treeNode.Name.Split('=')[1];
                    newAppUserItemListRow["ID_TYPE"] = ((string)treeNode.Tag).Split(';')[0];
                    newAppUserItemListRow["SORT_ORDER"] = treeNode.Index.ToString();
                    newAppUserItemListRow["TITLE"] = treeNode.Text;
                    //newAppUserItemListRow["DESCRIPTION"] = dr["DESCRIPTION"];
                    newAppUserItemListRow["PROPERTIES"] = treeNode.Tag.ToString();
                    // Now add it to the collection...
                    syncedItemList.Rows.Add(newAppUserItemListRow);
                }
            }
        }


        private DataSet SaveNavigatorTabControlData(TabControl tcUserItemLists, int cooperatorID)
        {
            DataSet modifiedData = new DataSet();
            DataSet saveErrors = new DataSet();

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            // Only perform the save if the currently selected list is owned by the login user...
if ((int)ux_comboboxCNO.SelectedValue == cooperatorID)
    {
                // Reload the user's item list from the remote database (so that we can sync to the current tabpage treeviews)...
                DataTable syncedItemList = GetUserItemList(cooperatorID);

                if (syncedItemList != null &&
                    tcUserItemLists != null &&
                    tcUserItemLists.Tag != null &&
                    tcUserItemLists.Tag.GetType() == typeof(Dictionary<string, TabPage>))
                {
                    // Get the complete collection of tab pages (both hidden and visible)...
                    Dictionary<string, TabPage> userTabPages = (Dictionary<string, TabPage>)tcUserItemLists.Tag;

                    // Iterate through these tabs to get the embedded treeview's nodes...
                    foreach (TabPage tp in userTabPages.Values)
                    {
                        TreeView tv;
                        if (tp.Controls.ContainsKey(tp.Name + "TreeView"))
                        {
                            tv = (TreeView)tp.Controls[tp.Name + "TreeView"];
                            //tv = (TreeView)tp.Controls["Test Hidden (1)TreeView"];
                            foreach (TreeNode tn in tv.Nodes)
                            {
                                SyncTreeNodesWithUserItemListTable(tn, syncedItemList);
                            }
                        }
                    }

                    // Delete any records associated with deleted treeview nodes...
                    DataRow deleteDR;
                    foreach (int pkey in _deletedTreeNodes)
                    {
                        deleteDR = syncedItemList.Rows.Find(pkey);
                        if (deleteDR != null &&
                            deleteDR.RowState != DataRowState.Deleted)
                        {
                            deleteDR.Delete();
                        }
                    }

                    // Get just the rows that have changed and put them in to a new dataset...
                    if (syncedItemList.GetChanges() != null)
                    {
                        modifiedData.Tables.Add(syncedItemList.GetChanges());
                    }
                    // Call the web method to update the changed data...
                    saveErrors = _sharedUtils.SaveWebServiceData(modifiedData);

                    // If the commandline during application startup had a parameter for _saveListDataDumpFile set to a valid filepath 
                    // save the data to this file in XML format...
                    if (!string.IsNullOrEmpty(_saveListDataDumpFile))
                    {
                        try
                        {
                            modifiedData.WriteXml(_saveListDataDumpFile, XmlWriteMode.WriteSchema);
                        }
                        catch (Exception err)
                        {
//MessageBox.Show("Error attempting to save XML dataset to: " + _saveListDataDumpFile + "\n\nError Message:\n" + err.Message);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error attempting to save XML dataset to: {0}\n\nError Message:\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SaveListsMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, _saveListDataDumpFile, err.Message);
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, _saveListDataDumpFile);
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, err.Message);
//}
string[] argsArray = new string[100];
argsArray[0] = _saveListDataDumpFile;
argsArray[1] = err.Message;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
                        }
                    }

                    if (saveErrors != null &&
                        saveErrors.Tables.Contains(syncedItemList.TableName))
                    {
                        foreach (DataRow dr in saveErrors.Tables[syncedItemList.TableName].Rows)
                        {
                            if (dr["SavedAction"].ToString() == "Insert" && dr["SavedStatus"].ToString() != "Success")
                            {
//MessageBox.Show("The " + dr["TITLE"].ToString() + " item could not be successfully added to your list.\n\nError message:\n\n" + dr["ExceptionMessage"].ToString());
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The {0} item could not be successfully added to your list.\n\nError message:\n\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SaveListsMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dr["TITLE"].ToString(), dr["ExceptionMessage"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dr["TITLE"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dr["ExceptionMessage"].ToString());
//}
string[] argsArray = new string[100];
argsArray[0] = dr["TITLE"].ToString();
argsArray[1] = dr["ExceptionMessage"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
                            }
                            else if (dr["SavedAction"].ToString() == "Update" && dr["SavedStatus"].ToString() != "Success")
                            {
//MessageBox.Show("The " + dr["TITLE"].ToString() + " item could not be successfully updated for your list.\n\nError message:\n\n" + dr["ExceptionMessage"].ToString());
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The {0} item could not be successfully updated for your list.\n\nError message:\n\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SaveListsMessage3";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, ggMessageBox.MessageText, dr["TITLE"].ToString(), dr["ExceptionMessage"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, ggMessageBox.MessageText, dr["TITLE"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dr["ExceptionMessage"].ToString());
//}
string[] argsArray = new string[100];
argsArray[0] = dr["TITLE"].ToString();
argsArray[1] = dr["ExceptionMessage"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
                            }
                            else if (dr["SavedAction"].ToString() == "Delete" && dr["SavedStatus"].ToString() != "Success")
                            {
//MessageBox.Show("The " + dr["TITLE"].ToString() + " item could not be successfully deleted from your list.\n\nError message:\n\n" + dr["ExceptionMessage"].ToString());
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The {0} item could not be successfully deleted from your list.\n\nError message:\n\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SaveListsMessage4";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, ggMessageBox.MessageText, dr["TITLE"].ToString(), dr["ExceptionMessage"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, ggMessageBox.MessageText, dr["TITLE"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dr["ExceptionMessage"].ToString());
//}
string[] argsArray = new string[100];
argsArray[0] = dr["TITLE"].ToString();
argsArray[1] = dr["ExceptionMessage"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
                            }
                        }
                    }
                }
                else
                {
                    if(tcUserItemLists != null)
                    {
//MessageBox.Show("There were errors syncronizing your lists with the remote server.\n\nYour list changes have not been saved.");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors syncronizing your lists with the remote server.\n\nYour list changes have not been saved.", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SaveListsMessage5";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                    }
                }
            }
            // Restore cursor to default cursor...
            Cursor.Current = origCursor;

            return saveErrors;
        }

        private void ux_tab_tsmi_NavigatorShowTabsItem_DropDownOpened(object sender, EventArgs e)
        {
            ToolStripDropDownItem tsddi = (ToolStripDropDownItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsddi.GetCurrentParent();
            _ux_NavigatorTabControl = (TabControl)cms.SourceControl;
        }

        private string EnsureUniqueTabText(TabControl NavigatorTabControl, string TabText)
        {
            // Now we can get a unique name for this tree node (starting with the default node name passed in)...
            String uniqueTabText = TabText;

            // Let's make sure the tab name is unique..
            bool duplicateText = true;
            int i = 1;
            while (duplicateText)
            {
                // Assume this name is unique (until proven otherwise)
                duplicateText = false;
                if (NavigatorTabControl != null &&
                    NavigatorTabControl.Tag != null &&
                    NavigatorTabControl.Tag.GetType() == typeof(Dictionary<string, TabPage>))
                {
                    foreach (string tabTextKey in ((Dictionary<string, TabPage>)NavigatorTabControl.Tag).Keys)
                    {
                        if (tabTextKey.ToUpper() == uniqueTabText.ToUpper())
                        {
                            // Nope...  This is not a unique node name so increment a counter until it is unique...
                            uniqueTabText = TabText + " (" + i++.ToString() + ")";
                            duplicateText = true;
                        }
                    }

                    //if(((Dictionary<string, TabPage>)NavigatorTabControl.Tag).ContainsKey(TabText))
                }
                //foreach (TabPage tp in NavigatorTabControl.TabPages)
                //{
                //    if (tp.Text.ToUpper() == TabText.ToUpper())
                //    {
                //        // Nope...  This is not a unique node name so increment a counter until it is unique...
                //        uniqueTabText = TabText + " (" + i++.ToString() + ")";
                //        duplicateText = true;
                //    }
                //}
            }

            return uniqueTabText;
        }

        private void ux_TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tc = (TabControl)sender;
            ContextMenuStrip cms = tc.ContextMenuStrip;

            if (tc.SelectedIndex > -1)
            {
                if (tc.SelectedTab.Name == "ux_tabpageGroupListNavigatorNewTab")
                //if (tc.SelectedIndex == (tc.TabPages.Count - 1))
                {
                    int indexOfNewTab = tc.SelectedIndex;
                    NavigatorTabProperties newTabDialog = new NavigatorTabProperties(_pathSeparator, "_", _sharedUtils);
                    newTabDialog.StartPosition = FormStartPosition.CenterParent;
                    if (newTabDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        string uniqueTabText = EnsureUniqueTabText(tc, newTabDialog.TabText);
                        TabPage tp = BuildTabPageAndTreeView(uniqueTabText, null);

                        if (tp != null)
                        {
                            tc.TabPages.Insert(indexOfNewTab, tp);
                            tc.SelectedIndex = indexOfNewTab;

                            // Create the new tool strip menu item and bind it to the click event handler...
                            ToolStripMenuItem tsmi = new ToolStripMenuItem(tp.Text, null, ux_tab_tsmi_NavigatorShowTabsItem_Click, tp.Text);
                            tsmi.Checked = true;
                            ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems.Add(tsmi);

                            // Add this new tabpage to the tabcontrol's dictionary list (for hide/show functionality)...
                            if (tc != null &&
                                tc.Tag != null &&
                                tc.Tag.GetType() == typeof(Dictionary<string, TabPage>) &&
                                !((Dictionary<string, TabPage>)tc.Tag).ContainsKey(tp.Name))
                            {
                                ((Dictionary<string, TabPage>)tc.Tag).Add(tp.Name, tp);
                            }

                            if (((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems.Count > 1)
                            {
                                // We have more than one tab in the ShowTabs list so enable some menu choices...
                                ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorHideTab"]).Enabled = true;
                                ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorDeleteTab"]).Enabled = true;
                                ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).Enabled = true;
                            }
                        }
                        else
                        {
                            tc.DeselectTab(indexOfNewTab);
                        }
                    }
                    else
                    {
                        tc.DeselectTab(indexOfNewTab);
                    }
                }
                else
                {
                    // Set focus on the treeview node for this treeview if it is not set...
                    foreach (Control ctrl in tc.SelectedTab.Controls)
                    {
                        if (ctrl.GetType() == typeof(TreeView))
                        {
                            TreeView tv = (TreeView)ctrl;
                            if (tv.SelectedNode == null)
                            {
                                RestoreActiveNode(tv);
                            }
                        }
                    }

                    // Now refresh the data views...
//lastFullPath = "";
//lastTabName = "";
                    SetAllUserSettings();
                    // Refresh the data view...
                    RefreshMainDGVData();
                    RefreshMainDGVFormatting();
                }
            }
        }

        private void RestoreActiveNode(TreeView tv)
        {
            TreeNode activeNode = null;
            string activeNodeFullPath = _sharedUtils.GetUserSetting("", tv.Name, "SelectedNode.FullPath", "");
            if (!string.IsNullOrEmpty(activeNodeFullPath))
            {
                string[] activeNodePathTokens = activeNodeFullPath.Trim().Split(new string[] { _pathSeparator }, StringSplitOptions.None);
                // Start the path from the treeview Nodes collection...
                if (activeNodePathTokens.Length > 0 &&
                    tv.Nodes.ContainsKey(activeNodePathTokens[0]))
                {
                    activeNode = tv.Nodes[activeNodePathTokens[0]];
                }
                // Iterate through the rest of the full path using the active Node object...
                for (int i = 1; i < activeNodePathTokens.Length; i++)
                {
                    if (activeNode != null &&
                        activeNode.Nodes.ContainsKey(activeNodePathTokens[i]))
                    {
                        activeNode = activeNode.Nodes[activeNodePathTokens[i]];
                    }
                }
            }
            if (activeNode != null)
            {
                tv.SelectedNode = activeNode;
            }
            else
            {
                tv.SelectedNode = tv.Nodes[0];
            }
        }

        private ContextMenuStrip BuildTabControlContextMenuStrip()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Name = "ux_tab_cms_Navigator";
            // Add the sub-menu items to the context menu strip
            cms.Items.Add(new ToolStripMenuItem("New Tab", null, ux_tab_cms_NavigatorNewTab_Click, "ux_tab_cms_NavigatorNewTab"));
            cms.Items.Add(new ToolStripMenuItem("Delete Tab", null, ux_tab_cms_NavigatorDeleteTab_Click, "ux_tab_cms_NavigatorDeleteTab"));
            cms.Items.Add(new ToolStripMenuItem("Hide Tab", null, ux_tab_cms_NavigatorHideTab_Click, "ux_tab_cms_NavigatorHideTab"));
            cms.Items.Add(new ToolStripMenuItem("Show Tab", null, ux_tab_cms_NavigatorShowTab_Click, "ux_tab_cms_NavigatorShowTab"));
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add(new ToolStripMenuItem("Properties...", null, ux_tab_cms_NavigatorProperties_Click, "ux_tab_cms_NavigatorProperties"));

            // Add an event handler to the Show Tab dropdown menu to remember the tab control (SourceControl) - this is a workaround for a .NET bug
            ((ToolStripDropDownItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownOpened += new EventHandler(ux_tab_tsmi_NavigatorShowTabsItem_DropDownOpened);
            return cms;
        }

        private void ux_tab_cms_NavigatorNewTab_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.GetCurrentParent();
            TabControl tc = (TabControl)cms.SourceControl;

            NavigatorTabProperties newTabDialog = new NavigatorTabProperties(_pathSeparator, "_", _sharedUtils);
            newTabDialog.StartPosition = FormStartPosition.CenterParent;
            if (newTabDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Build the empty tabpage (includes an empty treeview)...
                string uniqueTabText = EnsureUniqueTabText(tc, newTabDialog.TabText);
                TabPage tpNew = BuildTabPageAndTreeView(uniqueTabText, null);

                if (tpNew != null)
                {
                    if (tc.SelectedIndex > -1)
                    {
                        tc.TabPages.Insert(tc.SelectedIndex, tpNew);
                    }
                    else
                    {
                        tc.TabPages.Insert(tc.TabPages.IndexOfKey("ux_tabpageGroupListNavigatorNewTab"), tpNew);
                    }
                    // Create the new tool strip menu item and bind it to the click event handler...
                    ToolStripMenuItem tsmiNew = new ToolStripMenuItem(tpNew.Text, null, ux_tab_tsmi_NavigatorShowTabsItem_Click, tpNew.Text);
                    tsmiNew.Checked = true;
                    ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems.Add(tsmiNew);

                    // Add this new tabpage to the tabcontrol's dictionary list (for hide/show functionality)...
                    if (tc != null &&
                        tc.Tag != null &&
                        tc.Tag.GetType() == typeof(Dictionary<string, TabPage>) &&
                        !((Dictionary<string, TabPage>)tc.Tag).ContainsKey(tpNew.Name))
                    {
                        ((Dictionary<string, TabPage>)tc.Tag).Add(tpNew.Name, tpNew);
                    }

                    // Update the Show/Hide tabs menu items...
                    if (((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems.Count > 1)
                    {
                        // We have more than one tab in the ShowTabs list so enable some menu choices...
                        ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorHideTab"]).Enabled = true;
                        ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorDeleteTab"]).Enabled = true;
                        ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).Enabled = true;
                    }
                }
            }
        }

        private void ux_tab_cms_NavigatorDeleteTab_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.GetCurrentParent();
            TabControl tc = (TabControl)cms.SourceControl;

            string tabPageName = tc.SelectedTab.Name;
            ToolStripMenuItem tsmiDelete = (ToolStripMenuItem)((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems[tc.SelectedTab.Name];
            if (tabPageName != "ux_tabpageGroupListNavigatorNewTab")
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Warning!!! \n\n Deleting this tab will remove all of it's lists from the database permanently!", "Delete Tab", MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2);
                ggMessageBox.Name = "ux_navigatormenuDeleteTabMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                //if (DialogResult.OK == MessageBox.Show("Warning!!! \n\n Deleting this tab will remove all of it's lists from the database permanently!", "Delete Tab", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
//                if (DialogResult.OK == ggMessageBox.ShowDialog())
                {
                    int currentTabIndex = tc.SelectedIndex;

                    // Temporarily de-select all tabs before removing the selected tab...
//tc.SelectedIndex = -1;
//tc.SelectedTab = null;
//tc.TabPages.RemoveByKey(tabPageName);
//tc.SelectTab(0);
tc.TabPages.Remove(tc.SelectedTab);
//tc.TabPages.RemoveAt(currentTabIndex);

                    // Remove the tool strip menu item from the list...
                    ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems.Remove(tsmiDelete);

                    // Build a list of app_user_item_list_ids to remove...
foreach(Control ctrl in ((Dictionary<string, TabPage>)tc.Tag)[tabPageName].Controls)
{
    if (ctrl.GetType() == typeof(TreeView))
    {
        _deletedTreeNodes.AddRange(AddToDeletedTreeNodeList((TreeNode)((TreeView)ctrl).Nodes[0]));
    }
}

                    // Remove the tabpage from the tabcontrol's dictionary...
                    ((Dictionary<string, TabPage>)tc.Tag).Remove(tabPageName);

                    // Activate a new tab...
//ux_tabcontrolGroupListNavigator_SelectNewActiveTab(currentTabIndex);
//tc.SelectedIndex = Math.Max(0, currentTabIndex - 1);
// Update the Show/Hide tabs menu items...
if (((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems.Count < 2)
{
    // We have more than one tab in the ShowTabs list so enable some menu choices...
    ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorHideTab"]).Enabled = false;
    ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorDeleteTab"]).Enabled = false;
    ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).Enabled = false;
}
                }
            }
        }

        private void ux_tab_cms_NavigatorHideTab_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.GetCurrentParent();
            TabControl tc = (TabControl)cms.SourceControl;

            //int currentTabIndex = tc.SelectedIndex;
            string tabName = tc.SelectedTab.Name;
            ToolStripMenuItem tsmiHide = (ToolStripMenuItem)((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems[tc.SelectedTab.Name];
//ToolStripMenuItem jj = (ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"];
            if (tabName != "ux_tabpageGroupListNavigatorNewTab")
            {
//// Temporarily de-select all tabs before removing the selected tab...
//tc.SelectedIndex = -1;
//tc.TabPages.RemoveByKey(tabName);
                tc.TabPages.Remove(tc.SelectedTab);

                // Uncheck the menu item corresponding to the tab being hidden...
                if (tsmiHide != null) tsmiHide.Checked = false;

                // Activate a new tab...
//ux_tabcontrolGroupListNavigator_SelectNewActiveTab(currentTabIndex);
            }
        }

        private void ux_tab_cms_NavigatorShowTab_Click(object sender, EventArgs e)
        {
        }

        private void ux_tab_cms_NavigatorProperties_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.GetCurrentParent();
            TabControl tc = (TabControl)cms.SourceControl;

            NavigatorTabProperties newTabDialog = new NavigatorTabProperties(_pathSeparator, "_", _sharedUtils);
            newTabDialog.StartPosition = FormStartPosition.CenterParent;
            if (newTabDialog.ShowDialog(this) == DialogResult.OK)
            {
                string currentTabText = tc.SelectedTab.Text;
                if (tc != null &&
                    tc.Tag != null &&
                    tc.Tag.GetType() == typeof(Dictionary<string, TabPage>) &&
                    ((Dictionary<string, TabPage>)tc.Tag).ContainsKey(currentTabText))
                {
                    // Remove the current tab from the Show Tabs list (to make the list of unique names accurate 
                    // when calling EnsureUniqueTabText method is called)...
                    ((Dictionary<string, TabPage>)tc.Tag).Remove(currentTabText);
                    // Enusre the tab's text/name is unique...
                    string uniqueTabText = EnsureUniqueTabText(tc, newTabDialog.TabText);
                    // Rename the tab's text, name, and treeview control's name...
                    tc.SelectedTab.Text = uniqueTabText;
                    tc.SelectedTab.Name = uniqueTabText;
                    tc.SelectedTab.Controls[currentTabText + "TreeView"].Name = uniqueTabText + "TreeView";
                    // Re-Add the new tab name to the list of tabs (in the Show Tab menu)...
                    ((Dictionary<string, TabPage>)tc.Tag).Add(uniqueTabText, tc.SelectedTab);
                    // Re-Add the new tab name to the collection of tabs associated with this Navigator Control...
                    ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems[currentTabText].Text = uniqueTabText;
                    ((ToolStripMenuItem)cms.Items["ux_tab_cms_NavigatorShowTab"]).DropDownItems[currentTabText].Name = uniqueTabText;
                }
            }
        }

        private void ux_tab_tsmi_NavigatorShowTabsItem_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem tsddi = (ToolStripDropDownItem)sender;
            ToolStripMenuItem tsmi = (ToolStripMenuItem)tsddi.OwnerItem;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.GetCurrentParent();
            //TabControl tc = (TabControl)cms.SourceControl;

            if (!((ToolStripMenuItem)tsddi).Checked)
            {
                if (_ux_NavigatorTabControl != null &&
                    _ux_NavigatorTabControl.Tag != null &&
                    _ux_NavigatorTabControl.Tag.GetType() == typeof(Dictionary<string, TabPage>) &&
                    ((Dictionary<string, TabPage>)_ux_NavigatorTabControl.Tag).ContainsKey(tsddi.Name))
                {
                    ((ToolStripMenuItem)tsddi).Checked = true;
                    _ux_NavigatorTabControl.TabPages.Insert(_ux_NavigatorTabControl.TabPages.Count - 1, (TabPage)((Dictionary<string, TabPage>)_ux_NavigatorTabControl.Tag)[tsddi.Name]);
                    _ux_NavigatorTabControl.SelectedIndex = _ux_NavigatorTabControl.TabPages.Count - 2;
                }
            }
            else
            {
                ((ToolStripMenuItem)tsddi).Checked = false;
                _ux_NavigatorTabControl.TabPages.RemoveByKey(tsddi.Name);

                // Activate a new tab...
//ux_tabcontrolGroupListNavigator_SelectNewActiveTab(currentTabIndex);
            }
        }

        private ContextMenuStrip BuildTreeviewContextMenuStrip()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            if (_ux_treeview_cms_Navigator != null)
            {
                cms = _ux_treeview_cms_Navigator;
            }
            else
            {
                cms.Name = "ux_treeview_cms_Navigator";
                // Bind the event handler for show/hide properties menu item (based on item type)...
                cms.Opening += new System.ComponentModel.CancelEventHandler(ux_contextmenustripTreeView_Opening);

                // Add the sub-menu items to the context menu strip
                cms.Items.Add(new ToolStripMenuItem("New List", null, ux_treeview_cms_NavigatorNewList_Click, "ux_treeview_cms_NavigatorNewList"));
                cms.Items.Add(new ToolStripMenuItem("Clear List", null, ux_treeview_cms_NavigatorClearList_Click, "ux_treeview_cms_NavigatorClearList"));
                cms.Items.Add(new ToolStripMenuItem("Refresh List", null, ux_treeview_cms_NavigatorRefreshList_Click, "ux_treeview_cms_NavigatorRefreshList"));
                cms.Items.Add(new ToolStripMenuItem("Sort Ascending", null, ux_treeview_cms_NavigatorSortAscending_Click, "ux_treeview_cms_NavigatorSortAscending"));
                cms.Items.Add(new ToolStripMenuItem("Sort Descending", null, ux_treeview_cms_NavigatorSortDescending_Click, "ux_treeview_cms_NavigatorSortDescending"));
                cms.Items.Add(new ToolStripMenuItem("Rename", null, ux_treeview_cms_NavigatorRename_Click, "ux_treeview_cms_NavigatorRename"));
                cms.Items.Add(new ToolStripSeparator());
                cms.Items.Add(new ToolStripMenuItem("Cut", null, ux_treeview_cms_NavigatorCut_Click, "ux_treeview_cms_NavigatorCut"));
                cms.Items.Add(new ToolStripMenuItem("Copy", null, ux_treeview_cms_NavigatorCopy_Click, "ux_treeview_cms_NavigatorCopy"));
                cms.Items.Add(new ToolStripMenuItem("Paste", null, ux_treeview_cms_NavigatorPaste_Click, "ux_treeview_cms_NavigatorPaste"));
                cms.Items.Add(new ToolStripSeparator());
                cms.Items.Add(new ToolStripMenuItem("Delete", null, ux_treeview_cms_NavigatorDelete_Click, "ux_treeview_cms_NavigatorDelete"));
                cms.Items.Add(new ToolStripSeparator());
                cms.Items.Add(new ToolStripMenuItem("Properties...", null, ux_treeview_cms_NavigatorProperties_Click, "ux_treeview_cms_NavigatorProperties"));
                _ux_treeview_cms_Navigator = cms;
            }
            return cms;
        }

        private void ux_treeview_cms_NavigatorNewList_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            // Now we can get a unique name for this tree node (starting with the default node name: New List)...
            String newNodeText = "New List";
            TreeNode tnDestination = tv.SelectedNode;

            // Now add it to the treeview if it's parent is a list folder object...
            if (isFolder(tnDestination))
            {
                TreeNode tnNew = new TreeNode(newNodeText);

                // Let's make sure the new node name is unique..
                newNodeText = EnsureUniqueNodeText(tnDestination, tnNew);

                tnNew.Name = newNodeText;
                tnNew.Text = newNodeText;
                tnNew.Tag = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
                tnNew.ImageKey = "inactive_folder";
                tnNew.SelectedImageKey = "active_folder";
                tnDestination.Nodes.Add(tnNew);

                // If the path length exceeds the maximum allowed string length for a full path - remove the new folder...
                if (!PathLengthOK(tnNew))
                {
                    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Invalid Path", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "ux_treeviewmenuNewListMessage1";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, tnNew.FullPath);
string[] argsArray = new string[100];
argsArray[0] = tnNew.FullPath;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                    ggMessageBox.ShowDialog();
                    tnNew.Remove();
                }
                else
                {
                    // Make the new folder the active folder...
                    tnNew.EnsureVisible();
                }
            }
        }

        private void ux_treeview_cms_NavigatorClearList_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            // Remember the currently selected node...
            TreeNode currentNode = tv.SelectedNode;

            // Build a list of app_user_item_list_ids to remove...
            foreach (TreeNode tn in tv.SelectedNode.Nodes)
            {
                _deletedTreeNodes.AddRange(AddToDeletedTreeNodeList(tn));
            }

            // Delete the nodes from the treeview...
            tv.SelectedNode.Nodes.Clear();

            // If there are no nodes left in the treeview - make a new empty list folder...
            if (tv.Nodes.Count == 0)
            {
                TreeNode tnNew = new TreeNode();
                tnNew.Name = "New List";
                tnNew.Text = "New List";
                tnNew.Tag = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
                tv.Nodes.Add(tnNew);
            }

            // Force a refresh...
            lastFullPath = "";
            lastTabName = "";
            SetAllUserSettings();
            // Refresh the data view...
            RefreshMainDGVData();
        }

        private void ux_treeview_cms_NavigatorRefreshList_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            RefreshTreeviewNodeFormatting(tv.SelectedNode);

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void ux_treeview_cms_NavigatorSortAscending_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            if (isFolder(tv.SelectedNode))
            {
                sortFolder(tv.SelectedNode, "ASCENDING");
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void ux_treeview_cms_NavigatorSortDescending_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            if (isFolder(tv.SelectedNode))
            {
                sortFolder(tv.SelectedNode, "DESCENDING");
            }
            //// Create a new temp treeview to sort the selected rows
            //// because TreeView.Sort() works on all nodes (not just the selected nodes)...
            //TreeView tvSortAscending = new TreeView();
            //// Make a copy of the nodes to be sorted and add them to the temp treeview...
            //tvSortAscending.Nodes.Add((TreeNode)tv.SelectedNode.Clone());
            //// Bind the IComparer class to initiate the sorting...
            //tvSortAscending.TreeViewNodeSorter = new NodeSortDescending();
            //// Make a copy of the sorted nodes and add it to the nodes parent (either another node or the treeview itself)
            //TreeNode sortedNode = (TreeNode)tvSortAscending.Nodes[0].Clone();
            //if (tv.SelectedNode.Parent != null)
            //{
            //    tv.SelectedNode.Parent.Nodes.Insert(tv.SelectedNode.Index, sortedNode);
            //}
            //else
            //{
            //    tv.Nodes.Add(sortedNode);
            //}
            //// Cleanup - remove the original unsorted node and make the new node selected and expanded...
            //tv.SelectedNode.Remove();
            //tv.SelectedNode = sortedNode;
            //tv.SelectedNode.Expand();

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void ux_treeview_cms_NavigatorRename_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;
            if (!tv.LabelEdit) tv.LabelEdit = true;
            tv.SelectedNode.BeginEdit();
            // The renaming process is finished in the treeView_AfterLabelEdit() method below...
        }

        private void ux_treeview_cms_NavigatorCut_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            // Copy the selected node from the tree view to the clipboard...
            Clipboard.SetDataObject(tv.SelectedNode, false, 1, 1000);

            // Add the app_user_item_list pkey(s) associated with this node to the collection of records
            // that should be deleted from the server when the CT exits...
            _deletedTreeNodes.AddRange(AddToDeletedTreeNodeList(tv.SelectedNode));
            // Now remove the node from the current tree...
            tv.SelectedNode.Remove();
        }

        private void ux_treeview_cms_NavigatorCopy_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;
            
            // Copy the selected node from the tree view to the clipboard...
            TreeNode nodeCopy = (TreeNode)tv.SelectedNode.Clone();
            ResetTreeviewNodeToolTip(nodeCopy);
            Clipboard.SetDataObject(nodeCopy, false, 1, 1000);
        }

        private void ux_treeview_cms_NavigatorPaste_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            // Process a treenode from the clipboard...
            if (Clipboard.ContainsData(typeof(TreeNode).ToString()))
            {
                DataObject obj = (DataObject)Clipboard.GetDataObject();
                TreeNode tn = (TreeNode)obj.GetData(typeof(TreeNode));
RemoveFromDeletedTreeNodeList(tn, _deletedTreeNodes);
                // Make sure the new node has a unique name at the destination node...
                string uniqueText = EnsureUniqueNodeText(tv.SelectedNode, tn);
                if (isFolder(tn))
                {
                    tn.Name = uniqueText;
                    tn.Text = uniqueText;
                }
                else
                {
                    tn.Text = uniqueText;
                }
                tv.SelectedNode.Nodes.Add(tn);

                // Make sure the new node(s) do not exceed the max length of the full path...
                if (!PathLengthOK(tn))
                {
                    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Invalid Path", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "ux_treeviewmenuPasteListMessage1";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, tn.FullPath);
string[] argsArray = new string[100];
argsArray[0] = tn.FullPath;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                    ggMessageBox.ShowDialog();
                    tn.Remove();
                }
            }
        }

        private void ux_treeview_cms_NavigatorDelete_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;

            // Remember the parent of the node being removed...
            TreeNode currentNode = tv.SelectedNode.Parent;

            // Build a list of app_user_item_list_ids to remove...
            _deletedTreeNodes.AddRange(AddToDeletedTreeNodeList(tv.SelectedNode));

            // Delete the node from the tree view...
            tv.SelectedNode.Remove();

            // If there are no nodes left in the treeview - make a new empty list folder...
            if (tv.Nodes.Count == 0)
            {
                TreeNode tnNew = new TreeNode();
                tnNew.Name = "New List";
                tnNew.Text = "New List";
                tnNew.Tag = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
                tv.Nodes.Add(tnNew);
            }

            // Force a refresh...
            lastFullPath = "";
            lastTabName = "";
            SetAllUserSettings();
            // Refresh the data view...
            RefreshMainDGVData();
        }

        private List<int> AddToDeletedTreeNodeList(TreeNode deletedTreeNode)
        {
            List<int> deletedTreeNodeList = new List<int>();

            // First mark the record associated with this node for deletion...
            int pkey;
            if (int.TryParse(deletedTreeNode.ToolTipText, out pkey))
            {
                if (!deletedTreeNodeList.Contains(pkey)) deletedTreeNodeList.Add(pkey);
            }

            // Now if this node is a folder iterate through all of the child-nodes too...
            if (isFolder(deletedTreeNode))
            {
                foreach(TreeNode tn in deletedTreeNode.Nodes)
                {
                    deletedTreeNodeList.AddRange(AddToDeletedTreeNodeList(tn));
                }
            }

            return deletedTreeNodeList;
        }

        private void RemoveFromDeletedTreeNodeList(TreeNode removedTreeNode, List<int> deletedTreeNodeList)
        {
            // First mark the record associated with this node for deletion...
            int pkey;
            if (int.TryParse(removedTreeNode.ToolTipText, out pkey))
            {
                // Remove the pkey from the list of records to delete...
                if (deletedTreeNodeList.Contains(pkey)) deletedTreeNodeList.Remove(pkey);
                // Check again to make sure there are not duplicates in the list for this pkey...
                if (deletedTreeNodeList.Contains(pkey)) RemoveFromDeletedTreeNodeList(removedTreeNode, deletedTreeNodeList);
            }

            // Now if this node is a folder iterate through all of the child-nodes too...
            if (isFolder(removedTreeNode))
            {
                foreach (TreeNode tn in removedTreeNode.Nodes)
                {
                    RemoveFromDeletedTreeNodeList(tn, deletedTreeNodeList);
                }
            }
        }

        private void ResetTreeviewNodeToolTip(TreeNode newNode)
        {
            // Clear the ToolTipText for this node (this will make the node 
            // a new unsaved node)...
            newNode.ToolTipText = "";

            // Now if this node is a folder iterate through all of the child-nodes too...
            if (isFolder(newNode))
            {
                foreach (TreeNode tn in newNode.Nodes)
                {
                    ResetTreeviewNodeToolTip(tn);
                }
            }
        }

        private void ux_contextmenustripTreeView_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ContextMenuStrip cms = (ContextMenuStrip)sender;
            //tv.SelectedNode
            TreeView tv = (TreeView)cms.SourceControl;
            if (tv.SelectedNode != null &&
                tv.SelectedNode.Tag != null &&
                !string.IsNullOrEmpty(tv.SelectedNode.Tag.ToString()) &&
isFolder(tv.SelectedNode) &&
//(tv.SelectedNode.Tag.ToString().Trim().ToUpper().StartsWith("FOLDER") ||
// tv.SelectedNode.Tag.ToString().Trim().ToUpper().StartsWith("QUERY")) &&
                cms.Items.ContainsKey("ux_treeview_cms_NavigatorProperties"))
            {
                cms.Items["ux_treeview_cms_NavigatorProperties"].Enabled = true;
            }
            else
            {
                cms.Items["ux_treeview_cms_NavigatorProperties"].Enabled = false;
            }

        }

        private void ux_treeview_cms_NavigatorProperties_Click(object sender, EventArgs e)
        {
            // First we need to find out which tree view has been clicked...
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            TreeView tv = (TreeView)cms.SourceControl;
            // Now we need to instantiate a treeview node properties dialog box...
//string pKey = drs[0][userItemList.PrimaryKey[0].ColumnName].ToString();
//TreeviewNodeProperties tvnp = new TreeviewNodeProperties(_sharedUtils, pKey, userItemList);

// Iterate through the original node properties to get xxx_NAME_FORMULAs...
string[] origPropertyTokens = tv.SelectedNode.Tag.ToString().Split(';');
List<string> origNameFormulas = new List<string>();
foreach (string propertyToken in origPropertyTokens)
{
    if (propertyToken.Contains("_NAME_FORMULA"))
    {
        origNameFormulas.Add(propertyToken);
    }
}
// Get the original dynamic folder search criteria and resolve_to choice...
string origQuery = GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", tv.SelectedNode, false, "");
string origResolveTo = GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", tv.SelectedNode, false, "");
TreeviewNodeProperties tvnp = new TreeviewNodeProperties(_sharedUtils, tv.SelectedNode);
tvnp.StartPosition = FormStartPosition.CenterParent;
tvnp.ShowDialog();
// Get the current dynamic folder search criteria and resolve_to choice...
string currentQuery = GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", tv.SelectedNode, false, "");
string currentResolveTo = GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", tv.SelectedNode, false, "");
if (string.IsNullOrEmpty(currentQuery))
{
    // Update the treeview node to use the static folder icon...
    tv.SelectedNode.ImageKey = "inactive_folder";
    tv.SelectedNode.SelectedImageKey = "active_folder";
}
else
{
    // Update the treeview node to use the dynamic folder icon...
    tv.SelectedNode.ImageKey = "inactive_dynamic_folder";
    tv.SelectedNode.SelectedImageKey = "active_dynamic_folder";
}
// Iterate through the current node properties to get xxx_NAME_FORMULAs...
string[] currentPropertyTokens = tv.SelectedNode.Tag.ToString().Split(';');
List<string> currentNameFormulas = new List<string>();
foreach (string propertyToken in currentPropertyTokens)
{
    if (propertyToken.Contains("_NAME_FORMULA"))
    {
        currentNameFormulas.Add(propertyToken);
    }
}
// Compare the original and current name formulas - if there are differences force a treenode formatting refresh...
bool nodeNeedsFormatRefresh = false;
// Check to see if all the original name formulas are in the current node properties...
foreach (string origNameFormula in origNameFormulas)
{
    if (!currentNameFormulas.Contains(origNameFormula)) nodeNeedsFormatRefresh = true;
}
// Check to see if all the current name formulas were in the original node properties...
foreach (string currentNameFormula in currentNameFormulas)
{
    if (!origNameFormulas.Contains(currentNameFormula)) nodeNeedsFormatRefresh = true;
}
// Compare the original and current dynamic folder setting - if there are differences force a data refresh...
if (origQuery.Trim() != currentQuery.Trim() ||
    origResolveTo.Trim() != currentResolveTo.Trim())
{
    // Reset the last processed treeview node (to force a full data refresh)...
    lastFullPath = "";
    lastTabName = "";
    // So that this call will be refreshed properly...
    SetAllUserSettings();
    // Refresh the data view...
    RefreshMainDGVData();
}
// Refresh the node formatting if needed...
if (nodeNeedsFormatRefresh) RefreshTreeviewNodeFormatting(tv.SelectedNode);
            //if (tvnp.ShowDialog() == DialogResult.OK)
            //{
            //    tv.SelectedNode.Tag = tvnp.Properties;
            //}
        }

        private void RefreshTreeviewNodeFormatting(TreeNode treeNode)
        {
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            //if (isFolder(treeNode))
            if (treeNode.Nodes.Count > 0)
            {
                // First rename (title) the node items...
                //value = "{get_accession.accession_number_part1} + \" \" + {get_accession.accession_number_part2} + \" \" + {get_accession.accession_number_part3}; ";
                System.Collections.Generic.Dictionary<string, string> idTypeFormattingFormula = new System.Collections.Generic.Dictionary<string, string>();
                System.Collections.Generic.List<string> idTypes = new System.Collections.Generic.List<string>();
                System.Collections.Generic.Dictionary<string, string> idNumbers = new System.Collections.Generic.Dictionary<string, string>();
                DataSet ds = new DataSet();

                // First find all of the distinct ID_TYPES, 
                // their corresponding formatting formulas,
                // and gather all of the ID_NUMBERS for each ID_TYPE in the userItemList collection...
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    if (!isFolder(tn))
                    {
//string pKey = tn.Tag.ToString().Split(';')[1].Trim();
//string nodePKeyType = pKey.Split('=')[0].Replace(":", "").Trim().ToUpper();
//string nodePKeyValue = pKey.Split('=')[1].Trim().ToUpper();
string[] pKey = tn.Name.Split('=');
string nodePKeyType = pKey[0];
string nodePKeyValue = pKey[1];

                        // Get the ID_TYPE...
                        if (!idTypes.Contains(nodePKeyType)) idTypes.Add(nodePKeyType);
                        // Now get the formatting formula for each ID_TYPE in the collection...
                        if (!idTypeFormattingFormula.ContainsKey(nodePKeyType))
                        {
                            //string formula = GetTreeviewNodeProperty(nodePKeyType + "_NAME_FORMULA", tn);
                            string formula = GetTreeviewNodeProperty(tn.Tag.ToString().Split(';')[0].Trim().ToUpper() + "_NAME_FORMULA", tn, true, "");
                            if (string.IsNullOrEmpty(formula))
                            {
                                // Could not find a formula for this type of treenode object type...
                            }
                            idTypeFormattingFormula.Add(nodePKeyType, formula);
                        }
                        // Next collect all of the ID_NUMBERS for each of the ID_TYPES for the userItemList collection...
                        if (!idNumbers.ContainsKey(nodePKeyType))
                        {
                            idNumbers.Add(nodePKeyType, nodePKeyValue + ",");
                        }
                        else
                        {
                            idNumbers[nodePKeyType] = idNumbers[nodePKeyType] + nodePKeyValue + ",";
                        }
                    }
                    else
                    {
                        if (ux_checkboxIncludeSubFolders.Checked)
                        {
                            RefreshTreeviewNodeFormatting(tn);
                        }
                    }
                }

                Dictionary<string, Dictionary<int, string>> friendlyNames = new Dictionary<string,Dictionary<int,string>>();

                // Make all the trips to the server now to get all data needed for new userItemList titles...
                foreach (string idType in idTypes)
                {
                    // Create the new dictionary LU for the friendly name and add it to the collection...
                    friendlyNames.Add(idType, new Dictionary<int, string>());

                    // Break down the name formula into tokens and process the tokens one by one...
                    string[] formatTokens = idTypeFormattingFormula[idType].Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries);
                    string staticTextSeparator = "";
                    foreach (string formatToken in formatTokens)
                    {
                        if (formatToken.Contains("{") &&
                            formatToken.Contains("}"))
                        {
                            // This is a DB field used in the title - so if we don't already have it go get it now...
                            string[] dataviewAndField = formatToken.Trim().Replace("{", "").Replace("}", "").Trim().Split(new char[] { '.' });
                            if (!ds.Tables.Contains(dataviewAndField[0]))
                            {
                                DataSet newDS = _sharedUtils.GetWebServiceData(dataviewAndField[0], idType.Trim().ToLower() + "=" + idNumbers[idType].Trim().TrimEnd(','), 0, 0);
                                if (newDS != null &&
                                    newDS.Tables.Contains(dataviewAndField[0]))
                                {
                                    ds.Tables.Add(newDS.Tables[dataviewAndField[0]].Copy());
                                }
else if (newDS.Tables.Contains("ExceptionTable") &&
    newDS.Tables["ExceptionTable"].Rows.Count > 0)
{
    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error retrieving data from {0} to use in building a node friendly name.\n\nFull error message:\n{1}", "Get Name Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
    ggMessageBox.Name = "RefreshTreeviewNodeFormatting1";
    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewAndField[0], newDS.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewAndField[0]);
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, newDS.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
//}
string[] argsArray = new string[100];
argsArray[0] = dataviewAndField[0];
argsArray[1] = newDS.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
    ggMessageBox.ShowDialog();
}
                            }
                            // Process all of the rows in the table to add the dbToken to the friendly name...
                            // But first check to make sure the field exists in the datatable (if it doesn't exist just skip it)...
                            if (ds != null &&
                                ds.Tables.Count > 0 &&
                                ds.Tables[dataviewAndField[0]].Columns.Contains(dataviewAndField[1]))
                            {
                                foreach (DataRow dr in ds.Tables[dataviewAndField[0]].Rows)
                                {
                                    int pkey;
                                    string dbTokenText = dr[dataviewAndField[1]].ToString();
                                    DataColumn dc = ds.Tables[dataviewAndField[0]].Columns[dataviewAndField[1]];

                                    if (int.TryParse(dr[ds.Tables[dataviewAndField[0]].PrimaryKey[0]].ToString(), out pkey) &&
                                        !string.IsNullOrEmpty(dbTokenText))
                                    {
                                        // Resolve fkeys and code_values to the display_member if necessary...
                                        if (_sharedUtils.LookupTablesIsValidFKField(dc))
                                        {
                                            dbTokenText = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dbTokenText, "", dbTokenText);
                                        }
                                        else if (_sharedUtils.LookupTablesIsValidCodeValueField(dc))
                                        {
                                            dbTokenText = _sharedUtils.GetLookupDisplayMember("code_value_lookup", dbTokenText, dc.ExtendedProperties["group_name"].ToString(), dbTokenText);
                                        }
                                        // If the pkey is already in the dictionary append to it (otherwise add it)...
                                        if (friendlyNames[idType].ContainsKey(pkey))
                                        {
                                            friendlyNames[idType][pkey] += staticTextSeparator + dbTokenText;
                                        }
                                        else
                                        {
                                            friendlyNames[idType][pkey] = staticTextSeparator + dbTokenText;
                                        }
                                    }
                                }
                            }
                            staticTextSeparator = "";
                        }
                        else
                        {
                            staticTextSeparator += formatToken.Trim().Replace("\"", "");
                        }
                    }
                    if (!string.IsNullOrEmpty(staticTextSeparator))
                    {
                        for (int i = 0; i < friendlyNames[idType].Keys.Count; i++)
                        {
                            friendlyNames[idType][friendlyNames[idType].ElementAt(i).Key] += staticTextSeparator;
                        }
                    }
                }

                // Now refresh the titles for each item in the collection...
                // This next line REALLY speeds things up (it tells the control to stop painting during a major update)...
                treeNode.TreeView.BeginUpdate();
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    if (!isFolder(tn))
                    {
                        string[] pKey = tn.Name.Split('=');
                        string nodePKeyType = pKey[0];
                        string nodePKeyValue = pKey[1];
                        int pkey;
                        //
                        if (int.TryParse(nodePKeyValue, out pkey) &&
                            friendlyNames[nodePKeyType].ContainsKey(pkey))
                        {
                            string title = friendlyNames[nodePKeyType][pkey];
                            tn.Text = title;
                            title = EnsureUniqueNodeText(treeNode, tn);
                            tn.Text = title.Trim();
                        }
                    }
                }
                // We are all done updating names so now we can let the treeview resume painting its display...
                treeNode.TreeView.EndUpdate();


//                foreach (TreeNode tn in treeNode.Nodes)
//                {
//                    if (!isFolder(tn))
//                    {
////string pKey = tn.Tag.ToString().Split(';')[1].Trim();
////string nodePKeyType = pKey.Split('=')[0].Replace(":", "").Trim().ToUpper();
////string nodePKeyValue = pKey.Split('=')[1].Trim().ToUpper();
//string[] pKey = tn.Name.Split('=');
//string nodePKeyType = pKey[0];
//string nodePKeyValue = pKey[1];

//                        string title = "";
//                        string[] formatTokens = idTypeFormattingFormula[nodePKeyType].Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries);
//                        foreach (string formatToken in formatTokens)
//                        {
//                            if (formatToken.Contains("{") &&
//                                formatToken.Contains("}"))
//                            {
//                                // This is a DB field used in the title - so go get it...
//                                string[] dataviewAndField = formatToken.Trim().Replace("{", "").Replace("}", "").Trim().Split(new char[] { '.' });
//                                if (dataviewAndField.Length == 2)
//                                {
//                                    if (ds.Tables.Contains(dataviewAndField[0]) &&
//                                        ds.Tables[dataviewAndField[0]].Rows.Count > 0 &&
//                                        ds.Tables[dataviewAndField[0]].Columns.Contains(dataviewAndField[1]))
//                                    {
//                                        // The list of dataviews that can be used for building node titles must have a pkey that is one of the ID_TYPES
//                                        // otherwise this next line won't work...
//                                        DataRow tokenRawDataRow = ds.Tables[dataviewAndField[0]].Rows.Find(nodePKeyValue);
//                                        if (tokenRawDataRow != null)
//                                        {
//                                            DataColumn dc = tokenRawDataRow.Table.Columns[dataviewAndField[1]];
//                                            string valueMember = tokenRawDataRow[dataviewAndField[1]].ToString();
//                                            if (_sharedUtils.LookupTablesIsValidFKField(dc) &&
//                                                !string.IsNullOrEmpty(valueMember))
//                                            {
//                                                title += _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), valueMember, "", valueMember);
//                                            }
//                                            else if (_sharedUtils.LookupTablesIsValidCodeValueField(dc) &&
//                                                    !string.IsNullOrEmpty(valueMember))
//                                            {
////title += _sharedUtils.GetLookupDisplayMember("code_value_lookup", valueMember, "", valueMember);
//                                                title += _sharedUtils.GetLookupDisplayMember("code_value_lookup", valueMember, dc.ExtendedProperties["group_name"].ToString(), valueMember);
//                                            }
//                                            else
//                                            {
//                                                title += valueMember;
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                            else
//                            {
//                                title += formatToken.Trim().Replace("\"", "");
//                            }
//                        }
//                        // If there was success in building a new node title and it is a different name than the old title, make sure it is unique and then replace the old title...
//                        if (!string.IsNullOrEmpty(title))
//                        {
//                            tn.Text = title;
//title = EnsureUniqueNodeText(treeNode, tn);
//tn.Text = title.Trim();
//                        }
//                    }

//                }

                // Now resort the items...
                string sortMode = GetTreeviewNodeProperty("SORT_MODE", treeNode, true, "MANUAL");
                string folderGroupingMode = GetTreeviewNodeProperty("FOLDER_GROUPING_MODE", treeNode, true, "TOP");
                if (isFolder(treeNode) && sortMode.ToUpper().Trim() != "MANUAL") sortFolder(treeNode, sortMode);
            }

/*
            // Find the user_item_list row corresponding to this node...
            if (isFolder(treeNode))
            {
                // First rename (title) the node items...
                RefreshUserItemListTitles(drcItems);
                // Now resort the items...
                // Get the sort settings from the folder (actually it will come from the parent of the collection items)...
                if (drcItems.Length > 0)
                {
                    string sortMode = GetUserItemListProperty("SORT_MODE", drcItems[0]);
                    string folderGroupingMode = GetUserItemListProperty("FOLDER_GROUPING_MODE", drcItems[0]);
                    SortUserItemList(tabText, treeNode, sortMode, folderGroupingMode == "TOP");
                }
            }
*/
            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private string GetTreeviewNodeProperty(string propertyName, TreeNode currentNode, bool inheritValue, string defaultValue)
        {
            string value = null;
            if(currentNode != null && 
                currentNode.Tag != null)
            {
                // Break the tag (properties) up into individual property tokens...
                string[] properties = currentNode.Tag.ToString().Split(';');
                foreach (string property in properties)
                {
                    // If you find the token that contains the property name - retrieve the value
                    if (property.Contains(propertyName))
                    {
                        // Split it into the key/value pair...
                        string[] keyValuePair = property.Split('=');
                        if (keyValuePair.Length == 2)
                        {
                            value = keyValuePair[1];
                        }
                        else if (keyValuePair.Length > 2 &&
                            keyValuePair[0].Trim() == propertyName.Trim())
                        {
                            // The string has more than 1 '=' character in it so use
                            // the first '=' as the delimiter for the key/value pair
                            // (which will inlcude all remaining '=' chars in the value string
                            value = property.Substring(property.IndexOf('=') + 1);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(value) &&
                inheritValue)
            {
                if (currentNode.Parent != null)
                {
                    //  Didn't find the property - look at the parent's properties...
                    value = GetTreeviewNodeProperty(propertyName, currentNode.Parent, inheritValue, defaultValue);
                }
                else
                {
                    // There is no setting for this property in the tree - get it from the app...
                    value = _sharedUtils.GetAppSettingValue(propertyName);
                }
            }
            if (value == null && defaultValue != null) value = defaultValue;
            return value;
        }

        private void ux_tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            TabControl tc = (TabControl)sender;
            int clickedTabPage = tc.SelectedIndex;

            // Attempt to find the tabpage that was clicked...
            for (int i = 0; i < tc.TabPages.Count; i++)
            {
                if (tc.GetTabRect(i).Contains(e.Location)) clickedTabPage = i;
            }

            if (e.Button == MouseButtons.Left)
            {
                // Begin tabpage drag and drop move (if the clicked tab is not 
                // the "ux_tabpageGroupListNavigatorNewTab" tabpage (which is used to add new dataviews)...
                //if (tc.TabPages[clickedTabPage] != tc.TabPages["ux_tabpageGroupListNavigatorNewTab"])
                if (clickedTabPage != (tc.TabPages.Count - 1))
                {
                    tc.DoDragDrop(tc.TabPages[clickedTabPage], DragDropEffects.Move);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Make the right clicked tabpage the selected tabpage for the control...
                tc.SelectTab(clickedTabPage);
            }
        }

        private void ux_tabControl_DragDrop(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect == e.Effect)
            {
                TabControl tc = (TabControl)sender;

                // Convert the mouse coordinates from screen to client...
                Point ptClientCoord = tc.PointToClient(new Point(e.X, e.Y));

                int destinationTabPageIndex = -1;
                int originalTabPageIndex = -1;

                // Attempt to find where the tabpage should be dropped...
                for (int i = 0; i < tc.TabPages.Count; i++)
                {
//if (tc.TabPages[i] == e.Data.GetData(typeof(TabPage))) originalTabPageIndex = i;
if (tc.TabPages[i] == e.Data.GetData("System.Windows.Forms.TabPage")) originalTabPageIndex = i;
                    if (tc.GetTabRect(i).Contains(ptClientCoord)) destinationTabPageIndex = i;
                }

                // Now create a copy of the tabpage that is being moved so that 
                // you can remove the orginal and insert the copy at the right spot...
//TabPage newTabPage = new TabPage();
//newTabPage.Text = ((TabPage)e.Data.GetData(typeof(TabPage))).Text.Trim();
//newTabPage.Name = ((TabPage)e.Data.GetData(typeof(TabPage))).Name.Trim();// newTabPage.Text + "Page";
                //// Create a pointer to the existing tree view...
                //TreeView tv = (TreeView)((TabPage)e.Data.GetData(typeof(TabPage))).Controls.Find(newTabPage.Name + "TreeView", true)[0];
                // Find the treeview for this tab page so that it can be added to the new tab page...
//if (e.Data.GetDataPresent(typeof(TabPage)))
if (e.Data.GetDataPresent("System.Windows.Forms.TabPage"))
                {
//tc.TabPages.Remove((TabPage)e.Data.GetData(typeof(TabPage)));
tc.TabPages.Remove((TabPage)e.Data.GetData("System.Windows.Forms.TabPage"));
//tc.TabPages.Insert(destinationTabPageIndex, (TabPage)e.Data.GetData(typeof(TabPage)));
tc.TabPages.Insert(destinationTabPageIndex, (TabPage)e.Data.GetData("System.Windows.Forms.TabPage"));
//tc.SelectTab((TabPage)e.Data.GetData(typeof(TabPage)));
tc.SelectTab((TabPage)e.Data.GetData("System.Windows.Forms.TabPage"));
                    //TreeView tv = null;
//foreach (Control ctrl in ((TabPage)e.Data.GetData(typeof(TabPage))).Controls)
//{
//    if (ctrl.GetType() == typeof(TreeView)) tv = (TreeView)ctrl;
//}

//// If we found the treeview - set the location and size of this tree view in the new tab page...
//if (tv != null)
//{
//    tv.Location = new Point(0, 0);
//    tv.Size = new Size(newTabPage.Size.Width, newTabPage.Size.Height);
//    // Bind a context menu to the tree view...
//    tv.ContextMenuStrip = _ux_treeview_cms_Navigator;
//    // Now add the tree view to the new tab page...
//    newTabPage.Controls.Add(tv);
//    tc.TabPages.Insert(destinationTabPageIndex, newTabPage);
//    tc.SelectTab(destinationTabPageIndex);
//    if (originalTabPageIndex < destinationTabPageIndex)
//    {
//        tc.TabPages.RemoveAt(originalTabPageIndex);
//    }
//    else
//    {
//        tc.TabPages.RemoveAt(originalTabPageIndex + 1);
//    }
//}
                }
            }
        }

        private void ux_tabControl_DragOver(object sender, DragEventArgs e)
        {
            TabControl tc = (TabControl)sender;

            // Convert the mouse coordinates from screen to client...
            Point ptClientCoord = tc.PointToClient(new Point(e.X, e.Y));

            // User is re-ordering a tab...
            string[] junk = e.Data.GetFormats();
            string junk1 = e.Data.ToString();
            if (e.Data.GetType() == typeof(DataObject))
            {
//if (e.Data.GetDataPresent(typeof(TabPage)))
if (e.Data.GetDataPresent("System.Windows.Forms.TabPage"))
                {
//int destinationTabPage = tc.TabPages.IndexOf((TabPage)e.Data.GetData(typeof(TabPage)));
int destinationTabPage = tc.TabPages.IndexOf((TabPage)e.Data.GetData("System.Windows.Forms.TabPage"));
                    // Attempt to find the tabpage that is being dragged over...
                    for (int i = 0; i < tc.TabPages.Count; i++)
                    {
                        if (tc.GetTabRect(i).Contains(ptClientCoord)) destinationTabPage = i;
                    }

//if (e.Data.GetDataPresent(typeof(TabPage)) &&
if (e.Data.GetDataPresent("System.Windows.Forms.TabPage") &&
//tc.TabPages[destinationTabPage] != (TabPage)e.Data.GetData(typeof(TabPage)) /* &&
tc.TabPages[destinationTabPage] != (TabPage)e.Data.GetData("System.Windows.Forms.TabPage") /* &&
                destinationTabPage != ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabcontrolGroupListNavigator")*/
                                                                                                                                      )
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
//else if (e.Data.GetDataPresent(typeof(TreeNode)))
else if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode"))
                {
                    for (int i = 0; i < tc.TabCount; i++)
                    {
                        //if (tc.TabPages[i].Name != "ux_tabpageGroupListNavigatorNewTab" &&
                        //    tc.GetTabRect(i).Contains(ptClientCoord)) tc.SelectTab(i);
                        if (i != (tc.TabPages.Count - 1) &&
                            tc.GetTabRect(i).Contains(ptClientCoord)) tc.SelectTab(i);
                    }
                }
            }
        }

        private TabPage BuildTabPageAndTreeView(string tabPageName, DataTable treeviewItems)
        {
            TabPage newTabPage = new TabPage();
            TreeView newTabPageTreeView = new TreeView();

            newTabPage.Text = tabPageName;
            newTabPage.Name = tabPageName;

            TreeView tvNew = new TreeView();
            tvNew.Name = newTabPage.Name + "TreeView";

            // Set the location and size of the tree view...
            tvNew.PathSeparator = _pathSeparator;
            tvNew.ImageList = BuildTreeviewImageList();
            tvNew.ImageKey = "inactive_unknown";
            tvNew.SelectedImageKey = "active_unknown";
            tvNew.Location = new System.Drawing.Point(0, 0);
            tvNew.Size = new System.Drawing.Size(newTabPage.Size.Width, newTabPage.Size.Height);
            tvNew.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            tvNew.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvNew.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
            tvNew.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            tvNew.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
            tvNew.MouseClick += new MouseEventHandler(this.treeView_MouseClick);
            tvNew.AllowDrop = true;
            tvNew.ItemDrag += new ItemDragEventHandler(this.treeView_ItemDrag);
            tvNew.DragOver += new DragEventHandler(this.treeView_DragOver);
            tvNew.DragDrop += new DragEventHandler(this.treeView_DragDrop);
            //tvNew.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_BeforeExpand);

            // Bind a context menu to the tree view...
            tvNew.ContextMenuStrip = BuildTreeviewContextMenuStrip();


            if (treeviewItems != null &&
                treeviewItems.Rows.Count > 0)
            {
                // Create the treeview folders and items...
                treeviewItems.DefaultView.RowFilter = "TAB_NAME='" + tabPageName.Replace("'", "''") + "'";
                //treeviewItems.DefaultView.Sort = "app_user_item_list_id ASC";
                DataTable sortedTreeviewItems = treeviewItems.DefaultView.ToTable();

                // Step 1 - create the root folders...
                sortedTreeviewItems.DefaultView.RowFilter = "LIST_NAME='{DBNull.Value}' AND ID_TYPE='FOLDER'";
                sortedTreeviewItems.DefaultView.Sort = "LIST_NAME ASC, SORT_ORDER ASC";
                foreach (DataRowView drv in sortedTreeviewItems.DefaultView)
                {
                    TreeNode rootFolder = new TreeNode();
                    rootFolder.Name = drv["TITLE"].ToString();
                    rootFolder.Text = drv["TITLE"].ToString();
                    //rootFolder.Tag = "FOLDER";
                    rootFolder.Tag = drv["PROPERTIES"].ToString();
                    rootFolder.ImageKey = "inactive_folder";
                    rootFolder.SelectedImageKey = "active_folder";
                    if (rootFolder.Tag.ToString().Trim().ToUpper().StartsWith("QUERY"))
                    {
                        rootFolder.ImageKey = "inactive_dynamic_folder";
                        rootFolder.SelectedImageKey = "active_dynamic_folder";
                    }
rootFolder.ToolTipText=drv[0].ToString();
                    tvNew.Nodes.Add(rootFolder);
                }

                // Step 2 - create all remaining folders...
                sortedTreeviewItems.DefaultView.RowFilter = "LIST_NAME<>'{DBNull.Value}' AND ID_TYPE='FOLDER'";
                sortedTreeviewItems.DefaultView.Sort = "LIST_NAME ASC, SORT_ORDER ASC";
                foreach (DataRowView drv in sortedTreeviewItems.DefaultView)
                {
                    string[] nodePathNames = drv["LIST_NAME"].ToString().Split('|');
                    if (nodePathNames.Length > 0)
                    {
                        // Ensure the root folder is there...
                        if(!tvNew.Nodes.ContainsKey(nodePathNames[0]))
                        {
                            TreeNode rootFolder = new TreeNode();
                            rootFolder.Name = nodePathNames[0];
                            rootFolder.Text = nodePathNames[0];
                            rootFolder.Tag = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
                            rootFolder.ImageKey = "inactive_folder";
                            rootFolder.SelectedImageKey = "active_folder";
                            if (rootFolder.Tag.ToString().Trim().ToUpper().StartsWith("QUERY"))
                            {
                                rootFolder.ImageKey = "inactive_dynamic_folder";
                                rootFolder.SelectedImageKey = "active_dynamic_folder";
                            }
                            rootFolder.ToolTipText = "";
                            tvNew.Nodes.Add(rootFolder);
                        }
                        TreeNode currentNode = tvNew.Nodes[nodePathNames[0]];
                        for (int i = 1; i < nodePathNames.Length; i++)
                        {
                            // Ensure all other parent folders are there...
                            if (!currentNode.Nodes.ContainsKey(nodePathNames[i]))
                            {
                                // This sub-folder is missing so create it and make it the current folder...
                                TreeNode newFolder = new TreeNode();
                                newFolder.Name = nodePathNames[i];
                                newFolder.Text = nodePathNames[i];
                                //newFolder.Tag = "FOLDER";
                                newFolder.Tag = drv["PROPERTIES"].ToString();
                                newFolder.ImageKey = "inactive_folder";
                                newFolder.SelectedImageKey = "active_folder";
                                if (newFolder.Tag.ToString().Trim().ToUpper().StartsWith("QUERY"))
                                {
                                    newFolder.ImageKey = "inactive_dynamic_folder";
                                    newFolder.SelectedImageKey = "active_dynamic_folder";
                                }
//newFolder.ToolTipText = "-1";
newFolder.ToolTipText = "";
                                currentNode.Nodes.Add(newFolder);
                                currentNode = currentNode.Nodes[nodePathNames[i]];
                            }
                            else
                            {
                                // Found the folder - so make it the current folder...
                                currentNode = currentNode.Nodes[nodePathNames[i]];
                            }
                        }
                        // Create the new folder...
                        if (!currentNode.Nodes.ContainsKey(drv["TITLE"].ToString()))
                        {
                            TreeNode rootFolder = new TreeNode();
                            rootFolder.Name = drv["TITLE"].ToString();
                            rootFolder.Text = drv["TITLE"].ToString();
                            //rootFolder.Tag = "FOLDER";
                            rootFolder.Tag = drv["PROPERTIES"].ToString();
                            rootFolder.ImageKey = "inactive_folder";
                            rootFolder.SelectedImageKey = "active_folder";
                            if (rootFolder.Tag.ToString().Trim().ToUpper().StartsWith("QUERY"))
                            {
                                rootFolder.ImageKey = "inactive_dynamic_folder";
                                rootFolder.SelectedImageKey = "active_dynamic_folder";
                            }
                            rootFolder.ToolTipText = drv[0].ToString();
                            currentNode.Nodes.Add(rootFolder);
                        }
                        else
                        {
                            // Found the folder - so make it the current folder...
                            currentNode = currentNode.Nodes[drv["TITLE"].ToString()];
                            // Now update all of the node properties to reflect the saved folder properties...
                            currentNode.Name = drv["TITLE"].ToString();
                            currentNode.Text = drv["TITLE"].ToString();
                            //rootFolder.Tag = "FOLDER";
                            currentNode.Tag = drv["PROPERTIES"].ToString();
                            currentNode.ImageKey = "inactive_folder";
                            currentNode.SelectedImageKey = "active_folder";
                            if (currentNode.Tag.ToString().Trim().ToUpper().StartsWith("QUERY"))
                            {
                                currentNode.ImageKey = "inactive_dynamic_folder";
                                currentNode.SelectedImageKey = "active_dynamic_folder";
                            }
                            currentNode.ToolTipText = drv[0].ToString();
                        }
                    }
                }

                // Step 3 - create all remaining items...
                sortedTreeviewItems.DefaultView.RowFilter = "ID_TYPE<>'FOLDER'";
                sortedTreeviewItems.DefaultView.Sort = "LIST_NAME ASC, SORT_ORDER ASC";
                foreach (DataRowView drv in sortedTreeviewItems.DefaultView)
                {
                    string[] nodePathNames = drv["LIST_NAME"].ToString().Split('|');
                    if (nodePathNames.Length > 0 &&
                        tvNew.Nodes.ContainsKey(nodePathNames[0]))
                    {
                        TreeNode currentNode = tvNew.Nodes[nodePathNames[0]];
                        for (int i = 1; i < nodePathNames.Length; i++)
                        {
                            if (currentNode != null &&
                                currentNode.Nodes.ContainsKey(nodePathNames[i]))
                            {
                                // Found the folder - so make it the current folder...
                                currentNode = currentNode.Nodes[nodePathNames[i]];
                            }
                            else
                            {
                                // For some reason the folder record does not exist - but this item references it so create it...
                                TreeNode newFolder = new TreeNode();
                                newFolder.Name = nodePathNames[i];
                                newFolder.Text = nodePathNames[i];
                                newFolder.Tag = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
                                newFolder.ImageKey = "inactive_folder";
                                newFolder.SelectedImageKey = "active_folder";
//newFolder.ToolTipText = "-1";
newFolder.ToolTipText = "";
                                currentNode.Nodes.Add(newFolder);
                                currentNode = currentNode.Nodes[nodePathNames[i]];
                            }
                        }

                        // Now that we are in the right folder - add the item...
                        TreeNode newNode = new TreeNode();

                        newNode.Name = drv["PROPERTIES"].ToString().Split(';')[1].Trim();
                        newNode.Text = drv["TITLE"].ToString();
                        newNode.Tag = drv["PROPERTIES"].ToString();
                        newNode.ImageKey = "inactive_" + drv["ID_TYPE"].ToString();
                        newNode.SelectedImageKey = "active_" + drv["ID_TYPE"].ToString();
newNode.ToolTipText = drv[0].ToString();

                        // Now check to see if virtual nodes are wanted for this new node...
                        //if(_virtualNodeTypes.ContainsKey(drv["PROPERTIES"].ToString().Split(';')[0].Trim().ToUpper()))
                        if(!string.IsNullOrEmpty(_sharedUtils.GetAppSettingValue(drv["PROPERTIES"].ToString().Split(';')[0].Trim().ToUpper() + "_VIRTUAL_NODE_DATAVIEW")))
                        {
                            TreeNode dummyNode = new TreeNode();
                            string virtualQuery = _sharedUtils.GetAppSettingValue(drv["PROPERTIES"].ToString().Split(';')[0].Trim().ToUpper() + "_VIRTUAL_NODE_DATAVIEW") + ", ";
                            virtualQuery += drv["PROPERTIES"].ToString().Split(';')[1].Trim().ToLower();

                            dummyNode.Text = "!!DUMMYNODE!!";
                            dummyNode.Name = "!!DUMMYNODE!!"; ;
                            dummyNode.Tag = virtualQuery;
                            //dummyNode.ImageKey = "inactive_" + virtualQuery;
                            //dummyNode.SelectedImageKey = "active_" + virtualQuery;
                            newNode.Nodes.Add(dummyNode);
                        }
                        currentNode.Nodes.Add(newNode);
                    }
                }
            }
            else
            {
                // Build the root node...
                TreeNode tnNewRootNode = new TreeNode();
                tnNewRootNode.Name = "{DBNull.Value}";
                tnNewRootNode.Text = newTabPage.Text + " Root Folder";
                tnNewRootNode.Tag = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
                tnNewRootNode.ImageKey = "inactive_folder";
                tnNewRootNode.SelectedImageKey = "active_folder";
                tvNew.Nodes.Add(tnNewRootNode);
                // Build the first list node...
                TreeNode tnNewListNode = new TreeNode();
                tnNewListNode.Name = tnNewRootNode.FullPath;
                tnNewListNode.Text = "New List";
                tnNewListNode.Tag = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
                tnNewListNode.ImageKey = "inactive_folder";
                tnNewListNode.SelectedImageKey = "active_folder";
//tnNewListNode.ToolTipText = "-1";
tnNewListNode.ToolTipText = "";
                tnNewRootNode.Nodes.Add(tnNewListNode);
            }

            // Add the treeview to the tab...
            newTabPage.Controls.Add(tvNew);

            // Select the active node on the treeview by getting the last node saved in user settings...
            //            string selectedNodeFullPath = _sharedUtils.GetUserSetting(tvNew.Name, "SelectedNode.FullPath", "");
            //            SelectNodeByPath(selectedNodeFullPath, tvNew);

            return newTabPage;
        }


        #region to be removed (old BuildTabPage - does not handle treeview data)...
//        private TabPage BuildTabPage(string tabPageName)
//        {
//            TabPage newTabPage = new TabPage();
//            TreeView newTabPageTreeView = new TreeView();

//            newTabPage.Text = tabPageName;
//            newTabPage.Name = tabPageName;

//            TreeView tvNew = new TreeView();
//            tvNew.Name = newTabPage.Name + "TreeView";

//            // Set the location and size of the tree view...
//            tvNew.ImageList = BuildTreeviewImageList();
//            tvNew.ImageKey = "inactive_unknown";
//            tvNew.SelectedImageKey = "active_unknown";
//            tvNew.Location = new System.Drawing.Point(0, 0);
//            tvNew.Size = new System.Drawing.Size(newTabPage.Size.Width, newTabPage.Size.Height);
//            tvNew.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
//            tvNew.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
//            tvNew.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
//            tvNew.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
//            tvNew.MouseClick += new MouseEventHandler(this.treeView_MouseClick);
//            tvNew.AllowDrop = true;
//            tvNew.ItemDrag += new ItemDragEventHandler(this.treeView_ItemDrag);
//            tvNew.DragOver += new DragEventHandler(this.treeView_DragOver);
//            //tvNew.DragDrop += new DragEventHandler(this.treeView_DragDrop);
//            tvNew.DragDrop += new DragEventHandler(this.treeViewDragDrop);
//            //tvNew.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_BeforeExpand);

//            //// Create the root folder...
//            //tvNew.Nodes.Add("{DBNull.Value}", "");

//            // Bind a context menu to the tree view...
////            tvNew.ContextMenuStrip = ux_contextmenustripTreeView;

//            // Add the treeview to the tab...
//            newTabPage.Controls.Add(tvNew);

//            // Select the active node on the treeview by getting the last node saved in user settings...
////            string selectedNodeFullPath = _sharedUtils.GetUserSetting(tvNew.Name, "SelectedNode.FullPath", "");
////            SelectNodeByPath(selectedNodeFullPath, tvNew);

//            return newTabPage;
//        }
        #endregion

        private List<KeyValuePair<string, int>> BuildPKeyList(TreeNode treeNode, string dvParamList, string dvPKeyName, bool includeSubfolders)
        {
            List<KeyValuePair<string, int>> resultItemList = new List<KeyValuePair<string, int>>();
            Dictionary<string, string> searchItemList = new Dictionary<string, string>();

            if (treeNode != null)
            {
                if (isFolder(treeNode))
                {
                    // If this is a dynamic folder go resolve the query...
                    if (treeNode.Tag.ToString().Split(';')[0].Trim().ToUpper() == "QUERY")
                    {
                        // This is a query obect - pass it directly to the search engine to convert it to a collection of dataview supported item 'type'...
//string queryString = treeNode.Tag.ToString().Split(';')[1].Trim();
//queryString = queryString.Substring(queryString.IndexOf('=') + 1);
string queryString = GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", treeNode, false, "");
string resolveTo = GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", treeNode, false, "default");
//if (string.IsNullOrEmpty(resolveTo)) dvParamList = resolveTo;
                        resultItemList.AddRange(ResolveItemList(queryString, resolveTo, dvParamList, dvPKeyName, int.Parse(ux_numericupdownMaxRowsReturned.Value.ToString())));
                    }

                    // Now process each item this folder may contain...
                    foreach (TreeNode ftn in treeNode.Nodes)
                    {
                        //if (ftn.Tag.ToString().Split(';')[0].Trim().ToUpper() == "FOLDER")
                        if (isFolder(ftn))
                        {
                            // This is a collection of items so process the collection with a recursion call to this method...
                            if (includeSubfolders) resultItemList.AddRange(BuildPKeyList(ftn, dvParamList, dvPKeyName, includeSubfolders));
                        }
                        else if (dvParamList.Contains(ftn.Name.Split('=')[0].Trim().ToLower()))
                        {
                            // This is a single item with a 'type' that is directly supported by the dataview - so add it to the list...
                            string[] keyValuePair = ftn.Name.Split('=');
                            resultItemList.Add(new KeyValuePair<string, int>(keyValuePair[0].Trim(), int.Parse(keyValuePair[1])));
                        }
                        else
                        {
                            // This is a single item with a 'type' that is directly supported by the dataview - so we need to go
                            // get the original @table.field query string...
//string queryString = ftn.Tag.ToString().Split(';')[2].Trim();
                            if (ftn.Tag.ToString().Split(';')[0].Trim().ToUpper() == "QUERY")
                            {
                                // This is a query obect - pass it directly to the search engine to convert it to a collection of dataview supported item 'type'...
string queryString = GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", treeNode, false, "");
string resolveTo = GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", treeNode, false, "default");
                                resultItemList.AddRange(ResolveItemList(queryString, resolveTo, dvParamList, dvPKeyName, int.Parse(ux_numericupdownMaxRowsReturned.Value.ToString())));
                            }
                            else
                            {
                                // This is a single item with a 'type' that is NOT supported by the dataview - add this one to the Dictionary collection (to process later)...
//string[] keyValuePair = queryString.Split('=');
string[] keyValuePair = ftn.Tag.ToString().Split(';')[2].Trim().Split('=');
                                if (searchItemList.ContainsKey(keyValuePair[0].Trim()))
                                {
                                    searchItemList[keyValuePair[0].Trim()] = searchItemList[keyValuePair[0].Trim()] + "," + keyValuePair[1].Trim();
                                }
                                else
                                {
                                    searchItemList.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());
                                }
                            }
                        }
                    }
                }
                else
                {
                    string[] keyValuePair = treeNode.Name.Split('=');
                    if (dvParamList.Contains(keyValuePair[0].Trim().ToLower()))
                    {
                        resultItemList.Add(new KeyValuePair<string, int>(keyValuePair[0], int.Parse(keyValuePair[1])));
                    }
                    else
                    {
                        if (treeNode.Tag != null)
                        {
                            string[] properties = treeNode.Tag.ToString().Split(';');
string resolveTo = GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", treeNode.Parent, false, "default");
                            resultItemList.AddRange(ResolveItemList(properties[2], resolveTo, dvParamList, dvPKeyName, int.Parse(ux_numericupdownMaxRowsReturned.Value.ToString())));
                        }
                    }
                }
            }
            // Process any query collections gathered in the search dictionary...
            foreach (string key in searchItemList.Keys)
            {
                string queryString = key + " in (" + searchItemList[key] + ")";
string resolveTo = GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", treeNode.Parent, false, "default");
                resultItemList.AddRange(ResolveItemList(queryString, resolveTo, dvParamList, dvPKeyName, int.Parse(ux_numericupdownMaxRowsReturned.Value.ToString())));
            }
            return resultItemList;
        }

        private List<KeyValuePair<string, int>> ResolveItemList(string query, string resolveTo, string dvParamList, string dvPKeyName, int limit)
        {
            List<KeyValuePair<string, int>> resultItemList = new List<KeyValuePair<string, int>>();
            DataSet dsResolvedItems = new DataSet();
            string searchResultType = "inventory";
            string searchParameter = ":inventoryid";

            if (string.IsNullOrEmpty(resolveTo) ||
                resolveTo.Trim().ToLower() == "default")
            {
                //if (dvParamList.Contains(":accessionid"))
                //{
                //    searchResultType = "accession";
                //    searchParameter = ":accessionid";
                //}
                //else if (dvParamList.Contains(":inventoryid"))
                //{
                //    searchResultType = "inventory";
                //    searchParameter = ":inventoryid";
                //}
                //else if (dvParamList.Contains(":orderrequestid"))
                //{
                //    searchResultType = "order_request";
                //    searchParameter = ":orderrequestid";
                //}
                //else if (dvParamList.Contains(":cooperatorid"))
                //{
                //    searchResultType = "cooperator";
                //    searchParameter = ":cooperatorid";
                //}
//DataTable dt = null;
////if (ux_datagridviewMain.DataSource.GetType() == typeof(BindingSource) &&
////    ((BindingSource)ux_datagridviewMain.DataSource).DataSource.GetType() == typeof(DataTable))
////{
////    dt = (DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource;
////}
////else if (ux_datagridviewMain.DataSource.GetType() == typeof(DataTable))
////{
////    dt = (DataTable)ux_datagridviewMain.DataSource;
////}
//if (ux_tabcontrolCTDataviews.SelectedTab.Tag != null)
//{
//    string dataviewName = ((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName;
//    // We need to know the pkey for the currently selected dataview tab is so go get zero rows for that dataview 
//    // and inspect the result to learn the pkey name...
//    DataSet dsTemp = GetDGVData(dataviewName, new List<KeyValuePair<string,int>>(), dvParamList, Convert.ToInt32(ux_numericupdownQueryPageSize.Value));
//    dt = dsTemp.Tables[dataviewName];
//}
//if (dt != null && dt.PrimaryKey.Length == 1)
//{
//    string pkeyColumnName = dt.PrimaryKey[0].ColumnName.Trim().ToLower();
//    searchResultType = pkeyColumnName.Remove(pkeyColumnName.Length - 3);
//    searchParameter = ":" + pkeyColumnName.Replace("_", "");
//}dvPKeyName
                if (!string.IsNullOrEmpty(dvPKeyName))
                {
                    searchResultType = dvPKeyName.Remove(dvPKeyName.Length - 3);
                    searchParameter = ":" + dvPKeyName.Replace("_", "");
                }
            }
            else
            {
                searchResultType = resolveTo.Trim().ToLower();
                searchParameter = ":" + searchResultType.Replace("_", "") + "id";
            }

            // Call the search engine...
            dsResolvedItems = _sharedUtils.SearchWebService(query, true, true, "", searchResultType, 0, limit * 10);

            // Build the list from the search engine results...
            if (dsResolvedItems != null &&
                dsResolvedItems.Tables.Contains("SearchResult") &&
                dsResolvedItems.Tables["SearchResult"].Rows.Count > 0)
            {
                foreach (DataRow dr in dsResolvedItems.Tables["SearchResult"].Rows)
                {
                    resultItemList.Add(new KeyValuePair<string, int>(searchParameter, int.Parse(dr["ID"].ToString())));
                }
            }

            return resultItemList;
        }

        private ImageList BuildTabControlImageList()
        {
            if (_tabControlImageList == null)
            {
                ImageList tabControlImageList = new ImageList();

                // Load the images for the tree view(s)...
                tabControlImageList.ColorDepth = ColorDepth.Depth32Bit;
                tabControlImageList.Images.Add("new_tab", Icon.ExtractAssociatedIcon(@"Images\TabControl\GG_newtab.ico"));

                // Cache the list so you don't have to load it again...
                _tabControlImageList = tabControlImageList;
            }
            
            return _tabControlImageList;
        }

        private ImageList BuildTreeviewImageList()
        {
            if (_treeviewImageList == null)
            {
                ImageList treeviewImageList = new ImageList();

                // Load the images for the tree view(s)...
                treeviewImageList.ColorDepth = ColorDepth.Depth32Bit;
                //treeviewImageList.Images.Add("active_unknown", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_UNKNOWN.ico"));
                //treeviewImageList.Images.Add("inactive_unknown", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_UNKNOWN.ico"));
                //treeviewImageList.Images.Add("active_folder", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_Folder.ico"));
                //treeviewImageList.Images.Add("inactive_folder", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_Folder.ico"));
                //treeviewImageList.Images.Add("active_INVENTORY_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_INVENTORY_ID.ico"));
                //treeviewImageList.Images.Add("inactive_INVENTORY_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_INVENTORY_ID.ico"));
                //treeviewImageList.Images.Add("active_ACCESSION_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_ACCESSION_ID.ico"));
                //treeviewImageList.Images.Add("inactive_ACCESSION_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_ACCESSION_ID.ico"));
                //treeviewImageList.Images.Add("active_ORDER_REQUEST_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_ORDER_REQUEST_ID.ico"));
                //treeviewImageList.Images.Add("inactive_ORDER_REQUEST_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_ORDER_REQUEST_ID.ico"));
                //treeviewImageList.Images.Add("active_COOPERATOR_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_COOPERATOR_ID.ico"));
                //treeviewImageList.Images.Add("inactive_COOPERATOR_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_COOPERATOR_ID.ico"));

                //treeviewImageList.Images.Add("active_GEOGRAPHY_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_GEOGRAPHY_ID.ico"));
                //treeviewImageList.Images.Add("inactive_GEOGRAPHY_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_GEOGRAPHY_ID.ico"));
                //treeviewImageList.Images.Add("active_TAXONOMY_GENUS_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_TAXONOMY_GENUS_ID.ico"));
                //treeviewImageList.Images.Add("inactive_TAXONOMY_GENUS_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_TAXONOMY_GENUS_ID.ico"));
                //treeviewImageList.Images.Add("active_CROP_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_CROP_ID.ico"));
                //treeviewImageList.Images.Add("inactive_CROP_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_CROP_ID.ico"));
                //treeviewImageList.Images.Add("active_CROP_TRAIT_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\active_CROP_TRAIT_ID.ico"));
                //treeviewImageList.Images.Add("inactive_CROP_TRAIT_ID", Icon.ExtractAssociatedIcon(@"Images\TreeView\inactive_CROP_TRAIT_ID.ico"));

                // Load the icons from the image sub-directrory (this directory is found under the directory the Curator Tool was launched from)...
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                System.IO.FileInfo[] iconFiles = di.GetFiles("Images\\TreeView\\*.ico", System.IO.SearchOption.TopDirectoryOnly);
                if (iconFiles != null && iconFiles.Length > 0)
                {
                    for (int i = 0; i < iconFiles.Length; i++)
                    {
                        //System.Reflection.Assembly newAssembly = System.Reflection.Assembly.LoadFile(iconFiles[i].FullName);
                        string junk = iconFiles[i].Name.TrimEnd('.', 'i', 'c', 'o');
                        treeviewImageList.Images.Add(iconFiles[i].Name.TrimEnd('.', 'i', 'c', 'o'), Icon.ExtractAssociatedIcon(iconFiles[i].FullName));
                    }
                }

                // Cache the list so you don't have to load it again...
                _treeviewImageList = treeviewImageList;
            }

            return _treeviewImageList;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
          
// Resetting these two global variables will force a refresh of the DGV data (just to be safe)...
lastFullPath = "";
lastTabName = "";

////SetAllUserSettings();
//SetGroupListNavigatorUserSettings();
SetAllUserSettings();
// Refresh the data view...
RefreshMainDGVData();
RefreshMainDGVFormatting();
//ux_datagridviewMain.CurrentCell = null;
((TreeView)sender).Focus();

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            if (e.Node.Nodes.Count == 1 &&
                e.Node.Nodes[0].Name == "!!DUMMYNODE!!")
            {
                string[] nodeTag = e.Node.Nodes["!!DUMMYNODE!!"].Tag.ToString().Split(',');

                if (nodeTag.Length == 2)
                {
                    e.Node.Nodes["!!DUMMYNODE!!"].Remove();
                    DataSet ds = _sharedUtils.GetWebServiceData(nodeTag[0], nodeTag[1], 0, 0);
                    if (ds.Tables.Contains(nodeTag[0]))
                    {
                        foreach (DataRow dr in ds.Tables[nodeTag[0]].Rows)
                        {
                            TreeNode newNode = new TreeNode();

                            string dataviewPKeyName = dr.Table.PrimaryKey[0].ColumnName.Trim();
                            string tablePKeyName = dr.Table.PrimaryKey[0].ExtendedProperties["table_field_name"].ToString().Trim();
                            string tableName = dr.Table.PrimaryKey[0].ExtendedProperties["table_name"].ToString().Trim();
                            string properties = dataviewPKeyName.Trim().ToUpper();
                            properties += ";:" + tablePKeyName.Replace("_", "") + "=" + dr[dr.Table.PrimaryKey[0]].ToString();
                            properties += ";@" + tableName + "." + tablePKeyName + "=" + dr[dataviewPKeyName].ToString();
                            //newNode.Name = ":" + tablePKeyName.Replace("_", "") + "=" + dr[dr.Table.PrimaryKey[0]].ToString();
                            newNode.Name = properties.Split(';')[1].Trim();
                            newNode.Text = newNode.Name;
                            newNode.Tag = properties;
                            newNode.ImageKey = "inactive_" + tablePKeyName.ToUpper();
                            newNode.SelectedImageKey = "active_" + tablePKeyName.ToUpper();

                            e.Node.Nodes.Add(newNode);
                        }
                    }
                }

                // Refresh the title names for the new virtual nodes
                RefreshTreeviewNodeFormatting(e.Node);
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null && e.Label.Length == 0)
            {
                // Zero length treeview node names are not allowed so cancel the label edit action, inform the user...
                e.CancelEdit = true;
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the list name cannot be blank", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "treeView_AfterLabelEditMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                ggMessageBox.ShowDialog();
            }
            else if (e.Label != null)
            {
                if (e.Node.Parent != null && e.Node.Parent.Nodes.ContainsKey(e.Label))
                {
                    // Duplicate node names are not allowed
                    e.CancelEdit = true;
                    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the list name cannot be a duplicate.\n\nThere is already an item named '{0}' in the folder '{1}'.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "treeView_AfterLabelEditMessage2";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, e.Label, e.Node.Parent.Text);
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, e.Label);
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, e.Node.Parent.Text);
//}
string[] argsArray = new string[100];
argsArray[0] = e.Label;
argsArray[1] = e.Node.Parent.Text;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                    ggMessageBox.ShowDialog();
                }
                else
                {
                    // Remember the original text for the node (in case the nodes new name exceeds max path length)
                    string origNodeText = e.Node.Text;
                    // Set the new label...
                    e.Node.Text = e.Label;
                    if (!PathLengthOK(e.Node))
                    {
                        e.Node.Text = origNodeText;
                        e.CancelEdit = true;
                        GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                        ggMessageBox.Name = "treeView_AfterLabelEditMessage3";
                        _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, e.Node.FullPath);
string[] argsArray = new string[100];
argsArray[0] = e.Node.FullPath;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                        ggMessageBox.ShowDialog();
                    }

                    // Disable the treeview property that allows label edits...
                    ((TreeView)sender).LabelEdit = false;
                }
            }
        }

        private void sortFolder(TreeNode tnFolder, string sortMode)
        {
            // Create a new temp treeview to sort the selected rows
            // because TreeView.Sort() works on all nodes (not just the selected nodes)...
            TreeView tvSortAscending = new TreeView();
            // Make a copy of the nodes to be sorted and add them to the temp treeview...
            tvSortAscending.Nodes.Add((TreeNode)tnFolder.Clone());
            // Bind the sorting IComparer class that corresponds to the sort direction desired
            // NOTE: this will also initiate the sorting on the treeview...
            if (sortMode.Trim().ToUpper() == "DESCENDING")
            {
                // Do descending sort only when requested...
                tvSortAscending.TreeViewNodeSorter = new NodeSortDescending();
            }
            else
            {
                // Do ascending sort whenever descending sort is not requested...
                tvSortAscending.TreeViewNodeSorter = new NodeSortAscending();
            }
            // Make a copy of the sorted nodes and add it to the nodes parent (either another node or the treeview itself)
            TreeNode sortedFolder = (TreeNode)tvSortAscending.Nodes[0].Clone();
            if (tnFolder.Parent != null)
            {
                tnFolder.Parent.Nodes.Insert(tnFolder.Index, sortedFolder);
            }
            else
            {
                tnFolder.TreeView.Nodes.Add(sortedFolder);
            }
            // Cleanup - remove the original unsorted node and make the new node selected and expanded...
            tnFolder.Remove();
            sortedFolder.TreeView.SelectedNode = sortedFolder;
            sortedFolder.TreeView.SelectedNode.Expand();
        }

        private bool isFolder(TreeNode tn)
        {
            bool nodeIsAFolder = false;
            if (tn != null &&
                tn.Tag != null &&
                tn.Tag.ToString().Split(';').Length > 0 &&
                (tn.Tag.ToString().Split(';')[0].Trim().ToUpper() == "FOLDER" ||
                 tn.Tag.ToString().Split(';')[0].Trim().ToUpper() == "QUERY"))
            {
                nodeIsAFolder = true;
            }

            return nodeIsAFolder;
        }

        private string EnsureUniqueNodeText(TreeNode destinationTreeNode, TreeNode newNode)
        {
            // Now we can get a unique name for this tree node (starting with the default node name passed in)...
            String uniqueNodeText = newNode.Text;

            // Let's make sure the new node name is unique..
            bool duplicateText = true;
            int i = 1;
            while (duplicateText)
            {
                // Assume this name is unique (until proven otherwise)
                duplicateText = false;
                foreach (TreeNode tn in destinationTreeNode.Nodes)
                {
                    if (tn.Text == uniqueNodeText &&
                        tn != newNode)
                    {
                        // Nope...  This is not a unique node name so increment a counter until it is unique...
                        uniqueNodeText = newNode.Text + " (" + i++.ToString() + ")";
                        duplicateText = true;
                    }
                }
            }
            return uniqueNodeText;
        }

        private bool PathLengthOK(TreeNode tnNew)
        {
            bool pathLengthOK = false;

            // First check the lenght of the top node...
            if (tnNew.TreeView != null &&
                tnNew.FullPath.Length < _maxPathLength)
            {
                pathLengthOK = true;
            }
            else
            {
                if (tnNew.Tag != null &&
                    isFolder(tnNew))
                {
                    pathLengthOK = false;
                }
                else
                {
                    pathLengthOK = true;
                }
            }
            // Now check each child node...
            if (pathLengthOK)
            {
                foreach (TreeNode tn in tnNew.Nodes)
                {
                    pathLengthOK = PathLengthOK(tn);
                }
            }
            return pathLengthOK;
        }

        private void ResetTreeviewNodeFormatting(TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode tn in treeNodeCollection)
            {
                tn.BackColor = Color.Empty;
                tn.NodeFont = null;
                ResetTreeviewNodeFormatting(tn.Nodes);
            }
        }

        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            // This code below is to make the node that was right clicked the selected node
            // so that when a context menu item is selected the action will be performed on 
            // the correct node...
            if (e.Button == MouseButtons.Right)
            {
                if (((TreeView)sender).GetNodeAt(e.X, e.Y) != null)
                {
                    ((TreeView)sender).SelectedNode = ((TreeView)sender).GetNodeAt(e.X, e.Y);
                }

                if (((TreeView)sender).SelectedNode.Name == "{DBNull.Value}")
                {
                    //ux_treeviewmenuNewList.Enabled = true;
                    //ux_treeviewmenuClearList.Enabled = true;
                    //ux_treeviewmenuListSortAscending.Enabled = true;
                    //ux_treeviewmenuListSortDescending.Enabled = true;
                    //ux_treeviewmenuRenameList.Enabled = true;
                    //ux_treeviewmenuCutList.Enabled = false;
                    //ux_treeviewmenuCopyList.Enabled = true;
                    //ux_treeviewmenuPasteList.Enabled = true;
                    //ux_treeviewmenuDeleteList.Enabled = true;
                    //ux_treeviewmenuListProperties.Enabled = true;
                }
                else
                {
                    //ux_treeviewmenuNewList.Enabled = true;
                    //ux_treeviewmenuClearList.Enabled = true;
                    //ux_treeviewmenuListSortAscending.Enabled = true;
                    //ux_treeviewmenuListSortDescending.Enabled = true;
                    //ux_treeviewmenuRenameList.Enabled = true;
                    //ux_treeviewmenuCutList.Enabled = true;
                    //ux_treeviewmenuCopyList.Enabled = true;
                    //ux_treeviewmenuPasteList.Enabled = true;
                    //ux_treeviewmenuDeleteList.Enabled = true;
                    //ux_treeviewmenuListProperties.Enabled = true;
                }

                if (((TreeView)sender).SelectedNode.Tag != null &&
                    //((TreeView)sender).SelectedNode.Tag.ToString().ToUpper() == "FOLDER")
                    isFolder(((TreeView)sender).SelectedNode))
                {
                    //ux_treeviewmenuNewList.Enabled = true;
                    //ux_treeviewmenuClearList.Enabled = true;
                    //ux_treeviewmenuPasteList.Enabled = true;
                }
                else
                {
                    //ux_treeviewmenuNewList.Enabled = false;
                    //ux_treeviewmenuClearList.Enabled = false;
                    //ux_treeviewmenuPasteList.Enabled = false;
                }
            }
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // A treenode is being dragged - time to start a DragDrop operation...
            DataObject dndData = new DataObject();
            // First add the treenode to the data to be sent...
            dndData.SetData((TreeNode)e.Item);
            // Now begin the drag and drop operation...
            ((TreeView)sender).DoDragDrop((TreeNode)e.Item, DragDropEffects.Copy | DragDropEffects.Move);
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            // Okay we are in the middle of a Drag and Drop operation and the mouse is in 
            // the treeview control so lets handle this event...


            // This code will change the cursor icon to give the user feedback about whether or not
            // the drag-drop operation is allowed (in this case it is only allowed for plain text)...
            //
            //TreeView tv = (TreeView)sender;
            //if (tv.Tag == typeof(Point[]))
            //{
            //    Point[] insertbarPoints = new Point[2];
            //    insertbarPoints[0] = ptTreeNodeInsertLineStart;
            //    insertbarPoints[1] = ptTreeNodeInsertLineStop;
            //    tv.Tag = insertbarPoints;
            //}
            //else
            //{
            //    ptTreeNodeInsertLineStart = ((Point[])tv.Tag)[0];
            //    ptTreeNodeInsertLineStop = ((Point[])tv.Tag)[1];
            //}
            // Convert the mouse coordinates from screen to client...
            Point ptClientCoord = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            // Get the node closest to the mouse cursor (to make sure it is a folder)...
            TreeNode tnClosestToMouse = ((TreeView)sender).GetNodeAt(ptClientCoord);

            if (tnClosestToMouse != null)
            {
                tnClosestToMouse.EnsureVisible();
                // If the mouse is within 1 node of the top attempt to scroll up...
                if (ptClientCoord.Y < tnClosestToMouse.TreeView.ItemHeight)
                {
                    TreeNode pn = tnClosestToMouse.PrevNode;
                    if (pn != null)
                    {
                        pn.EnsureVisible();
                    }
                    else
                    {
                        pn = tnClosestToMouse.Parent;
                        if (pn != null) pn.EnsureVisible();
                    }
                }
                // If the mouse is within 1 node of the bottom attempt to scroll down...
                else if (ptClientCoord.Y > (tnClosestToMouse.TreeView.Height - tnClosestToMouse.TreeView.ItemHeight))
                {
                    TreeNode nn = tnClosestToMouse.NextNode;
                    if (nn != null) nn.EnsureVisible();
                }

                // Is this a collection of dataset rows being dragged to a node...
//if (e.Data.GetDataPresent(typeof(DataSet)))
if (e.Data.GetDataPresent("System.Data.DataSet"))
                {
                    //if (tnClosestToMouse.Tag.ToString().ToUpper() == "FOLDER" &&
                    if (isFolder(tnClosestToMouse) &&
                        tnClosestToMouse.Parent != null)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                // Is this a treeview node being dragged to a new location...
//else if (e.Data.GetDataPresent(typeof(TreeNode)))
else if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode"))
                {
//TreeNode tn = (TreeNode)e.Data.GetData(typeof(TreeNode));
TreeNode tn = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                    // Don't allow items to be dropped on the root folder...
                    //if (tn.Tag.ToString().Trim().ToUpper() != "FOLDER" &&
                    if (!isFolder(tn) &&
                        tnClosestToMouse.Parent == null)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    else if ((e.KeyState & 8) == 8) // Check to see if the CTRL key is down.  (4 = Shift, 8 = CTRL, 32 = ALT, 8 + 32 = CTRL + ALT, etc...)
                    {
                        // Ctrl key is down - so do a copy...
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        // Ctrl key is not down - so do a move...
                        e.Effect = DragDropEffects.Move;
                    }
                    // Draw the insertion line between nodes if 
                    if (ptClientCoord.Y > tnClosestToMouse.Bounds.Bottom - 5 &&
                        ptClientCoord.Y < tnClosestToMouse.Bounds.Bottom + 5)
                    {
                        Graphics g = tnClosestToMouse.TreeView.CreateGraphics();
                        Pen eraser = new Pen(tnClosestToMouse.TreeView.BackColor, 2.0f);
                        Pen insertLine = new Pen(Color.Black, 2.0f);
                        g.DrawLine(eraser, _ptTreeNodeInsertLineStart, _ptTreeNodeInsertLineStop);
                        _ptTreeNodeInsertLineStart = new Point(tnClosestToMouse.Bounds.Left, tnClosestToMouse.Bounds.Bottom);
                        _ptTreeNodeInsertLineStop = new Point(tnClosestToMouse.Bounds.Right, tnClosestToMouse.Bounds.Bottom);
                        g.DrawLine(insertLine, _ptTreeNodeInsertLineStart, _ptTreeNodeInsertLineStop);
                        if (tnClosestToMouse.PrevVisibleNode != null) tnClosestToMouse.PrevVisibleNode.BackColor = Color.Empty;
                        tnClosestToMouse.BackColor = Color.Empty;
                        if (tnClosestToMouse.NextVisibleNode != null) tnClosestToMouse.NextVisibleNode.BackColor = Color.Empty;
                    }
                    else
                    {
                        Graphics g = tnClosestToMouse.TreeView.CreateGraphics();
                        Pen eraser = new Pen(tnClosestToMouse.TreeView.BackColor, 2.0f);
                        g.DrawLine(eraser, _ptTreeNodeInsertLineStart, _ptTreeNodeInsertLineStop);
                        if (tnClosestToMouse.PrevVisibleNode != null) tnClosestToMouse.PrevVisibleNode.BackColor = Color.Empty;
                        //if (tnClosestToMouse.Tag.ToString().Trim().ToUpper() == "FOLDER") tnClosestToMouse.BackColor = Color.LightGreen;
                        if (isFolder(tnClosestToMouse)) tnClosestToMouse.BackColor = Color.LightGreen;
                        if (tnClosestToMouse.NextVisibleNode != null) tnClosestToMouse.NextVisibleNode.BackColor = Color.Empty;
                    }
                }
                // Is this an image being dragged to an inventory or accession object...
                else if (e.Data.GetDataPresent(DataFormats.FileDrop) &&
                    (tnClosestToMouse.Tag.ToString().ToUpper().StartsWith("INVENTORY_ID") || tnClosestToMouse.Tag.ToString().ToUpper().StartsWith("ACCESSION_ID")))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                // Is this a query criteria from the Search Tool...
                else if (e.Data.GetDataPresent(DataFormats.Text))
                {
                    string dndText = (string)e.Data.GetData(DataFormats.Text);
                    if (dndText.StartsWith("Search Tool Query :: "))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }

                // This code will expand/collapse the treenode if the mouse is hovering over a folder object...
                TimeSpan ts = DateTime.Now.Subtract(_dtMouseHoverStartTime);
                if (tnClosestToMouse == _tnMouseHoveringOverNode)
                {
                    if (ts.Seconds > 1)
                    {
                        //if (tnClosestToMouse.Tag.ToString().Trim().ToUpper() == "FOLDER" &&
                        if (isFolder(tnClosestToMouse) &&
                            tnClosestToMouse.Bounds.Bottom > ptClientCoord.Y + 5) tnClosestToMouse.Toggle(); //.Expand();
                        _tnMouseHoveringOverNode = null;
                    }
                }
                else
                {
                    _tnMouseHoveringOverNode = tnClosestToMouse;
                    _dtMouseHoverStartTime = DateTime.Now;
                }
            }
        }

        private void treeView_DragDrop(object sender, DragEventArgs e)
        {
            // The drag-drop event is coming to a close - process this event to handle the dropping of
            // data into the treeview...

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Convert the mouse coordinates from screen to client...
            Point ptClientCoord = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

            // Is this a collection of dataset rows being dragged to a node...
//if (e.Data.GetDataPresent(typeof(DataSet)) && e.Effect != DragDropEffects.None)
if (e.Data.GetDataPresent("System.Data.DataSet") && e.Effect != DragDropEffects.None)
            {
//DataSet dndData = (DataSet)e.Data.GetData(typeof(DataSet));
DataSet dndData = (DataSet)e.Data.GetData("System.Data.DataSet");

                // Set this treenode to a default of the currently selected node (just in 
                // case the hit test on the mouse coordinates does not land on a node)...
                TreeNode tnNodeClosestToTheDrop = ((TreeView)sender).SelectedNode;
                if (((TreeView)sender).GetNodeAt(ptClientCoord) != null)
                {
                    tnNodeClosestToTheDrop = ((TreeView)sender).GetNodeAt(ptClientCoord);
                }

                // Add each node based on the info in the datarow...
                foreach (DataRow dr in dndData.Tables[0].Rows)
                {
                    TreeNode newNode = new TreeNode();

                    string dataviewPKeyName = dr.Table.PrimaryKey[0].ColumnName.Trim();
                    string tablePKeyName = dr.Table.PrimaryKey[0].ExtendedProperties["table_field_name"].ToString().Trim();
                    string tableName = dr.Table.PrimaryKey[0].ExtendedProperties["table_name"].ToString().Trim();
                    string properties = dataviewPKeyName.Trim().ToUpper();
                    //properties += ";:" + tablePKeyName + "=" + dr[dr.Table.PrimaryKey[0]].ToString();
                    properties += ";:" + tablePKeyName.Replace("_", "") + "=" + dr[dr.Table.PrimaryKey[0]].ToString();
                    properties += ";@" + tableName + "." + tablePKeyName + "=" + dr[dataviewPKeyName].ToString();
                    newNode.Name = properties.Split(';')[1].Trim();
                    newNode.Text = newNode.Name;
                    newNode.Tag = properties;
                    newNode.ImageKey = "inactive_" + tablePKeyName.ToUpper();
                    newNode.SelectedImageKey = "active_" + tablePKeyName.ToUpper();

                    // Now check to see if virtual nodes are wanted for this new node...
                    if (!string.IsNullOrEmpty(_sharedUtils.GetAppSettingValue(tablePKeyName + "_VIRTUAL_NODE_DATAVIEW")))
                    {
                        TreeNode dummyNode = new TreeNode();
                        string virtualQuery = _sharedUtils.GetAppSettingValue(tablePKeyName + "_VIRTUAL_NODE_DATAVIEW") + ", ";
                        virtualQuery += properties.Split(';')[1].Trim().ToLower();

                        dummyNode.Text = "!!DUMMYNODE!!";
                        dummyNode.Name = "!!DUMMYNODE!!"; ;
                        dummyNode.Tag = virtualQuery;
                        //dummyNode.ImageKey = "inactive_" + virtualQuery;
                        //dummyNode.SelectedImageKey = "active_" + virtualQuery;
                        newNode.Nodes.Add(dummyNode);
                    }

                    tnNodeClosestToTheDrop.Nodes.Add(newNode);
                }
                
                // Give the new nodes their proper titles...
RefreshTreeviewNodeFormatting(tnNodeClosestToTheDrop);

                // Refresh the datagridview...
                tnNodeClosestToTheDrop.TreeView.SelectedNode = null;
                tnNodeClosestToTheDrop.TreeView.SelectedNode = tnNodeClosestToTheDrop;
            }

            // Is this a treeview node being dragged to a new location...
//if (e.Data.GetDataPresent(typeof(TreeNode)) && e.Effect != DragDropEffects.None)
if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode") && e.Effect != DragDropEffects.None)
            {
//TabPage SourceTab = (TabPage)((TreeNode)e.Data.GetData(typeof(TreeNode))).TreeView.Parent;
TabPage SourceTab = (TabPage)((TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode")).TreeView.Parent;
                TabPage DestinationTab = (TabPage)((TreeView)sender).Parent;
//TreeNode SourceNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
TreeNode SourceNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
//TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(((TreeView)sender).PointToClient(new Point(e.X, e.Y)));
TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(ptClientCoord);
                TreeNode newNode = (TreeNode)SourceNode.Clone();

                // If the GetNodeAt method failed try to get the bottom node and check to see if the user dropped below the last node...
                if (DestinationNode == null && ((TreeView)sender).Nodes.Count == 1)
                {
                    int lastNode = ((TreeView)sender).Nodes[0].Nodes.Count-1;
                    if (ptClientCoord.Y > ((TreeView)sender).Nodes[0].Nodes[lastNode].Bounds.Bottom)
                    {
                        DestinationNode = ((TreeView)sender).Nodes[0].Nodes[lastNode];
                    }
                }
                // Bail if the user drops the source back on itself...
                if (SourceNode == null || DestinationNode == null || SourceNode == DestinationNode) return;
                // Do nothing if the destination node is the root node...
                //if (DestinationNode.Tag.ToString().Trim().ToUpper() == "FOLDER" ||
                if (isFolder(DestinationNode) ||
                    DestinationNode.Parent != null)
                {
                    // Make sure the new node has a unique name at the destination node...

                    // If the destination node is a folder and the user is dropping the source node
                    // on the middle of the folder node (not the bottom edge - which indicates the user would
                    // like to insert the source node below the folder (not IN the folder)...
                    //if ((DestinationNode.Tag.ToString().Trim().ToUpper() == "FOLDER" &&
                    if ((isFolder(DestinationNode) &&
                        (DestinationNode.Bounds.Bottom > ptClientCoord.Y + 5) ||
                        SourceNode.Parent == DestinationNode ||
                        DestinationNode.Parent == null))
                    {
                        if (!DestinationNode.Nodes.Contains(SourceNode) || e.Effect == DragDropEffects.Copy)
                        {
                            // If the source node is not an existing node in the destination folder (ie it is not being moved/re-ordered) 
                            // or a copy of the source node is being made - get unique text for the new node...
                            newNode.Text = EnsureUniqueNodeText(DestinationNode, newNode);
                            // Make the nodes Name=Text if this is a folder...
                            //if (SourceNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
                            if (isFolder(SourceNode))
                            {
                                newNode.Name = newNode.Text;
                            }
                        }
                        DestinationNode.Nodes.Insert(0, newNode);
                    }
                    else
                    {
                        // The destination is an item (not a folder) or the user wants to drop the source node
                        // below the destination folder - so insert the new node in the destination node's parent folder (at the top of the list)...
                        if (!DestinationNode.Parent.Nodes.Contains(SourceNode) || e.Effect == DragDropEffects.Copy)
                        {
                            // If the source node is not an existing node in the destination folder (ie it is not being moved/re-ordered) 
                            // or a copy of the source node is being made - get unique text for the new node...
                            newNode.Text = EnsureUniqueNodeText(DestinationNode.Parent, newNode);
                            //if (SourceNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
                            if (isFolder(SourceNode))
                            {
                                newNode.Name = newNode.Text;
                            }
                        }
                        DestinationNode.Parent.Nodes.Insert(DestinationNode.Index + 1, newNode);
                    }

                    // New node is ready to go - now decide to either copy or move it to the destination...
                    if (e.Effect == DragDropEffects.Move)
                    {
                        if (PathLengthOK(newNode))
                        {
                            //DestinationNode.Nodes.Add(newNode);
                            SourceNode.Remove();
                        }
                        else
                        {
                            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                            ggMessageBox.Name = "treeView_DragDropMessage1";
                            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, newNode.FullPath);
string[] argsArray = new string[100];
argsArray[0] = newNode.FullPath;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                            ggMessageBox.ShowDialog();
                            newNode.Remove();
                        }
                    }
                    else if (e.Effect == DragDropEffects.Copy)
                    {
                        if (PathLengthOK(newNode))
                        {
                            ResetTreeviewNodeToolTip(newNode);
                            //DestinationNode.Nodes.Add(newNode);
                        }
                        else
                        {
                            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                            ggMessageBox.Name = "treeView_DragDropMessage2";
                            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, newNode.FullPath);
string[] argsArray = new string[100];
argsArray[0] = newNode.FullPath;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                            ggMessageBox.ShowDialog();
                            newNode.Remove();
                        }
                    }
                }
                // Clear special drag and drop visual highlighting and insert bars...
                ResetTreeviewNodeFormatting(DestinationNode.TreeView.Nodes);
            }

            // Are these image files being dropped on to an accession or inventory node...
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] fullPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                TreeNode destinationNode = ((TreeView)sender).GetNodeAt(((TreeView)sender).PointToClient(new Point(e.X, e.Y)));

                if (destinationNode.Tag.ToString().ToUpper().StartsWith("INVENTORY_ID"))
                {
                    string inventoryID = "";
                    string[] pkeyTokens = destinationNode.Name.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (pkeyTokens != null &&
                        pkeyTokens.Length == 2)
                    {
                        inventoryID = pkeyTokens[1];
                        LoadInventoryImages(fullPaths, inventoryID);
                    }
                }
                else if (destinationNode.Tag.ToString().ToUpper().StartsWith("ACCESSION_ID"))
                {
                    string inventoryIDs = "";
                    //DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=; :accessionid=" + destinationNode.Name + "; :orderrequestid=; :cooperatorid=;", 0, 0);
//DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + destinationNode.Name, 0, 0);
DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", destinationNode.Name, 0, 0);
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
                    if (!string.IsNullOrEmpty(inventoryIDs)) LoadInventoryImages(fullPaths, inventoryIDs);
                }
            }

            // Is this a query criteria from the Search Tool...
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string dndText = (string)e.Data.GetData(DataFormats.Text);
                TreeNode destinationNode = ((TreeView)sender).GetNodeAt(((TreeView)sender).PointToClient(new Point(e.X, e.Y)));
                if (dndText.StartsWith("Search Tool Query :: ") &&
                    isFolder(destinationNode))
                {
//returnedProperties = "QUERY; DYNAMIC_FOLDER_SEARCH_CRITERIA=" + ux_textboxDYNAMIC_FOLDER_SEARCH_CRITERIA.Text + "; ";
                    string newFolderTag = "";
                    string[] currentFolderProperties = destinationNode.Tag.ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] newFolderProperties = new string[currentFolderProperties.Length + 3];
                    string[] dndQueryProperties = dndText.Split(new string[] {" :: "}, StringSplitOptions.RemoveEmptyEntries);

                    // Create new dynamic folder properties for this node...
                    newFolderProperties[0] = "QUERY";
                    newFolderProperties[1] = "DYNAMIC_FOLDER_RESOLVE_TO=" + dndQueryProperties[1].Substring(dndQueryProperties[1].IndexOf("ResolveTo=") + 10);
                    newFolderProperties[2] = "DYNAMIC_FOLDER_SEARCH_CRITERIA=" + dndQueryProperties[2].Substring(dndQueryProperties[2].IndexOf("SearchCriteria=") + 15);
                    int newFolderPropertiesCounter = 3;

                    string currentSearchCriteria = GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", destinationNode, false, "DYNAMIC_FOLDER_SEARCH_CRITERIA_property_is_missing");
                    string currentResolveTo = GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", destinationNode, false, "DYNAMIC_FOLDER_RESOLVE_TO_property_is_missing");

                    // Add back in the existing folder properties that are not associated with the dynamic query...
                    for (int i = 0; i < currentFolderProperties.Length; i++)
                    {
                        if (!currentFolderProperties[i].Trim().ToUpper().StartsWith("FOLDER") &&
                            !currentFolderProperties[i].Trim().ToUpper().StartsWith("QUERY") &&
                            !currentFolderProperties[i].Trim().ToUpper().StartsWith("DYNAMIC_FOLDER_") &&
                            !string.IsNullOrEmpty(currentFolderProperties[i].Trim()))
                        {
                            newFolderProperties[newFolderPropertiesCounter] = currentFolderProperties[i];
                            newFolderPropertiesCounter++;
                        }
                    }

                    // Finally create the new folder properties string to store in the node Tag property...
                    for (int i = 0; i < newFolderProperties.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(newFolderProperties[i]) &&  
                            !string.IsNullOrEmpty(newFolderProperties[i].Trim())) newFolderTag += newFolderProperties[i] + "; ";
                    }
                    //if (folderProperties[0].Trim().ToUpper() == "FOLDER")
                    //{
                    //    folderProperties[0] = "QUERY";
                    //}
                    // Replace the folder's SEARCH_CRITERIA and RESOLVE_TO strings...
                    //if(currentResolveTo == "") folderProperties
//for (int i = 0; i < folderProperties.Length; i++)
//{
//    if (folderProperties[i].Trim().ToUpper().StartsWith("FOLDER")) folderProperties[i] = "QUERY";
//    if (folderProperties[i].Trim().ToUpper().StartsWith("DYNAMIC_FOLDER_SEARCH_CRITERIA")) folderProperties[i] = "DYNAMIC_FOLDER_SEARCH_CRITERIA=" + dndText.Substring(dndText.IndexOf(" :: SearchCriteria=") + 19);
//    if (folderProperties[i].Trim().ToUpper().StartsWith("DYNAMIC_FOLDER_RESOLVE_TO")) folderProperties[i] = "DYNAMIC_FOLDER_RESOLVE_TO=" + dndText.Substring(dndText.IndexOf(" :: SearchCriteria=") + 19);
//}
                //if (currentResolveTo == "DYNAMIC_FOLDER_RESOLVE_TO_property_is_missing")
                    //{
                    //    folderProperties = folderProperties.Replace("QUERY;", "QUERY; DYNAMIC_FOLDER_RESOLVE_TO=" + dndText.Substring(dndText.IndexOf(" :: ResolveTo=") + 14));
                    //}
                    //else if (currentResolveTo == "")
                    //{
                    //    folderProperties = folderProperties.Replace("DYNAMIC_FOLDER_RESOLVE_TO=;", "DYNAMIC_FOLDER_RESOLVE_TO=" + dndText.Substring(dndText.IndexOf(" :: ResolveTo=") + 14));
                    //}
                    //else
                    //{
                    //    folderProperties = folderProperties.Replace("DYNAMIC_FOLDER_RESOLVE_TO=" + currentResolveTo, "DYNAMIC_FOLDER_RESOLVE_TO=" + dndText.Substring(dndText.IndexOf(" :: ResolveTo=") + 14));
                    //}
                    //// Replace the folders search criteria string...
                    //string currentSearchCriteria = GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", destinationNode, false, "DYNAMIC_FOLDER_SEARCH_CRITERIA_property_is_missing");
                    //if (currentSearchCriteria == "DYNAMIC_FOLDER_SEARCH_CRITERIA_property_is_missing")
                    //{
                    //    folderProperties = folderProperties.Replace("QUERY;", "QUERY; DYNAMIC_FOLDER_SEARCH_CRITERIA=" + dndText.Substring(dndText.IndexOf(" :: SearchCriteria=") + 19) + "; DYNAMIC_FOLDER_RESOLVE_TO=;");
                    //}
                    //else if (currentSearchCriteria == "")
                    //{
                    //    folderProperties = folderProperties.Replace("DYNAMIC_FOLDER_SEARCH_CRITERIA=;", "DYNAMIC_FOLDER_SEARCH_CRITERIA=" + dndText.Substring(dndText.IndexOf(" :: SearchCriteria=") + 19));
                    //}
                    //else
                    //{
                    //    folderProperties = folderProperties.Replace("DYNAMIC_FOLDER_SEARCH_CRITERIA=" + currentSearchCriteria, "DYNAMIC_FOLDER_SEARCH_CRITERIA=" + dndText.Substring(dndText.IndexOf(" :: SearchCriteria=") + 19));
                    //}



                    // Update the treeview node...
                    //destinationNode.Tag = folderProperties;
                    destinationNode.Tag = newFolderTag;
                    destinationNode.ImageKey = "inactive_dynamic_folder";
                    destinationNode.SelectedImageKey = "active_dynamic_folder";

                    // If the query changed force a data refresh...
                    if (currentResolveTo != GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", destinationNode, false, "") ||
                        currentSearchCriteria != GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", destinationNode, false, ""))
                    {
                        // Refresh the data and a new copy of the table should be retrieved...
                        // Resetting these two global variables will force a refresh of the DGV data...
                        lastFullPath = "";
                        lastTabName = "";
                        SetAllUserSettings();
                        // Refresh the data view...
                        RefreshMainDGVData();
                    }
                }
            }
            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

    }

    public class NodeSortAscending : IComparer
    {
        // Compare the TreeNode.Text strings (broken up into words so
        // that numbers can be sorted differently than text.  This allows
        // PI 500 to come before PI 1000 in the sort
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            int result = 0;

            // Break the node.text up into tokens
            string[] xTokens = ((TreeNode)x).Text.Split(new char[] { ' ' });
            string[] yTokens = ((TreeNode)y).Text.Split(new char[] { ' ' });

            // Evaluate the tokens as strings or numbers 
            decimal decTokenX = 0;
            decimal decTokenY = 0;
            int i = 0;
            if (xTokens.Length <= yTokens.Length)
            {
                // Since x is shorter iterate through its tokens...
                while(result == 0 && i < xTokens.Length)
                {
                    if (decimal.TryParse(xTokens[i], out decTokenX) &&
                        decimal.TryParse(yTokens[i], out decTokenY))
                    {
                        // Both tokens are numbers so compare as numbers...
                        if (decTokenX == decTokenY) result = 0;
                        else if (decTokenX > decTokenY) result = 1;
                        else result = -1;
                        //result = Convert.ToInt32(decTokenX - decTokenY);
                    }
                    else
                    {
                        // One or both tokens are strings so compare as strings...
                        result = string.Compare(xTokens[i], yTokens[i]);
                    }
                    i++;
                }
                if (result == 0 && (xTokens.Length < yTokens.Length))
                {
                    // This is because we ran out of x tokens before finding a difference 
                    // (but there was one more y token - so y must be 'bigger')
                    return -1;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                // Since y is shorter iterate through its tokens...
                while (result == 0 && i < yTokens.Length)
                {
                    if (decimal.TryParse(xTokens[i], out decTokenX) &&
                        decimal.TryParse(yTokens[i], out decTokenY))
                    {
                        // Both tokens are numbers so compare as numbers...
                        result = Convert.ToInt32(decTokenX - decTokenY);
                    }
                    else
                    {
                        // One or both tokens are strings so compare as strings...
                        result = string.Compare(xTokens[i], yTokens[i]);
                    }
                    i++;
                }
                if (result == 0)
                {
                    // This is because we ran out of y tokens before finding a difference 
                    // (but there was one more x token - so x must be 'bigger')
                    return -1;
                }
                else
                {
                    return result;
                }
            }
        }
    }

    public class NodeSortDescending : IComparer
    {
        // Compare the TreeNode.Text strings (broken up into words so
        // that numbers can be sorted differently than text.  This allows
        // PI 500 to come before PI 1000 in the sort
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            int result = 0;

            // Break the node.text up into tokens
            string[] xTokens = ((TreeNode)x).Text.Split(new char[] { ' ' });
            string[] yTokens = ((TreeNode)y).Text.Split(new char[] { ' ' });

            // Evaluate the tokens as strings or numbers 
            decimal decTokenX = 0;
            decimal decTokenY = 0;
            int i = 0;
            if (xTokens.Length <= yTokens.Length)
            {
                // Since x is shorter iterate through its tokens...
                while (result == 0 && i < xTokens.Length)
                {
                    if (decimal.TryParse(xTokens[i], out decTokenX) &&
                        decimal.TryParse(yTokens[i], out decTokenY))
                    {
                        // Both tokens are numbers so compare as numbers...
                        if (decTokenY == decTokenX) result = 0;
                        else if (decTokenY > decTokenX) result = 1;
                        else result = -1;
                        //result = Convert.ToInt32(decTokenY - decTokenX);
                    }
                    else
                    {
                        // One or both tokens are strings so compare as strings...
                        result = string.Compare(yTokens[i], xTokens[i]);
                    }
                    i++;
                }
                if (result == 0 && (xTokens.Length < yTokens.Length))
                {
                    // This is because we ran out of y tokens before finding a difference 
                    // (but there was one more x token - so it must be 'bigger')
                    return -1;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                // Since y is shorter iterate through its tokens...
                while (result == 0 && i < yTokens.Length)
                {
                    if (decimal.TryParse(xTokens[i], out decTokenX) &&
                        decimal.TryParse(yTokens[i], out decTokenY))
                    {
                        // Both tokens are numbers so compare as numbers...
                        result = Convert.ToInt32(decTokenY - decTokenX);
                    }
                    else
                    {
                        // One or both tokens are strings so compare as strings...
                        result = string.Compare(yTokens[i], xTokens[i]);
                    }
                    i++;
                }
                if (result == 0)
                {
                    // This is because we ran out of y tokens before finding a difference 
                    // (but there was one more x token - so it must be 'bigger')
                    return -1;
                }
                else
                {
                    return result;
                }
            }
        }
    }
}
