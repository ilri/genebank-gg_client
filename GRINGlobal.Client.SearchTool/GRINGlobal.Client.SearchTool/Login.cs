using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRINGlobal.Client.SearchTool
{
    public partial class Login : Form
    {
        //GRINGlobalGUIWebServices  http://grin-global-test1.agron.iastate.edu/gringlobal/gui.asmx
        private GRINGlobalGUIWebServices.GUI _guiWebServices;
        private string _userName = "";
        private string _password = "";
        private string _userCooperatorID = "";
        private string _userSite = "";
        private string _userLanguageCode = "";

        public string UserName
        {
            get
            {
                return _userName;
            }
        }

        public string Password
        {
            get
            {
                return _password;
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

        public Login(string username, string password, string webServiceURL)
        {
            InitializeComponent();

            _guiWebServices = new GRINGlobalGUIWebServices.GUI();
            _guiWebServices.Url = webServiceURL;
            _userName = username;
            _password = password;
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
            ux_textboxPassword.Text = _password;
        }

        private void ux_buttonOk_Click(object sender, EventArgs e)
        {
            DataSet userData = _guiWebServices.ValidateLogin(true, ux_textboxUsername.Text, ux_textboxPassword.Text);
            if (userData != null &&
                userData.Tables.Contains("validate_login") && 
                userData.Tables["validate_login"].Rows.Count > 0)
            {
                // Successful login...
                //_validLogin = true;
                _userName = ux_textboxUsername.Text;
                _password = ux_textboxPassword.Text;
                _userCooperatorID = userData.Tables["validate_login"].Rows[0]["cooperator_id"].ToString();
                _userSite = userData.Tables["validate_login"].Rows[0]["site_code"].ToString();
                _userLanguageCode = userData.Tables["validate_login"].Rows[0]["sec_lang_id"].ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                ux_textboxLoginMessage.Text = "Error: Username/Password are not valid.  Please correct and try again.";
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

    }
}
