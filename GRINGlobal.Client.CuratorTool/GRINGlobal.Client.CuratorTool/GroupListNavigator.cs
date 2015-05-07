using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace GRINGlobal.Client.CuratorTool
{
//    public partial class GRINGlobalClientCuratorTool
//    {
//        DateTime dtMouseHoverStartTime = new DateTime();
//        TreeNode tnMouseHoveringOverNode = new TreeNode();
//        Point ptTreeNodeInsertLineStart = new Point(0, 0);
//        Point ptTreeNodeInsertLineStop = new Point(0, 0);

//        private void LoadCooperators(string localDBInstance)
//        {
//            DataTable cooperatorTable = null;
//            LocalDatabase localDB = new LocalDatabase(localDBInstance);

//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            // If the cooperator_lookup table does not exist in the local DB 
//            // create it using the GetDisplayMember method which will do three things...
//            if (!localDB.TableExists("cooperator_lookup"))
//            {
//                // This will force the lookupTables class to:
//                // 1) retrieve the record for the current cno from the remote DB
//                // 2) create a new cooperator_lookup table on the local DB with one row of data
//                // 3) copy that table to the in memory MRU table...
////lookupTables.GetDisplayMember("cooperator_lookup", cno, "", cno);
//                _sharedUtils.GetLookupDisplayMember("cooperator_lookup", cno, "", cno);

//                // Now we will background thread the cooperator_lookup table loading - because it is really needed
//                // and we can't count on the user to load it manually...
////new System.Threading.Thread(lookupTables.LoadTableFromDatabase).Start("cooperator_lookup");
////new System.Threading.Thread(lookupTables.LoadTableFromDatabase).Start("cooperator");
//                new System.Threading.Thread(_sharedUtils.LookupTablesLoadTableFromDatabase).Start("cooperator_lookup");
//            }
//            else
//            {
//                // Since the cooperator_lookup table exists - make sure the selected cno is one of the rows...
////lookupTables.GetDisplayMember("cooperator_lookup", cno, "", cno);
//                _sharedUtils.GetLookupDisplayMember("cooperator_lookup", cno, "", cno);
//            }
//            // Now that we are sure the cooperator lookup table exists use it...
////cooperatorTable = localDB.GetData("SELECT * FROM cooperator_lookup WHERE is_account_enabled = 'Y'");
//            cooperatorTable = localDB.GetData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", new string[1] {"@accountisenabled=Y"});
//            if (cooperatorTable.Columns.Contains("display_member")) cooperatorTable.DefaultView.Sort = "display_member ASC";
//            if (cooperatorTable.Columns.Contains("site")) cooperatorTable.DefaultView.RowFilter = "site = '" + site + "'";
////if (cooperatorTable.Columns.Contains("site_code")) cooperatorTable.DefaultView.RowFilter = "site_code = '" + site + "'";

//            // Bind the control to the data in grinLookups...
//            // WARNING!!!: You must set DisplayMember and ValueMember properties BEFORE setting 
//            //             DataSource - otherwise the cbCooperators.SelectedValue.ToString() method 
//            //             will return an object of DataRowView instead of the CNO value
//            ux_comboboxCNO.DisplayMember = "display_member";
//            ux_comboboxCNO.ValueMember = "value_member";
//            ux_comboboxCNO.DataSource = cooperatorTable;

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }
 
//        private void ux_comboboxCNO_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            if (ux_comboboxCNO.SelectedIndex != -1)
//            {
//                string tabOrder = "";

//                // Get the user's treeview lists...
//                userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//                // Remember which list the user was looking at...
////userSettings["ux_comboboxCNO", "SelectedIndex"] = ux_comboboxCNO.SelectedIndex.ToString();
//                _sharedUtils.SaveUserSetting("ux_comboboxCNO", "SelectedIndex", ux_comboboxCNO.SelectedIndex.ToString());
//                // If the user is switching back to their own list - go get the tabOrder property...
//                if (cno.ToLower().Trim() == ux_comboboxCNO.SelectedValue.ToString().ToLower().Trim())
//                {
//                    //tabOrder = userSettings[ux_tabcontrolGroupListNavigator.Name, "TabPages.Order"];
//                    tabOrder = _sharedUtils.GetUserSetting(ux_tabcontrolGroupListNavigator.Name, "TabPages.Order", "");

//// This is code to handle legacy user settings - pull this out after RC2...
//if (!tabOrder.Contains(_pathSeparator))
//{
//    tabOrder = "";
//}

//                }
//                else
//                {
//                    tabOrder = "";
//                }
//                // Reset the lastTabName and lastFullPath to empty...
//                lastTabName = "";
//                lastFullPath = "";
//                // Rebuild the Groups Navigator...
//                BuildGroupsNavigator(userItemList, tabOrder);
//            }
//            else
//            {
//                ux_tabcontrolGroupListNavigator.TabPages.Clear();
//            }

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void BuildGroupsNavigator(DataTable groups, string groupOrder)
//        {
//            //TabControl tcGroupsNavigator;
//            int selectedTab = -1;
//            string[] tabOrder;
//            DataTable distinctTABNAME;

//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            // Clear the tabpages and start with a fresh, clean slate...
//            ux_tabcontrolGroupListNavigator.TabPages.Clear();

//            // Now add back in the tabpage for adding new tabpages...
//            if (!ux_tabcontrolGroupListNavigator.TabPages.ContainsKey("ux_tabpageGroupListNavigatorNewTab"))
//            {
//                TabPage tp = new TabPage();
//                tp.Name = "ux_tabpageGroupListNavigatorNewTab";
//                tp.Text = "...";
//                tp.ToolTipText = "New Tab...";
//                ux_tabcontrolGroupListNavigator.TabPages.Add(tp);
//                // For some reason you have to add the tabpage to the tabcontrol *before* setting the imagekey
//                // in order for the binding of the image to occur properly...
//                tp.ImageKey = "new_tab";
//            }

//            // This is a neat trick to get the distinct values from a table...
//            distinctTABNAME = groups.DefaultView.ToTable(true, "TAB_NAME");

//            // Attempt to display only the tabs the user wants shown...
//            if (groupOrder.Length > 0)
//            {
//                // Split the space delimited order list into an array...
//                tabOrder = groupOrder.Split(new string[] { _pathSeparator }, StringSplitOptions.None);
//            }
//            else
//            {
//                // Looks like there is no user preference for tabs to display
//                // so display all tabnames in the groups table...
//                int i = 0;
//                tabOrder = new string[distinctTABNAME.Rows.Count];
//                foreach (DataRow dr in distinctTABNAME.Rows)
//                {
//                    tabOrder[i] = dr["TAB_NAME"].ToString();
//                    i++;
//                }
//            }

//            if (tabOrder.Length > 0)
//            {
//                foreach (string tabName in tabOrder)
//                {
//                    if (!tabName.StartsWith("SPGP_" /*+ strCNO*/))
//                    {
//                        TabPage tp = BuildNavigatorTabPage(tabName);
//                        ux_tabcontrolGroupListNavigator.TabPages.Insert(ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabpageGroupListNavigatorNewTab"), tp);
//                    }
//                }
//            }
//            else
//            {
//                // Looks like the user has no entries in the groups table so create an empty tab page...
//                TabPage tp = BuildNavigatorTabPage("Tab 1");
//                if (tp != null)
//                {
//                    ux_tabcontrolGroupListNavigator.TabPages.Insert(ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabpageGroupListNavigatorNewTab"), tp);
//                }
//            }

//            selectedTab = 0;
////int.TryParse(userSettings["ux_tabcontrolGroupListNavigator", "SelectedIndex"], out selectedTab);
//            int.TryParse(_sharedUtils.GetUserSetting("ux_tabcontrolGroupListNavigator", "SelectedIndex", "0"), out selectedTab);

//            // Select the tab stored in user settings (so that selected tab is not null)
//            if (ux_tabcontrolGroupListNavigator.TabPages.Count > selectedTab)
//            {
//                ux_tabcontrolGroupListNavigator.SelectedIndex = selectedTab;
//            }

//            // Attempt to build the dynamic show tabs menu...
//            // But first delete all of the current items in the Show Tabs menu...
//            ux_navigatormenuShowTabs.DropDownItems.Clear();
//            // Now we will use the list of all of the tab names for this cno and then collect by 
//            // using a neat trick ( in the ToTable method) to get the distinct values fo the tab names (from the GROUPS table)...
//            foreach (DataRow dr in distinctTABNAME.Rows)
//            {
//                if (!dr["TAB_NAME"].ToString().StartsWith("SPGP_" /*+ strCNO*/))
//                {
//                    // Create the new tool strip menu item and bind it to the click event handler...
//                    ToolStripMenuItem tsmi = new ToolStripMenuItem(dr["TAB_NAME"].ToString(), null, tsmiShowTabsItem_Click);
//                    tsmi.Name = dr["TAB_NAME"].ToString();
//                    if (groupOrder.Length == 0 || groupOrder.Contains(dr["TAB_NAME"].ToString())) tsmi.Checked = true;
//                    ux_navigatormenuShowTabs.DropDownItems.Add(tsmi);
//                }
//            }

//            if (ux_navigatormenuShowTabs.DropDownItems.Count == 1)
//            {
//                // We have more than one tab in the ShowTabs list so enable some menu choices...
//                ux_navigatormenuHideTab.Enabled = false;
//                ux_navigatormenuDeleteTab.Enabled = false;
//                ux_navigatormenuShowTabs.Enabled = false;
//            }

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        public TabPage BuildNavigatorTabPage(string tabName)
//        {
//            bool duplicateName = true;
//            string uniqueTabName = "Tab 1";

//            // Scrub the requested tabName of spaces...
////tabName = tabName.Trim().Replace(" ", "");
//            tabName = tabName.Trim();
//            if (tabName.Length == 0) tabName = "Tab 1";
//            uniqueTabName = tabName;

//            int i = 1;
//            TabPage newTabPage = new TabPage();
//            TreeView newTabPageTreeView = new TreeView();

//            while (duplicateName)
//            {
//                duplicateName = false;
//                // First check the visible tabs in the Navigator...
//                foreach (TabPage tp in ux_tabcontrolGroupListNavigator.TabPages)
//                {
//                    if (tp.Text == uniqueTabName)
//                    {
//                        uniqueTabName = tabName + "(" + i++.ToString() + ")";
//                        duplicateName = true;
//                    }
//                }
//                // Now double-check against the (possible) list of visible + hidden tabs in the context menu
//                // (these menu items are not always available so do not count on these being there - but if they are we should check)...
//                foreach (ToolStripMenuItem tsmi in ux_navigatormenuShowTabs.DropDownItems)
//                {
//                    if (tsmi.Text == uniqueTabName && tsmi.Checked == false)
//                    {
//                        uniqueTabName = tabName + "(" + i++.ToString() + ")";
//                        duplicateName = true;
//                    }
//                }
//            }
//            newTabPage.Text = uniqueTabName;
////newTabPage.Name = newTabPage.Text + "Page";
//            newTabPage.Name = uniqueTabName;

//            // Get the rows from the user list associated with this tab (use defaultview to get the proper sort order)...
//            string origRowFilter = userItemList.DefaultView.RowFilter;
//            userItemList.DefaultView.RowFilter = "TAB_NAME='" + newTabPage.Text.Replace("'", "''") + "'";
////DataRow[] drarrTabData = userItemList.Select("TAB_NAME='" + newTabPage.Text.Replace("'", "''") + "'");
//DataTable dtTabData = userItemList.DefaultView.ToTable();
//            userItemList.DefaultView.RowFilter = origRowFilter;

//            TreeView tvNew = new TreeView();
//            tvNew.Name = newTabPage.Name + "TreeView";

//            // Set the location and size of the tree view...
//            tvNew.Location = new Point(0, 0);
//            tvNew.Size = new Size(newTabPage.Size.Width, newTabPage.Size.Height);

//            // Bind a context menu to the tree view...
//            tvNew.ContextMenuStrip = ux_contextmenustripTreeView;

//            // Add the treeview to the tab and populate it...
//            newTabPage.Controls.Add(tvNew);
//            BuildTreeView(tvNew, dtTabData);
////// Processing in the new sorted order... Find the associated ITEM nodes in the collection, remove them
////// and then re-add them back to the collection.  Finally, update the user item list's sort order field with the new sorting order
//////foreach (DataRow dr in drarrTabData)
////foreach (DataRow dr in drarrTabData.Rows)
////{
////    TreeNode[] tnCollection = tvNew.Nodes.Find(dr["ID_NUMBER"].ToString(), false);
////    foreach (TreeNode tn in tnCollection)
////    {
////        if (tn.Tag != null &&
////            tn.Tag.ToString() == dr["ID_TYPE"].ToString())
////        {
////            tn.Remove();
////            tvNew.Nodes.Add(tn);
////        }
////    }
////    dr["SORT_ORDER"] = i++;
////}
//            // Select the active node on the treeview by getting the last node saved in user settings...
//            string selectedNodeFullPath = _sharedUtils.GetUserSetting(tvNew.Name, "SelectedNode.FullPath", "");
//            SelectNodeByPath(selectedNodeFullPath, tvNew);
////TreeNode[] tnc = tvNew.Nodes.Find(selectedNodeFullPath.Substring(selectedNodeFullPath.LastIndexOf(tvNew.PathSeparator) + 1), true);
////foreach (TreeNode tn in tnc)
////{
////    if (tn.FullPath == selectedNodeFullPath)
////    {
////        tvNew.SelectedNode = tn;
////        tn.Expand();
////    }
////}

//            return newTabPage;
//        }

//        //public TabPage BuildTabPage(string tabName)
//        //{
//        //    TabPage newTabPage = new TabPage();

//        //    if (!tabName.StartsWith("SPGP_" /*+ strCNO*/))
//        //    {
//        //        newTabPage.Text = tabName;
//        //        newTabPage.Name = tabName + "Page";
//        //        newTabPage.ContextMenuStrip = ux_contextMenustripNavigator;
//        //        // Build the DataRow array that will be passed to the TreeView builder...
//        //        DataRow[] drarrTabData = userItemList.Select("TAB_NAME='" + tabName + "'");
//        //        TreeView newTreeView = BuildTreeView(newTabPage.Name, drarrTabData);
//        //        // Set the location and size of the tree view...
//        //        newTreeView.Location = new Point(0, 0);
//        //        newTreeView.Size = new Size(newTabPage.Size.Width, newTabPage.Size.Height);
//        //        // Bind a context menu to the tree view...
//        //        newTreeView.ContextMenuStrip = ux_contextmenustripTreeView;
//        //        // Get the selected node from user settings...
//        //        //string selectedNode = GetUserSetting(cno, newTreeView.Name, "SelectedNode", "");
//        //        //if (newTreeView.Nodes.ContainsKey(selectedNode))
//        //        //{
//        //        //    newTreeView.SelectedNode = newTreeView.Nodes[selectedNode];
//        //        //}
//        //        // Select the active node on the treeview by getting the last node saved in user settings...
//        //        string selectedNodeFullPath = userSettings[newTreeView.Name, "SelectedNode.FullPath"];
//        //        TreeNode[] tnc = newTreeView.Nodes.Find(selectedNodeFullPath.Substring(selectedNodeFullPath.LastIndexOf(newTreeView.PathSeparator) + 1), true);
//        //        foreach (TreeNode tn in tnc)
//        //        {
//        //            if (tn.FullPath == selectedNodeFullPath)
//        //            {
//        //                newTreeView.SelectedNode = tn;
//        //                tn.Expand();
//        //            }
//        //        }
//        //        newTabPage.Controls.Add(newTreeView);
//        //    }
//        //    return newTabPage;
//        //}

////        public TreeView BuildTreeView(TreeView tvNew, DataRow[] userListData)
////        {
//////TreeView tvNew = new TreeView();
//////tvNew.Name = strName + "TreeView";
//////tvNew.Name = strName;
////            //tvNew.LabelEdit = true;
////            tvNew.FullRowSelect = true;
////            tvNew.AllowDrop = true;
////            tvNew.PathSeparator = _pathSeparator;
////            tvNew.ImageList = navigatorTreeViewImages;
////            tvNew.ImageKey = "inactive_folder";
////            tvNew.SelectedImageKey = "active_folder";
////            tvNew.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
////            tvNew.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
////            tvNew.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
////            tvNew.MouseClick += new MouseEventHandler(this.treeView_MouseClick);
////            tvNew.ItemDrag += new ItemDragEventHandler(this.treeView_ItemDrag);
////            tvNew.DragOver += new DragEventHandler(this.treeView_DragOver);
////            tvNew.DragDrop += new DragEventHandler(this.treeView_DragDrop);
////            tvNew.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_BeforeExpand);

////            foreach (DataRow dr in userListData)
////            {
////                TreeNode tnLastNode;
////                TreeNode tnNewNode = new TreeNode();

//////tnLastNode = BuildNodes(tvNew.Nodes, dr["LIST_NAME"].ToString(), tvNew.PathSeparator);

//////if (dr.Table.Columns.Contains("TITLE")) tnNewNode.Text = dr["TITLE"].ToString().Trim();
//////if (dr.Table.Columns.Contains("FRIENDLY_NAME")) tnNewNode.Text = dr["FRIENDLY_NAME"].ToString();                
//////tnNewNode.Name = dr["ID_NUMBER"].ToString();
//////tnNewNode.Tag = dr["ID_TYPE"].ToString();
//////tnNewNode.ImageKey = "inactive_" + dr["ID_TYPE"].ToString().Trim();
//////tnNewNode.SelectedImageKey = "active_" + dr["ID_TYPE"].ToString().Trim();
//////tnLastNode.Nodes.Add(tnNewNode);
//////// If this is an accession - auto add the child inventory stub...
//////if (dr["ID_TYPE"].ToString() == "ACCESSION_ID")
//////{
//////    tnNewNode.Nodes.Add(BuildDummyNode("INVENTORY_ID"));
//////}
//////// If this is an order - auto add the child order items stub...
//////if (dr["ID_TYPE"].ToString() == "ORDER_REQUEST_ID")
//////{
//////    tnNewNode.Nodes.Add(BuildDummyNode("ORDER_REQUEST_ITEM_ID"));
//////}

////tnLastNode = BuildFullPath(tvNew, dr["LIST_NAME"].ToString(), tvNew.PathSeparator);
////if(dr["ID_TYPE"].ToString().ToUpper() != "FOLDER") tnLastNode.Nodes.Add(BuildTreeNode(dr));

////            }

//////DataTable distinctFolders = dtIn.DefaultView.ToTable(true, new string[] {"LIST_NAME"});
//////foreach (DataRow dr in distinctFolders.Rows)
//////{
//////    string[] folderNames = dr["LIST_NAME"].ToString().Split(new string[] {tvNew.PathSeparator}, StringSplitOptions.None);
//////    TreeNode folder = tvNew.Nodes[0];
//////    foreach (string folderName in folderNames)
//////    {
//////        if (folder.Nodes.ContainsKey(folderName))
//////        {
//////            folder = folder.Nodes.Find(folderName, false)[0];
//////        }
//////    }
//////    //TreeNode[] junk = tvNew.Nodes.Find(dr["LIST_NAME"].ToString(), true);
//////    //junk[0].Text += " (" + dtIn.Select("LIST_NAME='" + dr["LIST_NAME"].ToString() + "'").Length.ToString() + ")";
//////    //tvNew.Nodes.Find(dr["LIST_NAME"].ToString(), true)[0].Text += " (" + dtIn.Select("LIST_NAME='" + dr["LIST_NAME"].ToString() + "'").Length.ToString() + ")";
//////    folder.Text += folder.Text + " (" + folder.Nodes.Count.ToString() + ")";
//////}

////            // The following code will handle the situation where a tab in the user profile exists with no
////            // data in the app_user_item_list table (so create the first node on the treeview)...
////            if (tvNew.Nodes.Count < 1)
////            {
//////TreeNode tnNewNode = new TreeNode("NewList");
////                TreeNode tnNewNode = new TreeNode();
//////tnNewNode.Name = strName;
//////tnNewNode.Name = GetNextUserListFolderID().ToString(); 
////if (tvNew.Parent != null) tnNewNode.Name = tvNew.Parent.Text;
//////tnNewNode.Text = strName;
////if (tvNew.Parent != null) tnNewNode.Text = tvNew.Parent.Text;
////                tnNewNode.Tag = "FOLDER";
////                tvNew.Nodes.Add(tnNewNode);
////if (tvNew.Parent != null) AddUserItemListRow(tnNewNode);
////            }

////            return tvNew;
////        }

//        public void BuildTreeView(TreeView tvNew, DataTable userListData)
//        {
//            tvNew.FullRowSelect = true;
//            tvNew.AllowDrop = true;
//            tvNew.PathSeparator = _pathSeparator;
//            tvNew.ImageList = navigatorTreeViewImages;
//            tvNew.ImageKey = "inactive_folder";
//            tvNew.SelectedImageKey = "active_folder";
//            tvNew.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
//            tvNew.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
//            tvNew.AfterLabelEdit += new NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
//            tvNew.MouseClick += new MouseEventHandler(this.treeView_MouseClick);
//            tvNew.ItemDrag += new ItemDragEventHandler(this.treeView_ItemDrag);
//            tvNew.DragOver += new DragEventHandler(this.treeView_DragOver);
//            tvNew.DragDrop += new DragEventHandler(this.treeView_DragDrop);
//            tvNew.BeforeExpand += new TreeViewCancelEventHandler(this.treeView_BeforeExpand);

//            // Sort by LIST_NAME and SORT_ORDER columns...
//            userListData.DefaultView.Sort = "LIST_NAME ASC, SORT_ORDER ASC";

//            foreach (DataRow dr in userListData.Rows)
//            {
//                TreeNode tnLastNode;
//                TreeNode tnNewNode = new TreeNode();

////if (dr["LIST_NAME"].ToString() == "{DBNull.Value}") dr["LIST_NAME"] = dr["TITLE"];
//                if (dr["LIST_NAME"].ToString() == "{DBNull.Value}")
//                {
//                    if (tvNew.Parent != null)
//                    {
//                        tnNewNode.Name = "{DBNull.Value}";
//                        tnNewNode.Text = dr["TITLE"].ToString().Trim();
//                        tnNewNode.Tag = "FOLDER";
//                        tvNew.Nodes.Add(tnNewNode);
//                    }
//                    continue;
//                }
////tnLastNode = BuildFullPath(tvNew, dr["LIST_NAME"].ToString(), tvNew.PathSeparator);
//                tnLastNode = FindLastNode(tvNew, dr["LIST_NAME"].ToString(), tvNew.PathSeparator);
//                if (tnLastNode != null)
//                {
//                    if (dr["ID_TYPE"].ToString().ToUpper() != "FOLDER")
//                    {
//                        tnLastNode.Nodes.Add(BuildTreeNode(dr));
//                    }
//                    else
//                    {
//                        //if(!tnLastNode.Nodes.Contains(dr["TITLE"]) tnLastNode.Nodes.Add(
//                        tnLastNode.Nodes.Add(BuildTreeNode(dr));
//                    }
//                }
//            }

//            // The following code will handle the situation where a tab in the user profile exists with no
//            // data in the app_user_item_list table (so create the first node on the treeview)...
//            if (tvNew.Nodes.Count < 1)
//            {
//                if (tvNew.Parent != null)
//                {
//                    // Build the root node...
//                    TreeNode tnNewRootNode = new TreeNode();
//                    tnNewRootNode.Name = "{DBNull.Value}";
//                    tnNewRootNode.Text = tvNew.Parent.Text + " Root Folder";
//                    tnNewRootNode.Tag = "FOLDER";
//                    tvNew.Nodes.Add(tnNewRootNode);
//                    // Build the first list node...
//                    TreeNode tnNewListNode = new TreeNode();
//                    tnNewListNode.Name = tnNewRootNode.FullPath;
//                    tnNewListNode.Text = "New List";
//                    tnNewListNode.Tag = "FOLDER";
//                    tnNewRootNode.Nodes.Add(tnNewListNode);
//                    // Add the new nodes to the user's item list table...
//                    AddUserItemListRow(tnNewRootNode);
//                    AddUserItemListRow(tnNewListNode);
//                    SaveLists(userItemList);
//                    userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//                    // Make the new list node the active node...
//                    tvNew.SelectedNode = tnNewListNode;
//                }
//            }
//        }

//        private TreeNode BuildDummyNode(string NodeType)
//        {
//            TreeNode dummyNode = new TreeNode();
//            dummyNode.Text = "!!DUMMYNODE!!";
//            dummyNode.Name = "!!DUMMYNODE!!"; ;
//            dummyNode.Tag = NodeType;
//            dummyNode.ImageKey = "inactive_" + NodeType;
//            dummyNode.SelectedImageKey = "active_" + NodeType;
//            //tnNewNode.Nodes.Add(dummyNode);
//            //DataRow[] accessionInventory = GetAccessionNodeChildren(dr["ID_NUMBER"].ToString());
//            //foreach (DataRow inventoryRow in accessionInventory)
//            //{
//            //    TreeNode inventoryNode = new TreeNode();
//            //    inventoryNode.Text = GetInventoryNodeFriendlyName(inventoryRow);
//            //    inventoryNode.Name = inventoryRow["inventory_id"].ToString();
//            //    inventoryNode.Tag = "INVENTORY_ID";
//            //    inventoryNode.ImageKey = "inactive_INVENTORY_ID";
//            //    inventoryNode.SelectedImageKey = "active_INVENTORY_ID";
//            //    tnNewNode.Nodes.Add(inventoryNode);
//            //}
//            return dummyNode;
//        }

//        private DataRow[] GetAccessionNodeChildren(string AccessionID)
//        {
//            DataRow[] childrenRows = new DataRow[0];
//            //DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + AccessionID + "; :inventoryid=; :orderrequestid=; :cooperatorid=;", 0, 0);
//            DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + AccessionID, 0, 0);
//            if (ds.Tables.Contains("get_inventory"))
//            {
//                childrenRows = new DataRow[ds.Tables["get_inventory"].Rows.Count];
//                ds.Tables["get_inventory"].Rows.CopyTo(childrenRows, 0);
//            }
//            return childrenRows;
//        }

//        private TreeNode FindLastNode(TreeView tv, string strFullPath, string strPathDelim)
//        {
//            TreeNode tnn = new TreeNode();
//            TreeNodeCollection tnc = tv.Nodes;
//            string[] folderNames = strFullPath.Split(new string[] { strPathDelim }, StringSplitOptions.None);

//            foreach (string folderName in folderNames)
//            {
//                if (tnc.ContainsKey(folderName))
                    
//                {
//                    tnn = tnc[folderName];
//                    tnc = tnn.Nodes;
//                }
//                else if (tnc.ContainsKey("{DBNull.Value}") &&
//                    tnc.Find("{DBNull.Value}", true)[0].Text == folderName)
//                {
//                    tnn = tnc["{DBNull.Value}"];
//                    tnc = tnn.Nodes;
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            return tnn;
//        }

//        private TreeNode BuildFullPath(TreeView tv, string strFullPath, string strPathDelim)
//        {
//            TreeNode tnn = new TreeNode();
//            TreeNodeCollection tnc = tv.Nodes;
//            string[] folderNames = strFullPath.Split(new string[] { strPathDelim }, StringSplitOptions.None);
////if (tv.Nodes.ContainsKey(folderNames[0]))
////{
////    tnn = tv.Nodes[folderNames[0]];
////}
////else
////{
////    tnn.Name = folderNames[0];
////    tnn.Text = folderNames[0];
////    tnn.Tag = "FOLDER";
////    tv.Nodes.Add(tnn);
////}
//            foreach (string folderName in folderNames)
//            {
//                if (!tnc.ContainsKey(folderName) ||
//                    !(tnc.ContainsKey("{DBNull.Value}") && tnc.Find("{DBNull.Value}", true)[0].Text == folderName))
//                {
//                    TreeNode tnNewNode = new TreeNode();
//tnNewNode.Name = folderName;
////tnNewNode.Name = GetNextUserListFolderID().ToString();
//tnNewNode.Text = folderName;
//                    tnNewNode.Tag = "FOLDER";
//                    tnc.Add(tnNewNode);
//                }
//                tnn = tnc[folderName];
//                tnc = tnn.Nodes;
//            }
//            return tnn;
//        }

//        /// <summary>
//        /// Recursively traverse the tree structure to find the node specified in the strFullPath.
//        /// This method will create any nodes in the path that do not exist.
//        /// </summary>
//        /// <param name="tncOrigin"></param>
//        /// <param name="strFullPath"></param>
//        /// <param name="strPathDelim"></param>
//        /// <returns>TreeNode object that is the last node specified in the strFullPath</returns>
//        public TreeNode BuildNodes(TreeNodeCollection tncOrigin, string strFullPath, string strPathDelim)
//        {
//            int intNodeNameLength = strFullPath.Length;
//            // Get the length of the first node in the full path
//            if (strFullPath.Contains(strPathDelim))
//            {
//                intNodeNameLength = strFullPath.IndexOf(strPathDelim);
//            }
//            // Pull the first node name out of the full path
//            string strNodeName = strFullPath.Substring(0, intNodeNameLength);
//            // Check to see if the node is already a child of the parent node
//            // and create it if it does not exist
//            if (!tncOrigin.ContainsKey(strNodeName))
//            {
//                TreeNode tnNewNode = new TreeNode();
//tnNewNode.Name = strNodeName;
////tnNewNode.Name = GetNextUserListFolderID().ToString(); 
//                tnNewNode.Text = strNodeName;
//                tnNewNode.Tag = "FOLDER";
//                tncOrigin.Add(tnNewNode);
//            }
//            // Check to see if the delimiter still exists in the full path - if so, play it again sam...  (one level down of course)
//            if (strFullPath.Contains(strPathDelim))
//            {
//                return BuildNodes(tncOrigin[strNodeName].Nodes, strFullPath.Remove(0, intNodeNameLength + 1), strPathDelim);
//            }
//            return tncOrigin[strNodeName];
//        }

//        private void ux_navigatormenuNewTab_Click(object sender, EventArgs e)
//        {
//            NavigatorTabProperties newTabDialog = new NavigatorTabProperties(_pathSeparator, "_", _sharedUtils);
//newTabDialog.StartPosition = FormStartPosition.CenterParent;
//            if (newTabDialog.ShowDialog(this) == DialogResult.OK)
//            {
//                TabPage tp = BuildNavigatorTabPage(newTabDialog.TabText);
//                if (tp != null)
//                {
//                    if (ux_tabcontrolGroupListNavigator.SelectedIndex > -1)
//                    {
//                        ux_tabcontrolGroupListNavigator.TabPages.Insert(ux_tabcontrolGroupListNavigator.SelectedIndex, tp);
//                    }
//                    else
//                    {
//                        ux_tabcontrolGroupListNavigator.TabPages.Insert(ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabpageGroupListNavigatorNewTab"), tp);
//                    }
//                    // Create the new tool strip menu item and bind it to the click event handler...
//                    ToolStripMenuItem tsmi = new ToolStripMenuItem(tp.Text, null, tsmiShowTabsItem_Click);
//                    tsmi.Name = tp.Text;
//                    tsmi.Checked = true;
//                    ux_navigatormenuShowTabs.DropDownItems.Add(tsmi);

//                    if (ux_navigatormenuShowTabs.DropDownItems.Count > 1)
//                    {
//                        // We have more than one tab in the ShowTabs list so enable some menu choices...
//                        ux_navigatormenuHideTab.Enabled = true;
//                        ux_navigatormenuDeleteTab.Enabled = true;
//                        ux_navigatormenuShowTabs.Enabled = true;
//                    }
//                }
//            }
//        }

//        private void ux_navigatormenuDeleteTab_Click(object sender, EventArgs e)
//        {
//            string tabPageName = ux_tabcontrolGroupListNavigator.SelectedTab.Name;
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)ux_navigatormenuShowTabs.DropDownItems[ux_tabcontrolGroupListNavigator.SelectedTab.Text];
//            if (tabPageName != "ux_tabpageGroupListNavigatorNewTab")
//            {
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Warning!!! \n\n Deleting this tab will remove all of it's lists from the database permanently!", "Delete Tab", MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button2);
//ggMessageBox.Name = "ux_navigatormenuDeleteTabMessage1";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
////if (DialogResult.OK == MessageBox.Show("Warning!!! \n\n Deleting this tab will remove all of it's lists from the database permanently!", "Delete Tab", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
//                if(DialogResult.OK == ggMessageBox.ShowDialog())
//                {
//                    int currentTabIndex = ux_tabcontrolGroupListNavigator.SelectedIndex;
//                    // Select the treeview and delete the nodes from the database...
////string treeviewName = tabPageName + "TreeView";
////TreeView tv = (TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[treeviewName];
////if (tv != null && tv.Nodes.Count > 0) DeleteTreeNodes(tv.Parent.Text, tv.Nodes[0]);
//                    foreach (Control ctrl in ux_tabcontrolGroupListNavigator.SelectedTab.Controls)
//                    {
//                        if (ctrl.GetType() == typeof(TreeView))
//                        {
//                            DeleteTreeNodes(ctrl.Parent.Text, ((TreeView)ctrl).Nodes[0]);
//                        }
//                    }
                    
//                    // Temporarily de-select all tabs before removing the selected tab...
//                    ux_tabcontrolGroupListNavigator.SelectedIndex = -1;
//                    ux_tabcontrolGroupListNavigator.TabPages.RemoveByKey(tabPageName);

//                    // Remove the tool strip menu item from the list...
//                    ux_navigatormenuShowTabs.DropDownItems.Remove(tsmi);

//                    // Activate a new tab...
//                    ux_tabcontrolGroupListNavigator_SelectNewActiveTab(currentTabIndex);

//                    // Save the list to the central database...
//                    if (userItemList.GetChanges() != null)
//                    {
//                        DataSet userItemListSaveResults = SaveLists(userItemList);

//                        // Get a fresh copy of the list items for this cooperator from the central database...
//                        userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//                    }
//                }
//            }
//        }

//        private void ux_navigatormenuHideTab_Click(object sender, EventArgs e)
//        {
//            int currentTabIndex = ux_tabcontrolGroupListNavigator.SelectedIndex;
//            string tabName = ux_tabcontrolGroupListNavigator.SelectedTab.Name;
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)ux_navigatormenuShowTabs.DropDownItems[ux_tabcontrolGroupListNavigator.SelectedTab.Text];
//            if (tabName != "ux_tabpageGroupListNavigatorNewTab")
//            {
//                // Temporarily de-select all tabs before removing the selected tab...
//                ux_tabcontrolGroupListNavigator.SelectedIndex = -1;
//                ux_tabcontrolGroupListNavigator.TabPages.RemoveByKey(tabName);

//                // Uncheck the menu item corresponding to the tab being hidden...
//                if (tsmi != null) tsmi.Checked = false;

//                // Activate a new tab...
//                ux_tabcontrolGroupListNavigator_SelectNewActiveTab(currentTabIndex);
//            }
//        }

//        private void ux_tabcontrolGroupListNavigator_SelectNewActiveTab(int DeactivatedTabIndex)
//        {
//            // Find (or create) a new selected tab - and enable/disable menu items as needed...
//            if (ux_navigatormenuShowTabs.DropDownItems.Count == 1)
//            {
//                // If we are down to one tab in the ShowTabs list - disable some menu choices...
//                ux_navigatormenuHideTab.Enabled = false;
//                ux_navigatormenuDeleteTab.Enabled = false;
//                ux_navigatormenuShowTabs.Enabled = false;
//            }
            
//            if (ux_tabcontrolGroupListNavigator.TabPages.Count > 1)
//            {
//                // Still have some visible tabs so select the tab just to the right of the deleted tab if possible...
//                if (ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabpageGroupListNavigatorNewTab") != DeactivatedTabIndex)
//                {
//                    ux_tabcontrolGroupListNavigator.SelectedTab = ux_tabcontrolGroupListNavigator.TabPages[DeactivatedTabIndex];
//                }
//                else
//                {
//                    ux_tabcontrolGroupListNavigator.SelectedTab = ux_tabcontrolGroupListNavigator.TabPages[DeactivatedTabIndex - 1];
//                }
//            }
//            else if (ux_navigatormenuShowTabs.DropDownItems.Count > 0)
//            {
//                // There are no exposed tabs, but there are some hidden ones so expose the first hidden tab in the list...
//                ((ToolStripMenuItem)ux_navigatormenuShowTabs.DropDownItems[0]).Checked = true;
//                TabPage tp = BuildNavigatorTabPage(ux_navigatormenuShowTabs.DropDownItems[0].Name);
//                ux_tabcontrolGroupListNavigator.TabPages.Insert(ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabpageGroupListNavigatorNewTab"), tp);
//            }
//            else
//            {
//                // Deleted the last visible tab so create a new visible tab...
//                TabPage tp = BuildNavigatorTabPage("Tab 1");
//                if (tp != null)
//                {
//                    ux_tabcontrolGroupListNavigator.TabPages.Insert(ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabpageGroupListNavigatorNewTab"), tp);
//                    // Create the new tool strip menu item and bind it to the click event handler...
//                    ToolStripMenuItem tsmi = new ToolStripMenuItem(tp.Text, null, tsmiShowTabsItem_Click);
//                    tsmi.Name = tp.Text;
//                    tsmi.Checked = true;
//                    ux_navigatormenuShowTabs.DropDownItems.Add(tsmi);
//                }
//            }
//        }

//        private void ux_navigatormenuTabProperties_Click(object sender, EventArgs e)
//        {
//            NavigatorTabProperties ntpDialog = new NavigatorTabProperties(_pathSeparator, "_", _sharedUtils);
//            string currentTabText = ux_tabcontrolGroupListNavigator.SelectedTab.Text;
//            ntpDialog.TabText = currentTabText;
//ntpDialog.StartPosition = FormStartPosition.CenterParent;
//            if (ntpDialog.ShowDialog() == DialogResult.OK)
//            {
//                if (currentTabText != ntpDialog.TabText && ntpDialog.TabText.Length > 0)
//                {
//                    //GRINGlobalWebServices.RenameTab(false, username, password, currentTabText, ntpDialog.TabText, int.Parse(ux_comboboxCNO.SelectedValue.ToString()));
//                    RenameTab(currentTabText, ntpDialog.TabText);
//                    ux_tabcontrolGroupListNavigator.SelectedTab.Text = ntpDialog.TabText;
//                    ux_tabcontrolGroupListNavigator.SelectedTab.Name = ntpDialog.TabText;

//                    // Save the list to the central database...
//                    if (userItemList.GetChanges() != null)
//                    {
//                        DataSet userItemListSaveResults = SaveLists(userItemList);

//                        // Get a fresh copy of the list items for this cooperator from the central database...
//                        userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//                    }
//                }
//            }
//        }

//        private void ux_tabcontrolGroupListNavigator_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (ux_tabcontrolGroupListNavigator.SelectedIndex > -1)
//            {
//                if (ux_tabcontrolGroupListNavigator.SelectedTab.Name == "ux_tabpageGroupListNavigatorNewTab")
//                {
//                    int indexOfNewTab = ux_tabcontrolGroupListNavigator.SelectedIndex;
//                    NavigatorTabProperties newTabDialog = new NavigatorTabProperties(_pathSeparator, "_", _sharedUtils);
//newTabDialog.StartPosition = FormStartPosition.CenterParent;
//                    if (newTabDialog.ShowDialog(this) == DialogResult.OK)
//                    {
//                        TabPage tp = BuildNavigatorTabPage(newTabDialog.TabText);
//                        if (tp != null)
//                        {
//                            ux_tabcontrolGroupListNavigator.TabPages.Insert(indexOfNewTab, tp);
//                            ux_tabcontrolGroupListNavigator.SelectedIndex = indexOfNewTab;

//                            // Create the new tool strip menu item and bind it to the click event handler...
//                            ToolStripMenuItem tsmi = new ToolStripMenuItem(tp.Text, null, tsmiShowTabsItem_Click);
//                            tsmi.Name = tp.Text;
//                            tsmi.Checked = true;
//                            ux_navigatormenuShowTabs.DropDownItems.Add(tsmi);
//                        }

//                        if (ux_navigatormenuShowTabs.DropDownItems.Count > 1)
//                        {
//                            // We have more than one tab in the ShowTabs list so enable some menu choices...
//                            ux_navigatormenuHideTab.Enabled = true;
//                            ux_navigatormenuDeleteTab.Enabled = true;
//                            ux_navigatormenuShowTabs.Enabled = true;
//                        }
//                    }
//                    else
//                    {
//                        ux_tabcontrolGroupListNavigator.DeselectTab(indexOfNewTab);
//                    }
//                }
//                else
//                {
//                    // Now refresh the data views...
//                    RefreshMainDGVData();
//                    RefreshMainDGVFormatting();
//                }
//            }
//        }

//        private void ux_tabcontrolGroupListNavigator_MouseDown(object sender, MouseEventArgs e)
//        {
//            int clickedTabPage = ux_tabcontrolGroupListNavigator.SelectedIndex;

//            // Attempt to find the tabpage that was clicked...
//            for (int i = 0; i < ux_tabcontrolGroupListNavigator.TabPages.Count; i++)
//            {
//                if (ux_tabcontrolGroupListNavigator.GetTabRect(i).Contains(e.Location)) clickedTabPage = i;
//            }

//            if (e.Button == MouseButtons.Left)
//            {
//                // Begin tabpage drag and drop move (if the clicked tab is not 
//                // the "ux_tabpageGroupListNavigatorNewTab" tabpage (which is used to add new dataviews)...
//                if (ux_tabcontrolGroupListNavigator.TabPages[clickedTabPage] != ux_tabcontrolGroupListNavigator.TabPages["ux_tabpageGroupListNavigatorNewTab"])
//                {
//                    ux_tabcontrolGroupListNavigator.DoDragDrop(ux_tabcontrolGroupListNavigator.TabPages[clickedTabPage], DragDropEffects.Move);
//                }
//            }
//            else if (e.Button == MouseButtons.Right)
//            {
//                // Make the right clicked tabpage the selected tabpage for the control...
//                ux_tabcontrolGroupListNavigator.SelectTab(clickedTabPage);
//            }
//        }

//        private void ux_tabcontrolGroupListNavigator_DragOver(object sender, DragEventArgs e)
//        {
//            // Convert the mouse coordinates from screen to client...
//            Point ptClientCoord = ux_tabcontrolGroupListNavigator.PointToClient(new Point(e.X, e.Y));

//            // User is re-ordering a tab...
//            if (e.Data.GetDataPresent(typeof(TabPage)))
//            {
//                int destinationTabPage = ux_tabcontrolGroupListNavigator.TabPages.IndexOf((TabPage)e.Data.GetData(typeof(TabPage)));
//                // Attempt to find the tabpage that is being dragged over...
//                for (int i = 0; i < ux_tabcontrolGroupListNavigator.TabPages.Count; i++)
//                {
//                    if (ux_tabcontrolGroupListNavigator.GetTabRect(i).Contains(ptClientCoord)) destinationTabPage = i;
//                }

//                if (e.Data.GetDataPresent(typeof(TabPage)) &&
//                    ux_tabcontrolGroupListNavigator.TabPages[destinationTabPage] != (TabPage)e.Data.GetData(typeof(TabPage)) /* &&
//                destinationTabPage != ux_tabcontrolGroupListNavigator.TabPages.IndexOfKey("ux_tabcontrolGroupListNavigator")*/
//                                                                                                                                  )
//                {
//                    e.Effect = DragDropEffects.Move;
//                }
//                else
//                {
//                    e.Effect = DragDropEffects.None;
//                }
//            }
//            else if (e.Data.GetDataPresent(typeof(TreeNode)))
//            {
//                TabControl tc = (TabControl)sender;

//                for (int i = 0; i < tc.TabCount; i++)
//                {
//                    if (tc.TabPages[i].Name != "ux_tabpageGroupListNavigatorNewTab" &&
//                        tc.GetTabRect(i).Contains(ptClientCoord)) tc.SelectTab(i);
//                }
//            }
//        }

//        private void ux_tabcontrolGroupListNavigator_DragDrop(object sender, DragEventArgs e)
//        {
//            if (e.AllowedEffect == e.Effect)
//            {
//                // Convert the mouse coordinates from screen to client...
//                Point ptClientCoord = ux_tabcontrolGroupListNavigator.PointToClient(new Point(e.X, e.Y));

//                int destinationTabPageIndex = -1;
//                int originalTabPageIndex = -1;

//                // Attempt to find where the tabpage should be dropped...
//                for (int i = 0; i < ux_tabcontrolGroupListNavigator.TabPages.Count; i++)
//                {
//                    if (ux_tabcontrolGroupListNavigator.TabPages[i] == e.Data.GetData(typeof(TabPage))) originalTabPageIndex = i;
//                    if (ux_tabcontrolGroupListNavigator.GetTabRect(i).Contains(ptClientCoord)) destinationTabPageIndex = i;
//                }

//                // Now create a copy of the tabpage that is being moved so that 
//                // you can remove the orginal and insert the copy at the right spot...
//                TabPage newTabPage = new TabPage();
//                newTabPage.Text = ((TabPage)e.Data.GetData(typeof(TabPage))).Text.Trim();
//                newTabPage.Name = ((TabPage)e.Data.GetData(typeof(TabPage))).Name.Trim();// newTabPage.Text + "Page";
////// Create a pointer to the existing tree view...
////TreeView tv = (TreeView)((TabPage)e.Data.GetData(typeof(TabPage))).Controls.Find(newTabPage.Name + "TreeView", true)[0];
//                // Find the treeview for this tab page so that it can be added to the new tab page...
//                if (e.Data.GetDataPresent(typeof(TabPage)))
//                {
//                    TreeView tv = null;
//                    foreach (Control ctrl in ((TabPage)e.Data.GetData(typeof(TabPage))).Controls)
//                    {
//                        if (ctrl.GetType() == typeof(TreeView)) tv = (TreeView)ctrl;
//                    }

//                    // If we found the treeview - set the location and size of this tree view in the new tab page...
//                    if (tv != null)
//                    {
//                        tv.Location = new Point(0, 0);
//                        tv.Size = new Size(newTabPage.Size.Width, newTabPage.Size.Height);
//                        // Bind a context menu to the tree view...
//                        tv.ContextMenuStrip = ux_contextmenustripTreeView;
//                        // Now add the tree view to the new tab page...
//                        newTabPage.Controls.Add(tv);
//                        ux_tabcontrolGroupListNavigator.TabPages.Insert(destinationTabPageIndex, newTabPage);
//                        ux_tabcontrolGroupListNavigator.SelectTab(destinationTabPageIndex);
//                        if (originalTabPageIndex < destinationTabPageIndex)
//                        {
//                            ux_tabcontrolGroupListNavigator.TabPages.RemoveAt(originalTabPageIndex);
//                        }
//                        else
//                        {
//                            ux_tabcontrolGroupListNavigator.TabPages.RemoveAt(originalTabPageIndex + 1);
//                        }
//                    }
//                }
//            }

//        }

//        private void ux_treeviewmenuNewList_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            // Now we can get a unique name for this tree node (starting with the default node name: New Folder)...
//            String newNodeText = "New List";//CreateUniqueTreeNodeName(/*treeView1.Nodes[0]*/, "New Folder");
//            TreeNode tn = tv.SelectedNode;

//            // Let's make sure the new node name is unique..
//            newNodeText = EnsureUniqueNodeText(tn, newNodeText);
////bool duplicateName = true;
////int i = 1;
////while (duplicateName)
////{
////    duplicateName = false;
////    foreach (TreeNode tnn in tn.Nodes)
////    {
////        if (tnn.Name == newNodeName)
////        {
////            newNodeName = "New List(" + i++.ToString() + ")";
////            duplicateName = true;
////        }
////    }
////}

//            // Now add it to the treeview if it's parent is a folder object (not an ACID, IVID, or ORNO)
//            if (tn.Tag.ToString().ToUpper() == "FOLDER")
//            {
//                TreeNode tnNew = new TreeNode();
//tnNew.Name = newNodeText;
////tnNew.Name = GetNextUserListFolderID().ToString();
//tnNew.Text = newNodeText;
//                tnNew.Tag = "FOLDER";
//                tn.Nodes.Add(tnNew);

//                // If the path length exceeds the maximum allowed string length for a full path - remove the new folder...
//                if (!PathLengthOK(tnNew))
//                {
////MessageBox.Show("Invalid - the 'Full Path' of: " + tnNew.FullPath + " (or one of the subfolders) exceeds maximum length.");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Invalid Path", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "ux_treeviewmenuNewListMessage1";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, tnNew.FullPath);
//ggMessageBox.ShowDialog();
//                    tnNew.Remove();
//                }
//                else
//                {
//                    // Add it to the user's list...
//                    AddTreeNodes(tnNew);
//                    // Make the new folder the active folder...
//                    tnNew.EnsureVisible();
//                }
//                //tnNew.BeginEdit();
//            }

//            // Save the list to the central database...
//            if (userItemList.GetChanges() != null)
//            {
//                DataSet userItemListSaveResults = SaveLists(userItemList);

//                // Get a fresh copy of the list items for this cooperator from the central database...
//                userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//            }
//        }

//        private int GetNextUserListFolderID()
//        {
//            int nextFolderID = -1;
//            string currentSort = userItemList.DefaultView.Sort;
//            userItemList.DefaultView.Sort = "ID_NUMBER ASC";
//            if (userItemList.Rows.Count > 0)
//            {
//                if (int.TryParse(userItemList.Rows[0]["ID_NUMBER"].ToString(), out nextFolderID))
//                {
//                    nextFolderID--;
//                }
//            }
//            return nextFolderID;
//        }

//        private void ux_treeviewmenuClearList_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            // Delete the items in the selected node from the user item list table...

//            DeleteTreeNodes(tv.Parent.Text, tv.SelectedNode);
////foreach (TreeNode tn in tv.SelectedNode.Nodes)
////{
////    // Delete the nodes from the app_user_item_list table...
////    DeleteTreeNodes(tv.Parent.Text, tn);
////}

//            // Now that the records have been deleted from the table delete the nodes from the treeview...
//            tv.SelectedNode.Nodes.Clear();

//            // If there are no nodes left in the treeview - make a new empty one...
//            if (tv.Nodes.Count == 0)
//            {
//                TreeNode tnNew = new TreeNode();
//tnNew.Name = "New List";
////tnNew.Name = GetNextUserListFolderID().ToString();
//                tnNew.Text = "New List";
//                tnNew.Tag = "FOLDER";
//                tv.Nodes.Add(tnNew);
//            }

//            // Save the list to the central database...
//            if (userItemList.GetChanges() != null)
//            {
//                DataSet userItemListSaveResults = SaveLists(userItemList);

//                // Get a fresh copy of the list items for this cooperator from the central database...
//                userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//            }

//            // Resetting these two global variables will force a refresh of the DGV data...
//            lastFullPath = "";
//            lastTabName = "";
//            RefreshMainDGVData();
//            RefreshMainDGVFormatting();
//        }

//        private void ux_treeviewmenuRefreshList_Click(object sender, EventArgs e)
//        {
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            RefreshUserItemListFormatting(tv.Parent.Text, tv.SelectedNode);
//            SaveLists(userItemList);
//            RefreshTreeViewData(tv);

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void ux_treeviewmenuCutList_Click(object sender, EventArgs e)
//        {
//            // To make this function we will probably have to use a global variable to compare this node against 
//            // the node delivered to the paste menu command - if they match perform a deletelist_click command...
            
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            // Copy the selected node from the tree view to the clipboard...
//            Clipboard.SetDataObject(tv.SelectedNode, false, 1, 1000);
//            DeleteTreeNodes(tv.Parent.Text, tv.SelectedNode);
//            tv.SelectedNode.Remove();
//        }

//        private void ux_treeviewmenuCopyList_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            // Copy the selected node from the tree view to the clipboard...
//            Clipboard.SetDataObject(tv.SelectedNode.Clone(), false, 1, 1000);
//        }

//        private void ux_treeviewmenuPasteList_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            if(Clipboard.ContainsData(typeof(TreeNode).ToString()))
//            {
//                DataObject obj = (DataObject)Clipboard.GetDataObject();
//                TreeNode tn = (TreeNode)obj.GetData(typeof(TreeNode));
//                // Make sure the new node has a unique name at the destination node...
//                string uniqueText = EnsureUniqueNodeText(tv.SelectedNode, tn.Text);
//                if (tn.Tag.ToString().Trim().ToUpper() == "FOLDER")
//                {
//tn.Name = uniqueText;
////tn.Name = GetNextUserListFolderID().ToString();
//                    tn.Text = uniqueText;
//                }
//                else
//                {
//                    tn.Text = uniqueText;
//                }
//                tv.SelectedNode.Nodes.Add(tn);

//                // Make sure the new node(s) do not exceed the max length of the full path...
//                if (PathLengthOK(tn))
//                {
//                    AddTreeNodes(tn);
//                }
//                else
//                {
////MessageBox.Show("Invalid - the 'Full Path' of: " + tn.FullPath + " (or one of the subfolders) exceeds maximum length.");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Invalid Path", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "ux_treeviewmenuPasteListMessage1";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, tn.FullPath);
//ggMessageBox.ShowDialog();
//                    tn.Remove();
//                }

//                // Save the list to the central database...
//                if (userItemList.GetChanges() != null)
//                {
//                    DataSet userItemListSaveResults = SaveLists(userItemList);

//                    // Get a fresh copy of the list items for this cooperator from the central database...
//                    userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//                }
//            }
//        }

//        private string EnsureUniqueNodeText(TreeNode destinationTreeNode, string newNodeText)
//        {
//            // Now we can get a unique name for this tree node (starting with the default node name passed in)...
//            String uniqueNodeText = newNodeText;

//            // Let's make sure the new node name is unique..
//            bool duplicateText = true;
//            int i = 1;
//            while (duplicateText)
//            {
//                // Assume this name is unique (until proven otherwise)
//                duplicateText = false;
//                foreach (TreeNode tn in destinationTreeNode.Nodes)
//                {
//                    if (tn.Text == uniqueNodeText)
//                    {
//                        // Nope...  This is not a unique node name so increment a counter until it is unique...
//                        uniqueNodeText = newNodeText + " (" + i++.ToString() + ")";
//                        duplicateText = true;
//                    }
//                }
//            }
//            return uniqueNodeText;
//        }

//        private bool PathLengthOK(TreeNode tnNew)
//        {
//            bool pathLengthOK = false;
//            if (userItemList != null &&
//                userItemList.Columns.Contains("LIST_NAME") &&
//                userItemList.Columns["LIST_NAME"].ExtendedProperties.Contains("max_length"))
//            {
//                int maxPathLength = -1;
//                if (int.TryParse(userItemList.Columns["LIST_NAME"].ExtendedProperties["max_length"].ToString(), out maxPathLength))
//                {
//                    // First check the lenght of the top node...
//                    if (tnNew.TreeView != null && 
//                        tnNew.FullPath.Length < maxPathLength)
//                    {
//                        pathLengthOK = true;
//                    }
//                    else
//                    {
//                        if (tnNew.Tag != null &&
//                            tnNew.Tag.ToString().ToUpper() == "FOLDER")
//                        {
//                            pathLengthOK = false;
//                        }
//                        else
//                        {
//                            pathLengthOK = true;
//                        }
//                    }
//                    // Now check each child node...
//                    if (pathLengthOK)
//                    {
//                        foreach (TreeNode tn in tnNew.Nodes)
//                        {
//                            pathLengthOK = PathLengthOK(tn);
//                        }
//                    }
//                }
//            }
//            return pathLengthOK;
//        }

//        private void ux_treeviewmenuDeleteList_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            // Delete the entries in the groups table...
//            DeleteTreeNodes(tv.Parent.Text, tv.SelectedNode);
//            // Delete the node from the tree view...
//            tv.SelectedNode.Remove();

//            // Save the list to the central database...
//            if (userItemList.GetChanges() != null)
//            {
//                DataSet userItemListSaveResults = SaveLists(userItemList);
//                DataTable junk = userItemList.GetChanges();

//                // Get a fresh copy of the list items for this cooperator from the central database...
//                userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//            }

//            // If there are no nodes left in the treeview - make a new empty one...
//            if (tv.Nodes.Count == 0)
//            {
//                TreeNode tnNew = new TreeNode();
//                tnNew.Name = "New List";
//tnNew.Text = "New List";
////tnNew.Name = GetNextUserListFolderID().ToString();
//                tnNew.Tag = "FOLDER";
//                tv.Nodes.Add(tnNew);
//AddUserItemListRow(tnNew);

//// Save the list to the central database...
//if (userItemList.GetChanges() != null)
//{
//    DataSet userItemListSaveResults = SaveLists(userItemList);

//    // Get a fresh copy of the list items for this cooperator from the central database...
//    userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//}
//                // Refresh the interface...
//                RefreshMainDGVData();
//                RefreshMainDGVFormatting();
//            }
//        }

//        private void ux_treeviewmenuRenameList_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;
//            if (!tv.LabelEdit) tv.LabelEdit = true;
//            tv.SelectedNode.BeginEdit();
//            // The renaming process is finished in the treeView_AfterLabelEdit() method below...

//        }

//        private void ux_treeviewmenuListProperties_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;

//            // Next find the primary key of the row that created this node...
//            string parentFullPath = "{DBNull.Value}";
//            if (tv.SelectedNode.Parent != null) parentFullPath = tv.SelectedNode.Parent.FullPath;
//            DataRow[] drs = userItemList.Select("ID_TYPE='FOLDER' AND LIST_NAME = '" + parentFullPath + "' AND TITLE = '" + tv.SelectedNode.Text + "'");
//            if (drs.Length > 0)
//            {
//                // Now we need to instantiate a treeview node properties dialog box...
//                string pKey = drs[0][userItemList.PrimaryKey[0].ColumnName].ToString();
//                TreeviewNodeProperties tvnp = new TreeviewNodeProperties(_sharedUtils, pKey, userItemList);
//tvnp.StartPosition = FormStartPosition.CenterParent;
//                if (tvnp.ShowDialog() == DialogResult.OK)
//                {
//                    if (drs[0]["properties"].ToString() != tvnp.Properties)
//                    {
//                        drs[0]["properties"] = tvnp.Properties;
////SortUserItemList(tv.Parent.Text, tv.SelectedNode, "ASCENDING", false);  // This is done in the RefreshUserItemListFormatting() method
//                        RefreshUserItemListFormatting(tv.Parent.Text, tv.SelectedNode);
//                        SaveLists(userItemList);
////GetLists(ux_comboboxCNO.SelectedValue.ToString());  // This is done in the RefreshTreeViewData() method
//                        RefreshTreeViewData(tv);
//                    }
//                }
//            }
//        }

//        private void ux_treeviewmenuListSortAscending_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;

////if (tv.TreeViewNodeSorter == null)
////{
////    TreeViewNodeSorter tvns = new TreeViewNodeSorter();
////    tvns.SortDirection = 1;
////    tv.TreeViewNodeSorter = tvns;
////}
////else
////{
////    TreeViewNodeSorter tvns = (TreeViewNodeSorter)tv.TreeViewNodeSorter;
////    tvns.SortDirection = 1;
////}
////tv.Sort();
//            SortUserItemList(tv.Parent.Text, tv.SelectedNode, "ASCENDING", false);

//            // Save the list to the central database...
//            if (userItemList.GetChanges() != null)
//            {
//                DataSet userItemListSaveResults = SaveLists(userItemList);

//                // Get a fresh copy of the list items for this cooperator from the central database...
////userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());  // This is done in the RefreshTreeViewData() method
//            }
//            // Now refresh the treeview to display the new sort order...
//            RefreshTreeViewData(tv);
//        }

//        private void ux_treeviewmenuListSortDescending_Click(object sender, EventArgs e)
//        {
//            // First we need to find out which tree view has been clicked...
//            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
//            ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
//            TreeView tv = (TreeView)cms.SourceControl;

////if (tv.TreeViewNodeSorter == null)
////{
////    TreeViewNodeSorter tvns = new TreeViewNodeSorter();
////    tvns.SortDirection = -1;
////    tv.TreeViewNodeSorter = tvns;
////}
////else
////{
////    TreeViewNodeSorter tvns = (TreeViewNodeSorter)tv.TreeViewNodeSorter;
////    tvns.SortDirection = -1;
////}
////tv.Sort();
//            SortUserItemList(tv.Parent.Text, tv.SelectedNode, "DESCENDING", true);

//            // Save the list to the central database...
//            if (userItemList.GetChanges() != null)
//            {
//                DataSet userItemListSaveResults = SaveLists(userItemList);

//                // Get a fresh copy of the list items for this cooperator from the central database...
////userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());  // This is done in the RefreshTreeViewData() method
//            }
//            // Now refresh the treeview to display the new sort order...
//            RefreshTreeViewData(tv);
//        }


//        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
//        {
//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            //SetAllUserSettings();
//            SetGroupListNavigatorUserSettings();
//            RefreshMainDGVData();
//            RefreshMainDGVFormatting();
//            ux_datagridviewMain.CurrentCell = null;
//            ((TreeView)sender).Focus();

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
//        {
//            if (e.Label != null && e.Label.Length == 0)
//            {
//                // Cancel the label edit action, inform the user, and place the node in edit mode again...
//                e.CancelEdit = true;
////MessageBox.Show("Invalid - the list name cannot be blank");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the list name cannot be blank", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "treeView_AfterLabelEditMessage1";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//ggMessageBox.ShowDialog();
//                //e.Node.BeginEdit();
//            }
//            else if(e.Label != null)
//            {
//                if (e.Node.Parent != null && e.Node.Parent.Nodes.ContainsKey(e.Label))
//                {
//                    e.CancelEdit = true;
////MessageBox.Show("Invalid - the list name cannot be a duplicate.\n\nThere is already an item named '" + e.Label + "' in the folder '" + e.Node.Parent.Text + "'.");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the list name cannot be a duplicate.\n\nThere is already an item named '{0}' in the folder '{1}'.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "treeView_AfterLabelEditMessage2";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
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
//ggMessageBox.ShowDialog();
//                }
//                else
//                {
//////string NewFullPath = e.Node.FullPath.Substring(0, e.Node.FullPath.LastIndexOf(e.Node.Text)) + e.Label;
////// Create a temp node with the new name...
//////TreeNode tempNewNameNode = new TreeNode(e.Label);
////TreeNode tempNewNameNode = (TreeNode)e.Node.Clone();
////tempNewNameNode.Text = e.Label;
////// Check to see if e.node is the root node to process properly...
////if (e.Node.Parent != null)
////{
////    // This is not the root node, so add the new node to e.Node.Parent...
////    e.Node.Parent.Nodes.Add(tempNewNameNode);
////}
////else
////{
////    // This is the root node, so add the new node to the e.Node.TreeView...
////    e.Node.TreeView.Nodes.Add(tempNewNameNode);
////}
//////if (tempNewNameNode.FullPath.Length < 100)

////if (PathLengthOK(tempNewNameNode))
//                    // Remember the original text for the node (in case the nodes new name exceeds max path length)
//                    string origNodeText = e.Node.Text;
//                    // Set the new label...
//                    e.Node.Text = e.Label;
//                    if(PathLengthOK(e.Node))
//                    {
////MoveTreeNodes(((TreeView)sender).Parent.Text, e.Node.FullPath, ((TreeView)sender).Parent.Text, NewFullPath);
////MoveTreeNodes(((TreeView)sender).Parent.Text, e.Node, ((TreeView)sender).Parent.Text, tempNewNameNode);

//// Restore the name back to original so that we can rename the objects in the user's list...
//e.Node.Text = origNodeText;
//RenameTreeNodes(((TreeView)sender).Parent.Text, e.Node, e.Label);
//// and then rename the node back again...
//e.Node.Text = e.Label;
//if (e.Node.Tag.ToString().Trim().ToUpper() == "FOLDER") e.Node.Name = e.Label;  // Only do this to Folders...
////if (e.Node.Tag.ToString().Trim().ToUpper() == "FOLDER") e.Node.Name = GetNextUserListFolderID().ToString();  // Only do this to Folders...

//                        // Save the list to the central database...
//                        if (userItemList.GetChanges() != null)
//                        {
//                            DataSet userItemListSaveResults = SaveLists(userItemList);

//                            // Get a fresh copy of the list items for this cooperator from the central database...
//                            userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//                        }
//                    }
//                    else
//                    {
//                        e.Node.Text = origNodeText;
//                        e.CancelEdit = true;
////MessageBox.Show("Invalid - the 'Full Path' of: " + tempNewNameNode.FullPath + " (or one of the subfolders) exceeds maximum length.");
////MessageBox.Show("Invalid - the 'Full Path' of: " + e.Node.FullPath + " (or one of the subfolders) exceeds maximum length.");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "treeView_AfterLabelEditMessage3";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, e.Node.FullPath);
//ggMessageBox.ShowDialog();
//                    }
////// Now delete the tempNewNameNode...
////if (e.Node.Parent != null)
////{
////    // This is not the root node, so remove the new node from e.Node.Parent...
////    e.Node.Parent.Nodes.Remove(tempNewNameNode);
////}
////else
////{
////    // This is the root node, so remove the new node from the e.Node.TreeView...
////    e.Node.TreeView.Nodes.Remove(tempNewNameNode);
////}
//                    // Disable the treeview property that allows label edits...
//                    ((TreeView)sender).LabelEdit = false;
//                }
//            }
//        }

//        void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
//        {
//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;
            
//            if (e.Node.Nodes.Count == 1 &&
//                e.Node.Nodes[0].Name == "!!DUMMYNODE!!")
//            {
//                e.Node.Nodes["!!DUMMYNODE!!"].Remove();
//                switch (e.Node.Tag.ToString().Trim().ToUpper())
//                {
//                    case "ACCESSION_ID":
//                        DataRow[] accessionInventory = GetAccessionNodeChildren(e.Node.Name);
//                        foreach (DataRow inventoryRow in accessionInventory)
//                        {
//                            TreeNode inventoryNode = new TreeNode();
//                            DataRow dr = userItemList.NewRow();
//                            dr["ID_TYPE"] = "INVENTORY_ID";
//                            dr["ID_NUMBER"] = inventoryRow["inventory_id"].ToString();
//                            dr["LIST_NAME"] = e.Node.Parent.FullPath;
//                            RefreshUserItemListTitles(new DataRow[] { dr });
////inventoryNode.Text = GetInventoryNodeFriendlyName(inventoryRow);
//                            inventoryNode.Text = dr["TITLE"].ToString().Trim();
//                            inventoryNode.Name = inventoryRow["inventory_id"].ToString();
//                            inventoryNode.Tag = "INVENTORY_ID";
//                            inventoryNode.ImageKey = "inactive_INVENTORY_ID";
//                            inventoryNode.SelectedImageKey = "active_INVENTORY_ID";
//                            e.Node.Nodes.Add(inventoryNode);
//                        }
//                        break;
//                    case "ORDER_REQUEST_ID":
//                        //
//                        break;
//                    default:
//                        break;
//                }
//            }

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void treeView_MouseClick(object sender, MouseEventArgs e)
//        {
//            // This code below is to make the node that was right clicked the selected node
//            // so that when a context menu item is selected the action will be performed on 
//            // the correct node...
//            if (e.Button == MouseButtons.Right)
//            {
//                if (((TreeView)sender).GetNodeAt(e.X, e.Y) != null)
//                {
//                    ((TreeView)sender).SelectedNode = ((TreeView)sender).GetNodeAt(e.X, e.Y);
//                }

//                if (((TreeView)sender).SelectedNode.Name == "{DBNull.Value}")
//                {
//                    ux_treeviewmenuNewList.Enabled = true;
//                    ux_treeviewmenuClearList.Enabled = true;
//                    ux_treeviewmenuListSortAscending.Enabled = true;
//                    ux_treeviewmenuListSortDescending.Enabled = true;
//                    ux_treeviewmenuRenameList.Enabled = true;
//                    ux_treeviewmenuCutList.Enabled = false;
//                    ux_treeviewmenuCopyList.Enabled = true;
//                    ux_treeviewmenuPasteList.Enabled = true;
//                    ux_treeviewmenuDeleteList.Enabled = true;
//                    ux_treeviewmenuListProperties.Enabled = true;
//                }
//                else
//                {
//                    ux_treeviewmenuNewList.Enabled = true;
//                    ux_treeviewmenuClearList.Enabled = true;
//                    ux_treeviewmenuListSortAscending.Enabled = true;
//                    ux_treeviewmenuListSortDescending.Enabled = true;
//                    ux_treeviewmenuRenameList.Enabled = true;
//                    ux_treeviewmenuCutList.Enabled = true;
//                    ux_treeviewmenuCopyList.Enabled = true;
//                    ux_treeviewmenuPasteList.Enabled = true;
//                    ux_treeviewmenuDeleteList.Enabled = true;
//                    ux_treeviewmenuListProperties.Enabled = true;
//                }

//                if (((TreeView)sender).SelectedNode.Tag != null &&
//                    ((TreeView)sender).SelectedNode.Tag.ToString().ToUpper() == "FOLDER")
//                {
//                    ux_treeviewmenuNewList.Enabled = true;
//                    ux_treeviewmenuClearList.Enabled = true;
//                    ux_treeviewmenuPasteList.Enabled = true;
//                }
//                else
//                {
//                    ux_treeviewmenuNewList.Enabled = false;
//                    ux_treeviewmenuClearList.Enabled = false;
//                    ux_treeviewmenuPasteList.Enabled = false;
//                }
//            }
//        }

//        void treeView_ItemDrag(object sender, ItemDragEventArgs e)
//        {
//            // A treenode is being dragged - time to start a DragDrop operation...
//            DataObject dndData = new DataObject();
//            TreeNode rootNode = (TreeNode)e.Item;
//            // First add the treenode to the data to be sent...
//            dndData.SetData(rootNode);
//            // Now add a text version of the data so that this can be dragged from
//            // one curator tool to another...
//            foreach (TreeNode tn in rootNode.Nodes)
//            {
//            }

//            ((TreeView)sender).DoDragDrop(rootNode, DragDropEffects.Copy | DragDropEffects.Move);
            
//        }

//        private void treeView_DragOver(object sender, DragEventArgs e)
//        {
//            // Okay we are in the middle of a Drag and Drop operation and the mouse is in 
//            // the treeview control so lets handle this event...

//            // This code will change the cursor icon to give the user feedback about whether or not
//            // the drag-drop operation is allowed (in this case it is only allowed for plain text)...
//            //
//            // Convert the mouse coordinates from screen to client...
//            Point ptClientCoord = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
//            // Get the node closest to the mouse cursor (to make sure it is a folder)...
//            TreeNode tnClosestToMouse = ((TreeView)sender).GetNodeAt(ptClientCoord);

//tnClosestToMouse.EnsureVisible();
//// If the mouse is within 1 node of the top attempt to scroll up...
//if (ptClientCoord.Y < tnClosestToMouse.TreeView.ItemHeight)
//{
//    TreeNode pn = tnClosestToMouse.PrevNode;
//    if (pn != null)
//    {
//        pn.EnsureVisible();
//    }
//    else
//    {
//        pn = tnClosestToMouse.Parent;
//        if (pn != null) pn.EnsureVisible();
//    }
//}
//// If the mouse is within 1 node of the bottom attempt to scroll down...
//else if (ptClientCoord.Y > (tnClosestToMouse.TreeView.Height - tnClosestToMouse.TreeView.ItemHeight))
//{
//    TreeNode nn = tnClosestToMouse.NextNode;
//    if (nn != null) nn.EnsureVisible();
//}

//            // Is this a collection of dataset rows being dragged to a node...
//            if (e.Data.GetDataPresent(typeof(DataSet)))
//            {
//                if (tnClosestToMouse.Tag.ToString().ToUpper() == "FOLDER" &&
//                tnClosestToMouse.Parent != null)
//                {
//                    e.Effect = DragDropEffects.Copy;
//                }
//                else
//                {
//                    e.Effect = DragDropEffects.None;
//                }
//            }
//            // Is this a treeview node being dragged to a new location...
//            else if (e.Data.GetDataPresent(typeof(TreeNode)))
//            //else if (e.Data.GetDataPresent(typeof(TreeNode)) && tnClosestToMouse.Tag.ToString().ToUpper() == "FOLDER")
//            {
//                TreeNode tn = (TreeNode)e.Data.GetData(typeof(TreeNode));
//                // Don't allow items to be dropped on the root folder...
//                if (tn.Tag.ToString().Trim().ToUpper() != "FOLDER" &&
//                    tnClosestToMouse.Parent == null)
//                {
//                    e.Effect = DragDropEffects.None;
//                }
//                else if ((e.KeyState & 8) == 8) // Check to see if the CTRL key is down.  (4 = Shift, 8 = CTRL, 32 = ALT, 8 + 32 = CTRL + ALT, etc...)
//                {
//                    // Ctrl key is down - so do a copy...
//                    e.Effect = DragDropEffects.Copy;
//                }
//                else
//                {
//                    // Ctrl key is not down - so do a move...
//                    e.Effect = DragDropEffects.Move;
//                }
////Graphics g = tnClosestToMouse.TreeView.CreateGraphics();
////Pen eraser = new Pen(tnClosestToMouse.TreeView.BackColor, 2.0f);
////Pen insertLine = new Pen(Color.Black, 2.0f);
////g.DrawLine(eraser, ptTreeNodeInsertLineStart, ptTreeNodeInsertLineStop);
////ptTreeNodeInsertLineStart = new Point(tnClosestToMouse.Bounds.Left, tnClosestToMouse.Bounds.Bottom);
////ptTreeNodeInsertLineStop = new Point(tnClosestToMouse.Bounds.Right, tnClosestToMouse.Bounds.Bottom);
////g.DrawLine(insertLine, ptTreeNodeInsertLineStart, ptTreeNodeInsertLineStop);
//if (ptClientCoord.Y > tnClosestToMouse.Bounds.Bottom - 5 &&
//    ptClientCoord.Y < tnClosestToMouse.Bounds.Bottom + 5)
//{
//    Graphics g = tnClosestToMouse.TreeView.CreateGraphics();
//    Pen eraser = new Pen(tnClosestToMouse.TreeView.BackColor, 2.0f);
//    Pen insertLine = new Pen(Color.Black, 2.0f);
//    g.DrawLine(eraser, ptTreeNodeInsertLineStart, ptTreeNodeInsertLineStop);
//    ptTreeNodeInsertLineStart = new Point(tnClosestToMouse.Bounds.Left, tnClosestToMouse.Bounds.Bottom);
//    ptTreeNodeInsertLineStop = new Point(tnClosestToMouse.Bounds.Right, tnClosestToMouse.Bounds.Bottom);
//    g.DrawLine(insertLine, ptTreeNodeInsertLineStart, ptTreeNodeInsertLineStop);
//    if(tnClosestToMouse.PrevVisibleNode != null) tnClosestToMouse.PrevVisibleNode.BackColor = Color.Empty;
//    tnClosestToMouse.BackColor = Color.Empty;
//    if (tnClosestToMouse.NextVisibleNode != null) tnClosestToMouse.NextVisibleNode.BackColor = Color.Empty;
//}
//else
//{
//    Graphics g = tnClosestToMouse.TreeView.CreateGraphics();
//    Pen eraser = new Pen(tnClosestToMouse.TreeView.BackColor, 2.0f);
//    g.DrawLine(eraser, ptTreeNodeInsertLineStart, ptTreeNodeInsertLineStop);
//    if (tnClosestToMouse.PrevVisibleNode != null) tnClosestToMouse.PrevVisibleNode.BackColor = Color.Empty;
//    if (tnClosestToMouse.Tag.ToString().Trim().ToUpper() == "FOLDER") tnClosestToMouse.BackColor = Color.LightGreen;
//    if (tnClosestToMouse.NextVisibleNode != null) tnClosestToMouse.NextVisibleNode.BackColor = Color.Empty;
//}
//            }
////// Is this a treeview node being re-ordered in the same folder...
////else if (e.Data.GetDataPresent(typeof(TreeNode)) &&
////    tnClosestToMouse.Tag.ToString().ToUpper() != "FOLDER" &&
////    (tnClosestToMouse.Parent != null &&
////    ((TreeNode)e.Data.GetData(typeof(TreeNode))).Parent != null &&
////    tnClosestToMouse.Parent.FullPath == ((TreeNode)e.Data.GetData(typeof(TreeNode))).Parent.FullPath))
////{
////    e.Effect = DragDropEffects.Move;
////}
//            // Is this an image being dragged to an inventory or accession object...
//            else if (e.Data.GetDataPresent(DataFormats.FileDrop) &&
//                (tnClosestToMouse.Tag.ToString().ToUpper() == "INVENTORY_ID" || tnClosestToMouse.Tag.ToString().ToUpper() == "ACCESSION_ID"))
//            {
//                e.Effect = DragDropEffects.Copy;
//            }
//            else
//            {
//                e.Effect = DragDropEffects.None;
//            }

//            // This code will expand/collapse the treenode if the mouse is hovering over a folder object...
//            TimeSpan ts = DateTime.Now.Subtract(dtMouseHoverStartTime);
//            if (tnClosestToMouse == tnMouseHoveringOverNode)
//            {
//                if (ts.Seconds > 1)
//                {
//                    if(tnClosestToMouse.Tag.ToString().Trim().ToUpper() == "FOLDER" &&
//                        tnClosestToMouse.Bounds.Bottom > ptClientCoord.Y + 5) tnClosestToMouse.Toggle(); //.Expand();
//                    tnMouseHoveringOverNode = null;
//                }
//            }
//            else
//            {
//                tnMouseHoveringOverNode = tnClosestToMouse;
//                dtMouseHoverStartTime = DateTime.Now;
//            }
//        }

//        private void treeView_DragDrop(object sender, DragEventArgs e)
//        {
//            // The drag-drop event is coming to a close - process this event to handle the dropping of
//            // data into the treeview...

//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            // Convert the mouse coordinates from screen to client...
//            Point ptClientCoord = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

//            // Is this a collection of dataset rows being dragged to a node...
//            if (e.Data.GetDataPresent(typeof(DataSet)) && e.Effect != DragDropEffects.None)
//            {
//                DataTable newUserItemList;
//                DataSet dndData = (DataSet)e.Data.GetData(typeof(DataSet));
////// Make sure the primary key is set for the dnd table...
////// NOTE: this should get pulled out when the web service starts populating this properly...
////System.Collections.Generic.List<DataColumn> pkeyColumns = new System.Collections.Generic.List<DataColumn>();
////foreach (DataColumn dc in dndData.Tables[0].Columns)
////{
////    if (dc.ExtendedProperties.ContainsKey("is_primary_key") &&
////        dc.ExtendedProperties["is_primary_key"].ToString() == "Y") pkeyColumns.Add(dc);
////}
////try
////{
////    dndData.Tables[0].PrimaryKey = pkeyColumns.ToArray();
////}
////catch
////{
////    MessageBox.Show("Error: The imported data does not include primary key information.");
////    // Restore cursor to default cursor...
////    Cursor.Current = origCursor;
////    return;
////}

//                // Create an empty copy of the groups data table...
//                newUserItemList = userItemList.Clone();
//                // Set this treenode to a default of the currently selected node (just in 
//                // case the hit test on the mouse coordinates does not land on a node)...
//                TreeNode tnNodeClosestToTheDrop = ((TreeView)sender).SelectedNode;
////// Get the screen coordinates from the event args and convert to client coordinates...
////Point ptMouseLocInScreen = new Point(e.X, e.Y); // This is in screen coordinates...
////Point ptMouseLocInClient = ((TreeView)sender).PointToClient(ptMouseLocInScreen);
////if (((TreeView)sender).GetNodeAt(ptMouseLocInClient) != null)
////{
////    tnNodeClosestToTheDrop = ((TreeView)sender).GetNodeAt(ptMouseLocInClient);
////}
//                if (((TreeView)sender).GetNodeAt(ptClientCoord) != null)
//                {
//                    tnNodeClosestToTheDrop = ((TreeView)sender).GetNodeAt(ptClientCoord);
//                }

//                // Remember the original center statusbar message and alignment...
//                string centerMessage = ux_statusCenterMessage.Text;
//                ContentAlignment centerMessageAlignment = ux_statusCenterMessage.TextAlign;
//                // Set the statusbar up to show the import progress...
//                ux_statusCenterMessage.TextAlign = ContentAlignment.MiddleRight;
//                ux_statusCenterMessage.Text = "Importing Data:";
//                ux_statusRightProgressBar.Visible = true;
//                ux_statusRightProgressBar.Minimum = 0;
//                ux_statusRightProgressBar.Maximum = 100; // dndData.Tables[0].Rows.Count;
//                ux_statusRightProgressBar.Step = 1;
//                ux_statusRightProgressBar.Value = 0;
//                ux_statusstripMain.Refresh();

//                foreach (DataRow dr in dndData.Tables[0].Rows)
//                {
//                    DataRow newItem = BuildUserItemListRow(dr, tnNodeClosestToTheDrop);
//                    if (newItem != null &&
//                        !tnNodeClosestToTheDrop.Nodes.ContainsKey(newItem["ID_NUMBER"].ToString()))
//                    {
//                        //NewGroupRecords.Tables["get_lists"].Rows.Add(newItem.ItemArray);
//                        newUserItemList.Rows.Add(newItem.ItemArray);
//                    }
//                }

//                // Update the progress bar...
//                ux_statusRightProgressBar.Value = 25;
//                // Save the user item list to the database
//                DataSet errorRecords = SaveLists(newUserItemList);

//                if (errorRecords.Tables.Contains("get_lists"))
//                {
//                    foreach (DataRow dr in errorRecords.Tables["get_lists"].Rows)
//                    {
//                        if (dr["SavedAction"].ToString() == "Insert" && dr["SavedStatus"].ToString() == "Success")
//                        {
//                            TreeNode tnNewNode = BuildTreeNode(dr);
////TreeNode tnNewNode = new TreeNode();
////if (dr.Table.Columns.Contains("TITLE")) tnNewNode.Text = dr["TITLE"].ToString().Trim();
////if (dr.Table.Columns.Contains("FRIENDLY_NAME")) tnNewNode.Text = dr["FRIENDLY_NAME"].ToString();
////tnNewNode.Name = dr["ID_NUMBER"].ToString();
////tnNewNode.Tag = dr["ID_TYPE"].ToString();
////tnNewNode.ImageKey = "inactive_" + dr["ID_TYPE"].ToString().Trim();
////tnNewNode.SelectedImageKey = "active_" + dr["ID_TYPE"].ToString().Trim();
//                            if (!tnNodeClosestToTheDrop.Nodes.ContainsKey(tnNewNode.Name))
//                            {
//                                tnNodeClosestToTheDrop.Nodes.Add(tnNewNode);
////// If this is an accession - auto add the child inventory stub...
////if (dr["ID_TYPE"].ToString() == "ACCESSION_ID")
////{
////    tnNewNode.Nodes.Add(BuildDummyNode("INVENTORY_ID"));
////}
////// If this is an order - auto add the child order items stub...
////if (dr["ID_TYPE"].ToString() == "ORDER_REQUEST_ID")
////{
////    tnNewNode.Nodes.Add(BuildDummyNode("ORDER_REQUEST_ITEM_ID"));
////}
//                            }
//                        }
//                    }
//                }
                
//                // Update the progress bar...
//                ux_statusRightProgressBar.Value = 50;
                
//                // And go get a fresh copy from the database to refresh the local copy...
//                userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());

//                // Refresh the formatting of the node...
//                RefreshUserItemListFormatting(ux_tabcontrolGroupListNavigator.SelectedTab.Text, tnNodeClosestToTheDrop);
//                SaveLists(userItemList);
//                RefreshTreeViewData(tnNodeClosestToTheDrop.TreeView);

//                // Update the progress bar...
//                ux_statusRightProgressBar.Value = 75;

//                // Restore the statusbar back to it's original state...
//                ux_statusCenterMessage.TextAlign = centerMessageAlignment;
//                ux_statusCenterMessage.Text = centerMessage;
//                ux_statusRightProgressBar.Visible = false;
//                ux_statusstripMain.Refresh();

//                // Resetting these two global variables will force a refresh of the DGV data...
//                lastFullPath = "";
//                lastTabName = "";
//                RefreshMainDGVData();
//                RefreshMainDGVFormatting();
//            }

//            // Is this a treeview node being dragged to a new location...
//            if (e.Data.GetDataPresent(typeof(TreeNode)) && e.Effect != DragDropEffects.None)
//            {
//                TabPage SourceTab = (TabPage)((TreeNode)e.Data.GetData(typeof(TreeNode))).TreeView.Parent;
//                TabPage DestinationTab = (TabPage)((TreeView)sender).Parent;
//                TreeNode SourceNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
//                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(((TreeView)sender).PointToClient(new Point(e.X, e.Y)));
//                TreeNode newNode = (TreeNode)SourceNode.Clone();
//// Bail if the user drops the source back on itself...
//if (SourceNode == DestinationNode) return;
//// Do nothing if the destination node is the root node...
//if (DestinationNode.Tag.ToString().Trim().ToUpper() == "FOLDER" ||
//    DestinationNode.Parent != null)
////if(DestinationNode.Tag.ToString() == "FOLDER" &&
////    DestinationNode.Parent.FullPath != SourceNode.Parent.FullPath)
//{
//                // Make sure the new node has a unique name at the destination node...
//                //string uniqueName;

//                // If the destination node is a folder and the user is dropping the source node
//                // on the middle of the folder node (not the bottom edge - which indicates the user would
//                // like to insert the source node below the folder (not IN the folder)...
//                if ((DestinationNode.Tag.ToString().Trim().ToUpper() == "FOLDER" &&
//                    DestinationNode.Bounds.Bottom > ptClientCoord.Y + 5) ||
//                    SourceNode.Parent == DestinationNode)
//                {
//                    if (!DestinationNode.Nodes.Contains(SourceNode) || e.Effect == DragDropEffects.Copy)
//                    {
//                        // If the source node is not an existing node in the destination folder (ie it is not being moved/re-ordered) 
//                        // or a copy of the source node is being made - get unique text for the new node...
//                        newNode.Text = EnsureUniqueNodeText(DestinationNode, newNode.Text);
//                        // Make the nodes Name=Text if this is a folder...
//                        if (SourceNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//                        {
//                            newNode.Name = newNode.Text;
//                        }
//                    }
//                    DestinationNode.Nodes.Insert(0, newNode);
//                }
//                else
//                {
//                    // The destination is an item (not a folder) or the user wants to drop the source node
//                    // below the destination folder - so insert the new node in the destination node's parent folder (at the top of the list)...
//                    if (!DestinationNode.Parent.Nodes.Contains(SourceNode) || e.Effect == DragDropEffects.Copy)
//                    {
//                        // If the source node is not an existing node in the destination folder (ie it is not being moved/re-ordered) 
//                        // or a copy of the source node is being made - get unique text for the new node...
//                        newNode.Text = EnsureUniqueNodeText(DestinationNode.Parent, newNode.Text);
//                        if (SourceNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//                        {
//                            newNode.Name = newNode.Text;
//                        }
//                    }
//                    DestinationNode.Parent.Nodes.Insert(DestinationNode.Index + 1, newNode);
//                }
////                if (newNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
////                {
////newNode.Name = uniqueName;
//////newNode.Name = GetNextUserListFolderID().ToString();
////                    newNode.Text = uniqueName;
////                }
////                else
////                {
////                    newNode.Text = uniqueName;
////                }
//                // New node is ready to go - now decide to either copy or move it to the destination...
//                if (e.Effect == DragDropEffects.Move)
//                {
////DestinationNode.Nodes.Add(newNode);
//                    if (PathLengthOK(newNode))
//                    {
//                        MoveTreeNodes(SourceTab.Text, SourceNode, DestinationTab.Text, newNode);
//                        SourceNode.Remove();
//                    }
//                    else
//                    {
////MessageBox.Show("Invalid - the 'Full Path' of: " + newNode.FullPath + " (or one of the subfolders) exceeds maximum length.");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "treeView_DragDropMessage1";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, newNode.FullPath);
//ggMessageBox.ShowDialog();
//                        newNode.Remove();
//                    }
//                }
//                else if (e.Effect == DragDropEffects.Copy)
//                {
////DestinationNode.Nodes.Add(newNode);
//                    if (PathLengthOK(newNode))
//                    {
//                        AddTreeNodes(newNode);
//                    }
//                    else
//                    {
////MessageBox.Show("Invalid - the 'Full Path' of: " + newNode.FullPath + " (or one of the subfolders) exceeds maximum length.");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Invalid - the 'Full Path' of: {0} (or one of the subfolders) exceeds maximum length.", "Label Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "treeView_DragDropMessage2";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, newNode.FullPath);
//ggMessageBox.ShowDialog();
//                        newNode.Remove();
//                    }
//                }
//SortUserItemList(DestinationTab.Text, DestinationNode, "MANUAL", true);
//}
////else
////{
////    if (DestinationNode.Index != SourceNode.Index)
////    {
////        SourceNode.Remove();
////        DestinationNode.Parent.Nodes.Insert(DestinationNode.Index, SourceNode);
////    }
////    SortTreeNodes(DestinationTab.Text, DestinationNode, "MANUAL", true);
////}

//                // Save the list to the central database...
//                if (userItemList.GetChanges() != null)
//                {
//                    DataTable dt = userItemList.GetChanges();
//                    DataSet userItemListSaveResults = SaveLists(userItemList.GetChanges());

//                    // Get a fresh copy of the list items for this cooperator from the central database...
//                    userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//                }
//foreach (TabPage tp in ux_tabcontrolGroupListNavigator.TabPages)
//{
//    foreach (Control ctrl in tp.Controls)
//    {
//        if (ctrl.GetType() == typeof(TreeView))
//        {
//            TreeView tv = (TreeView)ctrl;
//            ResetNodeFormat(tv.Nodes[0]);
//            tv.Refresh();
//        }
//    }
//}
//            }

//            // Are these image files being dropped on to an accession or inventory node...
//            if (e.Data.GetDataPresent(DataFormats.FileDrop))
//            {
//                string[] fullPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
//                TreeNode destinationNode = ((TreeView)sender).GetNodeAt(((TreeView)sender).PointToClient(new Point(e.X, e.Y)));

//                if (destinationNode.Tag.ToString().ToUpper() == "INVENTORY_ID")
//                {
//                    LoadInventoryImages(fullPaths, destinationNode.Name);
//                }
//                else if (destinationNode.Tag.ToString().ToUpper() == "ACCESSION_ID")
//                {
//                    string inventoryIDs = "";
//                    //DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=; :accessionid=" + destinationNode.Name + "; :orderrequestid=; :cooperatorid=;", 0, 0);
//                    DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + destinationNode.Name, 0, 0);
//                    if (ds.Tables.Contains("get_inventory"))
//                    {
                        
////DataRow[] drs = ds.Tables["get_inventory"].Select("inventory_type_code='**'");
//                        DataRow[] drs = ds.Tables["get_inventory"].Select("FORM_TYPE_CODE='**'");
//                        foreach (DataRow dr in drs)
//                        {
//                            inventoryIDs += dr["INVENTORY_ID"].ToString() + ",";
//                        }
//                        inventoryIDs = inventoryIDs.TrimEnd(',');
//                    }
//                    if (!string.IsNullOrEmpty(inventoryIDs)) LoadInventoryImages(fullPaths, inventoryIDs);
//                }
//            }
//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void AddUserItemListRow(TreeNode tn)
//        {
//            DataRow newdr = userItemList.NewRow();
//            newdr["COOPERATOR_ID"] = ux_comboboxCNO.SelectedValue.ToString();
//            newdr["TAB_NAME"] = tn.TreeView.Parent.Text;
//            if (tn.Tag.ToString() == "FOLDER")
//            {
//                // Full path of this folder...
//                //newdr["LIST_NAME"] = tn.FullPath;
//                if(tn.Parent != null) newdr["LIST_NAME"] = tn.Parent.FullPath;
////else newdr["LIST_NAME"] = "RootNode";
//else newdr["LIST_NAME"] = "{DBNull.Value}";
//            }
//            else
//            {
//                // Full path of the parent folder...
//                newdr["LIST_NAME"] = tn.Parent.FullPath;
//            }
//            int idNumber = -1;
//            if (int.TryParse(tn.Name, out idNumber)) newdr["ID_NUMBER"] = tn.Name;
//            newdr["ID_TYPE"] = (string)tn.Tag;
//            newdr["TITLE"] = tn.Text.Trim();
//            userItemList.Rows.Add(newdr);
//        }

//        private DataRow BuildUserItemListRow(DataRow sourceRow, TreeNode destinationNode)
//        {
//            DataRow newdr = userItemList.NewRow();

//            if (sourceRow.Table.PrimaryKey.Length == 1)
//            {
//                string pKeyColumnName = sourceRow.Table.PrimaryKey[0].ColumnName.Trim().ToUpper();
//                string friendlyName = "";

//                switch (pKeyColumnName)
//                {
//                    case "INVENTORY_ID":
//                        friendlyName = GetInventoryNodeFriendlyName(sourceRow);
//                        break;
//                    case "ACCESSION_ID":
//                        friendlyName = GetAccessionNodeFriendlyName(sourceRow);
//                        break;
//                    case "ORDER_REQUEST_ID":
//                        friendlyName = GetOrderRequestNodeFriendlyName(sourceRow);
//                        break;
//                    case "COOPERATOR_ID":
//                        friendlyName = GetCooperatorNodeFriendlyName(sourceRow);
//                        break;
//                    case "GEOGRAPHY_ID":
//                        friendlyName = GetGeographyNodeFriendlyName(sourceRow);
//                        break;
//                    case "TAXONOMY_GENUS_ID":
//                        friendlyName = GetTaxonomyGenusNodeFriendlyName(sourceRow);
//                        break;
//                    case "CROP_ID":
//                        friendlyName = GetCropNodeFriendlyName(sourceRow);
//                        break;
//                    default:
//                        newdr = null;
//                        break;
//                }
//                if (newdr != null)
//                {
//                    newdr["COOPERATOR_ID"] = ux_comboboxCNO.SelectedValue.ToString();
//                    newdr["TAB_NAME"] = ux_tabcontrolGroupListNavigator.SelectedTab.Text;

//                    if (destinationNode == null)
//                    {
//                        newdr["LIST_NAME"] = ((TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0]).TopNode.Text;
//                    }
//                    else if(destinationNode.Tag.ToString().ToUpper() == "FOLDER")
//                    {
//                        newdr["LIST_NAME"] = destinationNode.FullPath;
//                    }
//                    else if (destinationNode.Parent != null &&
//                        destinationNode.Parent.Tag.ToString().ToUpper() == "FOLDER")
//                    {
//                        newdr["LIST_NAME"] = destinationNode.Parent.FullPath;
//                    }
//                    else if (destinationNode.Parent.Parent != null)
//                    {
//                        newdr["LIST_NAME"] = destinationNode.Parent.Parent.FullPath;
//                    }
//                    else
//                    {
//                        newdr["LIST_NAME"] = "";
//                    }
//                    newdr["ID_NUMBER"] = sourceRow[pKeyColumnName].ToString();
//                    newdr["ID_TYPE"] = pKeyColumnName;
//                    newdr["TITLE"] = friendlyName.Trim();
//                }
//            }

//            return newdr;        
//        }

//        private string GetAccessionNodeFriendlyName(DataRow sourceRow)
//        {
//            string friendlyName = "";
//            if (sourceRow.Table.Columns.Contains("ACCESSION_NUMBER_PART1") && !string.IsNullOrEmpty(sourceRow["ACCESSION_NUMBER_PART1"].ToString())) friendlyName += sourceRow["ACCESSION_NUMBER_PART1"].ToString();
//            if (sourceRow.Table.Columns.Contains("ACCESSION_NUMBER_PART2") && !string.IsNullOrEmpty(sourceRow["ACCESSION_NUMBER_PART2"].ToString())) friendlyName += "_" + sourceRow["ACCESSION_NUMBER_PART2"].ToString();
//            if (sourceRow.Table.Columns.Contains("ACCESSION_NUMBER_PART3") && !string.IsNullOrEmpty(sourceRow["ACCESSION_NUMBER_PART3"].ToString())) friendlyName += "_" + sourceRow["ACCESSION_NUMBER_PART3"].ToString();
//            if (string.IsNullOrEmpty(friendlyName)) friendlyName = sourceRow[sourceRow.Table.PrimaryKey[0]].ToString();
//            return friendlyName.Trim();
//        }

//        private string GetInventoryNodeFriendlyName(DataRow sourceRow)
//        {
//            string friendlyName = "";
//            if (sourceRow.Table.Columns.Contains("INVENTORY_NUMBER_PART1") && !string.IsNullOrEmpty(sourceRow["INVENTORY_NUMBER_PART1"].ToString())) friendlyName += sourceRow["INVENTORY_NUMBER_PART1"].ToString();
//            if (sourceRow.Table.Columns.Contains("INVENTORY_NUMBER_PART2") && !string.IsNullOrEmpty(sourceRow["INVENTORY_NUMBER_PART2"].ToString())) friendlyName += "_" + sourceRow["INVENTORY_NUMBER_PART2"].ToString();
//            if (sourceRow.Table.Columns.Contains("INVENTORY_NUMBER_PART3") && !string.IsNullOrEmpty(sourceRow["INVENTORY_NUMBER_PART3"].ToString())) friendlyName += "_" + sourceRow["INVENTORY_NUMBER_PART3"].ToString();
//            if (sourceRow.Table.Columns.Contains("FORM_TYPE_CODE") && !string.IsNullOrEmpty(sourceRow["FORM_TYPE_CODE"].ToString())) friendlyName += "_" + sourceRow["FORM_TYPE_CODE"].ToString();
//            if (string.IsNullOrEmpty(friendlyName)) friendlyName = sourceRow[sourceRow.Table.PrimaryKey[0]].ToString();
//            return friendlyName.Trim();
//        }

//        private string GetOrderRequestNodeFriendlyName(DataRow sourceRow)
//        {
//            string friendlyName = "";
//            if (sourceRow.Table.Columns.Contains("ORDER_REQUEST_ID") && !string.IsNullOrEmpty(sourceRow["ORDER_REQUEST_ID"].ToString())) friendlyName = sourceRow["ORDER_REQUEST_ID"].ToString();
//            if (string.IsNullOrEmpty(friendlyName)) friendlyName = sourceRow[sourceRow.Table.PrimaryKey[0]].ToString();
//            return friendlyName.Trim();
//        }

//        private string GetCooperatorNodeFriendlyName(DataRow sourceRow)
//        {
//            string friendlyName = "";
//            if (sourceRow.Table.Columns.Contains("LAST_NAME") && !string.IsNullOrEmpty(sourceRow["LAST_NAME"].ToString())) friendlyName += sourceRow["LAST_NAME"].ToString();
//            if (sourceRow.Table.Columns.Contains("FIRST_NAME") && !string.IsNullOrEmpty(sourceRow["FIRST_NAME"].ToString())) friendlyName += ", " + sourceRow["FIRST_NAME"].ToString();
//            if (sourceRow.Table.Columns.Contains("INITIALS") && !string.IsNullOrEmpty(sourceRow["INITIALS"].ToString())) friendlyName += " " + sourceRow["INITIALS"].ToString();
//            if (sourceRow.Table.Columns.Contains("ORGANIZATION_ABBREV") && !string.IsNullOrEmpty(sourceRow["ORGANIZATION_ABBREV"].ToString())) friendlyName += ", " + sourceRow["ORGANIZATION_ABBREV"].ToString();
//            if (string.IsNullOrEmpty(friendlyName)) friendlyName = sourceRow[sourceRow.Table.PrimaryKey[0]].ToString();
//            return friendlyName.Trim();
//        }

//        private string GetGeographyNodeFriendlyName(DataRow sourceRow)
//        {
//            string friendlyName = "";
//            if (sourceRow.Table.Columns.Contains("COUNTRY_CODE") && !string.IsNullOrEmpty(sourceRow["COUNTRY_CODE"].ToString())) friendlyName = sourceRow["COUNTRY_CODE"].ToString();
//            if (sourceRow.Table.Columns.Contains("ADM1") && !string.IsNullOrEmpty(sourceRow["ADM1"].ToString())) friendlyName += ", " + sourceRow["ADM1"].ToString();
//            if (string.IsNullOrEmpty(friendlyName)) friendlyName = sourceRow[sourceRow.Table.PrimaryKey[0]].ToString();
//            return friendlyName.Trim();
//        }

//        private string GetTaxonomyGenusNodeFriendlyName(DataRow sourceRow)
//        {
//            string friendlyName = "";
//            if (sourceRow.Table.Columns.Contains("GENUS_NAME") && !string.IsNullOrEmpty(sourceRow["GENUS_NAME"].ToString())) friendlyName = sourceRow["GENUS_NAME"].ToString();
//            if (sourceRow.Table.Columns.Contains("SUBGENUS_NAME") && !string.IsNullOrEmpty(sourceRow["SUBGENUS_NAME"].ToString())) friendlyName += " " + sourceRow["SUBGENUS_NAME"].ToString();
//            if (sourceRow.Table.Columns.Contains("SECTION_NAME") && !string.IsNullOrEmpty(sourceRow["SECTION_NAME"].ToString())) friendlyName += " " + sourceRow["SECTION_NAME"].ToString();
//            if (sourceRow.Table.Columns.Contains("SUBSECTION_NAME") && !string.IsNullOrEmpty(sourceRow["SUBSECTION_NAME"].ToString())) friendlyName += " " + sourceRow["SUBSECTION_NAME"].ToString();
//            if (string.IsNullOrEmpty(friendlyName)) friendlyName = sourceRow[sourceRow.Table.PrimaryKey[0]].ToString();
//            return friendlyName.Trim();
//        }

//        private string GetCropNodeFriendlyName(DataRow sourceRow)
//        {
//            string friendlyName = "";
//            if (sourceRow.Table.Columns.Contains("NAME") && !string.IsNullOrEmpty(sourceRow["NAME"].ToString())) friendlyName = sourceRow["NAME"].ToString();
//            if (string.IsNullOrEmpty(friendlyName)) friendlyName = sourceRow[sourceRow.Table.PrimaryKey[0]].ToString();
//            return friendlyName.Trim();
//        }

//        private void RenameTab(string SourceTabText, string DestinationTabText)
//        {
//            DataRow[] drc = userItemList.Select("TAB_NAME='" + SourceTabText.Replace("'", "''") +
//                                                "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//            // Delete each row from the local copy of the app_user_item_list table
//            foreach (DataRow dr in drc)
//            {
//                dr["TAB_NAME"] = DestinationTabText;
//            }
////// Save the changes to the centralized database...
////SaveLists(userItemList);
////// Now get the records from the central database to refresh the local copy...
////userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//        }


//        //private void MoveTreeNodes(string SourceTabText, string SourceNodePath, string DestinationTabText, string DestinationNodePath)
//        //{
//        //    DataRow[] drc = userItemList.Select("TAB_NAME='" + SourceTabText.Replace("'", "''") +
//        //                                        "' AND LIST_NAME LIKE '" + SourceNodePath.Replace("'", "''") +
//        //                                        "%' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//        //    // Delete each row from the local copy of the app_user_item_list table
//        //    foreach (DataRow dr in drc)
//        //    {
//        //        dr["LIST_NAME"] = ((string)dr["LIST_NAME"]).Replace(SourceNodePath, DestinationNodePath);
//        //    }
//        //    // Save the changes to the centralized database...
//        //    SaveLists(userItemList);
//        //    // Now get the records from the central database to refresh the local copy...
//        //    userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//        //}

//        private void ResetNodeFormat(TreeNode treeNode)
//        {
//            treeNode.NodeFont = null;
//            treeNode.BackColor = Color.Empty;
//            foreach (TreeNode tn in treeNode.Nodes)
//            {
//                ResetNodeFormat(tn);
//            }
//        }

//        private void RefreshUserItemListFormatting(string tabText, TreeNode treeNode)
//        {
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            // Find the user_item_list row corresponding to this node...
//            if (treeNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//            {                
//                // Get the collection of items in the folder...
//                DataRow[] drcItems = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
//                                                    "' AND LIST_NAME='" + treeNode.FullPath.Replace("'", "''") +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");

//                // First rename (title) the node items...
//                RefreshUserItemListTitles(drcItems);
//                // Now resort the items...
//                // Get the sort settings from the folder (actually it will come from the parent of the collection items)...
//                if (drcItems.Length > 0)
//                {
//                    string sortMode = GetUserItemListProperty("SORT_MODE", drcItems[0]);
//                    string folderGroupingMode = GetUserItemListProperty("FOLDER_GROUPING_MODE", drcItems[0]);
//                    SortUserItemList(tabText, treeNode, sortMode, folderGroupingMode == "TOP");
//                }
//            }
//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void RefreshUserItemListTitles(DataRow[] userItemListRows)
//        {
////value = "{get_accession.accession_number_part1} + \" \" + {get_accession.accession_number_part2} + \" \" + {get_accession.accession_number_part3}; ";
//            System.Collections.Generic.Dictionary<string, string> idTypeFormattingFormula = new System.Collections.Generic.Dictionary<string, string>();
//            System.Collections.Generic.List<string> idTypes = new System.Collections.Generic.List<string>();
//            System.Collections.Generic.Dictionary<string, string> idNumbers = new System.Collections.Generic.Dictionary<string, string>();
//            DataSet ds = new DataSet();

//            // First find all of the distinct ID_TYPES, 
//            // their corresponding formatting formulas,
//            // and gather all of the ID_NUMBERS for each ID_TYPE in the userItemList collection...
//            foreach (DataRow dr in userItemListRows)
//            {
//                if (dr["ID_TYPE"].ToString().Trim().ToUpper() != "FOLDER")
//                {
//                    // Get the ID_TYPE...
//                    if (!idTypes.Contains(dr["ID_TYPE"].ToString())) idTypes.Add(dr["ID_TYPE"].ToString());
//                    // Now get the formatting formula for each ID_TYPE in the collection...
//                    if (!idTypeFormattingFormula.ContainsKey(dr["ID_TYPE"].ToString())) idTypeFormattingFormula.Add(dr["ID_TYPE"].ToString(), GetUserItemListProperty(dr["ID_TYPE"].ToString().Trim().ToUpper() + "_NAME_FORMULA", dr));
//                    // Next collect all of the ID_NUMBERS for each of the ID_TYPES for the userItemList collection...
//                    if (!idNumbers.ContainsKey(dr["ID_TYPE"].ToString()))
//                    {
//                        idNumbers.Add(dr["ID_TYPE"].ToString(), dr["ID_NUMBER"].ToString() + ",");
//                    }
//                    else
//                    {
//                        idNumbers[dr["ID_TYPE"].ToString()] = idNumbers[dr["ID_TYPE"].ToString()] + dr["ID_NUMBER"].ToString() + ",";
//                    }
//                }
//            }

//            // Make all the trips to the server now to get all data needed for new userItemList titles...
//            foreach (string idType in idTypes)
//            {
//                string[] formatTokens = idTypeFormattingFormula[idType].Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries);
//                foreach (string formatToken in formatTokens)
//                {
//                    if (formatToken.Contains("{") &&
//                        formatToken.Contains("}"))
//                    {
//                        // This is a DB field used in the title - so go get it...
//                        string[] dataviewAndField = formatToken.Trim().Replace("{", "").Replace("}", "").Trim().Split(new char[] { '.' });
//                        if (!ds.Tables.Contains(dataviewAndField[0]))
//                        {
//                            DataSet newDS = _sharedUtils.GetWebServiceData(dataviewAndField[0], ":" + idType.Trim().ToLower().Replace("_", "") + "=" + idNumbers[idType].Trim().TrimEnd(','), 0, 0);
//                            if (newDS != null &&
//                                newDS.Tables.Contains(dataviewAndField[0]))
//                            {
//                                ds.Tables.Add(newDS.Tables[dataviewAndField[0]].Copy());
//                            }
//                        }
//                    }
//                }
//            }

//            // Now refresh the titles for each item in the collection...
//            foreach (DataRow dr in userItemListRows)
//            {
//                if (dr["ID_TYPE"].ToString().Trim().ToUpper() != "FOLDER")
//                {
//                    string title = "";
//                    string[] formatTokens = idTypeFormattingFormula[dr["ID_TYPE"].ToString()].Split(new string[] { " + " }, StringSplitOptions.RemoveEmptyEntries);
//                    foreach (string formatToken in formatTokens)
//                    {
//                        if (formatToken.Contains("{") &&
//                            formatToken.Contains("}"))
//                        {
//                            // This is a DB field used in the title - so go get it...
//                            string[] dataviewAndField = formatToken.Trim().Replace("{", "").Replace("}", "").Trim().Split(new char[] { '.' });
//                            if (dataviewAndField.Length == 2)
//                            {
//                                if (ds.Tables.Contains(dataviewAndField[0]) &&
//                                    ds.Tables[dataviewAndField[0]].Rows.Count > 0 &&
//                                    ds.Tables[dataviewAndField[0]].Columns.Contains(dataviewAndField[1]))
//                                {
//                                    // The list of dataviews that can be used for building node titles must have a pkey that is one of the ID_TYPES
//                                    // otherwise this next line won't work...
//                                    DataRow tokenRawDataRow = ds.Tables[dataviewAndField[0]].Rows.Find(dr["ID_NUMBER"].ToString());
//                                    if (tokenRawDataRow != null)
//                                    {
//                                        DataColumn dc = tokenRawDataRow.Table.Columns[dataviewAndField[1]];
//                                        string valueMember = tokenRawDataRow[dataviewAndField[1]].ToString();
//                                        if (_sharedUtils.LookupTablesIsValidFKField(dc) &&
//                                            !string.IsNullOrEmpty(valueMember))
//                                        {
//                                            title += _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), valueMember, "", valueMember);
//                                        }
//                                        else if (_sharedUtils.LookupTablesIsValidCodeValueField(dc) &&
//                                                !string.IsNullOrEmpty(valueMember))
//                                        {
//                                            title += _sharedUtils.GetLookupDisplayMember("code_value_lookup", valueMember, "", valueMember);
//                                        }
//                                        else
//                                        {
//                                            title += valueMember;
//                                        }
//                                    }
// /* 
//else if (dc.ExtendedProperties.Contains("gui_hint") && dc.ExtendedProperties["gui_hint"].ToString().ToUpper().Trim() == "SMALL_SINGLE_SELECT_CONTROL" &&
//        dc.ExtendedProperties.Contains("group_name") && dc.ExtendedProperties["group_name"].ToString().Length > 0)
//        {
//            if (dr[dc.ColumnName] != DBNull.Value)
//            {
//                newDataRow[dc.ColumnName] = lookupTables.GetDisplayMember("code_value_lookup", dr[dc.ColumnName].ToString(), "group_name='" + dc.ExtendedProperties["group_name"].ToString() + "'", dr[dc.ColumnName].ToString());
//            }
//        }

//if (_sharedUtils.LookupTablesIsValidFKField(dc) && 
//    e.RowIndex < dv.Count &&
//    dv[e.RowIndex].Row.RowState != DataRowState.Deleted)
//    {
//        if (dv[e.RowIndex][e.ColumnIndex] != DBNull.Value)
//        {
//            e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
//        }
//        dgv[e.ColumnIndex, e.RowIndex].ErrorText = dv[e.RowIndex].Row.GetColumnError(dc);
//        e.FormattingApplied = true;
//    }
                                
// */                                
//                                }
//                            }
//                        }
//                        else
//                        {
//                            title += formatToken.Trim().Replace("\"", "");
//                        }
//                    }
//                    // If there was success in building a new node title and it is a different name than the old title, make sure it is unique and then replace the old title...
//                    if (!string.IsNullOrEmpty(title))
//                    {
//                        if (dr["TITLE"].ToString() != title.Trim())
//                        {
//                            title = EnsureUniqueUserItemListTitleText(userItemListRows, title.Trim());
//                            dr["TITLE"] = title.Trim();
//                        }
//                    }
//                }
//            }
//        }

//        private string EnsureUniqueUserItemListTitleText(DataRow[] userItemListRows, string title)
//        {
//            // Now we can get a unique name for this tree node (starting with the default node name passed in)...
//            String uniqueTitleText = title.Trim();

//            // Let's make sure the new node name is unique..
//            bool duplicateText = true;
//            int i = 1;
//            while (duplicateText)
//            {
//                // Assume this name is unique (until proven otherwise)
//                duplicateText = false;
//                foreach (DataRow dr in userItemListRows)
//                {
//                    if (dr["TITLE"].ToString().Trim() == uniqueTitleText)
//                    {
//                        // Nope...  This is not a unique node name so increment a counter until it is unique...
//                        uniqueTitleText = title + " (" + i++.ToString() + ")";
//                        duplicateText = true;
//                    }
//                }
//            }
//            return uniqueTitleText;
//        }

//        private string GetUserItemListProperty(string propertyName, DataRow userItemListRow)
//        {
//            string value = "";
//            if (userItemListRow.Table.Columns.Contains("properties"))
//            {
//                string nodeProperties = userItemListRow["properties"].ToString();
//                if (nodeProperties.Contains(propertyName))
//                {
//                    // Found a setting for this property - parse it out of the properties string...
//                    // Find the start of the value...
//                    int startIndex = nodeProperties.IndexOf("=", nodeProperties.IndexOf(propertyName)) + 1;
//                    // Find the end of the value...
//                    int stringLength = nodeProperties.IndexOf(";", startIndex) - startIndex;
//                    value = nodeProperties.Substring(startIndex, stringLength);
//                }
//                else
//                {
//                    // Could not find the property at this node level - look at an ancestor for the setting...
//                    string currentPath = userItemListRow["LIST_NAME"].ToString();
//                    if (currentPath.Contains("|"))
//                    {
//                        string parentPath = currentPath.Substring(0, currentPath.LastIndexOf('|')).Trim().TrimEnd(new char[] { '|' }).Replace("'", "''");
//                        string parentName = currentPath.Substring(currentPath.LastIndexOf('|') + 1, currentPath.Length - (currentPath.LastIndexOf('|') + 1)).Trim().TrimStart(new char[] { '|' }).Replace("'", "''");
//                        DataRow[] parentRows = userItemList.Select("ID_TYPE='FOLDER' AND LIST_NAME = '" + parentPath + "' AND TITLE = '" + parentName + "'");
//                        if (!parentRows[0]["properties"].ToString().Contains(propertyName))
//                        {
//                            // Haven't found the property setting in an ancestor yet - keep looking...
//                            value = GetUserItemListProperty(propertyName, parentRows[0]);
//                        }
//                        else
//                        {
//                            // Found the key/value pair!!!
//                            // Find the start of the value...
//                            int startIndex = parentRows[0]["properties"].ToString().IndexOf("=", parentRows[0]["properties"].ToString().IndexOf(propertyName)) + 1;
//                            // Find the end of the value...
//                            int stringLength = parentRows[0]["properties"].ToString().IndexOf(";", startIndex) - startIndex;
//                            value = parentRows[0]["properties"].ToString().Substring(startIndex, stringLength);
//                        }
//                    }
//                    else if (!currentPath.Contains("{DBNull.Value}"))
//                    {
//                        string parentPath = "{DBNull.Value}";
//                        string parentName = currentPath;
//                        DataRow[] parentRows = userItemList.Select("ID_TYPE='FOLDER' AND LIST_NAME = '" + parentPath + "' AND TITLE = '" + parentName + "'");
//                        if (!parentRows[0]["properties"].ToString().Contains(propertyName))
//                        {
//                            // Haven't found the property setting in an ancestor yet - keep looking...
//                            value = GetUserItemListProperty(propertyName, parentRows[0]);
//                        }
//                        else
//                        {
//                            // Found the key/value pair!!!
//                            // Find the start of the value...
//                            int startIndex = parentRows[0]["properties"].ToString().IndexOf("=", parentRows[0]["properties"].ToString().IndexOf(propertyName)) + 1;
//                            // Find the end of the value...
//                            int stringLength = parentRows[0]["properties"].ToString().IndexOf(";", startIndex) - startIndex;
//                            value = parentRows[0]["properties"].ToString().Substring(startIndex, stringLength);
//                        }
//                    }
//                    else
//                    {
//                        // This is the end of the road... return the default name formula...
//                        #region old_code...
//                        //switch (propertyName.Trim().ToUpper())
//                        //{
//                        //    case "SORT_MODE":
//                        //        value = "MANUAL";
//                        //        break;
//                        //    case "FOLDER_GROUPING_MODE":
//                        //        value = "TOP";
//                        //        break;
//                        //    case "ACCESSION_ID_NAME_FORMULA":
//                        //        value = "{get_accession.accession_number_part1} + \" \" + {get_accession.accession_number_part2} + \" \" + {get_accession.accession_number_part3}";
//                        //        break;
//                        //    case "INVENTORY_ID_NAME_FORMULA":
//                        //        value = "{get_inventory.inventory_number_part1} + \" \" + {get_inventory.inventory_number_part2} + \" \" + {get_inventory.inventory_number_part3} + \" \" + {get_inventory.form_type_code}";
//                        //        break;
//                        //    case "ORDER_REQUEST_ID_NAME_FORMULA":
//                        //        value = "{get_order_request.order_request_id}";
//                        //        break;
//                        //    case "COOPERATOR_ID_NAME_FORMULA":
//                        //        value = "{get_cooperator.last_name} + \", \" + {get_cooperator.first_name} + \" \" + {get_cooperator.organization_abbrev}";
//                        //        break;
//                        //    case "TAXONOMY_GENUS_ID_NAME_FORMULA":
//                        //        value = "{get_taxonomy_genus.genus_name} + \" \" + {get_taxonomy_genus.subgenus_name} + \" \" + {get_taxonomy_genus.section_name} + \" \" + {get_taxonomy_genus.subsection_name}";
//                        //        break;
//                        //    case "GEOGRAPHY_ID_NAME_FORMULA":
//                        //        value = "{get_geography.country_code} + \" \" + {get_geography.adm1}";
//                        //        break;
//                        //    case "CROP_ID_NAME_FORMULA":
//                        //        value = "{get_crop.name}";
//                        //        break;
//                        //    default:
//                        //        break;
//                        //}
//                        #endregion
//                         value = _sharedUtils.GetAppSettingValue(propertyName);
//                   }
//                }
//            }
//            return value.Trim();
//        }

//        private void SortUserItemList(string tabText, TreeNode treeNode, string sortMode, bool sortFoldersAtTop)
//        {
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            if (sortMode.Trim().ToUpper() == "MANUAL")
//            {
//                TreeNode folderToSort = treeNode;
//                if (treeNode.Tag.ToString().Trim().ToUpper() != "FOLDER" &&
//                    treeNode.Parent != null)
//                {
//                    folderToSort = treeNode.Parent;
//                }
//                DataRow[] drc = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
//                                                    "' AND LIST_NAME='" + folderToSort.FullPath.Replace("'", "''") +
////"' AND LIST_NAME like '" + folderToSort.FullPath.Replace("'", "''") + "%" +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                int i = 0;
//                foreach (TreeNode tn in folderToSort.Nodes)
//                {
//                    foreach (DataRow dr in drc)
//                    {
//                        //if (dr["TITLE"].ToString().Trim() == tn.Text.Trim())
//                        if (dr["ID_TYPE"].ToString().ToUpper() == "FOLDER" &&
//                            dr["LIST_NAME"].ToString() == tn.FullPath)
//                        {
//                            dr["SORT_ORDER"] = i;
//                        }
//                        else if (dr["LIST_NAME"].ToString() + "|" + dr["TITLE"].ToString() == tn.FullPath)
//                        {
//                            dr["SORT_ORDER"] = i;
//                        }
//                    }
//                    i++;
//                }
//            }
//            else if (treeNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//            {
//                DataRow[] drcItems = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
//                                                    "' AND LIST_NAME ='" + treeNode.FullPath.Replace("'", "''") +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + 
//                                                    "' AND ID_TYPE<>'FOLDER'");

//                DataRow[] drcFolders = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
//                                                    "' AND LIST_NAME ='" + treeNode.FullPath.Replace("'", "''") +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() +
//                                                    "' AND ID_TYPE='FOLDER'");

//                // Create a new list of the user item list datarows that will be used for sorting...
//                System.Collections.ArrayList alItems = new System.Collections.ArrayList(drcItems.Length);
//                foreach (DataRow dr in drcItems)
//                {
//                    alItems.Add(dr);
//                }

//                // Create a new list of the folder datarows that will be used for sorting...
//                System.Collections.ArrayList alFolders = new System.Collections.ArrayList(drcFolders.Length);
//                foreach (DataRow dr in drcFolders)
//                {
//                    alFolders.Add(dr);
//                }

//                // Instantiate a new datarow comparer and use it to sort the array list of ITEMS and FOLDERS...
//                UserItemListRowSorter uilrs = new UserItemListRowSorter();
//                bool sortAscending = true;
//                if (sortMode.Trim().ToUpper() == "DESCENDING") sortAscending = false;
//                uilrs.SortAscending = sortAscending;
//                alItems.Sort(uilrs);
//                alFolders.Sort(uilrs);

//                int i = 0;
//                if (sortFoldersAtTop)
//                {
//                    // Processing in the new sorted order... Find the associated FOLDER nodes in the collection, remove them
//                    // and then re-add them back to the collection.  Finally, update the user item list's sort order field with the new sorting order
//                    i = 0;
//                    foreach (DataRow dr in alFolders)
//                    {
////TreeNode[] tnCollection = treeNode.Nodes.Find(dr["TITLE"].ToString(), false);
////foreach (TreeNode tn in tnCollection)
////{
////    if (tn.Tag != null &&
////        tn.Tag.ToString() == dr["ID_TYPE"].ToString())
////    {
////        tn.Remove();
////        treeNode.Nodes.Add(tn);
////    }
////}
//                        dr["SORT_ORDER"] = i++;
//                    }
//                }
//                // Processing in the new sorted order... Find the associated ITEM nodes in the collection, remove them
//                // and then re-add them back to the collection.  Finally, update the user item list's sort order field with the new sorting order
//                foreach (DataRow dr in alItems)
//                {
////TreeNode[] tnCollection = treeNode.Nodes.Find(dr["ID_NUMBER"].ToString(), false);
////foreach (TreeNode tn in tnCollection)
////{
////    if (tn.Tag != null &&
////        tn.Tag.ToString() == dr["ID_TYPE"].ToString())
////    {
////        tn.Remove();
////        treeNode.Nodes.Add(tn);
////    }
////}
//                    dr["SORT_ORDER"] = i++;
//                }
//                if (!sortFoldersAtTop)
//                {
//                    // Processing in the new sorted order... Find the associated FOLDER nodes in the collection, remove them
//                    // and then re-add them back to the collection.  Finally, update the user item list's sort order field with the new sorting order
//                    foreach (DataRow dr in alFolders)
//                    {
////TreeNode[] tnCollection = treeNode.Nodes.Find(dr["TITLE"].ToString(), false);
////foreach (TreeNode tn in tnCollection)
////{
////    if (tn.Tag != null &&
////        tn.Tag.ToString() == dr["ID_TYPE"].ToString())
////    {
////        tn.Remove();
////        treeNode.Nodes.Add(tn);
////    }
////}
//                        dr["SORT_ORDER"] = i++;
//                    }
//                }
//            }

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        public class UserItemListRowSorter : System.Collections.IComparer
//        {
//            private bool _sortAscending = true;

//            public bool SortAscending
//            {
//                get
//                {
//                    return _sortAscending;
//                }
//                set
//                {
//                    _sortAscending = value;
//                }
//            }

//            #region IComparer Members

//            public int Compare(object x, object y)
//            {
//                DataRow drX = (DataRow)x;
//                DataRow drY = (DataRow)y;
//                if (drX.Table.Columns.Contains("TITLE") &&
//                    drY.Table.Columns.Contains("TITLE"))
//                {
//                    if (_sortAscending)
//                    {
//                        return string.Compare(drX["TITLE"].ToString().Trim(), drY["TITLE"].ToString().Trim());
//                    }
//                    else
//                    {
//                        return -1 * string.Compare(drX["TITLE"].ToString().Trim(), drY["TITLE"].ToString().Trim());
//                    }
//                }
//                else
//                {
//                    return 0;
//                }
//            }

//            #endregion
//        }

//        private void RenameTreeNodes(string tabText, TreeNode treeNode, string newName)
//        {
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;
//            string origNodeText = treeNode.Text;

//            // Build the new FullPath (using the newName)...
//            treeNode.Text = newName;
//            string newFullPath = treeNode.FullPath;
//            // Now restore the node text back to original...
//            treeNode.Text = origNodeText;

//            if (treeNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//            {
//                // First rename the folder...
//                string parentFullPath = "{DBNull.Value}";
//                if (treeNode.Parent != null) parentFullPath = treeNode.Parent.FullPath;
//                DataRow[] drc = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
//                                                    "' AND LIST_NAME = '" + parentFullPath.Replace("'", "''") +
//                                                    "' AND TITLE = '" + treeNode.Text.Replace("'", "''") +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                // Update each row from the local copy of the app_user_item_list table
//                foreach (DataRow dr in drc)
//                {
//                    dr["TITLE"] = newName.Trim();
//                }

//                // Then rename the contents of the folder...
//                drc = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
//                                            "' AND LIST_NAME LIKE '" + treeNode.FullPath.Replace("'", "''") +
//                                            "%' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                // Update each row from the local copy of the app_user_item_list table
//                foreach (DataRow dr in drc)
//                {
//                    dr["LIST_NAME"] = newFullPath + dr["LIST_NAME"].ToString().Substring(treeNode.FullPath.Length);
//                }

////DataRow[] drc = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
////                                    "' AND LIST_NAME LIKE '" + treeNode.FullPath.Replace("'", "''") +
////                                    "%' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
////// Update each row from the local copy of the app_user_item_list table
////// with a new concatenated name that replaces the old root path with the new root path...
////foreach (DataRow dr in drc)
////{
////    // First update the stored display text for this folder...
////    if (dr["LIST_NAME"].ToString() == treeNode.Parent.FullPath &&
////        dr["TITLE"].ToString().Trim() == origNodeText &&
////        dr["ID_TYPE"].ToString().ToUpper() == "FOLDER")
////    {
////        dr["TITLE"] = newName.Trim();
////    }
////    dr["TAB_NAME"] = tabText;
////    dr["LIST_NAME"] = newFullPath + dr["LIST_NAME"].ToString().Substring(treeNode.Parent.FullPath.Length);
////}
//            }
//            else
//            {
//                if (treeNode.Parent != null)
//                {
//                    DataRow[] drc = userItemList.Select("TAB_NAME='" + tabText.Replace("'", "''") +
//                                                        "' AND LIST_NAME='" + treeNode.Parent.FullPath.Replace("'", "''") +
//                                                        "' AND ID_TYPE='" + treeNode.Tag.ToString().Replace("'", "''") +
//                                                        "' AND ID_NUMBER='" + treeNode.Name.Replace("'", "''") +
//                                                        "' AND TITLE='" + treeNode.Text.Replace("'", "''") +
//                                                        "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
////// Rename the node (so that the FullPath is correct during the update)
////node.Text = newName;
//                    // Update the row (there should only be one) from the local copy of the app_user_item_list table...
//                    foreach (DataRow dr in drc)
//                    {
////dr["TAB_NAME"] = tabText;
////dr["LIST_NAME"] = node.Parent.FullPath;
//                        dr["TITLE"] = newName.Trim();
//                    }
//                }
//            }

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void MoveTreeNodes(string SourceTabText, TreeNode SourceNode, string DestinationTabText, TreeNode DestinationNode)
//        {
//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            if (SourceNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//            {
//                // First move the folder...
//                string parentFullPath = "{DBNull.Value}";
//                if (SourceNode.Parent != null) parentFullPath = SourceNode.Parent.FullPath;
//                DataRow[] drc = userItemList.Select("TAB_NAME='" + SourceTabText.Replace("'", "''") +
//                                                    "' AND LIST_NAME = '" + parentFullPath.Replace("'", "''") +
//                                                    "' AND TITLE = '" + SourceNode.Text.Trim().Replace("'", "''") +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                // Update each row from the local copy of the app_user_item_list table
//                foreach (DataRow dr in drc)
//                {
//                    dr["TAB_NAME"] = DestinationTabText;
//                    dr["LIST_NAME"] = DestinationNode.Parent.FullPath; // + dr["LIST_NAME"].ToString().Substring(SourceNode.Parent.FullPath.Length);
//                    dr["TITLE"] = DestinationNode.Text.Trim();
//                }

//                // Then move the contents of the folder...
//                drc = userItemList.Select("TAB_NAME='" + SourceTabText.Replace("'", "''") +
//                                            "' AND LIST_NAME LIKE '" + SourceNode.FullPath.Replace("'", "''") +
//                                            "%' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                // Update each row from the local copy of the app_user_item_list table
//                foreach (DataRow dr in drc)
//                {
//                    dr["TAB_NAME"] = DestinationTabText;
//                    dr["LIST_NAME"] = DestinationNode.FullPath + dr["LIST_NAME"].ToString().Substring(SourceNode.FullPath.Length);
//                    dr["TITLE"] = DestinationNode.Text.Trim();
//                }
//            }
//            else
//            {
//                if (SourceNode.Parent != null &&
//                    DestinationNode.Parent != null)
//                {
//                    DataRow[] drc = userItemList.Select("TAB_NAME='" + SourceTabText.Replace("'", "''") +
//                                                        "' AND LIST_NAME='" + SourceNode.Parent.FullPath.Replace("'", "''") +
//                                                        "' AND ID_TYPE='" + SourceNode.Tag.ToString().Replace("'", "''") +
//                                                        "' AND ID_NUMBER='" + SourceNode.Name.Replace("'", "''") +
//                                                        "' AND TITLE='" + SourceNode.Text.Trim().Replace("'", "''") +
//                                                        "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                    // Update each row from the local copy of the app_user_item_list table
//                    foreach (DataRow dr in drc)
//                    {
//                        dr["TAB_NAME"] = DestinationTabText;
//                        dr["LIST_NAME"] = DestinationNode.Parent.FullPath;
//                        dr["TITLE"] = DestinationNode.Text.Trim();
//                    }
//                }
//            }

////// Save the changes to the centralized database...
////SaveLists(userItemList);
////// Now get the records from the central database to refresh the local copy...
////userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        /*
//                private void AddTreeNodes(TreeNode NewNode)
//                {
//                    // Change cursor to the wait cursor...
//                    Cursor origCursor = Cursor.Current;
//                    Cursor.Current = Cursors.WaitCursor;
            
//                    // Create an empty copy of the groups data table...
//        //DataTable newUserItemList = userItemList.Clone();
//                    if (NewNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//                    {
//                        // This is a folder so let's go through each object in the folder...
//                        foreach (TreeNode tn in NewNode.Nodes)
//                        {
//                            if (tn.Tag.ToString().Trim().ToUpper() == "FOLDER")
//                            {
//                                AddTreeNodes(tn);
//                            }
//                            else
//                            {
//                                if (tn.Name.Trim().ToUpper() != "!!DUMMYNODE!!")
//                                {
//        //DataRow newdr = newUserItemList.NewRow();
//                                    DataRow newdr = userItemList.NewRow();
//                                    newdr["COOPERATOR_ID"] = ux_comboboxCNO.SelectedValue.ToString();
//                                    newdr["TAB_NAME"] = ((TabControl)ux_splitcontainerMain.Panel1.Controls["ux_tabcontrolGroupListNavigator"]).SelectedTab.Text; //"Amaranth";
//                                    newdr["LIST_NAME"] = tn.Parent.FullPath;
//                                    newdr["ID_NUMBER"] = tn.Name;
//                                    newdr["ID_TYPE"] = (string)tn.Tag;
//                                    if (newdr.Table.Columns.Contains("TITLE")) newdr["TITLE"] = tn.Text.Trim();
//                                    if (newdr.Table.Columns.Contains("FRIENDLY_NAME")) newdr["FRIENDLY_NAME"] = tn.Text;
//                                    //newdr["SITE"] = ""; //strSite;
//        //newUserItemList.Rows.Add(newdr);
//                                    userItemList.Rows.Add(newdr);
//                                }
//                            }
//                        }
//                    }
//                    else
//                    {
//                        // This is an Accession, Inventory, Order Request, or Cooperator so just add it directly to the user's list...
//        //DataRow newdr = newUserItemList.NewRow();
//                        DataRow newdr = userItemList.NewRow();
//                        newdr["COOPERATOR_ID"] = ux_comboboxCNO.SelectedValue.ToString();
//                        newdr["TAB_NAME"] = ((TabControl)ux_splitcontainerMain.Panel1.Controls["ux_tabcontrolGroupListNavigator"]).SelectedTab.Text; //"Amaranth";
//                        newdr["LIST_NAME"] = NewNode.Parent.FullPath;
//                        newdr["ID_NUMBER"] = NewNode.Name;
//                        newdr["ID_TYPE"] = (string)NewNode.Tag;
//                        if (newdr.Table.Columns.Contains("TITLE")) newdr["TITLE"] = NewNode.Text;
//                        if (newdr.Table.Columns.Contains("FRIENDLY_NAME")) newdr["FRIENDLY_NAME"] = NewNode.Text;
//                        //newdr["SITE"] = ""; //strSite;
//        //newUserItemList.Rows.Add(newdr);
//                        userItemList.Rows.Add(newdr);
//                    }

//        //// If any rows were created save them now...
//        //if (newUserItemList.Rows.Count > 0)
//        //{
//        //    DataSet errors;
//        //    // Save the new nodes to the central database...
//        //    errors = SaveLists(newUserItemList);
//        //    // Now get the records from the central database to refresh the local copy...
//        //    userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());
//        //}

//                    // Restore cursor to default cursor...
//                    Cursor.Current = origCursor;
//                }
//        */

//        private void AddTreeNodes(TreeNode newNode)
//        {
//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//if (newNode.Name.Trim().ToUpper() != "!!DUMMYNODE!!") AddUserItemListRow(newNode);

//if (newNode.Tag.ToString().Trim().ToUpper() == "FOLDER")
//{
//    // This is a folder so let's go through each object in the folder...
//    foreach (TreeNode tn in newNode.Nodes)
//    {
//        if (tn.Tag.ToString().Trim().ToUpper() == "FOLDER")
//        {
//            AddTreeNodes(tn);
//        }
//        else
//        {
//            if (tn.Name.Trim().ToUpper() != "!!DUMMYNODE!!") AddUserItemListRow(tn);
//        }
//    }
//}
////else
////{
////    if (newNode.Name.Trim().ToUpper() != "!!DUMMYNODE!!") AddUserItemListRow(newNode);
////}

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//        }

//        private void DeleteTreeNodes(string SourceTab, TreeNode SourceNode)
//        {
//            if (SourceNode.Tag != null &&
//                SourceNode.Tag.ToString().ToUpper() == "FOLDER")
//            {
//                // First delete the folder...
//                string parentFullPath = "{DBNull.Value}";
//                if (SourceNode.Parent != null) parentFullPath = SourceNode.Parent.FullPath;
//                DataRow[] drc = userItemList.Select("TAB_NAME='" + SourceTab.Replace("'", "''") +
//                                                    "' AND LIST_NAME = '" + parentFullPath.Replace("'", "''") +
//                                                    "' AND TITLE = '" + SourceNode.Text.Replace("'", "''") +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                // Update each row from the local copy of the app_user_item_list table
//                foreach (DataRow dr in drc)
//                {
//                    dr.Delete();
//                }

//                // Then delete the contents of the folder...
//                drc = userItemList.Select("TAB_NAME='" + SourceTab.Replace("'", "''") +
//                                            "' AND LIST_NAME LIKE '" + SourceNode.FullPath.Replace("'", "''") +
//                                            "%' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                // Update each row from the local copy of the app_user_item_list table
//                foreach (DataRow dr in drc)
//                {
//                    dr.Delete();
//                }

////                //Get the rows in this list (which should include all children folders too)...
////                DataRow[] drc = userItemList.Select("TAB_NAME='" + SourceTab.Replace("'", "''") +
////                                                    "' AND LIST_NAME LIKE '" + SourceNode.FullPath.Replace("'", "''").Replace("[", "[[]").Replace("*", "[*]").Replace("%", @"[%]") +
////                                                    "%' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
////                // Delete each row from the local copy of the app_user_item_list table
////                foreach (DataRow dr in drc)
////                {
////                    dr.Delete();
////                }
//////// Save the changes to the centralized database...
//////SaveLists(userItemList);
//////userItemList.AcceptChanges();
//            }
//            else
//            {
//                string folderPath = SourceNode.FullPath.Remove(SourceNode.FullPath.LastIndexOf(SourceNode.TreeView.PathSeparator));
//                // This should be only one row (since it is not a folder it should not contain children)...
//                DataRow[] drc = userItemList.Select("TAB_NAME='" + SourceTab.Replace("'", "''") +
//                                                    "' AND LIST_NAME='" + folderPath.Replace("'", "''") +
//                                                    "' AND ID_TYPE='" + SourceNode.Tag +
//                                                    "' AND ID_NUMBER='" + SourceNode.Name.Replace("'", "''") +
//                                                    "' AND TITLE='" + SourceNode.Text.Replace("'", "''") +
//                                                    "' AND COOPERATOR_ID='" + ux_comboboxCNO.SelectedValue.ToString() + "'");
//                // Delete each row from the local copy of the app_user_item_list table
//                foreach (DataRow dr in drc)
//                {
//                    dr.Delete();
//                }
//////// Save the changes to the centralized database...
//////SaveLists(userItemList);
//////userItemList.AcceptChanges();
//            }
//        }

//        private DataTable GetLists(string cooperatorID)
//        {
//            DataTable dt = null;
//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            //DataSet dsListsData = GRINGlobalWebServices.GetData("get_lists", ":accessionid=; :inventoryid=; :orderrequestid=; :cooperatorid=" + cooperatorID, 0, 0);
//            DataSet dsListsData = _sharedUtils.GetWebServiceData("get_lists", ":cooperatorid=" + cooperatorID, 0, 0);
//            if (dsListsData.Tables.Contains("get_lists"))
//            {
////// Clear out the old table data...
////dt = null;

//                // Set the user Item List with the data returned from the remote DB...
//                dt = dsListsData.Tables["get_lists"].Copy();

////// Set the primary key for the table...
////System.Collections.Generic.List<DataColumn> pKeys = new System.Collections.Generic.List<DataColumn>();
////foreach (DataColumn dc in dt.Columns)
////{
////    // Add the column to the primary key list if it is a primary key for the dataview...
////    if (dc.ExtendedProperties.Contains("is_primary_key") &&
////        dc.ExtendedProperties["is_primary_key"].ToString() == "Y")
////    {
////        pKeys.Add(dc);
////    }
////    // Set the autoincrement property indicated (typically for the primary key)...
////    if (dc.ExtendedProperties.Contains("is_autoincrement") &&
////        dc.ExtendedProperties["is_autoincrement"].ToString() == "Y")
////    {
////        dc.AutoIncrement = true;
////        dc.AutoIncrementSeed = -1;
////        dc.AutoIncrementStep = -1;
////    }
////}
////dt.PrimaryKey = pKeys.ToArray();
////dt.AcceptChanges();

//                // Set the sort order...
//                dt.DefaultView.Sort = "LIST_NAME ASC, SORT_ORDER ASC, TITLE ASC";
//            }
//            else if (dsListsData.Tables.Contains("ExceptionTable") &&
//                    dsListsData.Tables["ExceptionTable"].Rows.Count > 0)
//            {
////MessageBox.Show("There were errors retrieving user lists.\n\nFull error message:\n" + dsListsData.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors retrieving user lists.\n\nFull error message:\n{0}", "Get User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "GetListsMessage1";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dsListsData.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
//ggMessageBox.ShowDialog();
//            }

//            dsListsData.Dispose();
//            dsListsData = null;

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
//            return dt;
//        }

//        private DataSet SaveLists(DataTable currentUserItemList)
//        {
//            DataSet modifiedData = new DataSet();
//            DataSet saveErrors = new DataSet();

//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            // Reload the user's item list from the remote database (so that we can sync to it)...
////DataTable syncedItemList = GetLists(cno);
//            if (ux_comboboxCNO.SelectedValue != null)
//            {
//                DataTable syncedItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());

//                if (syncedItemList != null)
//                {
//                    // Syncronize the current user's list with the settings retrieved from the remote DB...
//                    foreach (DataRow dr in currentUserItemList.Rows)
//                    {
//                        DataRow syncDR = null;

//                        if (dr.RowState == DataRowState.Deleted)
//                        {
//                            // Attempt to find the row in the sync datatable for this row (use the original pkey to handle deleted rows in current User Item List)...
//                            syncDR = syncedItemList.Rows.Find(dr[dr.Table.PrimaryKey[0].ColumnName, DataRowVersion.Original]);
//                            // If this row was deleted in the current session and it still exists on the remote DB - delete it...
//                            if (syncDR != null)
//                            {
//                                syncDR.Delete();
//                            }
//                        }
//                        else
//                        {
//                            // Attempt to find the row in the sync datatable for this row...
//                            syncDR = syncedItemList.Rows.Find(dr[dr.Table.PrimaryKey[0].ColumnName, DataRowVersion.Current]);
//                            // If this row exist in the current session
//                            if (syncDR == null)
//                            {
//                                // Create a new record...
//                                syncDR = syncedItemList.NewRow();
//                                syncedItemList.Rows.Add(syncDR);
//                            }
//                            if (syncDR["COOPERATOR_ID"].ToString() != dr["COOPERATOR_ID"].ToString()) syncDR["COOPERATOR_ID"] = dr["COOPERATOR_ID"];
//                            if (syncDR["TAB_NAME"].ToString() != dr["TAB_NAME"].ToString()) syncDR["TAB_NAME"] = dr["TAB_NAME"];
//                            if (syncDR["LIST_NAME"].ToString() != dr["LIST_NAME"].ToString()) syncDR["LIST_NAME"] = dr["LIST_NAME"];
//                            if (syncDR["ID_NUMBER"].ToString() != dr["ID_NUMBER"].ToString()) syncDR["ID_NUMBER"] = dr["ID_NUMBER"];
//                            if (syncDR["ID_TYPE"].ToString() != dr["ID_TYPE"].ToString()) syncDR["ID_TYPE"] = dr["ID_TYPE"];
//                            if (syncDR["SORT_ORDER"].ToString() != dr["SORT_ORDER"].ToString()) syncDR["SORT_ORDER"] = dr["SORT_ORDER"];
//                            if (syncDR["TITLE"].ToString() != dr["TITLE"].ToString()) syncDR["TITLE"] = dr["TITLE"].ToString().Trim();
//                            if (syncDR["DESCRIPTION"].ToString() != dr["DESCRIPTION"].ToString()) syncDR["DESCRIPTION"] = dr["DESCRIPTION"];
//                            if (syncDR["PROPERTIES"].ToString() != dr["PROPERTIES"].ToString()) syncDR["PROPERTIES"] = dr["PROPERTIES"];
//                            //if (syncDR["SITE"].ToString() != dr["SITE"].ToString()) syncDR["SITE"] = ""; //strSite;
//                        }
//                    }

//                    // Get just the rows that have changed and put them in to a new dataset...
//                    if (syncedItemList.GetChanges() != null)
//                    {
//                        modifiedData.Tables.Add(syncedItemList.GetChanges());
//                    }
//                    // Call the web method to update the changed data...
//                    saveErrors = _sharedUtils.SaveWebServiceData(modifiedData);

//                    // If the commandline during application startup had a parameter for _saveListDataDumpFile set to a valid filepath 
//                    // save the data to this file in XML format...
//                    if (!string.IsNullOrEmpty(_saveListDataDumpFile))
//                    {
//                        try
//                        {
//                            modifiedData.WriteXml(_saveDGVDataDumpFile, XmlWriteMode.WriteSchema);
//                        }
//                        catch (Exception err)
//                        {
////MessageBox.Show("Error attempting to save XML dataset to: " + _saveListDataDumpFile + "\n\nError Message:\n" + err.Message);
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error attempting to save XML dataset to: {0}\n\nError Message:\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "SaveListsMessage1";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
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
//ggMessageBox.ShowDialog();
//                        }
//                    }

//                    if (saveErrors != null &&
//                        saveErrors.Tables.Contains(syncedItemList.TableName))
//                    {
//                        foreach (DataRow dr in saveErrors.Tables[syncedItemList.TableName].Rows)
//                        {
//                            if (dr["SavedAction"].ToString() == "Insert" && dr["SavedStatus"].ToString() != "Success")
//                            {
////MessageBox.Show("The " + dr["TITLE"].ToString() + " item could not be successfully added to your list.\n\nError message:\n\n" + dr["ExceptionMessage"].ToString());
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The {0} item could not be successfully added to your list.\n\nError message:\n\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "SaveListsMessage2";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
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
//ggMessageBox.ShowDialog();
//                            }
//                            else if (dr["SavedAction"].ToString() == "Update" && dr["SavedStatus"].ToString() != "Success")
//                            {
////MessageBox.Show("The " + dr["TITLE"].ToString() + " item could not be successfully updated for your list.\n\nError message:\n\n" + dr["ExceptionMessage"].ToString());
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The {0} item could not be successfully updated for your list.\n\nError message:\n\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "SaveListsMessage3";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
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
//ggMessageBox.ShowDialog();
//                            }
//                            else if (dr["SavedAction"].ToString() == "Delete" && dr["SavedStatus"].ToString() != "Success")
//                            {
////MessageBox.Show("The " + dr["TITLE"].ToString() + " item could not be successfully deleted from your list.\n\nError message:\n\n" + dr["ExceptionMessage"].ToString());
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The {0} item could not be successfully deleted from your list.\n\nError message:\n\n{1}", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "SaveListsMessage4";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
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
//ggMessageBox.ShowDialog();
//                            }
//                        }
//                    }
//                }
//                else
//                {
////MessageBox.Show("There were errors syncronizing your lists with the remote server.\n\nYour list changes have not been saved.");
//GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors syncronizing your lists with the remote server.\n\nYour list changes have not been saved.", "Save User Lists Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
//ggMessageBox.Name = "SaveListsMessage5";
//_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//ggMessageBox.ShowDialog();
//                }
//            }
//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;

//            return saveErrors;
//        }
//    }
}
