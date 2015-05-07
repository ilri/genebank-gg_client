using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace InventoryForm
{
    interface IGRINGlobalDataForm
    {
        string FormName { get; }
        string PreferredDataview { get; }
        bool EditMode { get; set; }
    }

    public partial class InventoryForm : Form, IGRINGlobalDataForm
    {
//LookupTables _lookupTables;
        SharedUtils _sharedUtils;
        bool _performLookups = false;
        BindingSource _bindingSource;
        bool _editMode = false;

//public InventoryForm(BindingSource bindingSource, bool performLookups, LookupTables lookupTables, bool editMode)
        public InventoryForm(BindingSource bindingSource, bool performLookups, SharedUtils sharedUtils, bool editMode)
        {
            InitializeComponent();

            _bindingSource = bindingSource;
            _bindingSource.ListChanged += new ListChangedEventHandler(bindingSource_ListChanged);
//_lookupTables = lookupTables;
            _sharedUtils = sharedUtils;
            _performLookups = performLookups;
            _editMode = editMode;
        }

        private void InventoryForm_Load(object sender, EventArgs e)
        {
            this.Text = FormName;
            bindControls(this.Controls);
            formatControls(this.Controls, _editMode);
            ux_bindingnavigatorForm.BindingSource = _bindingSource;
            
        }

        private void InventoryForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _bindingSource.ListChanged -= bindingSource_ListChanged;
        }

        public string FormName
        {
            get
            {
                return "Inventory Form";
            }
        }

        public string PreferredDataview
        {
            get
            {
                return "get_inventory";
            }
        }

        public bool EditMode
        {
            get
            {
                return _editMode;
            }
            set
            {
                _editMode = value;
                _performLookups = value;
                bindControls(this.Controls);
                if (_bindingSource.Count < 1)
                {
                    formatControls(this.Controls, false);
                }
                else
                {
                    formatControls(this.Controls, _editMode);
                }
            }
        }

        private void bindControls(Control.ControlCollection controlCollection)
        {
            foreach (Control ctrl in controlCollection)
            {
                if (ctrl != ux_bindingnavigatorForm)  // Leave the bindingnavigator alone
                {
                    // If the ctrl has children - bind them too...
                    if (ctrl.Controls.Count > 0)
                    {
                        bindControls(ctrl.Controls);
                    }
                    // Bind the control (by type)...
                    if (ctrl is ComboBox) bindComboBox((ComboBox)ctrl, _bindingSource);
                    if (ctrl is TextBox) bindTextBox((TextBox)ctrl, _bindingSource);
                    if (ctrl is CheckBox) bindCheckBox((CheckBox)ctrl, _bindingSource);
                    if (ctrl is DateTimePicker) bindDateTimePicker((DateTimePicker)ctrl, _bindingSource);
                    if (ctrl is Label) bindLabel((Label)ctrl, _bindingSource);
                }
            }
        }

        private void formatControls(Control.ControlCollection controlCollection, bool EditMode)
        {
            foreach (Control ctrl in controlCollection)
            {
                if (ctrl != ux_bindingnavigatorForm)  // Leave the bindingnavigator alone
                {
                    // If the ctrl has children - set their edit mode too...
                    if (ctrl.Controls.Count > 0)
                    {
                        formatControls(ctrl.Controls, EditMode);
                    }
                    // Set the edit mode for the control...
                    if (ctrl != null &&
                        ctrl.Tag != null &&
                        ctrl.Tag is string &&
                        _bindingSource != null &&
                        _bindingSource.DataSource is DataTable &&
                        ((DataTable)_bindingSource.DataSource).Columns.Contains(ctrl.Tag.ToString().Trim().ToLower()))
                    {
                        if (ctrl is TextBox)
                        {
                            // TextBoxes have a ReadOnly property in addition to an Enabled property so we handle this one separate...
                            ((TextBox)ctrl).ReadOnly = !EditMode || ((DataTable)_bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
                        }
                        else if (ctrl is Label)
                        {
                            // Do nothing to the Label
                        }
                        else
                        {
                            // All other control types (ComboBox, CheckBox, DateTimePicker) except Labels...
                            ctrl.Enabled = EditMode && !((DataTable)_bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
                        }
                    }
                }
            }
        }

        void control_TextChanged(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;

            // If the field is not nullable and the data is empty - indicate missing data using the control's background color...
            if (ctrl != null &&
                ctrl.Tag != null &&
                ctrl.Tag is string &&
                _bindingSource != null &&
                _bindingSource.DataSource is DataTable &&
                ((DataTable)_bindingSource.DataSource).Columns.Contains(ctrl.Tag.ToString().Trim().ToLower()))
            {
                if (((DataTable)_bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ExtendedProperties.Contains("is_nullable") &&
                    ((DataTable)_bindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ExtendedProperties["is_nullable"].ToString().Trim().ToUpper() == "N" &&
                    string.IsNullOrEmpty(ctrl.Text))
                {
                    ctrl.BackColor = Color.Plum;
                }
                else
                {
                    ctrl.BackColor = Color.Empty;
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
                }


//                if (_sharedUtils != null &&
//                    _sharedUtils.LookupTablesContains("MRU_code_value_lookup"))
//                {
//                    DataColumn dc = ((DataTable)bindingSource.DataSource).Columns[comboBox.Tag.ToString().Trim().ToLower()];
////DataTable cvl = _lookupTables["MRU_code_value_lookup"];
//                    DataTable cvl = _sharedUtils.LookupTablesGetMRUTable("MRU_code_value_lookup");

//                    //if (dc.ExtendedProperties.Contains("code_group_id") && cvl != null)
//                    if (dc.ExtendedProperties.Contains("group_name") && cvl != null)
//                    {
//                        //DataView dv = new DataView(cvl, "code_group_id='" + dc.ExtendedProperties["code_group_id"].ToString() + "'", "display_member ASC", DataViewRowState.CurrentRows);
//                        DataView dv = new DataView(cvl, "group_name='" + dc.ExtendedProperties["group_name"].ToString() + "'", "display_member ASC", DataViewRowState.CurrentRows);
//                        DataTable dt = dv.ToTable();
//                        if (dc.ExtendedProperties.Contains("is_nullable") && dc.ExtendedProperties["is_nullable"].ToString() == "Y")
//                        {
//                            DataRow dr = dt.NewRow();
//                            foreach (DataColumn cvldc in cvl.Columns)
//                            {
//                                // If there are any non-nullable fields - set them now...
//                                if (!cvldc.AllowDBNull)
//                                {
//                                    dr[cvldc.ColumnName] = -1;
//                                }
//                            }
//                            dr["display_member"] = "[Null]";
//                            dr["value_member"] = DBNull.Value;
//                            dt.Rows.InsertAt(dr, 0);
//                            dt.AcceptChanges();
//                        }
//                        comboBox.DisplayMember = "display_member";
//                        comboBox.ValueMember = "value_member";
//                        comboBox.DataSource = dt;
//                    }

//                    // Calculate the maximum width needed for displaying the dropdown items and set the combobox property...
//                    int maxWidth = comboBox.DropDownWidth;
//                    foreach (DataRow dr in cvl.Rows)
//                    {
//                        if (TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width > maxWidth)
//                        {
//                            maxWidth = TextRenderer.MeasureText(dr["display_member"].ToString().Trim(), comboBox.Font).Width;
//                        }
//                    }
//                    comboBox.DropDownWidth = maxWidth;

//                }

                if (_performLookups)
                {
                    // Bind the SelectedValue property to the binding source...
                    comboBox.DataBindings.Add("SelectedValue", bindingSource, comboBox.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);
                    // Wire up to an event handler if this column is a date_code (format) field...
                    if (comboBox.Tag.ToString().Trim().ToLower().EndsWith("_code") &&
                        ((DataTable)bindingSource.DataSource).Columns.Contains(comboBox.Tag.ToString().Trim().ToLower().Replace("_code", "")))
                    {
                        comboBox.SelectedIndexChanged -= new EventHandler(comboBox_SelectedIndexChanged);
                        comboBox.SelectedIndexChanged -= new EventHandler(comboBox_SelectedIndexChanged);
                        comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
                    }
                }
                else
                {
                    // Bind the Text property to the binding source...
                    comboBox.DataBindings.Add("Text", bindingSource, comboBox.Tag.ToString().Trim().ToLower(), true, DataSourceUpdateMode.OnPropertyChanged);
                }

                // Bind to an event handler for formatting the background color...
                comboBox.TextChanged += new EventHandler(control_TextChanged);
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
                if (_performLookups &&
                    _sharedUtils.LookupTablesIsValidFKField(dc))
                {
                    // Create a new binding that handles display_member/value_member conversions...
                    Binding textBinding = new Binding("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                    textBinding.Format += new ConvertEventHandler(textLUBinding_Format);
                    textBinding.Parse += new ConvertEventHandler(textLUBinding_Parse);
                    textBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    // Bind it to the textbox...
                    textBox.DataBindings.Add(textBinding);
                    // Add an event handler for processing the first key press (to display the lookup picker dialog)...
                    textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
                    textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
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
                    // Bind to the readonly datatable without lookups (because it is readonly and has already 
                    // had the Foreign Key and Code/Value lookup performed...
                    textBox.DataBindings.Add("Text", bindingSource, textBox.Tag.ToString().Trim().ToLower());
                }
            }

            // Bind to an event handler for formatting the background color...
            textBox.TextChanged += new EventHandler(control_TextChanged);
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
            // Bind to an event handler for formatting the background color...
            dateTimePicker.TextChanged += new EventHandler(control_TextChanged);
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

        void bindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (_bindingSource.List.Count < 1)
            {
                formatControls(this.Controls, false);
            }
            else
            {
                formatControls(this.Controls, _editMode);
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
                if (_editMode)
                {
                    dateFormat = drv[dc.ColumnName + "_code"].ToString().Trim();
                }
                else
                {
//dateFormat = _sharedUtils.GetLookupValueMember("code_value_lookup", drv[dc.ColumnName + "_code"].ToString().Trim(), "group_name='" + drv.Row.Table.Columns[dc.ColumnName + "_code"].ExtendedProperties["group_name"].ToString() + "'", dateFormat);
                    dateFormat = _sharedUtils.GetLookupValueMember(drv.Row, "code_value_lookup", drv[dc.ColumnName + "_code"].ToString().Trim(), drv.Row.Table.Columns[dc.ColumnName + "_code"].ExtendedProperties["group_name"].ToString(), dateFormat);
                }
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
            if (_performLookups &&
                !string.IsNullOrEmpty(e.Value.ToString()))
            {
//e.Value = _lookupTables.GetDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
                //e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
                e.Value = _sharedUtils.GetLookupDisplayMember(dc.ExtendedProperties["foreign_key_dataview_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
            }
        }

        void textLUBinding_Parse(object sender, ConvertEventArgs e)
        {
            Binding b = (Binding)sender;
            DataTable dt = (DataTable)((BindingSource)b.DataSource).DataSource;
            DataColumn dc = dt.Columns[b.BindingMemberInfo.BindingMember];
            if (_performLookups &&
                !string.IsNullOrEmpty(e.Value.ToString()))
            {
//e.Value = _lookupTables.GetValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
                //e.Value = _sharedUtils.GetLookupValueMember(dc.ExtendedProperties["foreign_key_resultset_name"].ToString(), e.Value.ToString(), "", e.Value.ToString());
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
                        ctrl.Text = ((DateTime)drv[dateColumnName]).ToString(dateFormat);
                    }
                }
            }
        }

        void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != Convert.ToChar(Keys.Escape)) // Ignore the Escape key and process anything else...
            {
                TextBox tb = (TextBox)sender;

                if (!tb.ReadOnly)
                {
//LookupTablePicker ltp = new LookupTablePicker(_lookupTables, tb.Tag.ToString(), ((DataRowView)_bindingSource.Current).Row, tb.Text);
                    LookupTablePicker ltp = new LookupTablePicker(_sharedUtils, tb.Tag.ToString(), ((DataRowView)_bindingSource.Current).Row, tb.Text);
                    ltp.StartPosition = FormStartPosition.CenterParent;
                    if (DialogResult.OK == ltp.ShowDialog())
                    {
                        tb.Text = ltp.NewValue.Trim();
                    }
                    e.Handled = true;
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
//LookupTablePicker ltp = new LookupTablePicker(_lookupTables, tb.Tag.ToString(), ((DataRowView)_bindingSource.Current).Row, tb.Text);
                    LookupTablePicker ltp = new LookupTablePicker(_sharedUtils, tb.Tag.ToString(), ((DataRowView)_bindingSource.Current).Row, tb.Text);
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
