using System;
using System.Data;
//using System.Drawing;
//using System.Windows.Forms;
using System.Linq;

namespace GRINGlobal.Client.Common
{
    public class UserSettings
    {
        private DataTable _userSettings;
        private WebServices _webServices;
        private string _cno = "";
        private string _app = "";

        public UserSettings(WebServices webServices, string cno, string app)
        {
            _userSettings = new DataTable();
            _webServices = webServices;
            _cno = cno;
            _app = app;
            Load();
        }

        public string this[string form, string resource, string key]
        {
            get
            {
                string selectRowsSQL = "cooperator_id='" + _cno.Replace("'", "''") + "'";
//selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
//selectRowsSQL += " AND form_name='" + form.Replace("'", "''") + "'";
                if (string.IsNullOrEmpty(_app))
                {
                    selectRowsSQL += " AND (app_name='" + _app.Replace("'", "''") + "' OR app_name IS NULL) ";
                }
                else
                {
                    selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
                }
                if (string.IsNullOrEmpty(form))
                {
                    selectRowsSQL += " AND (form_name='" + form.Replace("'", "''") + "' OR form_name IS NULL) ";
                }
                else
                {
                    selectRowsSQL += " AND form_name='" + form.Replace("'", "''") + "'";
                }
                selectRowsSQL += " AND resource_name='" + resource.Replace("'", "''") + "'";
                selectRowsSQL += " AND resource_key='" + key.Replace("'", "''") + "'";
                string resourceValue = "";
                try
                {
                    DataRow[] drs = _userSettings.Select(selectRowsSQL);
                    if (drs.Count() > 0)
                    {
                        // Use the first row retrieved that matches the search criteria...
                        resourceValue = drs[0]["resource_value"].ToString();
                    }
                }
                catch
                {
                }
                return resourceValue;
            }
            set
            {
                string selectRowsSQL = "cooperator_id='" + _cno.Replace("'", "''") + "'";
//selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
//selectRowsSQL += " AND form_name='" + form.Replace("'", "''") + "'";
                if (string.IsNullOrEmpty(_app))
                {
                    selectRowsSQL += " AND (app_name='" + _app.Replace("'", "''") + "' OR app_name IS NULL) ";
                }
                else
                {
                    selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
                }
                if (string.IsNullOrEmpty(form))
                {
                    selectRowsSQL += " AND (form_name='" + form.Replace("'", "''") + "' OR form_name IS NULL) ";
                }
                else
                {
                    selectRowsSQL += " AND form_name='" + form.Replace("'", "''") + "'";
                }
                selectRowsSQL += " AND resource_name='" + resource.Replace("'", "''") + "'";
                selectRowsSQL += " AND resource_key='" + key.Replace("'", "''") + "'";
                try
                {
                    DataRow[] drs = _userSettings.Select(selectRowsSQL);
                    if (drs.Count() > 0)
                    {
                        // Update the existing record...
                        if (drs[0]["resource_value"].ToString() != value.ToString())
                        {
                            drs[0]["resource_value"] = value;
                        }
                    }
                    else
                    {
                        // Create a new record...
                        DataRow dr = _userSettings.NewRow();
                        dr["cooperator_id"] = _cno;
                        dr["app_name"] = _app;
                        dr["form_name"] = form;
                        dr["resource_name"] = resource;
                        dr["resource_key"] = key;
                        dr["resource_value"] = value;
                        _userSettings.Rows.Add(dr);
                    }
                }
                catch
                {
                }
            }
        }

        public void Delete(string form, string resource, string key)
        {
            string selectRowsSQL = "cooperator_id='" + _cno.Replace("'", "''") + "'";
//selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
//selectRowsSQL += " AND form_name='" + form.Replace("'", "''") + "'";
            if (string.IsNullOrEmpty(_app))
            {
                selectRowsSQL += " AND (app_name='" + _app.Replace("'", "''") + "' OR app_name IS NULL) ";
            }
            else
            {
                selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
            }
            if (string.IsNullOrEmpty(form))
            {
                selectRowsSQL += " AND (form_name='" + form.Replace("'", "''") + "' OR form_name IS NULL) ";
            }
            else
            {
                selectRowsSQL += " AND form_name='" + form.Replace("'", "''") + "'";
            }
            selectRowsSQL += " AND resource_name='" + resource.Replace("'", "''") + "'";
            selectRowsSQL += " AND resource_key='" + key.Replace("'", "''") + "'";
            try
            {
                DataRow[] drs = _userSettings.Select(selectRowsSQL);

                foreach (DataRow dr in drs)
                {
                    dr.Delete();
                }
                //_userSettings.AcceptChanges();
            }
            catch
            {
            }
        }

        public void DeleteAll()
        {
            string selectRowsSQL = "cooperator_id='" + _cno.Replace("'", "''") + "'";
            //selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
            if (string.IsNullOrEmpty(_app))
            {
                selectRowsSQL += " AND (app_name='" + _app.Replace("'", "''") + "' OR app_name IS NULL) ";
            }
            else
            {
                selectRowsSQL += " AND app_name='" + _app.Replace("'", "''") + "'";
            }
            try
            {
                DataRow[] drs = _userSettings.Select(selectRowsSQL);

                foreach (DataRow dr in drs)
                {
                    dr.Delete();
                }
                //_userSettings.AcceptChanges();
            }
            catch
            {
            }
        }

        public void Load()
        {
            DataSet remoteDBUserSettings = new DataSet();
            // Get the user settings from the remote DB for the current user's CNO...
            remoteDBUserSettings = _webServices.GetData("get_user_settings", ":cooperatorid=" + _cno, 0, 0);
            if (remoteDBUserSettings.Tables.Contains("get_user_settings"))
            {
                _userSettings.Clear();
                remoteDBUserSettings.Tables["get_user_settings"].DefaultView.RowFilter = "app_name='" + _app.Replace("'", "''") + "'";
                _userSettings = remoteDBUserSettings.Tables["get_user_settings"].DefaultView.ToTable();
                _userSettings.AcceptChanges();
            }
            else if (remoteDBUserSettings.Tables.Contains("ExceptionTable") &&
                    remoteDBUserSettings.Tables["ExceptionTable"].Rows.Count > 0)
            {
//System.Windows.Forms.MessageBox.Show("There were errors retrieving user settings.\n\nFull error message:\n" + remoteDBUserSettings.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true, "");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors retrieving user settings.\n\nFull error message:\n{0}", "Load User Settings Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "UserSettings_LoadMessage1";
if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, remoteDBUserSettings.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
string[] argsArray = new string[100];
argsArray[0] = remoteDBUserSettings.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
ggMessageBox.ShowDialog();
            }
            // Clean up...
            remoteDBUserSettings.Dispose();
        }

        public void LoadXML(string userSettingsFileFullPath)
        {
            // NOTE: Did not use Application.UserAppDataPath.Replace(Application.ProductName + "\\" + Application.ProductVersion, "") + @"UserSettings.xml";
            // because it will automatically create the full AppDataPath (including product version like: Base Path\CompanyName\ProductName\ProductVersion
            // since we want to use the same UserSettings.xml file regardless of the version number we only want to use Base Path\CompanyName\UserSettings.xml
            //string userSettingsPath = settingsFilePath + @"\UserSettings.xml";
            DataSet remoteDBUserSettings = new DataSet();
            if (System.IO.File.Exists(userSettingsFileFullPath))
            {
                _userSettings.ReadXml(userSettingsFileFullPath);
            }
            else
            {
                ensureAppDataPathExists(userSettingsFileFullPath.Remove(userSettingsFileFullPath.LastIndexOf('\\')));
                // Get the user settings from the remote DB for the current user's CNO...
                //remoteDBUserSettings = _GUIWebServices.GetData(false, _username, _password, "get_user_settings", ":cooperatorid=" + _cno, 0, 0);
                remoteDBUserSettings = _webServices.GetData("get_user_settings", ":cooperatorid=" + _cno, 0, 0);
                _userSettings.Clear();
                _userSettings.Load(remoteDBUserSettings.Tables["get_user_settings"].CreateDataReader(), LoadOption.Upsert);
                // Save the data to an XML file on the local machine...
                _userSettings.WriteXml(userSettingsFileFullPath, XmlWriteMode.WriteSchema);
            }
        }

        public void Save()
        {
            DataSet modifiedData = new DataSet();
            DataSet saveErrors;
            //bool dataReloadNeeded = false;

            // Make a copy of the current user settings...
            DataTable currentUserSettings = _userSettings.Copy();
            // Reload the user settings from the remote database...
            Load();
            // Syncronize the current user settings with the settings retrieved from the remote DB...
            foreach (DataRow dr in currentUserSettings.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    Delete(dr["form_name", DataRowVersion.Original].ToString(), dr["resource_name", DataRowVersion.Original].ToString(), dr["resource_key", DataRowVersion.Original].ToString());
                }
                else
                {
                    this[dr["form_name"].ToString(), dr["resource_name"].ToString(), dr["resource_key"].ToString()] = dr["resource_value"].ToString();
                }
            }

            // Get just the rows that have changed and put them in to a new dataset...
            if (_userSettings.GetChanges() != null)
            {
                modifiedData.Tables.Add(_userSettings.GetChanges());
            }
            // Call the web method to update the syncronized user settings data...
            saveErrors = _webServices.SaveData(modifiedData);

            if (saveErrors != null &&
                saveErrors.Tables.Contains("ExceptionTable") &&
                saveErrors.Tables["ExceptionTable"].Rows.Count > 0)
            {
                //System.Windows.Forms.MessageBox.Show("There were errors saving user settings.\n\nFull error message:\n" + saveErrors.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
                SharedUtils sharedUtils = new SharedUtils(_webServices.Url, _webServices.Username, _webServices.Password_ClearText, true, "");
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors saving user settings.\n\nFull error message:\n{0}", "Save User Settings Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "UserSettings_SaveMessage1";
                if (sharedUtils != null && sharedUtils.IsConnected) sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
//if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, saveErrors.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
string[] argsArray = new string[100];
argsArray[0] = saveErrors.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, argsArray);
                ggMessageBox.ShowDialog();
            }
            else
            {
                Load();
            }
        }

        public void SaveXML(string userSettingsFileFullPath)
        {
            // NOTE: Did not use Application.UserAppDataPath.Replace(Application.ProductName + "\\" + Application.ProductVersion, "") + @"UserSettings.xml";
            // because it will automatically create the full AppDataPath (including product version like: Base Path\CompanyName\ProductName\ProductVersion
            // since we want to use the same UserSettings.xml file regardless of the version number we only want to use Base Path\CompanyName\UserSettings.xml
            ensureAppDataPathExists(userSettingsFileFullPath.Remove(userSettingsFileFullPath.LastIndexOf('\\')));
            _userSettings.WriteXml(userSettingsFileFullPath, XmlWriteMode.WriteSchema);
        }

        private void ensureAppDataPathExists(string appDataPath)
        {
            if (!System.IO.Directory.Exists(appDataPath))
            {
                System.IO.Directory.CreateDirectory(appDataPath);
            }
        }

    }
}