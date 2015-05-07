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
    public partial class TreeviewNodeProperties : Form
    {
        SharedUtils _sharedUtils;
        TreeNode _currentNode;
        string _selectedObjectType = "ACCESSION_ID";
        string _newProperties;

        public TreeviewNodeProperties(SharedUtils sharedUtils, TreeNode currentNode)
        {
            if (sharedUtils != null)
            {
                _sharedUtils = sharedUtils;

                if (currentNode != null)
                {
                    // Remember the current node passed into this dialog...
                    _currentNode = currentNode;
                    _newProperties = _currentNode.Tag.ToString();
                }
                else
                {
                    return;
                }
            }
            InitializeComponent();
        }

        private void TreeviewNodeProperties_Load(object sender, EventArgs e)
        {
            // Set default values for the node properties...
            // First the sorting mode...
            if (GetTreeviewNodeProperty("SORT_MODE", _currentNode) == "ASCENDING")
            {
                ux_radiobuttonSortAscending.Checked = true;
            }
            else if (GetTreeviewNodeProperty("SORT_MODE", _currentNode) == "DESCENDING")
            {
                ux_radiobuttonSortDescending.Checked = true;
            }
            else
            {
                ux_radiobuttonSortManual.Checked = true;
            }
            // Next folder grouping mode...
            if (GetTreeviewNodeProperty("FOLDER_GROUPING_MODE", _currentNode) == "BOTTOM")
            {
                ux_radiobuttonFoldersAtBottom.Checked = true;
            }
            else
            {
                ux_radiobuttonFoldersAtTop.Checked = true;
            }
            // Next ACCESSION_ID naming...
            ux_radiobuttonACCESSION_IDNamingOptions.Checked = true;
            ux_textboxNodeNameFormula.Text = GetTreeviewNodeProperty("ACCESSION_ID_NAME_FORMULA", _currentNode);
            if (_currentNode.Tag.ToString().Contains("ACCESSION_ID_NAME_FORMULA"))
            {
                // Found a setting for this property - flag as custom...
                ux_radiobuttonCustomName.Checked = true;
            }
            else
            {
                // Did not find a setting for this property - flag as inherited...
                ux_radiobuttonDefaultName.Checked = true;
            }

            // Next dynamic folder Resove To...
            switch (GetTreeviewNodeProperty("DYNAMIC_FOLDER_RESOLVE_TO", _currentNode).Trim().ToUpper())
            {
                case "ACCESSION":
                    ux_radiobuttonResolveToACCESSION_ID.Checked = true;
                    break;
                case "INVENTORY":
                    ux_radiobuttonResolveToINVENTORY_ID.Checked = true;
                    break;
                case "ORDER_REQUEST":
                    ux_radiobuttonResolveToORDER_REQUEST_ID.Checked = true;
                    break;
                case "COOPERATOR":
                    ux_radiobuttonResolveToCOOPERATOR_ID.Checked = true;
                    break;
                default:
                    ux_radiobuttonResolveToDefault.Checked = true;
                    break;
            }

            //string returnedProperties = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=" + GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", _currentNode) + "; ";
            // Finally DYNAMIC_FOLDER_SEARCH_CRITERIA...
            ux_textboxDYNAMIC_FOLDER_SEARCH_CRITERIA.Text = GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", _currentNode);

            _sharedUtils.UpdateControls(this.Controls, this.Name);
        }

        private string GetTreeviewNodeProperty(string propertyName, TreeNode currentNode)
        {
            string value = "";
            if (currentNode.Tag != null)
            {
                // Break the tag (properties) up into individual property tokens...
                string[] properties = currentNode.Tag.ToString().Split(';');
                foreach (string property in properties)
                {
                    // If you find the token that contains the property name - retrieve the value
                    if (property.Contains(propertyName))
                    {
                        // Split it into the key/value pair...
                        string[] keyValuePair = property.Split('=');
                        if (keyValuePair.Length == 2 &&
                            keyValuePair[0].Trim() == propertyName.Trim())
                        {
                            value = keyValuePair[1];
                        }
                        else if (keyValuePair.Length > 2 &&
                            keyValuePair[0].Trim() == propertyName.Trim())
                        {
                            // The string has more than 1 '=' character in it so use
                            // the first '=' as the delimiter for the key/value pair
                            // (which will inlcude all remaining '=' chars in the value string
                            value = property.Substring(property.IndexOf('=') + 1);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                if (currentNode.Parent != null)
                {
                    //  Didn't find the property - look at the parent's properties...
                    value = GetTreeviewNodeProperty(propertyName, currentNode.Parent);
                }
                else
                {
                    // There is no setting for this property in the tree - get it from the app...
                    value = _sharedUtils.GetAppSettingValue(propertyName);
                }
            }
            return value;
        }

        private void ux_buttonOK_Click(object sender, EventArgs e)
        {
//string returnedProperties = "FOLDER; DYNAMIC_FOLDER_SEARCH_CRITERIA=" + GetTreeviewNodeProperty("DYNAMIC_FOLDER_SEARCH_CRITERIA", _currentNode) + "; ";
//string returnedProperties = "FOLDER; DYNAMIC_FOLDER_RESOLVE_TO=default; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
//if (ux_textboxDYNAMIC_FOLDER_SEARCH_CRITERIA.Text.Length > 0)
//{
//    returnedProperties = "QUERY; DYNAMIC_FOLDER_SEARCH_CRITERIA=" + ux_textboxDYNAMIC_FOLDER_SEARCH_CRITERIA.Text + "; ";
//}
string returnedProperties = "";
// Calculate the Resolve To value...
string resolveTo = "default";
if (ux_radiobuttonResolveToACCESSION_ID.Checked) resolveTo = "accession";
if (ux_radiobuttonResolveToINVENTORY_ID.Checked) resolveTo = "inventory";
if (ux_radiobuttonResolveToORDER_REQUEST_ID.Checked) resolveTo = "order_request";
if (ux_radiobuttonResolveToCOOPERATOR_ID.Checked) resolveTo = "cooperator";
if (ux_textboxDYNAMIC_FOLDER_SEARCH_CRITERIA.Text.Length > 0)
{
    returnedProperties += "QUERY; DYNAMIC_FOLDER_RESOLVE_TO=" + resolveTo + "; DYNAMIC_FOLDER_SEARCH_CRITERIA=" + ux_textboxDYNAMIC_FOLDER_SEARCH_CRITERIA.Text + "; ";
}
else
{
    returnedProperties += "FOLDER; DYNAMIC_FOLDER_RESOLVE_TO=" + resolveTo + "; DYNAMIC_FOLDER_SEARCH_CRITERIA=; ";
}


            if (ux_radiobuttonSortAscending.Checked) returnedProperties += "SORT_MODE=ASCENDING; ";
            else if (ux_radiobuttonSortDescending.Checked) returnedProperties += "SORT_MODE=DESCENDING; ";
            else returnedProperties += "SORT_MODE=MANUAL; ";
            if (ux_radiobuttonFoldersAtBottom.Checked) returnedProperties += "FOLDER_GROUPING_MODE=BOTTOM; ";
            else returnedProperties += "FOLDER_GROUPING_MODE=TOP; ";
            // Find the NodeType radio button that is checked and toggle it (to save it's settings to _newProperties)...
            foreach (Control ctrl in ux_groupboxNamingOptions.Controls)
            {
                if (ctrl.GetType() == typeof(RadioButton))
                {
                    RadioButton rb = (RadioButton)ctrl;
                    if (rb.Checked)
                    {
                        rb.Checked = false;
                        rb.Checked = true;
                    }
                }
            }
            // Iterate through the _NAME_FORMULAs...
            string[] propertyTokens = _newProperties.Split(';');
            foreach (string propertyToken in propertyTokens)
            {
                if (propertyToken.Contains("_NAME_FORMULA"))
                {
                    returnedProperties += propertyToken + "; ";
                }
            }

            _currentNode.Tag = returnedProperties;
            this.Close();
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            // Revert everything back to the original settings for this node...
            //_newProperties = _userItemListRow["properties"].ToString();
            this.Close();
        }

        private void ux_radiobuttonSortAscending_CheckedChanged(object sender, EventArgs e)
        {
            ux_panelFolderGroupingOptions.Visible = true;
        }

        private void ux_radiobuttonSortDescending_CheckedChanged(object sender, EventArgs e)
        {
            ux_panelFolderGroupingOptions.Visible = true;
        }

        private void ux_radiobuttonSortManual_CheckedChanged(object sender, EventArgs e)
        {
            ux_panelFolderGroupingOptions.Visible = false;
        }

        private void ux_radiobuttonNodeTypeNamingOptions_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            string propertyName = rb.Tag.ToString().Trim().ToUpper() + "_NAME_FORMULA";

            if (rb.Checked)
            {
                _selectedObjectType = rb.Tag.ToString().Trim().ToUpper();
                // This is the 'newly selected' radio button - so populate the controls...
                if (_newProperties.Contains(propertyName))
                {
                    // Un-check the radio button to force the checked_changed event to always fire...
                    ux_radiobuttonCustomName.Checked = false;
                    ux_radiobuttonCustomName.Checked = true;
                }
                else
                {
                    // Un-check the radio button to force the checked_changed event to always fire...
                    ux_radiobuttonDefaultName.Checked = false;
                    ux_radiobuttonDefaultName.Checked = true;
                }
            }
            else
            {
                // This is the radio button losing selection - so store the settings now...
                if (ux_radiobuttonDefaultName.Checked ||
                    ux_textboxNodeNameFormula.Text.Length == 0)
                {
                    if (_newProperties.Contains(propertyName))
                    {
                        // There was an existing custom setting for this node type, but now 
                        // the default setting has been chosen - remove the setting so that this node 
                        // inherits settings for this node type from an ancestor...
                        // Find the start of the key/value pair...
                        int startIndex = _newProperties.IndexOf(propertyName);
                        // Find the end of the key/value pair...
                        int stringLength = Math.Min(_newProperties.IndexOf(";", startIndex) + 2, _newProperties.Length) - startIndex;
                        _newProperties = _newProperties.Remove(startIndex, stringLength);
                    }
                    else
                    {
                        // There was no previous setting for this node type,
                        // and that is still the case - so do nothing here...
                    }
                }
                else
                {
                    if (_newProperties.Contains(propertyName))
                    {
                        // This node type has a custom name setting and there was a previous setting
                        // so replace the previous setting with this new one...
                        // Find the start of the key/value pair...
                        int startIndex = _newProperties.IndexOf(propertyName);
                        // Find the end of the key/value pair...
                        int stringLength = Math.Min(_newProperties.IndexOf(";", startIndex) + 2, _newProperties.Length) - startIndex;
                        string oldKeyValuePair = _newProperties.Substring(startIndex, stringLength);
                        if (ux_textboxNodeNameFormula.Text.Length > 0)
                        {
                            _newProperties = _newProperties.Replace(oldKeyValuePair, propertyName + "=" + ux_textboxNodeNameFormula.Text + "; ");
                        }
                        else
                        {
                            _newProperties = _newProperties.Remove(startIndex, stringLength);
                        }
                    }
                    else
                    {
                        // This node type has a custom name setting but there was no previous setting
                        // so just add the new setting...
                        _newProperties += propertyName + "=" + ux_textboxNodeNameFormula.Text + "; ";
                    }
                }
            }
        }

        private void ux_radiobuttonDefaultName_CheckedChanged(object sender, EventArgs e)
        {
            if (ux_radiobuttonDefaultName.Checked)
            {
                ux_groupboxNameBuilder.Enabled = false;
                if (_currentNode.Parent != null)
                {
                    ux_textboxNodeNameFormula.Text = GetTreeviewNodeProperty(_selectedObjectType + "_NAME_FORMULA", _currentNode.Parent);
                }
                else
                {
                    ux_textboxNodeNameFormula.Text = _sharedUtils.GetAppSettingValue(_selectedObjectType + "_NAME_FORMULA");
                }
            }
            else
            {
                ux_textboxNodeNameFormula.Text = "";
            }
        }

        private void ux_radiobuttonCustomName_CheckedChanged(object sender, EventArgs e)
        {
            if (ux_radiobuttonCustomName.Checked)
            {
                ux_groupboxNameBuilder.Enabled = true;
                ux_textboxNodeNameFormula.Text = GetTreeviewNodeProperty(_selectedObjectType + "_NAME_FORMULA", _currentNode);
                PopulateDataviewComboboxes(_selectedObjectType);
            }
            else
            {
                ux_textboxNodeNameFormula.Text = "";
                ux_comboboxDataviewName.DataSource = null;
                ux_comboboxFieldName.DataSource = null;
            }
        }

        private void PopulateDataviewComboboxes(string _selectedObjectTypePropertyName)
        {
            DataTable dt;
            DataSet ds = _sharedUtils.GetWebServiceData("get_dataview_list", "", 0, 0);
            if (ds.Tables.Contains("get_dataview_list"))
            {
                dt = ds.Tables["get_dataview_list"].Copy();
                dt.DefaultView.Sort = "title asc";
                dt.DefaultView.RowFilter = "category_code='Client' AND primary_key='" + _selectedObjectTypePropertyName + "'";
                ux_comboboxDataviewName.ValueMember = "dataview_name";
                ux_comboboxDataviewName.DisplayMember = "title";
                ux_comboboxDataviewName.DataSource = dt;
                //ux_comboboxDataviewName.SelectedItem = -1;
            }
        }

        private void ux_comboboxDataviewName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ux_comboboxDataviewName.SelectedIndex > -1)
            {
                DataTable dt;
                DataSet ds = _sharedUtils.GetWebServiceData("get_dataview_field_list", ":dataviewname=" + ux_comboboxDataviewName.SelectedValue.ToString(), 0, 0);
                if (ds.Tables.Contains("get_dataview_field_list"))
                {
                    dt = ds.Tables["get_dataview_field_list"].Copy();
                    dt.DefaultView.Sort = "title asc";
                    ux_comboboxFieldName.ValueMember = "field_name";
                    ux_comboboxFieldName.DisplayMember = "title";
                    ux_comboboxFieldName.DataSource = dt;
                    //ux_comboboxFieldName.SelectedIndex = -1;
                }
            }
        }

        private void ux_comboboxFieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ux_comboboxFieldName.SelectedIndex > -1)
            {
                ux_buttonAdd.Enabled = true;
            }
            else
            {
                ux_buttonAdd.Enabled = false;
            }
        }

        private void ux_buttonAdd_Click(object sender, EventArgs e)
        {
            string nameToken = "";
            nameToken += "{" + ux_comboboxDataviewName.SelectedValue.ToString() + "." + ux_comboboxFieldName.SelectedValue.ToString() + "}";
            if (ux_checkboxAutoAddSpace.Checked && ux_textboxNodeNameFormula.Text.Length > 0) nameToken = " + \" \" + " + nameToken;
            ux_textboxNodeNameFormula.Text += nameToken;
        }

    }
}
