using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace AccessionWizard
{
    interface IGRINGlobalDataWizard
    {
        string FormName { get; }
        DataTable ChangedRecords { get; }
        string PKeyName { get; }
    }

    public partial class AccessionWizard : Form, IGRINGlobalDataWizard
    {
        SharedUtils _sharedUtils;
        DataTable _accession;
        DataTable _accessionName;
        DataTable _accessionSource;
        DataTable _accessionSourceCooperator;
        DataTable _accessionSourceDescObservation;
        DataTable _accessionAnnotation;
        DataTable _accessionVoucher;
        DataTable _accessionIPR;
        DataTable _accessionQuarantine;
        DataTable _accessionPedigree;
        DataTable _accessionAction;
        BindingSource _accessionBindingSource;
        BindingSource _accessionNameBindingSource;
        BindingSource _accessionSourceBindingSource;
        BindingSource _accessionSourceCooperatorBindingSource;
        BindingSource _accessionSourceDescObservationBindingSource;
        BindingSource _accessionAnnotationBindingSource;
        BindingSource _accessionVoucherBindingSource;
        BindingSource _accessionIPRBindingSource;
        BindingSource _accessionQuarantineBindingSource;
        BindingSource _accessionPedigreeBindingSource;
        BindingSource _accessionActionBindingSource;
        string _originalPKeys = "";
        string _accessionPKeys = "";
        DataSet _changedRecords = new DataSet();

        public AccessionWizard(string pKeys, SharedUtils sharedUtils)
        {
            InitializeComponent();

            _accessionBindingSource = new BindingSource();
            _accessionBindingSource.ListChanged += new ListChangedEventHandler(_mainBindingSource_ListChanged);
            _accessionBindingSource.CurrentChanged += new EventHandler(_mainBindingSource_CurrentChanged);
            _accessionNameBindingSource = new BindingSource();
            _accessionSourceBindingSource = new BindingSource();
            _accessionSourceBindingSource.CurrentChanged += new EventHandler(_accessionSourceBindingSource_CurrentChanged);
            _accessionSourceCooperatorBindingSource = new BindingSource();
            _accessionSourceDescObservationBindingSource = new BindingSource();
            _accessionAnnotationBindingSource = new BindingSource();
            _accessionVoucherBindingSource = new BindingSource();
            _accessionIPRBindingSource = new BindingSource();
            _accessionQuarantineBindingSource = new BindingSource();
            _accessionPedigreeBindingSource = new BindingSource();
            _accessionActionBindingSource = new BindingSource();
            _sharedUtils = sharedUtils;
            _originalPKeys = pKeys;
            // Ignore all pkey tokens except the accession_id pkeys...
            foreach (string pkeyToken in pKeys.Split(';'))
            {
                if (pkeyToken.Split('=')[0].Trim().ToUpper() == ":ACCESSIONID") _accessionPKeys = pkeyToken;
            }
        }

        private void AccessionWizard_Load(object sender, EventArgs e)
        {
            this.Text = FormName;

            DataSet ds;
            // Get the accession table and bind it to the main form on the General tabpage...
            ds = _sharedUtils.GetWebServiceData("get_accession", _accessionPKeys, 0, 0);

            if (ds.Tables.Contains("get_accession"))
            {
                _accession = ds.Tables["get_accession"].Copy();

                BuildAccessionNamePage();
                BuildAccessionSourcePage();
                BuildAccessionAnnotationPage();
                BuildAccessionVoucherPage();
                BuildAccessionIPRPage();
                BuildAccessionQuarantinePage();
                BuildAccessionPedigreePage();
                BuildAccessionActionPage();

                // Bind the bindingsource to the binding navigator toolstrip...
                _accessionBindingSource.DataSource = _accession;
                ux_bindingnavigatorForm.BindingSource = _accessionBindingSource;
            }


            // Format the controls on this dialog...
            bindControls(this.Controls);
            formatControls(this.Controls);

            if (_accessionBindingSource.List.Count > 0)
            {
                ux_tabcontrolMain.Enabled = true;
            }
            else
            {
                ux_tabcontrolMain.Enabled = false;
            }
_sharedUtils.UpdateComponents(this.components.Components, this.Name);
_sharedUtils.UpdateControls(this.Controls, this.Name);

// Force the row filters to be applied...
_mainBindingSource_CurrentChanged(sender, e);
        }

        public string FormName
        {
            get
            {
                return "Accession Wizard";
            }
        }

        public DataTable ChangedRecords
        {
            get
            {
                DataTable dt = new DataTable();
                if (_changedRecords.Tables.Contains(_accession.TableName))
                {
                    dt = _changedRecords.Tables[_accession.TableName].Copy();
                }
                return dt;
            }
        }

        public string PKeyName
        {
            get
            {
                return "accession_id";
            }
        }

        private void AccessionWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // The user might be closing the form during the middle of edit changes in the datagridview - if so ask the
            // user if they would like to save their data...
            int intRowEdits = 0;

            _accessionBindingSource.EndEdit();
            if (_accession.GetChanges() != null) intRowEdits = _accession.GetChanges().Rows.Count;
            _accessionNameBindingSource.EndEdit();
            if (_accessionName.GetChanges() != null) intRowEdits += _accessionName.GetChanges().Rows.Count;
            _accessionSourceBindingSource.EndEdit();
            if (_accessionSource.GetChanges() != null) intRowEdits += _accessionSource.GetChanges().Rows.Count;
            _accessionAnnotationBindingSource.EndEdit();
            if (_accessionAnnotation.GetChanges() != null) intRowEdits += _accessionAnnotation.GetChanges().Rows.Count;
            _accessionVoucherBindingSource.EndEdit();
            if (_accessionVoucher.GetChanges() != null) intRowEdits += _accessionVoucher.GetChanges().Rows.Count;
            _accessionIPRBindingSource.EndEdit();
            if (_accessionIPR.GetChanges() != null) intRowEdits += _accessionIPR.GetChanges().Rows.Count;
            _accessionQuarantineBindingSource.EndEdit();
            if (_accessionQuarantine.GetChanges() != null) intRowEdits += _accessionQuarantine.GetChanges().Rows.Count;
            _accessionPedigreeBindingSource.EndEdit();
            if (_accessionPedigree.GetChanges() != null) intRowEdits += _accessionPedigree.GetChanges().Rows.Count;
            _accessionActionBindingSource.EndEdit();
            if (_accessionAction.GetChanges() != null) intRowEdits += _accessionAction.GetChanges().Rows.Count;

            GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s), are you sure you want to cancel your edits and close this window?", "Cancel Edits and Close", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "AccessionWizard_FormClosingMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
if (intRowEdits > 0 && DialogResult.No == ggMessageBox.ShowDialog())
            {
                e.Cancel = true;
            }
        }

        private void AccessionWizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            _accessionBindingSource.ListChanged -= _mainBindingSource_ListChanged;
            _accessionBindingSource.ListChanged -= _mainBindingSource_ListChanged;
            _accessionBindingSource.ListChanged -= _mainBindingSource_ListChanged;
        }

        #region Customizations to Dynamic Controls...
        private void ux_textboxPrefix_TextChanged(object sender, EventArgs e)
        {
            bindingNavigatorAccessionNumber.Text = ux_textboxPrefix.Text + " " + ux_textboxNumber.Text + " " + ux_textboxSuffix.Text;
            if (string.IsNullOrEmpty(ux_textboxPrefix.Text) &&
                _accession.Columns[ux_textboxPrefix.Tag.ToString()].ExtendedProperties.Contains("is_nullable") &&
                _accession.Columns[ux_textboxPrefix.Tag.ToString()].ExtendedProperties["is_nullable"].ToString() == "N" &&
                !_accession.Columns[ux_textboxPrefix.Tag.ToString()].ReadOnly)
            {
                ux_textboxPrefix.BackColor = Color.Plum;
            }
            else
            {
                ux_textboxPrefix.BackColor = Color.Empty;
            }
        }

        private void ux_textboxNumber_TextChanged(object sender, EventArgs e)
        {
            if (ux_textboxNumber.TextLength == 0)
            {
                ux_textboxNumber.Clear();
                ux_textboxNumber.ClearUndo();
                ((DataRowView)_accessionBindingSource.Current).Row[ux_textboxNumber.Tag.ToString().Trim().ToLower()] = DBNull.Value;
            }
            bindingNavigatorAccessionNumber.Text = ux_textboxPrefix.Text + " " + ux_textboxNumber.Text + " " + ux_textboxSuffix.Text;
            if (string.IsNullOrEmpty(ux_textboxNumber.Text) &&
                _accession.Columns[ux_textboxNumber.Tag.ToString()].ExtendedProperties.Contains("is_nullable") &&
                _accession.Columns[ux_textboxNumber.Tag.ToString()].ExtendedProperties["is_nullable"].ToString() == "N" &&
                !_accession.Columns[ux_textboxNumber.Tag.ToString()].ReadOnly)
            {
                ux_textboxNumber.BackColor = Color.Plum;
            }
            else
            {
                ux_textboxNumber.BackColor = Color.Empty;
            }
        }

        private void ux_textboxSuffix_TextChanged(object sender, EventArgs e)
        {
            bindingNavigatorAccessionNumber.Text = ux_textboxPrefix.Text + " " + ux_textboxNumber.Text + " " + ux_textboxSuffix.Text;
        }

        private void ux_textboxTaxonomy_TextChanged(object sender, EventArgs e)
        {
            bindingNavigatorTaxonomy.Text = ux_textboxTaxonomy.Text;
            if (string.IsNullOrEmpty(ux_textboxTaxonomy.Text) &&
                _accession.Columns[ux_textboxTaxonomy.Tag.ToString()].ExtendedProperties.Contains("is_nullable") &&
                _accession.Columns[ux_textboxTaxonomy.Tag.ToString()].ExtendedProperties["is_nullable"].ToString() == "N" &&
                !_accession.Columns[ux_textboxTaxonomy.Tag.ToString()].ReadOnly)
            {
                ux_textboxTaxonomy.BackColor = Color.Plum;
            }
            else
            {
                ux_textboxTaxonomy.BackColor = Color.Empty;
            }
        }

        private void ux_textboxReceivedDate_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ux_textboxReceivedDate.Text) &&
                _accession.Columns[ux_textboxReceivedDate.Tag.ToString()].ExtendedProperties.Contains("is_nullable") &&
                _accession.Columns[ux_textboxReceivedDate.Tag.ToString()].ExtendedProperties["is_nullable"].ToString() == "N" &&
                !_accession.Columns[ux_textboxReceivedDate.Tag.ToString()].ReadOnly)
            {
                ux_textboxReceivedDate.BackColor = Color.Plum;
            }
            else
            {
                ux_textboxReceivedDate.BackColor = Color.Empty;
            }
        }

        private void ux_textboxActiveSite_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ux_textboxActiveSite.Text) &&
                _accession.Columns[ux_textboxActiveSite.Tag.ToString()].ExtendedProperties.Contains("is_nullable") &&
                _accession.Columns[ux_textboxActiveSite.Tag.ToString()].ExtendedProperties["is_nullable"].ToString() == "N" &&
                !_accession.Columns[ux_textboxActiveSite.Tag.ToString()].ReadOnly)
            {
                ux_textboxActiveSite.BackColor = Color.Plum;
            }
            else
            {
                ux_textboxActiveSite.BackColor = Color.Empty;
            }
        }
        #endregion

        #region Dynamic Controls logic...
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
                    if (ctrl is ComboBox) bindComboBox((ComboBox)ctrl, _accessionBindingSource);
                    if (ctrl is TextBox) bindTextBox((TextBox)ctrl, _accessionBindingSource);
                    if (ctrl is CheckBox) bindCheckBox((CheckBox)ctrl, _accessionBindingSource);
                    if (ctrl is DateTimePicker) bindDateTimePicker((DateTimePicker)ctrl, _accessionBindingSource);
                    if (ctrl is Label) bindLabel((Label)ctrl, _accessionBindingSource);
                }
            }
        }

        private void formatControls(Control.ControlCollection controlCollection)
        {
            foreach (Control ctrl in controlCollection)
            {
                if (ctrl != ux_bindingnavigatorForm)  // Leave the bindingnavigator alone
                {
                    // If the ctrl has children - set their edit mode too...
                    if (ctrl.Controls.Count > 0)
                    {
                        formatControls(ctrl.Controls);
                    }
                    // Set the edit mode for the control...
                    if (ctrl != null &&
                        ctrl.Tag != null &&
                        ctrl.Tag is string &&
                        _accessionBindingSource != null &&
                        _accessionBindingSource.DataSource is DataTable &&
                        ((DataTable)_accessionBindingSource.DataSource).Columns.Contains(ctrl.Tag.ToString().Trim().ToLower()))
                    {
                        if (ctrl is TextBox)
                        {
                            // TextBoxes have a ReadOnly property in addition to an Enabled property so we handle this one separate...
                            ((TextBox)ctrl).ReadOnly = ((DataTable)_accessionBindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
                        }
                        else if (ctrl is Label)
                        {
                            // Do nothing to the Label
                        }
                        else
                        {
                            // All other control types (ComboBox, CheckBox, DateTimePicker) except Labels...
                            ctrl.Enabled = !((DataTable)_accessionBindingSource.DataSource).Columns[ctrl.Tag.ToString().Trim().ToLower()].ReadOnly;
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
                        if (DateTime.TryParse(drv[dateColumnName].ToString(), null, System.Globalization.DateTimeStyles.AssumeLocal, out dt))
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
            if (dv != null && 
                e.ColumnIndex > -1 &&
                e.RowIndex > -1 &&
                e.RowIndex < dv.Count)
            {
                DataColumn dc = dv.Table.Columns[e.ColumnIndex];
                if (_sharedUtils.LookupTablesIsValidFKField(dc) &&
                    e.RowIndex < dv.Count &&
                    dv[e.RowIndex].Row.RowState != DataRowState.Deleted)
                {
                    if (dv[e.RowIndex][e.ColumnIndex] != DBNull.Value)
                    {
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
        private void bindingNavigatorSaveButton_Click(object sender, EventArgs e)
        {
            int errorCount = 0;
            errorCount = SaveAccessionData();
            if (errorCount == 0)
            {
//MessageBox.Show(this, "All data was saved successfully", "Accession Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Accession Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "AccessionWizard_bindingNavigatorSaveButtonMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
            }
            else
            {
//MessageBox.Show(this, "The data being saved has errors that should be reviewed.\n\n  Error Count: " + errorCount, "Accession Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\n  Error Count: {0}", "Accession Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "AccessionWizard_bindingNavigatorSaveButtonMessage2";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
ggMessageBox.ShowDialog();
            }
//RefreshAccessionData();
            //// Update the row error message for this accession row...
            //ux_labelAccessionRowError.Text = ((DataRowView)_accessionBindingSource.Current).Row.RowError;
            // Update the Accession Form (and child DataGridViews) data...
            _mainBindingSource_CurrentChanged(sender, e);
        }

        private void bindingNavigatorSaveAndExitButton_Click(object sender, EventArgs e)
        {
            int errorCount = 0;
            errorCount = SaveAccessionData();
            if (errorCount == 0)
            {
//MessageBox.Show(this, "All data was saved successfully", "Accession Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("All data was saved successfully", "Accession Wizard Data Save Results", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "AccessionWizard_bindingNavigatorSaveButtonMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
ggMessageBox.ShowDialog();
                this.Close();
            }
            else
            {
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Accession Wizard).\n\n  Error Count: {0}", "Accession Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "AccessionWizard_bindingNavigatorSaveButtonMessage3";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorCount);
//if (DialogResult.No == MessageBox.Show(this, "The data being saved has errors that should be reviewed.\n\nWould you like to review them now?\n\nClick Yes to review the errors now.\n(Click No to abandon the errors and exit the Accession Wizard).\n\n  Error Count: " + errorCount, "Accession Wizard Data Save Results", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
if (DialogResult.No == ggMessageBox.ShowDialog())
                {
                    this.Close();
                }
                else
                {
                    // Update the row error message for this accession row...
                    if (string.IsNullOrEmpty(((DataRowView)_accessionBindingSource.Current).Row.RowError))
                    {
                        ux_textboxAccessionRowError.Visible = false;
                        ux_textboxAccessionRowError.Text = "";
                    }
                    else
                    {
                        ux_textboxAccessionRowError.Visible = true;
                        ux_textboxAccessionRowError.ReadOnly = false;
                        ux_textboxAccessionRowError.Enabled = true;
                        ux_textboxAccessionRowError.Text = ((DataRowView)_accessionBindingSource.Current).Row.RowError;
                    }
                }
            }
        }

        //private void RefreshAccessionData()
        //{
        //    // Refresh the data...
        //    DataSet ds = _sharedUtils.GetWebServiceData("get_accession", _pKeys, 0, 0);
        //    if (ds.Tables.Contains("get_accession"))
        //    {
        //        _accession = ds.Tables["get_accession"].Copy();
        //        _accessionBindingSource.DataSource = _accession;
        //    }
        //}

        private int SaveAccessionData()
        {
            int errorCount = 0;
            DataSet accessionChanges = new DataSet();
            DataSet accessionSaveResults = new DataSet();
            DataSet accessionNameChanges = new DataSet();
            DataSet accessionNameSaveResults = new DataSet();
            DataSet accessionSourceChanges = new DataSet();
            DataSet accessionSourceSaveResults = new DataSet();
            DataSet accessionSourceCooperatorChanges = new DataSet();
            DataSet accessionSourceCooperatorSaveResults = new DataSet();
            DataSet accessionSourceDescObservationChanges = new DataSet();
            DataSet accessionSourceDescObservationSaveResults = new DataSet();
            DataSet accessionAnnotationChanges = new DataSet();
            DataSet accessionAnnotationSaveResults = new DataSet();
            DataSet accessionVoucherChanges = new DataSet();
            DataSet accessionVoucherSaveResults = new DataSet();
            DataSet accessionIPRChanges = new DataSet();
            DataSet accessionIPRSaveResults = new DataSet();
            DataSet accessionQuarantineChanges = new DataSet();
            DataSet accessionQuarantineSaveResults = new DataSet();
            DataSet accessionPedigreeChanges = new DataSet();
            DataSet accessionPedigreeSaveResults = new DataSet();
            DataSet accessionActionChanges = new DataSet();
            DataSet accessionActionSaveResults = new DataSet();

            // Process ACCESSIONS...
            // Make sure the last edited row in the Accessions Form has been commited to the datatable...
            _accessionBindingSource.EndEdit();

            // Make sure the navigator is not currently editing a cell...
            foreach (DataRowView drv in _accessionBindingSource.List)
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

            // Get the changes (if any) for the accession table and commit them to the remote database...
            if (_accession.GetChanges() != null)
            {
                accessionChanges.Tables.Add(_accession.GetChanges());
ScrubData(accessionChanges);
                // Save the changes to the remote server...
                accessionSaveResults = _sharedUtils.SaveWebServiceData(accessionChanges);
                if (accessionSaveResults.Tables.Contains(_accession.TableName))
                {
                    errorCount += SyncSavedResults(_accession, accessionSaveResults.Tables[_accession.TableName]);
                }
            }

            // Process ACCESSION_NAME...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionNameBindingSource.EndEdit();

            // Get the changes (if any) for the accession_name table and commit them to the remote database...
            if (_accessionName.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_name have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_name...
                DataRow[] accessionNameRowsWithNewParent = _accessionName.Select("accession_id<0");
                foreach (DataRow dr in accessionNameRowsWithNewParent)
                {
                    // "OriginalPrimaryKeyID" "NewPrimaryKeyID"
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
bool origRO = dr.Table.Columns["accession_id"].ReadOnly;
dr.Table.Columns["accession_id"].ReadOnly = false;
dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
dr.Table.Columns["accession_id"].ReadOnly = origRO;
dr["inventory_id"] = FindSystemGeneratedInvPKey((int)newParent[0]["NewPrimaryKeyID"]);
                    }
                }

                accessionNameChanges.Tables.Add(_accessionName.GetChanges());
ScrubData(accessionNameChanges);
                // Now save the changes to the remote server...
                accessionNameSaveResults = _sharedUtils.SaveWebServiceData(accessionNameChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionNameSaveResults.Tables.Contains(_accessionName.TableName))
                {
                    errorCount += SyncSavedResults(_accessionName, accessionNameSaveResults.Tables[_accessionName.TableName]);
                }
            }

            // Process ACCESSION_SOURCE...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionSourceBindingSource.EndEdit();

            // Get the changes (if any) for the accession_source table and commit them to the remote database...
            if (_accessionSource.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_name have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_source...
                DataRow[] accessionSourceRowsWithNewParent = _accessionSource.Select("accession_id<0");
                foreach (DataRow dr in accessionSourceRowsWithNewParent)
                {
                    // .Rows.Find(dr["OriginalPrimaryKeyID"])  dr["NewPrimaryKeyID"]
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }
                accessionSourceChanges.Tables.Add(_accessionSource.GetChanges());
ScrubData(accessionSourceChanges);
                // Now save the changes to the remote server...
                accessionSourceSaveResults = _sharedUtils.SaveWebServiceData(accessionSourceChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionSourceSaveResults.Tables.Contains(_accessionSource.TableName))
                {
                    errorCount += SyncSavedResults(_accessionSource, accessionSourceSaveResults.Tables[_accessionSource.TableName]);
                }
            }

            // Process ACCESSION_SOURCE_COOPERATOR...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionSourceCooperatorBindingSource.EndEdit();

            // Get the changes (if any) for the accession_source table and commit them to the remote database...
            if (_accessionSourceCooperator.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_name have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_source...
                DataRow[] accessionSourceCooperatorRowsWithNewParent = _accessionSourceCooperator.Select("accession_source_id<0");
                foreach (DataRow dr in accessionSourceCooperatorRowsWithNewParent)
                {
                    // .Rows.Find(dr["OriginalPrimaryKeyID"])  dr["NewPrimaryKeyID"]
                    DataRow[] newParent = accessionSourceSaveResults.Tables["get_accession_source"].Select("OriginalPrimaryKeyID=" + dr["accession_source_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["accession_source_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }
                accessionSourceCooperatorChanges.Tables.Add(_accessionSourceCooperator.GetChanges());
                ScrubData(accessionSourceCooperatorChanges);
                // Now save the changes to the remote server...
                accessionSourceCooperatorSaveResults = _sharedUtils.SaveWebServiceData(accessionSourceCooperatorChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionSourceCooperatorSaveResults.Tables.Contains(_accessionSourceCooperator.TableName))
                {
                    errorCount += SyncSavedResults(_accessionSourceCooperator, accessionSourceCooperatorSaveResults.Tables[_accessionSourceCooperator.TableName]);
                }
            }

            // Process ACCESSION_SOURCE_DESC_OBSERVATION...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionSourceDescObservationBindingSource.EndEdit();

            // Get the changes (if any) for the accession_source table and commit them to the remote database...
            if (_accessionSourceDescObservation.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_name have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_source...
                DataRow[] accessionSourceDescObservationRowsWithNewParent = _accessionSourceDescObservation.Select("accession_source_id<0");
                foreach (DataRow dr in accessionSourceDescObservationRowsWithNewParent)
                {
                    // .Rows.Find(dr["OriginalPrimaryKeyID"])  dr["NewPrimaryKeyID"]
                    DataRow[] newParent = accessionSourceSaveResults.Tables["get_accession_source"].Select("OriginalPrimaryKeyID=" + dr["accession_source_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["accession_source_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }
                accessionSourceDescObservationChanges.Tables.Add(_accessionSourceDescObservation.GetChanges());
                ScrubData(accessionSourceDescObservationChanges);
                // Now save the changes to the remote server...
                accessionSourceDescObservationSaveResults = _sharedUtils.SaveWebServiceData(accessionSourceDescObservationChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionSourceDescObservationSaveResults.Tables.Contains(_accessionSourceDescObservation.TableName))
                {
                    errorCount += SyncSavedResults(_accessionSourceDescObservation, accessionSourceDescObservationSaveResults.Tables[_accessionSourceDescObservation.TableName]);
                }
            }            
            
            // Process ACCESSION_ANNOTATION...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionAnnotationBindingSource.EndEdit();

            // Get the changes (if any) for the accession_annotation table and commit them to the remote database...
            if (_accessionAnnotation.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_annotation have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_annotation...
                DataRow[] accessionAnnotationRowsWithNewParent = _accessionAnnotation.Select("accession_id<0");
                foreach (DataRow dr in accessionAnnotationRowsWithNewParent)
                {
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
bool origRO = dr.Table.Columns["accession_id"].ReadOnly;
dr.Table.Columns["accession_id"].ReadOnly = false;
dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
dr.Table.Columns["accession_id"].ReadOnly = origRO;
dr["inventory_id"] = FindSystemGeneratedInvPKey((int)newParent[0]["NewPrimaryKeyID"]);
//DataSet newInventory = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + newParent[0]["NewPrimaryKeyID"].ToString() + ";", 0, 0);
//if (newInventory.Tables.Contains("get_inventory") &&
//    newInventory.Tables["get_inventory"].Rows.Count > 0)
//{
//    newInventory.Tables["get_inventory"].DefaultView.Sort = "inventory_id asc";
//    dr["inventory_id"] = newInventory.Tables["get_inventory"].DefaultView[0]["inventory_id"];
//}
                    }
                }
                accessionAnnotationChanges.Tables.Add(_accessionAnnotation.GetChanges());
ScrubData(accessionAnnotationChanges);
                // Save the changes to the remote server...
                accessionAnnotationSaveResults = _sharedUtils.SaveWebServiceData(accessionAnnotationChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionAnnotationSaveResults.Tables.Contains(_accessionAnnotation.TableName))
                {
                    errorCount += SyncSavedResults(_accessionAnnotation, accessionAnnotationSaveResults.Tables[_accessionAnnotation.TableName]);
                }
            }

            // Process ACCESSION_VOUCHER...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionVoucherBindingSource.EndEdit();

            // Get the changes (if any) for the accession_voucher table and commit them to the remote database...
            if (_accessionVoucher.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_voucher have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_voucher...
                DataRow[] accessionVoucherRowsWithNewParent = _accessionVoucher.Select("accession_id<0");
                foreach (DataRow dr in accessionVoucherRowsWithNewParent)
                {
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
bool origRO = dr.Table.Columns["accession_id"].ReadOnly;
dr.Table.Columns["accession_id"].ReadOnly = false;
dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
dr.Table.Columns["accession_id"].ReadOnly = origRO;
dr["inventory_id"] = FindSystemGeneratedInvPKey((int)newParent[0]["NewPrimaryKeyID"]);
//DataSet newInventory = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + newParent[0]["NewPrimaryKeyID"].ToString() + ";", 0, 0);
//if (newInventory.Tables.Contains("get_inventory") &&
//    newInventory.Tables["get_inventory"].Rows.Count > 0)
//{
//    newInventory.Tables["get_inventory"].DefaultView.Sort = "inventory_id asc";
//    dr["inventory_id"] = newInventory.Tables["get_inventory"].DefaultView[0]["inventory_id"];
//}
                    }
                }
                accessionVoucherChanges.Tables.Add(_accessionVoucher.GetChanges());
ScrubData(accessionVoucherChanges);
                // Save the changes to the remote server...
                accessionVoucherSaveResults = _sharedUtils.SaveWebServiceData(accessionVoucherChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionVoucherSaveResults.Tables.Contains(_accessionVoucher.TableName))
                {
                    errorCount += SyncSavedResults(_accessionVoucher, accessionVoucherSaveResults.Tables[_accessionVoucher.TableName]);
                }
            }

            // Process ACCESSION_IPR...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionIPRBindingSource.EndEdit();

            // Get the changes (if any) for the accession_IPR table and commit them to the remote database...
            if (_accessionIPR.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_IPR have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_IPR...
                DataRow[] accessionIPRRowsWithNewParent = _accessionIPR.Select("accession_id<0");
                foreach (DataRow dr in accessionIPRRowsWithNewParent)
                {
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }
                accessionIPRChanges.Tables.Add(_accessionIPR.GetChanges());
ScrubData(accessionIPRChanges);
                // Save the changes to the remote server...
                accessionIPRSaveResults = _sharedUtils.SaveWebServiceData(accessionIPRChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionIPRSaveResults.Tables.Contains(_accessionIPR.TableName))
                {
                    errorCount += SyncSavedResults(_accessionIPR, accessionIPRSaveResults.Tables[_accessionIPR.TableName]);
                }
            }

            // Process ACCESSION_QUARANTINE...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionQuarantineBindingSource.EndEdit();

            // Get the changes (if any) for the accession_quarantine table and commit them to the remote database...
            if (_accessionQuarantine.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_quarantine have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_Quarantine...
                DataRow[] accessionQuarantineRowsWithNewParent = _accessionQuarantine.Select("accession_id<0");
                foreach (DataRow dr in accessionQuarantineRowsWithNewParent)
                {
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }
                accessionQuarantineChanges.Tables.Add(_accessionQuarantine.GetChanges());
ScrubData(accessionQuarantineChanges);
                // Save the changes to the remote server...
                accessionQuarantineSaveResults = _sharedUtils.SaveWebServiceData(accessionQuarantineChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionQuarantineSaveResults.Tables.Contains(_accessionQuarantine.TableName))
                {
                    errorCount += SyncSavedResults(_accessionQuarantine, accessionQuarantineSaveResults.Tables[_accessionQuarantine.TableName]);
                }
            }

            // Process ACCESSION_PEDIGREE...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionPedigreeBindingSource.EndEdit();

            // Get the changes (if any) for the accession_pedigree table and commit them to the remote database...
            if (_accessionPedigree.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_pedigree have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_pedigree...
                DataRow[] accessionPedigreeRowsWithNewParent = _accessionPedigree.Select("accession_id<0");
                foreach (DataRow dr in accessionPedigreeRowsWithNewParent)
                {
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }
                accessionPedigreeChanges.Tables.Add(_accessionPedigree.GetChanges());
ScrubData(accessionPedigreeChanges);
                // Save the changes to the remote server...
                accessionPedigreeSaveResults = _sharedUtils.SaveWebServiceData(accessionPedigreeChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionPedigreeSaveResults.Tables.Contains(_accessionPedigree.TableName))
                {
                    errorCount += SyncSavedResults(_accessionPedigree, accessionPedigreeSaveResults.Tables[_accessionPedigree.TableName]);
                }
            }

            // Process ACCESSION_ACTION...
            // Make sure the last edited row in the DGV has been commited to the datatable...
            _accessionActionBindingSource.EndEdit();

            // Get the changes (if any) for the accession_action table and commit them to the remote database...
            if (_accessionAction.GetChanges() != null)
            {
                // Before saving the results to the remote server check to see if any new rows in accession_action have
                // a FK related to a new row in the accessions table (pkey < 0).  If so get the new pkey returned from
                // the get_accession save and update the records in accession_action...
                DataRow[] accessionActionRowsWithNewParent = _accessionAction.Select("accession_id<0");
                foreach (DataRow dr in accessionActionRowsWithNewParent)
                {
                    DataRow[] newParent = accessionSaveResults.Tables["get_accession"].Select("OriginalPrimaryKeyID=" + dr["accession_id"].ToString());
                    if (newParent != null && newParent.Length > 0)
                    {
                        dr["accession_id"] = newParent[0]["NewPrimaryKeyID"];
                    }
                }
                accessionActionChanges.Tables.Add(_accessionAction.GetChanges());
ScrubData(accessionActionChanges);
                // Save the changes to the remote server...
                accessionActionSaveResults = _sharedUtils.SaveWebServiceData(accessionActionChanges);
                // Sync the saved results with the original table (to give user feedback about results of save)...
                if (accessionActionSaveResults.Tables.Contains(_accessionAction.TableName))
                {
                    errorCount += SyncSavedResults(_accessionAction, accessionActionSaveResults.Tables[_accessionAction.TableName]);
                }
            }

            // Now add the new changes to the _changedRecords dataset (this data will be passed back to the calling program)...
            if (accessionSaveResults != null && accessionSaveResults.Tables.Contains(_accession.TableName))
            {
                string pkeyName = accessionSaveResults.Tables[_accession.TableName].PrimaryKey[0].ColumnName;
                bool origColumnReadOnlyValue = accessionSaveResults.Tables[_accession.TableName].Columns[pkeyName].ReadOnly;
                foreach (DataRow dr in accessionSaveResults.Tables[_accession.TableName].Rows)
                {
                    if (dr["SavedAction"].ToString().ToUpper() == "INSERT" &&
                        dr["SavedStatus"].ToString().ToUpper() == "SUCCESS")
                    {
                        dr.Table.Columns[pkeyName].ReadOnly = false;
                        dr[pkeyName] = dr["NewPrimaryKeyID"];
                        dr.AcceptChanges();
                    }
                }
                accessionSaveResults.Tables[_accession.TableName].Columns[pkeyName].ReadOnly = origColumnReadOnlyValue;

                if (_changedRecords.Tables.Contains(_accession.TableName))
                {
                    // If the saved results table exists - update or insert the new records...
                    _changedRecords.Tables[_accession.TableName].Load(accessionSaveResults.Tables[_accession.TableName].CreateDataReader(), LoadOption.Upsert);
                    _changedRecords.Tables[_accession.TableName].AcceptChanges();

                }
                else
                {
                    // If the saved results table doesn't exist - create it (and include the new records)...
                    _changedRecords.Tables.Add(accessionSaveResults.Tables[_accession.TableName].Copy());
                    _changedRecords.AcceptChanges();
                }
            }

            return errorCount;
        }

        private void ScrubData(DataSet ds)
        {
            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ExtendedProperties.Contains("is_nullable") &&
                            dc.ExtendedProperties["is_nullable"].ToString().Trim().ToUpper() == "N" &&
                            dr.RowState != DataRowState.Deleted &&
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
                                    bool origColumnReadOnlyValue = originalRow.Table.Columns[pKeyCol].ReadOnly;
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = false;
                                    originalRow[pKeyCol] = dr["NewPrimaryKeyID"];
                                    originalRow.AcceptChanges();
                                    originalRow.Table.Columns[pKeyCol].ReadOnly = origColumnReadOnlyValue;
                                    originalRow.ClearErrors();
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
                                // To do this you must first find the deleted row (NOTE: datatable.rows.find() method does not work on deleted rows)...
                                foreach (DataRow deletedRow in originalTable.Rows)
                                {
                                    if (deletedRow[0, DataRowVersion.Original].Equals(dr["OriginalPrimaryKeyID"]))
                                    {
                                        originalRow = deletedRow;
                                    }
                                }
                                if (originalRow != null)
                                {
                                    originalRow.AcceptChanges();
                                    originalRow.ClearErrors();
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

        private int FindSystemGeneratedInvPKey(int accessionPKey)
        {
            int invPKey = -1;
            DataSet newInventory = _sharedUtils.GetWebServiceData("get_inventory", ":accessionid=" + accessionPKey + ";", 0, 0);
            if (newInventory.Tables.Contains("get_inventory") &&
                newInventory.Tables["get_inventory"].Rows.Count > 0)
            {
                newInventory.Tables["get_inventory"].DefaultView.Sort = "form_type_code asc";
                invPKey = (int)newInventory.Tables["get_inventory"].DefaultView[0]["inventory_id"];
            }
            return invPKey;
        }

        void _mainBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            formatControls(this.Controls);
        }

        void _mainBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (_accessionBindingSource.List.Count > 0)
            {
                ux_tabcontrolMain.Enabled = true;
                // Update the row error message for this accession row...
                if (string.IsNullOrEmpty(((DataRowView)_accessionBindingSource.Current).Row.RowError))
                {
                    ux_textboxAccessionRowError.Visible = false;
                    ux_textboxAccessionRowError.Text = "";
                }
                else
                {
                    ux_textboxAccessionRowError.Visible = true;
                    ux_textboxAccessionRowError.ReadOnly = false;
                    ux_textboxAccessionRowError.Enabled = true;
                    ux_textboxAccessionRowError.Text = ((DataRowView)_accessionBindingSource.Current).Row.RowError;
                }
                // Update the Accession Number on the Navigator Bar...
                bindingNavigatorAccessionNumber.Text = ((DataRowView)_accessionBindingSource.Current).Row["accession_number_part1"].ToString() + " " +
                                        ((DataRowView)_accessionBindingSource.Current).Row["accession_number_part2"].ToString() + " " +
                                        ((DataRowView)_accessionBindingSource.Current).Row["accession_number_part3"].ToString();
                // Change the row filter for the child tables to reflect the new Accession's primary key...
                string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
                if (_accessionName != null && !string.IsNullOrEmpty(pkey) && _accessionName.Columns.Contains("accession_id")) _accessionName.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                if (_accessionSource != null && !string.IsNullOrEmpty(pkey) && _accessionSource.Columns.Contains("accession_id")) _accessionSource.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                if (_accessionAnnotation != null && !string.IsNullOrEmpty(pkey) && _accessionAnnotation.Columns.Contains("accession_id")) _accessionAnnotation.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                if (_accessionVoucher != null && !string.IsNullOrEmpty(pkey) && _accessionVoucher.Columns.Contains("accession_id")) _accessionVoucher.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                if (_accessionIPR != null && !string.IsNullOrEmpty(pkey) && _accessionIPR.Columns.Contains("accession_id")) _accessionIPR.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                if (_accessionQuarantine != null && !string.IsNullOrEmpty(pkey) && _accessionQuarantine.Columns.Contains("accession_id")) _accessionQuarantine.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                if (_accessionPedigree != null && !string.IsNullOrEmpty(pkey) && _accessionPedigree.Columns.Contains("accession_id")) _accessionPedigree.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                if (_accessionAction != null && !string.IsNullOrEmpty(pkey) && _accessionAction.Columns.Contains("accession_id")) _accessionAction.DefaultView.RowFilter = "accession_id=" + pkey.Trim().ToLower();
                // Update the Taxonomy on the Navigator Bar...
                bindingNavigatorTaxonomy.Text = ux_textboxTaxonomy.Text;
                // Update the Origin on the Navigator Bar...
                if (_accessionSource.DefaultView.Count > 0)
                {
                    string currentSourceSort = _accessionSource.DefaultView.Sort;
                    _accessionSource.DefaultView.Sort = "accession_source_id asc";

                    if (_accessionSource.Columns.Contains("geography_id") &&
                        _sharedUtils.LookupTablesIsValidFKField(_accessionSource.Columns["geography_id"]))
                    {
                        bindingNavigatorOrigin.Text = _sharedUtils.GetLookupDisplayMember(_accessionSource.Columns["geography_id"].ExtendedProperties["foreign_key_dataview_name"].ToString(),
                                                                                          _accessionSource.DefaultView[0]["geography_id"].ToString(), 
                                                                                          "", 
                                                                                          _accessionSource.Columns["geography_id"].ExtendedProperties["foreign_key_dataview_name"].ToString());
                    }
                    _accessionSource.DefaultView.Sort = currentSourceSort;
                }
                else
                {
                    bindingNavigatorOrigin.Text = "";
                }
                // Update the Top Name on the Navigator Bar...
                if (_accessionName.DefaultView.Count > 0)
                {
                    string currentAccessionNameSort = _accessionName.DefaultView.Sort;
                    _accessionName.DefaultView.Sort = "plant_name_rank asc";
                    bindingNavigatorTopName.Text = _accessionName.DefaultView[0]["plant_name"].ToString();
                    _accessionName.DefaultView.Sort = currentAccessionNameSort;
                }
                else
                {
                    bindingNavigatorTopName.Text = "";
                }
            }
            else
            {
                ux_tabcontrolMain.Enabled = false;
            }
        }
        #endregion

        #region Tab Control Logic...
        private void ux_tabcontrolMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ux_tabcontrolMain.SelectedIndex == 0)
            {
                bindingNavigatorAddNewItem.Enabled = true;
                bindingNavigatorDeleteItem.Enabled = true;
            }
            else
            {
                bindingNavigatorAddNewItem.Enabled = false;
                bindingNavigatorDeleteItem.Enabled = false;
                foreach (DataRowView drv in _accessionBindingSource.List)
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

        #region Accession Name logic...
        private void BuildAccessionNamePage()
        {
            DataSet ds;
            // Get the accession_name table and bind it to the DGV on the Names tabpage...
            ds = _sharedUtils.GetWebServiceData("get_accession_inv_name", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_inv_name"))
            {
                // Copy the accession_name table to a private variable...
                _accessionName = ds.Tables["get_accession_inv_name"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionNames.DataSource = _accessionNameBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionNames, _accessionName);

                // Order and display the columns the way the user wants...
                int i = 0;
//if (ux_datagridviewAccessionNames.Columns.Contains("INVENTORY_ID")) ux_datagridviewAccessionNames.Columns["INVENTORY_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionNames.Columns.Contains("PLANT_NAME")) ux_datagridviewAccessionNames.Columns["PLANT_NAME"].DisplayIndex = i++;
                if (ux_datagridviewAccessionNames.Columns.Contains("CATEGORY_CODE")) ux_datagridviewAccessionNames.Columns["CATEGORY_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionNames.Columns.Contains("PLANT_NAME_RANK")) ux_datagridviewAccessionNames.Columns["PLANT_NAME_RANK"].DisplayIndex = i++;
                if (ux_datagridviewAccessionNames.Columns.Contains("NAME_GROUP_ID")) ux_datagridviewAccessionNames.Columns["NAME_GROUP_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionNames.Columns.Contains("NAME_SOURCE_COOPERATOR_ID")) ux_datagridviewAccessionNames.Columns["NAME_SOURCE_COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionNames.Columns.Contains("NOTE")) ux_datagridviewAccessionNames.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionNames.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
            else
            {
                _accessionName = new DataTable();
            }
        }

        private void ux_buttonNewAccessionNameRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionName = _accessionName.NewRow();
            newAccessionName["accession_id"] = pkey;
            // For existing accession records - find the system-generated inventory pkey...
            int iPKey = -1;
            if (int.TryParse(pkey, out iPKey) &&
                iPKey > 0)
            {
                newAccessionName["inventory_id"] = FindSystemGeneratedInvPKey(iPKey);
            }
            _accessionName.Rows.Add(newAccessionName);
            int newRowIndex = ux_datagridviewAccessionNames.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionNames.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionNames.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionNames["accession_inv_name_id", i].Value.Equals(newAccessionName["accession_inv_name_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionNames.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionNames.CurrentCell = ux_datagridviewAccessionNames[newColIndex, newRowIndex];
        }
        #endregion

        #region Accession Source logic...
        private void BuildAccessionSourcePage()
        {
            DataSet ds;
            // Get the accession_source table...
            ds = _sharedUtils.GetWebServiceData("get_accession_source", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_source"))
            {
                // Copy the accession_source table to a private variable...
                _accessionSource = ds.Tables["get_accession_source"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionSource.DataSource = _accessionSourceBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionSource, _accessionSource);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewAccessionSource.Columns.Contains("SOURCE_TYPE_CODE")) ux_datagridviewAccessionSource.Columns["SOURCE_TYPE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("SOURCE_DATE")) ux_datagridviewAccessionSource.Columns["SOURCE_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("SOURCE_DATE_CODE")) ux_datagridviewAccessionSource.Columns["SOURCE_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("IS_ORIGIN")) ux_datagridviewAccessionSource.Columns["IS_ORIGIN"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("GEOGRAPHY_ID")) ux_datagridviewAccessionSource.Columns["GEOGRAPHY_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("NOTE")) ux_datagridviewAccessionSource.Columns["NOTE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("ELEVATION_METERS")) ux_datagridviewAccessionSource.Columns["ELEVATION_METERS"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("COLLECTOR_VERBATIM_LOCALITY")) ux_datagridviewAccessionSource.Columns["COLLECTOR_VERBATIM_LOCALITY"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("ACQUISITION_SOURCE_CODE")) ux_datagridviewAccessionSource.Columns["ACQUISITION_SOURCE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("QUANTITY_COLLECTED")) ux_datagridviewAccessionSource.Columns["QUANTITY_COLLECTED"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("UNIT_QUANTITY_COLLECTED_CODE")) ux_datagridviewAccessionSource.Columns["UNIT_QUANTITY_COLLECTED_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("COLLECTED_FORM_CODE")) ux_datagridviewAccessionSource.Columns["COLLECTED_FORM_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("NUMBER_PLANTS_SAMPLED")) ux_datagridviewAccessionSource.Columns["NUMBER_PLANTS_SAMPLED"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("ENVIRONMENT_DESCRIPTION")) ux_datagridviewAccessionSource.Columns["ENVIRONMENT_DESCRIPTION"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("LATITUDE")) ux_datagridviewAccessionSource.Columns["LATITUDE"].DisplayIndex = i++; ;
                if (ux_datagridviewAccessionSource.Columns.Contains("LONGITUDE")) ux_datagridviewAccessionSource.Columns["LONGITUDE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("UNCERTAINTY")) ux_datagridviewAccessionSource.Columns["UNCERTAINTY"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("FORMATTED_LOCALITY")) ux_datagridviewAccessionSource.Columns["FORMATTED_LOCALITY"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("GEOREFERENCE_DATUM")) ux_datagridviewAccessionSource.Columns["GEOREFERENCE_DATUM"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("GEOREFERENCE_PROTOCOL_CODE")) ux_datagridviewAccessionSource.Columns["GEOREFERENCE_PROTOCOL_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSource.Columns.Contains("GEOREFERENCE_ANNOTATION")) ux_datagridviewAccessionSource.Columns["GEOREFERENCE_ANNOTATION"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionSource.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if(dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }

            }
            else
            {
                _accessionSource = new DataTable();
            }

            // Get the accession_source_map table (cooperators)...
            ds = _sharedUtils.GetWebServiceData("get_accession_source_cooperator", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_source_cooperator"))
            {
                // Copy the accession_source table to a private variable...
                _accessionSourceCooperator = ds.Tables["get_accession_source_cooperator"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionSourceCooperator.DataSource = _accessionSourceCooperatorBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionSourceCooperator, _accessionSourceCooperator);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewAccessionSourceCooperator.Columns.Contains("COOPERATOR_ID")) ux_datagridviewAccessionSourceCooperator.Columns["COOPERATOR_ID"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionSourceCooperator.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
                // Make the cooperator column expand to fit the largest displayed text...
                if (ux_datagridviewAccessionSourceCooperator.Columns.Contains("COOPERATOR_ID"))
                {
                    ux_datagridviewAccessionSourceCooperator.Columns["COOPERATOR_ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
            }
            else
            {
                _accessionSourceCooperator = new DataTable();
            }

            // Get the source_desc_observation table...
            ds = _sharedUtils.GetWebServiceData("get_source_desc_observation", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_source_desc_observation"))
            {
                // Copy the accession_source table to a private variable...
                _accessionSourceDescObservation = ds.Tables["get_source_desc_observation"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionSourceDescObservation.DataSource = _accessionSourceDescObservationBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionSourceDescObservation, _accessionSourceDescObservation);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewAccessionSourceDescObservation.Columns.Contains("SOURCE_DESCRIPTOR_ID")) ux_datagridviewAccessionSourceDescObservation.Columns["SOURCE_DESCRIPTOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSourceDescObservation.Columns.Contains("SOURCE_DESCRIPTOR_CODE_ID")) ux_datagridviewAccessionSourceDescObservation.Columns["SOURCE_DESCRIPTOR_CODE_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSourceDescObservation.Columns.Contains("NUMERIC_VALUE")) ux_datagridviewAccessionSourceDescObservation.Columns["NUMERIC_VALUE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSourceDescObservation.Columns.Contains("STRING_VALUE")) ux_datagridviewAccessionSourceDescObservation.Columns["STRING_VALUE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSourceDescObservation.Columns.Contains("ORIGINAL_VALUE")) ux_datagridviewAccessionSourceDescObservation.Columns["ORIGINAL_VALUE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionSourceDescObservation.Columns.Contains("NOTE")) ux_datagridviewAccessionSourceDescObservation.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionSourceDescObservation.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }

            }
            else
            {
                _accessionSourceDescObservation = new DataTable();
            }
        }

        private void ux_buttonNewAccessionSourceRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionSource = _accessionSource.NewRow();
            newAccessionSource["accession_id"] = pkey;
            _accessionSource.Rows.Add(newAccessionSource);
            int newRowIndex = ux_datagridviewAccessionSource.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionSource.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionSource.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionSource["accession_source_id", i].Value.Equals(newAccessionSource["accession_source_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionSource.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionSource.CurrentCell = ux_datagridviewAccessionSource[newColIndex, newRowIndex];
        }

        private void ux_buttonNewAccessionSourceCooperatorRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionSourceBindingSource.Current)[_accessionSource.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionSourceCooperator = _accessionSourceCooperator.NewRow();
            newAccessionSourceCooperator["accession_source_id"] = pkey;
            _accessionSourceCooperator.Rows.Add(newAccessionSourceCooperator);
            int newRowIndex = ux_datagridviewAccessionSourceCooperator.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionSourceCooperator.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionSourceCooperator.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionSourceCooperator["accession_source_map_id", i].Value.Equals(newAccessionSourceCooperator["accession_source_map_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionSourceCooperator.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionSourceCooperator.CurrentCell = ux_datagridviewAccessionSourceCooperator[newColIndex, newRowIndex];
            ux_datagridviewAccessionSourceCooperator.Focus();
        }

        private void ux_buttonNewAccessionSourceDescriptorRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionSourceBindingSource.Current)[_accessionSource.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionSourceDescriptor = _accessionSourceDescObservation.NewRow();
            newAccessionSourceDescriptor["accession_source_id"] = pkey;
            _accessionSourceDescObservation.Rows.Add(newAccessionSourceDescriptor);
            int newRowIndex = ux_datagridviewAccessionSourceDescObservation.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionSourceDescObservation.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionSourceDescObservation.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionSourceDescObservation["source_desc_observation_id", i].Value.Equals(newAccessionSourceDescriptor["source_desc_observation_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionSourceDescObservation.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionSourceDescObservation.CurrentCell = ux_datagridviewAccessionSourceDescObservation[newColIndex, newRowIndex];
            ux_datagridviewAccessionSourceDescObservation.Focus();
        }

        void _accessionSourceBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (_accessionSourceBindingSource.List.Count > 0)
            {
                // Make sure the child dgvs are not in edit mode (thus a new row might not be saved to the datatable yet)...
                ux_datagridviewAccessionSourceCooperator.EndEdit();
                ux_datagridviewAccessionSourceDescObservation.EndEdit();
                // Change the row filter for the child tables to reflect the new Accession's primary key...
                string pkey = ((DataRowView)_accessionSourceBindingSource.Current)[_accessionSource.PrimaryKey[0].ColumnName].ToString();
                if (_accessionSourceCooperator != null && !string.IsNullOrEmpty(pkey) && _accessionSourceCooperator.Columns.Contains("accession_source_id")) _accessionSourceCooperator.DefaultView.RowFilter = "accession_source_id=" + pkey.Trim().ToLower();
                if (_accessionSourceDescObservation != null && !string.IsNullOrEmpty(pkey) && _accessionSourceDescObservation.Columns.Contains("accession_source_id")) _accessionSourceDescObservation.DefaultView.RowFilter = "accession_source_id=" + pkey.Trim().ToLower();
            }
            else
            {
                if (_accessionSourceCooperator != null && _accessionSourceCooperator.Columns.Contains("accession_source_id")) _accessionSourceCooperator.DefaultView.RowFilter = "accession_source_id is null";
                if (_accessionSourceDescObservation != null && _accessionSourceDescObservation.Columns.Contains("accession_source_id")) _accessionSourceDescObservation.DefaultView.RowFilter = "accession_source_id is null";
            }
        }

        #endregion

        #region Accession Annotation logic...
        private void BuildAccessionAnnotationPage()
        {
            DataSet ds;
            // Get the accession_annotation table...
            ds = _sharedUtils.GetWebServiceData("get_accession_inv_annotation", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_inv_annotation"))
            {
                // Copy the accession_source table to a private variable...
                _accessionAnnotation = ds.Tables["get_accession_inv_annotation"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionAnnotation.DataSource = _accessionAnnotationBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionAnnotation, _accessionAnnotation);

                // Order and display the columns the way the user wants...
                int i = 0;
//if (ux_datagridviewAccessionAnnotation.Columns.Contains("INVENTORY_ID")) ux_datagridviewAccessionAnnotation.Columns["INVENTORY_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("ANNOTATION_TYPE_CODE")) ux_datagridviewAccessionAnnotation.Columns["ANNOTATION_TYPE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("ANNOTATION_DATE")) ux_datagridviewAccessionAnnotation.Columns["ANNOTATION_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("ANNOTATION_DATE_CODE")) ux_datagridviewAccessionAnnotation.Columns["ANNOTATION_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("OLD_TAXONOMY_SPECIES_ID")) ux_datagridviewAccessionAnnotation.Columns["OLD_TAXONOMY_SPECIES_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("NEW_TAXONOMY_SPECIES_ID")) ux_datagridviewAccessionAnnotation.Columns["NEW_TAXONOMY_SPECIES_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("ANNOTATION_COOPERATOR_ID")) ux_datagridviewAccessionAnnotation.Columns["ANNOTATION_COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("ORDER_REQUEST_ID")) ux_datagridviewAccessionAnnotation.Columns["ORDER_REQUEST_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAnnotation.Columns.Contains("NOTE")) ux_datagridviewAccessionAnnotation.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionAnnotation.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
            else
            {
                _accessionAnnotation = new DataTable();
            }
        }

        private void ux_buttonNewAccessionAnnotationRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionAnnotation = _accessionAnnotation.NewRow();
            newAccessionAnnotation["accession_id"] = pkey;
            // For existing accession records - find the system-generated inventory pkey...
            int iPKey = -1;
            if (int.TryParse(pkey, out iPKey) &&
                iPKey > 0)
            {
                newAccessionAnnotation["inventory_id"] = FindSystemGeneratedInvPKey(iPKey);
            }
            _accessionAnnotation.Rows.Add(newAccessionAnnotation);
            int newRowIndex = ux_datagridviewAccessionAnnotation.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionAnnotation.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionAnnotation.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionAnnotation["accession_inv_annotation_id", i].Value.Equals(newAccessionAnnotation["accession_inv_annotation_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionAnnotation.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionAnnotation.CurrentCell = ux_datagridviewAccessionAnnotation[newColIndex, newRowIndex];
        }
        #endregion

        #region Accession Voucher logic...
        private void BuildAccessionVoucherPage()
        {
            DataSet ds;
            // Get the accession_voucher table...
            ds = _sharedUtils.GetWebServiceData("get_accession_inv_voucher", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_inv_voucher"))
            {
                // Copy the accession_voucher table to a private variable...
                _accessionVoucher = ds.Tables["get_accession_inv_voucher"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionVoucher.DataSource = _accessionVoucherBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionVoucher, _accessionVoucher);

                // Order and display the columns the way the user wants...
                int i = 0;
//if (ux_datagridviewAccessionVoucher.Columns.Contains("INVENTORY_ID")) ux_datagridviewAccessionVoucher.Columns["INVENTORY_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionVoucher.Columns.Contains("VOUCHER_LOCATION")) ux_datagridviewAccessionVoucher.Columns["VOUCHER_LOCATION"].DisplayIndex = i++;
                if (ux_datagridviewAccessionVoucher.Columns.Contains("VOUCHERED_DATE")) ux_datagridviewAccessionVoucher.Columns["VOUCHERED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionVoucher.Columns.Contains("VOUCHERED_DATE_CODE")) ux_datagridviewAccessionVoucher.Columns["VOUCHERED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionVoucher.Columns.Contains("COLLECTOR_VOUCHER_NUMBER")) ux_datagridviewAccessionVoucher.Columns["COLLECTOR_VOUCHER_NUMBER"].DisplayIndex = i++;
                if (ux_datagridviewAccessionVoucher.Columns.Contains("COLLECTOR_COOPERATOR_ID")) ux_datagridviewAccessionVoucher.Columns["COLLECTOR_COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionVoucher.Columns.Contains("NOTE")) ux_datagridviewAccessionVoucher.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionVoucher.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
            else
            {
                _accessionVoucher = new DataTable();
            }
        }

        private void ux_buttonNewAccessionVoucherRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionVoucher = _accessionVoucher.NewRow();
            newAccessionVoucher["accession_id"] = pkey;
            // For existing accession records - find the system-generated inventory pkey...
            int iPKey = -1;
            if (int.TryParse(pkey, out iPKey) && 
                iPKey > 0)
            {
                newAccessionVoucher["inventory_id"] = FindSystemGeneratedInvPKey(iPKey);
            }
            _accessionVoucher.Rows.Add(newAccessionVoucher);
            int newRowIndex = ux_datagridviewAccessionVoucher.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionVoucher.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionVoucher.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionVoucher["accession_inv_voucher_id", i].Value.Equals(newAccessionVoucher["accession_inv_voucher_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionVoucher.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionVoucher.CurrentCell = ux_datagridviewAccessionVoucher[newColIndex, newRowIndex];
        }
        #endregion

        #region Accession IPR logic...
        private void BuildAccessionIPRPage()
        {
            DataSet ds;
            // Get the accession_IPR table...
            ds = _sharedUtils.GetWebServiceData("get_accession_ipr", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_ipr"))
            {
                // Copy the accession_IPR table to a private variable...
                _accessionIPR = ds.Tables["get_accession_ipr"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionIPR.DataSource = _accessionIPRBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionIPR, _accessionIPR);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewAccessionIPR.Columns.Contains("TYPE_CODE")) ux_datagridviewAccessionIPR.Columns["TYPE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("IPR_NUMBER")) ux_datagridviewAccessionIPR.Columns["IPR_NUMBER"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("IPR_CROP_NAME")) ux_datagridviewAccessionIPR.Columns["IPR_CROP_NAME"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("IPR_FULL_NAME")) ux_datagridviewAccessionIPR.Columns["IPR_FULL_NAME"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("ISSUED_DATE")) ux_datagridviewAccessionIPR.Columns["ISSUED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("EXPIRED_DATE")) ux_datagridviewAccessionIPR.Columns["EXPIRED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("ACCEPTED_DATE")) ux_datagridviewAccessionIPR.Columns["ACCEPTED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("EXPECTED_DATE")) ux_datagridviewAccessionIPR.Columns["EXPECTED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("COOPERATOR_ID")) ux_datagridviewAccessionIPR.Columns["COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionIPR.Columns.Contains("NOTE")) ux_datagridviewAccessionIPR.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionIPR.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
            else
            {
                _accessionIPR = new DataTable();
            }
        }

        private void ux_buttonNewAccessionIPRRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionIPR = _accessionIPR.NewRow();
            newAccessionIPR["accession_id"] = pkey;
            _accessionIPR.Rows.Add(newAccessionIPR);
            int newRowIndex = ux_datagridviewAccessionIPR.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionIPR.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionIPR.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionIPR["accession_ipr_id", i].Value.Equals(newAccessionIPR["accession_ipr_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionIPR.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionIPR.CurrentCell = ux_datagridviewAccessionIPR[newColIndex, newRowIndex];
        }
        #endregion

        #region Accession Quarantine logic...
        private void BuildAccessionQuarantinePage()
        {
            DataSet ds;
            // Get the accession_quarantine table...
            ds = _sharedUtils.GetWebServiceData("get_accession_quarantine", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_quarantine"))
            {
                // Copy the accession_quarantine table to a private variable...
                _accessionQuarantine = ds.Tables["get_accession_quarantine"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionQuarantine.DataSource = _accessionQuarantineBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionQuarantine, _accessionQuarantine);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("QUARANTINE_TYPE_CODE")) ux_datagridviewAccessionQuarantine.Columns["QUARANTINE_TYPE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("PROGRESS_STATUS_CODE")) ux_datagridviewAccessionQuarantine.Columns["PROGRESS_STATUS_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("CUSTODIAL_COOPERATOR_ID")) ux_datagridviewAccessionQuarantine.Columns["CUSTODIAL_COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("ENTERED_DATE")) ux_datagridviewAccessionQuarantine.Columns["ENTERED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("ESTABLISHED_DATE")) ux_datagridviewAccessionQuarantine.Columns["ESTABLISHED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("EXPECTED_RELEASE_DATE")) ux_datagridviewAccessionQuarantine.Columns["EXPECTED_RELEASE_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("RELEASED_DATE")) ux_datagridviewAccessionQuarantine.Columns["RELEASED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionQuarantine.Columns.Contains("NOTE")) ux_datagridviewAccessionQuarantine.Columns["NOTE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionQuarantine.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
            else
            {
                _accessionQuarantine = new DataTable();
            }
        }

        private void ux_buttonNewAccessionQuarantineRow_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionQuarantine = _accessionQuarantine.NewRow();
            newAccessionQuarantine["accession_id"] = pkey;
            _accessionQuarantine.Rows.Add(newAccessionQuarantine);
            int newRowIndex = ux_datagridviewAccessionQuarantine.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionQuarantine.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionQuarantine.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionQuarantine["accession_quarantine_id", i].Value.Equals(newAccessionQuarantine["accession_quarantine_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionQuarantine.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionQuarantine.CurrentCell = ux_datagridviewAccessionQuarantine[newColIndex, newRowIndex];
        }
        #endregion

        #region Accession Pedigree logic...
        private void BuildAccessionPedigreePage()
        {
            DataSet ds;
            // Get the accession_pedigree table...
            ds = _sharedUtils.GetWebServiceData("get_accession_pedigree", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_pedigree"))
            {
                // Copy the accession_pedigree table to a private variable...
                _accessionPedigree = ds.Tables["get_accession_pedigree"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionPedigree.DataSource = _accessionPedigreeBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionPedigree, _accessionPedigree);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("FEMALE_ACCESSION_ID")) ux_datagridviewAccessionPedigree.Columns["FEMALE_ACCESSION_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("FEMALE_EXTERNAL_ACCESSION")) ux_datagridviewAccessionPedigree.Columns["FEMALE_EXTERNAL_ACCESSION"].DisplayIndex = i++;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("MALE_ACCESSION_ID")) ux_datagridviewAccessionPedigree.Columns["MALE_ACCESSION_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("MALE_EXTERNAL_ACCESSION")) ux_datagridviewAccessionPedigree.Columns["MALE_EXTERNAL_ACCESSION"].DisplayIndex = i++;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("CROSS_CODE")) ux_datagridviewAccessionPedigree.Columns["CROSS_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("DESCRIPTION")) ux_datagridviewAccessionPedigree.Columns["DESCRIPTION"].DisplayIndex = i++;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("RELEASED_DATE")) ux_datagridviewAccessionPedigree.Columns["RELEASED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionPedigree.Columns.Contains("RELEASED_DATE_CODE")) ux_datagridviewAccessionPedigree.Columns["RELEASED_DATE_CODE"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionPedigree.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
            else
            {
                _accessionPedigree = new DataTable();
            }
        }

        private void ux_buttonNewAccessionPedigree_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionPedigree = _accessionPedigree.NewRow();
            newAccessionPedigree["accession_id"] = pkey;
            _accessionPedigree.Rows.Add(newAccessionPedigree);
            int newRowIndex = ux_datagridviewAccessionPedigree.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionPedigree.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionPedigree.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionPedigree["accession_pedigree_id", i].Value.Equals(newAccessionPedigree["accession_pedigree_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionPedigree.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionPedigree.CurrentCell = ux_datagridviewAccessionPedigree[newColIndex, newRowIndex];
        }
        #endregion

        #region Accession Action logic...
        private void BuildAccessionActionPage()
        {
            DataSet ds;
            // Get the accession_action table...
            ds = _sharedUtils.GetWebServiceData("get_accession_action", _accessionPKeys, 0, 0);
            if (ds.Tables.Contains("get_accession_action"))
            {
                // Copy the accession_action table to a private variable...
                _accessionAction = ds.Tables["get_accession_action"].Copy();
                // Bind the DGV to the binding source...
                ux_datagridviewAccessionAction.DataSource = _accessionActionBindingSource;
                // Build the DGV using the new table and bind it to the DGV's binding source (this happens in the Build method)...
                _sharedUtils.BuildEditDataGridView(ux_datagridviewAccessionAction, _accessionAction);

                // Order and display the columns the way the user wants...
                int i = 0;
                if (ux_datagridviewAccessionAction.Columns.Contains("ACTION_NAME_CODE")) ux_datagridviewAccessionAction.Columns["ACTION_NAME_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("STARTED_DATE")) ux_datagridviewAccessionAction.Columns["STARTED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("STARTED_DATE_CODE")) ux_datagridviewAccessionAction.Columns["STARTED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("COMPLETED_DATE")) ux_datagridviewAccessionAction.Columns["COMPLETED_DATE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("COMPLETED_DATE_CODE")) ux_datagridviewAccessionAction.Columns["COMPLETED_DATE_CODE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("IS_WEB_VISIBLE")) ux_datagridviewAccessionAction.Columns["IS_WEB_VISIBLE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("NOTE")) ux_datagridviewAccessionAction.Columns["NOTE"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("COOPERATOR_ID")) ux_datagridviewAccessionAction.Columns["COOPERATOR_ID"].DisplayIndex = i++;
                if (ux_datagridviewAccessionAction.Columns.Contains("METHOD_ID")) ux_datagridviewAccessionAction.Columns["METHOD_ID"].DisplayIndex = i++;

                foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionAction.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.Automatic;
                    // Hide any columns not explicitly ordered in the above code...
                    if (dgvc.DisplayIndex >= i) dgvc.Visible = false;
                }
            }
            else
            {
                _accessionAction = new DataTable();
            }
        }

        private void ux_buttonNewAccessionAction_Click(object sender, EventArgs e)
        {
            string pkey = ((DataRowView)_accessionBindingSource.Current)[_accession.PrimaryKey[0].ColumnName].ToString();
            DataRow newAccessionAction = _accessionAction.NewRow();
            newAccessionAction["accession_id"] = pkey;
            _accessionAction.Rows.Add(newAccessionAction);
            int newRowIndex = ux_datagridviewAccessionAction.Rows.GetLastRow(System.Windows.Forms.DataGridViewElementStates.Displayed);
            int newColIndex = ux_datagridviewAccessionAction.Columns.GetFirstColumn(System.Windows.Forms.DataGridViewElementStates.Displayed).Index;
            for (int i = 0; i < ux_datagridviewAccessionAction.Rows.Count; i++)
            {
                if (ux_datagridviewAccessionAction["accession_action_id", i].Value.Equals(newAccessionAction["accession_action_id"])) newRowIndex = i;
            }
            foreach (DataGridViewColumn dgvc in ux_datagridviewAccessionAction.Columns)
            {
                if (dgvc.DisplayIndex == 0)
                {
                    newColIndex = dgvc.Index;
                    break;
                }
            }
            ux_datagridviewAccessionAction.CurrentCell = ux_datagridviewAccessionAction[newColIndex, newRowIndex];
        }
        #endregion

    }
}
