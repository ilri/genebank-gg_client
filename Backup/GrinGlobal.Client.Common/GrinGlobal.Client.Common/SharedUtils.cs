using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace GRINGlobal.Client.Common
{
    public struct DataviewProperties
    {
        public string TabName;
        public string DataviewName;
        public string StrongFormName;
        public string ViewerStyle;
        public string AlwaysOnTop;
    }

    public struct FormsData
    {
        public string FormName;
        public string PreferredDataviewName;
        public System.Reflection.ConstructorInfo ConstInfo;
        public string StrongFormName;
    }

    public class SharedUtils
    {
        private WebServices _webServices;
        private LookupTables _lookupTables;
        private LocalDatabase _localDatabase;
        private UserSettings _userSettings;
        private AppSettings _appSettings;
        private UserInterfaceUtils _userInterfaceUtils;
        private bool _isConnected = false;
        private Dictionary<string, string> _webServiceURLs;
        private string _userCooperatorID = "";
        private int _userLanguageCode = 0;
        private string _userSite = "";
        private string _appName = "GRINGlobalClientCuratorTool";

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
        }

        public string Username
        {
            get
            {
                return _webServices.Username;
            }
        }

        public string Password
        {
            get
            {
                return _webServices.Password;
            }
        }

        public string Password_ClearText
        {
            get
            {
                return _webServices.Password_ClearText;
            }
        }

        public string UserCooperatorID
        {
            get
            {
                return _userCooperatorID;
            }
        }

        public int UserLanguageCode
        {
            get
            {
                return _userLanguageCode;
            }
            set
            {
                _userLanguageCode = value;
                _appSettings = new AppSettings(_webServices, _userLanguageCode, _appName);
            }
        }

        public string UserSite
        {
            get
            {
                return _userSite;
            }
        }

        public string Url
        {
            get
            {
                return _webServices.Url;
            }
        }

        public Dictionary<string, string> WebServiceURLs
        {
            get
            {
                return _webServiceURLs;
            }
        }

        public int LookupTablesLoadingPageSize
        {
            get
            {
                return _lookupTables.PageSize;
            }
            set
            {
                _lookupTables.PageSize = value;
            }
        }


        public SharedUtils(string webServiceURL, string webServiceUsername, string webServicePassword, string localDBInstance, string appName, string cno)
        {
            _isConnected = TestCredentials(webServiceURL, webServiceUsername, webServicePassword);
            _appName = appName;
            if (!_isConnected)
            {
                Login loginDialog = new Login(webServiceUsername, webServicePassword, webServiceURL, null, false);
loginDialog.StartPosition = FormStartPosition.CenterScreen;
                loginDialog.ShowDialog();
                if (DialogResult.OK == loginDialog.DialogResult)
                {
                    _isConnected = true;
                    webServiceURL = loginDialog.SelectedWebServiceURL;
                    webServiceUsername = loginDialog.UserName;
                    webServicePassword = loginDialog.Password_ClearText;
                    //_userCooperatorID = loginDialog.UserCooperatorID;
                    int langCode = 1;
                    if (int.TryParse(loginDialog.UserLanguageCode, out langCode))
                    {
                        _userLanguageCode = langCode;
                    }
                    else
                    {
                        _userLanguageCode = 1;
                    }
                    _userSite = loginDialog.UserSite;
                    _webServiceURLs = loginDialog.WebServiceURLs;
                }
            }

            if (_isConnected)
            {
                _webServices = new WebServices(webServiceURL, webServiceUsername, SHA1EncryptionBase64(webServicePassword), webServicePassword, _userSite);
                _localDatabase = new LocalDatabase(localDBInstance);
                _userSettings = new UserSettings(_webServices, cno);
                _appSettings = new AppSettings(_webServices, _userLanguageCode, appName);
                _userInterfaceUtils = new UserInterfaceUtils(_webServices);
                bool optimizeLUTForSpeed = false;
                bool.TryParse(_userSettings["ux_checkboxOptimizeLUTForSpeed", "Checked"], out optimizeLUTForSpeed);
                _lookupTables = new LookupTables(_webServices, _localDatabase, optimizeLUTForSpeed);
            }
        }

        public SharedUtils(string webServiceURL, string webServiceUsername, string webServicePassword, bool hideServerList)
        {
            _isConnected = TestCredentials(webServiceURL, webServiceUsername, webServicePassword);
            _appName = "GRINGlobalClientCuratorTool";
            if (!_isConnected)
            {
                Login loginDialog = new Login(webServiceUsername, webServicePassword, webServiceURL, null, hideServerList);
loginDialog.StartPosition = FormStartPosition.CenterScreen;
                loginDialog.ShowDialog();
                if (DialogResult.OK == loginDialog.DialogResult)
                {
                    _isConnected = true;
                    webServiceURL = loginDialog.SelectedWebServiceURL;
                    webServiceUsername = loginDialog.UserName;
                    webServicePassword = loginDialog.Password_ClearText;
                    _userCooperatorID = loginDialog.UserCooperatorID;
                    int langCode = 1;
                    if (int.TryParse(loginDialog.UserLanguageCode, out langCode))
                    {
                        _userLanguageCode = langCode;
                    }
                    else
                    {
                        _userLanguageCode = 1;
                    }
                    _userSite = loginDialog.UserSite;
                    _webServiceURLs = loginDialog.WebServiceURLs;
                }
            }

            if (_isConnected)
            {
                // Build the local DB name from the web service URL...
                string localDBInstance = webServiceURL.ToLower().Replace("http://", "").Replace("/gringlobal/gui.asmx", "").Replace('-', '_').Replace('.', '_').Replace(':', '_');
                localDBInstance = "GRINGlobal_" + localDBInstance;
                _webServices = new WebServices(webServiceURL, webServiceUsername, SHA1EncryptionBase64(webServicePassword), webServicePassword, _userSite);
                _localDatabase = new LocalDatabase(localDBInstance);
                _userSettings = new UserSettings(_webServices, _userCooperatorID);
                DataSet userData = _webServices.ValidateLogin();
                if (userData != null &&
                    userData.Tables.Contains("validate_login") &&
                    userData.Tables["validate_login"].Rows.Count > 0)
                {
                    int sys_lang_id = 1;
                    if (int.TryParse(userData.Tables["validate_login"].Rows[0]["sys_lang_id"].ToString(), out sys_lang_id))
                    {
                        _userLanguageCode = sys_lang_id;
                    }
                    else
                    {
                        _userLanguageCode = 1;
                    }
                }
                _appSettings = new AppSettings(_webServices, _userLanguageCode, _appName);
                _userInterfaceUtils = new UserInterfaceUtils(_webServices);
                bool optimizeLUTForSpeed = false;
                bool.TryParse(_userSettings["ux_checkboxOptimizeLUTForSpeed", "Checked"], out optimizeLUTForSpeed);
                _lookupTables = new LookupTables(_webServices, _localDatabase, optimizeLUTForSpeed);
            }
        }

        private bool TestCredentials(string webServiceURL, string webServiceUsername, string webServicePassword)
        {
            bool validLoginCredentials = false;
            
            // Bail out now if the URL or username are empty strings...
            if (string.IsNullOrEmpty(webServiceURL) || string.IsNullOrEmpty(webServiceUsername)) return validLoginCredentials;

            try
            {
                _webServices = new WebServices(webServiceURL, webServiceUsername, SHA1EncryptionBase64(webServicePassword), webServicePassword, _userSite);
                DataSet userData = _webServices.ValidateLogin();
                if (userData != null &&
                    userData.Tables.Contains("validate_login") &&
                    userData.Tables["validate_login"].Rows.Count > 0)
                {
                    // Successful login...
                    validLoginCredentials = true;
                    if (userData.Tables["validate_login"].Columns.Contains("cooperator_id")) _userCooperatorID = userData.Tables["validate_login"].Rows[0]["cooperator_id"].ToString();
                    if (userData.Tables["validate_login"].Columns.Contains("site_code")) _userSite = userData.Tables["validate_login"].Rows[0]["site_code"].ToString();
                    if (userData.Tables["validate_login"].Columns.Contains("site")) _userSite = userData.Tables["validate_login"].Rows[0]["site"].ToString();
                    string langString = "0";
                    if (userData.Tables["validate_login"].Columns.Contains("sec_lang_id")) langString = userData.Tables["validate_login"].Rows[0]["sec_lang_id"].ToString();
                    if (userData.Tables["validate_login"].Columns.Contains("sys_lang_id")) langString = userData.Tables["validate_login"].Rows[0]["sys_lang_id"].ToString();
                    int langInt = 0;
                    if (int.TryParse(langString, out langInt)) _userLanguageCode = langInt;
                }
            }
            catch
            {
                // Unsuccessful login...
                validLoginCredentials = false;
            }
            return validLoginCredentials;
        }

        public DataSet ChangeLanguage(int newLanguage)
        {
            return _webServices.ChangeLanguage(newLanguage);
        }

        public DataSet ChangeOwnership(DataSet ownedDataset, string newCNO, bool includeChildren)
        {
            int cno = -1;
            if (!int.TryParse(newCNO, out cno))
            {
                // The supplied cooperator id is invalid...
                DataSet userData = _webServices.ValidateLogin();
                if (userData != null &&
                    userData.Tables.Contains("validate_login") &&
                    userData.Tables["validate_login"].Rows.Count > 0)
                {
                    // Successful login - so get the users cno...
                    if (userData.Tables["validate_login"].Columns.Contains("cooperator_id")) cno = (int)userData.Tables["validate_login"].Rows[0]["cooperator_id"];
                }
                else
                {
                    // Hummmm...  Things are looking real bad - set the cno to an -1 (there should never be a cno = -1, so this should error out on the server)
                    cno = -1;
                }
            }
            return _webServices.ChangeOwnership(ownedDataset, cno, includeChildren);
        }
        
        public string GetLookupDisplayMember(string lookupTable, string valueMember, string groupName, string defaultDisplayMember)
        {
            string displayMember = "";

            if (lookupTable.ToLower().Trim() != "code_value_lookup")
            {
                int pkey = -1;
                if (int.TryParse(valueMember, out pkey))
                {
                    displayMember = _lookupTables.GetPKeyDisplayMember(lookupTable, pkey, defaultDisplayMember);
                }
                else
                {
                    displayMember = defaultDisplayMember;
                }
            }
            else
            {
                displayMember = _lookupTables.GetCodeValueDisplayMember(groupName + valueMember, defaultDisplayMember);
            }
            return displayMember;
        }

        public string GetLookupValueMember(DataRow dr, string lookupTable, string displayMember, string groupName, string defaultValueMember)
        {
            string valueMember = "";
            if (lookupTable.ToLower().Trim() != "code_value_lookup")
            {
                int pkey;
                if (int.TryParse(defaultValueMember, out pkey))
                {
                    valueMember = _lookupTables.GetPKeyValueMember(dr, lookupTable, displayMember, pkey).ToString();
                }
                else
                {
                    valueMember = _lookupTables.GetPKeyValueMember(dr, lookupTable, displayMember, -1).ToString();
                    if (valueMember == "-1") valueMember = defaultValueMember;
                }
            }
            else
            {
                valueMember = _lookupTables.GetCodeValueValueMember(displayMember, groupName, defaultValueMember);
            }
            return valueMember;
        }

        public DataTable GetLocalData(string SQLSelect, string SQLparms)
        {
            DataTable dt;
            string[] parms = SQLparms.Split(';');
            dt = _localDatabase.GetData(SQLSelect, parms);
            return dt;
        }

        public bool SaveLocalData(DataTable DataTable)
        {
            return _localDatabase.SaveData(DataTable);
        }

        public DataSet GetWebServiceData(string dataviewName, string delimitedParameterList, int offset, int limit)
        {
            DataSet results = new DataSet();
            DataSet dataViewParams = new DataSet();
            if (IsConnected)
            {
                results = _webServices.GetData(dataviewName.ToLower(), delimitedParameterList, offset, limit);
            }      
            return results;
        }

        public DataSet SaveWebServiceData(DataSet modifiedDataSet)
        {
            DataSet ds = new DataSet();
            if (IsConnected)
            {
                ds = _webServices.SaveData(modifiedDataSet);
            }
            return ds;
        }

        public DataSet SearchWebService(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit)
        {
            DataSet ds;
            ds = _webServices.Search(query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit, "parseonly=false; passthru=NonIndexedOrComparison; languageid=0; lookupcodedvalues=true; ignorecache=false");
            return ds;
        }

        public DataSet SearchWebService(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit, string searchOptions)
        {
            DataSet ds;
            ds = _webServices.Search(query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit, searchOptions);
            return ds;
        }

        public string GetUserSetting(string resource, string key, string defaultReturnValue)
        {
            string returnValue = _userSettings[resource, key];
            if (string.IsNullOrEmpty(returnValue)) returnValue = defaultReturnValue;
            return returnValue;
        }

        public void DeleteUserSetting(string resource, string key)
        {
            _userSettings.Delete(resource, key);
        }

        public void SaveUserSetting(string resource, string key, string value)
        {
            _userSettings[resource, key] = value;
        }

        public void SaveAllUserSettings()
        {
            _userSettings.Save();
        }

        public void LoadAllUserSettings()
        {
            _userSettings.Load();
        }

        public void DeleteAllUserSettings()
        {
            _userSettings.DeleteAll();
        }

        public Cursor LoadCursor(string filePath)
        {
            Cursor interopCursor;
            try
            {
                interopCursor = new Cursor(LoadCursorFromFile(filePath));
            }
            catch
            {
                interopCursor = null;
            }
            return interopCursor;
        }

        public string SaveImage(string destinationFilePath, byte[] imageBytes, bool createThumbnail, bool overWriteIfExists)
        {
            return _webServices.UploadImage(destinationFilePath, imageBytes, createThumbnail, overWriteIfExists);
        }

        public byte[] GetImage(string filePath)
        {
            return _webServices.DownloadImage(filePath);
        }

        public string GetAppSettingValue(string name)
        {
            return _appSettings.GetAppSettingValue(name);
        }

        public void UpdateComponents(System.ComponentModel.ComponentCollection compCollection, string formName)
        {
            _appSettings.UpdateComponents(compCollection, formName);
        }

        public void UpdateControls(System.Windows.Forms.Control.ControlCollection ctrlCollection, string formName)
        {
            _appSettings.UpdateControls(ctrlCollection, formName);
        }

        public bool LocalDatabaseTableExists(string tableName)
        {
            return _localDatabase.TableExists(tableName);
        }

        public bool LocalDatabaseMakeAccessibleToAllUsers()
        {
            return _localDatabase.MakeAccessibleToAllUsers();
        }

        public DataTable LookupTablesGetMatchingRows(string dataviewName, string displayMemberLikeFilter, int maxReturnRows)
        {
            return _lookupTables.LookupTableGetMatchingRows(dataviewName, displayMemberLikeFilter, maxReturnRows);
        }

        public void LookupTablesUpdateTable(object objDataviewName, bool runAsBackgroundThread)
        {
            if (runAsBackgroundThread)
            {
                new System.Threading.Thread(_lookupTables.UpdateTable).Start(objDataviewName);
            }
            else
            {
                _lookupTables.UpdateTable(objDataviewName);
            }
        }

        public void LookupTablesLoadTableFromDatabase(object objDataviewName)
        {
            _lookupTables.LoadTableFromDatabase(objDataviewName);
        }

        public DataTable LookupTablesGetSynchronizationStats()
        {
            return _lookupTables.GetSynchronizationStats();
        }

        public bool LookupTablesIsUpdated(string tableName)
        {
            return _lookupTables.IsUpdated(tableName);
        }

        public void LookupTablesClearLookupTable(object objDataviewName)
        {
            _lookupTables.ClearLocalLookupTable(objDataviewName);
        }

        public bool LookupTablesIsValidFKField(DataColumn dc)
        {
            return _lookupTables.IsValidFKField(dc);
        }

        public bool LookupTablesIsValidCodeValueField(DataColumn dc)
        {
            return _lookupTables.IsValidCodeValueField(dc);
        }

        public void LookupTablesSaveALLCaches()
        {
            _lookupTables.SaveAllLUTDictionaries();
        }
        
        public void BuildReadOnlyDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            _userInterfaceUtils.buildReadOnlyDataGridView(dataGridView, dataTable, _lookupTables);
        }

        public void BuildEditDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            _userInterfaceUtils.buildEditDataGridView(dataGridView, dataTable, _lookupTables, _userCooperatorID);
        }

        public void BindComboboxToCodeValue(ComboBox comboBox, DataColumn dc)
        {
            if (dc.ExtendedProperties.Contains("group_name"))
            {
                string groupName = dc.ExtendedProperties["group_name"].ToString();
                DataTable dt = _lookupTables.GetCodeValueDataTableByGroupName(groupName);
                if (dc.ExtendedProperties.Contains("is_nullable") && dc.ExtendedProperties["is_nullable"].ToString() == "Y")
                {
                    DataRow dr = dt.NewRow();
                    dr["display_member"] = "[Null]";
                    dr["value_member"] = DBNull.Value;
                    dt.Rows.InsertAt(dr, 0);
                    dt.AcceptChanges();
                }

                comboBox.DisplayMember = "display_member";
                comboBox.ValueMember = "value_member";
                comboBox.DataSource = dt;
                //comboBox.DefaultCellStyle.DataSourceNullValue = DBNull.Value;
                //comboBox.DefaultCellStyle.NullValue = "[Null]";
            }
        }

        public bool ProcessDGVEditShortcutKeys(DataGridView dgv, KeyEventArgs e, string cno)
        {
            return _userInterfaceUtils.ProcessDGVEditShortcutKeys(dgv, e, cno, _lookupTables);
        }

        public bool ImportTextToDataTableUsingKeys(string rawImportText, DataTable destinationTable, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        {
            return _userInterfaceUtils.ImportTextToDataTableUsingKeys(rawImportText, destinationTable, rowDelimiters, columnDelimiters, out badRows, out missingRows, _lookupTables);
        }

        public bool ImportTextToDataTableUsingBlockStyle(string rawImportText, DataGridView dataGridView, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        {
            return _userInterfaceUtils.ImportTextToDataTableUsingBlockStyle(rawImportText, dataGridView, rowDelimiters, columnDelimiters, out badRows, out missingRows, _lookupTables);
        }

        public string GetFriendlyFieldName(DataColumn dc, string defaultName)
        {
            return _userInterfaceUtils.GetFriendlyFieldName(dc, defaultName);
        }

        public void ux_tabcontrolCreateNewTab(TabControl ux_tabcontrolDataview, int indexOfNewTab)
        {
            _userInterfaceUtils.ux_tabcontrolCreateNewTab(ux_tabcontrolDataview, indexOfNewTab);
        }

        public void ux_tabcontrolAddTab(TabControl ux_tabcontrolDataview, string text, DataviewProperties tag, int position)
        {
            _userInterfaceUtils.ux_tabcontrolAddTab(ux_tabcontrolDataview, text, tag, position);
        }

        public void ux_tabcontrolShowProperties(TabControl ux_tabcontrolDataview, int indexOfCurrentTab)
        {
            _userInterfaceUtils.ux_tabcontrolShowProperties(ux_tabcontrolDataview, indexOfCurrentTab);
        }

        public void ux_tabcontrolRemoveTab(TabControl ux_tabcontrolDataview, int indexOfTabToDelete)
        {
            _userInterfaceUtils.ux_tabcontrolRemoveTab(ux_tabcontrolDataview, indexOfTabToDelete);
        }

        public void ux_tabcontrolMouseDownEvent(TabControl ux_tabcontrolDataview, MouseEventArgs e)
        {
            _userInterfaceUtils.ux_tabcontrolMouseDownEvent(ux_tabcontrolDataview, e);
        }

        public void ux_tabcontrolDragOverEvent(TabControl ux_tabcontrolDataview, DragEventArgs e)
        {
            _userInterfaceUtils.ux_tabcontrolDragOverEvent(ux_tabcontrolDataview, e);
        }

        public void ux_tabcontrolDragDropEvent(TabControl ux_tabcontrolDataview, DragEventArgs e)
        {
            _userInterfaceUtils.ux_tabcontrolDragDropEvent(ux_tabcontrolDataview, e);
        }

        public void BuildDataviewTabControl(TabControl ux_tabcontrolDataview)
        {
            _userInterfaceUtils.buildDataviewTabControl(ux_tabcontrolDataview, this);
        }
             
        //// SHA1 encrypted bytes:  b3aca92c793ee0e9b1a9b0a5f5fc044e05140df3
        //// Base64 string conversion of SHA1 bytes: s6ypLHk+4OmxqbCl9fwETgUUDfM=
        //byte[] sha1 = sha1Hash.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(clearText));
        //string sha1Base64 = Convert.ToBase64String(sha1);
        public static string SHA1EncryptionBase64(string clearText)
        {
            string encryptedText = "";
            // Example encryption:  "administrator" = "s6ypLHk+4OmxqbCl9fwETgUUDfM="

            // First create a SHA1 class to do the encryption...
            System.Security.Cryptography.SHA1 sha1Hash = new System.Security.Cryptography.SHA1Managed();
            // This next line does three things:
            // 1) Encode the clearText into an array of UTF8 bytes 
            // 2) Pass the byte array to the SHA1 class (to compute the SHA1 hash)
            // 3) Convert the SHA1 encrypted bytes array to a Base 64 string (so that it can pass through to the webservice as standard text)...
            encryptedText = Convert.ToBase64String(sha1Hash.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(clearText)));

            return encryptedText;
        }

        public static bool TargetStringFitsPattern(string regexPattern, string targetString)
        {
            bool patternFound = false;

            if (targetString.Length > 0 &&
                regexPattern.Length > 0)
            {
                patternFound = System.Text.RegularExpressions.Regex.IsMatch(targetString, regexPattern);
            }

            return patternFound;
        }

        public FormsData[] GetDataviewFormsData()
        {
            FormsData[] localAssembliesFormsData = new FormsData[0];

            try
            {
                // Load the static forms from the same directory (and all subdirectories) where the Curator Tool was launched...
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
                System.IO.FileInfo[] dllFiles = null;
                if(System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory() + "\\Forms")) dllFiles = di.GetFiles("Forms\\*.dll", System.IO.SearchOption.AllDirectories);
                if (dllFiles != null && dllFiles.Length > 0)
                {
                    localAssembliesFormsData = new FormsData[dllFiles.Length];
                    for (int i = 0; i < dllFiles.Length; i++)
                    {
                        System.Reflection.Assembly newAssembly = System.Reflection.Assembly.LoadFile(dllFiles[i].FullName);
                        foreach (System.Type t in newAssembly.GetTypes())
                        {
                            if (t.GetInterface("IGRINGlobalDataForm", true) != null)
                            {
                                System.Reflection.ConstructorInfo constInfo = t.GetConstructor(new Type[] { typeof(BindingSource), typeof(bool), typeof(SharedUtils), typeof(bool) });
                                if (constInfo != null)
                                {
                                    // Instantiate an object of this type to inspect...
                                    Form f = (Form)constInfo.Invoke(new object[] { new BindingSource(), false, this, false });
                                    // Get the Form Name and save it to the array...
                                    System.Reflection.PropertyInfo propInfo = t.GetProperty("FormName", typeof(string));
                                    string formName = (string)propInfo.GetValue(f, null);
                                    if (string.IsNullOrEmpty(formName)) formName = t.Name;
                                    localAssembliesFormsData[i].FormName = formName;
                                    // Get the preferred dataview name and save it to the array...
                                    propInfo = t.GetProperty("PreferredDataview", typeof(string));
                                    string preferredDataview = (string)propInfo.GetValue(f, null);
                                    if (string.IsNullOrEmpty(preferredDataview)) preferredDataview = "";
                                    localAssembliesFormsData[i].PreferredDataviewName = preferredDataview;
                                    // Save the constructor info object to the array...
                                    localAssembliesFormsData[i].ConstInfo = constInfo;
                                    localAssembliesFormsData[i].StrongFormName = constInfo.Module.FullyQualifiedName + " : " + formName + " : " + preferredDataview;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show("Error binding to dataview Form.\nError Message: " + err.Message);
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error binding to dataview Form.\nError Message: {0}", "Form Binding Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "GrinGlobalClient_LoadMessage1";
                this.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, err.Message);
                ggMessageBox.ShowDialog();
            }

            return localAssembliesFormsData;
        }

        public Dictionary<string, string> GetReportsMapping()
        {
            string usersReportsMappingFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\ReportsMapping.txt";
            string allUsersReportsMappingFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\ReportsMapping.txt";
            string reportsMappingFilePath = "";

            Dictionary<string, string> reportsMapping = new Dictionary<string, string>();

            // Look for (and process) a ReportsMapping.txt file to allow custom application settings entries...
            // But first make sure the roaming profile directory exists...
            if (!System.IO.Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool")) System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool");

            // Now check the user's personal roaming profile for a copy of the ReportsMapping.txt file...
            if (System.IO.File.Exists(usersReportsMappingFilePath))
            {
                // Found a personal user's copy of the ReportsMapping.txt file - so use it...
                reportsMappingFilePath = usersReportsMappingFilePath;
            }
            else if (System.IO.File.Exists(allUsersReportsMappingFilePath))
            {
                // Couldn't find a copy of the ReportsMapping.txt file so copy the master into the user's personal roaming profile AppData directory...
                System.IO.File.Copy(allUsersReportsMappingFilePath, usersReportsMappingFilePath);
                reportsMappingFilePath = usersReportsMappingFilePath;
            }

            // Process the ReportsMapping.txt file...
            if (!string.IsNullOrEmpty(reportsMappingFilePath))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(reportsMappingFilePath);
                while (!sr.EndOfStream)
                {
                    string reportMap = sr.ReadLine().Trim().ToUpper();
                    if (!string.IsNullOrEmpty(reportMap.Trim()) &&
                        !reportMap.Trim().StartsWith("#"))
                    {
                        string[] keyValuePair = reportMap.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (keyValuePair != null)
                        {
                            // Force the key to uppercase and trim...
                            keyValuePair[0] = keyValuePair[0].Trim();
                            if (keyValuePair.Length >= 2)
                            {
                                // Working off the assumption that the first '=' sign found is the delimiter for the key/value pair so concat all fields after the first '=' sign...
                                for (int i = 2; i < keyValuePair.Length; i++)
                                {
                                    keyValuePair[1] += "=" + keyValuePair[i];
                                }
                                // Now that the value in the key/value pair has been knitted back together (ie. includes any other '=' sign characters the
                                // Value can be properly formatted using the vertical pipe as a delimiter...
                                string[] valueTokens = keyValuePair[1].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                if (valueTokens.Length > 1)
                                {
                                    keyValuePair[1] = "";
                                    foreach (string token in valueTokens)
                                    {
                                        keyValuePair[1] += " " + token.Trim() + " |";
                                    }
                                    keyValuePair[1] = keyValuePair[1].TrimEnd('|');
                                }
                                else if(valueTokens.Length == 1)
                                {
                                    keyValuePair[1] = " " + valueTokens[0].Trim() + " ";
                                }
                            }
                            else if (keyValuePair.Length < 2)
                            {
                                break;
                            }
                            // Update or Add the new Report Mapping...
                            if (!reportsMapping.ContainsKey(keyValuePair[0].Trim()))
                            {
                                // This setting does not exist so Add it...
                                reportsMapping.Add(keyValuePair[0].Trim(), keyValuePair[1]);
                            }
                            else
                            {
                                // This setting is already in the dictionary so update it's value...
                                reportsMapping[keyValuePair[0].Trim()] += "|" + keyValuePair[1];
                            }
                        }
                    }
                }
                sr.Close();
                sr.Dispose();
            }
            return reportsMapping;
        }

    }
}
