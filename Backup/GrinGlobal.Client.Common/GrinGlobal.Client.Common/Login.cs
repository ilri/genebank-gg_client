using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using GRINGlobal.Client.Common;

namespace GRINGlobal.Client.Common
{
    public partial class Login : Form
    {
        private WebServices _GRINGlobalWebServices;
        private string _userName = "";
        private string _password = "";
        private string _passwordClearText = "";
        private bool _hideServerList = false;
        private string _userCooperatorID = "";
        private string _userSite = "";
        private string _userLanguageCode = "";
        private string _selectedWebServiceURL = "";
        private Dictionary<string, string> _webServiceURLs;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string Password_ClearText
        {
            get
            {
                return _passwordClearText;
            }
            set
            {
                _passwordClearText = value;
            }
        }

        public bool HideServerList
        {
            get
            {
                return _hideServerList;
            }
            set
            {
                _hideServerList = value;
            }
        }

        public string UserCooperatorID
        {
            get
            {
                return _userCooperatorID;
            }
        }

        public string UserSite
        {
            get
            {
                return _userSite;
            }
        }

        public string UserLanguageCode
        {
            get
            {
                return _userLanguageCode;
            }
        }

        public string SelectedWebServiceURL
        {
            get
            {
                return _selectedWebServiceURL;
            }
        }

        public Dictionary<string, string> WebServiceURLs
        {
            get
            {
                return _webServiceURLs;
            }
        }

        public Login(string userName, string password, string preferredWebServiceURL, string webServiceHostsfileFullPath, bool hideServerList)
        {
            InitializeComponent();
            _userName = userName;
            _passwordClearText = password;
            _hideServerList = hideServerList;
            _selectedWebServiceURL = preferredWebServiceURL;
            try
            {
                _GRINGlobalWebServices = new WebServices(_selectedWebServiceURL, _userName, SharedUtils.SHA1EncryptionBase64(_passwordClearText), _passwordClearText, _userSite);
            }
            catch
            {
            }

            // First make sure the user's CT app data directory exists (and if not create it) just in case the CT was not installed under their name...
            if (!System.IO.Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\"))
            {
                System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\");
            }
            if (!System.IO.Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\"))
            {
                System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\");
            }
            
            if (!string.IsNullOrEmpty(webServiceHostsfileFullPath))
            {
                if (System.IO.File.Exists(webServiceHostsfileFullPath))
                {
                    _webServiceURLs = LoadWebServiceURLs(webServiceHostsfileFullPath);
                }
                else
                {
                    _webServiceURLs = new Dictionary<string, string>();
                    _webServiceURLs.Add("localhost", "http://localhost/GRINGlobal/GUI.asmx");
                }
            }
            else
            {
                // Now check to see if the WebServiceURL.txt file is there... 
                if (System.IO.File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\WebServiceURL.txt"))
                {
                    _webServiceURLs = LoadWebServiceURLs(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\WebServiceURL.txt");
                }
                // and if not look in the All Users app directory (built during installation)...
                else if (System.IO.File.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\WebServiceURL.txt"))
                {
                    System.IO.File.Copy(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\GRIN-Global\Curator Tool\WebServiceURL.txt",
                                        System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\WebServiceURL.txt", true);
                    _webServiceURLs = LoadWebServiceURLs(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\GRIN-Global\Curator Tool\WebServiceURL.txt");
                }
                // and if all else fails make one on the fly...
                else
                {
                    _webServiceURLs = new Dictionary<string, string>();
                    _webServiceURLs.Add("localhost", "http://localhost/GRINGlobal/GUI.asmx");
                }
            }
        }

        private Dictionary<string, string> LoadWebServiceURLs(string webServiceHostsfileFullPath)
        {
            Dictionary<string, string> WebServiceURLs = new Dictionary<string, string>();

            // Look for (and process) a WebServiceURL.txt file to allow custom web service server entries...
            if (System.IO.File.Exists(webServiceHostsfileFullPath))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(webServiceHostsfileFullPath);
                while (!sr.EndOfStream)
                {
                    string webServiceURL = sr.ReadLine();
                    if (!string.IsNullOrEmpty(webServiceURL.Trim()))
                    {
                        string[] keyValuePair = webServiceURL.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (keyValuePair != null)
                        {
                            if (keyValuePair.Length > 2)
                            {
                                // Working off the assumption that a URL does not contain spaces or tabs, concat all fields except the last into a single key...
                                for (int i = 1; i < keyValuePair.Length - 1; i++)
                                {
                                    keyValuePair[0] += " " + keyValuePair[i];
                                }
                                keyValuePair[1] = keyValuePair[keyValuePair.Length - 1];
                            }
                            else if (keyValuePair.Length < 2)
                            {
                                break;
                            }
                            if (!WebServiceURLs.ContainsKey(keyValuePair[0]))
                            {
                                WebServiceURLs.Add(keyValuePair[0], keyValuePair[1]);
                            }
                        }
                    }
                }
                sr.Close();
                sr.Dispose();
            }
            return WebServiceURLs;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            ux_buttonOk.Enabled = false;
            ux_buttonCancel.Enabled = true;
            this.AcceptButton = ux_buttonCancel;
            ux_textboxLoginMessage.Text = "Enter username and password, then click the 'OK' button to login to GRIN-Global.";
            ux_textboxLoginMessage.ForeColor = Color.Empty;
            // Leave these two lines last so that all text processing for the username and password are 
            // handled  normally when the dialog box first appears...
            ux_textboxUsername.Text = _userName;
//ux_textboxPassword.Text = _password;
ux_textboxPassword.Text = "";  // Blanked this out because the password is now SHA1 encrypted (and can't be reused in the dialog)...

            // Hide the server list combobox if the list is empty or retesting username/password credentials...
            if (_webServiceURLs == null || _hideServerList)
            {
                ux_comboboxServer.Hide();
                ux_labelServer.Hide();
                ux_buttonFindServers.Hide();
                ux_buttonChangePassword.Hide();
            }
            
            // Populate the server list combobox if a server list exists (it should or there are problems)...
            if(_webServiceURLs.Count > 0)
            {
                string tempURL = _selectedWebServiceURL;  // Cache the selected web server
                ux_comboboxServer.DisplayMember = "Key";
                ux_comboboxServer.ValueMember = "Value";
                ux_comboboxServer.DataSource = new BindingSource(_webServiceURLs, null);  // This line will auto choose the first web server in the list (and thus change _selectedWebServiceURL value)
                if(!string.IsNullOrEmpty(tempURL) && !tempURL.Equals(_selectedWebServiceURL)) _selectedWebServiceURL = tempURL;  // Restore the value of the _selecteWebServiceURL
            }
            ux_comboboxServer.SelectedValue = _selectedWebServiceURL;  // Set the selected Web Server URL...
        }

        private void ux_buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Update the settings for the webservice...
                _GRINGlobalWebServices.Url = _selectedWebServiceURL;
                _GRINGlobalWebServices.Username = ux_textboxUsername.Text;
                _GRINGlobalWebServices.Password = SharedUtils.SHA1EncryptionBase64(ux_textboxPassword.Text);

                // Attempt to login...
                DataSet userData = _GRINGlobalWebServices.ValidateLogin();
                if (userData != null &&
                    userData.Tables.Contains("validate_login") &&
                    userData.Tables["validate_login"].Rows.Count > 0)
                {
                    // Successful login...
                    _userName = ux_textboxUsername.Text;
_passwordClearText = ux_textboxPassword.Text;
                    _password = SharedUtils.SHA1EncryptionBase64(ux_textboxPassword.Text);
                    if (userData.Tables["validate_login"].Columns.Contains("cooperator_id")) _userCooperatorID = userData.Tables["validate_login"].Rows[0]["cooperator_id"].ToString();
                    if (userData.Tables["validate_login"].Columns.Contains("site_code")) _userSite = userData.Tables["validate_login"].Rows[0]["site_code"].ToString();
                    if (userData.Tables["validate_login"].Columns.Contains("site")) _userSite = userData.Tables["validate_login"].Rows[0]["site"].ToString();
                    if (userData.Tables["validate_login"].Columns.Contains("sec_lang_id")) _userLanguageCode = userData.Tables["validate_login"].Rows[0]["sec_lang_id"].ToString();
                    if (userData.Tables["validate_login"].Columns.Contains("sys_lang_id")) _userLanguageCode = userData.Tables["validate_login"].Rows[0]["sys_lang_id"].ToString();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (userData != null &&
                    userData.Tables.Contains("ExceptionTable") &&
                    userData.Tables["ExceptionTable"].Rows.Count > 0)

                {
                    ux_textboxLoginMessage.Text = userData.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
                    ux_textboxLoginMessage.ForeColor = Color.Red;
                    ux_textboxLoginMessage.Show();
                }
                else
                {
                    // No valid error messages coming back from the middle tier
                    ux_textboxLoginMessage.Text = "Error: Username/Password are not valid.  Please correct and try again.";
                    ux_textboxLoginMessage.ForeColor = Color.Red;
                    ux_textboxLoginMessage.Show();
                }
            }
            catch (Exception err)
            {
//MessageBox.Show("Error: The ValidateLogin web service is not functioning properly.  Please contact the system administrator", "Web Service Failure");
ux_textboxLoginMessage.ScrollBars = ScrollBars.Vertical;
ux_textboxLoginMessage.Text = "Web Service Error: " + err.Message;
ux_textboxLoginMessage.ForeColor = Color.Red;
ux_textboxLoginMessage.Show();
            }
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            _userName = "";
            _password = "";
            _userCooperatorID = "";
            _userSite = "";
            _userLanguageCode = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ux_textboxUsername_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxUsername.Text.Length > 0 && ux_textboxPassword.Text.Length > 0)
            {
                this.AcceptButton = ux_buttonOk;
                ux_buttonOk.Enabled = true;
                ux_textboxLoginMessage.Text = "";
                ux_textboxLoginMessage.ForeColor = Color.Empty;
            }
            else
            {
                this.AcceptButton = ux_buttonCancel;
                ux_buttonOk.Enabled = false;
                ux_textboxLoginMessage.Text = "Enter username and password, then click the 'OK' button to login to GRIN-Global.";
                ux_textboxLoginMessage.ForeColor = Color.Empty;
            }
        }

        private void ux_textboxPassword_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxUsername.Text.Length > 0 && ux_textboxPassword.Text.Length > 0)
            {
                this.AcceptButton = ux_buttonOk;
                ux_buttonOk.Enabled = true;
                ux_textboxLoginMessage.Text = "";
                ux_textboxLoginMessage.ForeColor = Color.Empty;
            }
            else
            {
                this.AcceptButton = ux_buttonCancel;
                ux_buttonOk.Enabled = false;
                ux_textboxLoginMessage.Text = "Enter username and password, then click the 'OK' button to login to GRIN-Global.";
                ux_textboxLoginMessage.ForeColor = Color.Empty;
            }
        }

        private void ux_comboboxServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ux_comboboxServer.SelectedValue != null) _selectedWebServiceURL = ux_comboboxServer.SelectedValue.ToString();
            if (_GRINGlobalWebServices != null)
            {
                _GRINGlobalWebServices.Url = _selectedWebServiceURL;
            }
        }

        private void ux_buttonChangePassword_Click(object sender, EventArgs e)
        {
            ChangePassword cp = new ChangePassword(ux_comboboxServer.SelectedValue.ToString(), ux_textboxUsername.Text, ux_textboxPassword.Text, null);
            cp.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == cp.ShowDialog(this))
            {
                ux_textboxPassword.Text = "";
            }
        }

        private void ux_buttonFindServers_Click(object sender, EventArgs e)
        {
            WebServicesLocator wsl = new WebServicesLocator(_webServiceURLs);
            wsl.StartPosition = FormStartPosition.CenterParent; 
            if (DialogResult.OK == wsl.ShowDialog(this))
            {
                ux_comboboxServer.DataSource = new BindingSource(_webServiceURLs, null);
            }
//SearchNetworkForWebServers(_webServiceURLs);
//if (_webServiceURLs != null && _webServiceURLs.Count > 0)
//{
//    ux_comboboxServer.DisplayMember = "Key";
//    ux_comboboxServer.ValueMember = "Value";
//    ux_comboboxServer.DataSource = new BindingSource(_webServiceURLs, null);
//}
        }

        private void SearchNetworkForWebServers(Dictionary<string, string> WebServiceURLs)
        {
            Cursor.Current = Cursors.WaitCursor;
            string origMessage = ux_textboxLoginMessage.Text;
            ux_textboxLoginMessage.Text = "Scanning...";
            ux_progressbarScanning.Step = 1;
            ux_progressbarScanning.Minimum = 0;
            ux_progressbarScanning.Maximum = 255;
            ux_progressbarScanning.Value = 1;
            ux_progressbarScanning.Show();

            System.Web.Services.Discovery.DiscoveryClientProtocol dcp = new System.Web.Services.Discovery.DiscoveryClientProtocol();
            dcp.Timeout = 1000;

            // Search for active servers hosting the GRIN-Global web services...
            // Try the localhost first...
            try
            {
                ux_textboxLoginMessage.Text = "Scanning localhost...";
                ux_textboxLoginMessage.Update();
                dcp.Discover("http://localhost/GRINGlobal/GUI.asmx");
                WebServiceURLs.Add("localhost", "http://localhost/GRINGlobal/GUI.asmx");
            }
            catch
            {
                // Silently fail...
            }


            // Now scan the class D subnet for all available ethernet adapters...
            System.Net.NetworkInformation.NetworkInterface[] adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            foreach (System.Net.NetworkInformation.NetworkInterface adapter in adapters)
            {
                if (adapter.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet &&
                    adapter.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                {
                    System.Net.NetworkInformation.IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    System.Net.NetworkInformation.UnicastIPAddressInformationCollection adapterAddresses = adapterProperties.UnicastAddresses;
                    foreach (System.Net.NetworkInformation.IPAddressInformation addressInfo in adapterAddresses)
                    {
                        DateTime dtStart = DateTime.Now;
                        string addrPrefix = addressInfo.Address.ToString().Substring(0, addressInfo.Address.ToString().LastIndexOf('.'));
                        string ipAddress = addrPrefix + ".1";
                        string webServiceURL = "http://" + ipAddress + "/GRINGlobal/GUI.asmx";
                        string serverName = System.Net.Dns.GetHostEntry(ipAddress).HostName;
                        System.Web.Services.Discovery.DiscoveryDocument dd = new System.Web.Services.Discovery.DiscoveryDocument();

                        for (int addrSuffix = 1; addrSuffix < 254; addrSuffix++)
                        {
                            try
                            {
                                // Now see if we can find a GG server...
                                ipAddress = addrPrefix + "." + addrSuffix;
                                // Display what IP address is being scanned...
                                ux_textboxLoginMessage.Text = "Scanning " + ipAddress + "...";
                                ux_textboxLoginMessage.Update();
                                // Attempt to discover the GG specific web services using the IP address...
                                dd = dcp.Discover("http://" + ipAddress + "/GRINGlobal/GUI.asmx");
                                // If we made it here, we found a webservice so now attempt to resolve the ipAddress to a friendlier host name 
                                // Why do this now?  Because DNS lookup can be expensive so only do it when really neccessary 
                                // (BTW... unresolved DNS lookups will return the ipAddress)...
                                serverName = System.Net.Dns.GetHostEntry(ipAddress).HostName;
                                webServiceURL = "http://" + serverName + "/GRINGlobal/GUI.asmx";
                                // Attempt to discover the GG specific web services (again - this time with a DNS resolved computer name)
                                // Why do this?  Because there is a slight possiblity the DNS lookup returned bogus data...
                                dd = dcp.Discover(webServiceURL);
                                for (int i = 0; i < dd.References.Count; i++)
                                {
                                    // There are many different references in a discovery document - we only want the SOAP Bindings...
                                    if (dd.References[i].GetType() == typeof(System.Web.Services.Discovery.SoapBinding))
                                    {
                                        System.Web.Services.Discovery.SoapBinding wsb = (System.Web.Services.Discovery.SoapBinding)dd.References[i];
                                        // The bad news is that the references have duplicates bindings (one for each protocol), but the good
                                        // news is that since the WebServiceURLs object is a dictionary... it will create and execption if the same Key entry is 
                                        // attempted to be added twice (net result is that it will silently fail in the catch block)...
                                        WebServiceURLs.Add(serverName, wsb.Address);
                                    }
                                }
                            }
                            catch
                            {
                                // Silently fail...
                            }
                            ux_progressbarScanning.PerformStep();
                            ux_progressbarScanning.Update();
                            // Check to see if we should bail out (at user request)...
                            Application.DoEvents();
                        }
                    }
                }
            }
            ux_progressbarScanning.Hide();
            ux_textboxLoginMessage.Text = origMessage;
        }
    }
}
