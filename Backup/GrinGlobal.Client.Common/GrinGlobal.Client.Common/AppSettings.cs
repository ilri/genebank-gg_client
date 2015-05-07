using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GRINGlobal.Client.Common
{
    class AppSettings
    {
        private DataTable _appResources;
        private DataTable _appSettings;
        private WebServices _webServices;
        private int _lang = 1;
        private string _appName = "GRINGlobalClientCuratorTool";

        public AppSettings(WebServices webServices, int lang, string appName)
        {
            _appResources = new DataTable();
            _webServices = webServices;
            if (lang != null) _lang = lang;
            if (!string.IsNullOrEmpty(appName)) _appName = appName;
            Load();
        }

        public void Load()
        {
            DataSet remoteDBData = new DataSet();
            
            // Get the user resources from the remote DB for the current user's chosen language...
            remoteDBData = _webServices.GetData("get_app_resource", ":appname=" + _appName + "; :syslangid=" + _lang.ToString() + ";", 0, 0);
            if (remoteDBData.Tables.Contains("get_app_resource"))
            {
                if(_appResources != null) _appResources.Clear();
                _appResources = remoteDBData.Tables["get_app_resource"].Copy();
                _appResources.DefaultView.RowFilter = "sys_lang_id=" + _lang.ToString();
            }

            // Get the app settings from the remote DB...
            remoteDBData = _webServices.GetData("get_app_setting", "", 0, 0);
            if (remoteDBData.Tables.Contains("get_app_setting"))
            {
                if(_appSettings != null) _appSettings.Clear();
                _appSettings = remoteDBData.Tables["get_app_setting"].Copy();
                
                // Now load any local app setting stored on the users local drive...
                Dictionary<string, string> localAppSettings = LoadLocalAppSettings();
                foreach(string name in localAppSettings.Keys)
                {
                    DataRow[] drs = _appSettings.DefaultView.ToTable().Select("name='" + name + "'");
                    if (drs.Length > 0)
                    {
                        foreach (DataRow dr in drs)
                        {
                            dr["value"] = localAppSettings[name];
                        }
                    }
                    else
                    {
                        DataRow newAppSetting = _appSettings.NewRow();
                        newAppSetting["name"] = name;
                        newAppSetting["value"] = localAppSettings[name];
                        _appSettings.Rows.Add(newAppSetting);
                    }
                }
            }

            // Clean up...
            remoteDBData.Dispose();
        }

        private Dictionary<string, string> LoadLocalAppSettings()
        {
            string usersAppSettingsFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\AppSettings.txt";
            string allUsersAppSettingsFilePath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\AppSettings.txt";
            string appSettingsFilePath = "";

            Dictionary<string, string> localAppSettings = new Dictionary<string, string>();

            // Look for (and process) a AppSettings.txt file to allow custom application settings entries...
            // But first make sure the roaming profile directory exists...
            if (!System.IO.Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool")) System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool");

            // Now check the user's personal roaming profile for a copy of the AppSetting.txt file...
            if (System.IO.File.Exists(usersAppSettingsFilePath))
            {
                // Found a personal user's copy of the AppSettings.txt file - so use it...
                appSettingsFilePath = usersAppSettingsFilePath;
            }
            else if (System.IO.File.Exists(allUsersAppSettingsFilePath))
            {
                // Couldn't find a copy of the AppSettings.txt file so copy the master into the user's personal roaming profile AppData directory...
                System.IO.File.Copy(allUsersAppSettingsFilePath, usersAppSettingsFilePath);
                appSettingsFilePath = usersAppSettingsFilePath;
            }

            // Process the AppSettings.txt file...
            if (!string.IsNullOrEmpty(appSettingsFilePath))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(appSettingsFilePath);
                while (!sr.EndOfStream)
                {
                    string localAppSetting = sr.ReadLine();
                    if (!string.IsNullOrEmpty(localAppSetting.Trim()) &&
                        !localAppSetting.Trim().StartsWith("#"))
                    {
                        string[] keyValuePair = localAppSetting.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        if (keyValuePair != null)
                        {
                            if (keyValuePair.Length > 2)
                            {
                                // Working off the assumption that the first '=' sign found is the delimiter for the key/value pair so concat all fields after the first '=' sign...
                                for (int i = 2; i < keyValuePair.Length; i++)
                                {
                                    keyValuePair[1] += "=" + keyValuePair[i];
                                }
                            }
                            else if (keyValuePair.Length < 2)
                            {
                                break;
                            }
                            // Update or Add the new App Setting...
                            if (!localAppSettings.ContainsKey(keyValuePair[0].Trim()))
                            {
                                // This setting does not exist so Add it...
                                localAppSettings.Add(keyValuePair[0].Trim(), keyValuePair[1].Trim());
                            }
                            else
                            {
                                // This setting is already in the dictionary so update it's value...
                                localAppSettings[keyValuePair[0].Trim()] = keyValuePair[1].Trim();
                            }
                        }
                    }
                }
                sr.Close();
                sr.Dispose();
            }
            return localAppSettings;
        }

        public void UpdateControls(System.Windows.Forms.Control.ControlCollection ctrlCollection, string formName)
        {
            // Set the row filter based on the Form's name and the lang...
            _appResources.DefaultView.RowFilter = "sys_lang_id=" + _lang.ToString() + " AND form_name='" + formName + "'";
// If this is the client window - change the title bar...
//if (ctrlCollection.Owner.GetType() == typeof(GRINGlobal.Client.CuratorTool.GRINGlobalClientCuratorTool))
//if (ctrlCollection.Owner.GetType().FullName == "GRINGlobal.Client.CuratorTool.GRINGlobalClientCuratorTool")
            {
                //this.Text = "GRIN-Global  v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                DataRow[] title = _appResources.DefaultView.ToTable().Select("app_resource_name='Text'");
                if (title != null && 
                    title.Length > 0)
                {
                    if (title[0].Table.Columns.Contains("display_member"))
                    {
                        if (title[0]["display_member"].ToString().Contains("{0}"))
                        {
                            ctrlCollection.Owner.Text = string.Format(title[0]["display_member"].ToString(), System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
                        }
                        else
                        {
                            ctrlCollection.Owner.Text = title[0]["display_member"].ToString() + " v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                        }
                    }
                    else if (!ctrlCollection.Owner.Text.Contains(System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()))
                    {
                        ctrlCollection.Owner.Text += " v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                    }
                }
                else
                {
                    if (!ctrlCollection.Owner.Text.Contains(System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()))
                    {
                        ctrlCollection.Owner.Text = "GRIN-Global  v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                    }
                }
            }

            // Iterate through the Form's controls...
            foreach (DataRowView drv in _appResources.DefaultView)
            {
                System.Windows.Forms.Control[] ctrlArray = ctrlCollection.Find(drv["app_resource_name"].ToString(), true);
                if (ctrlArray != null && ctrlArray.Length > 0)
                {
                    foreach (System.Windows.Forms.Control ctrl in ctrlArray)
                    {
                        ctrl.Text = drv["display_member"].ToString();
                    }
                }
            }

            // Iterate through the Form's toolstrips...
            foreach (System.Windows.Forms.Control ctrl in ctrlCollection)
            {
                if (ctrl is System.Windows.Forms.ToolStrip)
                {
                    System.Windows.Forms.ToolStrip ts = (System.Windows.Forms.ToolStrip)ctrl;
                    foreach (DataRowView drv in _appResources.DefaultView)
                    {
                        System.Windows.Forms.ToolStripItem[] tsiArray = ts.Items.Find(drv["app_resource_name"].ToString(), true);
                        if (tsiArray != null && tsiArray.Length > 0)
                        {
                            foreach (System.Windows.Forms.ToolStripItem tsi in tsiArray)
                            {
                                tsi.Text = drv["display_member"].ToString();
                            }
                        }
                    }
                }
            }
        }

        public void UpdateComponents(System.ComponentModel.ComponentCollection compCollection, string formName)
        {
            // Set the row filter based on the Form's name and the lang...
            _appResources.DefaultView.RowFilter = "sys_lang_id=" + _lang.ToString() + " AND form_name='" + formName + "'";
            // Iterate through the Forms context menus...
            foreach (System.ComponentModel.Component comp in compCollection)
            {
                if (comp is System.Windows.Forms.ContextMenuStrip)
                {
                    System.Windows.Forms.ContextMenuStrip cms = (System.Windows.Forms.ContextMenuStrip)comp;
                    foreach (DataRowView drv in _appResources.DefaultView)
                    {
                        System.Windows.Forms.ToolStripItem[] tsiArray = cms.Items.Find(drv["app_resource_name"].ToString(), true);
                        if (tsiArray != null && tsiArray.Length > 0)
                        {
                            foreach (System.Windows.Forms.ToolStripItem tsi in tsiArray)
                            {
                                tsi.Text = drv["display_member"].ToString();
                            }
                        }
                    }
                }
            }
        }

        public string GetAppSettingValue(string name)
        {
            string value = "";
            if (_appSettings != null && _appSettings.DefaultView.Count > 0)
            {
                DataRow[] drs = _appSettings.DefaultView.ToTable().Select("name='" + name + "'");
                if (drs != null && drs.Length > 0)
                {
                    value = drs[0]["value"].ToString();
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                switch (name.Trim().ToUpper())
                {
                    // Folder Properties...
                    case "SORT_MODE":
                        value = "MANUAL";
                        break;
                    case "FOLDER_GROUPING_MODE":
                        value = "TOP";
                        break;
                    // Friendly Name Formulas...
                    case "ACCESSION_ID_NAME_FORMULA":
                        value = "{get_accession.accession_number_part1} + \" \" + {get_accession.accession_number_part2} + \" \" + {get_accession.accession_number_part3}";
                        break;
                    case "INVENTORY_ID_NAME_FORMULA":
                        value = "{get_inventory.inventory_number_part1} + \" \" + {get_inventory.inventory_number_part2} + \" \" + {get_inventory.inventory_number_part3} + \" \" + {get_inventory.form_type_code}";
                        break;
                    case "ORDER_REQUEST_ID_NAME_FORMULA":
                        value = "{get_order_request.order_request_id}";
                        break;
                    case "ORDER_REQUEST_ITEM_ID_NAME_FORMULA":
                        value = "{get_order_request_item.sequence_number} + \" - \" + {get_order_request_item.name}";
                        break;
                    case "COOPERATOR_ID_NAME_FORMULA":
                        value = "{get_cooperator.last_name} + \", \" + {get_cooperator.first_name} + \" \" + {get_cooperator.organization_abbrev}";
                        break;
                    case "TAXONOMY_GENUS_ID_NAME_FORMULA":
                        value = "{get_taxonomy_genus.genus_name} + \" \" + {get_taxonomy_genus.subgenus_name} + \" \" + {get_taxonomy_genus.section_name} + \" \" + {get_taxonomy_genus.subsection_name}";
                        break;
                    case "GEOGRAPHY_ID_NAME_FORMULA":
                        value = "{get_geography.country_code} + \" \" + {get_geography.adm1}";
                        break;
                    case "CROP_ID_NAME_FORMULA":
                        value = "{get_crop.name}";
                        break;
                    case "CROP_TRAIT_ID_NAME_FORMULA":
                        value = "{get_crop_trait.title}";
                        break;
                    // Virtual Node Dataviews...
                    case "ACCESSION_ID_VIRTUAL_NODE_DATAVIEW":
                        value = "get_inventory";
                        break;
                    case "ORDER_REQUEST_ID_VIRTUAL_NODE_DATAVIEW":
                        value = "get_order_request_item";
                        break;
                    case "CROP_ID_VIRTUAL_NODE_DATAVIEW":
                        value = "get_crop_trait";
                        break;
                    default:
                        break;
                }
            }
            return value;
        }
    }
}
