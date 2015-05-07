using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace GRINGlobal.Client.CuratorTool
{
    public partial class GRINGlobalClientCuratorTool : Form
    {
        //GrinGlobal.Client.GrinGlobalGUIWebServices.GUI GUIWebServices;
        SharedUtils _sharedUtils;
//        WebServices GRINGlobalWebServices;
//ImageList navigatorTreeViewImages = new ImageList();
        Cursor _cursorGG;
        Cursor _cursorREQ;
        Cursor _cursorLUT;
        bool escapeKeyPressed = false;
        string _saveDGVDataDumpFile;
        string _saveListDataDumpFile;
        string _pathSeparator = "|";
        string username;
        string password;
        string passwordClearText;
        string url;
        string _usernameCooperatorID;
        string _currentCooperatorID;
        string site;
        string languageCode;
        string lastFullPath;
        string lastTabName;
        string commonUserApplicationDataPath;
        string roamingUserApplicationDataPath;
        string userSettingsXMLFilePath;
        string localDBInstance;
        DataSet grinData = new DataSet();
        DataTable userItemList = new DataTable();
        int mouseClickDGVColumnIndex;
        int mouseClickDGVRowIndex;
        FormsData[] localFormsAssemblies;
        BindingSource defaultBindingSource;
        Form dataviewForm;
        char _lastDGVCharPressed;

        //interface IGRINGlobalDataForm
        //{
        //    string FormName { get; }
        //    string PreferredDataview { get; }
        //    bool EditMode { get; set; }
        //}

        public string AFormName
        {
            get
            {
                return "Hello World";
            }
        }

        public GRINGlobalClientCuratorTool(string[] args)
        {
            InitializeComponent();

            // Parse through the commandline parameters...
            username = GetCommandLineParameter(args, "-user", "");
            passwordClearText = GetCommandLineParameter(args, "-password", "");
            password = SharedUtils.SHA1EncryptionBase64(passwordClearText);
            url = GetCommandLineParameter(args, "-url", "");
            _saveDGVDataDumpFile = GetCommandLineParameter(args, "-savedgvdatadumpfile", null);
            _saveListDataDumpFile = GetCommandLineParameter(args, "-savelistdatadumpfile", null);
        }

        private string GetCommandLineParameter(string[] args, string key, string defaultValue)
        {
            string value = defaultValue;
            char[] delimiters = new char[] { '=' };
            foreach (string keyValuePair in args)
            {
                if (keyValuePair.Contains(key)) value = keyValuePair.Split(delimiters)[1].Trim();
            }
            return value;
        }

        private void GrinGlobalClient_Load(object sender, EventArgs e)
        {
            bool validLogin = false;
//            bool connectedToWebService = false;
//            int selectedCNOIndex = -1;
            //string selectedNodeFullPath = "";
            //username = "";
            //password = "";
            _usernameCooperatorID = "";// "117534";
            _currentCooperatorID = "";// "117534";
            site = "";// "NC7";
            languageCode = "";
            lastFullPath = "";
            lastTabName = "";
            commonUserApplicationDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool";
            roamingUserApplicationDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool";
            userSettingsXMLFilePath = roamingUserApplicationDataPath + @"\UserSettings_v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.Build.ToString() + ".xml";

            // Wireup the binding navigator and the Main datagridview...
            // NOTE:(right now the binding source is empty - but later on it will get bound to a data table)...
            defaultBindingSource = new BindingSource();
            defaultBindingSource.DataSource = new DataTable();
            ux_bindingnavigatorMain.BindingSource = defaultBindingSource;
            ux_datagridviewMain.DataSource = defaultBindingSource;

//// Load the images for the tree view(s)...
//navigatorTreeViewImages.ColorDepth = ColorDepth.Depth32Bit;
//navigatorTreeViewImages.Images.Add("active_folder", Icon.ExtractAssociatedIcon(@"Images\active_Folder.ico"));
//navigatorTreeViewImages.Images.Add("inactive_folder", Icon.ExtractAssociatedIcon(@"Images\inactive_Folder.ico"));
//navigatorTreeViewImages.Images.Add("active_INVENTORY_ID", Icon.ExtractAssociatedIcon(@"Images\active_INVENTORY_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_INVENTORY_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_INVENTORY_ID.ico"));
//navigatorTreeViewImages.Images.Add("active_ACCESSION_ID", Icon.ExtractAssociatedIcon(@"Images\active_ACCESSION_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_ACCESSION_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_ACCESSION_ID.ico"));
//navigatorTreeViewImages.Images.Add("active_ORDER_REQUEST_ID", Icon.ExtractAssociatedIcon(@"Images\active_ORDER_REQUEST_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_ORDER_REQUEST_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_ORDER_REQUEST_ID.ico"));
//navigatorTreeViewImages.Images.Add("active_COOPERATOR_ID", Icon.ExtractAssociatedIcon(@"Images\active_COOPERATOR_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_COOPERATOR_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_COOPERATOR_ID.ico"));

//navigatorTreeViewImages.Images.Add("active_GEOGRAPHY_ID", Icon.ExtractAssociatedIcon(@"Images\active_GEOGRAPHY_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_GEOGRAPHY_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_GEOGRAPHY_ID.ico"));
//navigatorTreeViewImages.Images.Add("active_TAXONOMY_GENUS_ID", Icon.ExtractAssociatedIcon(@"Images\active_TAXONOMY_GENUS_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_TAXONOMY_GENUS_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_TAXONOMY_GENUS_ID.ico"));
//navigatorTreeViewImages.Images.Add("active_CROP_ID", Icon.ExtractAssociatedIcon(@"Images\active_CROP_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_CROP_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_CROP_ID.ico"));
//navigatorTreeViewImages.Images.Add("active_CROP_TRAIT_ID", Icon.ExtractAssociatedIcon(@"Images\active_CROP_TRAIT_ID.ico"));
//navigatorTreeViewImages.Images.Add("inactive_CROP_TRAIT_ID", Icon.ExtractAssociatedIcon(@"Images\inactive_CROP_TRAIT_ID.ico"));

//navigatorTreeViewImages.Images.Add("new_tab", Icon.ExtractAssociatedIcon(@"Images\GG_newtab.ico"));
//navigatorTreeViewImages.Images.Add("search", Icon.ExtractAssociatedIcon(@"Images\GG_search.ico"));


            try
            {
                // Load the wizards from the same directory (and all subdirectories) where the Curator Tool was launched...
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                System.IO.FileInfo[] dllFiles = di.GetFiles("Wizards\\*.dll", System.IO.SearchOption.AllDirectories);
                if (dllFiles != null && dllFiles.Length > 0)
                {
                    for (int i = 0; i < dllFiles.Length; i++)
                    {
                        System.Reflection.Assembly newAssembly = System.Reflection.Assembly.LoadFile(dllFiles[i].FullName);
                        foreach (System.Type t in newAssembly.GetTypes())
                        {
                            if (t.GetInterface("IGRINGlobalDataWizard", true) != null)
                            {
                                System.Reflection.ConstructorInfo constInfo = t.GetConstructor(new Type[] { typeof(string), typeof(SharedUtils) });

                                if (constInfo != null)
                                {
                                    string pkeyCollection = ":accessionid=; :inventoryid=; :orderrequestid=; :cooperatorid=; :geographyid=; :taxonomygenusid=; :cropid=";
                                    // Instantiate an object of this type to load...
                                    Form wizardForm = (Form)constInfo.Invoke(new object[] { pkeyCollection, _sharedUtils });
                                    // Get the Form Name and button with this name...
                                    System.Reflection.PropertyInfo propInfo = t.GetProperty("FormName", typeof(string));
                                    string formName = (string)propInfo.GetValue(wizardForm, null);
                                    if (string.IsNullOrEmpty(formName)) formName = t.Name;
                                    ToolStripButton tsbWizard = new ToolStripButton(formName, Icon.ExtractAssociatedIcon(newAssembly.ManifestModule.FullyQualifiedName).ToBitmap(), ux_buttonWizard_Click, "toolStripButton" + newAssembly.ManifestModule.Name);
                                    tsbWizard.Tag = "Wizards\\" + newAssembly.ManifestModule.Name;
                                    toolStrip1.Items.Add(tsbWizard);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error binding to Wizard Form.\nError Message: {0}", "Wizard Binding Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "GrinGlobalClient_LoadMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, err.Message);
ggMessageBox.ShowDialog();
            }
            
            // Create the middle tier utilities class and connect to the web services...
            _sharedUtils = new SharedUtils(url, username, passwordClearText, false);


// Display the splash page to let the user know that things are happening...
Splash splash = new Splash();
splash.StartPosition = FormStartPosition.CenterScreen;
splash.Show();
splash.Update();



            //if (validLogin)
            if (_sharedUtils.IsConnected)
            {
                localDBInstance = _sharedUtils.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
                localDBInstance = "GRINGlobal_" + localDBInstance;
//_sharedUtils = new SharedUtils(GRINGlobalWebServices.Url, username, password, localDBInstance, cno);
                username = _sharedUtils.Username;
                password = _sharedUtils.Password;
                passwordClearText = _sharedUtils.Password_ClearText;
                url = _sharedUtils.Url;
                _usernameCooperatorID = _sharedUtils.UserCooperatorID;
//_currentCooperatorID = _sharedUtils.UserCooperatorID;
                site = _sharedUtils.UserSite;
                languageCode = _sharedUtils.UserLanguageCode.ToString();

                // Load the application data...
                LoadApplicationData();

// Check the status of lookup tables and warn the user if some tables are missing data...
LookupTableStatusCheck();

                // Wire up the list of valid GG servers to the combobox...
                ux_comboboxActiveWebService.DataSource = new BindingSource(_sharedUtils.WebServiceURLs, null);
                ux_comboboxActiveWebService.DisplayMember = "Key";
                ux_comboboxActiveWebService.ValueMember = "Value";
                ux_comboboxActiveWebService.SelectedValue = _sharedUtils.Url;
// Indicate to the user which URL is the active GG server...
if (ux_statusCenterMessage.Tag != null)
{
    if (ux_statusCenterMessage.Tag.ToString().Contains("{0}"))
    {
        ux_statusCenterMessage.Text = string.Format(ux_statusCenterMessage.Tag.ToString(), _sharedUtils.Url);
    }
    else
    {
        ux_statusCenterMessage.Text = ux_statusCenterMessage.Tag.ToString() + " (" + _sharedUtils.Url + ")";
    }
}
else
{
    ux_statusCenterMessage.Text = ux_statusCenterMessage.Text + " (" + _sharedUtils.Url + ")";
}

// Get the list of Dataview Forms embedded in assemblies in the CT Forms directiory...
localFormsAssemblies = _sharedUtils.GetDataviewFormsData();

                // Save the list of valid web service urls...
                // But first make sure the roaming profile directory exists...
                if (!System.IO.Directory.Exists(roamingUserApplicationDataPath)) System.IO.Directory.CreateDirectory(roamingUserApplicationDataPath);
                // Now save the list of GRIN-Global servers...
                System.IO.StreamWriter sw = new System.IO.StreamWriter(roamingUserApplicationDataPath + @"\WebServiceURL.txt");
                foreach (KeyValuePair<string, string> kv in ux_comboboxActiveWebService.Items)
                {
                    if (kv.Key != "New...")
                    {
                        sw.WriteLine(kv.Key + "\t" + kv.Value);
                    }
                }
                sw.Close();
                sw.Dispose();

                // Load the cursors for the DGV...
                _cursorGG = _sharedUtils.LoadCursor(@"Images\cursor_GG.cur");
                if (_cursorGG == null) _cursorGG = Cursors.Default;
                _cursorLUT = _sharedUtils.LoadCursor(@"Images\cursor_LUT.cur");
                if (_cursorLUT == null) _cursorLUT = Cursors.Default;
                _cursorREQ = _sharedUtils.LoadCursor(@"Images\cursor_REQ.cur");
                if (_cursorREQ == null) _cursorREQ = Cursors.Default;
//this.Cursor = _cursorGG;
ux_datagridviewMain.Cursor = _cursorGG;
            }
            else
            {
                // Login aborted - disable controls...
                ux_tabcontrolDataviewOptions.Enabled = false;
                ux_tabcontrolCTDataviews.Enabled = false;
                ux_datagridviewMain.Enabled = false;
                ux_comboboxCNO.Enabled = false;
                ux_buttonEditData.Enabled = false;
                // Close the application???
                this.Close();
            }

// Close the splash page...
splash.Close();
        }

        private void GrinGlobalClient_Shown(object sender, EventArgs e)
        {
//// Check the status of lookup tables and warn the user if some tables are missing data...
//LookupTableStatusCheck();

            // Not sure why the VS Designer keeps changing the size of the checklistbox control - but it does...
            // so I put this next line of code in here to make sure the control is always the correct size.
            ux_checkedlistboxViewedColumns.Size = new Size(160, 409);

            // Don't wireup this event handler for index changes until after the application is fully loaded...
            ux_comboboxActiveWebService.SelectedIndexChanged += new System.EventHandler(this.ux_comboboxActiveWebService_SelectedIndexChanged);

            // Should be safe to wireup the event handlers for the bindingSource now...
            defaultBindingSource.CurrentChanged += new EventHandler(defaultBindingSource_CurrentChanged);
            //ux_bindingnavigatorMain.BindingSource.DataSourceChanged += new EventHandler(ux_bindingnavigatorMain_BindingSource_DataSourceChanged);
            //ux_bindingnavigatorMain.BindingSource.CurrentChanged += new EventHandler(ux_bindingnavigatorMain_BindingSource_CurrentChanged);
            //ux_bindingnavigatorMain.BindingSource.CurrentItemChanged += new EventHandler(BindingSource_CurrentItemChanged);

            // Wire up the event handler for formatting cells in readonly mode...
            ux_datagridviewMain.CellFormatting += new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_ReadOnlyDGVCellFormatting);
        }

        private void LoadApplicationData()
        {
            int numGroupListTabs = -1;
            int selectedGroupListTabIndex = -1;
            bool warnWhenLUTablesAreOutdated = true;
            int maxRowsReturned = 1000;
            int queryPageSize = 10;

int langCode = 1;
if (!int.TryParse(languageCode, out langCode))
{
    langCode = 1;
}

//AppSettings appSettings = new AppSettings(_sharedUtils, langCode);
//appSettings.Load();
//appSettings.UpdateControls(this.Controls);
//appSettings.UpdateComponents(this.components.Components);
_sharedUtils.UpdateComponents(this.components.Components, this.Name);
_sharedUtils.UpdateControls(this.Controls, this.Name);
// Save the statusbar text for the left, center, and right controls...
if (string.IsNullOrEmpty(ux_statusLeftMessage.Text)) ux_statusLeftMessage.Text = "";
if (string.IsNullOrEmpty(ux_statusCenterMessage.Text)) ux_statusCenterMessage.Text = "";
if (string.IsNullOrEmpty(ux_statusRightMessage.Text)) ux_statusRightMessage.Text = "";
ux_statusLeftMessage.Tag = ux_statusLeftMessage.Text;
ux_statusCenterMessage.Tag = ux_statusCenterMessage.Text;
ux_statusRightMessage.Tag = ux_statusRightMessage.Text;
// Clear the text for the right status bar message...
ux_statusRightMessage.Text = "";

            // Temp disable the control (until the standard tables are loaded)...
            ux_tabcontrolDataviewOptions.Enabled = false;
            ux_tabcontrolCTDataviews.Enabled = false;
            ux_datagridviewMain.Enabled = false;
            ux_comboboxCNO.Enabled = false;
            ux_buttonEditData.Enabled = true;
 
// Don't wireup this event handler for index changes until after the Cooperators are fully loaded and the app is ready to choose the correct Coop...
this.ux_comboboxCNO.SelectedIndexChanged -= new System.EventHandler(this.ux_comboboxCNO_SelectedIndexChanged);
this.ux_comboboxCNO.SelectedIndexChanged -= new System.EventHandler(this.ux_comboboxCNO_SelectedIndexChanged);

//// Instantiate a new user settings class...
//if (userSettings != null) userSettings = null;
//userSettings = new UserSettings(GRINGlobalWebServices, cno);

//// Get user settings from local xml file or from database if local file does not exist...
//// The file name uses the build number to assist with versioning (from <major version>.<minor version>.<build number>.<revision>)
////userSettings.LoadXML(userSettingsXMLFilePath);
//userSettings.Load();

////            // Instantiate a new lookup tables class...
////            if (lookupTables != null) lookupTables = null;
//////            lookupTables = new LookupTables(GUIWebServices.Url, username, password, localDBInstance);

            //// Load the MRU Lookup Tables..
            //backgroundWorker1.RunWorkerAsync(roamingUserApplicationDataPath); // In the background
            //lookupTables = LoadStandardTables(roamingUserApplicationDataPath); // In the foreground
//lookupTables = LoadStandardTables(localDBInstance); // In the foreground
LoadStandardTables();

//int.TryParse(GetUserSetting(cno, "ux_comboboxCNO", "SelectedIndex", "-1"), out selectedCNOIndex);
            //int.TryParse(GetUserSetting(cno, "ux_tabcontrolDataview", "TabPages.Count", "-1"), out numResultsetTabs);
            //int.TryParse(GetUserSetting(cno, "ux_tabcontrolGroupListNavigator", "TabPages.Count", "-1"), out numGroupListTabs);
            //int.TryParse(GetUserSetting(cno, "ux_tabcontrolGroupListNavigator", "SelectedIndex", "-1"), out selectedGroupListTabIndex);
//int.TryParse(userSettings["ux_tabcontrolDataview", "TabPages.Count"], out numResultsetTabs);
//int.TryParse(userSettings["ux_tabcontrolGroupListNavigator", "TabPages.Count"], out numGroupListTabs);
//int.TryParse(userSettings["ux_tabcontrolGroupListNavigator", "SelectedIndex"], out selectedGroupListTabIndex);
            int.TryParse(_sharedUtils.GetUserSetting("ux_tabcontrolGroupListNavigator", "TabPages.Count", "-1"), out numGroupListTabs);
            int.TryParse(_sharedUtils.GetUserSetting("ux_tabcontrolGroupListNavigator", "SelectedIndex", "-1"), out selectedGroupListTabIndex);

            // Get the setting for LU table warnings...
            if (bool.TryParse(_sharedUtils.GetUserSetting(ux_checkboxWarnWhenLUTablesAreOutdated.Name, "Checked", warnWhenLUTablesAreOutdated.ToString()), out warnWhenLUTablesAreOutdated))
            {
                ux_checkboxWarnWhenLUTablesAreOutdated.Checked = warnWhenLUTablesAreOutdated;
            }
            else
            {
                ux_checkboxWarnWhenLUTablesAreOutdated.Checked = true;
            }
            // Get the setting for max number of rows to return...
            if (int.TryParse(_sharedUtils.GetUserSetting(ux_numericupdownMaxRowsReturned.Name, "Value", maxRowsReturned.ToString()), out maxRowsReturned))
            {
                ux_numericupdownMaxRowsReturned.Value = maxRowsReturned;
            }
            else
            {
                ux_numericupdownMaxRowsReturned.Value = 1000;
            }
            // Get the setting for query page size to use...
            if (int.TryParse(_sharedUtils.GetUserSetting(ux_numericupdownQueryPageSize.Name, "Value", queryPageSize.ToString()), out queryPageSize))
            {
                ux_numericupdownQueryPageSize.Value = queryPageSize;
            }
            else
            {
                ux_numericupdownQueryPageSize.Value = 10;
            }

            // Build the horizontal Dataview tabs TabControl...
            _sharedUtils.BuildDataviewTabControl(ux_tabcontrolCTDataviews);
            // Bind the image list to the Dataview tabs TabControl (for the New Tab tab)...
//ux_tabcontrolCTDataviews.ImageList = navigatorTreeViewImages;
ux_tabcontrolCTDataviews.ImageList = BuildTabControlImageList();
            // Bind an image to the NewTab tab...
//ux_tabpageDataviewNewTab.ImageKey = "new_tab";
            if (ux_tabcontrolCTDataviews.TabPages.ContainsKey("ux_tabpageDataviewNewTab")) ux_tabcontrolCTDataviews.TabPages["ux_tabpageDataviewNewTab"].ImageKey = "new_tab";

            // Set the image for the Search button...
//ux_buttonSearch.ImageList = navigatorTreeViewImages;
ux_buttonSearch.ImageList = BuildTabControlImageList();
            ux_buttonSearch.ImageKey = "search";
            ux_buttonSearch.ImageAlign = ContentAlignment.MiddleLeft;

// Loading vital lookup tables is done and the dataview tabs are built - enable the controls...
ux_tabcontrolDataviewOptions.Enabled = true;
ux_tabcontrolCTDataviews.Enabled = true;
ux_datagridviewMain.Enabled = true;
ux_comboboxCNO.Enabled = true;
ux_buttonEditData.Enabled = true;


// Load the list navigator combobox with cooperators...
LoadCooperators(localDBInstance);
// Deselect the CNO in the cooperator combobox to make sure that when the following SelectedValue=cno will trigger the index changed event...
ux_comboboxCNO.SelectedValue = -1;
// We are done getting system data time to wireup this event handler...
this.ux_comboboxCNO.SelectedIndexChanged += new System.EventHandler(this.ux_comboboxCNO_SelectedIndexChanged);
// Select the CNO (from login) in the cooperator combobox (this will trigger code to populate the tabs and treeviews too)...
ux_comboboxCNO.SelectedValue = _usernameCooperatorID;

            // Save the state of the statusbar (to restore later)...
            ux_statusstripMain.DefaultStateSave();

//// Set the statusbar up to show the progress of standard table loading...
//ux_statusCenterMessage.TextAlign = ContentAlignment.MiddleRight;
//ux_statusCenterMessage.Text = "Loading lookup tables...";
//ux_statusRightProgressBar.Visible = true;
//ux_statusRightProgressBar.Minimum = 0;
//ux_statusRightProgressBar.Maximum = 100;
//ux_statusRightProgressBar.Step = 5;
//ux_statusRightProgressBar.Value = 0;
//ux_statusstripMain.Refresh();

                // Now load the Full Lookup Tables in the background...
//                backgroundWorker1.RunWorkerAsync(commonUserApplicationDataPath);
                // Disable the reset lookup tables button (so that we don't have two threads
                // doing the same thing...
//                ux_buttonResetLookupTables.Enabled = false;

                // Make sure the commonUserApplicationDataPath exists
            //ensureAppDataPathExists(commonUserApplicationDataPath);
                //// and then add a copy of the empty mdb lookup database to the directory
                //// if one is not already there...
                //string junk = commonUserApplicationDataPath + @"\Grin-Global_LookupTables.mdb";
                //if(!System.IO.File.Exists(commonUserApplicationDataPath + @"\Grin-Global_LookupTables.mdb"))
                //{
                //    System.IO.File.Copy("Grin-Global_EmptyLookupTables.mdb", commonUserApplicationDataPath + @"\Grin-Global_LookupTables.mdb");
                //}

                //// Restore the Group List Tab's saved user settings and select the node in each tab's treeview...
                //TabControl tc;
                //TreeView tv;
                //TreeNode[] tnc;
                //tc = (TabControl)this.Controls.Find("ux_tabcontrolGroupListNavigator", true)[0];
                //// Select the last used tab...
                //if (tc != null && selectedGroupListTabIndex > -1 && tc.TabPages.Count > selectedGroupListTabIndex) tc.SelectedIndex = selectedGroupListTabIndex;
                //// Iterate through all tabs and set the selected node in the tab's treeview...
                //foreach (TabPage tp in tc.TabPages)
                //{
                //    if (tp.Name != "ux_tabpageGroupListNavigatorNewTab")
                //    {
                //        tv = (TreeView)tc.Controls.Find(tp.Name + "TreeView", true)[0];
                //        selectedNodeFullPath = GetUserSetting(cno, tv.Name, "SelectedNode.FullPath", "");
                //        tnc = tv.Nodes.Find(selectedNodeFullPath.Substring(selectedNodeFullPath.LastIndexOf(tv.PathSeparator) + 1), true);
                //        foreach (TreeNode tn in tnc)
                //        {
                //            if (tn.FullPath == selectedNodeFullPath)
                //            {
                //                tv.SelectedNode = tn;
                //                tn.Expand();
                //            }
                //        }
                //    }
                //}
                //// Attempt to build the dynamic show tabs menu...
                //// First we need to get a list of all of the tab names for this cno and then
                //// we will use this neat trick (the ToTable method) to get the distinct values from a table...
                //DataTable dt = userSettings.Tables["GETGROUPS"].DefaultView.ToTable(true, "TABNAME");
                //string tabOrder = GetUserSetting(cno, ux_tabcontrolGroupListNavigator.Name, "TabPages.Order", "");
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (!dr["TABNAME"].ToString().StartsWith("SPGP_" /*+ strCNO*/))
                //    {
                //        ToolStripMenuItem tsmi = new ToolStripMenuItem(dr["TABNAME"].ToString(), null, tsmi_Click);
                //        if (tabOrder.Contains(dr["TABNAME"].ToString())) tsmi.Checked = true;
                //        navigatorShowTabs.DropDownItems.Add(tsmi);
                //    }
                //}
        }

        //private void BuildDataviewTabControl()
        //{
        //    int numDataviewTabs = -1;

        //    // Clear the dataview tabs...
        //    ux_tabcontrolDataview.TabPages.Clear();

        //    // Now add back in the tabpage for adding new tabpages...
        //    if (!ux_tabcontrolGroupListNavigator.TabPages.ContainsKey("ux_tabpageDataviewNewTab"))
        //    {
        //        ux_tabcontrolDataview.TabPages.Add(ux_tabpageDataviewNewTab);
        //    }

        //    // Get the number of tabpages saved in the current user's settings...
        //    int.TryParse(_sharedUtils.GetUserSetting("ux_tabcontrolDataview", "TabPages.Count", "-1"), out numDataviewTabs);

        //    // Create the dataview tabs...
        //    if (numDataviewTabs > 0)
        //    {
        //        for (int i = 0; i < numDataviewTabs; i++)
        //        {
        //            //string tabText = GetUserSetting(cno, "ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].Text", "Accessions");
        //            //string tabTag = GetUserSetting(cno, "ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].Tag", "GET_ACCESSION");
        //            //string tabText = userSettings["ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].Text"];
        //            //string tabTag = userSettings["ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].Tag"];
        //            DataviewProperties dp = new DataviewProperties();
        //            //dp.TabName = userSettings["ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].TabName"];
        //            //dp.DataviewName = userSettings["ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].DataviewName"];
        //            //dp.StrongFormName = userSettings["ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].FormName"];
        //            //dp.ViewerStyle = userSettings["ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].ViewerStyle"];
        //            //dp.AlwaysOnTop = userSettings["ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].AlwaysOnTop"];
        //            dp.TabName = _sharedUtils.GetUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].TabName", "");
        //            dp.DataviewName = _sharedUtils.GetUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].DataviewName", "");
        //            dp.StrongFormName = _sharedUtils.GetUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].FormName", "");
        //            dp.ViewerStyle = _sharedUtils.GetUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].ViewerStyle", "");
        //            dp.AlwaysOnTop = _sharedUtils.GetUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].AlwaysOnTop", "");
        //            _sharedUtils.ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(i, ux_tabcontrolDataview.TabPages.IndexOf(ux_tabpageDataviewNewTab)));
        //        }
        //    }
        //    else
        //    {
        //        //ux_tabcontrolDataview.TabPages.Insert(ux_tabcontrolDataview.TabPages.IndexOf(ux_tabpageDataviewNewTab), tabPage1);
        //        // Make the default Accession dataview tab...
        //        DataviewProperties dp = new DataviewProperties();
        //        dp.TabName = tabPage1.Text;
        //        dp.DataviewName = "get_accession";
        //        dp.StrongFormName = "";
        //        dp.ViewerStyle = "Spreadsheet";
        //        dp.AlwaysOnTop = "false";
        //        _sharedUtils.ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(0, ux_tabcontrolDataview.TabPages.IndexOf(ux_tabpageDataviewNewTab)));
        //        //ux_tabcontrolDataview.TabPages.Insert(ux_tabcontrolDataview.TabPages.IndexOf(ux_tabpageDataviewNewTab), tabPage2);
        //        // Make the default Inventory dataview tab...
        //        dp = new DataviewProperties();
        //        dp.TabName = tabPage2.Text;
        //        dp.DataviewName = "get_inventory";
        //        dp.StrongFormName = "";
        //        dp.ViewerStyle = "Spreadsheet";
        //        dp.AlwaysOnTop = "false";
        //        _sharedUtils.ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(1, ux_tabcontrolDataview.TabPages.IndexOf(ux_tabpageDataviewNewTab)));
        //        //ux_tabcontrolDataview.TabPages.Insert(ux_tabcontrolDataview.TabPages.IndexOf(ux_tabpageDataviewNewTab), tabPage3);
        //        // Make the default Orders dataview tab...
        //        dp = new DataviewProperties();
        //        dp.TabName = tabPage3.Text;
        //        dp.DataviewName = "get_order_request";
        //        dp.StrongFormName = "";
        //        dp.ViewerStyle = "Spreadsheet";
        //        dp.AlwaysOnTop = "false";
        //        _sharedUtils.ux_tabcontrolAddTab(ux_tabcontrolDataview, dp.TabName, dp, Math.Min(2, ux_tabcontrolDataview.TabPages.IndexOf(ux_tabpageDataviewNewTab)));
        //    }
        //    // Make the first tab active...
        //    ux_tabcontrolDataview.SelectedTab = ux_tabcontrolDataview.TabPages[0];

        //    // Set the image for the New Tab tab(s)...
        //    ux_tabcontrolDataview.ImageList = navigatorTreeViewImages;
        //    ux_tabpageDataviewNewTab.ImageKey = "new_tab";

        //}

//        private LookupTables LoadStandardTables(object localDBInstance)
//        {
////LocalDatabase localData = new LocalDatabase((string)localDBInstance);
////LookupTables standardTables = new LookupTables(GRINGlobalWebServices, localData);
            
//            // Load the lookup code values tables...
//            //if (!localData.TableExists("code_value_lookup"))
//            if (!_sharedUtils.LocalDatabaseTableExists("code_value_lookup"))
//            {
//                // Looks like the code_value lookup table is not downloaded to the local machine and because it is really needed
//                // and we can't count on the user to load it manually, we will automatically load it for them...
//                Splash splash = new Splash();
//                splash.StartPosition = FormStartPosition.CenterScreen;
//                splash.Show();
//                splash.Update();
////standardTables.LoadTableFromDatabase("code_value");
//                _sharedUtils.LookupTablesLoadTableFromDatabase("code_value");
//                splash.Close();
//            }
//            // Okay we *should* be fairly certain that the code_value table has been downloaded to the local machine now 
//            // so let's load it into memory for fast lookups...
////DataTable localDBCodeValueLookupTable = localData.GetData("SELECT * FROM code_value_lookup");
//            DataTable localDBCodeValueLookupTable = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup");
//            if (localDBCodeValueLookupTable.Rows.Count > 0)
//            {
//                // Since the MRU tables are never saved to the local database, we need to add them to the memory dataset each time...
//                localDBCodeValueLookupTable.TableName = "MRU_code_value_lookup";
//                standardTables.Tables.Add(localDBCodeValueLookupTable);
//                standardTables.Tables["MRU_code_value_lookup"].AcceptChanges();
//            }
//            return standardTables;
//        }


        private void LoadStandardTables()
        {
            if (!_sharedUtils.LocalDatabaseTableExists("code_value_lookup") ||
                !_sharedUtils.LocalDatabaseTableExists("cooperator_lookup"))
            {
                // Looks like the code_value lookup table is not downloaded to the local machine and because it is really needed
                // and we can't count on the user to load it manually, we will automatically load it for them...
                Splash splash = new Splash();
                splash.StartPosition = FormStartPosition.CenterScreen;
                splash.Show();
                splash.Update();
                //standardTables.LoadTableFromDatabase("code_value");
                _sharedUtils.LookupTablesLoadTableFromDatabase("code_value_lookup");
                _sharedUtils.LookupTablesLoadTableFromDatabase("cooperator_lookup");
                splash.Close();
            }
            else
            {
                // The code_value and cooperator LU tables exist so let's update them right now...
                _sharedUtils.LookupTablesUpdateTable("code_value_lookup", false);
                _sharedUtils.LookupTablesUpdateTable("cooperator_lookup", true);
            }
//// Okay we *should* be fairly certain that the code_value table has been downloaded to the local machine now 
//// so let's load it into memory for fast lookups...
////DataTable localDBCodeValueLookupTable = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup");
//DataTable localDBCodeValueLookupTable = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup", "");
//if (localDBCodeValueLookupTable.Rows.Count > 0)
//{
//    // Since the MRU tables are never saved to the local database, we need to add them to the memory dataset each time...
//    _sharedUtils.LookupTablesCacheMRUTable(localDBCodeValueLookupTable);
//}
        }

        private void LookupTableStatusCheck()
        {
//// Display the splash page to let the user know that things are happening...
//Splash splash = new Splash();
//splash.StartPosition = FormStartPosition.CenterScreen;
//splash.Show();
//splash.Update();
            try
            {
                int partiallyLoadedTables = 0;
                DataTable lookupTableStatus = _sharedUtils.LookupTablesGetSynchronizationStats();
                if (lookupTableStatus != null &&
                    lookupTableStatus.Rows.Count > 0)
                {
                    foreach (DataRow dr in lookupTableStatus.Rows)
                    {
                        if (dr["status"].ToString().Trim().ToUpper() != "COMPLETED" &&
                            dr["status"].ToString().Trim().ToUpper() != "UPDATED")
                        {
                            // The program will not auto-update a partially loaded LUT because there could potentially 
                            // be 10s of thousands of missing records - so just bail out and warn the user...
                            if (ux_checkboxWarnWhenLUTablesAreOutdated.Checked) partiallyLoadedTables++;
                        }
                        else
                        {
//// If the attempted update fails for any reason - warn the user...
//if (!_sharedUtils.LookupTablesUpdateTable(dr["dataview_name"].ToString()))
//{
//    if (ux_checkboxWarnWhenLUTablesAreOutdated.Checked) partiallyLoadedTables++;
//}
                            // Thread the LU table update as a background task...
                            _sharedUtils.LookupTablesUpdateTable(dr["dataview_name"].ToString(), true);
                        }
                    }
                }
                if (partiallyLoadedTables > 0)
                {
//if (DialogResult.Yes == MessageBox.Show(this, "There are " + partiallyLoadedTables.ToString() + " lookup tables with missing data.\nTo maximize performace of this application it is recommended that all lookup tables be downloaded.\n\nWould you like to do this now?", "Missing Lookup Table Data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There are {0} lookup tables with missing data.\nTo maximize performace of this application it is recommended that all lookup tables be downloaded.\n\nWould you like to do this now?", "Missing Lookup Table Data", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "LookupTableStatusCheckMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, partiallyLoadedTables.ToString());
if (DialogResult.Yes == ggMessageBox.ShowDialog(this))
                    {
                        LookupTableLoader ltl = new LookupTableLoader(localDBInstance, _sharedUtils);
ltl.StartPosition = FormStartPosition.CenterParent;
                        ltl.Show();
                        ltl.Focus();
                    }
                }
            }
            catch
            {
//MessageBox.Show("An error was encountered while performing the status check for lookup tables.  Stopping status check.", "Lookup Table Status Check Error");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("An error was encountered while performing the status check for lookup tables.  Stopping status check.", "Lookup Table Status Check Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "LookupTableStatusCheckMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog(this);
            }


//// Close the splash page...
//splash.Close();
        }

        void ux_DGVCellReport_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;

            DataTable dt = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Clone();

            // NOTE: because of the way the DGV adds rows to the selectedRows collection
            //       we have to process the rows in the opposite direction they were selected in...
            int rowStart = 0;
            int rowStop = ux_datagridviewMain.SelectedRows.Count;
            int stepValue = 1;
            if (ux_datagridviewMain.SelectedRows.Count > 1 && ux_datagridviewMain.SelectedRows[0].Index > ux_datagridviewMain.SelectedRows[1].Index)
            {
                rowStart = ux_datagridviewMain.SelectedRows.Count - 1;
                rowStop = -1;
                stepValue = -1;
            }

            DataGridViewRow dgvrow = null;
            // Process the rows in the opposite direction they were selected by the user...
            for (int i = rowStart; i != rowStop; i += stepValue)
            {
                dgvrow = ux_datagridviewMain.SelectedRows[i];
                if (!dgvrow.IsNewRow)
                {
                    dt.Rows.Add(((DataRowView)dgvrow.DataBoundItem).Row.ItemArray);
                }
            }

//            ReportForm crustyReport = new ReportForm(dt, @"C:\VisualStudio2008_SVN\MyPlayground\MyPlayground\FieldLabel1.rpt");
            string fullPathName = tsmi.Tag.ToString();
            if (System.IO.File.Exists(fullPathName))
            {
                ReportForm crustyReport = new ReportForm(dt, fullPathName);
crustyReport.StartPosition = FormStartPosition.CenterParent;
_sharedUtils.UpdateControls(crustyReport.Controls, crustyReport.Name);
                crustyReport.ShowDialog();
            }
            RefreshMainDGVFormatting();
        }

        private void RefreshMainDGVData()
        {
            string currentFullPath = "";
            string currentTabText = "";
            string dataGridViewTableName = "";
            string paramsIn = "";
            List<KeyValuePair<string, int>> itemList = new List<KeyValuePair<string, int>>();
            bool rebuildDGV = false;

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            
            if (ux_tabcontrolCTDataviews.SelectedTab == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag.GetType() != typeof(DataviewProperties) ||
                string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName))
            {
                return;
            }
            else
            {
                if (ux_tabcontrolCTDataviews.SelectedTab.Tag.GetType() == typeof(DataviewProperties))
                {
                    dataGridViewTableName = ((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName;
                }
            }

            // Get the currently selected node from the treeview embedded in the tabcontrol...
            // NOTE: if these tab pages start holding more than just the treeview this will need to inspect by name instead of using the index=0 to get to the treeview control
            if (ux_comboboxCNO.SelectedValue != null &&
                _ux_NavigatorTabControl.TabPages.Count > 1 &&
                _ux_NavigatorTabControl.SelectedIndex > -1 &&
                _ux_NavigatorTabControl.SelectedTab.Name != "ux_tabpageGroupListNavigatorNewTab")
            {
                TabPage tp = _ux_NavigatorTabControl.SelectedTab;
                TreeView tv = null;
                TreeNode tn = null;

                // Get the current dataview's parameter list...
                string dvParamList = "";
                DataSet dsParms = _sharedUtils.GetWebServiceData("get_dataview_parameters", ":dataview=" + dataGridViewTableName, 0, 0);
                if (dsParms != null && dsParms.Tables.Contains("get_dataview_parameters"))
                {
                    foreach (DataRow dr in dsParms.Tables["get_dataview_parameters"].Rows)
                    {
                        dvParamList += dr["param_name"].ToString().Trim().ToLower() + "; ";
                    }
                }

                // Get the current dataview's PKey...
                string dvPKeyName = "";
                if (ux_tabcontrolCTDataviews.SelectedTab.Tag != null)
                {
                    string dataviewName = ((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName;
                    // We need to know the pkey for the currently selected dataview tab is so go get zero rows for that dataview 
                    // and inspect the result to learn the pkey name...
                    DataSet dsTemp = GetDGVData(dataviewName, new List<KeyValuePair<string, int>>(), dvParamList, Convert.ToInt32(ux_numericupdownQueryPageSize.Value));
                    if (dsTemp != null &&
                        dsTemp.Tables.Contains(dataviewName) &&
                        dsTemp.Tables[dataviewName].PrimaryKey.Length == 1)
                    {
                        dvPKeyName = dsTemp.Tables[dataviewName].PrimaryKey[0].ColumnName.Trim().ToLower();
                    }
                }

                // Get the current treeview's selected node...
                if (tp != null) tv = (TreeView)tp.Controls[tp.Name + "TreeView"];
                if (tv != null) tn = tv.SelectedNode;

                if (tn != null)
                {
                    // Remember the current tab and node to the global variables (to decide if data needs to be retrieved)...
                    currentFullPath = tn.FullPath;
                    currentTabText = _ux_NavigatorTabControl.SelectedTab.Text;

                    // If the node has changed go get the data - but if we have already retrieved the data for this node don't do it again...
                    if (lastTabName.Trim() != currentTabText || lastFullPath.Trim() != currentFullPath.Trim())
                    {
                        // Build the pkey list that will be passed on to GetDGVData()...
                        itemList.AddRange(BuildPKeyList(tn, dvParamList, dvPKeyName, ux_checkboxIncludeSubFolders.Checked));

                        // Changed the list of IVIDs, ACIDs, and ORNOs so clear all tables in the dataset...
                        grinData.Tables.Clear();
                        // Go get the records for the new list and currently active table view (using the new pkey list)...
                        if (itemList.Count > 0 ||
                            tn.Tag.ToString().Trim().StartsWith("QUERY"))
                        {
                            grinData = GetDGVData(dataGridViewTableName, itemList, dvParamList, Convert.ToInt32(ux_numericupdownQueryPageSize.Value));
                        }

                        // There is new data so rebuild the DGV...
                        rebuildDGV = true;

                        lastFullPath = currentFullPath.Trim();
                        lastTabName = currentTabText.Trim();
                    }
                }

                // If the data for the currently selected dataview is not loaded - get it now...
                if (!grinData.Tables.Contains(dataGridViewTableName))
                {
                    DataSet dsTemp = new DataSet();

                    // Build the pkey list that will be passed on to GetDGVData()...
                    itemList.AddRange(BuildPKeyList(tn, dvParamList, dvPKeyName, ux_checkboxIncludeSubFolders.Checked));

                    // Have not looked at this view for the currently selected list of IVIDs, ACIDs, and ORNOs so go get it...
                    dsTemp = GetDGVData(dataGridViewTableName, itemList, dvParamList, Convert.ToInt32(ux_numericupdownQueryPageSize.Value));
                    if (dsTemp.Tables.Contains(dataGridViewTableName))
                    {
                        grinData.Tables.Add(dsTemp.Tables[dataGridViewTableName].Copy());
                    }
                    else
                    {
                        grinData.Tables.Add(new DataTable(dataGridViewTableName));
                    }

                    // There is new data so rebuild the DGV...
                    rebuildDGV = true;
                }
                else
                {
                    // If the user has clicked on a dataview that already has data...
                    if (ux_datagridviewMain != null &&
                        ux_datagridviewMain.DataSource != null)
                    {
                        //  The current dataview not the one that is being shown (but it should be so rebuild the DGV)...
                        if ((ux_datagridviewMain.DataSource.GetType() == typeof(BindingSource) &&
                            ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).TableName != dataGridViewTableName) ||
                            ((ux_datagridviewMain.DataSource.GetType() == typeof(DataTable) &&
                            ((DataTable)ux_datagridviewMain.DataSource).TableName != dataGridViewTableName)))
                        {
                            // Rebuild the DGV to show the data for the dataview tab the user just clicked on...
                            rebuildDGV = true;
                        }
                    }
                }

                // Do we need to rebuild the DGV???
                if (rebuildDGV)
                {
                    // Create a new datagridview for the spreadsheet view...
                    _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewMain, grinData.Tables[dataGridViewTableName]);

                    // Force a gargabe collection...
                    GC.Collect();
                }

                // Restore the sort order...
                try
                {
                    //grinData.Tables[dataGridViewTableName].DefaultView.Sort = userSettings[dataGridViewTableName, "DefaultView.Sort"];
                    grinData.Tables[dataGridViewTableName].DefaultView.Sort = _sharedUtils.GetUserSetting(dataGridViewTableName, "DefaultView.Sort", "");
                }
                catch (Exception)
                {
                    grinData.Tables[dataGridViewTableName].DefaultView.Sort = "";
                }
            }
            else
            {
                // There is no treeview node selected so create an empty dataview table...
                grinData = GetDGVData(dataGridViewTableName, itemList, "", Convert.ToInt32(ux_numericupdownQueryPageSize.Value));
                // Create a new datagridview for the spreadsheet view...
                _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewMain, grinData.Tables[dataGridViewTableName]);
            }


            // Bind the context menu to the column header and set the DGV header text...
            foreach (DataGridViewColumn dgvc in ux_datagridviewMain.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.Programmatic;
                dgvc.HeaderCell.ContextMenuStrip = ux_contextmenustripDGVHeader;
                dgvc.ContextMenuStrip = ux_contextmenustripReadOnlyDGVCell;
                dgvc.HeaderText = _sharedUtils.GetFriendlyFieldName(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Columns[dgvc.Index], ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Columns[dgvc.Index].ColumnName);
            }

            // Reset the checkedlistbox of column names (and then re-add them as unchecked)...
            ux_checkedlistboxViewedColumns.Items.Clear();
            foreach (DataColumn dc in ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Columns)
            {
                ux_checkedlistboxViewedColumns.Items.Add(_sharedUtils.GetFriendlyFieldName(dc, dc.ColumnName), false);
            }

            // Get the user's settings for visible columns...
//string[] columnsVisible = userSettings[dataGridViewTableName, "Columns.Visible"].Split(' ');
            string[] columnsVisible = _sharedUtils.GetUserSetting(dataGridViewTableName, "Columns.Visible", "").Split(' ');

            // Clear the visible columns...
            ux_checkboxSelectAll.CheckState = CheckState.Unchecked;

            // Now go back through the checkedlistbox and make columns visible based on user settings...
            if (columnsVisible.Length == 1 && columnsVisible[0] == "")
            {
                // Check all checkboxes in the checkbox list...
                ux_checkboxSelectAll.CheckState = CheckState.Checked;
            }
            else
            {
                int columnNum = -1;
                foreach (string column in columnsVisible)
                {
                    if (int.TryParse(column, out columnNum) &&
                        columnNum < ux_checkedlistboxViewedColumns.Items.Count) ux_checkedlistboxViewedColumns.SetItemChecked(columnNum, true);
                }
            }

// Update the statusbar...
if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
    ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), grinData.Tables[dataGridViewTableName].DefaultView.Count.ToString(), grinData.Tables[dataGridViewTableName].Rows.Count.ToString());
}
else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
{
    ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), grinData.Tables[dataGridViewTableName].Rows.Count.ToString());
}

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void RefreshMainDGVFormatting()
        {
            // If the datagridview has no data source or the selected tab's tag has no dataview string - bail out now...
            if (ux_datagridviewMain.DataSource == null ||
                ux_tabcontrolCTDataviews.SelectedTab == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag.GetType() != typeof(DataviewProperties) ||
                string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName)) return;

            // Highlight the changed grid cells for each datagridview row if the checkbox indicates and the DGV is in edit mode...
            bool useHighlighting = ux_checkboxHighlightChanges.Checked && ux_buttonSaveData.Enabled;
            foreach (DataGridViewRow dgvr in ux_datagridviewMain.Rows)
            {
                RefreshDGVRowFormatting(dgvr, useHighlighting);
            }

            // Make columns visible based on user settings (only in read-only mode)...
            if (ux_buttonEditData.Enabled)
            {
//string[] columnsVisible = userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "Columns.Visible"].Split(' ');
                string[] columnsVisible = _sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "Columns.Visible", "").Split(' ');
                if (columnsVisible.Length == 1 && columnsVisible[0] == "")
                {
                    // Check all checkboxes in the checkbox list...
                    ux_checkboxSelectAll.CheckState = CheckState.Checked;
                }
                else
                {
                    foreach (DataGridViewColumn dgvc in ux_datagridviewMain.Columns)
                    {
                        if (columnsVisible.Contains(dgvc.Index.ToString()))
                        {
                            dgvc.Visible = true;
                        }
                        else
                        {
                            dgvc.Visible = false;
                        }
                    }
                }
            }

            // Show SortGlyphs for the column headers (this takes two steps)...
            // First reset them all to No Sort...
            foreach (DataGridViewColumn dgvc in ux_datagridviewMain.Columns)
            {
                dgvc.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            // Now inspect the sort string from the datatable in use to set the SortGlyphs...
            string strOrder = "";
            if (ux_datagridviewMain.DataSource.GetType() == typeof(BindingSource))
            {
                strOrder = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.Sort;
            }
            else
            {
                strOrder = ((DataTable)ux_datagridviewMain.DataSource).DefaultView.Sort;
            }
            char[] chararrDelimiters = { ',' };
            string[] strarrSortCols = strOrder.Split(chararrDelimiters);
            foreach (string strSortCol in strarrSortCols)
            {
                if (strSortCol.Contains("ASC"))
                {
                    if (ux_datagridviewMain.Columns.Contains(strSortCol.Replace(" ASC", ""))) ux_datagridviewMain.Columns[strSortCol.Replace(" ASC", "")].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                if (strSortCol.Contains("DESC"))
                {
                    if (ux_datagridviewMain.Columns.Contains(strSortCol.Replace(" DESC", ""))) ux_datagridviewMain.Columns[strSortCol.Replace(" DESC", "")].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }

            // Now restore the column width and ordering...
//string[] columnWidths = userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "Columns.Width"].Split(' ');
//string[] columnOrder = userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "Columns.DisplayOrder"].Split(' ');
            string[] columnWidths = _sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "Columns.Width", "").Split(' ');
            string[] columnOrder = _sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "Columns.DisplayOrder", "").Split(' ');
            int columnNum = -1;
            if (columnWidths.Length == columnOrder.Length && columnWidths.Length == ux_datagridviewMain.Columns.Count)
            {
                for(int i = 0; i < ux_datagridviewMain.Columns.Count; i++)
                {
                    if (int.TryParse(columnWidths[i], out columnNum)) ux_datagridviewMain.Columns[i].Width = columnNum;// Convert.ToInt32(columnWidths[i]);
                    if (int.TryParse(columnOrder[i], out columnNum)) ux_datagridviewMain.Columns[i].DisplayIndex = columnNum;// Convert.ToInt32(columnOrder[i]);
                }
            }

            // Now restore the default cell color and alternating rows default cell color...
            int backgroundColor;
            ux_datagridviewMain.DefaultCellStyle.BackColor = Color.White;
            ux_datagridviewMain.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
//if (int.TryParse(userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "DefaultCellStyle.BackColor"], out backgroundColor))
            if (int.TryParse(_sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "DefaultCellStyle.BackColor", ""), out backgroundColor))
            {
                ux_datagridviewMain.DefaultCellStyle.BackColor = Color.FromArgb(backgroundColor);
            }
//if (int.TryParse(userSettings[((DataviewProperties)ux_tabcontrolDataview.SelectedTab.Tag).DataviewName, "AlternatingRowsDefaultCellStyle.BackColor"], out backgroundColor))
            if (int.TryParse(_sharedUtils.GetUserSetting(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName, "AlternatingRowsDefaultCellStyle.BackColor", ""), out backgroundColor))
            {
                ux_datagridviewMain.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(backgroundColor);
            }

// Update the row count in the status bar...
if (ux_statusLeftMessage.Tag != null)
{
    if (ux_statusLeftMessage.Tag.ToString().Contains("{0}") &&
        ux_statusLeftMessage.Tag.ToString().Contains("{1}"))
    {
        ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewMain.Rows.Count.ToString(), grinData.Tables[((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName].Rows.Count.ToString());
    }
    else if (ux_statusLeftMessage.Tag.ToString().Contains("{0}"))
    {
        ux_statusLeftMessage.Text = string.Format(ux_statusLeftMessage.Tag.ToString(), ux_datagridviewMain.Rows.Count.ToString());
    }
}
// Indicate to the user which URL is the active GG server...
if (ux_statusCenterMessage.Tag != null)
{
    if (ux_statusCenterMessage.Tag.ToString().Contains("{0}"))
    {
        ux_statusCenterMessage.Text = string.Format(ux_statusCenterMessage.Tag.ToString(), _sharedUtils.Url);
    }
    else if (!ux_statusCenterMessage.Tag.ToString().Contains(_sharedUtils.Url))
    {
        ux_statusCenterMessage.Text = ux_statusCenterMessage.Tag.ToString() + " (" + _sharedUtils.Url + ")";
    }
}
else
{
    if (!ux_statusCenterMessage.Text.Contains(_sharedUtils.Url))
    {
        ux_statusCenterMessage.Text = ux_statusCenterMessage.Text + " (" + _sharedUtils.Url + ")";
    }
}
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

            try
            {
                if (dgvr.Index > ((BindingSource)dgvr.DataGridView.DataSource).Count - 1) return;
            }
            catch
            {
                return;
            }

            if (!dgvr.IsNewRow)
            {
                dr = ((DataRowView)dgvr.DataBoundItem).Row;
                if (useHighlighting)
                {
                    // If this is a row that has just been added (or is a "NewRow" that is currently being edited)...
                    if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Detached)
                    {
                        foreach (DataGridViewCell dgvc in dgvr.Cells)
                        {
                            dgvc.Style.BackColor = Color.SteelBlue;
                            // If there is data in the datarow for this cell make it yellow...
                            if (dr.Table.Columns[dgvc.ColumnIndex].ExtendedProperties.Contains("is_readonly") &&
                                dr.Table.Columns[dgvc.ColumnIndex].ExtendedProperties["is_readonly"].ToString() == "Y")
                            {
                                dgvc.Style.BackColor = Color.LightGray;
                            }
                            // If there is data in the datarow for this cell make it yellow...
                            else if (dr[dgvc.ColumnIndex] != DBNull.Value)
                            {
                                if (dr.Table.Columns[dgvc.ColumnIndex].ExtendedProperties.Contains("default_value") &&
                                    dr.Table.Columns[dgvc.ColumnIndex].ExtendedProperties["default_value"].ToString() == dr[dgvc.ColumnIndex].ToString() &&
                                    dr.Table.Columns[dgvc.ColumnIndex].ColumnName.StartsWith("is_"))
                                {
                                    dgvc.Style.BackColor = Color.SteelBlue;
                                }
                                else
                                {
                                    //dgvc.Style.BackColor = Color.Bisque;
                                    dgvc.Style.BackColor = Color.PowderBlue;
                                }
                            }
                            // If the cell is in a required field make it violet (but not a bool field because it has a default value of 'N' already populated)...
                            else if (dr.Table.Columns[dgvc.ColumnIndex].ExtendedProperties.Contains("is_nullable") &&
                                dr.Table.Columns[dgvc.ColumnIndex].ExtendedProperties["is_nullable"].ToString() == "N" &&
                                !dr.Table.Columns[dgvc.ColumnIndex].ColumnName.StartsWith("is_"))
                            {
                                dgvc.Style.BackColor = Color.Plum;
                            }
                        }
                    }
                    // If the row has changes make each changed cell yellow...
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
            }
            else
            {
                if (dgvr.Cells != null && dgvr.DataGridView.SelectedCells != null && dgvr.DataGridView.SelectedCells.Count > 0 && dgvr.Cells.Contains(dgvr.DataGridView.SelectedCells[0]))
                {
                    foreach (DataGridViewCell dgvc in dgvr.Cells)
                    {
                        if (((DataTable)((BindingSource)dgvr.DataGridView.DataSource).DataSource).Columns[dgvc.ColumnIndex].ExtendedProperties.Contains("is_nullable") &&
                            ((DataTable)((BindingSource)dgvr.DataGridView.DataSource).DataSource).Columns[dgvc.ColumnIndex].ExtendedProperties["is_nullable"].ToString() == "N" &&
                            !((DataTable)((BindingSource)dgvr.DataGridView.DataSource).DataSource).Columns[dgvc.ColumnIndex].ColumnName.StartsWith("is_"))
                        {
                            dgvc.Style.BackColor = Color.Plum;
                        }
                        else
                        {
                            dgvc.Style.BackColor = Color.SandyBrown;
                        }
                    }
                }
            }
        }

        private void RefreshForm()
        {
            // If there is a form still attached to the global variable dataviewForm - dispose of it properly...
            if (dataviewForm != null)
            {
                dataviewForm.Close();
                dataviewForm.Dispose();
                dataviewForm = null;
            }

            // Display Form if user wants one...
            if (ux_tabcontrolCTDataviews.SelectedTab != null &&
                ux_tabcontrolCTDataviews.SelectedTab.Tag != null &&
                ux_tabcontrolCTDataviews.SelectedTab.Tag.GetType() == typeof(DataviewProperties) &&
                !string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).ViewerStyle))
            {
                DataviewProperties dvp = (DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag;
                System.Reflection.ConstructorInfo constInfo = null;
                // First attempt to find a compatible form for this dataview...
                if (!string.IsNullOrEmpty(dvp.StrongFormName) &&
                    localFormsAssemblies != null &&
                    localFormsAssemblies.Length > 0)
                {
                    foreach (FormsData fd in localFormsAssemblies)
                    {
                        if (!string.IsNullOrEmpty(fd.StrongFormName) &&
                            !string.IsNullOrEmpty(dvp.StrongFormName) &&
                            fd.StrongFormName.Trim().ToUpper() == dvp.StrongFormName.Trim().ToUpper()) constInfo = fd.ConstInfo;
                    }
                }
                // Now show the compatible form as the user wants it...
                switch (((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).ViewerStyle.Trim().ToUpper())
                {
                    case "SPREADSHEET":
                        break;
                    case "FORM":
                        if (constInfo != null &&
                            ux_bindingnavigatorMain.BindingSource != null)
                        {
//dataviewForm = (Form)constInfo.Invoke(new object[] { ux_bindingnavigatorMain.BindingSource, !ux_buttonEditData.Enabled, lookupTables, !ux_buttonEditData.Enabled });
                            dataviewForm = (Form)constInfo.Invoke(new object[] { ux_bindingnavigatorMain.BindingSource, !ux_buttonEditData.Enabled, _sharedUtils, !ux_buttonEditData.Enabled });
                            dataviewForm.Owner = this;
                            dataviewForm.TopLevel = false;
                            dataviewForm.FormBorderStyle = FormBorderStyle.None;
                            dataviewForm.Anchor = ux_datagridviewMain.Anchor;
                            Point upperLeftCorner = ux_datagridviewMain.Location;
                            upperLeftCorner.Offset(0, 1);
                            dataviewForm.Location = upperLeftCorner;
                            dataviewForm.Width = ux_datagridviewMain.Width;
                            dataviewForm.Height = ux_groupboxDataEditing.Location.Y - dataviewForm.Location.Y;
                            ux_splitcontainerViewer.Panel1.Controls.Add(dataviewForm);
                            dataviewForm.BringToFront();
                            dataviewForm.Show();
                            // Make sure the tab controls are 'on top' of this new form...
                            ux_tabcontrolDataviewOptions.BringToFront();
                        }
                        else
                        {
                            dvp.StrongFormName = "";
                            ux_tabcontrolCTDataviews.SelectedTab.Tag = dvp;
                        }
                        break;
                    case "BOTH":
                        if (constInfo != null &&
                            ux_bindingnavigatorMain.BindingSource != null)
                        {
//dataviewForm = (Form)constInfo.Invoke(new object[] { ux_bindingnavigatorMain.BindingSource, !ux_buttonEditData.Enabled, lookupTables, !ux_buttonEditData.Enabled });
                            dataviewForm = (Form)constInfo.Invoke(new object[] { ux_bindingnavigatorMain.BindingSource, !ux_buttonEditData.Enabled, _sharedUtils, !ux_buttonEditData.Enabled });
                            if (dvp.AlwaysOnTop.Trim().ToLower() == "true") dataviewForm.Owner = this;
                            dataviewForm.Show();
                        }
                        else
                        {
                            dvp.StrongFormName = "";
                            ux_tabcontrolCTDataviews.SelectedTab.Tag = dvp;
                        }
                        break;
                    default:
                        break;
                }

if (dataviewForm != null)
{
    _sharedUtils.UpdateControls(dataviewForm.Controls, this.Name);
}
            }
        }

        private void GrinGlobalClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            // The user might be closing the form during the middle of edit changes in the datagridview - if so ask the
            // user if they would like to save their data (via the normal Cancel Edit button event)...
            if (ux_datagridviewMain.EditMode != DataGridViewEditMode.EditProgrammatically) ux_buttonCancelEditData.PerformClick();

            // Now check to see if the user has chosen to remain in edit mode (because they clicked the 'No' button on the 
            // Cancel Edit dialog box - if so cancel the FormClose event so that the user can continue editing data...
            if (ux_datagridviewMain.EditMode != DataGridViewEditMode.EditProgrammatically) e.Cancel = true;

            // Update all TabControl settings on the userSettings dataset if a valid cno was obtained during login...
            if (_usernameCooperatorID.Length > 0)
            {
                // Change cursor to the wait cursor...
                Cursor origCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                // Save all user gui settings...
                SetAllUserSettings();
                _sharedUtils.SaveAllUserSettings();
                // Save user treeview lists...
                SaveNavigatorTabControlData(_ux_NavigatorTabControl, int.Parse(_usernameCooperatorID));
                // Save all LUT dictionary caches...
                _sharedUtils.LookupTablesSaveALLCaches();

                // Restore cursor to default cursor...
                Cursor.Current = origCursor;
            }
        }

        private void SetAllUserSettings()
        {
            if (ux_buttonEditData.Enabled)
            {
                SetGeneralUserSettings();
                SetDGVMainDataviewUserSettings();
                SetGroupListNavigatorUserSettings();
                SetTabControlDataviewUserSettings();
            }
        }

        private void SetGeneralUserSettings()
        {
            _sharedUtils.SaveUserSetting(ux_checkboxWarnWhenLUTablesAreOutdated.Name, "Checked", ux_checkboxWarnWhenLUTablesAreOutdated.Checked.ToString());
            _sharedUtils.SaveUserSetting(ux_numericupdownMaxRowsReturned.Name, "Value", ux_numericupdownMaxRowsReturned.Value.ToString());
            _sharedUtils.SaveUserSetting(ux_numericupdownQueryPageSize.Name, "Value", ux_numericupdownQueryPageSize.Value.ToString());
        }

        private void SetDGVMainDataviewUserSettings()
        {
            string columnsWidth = "";
            string columnsVisible = "";
            string columnsDisplayOrder = "";

            // If the selected tab's tag has no dataview string - bail out now...
            if (ux_tabcontrolCTDataviews.SelectedTab == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag == null ||
                ux_tabcontrolCTDataviews.SelectedTab.Tag.GetType() != typeof(DataviewProperties) ||
                string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName)) return;

            foreach (DataGridViewColumn dgvc in ux_datagridviewMain.Columns)
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
            string dataviewName = ((DataviewProperties)ux_tabcontrolCTDataviews.SelectedTab.Tag).DataviewName;
//userSettings[dataviewName, "Columns.Width"] = columnsWidth.Trim();
//userSettings[dataviewName, "Columns.Visible"] = columnsVisible.Trim();
//userSettings[dataviewName, "Columns.DisplayOrder"] = columnsDisplayOrder.Trim();
//userSettings[dataviewName, "DefaultCellStyle.BackColor"] = ux_datagridviewMain.DefaultCellStyle.BackColor.ToArgb().ToString();
//userSettings[dataviewName, "AlternatingRowsDefaultCellStyle.BackColor"] = ux_datagridviewMain.AlternatingRowsDefaultCellStyle.BackColor.ToArgb().ToString();
            _sharedUtils.SaveUserSetting(dataviewName, "Columns.Width", columnsWidth.Trim());
            _sharedUtils.SaveUserSetting(dataviewName, "Columns.Visible", columnsVisible.Trim());
            _sharedUtils.SaveUserSetting(dataviewName, "Columns.DisplayOrder", columnsDisplayOrder.Trim());
            _sharedUtils.SaveUserSetting(dataviewName, "DefaultCellStyle.BackColor", ux_datagridviewMain.DefaultCellStyle.BackColor.ToArgb().ToString());
            _sharedUtils.SaveUserSetting(dataviewName, "AlternatingRowsDefaultCellStyle.BackColor", ux_datagridviewMain.AlternatingRowsDefaultCellStyle.BackColor.ToArgb().ToString());
            if (ux_datagridviewMain.DataSource != null)
            {
                string dgvSort = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).DefaultView.Sort;
                if (!dgvSort.Contains("TempSortColumn"))
                {
//userSettings[dataviewName, "DefaultView.Sort"] = dgvSort;
                    _sharedUtils.SaveUserSetting(dataviewName, "DefaultView.Sort", dgvSort);
                }
            }
        }

        private void SetGroupListNavigatorUserSettings()
        {
            string tabOrder = "";
            TreeView tv;

//userSettings[ux_tabcontrolGroupListNavigator.Name, "TabPages.Count"] = ux_tabcontrolGroupListNavigator.TabPages.Count.ToString();
            _sharedUtils.SaveUserSetting(_ux_NavigatorTabControl.Name, "TabPages.Count", _ux_NavigatorTabControl.TabPages.Count.ToString());
            for (int i = 0; i < _ux_NavigatorTabControl.TabPages.Count; i++)
            {
                if (_ux_NavigatorTabControl.TabPages[i].Name != "ux_tabpageGroupListNavigatorNewTab")
                {
                    tabOrder += _ux_NavigatorTabControl.TabPages[i].Text + _pathSeparator;
//Control[] thisTabPagesTreeView = ux_tabcontrolGroupListNavigator.Controls.Find(ux_tabcontrolGroupListNavigator.TabPages[i].Name + "TreeView", true);
                    Control[] tabPages = _ux_NavigatorTabControl.Controls.Find(_ux_NavigatorTabControl.TabPages[i].Name, true);
//if (thisTabPagesTreeView.Length > 0)
                    foreach (Control tabPage in tabPages)
                    {
                        if (tabPage.GetType() == typeof(TabPage))
                        {
                            foreach (Control ctrl in tabPage.Controls)
                            {
                                if (ctrl.GetType() == typeof(TreeView))
                                {
//tv = (TreeView)thisTabPagesControls[0];
                                    tv = (TreeView)ctrl;
////if (ux_tabcontrolGroupListNavigator.SelectedIndex == i) userSettings[ux_tabcontrolGroupListNavigator.Name, "SelectedIndex"] = ux_tabcontrolGroupListNavigator.SelectedIndex.ToString();
                                    if (_ux_NavigatorTabControl.SelectedIndex == i) _sharedUtils.SaveUserSetting(_ux_NavigatorTabControl.Name, "SelectedIndex", _ux_NavigatorTabControl.SelectedIndex.ToString());
////if (tv != null && tv.SelectedNode != null) userSettings[tv.Name, "SelectedNode.FullPath"] = tv.SelectedNode.FullPath;
                                    if (tv != null && tv.SelectedNode != null) _sharedUtils.SaveUserSetting(tv.Name, "SelectedNode.FullPath", tv.SelectedNode.FullPath);
                                }
                            }
                        }
                    }
                }
            }

//if (ux_comboboxCNO.SelectedValue != null &&
//    ux_comboboxCNO.SelectedValue.ToString().ToLower().Trim() == _usernameCooperatorID.ToLower().Trim())
            if (_currentCooperatorID.ToLower().Trim() == _usernameCooperatorID.ToLower().Trim())
            {
////userSettings[ux_tabcontrolGroupListNavigator.Name, "TabPages.Order"] = tabOrder.Trim();
//_sharedUtils.SaveUserSetting(ux_tabcontrolGroupListNavigator.Name, "TabPages.Order", tabOrder.Trim());
                _sharedUtils.SaveUserSetting(_ux_NavigatorTabControl.Name, "TabPages.Order", tabOrder.TrimEnd(_pathSeparator.ToCharArray()));
            }
        }

        private void SetTabControlDataviewUserSettings()
        {
            int tabPageCount = ux_tabcontrolCTDataviews.TabPages.Count;
            if (ux_tabcontrolCTDataviews.TabPages.ContainsKey("ux_tabpageDataviewNewTab")) tabPageCount -= 1;
            // Check to see if the number of dataview tabs is less than what the user started with and if so delete the extra tabs...
            int oldTabPageCount = -1;
//int.TryParse(_sharedUtils.GetUserSetting("ux_tabcontrolDataview", "TabPages.Count", "-1"), out oldTabPageCount);
            int.TryParse(_sharedUtils.GetUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages.Count", "-1"), out oldTabPageCount);
            if (oldTabPageCount > tabPageCount)
            {
                for (int i = tabPageCount; i < oldTabPageCount; i++)
                {
//_sharedUtils.DeleteUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].Text"); // Legacy...
//_sharedUtils.DeleteUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].Tag"); // Legacy...
//_sharedUtils.DeleteUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].TabName");
//_sharedUtils.DeleteUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].DataviewName");
//_sharedUtils.DeleteUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].FormName");
//_sharedUtils.DeleteUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].ViewerStyle");
//_sharedUtils.DeleteUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].AlwaysOnTop");
                    _sharedUtils.DeleteUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].Text"); // Legacy...
                    _sharedUtils.DeleteUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].Tag"); // Legacy...
                    _sharedUtils.DeleteUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].TabName");
                    _sharedUtils.DeleteUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].DataviewName");
                    _sharedUtils.DeleteUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].FormName");
                    _sharedUtils.DeleteUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].ViewerStyle");
                    _sharedUtils.DeleteUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].AlwaysOnTop");
                }
            }

//_sharedUtils.SaveUserSetting("ux_tabcontrolDataview", "TabPages.Count", tabPageCount.ToString());
            _sharedUtils.SaveUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages.Count", tabPageCount.ToString());
            for (int i = 0; i < tabPageCount; i++)
            {
                if (ux_tabcontrolCTDataviews.TabPages[i].Tag != null &&
                    ux_tabcontrolCTDataviews.TabPages[i].Tag.GetType() == typeof(DataviewProperties) &&
                    !string.IsNullOrEmpty(((DataviewProperties)ux_tabcontrolCTDataviews.TabPages[i].Tag).DataviewName))
                {
//_sharedUtils.SaveUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].TabName", ((DataviewProperties)ux_tabcontrolDataview.TabPages[i].Tag).TabName + "");
//_sharedUtils.SaveUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].DataviewName", ((DataviewProperties)ux_tabcontrolDataview.TabPages[i].Tag).DataviewName + "");
//_sharedUtils.SaveUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].FormName", ((DataviewProperties)ux_tabcontrolDataview.TabPages[i].Tag).StrongFormName + "");
//_sharedUtils.SaveUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].ViewerStyle", ((DataviewProperties)ux_tabcontrolDataview.TabPages[i].Tag).ViewerStyle + "");
//_sharedUtils.SaveUserSetting("ux_tabcontrolDataview", "TabPages[" + i.ToString() + "].AlwaysOnTop", ((DataviewProperties)ux_tabcontrolDataview.TabPages[i].Tag).AlwaysOnTop + "");
                    _sharedUtils.SaveUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].TabName", ((DataviewProperties)ux_tabcontrolCTDataviews.TabPages[i].Tag).TabName + "");
                    _sharedUtils.SaveUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].DataviewName", ((DataviewProperties)ux_tabcontrolCTDataviews.TabPages[i].Tag).DataviewName + "");
                    _sharedUtils.SaveUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].FormName", ((DataviewProperties)ux_tabcontrolCTDataviews.TabPages[i].Tag).StrongFormName + "");
                    _sharedUtils.SaveUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].ViewerStyle", ((DataviewProperties)ux_tabcontrolCTDataviews.TabPages[i].Tag).ViewerStyle + "");
                    _sharedUtils.SaveUserSetting(ux_tabcontrolCTDataviews.Name, "TabPages[" + i.ToString() + "].AlwaysOnTop", ((DataviewProperties)ux_tabcontrolCTDataviews.TabPages[i].Tag).AlwaysOnTop + "");
                }
            }
        }

        public DataSet SaveDGVData(DataSet dataToSave)
        {
            DataSet rollupReturn = new DataSet();
            string centerMessage = ux_statusCenterMessage.Text;
            string rightMessage = ux_statusRightMessage.Text;
            ContentAlignment centerMessageAlignment = ux_statusCenterMessage.TextAlign;

            // Set the statusbar up to show the query progress...
            ux_statusCenterMessage.TextAlign = ContentAlignment.MiddleRight;
            ux_statusCenterMessage.Text = "Saving Data:";
            ux_statusRightProgressBar.Visible = true;
            ux_statusRightProgressBar.Minimum = 0;
            ux_statusRightProgressBar.Maximum = 100;
            ux_statusRightProgressBar.Step = 1; 
            ux_statusRightProgressBar.Value = 0;
//ux_statusRightProgressBar.Visible = true;
//ux_statusRightProgressBar.Minimum = 0;
//ux_statusRightProgressBar.Maximum = 100;
//ux_statusRightProgressBar.Step = 5;
//ux_statusRightProgressBar.Value = 75;
            ux_statusstripMain.Refresh();

            try
            {
                // Pass the web service method 1000 records at a time (to keep the call smaller than 10MB)...
                DataTable modifiedTable = dataToSave.Tables[0];
                string errorMessage = "";
                DataSet pagedModifiedDataset = new DataSet();
                pagedModifiedDataset.Tables.Add(modifiedTable.Clone());
                int pageSize = (int)ux_numericupdownQueryPageSize.Value;
                pageSize = Math.Min(10000, Math.Max(1, pageSize)); // Clamp the page size between 1 and 10,000
                int pageStart = 0;
                int pageStop = Math.Min(pageSize, modifiedTable.Rows.Count);
                while (pageStart < modifiedTable.Rows.Count)
                {
                    // Clear the table page from the dataset...
                    pagedModifiedDataset.Tables[modifiedTable.TableName].Clear();
                    pagedModifiedDataset.AcceptChanges();
                    
                    // Build a new 'modified data' table page row by row...
                    for (int i = pageStart; i < pageStop; i++)
                    {
                        // Make a copy of the datarow in the paged datatable...
                        DataRow newRow = pagedModifiedDataset.Tables[modifiedTable.TableName].NewRow();
                        switch (modifiedTable.Rows[i].RowState)
                        {
                            case DataRowState.Added:
                                // Populate the new row...
                                newRow.ItemArray = modifiedTable.Rows[i].ItemArray;
                                // Add it to the modified rows table...
                                pagedModifiedDataset.Tables[modifiedTable.TableName].Rows.Add(newRow);
                                break;
                            case DataRowState.Deleted:
                                // 'Undelete the original row (to allow access to the row's field data)...
                                modifiedTable.Rows[i].RejectChanges();
                                // Populate the new row...
                                newRow.ItemArray = modifiedTable.Rows[i].ItemArray;
                                // Add it to the modified rows table...
                                pagedModifiedDataset.Tables[modifiedTable.TableName].Rows.Add(newRow);
                                // Reset the rowstate for the new row...
                                newRow.AcceptChanges();
                                // Set the rowstate to the same as the original source rowstate...
                                newRow.Delete();
                                // 'Re-delete' the original row...
                                modifiedTable.Rows[i].Delete();
                                break;
                            case DataRowState.Detached:
                                break;
                            case DataRowState.Modified:
                                // Populate the new row with the original data from the modified row...
                                foreach (DataColumn dc in modifiedTable.Columns)
                                {
                                    newRow[dc.ColumnName] = modifiedTable.Rows[i][dc.ColumnName, DataRowVersion.Original];
                                }
                                // Add it to the modified rows table...
                                pagedModifiedDataset.Tables[modifiedTable.TableName].Rows.Add(newRow);
                                // Reset the rowstate for the new row...
                                newRow.AcceptChanges();
                                // Now modify the rows column to match the current data in the modified row...
                                foreach (DataColumn dc in modifiedTable.Columns)
                                {
                                    if (!modifiedTable.Rows[i][dc.ColumnName, DataRowVersion.Original].Equals(modifiedTable.Rows[i][dc.ColumnName, DataRowVersion.Current]))
                                    {
                                        newRow[dc.ColumnName] = modifiedTable.Rows[i][dc.ColumnName, DataRowVersion.Current];
                                    }
                                }
                                //newRow.ItemArray = modifiedTable.Rows[i].ItemArray;
                                // Set the rowstate to the same as the original source rowstate...
                                //newRow.SetModified();
                                break;
                            case DataRowState.Unchanged:
                                break;
                            default:
                                break;
                        } 
                   }

                    // Call the web method to update the changed/new data...
////errors = GRINGlobalWebServices.SaveData(dataToSave);
//            errors = _sharedUtils.SaveWebServiceData(dataToSave);
                    DataSet pagedModifiedDatasetResults = _sharedUtils.SaveWebServiceData(pagedModifiedDataset);

                    // Merge the returned rows into the dataset that will be returned...
                    if (pagedModifiedDatasetResults.Tables.Contains(modifiedTable.TableName) &&
                        pagedModifiedDatasetResults.Tables[modifiedTable.TableName].Rows.Count > 0)
                    {
                        // If the 'rollup' dataset does not have a return table - create it now...
                        if (!rollupReturn.Tables.Contains(modifiedTable.TableName))
                        {
                            rollupReturn.Tables.Add(pagedModifiedDatasetResults.Tables[modifiedTable.TableName].Clone());
                        }
                        // Load the returned rows to the 'rollup' dataset's table...
                        rollupReturn.Tables[modifiedTable.TableName].Load(pagedModifiedDatasetResults.Tables[modifiedTable.TableName].CreateDataReader(), LoadOption.Upsert);
                    }

                    // Roll up the dataset error messages for each page into one message...
                    if (pagedModifiedDatasetResults != null &&
                    pagedModifiedDatasetResults.Tables.Contains("ExceptionTable") &&
                    pagedModifiedDatasetResults.Tables["ExceptionTable"].Rows.Count > 0)
                    {
                        // If the 'rollup' dataset does not have an exceptions table - create it now...
                        if (!rollupReturn.Tables.Contains("ExceptionsTable") &&
                            pagedModifiedDatasetResults.Tables.Contains("ExceptionsTable"))
                        {
                            rollupReturn.Tables.Add(pagedModifiedDatasetResults.Tables["ExceptionsTable"].Clone());
                        }
                        // Append error message to the rollup error message...
                        errorMessage += "\n" + pagedModifiedDatasetResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
                    }

                    // Update the paging indexes...
                    pageStart = pageStop;
                    pageStop = Math.Min((pageStart + pageSize), modifiedTable.Rows.Count);

                    // Update the progress bar on the statusbar...
                    ux_statusRightProgressBar.Value = pageStart * 100 / modifiedTable.Rows.Count;
                    ux_statusstripMain.Refresh();

                    // Check to see if we should bail out (at user request)...
                    Application.DoEvents();
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors saving data.\n\nFull error message:\n{0}", "Save Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "SaveDGVDataMessage1";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                    if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorMessage);
                    ggMessageBox.ShowDialog();
                }
            }
            catch
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error saving data.", "Save Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "SaveDGVDataMessage2";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                ggMessageBox.ShowDialog();
            }









            // If the commandline during application startup had a parameter for _saveDataDumpFile set to a valid filepath 
            // save the data to this file in XML format...
            if (!string.IsNullOrEmpty(_saveDGVDataDumpFile))
            {
                try
                {
                    dataToSave.WriteXml(_saveDGVDataDumpFile, XmlWriteMode.WriteSchema);
                }
                catch (Exception err)
                {
//MessageBox.Show("Error attempting to save XML dataset to: " + _saveDGVDataDumpFile + "\n\nError Message:\n" + err.Message);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error attempting to save XML dataset to: {0}\n\nError Message:\n{1}", "Save Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SaveDGVDataMessage3";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}") &&
    ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, _saveDGVDataDumpFile, err.Message);
}
else if (ggMessageBox.MessageText.Contains("{0}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, _saveDGVDataDumpFile);
}
else if (ggMessageBox.MessageText.Contains("{1}"))
{
    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, err.Message);
}
ggMessageBox.ShowDialog();
                }
            }

            // Restore the statusbar back to it's original state...
            ux_statusCenterMessage.TextAlign = centerMessageAlignment;
            ux_statusCenterMessage.Text = centerMessage;
            ux_statusRightMessage.Text = rightMessage;
            ux_statusRightProgressBar.Visible = false;
            ux_statusstripMain.Refresh();

            return rollupReturn;
        }

//public DataSet GetDGVData(string dataviewName, DataRow[] groupList, string paramsIn, int pageSize)
        public DataSet GetDGVData(string dataviewName, List<KeyValuePair<string, int>> itemList, string parameterList, int pageSize)
        {
            DataSet results = new DataSet();

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Reset the escapeKeyPressed flag...
            escapeKeyPressed = false;

            // Remember the original layout of the status bar (before changing it)...
            ContentAlignment centerMessageAlignment = ux_statusCenterMessage.TextAlign;
            string centerMessage = ux_statusCenterMessage.Text;
            string rightMessage = ux_statusRightMessage.Text;
            ux_statusstripMain.DefaultStateSave();

            // Update the controls on the interface (to disable the user from 
            // choosing a new view tab or tree node during the middle of a query)...
            ux_splitcontainerMain.Panel1.Enabled = false;
            ux_splitcontainerMain.Panel2.Enabled = false;
            try
            {
                if (itemList.Count > 0)
                {
                    int nextPage = pageSize;

                    // Set the statusbar up to show the query progress...
                    ux_statusCenterMessage.TextAlign = ContentAlignment.MiddleRight;
                    ux_statusCenterMessage.Text = "Retrieving Data:";
                    ux_statusRightMessage.Text = "   Press the escape key (esc) to cancel query   ";
                    ux_statusRightProgressBar.Visible = true;
                    ux_statusRightProgressBar.Minimum = 0;
                    ux_statusRightProgressBar.Maximum = 100; // groupList.Length;
                    ux_statusRightProgressBar.Step = 1; // pageSize;
                    ux_statusRightProgressBar.Value = 0;
                    ux_statusstripMain.Refresh();

                    // Begin iterating through the collection of ACIDS, IVIDS, and ORNOS...
                    int pageStart = 0;
                    int pageStop = Math.Min(pageSize, itemList.Count);
                    while (pageStart < itemList.Count && !escapeKeyPressed)
                    {
                        string pkeyCollection = parameterList;
                        for (int i = pageStart; i < pageStop; i++)
                        {
                            if (pkeyCollection.Contains(itemList[i].Key.Trim().ToLower() + "="))
                            {
                                pkeyCollection = pkeyCollection.Replace(itemList[i].Key.Trim().ToLower() + "=", itemList[i].Key.Trim().ToLower() + "=" + itemList[i].Value.ToString() + ",");
                            }
                            else if (pkeyCollection.Contains(itemList[i].Key.Trim().ToLower() + ";"))
                            {
                                pkeyCollection = pkeyCollection.Replace(itemList[i].Key.Trim().ToLower() + ";", itemList[i].Key.Trim().ToLower() + "=" + itemList[i].Value.ToString() + ";");
                            }
                            else
                            {
                                pkeyCollection += itemList[i].Key.Trim().ToLower() + "=" + itemList[i].Value.ToString();
                            }
                        }
                        // Update the paging indexes...
                        pageStart = pageStop;
                        pageStop = Math.Min((pageStart + pageSize), itemList.Count);
                        // Build the param string and get the data...
                        //string pkeyCollection = ":accessionid=" + acids.TrimEnd(',') + "; :inventoryid=" + ivids.TrimEnd(',') + "; :orderrequestid=" + ornos.TrimEnd(',') + "; :cooperatorid=" + coops.TrimEnd(',') + "; :geographyid=" + geos.TrimEnd(',') + "; :taxonomygenusid=" + genus.TrimEnd(',') + "; :cropid=" + crops.TrimEnd(',');
                        DataSet pagedDataSet = _sharedUtils.GetWebServiceData(dataviewName, pkeyCollection, 0, Convert.ToInt32(ux_numericupdownMaxRowsReturned.Value));

                        // Add the results to the dataset that will be returned...
                        if (pagedDataSet.Tables.Contains(dataviewName) &&
                            results.Tables.Contains(dataviewName))
                        {
                            results.Tables[dataviewName].Merge(pagedDataSet.Tables[dataviewName].Copy());
                        }
                        else if (pagedDataSet.Tables.Contains(dataviewName))
                        {
                            results.Tables.Add(pagedDataSet.Tables[dataviewName].Copy());
                        }
                        else
                        {
                            if (pagedDataSet.Tables.Contains("ExceptionTable") &&
                                pagedDataSet.Tables["ExceptionTable"].Rows.Count > 0)
                            {
                                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error retrieving data for {0}.\n\nFull error message:\n{1}", "Get Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                                ggMessageBox.Name = "GetDGVDataMessage1";
                                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                                if (ggMessageBox.MessageText.Contains("{0}") &&
                                    ggMessageBox.MessageText.Contains("{1}"))
                                {
                                    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName, pagedDataSet.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
                                }
                                else if (ggMessageBox.MessageText.Contains("{0}"))
                                {
                                    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName);
                                }
                                else if (ggMessageBox.MessageText.Contains("{1}"))
                                {
                                    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, pagedDataSet.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
                                }
                                ggMessageBox.ShowDialog();
                            }
                        }

                        // Check to see if we have hit the maximum rows returned limit...
                        if (results.Tables[dataviewName].Rows.Count > ux_numericupdownMaxRowsReturned.Value)
                        {
                            // We have hit the max rows limit so set the page start value so that the loop exits normally...
                            pageStart = itemList.Count;
                            for (int i = (int)ux_numericupdownMaxRowsReturned.Value; i < results.Tables[dataviewName].Rows.Count; i++)
                            {
                                results.Tables[dataviewName].Rows[i].Delete();
                            }
                            results.Tables[dataviewName].AcceptChanges();
                        }

                        // Update the progress bar on the statusbar...
                        ux_statusRightProgressBar.Value = pageStart * 100 / itemList.Count; // pageStart;
                        ux_statusstripMain.Refresh();

                        // Check to see if we should bail out (at user request)...
                        Application.DoEvents();
                        Cursor.Current = Cursors.WaitCursor;
                    }
                }
                else
                {
                    // If no primary keys were found return an empty dataset...
                    results = _sharedUtils.GetWebServiceData(dataviewName, "", 0, Convert.ToInt32(ux_numericupdownMaxRowsReturned.Value));

                    if (!results.Tables.Contains(dataviewName) &&
                        results.Tables.Contains("ExceptionTable") &&
                        results.Tables["ExceptionTable"].Rows.Count > 0)
                    {
                        GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error retrieving data for {0}.\n\nFull error message:\n{1}", "Get Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                        ggMessageBox.Name = "GetDGVDataMessage2";
                        _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                        if (ggMessageBox.MessageText.Contains("{0}") &&
                            ggMessageBox.MessageText.Contains("{1}"))
                        {
                            ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName, results.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
                        }
                        else if (ggMessageBox.MessageText.Contains("{0}"))
                        {
                            ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName);
                        }
                        else if (ggMessageBox.MessageText.Contains("{1}"))
                        {
                            ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, results.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
                        }
                        ggMessageBox.ShowDialog();
                    }
                }

            }
            catch (Exception err)
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error retrieving data for {0}.\n\nFull error message:\n{1}", "Get Data Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "GetDGVDataMessage3";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}") &&
                    ggMessageBox.MessageText.Contains("{1}"))
                {
                    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName, err.Message);
                }
                else if (ggMessageBox.MessageText.Contains("{0}"))
                {
                    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, dataviewName);
                }
                else if (ggMessageBox.MessageText.Contains("{1}"))
                {
                    ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, err.Message);
                }
                ggMessageBox.ShowDialog();
                if (!results.Tables.Contains(dataviewName))
                {
                    results.Tables.Add(dataviewName);
                }
            }
            finally
            {
                // Restore the statusbar back to it's original state...
                ux_statusCenterMessage.TextAlign = centerMessageAlignment;
                ux_statusCenterMessage.Text = centerMessage;
                ux_statusRightMessage.Text = rightMessage;
                ux_statusRightProgressBar.Visible = false;
                ux_statusstripMain.Refresh();

                // Allow the user to 'move freely about the cabin' again...
                ux_splitcontainerMain.Panel1.Enabled = true;
                ux_splitcontainerMain.Panel2.Enabled = true;

                // Restore cursor to default cursor...
                Cursor.Current = origCursor;
            }

            return results;

        }

        //private string BuildDelimitedParameterList(string dataviewName, string delimitedParameterList)
        //{
        //    DataSet dataViewParams;
        //    string fullParamList = "";
        //    //dataViewParams = _webServices.GetData("get_dataview_parameters", ":dataview=" + dataviewName, 0, 0);
        //    dataViewParams = _sharedUtils.GetWebServiceData("get_dataview_parameters", ":dataview=" + dataviewName, 0, 0);
        //    if (dataViewParams.Tables.Contains("get_dataview_parameters"))
        //    {
        //        string[] paramKeyValueList = delimitedParameterList.Split(new char[] { ';' });
        //        foreach (DataRow dr in dataViewParams.Tables["get_dataview_parameters"].Rows)
        //        {
        //            string paramName = dr["param_name"].ToString();
        //            fullParamList += paramName + "=; ";
        //            foreach (string paramKeyValue in paramKeyValueList)
        //            {
        //                if (paramKeyValue.Contains(paramName))
        //                {
        //                    fullParamList = fullParamList.Replace(paramName + "=; ", paramName + "=" + paramKeyValue.Substring(paramKeyValue.IndexOf('=') + 1).Trim() + "; ");
        //                }
        //            }
        //        }
        //    }
        //    //string returnParams = ":accessionid=" + acids.TrimEnd(',') + "; :inventoryid=" + ivids.TrimEnd(',') + "; :orderrequestid=" + ornos.TrimEnd(',') + "; :cooperatorid=" + coops.TrimEnd(',');
        //    return fullParamList;
        //}

        private void LoadInventoryImages(string[] fullPaths, string inventoryIDs)
        {
            // Create a new DGV that is wired up to the get_inventory_attach dataview...
            DataGridView imageDGV = new DataGridView();
            imageDGV.DataSource = new BindingSource();
            //DataTable inventoryImages = _sharedUtils.GetWebServiceData("get_inventory_attach", ":inventoryid=0; :accessionid=; :orderrequestid=; :cooperatorid=;", 0, 0).Tables["get_inventory_attach"];
            DataTable inventoryImages = _sharedUtils.GetWebServiceData("get_accession_inv_attach", ":inventoryid=0", 0, 0).Tables["get_accession_inv_attach"];
            if (inventoryImages != null)
            {
                _sharedUtils.BuildEditDataGridView(imageDGV, inventoryImages);

                // Wireup the event handlers for Foreign Key columns...
                imageDGV.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(ux_datagridviewMain_EditingControlShowing);
                imageDGV.CellFormatting += new DataGridViewCellFormattingEventHandler(ux_datagridviewMain_EditDGVCellFormatting);
                imageDGV.DataError += new DataGridViewDataErrorEventHandler(dataGridView_DataError);
            }

            // Show the inventory image loader dialog box for the inventory IDs found...
            if (!string.IsNullOrEmpty(inventoryIDs) &&
                imageDGV != null)
            {
                InventoryImageLoader iil = new InventoryImageLoader(_sharedUtils, imageDGV, fullPaths, inventoryIDs);
iil.StartPosition = FormStartPosition.CenterParent;
                iil.Show();
            }
        }

        private void ux_buttonSearch_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process newSearch = System.Diagnostics.Process.Start(@"C:\VisualStudio2008_SVN\GRINGlobalSearch\GRINGlobalSearch\bin\Debug\GrinGlobalSearch.exe");
//string commandLineParameters = "-user=" + username + " -password=" + password + " -url=" + GRINGlobalWebServices.Url;

            if (SharedUtils.TargetStringFitsPattern(@"^\s*[\S]{1,50}\s*", username) &&
                SharedUtils.TargetStringFitsPattern(@"^\s*[\S]{1,255}\s*", passwordClearText) &&
                SharedUtils.TargetStringFitsPattern(@"^\s*[\S]{1,255}\s*", _sharedUtils.Url))
            {
                string commandLineParameters = "-user=" + username + " -password=" + passwordClearText + " -url=" + _sharedUtils.Url;
                System.Diagnostics.Process newSearch = System.Diagnostics.Process.Start(@"GRINGlobal-SearchTool.exe", commandLineParameters);
            }
        }

        private void GrinGlobalClient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                escapeKeyPressed = true;
            }
            else if (e.KeyCode == Keys.F5 && ux_buttonEditData.Enabled)
            {
                // Resetting these two global variables will force a refresh of the DGV data...
                lastFullPath = "";
                lastTabName = "";
                RefreshMainDGVData();
                RefreshMainDGVFormatting();
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //if (ux_statusRightProgressBar.Visible != true) ux_statusRightProgressBar.Visible = true;
            //ux_statusRightProgressBar.Value = e.ProgressPercentage; 
            //ux_statusCenterMessage.Text = (string) e.UserState;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            //LookupTables tempLookupTables = (LookupTables)e.Result;
            //// Set the statusbar up to show the progress of reloading the lookup tables...
            //ux_statusCenterMessage.TextAlign = ContentAlignment.MiddleRight;
            //ux_statusCenterMessage.Text = "One Moment Please - Updating Lookup Table Data:";
            //ux_statusRightProgressBar.Visible = true;
            //ux_statusRightProgressBar.Minimum = 0;
            //ux_statusRightProgressBar.Maximum = 100; // tempLookupTables.Tables.Count;
            //ux_statusRightProgressBar.Step = 1;
            //ux_statusRightProgressBar.Value = 0;
            //ux_statusstripMain.Refresh();
            //int i = 0;
            //foreach (DataTable dt in tempLookupTables.Tables)
            //{
            //    // Update the progress bar on the statusbar...
            //    ux_statusRightProgressBar.Value = (i++) * 100 / tempLookupTables.Tables.Count; //++;
            //    // Update the lookupTables...
            //    if (lookupTables.Contains(dt.TableName)) lookupTables.Remove(dt.TableName);
            //    lookupTables.Add(dt.Copy());
            //}

            //// Prepare the MRU tables for editing needs of the user...
            //lookupTables.DoSmartMRUTableLoading(site, cno, userSettings);

            //// Refresh the DGV, but first check to make sure the user is not in edit mode...
            //if (ux_buttonEditData.Enabled == true)
            //{
            //    //RefreshMainDGVData();
            //    //RefreshMainDGVFormatting();
            //}

            //// Finally...  Save the new tables to the local hard drive in the background...
            //new System.Threading.Thread(lookupTables.SaveAll).Start();

            //// Restore the statusbar back to it's original state...
            //ux_statusstripMain.DefaultStateRestore();
            //ux_statusstripMain.Refresh();
            //// Enable the reset lookup tables button...
            //ux_buttonLookupTableMaintenance.Enabled = true;

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //e.Result = LoadStandardTables(e.Argument);
        }

        private void ux_buttonLookupTableMaintenance_Click(object sender, EventArgs e)
        {
//LookupTableLoader ltl = new LookupTableLoader(localDBInstance, lookupTables); 
            LookupTableLoader ltl = new LookupTableLoader(localDBInstance, _sharedUtils);
ltl.StartPosition = FormStartPosition.CenterParent;
            ltl.Show();
            //ltl.Focus();

            // Change cursor to the wait cursor...
            //Cursor origCursor = Cursor.Current;
            //Cursor.Current = Cursors.WaitCursor;

            //GrinGlobalClient.ActiveForm.Enabled = false;
            //lookupTables.ResetLookupTables();
            //lookupTables.GetUpdates();
            //GrinGlobalClient.ActiveForm.Enabled = true;

            // Restore cursor to default cursor...
            //Cursor.Current = origCursor;
        }

        private void ux_comboboxActiveWebService_SelectedIndexChanged(object sender, EventArgs e)
        {
// Remember the URL that the user is currently using (in case we need to revert)...
//string originalURL = GRINGlobalWebServices.Url;

            // Save the GUI settings for the user...
            SetAllUserSettings();
//userSettings.Save();
            _sharedUtils.SaveAllUserSettings();

            // Now try to establish a new web service connection...
            try
            {
//                GRINGlobalWebServices.Url = ux_comboboxActiveWebService.SelectedValue.ToString();
//                string webserviceVersion = GRINGlobalWebServices.GetVersion();
////Login loginDialog = new Login(username, password, GUIWebServices.Url, null);
//                Login loginDialog = new Login(GRINGlobalWebServices, GRINGlobalWebServices.Url, null);
//                loginDialog.UserName = username;
//                loginDialog.Password = password;
//                loginDialog.StartPosition = FormStartPosition.CenterScreen;
//                loginDialog.ShowDialog();
SharedUtils tempSharedUtils = new SharedUtils(ux_comboboxActiveWebService.SelectedValue.ToString(), username, passwordClearText, true);

                //if (DialogResult.OK == loginDialog.DialogResult)
                if (tempSharedUtils.IsConnected)
                {
                    // Set the new global SharedUtils global variable to the newly created tempSharedUtils...
                    _sharedUtils = tempSharedUtils;
                    // Get the user login details for this user...
                    username = _sharedUtils.Username;
                    password = _sharedUtils.Password;
                    passwordClearText = _sharedUtils.Password_ClearText;
                    _usernameCooperatorID = _sharedUtils.UserCooperatorID;
                    _currentCooperatorID = _sharedUtils.UserCooperatorID;
                    site = _sharedUtils.UserSite;
                    languageCode = _sharedUtils.UserLanguageCode.ToString();
                    // Create new instance of shared utilities...
//_sharedUtils = new SharedUtils(GRINGlobalWebServices.Url, username, password, localDBInstance, cno);

                    // Set the global variable for the localDBInstance...
                    //localDBInstance = GRINGlobalWebServices.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
                    localDBInstance = _sharedUtils.Url.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
                    localDBInstance = "GRINGlobal_" + localDBInstance;
                    // Reload/rebuild the interface...
                    LoadApplicationData();
                    // Check the status of lookup tables and warn the user if some tables are missing data...
                    LookupTableStatusCheck();
                    // Indicate the web service connection...
                    ux_statusCenterMessage.Text = "Connected to: " + _sharedUtils.Url;
                }
                else
                {
//MessageBox.Show("Unable to connect to " + ux_comboboxActiveWebService.Text + "\nAborting connection request.");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Unable to connect to {0}\nAborting connection request.", "Connection Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ux_comboboxActiveWebServiceMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, ux_comboboxActiveWebService.Text);
ggMessageBox.ShowDialog();
ux_comboboxActiveWebService.SelectedValue = _sharedUtils.Url;
//GRINGlobalWebServices.Url = originalURL;
                }
            }
            catch
            {
//GRINGlobalWebServices.Url = originalURL;
            }
        }

        void defaultBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            //SetAllUserSettings();
            ////userSettings.Save();
            //RefreshMainDGVFormatting();
        }


        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            // This will unselect any rows that might have been multi-selected in the DGV prior to pressing the 
            // red 'X' button on the Navigation Bar...
            ux_datagridviewMain.ClearSelection();
            RefreshMainDGVFormatting();
            //if (ux_datagridviewMain.SelectedRows.Count > 1 &&
            //    DialogResult.Cancel != MessageBox.Show("Warning!!!", "Multi-Row Delete", MessageBoxButtons.OKCancel))
            //{
            //    foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
            //    {
            //        dgvr.DataGridView.Rows.Remove(dgvr);
            //    }
            //}
        }

        private void ux_buttonSaveUserSettingsNow_Click(object sender, EventArgs e)
        {
            SetAllUserSettings();
            _sharedUtils.SaveAllUserSettings();
        }

        //private void ux_buttonAccessionWizard_Click(object sender, EventArgs e)
        //{
        //    Cursor.Current = Cursors.WaitCursor;

        //    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
        //    System.IO.FileInfo[] dllFiles = di.GetFiles("Wizards\\AccessionWizard.dll", System.IO.SearchOption.AllDirectories);
        //    if (dllFiles != null && dllFiles.Length > 0)
        //    {
        //        for (int i = 0; i < dllFiles.Length; i++)
        //        {
        //            System.Reflection.Assembly newAssembly = System.Reflection.Assembly.LoadFile(dllFiles[i].FullName);
        //            foreach (System.Type t in newAssembly.GetTypes())
        //            {
        //                if (t.GetInterface("IGRINGlobalDataWizard", true) != null)
        //                {
        //                    System.Reflection.ConstructorInfo constInfo = t.GetConstructor(new Type[] { typeof(string), typeof(SharedUtils) });
        //                    if (constInfo != null)
        //                    {
        //                        DataTable dt = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource);
        //                        string acids = "";
        //                        string ivids = "";
        //                        string ornos = "";
        //                        string coops = "";
        //                        string geos = "";
        //                        string genus = "";
        //                        string crops = "";
        //                        // Create the string of pkeys to be passed on to the wizard...
        //                        foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
        //                        {
        //                            switch (dt.PrimaryKey[0].ColumnName.Trim().ToUpper())
        //                            {
        //                                case "ACCESSION_ID":
        //                                    acids += dgvr.Cells["accession_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "INVENTORY_ID":
        //                                    ivids += dgvr.Cells["inventory_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "ORDER_REQUEST_ID":
        //                                    ornos += dgvr.Cells["order_request_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "COOPERATOR_ID":
        //                                    coops += dgvr.Cells["cooperator_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "GEOGRAPHY_ID":
        //                                    geos += dgvr.Cells["geography_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "TAXONOMY_GENUS_ID":
        //                                    genus += dgvr.Cells["taxonomy_genus_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "CROP_ID":
        //                                    crops += dgvr.Cells["crop_id"].Value.ToString() + ",";
        //                                    break;
        //                                default:
        //                                    break;
        //                            }
        //                        }
        //                        string pkeyCollection = ":accessionid=" + acids.TrimEnd(',') + "; :inventoryid=" + ivids.TrimEnd(',') + "; :orderrequestid=" + ornos.TrimEnd(',') + "; :cooperatorid=" + coops.TrimEnd(',') + "; :geographyid=" + geos.TrimEnd(',') + "; :taxonomygenusid=" + genus.TrimEnd(',') + "; :cropid=" + crops.TrimEnd(',');
        //                        // Instantiate an object of this type to load...
        //                        Form accessionWizard = (Form)constInfo.Invoke(new object[] { pkeyCollection, _sharedUtils });
        //                        //accessionWizard.Owner = this;
        //                        accessionWizard.FormClosing += new FormClosingEventHandler(wizard_FormClosing);
        //                        accessionWizard.Show();
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //Cursor.Current = Cursors.Default;
        //    Cursor.Current = _cursorGG;
        //}

        private void ux_buttonWizard_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            string wizardDLLPath = "";

            if (sender.GetType() == typeof(ToolStripButton) &&
                ((ToolStripButton)sender).Tag != null) wizardDLLPath = ((ToolStripButton)sender).Tag.ToString();

            if (!string.IsNullOrEmpty(wizardDLLPath))
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                System.IO.FileInfo[] dllFiles = di.GetFiles(wizardDLLPath, System.IO.SearchOption.AllDirectories);
                if (dllFiles != null && dllFiles.Length > 0)
                {
                    for (int i = 0; i < dllFiles.Length; i++)
                    {
                        System.Reflection.Assembly newAssembly = System.Reflection.Assembly.LoadFile(dllFiles[i].FullName);
                        foreach (System.Type t in newAssembly.GetTypes())
                        {
                            if (t.GetInterface("IGRINGlobalDataWizard", true) != null)
                            {
                                System.Reflection.ConstructorInfo constInfo = t.GetConstructor(new Type[] { typeof(string), typeof(SharedUtils) });
                                if (constInfo != null)
                                {
                                    DataTable dt = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource);
                                    string acids = "";
                                    string ivids = "";
                                    string ornos = "";
                                    string coops = "";
                                    string geos = "";
                                    string genus = "";
                                    string crops = "";
                                    // Create the string of pkeys to be passed on to the wizard...
                                    foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
                                    {
                                        switch (dt.PrimaryKey[0].ColumnName.Trim().ToUpper())
                                        {
                                            case "ACCESSION_ID":
                                                acids += dgvr.Cells["accession_id"].Value.ToString() + ",";
                                                break;
                                            case "INVENTORY_ID":
                                                ivids += dgvr.Cells["inventory_id"].Value.ToString() + ",";
                                                break;
                                            case "ORDER_REQUEST_ID":
                                                ornos += dgvr.Cells["order_request_id"].Value.ToString() + ",";
                                                break;
                                            case "COOPERATOR_ID":
                                                coops += dgvr.Cells["cooperator_id"].Value.ToString() + ",";
                                                break;
                                            case "GEOGRAPHY_ID":
                                                geos += dgvr.Cells["geography_id"].Value.ToString() + ",";
                                                break;
                                            case "TAXONOMY_GENUS_ID":
                                                genus += dgvr.Cells["taxonomy_genus_id"].Value.ToString() + ",";
                                                break;
                                            case "CROP_ID":
                                                crops += dgvr.Cells["crop_id"].Value.ToString() + ",";
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    string pkeyCollection = ":accessionid=" + acids.TrimEnd(',') + "; :inventoryid=" + ivids.TrimEnd(',') + "; :orderrequestid=" + ornos.TrimEnd(',') + "; :cooperatorid=" + coops.TrimEnd(',') + "; :geographyid=" + geos.TrimEnd(',') + "; :taxonomygenusid=" + genus.TrimEnd(',') + "; :cropid=" + crops.TrimEnd(',');
                                    // Instantiate an object of this type to load...
                                    Form wizardForm = (Form)constInfo.Invoke(new object[] { pkeyCollection, _sharedUtils });
                                    //wizardForm.Owner = this;
                                    wizardForm.FormClosing += new FormClosingEventHandler(wizard_FormClosing);
                                    wizardForm.Show();

                                }
                            }
                        }
                    }
                }
            }

            Cursor.Current = Cursors.Default;
        }

        //private void ux_buttonOrderWizard_Click(object sender, EventArgs e)
        //{
        //    Cursor.Current = Cursors.WaitCursor;

        //    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
        //    System.IO.FileInfo[] dllFiles = di.GetFiles("Wizards\\OrderWizard.dll", System.IO.SearchOption.AllDirectories);
        //    if (dllFiles != null && dllFiles.Length > 0)
        //    {
        //        for (int i = 0; i < dllFiles.Length; i++)
        //        {
        //            System.Reflection.Assembly newAssembly = System.Reflection.Assembly.LoadFile(dllFiles[i].FullName);
        //            foreach (System.Type t in newAssembly.GetTypes())
        //            {
        //                if (t.GetInterface("IGRINGlobalDataWizard", true) != null)
        //                {
        //                    System.Reflection.ConstructorInfo constInfo = t.GetConstructor(new Type[] { typeof(string), typeof(SharedUtils) });
        //                    if (constInfo != null)
        //                    {
        //                        DataTable dt = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource);
        //                        string acids = "";
        //                        string ivids = "";
        //                        string ornos = "";
        //                        string coops = "";
        //                        string geos = "";
        //                        string genus = "";
        //                        string crops = "";
        //                        // Create the string of pkeys to be passed on to the wizard...
        //                        foreach (DataGridViewRow dgvr in ux_datagridviewMain.SelectedRows)
        //                        {
        //                            switch (dt.PrimaryKey[0].ColumnName.Trim().ToUpper())
        //                            {
        //                                case "ACCESSION_ID":
        //                                    acids += dgvr.Cells["accession_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "INVENTORY_ID":
        //                                    ivids += dgvr.Cells["inventory_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "ORDER_REQUEST_ID":
        //                                    ornos += dgvr.Cells["order_request_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "COOPERATOR_ID":
        //                                    coops += dgvr.Cells["cooperator_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "GEOGRAPHY_ID":
        //                                    geos += dgvr.Cells["geography_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "TAXONOMY_GENUS_ID":
        //                                    genus += dgvr.Cells["taxonomy_genus_id"].Value.ToString() + ",";
        //                                    break;
        //                                case "CROP_ID":
        //                                    crops += dgvr.Cells["crop_id"].Value.ToString() + ",";
        //                                    break;
        //                                default:
        //                                    break;
        //                            }
        //                        }
        //                        string pkeyCollection = ":accessionid=" + acids.TrimEnd(',') + "; :inventoryid=" + ivids.TrimEnd(',') + "; :orderrequestid=" + ornos.TrimEnd(',') + "; :cooperatorid=" + coops.TrimEnd(',') + "; :geographyid=" + geos.TrimEnd(',') + "; :taxonomygenusid=" + genus.TrimEnd(',') + "; :cropid=" + crops.TrimEnd(',');
        //                        // Instantiate an object of this type to load...
        //                        Form orderWizard = (Form)constInfo.Invoke(new object[] { pkeyCollection, _sharedUtils });
        //                        //orderWizard.Owner = this;
        //                        orderWizard.FormClosing += new FormClosingEventHandler(wizard_FormClosing);
        //                        orderWizard.Show(); 
                               
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    Cursor.Current = Cursors.Default;
        //}

        void wizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form wizard = (Form)sender;
            if (e.Cancel)
            {
                wizard.Activate();
            }
            else
            {
                System.Reflection.PropertyInfo pi = wizard.GetType().GetProperty("ChangedRecords");
                if (pi != null)
                {
                    DataTable dt = (DataTable)pi.GetValue(wizard, null);
//SyncUserItemList(dt);
SyncSavedRecordsWithTreeViewAndDGV(dt);
//SaveTabControlData(_ux_NavigatorTabControl, int.Parse(cno));

                    // The next four lines force the RefreshDGVData method to retrieve a fresh copy of the data...
                    lastFullPath = "";
                    lastTabName = "";
                    RefreshMainDGVData();
                    RefreshMainDGVFormatting();
                }
            }
        }

        private int SyncSavedRecordsWithTreeViewAndDGV(DataTable savedRecords)
        {
            int errorCount = 0;
            TreeNode tempTreeNodeParent = new TreeNode();

            if (savedRecords != null && savedRecords.PrimaryKey.Length == 1)
            {
                // Make an empty copy of the user's item list...
//DataTable newGETLISTSItems = userItemList.Clone();
                string pKeyCol = savedRecords.PrimaryKey[0].ColumnName.Trim().ToUpper();
                savedRecords.Columns[pKeyCol].ReadOnly = false;
                foreach (DataRow dr in savedRecords.Rows)
                {
                    DataRow originalRow = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Rows.Find(dr["OriginalPrimaryKeyID"]);

                    switch (dr["SavedAction"].ToString())
                    {
                        case "Insert":
                            // "NewPrimaryKeyID"
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Create a new new TreeNode...
                                TreeNode newNode = new TreeNode();

string newPKeyValue = dr["NewPrimaryKeyID"].ToString();
                                string dataviewPKeyName = dr.Table.PrimaryKey[0].ColumnName.Trim();
                                string tablePKeyName = dr.Table.PrimaryKey[0].ExtendedProperties["table_field_name"].ToString().Trim();
                                string tableName = dr.Table.PrimaryKey[0].ExtendedProperties["table_name"].ToString().Trim();
                                string properties = dataviewPKeyName.Trim().ToUpper();
//properties += ";:" + tablePKeyName + "=" + dr[dr.Table.PrimaryKey[0]].ToString();
//properties += ";:" + tablePKeyName + "=" + newPKeyValue;
properties += ";:" + tablePKeyName.Replace("_", "") + "=" + newPKeyValue;
//properties += ";@" + tableName + "." + tablePKeyName + "=" + dr[dataviewPKeyName].ToString();
properties += ";@" + tableName + "." + tablePKeyName + "=" + newPKeyValue;
                                newNode.Name = properties.Split(';')[1].Trim();
                                newNode.Text = newNode.Name;
                                newNode.Tag = properties;
                                newNode.ImageKey = "inactive_" + tablePKeyName.ToUpper();
                                newNode.SelectedImageKey = "active_" + tablePKeyName.ToUpper();

//newNode.ToolTipText = "-1";
newNode.ToolTipText = "";
// Now check to see if virtual nodes are wanted for this new node...
//if(_virtualNodeTypes.ContainsKey(drv["PROPERTIES"].ToString().Split(';')[0].Trim().ToUpper()))
if (!string.IsNullOrEmpty(_sharedUtils.GetAppSettingValue(properties.Split(';')[0].Trim().ToUpper() + "_VIRTUAL_NODE_DATAVIEW")))
{
    TreeNode dummyNode = new TreeNode();
    string virtualQuery = _sharedUtils.GetAppSettingValue(properties.Split(';')[0].Trim().ToUpper() + "_VIRTUAL_NODE_DATAVIEW") + ", ";
    virtualQuery += properties.Split(';')[1].Trim().ToLower();

    dummyNode.Text = "!!DUMMYNODE!!";
    dummyNode.Name = "!!DUMMYNODE!!"; ;
    dummyNode.Tag = virtualQuery;
    //dummyNode.ImageKey = "inactive_" + virtualQuery;
    //dummyNode.SelectedImageKey = "active_" + virtualQuery;
    newNode.Nodes.Add(dummyNode);
}

                                // Add the node to the collection of nodes for new records...
                                tempTreeNodeParent.Nodes.Add(newNode);

//// Create a new row for the user's item list...
//dr[pKeyCol] = dr["NewPrimaryKeyID"];
//if (((TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0]).SelectedNode != null)
//{
//    DataRow nItem = BuildUserItemListRow(dr, ((TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0]).SelectedNode);
//    if (nItem != null) newGETLISTSItems.Rows.Add(nItem.ItemArray);
//}
                                // Set the datagridview row's status for this new row to committed (and update the pkey with the int returned from the server DB)...
                                if (originalRow != null)
                                {
                                    bool origColumnReadOnlyValue = originalRow.Table.Columns[pKeyCol].ReadOnly;
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = false;
                                    originalRow[pKeyCol] = dr["NewPrimaryKeyID"];
                                    originalRow.AcceptChanges();
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = origColumnReadOnlyValue;
                                }
                            }
                            else
                            {
                                //MessageBox.Show(dr["ExceptionMessage"].ToString());
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Update":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                dr.ClearErrors();
                            }
                            else
                            {
                                //MessageBox.Show(dr["ExceptionMessage"].ToString());
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Delete":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Find the treeview nodes with this pkey and pkey_type...
                                foreach (TabPage tp in _ux_NavigatorTabControl.TabPages)
                                {
                                    foreach (Control ctrl in tp.Controls)
                                    {
                                        if (ctrl.GetType() == typeof(TreeView))
                                        {
                                            TreeView tv = (TreeView)ctrl;
                                            TreeNode[] deletedNodes = tv.Nodes.Find(":" + pKeyCol.Trim().ToLower().Replace("_", "") + "=" + dr[pKeyCol].ToString(), true);
                                            foreach (TreeNode tn in deletedNodes)
                                            {
                                                // Delete the entries in the users item list records...
//DeleteTreeNodes(tv.Parent.Text, tn);
// Add the app_user_item_list pkey(s) associated with this node to the collection of records
// that should be deleted from the server when the CT exits...
_deletedTreeNodes.AddRange(AddToDeletedTreeNodeList(tn));
// Delete the node from the tree view...
tn.Remove();
                                            }
                                        }
                                    }
                                }
                                // Set the row's status for this deleted row to committed...
                                if (originalRow != null)
                                {
                                    originalRow.AcceptChanges();
                                }
                            }
                            else
                            {
                                //MessageBox.Show(dr["ExceptionMessage"].ToString());
                                errorCount++;
                                // Find the deleted row (NOTE: datatable.rows.find() method does not work on deleted rows)...
                                foreach (DataRow deletedRow in ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Rows)
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

            // Ask the user if they would like to add the newly saved items to their treeview's selected node...
            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have successfully added {0} new records to the database.\nWould you like links to these new records added to your current list folder?", "Add new item links", MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "SyncUserItemListMessage1";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, tempTreeNodeParent.Nodes.Count.ToString());
            if (tempTreeNodeParent.Nodes.Count > 0 &&
                DialogResult.OK == ggMessageBox.ShowDialog())
            {
                // Add the new nodes to the active node for the selected tab's treeview...
                foreach (Control ctrl in _ux_NavigatorTabControl.SelectedTab.Controls)
                {
                    if (ctrl.GetType() == typeof(TreeView))
                    {
                        TreeView tv = (TreeView)ctrl;
                        TreeNode parentFolder = null;

// New method...
                        // Find a suitable folder to contain the new nodes...
                        if (tv != null && isFolder(tv.SelectedNode))
                        {
                            parentFolder = tv.SelectedNode;
                        }
                        else if (tv != null &&
                                tv.SelectedNode.Parent != null &&
                                isFolder(tv.SelectedNode.Parent))
                        {
                            parentFolder = tv.SelectedNode.Parent;
                        }
                        else if (tv != null &&
                                tv.SelectedNode.Parent != null &&
                                tv.SelectedNode.Parent.Parent != null)
                        {
                            parentFolder = tv.SelectedNode.Parent.Parent;
                        }

                        // Hopefully we have found a suitable folder to contain the new nodes
                        // and if so add them to that folder now and then refresh the folder's formatting...
                        if (parentFolder != null)
                        {
                            // Add the nodes to the folder...
                            foreach (TreeNode tnNewNode in tempTreeNodeParent.Nodes)
                            {
                                parentFolder.Nodes.Add(tnNewNode);
                            }
                            // Refresh the folder formatting to give the new nodes their proper titles...
                            RefreshTreeviewNodeFormatting(parentFolder);
                        }

                        
// Old method...
//                        foreach (TreeNode tnNewNode in tempTreeNodeParent.Nodes)
//                        {
////if (tv != null && tv.SelectedNode.Tag.ToString().ToUpper() == "FOLDER")
//                            if (tv != null && isFolder(tv.SelectedNode))
//                            {
//                                tv.SelectedNode.Nodes.Add(tnNewNode);
//                                // Give the new nodes their proper titles...
//                                RefreshTreeviewNodeFormatting(tv.SelectedNode);
//                            }
//                            else if (tv.Parent != null &&
//                                    tv.SelectedNode.Parent != null &&
//                                    isFolder(tv.SelectedNode.Parent))
////tv.SelectedNode.Parent.Tag.ToString().ToUpper() == "FOLDER")
//                            {
//                                tv.SelectedNode.Parent.Nodes.Add(tnNewNode);
//                                // Give the new nodes their proper titles...
//                                RefreshTreeviewNodeFormatting(tv.SelectedNode.Parent);
//                            }
//                            else if (tv.Parent != null &&
//                                    tv.SelectedNode.Parent != null &&
//                                    tv.SelectedNode.Parent.Parent != null)
//                            {
//                                tv.SelectedNode.Parent.Parent.Nodes.Add(tnNewNode);
//                                // Give the new nodes their proper titles...
//                                RefreshTreeviewNodeFormatting(tv.SelectedNode.Parent.Parent);
//                            }
//                        }
                    }
                }
            }
            return errorCount;
        }

//        private int SyncTreeViewWithSavedRecords(DataTable savedRecords)
//        {
//            int errorCount = 0;

//            if (savedRecords != null && savedRecords.PrimaryKey.Length == 1)
//            {
//                // Make an empty copy of the user's item list...
//                DataTable newGETLISTSItems = userItemList.Clone();
//                string pKeyCol = savedRecords.PrimaryKey[0].ColumnName.Trim().ToUpper();
//                savedRecords.Columns[pKeyCol].ReadOnly = false;
//                foreach (DataRow dr in savedRecords.Rows)
//                {
//                    DataRow originalRow = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Rows.Find(dr["OriginalPrimaryKeyID"]);

//                    switch (dr["SavedAction"].ToString())
//                    {
//                        case "Insert":
//                            // "NewPrimaryKeyID"
//                            if (dr["SavedStatus"].ToString() == "Success")
//                            {
//                                // Create a new row for the user's item list...
//                                dr[pKeyCol] = dr["NewPrimaryKeyID"];
//                                if (((TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0]).SelectedNode != null)
//                                {
//                                    DataRow nItem = BuildUserItemListRow(dr, ((TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0]).SelectedNode);
//                                    if (nItem != null) newGETLISTSItems.Rows.Add(nItem.ItemArray);
//                                }
//                                // Set the datagridview row's status for this new row to committed (and update the pkey with the int returned from the server DB)...
//                                if (originalRow != null)
//                                {
//                                    bool origColumnReadOnlyValue = originalRow.Table.Columns[pKeyCol].ReadOnly;
//                                    originalRow.Table.Columns[pKeyCol].ReadOnly = false;
//                                    originalRow[pKeyCol] = dr["NewPrimaryKeyID"];
//                                    originalRow.AcceptChanges();
//                                    originalRow.Table.Columns[pKeyCol].ReadOnly = origColumnReadOnlyValue;
//                                }
//                            }
//                            else
//                            {
//                                //MessageBox.Show(dr["ExceptionMessage"].ToString());
//                                errorCount++;
//                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
//                            }
//                            break;
//                        case "Update":
//                            if (dr["SavedStatus"].ToString() == "Success")
//                            {
//                                dr.ClearErrors();
//                            }
//                            else
//                            {
//                                //MessageBox.Show(dr["ExceptionMessage"].ToString());
//                                errorCount++;
//                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
//                            }
//                            break;
//                        case "Delete":
//                            if (dr["SavedStatus"].ToString() == "Success")
//                            {
//                                // Find the treeview nodes with this accession/inventory/order id...
//                                foreach (TabPage tp in ux_tabcontrolGroupListNavigator.TabPages)
//                                {
//                                    foreach (Control ctrl in tp.Controls)
//                                    {
//                                        if (ctrl.GetType() == typeof(TreeView))
//                                        {
//                                            //TreeView tv = (TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0];
//                                            TreeView tv = (TreeView)ctrl;
//                                            TreeNode[] deletedNodes = tv.Nodes.Find(dr[pKeyCol].ToString(), true);
//                                            foreach (TreeNode tn in deletedNodes)
//                                            {
//                                                // Delete the entries in the users item list records...
//                                                DeleteTreeNodes(tv.Parent.Text, tn);
//                                                //// Delete the node from the tree view...
//                                                //tn.Remove();
//                                            }
//                                        }
//                                    }
//                                }
//                                // Set the row's status for this deleted row to committed...
//                                if (originalRow != null)
//                                {
//                                    originalRow.AcceptChanges();
//                                }
//                            }
//                            else
//                            {
//                                //MessageBox.Show(dr["ExceptionMessage"].ToString());
//                                errorCount++;
//                                // Find the deleted row (NOTE: datatable.rows.find() method does not work on deleted rows)...
//                                foreach (DataRow deletedRow in ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Rows)
//                                {
//                                    if (deletedRow[0, DataRowVersion.Original].Equals(dr["OriginalPrimaryKeyID"]))
//                                    {
//                                        deletedRow.RejectChanges();
//                                        deletedRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
//                                    }
//                                }
//                            }
//                            break;
//                        default:
//                            break;
//                    }
//                }

//                // Ask the user if they would like to add the newly saved items to their treeview's selected node...
//                //if (newGETLISTSItems.Rows.Count > 0 &&
//                //    DialogResult.OK == MessageBox.Show("You have successfully added " + newGETLISTSItems.Rows.Count.ToString() + " new records to the database.\nWould you like links to these new records added to your current list folder?", "Add new item links", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
//                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have successfully added {0} new records to the database.\nWould you like links to these new records added to your current list folder?", "Add new item links", MessageBoxButtons.OKCancel, MessageBoxDefaultButton.Button1);
//                ggMessageBox.Name = "SyncUserItemListMessage1";
//                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, newGETLISTSItems.Rows.Count.ToString());
//                if (newGETLISTSItems.Rows.Count > 0 &&
//                    DialogResult.OK == ggMessageBox.ShowDialog())
//                {
//                    // Save the list to the central database...
////DataSet userItemListSaveResults = SaveLists(newGETLISTSItems);

//                    //// Get a fresh copy of the list items for this cooperator from the central database...
//                    //userItemList = GetLists(ux_comboboxCNO.SelectedValue.ToString());

//                    //if (userItemListSaveResults != null && userItemListSaveResults.Tables.Contains(newGETLISTSItems.TableName))
//                    //{
//                    //    // Create a new treenode for each successfully saved new record...
//                    //    foreach (DataRow dr in userItemListSaveResults.Tables[newGETLISTSItems.TableName].Rows)
//                    //    {
//                    //        if (dr["SavedAction"].ToString() == "Insert" && dr["SavedStatus"].ToString() == "Success")
//                    //        {
//                    //            // Build the new treenode...
//                    //            TreeNode tnNewNode = BuildTreeNode(dr);
//                    //            // Add the new node to the active node for the selected tab's treeview...
//                    //            TreeView tv = (TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0];
//                    //            if (tv != null && tv.SelectedNode.Tag.ToString().ToUpper() == "FOLDER")
//                    //            {
//                    //                tv.SelectedNode.Nodes.Add(tnNewNode);
//                    //            }
//                    //            else if (tv.Parent != null &&
//                    //                    tv.SelectedNode.Parent != null &&
//                    //                    tv.SelectedNode.Parent.Tag.ToString().ToUpper() == "FOLDER")
//                    //            {
//                    //                tv.SelectedNode.Parent.Nodes.Add(tnNewNode);
//                    //            }
//                    //            else if (tv.SelectedNode.Parent.Parent != null)
//                    //            {
//                    //                tv.SelectedNode.Parent.Parent.Nodes.Add(tnNewNode);
//                    //            }

//                    //        }
//                    //    }
//                    //}
//                }

//                //// Auto refresh accession and order items in the treeview that would have new children just added...
//                //if (pKeyCol == "INVENTORY_ID" &&
//                //   savedRecords.Columns.Contains("ACCESSION_ID"))
//                //{
//                //    foreach (DataRow dr in savedRecords.Rows)
//                //    {
//                //        TreeNode[] parentNodes = ((TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0]).Nodes.Find(dr["ACCESSION_ID"].ToString(), true);
//                //        foreach (TreeNode tn in parentNodes)
//                //        {
//                //            RefreshVirtualTreeNodes(tn);
//                //        }
//                //    }
//                //}
//                //else if (pKeyCol == "ORDER_REQUEST_ITEM_ID" &&
//                //   savedRecords.Columns.Contains("ORDER_REQUEST_ID"))
//                //{
//                //    foreach (DataRow dr in savedRecords.Rows)
//                //    {
//                //        TreeNode[] parentNodes = ((TreeView)ux_tabcontrolGroupListNavigator.SelectedTab.Controls[0]).Nodes.Find(dr["ORDER_REQUEST_ID"].ToString(), true);
//                //        foreach (TreeNode tn in parentNodes)
//                //        {
//                //            RefreshVirtualTreeNodes(tn);
//                //        }
//                //    }
//                //}
//            }

//            // Save the list to the central database...
////SaveLists(userItemList);
//            //// The next four lines force the RefreshDGVData method to retrieve a fresh copy of the data...
//            //lastFullPath = "";
//            //lastTabName = "";
//            //RefreshMainDGVData();
//            //RefreshMainDGVFormatting();

//            //foreach (TabPage tp in ux_tabcontrolGroupListNavigator.TabPages)
//            //{
//            //    foreach (Control ctrl in tp.Controls)
//            //    {
//            //        if (ctrl.GetType() == typeof(TreeView))
//            //        {
//            //            RefreshTreeViewData((TreeView)ctrl);
//            //        }
//            //    }
//            //}
//            return errorCount;
//        }

        private void ux_menuitemFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            ux_buttonSearch_Click(sender, e);
        }

        private void ux_contextmenustripDataview_Opening(object sender, CancelEventArgs e)
        {
            if (ux_tabcontrolCTDataviews.TabPages.Count < 3)
            {
                ux_dataviewmenuDeleteTab.Enabled = false;
            }
            else
            {
                ux_dataviewmenuDeleteTab.Enabled = true;
            }
        }

        private void ux_checkboxIncludeSubFolders_CheckedChanged(object sender, EventArgs e)
        {
            // Reset the last processed treeview node (to force a full data refresh)...
            lastFullPath = "";
            lastTabName = "";
            // So that this call will be refreshed properly...
            RefreshMainDGVData();
        }

        private void ux_checkboxHotSyncTreeview_CheckedChanged(object sender, EventArgs e)
        {
            if (ux_checkboxHotSyncTreeview.Checked)
            {
                DataGridViewCell dgvc = ux_datagridviewMain.CurrentCell;
                ux_datagridviewMain.CurrentCell = null;
                ux_datagridviewMain.CurrentCell = dgvc;
            }
            else
            {
                TabPage tp = _ux_NavigatorTabControl.SelectedTab;
                foreach (Control ctrl in tp.Controls)
                {
                    if (ctrl.GetType() == typeof(TreeView))
                    {
                        // Reset each node to the default font for the control...
ResetTreeviewNodeFormatting(((TreeView)ctrl).Nodes);
//foreach (TreeNode tn in ((TreeView)ctrl).Nodes)
//{
//    ResetNodeFormat(tn);
//}
                    }
                }
            }
        }

        private void ux_menuitemChangePassword_Click(object sender, EventArgs e)
        {
            ChangePassword cp = new ChangePassword(_sharedUtils.Url, _sharedUtils.Username, "", _sharedUtils);
            cp.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == cp.ShowDialog(this))
            {
                _sharedUtils = new SharedUtils(_sharedUtils.Url, _sharedUtils.Username, cp.Password, true);
            }
        }

        void tsmiLanguage_Click(object sender, EventArgs e)
        {
            int newLangID = 1;
            ToolStripMenuItem tsmi = (ToolStripMenuItem) sender;
            if (int.TryParse(tsmi.Tag.ToString(), out newLangID))
            {
                _sharedUtils.ChangeLanguage(newLangID);
                languageCode = newLangID.ToString();
                _sharedUtils.UserLanguageCode = newLangID;
                _sharedUtils.UpdateComponents(this.components.Components, this.Name);
                _sharedUtils.UpdateControls(this.Controls, this.Name);
                // Save the statusbar text for the left, center, and right controls...
                ux_statusLeftMessage.Tag = ux_statusLeftMessage.Text;
                ux_statusCenterMessage.Tag = ux_statusCenterMessage.Text;
                ux_statusRightMessage.Tag = ux_statusRightMessage.Text;
                ux_statusRightMessage.Text = "";
                // Refresh the DGV data to get new column headings...
                ux_buttonRefreshData.PerformClick();
// Warn the user that they should reload their LU tables...
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have successfully changed your language to {0}.\n\nWarning: Your lookup tables are language specific so you should reload them very soon.\n\nWould you like to do this now?", "Reload Lookup Tables", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "tsmiLanguage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, tsmi.Text);
if (DialogResult.Yes == ggMessageBox.ShowDialog())
{
    // Reload LU tables now...
    LookupTableLoader ltl = new LookupTableLoader(localDBInstance, _sharedUtils);
    ltl.StartPosition = FormStartPosition.CenterParent;
    ltl.Show();
}
            }
        }

        private void ux_menuitemFile_DropDownOpening(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            DataSet ds = new DataSet();
            ds = _sharedUtils.GetWebServiceData("get_sys_language", "", 0, 0);
            // Clear the drop down list of languages...
            ux_menuitemLanguage.DropDownItems.Clear();
            // Rebuild the list of supported languages (English is the default required language)...
            if (ds.Tables.Contains("get_sys_language") &&
                ds.Tables["get_sys_language"].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables["get_sys_language"].Rows)
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(dr["TITLE"].ToString(), null, tsmiLanguage_Click);
                    tsmi.Tag = dr["sys_lang_id"];
                    ux_menuitemLanguage.DropDownItems.Add(tsmi);
                }
            }
            else
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem("English", null, tsmiLanguage_Click);
                tsmi.Tag = 1;
                ux_menuitemLanguage.DropDownItems.Add(tsmi);
            }

            Cursor.Current = Cursors.Default;
        }

        private void ux_menuitemHelpAbout_Click(object sender, EventArgs e)
        {
            //GGMessageBox.Show("Hello World", "Caption Text here...");
            GGMessageBox ggMessageBox = new GGMessageBox("GRIN-Global Curator Tool v{0}\n\n* * * * *\n\nThis software was created by USDA/ARS, with Bioversity International coordinating testing and feedback from the international genebank community.  Development was supported financially by USDA/ARS and by a major grant from the Global Crop Diversity Trust.  This statement by USDA does not imply approval of these enterprises to the exclusion of others which might also be suitable.\n\nUSDA grants to each Recipient of this software non-exclusive, royalty free, world-wide, permission to use, copy, modify, publish, distribute, perform publicly and display publicly this software.  Notice of this permission as well as the other paragraphs in this notice shall be included in all copies or modifications of this software.\n\nThis software application has not been tested or otherwise examined for suitability for implementation on, or compatibility with, any other computer systems.  USDA does not warrant, either explicitly or implicitly, that this software program will not cause damage to the user’s computer or computer operating system, nor does USDA warrant, either explicitly or implicitly, the effectiveness of the software application.\n\nThe English text above shall take precedence in the event of any inconsistencies between the English text and any translation of this notice.\n\n* * * * *", "GRIN-Global Curator Tool v{0}", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "ux_menuitemHelpAboutMessageBox1";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.Caption.Contains("{0}")) ggMessageBox.Caption = string.Format(ggMessageBox.Caption, System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
            DialogResult returnJunk = ggMessageBox.ShowDialog();
        }

        private void ux_menuitemHelpMakeDBAccessible_Click(object sender, EventArgs e)
        {
            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("This action will add all users on this computer to the local SQL Server database.\n\nDo you wish to continue with this action?", "Make Local Database Accessible", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "ux_menuitemHelpMakeDBAccessibleMessageBox1";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.Caption.Contains("{0}")) ggMessageBox.Caption = string.Format(ggMessageBox.Caption, System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
            if (DialogResult.Yes == ggMessageBox.ShowDialog())
            {
                // Execute SQL to add the BUILTIN\Users account to the sysadmins role on the local database...
                // The SQL looks something like this: EXEC master..sp_addsrvrolemember @loginame = N'BUILTIN\Users', @rolename = N'sysadmin'
                if (_sharedUtils.LocalDatabaseMakeAccessibleToAllUsers())
                {
                    ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Local SQL Server database is now accessible to all users of this computer.", "Make Local Database Accessible Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "ux_menuitemHelpMakeDBAccessibleMessageBox2";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                    if (ggMessageBox.Caption.Contains("{0}")) ggMessageBox.Caption = string.Format(ggMessageBox.Caption, System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
                    ggMessageBox.ShowDialog();
                }
                else
                {
                    ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Your attempt to make the local SQL Server database accessible to all users failed.\n\nNOTE: You must be an administrator on the local SQL Server database to perform this action.\n\nRefer to installation manual to perform this task manually if the problem persists.", "Make Local Database Accessible Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "ux_menuitemHelpMakeDBAccessibleMessageBox3";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                    if (ggMessageBox.Caption.Contains("{0}")) ggMessageBox.Caption = string.Format(ggMessageBox.Caption, System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
                    ggMessageBox.ShowDialog();
                }
            }
        }

        private void ux_buttonRefreshData_Click(object sender, EventArgs e)
        {
            if (ux_buttonEditData.Enabled) // This is true when the CT is not in edit mode
            {
                // Resetting these two global variables will force a refresh of the DGV data...
                ux_buttonRefreshData.Enabled = false;
                lastFullPath = "";
                lastTabName = "";
                RefreshMainDGVData();
                RefreshMainDGVFormatting();
                ux_buttonRefreshData.Enabled = true;
            }
        }

        private void ux_menuitemToolsResetTreeviewLists_Click(object sender, EventArgs e)
        {
            DataSet UserListItems = _sharedUtils.GetWebServiceData("get_lists", ":cooperatorid=" + _usernameCooperatorID, 0, 0);
            if (UserListItems.Tables.Contains("get_lists"))
            {
                // Iterate through each record in the users item list and mark it for deletion...
                foreach (DataRow dr in UserListItems.Tables["get_lists"].Rows)
                {
                    dr.Delete();
                }
                // Now save the changes back to the database...
                _sharedUtils.SaveWebServiceData(UserListItems);

                // Reset the user preference for visible tabs to nothing...
                _sharedUtils.SaveUserSetting(_ux_NavigatorTabControl.Name, "TabPages.Order", "");

                // Force a rebuild of the navigator tabs...
                int currentCNOValue = (int)ux_comboboxCNO.SelectedValue;
                ux_comboboxCNO.SelectedValue = -1;
                ux_comboboxCNO.SelectedValue = currentCNOValue;
            }
        }

        private void ux_menuitemToolsResetUserSettings_Click(object sender, EventArgs e)
        {
            // Reset all user preferences for the login account...
            _sharedUtils.DeleteAllUserSettings();
            _sharedUtils.SaveAllUserSettings();
            LoadApplicationData();
        }


        //private void ux_dgvcellmenuEditDelete_Click(object sender, EventArgs e)
        //{

        //}

        //public void UpdateDataviewRowData(object[] updatedItemArray)
        //{
        //    if (ux_buttonEditData.Enabled == false)
        //    {
        //        DataRow dr = ((DataRowView)ux_datagridviewMain.CurrentCell.OwningRow.DataBoundItem).Row;
        //        for (int i = 0; i < updatedItemArray.Length; i++)
        //        {
        //            if (!dr[i].Equals(updatedItemArray[i]))
        //            {
        //                dr[i] = updatedItemArray[i];
        //                ux_datagridviewMain.CurrentCell.OwningRow.Cells[i].Style.BackColor = Color.Yellow;
        //            }
        //        }
        //    }
        //}

        //public int ChangeDataviewRowNumber(int rowNumber)
        //{
        //    int newRowNumber = rowNumber;
        //    if (newRowNumber < 0) newRowNumber = 0;
        //    if (newRowNumber > (ux_datagridviewMain.Rows.Count - 1)) newRowNumber = (ux_datagridviewMain.Rows.Count - 1);
        //    ux_datagridviewMain.CurrentCell = ux_datagridviewMain[ux_datagridviewMain.CurrentCell.ColumnIndex, newRowNumber];
        //    return newRowNumber;
        //}

        //private void ux_treeviewmenuDeleteList_Click(object sender, EventArgs e)
        //{

        //}

        //private void ux_treeviewmenuNewList_Click(object sender, EventArgs e)
        //{

        //}

//        private void OldLoadLookupCodes(string username, string password)
//        {
////MessageBox.Show("Starting LoadLookupCodes");
//            DataSet lookupCodes;

//            // Change cursor to the wait cursor...
//            Cursor origCursor = Cursor.Current;
//            Cursor.Current = Cursors.WaitCursor;

//            // Get the Codes from GRIN...
//            if (GUIWebServices != null)
//            {
//                lookupCodes = GUIWebServices.GetData(false, username, password, "code_value_lookup", "", 0);
//                // Copy the returned table of codes to the grinLookups dataset...
//                if (grinLookups.Tables.Contains("code_value_lookup")) grinLookups.Tables.Remove("code_value_lookup");
//                grinLookups.Tables.Add(lookupCodes.Tables["code_value_lookup"].Copy());
//                grinLookups.Tables["code_value_lookup"].DefaultView.Sort = "code_group_id asc, name asc";
//                if (site.Length > 0) grinLookups.Tables["code_value_lookup"].DefaultView.RowFilter = "site_code='" + site + "' OR is_standard='Y'";
//            }

//            // Restore cursor to default cursor...
//            Cursor.Current = origCursor;
////MessageBox.Show("Finished LoadLookupCodes");
//        }

    }

    public static class StatusStripExtensions
    {
        private static ContentAlignment _ux_statusLeftMessage_TextAlign = ContentAlignment.MiddleLeft;
        private static ContentAlignment _ux_statusCenterMessage_TextAlign = ContentAlignment.MiddleLeft;
        private static ContentAlignment _ux_statusRightMessage_TextAlign = ContentAlignment.MiddleLeft;
        private static string _ux_statusLeftMessage_Text = "";
        private static string _ux_statusCenterMessage_Text = "";
        private static string _ux_statusRightMessage_Text = "";
        private static bool _ux_statusLeftProgressBar_Visible = false;
        private static int _ux_statusLeftProgressBar_Minimum = 0;
        private static int _ux_statusLeftProgressBar_Maximum = 100;
        private static bool _ux_statusRightProgressBar_Visible = false;
        private static int _ux_statusRightProgressBar_Minimum = 0;
        private static int _ux_statusRightProgressBar_Maximum = 100;

        public static void DefaultStateRestore(this System.Windows.Forms.StatusStrip ss)
        {
            ss.Items[0].TextAlign = _ux_statusLeftMessage_TextAlign;
            ss.Items[0].Text = _ux_statusLeftMessage_Text;
            ss.Items[1].Visible = _ux_statusLeftProgressBar_Visible;
            ((ToolStripProgressBar)ss.Items[1]).Minimum = _ux_statusLeftProgressBar_Minimum;
            ((ToolStripProgressBar)ss.Items[1]).Maximum = _ux_statusLeftProgressBar_Maximum;
            ss.Items[2].TextAlign = _ux_statusCenterMessage_TextAlign;
            ss.Items[2].Text = _ux_statusCenterMessage_Text;
            ss.Items[3].Visible = _ux_statusRightProgressBar_Visible;
            ((ToolStripProgressBar)ss.Items[3]).Minimum = _ux_statusRightProgressBar_Minimum;
            ((ToolStripProgressBar)ss.Items[3]).Maximum = _ux_statusRightProgressBar_Maximum;
            ss.Items[4].TextAlign = _ux_statusRightMessage_TextAlign;
            ss.Items[4].Text = _ux_statusRightMessage_Text;
        }

        public static void DefaultStateSave(this System.Windows.Forms.StatusStrip ss)
        {
            _ux_statusLeftMessage_TextAlign = ss.Items[0].TextAlign;
            _ux_statusLeftMessage_Text = ss.Items[0].Text;
            _ux_statusLeftProgressBar_Visible = ss.Items[1].Visible;
            _ux_statusLeftProgressBar_Minimum = ((ToolStripProgressBar)ss.Items[1]).Minimum;
            _ux_statusLeftProgressBar_Maximum = ((ToolStripProgressBar)ss.Items[1]).Maximum;
            _ux_statusCenterMessage_TextAlign = ss.Items[2].TextAlign;
            _ux_statusCenterMessage_Text = ss.Items[2].Text;
            _ux_statusRightProgressBar_Visible = ss.Items[3].Visible;
            _ux_statusRightProgressBar_Minimum = ((ToolStripProgressBar)ss.Items[3]).Minimum;
            _ux_statusRightProgressBar_Maximum = ((ToolStripProgressBar)ss.Items[3]).Maximum;
            _ux_statusRightMessage_TextAlign = ss.Items[4].TextAlign;
            _ux_statusRightMessage_Text = ss.Items[4].Text;
        }
    }

}
