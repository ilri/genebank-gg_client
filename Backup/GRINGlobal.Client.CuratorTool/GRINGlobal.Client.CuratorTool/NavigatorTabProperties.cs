using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GRINGlobal.Client
{
    public partial class NavigatorTabProperties : Form
    {
        private string _tabText = "";
        private string _invalidString = "";
        private string _replacementString = "";

        public NavigatorTabProperties(string invalidString, string replacementString, GRINGlobal.Client.Common.SharedUtils sharedUtils)
        {
            _invalidString = invalidString;
            _replacementString = replacementString;
            InitializeComponent();
sharedUtils.UpdateControls(this.Controls, this.Name);
        }

        private void NavigatorTabProperties_Load(object sender, EventArgs e)
        {
            ux_buttonOK.Enabled = false;
            this.AcceptButton = ux_buttonOK;
            this.CancelButton = ux_buttonCancel;
        }    

        public string TabText 
        {
            get
            {
                return _tabText;
            }

            set
            {
                _tabText = value;
                ux_textboxTabText.Text = _tabText;
            }
        }

        private void ux_buttonOK_Click(object sender, EventArgs e)
        {
            _tabText = ux_textboxTabText.Text.Trim();
//_tabText = ux_textboxTabText.Text.Trim().Replace(' ', '_');
            _tabText = ux_textboxTabText.Text.Replace(_invalidString, _replacementString);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ux_textboxTabText_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxTabText.Text.Length > 0)
            {
                ux_buttonOK.Enabled = true;
            }
            else
            {
                ux_buttonOK.Enabled = false;
            }
        }
    }
}
