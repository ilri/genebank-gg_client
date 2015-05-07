using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace OrderWizard
{
    interface IGRINGlobalDataWizard
    {
        string FormName { get; }
        DataTable ChangedRecords { get; }
        string PKeyName { get; }
        //string PreferredDataview { get; }
        //bool EditMode { get; set; }
    }

    public partial class OrderWizard : Form, IGRINGlobalDataWizard
    {
        SharedUtils _sharedUtils;
        DataTable _orderRequest;
        DataTable _orderRequestItem;
        DataTable _orderRequestAction;
        DataTable _orderRequestStatusCodes;
        DataTable _webOrderRequest;
        DataTable _webOrderRequestItem;
        DataTable _webOrderRequestStatusCodes;
        BindingSource _orderRequestBindingSource;
        BindingSource _orderRequestItemBindingSource;
        BindingSource _orderRequestActionBindingSource;
        BindingSource _webOrderRequestBindingSource;
        BindingSource _webOrderRequestItemBindingSource;
        string _originalPKeys = "";
        string _orderRequestPKeys = "";
        string _orderRequestStatusFilter = "";
        string _webOrderRequestStatusFilter = "";
        DataSet _changedRecords = new DataSet();
        int _mouseClickDGVColumnIndex;
        int _mouseClickDGVRowIndex;
        string _sortOrder = "";
        string _selectionList = "";

        public OrderWizard(string pKeys, SharedUtils sharedUtils)
        {
            InitializeComponent();

            // Wire up the event handlers for Order Request binding source...
            _orderRequestBindingSource = new BindingSource();
            _orderRequestBindingSource.ListChanged += new ListChangedEventHandler(_orderRequestBindingSource_ListChanged);
            _orderRequestBindingSource.CurrentChanged += new EventHandler(_orderRequestBindingSource_CurrentChanged);
            _orderRequestItemBindingSource = new BindingSource();
            _orderRequestActionBindingSource = new BindingSource();
            _sharedUtils = sharedUtils;
            _originalPKeys = pKeys;
            foreach (string pkeyToken in pKeys.Split(';'))
            {
                if (pkeyToken.Split('=')[0].Trim().ToUpper() == ":ORDERREQUESTID") _selectionList = pkeyToken.Split('=')[1];
            }

            // Wire up the event handlers for Web Order Request binding source...
            _webOrderRequestBindingSource = new BindingSource();
            _webOrderRequestBindingSource.CurrentChanged += new EventHandler(_webOrderRequestBindingSource_CurrentChanged);
            _webOrderRequestItemBindingSource = new BindingSource();

            // Make the filter groupboxes the same size and location...
            ux_groupboxWebOrderFilters.Size = ux_groupboxOrderFilters.Size;
            ux_groupboxWebOrderFilters.Location = ux_groupboxOrderFilters.Location;
            ux_groupboxWebOrderFilters.Visible = false;
            ux_groupboxOrderFilters.Visible = true;
        }

        private void OrderWizard_Load(object sender, EventArgs e)
        {
            this.Text = FormName;

            BuildOrderRequestActionsPage();
            //BuildWebOrderRequestPage();

            //DataSet ds;
            //// Get the order_request table and bind it to the main form on the General tabpage...
            //ds = _sharedUtils.GetWebServiceData("get_order_request", _origPKeys, 0, 0);
            //_orderRequestPKeys = "";
            //if (ds.Tables.Contains("get_order_request"))
            //{
            //    _orderRequest = ds.Tables["get_order_request"].Copy();
            //    _orderRequestBindingSource.DataSource = _orderRequest;
            //    // Build a list of order_request_ids to use for gathering the order_request_items...
            //    _orderRequestPKeys = ":orderrequestid=";
            //    foreach (DataRow dr in _orderRequest.Rows)
            //    {
            //        _orderRequestPKeys += dr["order_request_id"].ToString() + ",";
            //    }
            //    //_orderRequestPKeys = _origPKeys.Replace(":orderrequestid=", orderRequestIDs.TrimEnd(','));
            //    _orderRequestPKeys = _orderRequestPKeys.TrimEnd(',');
            //}

            //// Get the order_request_item table and bind it to the main form on the General tabpage...
            //ds = _sharedUtils.GetWebServiceData("order_wizard_get_order_request_item", _orderRequestPKeys, 0, 0);
            //if (ds.Tables.Contains("order_wizard_get_order_request_item"))
            //{
            //    _orderRequestItem = ds.Tables["order_wizard_get_order_request_item"].Copy();
            //    ux_datagridviewOrderRequestItem.DataSource = _orderRequestItemBindingSource;
            //    _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestItem, _orderRequestItem);
            //}

            //// Refresh the order request data binding...
            //RefreshOrderData();

            //// Refresh the web order request data binding...
            //RefreshWebOrderData();

            // Add items to the order request status checked listbox control...
            _orderRequestStatusCodes = _sharedUtils.GetLocalData("select * from code_value_lookup where group_name=@groupname", "@groupname=ORDER_REQUEST_ITEM_STATUS");
            if (_orderRequestStatusCodes != null &&
                _orderRequestStatusCodes.Columns.Contains("value_member") &&
                _orderRequestStatusCodes.Columns.Contains("display_member"))
            {
                //DataRow newDR = _orderRequestStatusCodes.NewRow();
                //newDR["code_value_id"] = -1;
                //newDR["group_name"] = "ORDER_REQUEST_ITEM_STATUS";
                //newDR["value_member"] = "NEW";
                //newDR["display_member"] = "New";
                //newDR["sys_lang_id"] = 1;
                //_orderRequestStatusCodes.Rows.InsertAt(newDR, 0);
                ux_checkedlistboxOrderItemStatus.Items.Clear();
                _orderRequestStatusFilter = "";
                foreach (DataRow dr in _orderRequestStatusCodes.Rows)
                {
                    if (dr["value_member"].ToString().ToUpper() == "NEW")
                    {
                        ux_checkedlistboxOrderItemStatus.Items.Add(dr["display_member"].ToString(), true);
                    }
                    else
                    {
                        ux_checkedlistboxOrderItemStatus.Items.Add(dr["display_member"].ToString(), false);
                    }
                }
            }

            // Add items to the web order request status checked listbox control...
            _webOrderRequestStatusCodes = _sharedUtils.GetLocalData("select * from code_value_lookup where group_name=@groupname", "@groupname=WEB_ORDER_REQUEST_STATUS");
            if (_webOrderRequestStatusCodes != null &&
                _webOrderRequestStatusCodes.Columns.Contains("value_member") &&
                _webOrderRequestStatusCodes.Columns.Contains("display_member"))
            {
                ux_checkedlistboxWebOrderItemStatus.Items.Clear();
                _webOrderRequestStatusFilter = "";
                foreach (DataRow dr in _webOrderRequestStatusCodes.Rows)
                {
                    if (dr["value_member"].ToString().ToUpper() == "SUBMITTED")
                    {
                        ux_checkedlistboxWebOrderItemStatus.Items.Add(dr["display_member"].ToString(), true);
                    }
                    else
                    {
                        ux_checkedlistboxWebOrderItemStatus.Items.Add(dr["display_member"].ToString(), false);
                    }
                }
            }

// Load the list of Crystal Reports...
System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\Reports");
foreach (string reportName in _sharedUtils.GetAppSettingValue("OrderWizardCrystalReports").Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
{
    foreach (System.IO.FileInfo fi in di.GetFiles(reportName.Trim(), System.IO.SearchOption.AllDirectories))
    {
        if (fi.Name.Trim().ToUpper() == reportName.Trim().ToUpper())
        {
            ux_comboboxCrystalReports.Items.Add(fi.Name);
        }
    }
}

//RefreshOrderData();
ux_radiobuttonSelectionOrders.Checked = true;
            // Bind the bindingsource to the binding navigator toolstrip on the main form...
RefreshOrderData();
            ux_bindingnavigatorForm.BindingSource = _orderRequestBindingSource;

            // Bind the bindingsource to the binding navigator toolstrip on the web_order_request tab page...
RefreshWebOrderData();
            ux_bindingNavigatorWebOrders.BindingSource = _webOrderRequestBindingSource;

            // Format the controls on this dialog...
            //bindControls(this.Controls);
            //formatControls(this.Controls);
            // Bind and format the controls on the Order tab...
            bindControls(OrderPage.Controls, _orderRequestBindingSource);
            formatControls(OrderPage.Controls, _orderRequestBindingSource);

            // Populate the Order Type Filter combobox....
            _sharedUtils.BindComboboxToCodeValue(ux_comboboxOrderTypeFilter, _orderRequest.Columns["order_type_code"]);

            // Re-Bind (but don't format) the controls on the Web Order tab to a different binding source...
            bindControls(WebOrderPage.Controls, _webOrderRequestBindingSource);
            //formatControls(WebOrderPage.Controls, _webOrderRequestBindingSource);

            if (_orderRequestBindingSource.List.Count > 0)
            {
                //ux_tabcontrolMain.Enabled = true;
                //OrderPage.Show();
                //ActionsPage.Show();
                OrderPage.Enabled = true;
                ActionsPage.Enabled = true;
            }
            else
            {
                //ux_tabcontrolMain.Enabled = false;
                //OrderPage.Hide();
                //ActionsPage.Hide();
                OrderPage.Enabled = false;
                ActionsPage.Enabled = false;
            }

            _sharedUtils.UpdateComponents(this.components.Components, this.Name);
            _sharedUtils.UpdateControls(this.Controls, this.Name);

            // Force the row filters to be applied...
            _orderRequestBindingSource_CurrentChanged(sender, e);
        }

        public string FormName
        {
            get
            {
                return "Order Wizard";
            }
        }

        public DataTable ChangedRecords
        {
            get
            {
                DataTable dt = new DataTable();
                if (_changedRecords.Tables.Contains(_orderRequest.TableName))
                {
                    dt = _changedRecords.Tables[_orderRequest.TableName].Copy();
                }
                return dt;
            }
        }

        public string PKeyName
        {
            get
            {
                return "order_request_id";
            }
        }

        private void OrderWizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            _orderRequestBindingSource.ListChanged -= _orderRequestBindingSource_ListChanged;
            _orderRequestBindingSource.CurrentChanged -= _orderRequestBindingSource_CurrentChanged;
            _webOrderRequestBindingSource.CurrentChanged -= _webOrderRequestBindingSource_CurrentChanged;
        }

        private void OrderWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // The user might be closing the form during the middle of edit changes in the datagridview - if so ask the
            // user if they would like to save their data...
             int intRowEdits = 0;

            _orderRequestBindingSource.EndEdit();
            if (_orderRequest.GetChanges() != null) intRowEdits = _orderRequest.GetChanges().Rows.Count;
            _orderRequestItemBindingSource.EndEdit();
            if (_orderRequestItem.GetChanges() != null) intRowEdits += _orderRequestItem.GetChanges().Rows.Count;
            _orderRequestActionBindingSource.EndEdit();
            if (_orderRequestAction.GetChanges() != null) intRowEdits += _orderRequestAction.GetChanges().Rows.Count;
            //_webOrderRequestBindingSource.EndEdit();
            //if (_webOrderRequest.GetChanges() != null) intRowEdits += _webOrderRequest.GetChanges().Rows.Count;
            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s), are you sure you want to cancel your edits and close this window?", "Cancel Edits and Close", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "OrderWizard_FormClosingMessage1";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
            //if (intRowEdits > 0 &&
            //    DialogResult.No == MessageBox.Show("You have " + intRowEdits + " unsaved row change(s), are you sure you want to cancel?", "Cancel Edits", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
            if (intRowEdits > 0 && DialogResult.No == ggMessageBox.ShowDialog())
            {
                e.Cancel = true;
            }
        }

        #region Customizations to Dynamic Controls...
        private void ux_textboxFinalDestination_TextChanged(object sender, EventArgs e)
        {
            bindingNavigatorOrderNumber.Text = "";

            if (string.IsNullOrEmpty(ux_textboxFinalDestination.Text) &&
                _orderRequest.Columns[ux_textboxFinalDestination.Tag.ToString()].ExtendedProperties.Contains("is_nullable") &&
                _orderRequest.Columns[ux_textboxFinalDestination.Tag.ToString()].ExtendedProperties["is_nullable"].ToString() == "N" &&
                !_orderRequest.Columns[ux_textboxFinalDestination.Tag.ToString()].ReadOnly)
            {
                ux_textboxFinalDestination.BackColor = Color.Plum;
            }
            else
            {
                ux_textboxFinalDestination.BackColor = Color.Empty;
                if (string.IsNullOrEmpty(ux_textboxRequestor.Text))
                {
                    ux_textboxRequestor.Text = ux_textboxFinalDestination.Text;
                }
                if (string.IsNullOrEmpty(ux_textboxShipTo.Text))
                {
                    ux_textboxShipTo.Text = ux_textboxFinalDestination.Text;
                }
            }
        }
        #endregion

        #region Dynamic Controls logic...
        private void bindControls(Control.ControlCollection controlCollection, BindingSource bindingSource)
        {
            foreach (Control ctrl in controlCollection)
            {
                //if (ctrl != ux_bindingnavigatorForm)  // Leave the bindingnavigator alone
                if (!(ctrl is BindingNavigator))  // Leave the bindingnavigators alone
                {
                    // If the ctrl has children - bind them too...
                    if (ctrl.Controls.Count > 0)
                    {
                        bindControls(ctrl.Controls, bindingSource);
                    }
                    // Bind the control (by type)...
                    if (ctrl is ComboBox) bindComboBox((ComboBox)ctrl, bindingSource);
                    if (ctrl is TextBox) bindTextBox((TextBox)ctrl, bindingSource);
                    if (ctrl is CheckBox) bindCheckBox((CheckBox)ctrl, bindingSource);
                    if (ctrl is DateTimePicker) bindDateTimePicker((DateTimePicker)ctrl, bindingSource);
                    if (ctrl is Label) bindLabel((Label)ctrl, bindingSource);
                }
            }
        }

        private void formatControls(Control.ControlCollection controlCollection, BindingSource bindingSource)
        {
            foreach (Control ctrl in controlCollection)
            {
                if (ctrl != ux_bindingnavigatorForm)  // Leave the bindingnavigator alone
                {
                    // If the ctrl has children - set their edit mode too...
                    if (ctrl.Controls.Count > 0)
                    {
                        formatControls(ctrl.Controls, bindingSource);
                    }
                    // Set the edit mode for the control...
                    if (ctrl != null &&
                        ctrl.Tag != null &&
                        ctrl.Tag is string &&
                        bindingSource != null &&
                        bindingSource.DataSource is DataTable &&
                        ((DataTable)bindingSource.DataSource).Columns.Contains(ctrl.Tag.ToString().Trim().ToLower()))
                    {
                        if (ctrl is TextBox)
                        {
                            // TextBoxes have a ReadOnly property in addition to an Enabled property so we handle this one separate...
                            ((TextBox)ctrl).ReadOnly = ((DataTable)bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
                        }
                        else if (ctrl is Label)
                        {
                            // Do nothing to the Label
                        }
                        else
                        {
                            // All other control types (ComboBox, CheckBox, DateTimePicker) except Labels...
                            ctrl.Enabled = !((DataTable)bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
                        }
                    }
                }
            }
        }

        private void bindComboBox(ComboBox comboBox, BindingSource bindingSource)
        {
            comboBox.DataBindings.Clear();
            comboBox.Enabled = false;
            if (comboBox != null &&
                comboBox.Tag != null &&
                comboBox.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(comboBox.Tag.ToString().Trim().ToLower()))
            {
                if (_sharedUtils != null)
                {
                    DataColumn dc = ((DataTable)bindingSource.DataSource).Columns[comboBox.Tag.ToString().Trim().ToLower()];
                    _sharedUtils.BindComboboxToCodeValue(comboBox, dc);
                    if (comboBox.DataSource.GetType() == typeof(DataTable))
                    {
                        // Calculate the maximum width needed for displaying the dropdown items and set the combobox property...
                        int maxWidth = comboBox.DropDownWidth;
                        foreach (DataRow dr in ((DataTable)comboBox.DataSource).Rows)
                        {
                            if (TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width > maxWidth)
                            {
                                maxWidth = TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width;
                            }
                        }
                        comboBox.DropDownWidth = maxWidth;
                    }

                    //if (_sharedUtils != null &&
                    //    _sharedUtils.LookupTablesContains("MRU_code_value_lookup"))
                    //{
                    //    DataColumn dc = ((DataTable)bindingSource.DataSource).Columns[comboBox.Tag.ToString().Trim().ToLower()];
                    //    DataTable cvl = _sharedUtils.LookupTablesGetMRUTable("MRU_code_value_lookup");

                    //    //if (dc.ExtendedProperties.Contains("code_group_id") && cvl != null)
                    //    if (dc.ExtendedProperties.Contains("group_name") && cvl != null)
                    //    {
                    //        //DataView dv = new DataView(cvl, "code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "'", "display_member ASC", DataViewRowState.CurrentRows);
                    //        DataView dv = new DataView(cvl, "group_name='" + dc.ExtendedProperties["group_name"].ToString() + "'", "display_member ASC", DataViewRowState.CurrentRows);
                    //        DataTable dt = dv.ToTable();
                    //        if (dc.ExtendedProperties.Contains("is_nullable") && dc.ExtendedProperties["is_nullable"].ToString() == "Y")
                    //        {
                    //            DataRow dr = dt.NewRow();
                    //            foreach (DataColumn cvldc in cvl.Columns)
                    //            {
                    //                // If there are any non-nullable fields - set them now...
                    //                if (!cvldc.AllowDBNull)
                    //                {
                    //                    dr[cvldc.ColumnName] = -1;
                    //                }
                    //            }
                    //            dr["display_member"] = "[Null]";
                    //            dr["value_member"] = DBNull.Value;
                    //            dt.Rows.InsertAt(dr, 0);
                    //            dt.AcceptChanges();
                    //        }
                    //        comboBox.DisplayMember = "display_member";
                    //        comboBox.ValueMember = "value_member";
                    //        comboBox.DataSource = dt;
                    //    }

                    //    // Calculate the maximum width needed for displaying the dropdown items and set the combobox property...
                    //    int maxWidth = comboBox.DropDownWidth;
                    //    foreach (DataRow dr in cvl.Rows)
                    //    {
                    //        if (TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width > maxWidth)
                    //        {
                    //            maxWidth = TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width;
                    //        }
                    //    }
                    //    comboBox.DropDownWidth = maxWidth;

                    // Bind the SelectedValue property to the binding source...
                    comboBox.DataBindings.Add("SelectedValue", bindingSource, comboBox.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);

                    // Wire up to an event handler if this column is a date_code (format) field...
                    if (dc.ColumnName.Trim().ToLower().EndsWith("_code") &&
                        dc.Table.Columns.Contains(dc.ColumnName.Trim().ToLower().Substring(0, dc.ColumnName.Trim().ToLower().LastIndexOf("_code"))))
                    {
                        comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
                    }
                }
                else
                {
                    // Bind the Text property to the binding source...
                    comboBox.DataBindings.Add("Text", bindingSource, comboBox.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);
                }
            }
        }

        private void bindTextBox(TextBox textBox, BindingSource bindingSource)
        {
            textBox.DataBindings.Clear();
            textBox.ReadOnly = true;
            if (textBox != null &&
                textBox.Tag != null &&
                textBox.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(textBox.Tag.ToString().Trim().ToLower()))
            {
                DataTable dt = (DataTable)bindingSource.DataSource;
                DataColumn dc = dt.Columns[textBox.Tag.ToString().Trim().ToLower()];
                if (_sharedUtils.LookupTablesIsValidFKField(dc))
                {
                    // Create a new binding that handles display_member/value_member conversions...
                    Binding textBinding = new Binding("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                    textBinding.Format += new ConvertEventHandler(textLUBinding_Format);
                    textBinding.Parse += new ConvertEventHandler(textLUBinding_Parse);
                    textBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    // Bind it to the textbox...
                    textBox.DataBindings.Add(textBinding);
                }
                else if (dc.DataType == typeof(DateTime))
                {
                    // Create a new binding that handles display_member/value_member conversions...
                    Binding textBinding = new Binding("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                    textBinding.Format += new ConvertEventHandler(textDateTimeBinding_Format);
                    textBinding.Parse += new ConvertEventHandler(textDateTimeBinding_Parse);
                    textBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    // Bind it to the textbox...
                    textBox.DataBindings.Add(textBinding);
                }
                else
                {
                    // Bind to a plain-old text field in the database (no LU required)...
                    textBox.DataBindings.Add("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                }

                // Add an event handler for processing the first key press (to display the lookup picker dialog)...
                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
            }
        }

        private void bindCheckBox(CheckBox checkBox, BindingSource bindingSource)
        {
            checkBox.DataBindings.Clear();
            checkBox.Enabled = false;
            if (checkBox != null &&
                checkBox.Tag != null &&
                checkBox.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(checkBox.Tag.ToString().Trim().ToLower()))
            {
                DataTable dt = (DataTable)bindingSource.DataSource;
                DataColumn dc = dt.Columns[checkBox.Tag.ToString().Trim().ToLower()];
                checkBox.Text = dc.Caption;
                Binding boolBinding = new Binding("Checked", bindingSource, checkBox.Tag.ToString().Trim().ToLower());
                boolBinding.Format += new ConvertEventHandler(boolBinding_Format);
                boolBinding.Parse += new ConvertEventHandler(boolBinding_Parse);
                boolBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                checkBox.DataBindings.Add(boolBinding);
            }
        }

        private void bindDateTimePicker(DateTimePicker dateTimePicker, BindingSource bindingSource)
        {
            dateTimePicker.DataBindings.Clear();
            dateTimePicker.Enabled = false;
            if (dateTimePicker != null &&
                dateTimePicker.Tag != null &&
                dateTimePicker.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(dateTimePicker.Tag.ToString().Trim().ToLower()))
            {
                // Now bind the control to the column in the bindingSource...
                dateTimePicker.DataBindings.Add("Text", bindingSource, dateTimePicker.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void bindLabel(Label label, BindingSource bindingSource)
        {
            if (label != null &&
                label.Tag != null &&
                label.Tag is string &&
                bindingSource != null &&
                bindingSource.DataSource is DataTable &&
                ((DataTable)bindingSource.DataSource).Columns.Contains(label.Tag.ToString().Trim().ToLower()))
            {
                //label.DataBindings.Add("Text", bindingSource, label.Tag.ToString().Trim().ToLower());
                label.Text = ((DataTable)bindingSource.DataSource).Columns[label.Tag.ToString().Trim().ToLower()].Caption;
            }
        }

        void boolBinding_Format(object sender, ConvertEventArgs e)
        {
            switch (e.Value.ToString().ToUpper())
            {
                case "Y":
                    e.Value = true;
                    break;
                case "N":
                    e.Value = false;
                    break;
                default:
                    e.Value = false;
                    break;
            }
        }

        void boolBinding_Parse(object sender, ConvertEventArgs e)
        {
            if (e.Value != null)
            {
                switch ((bool)e.Value)
                {
                    case true:
                        e.Value = "Y";
                        break;
                    case false:
                        e.Value = "N";
                        break;
                    default:
                        e.Value = "N";
                        break;
                }
            }
            else
            {
                e.Value = "N";
            }
        }

        void textDateTimeBinding_Format(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (dc.DataType == typeof(DateTime) &&
                !string.IsNullOrEmpty(e.Value.ToString()) &&
                dt.Columns.Contains(dc.ColumnName.Trim().ToLower() + "_code"))
            {
                DataRowView drv = (DataRowView)((BindingSource)b.DataSource).Current;
                string dateFormat = "MM/dd/yyyy";
                dateFormat = drv[dc.ColumnName + "_code"].ToString().Trim();
                e.Value = ((DateTime)e.Value).ToString(dateFormat);
            }
        }

        void textDateTimeBinding_Parse(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (dc.DataType == typeof(DateTime) &&
                !string.IsNullOrEmpty(e.Value.ToString()) &&
                dt.Columns.Contains(dc.ColumnName.Trim().ToLower() + "_code"))
            {
                DataRowView drv = (DataRowView)((BindingSource)b.DataSource).Current;
                string dateFormat = drv[dc.ColumnName + "_code"].ToString().Trim();
                DateTime parsedDateTime;
                if (DateTime.TryParseExact(e.Value.ToString(), dateFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out parsedDateTime))
                {
                    e.Value = parsedDateTime;
                }
            }
        }

        void textLUBinding_Format(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                //e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
                e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
            }
        }

        void textLUBinding_Parse(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (!string.IsNullOrEmpty(e.Value.ToString()))
            {
                //e.Value = _sharedUtils.GetLookupValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());  ((DataRowView)((BindingSource)b.DataSource).Current).Row
                e.Value = _sharedUtils.GetLookupValueMember(((DataRowView)((BindingSource)b.DataSource).Current).Row, dc.ExtendedProperties["foreign_key_dataview_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
            }
        }

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string dateColumnName = cb.Tag.ToString().Replace("_code", "");
            foreach (Binding b in cb.DataBindings)
            {
                foreach (Control ctrl in cb.Parent.Controls)
                {
                    if (ctrl.Tag.ToString() == dateColumnName &&
                        ctrl.GetType() == typeof(TextBox))
                    {
                        DataRowView drv = (DataRowView)((BindingSource)b.DataSource).Current;
                        string dateFormat = "MM/dd/yyyy";
                        dateFormat = drv[cb.Tag.ToString()].ToString().Trim();
                        DateTime dt;
                        if (DateTime.TryParseExact(drv[dateColumnName].ToString(), dateFormat, null, System.Globalization.DateTimeStyles.AssumeLocal, out dt))
                        {
                            ctrl.Text = ((DateTime)drv[dateColumnName]).ToString(dateFormat);
                        }
                    }
                }
            }
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;


            if (!tb.ReadOnly)
            {
                foreach (Binding b in tb.DataBindings)
                {
                    if (b.BindingManagerBase != null &&
                        b.BindingManagerBase.Current != null &&
                        b.BindingManagerBase.Current is DataRowView &&
                        b.BindingMemberInfo.BindingField != null)
                    {
                        if (_sharedUtils.LookupTablesIsValidFKField(((DataRowView)b.BindingManagerBase.Current).Row.Table.Columns[b.BindingMemberInfo.BindingField]) &&
                            e.KeyChar != Convert.ToChar(Keys.Escape)) // Ignore the Escape key and process anything else...
                        {
                            LookupTablePicker ltp = new LookupTablePicker(_sharedUtils, tb.Tag.ToString(), ((DataRowView)b.BindingManagerBase.Current).Row, tb.Text);
                            ltp.StartPosition = FormStartPosition.CenterParent;
                            if (DialogResult.OK == ltp.ShowDialog())
                            {
                                tb.Text = ltp.NewValue.Trim();
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) // Process the Delete key (since it is not passed on to the KeyPress event handler)...
            {
                TextBox tb = (TextBox)sender;

                if (!tb.ReadOnly)
                {
                    foreach (Binding b in tb.DataBindings)
                    {
                        if (b.BindingManagerBase != null &&
                            b.BindingManagerBase.Current != null &&
                            b.BindingManagerBase.Current is DataRowView &&
                            b.BindingMemberInfo.BindingField != null)
                        {
                            // Just in case the user selected only a part of the full text to delete - strip out the selected text and process normally...
                            string remainingText = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength);
                            if (string.IsNullOrEmpty(remainingText))
                            {
                                // When a textbox is bound to a table - some datatypes will not revert to a DBNull via the bound control - so
                                // take control of the update and force the field back to a null (non-nullable fields should show up to the GUI with colored background)...
                                ((DataRowView)b.BindingManagerBase.Current).Row[b.BindingMemberInfo.BindingField] = DBNull.Value;
                                b.ReadValue();
                                e.Handled = true;
                            }
                            else
                            {
                                if (_sharedUtils.LookupTablesIsValidFKField(((DataRowView)b.BindingManagerBase.Current).Row.Table.Columns[b.BindingMemberInfo.BindingField]))
                                {
                                    LookupTablePicker ltp = new LookupTablePicker(_sharedUtils, tb.Tag.ToString(), ((DataRowView)b.BindingManagerBase.Current).Row, remainingText);
                                    ltp.StartPosition = FormStartPosition.CenterParent;
                                    if (DialogResult.OK == ltp.ShowDialog())
                                    {
                                        tb.Text = ltp.NewValue.Trim();
                                        b.WriteValue();
                                        e.Handled = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region DGV control logic...
        private void ux_datagridview_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            DataView dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            if (dv != null && e.ColumnIndex > -1)
            {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                if (_sharedUtils.LookupTablesIsValidFKField(dc) &&
                    e.RowIndex < dv.Count &&
                    dv[e.RowIndex].Row.RowState != DataRowState.Deleted)
                {
                    if (dv[e.RowIndex][e.ColumnIndex] != DBNull.Value)
                    {
                        //e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                        e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                    }
                    dgv[e.ColumnIndex, e.RowIndex].ErrorText = dv[e.RowIndex].Row.GetColumnError(dc);
                    e.FormattingApplied = true;
                }

                if (dc.ReadOnly)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }

                if (dc.ExtendedProperties.Contains("is_nullable") &&
                    dc.ExtendedProperties["is_nullable"].ToString() == "N" &&
                    string.IsNullOrEmpty(dv[e.RowIndex][e.ColumnIndex].ToString()))
                {
                    e.CellStyle.BackColor = Color.Plum;
                }
            }
        }

        private void ux_datagridview_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string columnName = dgv.CurrentCell.OwningColumn.Name;
            DataColumn dc = dt.Columns[columnName];
            DataRow dr;

            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                //string luTableName = dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim();
                string luTableName = dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim();
                dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                string suggestedFilter = dgv.CurrentCell.EditedFormattedValue.ToString();
                //if (_lastDGVCharPressed > 0) suggestedFilter = _lastDGVCharPressed.ToString();
                ////GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(lookupTables, columnName, dr, suggestedFilter);
                GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(_sharedUtils, columnName, dr, suggestedFilter);
                //_lastDGVCharPressed = (char)0;
                ltp.StartPosition = FormStartPosition.CenterParent;
                if (DialogResult.OK == ltp.ShowDialog())
                {
                    if (dr != null)
                    {
                        if (ltp.NewKey != null && dr[dgv.CurrentCell.ColumnIndex].ToString().Trim() != ltp.NewKey.Trim())
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = ltp.NewKey.Trim();
                            dgv.CurrentCell.Value = ltp.NewValue.Trim();
                        }
                        else if (ltp.NewKey == null)
                        {
                            dr[dgv.CurrentCell.ColumnIndex] = DBNull.Value;
                            dgv.CurrentCell.Value = "";
                        }
                        dr.SetColumnError(dgv.CurrentCell.ColumnIndex, null);
                    }
                }
                dgv.EndEdit();
            }
        }

        private void ux_datagridview_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            string errorMessage = e.Exception.Message;
            int columnWithError = -1;

            // Find the cell the error belongs to (don't use e.ColumnIndex because it points to the current cell *NOT* the offending cell)...
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (errorMessage.Contains(col.Name))
                {
                    dgv[col.Name, e.RowIndex].ErrorText = errorMessage;
                    columnWithError = col.Index;
                }
            }
        }
        #endregion

        #region Binding Navigator Logic...
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            // Pre-populate the form with default values...
            //ux_textboxSite.Enabled = false;
            ux_textboxSite.Text = _sharedUtils.UserSite;
            //((DataRowView)_orderRequestBindingSource.Current)["site"] = _sharedUtils.UserSite;
            ux_comboboxOrderType.SelectedValue = "DI";
            ux_comboboxStatus.SelectedValue = "NEW";
            //ux_textboxOrderedDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            //((DataRowView)_orderRequestBindingSource.Current)["ordered_date"] = DateTime.Today.ToString("MM/dd/yyyy");
            ux_textboxOrderedDate.Text = DateTime.Today.ToString("d");
            ((DataRowView)_orderRequestBindingSource.Current)["ordered_date"] = DateTime.Today.ToString("d");
            ux_checkboxIsCompleted.Checked = false;
            ux_checkboxIsSupplyLow.Checked = false;

            // Create an Action for the status_code changes (one per staus_code with a count for how many items were changed to that status)...
            DataRow newOrderRequestAction = _orderRequestAction.NewRow();
            if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = ((DataRowView)_orderRequestBindingSource.Current)["order_request_id"].ToString();
            if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "NEW";
            if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
            if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
            if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
            if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
            if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
            if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
            if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
            if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "NEW Order Request created by " + _sharedUtils.Username;
            // And add it to the Order Request Action Table...
            _orderRequestAction.Rows.Add(newOrderRequestAction);
        }

        private void bindingNavigatorSaveButton_Click(object sender, EventArgs e)
        {
            //SaveOrderData();
            //RefreshOrderData();

            int errorCount = 0;
            errorCount = SaveOrderData();
            if (errorCount == 0)
            {
                //MessageBox.Show(this, "All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                ggMessageBox.ShowDialog();
            }
            else
            {
                //MessageBox.Show(this, "The data being saved has errors that should be reviewed.\n\n  Error Count: " + errorCount, "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\n  Error Count: {0}", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage2";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
                ggMessageBox.ShowDialog();
            }

            // Refresh the Order Request Items and Actions DGV formatting...
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
            RefreshDGVFormatting(ux_datagridviewOrderRequestAction);

            // Force the row filters to be applied to order items and order actions...
            _orderRequestBindingSource_CurrentChanged(sender, e);
        }

        private void bindingNavigatorSaveAndExitButton_Click(object sender, EventArgs e)
        {
            //SaveOrderData();
            //RefreshOrderData();
            //this.Close();
            int errorCount = 0;
            errorCount = SaveOrderData();
            if (errorCount == 0)
            {
                //MessageBox.Show(this, "All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Order Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage1";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                ggMessageBox.ShowDialog();
                this.Close();
            }
            else
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Order Wizard).\n\n  Error Count: {0}", "Order Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "OrderWizard_bindingNavigatorSaveButtonMessage3";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
                //if (DialogResult.No == MessageBox.Show(this, "The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Order Wizard).\n\n  Error Count: " + errorCount, "Order Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
                if (DialogResult.No == ggMessageBox.ShowDialog())
                {
                    this.Close();
                }
                else
                {
                    // Update the row error message for this accession row...
                    if (string.IsNullOrEmpty(((DataRowView)_orderRequestBindingSource.Current).Row.RowError))
                    {
                        ux_textboxOrderRequestRowError.Visible = false;
                        ux_textboxOrderRequestRowError.Text = "";
                    }
                    else
                    {
                        ux_textboxOrderRequestRowError.Visible = true;
                        ux_textboxOrderRequestRowError.ReadOnly = false;
                        ux_textboxOrderRequestRowError.Enabled = true;
                        ux_textboxOrderRequestRowError.Text = ((DataRowView)_orderRequestBindingSource.Current).Row.RowError;
                    }
                }
            }
        }

        private void RefreshOrderData()
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Check to see if there is unsaved data and if some is found - ask to save before refreshing data...
            int intRowEdits = 0;

            _orderRequestBindingSource.EndEdit();
            if (_orderRequest != null && _orderRequest.GetChanges() != null) intRowEdits = _orderRequest.GetChanges().Rows.Count;
            _orderRequestItemBindingSource.EndEdit();
            if (_orderRequestItem != null && _orderRequestItem.GetChanges() != null) intRowEdits += _orderRequestItem.GetChanges().Rows.Count;
            _orderRequestActionBindingSource.EndEdit();
            if (_orderRequestAction != null && _orderRequestAction.GetChanges() != null) intRowEdits += _orderRequestAction.GetChanges().Rows.Count;
            //_webOrderRequestBindingSource.EndEdit();
            //if (_webOrderRequest.GetChanges() != null) intRowEdits += _webOrderRequest.GetChanges().Rows.Count;

            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s) that will be lost.\n\nWould you like to save them now?", "Save Edits", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "OrderWizard_ux_radiobuttonOrderFilterMessage1";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
            if (intRowEdits > 0 && DialogResult.Yes == ggMessageBox.ShowDialog())
            {
                SaveOrderData();
            }

            Cursor.Current = Cursors.WaitCursor;

            // Refresh the Order data...
            ////DataSet ds;
            ////// Refresh the order_request table and bind it to the main form on the General tabpage...
            ////ds = _sharedUtils.GetWebServiceData("get_order_request", _orderRequestPKeys, 0, 0);
            ////if (ds.Tables.Contains("get_order_request"))
            ////{
            ////    _orderRequest = ds.Tables["get_order_request"].Copy();
            ////    _orderRequestBindingSource.DataSource = _orderRequest;
            ////}

            ////// Get the order_request_item table and bind it to the main form on the General tabpage...
            ////ds = _sharedUtils.GetWebServiceData("order_wizard_get_order_request_item", _orderRequestPKeys, 0, 0);
            ////if (ds.Tables.Contains("order_wizard_get_order_request_item"))
            ////{
            ////    _orderRequestItem = ds.Tables["order_wizard_get_order_request_item"].Copy();
            ////    ux_datagridviewOrderRequestItem.DataSource = _orderRequestItemBindingSource;
            ////    _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestItem, _orderRequestItem);
            ////}

            //            DataSet ds;
            //            string dataviewParameters = "";
            //            string orderRequestPKeys = "";

            //            if (ux_radiobuttonMyOrders.Checked)
            //            {
            //                dataviewParameters = ":ownedby=" + _sharedUtils.UserCooperatorID;
            //            }
            //            else if (ux_radiobuttonMySitesOrders.Checked)
            //            {
            //                string siteCooperatorIDs = "";
            //                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled AND site = @site", "@accountisenabled=Y; @site=" + _sharedUtils.UserSite);
            //                foreach (DataRow dr in dt.Rows)
            //                {
            //                    siteCooperatorIDs += dr["value_member"].ToString() + ",";
            //                }
            //                siteCooperatorIDs = siteCooperatorIDs.TrimEnd(',');
            //                dataviewParameters = ":ownedby=" + siteCooperatorIDs;
            //            }
            //            else if (ux_radiobuttonAllSitesOrders.Checked)
            //            {
            //                string allCooperatorIDs = "";
            //                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", "@accountisenabled=Y;");
            //                foreach (DataRow dr in dt.Rows)
            //                {
            //                    allCooperatorIDs += dr["value_member"].ToString() + ",";
            //                }
            //                allCooperatorIDs = allCooperatorIDs.TrimEnd(',');
            //                dataviewParameters = ":ownedby=" + allCooperatorIDs;
            //            }
            //            else // ux_radiobuttonSelectionOrders.Checked must be true...
            //            {
            //                dataviewParameters = _originalPKeys;
            //            }

            //            // Add on status filters to the dataview parameters...
            //            dataviewParameters += ";:orderitemstatus=" + _orderRequestStatusFilter;

            //            // Get the order_request table and bind it to the main form on the General tabpage...
            //            ds = _sharedUtils.GetWebServiceData("get_order_request", dataviewParameters, 0, 10000);
            //            if (ds.Tables.Contains("get_order_request"))
            //            {
            //                _orderRequest = ds.Tables["get_order_request"].Copy();
            ////_orderRequestBindingSource.DataSource = _orderRequest;
            //                // Build a list of order_request_ids to use for gathering the order_request_items...
            //                orderRequestPKeys = ":orderrequestid=";
            //                foreach (DataRow dr in _orderRequest.Rows)
            //                {
            //                    orderRequestPKeys += dr["order_request_id"].ToString() + ",";
            //                }
            //                orderRequestPKeys = orderRequestPKeys.TrimEnd(',');
            //            }
            //            else
            //            {
            //                _orderRequest = new DataTable();
            //            }

            //            // Get the order_request_item table and bind it to the main form on the General tabpage...
            //            ds = _sharedUtils.GetWebServiceData("order_wizard_get_order_request_item", orderRequestPKeys, 0, 0);
            //            if (ds.Tables.Contains("order_wizard_get_order_request_item"))
            //            {
            //                _orderRequestItem = ds.Tables["order_wizard_get_order_request_item"].Copy();
            //                ux_datagridviewOrderRequestItem.DataSource = _orderRequestItemBindingSource;
            //                _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestItem, _orderRequestItem);
            //            }
            //            else
            //            {
            //                _orderRequestItem = new DataTable();
            //            }

            //            // Re-bind the bindingsource to the new table of web orders...
            //            // NOTE: this will force a change in the child table filter and this should not happen
            //            //       until all of the child table DGV have been built, so this should be done last...
            //            _orderRequestBindingSource.DataSource = _orderRequest;

            // Calculate the Order Request Item Status filter...
            if (ux_tabcontrolMain.SelectedTab == OrderPage ||
                ux_tabcontrolMain.SelectedTab == ActionsPage)
            {
                _orderRequestStatusFilter = " AND (";
                // Process the listbox items that are checked...
                for (int i = 0; i < ux_checkedlistboxOrderItemStatus.Items.Count; i++)
                {
                    if (ux_checkedlistboxOrderItemStatus.GetItemChecked(i))
                    {
                        // Add all listbox items that are checked...
                        _orderRequestStatusFilter += "@order_request_item.status_code='" + _orderRequestStatusCodes.Rows[i]["value_member"].ToString() + "' OR ";
                    }
                }
                // Drop the extra semicolon at the end of the string...
                if (_orderRequestStatusFilter.Length > 6)
                {
                    _orderRequestStatusFilter = _orderRequestStatusFilter.Remove(_orderRequestStatusFilter.Length - 4) + ")";
                }
                else
                {
                    _orderRequestStatusFilter = "";
                }
            }

            // Next get the order type filter...
            if (ux_comboboxOrderTypeFilter.SelectedValue != null &&
                ux_comboboxOrderTypeFilter.SelectedValue != DBNull.Value)
            {
                _orderRequestStatusFilter += " AND @order_request.order_type_code = '" + ux_comboboxOrderTypeFilter.SelectedValue.ToString().Trim() + "'";
            }

            // Now calculate the Order Request Date filter...
            if (!string.IsNullOrEmpty(ux_textboxOrderDateFilter.Text))
            {
                if (ux_textboxOrderDateFilter.Text.Contains("*") ||
                    ux_textboxOrderDateFilter.Text.Contains("%"))
                {
                    _orderRequestStatusFilter += " AND @order_request.ordered_date LIKE '" + ux_textboxOrderDateFilter.Text.Replace("*", "%").Replace("'", "") + "'";
                }
                else if (ux_textboxOrderDateFilter.Text.Trim().StartsWith("=") ||
                    ux_textboxOrderDateFilter.Text.Trim().StartsWith("<") ||
                    ux_textboxOrderDateFilter.Text.Trim().StartsWith(">"))
                {
                    _orderRequestStatusFilter += " AND @order_request.ordered_date " + ux_textboxOrderDateFilter.Text.Replace(" ", "").Replace("<", "<'").Replace(">", ">'").Replace("=", "='") + "'";
                }
                else
                {
                    _orderRequestStatusFilter += " AND @order_request.ordered_date = '" + ux_textboxOrderDateFilter.Text + "'";
                }
            }

            _orderRequestPKeys = "";
            string seQueryString = "";
//if (string.IsNullOrEmpty(_orderRequestStatusFilter)) _orderRequestStatusFilter = "null";
            if (ux_radiobuttonMyOrders.Checked)
            {
                //seQueryString = "@order_request.owned_by=" + _sharedUtils.UserCooperatorID + " AND @order_request_item.status_code IN (" + _orderRequestStatusFilter + ")";
                seQueryString = "@order_request.owned_by=" + _sharedUtils.UserCooperatorID + _orderRequestStatusFilter;
            }
            else if (ux_radiobuttonMySitesOrders.Checked)
            {
                string siteCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled AND site = @site", "@accountisenabled=Y; @site=" + _sharedUtils.UserSite);
                foreach (DataRow dr in dt.Rows)
                {
                    siteCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                siteCooperatorIDs = siteCooperatorIDs.TrimEnd(',');
                if (string.IsNullOrEmpty(siteCooperatorIDs)) siteCooperatorIDs = "null";
                seQueryString = "@order_request.owned_by IN (" + siteCooperatorIDs + ")" + _orderRequestStatusFilter;
            }
            else if (ux_radiobuttonAllSitesOrders.Checked)
            {
                string allCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", "@accountisenabled=Y;");
                foreach (DataRow dr in dt.Rows)
                {
                    allCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                allCooperatorIDs = allCooperatorIDs.TrimEnd(',');
                if (string.IsNullOrEmpty(allCooperatorIDs)) allCooperatorIDs = "null";
                seQueryString = "@order_request.owned_by IN (" + allCooperatorIDs + ")" + _orderRequestStatusFilter;
            }
            else // ux_radiobuttonSelectionOrders.Checked must be true...
            {
//string[] originalPKeyTokens = _originalPKeys.Split(new char[] { ';' });
//foreach (string originalPKeyToken in originalPKeyTokens)
//{
//    if (originalPKeyToken.Contains(":orderrequestid="))
//    {
//        string orderRequestIDs = originalPKeyToken.Replace(":orderrequestid=", "").Trim();
//        if (string.IsNullOrEmpty(orderRequestIDs)) orderRequestIDs = "null";
//        //seQueryString = "@order_request.order_request_id IN (" + orderRequestIDs + ")" + _orderRequestStatusFilter;
//        seQueryString = "@order_request.order_request_id IN (" + orderRequestIDs + ")";
//    }
//}
seQueryString = "@order_request.order_request_id IN (" + _selectionList + ")";
            }

            // Use the Search Engine to find the order_request records...
            DataSet ds = _sharedUtils.SearchWebService(seQueryString, true, true, null, "order_request", 0, 1000);
            // Build the parameter string to pass to the get_order_request, order_wizard_get_order_request_item, and get_order_request_action dataview...
            if (ds.Tables.Contains("SearchResult"))
            {
                // Build a list of order_request_ids to use for gathering the order_request_items...
                _orderRequestPKeys = ":orderrequestid=";
                foreach (DataRow dr in ds.Tables["SearchResult"].Rows)
                {
                    _orderRequestPKeys += dr["ID"].ToString() + ",";
                }
                _orderRequestPKeys = _orderRequestPKeys.TrimEnd(',');
            }

            // Get the order_request_item table and bind it to the main form on the General tabpage...
            ds = _sharedUtils.GetWebServiceData("order_wizard_get_order_request_item", _orderRequestPKeys, 0, 0);
            if (ds.Tables.Contains("order_wizard_get_order_request_item"))
            {
                // Load the order request items from the server DB...
                _orderRequestItem = ds.Tables["order_wizard_get_order_request_item"].Copy();
                // Load as a Read Only dataview to ensure all LU dictionaries have required LUs (this really 
                // speeds up the Edit dataview loading as well as the programmatic populating of sortable fields loaded next)...
                _sharedUtils.BuildReadOnlyDataGridView(new DataGridView(), _orderRequestItem);
                // Now that the LU dictionaries are pre-loaded - build the sortable fields and populate them...
                foreach (DataColumn dc in ds.Tables["order_wizard_get_order_request_item"].Columns)
                {
                    _orderRequestItem.Columns.Add(dc.ColumnName + "_sortable", typeof(string));
                    // Populate the new column...
                    if (_sharedUtils.LookupTablesIsValidFKField(dc))
                    {
                        foreach (DataRow dr in _orderRequestItem.Rows)
                        {
                            string lookupTable = dc.ExtendedProperties["foreign_key_dataview_name"].ToString();
                            if (dr[dc.ColumnName] != DBNull.Value)
                            {
                                dr[dc.ColumnName + "_sortable"] = _sharedUtils.GetLookupDisplayMember(lookupTable, dr[dc.ColumnName].ToString(), "", dr[dc.ColumnName].ToString());
                            }
                        }
                    }
                    else if (_sharedUtils.LookupTablesIsValidCodeValueField(dc))
                    {
                        foreach (DataRow dr in _orderRequestItem.Rows)
                        {
                            if (dr[dc.ColumnName] != DBNull.Value)
                            {
                                dr[dc.ColumnName + "_sortable"] = _sharedUtils.GetLookupDisplayMember("code_value_lookup", dr[dc.ColumnName].ToString(), dc.ExtendedProperties["group_name"].ToString(), dr[dc.ColumnName].ToString());
                            }
                        }
                    }
                    else if (dc.DataType == typeof(int) || dc.DataType == typeof(decimal))
                    {
                        foreach (DataRow dr in _orderRequestItem.Rows)
                        {
                            if (dr[dc.ColumnName] != DBNull.Value)
                            {
                                dr[dc.ColumnName + "_sortable"] = decimal.Parse(dr[dc.ColumnName].ToString()).ToString("00000000000000000000.0000000000");
                            }
                        }
                    }
                    else if (dc.DataType == typeof(DateTime))
                    {
                        foreach (DataRow dr in _orderRequestItem.Rows)
                        {
                            if (dr[dc.ColumnName] != DBNull.Value)
                            {
                                dr[dc.ColumnName + "_sortable"] = ((DateTime)dr[dc.ColumnName]).ToString("u");
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in _orderRequestItem.Rows)
                        {
                            if (dr[dc.ColumnName] != DBNull.Value)
                            {
                                dr[dc.ColumnName + "_sortable"] = dr[dc.ColumnName].ToString().ToLower();
                            }
                        }
                    }
                }
            }
            else
            {
                _orderRequestItem = new DataTable();
            }
            // Accept all the changes to the bound datatable (should only be the new sortable columns just added)...
            _orderRequestItemBindingSource.EndEdit();
            _orderRequestItem.AcceptChanges();
            ux_datagridviewOrderRequestItem.DataSource = _orderRequestItemBindingSource;
            // Now that the LU dictionaries are primed load the Edit dataview
            _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestItem, _orderRequestItem);
            // Set Sortmode for the columns...
            foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestItem.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.Programmatic;
                dgvc.HeaderCell.ContextMenuStrip = ux_contextmenustripDGVHeader;
                //dgvc.ContextMenuStrip = ux_contextmenustripReadOnlyDGVCell;
                //dgvc.HeaderText = _sharedUtils.GetFriendlyFieldName(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Columns[dgvc.Index], ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Columns[dgvc.Index].ColumnName);
                if (dgvc.Name.EndsWith("_sortable")) dgvc.Visible = false;
            }

            // Get the order_request_action table and bind it to the main form on the General tabpage...
            ds = _sharedUtils.GetWebServiceData("order_wizard_get_order_request_action", _orderRequestPKeys, 0, 0);
            if (ds.Tables.Contains("order_wizard_get_order_request_action"))
            {
                _orderRequestAction = ds.Tables["order_wizard_get_order_request_action"].Copy();
            }
            else
            {
                _orderRequestAction = new DataTable();
            }
            ux_datagridviewOrderRequestAction.DataSource = _orderRequestActionBindingSource;
            _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestAction, _orderRequestAction);
            // Set Sortmode for the columns...
            foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                //dgvc.HeaderCell.ContextMenuStrip = ux_contextmenustripDGVHeader;
                //dgvc.ContextMenuStrip = ux_contextmenustripReadOnlyDGVCell;
                //dgvc.HeaderText = _sharedUtils.GetFriendlyFieldName(((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Columns[dgvc.Index], ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Columns[dgvc.Index].ColumnName);
            }

            // Get the order_request table and bind it to the main form on the General tabpage...
            ds = _sharedUtils.GetWebServiceData("order_wizard_get_order_request", _orderRequestPKeys, 0, 0);
            if (ds.Tables.Contains("order_wizard_get_order_request"))
            {
                _orderRequest = ds.Tables["order_wizard_get_order_request"].Copy();
            }
            else
            {
                _orderRequest = new DataTable();
            }
            _orderRequestBindingSource.DataSource = _orderRequest;

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void RefreshWebOrderData()
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Calculate the Order Request Item Status filter...
//if (ux_tabcontrolMain.SelectedTab == WebOrderPage)                
            if (ux_checkedlistboxWebOrderItemStatus.Items.Count > 0)
                {
                _webOrderRequestStatusFilter = " AND (";
                // Process the listbox item that is changing by peeking at its 'new value'...
                for (int i = 0; i < ux_checkedlistboxWebOrderItemStatus.Items.Count; i++)
                {
                    if (ux_checkedlistboxWebOrderItemStatus.GetItemChecked(i))
                    {
                        // Add all listbox items that are checked...
                        _webOrderRequestStatusFilter += "@web_order_request.status_code='" + _webOrderRequestStatusCodes.Rows[i]["value_member"].ToString() + "' OR ";
                    }
                }
                // Drop the extra semicolon at the end of the string...
                if (_webOrderRequestStatusFilter.Length > 6)
                {
                    _webOrderRequestStatusFilter = _webOrderRequestStatusFilter.Remove(_webOrderRequestStatusFilter.Length - 4) + ")";
                }
                else
                {
                    _webOrderRequestStatusFilter = "";
                }
            }

            // Now calculate the Order Request Date filter...
            if (!string.IsNullOrEmpty(ux_textboxWebOrderDateFilter.Text))
            {
                if (ux_textboxWebOrderDateFilter.Text.Contains("*") ||
                    ux_textboxWebOrderDateFilter.Text.Contains("%"))
                {
                    _webOrderRequestStatusFilter += " AND @web_order_request.ordered_date LIKE '" + ux_textboxWebOrderDateFilter.Text.Replace("*", "%").Replace("'", "") + "'";
                }
                else if (ux_textboxWebOrderDateFilter.Text.Trim().StartsWith("=") ||
                    ux_textboxWebOrderDateFilter.Text.Trim().StartsWith("<") ||
                    ux_textboxWebOrderDateFilter.Text.Trim().StartsWith(">"))
                {
                    _webOrderRequestStatusFilter += " AND @web_order_request.ordered_date " + ux_textboxWebOrderDateFilter.Text.Replace(" ", "").Replace("<", "<'").Replace(">", ">'").Replace("=", "='") + "'";
                }
                else
                {
                    _webOrderRequestStatusFilter += " AND @web_order_request.ordered_date = '" + ux_textboxWebOrderDateFilter.Text + "'";
                }
            }

//// Refresh the Web Order data...
//string dataviewParameters = "";
//string webOrderRequestPKeys = "";

//if (ux_radiobuttonMyWebOrders.Checked)
//{
//    dataviewParameters = ":ownedby=" + _sharedUtils.UserCooperatorID;
//}
//else if (ux_radiobuttonMySitesWebOrders.Checked)
//{
//    string siteCooperatorIDs = "";
//    DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled AND site = @site", "@accountisenabled=Y; @site=" + _sharedUtils.UserSite);
//    foreach (DataRow dr in dt.Rows)
//    {
//        siteCooperatorIDs += dr["value_member"].ToString() + ",";
//    }
//    siteCooperatorIDs = siteCooperatorIDs.TrimEnd(',');
//    dataviewParameters = ":ownedby=" + siteCooperatorIDs;
//}
//else if (ux_radiobuttonAllSitesWebOrders.Checked)
//{
//    string allCooperatorIDs = "";
//    DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", "@accountisenabled=Y;");
//    foreach (DataRow dr in dt.Rows)
//    {
//        allCooperatorIDs += dr["value_member"].ToString() + ",";
//    }
//    allCooperatorIDs = allCooperatorIDs.TrimEnd(',');
//    dataviewParameters = ":ownedby=" + allCooperatorIDs;
//}
//else // ux_radiobuttonSelectionOrders.Checked must be true...
//{
//    dataviewParameters = _originalPKeys;
//}

            string webOrderRequestPKeys = "";
            string seQueryString = "";
            if (ux_radiobuttonMyWebOrders.Checked)
            {
                seQueryString = "@accession.owned_by=" + _sharedUtils.UserCooperatorID + _webOrderRequestStatusFilter;
            }
            else if (ux_radiobuttonMySitesWebOrders.Checked)
            {
                string siteCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled AND site = @site", "@accountisenabled=Y; @site=" + _sharedUtils.UserSite);
                foreach (DataRow dr in dt.Rows)
                {
                    siteCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                siteCooperatorIDs = siteCooperatorIDs.TrimEnd(',');
                if (string.IsNullOrEmpty(siteCooperatorIDs)) siteCooperatorIDs = "null";
                seQueryString = "@accession.owned_by IN (" + siteCooperatorIDs + ")" + _webOrderRequestStatusFilter;
            }
            else if (ux_radiobuttonAllSitesWebOrders.Checked)
            {
                string allCooperatorIDs = "";
                DataTable dt = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", "@accountisenabled=Y;");
                foreach (DataRow dr in dt.Rows)
                {
                    allCooperatorIDs += dr["value_member"].ToString() + ",";
                }
                allCooperatorIDs = allCooperatorIDs.TrimEnd(',');
                if (string.IsNullOrEmpty(allCooperatorIDs)) allCooperatorIDs = "null";
                seQueryString = "@accession.owned_by IN (" + allCooperatorIDs + ")" + _webOrderRequestStatusFilter;
            }

            // Use the Search Engine to find the order_request records...
            DataSet ds = _sharedUtils.SearchWebService(seQueryString, true, true, null, "web_order_request", 0, 0);
            // Build the parameter string to pass to the get_order_request, order_wizard_get_order_request_item, and get_order_request_action dataview...
            if (ds.Tables.Contains("SearchResult"))
            {
                // Build a list of order_request_ids to use for gathering the order_request_items...
                webOrderRequestPKeys = ":weborderrequestid=";
                foreach (DataRow dr in ds.Tables["SearchResult"].Rows)
                {
                    webOrderRequestPKeys += dr["ID"].ToString() + ",";
                }
                webOrderRequestPKeys = webOrderRequestPKeys.TrimEnd(',');
            }

            // Get the web_order_request table and bind it to the main form on the OrdersPage tabpage...
            ds = _sharedUtils.GetWebServiceData("order_wizard_get_web_order_request", webOrderRequestPKeys, 0, 0);
            if (ds.Tables.Contains("order_wizard_get_web_order_request"))
            {
                _webOrderRequest = ds.Tables["order_wizard_get_web_order_request"].Copy();
            }
            else
            {
                _webOrderRequest = new DataTable();
            }

            // Get the web_order_request_item table and bind it to the dgv on the WebOrdersPage tabpage...
            ds = _sharedUtils.GetWebServiceData("order_wizard_get_web_order_request_item", webOrderRequestPKeys, 0, 0);
            if (ds.Tables.Contains("order_wizard_get_web_order_request_item"))
            {
                _webOrderRequestItem = ds.Tables["order_wizard_get_web_order_request_item"].Copy();
                ux_datagridviewWebOrderRequestItem.DataSource = _webOrderRequestItemBindingSource;
                _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewWebOrderRequestItem, _webOrderRequestItem);
            }
            else
            {
                _webOrderRequestItem = new DataTable();
            }

            // Re-bind the bindingsource to the new table of web orders...
            // NOTE: this will force a change in the child table filter and this should not happen
            //       until all of the child table DGV have been built, so this should be done last...
            _webOrderRequestBindingSource.DataSource = _webOrderRequest;

            // Enable (or disable) the buttons for creating/deleting orders and cooperators...
            if (_webOrderRequest != null &&
                _webOrderRequest.Rows.Count > 0)
            {
                ux_buttonCreateOrderRequest.Enabled = true;
                ux_checkboxMySitesAccessionsOnly.Enabled = true;
                ux_buttonCreateCooperator.Enabled = true;
                ux_buttonCancelWebOrderRequest.Enabled = true;
                bindingNavigatorAddNewItem1.Enabled = false;
                bindingNavigatorDeleteItem1.Enabled = false;
            }
            else
            {
                ux_buttonCreateOrderRequest.Enabled = false;
                ux_checkboxMySitesAccessionsOnly.Enabled = false;
                ux_buttonCreateCooperator.Enabled = false;
                ux_buttonCancelWebOrderRequest.Enabled = false;
                bindingNavigatorAddNewItem1.Enabled = false;
                bindingNavigatorDeleteItem1.Enabled = false;
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private int SaveOrderData()
        {
            int errorCount = 0;
            DataSet orderRequestChanges = new DataSet();
            DataSet orderRequestSaveResults = new DataSet();
            DataSet orderRequestItemChanges = new DataSet();
            DataSet orderRequestItemSaveResults = new DataSet();
            DataSet orderRequestActionChanges = new DataSet();
            DataSet orderRequestActionSaveResults = new DataSet();
            DataSet webOrderRequestChanges = new DataSet();
            DataSet webOrderRequestSaveResults = new DataSet();
            DataSet inventoryOrderRequestItemChanges = new DataSet();

            // Process Order Requests...
            // Make sure the last edited row in the Order Header Form has been commited to the datatable...
            _orderRequestBindingSource.EndEdit();

            // Make sure the navigator is not still editing a cell...
            foreach (DataRowView drv in _orderRequestBindingSource.List)
            {
                if (drv.IsEdit ||
                    drv.Row.RowState == DataRowState.Added ||
                    drv.Row.RowState == DataRowState.Deleted ||
                    drv.Row.RowState == DataRowState.Detached ||
                    drv.Row.RowState == DataRowState.Modified)
                {

                    drv.EndEdit();
                    //drv.Row.ClearErrors();
                }
            }

            // Get the changes (if any) for the order_request table and commit them to the remote database...
            if (_orderRequest.GetChanges() != null)
            {
                orderRequestChanges.Tables.Add(_orderRequest.GetChanges());
                // Save the changes to the remote server...
                orderRequestSaveResults = _sharedUtils.SaveWebServiceData(orderRequestChanges);
                if (orderRequestSaveResults.Tables.Contains(_orderRequest.TableName))
                {
                    errorCount += SyncSavedResults(_orderRequest, orderRequestSaveResults.Tables[_orderRequest.TableName]);
                }
            }

            // Process Order Request Items...
            // Make sure no DGV cells are being edited...
            ux_datagridviewOrderRequestItem.EndEdit();
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _orderRequestItemBindingSource.EndEdit();

            // Get the changes (if any) for the order_request_item table and commit them to the remote database...
            if (_orderRequestItem.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in order_request_item have
                // a FK related to a new row in the order_request table (pkey < 0).  If so get the new pkey returned from
                // the get_order_reqeust save and update the records in order_request_item...
                DataRow[] orderRequestItemRowsWithNewParent = _orderRequestItem.Select("order_request_id<0");
                foreach (DataRow dr in orderRequestItemRowsWithNewParent)
                {
                    // "OriginalPrimaryKeyID" "NewPrimaryKeyID"
                    DataRow[] newParent = orderRequestSaveResults.Tables["order_wizard_get_order_request"].Select("OriginalPrimaryKeyID=" + dr["order_request_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["order_request_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }

//// Get the Order Request Item table changes...
//DataTable dtItemChanges = _orderRequestItem.Clone();
//// Drop the columns used for sorting...
//foreach (DataColumn dc in _orderRequestItem.Columns)
//{
//    if (dc.ColumnName.EndsWith("_sortable") &&
//        dtItemChanges.Columns.Contains(dc.ColumnName))
//    {
//        dtItemChanges.Columns.Remove(dc.ColumnName);
//    }
//}
//dtItemChanges.AcceptChanges();
//// Now add the changed rows to this new table...
//foreach (DataRow dr in _orderRequestItem.GetChanges().Rows)
//{

//    foreach(DataColumn dc in dtItemChanges.Columns)
//    {

//    }
//}
                DataTable dtItemChanges = _orderRequestItem.GetChanges();
                // Drop the columns used for sorting...
                foreach (DataColumn dc in _orderRequestItem.Clone().Columns)
                {
                    if (dc.ColumnName.EndsWith("_sortable") &&
                        dtItemChanges.Columns.Contains(dc.ColumnName))
                    {
                        dtItemChanges.Columns.Remove(dc.ColumnName);
                    }
                }
                orderRequestItemChanges.Tables.Add(dtItemChanges);
                ScrubData(orderRequestItemChanges);

                // Make a copy of the changed Order Request Items (for processing auto-deduct further down)...
                inventoryOrderRequestItemChanges = orderRequestItemChanges.Copy();

                // For each Order Request Item that has a changed status_code - update the status_date field...
                Dictionary<string, int> statusChangeCounts = new Dictionary<string, int>();
                foreach (DataRow dr in orderRequestItemChanges.Tables[_orderRequestItem.TableName].Rows)
                {
                    // For any status changes - set the status date...
                    if (dr.Table.Columns.Contains("status_code") &&
                        (dr.RowState == DataRowState.Modified && dr["status_code", DataRowVersion.Original].ToString().Trim() != dr["status_code", DataRowVersion.Current].ToString().Trim()))
                    {
//// Since the order request item's status has changed set the status date to UTC Now()...
//dr["status_date"] = DateTime.UtcNow.ToString("d");
//_orderRequestItem.Rows.Find(dr[dr.Table.PrimaryKey[0]])["status_date"] = dr["status_date"];
                        string orderStatusCode = dr["order_request_id", DataRowVersion.Current].ToString() + "|" + dr["status_code", DataRowVersion.Current].ToString();
                        if (statusChangeCounts.ContainsKey(orderStatusCode))
                        {
                            statusChangeCounts[orderStatusCode] = statusChangeCounts[orderStatusCode] + 1;
                        }
                        else
                        {
                            statusChangeCounts.Add(orderStatusCode, 1);
                        }
                    }
                }

                foreach (string orderStatusCode in statusChangeCounts.Keys)
                {
                    // Create an Action for the status_code changes (one per staus_code with a count for how many items were changed to that status)...
                    DataRow newOrderRequestAction = _orderRequestAction.NewRow();
                    string currentOrderRequestPKey = orderStatusCode.Split(new char[] { '|' })[0];
                    string currentOrderRequestAction = orderStatusCode.Split(new char[] { '|' })[1];
                    if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = currentOrderRequestPKey;
                    if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = currentOrderRequestAction;
                    if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "Order Request Item status_code changed by " + _sharedUtils.Username + " to " + currentOrderRequestAction + " for " + statusChangeCounts[orderStatusCode].ToString() + " items.";
                    // And add it to the Order Request Action Table...
                    _orderRequestAction.Rows.Add(newOrderRequestAction);
                }

                // Save the order request item changes to the remote server...
                orderRequestItemSaveResults = _sharedUtils.SaveWebServiceData(orderRequestItemChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (orderRequestItemSaveResults.Tables.Contains(_orderRequestItem.TableName))
                {
                    errorCount += SyncSavedResults(_orderRequestItem, orderRequestItemSaveResults.Tables[_orderRequestItem.TableName]);
                }
            }

            // Check to see if any Order Request Item rows status_code column has changed to 'SHIPPED'
            // and if so auto-deduct the inventory (when auto_deduct is set in the inventory table)...
            if (orderRequestItemChanges.Tables.Contains(_orderRequestItem.TableName) &&
                orderRequestItemSaveResults.Tables.Contains(_orderRequestItem.TableName))
            //if (inventoryOrderRequestItemChanges.Tables.Contains(_orderRequestItem.TableName))
            {
                foreach (DataRow dr in orderRequestItemChanges.Tables[_orderRequestItem.TableName].Rows)
                //foreach (DataRow dr in inventoryOrderRequestItemChanges.Tables[_orderRequestItem.TableName].Rows)
                {
                    // Pull the results from the save re
                    // For status changes of 'SHIPPED' - auto deduct the inventory amount...
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        int origPKey = (int)dr[dr.Table.PrimaryKey[0].ColumnName];
                        DataRow saveResultsDR = orderRequestItemSaveResults.Tables[_orderRequestItem.TableName].Rows.Find(origPKey);
                        int newPKey = origPKey;
                        string savedStatus = "";
                        if (saveResultsDR != null)
                        {
                            newPKey = (int)saveResultsDR["NewPrimaryKeyID"];
                            savedStatus = saveResultsDR["SavedStatus"].ToString().Trim().ToUpper();
                        }
                        string originalStatus = "";
                        if (dr.RowState == DataRowState.Modified)
                        {
                            originalStatus = dr["status_code", DataRowVersion.Original].ToString().Trim().ToUpper();
                        }
                        string currentStatus = dr["status_code", DataRowVersion.Current].ToString().Trim().ToUpper();
                        if (savedStatus == "SUCCESS" &&
                            dr.RowState != DataRowState.Deleted &&
                            dr.Table.Columns.Contains("status_code") &&
                            dr.Table.Columns.Contains("inventory_id") &&
                            ((currentStatus == "INSPECT" && dr.RowState == DataRowState.Added) ||
                             (currentStatus == "INSPECT" && dr.RowState == DataRowState.Modified && originalStatus != currentStatus && originalStatus != "SHIPPED") ||
                             (currentStatus == "SHIPPED" && dr.RowState == DataRowState.Added) ||
                             (currentStatus == "SHIPPED" && dr.RowState == DataRowState.Modified && originalStatus != currentStatus && originalStatus != "INSPECT")
                            ))
                        {
                            // Go get the inventory record...
                            DataSet orderItemInventoryRow = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + dr["inventory_id"], 0, 0);
                            if (orderItemInventoryRow != null &&
                                orderItemInventoryRow.Tables.Contains("get_inventory") &&
                                orderItemInventoryRow.Tables["get_inventory"].Rows.Count > 0)
                            {
                                foreach (DataRow inventoryRow in orderItemInventoryRow.Tables["get_inventory"].Rows)
                                {
                                    if (orderItemInventoryRow.Tables["get_inventory"].Columns.Contains("quantity_on_hand") &&
                                        orderItemInventoryRow.Tables["get_inventory"].Columns.Contains("quantity_on_hand_unit_code") &&
                                        inventoryRow["quantity_on_hand_unit_code"].ToString().Trim().ToUpper() == dr["quantity_shipped_unit_code"].ToString().Trim().ToUpper() &&
                                        orderItemInventoryRow.Tables["get_inventory"].Columns.Contains("is_auto_deducted") &&
                                        inventoryRow["is_auto_deducted"].ToString().Trim().ToUpper() == "Y")
                                    {
                                        // Deduct the inventory amount if distribution units match...
                                        inventoryRow["quantity_on_hand"] = (decimal)inventoryRow["quantity_on_hand"] - (decimal)dr["quantity_shipped"];
                                        if (_orderRequestItem.Columns.Contains("quantity_on_hand"))
                                        {
                                            bool origReadOnlyValue = false;
                                            origReadOnlyValue = _orderRequestItem.Columns["quantity_on_hand"].ReadOnly;
                                            _orderRequestItem.Columns["quantity_on_hand"].ReadOnly = false;
                                            _orderRequestItem.Rows.Find(newPKey)["quantity_on_hand"] = inventoryRow["quantity_on_hand"];
                                            _orderRequestItem.Rows.Find(newPKey).AcceptChanges();
                                            _orderRequestItem.Columns["quantity_on_hand"].ReadOnly = origReadOnlyValue;
                                        }
                                    }
                                }
                                // Save the inventory deductions to the remote server...
                                DataSet invSaveResults = _sharedUtils.SaveWebServiceData(orderItemInventoryRow);
                                // If there are errors in saving the inventory record - manually increment the error count and add the error message to the base record...
                                if (invSaveResults.Tables.Contains("ExceptionTable") && invSaveResults.Tables["ExceptionTable"].Rows.Count > 0)
                                {
                                    errorCount++;
                                    string errorMsg = "\tError auto-deducting inventory amount.  Error Message: " + invSaveResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
                                    _orderRequestItem.Rows.Find(dr["order_request_item_id"]).RowError += errorMsg;
                                }
                            }
                        }
                    }
                }
            }

            // Process Order Request Actions...
            // Make sure no DGV cells are being edited...
            ux_datagridviewOrderRequestAction.EndEdit();
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _orderRequestActionBindingSource.EndEdit();

            // Get the changes (if any) for the order_request_action table and commit them to the remote database...
            if (_orderRequestAction.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in order_request_item have
                // a FK related to a new row in the order_request table (pkey < 0).  If so get the new pkey returned from
                // the get_order_reqeust save and update the records in order_request_item...
                DataRow[] orderRequestActionRowsWithNewParent = _orderRequestAction.Select("order_request_id<0");
                foreach (DataRow dr in orderRequestActionRowsWithNewParent)
                {
                    // "OriginalPrimaryKeyID" "NewPrimaryKeyID"
                    DataRow[] newParent = orderRequestSaveResults.Tables["order_wizard_get_order_request"].Select("OriginalPrimaryKeyID=" + dr["order_request_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["order_request_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }

                orderRequestActionChanges.Tables.Add(_orderRequestAction.GetChanges());
                ScrubData(orderRequestActionChanges);

                // Save the order request action changes to the remote server...
                orderRequestActionSaveResults = _sharedUtils.SaveWebServiceData(orderRequestActionChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (orderRequestActionSaveResults.Tables.Contains(_orderRequestAction.TableName))
                {
                    errorCount += SyncSavedResults(_orderRequestAction, orderRequestActionSaveResults.Tables[_orderRequestAction.TableName]);
                }
            }



            // Process Web Order Requests...
            // Make sure the last edited value in the Form has been commited to the datatable...
            _webOrderRequestBindingSource.EndEdit();

            // Get the changes (if any) for the web_order_request table and commit them to the remote database...
            if (_webOrderRequest != null && _webOrderRequest.GetChanges() != null)
            {
                webOrderRequestChanges.Tables.Add(_webOrderRequest.GetChanges());
                ScrubData(webOrderRequestChanges);

                // Save the web order request changes to the remote server...
                webOrderRequestSaveResults = _sharedUtils.SaveWebServiceData(webOrderRequestChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (webOrderRequestSaveResults.Tables.Contains(_webOrderRequest.TableName))
                {
                    //errorCount += SyncSavedResults(_webOrderRequest, webOrderRequestSaveResults.Tables[_webOrderRequest.TableName]);
                    SyncSavedResults(_webOrderRequest, webOrderRequestSaveResults.Tables[_webOrderRequest.TableName]);
                }
            }



            // Now add the new changes to the _changedRecords dataset (this data will be passed back to the calling program)...
            if (orderRequestSaveResults != null && orderRequestSaveResults.Tables.Contains(_orderRequest.TableName))
            {
                string pkeyName = orderRequestSaveResults.Tables[_orderRequest.TableName].PrimaryKey[0].ColumnName;
                bool origColumnReadOnlyValue = orderRequestSaveResults.Tables[_orderRequest.TableName].Columns[pkeyName].ReadOnly;
                foreach (DataRow dr in orderRequestSaveResults.Tables[_orderRequest.TableName].Rows)
                {
                    if (dr["SavedAction"].ToString().ToUpper() == "INSERT" &&
                        dr["SavedStatus"].ToString().ToUpper() == "SUCCESS")
                    {
                        dr.Table.Columns[pkeyName].ReadOnly = false;
                        dr[pkeyName] = dr["NewPrimaryKeyID"];
                        dr.AcceptChanges();
                    }
                }
                orderRequestSaveResults.Tables[_orderRequest.TableName].Columns[pkeyName].ReadOnly = origColumnReadOnlyValue;

                if (_changedRecords.Tables.Contains(_orderRequest.TableName))
                {
                    // If the saved results table exists - update or insert the new records...
                    _changedRecords.Tables[_orderRequest.TableName].Load(orderRequestSaveResults.Tables[_orderRequest.TableName].CreateDataReader(), LoadOption.Upsert);
                    _changedRecords.Tables[_orderRequest.TableName].AcceptChanges();

                }
                else
                {
                    // If the saved results table doesn't exist - create it (and include the new records)...
                    _changedRecords.Tables.Add(orderRequestSaveResults.Tables[_orderRequest.TableName].Copy());
                    _changedRecords.AcceptChanges();
                }
            }

            return errorCount;
        }

        private void ScrubData(DataSet ds)
        {
            // Make sure all non-nullable fields do not contain a null value - if they do, replace it with the default value...
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.ExtendedProperties.Contains("is_nullable") &&
                                dc.ExtendedProperties["is_nullable"].ToString().Trim().ToUpper() == "N" &&
                                dr[dc] == DBNull.Value)
                            {
                                if (dc.ExtendedProperties.Contains("default_value") &&
                                    !string.IsNullOrEmpty(dc.ExtendedProperties["default_value"].ToString()) &&
                                    dc.ExtendedProperties["default_value"].ToString().Trim().ToUpper() != "{DBNULL.VALUE}")
                                {
                                    dr[dc] = dc.ExtendedProperties["default_value"].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        private int SyncSavedResults(DataTable originalTable, DataTable savedResults)
        {
            int errorCount = 0;

            if (savedResults != null && savedResults.PrimaryKey.Length == 1)
            {
                string pKeyCol = savedResults.PrimaryKey[0].ColumnName.Trim().ToUpper();
                savedResults.Columns[pKeyCol].ReadOnly = false;
                foreach (DataRow dr in savedResults.Rows)
                {
                    DataRow originalRow = originalTable.Rows.Find(dr["OriginalPrimaryKeyID"]);

                    switch (dr["SavedAction"].ToString())
                    {
                        case "Insert":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Set the originalTable row's status for this new row to committed (and update the pkey with the int returned from the server DB)...
                                if (originalRow != null)
                                {
                                    // Get the new row from the database (because it will have created_by and owned_by data - which is needed for future updates)...
                                    DataSet ds = _sharedUtils.GetWebServiceData(originalTable.TableName, ":" + pKeyCol.Replace("_", "").ToLower() + "=" + dr["NewPrimaryKeyID"].ToString(), 0, 0);
                                    if (ds.Tables.Contains(originalTable.TableName) &&
                                        ds.Tables[originalTable.TableName].Rows.Count > 0)
                                    {
                                        foreach (DataColumn dc in ds.Tables[originalTable.TableName].Columns)
                                        {
                                            if (dc.ReadOnly)
                                            {
                                                originalRow.Table.Columns[dc.ColumnName].ReadOnly = false;
                                                originalRow[dc.ColumnName] = ds.Tables[originalTable.TableName].Rows[0][dc.ColumnName];
                                                originalRow.Table.Columns[dc.ColumnName].ReadOnly = true;
                                            }
                                            else
                                            {
                                                originalRow[dc.ColumnName] = ds.Tables[originalTable.TableName].Rows[0][dc.ColumnName];
                                            }
                                        }
                                        originalRow.AcceptChanges();
                                        originalRow.ClearErrors();
                                    }
                                    else
                                    {
                                        bool origColumnReadOnlyValue = originalRow.Table.Columns[pKeyCol].ReadOnly;
                                        originalRow.Table.Columns[pKeyCol].ReadOnly = false;
                                        originalRow[pKeyCol] = dr["NewPrimaryKeyID"];
                                        originalRow.AcceptChanges();
                                        originalRow.Table.Columns[pKeyCol].ReadOnly = origColumnReadOnlyValue;
                                        originalRow.ClearErrors();
                                    }
                                }
                            }
                            else
                            {
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Update":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                originalRow.AcceptChanges();
                                originalRow.ClearErrors();
                            }
                            else
                            {
                                errorCount++;
                                if (originalRow != null) originalRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                            }
                            break;
                        case "Delete":
                            if (dr["SavedStatus"].ToString() == "Success")
                            {
                                // Set the row's status for this deleted row to committed...
                                if (originalRow != null)
                                {
                                    originalRow.AcceptChanges();
                                    originalRow.ClearErrors();
                                }
                                else
                                {
                                    //DataTable dt = originalTable.GetChanges(DataRowState.Deleted);
                                    //DataRow deletedRow = null;
                                    //foreach (DataRow deldr in dt.Rows)
                                    //{
                                    //    if (deldr[0, DataRowVersion.Original].ToString() == dr["OriginalPrimaryKeyID"].ToString())
                                    //    {
                                    //        deletedRow = deldr;
                                    //    }
                                    //}
                                    DataRow deletedRow = null;
                                    foreach (DataRow deleteddr in originalTable.Rows)
                                    {
                                        if (deleteddr.RowState == DataRowState.Deleted && deleteddr[0, DataRowVersion.Original].Equals(dr["OriginalPrimaryKeyID"]))
                                        {
                                            deletedRow = deleteddr;
                                        }
                                    }
                                    deletedRow.AcceptChanges();
                                    deletedRow.ClearErrors();
                                }
                            }
                            else
                            {
                                errorCount++;
                                // Find the deleted row (NOTE: datatable.rows.find() method does not work on deleted rows)...
                                foreach (DataRow deletedRow in originalTable.Rows)
                                {
                                    if (deletedRow[0, DataRowVersion.Original].Equals(dr["OriginalPrimaryKeyID"]))
                                    {
                                        deletedRow.RejectChanges();
                                        deletedRow.RowError = "\t" + dr["ExceptionMessage"].ToString();
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return errorCount;
        }

        void _orderRequestBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (_orderRequestBindingSource.List.Count > 0)
            {
                ////ux_tabcontrolMain.Enabled = true;
                //OrderPage.Show();
                //ActionsPage.Show();
                OrderPage.Enabled = true;
                ActionsPage.Enabled = true;
            }
            else
            {
                ////ux_tabcontrolMain.Enabled = false;
                //OrderPage.Hide();
                //ActionsPage.Hide();
                OrderPage.Enabled = false;
                ActionsPage.Enabled = false;
            }

            formatControls(OrderPage.Controls, _orderRequestBindingSource);
            ux_tabcontrolMain.SelectTab(OrderPage);
        }

        void _orderRequestBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (_orderRequestBindingSource.List.Count > 0)
            {
                // Update the row error message for this accession row...
                if (string.IsNullOrEmpty(((DataRowView)_orderRequestBindingSource.Current).Row.RowError))
                {
                    ux_textboxOrderRequestRowError.Visible = false;
                    ux_textboxOrderRequestRowError.Text = "";
                }
                else
                {
                    ux_textboxOrderRequestRowError.Visible = true;
                    ux_textboxOrderRequestRowError.ReadOnly = false;
                    ux_textboxOrderRequestRowError.Enabled = true;
                    ux_textboxOrderRequestRowError.Text = ((DataRowView)_orderRequestBindingSource.Current).Row.RowError;
                }
                string pkey = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
                if (_orderRequestItem != null && !string.IsNullOrEmpty(pkey) && _orderRequestItem.Columns.Contains("order_request_id")) _orderRequestItem.DefaultView.RowFilter = "order_request_id=" + pkey.Trim().ToLower();
                if (_orderRequestAction != null && !string.IsNullOrEmpty(pkey) && _orderRequestAction.Columns.Contains("order_request_id"))
                {
                    decimal totalCost = 0;
                    _orderRequestAction.DefaultView.RowFilter = "order_request_id=" + pkey.Trim().ToLower();
                    foreach (DataRowView drv in _orderRequestAction.DefaultView)
                    {
                        decimal itemCost = 0;
                        if (decimal.TryParse(drv["action_cost"].ToString(), out itemCost))
                        {
                            totalCost += itemCost;
                        }
                    }
                    ux_textboxTotalCost.Text = totalCost.ToString("c");
                }
                bindingNavigatorOrderNumber.Text = ((DataRowView)_orderRequestBindingSource.Current).Row["order_request_id"].ToString();
                bindingNavigatorItems.Text = ux_datagridviewOrderRequestItem.Rows.Count.ToString();
            }
        }

        void _webOrderRequestBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (_webOrderRequestBindingSource.List.Count > 0)
            {
                string pkey = ((DataRowView)_webOrderRequestBindingSource.Current)[_webOrderRequest.PrimaryKey[0].ColumnName].ToString();
                //if (_webOrderRequestItem != null && !string.IsNullOrEmpty(pkey)) _webOrderRequestItem.DefaultView.RowFilter = "web_order_request_id=" + pkey.Trim().ToLower();
                if (_webOrderRequestItemBindingSource != null && !string.IsNullOrEmpty(pkey)) ((DataTable)_webOrderRequestItemBindingSource.DataSource).DefaultView.RowFilter = "web_order_request_id=" + pkey.Trim().ToLower();
            }
        }

        #endregion

        #region Tab Control Logic...
        private void ux_tabcontrolMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ux_tabcontrolMain.SelectedIndex == 0)
            if (ux_tabcontrolMain.SelectedTab == OrderPage)
            {
                bindingNavigatorAddNewItem.Enabled = true;
                bindingNavigatorDeleteItem.Enabled = true;
                ux_groupboxOrderFilters.Visible = true;
                ux_groupboxWebOrderFilters.Visible = false;
            }
            else if (ux_tabcontrolMain.SelectedTab == WebOrderPage)
            {
                bindingNavigatorAddNewItem.Enabled = false;
                bindingNavigatorDeleteItem.Enabled = false;
                ux_groupboxOrderFilters.Visible = false;
                ux_groupboxWebOrderFilters.Visible = true;
                bindingNavigatorAddNewItem1.Enabled = false;
                bindingNavigatorDeleteItem1.Enabled = false;
            }
            else
            {
                bindingNavigatorAddNewItem.Enabled = false;
                bindingNavigatorDeleteItem.Enabled = false;
                ux_groupboxOrderFilters.Visible = true;
                ux_groupboxWebOrderFilters.Visible = false;
                //if(((DataRowView)_mainBindingSource.Current).Row.RowState == DataRowState.Detached) ((DataRowView)_mainBindingSource.Current).EndEdit();
                foreach (DataRowView drv in _orderRequestBindingSource.List)
                {
                    if (drv.IsEdit ||
                        drv.Row.RowState == DataRowState.Added ||
                        drv.Row.RowState == DataRowState.Deleted ||
                        drv.Row.RowState == DataRowState.Detached ||
                        drv.Row.RowState == DataRowState.Modified)
                    {
                        drv.EndEdit();
                        //drv.Row.ClearErrors();
                    }
                }
            }
        }
        #endregion

        #region Web Order Request logic...
        private void BuildWebOrderRequestPage()
        {
            //DataSet ds;
            //// Get the order_request_action table and bind it to the DGV on the Names tabpage...
            //ds = _sharedUtils.GetWebServiceData("get_order_request_action", _originalPKeys, 0, 0);
            //if (ds.Tables.Contains("get_order_request_action"))
            //{
            //    // Copy the order_request_action table to a private variable...
            //    _orderRequestAction = ds.Tables["get_order_request_action"].Copy();
            //    // Bind the DGV to the binding source...
            //    ux_datagridviewOrderRequestAction.DataSource = _orderRequestActionBindingSource;
            //    // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
            //    _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestAction, _orderRequestAction);

            //    // Order and display the columns the way the user wants...
            //    int i = 0;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_NAME_CODE")) ux_datagridviewOrderRequestAction.Columns["ACTION_NAME_CODE"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE_CODE"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE_CODE"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_INFORMATION")) ux_datagridviewOrderRequestAction.Columns["ACTION_INFORMATION"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_COST")) ux_datagridviewOrderRequestAction.Columns["ACTION_COST"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("COOPERATOR_ID")) ux_datagridviewOrderRequestAction.Columns["COOPERATOR_ID"].DisplayIndex = i++;
            //    if (ux_datagridviewOrderRequestAction.Columns.Contains("NOTE")) ux_datagridviewOrderRequestAction.Columns["NOTE"].DisplayIndex = i++;

            //    foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
            //    {
            //        dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
            //        // Hide any columns not explicitly ordered in the above code...
            //        if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
            //    }
            //}
        }

        #endregion

        #region Order Request Action logic...
        private void BuildOrderRequestActionsPage()
        {
            DataSet ds;
            // Get the order_request_action table and bind it to the DGV on the Names tabpage...
            ds = _sharedUtils.GetWebServiceData("order_wizard_get_order_request_action", _originalPKeys, 0, 0);
            if (ds.Tables.Contains("order_wizard_get_order_request_action"))
            {
                // Copy the order_request_action table to a private variable...
                _orderRequestAction = ds.Tables["order_wizard_get_order_request_action"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewOrderRequestAction.DataSource = _orderRequestActionBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewOrderRequestAction, _orderRequestAction);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_NAME_CODE")) ux_datagridviewOrderRequestAction.Columns["ACTION_NAME_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("STARTED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["STARTED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COMPLETED_DATE_CODE")) ux_datagridviewOrderRequestAction.Columns["COMPLETED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_INFORMATION")) ux_datagridviewOrderRequestAction.Columns["ACTION_INFORMATION"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("ACTION_COST")) ux_datagridviewOrderRequestAction.Columns["ACTION_COST"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("COOPERATOR_ID")) ux_datagridviewOrderRequestAction.Columns["COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewOrderRequestAction.Columns.Contains("NOTE")) ux_datagridviewOrderRequestAction.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }

                //int i = 5;
                //foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
                //{
                //    switch (dgvc.Name.Trim().ToUpper())
                //    {
                //        case "ACTION_NAME":
                //            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                //            dgvc.DisplayIndex = 0;
                //            break;
                //        case "ACTED_DATE":
                //            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                //            dgvc.DisplayIndex = 1;
                //            break;
                //        case "ACTION_FOR_ID":
                //            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                //            dgvc.DisplayIndex = 2;
                //            break;
                //        case "COOPERATOR_ID":
                //            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                //            dgvc.DisplayIndex = 3;
                //            break;
                //        case "NOTE":
                //            dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                //            dgvc.DisplayIndex = 4;
                //            break;
                //        default:
                //            dgvc.Visible = false;
                //            dgvc.DisplayIndex = System.Math.Min(i++, ux_datagridviewOrderRequestAction.Columns.Count - 1);
                //            break;
                //    }
                //}
            }
        }

        private void ux_buttonNewOrderRequestActionRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
            DataRow newOrderRequestAction = _orderRequestAction.NewRow();
            newOrderRequestAction["order_request_id"] = pkey;
            _orderRequestAction.Rows.Add(newOrderRequestAction);
            //ux_datagridviewOrderRequestAction.CurrentCell = ux_datagridviewOrderRequestAction.Rows[ux_datagridviewOrderRequestAction.Rows.GetLastRow(DataGridViewElementStates.Displayed)].Cells["name"];
            int newRowIndex = ux_datagridviewOrderRequestAction.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewOrderRequestAction.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewOrderRequestAction.Rows.Count; i++)
            {
                if (ux_datagridviewOrderRequestAction["order_request_action_id", i].Value.Equals(newOrderRequestAction["order_request_action_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestAction.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewOrderRequestAction.CurrentCell = ux_datagridviewOrderRequestAction[newColIndex, newRowIndex];
        }
        #endregion

        #region Order Items DGV logic...
        private void ux_datagridviewOrderRequestItem_DragOver(object sender, DragEventArgs e)
        {
            // Okay we are in the middle of a Drag and Drop operation and the mouse is in 
            // the DGV control so lets handle this event...

            // This code will change the cursor icon to give the user feedback about whether or not
            // the drag-drop operation is allowed...
            //

            // Get the DGV object...
            DataGridView dgv = (DataGridView)sender;

            // Convert the mouse coordinates from screen to client...
            Point ptClientCoord = dgv.PointToClient(new Point(e.X, e.Y));

            // Is this a string being dragged to the DGV...
            if (e.Data.GetDataPresent(typeof(string)) && !dgv.ReadOnly)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(typeof(DataSet)) && !dgv.ReadOnly)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ux_datagridviewOrderRequestItem_DragDrop(object sender, DragEventArgs e)
        {
            // The drag-drop event is coming to a close process this event to handle the dropping of
            // data into the treeview...

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Get the DGV object...
            DataGridView dgv = (DataGridView)sender;
            DataTable destinationTable = (DataTable)((BindingSource)dgv.DataSource).DataSource;

            // Is this an allowed drop???
            if (e.Effect != DragDropEffects.None)
            {
                if (e.Data.GetDataPresent(typeof(DataSet)) && e.Effect != DragDropEffects.None)
                {
                    // Is this a collection of dataset rows being dragged to the DGV...
                    DataSet dndData = (DataSet)e.Data.GetData(typeof(DataSet));
                    DataTable sourceTable = dndData.Tables[0];
                    if (sourceTable.PrimaryKey.Length == 1)
                    {
                        if (sourceTable.PrimaryKey[0].ColumnName.ToUpper() == "INVENTORY_ID")
                        {
                            foreach (DataRow dr in sourceTable.Rows)
                            {
                                DataRow newOrderItem = BuildOrderRequestItemRow(dr[sourceTable.PrimaryKey[0].ColumnName].ToString(), destinationTable);
                                if (newOrderItem != null) destinationTable.Rows.Add(newOrderItem);
                            }
                        }
                        else if (sourceTable.PrimaryKey[0].ColumnName.ToUpper() == "ACCESSION_ID")
                        {
                            foreach (DataRow dr in sourceTable.Rows)
                            {
                                string inventoryID = FindInventoryFromAccession(dr[sourceTable.PrimaryKey[0].ColumnName].ToString());
                                DataRow newOrderItem = BuildOrderRequestItemRow(inventoryID, destinationTable);
                                if (newOrderItem != null) destinationTable.Rows.Add(newOrderItem);
                            }
                        }
                    }
                }
                else if (e.Data.GetDataPresent(typeof(string)))
                {
                    // Is this a string being dragged to the DGV...
                    char[] rowDelimiters = new char[] { '\r', '\n' };
                    char[] columnDelimiters = new char[] { '\t', ' ', ',' };
                    string rawText = (string)e.Data.GetData(typeof(string));
                    int badRows = 0;
                    int missingRows = 0;
                    bool importSuccess = false;
                    importSuccess = ImportTextToDataTableUsingAltKeys(rawText, destinationTable, rowDelimiters, columnDelimiters, out badRows, out missingRows);
                }
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private bool ImportTextToDataTableUsingAltKeys(string rawImportText, DataTable destinationTable, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        {
            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            bool processedImportSuccessfully = false;
            badRows = 0;
            missingRows = 0;
            // Make sure there is text to process - if not bail out now...
            if (rawImportRows == null || rawImportRows.Length <= 0) return false;

            // Now start processing the rows...
            for (int i = 0; i < rawImportRows.Length; i++)
            {
                // Split the row into id parts - then reassemble it using only space delimiter...
                string distributionInventoryID = "";
                string[] idParts = rawImportRows[i].Split(columnDelimiters, StringSplitOptions.None);
                string fullID = "";
                foreach (string idPart in idParts)
                {
                    fullID += " " + idPart.Trim();
                }
                // Try to find a matching accession number first...
                string accessionPKey = _sharedUtils.GetLookupValueMember(null, "accession_lookup", fullID.Trim(), "", "");
                if (!string.IsNullOrEmpty(accessionPKey))
                {
                    // Found an accession number - now decide which inventory row to use...
                    distributionInventoryID = FindInventoryFromAccession(accessionPKey);
                    #region old_code...
                    //                    DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + accessionPKey, 0, 0);
                    //                    if (ds.Tables.Contains("get_inventory") &&
                    //                        ds.Tables["get_inventory"].Rows.Count > 0)
                    //                    {
                    //                        // Make sure the rows are ordered oldest to newest...
                    //                        ds.Tables["get_inventory"].DefaultView.Sort = "inventory_id ASC";

                    //                        // Try to find a row that is marked as distributable and has a status of available from the user's site...
                    //                        if (string.IsNullOrEmpty(distributionInventoryID))
                    //                        {
                    ////if (string.IsNullOrEmpty(_sharedUtils.UserSite))
                    ////{
                    ////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    ////        ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    ////    {
                    ////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    ////        ds.Tables["get_inventory"].Columns.Contains("availability_status_code") &&
                    ////        ds.Tables["get_inventory"].Columns.Contains("site"))
                    ////    {
                    ////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL' AND site='" + _sharedUtils.UserSite + "'";
                    ////    }
                    ////}
                    //                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    //                                ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    //                            {
                    //                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    //                            }
                    //                            if (ds.Tables["get_inventory"].Columns.Contains("site") &&
                    //                                string.IsNullOrEmpty(_sharedUtils.UserSite))
                    //                            {
                    //                                ds.Tables["get_inventory"].DefaultView.RowFilter += " AND site='" + _sharedUtils.UserSite + "'";
                    //                            }
                    //                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    //                            {
                    //                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    //                            }
                    //                        }
                    //                        // Couldn't find a row using above criteria - try to find a row that is marked as distributable from the user's site...
                    //                        if (string.IsNullOrEmpty(distributionInventoryID))
                    //                        {
                    ////if (string.IsNullOrEmpty(_sharedUtils.UserSite))
                    ////{
                    ////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    ////    {
                    ////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    ////       ds.Tables["get_inventory"].Columns.Contains("site"))
                    ////    {
                    ////        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND site='" + _sharedUtils.UserSite + "'";
                    ////    }
                    ////}
                    //                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    //                            {
                    //                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    //                            }
                    //                            if (ds.Tables["get_inventory"].Columns.Contains("site") &&
                    //                                string.IsNullOrEmpty(_sharedUtils.UserSite))
                    //                            {
                    //                                ds.Tables["get_inventory"].DefaultView.RowFilter += " AND site='" + _sharedUtils.UserSite + "'";
                    //                            }
                    //                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    //                            {
                    //                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    //                            }
                    //                        }
                    //                        // Couldn't find a row using above criteria - try to find a row that is from the user's site...
                    //                        if (string.IsNullOrEmpty(distributionInventoryID))
                    //                        {
                    ////if (!string.IsNullOrEmpty(_sharedUtils.UserSite))
                    ////{
                    ////    if (ds.Tables["get_inventory"].Columns.Contains("site"))
                    ////    {
                    ////        ds.Tables["get_inventory"].DefaultView.RowFilter = "site='" + _sharedUtils.UserSite + "'";
                    ////    }
                    ////}
                    //                            if (ds.Tables["get_inventory"].Columns.Contains("site") &&
                    //                                string.IsNullOrEmpty(_sharedUtils.UserSite))
                    //                            {
                    //                                ds.Tables["get_inventory"].DefaultView.RowFilter = "site='" + _sharedUtils.UserSite + "'";
                    //                            }
                    //                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    //                            {
                    //                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    //                            }
                    //                        }
                    //                        // Couldn't find a row using above criteria - try to find a row that is marked as distributable and has a status of available from any site...
                    //                        if (string.IsNullOrEmpty(distributionInventoryID))
                    //                        {
                    //                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    //                               ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    //                            {
                    //                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    //                            }
                    //                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    //                            {
                    //                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    //                            }
                    //                        }
                    //                        // Couldn't find a row using above criteria - try to find a row that is marked available from any site...
                    //                        if (string.IsNullOrEmpty(distributionInventoryID))
                    //                        {
                    //                            if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    //                            {
                    //                                ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    //                            }
                    //                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    //                            {
                    //                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    //                            }
                    //                        }
                    //                        // Couldn't find a row using above criteria - use the first row found...
                    //                        if (string.IsNullOrEmpty(distributionInventoryID))
                    //                        {
                    //                            ds.Tables["get_inventory"].DefaultView.RowFilter = "";
                    //                            if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    //                            {
                    //                                distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    //                            }
                    //                        }
                    //                    }
                    #endregion
                }
                else
                {
                    // Couldn't find an accession number so try to find an inventory number...
                    distributionInventoryID = _sharedUtils.GetLookupValueMember(null, "inventory_lookup", fullID.Trim(), "", "");
                }
                // If an inventory id was found add a new row to the order items...
                if (!string.IsNullOrEmpty(distributionInventoryID))
                {
                    DataRow newOrderItem = BuildOrderRequestItemRow(distributionInventoryID, destinationTable);
                    #region old_code...
                    //// Create a new order_request_item row...
                    //DataRow newOrderItem = destinationTable.NewRow();
                    //// Find the maximum sequence number for this order...
                    //int maxSequence = 1;
                    //if (destinationTable.DefaultView.Count > 0)
                    //{
                    //    string currentSort = destinationTable.DefaultView.Sort;
                    //    destinationTable.DefaultView.Sort = "sequence_number DESC";
                    //    if (int.TryParse(destinationTable.DefaultView[0]["sequence_number"].ToString(), out maxSequence))
                    //    {
                    //        maxSequence++;
                    //    }
                    //    else
                    //    {
                    //        maxSequence = 1;
                    //    }
                    //    destinationTable.DefaultView.Sort = currentSort;
                    //}
                    //// Go get all the inventory data for the chosen distribution inventory lot...
                    //DataSet distributionInventory = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + distributionInventoryID, 0, 0);
                    //if (distributionInventory.Tables.Contains("get_inventory") &&
                    //   distributionInventory.Tables["get_inventory"].Rows.Count > 0)
                    //{
                    //    // Now populate the new order_request_item row with the inventory default distribution data...
                    //    DataRow inventoryDataRow = distributionInventory.Tables["get_inventory"].Rows[0];
                    //    newOrderItem["order_request_id"] = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
                    //    newOrderItem["sequence_number"] = maxSequence;
                    //    newOrderItem["name"] = inventoryDataRow["accession_name"].ToString();
                    //    newOrderItem["quantity_shipped"] = inventoryDataRow["distribution_default_quantity"].ToString();
                    //    newOrderItem["quantity_shipped_unit_code"] = inventoryDataRow["distribution_unit_code"].ToString();
                    //    newOrderItem["distribution_form_code"] = inventoryDataRow["distribution_default_form_code"].ToString();
                    //    newOrderItem["availability_status_code"] = inventoryDataRow["availability_status_code"].ToString();
                    //    newOrderItem["inventory_id"] = distributionInventoryID;
                    //    newOrderItem["accession_id"] = inventoryDataRow["accession_id"].ToString();
                    //    newOrderItem["taxonomy_species_id"] = inventoryDataRow["taxonomy_species_id"].ToString();
                    //    // Go get all the accession data for the chosen distribution inventory lot...
                    //    DataSet distributionAccessionIPR = _sharedUtils.GetWebServiceData("get_accession_ipr", ":accessionid=" + inventoryDataRow["accession_id"].ToString(), 0, 0);
                    //    if (distributionAccessionIPR.Tables.Contains("get_accession_ipr") &&
                    //       distributionAccessionIPR.Tables["get_accession_ipr"].Rows.Count > 0)
                    //    {
                    //        // Now populate the new order_request_item row with the accession data...
                    //        distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Sort = "accession_ipr_id ASC";
                    //        distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.RowFilter = "expired_date is null";
                    //        if (distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Count > 0)
                    //        {
                    //            DataRow accessionIPRDataRow = distributionAccessionIPR.Tables["get_accession_ipr"].Rows[0];
                    //            newOrderItem["ipr_restriction"] = accessionIPRDataRow["assigned_type"].ToString();
                    //        }
                    //    }
                    //}
                    #endregion
                    destinationTable.Rows.Add(newOrderItem);
                    processedImportSuccessfully = true;
                }
            }
            return processedImportSuccessfully;
        }

        private bool ImportTextToDataTableUsingBlockStyle(string rawImportText, DataGridView dgv, char[] rowDelimiters, char[] columnDelimiters, out int badRows, out int missingRows)
        {
            bool processedImportSuccessfully = true;
            DataTable destinationTable = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string[] rawImportRows = rawImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            string[] tempColumns = null;
            string newImportText = "";
            string newImportRowText = "";
            badRows = 0;
            missingRows = 0;

            // If the DGV does not have a currently active cell bail out now...
            if (dgv.CurrentCell == null) return false;
            // If the import string is empty bail out now...
            if (string.IsNullOrEmpty(rawImportText) || rawImportRows.Length < 1) return false;

            // Okay we need to build a new importText string that has column headers that include the friendly names for the primary key columns
            // and the friendly names for the dgv columns starting at the currenly active cell in the dgv...  Why are we doing this?  Because
            // we are going to pass this new importText string off to the 'ImportTextToDataTableUsingKeys' method, and since that method
            // requires a primary key or alternate pkey we are going to get them from the dgv starting at the current row of the current cell...

            // Step 1 - Determine the number of rows and columns in the incoming rawImportText (to use later for building the new ImportText string)...
            int rawImportRowCount = 0;
            int rawImportColCount = 0;
            // Estimate the number of rows and columns in the import text (assumes a rectangular shape)
            if (rawImportRows != null && rawImportRows.Length > 0)
            {
                rawImportRowCount = rawImportRows.Length;
                tempColumns = rawImportRows[0].Split(columnDelimiters, StringSplitOptions.None);
                if (tempColumns != null && tempColumns.Length > 0)
                {
                    rawImportColCount = tempColumns.Length;
                }
            }

            int minSelectedCol = dgv.Columns.Count;
            int maxSelectedCol = -1;
            int minSelectedRow = dgv.Rows.Count;
            int maxSelectedRow = -1;
            // Check to see if the datagridview's selected cells contains the CurrentCell
            // and if so use the selected cells as the destination cells...

            // Find the bounding rectangle for the selected cells...
            if (dgv.SelectedCells.Count == 1)
            {
                minSelectedCol = dgv.CurrentCell.ColumnIndex;
                maxSelectedCol = dgv.CurrentCell.ColumnIndex + rawImportColCount - 1;
                minSelectedRow = dgv.CurrentCell.RowIndex;
                maxSelectedRow = dgv.CurrentCell.RowIndex + rawImportRowCount - 1;
            }
            else
            {
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    if (dgvc.ColumnIndex < minSelectedCol) minSelectedCol = dgvc.ColumnIndex;
                    if (dgvc.ColumnIndex > maxSelectedCol) maxSelectedCol = dgvc.ColumnIndex;
                    if (dgvc.RowIndex < minSelectedRow) minSelectedRow = dgvc.RowIndex;
                    if (dgvc.RowIndex > maxSelectedRow) maxSelectedRow = dgvc.RowIndex;
                }
                if ((maxSelectedCol - minSelectedCol) < (rawImportColCount - 1)) maxSelectedCol = minSelectedCol + rawImportColCount - 1;
                if ((maxSelectedRow - minSelectedRow) < (rawImportRowCount - 1)) maxSelectedRow = minSelectedRow + rawImportRowCount - 1;
            }

            string modifiedImportText = "";
            // Now fill (or clip) the import data to fit the selected cells...
            for (int iSelectedRow = 0; iSelectedRow <= (maxSelectedRow - minSelectedRow); iSelectedRow++)
            {
                // 
                tempColumns = rawImportRows[iSelectedRow % rawImportRowCount].Split(columnDelimiters, StringSplitOptions.None);
                for (int iSelectedCol = 0; iSelectedCol <= (maxSelectedCol - minSelectedCol); iSelectedCol++)
                {
                    //
                    modifiedImportText += tempColumns[iSelectedCol % rawImportColCount] + "\t";
                }
                // Strip the last tab character and add a CR LF...
                modifiedImportText = modifiedImportText.Substring(0, modifiedImportText.Length - 1) + "\r\n";
            }

            // Step 2 - Get the primary key column names for the new column header row text...
            if (destinationTable.PrimaryKey.Length > 0)
            {
                foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                {
                    newImportText += _sharedUtils.GetFriendlyFieldName(pKeyColumn, pKeyColumn.ColumnName) + "\t";
                }
            }

            // Step 3 - Continue adding friendly column names to the import text (starting with the column name of the current cell's column HeaderText)...
            //DataGridViewColumn currColumn = dgv.CurrentCell.OwningColumn;
            DataGridViewColumn currColumn = dgv.Columns[minSelectedCol];
            // Step 4 - Now repeat this process for each additional column in the rawImportText...
            //foreach(string tempCol in tempColumns)
            for (int i = 0; i < Math.Max(rawImportColCount, maxSelectedCol - minSelectedCol + 1); i++)
            {
                if (currColumn != null)
                {
                    newImportText += currColumn.HeaderText + "\t";
                }
                else
                {
                    newImportText += "\t";
                }
                // Try to find the next visible column...
                currColumn = dgv.Columns.GetNextColumn(currColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.Frozen);
            }
            // Strip the last tab character and add a CR LF...
            newImportText = newImportText.Substring(0, newImportText.Length - 1) + "\r\n";

            // Step 5 - Get the primary key for each row receiving pasted text and prepend it to the orginal import raw text...
            string[] modifiedImportRows = modifiedImportText.Split(rowDelimiters, StringSplitOptions.RemoveEmptyEntries);
            ////DataGridViewRow currRow = dgv.CurrentCell.OwningRow;
            DataGridViewRow currRow = dgv.Rows[minSelectedRow];
            int nextRowIndex = currRow.Index;
            for (int i = 0; i < modifiedImportRows.Length; i++)
            {
                newImportRowText = "";
                if (currRow != null)
                {
                    if (destinationTable.PrimaryKey.Length > 0)
                    {
                        foreach (DataColumn pKeyColumn in destinationTable.PrimaryKey)
                        {
                            newImportRowText += ((DataRowView)currRow.DataBoundItem).Row[pKeyColumn].ToString() + "\t";
                        }
                    }
                    // Now add the original import row text to the new import row text...
                    //newImportRowText += rawImportRows[i] + "\r\n";
                    newImportRowText += modifiedImportRows[i] + "\r\n";
                    // And now add it to the new import text string...
                    newImportText += newImportRowText;
                }

                // Finally, try to find the next visible row...
                nextRowIndex = dgv.Rows.GetNextRow(currRow.Index, DataGridViewElementStates.Visible);
                if (nextRowIndex != -1 &&
                    !dgv.Rows[nextRowIndex].IsNewRow &&
                    nextRowIndex >= minSelectedRow &&
                    nextRowIndex <= maxSelectedRow)
                {
                    currRow = dgv.Rows[nextRowIndex];
                }
                else
                {
                    // Looks like we hit the end of the rows in the DGV - bailout now...
                    //currRow = null;
                    break;
                }
            }

            // Step 6 - Now that we have built a new ImportText string that contains pkeys, we can pass it off to the 'ImportTextToDataTableUsingKeys' 
            processedImportSuccessfully = _sharedUtils.ImportTextToDataTableUsingKeys(newImportText, destinationTable, rowDelimiters, columnDelimiters, out badRows, out missingRows);

            return processedImportSuccessfully;
        }

        private string FindInventoryFromAccession(string accessionPKey)
        {
            string distributionInventoryID = "";
            DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + accessionPKey, 0, 0);
            if (ds.Tables.Contains("get_inventory") &&
                ds.Tables["get_inventory"].Rows.Count > 0)
            {
                // Make sure the rows are ordered oldest to newest...
                ds.Tables["get_inventory"].DefaultView.Sort = "inventory_id ASC";

                // Try to find a row that is marked as distributable and has a status of available from the user's site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    //if (string.IsNullOrEmpty(_sharedUtils.UserSite))
                    //{
                    //    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    //        ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    //    {
                    //        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    //    }
                    //}
                    //else
                    //{
                    //    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    //        ds.Tables["get_inventory"].Columns.Contains("availability_status_code") &&
                    //        ds.Tables["get_inventory"].Columns.Contains("site"))
                    //    {
                    //        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL' AND site='" + _sharedUtils.UserSite + "'";
                    //    }
                    //}
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                        ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    }
                    if (ds.Tables["get_inventory"].Columns.Contains("owner_site_id") &&
                        string.IsNullOrEmpty(_sharedUtils.UserSite))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter += " AND owner_site_id='" + _sharedUtils.UserSite + "'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is marked as distributable from the user's site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    //if (string.IsNullOrEmpty(_sharedUtils.UserSite))
                    //{
                    //    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    //    {
                    //        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    //    }
                    //}
                    //else
                    //{
                    //    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                    //       ds.Tables["get_inventory"].Columns.Contains("site"))
                    //    {
                    //        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND site='" + _sharedUtils.UserSite + "'";
                    //    }
                    //}
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    }
                    if (ds.Tables["get_inventory"].Columns.Contains("owner_site_id") &&
                        string.IsNullOrEmpty(_sharedUtils.UserSite))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter += " AND owner_site_id='" + _sharedUtils.UserSite + "'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is from the user's site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    //if (!string.IsNullOrEmpty(_sharedUtils.UserSite))
                    //{
                    //    if (ds.Tables["get_inventory"].Columns.Contains("site"))
                    //    {
                    //        ds.Tables["get_inventory"].DefaultView.RowFilter = "site='" + _sharedUtils.UserSite + "'";
                    //    }
                    //}
                    if (ds.Tables["get_inventory"].Columns.Contains("owner_site_id") &&
                        string.IsNullOrEmpty(_sharedUtils.UserSite))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "owner_site_id='" + _sharedUtils.UserSite + "'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is marked as distributable and has a status of available from any site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable") &&
                       ds.Tables["get_inventory"].Columns.Contains("availability_status_code"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y' AND availability_status_code='AVAIL'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - try to find a row that is marked available from any site...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    if (ds.Tables["get_inventory"].Columns.Contains("is_distributable"))
                    {
                        ds.Tables["get_inventory"].DefaultView.RowFilter = "is_distributable = 'Y'";
                    }
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
                // Couldn't find a row using above criteria - use the first row found...
                if (string.IsNullOrEmpty(distributionInventoryID))
                {
                    ds.Tables["get_inventory"].DefaultView.RowFilter = "";
                    if (ds.Tables["get_inventory"].DefaultView.Count > 0)
                    {
                        distributionInventoryID = ds.Tables["get_inventory"].DefaultView[0]["inventory_id"].ToString();
                    }
                }
            }
            return distributionInventoryID;
        }

        private DataRow BuildOrderRequestItemRow(string distributionInventoryID, DataTable destinationTable)
        {
            // Create a new order_request_item row...
            DataRow newOrderItem = destinationTable.NewRow();
            // Find the maximum sequence number for this order...
            int maxSequence = 1;
            if (destinationTable.DefaultView.Count > 0)
            {
                string currentSort = destinationTable.DefaultView.Sort;
                destinationTable.DefaultView.Sort = "sequence_number DESC";
                if (int.TryParse(destinationTable.DefaultView[0]["sequence_number"].ToString(), out maxSequence))
                {
                    maxSequence++;
                }
                else
                {
                    maxSequence = 1;
                }
                destinationTable.DefaultView.Sort = currentSort;
            }
            // Populate the fields not dependent on inventory_id...
            newOrderItem["order_request_id"] = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
            newOrderItem["sequence_number"] = maxSequence;

            // Now go get all the inventory data for the chosen distribution inventory lot...
            DataSet distributionInventory = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + distributionInventoryID, 0, 0);
            if (distributionInventory.Tables.Contains("get_inventory") &&
               distributionInventory.Tables["get_inventory"].Rows.Count > 0)
            {
                // Now populate the new order_request_item row with the inventory default distribution data...
                DataTable dt = distributionInventory.Tables["get_inventory"];
                DataRow inventoryDataRow = dt.Rows[0];
                if (dt.Columns.Contains("accession_id")) newOrderItem["accession_id"] = inventoryDataRow["accession_id"].ToString();
                if (newOrderItem.Table.Columns.Contains("site_id") && dt.Columns.Contains("site_id") && inventoryDataRow["site_id"] != DBNull.Value) newOrderItem["site_id"] = inventoryDataRow["site_id"].ToString();
                if (newOrderItem.Table.Columns.Contains("inventory_id")) newOrderItem["inventory_id"] = distributionInventoryID;
                if (newOrderItem.Table.Columns.Contains("name") && dt.Columns.Contains("plant_name") && inventoryDataRow["plant_name"] != DBNull.Value) newOrderItem["name"] = inventoryDataRow["plant_name"].ToString();
                if (newOrderItem.Table.Columns.Contains("external_taxonomy") && dt.Columns.Contains("taxonomy_species_id") && inventoryDataRow["taxonomy_species_id"] != DBNull.Value) newOrderItem["external_taxonomy"] = _sharedUtils.GetLookupDisplayMember("taxonomy_species_lookup", inventoryDataRow["taxonomy_species_id"].ToString(), "", inventoryDataRow["taxonomy_species_id"].ToString());
                if (newOrderItem.Table.Columns.Contains("quantity_on_hand") && dt.Columns.Contains("quantity_on_hand") && inventoryDataRow["quantity_on_hand"] != DBNull.Value) newOrderItem["quantity_on_hand"] = inventoryDataRow["quantity_on_hand"].ToString();
                if (newOrderItem.Table.Columns.Contains("quantity_on_hand_unit_code") && dt.Columns.Contains("quantity_on_hand_unit_code") && inventoryDataRow["quantity_on_hand_unit_code"] != DBNull.Value) newOrderItem["quantity_on_hand_unit_code"] = inventoryDataRow["quantity_on_hand_unit_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("quantity_shipped") && dt.Columns.Contains("distribution_default_quantity") && inventoryDataRow["distribution_default_quantity"] != DBNull.Value) newOrderItem["quantity_shipped"] = inventoryDataRow["distribution_default_quantity"].ToString();
                if (newOrderItem.Table.Columns.Contains("quantity_shipped_unit_code") && dt.Columns.Contains("distribution_unit_code") && inventoryDataRow["distribution_unit_code"] != DBNull.Value) newOrderItem["quantity_shipped_unit_code"] = inventoryDataRow["distribution_unit_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("distribution_form_code") && dt.Columns.Contains("distribution_default_form_code") && inventoryDataRow["distribution_default_form_code"] != DBNull.Value) newOrderItem["distribution_form_code"] = inventoryDataRow["distribution_default_form_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("availability_status_code") && dt.Columns.Contains("availability_status_code") && inventoryDataRow["availability_status_code"] != DBNull.Value) newOrderItem["availability_status_code"] = inventoryDataRow["availability_status_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("geography_id") && dt.Columns.Contains("geography_id") && inventoryDataRow["geography_id"] != DBNull.Value) newOrderItem["geography_id"] = inventoryDataRow["geography_id"].ToString();
                if (newOrderItem.Table.Columns.Contains("is_distributable") && dt.Columns.Contains("is_distributable") && inventoryDataRow["is_distributable"] != DBNull.Value) newOrderItem["is_distributable"] = inventoryDataRow["is_distributable"].ToString();
                if (newOrderItem.Table.Columns.Contains("distribution_default_form_code") && dt.Columns.Contains("distribution_default_form_code") && inventoryDataRow["distribution_default_form_code"] != DBNull.Value) newOrderItem["distribution_default_form_code"] = inventoryDataRow["distribution_default_form_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("distribution_unit_code") && dt.Columns.Contains("distribution_unit_code") && inventoryDataRow["distribution_unit_code"] != DBNull.Value) newOrderItem["distribution_unit_code"] = inventoryDataRow["distribution_unit_code"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part1") && dt.Columns.Contains("storage_location_part1") && inventoryDataRow["storage_location_part1"] != DBNull.Value) newOrderItem["storage_location_part1"] = inventoryDataRow["storage_location_part1"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part2") && dt.Columns.Contains("storage_location_part2") && inventoryDataRow["storage_location_part2"] != DBNull.Value) newOrderItem["storage_location_part2"] = inventoryDataRow["storage_location_part2"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part3") && dt.Columns.Contains("storage_location_part3") && inventoryDataRow["storage_location_part3"] != DBNull.Value) newOrderItem["storage_location_part3"] = inventoryDataRow["storage_location_part3"].ToString();
                if (newOrderItem.Table.Columns.Contains("storage_location_part4") && dt.Columns.Contains("storage_location_part4") && inventoryDataRow["storage_location_part4"] != DBNull.Value) newOrderItem["storage_location_part4"] = inventoryDataRow["storage_location_part4"].ToString();
                if (newOrderItem.Table.Columns.Contains("status_code")) newOrderItem["status_code"] = "NEW";
                if (newOrderItem.Table.Columns.Contains("status_date")) newOrderItem["status_date"] = DateTime.UtcNow.ToString("d");
                // Go get all the accession data for the chosen distribution inventory lot...
                DataSet distributionAccessionIPR = _sharedUtils.GetWebServiceData("get_accession_ipr", ":accessionid=" + inventoryDataRow["accession_id"].ToString(), 0, 0);
                if (distributionAccessionIPR.Tables.Contains("get_accession_ipr") &&
                   distributionAccessionIPR.Tables["get_accession_ipr"].Rows.Count > 0)
                {
                    // Now populate the new order_request_item row with the accession data...
                    distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Sort = "accession_ipr_id ASC";
                    distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.RowFilter = "expired_date is null";
                    if (distributionAccessionIPR.Tables["get_accession_ipr"].DefaultView.Count > 0)
                    {
                        DataRow accessionIPRDataRow = distributionAccessionIPR.Tables["get_accession_ipr"].Rows[0];
                        if (newOrderItem.Table.Columns.Contains("ipr_restriction") && accessionIPRDataRow.Table.Columns.Contains("assigned_type") && accessionIPRDataRow["assigned_type"] != DBNull.Value) newOrderItem["ipr_restriction"] = accessionIPRDataRow["assigned_type"].ToString();
                    }
                }
                // Next populate the sortable fields...
                foreach (DataColumn dc in newOrderItem.Table.Columns)
                {
                    if (dc.ColumnName.EndsWith("_sortable"))
                    {
                        newOrderItem[dc.ColumnName] = GetSortableColumnText(newOrderItem, dc.ColumnName.Replace("_sortable", ""));
                    }
                }
            }

            // Finally accept changes to this new row so that original value is populated...
//newOrderItem.AcceptChanges();

            return newOrderItem;
        }

        private void ux_datagridviewOrderRequestItem_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            // Check to see if this cell is in a column that needs a FK lookup...
            DataView dv = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView;
            if (dv != null && e.ColumnIndex > -1)
            {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                if (_sharedUtils.LookupTablesIsValidFKField(dc) &&
                    e.RowIndex < dv.Count &&
                    dv[e.RowIndex].Row.RowState != DataRowState.Deleted)
                {
                    if (dv[e.RowIndex][e.ColumnIndex] != DBNull.Value)
                    {
                        //e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                        e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
//dgv[e.ColumnIndex, e.RowIndex].Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim(), dv[e.RowIndex][e.ColumnIndex].ToString().Trim(), "", dv[e.RowIndex][e.ColumnIndex].ToString().Trim());
                    }
                    dgv[e.ColumnIndex, e.RowIndex].ErrorText = dv[e.RowIndex].Row.GetColumnError(dc);
                    e.FormattingApplied = true;
                }

                if (dc.ReadOnly)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                }
            }
        }

        private void ux_datagridviewOrderRequestItem_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            string errorMessage = e.Exception.Message;
            int columnWithError = -1;

            // Find the cell the error belongs to (don't use e.ColumnIndex because it points to the current cell *NOT* the offending cell)...
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (errorMessage.Contains(col.Name))
                {
                    dgv[col.Name, e.RowIndex].ErrorText = errorMessage;
                    columnWithError = col.Index;
                }
            }
        }

        private void ux_datagridviewOrderRequestItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;
            string columnName = dgv.CurrentCell.OwningColumn.Name;
            DataColumn dc = dt.Columns[columnName];
            DataRow dr;

            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                //string luTableName = dc.ExtendedProperties["foreign_key_resultset_name"].ToString().Trim();
                string luTableName = dc.ExtendedProperties["foreign_key_dataview_name"].ToString().Trim();
                dr = ((DataRowView)dgv.CurrentRow.DataBoundItem).Row;
                //GrinGlobal.Client.Data.LookupTablePicker ltp = new GrinGlobal.Client.Data.LookupTablePicker(lookupTables, localDBInstance, columnName, dr, dgv.CurrentCell.EditedFormattedValue.ToString());
                string suggestedFilter = "";

                //if (_lastDGVCharPressed > 0) suggestedFilter = _lastDGVCharPressed.ToString();
                if (dc.ColumnName.Trim().ToLower() != "inventory_id")
                {
                    if (dgv.CurrentRow != null &&
                       dgv.CurrentCell.EditedFormattedValue != null)
                    {
                        suggestedFilter = dgv.CurrentCell.EditedFormattedValue.ToString();
                    }

                    GRINGlobal.Client.Common.LookupTablePicker ltp = new GRINGlobal.Client.Common.LookupTablePicker(_sharedUtils, columnName, dr, suggestedFilter);
                    ltp.StartPosition = FormStartPosition.CenterParent;
                    if (DialogResult.OK == ltp.ShowDialog())
                    {
                        if (dr != null)
                        {
                            if (ltp.NewKey != null && dr[dgv.CurrentCell.ColumnIndex].ToString().Trim() != ltp.NewKey.Trim())
                            {
                                dr[dgv.CurrentCell.ColumnIndex] = ltp.NewKey.Trim();
                                dgv.CurrentCell.Value = ltp.NewValue.Trim();
                            }
                            else if (ltp.NewKey == null)
                            {
                                dr[dgv.CurrentCell.ColumnIndex] = DBNull.Value;
                                dgv.CurrentCell.Value = "";
                            }
                            dr.SetColumnError(dgv.CurrentCell.ColumnIndex, null);
//RefreshDGVFormatting(dgv);
                        }
                    }
                }
                else
                {
                    if (dgv.CurrentRow != null &&
                       dgv.CurrentRow.Cells["accession_id"].EditedFormattedValue != null)
                    {
                        suggestedFilter = dgv.CurrentRow.Cells["accession_id"].EditedFormattedValue.ToString();
                    }

                    FKeyPicker fkp = new FKeyPicker(_sharedUtils, columnName, dr, suggestedFilter);
                    fkp.StartPosition = FormStartPosition.CenterParent;
                    if (DialogResult.OK == fkp.ShowDialog())
                    {
                        if (dr != null)
                        {
                            if (fkp.NewKey != null && dr[dgv.CurrentCell.ColumnIndex].ToString().Trim() != fkp.NewKey.Trim())
                            {
                                dr[dgv.CurrentCell.ColumnIndex] = fkp.NewKey.Trim();
                                dgv.CurrentCell.Value = fkp.NewValue.Trim();
                            }
                            else if (fkp.NewKey == null)
                            {
                                dr[dgv.CurrentCell.ColumnIndex] = DBNull.Value;
                                dgv.CurrentCell.Value = "";
                            }
                            dr.SetColumnError(dgv.CurrentCell.ColumnIndex, null);
//RefreshDGVFormatting(dgv);
                        }
                    }
                }
                //_lastDGVCharPressed = (char)0;
                dgv.EndEdit();
            }
        }

        #endregion

        private void ux_datagridviewOrderRequestItem_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            //// Set the global variables so that later processing will know where to apply the command from the context menu...
            _mouseClickDGVColumnIndex = e.ColumnIndex;
            _mouseClickDGVRowIndex = e.RowIndex;
            DataGridView dgv = (DataGridView)sender;
            ContextMenuStrip cms = dgv.ContextMenuStrip;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;

            // Change the color of the cell background so that the user
            // knows what cell the context menu applies to...
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                // If the user right clicks outside of the currently selected cells - reset the selected cells to the one under the mouser cursor...
                if (!dgv.SelectedCells.Contains(dgv[e.ColumnIndex, e.RowIndex]))
                {
                    dgv.CurrentCell = dgv[e.ColumnIndex, e.RowIndex];
                }
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Red;
                dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.Red;
            }

            // Refresh the reports tool strip menu items...
            ux_dgvcellmenuReports.DropDownItems.Clear();
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory()); //System.Environment.GetFolderPath(Environment.SpecialFolder. System.Reflection.Assembly.GetExecutingAssembly()..Location);
            foreach (System.IO.FileInfo fi in di.GetFiles("*.rpt", System.IO.SearchOption.AllDirectories))
            {
                ToolStripItem tsi = ux_dgvcellmenuReports.DropDownItems.Add(fi.Name, null, ux_DGVCellReport_Click);
                tsi.Tag = fi.FullName;
            }

            foreach (object o in cms.Items)
            {
                if (o.GetType() == typeof(ToolStripMenuItem))
                {
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)o;
                    if (tsmi.Tag != null &&
                        !string.IsNullOrEmpty(tsmi.Tag.ToString()) &&
                        dt.Columns.Contains(tsmi.Tag.ToString().Trim().ToLower()) &&
                        dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties.Contains("gui_hint") &&
                        dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties["gui_hint"].ToString().Trim().ToUpper() == "SMALL_SINGLE_SELECT_CONTROL" &&
                        dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties.Contains("group_name") &&
                        !string.IsNullOrEmpty(dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties["group_name"].ToString()))
                    {
                        tsmi.Text = dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].Caption + "...";
                        tsmi.DropDownItems.Clear();
                        DataTable menuCodes = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup WHERE group_name = @groupname", "@groupname=" + dt.Columns[tsmi.Tag.ToString().Trim().ToLower()].ExtendedProperties["group_name"].ToString());
                        if (menuCodes != null &&
                            menuCodes.Rows.Count > 0)
                        {
                            foreach (DataRow dr in menuCodes.Rows)
                            {
                                ToolStripItem tsiCode = tsmi.DropDownItems.Add(dr["display_member"].ToString(), null, ux_AutoDGVColumnCode_Click);
                                tsiCode.Tag = dr["value_member"].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void ux_buttonShipAllRemaining_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in ux_datagridviewOrderRequestItem.Rows)
            {
if (ux_datagridviewOrderRequestItem.Columns.Contains("status_code") &&
    ux_datagridviewOrderRequestItem.Columns.Contains("status_date") &&
    (((DataRowView)dgvr.DataBoundItem).Row["status_code"].ToString() == "NEW" ||
    ((DataRowView)dgvr.DataBoundItem).Row["status_code"].ToString() == "PENDING" ||
    ((DataRowView)dgvr.DataBoundItem).Row["status_code"].ToString() == "INSPECT"))
{
    ((DataRowView)dgvr.DataBoundItem).Row["status_code"] = "SHIPPED";
    ApplyOrderItemBusinessRules(dgvr);
    //((DataRowView)dgvr.DataBoundItem).Row["status_date"] = DateTime.UtcNow.ToString("d");
}
            }
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        private void ux_contextmenustripDGVCell_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        void ux_AutoDGVColumnCode_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            //ContextMenuStrip cms = (ContextMenuStrip)tsmi.OwnerItem.Owner;
            //DataGridView dgv = (DataGridView)cms.SourceControl;
            DataGridView dgv = ux_datagridviewOrderRequestItem;
            DataTable dt = (DataTable)((BindingSource)dgv.DataSource).DataSource;

            if (tsmi.Tag.ToString().Trim().ToUpper() == "SPLIT")
            {
                SplitOrder(dgv);
            }
            else if (tsmi.Tag.ToString().Trim().ToUpper() == "INSPECT")
            {
                InspectOrder(dgv);
            }
            else if (tsmi.Tag.ToString().Trim().ToUpper() == "QUALITYTEST")
            {
                QualityTestOrder(dgv);
            }
            else
            {
                // Iterate through the selected cells and modify the data in the code column associated with this context menu item...
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    object pKey = dgvc.OwningRow.Cells[dt.PrimaryKey[0].ColumnName].Value;
                    DataRow dr = dt.Rows.Find(pKey);
                    if (dr != null)
                    {
                        string codeColumnName = tsmi.OwnerItem.Tag.ToString().Trim().ToLower();
                        string code = tsmi.Tag.ToString().Trim().ToUpper();
                        if (!string.IsNullOrEmpty(codeColumnName) &&
                            !string.IsNullOrEmpty(code))
                        {
                            if (dt.Columns.Contains(codeColumnName)) dr[codeColumnName] = code;
                            ApplyOrderItemBusinessRules(dgvc.OwningRow);
                        }
                    }
                }
                RefreshDGVFormatting(dgv);
            }
        }

        private void SplitOrder(DataGridView dgv)
        {
            // Process for splitting an order in to two orders with only selected rows going to the new order... 
            DataRow currentOrderRequest = ((DataRowView)_orderRequestBindingSource.Current).Row;
            List<DataRow> selectedRows = new List<DataRow>();

            // Selected rows are different than selected cells - so look for both...
            if (dgv.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgv.SelectedRows)
                {
                    selectedRows.Add(((DataRowView)dgvr.DataBoundItem).Row);
                }
            }
            else if (dgv.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    if (!selectedRows.Contains(((DataRowView)dgvc.OwningRow.DataBoundItem).Row))
                    {
                        selectedRows.Add(((DataRowView)dgvc.OwningRow.DataBoundItem).Row);
                    }
                }
            }

            // Warn the user about what they are about to do and prompt to save if the order is brand new...
            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You are about to split {0} items from this order.\n\nAre you sure you want to do this?", "Split Order Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage1";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, selectedRows.Count.ToString());
            //if (DialogResult.Yes == MessageBox.Show("You are about to split " + selectedRows.Count.ToString() + " items from this order.\n\nAre you sure you want to do this?", "Split Order Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            if (DialogResult.Yes == ggMessageBox.ShowDialog())
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox1 = new GRINGlobal.Client.Common.GGMessageBox("You must save this order before you can split it.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                ggMessageBox1.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage2";
                _sharedUtils.UpdateControls(ggMessageBox1.Controls, ggMessageBox.Name);
                //if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
                //    DialogResult.Yes == MessageBox.Show("You must save this order before you can split it.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
                if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
                    DialogResult.Yes == ggMessageBox1.ShowDialog())
                {
                    int errorCount = SaveOrderData();
                }

                if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] > 0)
                {
                    // First create a copy of the Order Request record...
                    DataRow newOrderRequest = _orderRequest.NewRow();
                    if (_orderRequest.Columns.Contains("original_order_request_id")) newOrderRequest["original_order_request_id"] = currentOrderRequest["order_request_id"];
                    if (_orderRequest.Columns.Contains("web_order_request_id")) newOrderRequest["web_order_request_id"] = currentOrderRequest["web_order_request_id"];
                    if (_orderRequest.Columns.Contains("order_type_code")) newOrderRequest["order_type_code"] = currentOrderRequest["order_type_code"];
                    if (_orderRequest.Columns.Contains("ordered_date")) newOrderRequest["ordered_date"] = currentOrderRequest["ordered_date"];
                    if (_orderRequest.Columns.Contains("intended_use_code")) newOrderRequest["intended_use_code"] = currentOrderRequest["intended_use_code"];
                    if (_orderRequest.Columns.Contains("intended_use_note")) newOrderRequest["intended_use_note"] = currentOrderRequest["intended_use_note"];
                    if (_orderRequest.Columns.Contains("requestor_cooperator_id")) newOrderRequest["requestor_cooperator_id"] = currentOrderRequest["requestor_cooperator_id"];
                    if (_orderRequest.Columns.Contains("ship_to_cooperator_id")) newOrderRequest["ship_to_cooperator_id"] = currentOrderRequest["ship_to_cooperator_id"];
                    if (_orderRequest.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = currentOrderRequest["final_recipient_cooperator_id"];
                    if (_orderRequest.Columns.Contains("order_obtained_via")) newOrderRequest["order_obtained_via"] = currentOrderRequest["order_obtained_via"];
                    if (_orderRequest.Columns.Contains("special_instruction")) newOrderRequest["special_instruction"] = currentOrderRequest["special_instruction"];
                    if (_orderRequest.Columns.Contains("note")) newOrderRequest["note"] = currentOrderRequest["note"];
                    if (_orderRequest.Columns.Contains("owner_site_id")) newOrderRequest["owner_site_id"] = currentOrderRequest["owner_site_id"];
                    // And add it to the Order Request Table...
                    _orderRequest.Rows.Add(newOrderRequest);

                    // Now move each Order Request Item record selected to the new Order Request record...
                    foreach (DataRow dr in selectedRows)
                    {
                        if (dr.Table.Columns.Contains("order_request_id")) dr["order_request_id"] = newOrderRequest["order_request_id"];
                        //if (dr.Table.Columns.Contains("status_code")) dr["status_code"] = DBNull.Value;
                        if (dr.Table.Columns.Contains("status_code")) dr["status_code"] = "NEW";
//if (dr.Table.Columns.Contains("status_date")) dr["status_date"] = DBNull.Value;
if (dr.Table.Columns.Contains("status_date")) dr["status_date"] = DateTime.UtcNow.ToString("d");
                    }

                    // Now create an Action for the original order...
                    DataRow newOrderRequestAction = _orderRequestAction.NewRow();
                    if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = currentOrderRequest["order_request_id"];
                    if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "SPLIT";
                    if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "Order was split by " + _sharedUtils.Username;
                    // And add it to the Order Request Action Table...
                    _orderRequestAction.Rows.Add(newOrderRequestAction);

                    // Finally create an Action for the new (split out) order...
                    newOrderRequestAction = _orderRequestAction.NewRow();
                    if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = newOrderRequest["order_request_id"];
                    if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "SPLIT";
                    if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "Order created by splitting items from Order: " + currentOrderRequest["order_request_id"].ToString() + " into this sub-order by " + _sharedUtils.Username;
                    // And add it to the Order Request Action Table...
                    _orderRequestAction.Rows.Add(newOrderRequestAction);
                }
            }
        }

        private void InspectOrder(DataGridView dgv)
        {
            // Process for marking an order for phyto inspection... 
            DataRow currentOrderRequest = ((DataRowView)_orderRequestBindingSource.Current).Row;
            List<DataRow> selectedRows = new List<DataRow>();

            // Selected rows are different than selected cells - so look for both...
            if (dgv.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgv.SelectedRows)
                {
                    selectedRows.Add(((DataRowView)dgvr.DataBoundItem).Row);
                }
            }
            else if (dgv.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    if (!selectedRows.Contains(((DataRowView)dgvc.OwningRow.DataBoundItem).Row))
                    {
                        selectedRows.Add(((DataRowView)dgvc.OwningRow.DataBoundItem).Row);
                    }
                }
            }

            // Warn the user about what they are about to do and prompt to save if the order is brand new...
            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You are about to send {0} items to Plant Inspection.\n\nAre you sure you want to do this?", "Split Order Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage3";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, selectedRows.Count.ToString());
            //if (DialogResult.Yes == MessageBox.Show("You are about to split " + selectedRows.Count.ToString() + " items from this order.\n\nAre you sure you want to do this?", "Split Order Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            if (DialogResult.Yes == ggMessageBox.ShowDialog())
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox1 = new GRINGlobal.Client.Common.GGMessageBox("You must save this order before you send it to Plant Inspection.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                ggMessageBox1.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage4";
                _sharedUtils.UpdateControls(ggMessageBox1.Controls, ggMessageBox.Name);
                //if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
                //    DialogResult.Yes == MessageBox.Show("You must save this order before you can split it.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
                if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
                    DialogResult.Yes == ggMessageBox1.ShowDialog())
                {
                    int errorCount = SaveOrderData();
                }

                if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] > 0)
                {
                    // Modify the Order Request record...
                    if (_orderRequest.Columns.Contains("completed_date")) currentOrderRequest["completed_date"] = DateTime.UtcNow.ToString("d");

                    // Now modify each Order Request Item record selected...
                    foreach (DataRow dr in selectedRows)
                    {
                        if (dr.Table.Columns.Contains("status_code")) dr["status_code"] = "INSPECT";
                        if (dr.Table.Columns.Contains("status_date")) dr["status_date"] = DateTime.UtcNow.ToString("d");
                    }

                    // Finally create an Action for this order...
                    DataRow newOrderRequestAction = _orderRequestAction.NewRow();
                    if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = currentOrderRequest["order_request_id"];
                    if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "INSPECT";
                    if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "Order sent to Plant Inspection by " + _sharedUtils.Username;
                    // And add it to the Order Request Action Table...
                    _orderRequestAction.Rows.Add(newOrderRequestAction);
                }
            }
        }

        private void QualityTestOrder(DataGridView dgv)
        {
            // Process for duplicating an order with the duplicate order being sent to quality testing... 
            DataRow currentOrderRequest = ((DataRowView)_orderRequestBindingSource.Current).Row;
            List<DataRow> selectedRows = new List<DataRow>();

            // Selected rows are different than selected cells - so look for both...
            if (dgv.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in dgv.SelectedRows)
                {
                    selectedRows.Add(((DataRowView)dgvr.DataBoundItem).Row);
                }
            }
            else if (dgv.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell dgvc in dgv.SelectedCells)
                {
                    if (!selectedRows.Contains(((DataRowView)dgvc.OwningRow.DataBoundItem).Row))
                    {
                        selectedRows.Add(((DataRowView)dgvc.OwningRow.DataBoundItem).Row);
                    }
                }
            }

            // Warn the user about what they are about to do and prompt to save if the order is brand new...
            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You are about to create a new order with copies of {0} items from this order for Quality Testing.\n\nAre you sure you want to do this?", "Duplicate Order Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ggMessageBox.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage5";
            _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, selectedRows.Count.ToString());
            //if (DialogResult.Yes == MessageBox.Show("You are about to split " + selectedRows.Count.ToString() + " items from this order.\n\nAre you sure you want to do this?", "Split Order Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
            if (DialogResult.Yes == ggMessageBox.ShowDialog())
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox1 = new GRINGlobal.Client.Common.GGMessageBox("You must save this order before you can create the duplicate order for Quality Testing.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                ggMessageBox1.Name = "OrderWizard_ux_AutoDGVColumnCodeMessage2";
                _sharedUtils.UpdateControls(ggMessageBox1.Controls, ggMessageBox.Name);
                //if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
                //    DialogResult.Yes == MessageBox.Show("You must save this order before you can split it.\n\nWould you like to do this now?", "Save Data Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
                if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] < 0 &&
                    DialogResult.Yes == ggMessageBox1.ShowDialog())
                {
                    int errorCount = SaveOrderData();
                }

                if ((int)currentOrderRequest[currentOrderRequest.Table.PrimaryKey[0].ColumnName] > 0)
                {
                    // First create a copy of the Order Request record...
                    DataRow newOrderRequest = _orderRequest.NewRow();
                    if (_orderRequest.Columns.Contains("original_order_request_id")) newOrderRequest["original_order_request_id"] = currentOrderRequest["order_request_id"];
                    if (_orderRequest.Columns.Contains("web_order_request_id")) newOrderRequest["web_order_request_id"] = DBNull.Value;
                    if (_orderRequest.Columns.Contains("order_type_code")) newOrderRequest["order_type_code"] = "PT";
                    if (_orderRequest.Columns.Contains("ordered_date")) newOrderRequest["ordered_date"] = DateTime.UtcNow.ToString("d");
                    if (_orderRequest.Columns.Contains("intended_use_code")) newOrderRequest["intended_use_code"] = currentOrderRequest["intended_use_code"];
                    if (_orderRequest.Columns.Contains("intended_use_note")) newOrderRequest["intended_use_note"] = currentOrderRequest["intended_use_note"];
                    if (_orderRequest.Columns.Contains("requestor_cooperator_id")) newOrderRequest["requestor_cooperator_id"] = currentOrderRequest["requestor_cooperator_id"];
                    if (_orderRequest.Columns.Contains("ship_to_cooperator_id")) newOrderRequest["ship_to_cooperator_id"] = currentOrderRequest["ship_to_cooperator_id"];
                    if (_orderRequest.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = currentOrderRequest["final_recipient_cooperator_id"];
                    if (_orderRequest.Columns.Contains("order_obtained_via")) newOrderRequest["order_obtained_via"] = currentOrderRequest["order_obtained_via"];
                    if (_orderRequest.Columns.Contains("special_instruction")) newOrderRequest["special_instruction"] = currentOrderRequest["special_instruction"];
                    if (_orderRequest.Columns.Contains("note")) newOrderRequest["note"] = currentOrderRequest["note"];
                    if (_orderRequest.Columns.Contains("owner_site_id")) newOrderRequest["owner_site_id"] = currentOrderRequest["owner_site_id"];
                    // And add it to the Order Request Table...
                    _orderRequest.Rows.Add(newOrderRequest);

                    // Now move each Order Request Item record selected to the new Order Request record...
                    int seqNum = 1;
                    foreach (DataRow dr in selectedRows)
                    {
                        //if (dr.Table.Columns.Contains("order_request_id")) dr["order_request_id"] = newOrderRequest["order_request_id"];
                        ////if (dr.Table.Columns.Contains("status_code")) dr["status_code"] = DBNull.Value;
                        //if (dr.Table.Columns.Contains("status_code")) dr["status_code"] = "NEW";
                        //if (dr.Table.Columns.Contains("status_date")) dr["status_date"] = DBNull.Value;
                        DataRow newOrderRequestItem = _orderRequestItem.NewRow();
                        if (dr.Table.Columns.Contains("order_request_id")) newOrderRequestItem["order_request_id"] = newOrderRequest["order_request_id"];
                        if (dr.Table.Columns.Contains("sequence_number")) newOrderRequestItem["sequence_number"] = seqNum++;
                        if (dr.Table.Columns.Contains("name")) newOrderRequestItem["name"] = dr["name"];
                        if (dr.Table.Columns.Contains("quantity_shipped")) newOrderRequestItem["quantity_shipped"] = dr["quantity_shipped"];
                        if (dr.Table.Columns.Contains("quantity_shipped_unit_code")) newOrderRequestItem["quantity_shipped_unit_code"] = dr["quantity_shipped_unit_code"];
                        if (dr.Table.Columns.Contains("distribution_form_code")) newOrderRequestItem["distribution_form_code"] = dr["distribution_form_code"];
                        if (dr.Table.Columns.Contains("status_code")) newOrderRequestItem["status_code"] = "NEW";
//if (dr.Table.Columns.Contains("status_date")) newOrderRequestItem["status_date"] = DBNull.Value;
if (dr.Table.Columns.Contains("status_date")) newOrderRequestItem["status_date"] = DateTime.UtcNow.ToString("d");
                        if (dr.Table.Columns.Contains("inventory_id")) newOrderRequestItem["inventory_id"] = dr["inventory_id"];
                        if (dr.Table.Columns.Contains("accession_id")) newOrderRequestItem["accession_id"] = dr["accession_id"];
                        if (dr.Table.Columns.Contains("external_taxonomy")) newOrderRequestItem["external_taxonomy"] = dr["external_taxonomy"];
                        if (dr.Table.Columns.Contains("note")) newOrderRequestItem["note"] = dr["note"];
                        if (dr.Table.Columns.Contains("source_cooperator_id")) newOrderRequestItem["source_cooperator_id"] = dr["source_cooperator_id"];
                        // And add it to the Order Request Item Table...
                        _orderRequestItem.Rows.Add(newOrderRequestItem);
                    }

                    // Now create an Action for the original order...
                    DataRow origOrderRequestAction = _orderRequestAction.NewRow();
                    if (_orderRequestAction.Columns.Contains("order_request_id")) origOrderRequestAction["order_request_id"] = currentOrderRequest["order_request_id"];
                    if (_orderRequestAction.Columns.Contains("action_name_code")) origOrderRequestAction["action_name_code"] = "QUALITYTEST";
                    if (_orderRequestAction.Columns.Contains("started_date")) origOrderRequestAction["started_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("started_date_code")) origOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("completed_date")) origOrderRequestAction["completed_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("completed_date_code")) origOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("action_information")) origOrderRequestAction["action_information"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("action_cost")) origOrderRequestAction["action_cost"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("cooperator_id")) origOrderRequestAction["cooperator_id"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("note")) origOrderRequestAction["note"] = "Order sent to Quality Testing by " + _sharedUtils.Username;
                    // And add it to the Order Request Action Table...
                    _orderRequestAction.Rows.Add(origOrderRequestAction);

                    // Finally create an Action for the new order...
                    DataRow newOrderRequestAction = _orderRequestAction.NewRow();
                    if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = newOrderRequest["order_request_id"];
                    if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "NEW";
                    if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
                    if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
                    if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
                    if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "Order created for Quality Testing by " + _sharedUtils.Username;
                    // And add it to the Order Request Action Table...
                    _orderRequestAction.Rows.Add(newOrderRequestAction);
                }
            }
        }

        void ux_DGVCellReport_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            //ContextMenuStrip cms = (ContextMenuStrip)tsmi.Owner;
            //DataGridView dgv = (DataGridView)cms.SourceControl;
            DataGridView dgv = ux_datagridviewOrderRequestItem;
            DataTable dt = ((DataTable)((BindingSource)dgv.DataSource).DataSource).Clone();

            // NOTE: because of the way the DGV adds rows to the selectedRows collection
            //       we have to process the rows in the opposite direction they were selected in...
            int rowStart = 0;
            int rowStop = dgv.SelectedRows.Count;
            int stepValue = 1;
            if (dgv.SelectedRows.Count > 1 && dgv.SelectedRows[0].Index > dgv.SelectedRows[1].Index)
            {
                rowStart = dgv.SelectedRows.Count - 1;
                rowStop = -1;
                stepValue = -1;
            }

            DataGridViewRow dgvrow = null;
            // Process the rows in the opposite direction they were selected by the user...
            for (int i = rowStart; i != rowStop; i += stepValue)
            {
                dgvrow = dgv.SelectedRows[i];
                if (!dgvrow.IsNewRow)
                {
                    dt.Rows.Add(((DataRowView)dgvrow.DataBoundItem).Row.ItemArray);
                }
            }

            string fullPathName = tsmi.Tag.ToString();
            if (System.IO.File.Exists(fullPathName))
            {
                //ReportForm crustyReport = new ReportForm(dt, fullPathName);
                //crustyReport.StartPosition = FormStartPosition.CenterParent;
                //crustyReport.ShowDialog();
            }
        }

        private void ApplyOrderItemBusinessRules(DataGridViewRow dgvr)
        {
            DataGridView dgv = dgvr.DataGridView;
            DataRow dr = ((DataRowView)dgvr.DataBoundItem).Row;
            dr.EndEdit();
            // If this is an order_request_item row and it has changes - apply business rules to harmonize data within the row...
            if (dr.Table.TableName == "order_wizard_get_order_request_item")
            {
                // If the inventory_id changed for this order_request_item...
                if (dr.RowState == DataRowState.Added ||
                    (dr.RowState == DataRowState.Modified && !dr["inventory_id", DataRowVersion.Original].Equals(dr["inventory_id", DataRowVersion.Current])))
                {
                    DataSet ds = _sharedUtils.GetWebServiceData("order_wizard_get_inventory", ":inventoryid=" + dr["inventory_id", DataRowVersion.Current], 0, 0);
                    if (ds != null &&
                        ds.Tables.Contains("order_wizard_get_inventory") &&
                        ds.Tables["order_wizard_get_inventory"].Rows.Count > 0)
                    {
                        DataRow invRow = ds.Tables["order_wizard_get_inventory"].Rows.Find(dr["inventory_id"]);
                        // If the inventory row was found - copy all data that has matching column names AND is different...
                        if (invRow != null)
                        {
                            foreach (DataColumn dc in dr.Table.Columns)
                            {
                                syncDataRow(invRow, dc.ColumnName, dr, dc.ColumnName);
                            }
                            // Now manually sync data for mismatched column names...
                            syncDataRow(invRow, "plant_name", dr, "name");
                            syncDataRow(invRow, "taxonomy_species_id", dr, "external_taxonomy");
                            syncDataRow(invRow, "distribution_default_quantity", dr, "quantity_shipped");
                            syncDataRow(invRow, "distribution_unit_code", dr, "quantity_shipped_unit_code");
                            syncDataRow(invRow, "form_type_code", dr, "distribution_form_code");
                        }
                    }
                }
                // If an order item's status has changed...
                if (dr.RowState == DataRowState.Added ||
                (dr.RowState == DataRowState.Modified && !dr["status_code", DataRowVersion.Original].Equals(dr["status_code", DataRowVersion.Current])))
                {
                    bool orderComplete = true;
                    // Update the status date...
                    if (dr.RowState == DataRowState.Modified && dr.Table.Columns.Contains("status_date") &&
                        dr["status_date", DataRowVersion.Original].Equals(dr["status_date", DataRowVersion.Current]) &&
                        (dr["status_date", DataRowVersion.Original] == DBNull.Value ||
                        ((DateTime)dr["status_date", DataRowVersion.Current]).ToString("d").Trim().ToUpper() != DateTime.UtcNow.ToString("d").Trim().ToUpper()))
                    {
                        dr["status_date"] = DateTime.UtcNow.ToString("d");
                        if (dr.Table.Columns.Contains("status_date_sortable")) dr["status_date"] = DateTime.UtcNow.ToString("d");
                    }
                    // Now check to see if all items in this order are either marked as 'SHIPPED' or 'CANCEL'
                    // and if so set the order request header's completed_date...
                    foreach (DataGridViewRow ori_dgvr in dgv.Rows)
                    {
                        if (ori_dgvr.Cells["status_code"].Value.ToString().Trim().ToUpper() != "SHIPPED" &&
                            ori_dgvr.Cells["status_code"].Value.ToString().Trim().ToUpper() != "CANCEL") orderComplete = false;
                    }
                    if (orderComplete)
                    {
                        // Mark the order_request completed_date to reflect the order is completed...
_orderRequestBindingSource.EndEdit();
DataRow orderRequest = _orderRequest.Rows.Find(dr["order_request_id"]);  // This line does not seem to work when the order_request is still in edit mode...
//DataRow orderRequest = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();  // This line does seem to work though...
                        if (orderRequest != null)
                        {
                            orderRequest["completed_date"] = DateTime.UtcNow.ToString("d");
                        }
                        else
                        {

                        }
                        // Put an order_request_action in to document the order is completed...
                        // Create an Action for the status_code changes (one per staus_code with a count for how many items were changed to that status)...
                        DataRow newOrderRequestAction = _orderRequestAction.NewRow();
                        if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = dr["order_request_id"];
                        if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "DONE";
                        if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
                        if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
                        if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
                        if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
                        if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
                        if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
                        if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
                        if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "Order Request has been marked as COMPLETED by " + _sharedUtils.Username;
                        // And add it to the Order Request Action Table...
                        // Check to see if this row already has been added in this session...
                        DataRow[] dupActionRecords = _orderRequestAction.Select("order_request_id=" + dr["order_request_id"].ToString() + " AND action_name_code='DONE'", "", DataViewRowState.Added);
                        if (dupActionRecords.Length == 0) _orderRequestAction.Rows.Add(newOrderRequestAction);
                    }
                    else
                    {
                        DataRow orderRequest = _orderRequest.Rows.Find(dr["order_request_id"]);
                        if (orderRequest != null) orderRequest["completed_date"] = DBNull.Value;
                    }
                }
            }
        }

        private void RefreshDGVFormatting(DataGridView dgv)
        {
            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
//ApplyOrderItemBusinessRules(dgvr);
                RefreshDGVRowFormatting(dgvr);
            }

            // Show SortGlyphs for the column headers (this takes two steps)...
            // First reset them all to No Sort...
            foreach (DataGridViewColumn dgvc in dgv.Columns)
            {
                dgvc.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            // Now inspect the sort string from the datatable in use to set the SortGlyphs...
            string strOrder = "";
            if (dgv.DataSource.GetType() == typeof(BindingSource))
            {
                strOrder = ((DataTable)((BindingSource)dgv.DataSource).DataSource).DefaultView.Sort;
            }
            else
            {
                strOrder = ((DataTable)dgv.DataSource).DefaultView.Sort;
            }
            char[] chararrDelimiters = { ',' };
            string[] strarrSortCols = strOrder.Split(chararrDelimiters);
            foreach (string strSortCol in strarrSortCols)
            {
                if (strSortCol.Contains("ASC"))
                {
                    if (dgv.Columns.Contains(strSortCol.Replace(" ASC", "").Replace("_sortable", "").Trim())) dgv.Columns[strSortCol.Replace(" ASC", "").Replace("_sortable", "").Trim()].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                }
                if (strSortCol.Contains("DESC"))
                {
                    if (dgv.Columns.Contains(strSortCol.Replace(" DESC", "").Replace("_sortable", "").Trim())) dgv.Columns[strSortCol.Replace(" DESC", "").Replace("_sortable", "").Trim()].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                }
            }
        }

        private void RefreshDGVRowFormatting(DataGridViewRow dgvr)
        {
            foreach (DataGridViewCell dgvc in dgvr.Cells)
            {
                // Reset the background and foreground color...
                dgvc.Style.BackColor = Color.Empty;
                dgvc.Style.ForeColor = Color.Empty;
                dgvc.Style.SelectionBackColor = Color.Empty;
                dgvc.Style.SelectionForeColor = Color.Empty;
            }
            // If the row has changes make each changed cell yellow...
            DataRow dr = ((DataRowView)dgvr.DataBoundItem).Row;
            if (dr.RowState == DataRowState.Modified)
            {
                foreach (DataGridViewCell dgvc in dgvr.Cells)
                {
                    string dcName = dgvc.OwningColumn.Name;
                    // If the cell has been changed make it yellow...
                    //if (!dr[dcName, DataRowVersion.Original].Equals(dr[dcName, DataRowVersion.Current]))
                    if (dr[dcName, DataRowVersion.Original].ToString().Trim() != dr[dcName, DataRowVersion.Current].ToString().Trim())
                    {
                        dgvc.Style.BackColor = Color.Yellow;
                        dr.SetColumnError(dcName, null);
                    }
                    // Use default background color for this cell...
                    else
                    {
                        dgvc.Style.BackColor = Color.Empty;
                    }
                }
            }
//if (dr.RowState == DataRowState.Added)
//{
//    foreach (DataGridViewCell dgvc in dgvr.Cells)
//    {
//        string dcName = dgvc.OwningColumn.Name;
//        // If the cell has been changed make it yellow...
//        if (_sharedUtils.LookupTablesIsValidFKField(dr.Table.Columns[dcName]))
//        {
//            if (dgvc.FormattedValue != null &&
//                dgvc.FormattedValue.ToString().Trim() != _sharedUtils.GetLookupDisplayMember(dr.Table.Columns[dcName].ExtendedProperties["foreign_key_dataview_name"].ToString(), dr[dcName, DataRowVersion.Current].ToString().Trim(), "", dr[dcName, DataRowVersion.Current].ToString().Trim()))
//            {
//                dgvc.Style.BackColor = Color.Yellow;
//                dr.SetColumnError(dcName, null);
//            }
//        }
//        else if (_sharedUtils.LookupTablesIsValidCodeValueField(dr.Table.Columns[dcName]))
//        {
//            if (dgvc.FormattedValue != null &&
//                dgvc.FormattedValue.ToString().Trim() != _sharedUtils.GetLookupDisplayMember("code_value_lookup", dr[dcName, DataRowVersion.Current].ToString().Trim(), dr.Table.Columns[dcName].ExtendedProperties["group_name"].ToString(), dr[dcName, DataRowVersion.Current].ToString().Trim()))
//            {
//                dgvc.Style.BackColor = Color.Yellow;
//                dr.SetColumnError(dcName, null);
//            }
//        }
//        else
//        {
//            if (dgvc.FormattedValue != null &&
//                dgvc.FormattedValue.ToString().Trim() != dr[dcName, DataRowVersion.Current].ToString().Trim())
//            {
//                dgvc.Style.BackColor = Color.Yellow;
//                dr.SetColumnError(dcName, null);
//            }
//        }
//    }
//}
        }

        private void ux_datagridview_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //_sharedUtils.ProcessDGVEditShortcutKeys(dgv, e, _sharedUtils.UserCooperatorID, ImportTextToDataTableUsingAltKeys, ImportTextToDataTableUsingBlockStyle);
            if (_sharedUtils.ProcessDGVEditShortcutKeys(dgv, e, _sharedUtils.UserCooperatorID))
            {
                foreach (DataGridViewRow dgvr in dgv.Rows)
                {
                    ApplyOrderItemBusinessRules(dgvr);
                }
                RefreshDGVFormatting(dgv);
            }
        }

        private void ux_radiobuttonOrderFilter_CheckedChanged(object sender, EventArgs e)
        {
            // This event is fired for radio buttons that are being checked and unchecked
            // so ignore the event for radio buttons that are being unchecked...
            //            if (((RadioButton)sender).Checked)
            //            {
            ////                int intRowEdits = 0;

            ////                _orderRequestBindingSource.EndEdit();
            ////                if (_orderRequest.GetChanges() != null) intRowEdits = _orderRequest.GetChanges().Rows.Count;
            ////                _orderRequestItemBindingSource.EndEdit();
            ////                if (_orderRequestItem.GetChanges() != null) intRowEdits += _orderRequestItem.GetChanges().Rows.Count;
            ////                _orderRequestActionBindingSource.EndEdit();
            ////                if (_orderRequestAction.GetChanges() != null) intRowEdits += _orderRequestAction.GetChanges().Rows.Count;
            //////_webOrderRequestBindingSource.EndEdit();
            //////if (_webOrderRequest.GetChanges() != null) intRowEdits += _webOrderRequest.GetChanges().Rows.Count;

            ////                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s) that will be lost.\n\nWould you like to save them now?", "Save Edits", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
            ////                ggMessageBox.Name = "OrderWizard_ux_radiobuttonOrderFilterMessage1";
            ////                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
            ////                if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
            ////                if (intRowEdits > 0 && DialogResult.Yes == ggMessageBox.ShowDialog())
            ////                {
            ////                    SaveOrderData();
            ////                }

            //                // If the Order tab is active, refresh the Order data...
            //                if (ux_tabcontrolMain.SelectedTab == OrderPage || ux_tabcontrolMain.SelectedTab == ActionsPage) RefreshOrderData();
            //                // If the Web Order tab is active, refresh the Web Order data...
            //                if (ux_tabcontrolMain.SelectedTab == WebOrderPage) RefreshWebOrderData();
            //            }
            if (((RadioButton)sender).Checked)
            {
                if (ux_tabcontrolMain.SelectedTab == OrderPage || ux_tabcontrolMain.SelectedTab == ActionsPage)
                {
                    if (ux_radiobuttonSelectionOrders.Checked)
                    {
                        //ux_buttonFindOrders.Enabled = false;

                        ux_textboxSelection.Size = ux_checkedlistboxOrderItemStatus.Size;
                        ux_textboxSelection.Location = ux_checkedlistboxOrderItemStatus.Location;
                        ux_textboxSelection.Text = _selectionList;
                        ux_textboxSelection.Enabled = true;
                        ux_textboxSelection.Visible = true;
                        ux_checkedlistboxOrderItemStatus.Enabled = false;
                        ux_checkedlistboxOrderItemStatus.Visible = false;
                        ux_textboxOrderDateFilter.Enabled = false;
                        ux_textboxOrderDateFilter.Visible = false;
                        ux_comboboxOrderTypeFilter.Enabled = false;
                        ux_comboboxOrderTypeFilter.Visible = false;
                        ux_labelOrderDateFilter.Visible = false;
                        ux_labelOrderTypeFilter.Visible = false;
                        ux_labelItemStatus.Visible = false;
//RefreshOrderData();
                    }
                    else
                    {
                        //ux_buttonFindOrders.Enabled = true;
                        ux_textboxSelection.Enabled = false;
                        ux_textboxSelection.Visible = false;
                        ux_checkedlistboxOrderItemStatus.Enabled = true;
                        ux_checkedlistboxOrderItemStatus.Visible = true;
                        ux_textboxOrderDateFilter.Enabled = true;
                        ux_textboxOrderDateFilter.Visible = true;
                        ux_comboboxOrderTypeFilter.Enabled = true;
                        ux_comboboxOrderTypeFilter.Visible = true;
                        ux_labelOrderDateFilter.Visible = true;
                        ux_labelOrderTypeFilter.Visible = true;
                        ux_labelItemStatus.Visible = true;
                    }
                }
                if (ux_tabcontrolMain.SelectedTab == WebOrderPage)
                {
//RefreshWebOrderData();
                }
            }
        }

        private void ux_checkedlistboxOrderItemStatus_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //            if (ux_tabcontrolMain.SelectedTab == OrderPage ||
            //                ux_tabcontrolMain.SelectedTab == ActionsPage)
            //            {
            //                _orderRequestStatusFilter = " AND (";
            //                // Process the listbox item that is changing by peeking at its 'new value'...
            //                for (int i = 0; i < ux_checkedlistboxOrderItemStatus.Items.Count; i++)
            //                {
            //                    if (i != e.Index &&
            //                        ux_checkedlistboxOrderItemStatus.GetItemChecked(i))
            //                    {
            //                        // Add all listbox items that are checked...
            ////_orderRequestStatusFilter += "'" + _orderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
            //_orderRequestStatusFilter += "@order_request_item.status_code='" + _orderRequestStatusCodes.Rows[i]["value_member"].ToString() + "' OR ";
            //                    }
            //                    else if (i == e.Index &&
            //                        e.NewValue == CheckState.Checked)
            //                    {
            //                        // Process the listbox item that is changing differently because its new checked state is not saved until after this
            //                        // event has finished processing - so determine if it should be added to the list by peeking at its 'new value'...
            ////_orderRequestStatusFilter += "'" + _orderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
            //_orderRequestStatusFilter += "@order_request_item.status_code='" + _orderRequestStatusCodes.Rows[i]["value_member"].ToString() + "' OR ";
            //                    }
            //                }
            //                // Drop the extra semicolon at the end of the string...
            ////_orderRequestStatusFilter = _orderRequestStatusFilter.TrimEnd(',');
            //_orderRequestStatusFilter = _orderRequestStatusFilter.Remove(_orderRequestStatusFilter.Length - 4) + ")";

            //                // Refresh the order data...
            //                RefreshOrderData();
            //            }
        }

        private void ux_buttonFindWebOrders_Click(object sender, EventArgs e)
        {
            // If the Web Order tab is active, refresh the Web Order data...
            if (ux_tabcontrolMain.SelectedTab == WebOrderPage) RefreshWebOrderData();
        }

        private void ux_checkedlistboxWebOrderItemStatus_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ////if (ux_tabcontrolMain.SelectedTab == WebOrderPage)
            //{
            //    _webOrderRequestStatusFilter = "";
            //    // Process the listbox item that is changing by peeking at its 'new value'...
            //    for (int i = 0; i < ux_checkedlistboxWebOrderItemStatus.Items.Count; i++)
            //    {
            //        if (i != e.Index &&
            //            ux_checkedlistboxWebOrderItemStatus.GetItemChecked(i))
            //        {
            //            // Add all listbox items that are checked...
            //            _webOrderRequestStatusFilter += "'" + _webOrderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
            //        }
            //        else if (i == e.Index &&
            //            e.NewValue == CheckState.Checked)
            //        {
            //            // Process the listbox item that is changing differently because its new checked state is not saved until after this
            //            // event has finished processing - so determine if it should be added to the list by peeking at its 'new value'...
            //            _webOrderRequestStatusFilter += "'" + _webOrderRequestStatusCodes.Rows[i]["value_member"].ToString() + "',";
            //        }
            //    }
            //    // Drop the extra semicolon at the end of the string...
            //    _webOrderRequestStatusFilter = _webOrderRequestStatusFilter.TrimEnd(',');

            //    // Refresh the web order data...
            //    RefreshWebOrderData();
            //}
        }

        private void ux_buttonCreateOrderRequest_Click(object sender, EventArgs e)
        {
            // If there is no active row in the web_order_request DGV - bail out now...
            if (ux_bindingNavigatorWebOrders.BindingSource.Current == null) return;

            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            // Create a new order_request record...
            DataRow newOrderRequest = ((DataTable)ux_bindingnavigatorForm.BindingSource.DataSource).NewRow();
            ((DataTable)ux_bindingnavigatorForm.BindingSource.DataSource).Rows.Add(newOrderRequest);
            // Move to it in the DGV...
            ux_bindingnavigatorForm.BindingSource.MoveLast();

            // Populate the new order_request record with data from the web_order_request record...
            DataRow webOrderRequest = ((DataRowView)ux_bindingNavigatorWebOrders.BindingSource.Current).Row;
            if (newOrderRequest.Table.Columns.Contains("ordered_date") && webOrderRequest.Table.Columns.Contains("ordered_date")) newOrderRequest["ordered_date"] = DateTime.Parse(webOrderRequest["ordered_date"].ToString()).ToString("d");
//if (newOrderRequest.Table.Columns.Contains("order_type_code")) newOrderRequest["order_type_code"] = "DI";
            if (newOrderRequest.Table.Columns.Contains("order_type_code") && webOrderRequest.Table.Columns.Contains("intended_use_code"))
            {
                string orderType = "DI";
                switch (webOrderRequest["intended_use_code"].ToString().Trim().ToUpper())
                {
                    case "HOME":
                        orderType = "NR";
                        break;
                    case "REPATRIATION":
                        orderType = "RP";
                        break;
                    case "RESEARCH":
                    case "EDUCATION":
                    case "OTHER":
                        orderType = "DI";
                        break;
                    default:
                        orderType = "DI";
                        break;
                }
                newOrderRequest["order_type_code"] = orderType;
            }
            if (newOrderRequest.Table.Columns.Contains("order_obtained_via")) newOrderRequest["order_obtained_via"] = "Web Order";
            if (newOrderRequest.Table.Columns.Contains("intended_use_code") && webOrderRequest.Table.Columns.Contains("intended_use_code")) newOrderRequest["intended_use_code"] = webOrderRequest["intended_use_code"];
            if (newOrderRequest.Table.Columns.Contains("intended_use_note") && webOrderRequest.Table.Columns.Contains("intended_use_note")) newOrderRequest["intended_use_note"] = webOrderRequest["intended_use_note"];
            if (newOrderRequest.Table.Columns.Contains("web_order_request_id") && webOrderRequest.Table.Columns.Contains("web_order_request_id")) newOrderRequest["web_order_request_id"] = webOrderRequest["web_order_request_id"];
            if (newOrderRequest.Table.Columns.Contains("web_cooperator_id") && webOrderRequest.Table.Columns.Contains("web_cooperator_id")) newOrderRequest["web_cooperator_id"] = webOrderRequest["web_cooperator_id"];
            if (newOrderRequest.Table.Columns.Contains("email") && webOrderRequest.Table.Columns.Contains("email")) newOrderRequest["email"] = webOrderRequest["email"];
            if (newOrderRequest.Table.Columns.Contains("primary_phone") && webOrderRequest.Table.Columns.Contains("primary_phone")) newOrderRequest["primary_phone"] = webOrderRequest["primary_phone"];
            if (newOrderRequest.Table.Columns.Contains("note") && webOrderRequest.Table.Columns.Contains("note")) newOrderRequest["note"] = webOrderRequest["note"];
            if (newOrderRequest.Table.Columns.Contains("special_instruction") && webOrderRequest.Table.Columns.Contains("special_instruction")) newOrderRequest["special_instruction"] = webOrderRequest["special_instruction"];

            // Attempt to find a cooperator that matches the web_cooperator in the web_order_request record...
            if (webOrderRequest.Table.Columns.Contains("last_name") &&
                webOrderRequest.Table.Columns.Contains("first_name") &&
                webOrderRequest.Table.Columns.Contains("organization") &&
                webOrderRequest.Table.Columns.Contains("geography_id") &&
                webOrderRequest.Table.Columns.Contains("address_line1"))
            {
                string last_name = webOrderRequest["last_name"].ToString();
                string first_name = webOrderRequest["first_name"].ToString();
                string organization = webOrderRequest["organization"].ToString();
                string geography_id = webOrderRequest["geography_id"].ToString();
                string address_line1 = webOrderRequest["address_line1"].ToString();
                string cooperator_id = FindCooperator(last_name, first_name, organization, geography_id, address_line1);
                if (!string.IsNullOrEmpty(cooperator_id))
                {
                    // Looks like a matching cooperator record exists...
                    if (newOrderRequest.Table.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = cooperator_id;
                }
                else
                {
                    // Looks like no matching cooperator exists for the cooperator in the web_order_request so ask the
                    // if they would like to create a new cooperator based on the data in the web_order_request...
                    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The recipient in this web order is not listed in the Cooperator Table.\n\nWould you like to create a new Cooperator now?\n(Clicking Yes will create the Cooperator and add it to the new Order Request.  Clicking No will create the Order Request with Final Recipient left blank).", "Order Wizard Cooperator Missing", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "OrderWizard_ux_buttonCreateOrderRequest1";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                    //if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
                    if (DialogResult.Yes == ggMessageBox.ShowDialog())
                    {
                        // Create a new cooperator
//ux_buttonCreateCooperator.PerformClick();
Cursor.Current = Cursors.WaitCursor;
CreateCooperator();
Cursor.Current = Cursors.WaitCursor;
                        cooperator_id = FindCooperator(last_name, first_name, organization, geography_id, address_line1);
                        if (!string.IsNullOrEmpty(cooperator_id))
                        {
                            if (newOrderRequest.Table.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = cooperator_id;
                        }
                    }
                }
            }

            // Create the new order_request_item records...
            foreach (DataRowView drv in ((DataTable)_webOrderRequestItemBindingSource.DataSource).DefaultView)
            {
                string distributionInventoryID = FindInventoryFromAccession(_webOrderRequestItem.Rows.Find(drv["web_order_request_item_id"])["accession_id"].ToString());
//DataTable destinationTable = (DataTable)((BindingSource)ux_datagridviewOrderRequestItem.DataSource).DataSource;
//DataRow newOrderItem = BuildOrderRequestItemRow(distributionInventoryID, destinationTable);
                DataRow newOrderItem = null;
                if (!ux_checkboxMySitesAccessionsOnly.Checked ||
                    !drv.Row.Table.Columns.Contains("site_id") ||
                    drv["site_id"].ToString().Trim().ToUpper() == _sharedUtils.UserSite.Trim().ToUpper())
                {
                    newOrderItem = BuildOrderRequestItemRow(distributionInventoryID, _orderRequestItem);
                    // Populate fields pulled from web_order_item now...
                    if (drv.Row.Table.Columns.Contains("web_order_request_item_id") && _orderRequestItem.Columns.Contains("web_order_request_item_id")) newOrderItem["web_order_request_item_id"] = drv["web_order_request_item_id"];
                    _orderRequestItem.Rows.Add(newOrderItem);
                }
            }

            // Create an Action for this order...
            DataRow newOrderRequestAction = _orderRequestAction.NewRow();
            if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = newOrderRequest["order_request_id"];
            if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "NEW";
            if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
            if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
            if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
            if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
            if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
            if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
            if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
            if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "New Order created from Web Order by " + _sharedUtils.Username;
            _orderRequestAction.Rows.Add(newOrderRequestAction);

            // Change the status of the web order request...
            webOrderRequest["status_code"] = "ACCEPTED";

            // Put focus on the Order tab page...
            ux_tabcontrolMain.SelectedTab = OrderPage;

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private string FindCooperator(string last_name, string first_name, string organization, string geography_id, string address_line1)
        {
            string cooperator_id = "";
            string findText = "";
            string searchPKeys = ":cooperatorid=";
            DataTable cooperators = new DataTable();

            // Force a refresh of the Cooperator LU table to get any brand new cooperators...
            _sharedUtils.LookupTablesUpdateTable("cooperator_lookup", false);

            // First get the cooperator_ids from the local LU table that match the lastname...
            if (!string.IsNullOrEmpty(last_name)) findText += last_name + "%";
            if (!string.IsNullOrEmpty(findText))
            {
                DataTable dt = new DataTable();

                dt = _sharedUtils.LookupTablesGetMatchingRows("cooperator_lookup", findText, 1000);
                foreach (DataRow dr in dt.Rows)
                {
                    searchPKeys += dr["value_member"].ToString() + ",";
                }
            }
            // Remove the last trailing comma...
            searchPKeys = searchPKeys.TrimEnd(',');
            // Next get the full collection of cooperator_ids that match the last name of the web_cooperator...
            DataSet ds = _sharedUtils.GetWebServiceData("get_cooperator", searchPKeys, 0, 0);
            if (ds.Tables.Contains("get_cooperator"))
            {
                cooperators = ds.Tables["get_cooperator"].Copy();
            }
            // Next iterate through the cooperator records to see if there is a perfect match for the web_cooperator info in the web_order_request...
            // based on last_name, first_name, organization, geography_id, and address_line1
            foreach (DataRow dr in cooperators.Rows)
            {
                bool match = true;
                //if (cooperators.Columns.Contains("last_name") && webOrderRequest.Table.Columns.Contains("last_name") && (dr["last_name"].ToString().ToLower() != webOrderRequest["last_name"].ToString().ToLower())) match = false;
                //if (cooperators.Columns.Contains("first_name") && webOrderRequest.Table.Columns.Contains("first_name") && (dr["first_name"].ToString().ToLower() != webOrderRequest["first_name"].ToString().ToLower())) match = false;
                //if (cooperators.Columns.Contains("organization") && webOrderRequest.Table.Columns.Contains("organization") && (dr["organization"].ToString().ToLower() != webOrderRequest["organization"].ToString().ToLower())) match = false;
                //if (cooperators.Columns.Contains("geography_id") && webOrderRequest.Table.Columns.Contains("geography_id") && (dr["geography_id"].ToString().ToLower() != webOrderRequest["geography_id"].ToString().ToLower())) match = false;
                //if (cooperators.Columns.Contains("address_line1") && webOrderRequest.Table.Columns.Contains("address_line1") && (dr["address_line1"].ToString().ToLower() != webOrderRequest["address_line1"].ToString().ToLower())) match = false;
                //if (match && newOrderRequest.Table.Columns.Contains("final_recipient_cooperator_id")) newOrderRequest["final_recipient_cooperator_id"] = dr["cooperator_id"].ToString();
                if (dr.Table.Columns.Contains("last_name") && (dr["last_name"].ToString().ToLower() != last_name.ToLower())) match = false;
                if (dr.Table.Columns.Contains("first_name") && (dr["first_name"].ToString().ToLower() != first_name.ToLower())) match = false;
                if (dr.Table.Columns.Contains("organization") && (dr["organization"].ToString().ToLower() != organization.ToLower())) match = false;
                if (dr.Table.Columns.Contains("geography_id") && (dr["geography_id"].ToString().ToLower() != geography_id.ToLower())) match = false;
                if (dr.Table.Columns.Contains("address_line1") && (dr["address_line1"].ToString().ToLower() != address_line1.ToLower())) match = false;
                if (match && dr.Table.Columns.Contains("cooperator_id")) cooperator_id = dr["cooperator_id"].ToString();
            }

            return cooperator_id;
        }

        private void ux_buttonCreateCooperator_Click(object sender, EventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            CreateCooperator();
            
            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void CreateCooperator()
        {
            // If there is no active row in the web_order_request DGV - bail out now...
            if (ux_bindingNavigatorWebOrders.BindingSource.Current == null) return;

            // Get an empty cooperator table from the remote server...
            DataSet ds = _sharedUtils.GetWebServiceData("get_cooperator", ":cooperatorid=", 0, 0);
            // Get the active web_order_request record in the DGV...
            DataRow webOrderRequest = ((DataRowView)ux_bindingNavigatorWebOrders.BindingSource.Current).Row;
            if (ds.Tables.Contains("get_cooperator") && webOrderRequest != null)
            {
                string last_name = webOrderRequest["last_name"].ToString();
                string first_name = webOrderRequest["first_name"].ToString();
                string organization = webOrderRequest["organization"].ToString();
                string geography_id = webOrderRequest["geography_id"].ToString();
                string address_line1 = webOrderRequest["address_line1"].ToString();
                // Check to see if this cooperator is already in the system...
                string cooperator_id = FindCooperator(last_name, first_name, organization, geography_id, address_line1);
                // If the cooperator does not exist in the cooperator_table - create it now...
                if (string.IsNullOrEmpty(cooperator_id))
                {
                    DataRow newCooperator = ds.Tables["get_cooperator"].NewRow();

                    if (newCooperator.Table.Columns.Contains("last_name") && webOrderRequest.Table.Columns.Contains("last_name")) newCooperator["last_name"] = webOrderRequest["last_name"];
                    if (newCooperator.Table.Columns.Contains("title") && webOrderRequest.Table.Columns.Contains("title")) newCooperator["title"] = webOrderRequest["title"];
                    if (newCooperator.Table.Columns.Contains("first_name") && webOrderRequest.Table.Columns.Contains("first_name")) newCooperator["first_name"] = webOrderRequest["first_name"];
                    if (newCooperator.Table.Columns.Contains("organization") && webOrderRequest.Table.Columns.Contains("organization")) newCooperator["organization"] = webOrderRequest["organization"];
                    if (newCooperator.Table.Columns.Contains("address_line1") && webOrderRequest.Table.Columns.Contains("address_line1")) newCooperator["address_line1"] = webOrderRequest["address_line1"];
                    if (newCooperator.Table.Columns.Contains("address_line2") && webOrderRequest.Table.Columns.Contains("address_line2")) newCooperator["address_line2"] = webOrderRequest["address_line2"];
                    if (newCooperator.Table.Columns.Contains("address_line3") && webOrderRequest.Table.Columns.Contains("address_line3")) newCooperator["address_line3"] = webOrderRequest["address_line3"];
                    if (newCooperator.Table.Columns.Contains("city") && webOrderRequest.Table.Columns.Contains("city")) newCooperator["city"] = webOrderRequest["city"];
                    if (newCooperator.Table.Columns.Contains("postal_index") && webOrderRequest.Table.Columns.Contains("postal_index")) newCooperator["postal_index"] = webOrderRequest["postal_index"];
                    if (newCooperator.Table.Columns.Contains("geography_id") && webOrderRequest.Table.Columns.Contains("geography_id")) newCooperator["geography_id"] = webOrderRequest["geography_id"];
                    if (newCooperator.Table.Columns.Contains("status_code")) newCooperator["status_code"] = "ACTIVE";
                    if (newCooperator.Table.Columns.Contains("sys_lang_id")) newCooperator["sys_lang_id"] = 1;
                    if (newCooperator.Table.Columns.Contains("primary_phone") && webOrderRequest.Table.Columns.Contains("primary_phone")) newCooperator["primary_phone"] = webOrderRequest["primary_phone"];
                    if (newCooperator.Table.Columns.Contains("email") && webOrderRequest.Table.Columns.Contains("email")) newCooperator["email"] = webOrderRequest["email"];
                    if (newCooperator.Table.Columns.Contains("web_cooperator_id") && webOrderRequest.Table.Columns.Contains("web_cooperator_id")) newCooperator["web_cooperator_id"] = webOrderRequest["web_cooperator_id"];
                    // Now add the new cooperator record to the table and save the results...
                    ds.Tables["get_cooperator"].Rows.Add(newCooperator);
                    DataSet saveCooperatorResults = _sharedUtils.SaveWebServiceData(ds);
                    if (saveCooperatorResults != null &&
                        saveCooperatorResults.Tables.Contains("ExceptionTable") &&
                        saveCooperatorResults.Tables["ExceptionTable"].Rows.Count > 0)
                    {
                        GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There were errors creating the new Cooperator record.\n\nFull error message:\n{0}", "Order Wizard Create Cooperator Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                        ggMessageBox.Name = "OrderWizard_ux_buttonCreateCooperatorMessage1";
                        _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                        if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, saveCooperatorResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString());
                        ggMessageBox.ShowDialog();
                    }
                    else
                    {
                        // Wait 5 seconds (so the cooperator LU table can successfully be updated)...
                        System.Threading.Thread.Sleep(5000);
                        GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The new Cooperator record was successfully created.", "Order Wizard Create Cooperator Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                        ggMessageBox.Name = "OrderWizard_ux_buttonCreateCooperatorMessage2";
                        _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                        ggMessageBox.ShowDialog();
                    }
                }
                else
                {
                    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("This Cooperator is already in the system - duplicate Cooperator records are not allowed.", "Order Wizard Duplicate Cooperator Not Allowed", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "OrderWizard_ux_buttonCreateCooperatorMessage3";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                    ggMessageBox.ShowDialog();
                }
            }
            // Force the new cooperator to be loaded in the Cooperator LU table (a Threading.Sleep above should have provided enough dwell time to allow a successful update)...
            _sharedUtils.LookupTablesUpdateTable("cooperator_lookup", false);
        }

        private void ux_buttonNewOrderRequestItemRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_orderRequestBindingSource.Current)[_orderRequest.PrimaryKey[0].ColumnName].ToString();
            DataRow newOrderRequestItem = _orderRequestItem.NewRow();
            newOrderRequestItem["order_request_id"] = pkey;
            //_orderRequestItem.Rows.Add(newOrderRequestItem);

            // Set the current cell at the beginning of this new record...
            int newRowIndex = ux_datagridviewOrderRequestItem.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewOrderRequestItem.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            // Find the row index of the new record...
            for (int i = 0; i < ux_datagridviewOrderRequestItem.Rows.Count; i++)
            {
                if (ux_datagridviewOrderRequestItem["order_request_item_id", i].Value.Equals(newOrderRequestItem["order_request_item_id"])) newRowIndex = i;
            }
            // Find the left-most visible column index...
            foreach (DataGridViewColumn dgvc in ux_datagridviewOrderRequestItem.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            // Set the current cell to the new records left-most column...
            if (newColIndex > -1 && newRowIndex > -1) ux_datagridviewOrderRequestItem.CurrentCell = ux_datagridviewOrderRequestItem[newColIndex, newRowIndex];

            GRINGlobal.Client.Common.FKeyPicker ftp = new GRINGlobal.Client.Common.FKeyPicker(_sharedUtils, "inventory_id", newOrderRequestItem, "");
            ftp.StartPosition = FormStartPosition.CenterParent;
            if (DialogResult.OK == ftp.ShowDialog())
            {
                //if (newOrderRequestItem != null)
                //{
                //    if (ftp.NewKey != null && newOrderRequestItem["inventory_id"].ToString().Trim() != ftp.NewKey.Trim())
                //    {
                //        newOrderRequestItem["inventory_id"] = ltp.NewKey.Trim();
                //        ux_datagridviewOrderRequestItem["inventory_id", newRowIndex].Value = ltp.NewValue.Trim();

                //        DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + ftp.NewKey.Trim(), 0, 0);
                //        if (ds.Tables.Contains("get_inventory") &&
                //            ds.Tables["get_inventory"].Rows.Count > 0)
                //        {
                //            DataRow dr = ds.Tables["get_inventory"].Rows[0];
                //            newOrderRequestItem["sequence_number"] = 99;
                //            newOrderRequestItem["name"] = dr["plant_name"];
                //            newOrderRequestItem["quantity_shipped"] = dr["distribution_default_quantity"];
                //            newOrderRequestItem["quantity_shipped_unit_code"] = dr["distribution_unit_code"];
                //            newOrderRequestItem["distribution_form_code"] = dr["distribution_default_form_code"];
                //            newOrderRequestItem["status_code"] = "NEW";
                //            newOrderRequestItem["status_date"] = DBNull.Value;
                //            newOrderRequestItem["inventory_id"] = dr["inventory_id"];
                //            newOrderRequestItem["accession_id"] = dr["accession_id"];
                //            newOrderRequestItem["external_taxonomy"] = _sharedUtils.GetLookupDisplayMember("taxonomy_species_lookup", dr["taxonomy_species_id"].ToString(), "", "");
                //        }
                //    }
                //    else if (ftp.NewKey == null)
                //    {
                //        newOrderRequestItem["inventory_id"] = DBNull.Value;
                //        ux_datagridviewOrderRequestItem["inventory_id", newRowIndex].Value = "";
                //    }
                //    newOrderRequestItem.SetColumnError("inventory_id", null);
                //    RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
                //}
                if (_orderRequestItem != null)
                {
                    string newKey = "";
                    if(!string.IsNullOrEmpty(ftp.NewKey)) newKey = ftp.NewKey.Trim();
                    newOrderRequestItem = BuildOrderRequestItemRow(newKey, _orderRequestItem);
                    //newOrderRequestItem.SetColumnError("inventory_id", null);
                    _orderRequestItem.Rows.Add(newOrderRequestItem);
                }
                RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
            }

        }

        private void ux_buttonOrderRequestItemRenumberItems_Click(object sender, EventArgs e)
        {
            int seqNo = 1;
            foreach (DataRowView drv in _orderRequestItem.DefaultView)
            {
                drv.Row["sequence_number"] = seqNo++;
            }
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        private void ux_buttonFindOrders_Click(object sender, EventArgs e)
        {
            if (ux_radiobuttonSelectionOrders.Checked)
            {
                _selectionList = "";
                string[] pkeys = ux_textboxSelection.Text.Split(new char[] { ',', ' ', '\t', '\n', 'r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string pkey in pkeys)
                {
                    _selectionList += pkey + ",";
                }
                _selectionList = _selectionList.TrimEnd(',');
            }
            // If the Order tab is active, refresh the Order data...
            if (ux_tabcontrolMain.SelectedTab == OrderPage || ux_tabcontrolMain.SelectedTab == ActionsPage) RefreshOrderData();
            // If the Web Order tab is active, refresh the Web Order data...
            if (ux_tabcontrolMain.SelectedTab == WebOrderPage) RefreshWebOrderData();
        }

        private void ux_dgvheadermenuSortAscending_Click(object sender, EventArgs e)
        {
            string fieldName = "";

            fieldName = ux_datagridviewOrderRequestItem.Columns[_mouseClickDGVColumnIndex].Name + "_sortable";
            if (_sortOrder.Contains(fieldName + " DESC"))
            {
                _sortOrder = _sortOrder.Replace(fieldName + " DESC", fieldName + " ASC");
            }
            else if (!_sortOrder.Contains(fieldName + " ASC"))
            {
                if (_sortOrder.Length > 0)
                {
                    _sortOrder += "," + fieldName + " ASC";
                }
                else
                {
                    _sortOrder += fieldName + " ASC";
                }
            }
            _sortOrder = _sortOrder.Replace(",,", ",");
            ((DataTable)((BindingSource)ux_datagridviewOrderRequestItem.DataSource).DataSource).DefaultView.Sort = _sortOrder;
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        private void ux_dgvheadermenuSortDescending_Click(object sender, EventArgs e)
        {
            string fieldName = "";

            fieldName = ux_datagridviewOrderRequestItem.Columns[_mouseClickDGVColumnIndex].Name + "_sortable";
            if (_sortOrder.Contains(fieldName + " ASC"))
            {
                _sortOrder = _sortOrder.Replace(fieldName + " ASC", fieldName + " DESC");
            }
            else if (!_sortOrder.Contains(fieldName + " DESC"))
            {
                if (_sortOrder.Length > 0)
                {
                    _sortOrder += "," + fieldName + " DESC";
                }
                else
                {
                    _sortOrder += fieldName + " DESC";
                }
            }
            _sortOrder = _sortOrder.Replace(",,", ",");
            ((DataTable)((BindingSource)ux_datagridviewOrderRequestItem.DataSource).DataSource).DefaultView.Sort = _sortOrder;
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        private void ux_dgvheadermenuNoSort_Click(object sender, EventArgs e)
        {
            string fieldName = "";

            fieldName = ux_datagridviewOrderRequestItem.Columns[_mouseClickDGVColumnIndex].Name + "_sortable";
            if (_sortOrder.Contains(fieldName + " DESC"))
            {
                _sortOrder = _sortOrder.Replace(fieldName + " DESC", "");
            }
            else if (_sortOrder.Contains(fieldName + " ASC"))
            {
                _sortOrder = _sortOrder.Replace(fieldName + " ASC", "");
            }
            // Remove all commas at the start and end of the string...
            _sortOrder = _sortOrder.TrimStart(',').TrimEnd(',');
            // Remove any double commas from the string...
            _sortOrder = _sortOrder.Replace(",,", ",");

            ((DataTable)((BindingSource)ux_datagridviewOrderRequestItem.DataSource).DataSource).DefaultView.Sort = _sortOrder;
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        private void ux_dgvheadermenuResetAllSorting_Click(object sender, EventArgs e)
        {
            _sortOrder = "";
            ((DataTable)((BindingSource)ux_datagridviewOrderRequestItem.DataSource).DataSource).DefaultView.Sort = _sortOrder;
            RefreshDGVFormatting(ux_datagridviewOrderRequestItem);
        }

        private void ux_datagridviewOrderRequestItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
//// Make sure the datagridview row is not still in edit mode (this will commit the data change to the datatable row)...
//((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).EndEdit();
//// Get the datatable datarow and column name...
//DataRow orderRequestItemRow = ((DataRowView)((DataGridView)sender).Rows[e.RowIndex].DataBoundItem).Row;
//string colName = ((DataGridView)sender).Columns[e.ColumnIndex].Name;
//// Begin processing the row sweep/sync...
//if (colName.ToLower() == "inventory_id")
//{
//    // If the inventory changed...
//    DataSet ds = _sharedUtils.GetWebServiceData("get_inventory", ":inventoryid=" + orderRequestItemRow[colName], 0, 0);
//    if (ds != null &&
//        ds.Tables.Contains("get_inventory") &&
//        ds.Tables["get_inventory"].Rows.Count > 0)
//    {
//        DataRow invRow = ds.Tables["get_inventory"].Rows[0];
//        // First copy all data that has matching column names AND is different...
//        foreach (DataColumn dc in orderRequestItemRow.Table.Columns)
//        {
//            syncDataRow(invRow, dc.ColumnName, orderRequestItemRow, dc.ColumnName);
//        }
//        // Now manually sync data for mismatched column names...
//        syncDataRow(invRow, "plant_name", orderRequestItemRow, "name");
//        syncDataRow(invRow, "taxonomy_species_id", orderRequestItemRow, "external_taxonomy");
//    }
//}
//else if (colName.ToLower() == "status_code")
//{
//    // If the item's status code changed...
//    if (orderRequestItemRow[colName, DataRowVersion.Original] != orderRequestItemRow[colName, DataRowVersion.Current])
//    {
//        bool orderComplete = true;
//        // Update the status date...
//        orderRequestItemRow["status_date"] = DateTime.UtcNow.ToString("d");
//        // Now check to see if all items in this order are either marked as 'SHIPPED' or 'CANCEL'
//        // and if so set the order request header's completed_date...
//        foreach (DataGridViewRow dgvr in ((DataGridView)sender).Rows)
//        {
//            if (dgvr.Cells["status_code"].Value.ToString().Trim().ToUpper() != "SHIPPED" &&
//                dgvr.Cells["status_code"].Value.ToString().Trim().ToUpper() != "CANCEL") orderComplete = false;
//        }
//        if (orderComplete)
//        {
//            // Mark the order_request completed_date to reflect the order is completed...
//            DataRow orderRequest = _orderRequest.Rows.Find(orderRequestItemRow["order_request_id"]);
//            if (orderRequest != null) orderRequest["completed_date"] = DateTime.UtcNow.ToString("d");
//            // Put an order_request_action in to document the order is completed...
//            // Create an Action for the status_code changes (one per staus_code with a count for how many items were changed to that status)...
//            DataRow newOrderRequestAction = _orderRequestAction.NewRow();
//            if (_orderRequestAction.Columns.Contains("order_request_id")) newOrderRequestAction["order_request_id"] = orderRequestItemRow["order_request_id"];
//            if (_orderRequestAction.Columns.Contains("action_name_code")) newOrderRequestAction["action_name_code"] = "SHIPPED";
//            if (_orderRequestAction.Columns.Contains("started_date")) newOrderRequestAction["started_date"] = DateTime.UtcNow;
//            if (_orderRequestAction.Columns.Contains("started_date_code")) newOrderRequestAction["started_date_code"] = "MM/dd/yyyy";
//            if (_orderRequestAction.Columns.Contains("completed_date")) newOrderRequestAction["completed_date"] = DateTime.UtcNow;
//            if (_orderRequestAction.Columns.Contains("completed_date_code")) newOrderRequestAction["completed_date_code"] = "MM/dd/yyyy";
//            if (_orderRequestAction.Columns.Contains("action_information")) newOrderRequestAction["action_information"] = DBNull.Value;
//            if (_orderRequestAction.Columns.Contains("action_cost")) newOrderRequestAction["action_cost"] = DBNull.Value;
//            if (_orderRequestAction.Columns.Contains("cooperator_id")) newOrderRequestAction["cooperator_id"] = DBNull.Value;
//            if (_orderRequestAction.Columns.Contains("note")) newOrderRequestAction["note"] = "Order Request has been marked as COMPLETED by " + _sharedUtils.Username;
//            // And add it to the Order Request Action Table...
//            _orderRequestAction.Rows.Add(newOrderRequestAction);

//        }
//        else
//        {
//            DataRow orderRequest = _orderRequest.Rows.Find(orderRequestItemRow["order_request_id"]);
//            if (orderRequest != null) orderRequest["completed_date"] = DBNull.Value;
//        }
//    }
//}
//// Refresh cell formating colors to show changed cells...
//RefreshDGVFormatting((DataGridView)sender);

        }

        private void syncDataRow(DataRow fromRow, string fromColumnName, DataRow toRow, string toColumnName)
        {
            if (fromRow.Table.Columns.Contains(fromColumnName) &&
                toRow.Table.Columns.Contains(toColumnName) &&
                toRow[toColumnName] != fromRow[fromColumnName])
            {
                // Make sure the data types match...
                if (fromRow.Table.Columns[fromColumnName].DataType == toRow.Table.Columns[toColumnName].DataType)
                {
                    if (toRow.Table.Columns[toColumnName].ReadOnly)
                    {
                        toRow.Table.Columns[toColumnName].ReadOnly = false;
                        toRow[toColumnName] = fromRow[fromColumnName];
                        toRow.Table.Columns[toColumnName].ReadOnly = true;
                    }
                    else
                    {
                        toRow[toColumnName] = fromRow[fromColumnName];
                    }
                }
                // If the data types do not match attempt to convert...
                else
                {
                    if (toRow.Table.Columns[toColumnName].DataType == typeof(string))
                    {
                        if (_sharedUtils.LookupTablesIsValidFKField(fromRow.Table.Columns[fromColumnName]))
                        {
                            if (toRow.Table.Columns[toColumnName].ReadOnly)
                            {
                                toRow.Table.Columns[toColumnName].ReadOnly = false;
                                toRow[toColumnName] = _sharedUtils.GetLookupDisplayMember(fromRow.Table.Columns[fromColumnName].ExtendedProperties["foreign_key_dataview_name"].ToString(), fromRow[fromColumnName].ToString(), "", fromRow[fromColumnName].ToString());
                                toRow.Table.Columns[toColumnName].ReadOnly = true;
                            }
                            else
                            {
                                toRow[toColumnName] = _sharedUtils.GetLookupDisplayMember(fromRow.Table.Columns[fromColumnName].ExtendedProperties["foreign_key_dataview_name"].ToString(), fromRow[fromColumnName].ToString(), "", fromRow[fromColumnName].ToString());
                            }
                        }
                        else if (_sharedUtils.LookupTablesIsValidCodeValueField(fromRow.Table.Columns[fromColumnName]))
                        {
                            if (toRow.Table.Columns[toColumnName].ReadOnly)
                            {
                                toRow.Table.Columns[toColumnName].ReadOnly = false;
                                toRow[toColumnName] = _sharedUtils.GetLookupDisplayMember("code_value_lookup", fromRow[fromColumnName].ToString(), fromRow.Table.Columns[fromColumnName].ExtendedProperties["group_name"].ToString(), fromRow[fromColumnName].ToString());
                                toRow.Table.Columns[toColumnName].ReadOnly = true;
                            }
                            else
                            {
                                toRow[toColumnName] = _sharedUtils.GetLookupDisplayMember("code_value_lookup", fromRow[fromColumnName].ToString(), fromRow.Table.Columns[fromColumnName].ExtendedProperties["group_name"].ToString(), fromRow[fromColumnName].ToString());
                            }
                        }
                        else
                        {
                            if (toRow.Table.Columns[toColumnName].ReadOnly)
                            {
                                toRow.Table.Columns[toColumnName].ReadOnly = false;
                                toRow[toColumnName] = fromRow[fromColumnName].ToString();
                                toRow.Table.Columns[toColumnName].ReadOnly = true;
                            }
                            else
                            {
                                toRow[toColumnName] = fromRow[fromColumnName].ToString();
                            }
                        }
                    }
                }
                // Sync the sortable text field if one exists...
                if (toRow.Table.Columns.Contains(toColumnName + "_sortable")) toRow[toColumnName + "_sortable"] = GetSortableColumnText(toRow, toColumnName);
            }
        }

        private void ux_datagridviewOrderRequestItem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            dgv.EndEdit();
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                DataGridViewCell dgvc = dgv[e.ColumnIndex, e.RowIndex];
                DataRow dr = ((DataRowView)((DataGridViewRow)dgvc.OwningRow).DataBoundItem).Row;
                dr.EndEdit();
                string dcName = ((DataGridViewColumn)dgvc.OwningColumn).Name;
                string currentValue = dr[dcName, DataRowVersion.Current].ToString().Trim();
                string originalValue = currentValue;
                if (dr.RowState == DataRowState.Modified) originalValue = dr[dcName, DataRowVersion.Original].ToString().Trim();
                string sortableValue = dr[dcName + "_sortable", DataRowVersion.Current].ToString().Trim();
                if (originalValue != currentValue ||
                    GetSortableColumnText(dr, dcName) != sortableValue)
                {
                    //dr.SetColumnError(dcName, null);
                    //dr.EndEdit();
                    if (dr.Table.Columns.Contains(dcName + "_sortable")) dr[dcName + "_sortable"] = GetSortableColumnText(dr, dcName);
                    ApplyOrderItemBusinessRules(dgv.Rows[e.RowIndex]);
                    RefreshDGVFormatting(dgv);
                    //dgvc.Style.BackColor = Color.Yellow;
                }
                //else
                //{
                //    dgvc.Style.BackColor = Color.Empty;
                //}
            }
        }

        private string GetSortableColumnText(DataRow dr, string dcName)
        {
            string sortableValue = "";
            DataColumn dc = dr.Table.Columns[dcName];
            // Update the sortable (hidden) columns...
            if (_sharedUtils.LookupTablesIsValidFKField(dc))
            {
                string lookupTable = dc.ExtendedProperties["foreign_key_dataview_name"].ToString();
                if (dr[dcName] != DBNull.Value)
                {
//dr[dcName + "_sortable"] = _sharedUtils.GetLookupDisplayMember(lookupTable, dr[dcName].ToString(), "", dr[dcName].ToString());
sortableValue = _sharedUtils.GetLookupDisplayMember(lookupTable, dr[dcName].ToString(), "", dr[dcName].ToString());
                }
            }
            else if (_sharedUtils.LookupTablesIsValidCodeValueField(dc))
            {
                if (dr[dcName] != DBNull.Value)
                {
//dr[dcName + "_sortable"] = _sharedUtils.GetLookupDisplayMember("code_value_lookup", dr[dcName].ToString(), dc.ExtendedProperties["group_name"].ToString(), dr[dcName].ToString());
sortableValue = _sharedUtils.GetLookupDisplayMember("code_value_lookup", dr[dcName].ToString(), dc.ExtendedProperties["group_name"].ToString(), dr[dcName].ToString());
                }
            }
            else if (dc.DataType == typeof(int) || dc.DataType == typeof(decimal))
            {
                if (dr[dcName] != DBNull.Value)
                {
//dr[dcName + "_sortable"] = decimal.Parse(dr[dcName].ToString()).ToString("00000000000000000000.0000000000");
sortableValue = decimal.Parse(dr[dcName].ToString()).ToString("00000000000000000000.0000000000");
                }
            }
            else if (dc.DataType == typeof(DateTime))
            {
                if (dr[dcName] != DBNull.Value)
                {
//dr[dcName + "_sortable"] = ((DateTime)dr[dcName]).ToString("u");
sortableValue = ((DateTime)dr[dcName]).ToString("u");
                }
            }
            else
            {
                if (dr[dcName] != DBNull.Value)
                {
//dr[dcName + "_sortable"] = dr[dcName].ToString().ToLower();
sortableValue = dr[dcName].ToString().ToLower();
                }
            }

            return sortableValue;
        }

        private void ux_buttonPrintCrystalReport_Click(object sender, EventArgs e)
        {
//DataTable dt = ((DataTable)((BindingSource)ux_datagridviewMain.DataSource).DataSource).Clone();

//// NOTE: because of the way the DGV adds rows to the selectedRows collection
////       we have to process the rows in the opposite direction they were selected in...
//int rowStart = 0;
//int rowStop = ux_datagridviewMain.SelectedRows.Count;
//int stepValue = 1;
//if (ux_datagridviewMain.SelectedRows.Count > 1 && ux_datagridviewMain.SelectedRows[0].Index > ux_datagridviewMain.SelectedRows[1].Index)
//{
//    rowStart = ux_datagridviewMain.SelectedRows.Count - 1;
//    rowStop = -1;
//    stepValue = -1;
//}

//DataGridViewRow dgvrow = null;
//// Process the rows in the opposite direction they were selected by the user...
//for (int i = rowStart; i != rowStop; i += stepValue)
//{
//    dgvrow = ux_datagridviewMain.SelectedRows[i];
//    if (!dgvrow.IsNewRow)
//    {
//        dt.Rows.Add(((DataRowView)dgvrow.DataBoundItem).Row.ItemArray);
//    }
//}

//            ReportForm crustyReport = new ReportForm(dt, @"C:\VisualStudio2008_SVN\MyPlayground\MyPlayground\FieldLabel1.rpt");
            string fullPathName = System.Windows.Forms.Application.StartupPath + "\\Reports\\" + ux_comboboxCrystalReports.Text;
            if (System.IO.File.Exists(fullPathName))
            {
                // By default print just the current order...
                string pkeys = ":orderrequestid=" + ((DataRowView)_orderRequestBindingSource.Current).Row["order_request_id"].ToString();
                // Unless the user wants all orders in the Wizard printed at once...
                if (ux_checkboxPrintAllOrders.Checked) pkeys = _orderRequestPKeys;
                // Find a compatible dataview for the selected Crystal Report...
                System.Collections.Generic.Dictionary<string, string> reportsMap = _sharedUtils.GetReportsMapping();
                string dataviewName = "";
                if (reportsMap.ContainsKey(ux_comboboxCrystalReports.Text.Trim().ToUpper()))
                {
                    string dataviewNames = reportsMap[ux_comboboxCrystalReports.Text.Trim().ToUpper()];
                    if (dataviewNames.Length > 0)
                    {
                        dataviewName = dataviewNames.Split('|')[0].Trim().ToLower();
                    }
                }

                if (!string.IsNullOrEmpty(dataviewName))
                {
                    // Get the data using the suggested dataview...
                    DataSet ds = _sharedUtils.GetWebServiceData(dataviewName, pkeys, 0, 0);
                    // Get a copy of the Reports-->Dataviews Mapping...
                    if (ds.Tables.Contains(dataviewName))
                    {
                        DataGridView dgv = new DataGridView();
                        _sharedUtils.BuildReadOnlyDataGridView(dgv, ds.Tables[dataviewName]);
                        if (dgv != null &&
                            dgv.DataSource != null &&
                            dgv.DataSource.GetType() == typeof(DataTable))
                        {
                            // Okay it looks like we have a datagridview with a datasource = datatable (with FKeys and code_values resolved) - so extract it and use in the report...
                            DataTable dt = (DataTable)dgv.DataSource;
                            // Got the data so now we can generate the Crystal Report...
                            GRINGlobal.Client.Common.ReportForm crustyReport = new ReportForm(dt, fullPathName);
                            crustyReport.StartPosition = FormStartPosition.CenterParent;
                            _sharedUtils.UpdateControls(crustyReport.Controls, crustyReport.Name);
                            crustyReport.ShowDialog();
                        }
                    }
                }
            }
        }
    }
}


//            private void TestDGVSorter()
//        {
//            List<KeyValuePair<DataGridViewColumn, bool>> sortOrder = new List<KeyValuePair<DataGridViewColumn,bool>>();
//            sortOrder.Add(new KeyValuePair<DataGridViewColumn, bool>(ux_datagridviewOrderRequestItem.Columns[1], true));
//            sortOrder.Add(new KeyValuePair<DataGridViewColumn, bool>(ux_datagridviewOrderRequestItem.Columns[2], true));
//            sortOrder.Add(new KeyValuePair<DataGridViewColumn, bool>(ux_datagridviewOrderRequestItem.Columns[3], true));

//            GridRowComparer grc = new GridRowComparer(sortOrder);
//            ux_datagridviewOrderRequestItem.Sort(grc);
//        }


//    public class GridRowComparer : System.Collections.IComparer
//    {
//        private List<KeyValuePair<DataGridViewColumn, bool>> _columnList;

//        public GridRowComparer(List<KeyValuePair<DataGridViewColumn, bool>> columnList)
//        {
//            _columnList = columnList;
//        }

//        public int Compare(object x, object y)
//        {
//            DataGridViewRow DataGridViewRow1 = ((DataGridViewRow)(x));
//            DataGridViewRow DataGridViewRow2 = ((DataGridViewRow)(y));

//            int CompareResult = compareResult(DataGridViewRow1, DataGridViewRow2, 0);

//            return (CompareResult);
//        }

//        public int compareResult(DataGridViewRow DataGridViewRow1, DataGridViewRow DataGridViewRow2, int i)
//        {
//            DataGridViewColumn dgvColumn = _columnList[i].Key;

//            int sortOrderModifier = 0;
//            if (_columnList[i].Value)
//                sortOrderModifier = 1;
//            else
//                sortOrderModifier = -1;

//            int CompareResult = 0;

//            object value1 = DataGridViewRow1.Cells[dgvColumn.Index].Value;
//            object value2 = DataGridViewRow2.Cells[dgvColumn.Index].Value;

//            //Sort Images Together if images are in datagrid view
//            if ((value1 is System.Drawing.Bitmap) && !(value2 is System.Drawing.Bitmap))
//                return -1 * sortOrderModifier;
//            else if (!(value1 is System.Drawing.Bitmap) && 
//            (value2 is System.Drawing.Bitmap))
//                return 1 * sortOrderModifier;
//            else if (value1 is System.Drawing.Bitmap && value2 is System.Drawing.Bitmap)
//                return 0;

//            string cellValue1 = Convert.ToString
//                (DataGridViewRow1.Cells[dgvColumn.Index].Value);
//            string cellValue2 = Convert.ToString
//                (DataGridViewRow2.Cells[dgvColumn.Index].Value);

//            //When Cell value is null or empty
//            if ((cellValue1 == null || cellValue1 == string.Empty) && 
//            (cellValue2 != null || cellValue2 != string.Empty))
//                return -1 * sortOrderModifier;
//            else if ((cellValue1 != null || cellValue1 != string.Empty) && 
//            (cellValue2 == null || cellValue2 == string.Empty))
//                return 1 * sortOrderModifier;
//            else if ((cellValue1 == null || cellValue1 == string.Empty) && 
//            (cellValue2 == null || cellValue2 != string.Empty))
//                return 0;

//            //compare Numeric values
//            if (dgvColumn.ValueType == typeof(Double))
//            {
//                double numVal1 = Convert.ToDouble(cellValue1);
//                double numVal2 = Convert.ToDouble(cellValue2);

//                if (numVal1 > numVal2)
//                    CompareResult = 1;
//                else if (numVal1 < numVal2)
//                    CompareResult = -1;
//                else
//                    CompareResult = 0;
//            }
//            //compare date values
//            else if (dgvColumn.ValueType == typeof(DateTime))
//            {
//                DateTime cellValueDt1;
//                DateTime cellValueDt2;

//                if ((DateTime.TryParse(cellValue1, out cellValueDt1)) && 
//                    (DateTime.TryParse(cellValue2, out cellValueDt2)))
//                {
//                    if (cellValueDt1 > cellValueDt2)
//                        CompareResult = 1;
//                    else if (cellValueDt1 < cellValueDt2)
//                        CompareResult = -1;
//                    else
//                        CompareResult = 0;
//                }
//            }
//            else //compare string values
//            {
//                CompareResult = System.String.Compare(cellValue1, cellValue2);
//            }

//            CompareResult = CompareResult * sortOrderModifier;

//            //if same values, perform this routine again
//            if (CompareResult == 0)
//            {
//                if (i != _columnList.Count - 1)
//                {
//                    i++;
//                    CompareResult = compareResult(DataGridViewRow1, DataGridViewRow2, i);
//                }
//            }
//            return CompareResult;
//        }
//    }


