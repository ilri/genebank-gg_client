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
    public partial class DataviewTabProperties : Form
    {
        //private DataSet _dataviewData;
        private DataviewProperties _tabProperties;
        private DataTable _dataviewList = new DataTable();
        private DataTable _formsList = new DataTable();
        private DataTable _category = new DataTable();
        private DataTable _databaseArea = new DataTable();

        //public DataviewTabProperties(SharedUtils sharedUtils, DataviewProperties dataviewProperties, FormsData[] formsList)
        public DataviewTabProperties(WebServices webServices, DataviewProperties dataviewProperties)
        {
            InitializeComponent();

            // Create a SharedUtils object...
            SharedUtils sharedUtils = new SharedUtils(webServices.Url, webServices.Username, webServices.Password_ClearText, true);

            // Get the list of dataviews...
            DataSet _dataviewData = sharedUtils.GetWebServiceData("get_dataview_list", "", 0, 0);
            // Populate the combobox with the list of available dataviews...
            if (_dataviewData.Tables.Contains("get_dataview_list"))
            {
                _dataviewList = _dataviewData.Tables["get_dataview_list"].Copy();
                _dataviewList.Columns.Add("display_member", typeof(string));
                _dataviewList.Columns.Add("category_name", typeof(string));
                _dataviewList.Columns.Add("database_area", typeof(string));
                foreach (DataRow dr in _dataviewList.Rows)
                {
                    string friendlyName = "";
                    if (dr.Table.Columns.Contains("title")) friendlyName += dr["title"].ToString().Trim();
                    dr["display_member"] = friendlyName;
                    if (!string.IsNullOrEmpty(dr["category_code"].ToString()))
                    {
                        dr["category_name"] = sharedUtils.GetLookupDisplayMember("code_value_lookup", dr["category_code"].ToString(), dr.Table.Columns["category_code"].ExtendedProperties["group_name"].ToString(), dr["category_code"].ToString());
                    }
                    else
                    {
                        dr["category_name"] = "";
                    }

                    if (!string.IsNullOrEmpty(dr["database_area_code"].ToString()))
                    {
                        dr["database_area"] = sharedUtils.GetLookupDisplayMember("code_value_lookup", dr["database_area_code"].ToString(), dr.Table.Columns["database_area_code"].ExtendedProperties["group_name"].ToString(), dr["database_area_code"].ToString());
                    }
                    else
                    {
                        dr["database_area"] = "";
                    }
                }
                // Build the distinct list of categories and bind it to the combobox...
                _category = _dataviewList.DefaultView.ToTable(true, new string[] { "category_name" });
                _category.DefaultView.Sort = "category_name asc";
                ux_comboboxDataviewCategory.DisplayMember = "category_name";
                ux_comboboxDataviewCategory.ValueMember = "category_name";
                ux_comboboxDataviewCategory.DataSource = _category;

                // Bind the list to the dropdown combobox...
                if (_dataviewList.Columns.Contains("display_member")) _dataviewList.DefaultView.Sort = "display_member asc";
                ux_comboboxDataviews.DisplayMember = "display_member";
                ux_comboboxDataviews.ValueMember = "dataview_name";
                ux_comboboxDataviews.DataSource = _dataviewList;
            }
            else
            {
                // Did not return the dataview list - bail out now...
                return;
            }

            // Get the dataview record that matches what is in the dataviewProperties.dataviewname property (there should only be one with this name)...
            DataRow[] currentDataviewRows = _dataviewList.Select("is_enabled='Y' AND dataview_name='" + dataviewProperties.DataviewName + "'");
            DataRow currentDataviewRow = null;
            if (currentDataviewRows.Length > 0) currentDataviewRow = currentDataviewRows[0];

            // Build the list of compatible forms and bind it to the combobox...
            FormsData[] formsDataList = sharedUtils.GetDataviewFormsData();

            if (formsDataList != null &&
                formsDataList.Length > 0)
            {
                _formsList.Columns.Add("DisplayMember", typeof(string));
                _formsList.Columns.Add("ValueMember", typeof(string));
                _formsList.Columns.Add("PreferredDataviewName", typeof(string));
                //_formsList.PrimaryKey = new DataColumn[] { _formsList.Columns["ValueMember"] };
                foreach (FormsData fd in formsDataList)
                {
                    if (!string.IsNullOrEmpty(fd.PreferredDataviewName))
                    {
                        string[] preferredDataviews = fd.PreferredDataviewName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string preferredDataview in preferredDataviews)
                        {
                            if (fd.ConstInfo != null &&
                                !string.IsNullOrEmpty(fd.StrongFormName))
                            {
                                DataRow newRow = _formsList.NewRow();
                                newRow["DisplayMember"] = fd.FormName + "   (" + fd.ConstInfo.Module.Name + ")";
                                newRow["ValueMember"] = fd.StrongFormName;
                                newRow["PreferredDataviewName"] = preferredDataview.Trim().ToLower();
                                _formsList.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }
            // Set the tab properties to the values passed into the dialog...
            this.TabProperties = dataviewProperties;

            // Populate the controls with the currently chosen dataview (if this is not a brand new dataview)...
            // (order matters here - first choose category, then DB area, and finally dataview)
            // STEP 1 - Catetory
            if (currentDataviewRow != null &&
                ux_comboboxDataviewCategory.Items.Count > 0)
            {
                ux_comboboxDataviewCategory.SelectedValue = currentDataviewRow["category_name"].ToString();
            }
            // STEP 2 - Database Area
            if (currentDataviewRow != null &&
                ux_comboboxDataviewCategory.Items.Count > 0)
            {
                ux_comboboxDatabaseArea.SelectedValue = currentDataviewRow["database_area"].ToString();
            }
            // STEP 3 - Dataview
            if (currentDataviewRow != null &&
                ux_comboboxDataviewCategory.Items.Count > 0)
            {
                ux_comboboxDataviews.SelectedValue = currentDataviewRow["dataview_name"].ToString();
            }

            // Disable the OK button and the radio buttons for forms viewer (by default)...
            if (ux_comboboxDataviews.SelectedValue == null)
            {
                ux_buttonOK.Enabled = false;
                ux_radiobuttonFormStyle.Enabled = false;
                ux_radiobuttonBothStyle.Enabled = false;
                ux_comboboxForm.Enabled = false;
                ux_checkboxAlwayOnTop.Enabled = false;
                ux_checkboxAlwayOnTop.Hide();
            }
sharedUtils.UpdateControls(this.Controls, this.Name);
        }

        public DataviewProperties TabProperties
        {
            get
            {
                return _tabProperties;
            }
            set
            {
                _tabProperties = value;
                // Update the control for the Tab Name...
                ux_textboxTabName.Text = _tabProperties.TabName;
                // Update the control for the list of dataviews...
                try
                {
                    //ux_comboboxDataviews.Text = _tabProperties.DataviewName;
                    ux_comboboxDataviews.SelectedValue = _tabProperties.DataviewName;
                }
                catch
                {
                    ux_comboboxDataviews.SelectedIndex = 0;
                }
                // If any compatible forms were found enable the radio buttons and update the Forms List...
                UpdateDataviewFormsControls();
            }
        }

        private void UpdateDataviewFormsControls()
        {
            if(_formsList.Columns.Contains("PreferredDataviewName")) _formsList.DefaultView.RowFilter = "PreferredDataviewName='" + _tabProperties.DataviewName.Trim().ToLower() + "'";
            if (_formsList.DefaultView.Count > 0)
            {
                ux_radiobuttonSpreadsheetStyle.Enabled = true;
                ux_radiobuttonFormStyle.Enabled = true;
                ux_radiobuttonBothStyle.Enabled = true;
                // Set the preferred viewing style for the Form...
                switch (_tabProperties.ViewerStyle.Trim().ToUpper())
                {
                    case "SPREADSHEET":
                        // Toggle the radiobutton...
                        ux_radiobuttonSpreadsheetStyle.Checked = false;
                        ux_radiobuttonSpreadsheetStyle.Checked = true;
                        break;
                    case "FORM":
                        // Toggle the radiobutton...
                        ux_radiobuttonFormStyle.Checked = false;
                        ux_radiobuttonFormStyle.Checked = true;
                        break;
                    case "BOTH":
                        // Toggle the radiobutton...
                        ux_radiobuttonBothStyle.Checked = false;
                        ux_radiobuttonBothStyle.Checked = true;
                        break;
                    default:
                        // Toggle the radiobutton...
                        ux_radiobuttonSpreadsheetStyle.Checked = false;
                        ux_radiobuttonSpreadsheetStyle.Checked = true;
                        break;
                }
                // Bind the forms list to the combobox...
                ux_comboboxForm.DisplayMember = "DisplayMember";
                ux_comboboxForm.ValueMember = "ValueMember";
                ux_comboboxForm.DataSource = _formsList;
                // Attempt to set the preferred form view saved with the tab properties...
                if (_formsList.DefaultView.ToTable().Select("ValueMember='" + _tabProperties.StrongFormName.Replace("'", "''") + "'").Length > 0)
                {
                    ux_comboboxForm.SelectedValue = _tabProperties.StrongFormName;
                }
                else
                {
                    ux_comboboxForm.SelectedIndex = 0;
                }
                bool alwaysOnTop = false;
                bool.TryParse(_tabProperties.AlwaysOnTop.Trim().ToLower(), out alwaysOnTop);
                ux_checkboxAlwayOnTop.Checked = alwaysOnTop;
            }
            else
            {
                // Toggle the radiobutton...
                ux_radiobuttonSpreadsheetStyle.Checked = false;
                ux_radiobuttonSpreadsheetStyle.Checked = true;
                ux_radiobuttonFormStyle.Enabled = false;
                ux_radiobuttonBothStyle.Enabled = false;
                ux_comboboxForm.Enabled = false;
                ux_checkboxAlwayOnTop.Enabled = false;
                ux_checkboxAlwayOnTop.Hide();
            }
        }

        private void ux_buttonOK_Click(object sender, EventArgs e)
        {
            _tabProperties.TabName = ux_textboxTabName.Text;
            _tabProperties.DataviewName = ux_comboboxDataviews.SelectedValue.ToString();
            if (ux_comboboxForm.SelectedValue != null)
            {
                _tabProperties.StrongFormName = ux_comboboxForm.SelectedValue.ToString();
            }
            else
            {
                _tabProperties.StrongFormName = "";
                ux_radiobuttonSpreadsheetStyle.Checked = true;
            }
            if (ux_radiobuttonSpreadsheetStyle.Checked) _tabProperties.ViewerStyle = "Spreadsheet";
            if (ux_radiobuttonFormStyle.Checked) _tabProperties.ViewerStyle = "Form";
            if (ux_radiobuttonBothStyle.Checked) _tabProperties.ViewerStyle = "Both";
            if (ux_checkboxAlwayOnTop.Checked)
            {
                _tabProperties.AlwaysOnTop = "true";
            }
            else
            {
                _tabProperties.AlwaysOnTop = "false";
            }

            this.Close();
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            _tabProperties.TabName = "";
            _tabProperties.DataviewName = "";
            _tabProperties.StrongFormName = "";
            _tabProperties.ViewerStyle = "";
            _tabProperties.AlwaysOnTop = "";

            this.Close();
        }

        private void ux_textboxTabName_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxTabName.Text.Length > 0 &&
                ux_comboboxDataviews.SelectedValue != null)
            {
                ux_buttonOK.Enabled = true;
            }
            else
            {
                ux_buttonOK.Enabled = false;
            }
        }

        private void ux_radiobuttonSpreadsheetStyle_CheckedChanged(object sender, EventArgs e)
        {
            ux_comboboxForm.Enabled = false;
            ux_checkboxAlwayOnTop.Enabled = false;
            ux_checkboxAlwayOnTop.Hide();
        }

        private void ux_radiobuttonFormStyle_CheckedChanged(object sender, EventArgs e)
        {
            ux_comboboxForm.Enabled = true;
            ux_checkboxAlwayOnTop.Enabled = false;
            ux_checkboxAlwayOnTop.Hide();
        }

        private void ux_radiobuttonBothStyle_CheckedChanged(object sender, EventArgs e)
        {
            ux_comboboxForm.Enabled = true;
            ux_checkboxAlwayOnTop.Enabled = true;
            ux_checkboxAlwayOnTop.Show();

        }

        private void ux_comboboxDataviewCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ux_comboboxDataviewCategory.SelectedValue != null)
            {
                _dataviewList.DefaultView.RowFilter = "category_name='" + ux_comboboxDataviewCategory.SelectedValue.ToString() + "'";
                // Get the distinct DB areas based on the new row filter...
                _databaseArea = _dataviewList.DefaultView.ToTable(true, new string[] { "database_area" });
                _databaseArea.DefaultView.Sort = "database_area asc";
                ux_comboboxDatabaseArea.DisplayMember = "database_area";
                ux_comboboxDatabaseArea.ValueMember = "database_area";
                ux_comboboxDatabaseArea.DataSource = _databaseArea;
                if (ux_comboboxDatabaseArea.Items.Count > 0)
                {
                    ux_comboboxDatabaseArea.SelectedIndex = -1;
                    ux_comboboxDatabaseArea.SelectedIndex = 0;
                }
            }
        }

        private void ux_comboboxDatabaseArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ux_comboboxDatabaseArea.SelectedValue != null)
            {
                _dataviewList.DefaultView.RowFilter = "category_name='" + ux_comboboxDataviewCategory.SelectedValue.ToString() + "' AND database_area='" + ux_comboboxDatabaseArea.SelectedValue.ToString() + "'";
                ux_comboboxDataviews.SelectedIndex = -1;
            }
        }

        private void ux_comboboxDataviews_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ux_comboboxDataviews.SelectedValue != null)
            {
                _tabProperties.DataviewName = ux_comboboxDataviews.SelectedValue.ToString();
                // Enable Forms control choices if there are valid Forms available...
                UpdateDataviewFormsControls();
                // Enable the OK button if a dataview has been chosen and there is a name for the tab...
                if(ux_textboxTabName.Text.Length > 0) ux_buttonOK.Enabled = true;
            }
            else
            {
                _tabProperties.DataviewName = "";
                // Disable the Forms control choices (because no dataview is choosen)...
                UpdateDataviewFormsControls();
                // Disable the OK button
                ux_buttonOK.Enabled = false;
            }
        }
    }
}
