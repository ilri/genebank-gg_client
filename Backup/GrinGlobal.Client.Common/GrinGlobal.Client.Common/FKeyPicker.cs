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
    public partial class FKeyPicker : Form
    {
        SharedUtils _sharedUtils;
        private string _currentKey = "";
        private string _newKey = "";
        private string _newValue = "";
        private Timer textChangeDelayTimer = new Timer();

        public FKeyPicker(SharedUtils sharedUtils, string columnNameToLookup, DataRow parentRow, string currentValue)
        {
            InitializeComponent();
            // Initialize new objects to access the local lookup tables...
            _sharedUtils = sharedUtils;
            // Save the key for the cell that is being edited...
            _currentKey = parentRow[columnNameToLookup].ToString();
            _newKey = _currentKey;
            ux_textboxAccessionNumber.Text = currentValue;
            // Set event handler for timer to wait a brief time before refreshing data (afte text has been typed)...
            textChangeDelayTimer.Tick += new EventHandler(timerDelay_Tick);
        }

        public string NewKey
        {
            get
            {
                return _newKey;
            }
        }

        public string NewValue
        {
            get
            {
                return _newValue;
            }
        }
        
        private void ux_textboxAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            textChangeDelayTimer.Stop();
            textChangeDelayTimer.Interval = 1000;
            textChangeDelayTimer.Start();
        }

        private void ux_textboxAccessionName_TextChanged(object sender, EventArgs e)
        {
            textChangeDelayTimer.Stop();
            textChangeDelayTimer.Interval = 1000;
            textChangeDelayTimer.Start();
        }

        private void ux_textboxTaxonomy_TextChanged(object sender, EventArgs e)
        {
            textChangeDelayTimer.Stop();
            textChangeDelayTimer.Interval = 1000;
            textChangeDelayTimer.Start();
        }
        
        private void RefreshData()
        {
//DataTable dt = new DataTable();
//string accessionRowFilter = "display_member like @displaymember + '%'";

//if (_sharedUtils.LocalDatabaseTableExists("accession_lookup"))
//{
//    // Query the local copy of the lookup...
//    // Get rows from the full lookup table that match the 'Find Filter' text...
//    dt = _sharedUtils.GetLocalData("SELECT TOP 1000 * FROM accession_lookup WHERE " + accessionRowFilter + " ORDER BY display_member ASC", "@displaymember=" + ux_textboxAccessionNumber.Text);
//}
//else
//{
//    dt = _sharedUtils.LookupTablesGetMatchingRows("accession_lookup", ux_textboxAccessionNumber.Text, 1000);
//}
            string query = "";
            int number;

            if (!string.IsNullOrEmpty(ux_textboxAccessionNumber.Text))
            {
                string[] accessionNumberTokens = ux_textboxAccessionNumber.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                switch (accessionNumberTokens.Length)
                {
                    case 1:
                        query = "@accession.accession_number_part1 LIKE '" + accessionNumberTokens[0].Trim().Replace("*", "%") + "'";
                        break;
                    case 2:
                        query = "@accession.accession_number_part1 LIKE '" + accessionNumberTokens[0].Trim().Replace("*", "%") + "'";
                        if (int.TryParse(accessionNumberTokens[1].Trim(), out number))
                        {
                            query += " AND @accession.accession_number_part2 = " + number.ToString();
                        }
                        break;
                    case 3:
                        query = "@accession.accession_number_part1 LIKE '" + accessionNumberTokens[0].Trim().Replace("*", "%") + "'";
                        if (int.TryParse(accessionNumberTokens[1].Trim(), out number))
                        {
                            query += " AND @accession.accession_number_part2 = " + number.ToString();
                        }
                        query += " AND @accession.accession_number_part3 LIKE '" + accessionNumberTokens[2].Trim().Replace("*", "%") + "'";
                        break;
                    default:
                        break;
                }
            }
            
            if (!string.IsNullOrEmpty(ux_textboxAccessionName.Text))
            {
                if (!string.IsNullOrEmpty(query))
                {
                    query += " AND @accession_inv_name.plant_name LIKE '" + ux_textboxAccessionName.Text.Replace("*", "%") + "'";
                }
                else
                {
                    query = "@accession_inv_name.plant_name LIKE '" + ux_textboxAccessionName.Text.Replace("*", "%") + "'";
                }
            }

            if (!string.IsNullOrEmpty(ux_textboxTaxonomy.Text))
            {
                if (!string.IsNullOrEmpty(query))
                {
                    query += " AND @taxonomy_species.name LIKE '" + ux_textboxTaxonomy.Text.Replace("*", "%") + "'";
                }
                else
                {
                    query = "@taxonomy_species.name LIKE '" + ux_textboxTaxonomy.Text.Replace("*", "%") + "'";
                }
            }

            DataSet ds = _sharedUtils.SearchWebService(query, true, true, "", "accession", 0, 1000);
            if (ds.Tables.Contains("SearchResult"))
            {
                // Build a list of accesion_ids to pass off to the inventory dataview...
                string accessionIDList = ":accessionid=";
                foreach (DataRow dr in ds.Tables["SearchResult"].Rows)
                {
                    accessionIDList += dr["ID"].ToString() + ",";
                }
                DataSet dsTemp = _sharedUtils.GetWebServiceData("order_wizard_get_inventory", accessionIDList, 0, 0);
                if (dsTemp.Tables.Contains("order_wizard_get_inventory"))
                {
                    _sharedUtils.BuildReadOnlyDataGridView(ux_datagridviewInventory, dsTemp.Tables["order_wizard_get_inventory"]);
                    int firstDistAvailRow = 0;
                    if (ux_datagridviewInventory.Columns.Contains("is_distributable") &&
                        ux_datagridviewInventory.Columns.Contains("is_available"))
                    {

                        for (int i = 0; i < ux_datagridviewInventory.Rows.Count; i++)
                        {
                            if (ux_datagridviewInventory.Rows[i].Cells["is_distributable"].Value.ToString().ToUpper() == "Y" &&
                                ux_datagridviewInventory.Rows[i].Cells["is_available"].Value.ToString().ToUpper() == "Y")
                            {
                                firstDistAvailRow = i;
                                break;
                            }
                        }
                        if (ux_datagridviewInventory.Rows.Count > 0 &&
                            ux_datagridviewInventory.Rows.Count >= firstDistAvailRow)
                        {
                            ux_datagridviewInventory.CurrentCell = ux_datagridviewInventory.Rows[firstDistAvailRow].Cells[0];
                        }
                    }
                }
                else
                {
                    ux_datagridviewInventory.DataSource = null;
                }
            }
            else
            {
                ux_datagridviewInventory.DataSource = null;
            }
        }

        private void timerDelay_Tick(object sender, EventArgs e)
        {
            // Change cursor to the wait cursor...
            Cursor origCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            textChangeDelayTimer.Stop();
            RefreshData();

            // Restore cursor to default cursor...
            Cursor.Current = origCursor;
        }

        private void ux_buttonOK_Click(object sender, EventArgs e)
        {
            if (ux_datagridviewInventory.CurrentRow != null)
            {
                _newKey = ux_datagridviewInventory.CurrentRow.Cells["inventory_id"].Value.ToString();
                _newValue = _sharedUtils.GetLookupDisplayMember("inventory_lookup", _newKey, "", _newKey);
            }
            else
            {
                _newKey = null;
                _newValue = "";
            }
            ux_datagridviewInventory.DataSource = null;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            ux_datagridviewInventory.DataSource = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ux_datagridviewInventory_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
            ux_buttonOK.PerformClick();
        }

    }
}
