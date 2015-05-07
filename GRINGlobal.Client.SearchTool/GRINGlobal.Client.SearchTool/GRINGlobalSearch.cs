using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace GRINGlobal.Client.SearchTool
{
    public partial class GRINGlobalClientSearchTool : Form
    {
        private string _user = "";
        private string _password = "";
        private string _url = "";
        bool escapeKeyPressed = false;
        DataSet GUIData;
        DataSet queryResults = new DataSet();
        TreeView uxtvTableFields;
        string DataGridViewSortOrder = "";
        string DataGridViewFilter = "";
        int MouseClickRowIndex = -1;
        int MouseClickColumnIndex = -1;
        int txtBoxSelectionStart = 0;
        int txtBoxSelectionLength = 0;
        string searchResultType = "accession";
        // Attach to the GRIN-Global web service...
        //GRINGlobalGUIWebServices.GUI GUIWebServices = new GRINGlobalGUIWebServices.GUI();
        DataRow[] basicQuerySearchResults;
        GRINGlobal.Client.Common.SharedUtils _sharedUtils;
        Cursor _cursorGG;

        public GRINGlobalClientSearchTool(string[] args)
        {
            InitializeComponent();
            // Parse through the commandline parameters...
            _user = GetCommandLineParameter(args, "-user", "");
            _password = GetCommandLineParameter(args, "-password", "");
            _url = GetCommandLineParameter(args, "-url", "");
            _sharedUtils = new GRINGlobal.Client.Common.SharedUtils(_url, _user, _password, false, "GRINGlobalClientSearchTool");
//// Pre-load the code value table into memory...
//if (_sharedUtils.IsConnected &&
//    _sharedUtils.LocalDatabaseTableExists("code_value_lookup"))
//{
//    //DataTable localDBCodeValueLookupTable = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup");
//    DataTable localDBCodeValueLookupTable = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup", "");
//    if (localDBCodeValueLookupTable.Rows.Count > 0)
//    {
//        // Since the MRU tables are never saved to the local database, we need to add them to the memory dataset each time...
//        _sharedUtils.LookupTablesCacheMRUTable(localDBCodeValueLookupTable);
//    }
//}
        }

        private void GRINGlobalClientSearchTool_Load(object sender, EventArgs e)
        {
            // Indicate the web service connection...
            List<string> URLs = new List<string>();
            if (!string.IsNullOrEmpty(_url)) URLs.Add(_url);
            URLs.Add("http://localhost/GrinGlobal/GUI.asmx");
            URLs.Add("http://GRIN-Global-Test1.agron.iastate.edu/GrinGlobal/GUI.asmx");
            URLs.Add("http://GRIN-Global-Dev1.agron.iastate.edu/GrinGlobal/GUI.asmx");
            URLs.Add("http://GRIN-Global-Dev2.agron.iastate.edu/GrinGlobal/GUI.asmx");

            bool connectedToWebService = false;
//_sharedUtils = new GRINGlobal.Client.Common.SharedUtils(_url, _user, _password);
            if (_sharedUtils.IsConnected)
            {
                _url = _sharedUtils.Url;
                _user = _sharedUtils.Username;
                _password = _sharedUtils.Password;
//_cursorGG = _sharedUtils.LoadCursor(@"Images\cursor_GG.cur");
_cursorGG = Cursors.Default;
            }
            else
            {
                // Could not find a web service - so bail out now...
//MessageBox.Show("Error: Could not connect to GRIN-Global Web Services");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error: Could not connect to GRIN-Global Web Services", "Connection Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "GRINGlobalClientSearchTool_LoadMessage1";
if (_sharedUtils.IsConnected) _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                this.Close();
            }
//else
//{
//    Login loginDialog = new Login(_user, _password, GUIWebServices.Url);
//    loginDialog.StartPosition = FormStartPosition.CenterScreen;
//    loginDialog.ShowDialog();
//    if (DialogResult.OK == loginDialog.DialogResult)
//    {
//        //validLogin = true;
//        _user = loginDialog.UserName;
//        _password = loginDialog.Password;
//        //cno = loginDialog.UserCooperatorID;
//        //site = loginDialog.UserSite;
//        //languageCode = loginDialog.UserLanguageCode;
//    }
//    else
//    {
//        // Login is unsuccessful - so bail out now...
//        this.Close();
//    }
//}

            if (!this.IsDisposed)
            {
                GUIData = GetGUIData(_user, _password);
                uxtvTableFields = BuildTreeView(GUIData);
                //uxtvTableFields.Height = splitContainer1.Panel2.Height;
                //uxtvTableFields.Width = splitContainer1.Panel2.Width;
                uxtvTableFields.Location = new Point(0, 0);
                uxtvTableFields.Size = new Size(splitContainer1.Panel2.Size.Width, splitContainer1.Panel2.Size.Height);
                uxtvTableFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                splitContainer1.Panel2.Controls.Add(uxtvTableFields);
                //toolTip1.SetToolTip(uxtvTableFields, "This is a treeview...");
                toolTip1.SetToolTip(ux_buttonDoAdvancedQuery, "This is a button...");

                ux_tabpageAdvancedQuery.Enabled = false;
                ux_tabpageAdvancedQuery.Hide();

//// Set the custom GG cursor for the form (if it is available)...
//if (_cursorGG != null)
//{
//    this.Cursor = _cursorGG;
//}

                // Set the title bar with the name and version of this application...
                this.Text = "GRIN-Global Search  v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

// Populate the dropdown list of 'Find Types'...
DataSet ds = _sharedUtils.GetWebServiceData("sys_table", "", 0, 0);
if (ds != null && ds.Tables.Contains("sys_table"))
{
    ds.Tables["sys_table"].DefaultView.Sort = "table_name ASC";
    //foreach (DataRow dr in ds.Tables["sys_table"].Rows)
    //{
    //    ux_comboboxCustomFind.Items.Add(dr["table_name"]);
    //}
    //ux_comboboxCustomFind.SelectedIndex = 0;
    foreach (DataRowView drv in ds.Tables["sys_table"].DefaultView)
    {
        ux_comboboxCustomFind.Items.Add(drv["table_name"]);
    }
    ux_comboboxCustomFind.SelectedIndex = 0;
}

// Build the results Dataview horizontal TabControl...
_sharedUtils.BuildDataviewTabControl(ux_tabcontrolSTDataviews);

// Get language translations for the components and controls in this applicataion...
if (this.components != null && this.components.Components != null) _sharedUtils.UpdateComponents(this.components.Components, this.Name);
if (this.Controls != null) _sharedUtils.UpdateControls(this.Controls, this.Name);

// Save the statusbar text for the left, center, and right controls...
if (string.IsNullOrEmpty(ux_statusLeftMessage.Text)) ux_statusLeftMessage.Text = "";
if (string.IsNullOrEmpty(ux_statusCenterMessage.Text)) ux_statusCenterMessage.Text = "";
if (string.IsNullOrEmpty(ux_statusRightMessage.Text)) ux_statusRightMessage.Text = "";
ux_statusLeftMessage.Tag = ux_statusLeftMessage.Text;
ux_statusCenterMessage.Tag = ux_statusCenterMessage.Text;
ux_statusRightMessage.Tag = ux_statusRightMessage.Text;

// Initialize/Update the statusbar...
// First the left message...
if (ux_statusLeftMessage.Tag != null)
{
    if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
        ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
    {
        ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), 0, 0);
    }
    else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
    {
        ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), 0);
    }
}
else
{
    ux_statusLeftMessage.Text = "Showing rows: 0 of 0 retrieved";
}
// Then the center message...
if (ux_statusCenterMessage.Tag != null)
{
    if (ux_statusCenterMessage.Tag.ToString().Contains("{0}"))
    {
        ux_statusCenterMessage.Text = string.Format(ux_statusCenterMessage.Tag.ToString(), _url);
    }
    else
    {
        ux_statusCenterMessage.Text = ux_statusCenterMessage.Tag.ToString() + " (" + _url + ")";
    }
}
else
{
    ux_statusCenterMessage.Text = "Connected to: " + _url;
}
ux_statusProgressBar.Visible = false;
ux_statusRightMessage.Text = "";

LoadUserSettings();

                // Select the first tab (initiates the updating of the DGV control...
                basicQuerySearchResults = new DataRow[0];
                ux_tabcontrolSTDataviews.SelectedIndex = -1;
                ux_tabcontrolSTDataviews.SelectedIndex = 0;

// TODO:  Temp code to remove the second tab from the ST main tabs until enhancemnts can be made
// (esentially this hides the 'Under Construction' tab from the user w/o removing the code from the ST)
ux_tabcontrolQueryEngine.TabPages.RemoveByKey("ux_tabpageAdvancedQuery");

                // Set focus to the Basic Query textbox...
                ux_textboxBasicQueryText.Focus();
            }
        }

        private void GRINGlobalClientSearchTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_sharedUtils.IsConnected)
            {
                SetAllUserSettings();
                _sharedUtils.SaveAllUserSettings();
            }
        }

        private string GetCommandLineParameter(string[] args, string key, string defaultValue)
        {
            string value = defaultValue;

            string keyPattern = @"^\s*" + key + @"\s*=\s*[\S]+(\s+|$)";
            foreach (string keyValue in args)
            {
                if (SharedUtils.TargetStringFitsPattern(keyPattern, keyValue))
                {
                    value = keyValue.Substring(keyValue.IndexOf('=') + 1);
                }
            }
            return value;
        }

        #region Advanced Query Logic...

        private DataSet GetGUIData(string strUsername, string strPassword)
        {
            /*            
                        DataSet GUIData = new DataSet();
                        System.Data.Odbc.OdbcConnection objDBConnection;
                        System.Data.Odbc.OdbcDataAdapter objDBDataAdapter;


                        string strSelectSQL = "";
                        //string strDBConnection = "Provider=MSDAORA; User ID=" + strUsername + "; Password=" + strPassword + "; Data Source=npgs.ars-grin.gov";
                        //string strDBConnection = "Driver={Microsoft ODBC for Oracle};Server=npgs.ars-grin.gov;Uid=" + strUsername + ";Pwd=" + strPassword;
                        string strDBConnection = "Driver={mySQL ODBC 3.51 Driver}; Server=127.0.0.1; Port=3306; Option=4; Database=prod; Uid=" + strUsername + "; Pwd=" + strPassword;

                        // Create and open the database connection...
                        objDBConnection = new System.Data.Odbc.OdbcConnection(strDBConnection);
                        objDBDataAdapter = new System.Data.Odbc.OdbcDataAdapter(strSelectSQL, objDBConnection);
                        objDBConnection.Open();

                        // Now get the table/field data...
                        strSelectSQL = @"
            select
            srf.sec_resultset_field_id,
            saf.sec_actual_field_id,
            srf.programmatic_resultset_name,
            saf.actual_table_name,
            srf.programmatic_field_name,
            saf.actual_field_name,
            saf.actual_field_purpose,
            coalesce(saf.actual_field_type, 'STRING') as actual_field_type,
            coalesce(srff.friendly_field_name, srf.programmatic_field_name) as friendly_field_name
            from
            sec_resultset_field srf
            inner join sec_actual_field saf
            on srf.sec_actual_field_id = saf.sec_actual_field_id
            left join sec_resultset_field_friendly srff
            on srff.sec_resultset_field_id = srf.sec_resultset_field_id
            and coalesce(srff.language_code,'') = coalesce('ENG','')
            ";
                        objDBDataAdapter.SelectCommand.CommandText = strSelectSQL;
                        objDBDataAdapter.Fill(GUIData, "mappingResults");

                        // Now get the combobox data...
                        strSelectSQL = @"
            SELECT * FROM prod.resource
            ";
                        objDBDataAdapter.SelectCommand.CommandText = strSelectSQL;
                        objDBDataAdapter.Fill(GUIData, "prod.RESOURCE");

                        // Cleanup...
                        objDBDataAdapter.Dispose();
                        objDBConnection.Close();
                        objDBConnection.Dispose();
            */
            //DataSet resultsetMappings = GUIWebServices.GetData(false, _user, _password, "get_resultset_mappings", ":languagecode=1", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString())); //.GetResultsetMappings(false, "test1", "test1", "");
            //DataSet resultsetMappings = _sharedUtils.GetWebServiceData("get_resultset_mappings", ":languagecode=1", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
            DataSet dataviewMappings = _sharedUtils.GetWebServiceData("get_dataview_mappings", ":languagecode=1", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
            if (!dataviewMappings.Tables.Contains("get_dataview_mappings"))
            {
                dataviewMappings = _sharedUtils.GetWebServiceData("get_dataview_mappings", ":languagecode=1", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
                if (dataviewMappings.Tables.Contains("get_dataview_mappings"))
                {
                    dataviewMappings.Tables["get_dataview_mappings"].TableName = "get_dataview_mappings";
                }
            }
            //DataSet resources = GUIWebServices.GetData(false, _user, _password, "get_app_resource", "", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString())); //.GetResources(false, "test1", "test1");
            DataSet resources = _sharedUtils.GetWebServiceData("get_app_resource", "", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString())); //.GetResources(false, "test1", "test1");
            if (resources.Tables.Contains("get_app_resource"))
            {
                dataviewMappings.Tables.Add(resources.Tables["get_app_resource"].Copy());
            }
            //DataSet sec_table = GUIWebServices.GetData(false, _user, _password, "sec_table", "", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
            DataSet sys_table = _sharedUtils.GetWebServiceData("sys_table", "", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
            if (sys_table.Tables.Contains("sys_table"))
            {
                dataviewMappings.Tables.Add(sys_table.Tables["sys_table"].Copy());
            }
            else
            {
                sys_table = _sharedUtils.GetWebServiceData("sec_table", "", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
                if (sys_table.Tables.Contains("sec_table"))
                {
                    sys_table.Tables["sec_table"].TableName = "sys_table";
                    dataviewMappings.Tables.Add(sys_table.Tables["sys_table"].Copy());
                }
            }
            return dataviewMappings;
        }

        private TreeView BuildTreeView(DataSet TableData)
        {
            // Create the treeview...
            TreeView TableFields = new TreeView();
            TableFields.Name = "TableFields";
            TableFields.LabelEdit = false;
            TableFields.FullRowSelect = true;
            TableFields.AllowDrop = true;
            TableFields.PathSeparator = ".";
            TableFields.ShowNodeToolTips = true;
            //tvTableFields.ImageList = ilTreeViewImages;
            //tvTableFields.ImageKey = "disabled_folder";
            //tvTableFields.SelectedImageKey = "active_folder";
            TableFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TableFields.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(TableFields_NodeMouseDoubleClick);
            //tvTableFields.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
            //tvTableFields.MouseClick += new MouseEventHandler(this.treeView_MouseClick);
            //tvTableFields.DragOver += new DragEventHandler(this.treeView_DragOver);
            //tvTableFields.DragDrop += new DragEventHandler(this.treeView_DragDrop);

            //foreach (DataRow dr in TableData.Tables["get_resultset_mappings"].Rows)
            foreach (DataRow dr in TableData.Tables["get_dataview_mappings"].Rows)
            {
                string TableName = "";
                if (TableData.Tables["get_dataview_mappings"].Columns.Contains("programmatic_dataview_name"))
                {
                    TableName = dr["programmatic_dataview_name"].ToString();
                }
                else
                {
                    TableName = dr["programmatic_resultset_name"].ToString();
                }
                
                //TreeNode tnLastNode;
                TreeNode NewFieldNode = new TreeNode();
                if (!TableFields.Nodes.ContainsKey(TableName))
                {
                    TreeNode NewTableNode = new TreeNode();
                    NewTableNode.Name = TableName;
                    NewTableNode.Text = TableName;
                    NewTableNode.Tag = "Table";
                    TableFields.Nodes.Add(NewTableNode);
                }
                //tnLastNode = BuildNodes(tvNew.Nodes, dr["GROUPNAME"].ToString(), ".");
                NewFieldNode.Text = dr["friendly_field_name"].ToString();
                NewFieldNode.Name = dr["actual_table_name"].ToString() + "." + dr["actual_field_name"].ToString();
                NewFieldNode.Tag = dr["actual_field_type"].ToString();
                NewFieldNode.ToolTipText = NewFieldNode.Name;
                //tnNewNode.ImageKey = "disabled_" + dr["IDNOTYPE"].ToString().Trim();
                //tnNewNode.SelectedImageKey = "active_" + dr["IDNOTYPE"].ToString().Trim();
                TableFields.Nodes[TableName].Nodes.Add(NewFieldNode);
            }

            foreach (TreeNode tn in TableFields.Nodes)
            {
                tn.Expand();
            }

            if (TableFields.Nodes.Count > 0)
            {
                TableFields.SelectedNode = TableFields.Nodes[0];
            }

            return TableFields;
        }

        void TableFields_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag.ToString().ToUpper() == "INTEGER")
            {
                int intRowNum = tableLayoutPanel1.Controls.Count / tableLayoutPanel1.ColumnCount;
                tableLayoutPanel1.Controls.Add(CreateTableCellLabel(e.Node.Text, e.Node.Name, e.Node.Tag.ToString().ToUpper()), 0, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellComboBox(typeof(int)), 1, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellTextBox(""), 2, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellTextBox(""), 3, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellButton("Remove"), 4, intRowNum);
                ((TextBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 2]).Hide();
                ((TextBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 3]).Hide();
                ((ComboBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 4]).SelectedIndex = -1;
            }
            else if (e.Node.Tag.ToString().ToUpper() == "STRING")
            {
                int intRowNum = tableLayoutPanel1.Controls.Count / tableLayoutPanel1.ColumnCount;
                tableLayoutPanel1.Controls.Add(CreateTableCellLabel(e.Node.Text, e.Node.Name, e.Node.Tag.ToString().ToUpper()), 0, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellComboBox(typeof(string)), 1, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellTextBox(""), 2, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellTextBox(""), 3, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellButton("Remove"), 4, intRowNum);
                ((TextBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 2]).Hide();
                ((TextBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 3]).Hide();
                ((ComboBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 4]).SelectedIndex = -1;
            }
            else if (e.Node.Tag.ToString().ToUpper() == "DATETIME")
            {
                int intRowNum = tableLayoutPanel1.Controls.Count / tableLayoutPanel1.ColumnCount;
                tableLayoutPanel1.Controls.Add(CreateTableCellLabel(e.Node.Text, e.Node.Name, e.Node.Tag.ToString().ToUpper()), 0, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellComboBox(typeof(DateTime)), 1, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellTextBox(""), 2, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellTextBox(""), 3, intRowNum);
                tableLayoutPanel1.Controls.Add(CreateTableCellButton("Remove"), 4, intRowNum);
                ((TextBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 2]).Hide();
                ((TextBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 3]).Hide();
                ((ComboBox)tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 4]).SelectedIndex = -1;
            }
        }

        private Label CreateTableCellLabel(string displayText, string nameText, string tagText)
        {
            Label lbl = new Label();
            //lbl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            lbl.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            lbl.Text = displayText;
            lbl.Name = nameText;
            lbl.Height = 21;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.AutoSize = true;
            //lbl.Tag = uxtvTableFields.Nodes[strText.Substring(0, strText.LastIndexOf(uxtvTableFields.PathSeparator))].Tag;
            lbl.Tag = tagText;
            return lbl;
        }

        private ComboBox CreateTableCellComboBox(Type fieldType)
        {
            ComboBox cb = new ComboBox();
            DataView dv;
            if (fieldType == typeof(int) || fieldType == typeof(DateTime))
            {
                dv = new DataView(GUIData.Tables["get_app_resource"], "app_resource_name = 'cboNumberOperators'", "sort_order asc", DataViewRowState.CurrentRows);
            }
            else //if (typeField == typeof(string))
            {
                dv = new DataView(GUIData.Tables["get_app_resource"], "app_resource_name = 'cboStringOperators'", "sort_order asc", DataViewRowState.CurrentRows);
            }
            //cb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            cb.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            cb.Height = 21;
            cb.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            cb.DisplayMember = "display_member";
            cb.ValueMember = "value_member";
            cb.DataSource = dv;
            cb.MaxDropDownItems = Math.Min(dv.Count, 15);
            return cb;
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.SelectedIndex >= 0)
            {
                if (cb.SelectedValue.ToString().ToLower().Contains("_string2_"))
                {
                    tableLayoutPanel1.SetColumnSpan(tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 1], 1);
                    tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 1].Show();
                    tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 2].Show();
                }
                else if (cb.SelectedValue.ToString().ToLower().Contains("_string_"))
                {
                    tableLayoutPanel1.SetColumnSpan(tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 1], 2);
                    tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 1].Show();
                    tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 2].Hide();
                }
                else
                {
                    tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 1].Hide();
                    tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.GetChildIndex(cb) + 2].Hide();
                }
            }
        }

        private TextBox CreateTableCellTextBox(string text)
        {
            TextBox tb = new TextBox();
            //tb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            tb.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            tb.Height = 21;
            tb.TextAlign = HorizontalAlignment.Left;
            tb.Text = text;
            return tb;
        }

        private Button CreateTableCellButton(string text)
        {
            Button btn = new Button();
            //btn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
            btn.Height = 21;
            btn.Click += new EventHandler(btn_Click);
            btn.Text = text;
            return btn;
        }

        void btn_Click(object sender, EventArgs e)
        {
            // This is the code necessary to delete a row from a TableLayoutPanel
            // because the .NET class does not have a built in rows[0].remove() method
            // I got this method from a link on the MS newsgroups, so click on
            //  http://www.msnewsgroups.net/group/microsoft.public.dotnet.languages.csharp/topic26883.aspx 
            // to get the details...
            Button btn = (Button)sender;
            int RowIndex = tableLayoutPanel1.GetRow(btn);
            Label nextlbl = (Label)tableLayoutPanel1.GetNextControl(btn, true);
            TextBox tb2 = (TextBox)tableLayoutPanel1.GetNextControl(btn, false);
            TextBox tb1 = (TextBox)tableLayoutPanel1.GetNextControl(tb2, false);
            ComboBox cb = (ComboBox)tableLayoutPanel1.GetNextControl(tb1, false);
            Label lbl = (Label)tableLayoutPanel1.GetNextControl(cb, false);

            if (tableLayoutPanel1.RowCount > 5)
            {
                // First decrement the row count for the tablelayoutpanel...
                tableLayoutPanel1.RowCount -= 1;
                // Now remove the row style for the row being deleted...
                tableLayoutPanel1.RowStyles.RemoveAt(RowIndex);
            }

            // Now remove the controls from the row being deleted...
            tableLayoutPanel1.Controls.Remove(btn);
            tableLayoutPanel1.Controls.Remove(tb2);
            tableLayoutPanel1.Controls.Remove(tb1);
            tableLayoutPanel1.Controls.Remove(cb);
            tableLayoutPanel1.Controls.Remove(lbl);

            // Finally change the row indexes for the controls that will remain...
            if (tableLayoutPanel1.Controls.IndexOf(nextlbl) > -1)
            {
                for (int controlsIndex = tableLayoutPanel1.Controls.IndexOf(nextlbl); controlsIndex < tableLayoutPanel1.Controls.Count; controlsIndex++)
                {
                    tableLayoutPanel1.SetRow(tableLayoutPanel1.Controls[controlsIndex], tableLayoutPanel1.GetRow(tableLayoutPanel1.Controls[controlsIndex]) - 1);
                }
            }
        }

        private string BuildSQLCommand()
        {
            //string strSQLCommand = "SELECT prod.iv.* FROM ";
            string strSQLCommand = "SELECT inventory.* FROM ";
            string strTableNames = "";
            string strTableJoins = " WHERE ";
            string strFilterCriteria = " ";

            // Build the table names to select from and the filter criteria...
            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                //if (ctrl.GetType() == typeof(Label) && !strTableNames.Contains(ctrl.Text.Substring(0, ctrl.Text.LastIndexOf(treeView1.PathSeparator))))
                if (tableLayoutPanel1.GetColumn(ctrl) == 0)
                {
                    int intRow = tableLayoutPanel1.GetRow(ctrl);
                    string strTableName = ctrl.Name.Substring(0, ctrl.Name.LastIndexOf(uxtvTableFields.PathSeparator));
                    if (!strTableNames.Contains(strTableName))
                    {
                        strTableNames += strTableName + ",";
                    }
                    // Retrieve the table.fieldname...
                    strFilterCriteria += tableLayoutPanel1.GetControlFromPosition(0, intRow).Name + " ";
                    // Build the filter criteria...
                    string FilterTemplate = ((ComboBox)tableLayoutPanel1.GetControlFromPosition(1, intRow)).SelectedValue.ToString();
                    if (FilterTemplate.ToLower().Contains("in ("))
                    {
                        char[] Delimiter = new char[] { ' ', ',', '\t', '\n', '\r' };
                        strFilterCriteria += FilterTemplate + " AND ";
                        // Process the parameters by first stripping any leading or trailing spaces...
                        string RawInParam = tableLayoutPanel1.GetControlFromPosition(2, intRow).Text.Trim();
                        // Remove the beginning and/or ending parentheses if the user included them in the string...
                        if (RawInParam.StartsWith("(")) RawInParam = RawInParam.Remove(0, 1);
                        if (RawInParam.EndsWith(")")) RawInParam = RawInParam.Remove(RawInParam.Length - 1, 1);
                        // Remove any single or double quotes (they will be re-added as needed below)...
                        RawInParam = RawInParam.Replace("'", "").Replace("\"", "");
                        // Split the remaining text into words using the delimiters from above...
                        string[] InParams = RawInParam.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);
                        // Concatenate each word into the list for the SQL statement... 
                        string FormattedParam = "";
                        foreach (string Param in InParams)
                        {
                            FormattedParam += "'" + Param + "'" + ", ";
                        }
                        FormattedParam = FormattedParam.Remove(FormattedParam.Length - 2);
                        strFilterCriteria = strFilterCriteria.Replace("_string_", FormattedParam);
                    }
                    else if (FilterTemplate.ToLower().Contains("between"))
                    {
                        strFilterCriteria += FilterTemplate + " AND ";
                        if (tableLayoutPanel1.GetControlFromPosition(0, intRow).Tag.ToString().ToUpper() == "DATETIME")
                        {
                            strFilterCriteria = strFilterCriteria.Replace("_string1_", "'" + ParseDateTimeParam(tableLayoutPanel1.GetControlFromPosition(2, intRow).Text.Replace("'", "").Replace("\"", "")) + "'");
                            strFilterCriteria = strFilterCriteria.Replace("_string2_", "'" + ParseDateTimeParam(tableLayoutPanel1.GetControlFromPosition(3, intRow).Text.Replace("'", "").Replace("\"", "")) + "'");
                        }
                        else
                        {
                            strFilterCriteria = strFilterCriteria.Replace("_string1_", tableLayoutPanel1.GetControlFromPosition(2, intRow).Text.Replace("'", "").Replace("\"", ""));
                            strFilterCriteria = strFilterCriteria.Replace("_string2_", tableLayoutPanel1.GetControlFromPosition(3, intRow).Text.Replace("'", "").Replace("\"", ""));
                        }
                    }
                    else
                    {
                        strFilterCriteria += FilterTemplate + " AND ";
                        if (tableLayoutPanel1.GetControlFromPosition(0, intRow).Tag.ToString().ToUpper() == "DATETIME")
                        {
                            strFilterCriteria = strFilterCriteria.Replace("_string_", "'" + ParseDateTimeParam(tableLayoutPanel1.GetControlFromPosition(2, intRow).Text.Replace("'", "").Replace("\"", "")) + "'");
                        }
                        else
                        {
                            strFilterCriteria = strFilterCriteria.Replace("_string_", tableLayoutPanel1.GetControlFromPosition(2, intRow).Text.Replace("'", "").Replace("\"", ""));
                        }
                    }
                }
            }

            // Build the joins...
            //if (strTableNames.ToUpper().Contains("PROD.IV") && strTableNames.ToUpper().Contains("PROD.ACC"))
            //{
            //    strTableJoins += " prod.iv.acid = prod.acc.acid AND ";
            //}
            //if (strTableNames.Contains("PROD.IV") && strTableNames.Contains("PROD.ORD"))
            //{
            //    strTableJoins += " prod.iv.ivid = prod.oi.ivid AND prod.oi.orno = prod.ord.orno AND ";
            //    if (!strTableNames.Contains("prod.oi")) strTableNames += "prod.oi" + ",";
            //}
            //if (strTableNames.Contains("PROD.ACC") && strTableNames.Contains("PROD.ORD"))
            //{
            //    strTableJoins += " prod.iv.acid = prod.oi.acid AND prod.oi.orno = prod.ord.orno AND ";
            //    if (!strTableNames.Contains("prod.oi")) strTableNames += "prod.oi" + ",";
            //}
            if (strTableNames.ToUpper().Contains("INVENTORY") && strTableNames.ToUpper().Contains("ACCESSION"))
            {
                strTableJoins += " inventory.acid = accession.acid AND ";
            }
            if (strTableNames.Contains("INVENTORY") && strTableNames.Contains("ORDER_ENTRY"))
            {
                strTableJoins += " inventory.ivid = order_item.ivid AND order_item.orno = order_entry.orno AND ";
                if (!strTableNames.Contains("ORDER_ITEM")) strTableNames += "order_item" + ",";
            }
            if (strTableNames.Contains("ACCESSION") && strTableNames.Contains("ORDER_ENTRY"))
            {
                strTableJoins += " inventory.acid = order_item.acid AND order_item.orno = order_entry.orno AND ";
                if (!strTableNames.Contains("order_item")) strTableNames += "order_item" + ",";
            }


            // Strip out the extra syntax...
            if (strTableNames[strTableNames.Length - 1] == ',') strTableNames = strTableNames.Substring(0, strTableNames.Length - 1);
            if (strFilterCriteria.LastIndexOf(" AND ") >= (strFilterCriteria.Length - 5)) strFilterCriteria = strFilterCriteria.Substring(0, strFilterCriteria.Length - 5);
            //if (strTableJoins.LastIndexOf(" AND ") >= (strTableJoins.Length - 5)) strTableJoins = strTableJoins.Substring(0, strTableJoins.Length - 5);


            return strSQLCommand + strTableNames + strTableJoins + strFilterCriteria;
        }

        private string ParseDateTimeParam(string InParam)
        {
            string Result = "";
            DateTime newDate;
            if (DateTime.TryParse(InParam, out newDate))
            {
                Result = newDate.ToString("yyyy-MM-dd");
            }
            return Result;
        }

        private void btnDoSearch_Click(object sender, EventArgs e)
        {
            //            string strUsername = "test1";
            //            string strPassword = "test1";
            string strSQLCommand = BuildSQLCommand();
            //            MessageBox.Show(strSQLCommand);

            Cursor.Current = Cursors.WaitCursor;

            if (queryResults.Tables.Contains("AdvancedSearchResults")) queryResults.Tables.Remove("AdvancedSearchResults");
            DataSet advancedQueryResults = new DataSet();
            advancedQueryResults.Tables.Add("SearchResults");
            //DataSet advancedQueryResults = GUIWebServices.Search(false, strUsername, strPassword, ":" + strSQLCommand);
            queryResults.Tables.Add(advancedQueryResults.Tables["SearchResults"].Copy());
            queryResults.Tables["SearchResults"].TableName = "AdvancedSearchResults";

            // Reset the rowfilter and the sortorder...
            DataGridViewSortOrder = "";
            DataGridViewFilter = "";
            queryResults.Tables["AdvancedSearchResults"].DefaultView.Sort = DataGridViewSortOrder;
            queryResults.Tables["AdvancedSearchResults"].DefaultView.RowFilter = DataGridViewFilter;

            // Now rebind the DataGridView to the DataSet Table...
            ux_datagridviewQueryResults.DataSource = queryResults.Tables["AdvancedSearchResults"];

// Update the statusbar...
if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewQueryResults.Rows.Count.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
//ux_statusLeftMessage.Text = "Showing rows: " + ux_datagridviewQueryResults.Rows.Count.ToString() + " of " + ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString() + " retrieved";

            foreach (DataGridViewColumn dgvc in ux_datagridviewQueryResults.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.Programmatic;
                dgvc.ContextMenuStrip = ux_contextmenustripDGVResultsCell;
            }

            //Cursor.Current = Cursors.Default;
            Cursor.Current = _cursorGG;
        }

        #endregion

        private void RefreshDGVFormatting()
        {
// If the datagridview has no data source or the selected tab's tag has no dataview string - bail out now...
if (ux_datagridviewSearchCriteria.DataSource == null ||
    ux_tabcontrolSTDataviews.SelectedTab == null ||
    ux_tabcontrolSTDataviews.SelectedTab.Tag == null ||
    ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() != typeof(GRINGlobal.Client.Common.DataviewProperties) ||
    string.IsNullOrEmpty(((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName)) return;

//// Get a fresh copy of the user settings (in case the user has changed a dataview setting)...
//_sharedUtils.LoadAllUserSettings();

            // Hide the Search Criteria gridview's column header...
            ux_datagridviewSearchCriteria.ColumnHeadersVisible = false;

            // Show SortGlyphs for the column headers...
            // First reset them all to No Sort...
            foreach (DataGridViewColumn dgvc in ux_datagridviewQueryResults.Columns)
            {
                dgvc.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            // Now inspect the sort string from the datatable in use to set the SortGlyphs...
            string strOrder = ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.Sort;
            char[] chararrDelimiters = { ',' };
            string[] strarrSortCols = strOrder.Split(chararrDelimiters);
            foreach (string strSortCol in strarrSortCols)
            {
                if (strSortCol.Contains("ASC")) ux_datagridviewQueryResults.Columns[strSortCol.Replace(" ASC", "")].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                if (strSortCol.Contains("DESC")) ux_datagridviewQueryResults.Columns[strSortCol.Replace(" DESC", "")].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            }

            // Determine which dataview is being displayed in the DGV...
            string currentDataView = "get_accession";
////if (ux_radiobuttonBasicReturnAccessions.Checked) currentDataView = "get_accession";
////if (ux_radiobuttonBasicReturnInventory.Checked) currentDataView = "get_inventory";
////if (ux_radiobuttonBasicReturnOrders.Checked) currentDataView = "get_order_request";
////if (ux_radiobuttonBasicReturnCooperators.Checked) currentDataView = "get_cooperator";

//if (ux_tabcontrolDataview.SelectedTab.Tag != null &&
//    ux_tabcontrolDataview.SelectedTab.Tag.ToString().Length > 0)
//{
//    currentDataView = ux_tabcontrolDataview.SelectedTab.Tag.ToString();
//}
if (ux_tabcontrolSTDataviews.SelectedTab == null ||
    ux_tabcontrolSTDataviews.SelectedTab.Tag == null ||
    ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() != typeof(GRINGlobal.Client.Common.DataviewProperties) ||
    string.IsNullOrEmpty(((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName))
{
    return;
}
else
{
    if (ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() == typeof(GRINGlobal.Client.Common.DataviewProperties))
    {
        currentDataView = ((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName;
    }
}

            // Now restore the column width and ordering...
            string[] columnWidths = _sharedUtils.GetUserSetting("", currentDataView, "Columns.Width", "").Split(' ');
            string[] columnOrder = _sharedUtils.GetUserSetting("", currentDataView, "Columns.DisplayOrder", "").Split(' ');
            int columnNum = -1;
            if (columnWidths.Length == columnOrder.Length && columnWidths.Length == ux_datagridviewQueryResults.Columns.Count)
            {
                // First process the results datagridview...
                for (int i = 0; i < ux_datagridviewQueryResults.Columns.Count; i++)
                {
                    if (int.TryParse(columnWidths[i], out columnNum)) ux_datagridviewQueryResults.Columns[i].Width = columnNum;
                    if (int.TryParse(columnOrder[i], out columnNum)) ux_datagridviewQueryResults.Columns[i].DisplayIndex = columnNum;
                }
                // Now process the search criteria datagridview...
                for (int i = 0; i < ux_datagridviewSearchCriteria.Columns.Count; i++)
                {
                    if (int.TryParse(columnWidths[i], out columnNum)) ux_datagridviewSearchCriteria.Columns[i].Width = columnNum;
                    if (int.TryParse(columnOrder[i], out columnNum)) ux_datagridviewSearchCriteria.Columns[i].DisplayIndex = columnNum;
                }
            }

            // Make columns visible based on user settings...
            string[] columnsVisible = _sharedUtils.GetUserSetting("", currentDataView, "Columns.Visible", "").Split(' ');
            if (columnsVisible.Length == 1 && columnsVisible[0] == "")
            {
                // No setting was found so show all columns...
                // Set background color for readonly columns in the search criteria...
                foreach (DataGridViewColumn dgvc in ux_datagridviewSearchCriteria.Columns)
                {
                    if (dgvc.ReadOnly) dgvc.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
            else
            {
                // First process the Search Results datagridview columns...
                foreach (DataGridViewColumn dgvc in ux_datagridviewQueryResults.Columns)
                {
                    if (columnsVisible.Contains(dgvc.Index.ToString()) ||
                        ux_checkboxShowAllDGVColumns.Checked)
                    {
                        dgvc.Visible = true;
                    }
                    else
                    {
                        dgvc.Visible = false;
                    }
                }
                // Now process the SearchCcriteria datagridview columns...
                foreach (DataGridViewColumn dgvc in ux_datagridviewSearchCriteria.Columns)
                {
                    if (dgvc.ReadOnly) dgvc.DefaultCellStyle.BackColor = Color.LightGray;
                    if (columnsVisible.Contains(dgvc.Index.ToString()) ||
                        ux_checkboxShowAllDGVColumns.Checked)
                    {
                        dgvc.Visible = true;
                    }
                    else
                    {
                        dgvc.Visible = false;
                    }
                }
            }

            // Now format the Search Criteria DGV row(s)...
            foreach (DataGridViewRow dgvr in ux_datagridviewSearchCriteria.Rows)
            {
                RefreshDGVRowFormatting(dgvr, true);
            }

            // Reset DGV to the default full-row select mode...
            ux_datagridviewQueryResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // And reset the current cell (to lose the highlight)...
            ux_datagridviewQueryResults.CurrentCell = null;
            ux_datagridviewSearchCriteria.CurrentCell = null;

            // Set focus to the text box...
            ux_textboxBasicQueryText.Focus();
        }

        private void RefreshDGVRowFormatting(DataGridViewRow dgvr, bool useHighlighting)
        {
            DataRow dr;

            // Reset the rows cells to the default color scheme...
            foreach (DataGridViewCell dgvc in dgvr.Cells)
            {
                dgvc.Style.BackColor = Color.Empty;
                dgvc.Style.SelectionBackColor = Color.Empty;
            }

            if (useHighlighting)
            {
                //// If the row has changes make each changed cell yellow...
                //foreach (DataGridViewCell dgvc in dgvr.Cells)
                //{
                //    // If the cell has been changed make it yellow...
                //    //if (!dr[dgvc.ColumnIndex, DataRowVersion.Original].Equals(dr[dgvc.ColumnIndex, DataRowVersion.Current]))
                //    if (dgvc.Value != null &&
                //        dgvc.Value.ToString() != "")
                //    {
                //        dgvc.Style.BackColor = Color.PaleGreen;
                //    }
                //    // Use default background color for this cell...
                //    else
                //    {
                //        dgvc.Style.BackColor = Color.Empty;
                //    }
                //}
            }
        }

        private void LoadUserSettings()
        {
            bool tempBool = true;
            int tempInt = 0;

            // Select the search areas....
            // Get the setting for IgnoreCaseCheckbox...
            tempBool = ux_checkboxIgnoreCase.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxIgnoreCase.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxIgnoreCase.Checked = tempBool;
            }
            // Get the setting for AccessionAreaCheckbox...
            tempBool = ux_checkboxSearchAccessionArea.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxSearchAccessionArea.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxSearchAccessionArea.Checked = tempBool;
            }
            // Get the setting for InventoryAreaCheckbox...
            tempBool = ux_checkboxSearchInventoryArea.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxSearchInventoryArea.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxSearchInventoryArea.Checked = tempBool;
            }
            // Get the setting for OrderAreaCheckbox...
            tempBool = ux_checkboxSearchOrderArea.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxSearchOrderArea.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxSearchOrderArea.Checked = tempBool;
            }
            // Get the setting for CropTraitAreaCheckbox...
            tempBool = ux_checkboxSearchCropTraitArea.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxSearchCropTraitArea.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxSearchCropTraitArea.Checked = tempBool;
            }
            // Get the setting for TaxonomyAreaCheckbox...
            tempBool = ux_checkboxSearchTaxonomyArea.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxSearchTaxonomyArea.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxSearchTaxonomyArea.Checked = tempBool;
            }
            // Get the setting for AllOtherAreasCheckbox...
            tempBool = ux_checkboxSearchAllOtherAreas.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxSearchAllOtherAreas.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxSearchAllOtherAreas.Checked = tempBool;
            }

            // General settings...
            // Get the setting for ShowAllDGVColumnsCheckbox...
            tempBool = ux_checkboxShowAllDGVColumns.Checked;
            if (bool.TryParse(_sharedUtils.GetUserSetting("", ux_checkboxShowAllDGVColumns.Name, "Checked", tempBool.ToString()), out tempBool))
            {
                ux_checkboxShowAllDGVColumns.Checked = tempBool;
            }
            // Get the setting for max number of rows to return...
            tempInt = (int)ux_numericupdownBasicMaxRecords.Value;
            if (int.TryParse(_sharedUtils.GetUserSetting("", ux_numericupdownBasicMaxRecords.Name, "Value", tempInt.ToString()), out tempInt))
            {
                ux_numericupdownBasicMaxRecords.Value = tempInt;
            }
        }

        private void SetAllUserSettings()
        {
            SetGeneralUserSettings();
            SetDGVQueryResultsDataviewUserSettings();
            SetTabControlDataviewUserSettings();
        }

        private void SetGeneralUserSettings()
        {
            _sharedUtils.SaveUserSetting("", ux_checkboxIgnoreCase.Name, "Checked", ux_checkboxIgnoreCase.Checked.ToString());

            _sharedUtils.SaveUserSetting("", ux_checkboxSearchAccessionArea.Name, "Checked", ux_checkboxSearchAccessionArea.Checked.ToString());
            _sharedUtils.SaveUserSetting("", ux_checkboxSearchInventoryArea.Name, "Checked", ux_checkboxSearchInventoryArea.Checked.ToString());
            _sharedUtils.SaveUserSetting("", ux_checkboxSearchOrderArea.Name, "Checked", ux_checkboxSearchOrderArea.Checked.ToString());
            _sharedUtils.SaveUserSetting("", ux_checkboxSearchCropTraitArea.Name, "Checked", ux_checkboxSearchCropTraitArea.Checked.ToString());
            _sharedUtils.SaveUserSetting("", ux_checkboxSearchTaxonomyArea.Name, "Checked", ux_checkboxSearchTaxonomyArea.Checked.ToString());
            _sharedUtils.SaveUserSetting("", ux_checkboxSearchAllOtherAreas.Name, "Checked", ux_checkboxSearchAllOtherAreas.Checked.ToString());
            _sharedUtils.SaveUserSetting("", ux_checkboxShowAllDGVColumns.Name, "Checked", ux_checkboxShowAllDGVColumns.Checked.ToString());
            _sharedUtils.SaveUserSetting("", ux_numericupdownBasicMaxRecords.Name, "Value", ux_numericupdownBasicMaxRecords.Value.ToString());
        }

        private void SetTabControlDataviewUserSettings()
        {
            int tabPageCount = ux_tabcontrolSTDataviews.TabPages.Count;
            if (ux_tabcontrolSTDataviews.TabPages.ContainsKey("ux_tabpageDataviewNewTab")) tabPageCount -= 1;
            // Check to see if the number of dataview tabs is less than what the user started with and if so delete the extra tabs...
            int oldTabPageCount = -1;
            int.TryParse(_sharedUtils.GetUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages.Count", "-1"), out oldTabPageCount);
            if (oldTabPageCount > tabPageCount)
            {
                for (int i = tabPageCount; i < oldTabPageCount; i++)
                {
                    _sharedUtils.DeleteUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].Text"); // Legacy...
                    _sharedUtils.DeleteUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].Tag"); // Legacy...
                    _sharedUtils.DeleteUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].TabName");
                    _sharedUtils.DeleteUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].DataviewName");
                    _sharedUtils.DeleteUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].FormName");
                    _sharedUtils.DeleteUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].ViewerStyle");
                    _sharedUtils.DeleteUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].AlwaysOnTop");
                }
            }

            _sharedUtils.SaveUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages.Count", tabPageCount.ToString());
            for (int i = 0; i < tabPageCount; i++)
            {
                if (ux_tabcontrolSTDataviews.TabPages[i].Tag != null &&
                    ux_tabcontrolSTDataviews.TabPages[i].Tag.GetType() == typeof(DataviewProperties) &&
                    !string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolSTDataviews.TabPages[i].Tag).DataviewName))
                {
                    _sharedUtils.SaveUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].TabName", ((DataviewProperties)ux_tabcontrolSTDataviews.TabPages[i].Tag).TabName + "");
                    _sharedUtils.SaveUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].DataviewName", ((DataviewProperties)ux_tabcontrolSTDataviews.TabPages[i].Tag).DataviewName + "");
                    _sharedUtils.SaveUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].FormName", ((DataviewProperties)ux_tabcontrolSTDataviews.TabPages[i].Tag).StrongFormName + "");
                    _sharedUtils.SaveUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].ViewerStyle", ((DataviewProperties)ux_tabcontrolSTDataviews.TabPages[i].Tag).ViewerStyle + "");
                    _sharedUtils.SaveUserSetting("", ux_tabcontrolSTDataviews.Name, "TabPages[" + i.ToString() + "].AlwaysOnTop", ((DataviewProperties)ux_tabcontrolSTDataviews.TabPages[i].Tag).AlwaysOnTop + "");
                }
            }
        }

        private void SetDGVQueryResultsDataviewUserSettings()
        {
            string columnsWidth = "";
            string columnsVisible = "";
            string columnsDisplayOrder = "";

            // If the selected tab's tag has no dataview string - bail out now...
            if (ux_tabcontrolSTDataviews.SelectedTab == null ||
                ux_tabcontrolSTDataviews.SelectedTab.Tag == null ||
                ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() != typeof(DataviewProperties) ||
                string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName)) return;

            foreach (DataGridViewColumn dgvc in ux_datagridviewQueryResults.Columns)
            {
                if (dgvc.Name != "TempSortColumn")
                {
                    columnsWidth += dgvc.Width + " ";
                    if (dgvc.Visible) columnsVisible += dgvc.Index + " ";
                    columnsDisplayOrder += dgvc.DisplayIndex + " ";
                }
            }

            // Update the TabControl settings to the userSettings dataset...
            // NOTE: this could be saved to SelectedTab.Text instead of SelectedTab.Tag
            //       so that the settings follow the tab name instead of the dataview name,
            //       but this creates problems elsewhere so we won't do it unless forced to
            string dataviewName = ((DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName;
            _sharedUtils.SaveUserSetting("", dataviewName, "Columns.Width", columnsWidth.Trim());
            _sharedUtils.SaveUserSetting("", dataviewName, "Columns.Visible", columnsVisible.Trim());
            _sharedUtils.SaveUserSetting("", dataviewName, "Columns.DisplayOrder", columnsDisplayOrder.Trim());
            _sharedUtils.SaveUserSetting("", dataviewName, "DefaultCellStyle.BackColor", ux_datagridviewQueryResults.DefaultCellStyle.BackColor.ToArgb().ToString());
            _sharedUtils.SaveUserSetting("", dataviewName, "AlternatingRowsDefaultCellStyle.BackColor", ux_datagridviewQueryResults.AlternatingRowsDefaultCellStyle.BackColor.ToArgb().ToString());
            if (ux_datagridviewQueryResults.DataSource != null)
            {
                string dgvSort = ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.Sort;
                if (!dgvSort.Contains("TempSortColumn"))
                {
                    _sharedUtils.SaveUserSetting("", dataviewName, "DefaultView.Sort", dgvSort);
                }
            }
        }

        private void sortAscendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ColName = "Error!";
            if(MouseClickColumnIndex >= 0) ColName = ux_datagridviewQueryResults.Columns[MouseClickColumnIndex].Name;
            if (DataGridViewSortOrder.Contains(ColName + " DESC"))
            {
                DataGridViewSortOrder = DataGridViewSortOrder.Replace(ColName + " DESC", ColName + " ASC");
            }
            else if (!DataGridViewSortOrder.Contains(ColName + " ASC"))
            {
                if (DataGridViewSortOrder.Length > 0)
                {
                    DataGridViewSortOrder += "," + ColName + " ASC";
                }
                else
                {
                    DataGridViewSortOrder += ColName + " ASC";
                }
            }
            DataGridViewSortOrder = DataGridViewSortOrder.Replace(",,", ",").Trim();
            ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.Sort = DataGridViewSortOrder;
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshDGVFormatting();
        }

        private void sortDescendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ColName = "Error!";
            if (MouseClickColumnIndex >= 0) ColName = ux_datagridviewQueryResults.Columns[MouseClickColumnIndex].Name;
            if (DataGridViewSortOrder.Contains(ColName + " ASC"))
            {
                DataGridViewSortOrder = DataGridViewSortOrder.Replace(ColName + " ASC", ColName + " DESC");
            }
            else if (!DataGridViewSortOrder.Contains(ColName + " DESC"))
            {
                if (DataGridViewSortOrder.Length > 0)
                {
                    DataGridViewSortOrder += "," + ColName + " DESC";
                }
                else
                {
                    DataGridViewSortOrder += ColName + " DESC";
                }
            }
            DataGridViewSortOrder = DataGridViewSortOrder.Replace(",,", ",").Trim();
            ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.Sort = DataGridViewSortOrder;
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshDGVFormatting();
        }

        private void noSortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ColName = "Error!";
            if (MouseClickColumnIndex >= 0) ColName = ux_datagridviewQueryResults.Columns[MouseClickColumnIndex].Name;
            if (DataGridViewSortOrder.Contains(ColName + " DESC"))
            {
                DataGridViewSortOrder = DataGridViewSortOrder.Replace(ColName + " DESC", "");
            }
            else if (DataGridViewSortOrder.Contains(ColName + " ASC"))
            {
                DataGridViewSortOrder = DataGridViewSortOrder.Replace(ColName + " ASC", "");
            }
            DataGridViewSortOrder = DataGridViewSortOrder.Replace(",,", ",").Trim();
            if (DataGridViewSortOrder.Length > 0 && DataGridViewSortOrder[0] == ',') DataGridViewSortOrder = DataGridViewSortOrder.Substring(1, DataGridViewSortOrder.Length - 1);
            if (DataGridViewSortOrder.Length > 0 && DataGridViewSortOrder[DataGridViewSortOrder.Length - 1] == ',') DataGridViewSortOrder = DataGridViewSortOrder.Remove(DataGridViewSortOrder.Length - 1);
            ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.Sort = DataGridViewSortOrder;
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshDGVFormatting();
        }

        private void resetAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewSortOrder = "";
            ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.Sort = DataGridViewSortOrder;
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshDGVFormatting();
        }

        private void showOnlyRowsWithThisDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ColName = "Error!";
            string CellValue = "Error!";
            if (MouseClickColumnIndex >= 0 && MouseClickRowIndex >= 0)
            {
                //ColName = ux_datagridviewQueryResults.Columns[MouseClickColumnIndex].Name;
                //CellValue = ux_datagridviewQueryResults[MouseClickColumnIndex, MouseClickRowIndex].Value.ToString().Replace("'", "''");
// Get the column name for the bound datatable (NOTE: convert the dbnulls to '' using the isnull(,) function so that they are filtered properly)...
ColName = "convert(isnull(" + ux_datagridviewQueryResults.Columns[MouseClickColumnIndex].Name + ", ''), 'System.String')";
CellValue = "'" + ux_datagridviewQueryResults[MouseClickColumnIndex, MouseClickRowIndex].Value.ToString().Replace("'", "''") + "'";
                if (DataGridViewFilter.Length > 0)
                {
                    //DataGridViewFilter += " AND " + ColName + " = '" + CellValue + "'";
DataGridViewFilter += " AND " + ColName + " = " + CellValue;
                }
                else
                {
                    //DataGridViewFilter = ColName + " = '" + CellValue + "'";
DataGridViewFilter = ColName + " = " + CellValue;
                }
            }
            ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.RowFilter = DataGridViewFilter;
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshDGVFormatting();
// Update the statusbar...
if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewQueryResults.Rows.Count.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
//ux_statusLeftMessage.Text = "Showing " + ux_datagridviewQueryResults.Rows.Count.ToString() + " rows (of " + ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString() + " retrieved)";
        }

        private void hideRowsWithThisDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ColName = "Error!";
            string CellValue = "Error!";
            if (MouseClickColumnIndex >= 0 && MouseClickRowIndex >= 0)
            {
                //ColName = ux_datagridviewQueryResults.Columns[MouseClickColumnIndex].Name;
                //CellValue = ux_datagridviewQueryResults[MouseClickColumnIndex, MouseClickRowIndex].Value.ToString().Replace("'", "''");
// Get the column name for the bound datatable (NOTE: convert the dbnulls to '' using the isnull(,) function so that they are filtered properly)...
ColName = "convert(isnull(" + ux_datagridviewQueryResults.Columns[MouseClickColumnIndex].Name + ", ''), 'System.String')";
CellValue = "'" + ux_datagridviewQueryResults[MouseClickColumnIndex, MouseClickRowIndex].Value.ToString().Replace("'", "''") + "'";
                if (DataGridViewFilter.Length > 0)
                {
                    //DataGridViewFilter += " AND " + ColName + " <> '" + CellValue + "'";
DataGridViewFilter += " AND " + ColName + " <> " + CellValue;
                }
                else
                {
                    //DataGridViewFilter = ColName + " <> '" + CellValue + "'";
DataGridViewFilter = ColName + " <> " + CellValue;
                }
            }
            ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.RowFilter = DataGridViewFilter;
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshDGVFormatting();
// Update the statusbar...
if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewQueryResults.Rows.Count.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
//ux_statusLeftMessage.Text = "Showing " + ux_datagridviewQueryResults.Rows.Count.ToString() + " rows (of " + ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString() + " retrieved)";
        }

        private void resetRowFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewFilter = "";
            ((DataTable)ux_datagridviewQueryResults.DataSource).DefaultView.RowFilter = DataGridViewFilter;
            // Refresh the formatting for the datagridview to show the sort glyphs...
            RefreshDGVFormatting();
// Update the statusbar...
if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewQueryResults.Rows.Count.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
}
//ux_statusLeftMessage.Text = "Showing " + ux_datagridviewQueryResults.Rows.Count.ToString() + " rows (of " + ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString() + " retrieved)";
        }

        private void ux_datagridviewQueryResults_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            MouseClickColumnIndex = e.ColumnIndex;
            MouseClickRowIndex = e.RowIndex;
        }

        private void ux_datagridviewQueryResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataObject doDragDropData;

            if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && (ux_datagridviewQueryResults.Rows[e.RowIndex].Selected) && (e.Button == MouseButtons.Left))
            {
                // If there were selected rows lets start the drag-drop operation...
                if (ux_datagridviewQueryResults.SelectedRows.Count > 0)
                {
                    // Get the string of selected rows extracted from the GridView...
                    doDragDropData = BuildDGVDragAndDropData(ux_datagridviewQueryResults);
                    ux_datagridviewQueryResults.DoDragDrop(doDragDropData, DragDropEffects.Copy);
                }
            }
            if ((e.RowIndex > -1) && (e.ColumnIndex > -1) && (e.Button == MouseButtons.Right))
            {
                // Change the color of the cell background so that the user
                // knows what cell the context menu applies to...
                ux_datagridviewQueryResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                ux_datagridviewQueryResults.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Red;
            }
        }

        private void ux_datagridviewQueryResults_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ux_contextmenustripDGVResultsColumnHeader.Show(ux_datagridviewQueryResults.Columns[e.ColumnIndex].HeaderCell.AccessibilityObject.Bounds.Location);
            }
        }

        private void ux_datagridviewQueryResults_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            // For some reason changing the column width of the QueryResults DGV will reset its HorizontalScrollingOffset back to zero - therefore 
            // remember the HorizontalScrollingOffset before changing the column width so it can be restored...
            int hso = ux_datagridviewQueryResults.HorizontalScrollingOffset;
            ux_datagridviewSearchCriteria.Columns[e.Column.Name].Width = ux_datagridviewQueryResults.Columns[e.Column.Name].Width;
            ux_datagridviewQueryResults.HorizontalScrollingOffset = hso;
            ux_datagridviewSearchCriteria.HorizontalScrollingOffset = hso;
        }

        private void ux_datagridviewQueryResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (_sharedUtils.ProcessDGVReadOnlyShortcutKeys(ux_datagridviewQueryResults, e, _sharedUtils.UserCooperatorID))
            {
                RefreshDGVFormatting();
                // The user might have been in cell-select (block) select mode - so reset it to the default row-select mode now...
                ux_datagridviewQueryResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        //private void ux_datagridviewQueryResults_KeyUp(object sender, KeyEventArgs e)
        //{
        //    // Check to see if user was selecting cells (block-mode) instead of default full-row select - and reset it back to full-row select
        //    if (!e.Alt) ux_datagridviewQueryResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //}

        private void ux_datagridviewQueryResults_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                ux_datagridviewSearchCriteria.HorizontalScrollingOffset = ux_datagridviewQueryResults.HorizontalScrollingOffset;
            }
        }

        public DataObject BuildDGVDragAndDropData(DataGridView dgv)
        {
            // Depending on how many rows are selected, this operation
            // could take a while - so change the cursor...
            Cursor.Current = Cursors.WaitCursor;

            DataObject dndData = new DataObject();

            // Write the rows out in text format first...
            StringBuilder sb = new StringBuilder("");

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
            sb.Append(columnHeader);
            // Build the array of visible column names...
            string[] columnList = columnNames.Trim().Split('\t');
            // And use that list for writing the values to the string builder...
            foreach (DataGridViewRow dgvr in dgv.SelectedRows)
            {
                sb.Append("\n");
                foreach (string columnName in columnList)
                {
                    sb.Append(dgvr.Cells[columnName].FormattedValue.ToString());
                    sb.Append("\t");
                }
            }

            // Finally write it out as a collection of data table rows (for tree view drag and drop).
            DataSet dsData = new DataSet();
            dsData.Tables.Add(((DataTable)dgv.DataSource).Clone());
            foreach (DataGridViewRow dgvr in dgv.SelectedRows)
            {
                dsData.Tables[0].Rows.Add(((DataRowView)dgvr.DataBoundItem).Row.ItemArray);
            }

            // Set the data types into the data object and return...
            dndData.SetData(typeof(string), sb.ToString());
            dndData.SetData(typeof(DataSet), dsData);

            // We are all done so change the cursor back and return the data...
            //Cursor.Current = Cursors.Default;
            Cursor.Current = _cursorGG;

            return dndData;
        }

        private void ux_buttonDoBasicQuery_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ux_buttonDoBasicQuery.Enabled = false;

            // Add any criteria in the criteria row to the query text box...
            ux_buttonAddToQuery.PerformClick();

            string strUsername = _user;
            string strPassword = _password;
            string filter = "ID IS NOT NULL";
//int pageSize = 100;
            // Build the list of tables to search...
            string tablesToSearch = buildListOfTablesToSearch();

            // Detect the type of result the user wants...
//if (ux_radiobuttonBasicReturnCooperators.Checked)
//{
//    searchResultType = "cooperator";
//    filter = "ID IS NOT NULL";
//}
//else if (ux_radiobuttonBasicReturnOrders.Checked)
//{
//    searchResultType = "order_request";
//    filter = "ID IS NOT NULL";
//}
//else if (ux_radiobuttonBasicReturnInventory.Checked)
//{
//    searchResultType = "inventory";
//    filter = "ID IS NOT NULL";
//}
//else
//{
//    searchResultType = "accession";
//    filter = "ID IS NOT NULL";
//}
if (ux_radiobuttonDefaultFind.Checked)
{
    searchResultType = "accession";
    DataviewProperties dp = (DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag;
    DataSet ds = _sharedUtils.GetWebServiceData("get_dataview_list", "", 0, 0);
    if (ds != null && ds.Tables.Contains("get_dataview_list"))
    {
        DataRow[] dvListRows = ds.Tables["get_dataview_list"].Select("dataview_name='" + dp.DataviewName + "'");
        if(dvListRows != null && dvListRows.Length > 0)
        {
            searchResultType = dvListRows[0]["primary_key"].ToString();
            searchResultType = searchResultType.Remove(searchResultType.Length - 3);
        }
    }
}
else
{
    searchResultType = ux_comboboxCustomFind.SelectedItem.ToString();
}

            // Clear the previous results and get new results...
            if (queryResults.Tables.Contains("BasicSearchResults")) queryResults.Tables.Remove("BasicSearchResults");
            // Clear the previous search results...
            basicQuerySearchResults = new DataRow[0];
            if (!string.IsNullOrEmpty(ux_textboxBasicQueryText.Text))
            {
            try
            {
                string searchCriteria = ux_textboxBasicQueryText.Text;
                DataSet dsBasicQuerySearchResults = null;
                if (ux_radiobuttonBasicMatchList.Checked)
                {
                    //searchCriteria = BuildSearchStringFromItemList(ux_textboxBasicQueryText.Text);
                    dsBasicQuerySearchResults = _sharedUtils.SearchWebService(searchCriteria, ux_checkboxIgnoreCase.Checked, ux_radiobuttonBasicMatchAll.Checked, tablesToSearch, searchResultType, 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()), "OrMultipleLines=true");
//ux_textboxBasicQueryText.Text = searchCriteria;
                }
                else
                {
                    dsBasicQuerySearchResults = _sharedUtils.SearchWebService(searchCriteria, ux_checkboxIgnoreCase.Checked, ux_radiobuttonBasicMatchAll.Checked, tablesToSearch, searchResultType, 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
                }
                if (dsBasicQuerySearchResults != null && dsBasicQuerySearchResults.Tables.Contains("SearchResult"))
                {
                    basicQuerySearchResults = dsBasicQuerySearchResults.Tables["SearchResult"].Select(filter);
//DialogResult userResponse = MessageBox.Show("Found at least " + basicQuerySearchResults.Length + " 'possible' matches in the database.  Continue to retrieve data?", "Query Results", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
DialogResult userResponse = DialogResult.Cancel;
if (basicQuerySearchResults.Length == 0)
{
    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("No matches in the database were found.", "Query Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
    ggMessageBox.Name = "GRINGlobalClientSearchTool_ux_buttonDoBasicQueryMessage5";
    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
    userResponse = ggMessageBox.ShowDialog();
}
else
{
    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Found at least {0} 'possible' matches in the database.  Continue to retrieve data?", "Query Results", MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button1);
    ggMessageBox.Name = "GRINGlobalClientSearchTool_ux_buttonDoBasicQueryMessage1";
    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, basicQuerySearchResults.Length);
string[] argsArray = new string[100];
argsArray[0] = basicQuerySearchResults.Length.ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
    userResponse = ggMessageBox.ShowDialog();
}
                    if (userResponse == DialogResult.Cancel)
                    {
                        // User is bailing out on the query so clear the results with an empty array...
                        basicQuerySearchResults = new DataRow[0];
                    }
                }
                else
                {
                    if (dsBasicQuerySearchResults != null && dsBasicQuerySearchResults.Tables.Contains("ExceptionTable"))
                    {
//MessageBox.Show("There was an unexpected error searching for data.\n\nFull error message:\n" + dsBasicQuerySearchResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error searching for data.\n\nFull error message:\n{0}", "Search Engine Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "GRINGlobalClientSearchTool_ux_buttonDoBasicQueryMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dsBasicQuerySearchResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
string[] argsArray = new string[100];
argsArray[0] = dsBasicQuerySearchResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
                    }
                    else
                    {
//MessageBox.Show("There was an unexpected error searching for data.\n\nWeb Service at '" + _url + "' did not respond to your search request.");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error searching for data.\n\nWeb Service at '{0}' did not respond to your search request.", "Search Engine Response Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "GRINGlobalClientSearchTool_ux_buttonDoBasicQueryMessage3";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, _url);
string[] argsArray = new string[100];
argsArray[0] = _url;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
                    }
                    // Clear the results with an empty array...
                    basicQuerySearchResults = new DataRow[0];
                }
            }
            catch (Exception err)
            {
//MessageBox.Show("There was an unexpected error searching for data.\n\nWeb Service at '" + _url + "' did not respond to your search request.\n\nFull error message:\n" + err.Message);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error searching for data.\n\nWeb Service at '{0}' did not respond to your search request.\n\nFull error message:\n{1}", "Search Engine Response Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "GRINGlobalClientSearchTool_ux_buttonDoBasicQueryMessage4";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, ggMessageBox.MessageText, _url, err.Message);
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, ggMessageBox.MessageText, _url);
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, err.Message);
//}
string[] argsArray = new string[100];
argsArray[0] = _url;
argsArray[1] = err.Message;
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
            }

            }
            #region Old code for refreshing data...

//// Get the name of the dataview to use...
//string dataGridViewTableName = "get_accession";
//if (ux_tabcontrolDataview.SelectedTab == null ||
//    ux_tabcontrolDataview.SelectedTab.Tag == null ||
//    ux_tabcontrolDataview.SelectedTab.Tag.GetType() != typeof(GRINGlobal.Client.Common.DataviewProperties) ||
//    string.IsNullOrEmpty(((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName))
//{
//    return;
//}
//else
//{
//    if (ux_tabcontrolDataview.SelectedTab.Tag.GetType() == typeof(GRINGlobal.Client.Common.DataviewProperties))
//    {
//        dataGridViewTableName = ((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName;
//    }
//}

//            //if (userResponse == DialogResult.OK)
//            {
//                // Get the data for the selected dataview tab based on the search results...
////DataSet ds = GetDGVData(ux_tabcontrolDataview.SelectedTab.Tag.ToString(), basicQuerySearchResults, pageSize);
////if (queryResults.Tables.Contains("BasicSearchResults")) queryResults.Tables.Remove("BasicSearchResults");
////if (ds.Tables.Contains(ux_tabcontrolDataview.SelectedTab.Tag.ToString()))
//DataSet ds = GetDGVData(dataGridViewTableName, basicQuerySearchResults, pageSize);
//if (queryResults.Tables.Contains("BasicSearchResults")) queryResults.Tables.Remove("BasicSearchResults");
//if (ds.Tables.Contains(dataGridViewTableName))
//                {
////queryResults.Tables.Add(ds.Tables[ux_tabcontrolDataview.SelectedTab.Tag.ToString()].Copy());
////queryResults.Tables[ux_tabcontrolDataview.SelectedTab.Tag.ToString()].TableName = "BasicSearchResults";
//queryResults.Tables.Add(ds.Tables[dataGridViewTableName].Copy());
//queryResults.Tables[dataGridViewTableName].TableName = "BasicSearchResults";

//                    // Reset the rowfilter and the sortorder...
//                    DataGridViewSortOrder = "";
//                    DataGridViewFilter = "";
//                    queryResults.Tables["BasicSearchResults"].DefaultView.Sort = DataGridViewSortOrder;
//                    queryResults.Tables["BasicSearchResults"].DefaultView.RowFilter = DataGridViewFilter;

//                    // Now build the DGV and bind it to the datasource...
//                    _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewQueryResults, queryResults.Tables["BasicSearchResults"]);

//// Update the statusbar...
//if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
//    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
//{
//    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewQueryResults.Rows.Count.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
//}
//else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
//{
//    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
//}
////ux_statusLeftMessage.Text = "Showing rows: " + ux_datagridviewQueryResults.Rows.Count.ToString() + " of " + ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString() + " retrieved";

//                    foreach (DataGridViewColumn dgvc in ux_datagridviewQueryResults.Columns)
//                    {
//                        dgvc.SortMode = DataGridViewColumnSortMode.Programmatic;
//                        dgvc.ContextMenuStrip = ux_contextmenustripDGVResultsCell;
//                        DataColumn dc = ((DataTable)dgvc.DataGridView.DataSource).Columns[dgvc.DataPropertyName];
//                        dgvc.HeaderText = _sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName);
//                    }
//                    RefreshDGVFormatting();
//                }
//            }
            
            #endregion

            // Save the DGV user settings before refreshing the DGV...
            SetDGVQueryResultsDataviewUserSettings();
            // Refresh the DGV data and then the formatting...
            RefreshDGVData();
            RefreshDGVFormatting();

            //Cursor.Current = Cursors.Default;
            Cursor.Current = _cursorGG;
            ux_buttonDoBasicQuery.Enabled = true;

            // Set focus to the text box...
            ux_textboxBasicQueryText.Focus();
        }

        private void RefreshDGVData()
        {
            int pageSize = 100;

            // Get the name of the dataview to use...
            string dataGridViewTableName = "get_accession";
            if (ux_tabcontrolSTDataviews.SelectedTab == null ||
                ux_tabcontrolSTDataviews.SelectedTab.Tag == null ||
                ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() != typeof(GRINGlobal.Client.Common.DataviewProperties) ||
                string.IsNullOrEmpty(((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName))
            {
                return;
            }
            else
            {
                if (ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() == typeof(GRINGlobal.Client.Common.DataviewProperties))
                {
                    dataGridViewTableName = ((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName;
                }
            }

            // Get the data for the selected dataview tab based on the search results...
            DataSet ds = GetDGVData(dataGridViewTableName, basicQuerySearchResults, pageSize);
            if (queryResults.Tables.Contains("BasicSearchResults")) queryResults.Tables.Remove("BasicSearchResults");
            if (ds.Tables.Contains(dataGridViewTableName))
            {
                queryResults.Tables.Add(ds.Tables[dataGridViewTableName].Copy());
                queryResults.Tables[dataGridViewTableName].TableName = "BasicSearchResults";

                // Reset the rowfilter and the sortorder...
                DataGridViewSortOrder = "";
                DataGridViewFilter = "";
                queryResults.Tables["BasicSearchResults"].DefaultView.Sort = DataGridViewSortOrder;
                queryResults.Tables["BasicSearchResults"].DefaultView.RowFilter = DataGridViewFilter;

                // Now build the DGV and bind it to the datasource...
                _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewQueryResults, queryResults.Tables["BasicSearchResults"]);

                // Update the statusbar...
                if (ux_statusLeftMessage.Tag != null &&
                    ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
                    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
                {
                    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewQueryResults.Rows.Count.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
                }
                else if (ux_statusLeftMessage.Tag != null && 
                    ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
                {
                    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
                }
                //ux_statusLeftMessage.Text = "Showing rows: " + ux_datagridviewQueryResults.Rows.Count.ToString() + " of " + ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString() + " retrieved";

                foreach (DataGridViewColumn dgvc in ux_datagridviewQueryResults.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Programmatic;
                    dgvc.ContextMenuStrip = ux_contextmenustripDGVResultsCell;
                    DataColumn dc = ((DataTable)dgvc.DataGridView.DataSource).Columns[dgvc.DataPropertyName];
                    dgvc.HeaderText = _sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName);
                }
//RefreshDGVFormatting();
            }
        }

        private string BuildSearchStringFromItemList(string itemList)
        {
            string searchString = "";
            string[] items = itemList.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                string[] tokens = item.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int number = 0;
                switch (tokens.Length)
                {
                    case 1:
                        // Assume this is an order_request_id or a xxx_part1 item...
                        number = 0;
                        if (int.TryParse(tokens[0], out number))
                        {
                            searchString += "(@order_request.order_request_id=" + number.ToString() + ") OR ";
                        }
                        //else
                        //{
                            searchString += "(@accession.accession_number_part1='" + tokens[0] + "') OR ";
                            searchString += "(@inventory.inventory_number_part1='" + tokens[0] + "') OR ";
                        //}
                        break;
                    case 2:
                        // Assume this is a xxx_part1 and xxx_part2 item...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) &&
                            int.TryParse(tokens[1], out number))
                        {
                            searchString += "(@accession.accession_number_part1='" + tokens[0] + "' AND @accession.accession_number_part2=" + number.ToString() + ") OR ";
                            searchString += "(@inventory.inventory_number_part1='" + tokens[0] + "' AND @inventory.inventory_number_part2=" + number.ToString() + ") OR ";
                        }
                        break;
                    case 3:
                        // Assume this is a xxx_part1, xxx_part2 and xxx_part3 item...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) &&
                            int.TryParse(tokens[1], out number))
                        {
                            searchString += "(@accession.accession_number_part1='" + tokens[0] + "' AND @accession.accession_number_part2=" + number.ToString() + " AND @accession.accession_number_part3='" + tokens[2] + "') OR ";
                            searchString += "(@inventory.inventory_number_part1='" + tokens[0] + "' AND @inventory.inventory_number_part2=" + number.ToString() + " AND @inventory.inventory_number_part3='" + tokens[2] + "') OR ";
                        }
                        break;
                    case 4:
                        // Assume this is a xxx_part1, xxx_part2, xxx_part3 and xxx_part4 item (which can only be an inventory)...
                        number = 0;
                        if (!int.TryParse(tokens[0], out number) &&
                            int.TryParse(tokens[1], out number))
                        {
//tokens[3] = _sharedUtils.GetLookupValueMember("code_value_lookup", tokens[3], "group_name='GERMPLASM_FORM'", tokens[3]);
                            tokens[3] = _sharedUtils.GetLookupValueMember(null, "code_value_lookup", tokens[3], "GERMPLASM_FORM", tokens[3]);
                            searchString += "(@inventory.inventory_number_part1='" + tokens[0] + "' AND @inventory.inventory_number_part2=" + number.ToString() + " AND @inventory.inventory_number_part3='" + tokens[2] + "' AND @inventory.form_type_code='" + tokens[3] + "') OR ";
                        }
                        break;
                    default:
                        // Ignore this item...
                        break;
                }
            }
            return searchString.Substring(0, searchString.Length-3);
        }

        private string buildSearchCriteria()
        {
            DataTable dt = (DataTable)ux_datagridviewSearchCriteria.DataSource;

            if (dt == null || dt.Rows.Count < 1) return "";

            string searchCriteria = "";

            foreach (DataGridViewCell dgvc in ux_datagridviewSearchCriteria.Rows[0].Cells)
            {
                if (dgvc.Value != null &&
                    !string.IsNullOrEmpty(dgvc.Value.ToString()) &&
                    dt.Columns[dgvc.ColumnIndex].ExtendedProperties.Contains("table_name") &&
                    !string.IsNullOrEmpty(dt.Columns[dgvc.ColumnIndex].ExtendedProperties["table_name"].ToString()) &&
                    dt.Columns[dgvc.ColumnIndex].ExtendedProperties.Contains("table_field_name") &&
                    !string.IsNullOrEmpty(dt.Columns[dgvc.ColumnIndex].ExtendedProperties["table_field_name"].ToString()))
                {
                    string fieldName = "@" + dt.Columns[dgvc.ColumnIndex].ExtendedProperties["table_name"].ToString().ToLower().Trim() + 
                                        "." + dt.Columns[dgvc.ColumnIndex].ExtendedProperties["table_field_name"].ToString().ToLower().Trim();

                    // Processing for dropdown controls for the code_value fields...
                    if (dt.Columns[dgvc.ColumnIndex].ExtendedProperties["gui_hint"].ToString().ToUpper() == "SMALL_SINGLE_SELECT_CONTROL")
                    {
                        string fieldCriteria = ((DataRowView)dgvc.OwningRow.DataBoundItem).Row[dgvc.ColumnIndex].ToString().Trim();

                        if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:is\s+|IS\s+|=\s*)(?:null|NULL)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                        {
                            fieldCriteria = " IS NULL";
                        }
                        else if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:(?:is\s+|IS\s+)(?:not\s+|NOT\s+)|<>\s*)(?:null|NULL)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                        {
                            fieldCriteria = " IS NOT NULL";
                        }
                        else
                        {
                            fieldCriteria = " = '" + fieldCriteria + "'";
                        }

                        if (searchCriteria.Trim().Length > 0)
                        {
                            searchCriteria += " AND " + fieldName + fieldCriteria;
                        }
                        else
                        {
                            searchCriteria += fieldName + fieldCriteria;
                        }
                    }
                    // Processing for foreign key fields...
                    else if (dt.Columns[dgvc.ColumnIndex].ExtendedProperties["gui_hint"].ToString().ToUpper() == "LARGE_SINGLE_SELECT_CONTROL")
                    {
                        if (!_sharedUtils.LookupTablesIsUpdated(dt.Columns[dgvc.ColumnIndex].ExtendedProperties["foreign_key_dataview_name"].ToString()))
                        {
                            //MessageBox.Show("The lookup table associated with the data in this column is missing or incomplete - this could compromise your search criteria.");
                            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The lookup table associated with the data in this column is missing or incomplete - this could compromise your search criteria.", "Missing Lookup Data", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                            ggMessageBox.Name = "GRINGlobalClientSearchTool_buildSearchCriteriaMessage1";
                            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                            ggMessageBox.ShowDialog();
                        }
                        DataTable matchingPKeys = _sharedUtils.LookupTablesGetMatchingRows(dt.Columns[dgvc.ColumnIndex].ExtendedProperties["foreign_key_dataview_name"].ToString(), dgvc.FormattedValue.ToString().TrimEnd('*').Replace('*', '%'), 0);

                        if (matchingPKeys != null &&
                            matchingPKeys.Rows.Count > 0)
                        {
                            if (matchingPKeys.Columns.Contains("display_member"))
                            {
                                if (System.Text.RegularExpressions.Regex.Match(dgvc.FormattedValue.ToString().Replace('*', '%'), @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+))\s*\S+\s*").Success)
                                {
                                    string searchOperator = "LIKE";
                                    string searchValue = dgvc.FormattedValue.ToString();
                                    searchOperator = System.Text.RegularExpressions.Regex.Match(dgvc.FormattedValue.ToString(), @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+))\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Trim().ToUpper();

                                    string[] splitValues = System.Text.RegularExpressions.Regex.Split(dgvc.FormattedValue.ToString(), @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+))\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                    if (splitValues != null &&
                                        splitValues.Length > 0)
                                    {
                                        searchValue = splitValues[splitValues.Length - 1].Trim();
                                        if (!searchValue.StartsWith("'") &&
                                            !searchValue.EndsWith("'"))
                                        {
                                            searchValue = "'" + searchValue + "'";
                                        }
                                    }
                                    try
                                    {
                                        matchingPKeys.DefaultView.RowFilter = "display_member " + searchOperator + " " + searchValue;
                                    }
                                    catch
                                    {
                                        // The row filter does not handle some filters like smith%h% properly so in these cases, apply no row filter...
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        matchingPKeys.DefaultView.RowFilter = "display_member LIKE '" + dgvc.FormattedValue.ToString().Replace('_', '%') + "'";
                                    }
                                    catch
                                    {
                                        matchingPKeys.DefaultView.RowFilter = "";
                                        // The row filter does not handle some filters like smith%h% properly so in these cases, apply no row filter...
                                    }
                                }
                            }
                            // Add all rows matching the row filter to the list...
                            StringBuilder sb = new StringBuilder();
                            foreach (DataRowView drv in matchingPKeys.DefaultView)
                            {
                                sb.Append(drv["value_member"].ToString() + ", ");
                            }
                            if (sb.Length > 0)
                            {
                                if (searchCriteria.Trim().Length > 0)
                                {
                                    searchCriteria += " AND " + fieldName + " IN (" + sb.ToString().TrimEnd(' ').TrimEnd(',') + ")";
                                }
                                else
                                {
                                    searchCriteria += fieldName + " IN (" + sb.ToString().TrimEnd(' ').TrimEnd(',') + ")";
                                }
                            }
                            else
                            {
                                // No matches for your “Taxon” filter were found.  To broaden a search, remember that the wildcards  (  *   %  ) may be used.
                                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("No matches for your '{0}' filter were found. To broaden a search, remember that the wildcards  (  *   %  ) may be used.\n\nBecause no matches were found, this filter will not contribute to the search results returned.", "No Lookup Data Found", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                                ggMessageBox.Name = "GRINGlobalClientSearchTool_buildSearchCriteriaMessage2";
                                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dt.Columns[dgvc.ColumnIndex].ExtendedProperties["title"].ToString());
string[] argsArray = new string[100];
argsArray[0] = dt.Columns[dgvc.ColumnIndex].ExtendedProperties["title"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                                ggMessageBox.ShowDialog();
                            }
                        }
                    }
                    // Processing for everything else...
                    else
                    {
                        string fieldCriteria = dgvc.FormattedValue.ToString().Trim();
                        // Tidy up any 'bad' (math) grammar from the user...
                        fieldCriteria = System.Text.RegularExpressions.Regex.Replace(fieldCriteria, @"^\s*=\s*<\s*|^\s*<\s*=\s*", "<= ");
                        fieldCriteria = System.Text.RegularExpressions.Regex.Replace(fieldCriteria, @"^\s*=\s*>\s*|^\s*>\s*=\s*", ">= ");
                        fieldCriteria = System.Text.RegularExpressions.Regex.Replace(fieldCriteria, @"^\s*>\s*<\s*|^\s*<\s*>\s*", "<> ");
                        // First test to see if the user wants to search on IS NULL...
                        if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:is\s+|IS\s+|=\s*)(?:null|NULL)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                        {
                            fieldCriteria = "IS NULL";
                        }
                        // or search on IS NOT NULL...
                        else if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:(?:is\s+|IS\s+)(?:not\s+|NOT\s+)|<>\s*)(?:null|NULL)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                        {
                            fieldCriteria = "IS NOT NULL";
                        }
                        // or wants to search on other math/SQL operators
                        else
                        {
                            string searchOperator = "";
                            string searchValue = "";

                            // Test to see if the user wants to search on the LIKE operator...
                            if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:like\s+|LIKE\s+)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                            {
                                searchOperator = System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:like\s+|LIKE\s+)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Trim().ToUpper() + " ";
                                string[] splitValues = System.Text.RegularExpressions.Regex.Split(fieldCriteria, @"^\s*(?:like\s+|LIKE\s+)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                if (splitValues != null &&
                                    splitValues.Length > 0)
                                {
                                    searchValue = splitValues[splitValues.Length - 1].Trim();
                                    searchValue = searchValue.Replace('*', '%');
                                }
                            }
                            // or test to see if the user wants to search on the NOT LIKE operator...
                            else if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:not\s+|NOT\s+)(?:like\s+|LIKE\s+)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                            {
                                searchOperator = System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:not\s+|NOT\s+)(?:like\s+|LIKE\s+)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Trim().ToUpper() + " ";
                                string[] splitValues = System.Text.RegularExpressions.Regex.Split(fieldCriteria, @"^\s*(?:not\s+|NOT\s+)(?:like\s+|LIKE\s+)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                if (splitValues != null &&
                                    splitValues.Length > 0)
                                {
                                    searchValue = splitValues[splitValues.Length - 1].Trim();
                                    searchValue = searchValue.Replace('*', '%');
                                }
                            }
                            // or test to see if this is a two character operator...
                            else if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:<=|>=|<>)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                            {
                                // The math/SQL operator is two characters...
                                searchOperator = System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:<=|>=|<>)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Trim();
                                string[] splitValues = System.Text.RegularExpressions.Regex.Split(fieldCriteria, @"^\s*(?:<=|>=|<>)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                if (splitValues != null &&
                                    splitValues.Length > 0)
                                {
                                    searchValue = splitValues[splitValues.Length - 1].Trim();
                                }
                            }
                            // or test to see if this is a one character operator...
                            else if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:<|>|=)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                            {
                                // The math/SQL operator is one character...
                                searchOperator = System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*(?:<|>|=)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.Trim();
                                string[] splitValues = System.Text.RegularExpressions.Regex.Split(fieldCriteria, @"^\s*(?:<|>|=)\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                if (splitValues != null &&
                                    splitValues.Length > 0)
                                {
                                    searchValue = splitValues[splitValues.Length - 1].Trim();
                                }
                            }
                            //// or test to see if the text is quoted (single or double quotes)...
                            //else if (System.Text.RegularExpressions.Regex.Match(fieldCriteria, @"^\s*""[^\a\b\r\v\f\n\e]+""(\s+|$)|^\s*'[^\a\b\r\v\f\n\e]+'(\s+|$)", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success)
                            //{
                            //}
                            else
                            {
                                //
                                searchValue = fieldCriteria.Replace('*', '%');
                                if (searchValue.Contains('%') ||
                                    searchValue.Contains('_'))
                                {
                                    searchOperator = "LIKE";
                                }
                                else
                                {
                                    searchOperator = "=";
                                }
                            }

                            // If the table field has a text datatype, enclose the search value in single quotes...
                            if (dt.Columns[dgvc.ColumnIndex].ExtendedProperties["gui_hint"].ToString().ToUpper() == "TOGGLE_CONTROL" ||
                                dt.Columns[dgvc.ColumnIndex].ExtendedProperties["gui_hint"].ToString().ToUpper() == "TEXT_CONTROL")
                            {
                                if (searchValue.StartsWith("\"") &&
                                    searchValue.EndsWith("\""))
                                {
                                    searchValue = searchValue.TrimEnd('"').TrimStart('"');
                                }
                                if (!searchValue.StartsWith("'") &&
                                    !searchValue.EndsWith("'"))
                                {
                                    // Now wrap the search value in single quotes...
                                    searchValue = " '" + searchValue + "'";
                                }
                                else
                                {
                                    // The single quotes are there, so just add a space padding character...
                                    searchValue = " " + searchValue;
                                }
                            }
                            // If the table field has a date datatype, format the date and enclose it in single quotes...
                            else if (dt.Columns[dgvc.ColumnIndex].ExtendedProperties["gui_hint"].ToString().ToUpper() == "DATE_CONTROL")
                            {
                                DateTime searchDate;
                                // Scrub any beginning/ending single/double quotes the user may have provided...
                                if (searchValue.StartsWith("'") ||
                                    searchValue.StartsWith("\""))
                                {
                                    searchValue = searchValue.Substring(1);
                                }
                                if (searchValue.EndsWith("'") ||
                                    searchValue.EndsWith("\""))
                                {
                                    searchValue = searchValue.Substring(0, searchValue.Length - 1);
                                }
                                // Now see if you can convert the string to a valid datetime...
                                if (DateTime.TryParse(searchValue, out searchDate))
                                {
                                    // Format the date as 01-JAN-0001 format because Oracle accepts no other format (and the other databases are flexible enough to handle this format)...
                                    searchValue = searchDate.ToString("dd-MMM-yyyy");
                                }
                                // Now wrap the datetime back in single quotes...
                                searchValue = " '" + searchValue + "'";
                            }
                            // If the table field is a number datatype, do not enclose the search value in single quotes...
                            else
                            {
                                // Scrub any beginning/ending single/double quotes the user may have provided...
                                if (searchValue.StartsWith("'") ||
                                    searchValue.StartsWith("\""))
                                {
                                    searchValue = searchValue.Substring(1);
                                }
                                if (searchValue.EndsWith("'") ||
                                    searchValue.EndsWith("\""))
                                {
                                    searchValue = searchValue.Substring(0, searchValue.Length - 1);
                                }
                                // Now test to see if the user is wildcarding the number
                                if (searchOperator != "LIKE")
                                {
                                    // No wildcarding so simply add a space delimiter...
                                    searchValue = " " + searchValue;
                                }
                                else
                                {
                                    // Using numeric wildcards so wrap the number back in single quotes...
                                    searchValue = " '" + searchValue + "'";
                                }
                            }

                            // Build the search criteria for this field...
                            fieldCriteria = searchOperator + searchValue;
                        }

                        // Now stitch it all back together...
                        if (searchCriteria.Trim().Length > 0)
                        {
                            searchCriteria += " AND " + fieldName + " " + fieldCriteria + "";
                        }
                        else
                        {
                            searchCriteria += fieldName + " " + fieldCriteria + "";
                        }

                    }
                }
            }
            return searchCriteria;
        }

        private string buildListOfTablesToSearch()
        {
            string listOfTables = "";
            foreach (DataRow dr in GUIData.Tables["sys_table"].Rows)
            {
if (dr["database_area_code"].ToString().ToUpper() == "ACCESSION" && ux_checkboxSearchAccessionArea.Checked) listOfTables += dr["table_name"].ToString().ToLower().Trim() + " ";
if (dr["database_area_code"].ToString().ToUpper() == "INVENTORY" && ux_checkboxSearchInventoryArea.Checked) listOfTables += dr["table_name"].ToString().ToLower().Trim() + " ";
if (dr["database_area_code"].ToString().ToUpper() == "ORDER" && ux_checkboxSearchOrderArea.Checked) listOfTables += dr["table_name"].ToString().ToLower().Trim() + " ";
if (dr["database_area_code"].ToString().ToUpper() == "CROP" && ux_checkboxSearchCropTraitArea.Checked) listOfTables += dr["table_name"].ToString().ToLower().Trim() + " ";
if (dr["database_area_code"].ToString().ToUpper() == "TAXONOMY" && ux_checkboxSearchTaxonomyArea.Checked) listOfTables += dr["table_name"].ToString().ToLower().Trim() + " ";
if (dr["database_area_code"].ToString().ToUpper() == "OTHER" && ux_checkboxSearchAllOtherAreas.Checked) listOfTables += dr["table_name"].ToString().ToLower().Trim() + " ";
            }
            return listOfTables;
        }

        private void ux_tabcontrolQueryEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ux_tabcontrolQueryEngine.SelectedTab == ux_tabpageBasicQuery)
            //{
            //    if (queryResults.Tables.Contains("BasicSearchResults"))
            //    {
            //        ux_datagridviewQueryResults.DataSource = queryResults.Tables["BasicSearchResults"];
            //    }
            //    else
            //    {
            //        ux_datagridviewQueryResults.DataSource = null;
            //    }
            //    ux_datagridviewQueryResults.Size = ux_panelBasicQueryDGVPlaceholder.Size - new Size(6, 6);
            //    ux_datagridviewQueryResults.Location = new Point(3, 3);
            //    if(!ux_panelBasicQueryDGVPlaceholder.Controls.Contains(ux_datagridviewQueryResults)) ux_panelBasicQueryDGVPlaceholder.Controls.Add(ux_datagridviewQueryResults);
            //}
            //else
            //{
            //    if (queryResults.Tables.Contains("AdvancedSearchResults"))
            //    {
            //        ux_datagridviewQueryResults.DataSource = queryResults.Tables["AdvancedSearchResults"];
            //    }
            //    else
            //    {
            //        ux_datagridviewQueryResults.DataSource = null;
            //    }
            //    ux_datagridviewQueryResults.Size = ux_panelAdvancedQueryDGVPlaceholder.Size - new Size(6, 6);
            //    ux_datagridviewQueryResults.Location = new Point(3, 3);
            //    if(!ux_panelAdvancedQueryDGVPlaceholder.Controls.Contains(ux_datagridviewQueryResults)) ux_panelAdvancedQueryDGVPlaceholder.Controls.Add(ux_datagridviewQueryResults);
            //}
        }

        public DataSet GetDGVData(string dataviewName, DataRow[] groupList, int pageSize)
        {
            DataSet results = new DataSet();

            // Reset the escapeKeyPressed flag...
            escapeKeyPressed = false;

            if (groupList != null &&
                groupList.Length > 0)
            {
//string acids = "";
//string ivids = "";
//string ornos = "";
//string coops = "";
                string centerMessage = ux_statusCenterMessage.Text;
                string rightMessage = ux_statusRightMessage.Text;
                ContentAlignment centerMessageAlignment = ux_statusCenterMessage.TextAlign;
                int nextPage = pageSize;

                // Set the statusbar up to show the query progress...
                ux_statusCenterMessage.TextAlign = ContentAlignment.MiddleRight;
//ux_statusCenterMessage.Text = "Retrieving Data:";
ux_statusCenterMessage.Text = "";
// Update the statusbar...
ux_statusLeftMessage.Text = ux_statusLeftMessage.Tag.ToString();
//ux_statusRightMessage.Text = "   Press the escape key (esc) to cancel query   ";
                ux_statusProgressBar.Visible = true;
                ux_statusProgressBar.Minimum = 0;
                ux_statusProgressBar.Maximum = groupList.Length;
                ux_statusProgressBar.Step = pageSize;
                ux_statusProgressBar.Value = 0;
                ux_statusstripMain.Refresh();

                // Begin iterating through the collection of ACIDS, IVIDS, and ORNOS...
                int pageStart = 0;
                int pageStop = Math.Min(pageSize, groupList.Length);
                while (pageStart < groupList.Length && !escapeKeyPressed)
                {
//acids = "";
//ivids = "";
//ornos = "";
//coops = "";
string pkeyCollection = ":" + searchResultType.Replace("_", "") + "id=";
                    for (int i = pageStart; i < pageStop; i++)
                    {
//switch (searchResultType)
//{
//    case "accession":
//        acids += groupList[i]["ID"].ToString() + ",";
//        break;
//    case "inventory":
//        ivids += groupList[i]["ID"].ToString() + ",";
//        break;
//    case "order_request":
//        ornos += groupList[i]["ID"].ToString() + ",";
//        break;
//    case "cooperator":
//        coops += groupList[i]["ID"].ToString() + ",";
//        break;
//    default:
//        acids += groupList[i]["ID"].ToString() + ",";
//        break;
//}
pkeyCollection += groupList[i]["ID"].ToString() + ",";
                    }
                    // Update the paging indexes...
                    pageStart = pageStop;
                    pageStop = Math.Min((pageStart + pageSize), groupList.Length);
                    // Build the param string and get the data...
//string pkeyCollection = ":accessionid=" + acids.TrimEnd(',') + "; :inventoryid=" + ivids.TrimEnd(',') + "; :orderrequestid=" + ornos.TrimEnd(',') + "; :cooperatorid=" + coops.TrimEnd(',');
pkeyCollection = pkeyCollection.TrimEnd(',');
                    //DataSet pagedDataSet = GUIWebServices.GetData(false, _user, _password, resultsetName, pkeyCollection, 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
                    DataSet pagedDataSet = _sharedUtils.GetWebServiceData(dataviewName, pkeyCollection, 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
                    // Add the results to the dataset that will be returned...
                    if (results.Tables.Contains(dataviewName))
                    {
                        results.Tables[dataviewName].Merge(pagedDataSet.Tables[dataviewName].Copy());
                    }
                    else
                    {
                        results.Tables.Add(pagedDataSet.Tables[dataviewName].Copy());
                    }

                    // Update the progress bar on the statusbar...
                    ux_statusProgressBar.Value = pageStop;

                    // Check to see if we should bail out (at user request)...
                    Cursor currentCursor = Cursor.Current;
                    Application.DoEvents();
                    Cursor.Current = currentCursor;
                }

                // Now we need to get only distinct rows in the dataset to return...
                // First build a string array of the columns to return...
                ux_statusCenterMessage.Text = "Removing duplicate rows...  Please wait.";
                string[] columnNames = new string[results.Tables[dataviewName].Columns.Count];
                foreach (DataColumn dc in results.Tables[dataviewName].Columns)
                {
                    columnNames[dc.Ordinal] = dc.ColumnName;
                }
                DataTable dt = results.Tables[dataviewName].DefaultView.ToTable("distinct" + dataviewName, true, columnNames);
                results.Tables.Add(dt);
                results.Tables.Remove(dataviewName);
                results.Tables["distinct" + dataviewName].TableName = dataviewName;
                // Restore the statusbar back to it's original state...
                ux_statusCenterMessage.TextAlign = centerMessageAlignment;
                ux_statusCenterMessage.Text = centerMessage;
                ux_statusRightMessage.Text = rightMessage;
                ux_statusProgressBar.Visible = false;
                ux_statusstripMain.Refresh();
            }
            else
            {
                // If no primary keys were found return an empty dataset...
                results = _sharedUtils.GetWebServiceData(dataviewName, ":accessionid=; :inventoryid=; :orderrequestid=", 0, Int32.Parse(ux_numericupdownBasicMaxRecords.Value.ToString()));
            }

            if (results.Tables.Contains("ExceptionTable") &&
                results.Tables["ExceptionTable"].Rows.Count > 0)
            {
//MessageBox.Show("There was an unexpected error retrieving data for " + dataviewName + ".\n\nFull error message:\n" + results.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error retrieving data for {0}.\n\nFull error message:\n{1}", "Search Engine Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "GRINGlobalClientSearchTool_GetDGVDataMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}") &&
//    ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName, results.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
//}
//else if (ggMessageBox.MessageText.Contains("{0}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName);
//}
//else if (ggMessageBox.MessageText.Contains("{1}"))
//{
//    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, results.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
//}
string[] argsArray = new string[100];
argsArray[0] = dataviewName;
argsArray[1] = results.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
            }

            return results;
        }

        //private string GetFriendlyFieldName(DataColumn dc, string defaultName)
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

        private void ux_optionControlChanged(object sender, EventArgs e)
        {
//if (ux_radiobuttonBasicReturnAccessions.Checked) searchResultType = "accession";
//if (ux_radiobuttonBasicReturnInventory.Checked) searchResultType = "inventory";
//if (ux_radiobuttonBasicReturnOrders.Checked) searchResultType = "order_request";
//if (ux_radiobuttonBasicReturnCooperators.Checked) searchResultType = "cooperator";

            if (ux_radiobuttonDefaultFind.Checked)
            {
                ux_comboboxCustomFind.Enabled = false;
            }
            else
            {
                ux_comboboxCustomFind.Enabled = true;
            }

            ux_textboxBasicQueryText.Focus();
        }

        private void ux_checkboxSearchAreas_CheckedChanged(object sender, EventArgs e)
        {
            int checkedCount = 0;
            int uncheckedCount = 0;
            foreach (Control ctrl in ux_groupboxBasicSearchAreas.Controls)
            {
                if (ctrl.GetType() == typeof(CheckBox) &&
                    ctrl != ux_checkboxSearchAllAreas)
                {
                    if (((CheckBox)ctrl).Checked)
                    {
                        checkedCount++;
                    }
                    else
                    {
                        uncheckedCount++;
                    }
                }
            }
            if (checkedCount == 0)
            {
                ux_checkboxSearchAllAreas.CheckState = CheckState.Unchecked;
            }
            else if (uncheckedCount == 0)
            {
                ux_checkboxSearchAllAreas.CheckState = CheckState.Checked;
            }
            else
            {
                ux_checkboxSearchAllAreas.CheckState = CheckState.Indeterminate;
            }
        }

        private void ux_checkboxSearchAllAreas_CheckedChanged(object sender, EventArgs e)
        {
            bool currentCheckedStatus = ux_checkboxSearchAllAreas.Checked;
            if (ux_checkboxSearchAllAreas.CheckState != CheckState.Indeterminate)
            {
                ux_checkboxSearchAccessionArea.Checked = currentCheckedStatus;
                ux_checkboxSearchInventoryArea.Checked = currentCheckedStatus;
                ux_checkboxSearchOrderArea.Checked = currentCheckedStatus;
                ux_checkboxSearchCropTraitArea.Checked = currentCheckedStatus;
                ux_checkboxSearchTaxonomyArea.Checked = currentCheckedStatus;
                ux_checkboxSearchAllOtherAreas.Checked = currentCheckedStatus;
            }
        }

        private void ux_tabcontrolDataview_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            // Save all of the DGV settings to the app_settings memory cache...
            SetDGVQueryResultsDataviewUserSettings();
        }

        private void ux_tabcontrolDataview_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            TabControl tc = (TabControl)sender;
//string dataGridViewTableName = "get_accession";

            if (tc.SelectedIndex > -1)
            {
                if (tc.SelectedTab.Name == "ux_tabpageDataviewNewTab")
                {
                    int indexOfNewTab = tc.SelectedIndex;
                    // Add the tab to the tabcontrol...
                    _sharedUtils.ux_tabcontrolCreateNewTab(tc, indexOfNewTab);
                }
            }

            // Add any criteria in the criteria row to the query text box...
            ux_buttonAddToQuery.PerformClick();

            // Get the name of the dataview to use...
            if (ux_tabcontrolSTDataviews.SelectedTab == null ||
                ux_tabcontrolSTDataviews.SelectedTab.Tag == null ||
                ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() != typeof(GRINGlobal.Client.Common.DataviewProperties) ||
                string.IsNullOrEmpty(((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName))
            {
                return;
            }
//            else
//            {
//                if (ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() == typeof(GRINGlobal.Client.Common.DataviewProperties))
//                {
//                    dataGridViewTableName = ((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName;
//                }
//            }


//            // Go get the data from the server (based on which tab is active)...
////if (basicQuerySearchResults != null &&
////    tc.SelectedTab != null)
//if (!string.IsNullOrEmpty(dataGridViewTableName))
//            {
////DataSet ds = GetDGVData(tc.SelectedTab.Tag.ToString(), basicQuerySearchResults, 100);
////if (queryResults.Tables.Contains("BasicSearchResults")) queryResults.Tables.Remove("BasicSearchResults");
////queryResults.Tables.Add(ds.Tables[tc.SelectedTab.Tag.ToString()].Copy());
////queryResults.Tables[tc.SelectedTab.Tag.ToString()].TableName = "BasicSearchResults";
//DataSet ds = GetDGVData(dataGridViewTableName, basicQuerySearchResults, 100);
//if (queryResults.Tables.Contains("BasicSearchResults")) queryResults.Tables.Remove("BasicSearchResults");
//queryResults.Tables.Add(ds.Tables[dataGridViewTableName].Copy());
//queryResults.Tables[dataGridViewTableName].TableName = "BasicSearchResults";

//                // Reset the rowfilter and the sortorder...
//                DataGridViewSortOrder = "";
//                DataGridViewFilter = "";
//                queryResults.Tables["BasicSearchResults"].DefaultView.Sort = DataGridViewSortOrder;
//                queryResults.Tables["BasicSearchResults"].DefaultView.RowFilter = DataGridViewFilter;

//                // Build the search results DGV...
//                _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewQueryResults, queryResults.Tables["BasicSearchResults"]);
RefreshDGVData();

                // Build the search criteria DGV...
//DataTable dt = queryResults.Tables["BasicSearchResults"].Clone();
//_sharedUtils.BuildEditDataGridView(ux_datagridviewSearchCriteria, dt);
//DataRow newRow = ((DataTable)ux_datagridviewSearchCriteria.DataSource).NewRow();
//((DataTable)ux_datagridviewSearchCriteria.DataSource).Rows.Add(newRow);
//((DataTable)ux_datagridviewSearchCriteria.DataSource).AcceptChanges();

                DataTable dt = queryResults.Tables["BasicSearchResults"].Clone();
dt.TableName = "BasicSearchCriteria";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ExtendedProperties.Contains("gui_hint"))
                    {
                        switch (dc.ExtendedProperties["gui_hint"].ToString())
                        {
                            case "LARGE_SINGLE_SELECT_CONTROL":
                                dc.AllowDBNull = true;
                                dc.DefaultValue = DBNull.Value;
                                dc.DataType = typeof(string);
                                break;
                            case "SMALL_SINGLE_SELECT_CONTROL":
                                dc.AllowDBNull = true;
                                dc.DefaultValue = DBNull.Value;
                                dc.DataType = typeof(string);
                                break;
                            case "TOGGLE_CONTROL":
                                dc.AllowDBNull = true;
                                dc.DefaultValue = DBNull.Value;
                                dc.DataType = typeof(string);
                                dc.ExtendedProperties["gui_hint"] = "TEXT_CONTROL";
                                //dc.ExtendedProperties["gui_hint"] = "SMALL_SINGLE_SELECT_CONTROL";
                                //if (dc.ExtendedProperties.Contains("code_group_code")) dc.ExtendedProperties["code_group_code"] = 9;
                                //if (dc.ExtendedProperties.Contains("code_group_id")) dc.ExtendedProperties["code_group_id"] = 9;
                                break;
                            case "DATE_CONTROL":
                            case "INTEGER_CONTROL":
                            case "TEXT_CONTROL":
                            default:
                                dc.AllowDBNull = true;
                                dc.DefaultValue = DBNull.Value;
                                dc.DataType = typeof(string);
                                break;
                        }
                    }
                    if (dc.ExtendedProperties.Contains("is_primary_key") &&
                        dc.ExtendedProperties["is_primary_key"].ToString().ToUpper() == "Y")
                    {
                        dc.ExtendedProperties["is_primary_key"] = "N";
                        dc.AllowDBNull = true;
                        dc.AutoIncrement = false;
                        dc.ReadOnly = false;
                    }
// New code to 'reset' all column's readonly property so that users can edit all of the criteria fields...
//dc.ReadOnly = false;
if (dc.ExtendedProperties.Contains("is_readonly") &&
    dc.ExtendedProperties["is_readonly"].ToString().ToUpper() == "Y")
{
    dc.ExtendedProperties["is_readonly"] = "N";
    dc.ReadOnly = false;
}
                    if (dc.ExtendedProperties.Contains("is_nullable") &&
                       dc.ExtendedProperties["is_nullable"].ToString().ToUpper() == "N")
                    {
                        dc.ExtendedProperties["is_nullable"] = "Y";
                        dc.AllowDBNull = true;
                    }
                    if (dc.ExtendedProperties.Contains("is_autoincrement") &&
                        dc.ExtendedProperties["is_autoincrement"].ToString().ToUpper() == "Y")
                    {
                        dc.ExtendedProperties["is_autoincrement"] = "N";
                        dc.AutoIncrement = false;
                    }
                    if ((dc.ExtendedProperties.Contains("table_name") &&
                        string.IsNullOrEmpty(dc.ExtendedProperties["table_name"].ToString())) ||
                        (dc.ExtendedProperties.Contains("table_field_name") &&
                        string.IsNullOrEmpty(dc.ExtendedProperties["table_field_name"].ToString())))
                    {
                        dc.ReadOnly = true;
                    }
                    // The following line is here to handle columns with less varchar length than "<> NULL" or "= NULL"
                    if (dc.ExtendedProperties.Contains("max_length") &&
                        !string.IsNullOrEmpty(dc.ExtendedProperties["max_length"].ToString()) &&
                        dc.MaxLength > 0 &&
                        dc.MaxLength < 50)
                    {
                        dc.MaxLength = 50;
                        dc.ExtendedProperties["max_length"] = "50";
                    }
                }

                _sharedUtils.BuildEditDataGridView(ux_datagridviewSearchCriteria, dt);
                foreach (DataGridViewColumn dgvc in ux_datagridviewSearchCriteria.Columns)
                {
                    if (dgvc.GetType() == typeof(DataGridViewComboBoxColumn) &&
                        ((DataGridViewComboBoxColumn)dgvc).DataSource != null &&
                        ((DataGridViewComboBoxColumn)dgvc).DataSource.GetType() == typeof(DataTable))
                    {
                        DataTable comboboxDataTable = (DataTable)((DataGridViewComboBoxColumn)dgvc).DataSource;
                        if (comboboxDataTable.Rows[0]["display_member"].ToString().ToLower().Trim() == "[null]")
                        {
//((DataTable)dgvc.DataGridView.DataSource).Columns[dgvc.DataPropertyName].MaxLength = Math.Max(10, ((DataTable)dgvc.DataGridView.DataSource).Columns[dgvc.DataPropertyName].MaxLength);
                            comboboxDataTable.Rows[0]["display_member"] = "";
                            ((DataGridViewComboBoxColumn)dgvc).DefaultCellStyle.NullValue = "";
                            DataRow newComboboxDataRow = comboboxDataTable.NewRow();
                            newComboboxDataRow["display_member"] = "<> NULL";
                            newComboboxDataRow["value_member"] = "<>NULL";
                            foreach (DataColumn dc in comboboxDataTable.Columns)
                            {
                                // If there are any non-nullable fields - set them now...
                                if (!dc.AllowDBNull)
                                {
                                    newComboboxDataRow[dc.ColumnName] = -100;
                                }
                            }
                            comboboxDataTable.Rows.InsertAt(newComboboxDataRow, 1);
                            newComboboxDataRow = comboboxDataTable.NewRow();
                            newComboboxDataRow["display_member"] = "= NULL";
                            newComboboxDataRow["value_member"] = "=NULL";
                            foreach (DataColumn dc in comboboxDataTable.Columns)
                            {
                                // If there are any non-nullable fields - set them now...
                                if (!dc.AllowDBNull)
                                {
                                    newComboboxDataRow[dc.ColumnName] = -101;
                                }
                            }
                            comboboxDataTable.Rows.InsertAt(newComboboxDataRow, 1);
                            comboboxDataTable.AcceptChanges();
                        }
                    }
                }
                DataRow newRow = ((DataTable)ux_datagridviewSearchCriteria.DataSource).NewRow();
                ((DataTable)ux_datagridviewSearchCriteria.DataSource).Rows.Add(newRow);
                ((DataTable)ux_datagridviewSearchCriteria.DataSource).AcceptChanges();



//// Update the statusbar...
//if (ux_statusLeftMessage.Tag != null &&
//    ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
//    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
//{
//    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewQueryResults.Rows.Count.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
//}
//else if (ux_statusLeftMessage.Tag != null &&
//    ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
//{
//    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString());
//}
////ux_statusLeftMessage.Text = "Showing rows: " + ux_datagridviewQueryResults.Rows.Count.ToString() + " of " + ((DataTable)ux_datagridviewQueryResults.DataSource).Rows.Count.ToString() + " retrieved";
//                foreach (DataGridViewColumn dgvc in ux_datagridviewQueryResults.Columns)
//                {
//                    dgvc.SortMode = DataGridViewColumnSortMode.Programmatic;
//                    dgvc.ContextMenuStrip = ux_contextmenustripDGVResultsCell;
//                    DataColumn dc = ((DataTable)dgvc.DataGridView.DataSource).Columns[dgvc.DataPropertyName];
//                    dgvc.HeaderText = _sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName);
//                }
//                // Refresh DGV formatting...
//                RefreshDGVFormatting();
//            }

// Refresh DGV formatting...
RefreshDGVFormatting();

            //Cursor.Current = Cursors.Default;
            Cursor.Current = _cursorGG;
        }

        private void ux_tabcontrolDataview_MouseDown(object sender, MouseEventArgs e)
        {
            _sharedUtils.ux_tabcontrolMouseDownEvent(ux_tabcontrolSTDataviews, e);
        }

        private void ux_tabcontrolDataview_DragOver(object sender, DragEventArgs e)
        {
            _sharedUtils.ux_tabcontrolDragOverEvent(ux_tabcontrolSTDataviews, e);
        }

        private void ux_tabcontrolDataview_DragDrop(object sender, DragEventArgs e)
        {
            _sharedUtils.ux_tabcontrolDragDropEvent(ux_tabcontrolSTDataviews, e);
        }

        private void ux_dataviewmenuNewTab_Click(object sender, EventArgs e)
        {
            int indexOfNewTab = ux_tabcontrolSTDataviews.SelectedIndex;
            // Add the tab to the tabcontrol...
            _sharedUtils.ux_tabcontrolCreateNewTab(ux_tabcontrolSTDataviews, indexOfNewTab);
        }

        private void ux_dataviewmenuDeleteTab_Click(object sender, EventArgs e)
        {
            int indexOfTabToDelete = ux_tabcontrolSTDataviews.SelectedIndex;
            _sharedUtils.ux_tabcontrolRemoveTab(ux_tabcontrolSTDataviews, indexOfTabToDelete);

//// Save the changes to tabcontrol in the usersettings...
//SetAllUserSettings();
        }

        private void ux_dataviewmenuTabProperties_Click(object sender, EventArgs e)
        {
            int indexOfCurrentTab = ux_tabcontrolSTDataviews.SelectedIndex;
            string origDataViewName = "";
            // Remember the name of the current tabs dataview...
            if (ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() == typeof(GRINGlobal.Client.Common.DataviewProperties) &&
                !string.IsNullOrEmpty(((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName))
            {
                origDataViewName = ((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName;
            }

            // Modify the properties of the dataview...
            _sharedUtils.ux_tabcontrolShowProperties(ux_tabcontrolSTDataviews, indexOfCurrentTab);

            // If the user changed the dataview associated with this tab - refresh the data now...
            if (ux_tabcontrolSTDataviews.SelectedTab.Tag.GetType() == typeof(GRINGlobal.Client.Common.DataviewProperties) &&
                ((GRINGlobal.Client.Common.DataviewProperties)ux_tabcontrolSTDataviews.SelectedTab.Tag).DataviewName != origDataViewName)
            {
                //RefreshDGVData();
                //RefreshDGVFormatting();
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("WARNING!\nYou have changed the DataView associated with this tab.\n\nWould you like clear your search criteria and search results?", "Changed DataView", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "GRINGlobalClientSearchTool_ux_dataviewmenuTabPropertiesMessage1";
if (_sharedUtils != null && _sharedUtils.IsConnected) _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
string[] argsArray = new string[100];
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
if (System.Windows.Forms.DialogResult.Yes == ggMessageBox.ShowDialog())
{
    ux_buttonClearQuery.PerformClick();
}
                // Refresh the DataGridView now...
                ux_tabcontrolDataview_SelectedIndexChanged(ux_tabcontrolSTDataviews, e);
            }


//SetAllUserSettings();
//// Refresh the data view...
//RefreshMainDGVData();
//RefreshMainDGVFormatting();
//RefreshForm();
        }

        private void ux_checkboxShowAllDGVColumns_CheckedChanged(object sender, EventArgs e)
        {
            if (ux_datagridviewQueryResults.DataSource != null) RefreshDGVFormatting();
        }

        private void GRINGlobalClientSearchTool_KeyDown(object sender, KeyEventArgs e)
        {
            // Check to see if user desires to abort query request...
            if (e.KeyCode == Keys.Escape) escapeKeyPressed = true;

            // Check to see if the user desired to select cells (block-mode) instead of default full-row select...
            if (e.Alt)
            {
                ux_datagridviewQueryResults.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }
            else if(!e.Control)
            {
                ux_datagridviewQueryResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void ux_datagridviewSearchCriteria_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                ux_datagridviewQueryResults.HorizontalScrollingOffset = ux_datagridviewSearchCriteria.HorizontalScrollingOffset;
            }
        }

        private void ux_datagridviewSearchCriteria_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //ux_datagridviewSearchCriteria[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Yellow;
            RefreshDGVRowFormatting(ux_datagridviewSearchCriteria.Rows[0], true);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ux_datagridviewSearchCriteria.CurrentCell.Value = ux_datagridviewSearchCriteria.CurrentCell.DefaultNewRowValue;
            //DataRow dr = ((DataRowView)ux_datagridviewSearchCriteria.CurrentCell.OwningRow.DataBoundItem).Row;  //[ux_datagridviewSearchCriteria.CurrentCell.OwningColumn.Name];
            RefreshDGVRowFormatting(ux_datagridviewSearchCriteria.Rows[0], true);
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((DataTable)ux_datagridviewSearchCriteria.DataSource).RejectChanges();
            foreach (DataGridViewCell dgvc in ux_datagridviewSearchCriteria.Rows[0].Cells)
            {
                if (!dgvc.ReadOnly)
                {
                    dgvc.Value = null;
                }
            }
            RefreshDGVRowFormatting(ux_datagridviewSearchCriteria.Rows[0], true);
        }

        private void ux_datagridviewSearchCriteria_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            ux_datagridviewSearchCriteria.CurrentCell = ux_datagridviewSearchCriteria[e.ColumnIndex, e.RowIndex];
        }

        private void ux_buttonAddToQuery_Click(object sender, EventArgs e)
        {
            // Build search criteria...
            string searchCriteria = buildSearchCriteria();

            string[] searchCriteriaTokens = searchCriteria.Split(new string[] { " AND ", " OR " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in searchCriteriaTokens)
            {
                string fieldName = token.Remove(token.IndexOf(" ")).Trim();
                string fieldValue = token.Substring(token.IndexOf(" ")).Trim();
                if (ux_textboxBasicQueryText.Text.Contains(fieldName))
                {
                    System.Text.RegularExpressions.Match existingToken;
                    System.Text.RegularExpressions.Match newToken;
                    string replacementToken = "";

                    //
                    // See if this is an FKEY field that is handled with a table.field IN (...) pattern:  @"^\s*@\w+\.\w+\s+(?:not\s+|NOT\s+)*(?:in|IN)\s*\((?:\s*\S*\s*[,|\)])*"
                    //
                    // Look for a matching table.field IN (...) criteria token in the existing text in the textbox...
                    existingToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s+(?:not\s+|NOT\s+)*(?:in|IN)\s*\((?:\s*\S*\s*[,|\)])*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    // Look for a matching table.field IN (...) criteria token in the new searchCriteria string...
                    newToken = System.Text.RegularExpressions.Regex.Match(token.Trim(), @"^" + fieldName + @"\s+(?:not\s+|NOT\s+)*(?:in|IN)\s*\((?:\s*\S*\s*[,|\)])*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    
                    // If this new FKEY table.field already exists in the query textbox, merge the new one with the existing one...
                    if (existingToken.Success &&
                        newToken.Success)
                    {
                        // Looks like we are dealing with an FKEY field (like taxonomy_species)...
                        System.Text.RegularExpressions.Match existingCollection;
                        System.Text.RegularExpressions.Match newCollection;
                        List<string> mergedCollectionList = new List<string>();
                        List<string> newCollectionList = new List<string>();

                        // Get the existing collection of FKEYS and the new FKEYS to be added...
                        existingCollection = System.Text.RegularExpressions.Regex.Match(existingToken.Value.ToString().Trim(), @"\((?:\s*\S*\s*[,|\)])*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        newCollection = System.Text.RegularExpressions.Regex.Match(token.Trim(), @"\((?:\s*\S*\s*[,|\)])*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                        // Convert the Match to a list so we can merge the existing collection of FKEYS with the new FKEYS...
                        mergedCollectionList = existingCollection.Value.ToString().TrimStart('(').TrimEnd(')').Split(',').ToList();
                        newCollectionList = newCollection.Value.ToString().TrimStart('(').TrimEnd(')').Split(',').ToList();
                        foreach (string pkey in newCollectionList)
                        {
                            if (!mergedCollectionList.Contains(pkey))
                            {
                                mergedCollectionList.Add(pkey);
                            }
                        }

                        // Create the replacement token string...
                        replacementToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s+(?:not\s+|NOT\s+)*(?:in|IN)", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value.ToString().Trim();
                        replacementToken += " (";
                        StringBuilder sb = new StringBuilder(replacementToken);
                        foreach (string pkey in mergedCollectionList)
                        {
                            sb.Append(pkey + ",");
                        }
                        replacementToken = sb.ToString().TrimEnd(',') + ")";

                        // Finally...  Replace the existing table.field IN (...) with the new merged one...
                        ux_textboxBasicQueryText.Text = ux_textboxBasicQueryText.Text.Replace(existingToken.ToString().Trim(), replacementToken);
                    }

                    //
                    // Try processing a field criteria that contains a > or >= operator...
                    //
                    newToken = System.Text.RegularExpressions.Regex.Match(token.Trim(), @"^" + fieldName + @"\s*(?:>=|>)\s*\S*\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    
                    // If a > or >= criteria is requested, check to see if other math operators exist for this table.field...
                    if (newToken.Success)
                    {
                        // Look for a matching table.field > or >= criteria token in the existing text in the textbox...
                        existingToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s*(?:>=|>)\s*\S*\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if (existingToken.Success)
                        {
                            // Replace the existing criteria with this new one...
                            replacementToken = newToken.Value.ToString().Trim();
                        }
                        else
                        {
                            // Look for a contrasting table.field < or <= criteria token in the existing text in the textbox...
                            existingToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s*(?:<=|<)\s*\S*\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            if (existingToken.Success)
                            {
                                // Group the two contrasting criteria into a single group criteria of ( xxx.xxx > nnn AND xxx.xxx < mmm ) format...
                                replacementToken = "( " + newToken.Value.ToString().Trim() + " AND " + existingToken.Value.ToString().Trim() + " )";
                            }
                        }
                        // Finally...  Replace the existing table.field IN (...) with the new merged one...
                        ux_textboxBasicQueryText.Text = ux_textboxBasicQueryText.Text.Replace(existingToken.ToString().Trim(), replacementToken);
                    }

                    //
                    // Or Try processing a field criteria contains a < or <= operator...
                    //
                    newToken = System.Text.RegularExpressions.Regex.Match(token.Trim(), @"^" + fieldName + @"\s*(?:<=|<)\s*\S*\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    // If a > or >= criteria is requested, check to see if other math operators exist for this table.field...
                    if (newToken.Success)
                    {
                        // Look for a matching table.field > or >= criteria token in the existing text in the textbox...
                        existingToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s*(?:<=|<)\s*\S*\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if (existingToken.Success)
                        {
                            // Replace the existing criteria with this new one...
                            replacementToken = newToken.Value.ToString().Trim();
                        }
                        else
                        {
                            // Look for a contrasting table.field < or <= criteria token in the existing text in the textbox...
                            existingToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s*(?:>=|>)\s*\S*\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            if (existingToken.Success)
                            {
                                // Group the two contrasting criteria into a single group criteria of ( xxx.xxx > nnn AND xxx.xxx < mmm ) format...
                                replacementToken = "( " + existingToken.Value.ToString().Trim() + " AND " + newToken.Value.ToString().Trim() + " )";
                            }
                        }
                        // Finally...  Replace the existing table.field IN (...) with the new merged one...
                        ux_textboxBasicQueryText.Text = ux_textboxBasicQueryText.Text.Replace(existingToken.ToString().Trim(), replacementToken);
                    }

                    //
                    // Or try processing a field criteria containing an = operator...
                    //
//newToken = System.Text.RegularExpressions.Regex.Match(token.Trim(), @"^" + fieldName + @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+)|=|<>)\s*\S+\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
newToken = System.Text.RegularExpressions.Regex.Match(token.Trim(), @"^" + fieldName + @"\s*(?:=\s*\S+\s*|(?:\s+not)*\s+like\s+\S+\s*|\s+is(?:\s+not)*\s+null)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    // If a = criteria is requested, check to see if other math operators exist for this table.field...
                    if (newToken.Success &&
                        !ux_textboxBasicQueryText.Text.Contains(newToken.Value.ToString().Trim()))
                    {
                        // Look for a matching table.field = criteria token in the existing text in the textbox...
                        //@table.field\s*(?:=)\s*\S+\s*|\((@table.field\s*(?:=)\s*\S+\s*(?:\s+or\s+|\s+OR\s+|\)))+
                        //\s*@table.field\s*(?:=\s*\S+\s*|(?:\s+not)*\s+like\s+\S+\s*|\s+is(?:\s+not)*\s+null)|\((?:\s*@table.field\s*(?:=\s*\S+\s*|(?:\s+not)*\s+like\s+\S+\s*|\s+is(?:\s+not)*\s+null)(?:\s+and\s+|\s+or\s+|\s*\)))+\s*
//existingToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+)|=|<>)\s*\S+\s*|\((" + fieldName + @"\s*(?:(?:not\s+|NOT\s+)*(?:like\s+|LIKE\s+)|=)\s*\S+\s*(?:\s+or\s+|\s+OR\s+|\)))+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
existingToken = System.Text.RegularExpressions.Regex.Match(ux_textboxBasicQueryText.Text, fieldName + @"\s*(?:=\s*\S+\s*|(?:\s+not)*\s+like\s+\S+\s*|\s+is(?:\s+not)*\s+null)|\((?:\s*" + fieldName + @"\s*(?:=\s*\S+\s*|(?:\s+not)*\s+like\s+\S+\s*|\s+is(?:\s+not)*\s+null)(?:\s+and\s+|\s+or\s+|\s*\)))+\s*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
if (existingToken.Success)
{
    if (existingToken.Value.ToString().Trim().StartsWith("("))
    {
        // Replace the existing criteria with this new one...
        //replacementToken = newToken.Value.ToString().Trim();
        replacementToken = existingToken.Value.ToString().Trim().Replace(" )", " OR " + newToken.Value.ToString().Trim() + " )");
    }
    else
    {
        replacementToken = "( " + existingToken.Value.ToString().Trim() + " OR " + newToken.Value.ToString().Trim() + " )";
    }
    // Finally...  Replace the existing table.field IN (...) with the new merged one...
    ux_textboxBasicQueryText.Text = ux_textboxBasicQueryText.Text.Replace(existingToken.ToString().Trim(), replacementToken);
}
else
{
    if (string.IsNullOrEmpty(ux_textboxBasicQueryText.Text))
    {
        ux_textboxBasicQueryText.Text += newToken;
    }
    else
    {
        if (ux_radiobuttonBasicMatchAll.Checked)
        {
            ux_textboxBasicQueryText.Text += " AND " + newToken;
        }
        else
        {
            ux_textboxBasicQueryText.Text += " OR " + newToken;
        }
    }
}
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(ux_textboxBasicQueryText.Text))
                    {
                        ux_textboxBasicQueryText.Text += token;
                    }
                    else
                    {
                        if (ux_radiobuttonBasicMatchAll.Checked)
                        {
                            ux_textboxBasicQueryText.Text += " AND " + token;
                        }
                        else
                        {
                            ux_textboxBasicQueryText.Text += " OR " + token;
                        }
                    }
                }
            }
        }

        private void ux_buttonClearQueryText_Click(object sender, EventArgs e)
        {
            ux_textboxBasicQueryText.Text = "";
        }
        
        private void ux_buttonClearQuery_Click(object sender, EventArgs e)
        {
            ux_textboxBasicQueryText.Text = "";
            ((DataTable)ux_datagridviewSearchCriteria.DataSource).RejectChanges();
            foreach (DataGridViewCell dgvc in ux_datagridviewSearchCriteria.Rows[0].Cells)
            {
                if (!dgvc.ReadOnly)
                {
                    dgvc.Value = null;
                }
            }
            RefreshDGVRowFormatting(ux_datagridviewSearchCriteria.Rows[0], true);
            // Clear the search results with an empty array...
            basicQuerySearchResults = new DataRow[0];
            // Clear the currently active tab's dataview data...
            //if (queryResults.Tables.Contains("BasicSearchResults")) queryResults.Tables["BasicSearchResults"].Clear();
            if (ux_datagridviewQueryResults.DataSource != null &&
                ux_datagridviewQueryResults.DataSource.GetType() == typeof(DataTable)) ((DataTable)ux_datagridviewQueryResults.DataSource).Clear();
            RefreshDGVFormatting();
        }

        private void ux_textboxBasicQueryText_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            if (e.Button == MouseButtons.Left)
            {
                int pointerCharIndex = txtBox.GetCharIndexFromPosition(new Point(e.X, e.Y));
                // Check to see if the left mouse click contains the currently selected text
                // and if so start a Drag and Drop operation...
                if (pointerCharIndex > txtBoxSelectionStart + 1 &&
                    pointerCharIndex < (txtBoxSelectionStart + txtBoxSelectionLength - 2))
                {
                    string searchCriteria = "Search Tool Query :: ";
                    if (ux_radiobuttonDefaultFind.Checked)
                    {
                        searchCriteria += "ResolveTo=default :: ";
                    }
                    else
                    {
                        searchCriteria += "ResolveTo=" + searchResultType + " :: ";
                    }
                    searchCriteria += "SearchCriteria=" + txtBox.Text.Substring(System.Math.Max(txtBoxSelectionStart, 0), System.Math.Min(txtBoxSelectionLength, txtBox.Text.Length - System.Math.Max(txtBoxSelectionStart, 0)));
                    txtBox.DoDragDrop(searchCriteria, DragDropEffects.Copy);
                    // Reset the global selection indexes...
                    txtBoxSelectionStart = 0;
                    txtBoxSelectionLength = 0;
                    // Reselect the text (since the control cleared the select)...
                    txtBox.SelectionStart = txtBoxSelectionStart;
                    txtBox.SelectionLength = txtBoxSelectionLength;
                    txtBox.Refresh();
                }
            }
        }

        private void ux_textboxBasicQueryText_MouseMove(object sender, MouseEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            txtBox.Cursor = Cursors.IBeam;
            if (e.Button == MouseButtons.None)
            {
                int pointerCharIndex = txtBox.GetCharIndexFromPosition(new Point(e.X, e.Y));
                if (pointerCharIndex > txtBoxSelectionStart + 1 &&
                    pointerCharIndex < (txtBoxSelectionStart + txtBoxSelectionLength - 2))
                {
                    txtBox.Cursor = Cursors.Arrow;
                }
//this.Text = "Form1 [" + txtBox.SelectionStart.ToString() + ", " + txtBox.SelectionLength.ToString() + "][" + pointerCharIndex.ToString() + "]";
            }
        }

        private void ux_textboxBasicQueryText_MouseUp(object sender, MouseEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            txtBoxSelectionStart = txtBox.SelectionStart;
            txtBoxSelectionLength = txtBox.SelectionLength;
        }

        private void ux_textboxBasicQueryText_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void ux_textboxBasicQueryText_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            TextBox txtBox = (TextBox)sender;
            // Reselect the text (since the control cleared the select)...
            txtBox.SelectionStart = txtBoxSelectionStart;
            txtBox.SelectionLength = txtBoxSelectionLength;
            txtBox.Refresh();
        }

        private void ux_textboxBasicQueryText_DragLeave(object sender, EventArgs e)
        {
            //TextBox txtBox = (TextBox)sender;
            //// Reselect the text (since the control cleared the select)...
            //txtBox.SelectionStart = txtBoxSelectionStart;
            //txtBox.SelectionLength = txtBoxSelectionLength;
            //txtBox.Refresh();
        }

        private void ux_textboxBasicQueryText_DragOver(object sender, DragEventArgs e)
        {
            //TextBox txtBox = (TextBox)sender;
            //// Reselect the text (since the control cleared the select)...
            //txtBox.SelectionStart = txtBoxSelectionStart;
            //txtBox.SelectionLength = txtBoxSelectionLength;
            //txtBox.Refresh();
        }

        private void ux_contextmenustripDGVResultsCell_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // Now format the Search Criteria DGV row(s)...
            foreach (DataGridViewRow dgvr in ux_datagridviewQueryResults.Rows)
            {
                RefreshDGVRowFormatting(dgvr, true);
            }
        }
    }
}





/* // Code from btnDoSearch()...

            // Now go get the data from the database and return it to the DataGridView...
            DataSet GUIData = new DataSet();
            System.Data.Odbc.OdbcConnection objDBConnection;
            System.Data.Odbc.OdbcDataAdapter objDBDataAdapter;
            //string strDBConnection = "Provider=MSDAORA; User ID=" + strUsername + "; Password=" + strPassword + "; Data Source=npgs.ars-grin.gov";
            //string strDBConnection = "Driver={Microsoft ODBC for Oracle};Server=npgs.ars-grin.gov;Uid=" + strUsername + ";Pwd=" + strPassword;
            //string strDBConnection = "Driver={mySQL ODBC 3.51 Driver}; Server=129.186.187.168; Port=3306; Option=4; Database=prod; Uid=" + strUsername + "; Pwd=" + strPassword;
            string strDBConnection = "Driver={mySQL ODBC 3.51 Driver}; Server=127.0.0.1; Port=3306; Option=4; Database=prod; Uid=" + strUsername + "; Pwd=" + strPassword;

            // Create and open the database connection...
            objDBConnection = new System.Data.Odbc.OdbcConnection(strDBConnection);
            objDBDataAdapter = new System.Data.Odbc.OdbcDataAdapter(strSQLCommand, objDBConnection);
            objDBConnection.Open();

            // Now get the table/field data...
            objDBDataAdapter.SelectCommand.CommandText = strSQLCommand;
            objDBDataAdapter.Fill(GridViewData, "SearchResults");

            // Cleanup...
            objDBDataAdapter.Dispose();
            objDBConnection.Close();
            objDBConnection.Dispose();
*/



/*
                private void dgvMain_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
                {
                    {
                        DataObject doDragDropData;

                        if ((e.RowIndex > 0) && (dgvMain.Rows[e.RowIndex].Selected))
                        {
                            // Get the string of selected rows extracted from the GridView...
                            doDragDropData = BuildDragAndDropStringFromGridView(dgvMain);

                            // If there were selected rows lets start the drag-drop operation...
                            if (dgvMain.SelectedRows.Count > 0)
                            {
                                dgvMain.DoDragDrop(doDragDropData, DragDropEffects.Copy);
                            }
                        }
                    }
                }

                public DataObject BuildDragAndDropStringFromGridView(DataGridView dgv)
                {
                    DataObject doReturn = new DataObject();

                    // Write the rows out in text format first...
                    string strRows = "";
                    // Write out the column headers...
                    foreach (DataGridViewColumn dgvCol in dgv.Columns)
                    {
                        if (dgvCol.Visible) strRows += dgvCol.HeaderText + "\t";
                    }
                    // Write out the values in the visible cells of the selected rows...
                    foreach(DataGridViewRow dgvr in dgv.SelectedRows)
                    {
                        strRows += "\n";
                        foreach(DataGridViewCell dgvCell in dgvr.Cells)
                        {
                            if(dgvCell.Visible) strRows += dgvCell.FormattedValue.ToString() + "\t";
                        }
                    }

                    // Now write it out as a collection of data table rows (for tree view drag and drop).
                    DataGridViewSelectedRowCollection dgvsrcRows = dgv.SelectedRows;

                    // Set the data types into the data object and return...
                    doReturn.SetData(typeof(string), strRows);
                    doReturn.SetData(typeof(DataGridViewSelectedRowCollection), dgvsrcRows);

                    return doReturn;
                }
*/

