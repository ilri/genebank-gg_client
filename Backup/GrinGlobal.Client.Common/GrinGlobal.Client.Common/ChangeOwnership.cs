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
    public partial class ChangeOwnership : Form
    {
        SharedUtils _sharedUtils;
        DataSet _ownedDataset;

        public ChangeOwnership(DataSet ownedDataset, SharedUtils sharedUtils)
        {
            InitializeComponent();
            _sharedUtils = sharedUtils;
            _ownedDataset = ownedDataset;
            DataTable cooperatorTable = _sharedUtils.GetLocalData("SELECT * FROM cooperator_lookup WHERE account_is_enabled = @accountisenabled", "@accountisenabled=Y;");
            if (cooperatorTable.Columns.Contains("display_member")) cooperatorTable.DefaultView.Sort = "display_member ASC";

            // Bind the control to the data in grinLookups...
            // WARNING!!!: You must set DisplayMember and ValueMember properties BEFORE setting 
            //             DataSource - otherwise the cbCooperators.SelectedValue.ToString() method 
            //             will return an object of DataRowView instead of the CNO value
            ux_comboboxNewOwner.DisplayMember = "display_member";
            ux_comboboxNewOwner.ValueMember = "value_member";
            ux_comboboxNewOwner.DataSource = cooperatorTable;
sharedUtils.UpdateControls(this.Controls, this.Name);
ux_radiobuttonSelectedRowsOnly.Checked = false;
ux_radiobuttonSelectedRowsAndChildren.Checked = false;
ux_buttonOk.Enabled = false;
ux_comboboxNewOwner.SelectedIndex = -1;
        }

        private void ux_buttonOk_Click(object sender, EventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string newOwner = ux_comboboxNewOwner.SelectedValue.ToString();

                // Pass the web service method 1000 records at a time (to keep the call smaller than 10MB)...
                string errorMessage = "";
                DataSet pagedOwnedDataset = new DataSet();
                pagedOwnedDataset.Tables.Add(_ownedDataset.Tables[0].Clone());
                int pageSize = 1000;
                int pageStart = 0;
                int pageStop = Math.Min(pageSize, _ownedDataset.Tables[0].Rows.Count);
                while (pageStart < _ownedDataset.Tables[0].Rows.Count)
                {
                    // Clear the table page from the dataset...
                    pagedOwnedDataset.Tables[0].Clear();
                    pagedOwnedDataset.AcceptChanges();
                    // Build a new table page row by row...
                    for (int i = pageStart; i < pageStop; i++)
                    {
                        // Make a copy of the datarow in the paged datatable...
                        pagedOwnedDataset.Tables[0].Rows.Add(_ownedDataset.Tables[0].Rows[i].ItemArray);
                    }
                    // Change the ownership of the rows in the paged table...
                    DataSet changeOwnershipResults = _sharedUtils.ChangeOwnership(pagedOwnedDataset, newOwner, ux_radiobuttonSelectedRowsAndChildren.Checked);
                    if (changeOwnershipResults != null &&
                    changeOwnershipResults.Tables.Contains("ExceptionTable") &&
                    changeOwnershipResults.Tables["ExceptionTable"].Rows.Count > 0)
                    {
                        errorMessage += "\n" + changeOwnershipResults.Tables["ExceptionTable"].Rows[0]["Message"].ToString();
                    }

                    // Update the paging indexes...
                    pageStart = pageStop;
                    pageStop = Math.Min((pageStart + pageSize), _ownedDataset.Tables[0].Rows.Count);
                }

                if (string.IsNullOrEmpty(errorMessage))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error changing ownership of your records.\n\nFull error message:\n{0}", "Save Ownership Changes Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                    ggMessageBox.Name = "ChangeOwnership_ux_buttonOKMessage1";
                    _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                    if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, errorMessage);
                    ggMessageBox.ShowDialog();
                }
            }
            catch
            {
                GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("There was an unexpected error changing ownership of your records.", "Save Ownership Changes Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                ggMessageBox.Name = "ChangeOwnership_ux_buttonOKMessage2";
                _sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
                ggMessageBox.ShowDialog();
            }

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ux_comboboxNewOwner_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ux_radiobuttonSelectedRowsOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (ux_radiobuttonSelectedRowsOnly.Checked) ux_buttonOk.Enabled = true;
        }

        private void ux_radiobuttonSelectedRowsAndChildren_CheckedChanged(object sender, EventArgs e)
        {
            if (ux_radiobuttonSelectedRowsAndChildren.Checked) ux_buttonOk.Enabled = true;
        }
    }
}
