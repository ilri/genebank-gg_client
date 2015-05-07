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
    public partial class WebServicesLocator : Form
    {
        Dictionary<string, string> _origWebServiceURLs;
        DataTable _webServiceURLs;
        BindingSource _webServiceURLsBindingSource;

        public WebServicesLocator(Dictionary<string, string> WebServiceURLs)
        {
            InitializeComponent();
            _origWebServiceURLs = WebServiceURLs;
            _webServiceURLs = new DataTable();
            _webServiceURLs.Columns.Add("display_member", typeof(string));
            _webServiceURLs.Columns.Add("value_member", typeof(string));
            foreach (string key in WebServiceURLs.Keys)
            {
                DataRow newURL = _webServiceURLs.NewRow();
                newURL["value_member"] = WebServiceURLs[key].ToString().Replace("http://", "").Replace("/GRINGlobal/GUI.asmx", "").Trim();
                newURL["display_member"] = key.ToString();
                _webServiceURLs.Rows.Add(newURL);
            }
            ux_listboxWebserviceURLNames.DisplayMember = "display_member";
            ux_listboxWebserviceURLNames.ValueMember = "value_member";
            _webServiceURLsBindingSource = new BindingSource(_webServiceURLs, null);
            ux_listboxWebserviceURLNames.DataSource = _webServiceURLsBindingSource;
            ux_textboxListName.DataBindings.Add("Text", _webServiceURLsBindingSource, "display_member");
            ux_textboxServerName.DataBindings.Add("Text", _webServiceURLsBindingSource, "value_member");
        }

        private void ux_buttonOK_Click(object sender, EventArgs e)
        {
            _origWebServiceURLs.Clear();
            foreach (DataRow dr in _webServiceURLs.Rows)
            {
                _origWebServiceURLs.Add(dr["display_member"].ToString(), "http://" + dr["value_member"].ToString() + "/GRINGlobal/GUI.asmx");
            }
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
        }

        private void ux_buttonTestServer_Click(object sender, EventArgs e)
        {
            System.Web.Services.Discovery.DiscoveryClientProtocol dcp = new System.Web.Services.Discovery.DiscoveryClientProtocol();
            dcp.Timeout = 1000;
            dcp.AllowAutoRedirect = true;

            // Test the address for the GRIN-Global web services...
            try
            {
                string serverAddress = "http://" + ux_textboxServerName.Text.Trim() + "/GRINGlobal/GUI.asmx";
                dcp.Discover(serverAddress);
                MessageBox.Show("Successfully connected!");
                dcp.Dispose();
            }
            catch
            {
                MessageBox.Show("Connection attempt failed");
                dcp.Dispose();
            }
        }

        private void ux_buttonAddNewListItem_Click(object sender, EventArgs e)
        {
            string serverAddress = "localhost";
            DataRow newURL = _webServiceURLs.NewRow();
            newURL["value_member"] = serverAddress;
            newURL["display_member"] = ux_groupboxListName.Text.Trim();
            _webServiceURLs.Rows.Add(newURL);
            ux_listboxWebserviceURLNames.SelectedIndex = ux_listboxWebserviceURLNames.Items.Count - 1;
            ux_textboxListName.Focus();
            ux_textboxListName.SelectAll();
        }

        private void ux_buttonDeleteListItem_Click(object sender, EventArgs e)
        {
            _webServiceURLs.Rows[_webServiceURLsBindingSource.Position].Delete();
        }

        private void ux_buttonMoveUp_Click(object sender, EventArgs e)
        {
            if (_webServiceURLsBindingSource.Position > 0)
            {
                int currentIndex = _webServiceURLsBindingSource.Position;
                string tempDisplayMember = _webServiceURLs.Rows[currentIndex]["display_member"].ToString();
                string tempValueMember = _webServiceURLs.Rows[currentIndex]["value_member"].ToString();
                _webServiceURLs.Rows[currentIndex]["display_member"] = _webServiceURLs.Rows[currentIndex - 1]["display_member"];
                _webServiceURLs.Rows[currentIndex]["value_member"] = _webServiceURLs.Rows[currentIndex - 1]["value_member"];
                _webServiceURLs.Rows[currentIndex - 1]["display_member"] = tempDisplayMember;
                _webServiceURLs.Rows[currentIndex - 1]["value_member"] = tempValueMember;
                _webServiceURLsBindingSource.MovePrevious();
            }
        }

        private void ux_buttonMoveDown_Click(object sender, EventArgs e)
        {
            if (_webServiceURLsBindingSource.Position < (_webServiceURLsBindingSource.Count - 1))
            {
                int currentIndex = _webServiceURLsBindingSource.Position;
                string tempDisplayMember = _webServiceURLs.Rows[currentIndex]["display_member"].ToString();
                string tempValueMember = _webServiceURLs.Rows[currentIndex]["value_member"].ToString();
                _webServiceURLs.Rows[currentIndex]["display_member"] = _webServiceURLs.Rows[currentIndex + 1]["display_member"];
                _webServiceURLs.Rows[currentIndex]["value_member"] = _webServiceURLs.Rows[currentIndex + 1]["value_member"];
                _webServiceURLs.Rows[currentIndex + 1]["display_member"] = tempDisplayMember;
                _webServiceURLs.Rows[currentIndex + 1]["value_member"] = tempValueMember;
                _webServiceURLsBindingSource.MoveNext();
            }
        }

        private void ux_textboxListName_TextChanged(object sender, EventArgs e)
        {
            ((DataRowView)_webServiceURLsBindingSource.Current)["display_member"] = ux_textboxListName.Text;
        }
    }
}
