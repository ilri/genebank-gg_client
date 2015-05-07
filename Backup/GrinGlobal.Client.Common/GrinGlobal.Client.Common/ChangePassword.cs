using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRINGlobal.Client.Common
{
    public partial class ChangePassword : Form
    {
        WebServices _webServices;
        string _webServiceURL;
        string _userName;
        string _password;
        string _site;
        SharedUtils _sharedUtils;

        public ChangePassword(string webServiceURL, string userName, string password, SharedUtils sharedUtils)
        {
            InitializeComponent();
            _webServiceURL = webServiceURL;
            _userName = userName;
            _password = password;
            _site = "";
            if (sharedUtils != null)
            {
                _sharedUtils = sharedUtils;
                _sharedUtils.UpdateControls(this.Controls, this.Name);
                _site = sharedUtils.UserSite;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
        }

        private void ux_buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                _webServices = new WebServices(_webServiceURL, ux_textboxUsername.Text, SharedUtils.SHA1EncryptionBase64(ux_textboxCurrentPassword.Text), ux_textboxCurrentPassword.Text, _site);
                DataSet userData = _webServices.ValidateLogin();
                if (userData != null &&
                    userData.Tables.Contains("validate_login") &&
                    userData.Tables["validate_login"].Rows.Count > 0)
                {
                    
                    DataSet newPasswordResults = _webServices.ChangePassword(SharedUtils.SHA1EncryptionBase64(ux_textboxNewPassword.Text));
                    if (newPasswordResults != null &&
                        newPasswordResults.Tables.Contains("ExceptionTable") &&
                        newPasswordResults.Tables["ExceptionTable"].Rows.Count == 0)
                    {
                        _password = ux_textboxNewPassword.Text;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else if (newPasswordResults != null &&
                        newPasswordResults.Tables.Contains("ExceptionTable") &&
                        newPasswordResults.Tables["ExceptionTable"].Rows.Count > 0)
                    {
//MessageBox.Show("There was an unexpected error changing your password.\n\nFull error message:\n" + newPasswordResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error changing your password.\n\nFull error message:\n{0}", "Change Password Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ChangePassword_ux_buttonOKMessage1";
if (_sharedUtils != null) _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, newPasswordResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
ggMessageBox.ShowDialog();
                    }
                    else
                    {
//MessageBox.Show("There was an unexpected error changing your password.");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error changing your password.", "Change Password Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ChangePassword_ux_buttonOKMessage2";
if (_sharedUtils != null) _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                    }
                }
                else
                {
//MessageBox.Show("Error: The Username/Password are not valid for server:\n\n" + _webServiceURL + "\n\nPlease correct and try again.");
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error: The Username/Password are not valid for server:\n\n{0}\n\nPlease correct and try again.", "Invalid Username or Password", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ChangePassword_ux_buttonOKMessage3";
if (_sharedUtils != null) _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, _webServiceURL);
ggMessageBox.ShowDialog();
                }
            }
            catch
            {
//MessageBox.Show("Error connecting to server: " + _webServiceURL);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error connecting to server: {0}", "Server Connection Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ChangePassword_ux_buttonOKMessage4";
if (_sharedUtils != null) _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, _webServiceURL);
ggMessageBox.ShowDialog();
            }
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ux_textboxUsername_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxUsername.Text.Length > 0 &&
                ux_textboxCurrentPassword.Text.Length > 0 &&
                ux_textboxNewPassword.Text.Length > 0)
            {
                ux_buttonOk.Enabled = true;
            }
            else
            {
                ux_buttonOk.Enabled = false;
            }
        }

        private void ux_textboxCurrentPassword_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxUsername.Text.Length > 0 &&
                ux_textboxCurrentPassword.Text.Length > 0 &&
                ux_textboxNewPassword.Text.Length > 0)
            {
                ux_buttonOk.Enabled = true;
            }
            else
            {
                ux_buttonOk.Enabled = false;
            }
        }

        private void ux_textboxNewPassword_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxUsername.Text.Length > 0 &&
                ux_textboxCurrentPassword.Text.Length > 0 &&
                ux_textboxNewPassword.Text.Length > 0)
            {
                ux_buttonOk.Enabled = true;
            }
            else
            {
                ux_buttonOk.Enabled = false;
            }
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            ux_textboxUsername.Text = _userName;
            ux_textboxCurrentPassword.Text = _password;
        }
    }
}
