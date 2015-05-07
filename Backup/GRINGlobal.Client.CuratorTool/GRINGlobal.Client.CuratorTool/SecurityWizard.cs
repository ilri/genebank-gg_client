using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GRINGlobal.Client.Common;

namespace GRINGlobal.Client
{
    public partial class SecurityWizard : Form
    {
        string _tableName;
        string _tableID = "-1";
        string _pkeyColumnName;
        string _pkeyColumnID = "-1";
        string _ownedByColumnID = "-1";
        string _pkeyCollection;
        string _cooperatorID = "-1";
        string _currentSysGroup = "";
        SharedUtils _sharedUtils;
        DataTable _sysGroup;
        DataTable _sysGroupPermissionMap;
        DataTable _sysGroupUserMap;
        DataTable _sysPermission;
        DataTable _sysPermissionField;
        DataTable _sysPermissionCode;
        DataTable _sysUser;
        DataTable _sysTableFieldRelationship;
        DataTable _sysPermissionFieldCollection;

        public SecurityWizard(string tableName, string pkeyColumnName, string pkeyCollection, SharedUtils sharedUtils)
        {
            InitializeComponent();

            _tableName = tableName;
            _pkeyColumnName = pkeyColumnName;
            _pkeyCollection = pkeyCollection;
            _sharedUtils = sharedUtils;
            _cooperatorID = _sharedUtils.UserCooperatorID;
        }

        private void SecurityWizard_Load(object sender, EventArgs e)
        {
            #region old_code...
            //DataSet ds = new DataSet();


            //// Get the sys_user table and wire it up to the ux_comboboxUser control...
            //ds = _sharedUtils.GetWebServiceData("get_sys_user", "", 0, 0);
            //if (ds.Tables.Contains("get_sys_user"))
            //{
            //    _sysUser = ds.Tables["get_sys_user"].Copy();
            //    // Build a display name for the cooperator_id...
            //    _sysUser.Columns.Add("cooperator_name", typeof(string));
            //    foreach (DataRow dr in _sysUser.Rows)
            //    {
            //        dr["cooperator_name"] = _sharedUtils.GetLookupDisplayMember("cooperator_lookup", dr["cooperator_id"].ToString(), "", dr["cooperator_id"].ToString());
            //    }
            //    _sysUser.DefaultView.Sort = "cooperator_name asc";
            //    ux_listboxAllUsers.DisplayMember = "cooperator_name";
            //    ux_listboxAllUsers.ValueMember = "sys_user_id";
            //    ux_listboxAllUsers.DataSource = _sysUser;
                
            //    // Accept the changes to clean out all of the table modifications...
            //    _sysUser.AcceptChanges();
            //}
            //else
            //{
            //    MessageBox.Show("Error retrieving the get_sys_user dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}
            //// Get the sys_group_permission_map table...
            //ds = _sharedUtils.GetWebServiceData("get_sys_group_permission_map", ":cooperatorid=" + _sharedUtils.UserCooperatorID, 0, 0);
            //if (ds.Tables.Contains("get_sys_group_permission_map"))
            //{
            //    _sysGroupPermissionMap = ds.Tables["get_sys_group_permission_map"].Copy();
            //}
            //else
            //{
            //    MessageBox.Show("Error retrieving the get_sys_group_permission_map dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}
            //// Get the sys_group_user_map table...
            //ds = _sharedUtils.GetWebServiceData("get_sys_group_user_map", ":cooperatorid=" + _sharedUtils.UserCooperatorID, 0, 0);
            //if (ds.Tables.Contains("get_sys_group_user_map"))
            //{
            //    _sysGroupUserMap = ds.Tables["get_sys_group_user_map"].Copy();
            //    // Build a display name for the cooperator_id...
            //    _sysGroupUserMap.Columns.Add("cooperator_name", typeof(string));
            //    foreach (DataRow dr in _sysGroupUserMap.Rows)
            //    {
            //        dr["cooperator_name"] = _sysUser.Rows.Find(dr["sys_user_id"])["cooperator_name"];
            //    }
            //    ux_listboxUsers.DisplayMember = "cooperator_name";
            //    ux_listboxUsers.ValueMember = "sys_user_id";
            //    ux_listboxUsers.DataSource = _sysGroupUserMap;

            //    // Accept the changes to clean out all of the table modifications...
            //    _sysGroupUserMap.AcceptChanges();
            //}
            //else
            //{
            //    MessageBox.Show("Error retrieving the get_sys_group_user_map dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}
            //// Get the sys_permission_codes...
            //_sysPermissionCode = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup WHERE group_name='sys_permission_code'", "");
            //if (_sysPermissionCode == null)
            //{
            //    _sysPermissionCode = new DataTable("sys_permission_code");
            //    _sysPermissionCode.Columns.Add("display_member", typeof(string));
            //    _sysPermissionCode.Columns.Add("value_member", typeof(string));
            //}
            //if(_sysPermissionCode.Rows.Count == 0)
            //{
            //    DataRow newSysPermissionCodeRow = _sysPermissionCode.NewRow();
            //    newSysPermissionCodeRow["display_member"] = "Inherit";
            //    newSysPermissionCodeRow["value_member"] = "I";
            //    _sysPermissionCode.Rows.Add(newSysPermissionCodeRow);
            //    newSysPermissionCodeRow = _sysPermissionCode.NewRow();
            //    newSysPermissionCodeRow["display_member"] = "Deny";
            //    newSysPermissionCodeRow["value_member"] = "D";
            //    _sysPermissionCode.Rows.Add(newSysPermissionCodeRow);
            //    newSysPermissionCodeRow = _sysPermissionCode.NewRow();
            //    newSysPermissionCodeRow["display_member"] = "Allow";
            //    newSysPermissionCodeRow["value_member"] = "A";
            //    _sysPermissionCode.Rows.Add(newSysPermissionCodeRow);
            //}
            //// Wire up the permission comboboxes...
            //ux_comboboxRead.DisplayMember = "display_member";
            //ux_comboboxRead.ValueMember = "value_member";
            //ux_comboboxRead.DataSource = _sysPermissionCode.Copy();
            //ux_comboboxUpdate.DisplayMember = "display_member";
            //ux_comboboxUpdate.ValueMember = "value_member";
            //ux_comboboxUpdate.DataSource = _sysPermissionCode.Copy();
            //ux_comboboxCreate.DisplayMember = "display_member";
            //ux_comboboxCreate.ValueMember = "value_member";
            //ux_comboboxCreate.DataSource = _sysPermissionCode.Copy();
            //ux_comboboxDelete.DisplayMember = "display_member";
            //ux_comboboxDelete.ValueMember = "value_member";
            //ux_comboboxDelete.DataSource = _sysPermissionCode.Copy();

            //// Get the sys_permission table...
            //ds = _sharedUtils.GetWebServiceData("get_sys_permission", ":cooperatorid=" + _sharedUtils.UserCooperatorID, 0, 0);
            //if (ds.Tables.Contains("get_sys_permission"))
            //{
            //    _sysPermission = ds.Tables["get_sys_permission"].Copy();
            //}
            //else
            //{
            //    MessageBox.Show("Error retrieving the get_sys_permission dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}
            //// Get the sys_permission_field table...
            //ds = _sharedUtils.GetWebServiceData("get_sys_permission_field", ":cooperatorid=" + _sharedUtils.UserCooperatorID, 0, 0);
            //if (ds.Tables.Contains("get_sys_permission_field"))
            //{
            //    _sysPermissionField = ds.Tables["get_sys_permission_field"].Copy();
            //}
            //else
            //{
            //    MessageBox.Show("Error retrieving the get_sys_permission_field dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}

            //// Build _sysPermissionFieldCollection table...
            //_sysPermissionFieldCollection = new DataTable();
            //_sysPermissionFieldCollection.Columns.Add(_pkeyColumnName, typeof(int));
            //_sysPermissionFieldCollection.Columns.Add("sys_permission_id", typeof(int));
            //_sysPermissionFieldCollection.Columns.Add("display_member", typeof(string));
            //_sysPermissionFieldCollection.PrimaryKey = new DataColumn[] { _sysPermissionFieldCollection.Columns[_pkeyColumnName], _sysPermissionFieldCollection.Columns["sys_permission_id"] };
            ////PopulateRowRestrictionsListbox(_pkeyCollection);
            //ux_listboxRowRestrictions.ValueMember = _pkeyColumnName;
            //ux_listboxRowRestrictions.DisplayMember = "display_member";
            //ux_listboxRowRestrictions.DataSource = _sysPermissionFieldCollection;

            //// Get the sys_table_field_relationship table...
            //ds = _sharedUtils.GetWebServiceData("sys_table_field_relationship", "", 0, 0);
            //if (ds.Tables.Contains("sys_table_field_relationship"))
            //{
            //    _sysTableFieldRelationship = ds.Tables["sys_table_field_relationship"].Copy();
            //    _sysTableFieldRelationship.DefaultView.RowFilter = "(child_field_name='" + _pkeyColumnName + "' AND child_field_purpose='PRIMARY_KEY') OR parent_field_name='" + _pkeyColumnName + "'";
            //}
            //else
            //{
            //    MessageBox.Show("Error retrieving the sys_table_field_relationship dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}

            //// Leave this table to last (so that it will filter the other lists properly when it gets populated - triggers a SelectedIndexChanged event)...
            //// Get the sys_group table...
            //ds = _sharedUtils.GetWebServiceData("get_sys_group", ":cooperatorid=" + _sharedUtils.UserCooperatorID, 0, 0);
            //if (ds.Tables.Contains("get_sys_group"))
            //{
            //    _sysGroup = ds.Tables["get_sys_group"].Copy();
            //    // Add two extra columns for tracking row restrictions and child table scope (by group_id)...
            //    _sysGroup.Columns.Add("include_child_tables", typeof(bool));
            //    _sysGroup.Columns.Add("selected_rows_only", typeof(bool));
            //    _sysGroup.Columns["include_child_tables"].DefaultValue = true;
            //    _sysGroup.Columns["selected_rows_only"].DefaultValue = false;

            //    // Wire up the sys_group table with the list box...
            //    ux_listboxPolicies.DisplayMember = "group_tag";
            //    ux_listboxPolicies.ValueMember = "sys_group_id";
            //    ux_listboxPolicies.DataSource = _sysGroup;

            //    // Get the 'owned_by' table_field_ID (using the table name and 'owned_by' field name in the table_field_relationship table)...
            //    DataRow[] drsSysTableFieldRelationship = _sysTableFieldRelationship.Select("child_table_name='" + _tableName + "' AND child_field_name='owned_by' AND child_field_purpose='AUTO_ASSIGN_OWN'");
            //    if (drsSysTableFieldRelationship.Length > 0)
            //    {
            //        // Populate the _ownedByColumnID variables...
            //        _ownedByColumnID = (int)drsSysTableFieldRelationship[0]["child_table_field_id"];
            //    }
            //    // Filter out all of the groups that do not contain a permission that uses the currentTableID...
            //    // Get the table_ID and pkey table_field_ID (using the table name and field name in the table_field_relationship table)...
            //    drsSysTableFieldRelationship = _sysTableFieldRelationship.Select("child_table_name='" + _tableName + "' AND child_field_name='" + _pkeyColumnName + "' AND child_field_purpose='PRIMARY_KEY'");
            //    string validSysGroups = "null";
            //    if (drsSysTableFieldRelationship.Length > 0)
            //    {
            //        // Populate the _tableID and pkeyColumnID variables...
            //        _tableID = (int)drsSysTableFieldRelationship[0]["child_table_id"];
            //        _pkeyColumnID = (int)drsSysTableFieldRelationship[0]["child_table_field_id"];

            //        foreach (DataRow sysGroupRow in _sysGroup.Rows)
            //        {
            //            bool foundTableInGroupPolicy = false;
            //            _sysGroupPermissionMap.DefaultView.RowFilter = "sys_group_id=" + sysGroupRow["sys_group_id"];
            //            foreach (DataRowView sysGroupPermissionRow in _sysGroupPermissionMap.DefaultView)
            //            {
            //                int sysTableID = 0;
            //                DataRow sysPermissionRow = _sysPermission.Rows.Find(sysGroupPermissionRow["sys_permission_id"]);
            //                if (sysPermissionRow != null &&
            //                    int.TryParse(sysPermissionRow["sys_table_id"].ToString(), out sysTableID) &&
            //                    sysTableID == _tableID) foundTableInGroupPolicy = true;
            //            }
            //            // If this is a valid group - add it to the list...
            //            if (foundTableInGroupPolicy) validSysGroups +=  "," + sysGroupRow["sys_group_id"].ToString();
            //        }
            //    }
            //    _sysGroup.DefaultView.RowFilter = "sys_group_id IN (" + validSysGroups.TrimEnd(',') + ")";
            //    ux_listboxPolicies.SelectedIndex = -1;
            //    if (_sysGroup.DefaultView.Count < 1)
            //    {
            //        ux_groupboxPermissions.Visible = false;
            //        ux_groupboxScope.Visible = false;
            //        ux_buttonEditUserListDone.PerformClick();
            //    }
            //    else
            //    {
            //        ux_listboxPolicies.SelectedIndex = 0;
            //        ux_groupboxPermissions.Visible = true;
            //        ux_groupboxScope.Visible = true;
            //        ux_buttonEditUserListDone.PerformClick();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Error retrieving the get_sys_group dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.DialogResult = DialogResult.Cancel;
            //    this.Close();
            //}
            #endregion
            // Load the users group data from the DB...
            LoadDBTables(_cooperatorID);

            // Wire up the sys_group table with the list box...
            ux_listboxPolicies.DisplayMember = "group_tag";
            ux_listboxPolicies.ValueMember = "sys_group_id";
            ux_listboxPolicies.DataSource = _sysGroup;

            // Wire up the sys_users table to the 'all user' listbox...
            _sysUser.DefaultView.Sort = "cooperator_name asc";
            ux_listboxAllUsers.DisplayMember = "cooperator_name";
            ux_listboxAllUsers.ValueMember = "sys_user_id";
            ux_listboxAllUsers.DataSource = _sysUser;
_sharedUtils.UpdateControls(this.Controls, this.Name);
        }

        private void ux_buttonSave_Click(object sender, EventArgs e)
        {
            // Force one last update for the controls (to make sure everything is commited to the tables 
            // before attempting to save the data to the database...
            ux_listboxPolicies.SelectedIndex = -1;

            SaveInserts();
            SaveUpdates();
            SaveDeletes();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void SaveInserts()
        {
            // STEP 1
            // Add/Update the groups to the sys_group table...
            DataSet sysGroupChanges = new DataSet();
            DataSet sysGroupSaveResults;
            if (_sysGroup.GetChanges(DataRowState.Added) != null)
            {
                sysGroupChanges.Tables.Add(_sysGroup.GetChanges(DataRowState.Added));
                sysGroupSaveResults = _sharedUtils.SaveWebServiceData(sysGroupChanges);
                // Iterate through the saved records to update the pkeys in child tables...
                if (sysGroupSaveResults.Tables.Contains(_sysGroup.TableName))
                {
                    foreach (DataRow groupRow in sysGroupSaveResults.Tables[_sysGroup.TableName].Rows)
                    {
                        // If the sys_group row saved successfully - update the sys_group_user_map and sys_group_permission_map tables with new pkeys...
                        if (groupRow["SavedAction"].ToString().Trim().ToUpper() == "INSERT" &&
                            groupRow["SavedStatus"].ToString().Trim().ToUpper() == "SUCCESS")
                        {
                            // Update sys_group_user_map rows...
                            DataRow[] groupUserMapRows = _sysGroupUserMap.Select("sys_group_id=" + groupRow["OriginalPrimaryKeyID"]);
                            foreach (DataRow groupUserMapRow in groupUserMapRows)
                            {
                                groupUserMapRow["sys_group_id"] = groupRow["NewPrimaryKeyID"];
                            }
                            // Update sys_group_permission_map rows...
                            DataRow[] groupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + groupRow["OriginalPrimaryKeyID"]);
                            foreach (DataRow groupPermissionMapRow in groupPermissionMapRows)
                            {
                                groupPermissionMapRow["sys_group_id"] = groupRow["NewPrimaryKeyID"];
                            }
                        }
                    }
                }
            }

            // STEP 2
            // Add/Update the permissions to the sys_permission table...
            DataSet sysPermissionChanges = new DataSet();
            DataSet sysPermissionSaveResults;
            if (_sysPermission.GetChanges(DataRowState.Added) != null)
            {
                sysPermissionChanges.Tables.Add(_sysPermission.GetChanges(DataRowState.Added));
                sysPermissionSaveResults = _sharedUtils.SaveWebServiceData(sysPermissionChanges);
                // Iterate through the saved records to update the pkeys in child tables...
                if (sysPermissionSaveResults.Tables.Contains(_sysPermission.TableName))
                {
                    foreach (DataRow permissionRow in sysPermissionSaveResults.Tables[_sysPermission.TableName].Rows)
                    {
                        // If the sys_permission row saved successfully - update the:
                        // sys_group_user_map and sys_group_permission_map and sys_permission_field tableswith new pkeys...
                        if (permissionRow["SavedAction"].ToString().Trim().ToUpper() == "INSERT" &&
                            permissionRow["SavedStatus"].ToString().Trim().ToUpper() == "SUCCESS")
                        {
                            // Update sys_group_permission_map rows...
                            DataRow[] groupPermissionMapRows = _sysGroupPermissionMap.Select("sys_permission_id=" + permissionRow["OriginalPrimaryKeyID"]);
                            foreach (DataRow groupPermissionMapRow in groupPermissionMapRows)
                            {
                                groupPermissionMapRow["sys_permission_id"] = permissionRow["NewPrimaryKeyID"];
                            }
                            // Update sys_permission_field rows...
                            DataRow[] permissionFieldRows = _sysPermissionField.Select("sys_permission_id=" + permissionRow["OriginalPrimaryKeyID"]);
                            foreach (DataRow permissionFieldRow in permissionFieldRows)
                            {
                                permissionFieldRow["sys_permission_id"] = permissionRow["NewPrimaryKeyID"];
                            }
                        }
                    }
                }
            }

            // STEP 3
            // Add/Update the user_maps to the sys_group_user_map table...
            DataSet sysGroupUserMapChanges = new DataSet();
            DataSet sysGroupUserMapSaveResults;
            if (_sysGroupUserMap.GetChanges(DataRowState.Added) != null)
            {
                sysGroupUserMapChanges.Tables.Add(_sysGroupUserMap.GetChanges(DataRowState.Added));
                sysGroupUserMapSaveResults = _sharedUtils.SaveWebServiceData(sysGroupUserMapChanges);
            }

            // STEP 4
            // Add/Update the permission_maps to the sys_group_permission_map table...
            DataSet sysGroupPermissionMapChanges = new DataSet();
            DataSet sysGroupPermissionMapSaveResults;
            if (_sysGroupPermissionMap.GetChanges(DataRowState.Added) != null)
            {
                sysGroupPermissionMapChanges.Tables.Add(_sysGroupPermissionMap.GetChanges(DataRowState.Added));
                sysGroupPermissionMapSaveResults = _sharedUtils.SaveWebServiceData(sysGroupPermissionMapChanges);
            }

            // STEP 5
            // Add/Update the permissionFields to the sys_permission_field table...
            DataSet sysPermissionFieldChanges = new DataSet();
            DataSet sysPermissionFieldSaveResults;
            if (_sysPermissionField.GetChanges(DataRowState.Added) != null)
            {
                sysPermissionFieldChanges.Tables.Add(_sysPermissionField.GetChanges(DataRowState.Added));
                sysPermissionFieldSaveResults = _sharedUtils.SaveWebServiceData(sysPermissionFieldChanges);
            }
        }

        private void SaveUpdates()
        {
            // STEP 1
            // Add/Update the groups to the sys_group table...
            DataSet sysGroupChanges = new DataSet();
            DataSet sysGroupSaveResults;
            if (_sysGroup.GetChanges(DataRowState.Modified) != null)
            {
                sysGroupChanges.Tables.Add(_sysGroup.GetChanges(DataRowState.Modified));
                sysGroupSaveResults = _sharedUtils.SaveWebServiceData(sysGroupChanges);
            }

            // STEP 2
            // Add/Update the permissions to the sys_permission table...
            DataSet sysPermissionChanges = new DataSet();
            DataSet sysPermissionSaveResults;
            if (_sysPermission.GetChanges(DataRowState.Modified) != null)
            {
                sysPermissionChanges.Tables.Add(_sysPermission.GetChanges(DataRowState.Modified));
                sysPermissionSaveResults = _sharedUtils.SaveWebServiceData(sysPermissionChanges);
            }

            // STEP 3
            // Add/Update the user_maps to the sys_group_user_map table...
            DataSet sysGroupUserMapChanges = new DataSet();
            DataSet sysGroupUserMapSaveResults;
            if (_sysGroupUserMap.GetChanges(DataRowState.Modified) != null)
            {
                sysGroupUserMapChanges.Tables.Add(_sysGroupUserMap.GetChanges(DataRowState.Modified));
                sysGroupUserMapSaveResults = _sharedUtils.SaveWebServiceData(sysGroupUserMapChanges);
            }

            // STEP 4
            // Add/Update the permission_maps to the sys_group_permission_map table...
            DataSet sysGroupPermissionMapChanges = new DataSet();
            DataSet sysGroupPermissionMapSaveResults;
            if (_sysGroupPermissionMap.GetChanges(DataRowState.Modified) != null)
            {
                sysGroupPermissionMapChanges.Tables.Add(_sysGroupPermissionMap.GetChanges(DataRowState.Modified));
                sysGroupPermissionMapSaveResults = _sharedUtils.SaveWebServiceData(sysGroupPermissionMapChanges);
            }

            // STEP 5
            // Add/Update the permissionFields to the sys_permission_field table...
            DataSet sysPermissionFieldChanges = new DataSet();
            DataSet sysPermissionFieldSaveResults;
            if (_sysPermissionField.GetChanges(DataRowState.Modified) != null)
            {
                sysPermissionFieldChanges.Tables.Add(_sysPermissionField.GetChanges(DataRowState.Modified));
                sysPermissionFieldSaveResults = _sharedUtils.SaveWebServiceData(sysPermissionFieldChanges);
            }
        }

        private void SaveDeletes()
        {
            // STEP 1
            // Add/Update the permissionFields to the sys_permission_field table...
            DataSet sysPermissionFieldChanges = new DataSet();
            DataSet sysPermissionFieldSaveResults;
            if (_sysPermissionField.GetChanges(DataRowState.Deleted) != null)
            {
                sysPermissionFieldChanges.Tables.Add(_sysPermissionField.GetChanges(DataRowState.Deleted));
                sysPermissionFieldSaveResults = _sharedUtils.SaveWebServiceData(sysPermissionFieldChanges);
            }

            // STEP 2
            // Add/Update the permission_maps to the sys_group_permission_map table...
            DataSet sysGroupPermissionMapChanges = new DataSet();
            DataSet sysGroupPermissionMapSaveResults;
            if (_sysGroupPermissionMap.GetChanges(DataRowState.Deleted) != null)
            {
                sysGroupPermissionMapChanges.Tables.Add(_sysGroupPermissionMap.GetChanges(DataRowState.Deleted));
                sysGroupPermissionMapSaveResults = _sharedUtils.SaveWebServiceData(sysGroupPermissionMapChanges);
            }

            // STEP 3
            // Add/Update the user_maps to the sys_group_user_map table...
            DataSet sysGroupUserMapChanges = new DataSet();
            DataSet sysGroupUserMapSaveResults;
            if (_sysGroupUserMap.GetChanges(DataRowState.Deleted) != null)
            {
                sysGroupUserMapChanges.Tables.Add(_sysGroupUserMap.GetChanges(DataRowState.Deleted));
                sysGroupUserMapSaveResults = _sharedUtils.SaveWebServiceData(sysGroupUserMapChanges);
            }

            // STEP 4
            // Add/Update the permissions to the sys_permission table...
            DataSet sysPermissionChanges = new DataSet();
            DataSet sysPermissionSaveResults;
            if (_sysPermission.GetChanges(DataRowState.Deleted) != null)
            {
                sysPermissionChanges.Tables.Add(_sysPermission.GetChanges(DataRowState.Deleted));
                sysPermissionSaveResults = _sharedUtils.SaveWebServiceData(sysPermissionChanges);
            }

            // STEP 5
            // Add/Update the groups to the sys_group table...
            DataSet sysGroupChanges = new DataSet();
            DataSet sysGroupSaveResults;
            if (_sysGroup.GetChanges(DataRowState.Deleted) != null)
            {
                sysGroupChanges.Tables.Add(_sysGroup.GetChanges(DataRowState.Deleted));
                sysGroupSaveResults = _sharedUtils.SaveWebServiceData(sysGroupChanges);
            }
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ux_buttonAddPolicy_Click(object sender, EventArgs e)
        {
            BuildNewPolicy();
        }

        private void BuildNewPolicy()
        {
            // STEP 1: Create new sys_group record...
            DataRow newPolicy = _sysGroup.NewRow();
            newPolicy["group_tag"] = "Security Policy " + ux_listboxPolicies.Items.Count.ToString();
            newPolicy["is_enabled"] = "Y";
            _sysGroup.Rows.Add(newPolicy);

            // STEP 2: Create new sys_permission record...
            DataRow newPermission = _sysPermission.NewRow();
            newPermission["sys_table_id"] = _tableID;
            newPermission["permission_tag"] = "SW - " + _sharedUtils.Username + " - " + newPolicy["group_tag"].ToString();
            newPermission["is_enabled"] = "Y";
            newPermission["read_permission"] = "I";
            newPermission["update_permission"] = "I";
            newPermission["create_permission"] = "I";
            newPermission["delete_permission"] = "I";
            _sysPermission.Rows.Add(newPermission);

            // STEP 3: Create new sys_permission_field record (for compare_mode='current' and 'parent')...
            // First grant permissions to the users for all rows owned by CT user...
            DataRow newPermissionFieldCurrent = _sysPermissionField.NewRow();
            newPermissionFieldCurrent["sys_permission_id"] = newPermission["sys_permission_id"];
            //newPermissionFieldCurrent["sys_table_field_id"] = _pkeyColumnID;
            newPermissionFieldCurrent["sys_table_field_id"] = _ownedByColumnID;
            newPermissionFieldCurrent["field_type"] = "INTEGER";
            newPermissionFieldCurrent["compare_operator"] = "in";
            //newPermissionFieldCurrent["compare_value"] = _pkeyCollection;
            newPermissionFieldCurrent["compare_value"] = _cooperatorID;
            newPermissionFieldCurrent["compare_mode"] = "current";
            _sysPermissionField.Rows.Add(newPermissionFieldCurrent);
            // Second grant permissions to the users for all children rows related to all rows owned by CT user...
            DataRow newPermissionFieldParent = _sysPermissionField.NewRow();
            newPermissionFieldParent["sys_permission_id"] = newPermission["sys_permission_id"];
            //newPermissionFieldParent["parent_table_field_id"] = _pkeyColumnID;
            newPermissionFieldParent["parent_table_field_id"] = _ownedByColumnID;
            newPermissionFieldParent["parent_field_type"] = "INTEGER";
            newPermissionFieldParent["parent_compare_operator"] = "in";
            //newPermissionFieldParent["parent_compare_value"] = _pkeyCollection;
            newPermissionFieldParent["parent_compare_value"] = _cooperatorID;
            newPermissionFieldParent["compare_mode"] = "parent";
            _sysPermissionField.Rows.Add(newPermissionFieldParent);

            // STEP 4: Create new sys_group_permission_map record...
            DataRow newGroupPermissionMap = _sysGroupPermissionMap.NewRow();
            newGroupPermissionMap["sys_group_id"] = newPolicy["sys_group_id"];
            newGroupPermissionMap["sys_permission_id"] = newPermission["sys_permission_id"];
            _sysGroupPermissionMap.Rows.Add(newGroupPermissionMap);

            // STEP 5: Populate the user controls with data from the new policy...
            // Add new record ID to _sysGroup row filter (so the use can see the new row)...
            _sysGroup.DefaultView.RowFilter = _sysGroup.DefaultView.RowFilter.Replace("null", "null," + newPolicy["sys_group_id"].ToString());
            // Make the new policy the selected item in the listbox (to trigger all dependent controls to update)...
            ux_listboxPolicies.SelectedIndex = -1;
            ux_listboxPolicies.SelectedValue = newPolicy["sys_group_id"];
            ux_checkboxIncludeChildTables.Checked = false;
            ux_checkboxIncludeChildTables.Checked = true;
        }

        private void ux_listboxPolicies_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region old_code...
            //if (ux_listboxPolicies.SelectedIndex >= 0)
            //{
            //    // Retrieve the group row for this listbox selection...
            //    DataRow sysGroupRow = _sysGroup.Rows.Find(ux_listboxPolicies.SelectedValue);
            //    // Filter the users for this group...
            //    if (_sysGroupUserMap != null) _sysGroupUserMap.DefaultView.RowFilter = "sys_group_id=" + ux_listboxPolicies.SelectedValue.ToString().Trim();
            //    // Filter the permissions for this group...
            //    if (_sysGroupPermissionMap != null)
            //    {
            //        _sysGroupPermissionMap.DefaultView.RowFilter = "sys_group_id=" + ux_listboxPolicies.SelectedValue.ToString().Trim();
            //        if (_sysGroupPermissionMap.DefaultView.Count > 0)
            //        {
            //            // Retrieve the permission record and populate the comboboxes...
            //            DataRow sysPermissionRow = _sysPermission.Rows.Find(_sysGroupPermissionMap.DefaultView[0]["sys_permission_id"]);
            //            if (sysPermissionRow != null)
            //            {
            //                // Set the row filters for the permissions tables...
            //                _sysPermission.DefaultView.RowFilter = "sys_permission_id=" + sysPermissionRow["sys_permission_id"];
            //                _sysPermissionFieldCollection.DefaultView.RowFilter = "sys_permission_id=" + sysPermissionRow["sys_permission_id"];
            //                // Populate the comboboxes...
            //                ux_comboboxRead.SelectedValue = sysPermissionRow["read_permission"];
            //                ux_comboboxUpdate.SelectedValue = sysPermissionRow["update_permission"];
            //                ux_comboboxCreate.SelectedValue = sysPermissionRow["create_permission"];
            //                ux_comboboxDelete.SelectedValue = sysPermissionRow["delete_permission"];
            //            }
            //            DataRow[] sysPermissionFieldRows = _sysPermissionField.Select("sys_permission_id=" + _sysGroupPermissionMap.DefaultView[0]["sys_permission_id"]);
            //            if (sysPermissionFieldRows != null)
            //            {
            //                foreach (DataRow dr in sysPermissionFieldRows)
            //                {
            //                    if (dr["compare_mode"].ToString().Trim().ToUpper() == "CURRENT")
            //                    {
            //                        // Check to see if the compare_mode='current' permission_field compares the owned_by field against cno...
            //                        int compareValue = -1;
            //                        if ((int)dr["sys_table_field_id"] == _ownedByColumnID &&
            //                            int.TryParse(dr["compare_value"].ToString(), out compareValue) &&
            //                            compareValue.ToString() == _sharedUtils.UserCooperatorID)
            //                        {
            //                            if (sysGroupRow["selected_rows_only"] != null &&
            //                                sysGroupRow["selected_rows_only"] != DBNull.Value &&
            //                                (bool)sysGroupRow["selected_rows_only"]) sysGroupRow["selected_rows_only"] = false;
            //                        }
            //                        else
            //                        {
            //                            // Populate the listbox of selected pkeys...
            //                            PopulateRowRestrictionsListbox(dr["compare_value"].ToString());
            //                        }
            //                    }
            //                    if (dr["compare_mode"].ToString().Trim().ToUpper() == "PARENT")
            //                    {
            //                        if (sysGroupRow["include_child_tables"] != null &&
            //                            sysGroupRow["include_child_tables"] != DBNull.Value &&
            //                            !(bool)sysGroupRow["include_child_tables"]) sysGroupRow["include_child_tables"] = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    // Populate the row restriction radio buttons...
            //    if (sysGroupRow != null && sysGroupRow["selected_rows_only"] == DBNull.Value)
            //    {
            //        ux_radiobuttonSelectedRows.Checked = false;
            //        sysGroupRow["selected_rows_only"] = false;
            //    }
            //    else if ((bool)sysGroupRow["selected_rows_only"])
            //    {
            //        ux_radiobuttonSelectedRows.Checked = true;
            //    }
            //    else
            //    {
            //        ux_radiobuttonAllMyRows.Checked = true;
            //    }
            //    // Populate the 'include child table' checkbox...
            //    if (sysGroupRow != null && sysGroupRow["include_child_tables"] == DBNull.Value)
            //    {
            //        ux_checkboxIncludeChildTables.Checked = true;
            //        sysGroupRow["include_child_tables"] = true;
            //    }
            //    else
            //    {
            //        ux_checkboxIncludeChildTables.Checked = (bool)sysGroupRow["include_child_tables"];
            //    }
            //    // Refresh user interface
            //    ux_groupboxPermissions.Visible = true;
            //    ux_groupboxScope.Visible = true;
            //    ux_buttonEditUserListDone.PerformClick();
            //}
            //else
            //{
            //    if (_sysGroupUserMap != null) _sysGroupUserMap.DefaultView.RowFilter = "sys_group_id=-999999999";
            //    if (_sysGroupPermissionMap != null) _sysGroupPermissionMap.DefaultView.RowFilter = "sys_group_id=-999999999";
            //    if (_sysPermission != null) _sysPermission.DefaultView.RowFilter = "sys_permission_id=-999999999";
            //    ux_comboboxRead.SelectedValue = "I";
            //    ux_comboboxUpdate.SelectedValue = "I";
            //    ux_comboboxCreate.SelectedValue = "I";
            //    ux_comboboxDelete.SelectedValue = "I";
            //    // Refresh user interface
            //    ux_groupboxPermissions.Visible = false;
            //    ux_groupboxScope.Visible = false;
            //    ux_buttonEditUserListDone.PerformClick();
            //}
            #endregion
            // First - Save the changes for the policy that was active until just now...
            UpdateSysGroupUserMapTable(_currentSysGroup);
            UpdateSysGroupPermissionMapTable(_currentSysGroup);
            UpdateSysPermissionTable(_currentSysGroup);
            UpdateSysPermissionFieldTable(_currentSysGroup);

            // Now - Update the controls to reflect the settings for the new 'current sys_group'...
            if (ux_listboxPolicies.SelectedValue != null) _currentSysGroup = ux_listboxPolicies.SelectedValue.ToString();
            UpdatePermissionControls(_currentSysGroup);
            UpdateUserListControls(_currentSysGroup);
            UpdateRowRestrictionControls(_currentSysGroup);
            UpdateTableListControls(_currentSysGroup);
        }

        private void ux_checkboxIncludeChildTables_CheckedChanged(object sender, EventArgs e)
        {
            #region old_code...
            //// Update the columns for this group record
            //DataRow sysGroupRow = _sysGroup.Rows.Find(ux_listboxPolicies.SelectedValue);
            //if (sysGroupRow != null)
            //{
            //    sysGroupRow["include_child_tables"] = ux_checkboxIncludeChildTables.Checked;
            //}
            //// Build the list of tables this security policy is applied to...
            //ux_listboxTables.Items.Clear();
            //if (!string.IsNullOrEmpty(_tableName))
            //{
            //    // Add back in the active dataview's table name...
            //    ux_listboxTables.Items.Add(_tableName);
            //    if (ux_checkboxIncludeChildTables.Checked &&
            //        _sysTableFieldRelationship != null)
            //    {
            //        _sysTableFieldRelationship.DefaultView.RowFilter = "parent_table_name='" + _tableName + "' AND relationship_type_tag = 'OWNER_PARENT'";
            //        foreach (DataRowView drv in _sysTableFieldRelationship.DefaultView)
            //        {
            //            ux_listboxTables.Items.Add("  - " + drv["child_table_name"].ToString());
            //        }
            //    }
            //}
            #endregion
            // Build the list of tables this security policy is applied to...
            ux_listboxTables.Items.Clear();
            if (!string.IsNullOrEmpty(_tableName) &&
                _sysTableFieldRelationship != null)
            {
                // Add back in the active dataview's table name...
                ux_listboxTables.Items.Add(_tableName);
                if (ux_checkboxIncludeChildTables.Checked &&
                    _sysTableFieldRelationship != null)
                {
                    _sysTableFieldRelationship.DefaultView.RowFilter = "parent_table_name='" + _tableName + "' AND relationship_type_tag = 'OWNER_PARENT'";
                    foreach (DataRowView drv in _sysTableFieldRelationship.DefaultView)
                    {
                        ux_listboxTables.Items.Add("  - " + drv["child_table_name"].ToString());
                    }
                }
            }
        }

        private void ux_radiobuttonSelectedRows_CheckedChanged(object sender, EventArgs e)
        {
            #region old_code...
            //// Update the columns for this group record
            //DataRow sysGroupRow = _sysGroup.Rows.Find(ux_listboxPolicies.SelectedValue);
            //if (sysGroupRow != null)
            //{
            //    sysGroupRow["selected_rows_only"] = ux_radiobuttonSelectedRows.Checked;

            //    DataRow[] sysPermissionFieldRows = _sysPermissionField.Select("sys_permission_id=" + _sysGroupPermissionMap.DefaultView[0]["sys_permission_id"]);
            //    if (sysPermissionFieldRows != null)
            //    {
            //        foreach (DataRow dr in sysPermissionFieldRows)
            //        {
            //            if (dr["compare_mode"].ToString().Trim().ToUpper() == "CURRENT")
            //            {
            //                dr["sys_table_field_id"] = _pkeyColumnID;
            //                dr["compare_value"] = _pkeyCollection;
            //            }
            //            if (dr["compare_mode"].ToString().Trim().ToUpper() == "PARENT")
            //            {
            //                dr["parent_table_field_id"] = _pkeyColumnID;
            //                dr["parent_compare_value"] = _pkeyCollection;
            //            }
            //        }
            //    }

            //}
            //if (ux_radiobuttonSelectedRows.Checked)
            //{
            //    ux_listboxRowRestrictions.Enabled = true;
            //    ux_buttonAddSelectedRows.Enabled = true;
            //    ux_listboxRowRestrictions.Visible = true;
            //    ux_buttonAddSelectedRows.Visible = true;
            //}
            //else
            //{
            //    ux_listboxRowRestrictions.Enabled = false;
            //    ux_buttonAddSelectedRows.Enabled = false;
            //    ux_listboxRowRestrictions.Visible = false;
            //    ux_buttonAddSelectedRows.Visible = false;
            //}
            #endregion
            if (ux_radiobuttonSelectedRows.Checked)
            {
                ux_listboxRowRestrictions.Enabled = true;
                ux_buttonAddSelectedRows.Enabled = true;
                ux_listboxRowRestrictions.Visible = true;
                ux_buttonAddSelectedRows.Visible = true;
            }
            else
            {
                ux_listboxRowRestrictions.Enabled = false;
                ux_buttonAddSelectedRows.Enabled = false;
                ux_listboxRowRestrictions.Visible = false;
                ux_buttonAddSelectedRows.Visible = false;
            }
        }

        private void ux_listbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender.GetType() == typeof(ListBox))
            {
                // This code will make the item under the mouse cursor the selected item (regardless of which mouse button was clicked)...
                ListBox lb = (ListBox)sender;
                int i = e.Y / lb.ItemHeight;
                if (i > -1 &&
                    i < lb.Items.Count)
                {
                    lb.SelectedIndex = i;
                }
            }
        }

        private void ux_buttonMoveUsersRight_Click(object sender, EventArgs e)
        {
            #region old_code...
            //foreach (DataRowView incomingUser in ux_listboxAllUsers.SelectedItems)
            //{
            //    DataRow[] drs = _sysGroupUserMap.Select("sys_user_id=" + incomingUser["sys_user_id"].ToString() + " AND sys_group_id=" + ux_listboxPolicies.SelectedValue.ToString());
            //    if (drs.Length > 0)
            //    {
            //        foreach (DataRow dr in drs)
            //        {
            //            dr.Delete();
            //        }
            //    }

            //    // Add a new row...
            //    DataRow newGroupUserMap = _sysGroupUserMap.NewRow();
            //    newGroupUserMap["sys_group_id"] = ux_listboxPolicies.SelectedValue;
            //    newGroupUserMap["sys_user_id"] = incomingUser["sys_user_id"];
            //    newGroupUserMap["cooperator_name"] = incomingUser["cooperator_name"];
            //    _sysGroupUserMap.Rows.Add(newGroupUserMap);
            //}
            #endregion
            foreach (DataRowView user in ux_listboxAllUsers.SelectedItems)
            {
                if (!ux_listboxUsers.Items.Contains(user["cooperator_name"].ToString()))
                {
                    ux_listboxUsers.Items.Add(user["cooperator_name"].ToString());
                }
            }
            ux_listboxAllUsers.ClearSelected();
        }

        private void ux_buttonMoveUsersLeft_Click(object sender, EventArgs e)
        {
            #region old_code...
            //List<int> deletedIndices = new List<int>();
            //// Get the list of items...
            //foreach (int i in ux_listboxUsers.SelectedIndices)
            //{
            //    deletedIndices.Add(i);
            //}
            //// Now sort the indexes
            //deletedIndices.Sort();
            //deletedIndices.Reverse();
            //foreach (int i in deletedIndices)
            //{
            //    ((DataRowView)ux_listboxUsers.Items[i]).Delete();
            //}
            #endregion
            while (ux_listboxUsers.SelectedItems.Count > 0)
            {
                ux_listboxUsers.Items.Remove(ux_listboxUsers.SelectedItems[0]);
            }
        }

        private void ux_buttonEditUserList_Click(object sender, EventArgs e)
        {
            ux_panelTablesAndRows.Visible = false;
            ux_buttonEditUserList.Visible = false;
        }

        private void ux_buttonEditUserListDone_Click(object sender, EventArgs e)
        {
            ux_panelTablesAndRows.Visible = true;
            ux_buttonEditUserList.Visible = true;
        }

        private void ux_cmsListboxNewPolicy_Click(object sender, EventArgs e)
        {
            ux_buttonAddPolicy.PerformClick();
        }

        private void ux_cmsListboxRemovePolicy_Click(object sender, EventArgs e)
        {
            if (ux_listboxPolicies.SelectedValue != null)
            {
                #region old_code...
                //// Delete the associated group-permission map records...
                //foreach (DataRowView drv in _sysGroupPermissionMap.DefaultView)
                //{
                //    drv.Delete();
                //}
                //// Delete the associated group-user map records...
                //foreach (DataRowView drv in _sysGroupUserMap.DefaultView)
                //{
                //    drv.Delete();
                //}
                //// Delete the associated row filter records...
                //foreach (DataRowView drv in _sysPermissionField.DefaultView)
                //{
                //    drv.Delete();
                //}
                //// Delete the associated permission record (should only be one)...
                //foreach (DataRowView drv in _sysPermission.DefaultView)
                //{
                //    drv.Delete();
                //}
                //// Delete the associated group record...
                //DataRow sysGroupDataRow = _sysGroup.Rows.Find(ux_listboxPolicies.SelectedValue);
                //if (sysGroupDataRow != null) sysGroupDataRow.Delete();
                #endregion
                DataRow sysGroupDeleteRow = _sysGroup.Rows.Find(ux_listboxPolicies.SelectedValue);
                if (sysGroupDeleteRow != null)
                {
                    string sysGroupID = sysGroupDeleteRow["sys_group_id"].ToString();

                    // STEP 1 - Delete the associated group-user map records...
                    DataRow[] sysGroupUserMapRows = _sysGroupUserMap.Select("sys_group_id=" + sysGroupID);
                    foreach (DataRow sysGroupUserMapRow in sysGroupUserMapRows)
                    {
                        sysGroupUserMapRow.Delete();
                    }
                    // STEP 2 - Delete the associated group-permission map records...
                    DataRow[] sysGroupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + sysGroupID);
                    foreach (DataRow sysGroupPermissionMapRow in sysGroupPermissionMapRows)
                    {
                        // Before deleting this group-permission map record...
                        // First - delete the permission_field records
                        DataRow[] permissionFieldRows = _sysPermissionField.Select("sys_permission_id=" + sysGroupPermissionMapRow["sys_permission_id"].ToString());
                        foreach (DataRow permissionFieldRow in permissionFieldRows)
                        {
                            permissionFieldRow.Delete();
                        }
                        // Next - delete the permission records
                        DataRow[] permissionRows = _sysPermission.Select("sys_permission_id=" + sysGroupPermissionMapRow["sys_permission_id"].ToString());
                        foreach (DataRow permissionRow in permissionRows)
                        {
                            permissionRow.Delete();
                        }
                        // Finally - delete the group-permission map record...
                        sysGroupPermissionMapRow.Delete();
                    }
                    // STEP 3 - Delete the associated group record...
                    sysGroupDeleteRow.Delete();
                }
            }
        }

        private void ux_cmsListboxRenamePolicy_Click(object sender, EventArgs e)
        {
            if (ux_listboxPolicies.SelectedValue != null)
            {
                // Find the associated group record...
                DataRow sysGroupDataRow = _sysGroup.Rows.Find(ux_listboxPolicies.SelectedValue);
                if (sysGroupDataRow != null)
                {
                    ux_textboxRenameGroup.AutoSize = false;
                    ux_textboxRenameGroup.Size = ux_listboxPolicies.GetItemRectangle(ux_listboxPolicies.SelectedIndex).Size;
                    ux_textboxRenameGroup.Height += 3;
                    ux_textboxRenameGroup.Location = new Point(0, (ux_listboxPolicies.SelectedIndex * ux_listboxPolicies.ItemHeight) - 1);
                    ux_textboxRenameGroup.Text = sysGroupDataRow["group_tag"].ToString();
                    ux_listboxPolicies.Controls.Add(ux_textboxRenameGroup);
                    ux_textboxRenameGroup.Visible = true;
                    ux_textboxRenameGroup.Focus();
                    ux_textboxRenameGroup.SelectAll();
                }
            }
        }

        private void ux_textboxRenameGroup_Leave(object sender, EventArgs e)
        {
            if (ux_listboxPolicies.SelectedValue != null)
            {
                // Find the associated group record...
                DataRow sysGroupDataRow = _sysGroup.Rows.Find(ux_listboxPolicies.SelectedValue);
                if (sysGroupDataRow != null)
                {
                    sysGroupDataRow["group_tag"] = ux_textboxRenameGroup.Text;
                    // Now - Find the associated permission record(s)...
                    DataRow[] sysGroupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + ux_listboxPolicies.SelectedValue.ToString());
                    foreach (DataRow sysGroupPermissionMapRow in sysGroupPermissionMapRows)
                    {
                        DataRow[] sysPermissionRows = _sysPermission.Select("sys_permission_id=" + sysGroupPermissionMapRow["sys_permission_id"].ToString());
                        foreach (DataRow sysPermissionRow in sysPermissionRows)
                        {
                            sysPermissionRow["permission_tag"] = "SW - " + _sharedUtils.Username + " - " + sysGroupDataRow["group_tag"].ToString();
                        }
                    }
                }
            }
            ux_textboxRenameGroup.Visible = false;
        }

        private void ux_textboxRenameGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter )
            {
                ux_listboxPolicies.Focus();
            }
        }

        private void ux_buttonAddSelectedRows_Click(object sender, EventArgs e)
        {
            PopulateRowRestrictionsListbox(_pkeyCollection);
        }

        private void PopulateRowRestrictionsListbox(string pkeyCollection)
        {
            #region old_code...
            //string[] pkeys = pkeyCollection.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (string pkey in pkeys)
            //{
            //    string displayMember = _sharedUtils.GetLookupDisplayMember(_tableName + "_lookup", pkey, "", pkey);
            //    string sysPermissionID = _sysPermission.DefaultView[0]["sys_permission_id"].ToString();
            //    DataRow dr = _sysPermissionFieldCollection.Rows.Find(new object[] { pkey, sysPermissionID });
            //    if (dr == null) _sysPermissionFieldCollection.Rows.Add(new object[] { pkey, sysPermissionID, displayMember });
            //}
            #endregion
            string[] pkeys = pkeyCollection.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pkey in pkeys)
            {
                string displayMember = _sharedUtils.GetLookupDisplayMember(_tableName + "_lookup", pkey, "", pkey);
                if (!ux_listboxRowRestrictions.Items.Contains(displayMember)) ux_listboxRowRestrictions.Items.Add(displayMember);
            }
        }

        private void LoadDBTables(string cooperatorID)
        {
            DataSet ds = new DataSet();

            // Get the sys_user table...
            ds = _sharedUtils.GetWebServiceData("get_sys_user", "", 0, 0);
            if (ds.Tables.Contains("get_sys_user"))
            {
                _sysUser = ds.Tables["get_sys_user"].Copy();
                // Build a display name for the cooperator_id...
                _sysUser.Columns.Add("cooperator_name", typeof(string));
                foreach (DataRow dr in _sysUser.Rows)
                {
                    dr["cooperator_name"] = _sharedUtils.GetLookupDisplayMember("cooperator_lookup", dr["cooperator_id"].ToString(), "", dr["cooperator_id"].ToString());
                }
                // Accept the changes to clean out all of the table modifications...
                _sysUser.AcceptChanges();
            }
            else
            {
//MessageBox.Show("Error retrieving the get_sys_user dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving the {0} dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SecurityWizard_LoadDBTablesMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, "get_sys_user");
ggMessageBox.ShowDialog();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            // Get the sys_group_permission_map table...
            ds = _sharedUtils.GetWebServiceData("get_sys_group_permission_map", ":cooperatorid=" + cooperatorID, 0, 0);
            if (ds.Tables.Contains("get_sys_group_permission_map"))
            {
                _sysGroupPermissionMap = ds.Tables["get_sys_group_permission_map"].Copy();
            }
            else
            {
//MessageBox.Show("Error retrieving the get_sys_group_permission_map dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving the {0} dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SecurityWizard_LoadDBTablesMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, "get_sys_group_permission_map");
ggMessageBox.ShowDialog();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            // Get the sys_group_user_map table...
            ds = _sharedUtils.GetWebServiceData("get_sys_group_user_map", ":cooperatorid=" + cooperatorID, 0, 0);
            if (ds.Tables.Contains("get_sys_group_user_map"))
            {
                _sysGroupUserMap = ds.Tables["get_sys_group_user_map"].Copy();
            }
            else
            {
//MessageBox.Show("Error retrieving the get_sys_group_user_map dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving the {0} dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SecurityWizard_LoadDBTablesMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, "get_sys_group_user_map");
ggMessageBox.ShowDialog();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            // Get the sys_permission_codes...
            _sysPermissionCode = _sharedUtils.GetLocalData("SELECT * FROM code_value_lookup WHERE group_name='sys_permission_code'", "");
            if (_sysPermissionCode == null)
            {
                _sysPermissionCode = new DataTable("sys_permission_code");
                _sysPermissionCode.Columns.Add("display_member", typeof(string));
                _sysPermissionCode.Columns.Add("value_member", typeof(string));
            }
            if (_sysPermissionCode.Rows.Count == 0)
            {
                DataRow newSysPermissionCodeRow = _sysPermissionCode.NewRow();
                newSysPermissionCodeRow["display_member"] = "Inherit";
                newSysPermissionCodeRow["value_member"] = "I";
                _sysPermissionCode.Rows.Add(newSysPermissionCodeRow);
                newSysPermissionCodeRow = _sysPermissionCode.NewRow();
                newSysPermissionCodeRow["display_member"] = "Deny";
                newSysPermissionCodeRow["value_member"] = "D";
                _sysPermissionCode.Rows.Add(newSysPermissionCodeRow);
                newSysPermissionCodeRow = _sysPermissionCode.NewRow();
                newSysPermissionCodeRow["display_member"] = "Allow";
                newSysPermissionCodeRow["value_member"] = "A";
                _sysPermissionCode.Rows.Add(newSysPermissionCodeRow);
            }
            // Wire up the permission comboboxes valid responses...
            ux_comboboxRead.DisplayMember = "display_member";
            ux_comboboxRead.ValueMember = "value_member";
            ux_comboboxRead.DataSource = _sysPermissionCode.Copy();
            ux_comboboxUpdate.DisplayMember = "display_member";
            ux_comboboxUpdate.ValueMember = "value_member";
            ux_comboboxUpdate.DataSource = _sysPermissionCode.Copy();
            ux_comboboxCreate.DisplayMember = "display_member";
            ux_comboboxCreate.ValueMember = "value_member";
            ux_comboboxCreate.DataSource = _sysPermissionCode.Copy();
            ux_comboboxDelete.DisplayMember = "display_member";
            ux_comboboxDelete.ValueMember = "value_member";
            ux_comboboxDelete.DataSource = _sysPermissionCode.Copy();

            // Get the sys_permission table...
            ds = _sharedUtils.GetWebServiceData("get_sys_permission", ":cooperatorid=" + cooperatorID, 0, 0);
            if (ds.Tables.Contains("get_sys_permission"))
            {
                _sysPermission = ds.Tables["get_sys_permission"].Copy();
            }
            else
            {
//MessageBox.Show("Error retrieving the get_sys_permission dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving the {0} dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SecurityWizard_LoadDBTablesMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, "get_sys_permission");
ggMessageBox.ShowDialog();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            // Get the sys_permission_field table...
            ds = _sharedUtils.GetWebServiceData("get_sys_permission_field", ":cooperatorid=" + cooperatorID, 0, 0);
            if (ds.Tables.Contains("get_sys_permission_field"))
            {
                _sysPermissionField = ds.Tables["get_sys_permission_field"].Copy();
            }
            else
            {
//MessageBox.Show("Error retrieving the get_sys_permission_field dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving the {0} dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SecurityWizard_LoadDBTablesMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, "get_sys_permission_field");
ggMessageBox.ShowDialog();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            // Get the sys_table_field_relationship table...
            ds = _sharedUtils.GetWebServiceData("sys_table_field_relationship", "", 0, 0);
            if (ds.Tables.Contains("sys_table_field_relationship"))
            {
                _sysTableFieldRelationship = ds.Tables["sys_table_field_relationship"].Copy();
                //_sysTableFieldRelationship.DefaultView.RowFilter = "(child_field_name='" + _pkeyColumnName + "' AND child_field_purpose='PRIMARY_KEY') OR parent_field_name='" + _pkeyColumnName + "'";
            }
            else
            {
//MessageBox.Show("Error retrieving the sys_table_field_relationship dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving the {0} dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SecurityWizard_LoadDBTablesMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, "sys_table_field_relationship");
ggMessageBox.ShowDialog();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }

            // Get the 'owned_by' table_field_ID (using the table name and 'owned_by' field name in the table_field_relationship table)...
            DataRow[] drsSysTableFieldRelationship = _sysTableFieldRelationship.Select("child_table_name='" + _tableName + "' AND child_field_name='owned_by' AND child_field_purpose='AUTO_ASSIGN_OWN'");
            if (drsSysTableFieldRelationship.Length > 0)
            {
                // Populate the _ownedByColumnID variables...
                _ownedByColumnID = drsSysTableFieldRelationship[0]["child_table_field_id"].ToString();
            }

            // Get the table_ID and pkey table_field_ID (using the table name and field name in the table_field_relationship table)...
            drsSysTableFieldRelationship = _sysTableFieldRelationship.Select("child_table_name='" + _tableName + "' AND child_field_name='" + _pkeyColumnName + "' AND child_field_purpose='PRIMARY_KEY'");
            if (drsSysTableFieldRelationship.Length > 0)
            {
                // Populate the _tableID and pkeyColumnID variables...
                _tableID = drsSysTableFieldRelationship[0]["child_table_id"].ToString();
                _pkeyColumnID = drsSysTableFieldRelationship[0]["child_table_field_id"].ToString();
            }

            // Leave this table to last (so that it will filter the other lists properly when it gets populated - triggers a SelectedIndexChanged event)...
            // Get the sys_group table...
            ds = _sharedUtils.GetWebServiceData("get_sys_group", ":cooperatorid=" + cooperatorID, 0, 0);
            if (ds.Tables.Contains("get_sys_group"))
            {
                _sysGroup = ds.Tables["get_sys_group"].Copy();

                // Filter out all of the groups that do not contain a permission that uses the currentTableID...
                string validSysGroups = "null";
                if (_tableID != "-1" &&
                    _pkeyColumnID != "-1" &&
                    _ownedByColumnID != "-1")
                {
                    foreach (DataRow sysGroupRow in _sysGroup.Rows)
                    {
                        bool foundTableInGroupPolicy = false;
                        _sysGroupPermissionMap.DefaultView.RowFilter = "sys_group_id=" + sysGroupRow["sys_group_id"];
                        foreach (DataRowView sysGroupPermissionRow in _sysGroupPermissionMap.DefaultView)
                        {
                            DataRow sysPermissionRow = _sysPermission.Rows.Find(sysGroupPermissionRow["sys_permission_id"]);
                            if (sysPermissionRow != null &&
                                sysPermissionRow["sys_table_id"].ToString() == _tableID) foundTableInGroupPolicy = true;
                        }
                        // If this is a valid group - add it to the list...
                        if (foundTableInGroupPolicy) validSysGroups += "," + sysGroupRow["sys_group_id"].ToString();
                    }
                }
                _sysGroup.DefaultView.RowFilter = "sys_group_id IN (" + validSysGroups.TrimEnd(',') + ")";
            }
            else
            {
//MessageBox.Show("Error retrieving the get_sys_group dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("Error retrieving the {0} dataview", "Security Wizard Error", MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "SecurityWizard_LoadDBTablesMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, "get_sys_group");
ggMessageBox.ShowDialog();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void UpdateTableListControls(string groupID)
        {
            // Reset the 'include child tables' checkbox...
            ux_checkboxIncludeChildTables.Checked = false;
            // Detect if the 'include child tables' checkbox should be checked...
            // First get the permission row...
            if (!string.IsNullOrEmpty(groupID) &&
                _sysGroupPermissionMap != null)
            {
                DataRow[] sysGroupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + groupID);
                // There should only be one row - so use the first one...
                if (sysGroupPermissionMapRows.Length > 0)
                {
                    // Get the permission row...
                    DataRow sysPermissionRow = _sysPermission.Rows.Find(sysGroupPermissionMapRows[0]["sys_permission_id"]);
                    if (sysPermissionRow != null)
                    {
                        // Now get the permission_field row...
                        if (!string.IsNullOrEmpty(sysPermissionRow["sys_permission_id"].ToString()) &&
                            _sysPermissionField != null)
                        {
                            DataRow[] drs = _sysPermissionField.Select("sys_permission_id=" + sysPermissionRow["sys_permission_id"].ToString());
                            foreach (DataRow dr in drs)
                            {
                                // See if one of the rows in this collection has a compare_mode='parent' and
                                // if so check the 'include child tables' checkbox...
                                if (dr["compare_mode"].ToString().Trim().ToUpper() == "PARENT")
                                {
                                    ux_checkboxIncludeChildTables.Checked = true;
                                }
                            }
                        }
                    }
                }
            }

            // Build the list of tables this security policy is applied to...
            ux_listboxTables.Items.Clear();
            if (!string.IsNullOrEmpty(_tableName) &&
                _sysTableFieldRelationship != null)
            {
                // Add back in the active dataview's table name...
                ux_listboxTables.Items.Add(_tableName);
                if (ux_checkboxIncludeChildTables.Checked &&
                    _sysTableFieldRelationship != null)
                {
                    _sysTableFieldRelationship.DefaultView.RowFilter = "parent_table_name='" + _tableName + "' AND relationship_type_tag = 'OWNER_PARENT'";
                    foreach (DataRowView drv in _sysTableFieldRelationship.DefaultView)
                    {
                        ux_listboxTables.Items.Add("  - " + drv["child_table_name"].ToString());
                    }
                }
            }
        }

        private void UpdatePermissionControls(string groupID)
        {
            if (!string.IsNullOrEmpty(groupID) &&
                _sysGroupPermissionMap != null)
            {
                DataRow[] sysGroupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + groupID);
                // There should only be one row - so use the first one...
                if (sysGroupPermissionMapRows.Length > 0)
                {
                    // Get the permission row...
                    DataRow sysPermissionRow = _sysPermission.Rows.Find(sysGroupPermissionMapRows[0]["sys_permission_id"]);
                    if (sysPermissionRow != null)
                    {
                        // Populate the comboboxes with data from the permission row...
                        ux_comboboxRead.SelectedValue = sysPermissionRow["read_permission"];
                        ux_comboboxUpdate.SelectedValue = sysPermissionRow["update_permission"];
                        ux_comboboxCreate.SelectedValue = sysPermissionRow["create_permission"];
                        ux_comboboxDelete.SelectedValue = sysPermissionRow["delete_permission"];
                    }
                }
            }
        }

        private void UpdateUserListControls(string groupID)
        {
            ux_listboxUsers.Items.Clear();
            if (!string.IsNullOrEmpty(groupID) &&
                _sysGroupPermissionMap != null)
            {
                DataRow[] sysGroupUserMapRows = _sysGroupUserMap.Select("sys_group_id=" + groupID);
                foreach (DataRow sysGroupUserMapRow in sysGroupUserMapRows)
                {
                    DataRow userRow = _sysUser.Rows.Find(sysGroupUserMapRow["sys_user_id"]);
                    string userName = "";
                    if (userRow != null) userName = userRow["cooperator_name"].ToString();
                    if(!string.IsNullOrEmpty(userName)) ux_listboxUsers.Items.Add(userName);
                }
            }
        }

        private void UpdateRowRestrictionControls(string groupID)
        {
            // NOTE1: The sys_table_field_id is used to detect which radio button to check...
            // NOTE2: The value stored in the compare_value field is left unchanged from
            //        original (just in case the user orginally had a row restriction, changed it 
            //        to all owned rows and then wanted to change back to row restriction again - the 
            //        original list of rows will still be there - but remember the compare_value must
            //        be changed to the cooperatorID during the Save button click event)

            // First get the permission row...
            if (!string.IsNullOrEmpty(groupID) &&
                _sysGroupPermissionMap != null)
            {
                DataRow[] sysGroupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + groupID);
                // There should only be one row - so use the first one...
                if (sysGroupPermissionMapRows.Length > 0)
                {
                    // Get the permission row...
                    DataRow sysPermissionRow = _sysPermission.Rows.Find(sysGroupPermissionMapRows[0]["sys_permission_id"]);
                    if (sysPermissionRow != null)
                    {
                        // Now go get the permission_field row(s)...
                        if (!string.IsNullOrEmpty(sysPermissionRow["sys_permission_id"].ToString()) &&
                            _sysPermissionField != null)
                        {
                            DataRow[] drs = _sysPermissionField.Select("sys_permission_id=" + sysPermissionRow["sys_permission_id"].ToString());
                            foreach (DataRow dr in drs)
                            {
                                // Since the compare_mode='current' will always be created by the wizard, but the
                                // compare_mode='parent' is only present if child tables are included - use 'current' 
                                // for evaluating what row restriction mode the user wants...
                                if (dr["compare_mode"].ToString().Trim().ToUpper() == "CURRENT")
                                {
                                    if (dr["sys_table_field_id"].ToString() == _ownedByColumnID &&
                                        dr["compare_value"].ToString() == _cooperatorID)
                                    {
                                        ux_radiobuttonAllMyRows.Checked = true;
                                        // Clear the old list...
                                        ux_listboxRowRestrictions.Items.Clear();
                                    }
                                    else
                                    {
                                        ux_radiobuttonSelectedRows.Checked = true;
                                        // Clear the old list...
                                        ux_listboxRowRestrictions.Items.Clear();
                                        // Add dataview rows to listbox...
                                        PopulateRowRestrictionsListbox(dr["compare_value"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (ux_radiobuttonSelectedRows.Checked)
            {
                ux_listboxRowRestrictions.Enabled = true;
                ux_buttonAddSelectedRows.Enabled = true;
                ux_listboxRowRestrictions.Visible = true;
                ux_buttonAddSelectedRows.Visible = true;
            }
            else
            {
                ux_listboxRowRestrictions.Enabled = false;
                ux_buttonAddSelectedRows.Enabled = false;
                ux_listboxRowRestrictions.Visible = false;
                ux_buttonAddSelectedRows.Visible = false;
            }
        }

        private void UpdateSysPermissionTable(string groupID)
        {
            if (!string.IsNullOrEmpty(groupID) &&
               _sysGroupPermissionMap != null)
            {
                DataRow[] sysGroupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + groupID);
                // There should only be one row - so use the first one...
                if (sysGroupPermissionMapRows.Length > 0)
                {
                    // Get the permission row...
                    DataRow sysPermissionRow = _sysPermission.Rows.Find(sysGroupPermissionMapRows[0]["sys_permission_id"]);
                    if (sysPermissionRow != null)
                    {
                        sysPermissionRow["read_permission"] = ux_comboboxRead.SelectedValue;
                        sysPermissionRow["update_permission"] = ux_comboboxUpdate.SelectedValue;
                        sysPermissionRow["create_permission"] = ux_comboboxCreate.SelectedValue;
                        sysPermissionRow["delete_permission"] = ux_comboboxDelete.SelectedValue;
                    }
                }
            }
        }

        private void UpdateSysPermissionFieldTable(string groupID)
        {
            DataRow currentCompareModeRow = null;
            DataRow parentCompareModeRow = null;
            DataRow sysPermissionRow = null;
            string compare_value = "";
            string sys_table_field_id = "";

            // First - Build the compare_value string and the sys_table_field_id/parent_table_field_id string
            // Determine the compare_value and compare field settings (based on user gui controls)...
            if (ux_radiobuttonAllMyRows.Checked)
            {
                // The user wants the security applied to all of his/her owned rows...
                sys_table_field_id = _ownedByColumnID;
                compare_value = _cooperatorID;
            }
            else
            {
                // The user wants the security applied to a group of specific rows in the dataview - so build the list...
                sys_table_field_id = _pkeyColumnID;
                foreach (string row in ux_listboxRowRestrictions.Items)
                {
                    string pkey = _sharedUtils.GetLookupValueMember(null, _tableName + "_lookup", row, "", row);
                    if (pkey != row) compare_value += pkey + ",";
                }
                compare_value = compare_value.TrimEnd(',');
            }

            // Now - Get the permission row from the group-permission map...
            if (!string.IsNullOrEmpty(groupID) &&
                _sysGroupPermissionMap != null)
            {
                DataRow[] sysGroupPermissionMapRows = _sysGroupPermissionMap.Select("sys_group_id=" + groupID);
                // There should only be one row - so use the first one...
                if (sysGroupPermissionMapRows.Length > 0)
                {
                    // Get the permission row...
                    sysPermissionRow = _sysPermission.Rows.Find(sysGroupPermissionMapRows[0]["sys_permission_id"]);
                    if (sysPermissionRow != null)
                    {
                        // Now go get the permission_field row(s)...
                        if (!string.IsNullOrEmpty(sysPermissionRow["sys_permission_id"].ToString()) &&
                            _sysPermissionField != null)
                        {
                            DataRow[] drs = _sysPermissionField.Select("sys_permission_id=" + sysPermissionRow["sys_permission_id"].ToString());
                            foreach (DataRow dr in drs)
                            {
                                if (dr["compare_mode"].ToString().Trim().ToUpper() == "CURRENT") currentCompareModeRow = dr;
                                else if (dr["compare_mode"].ToString().Trim().ToUpper() == "PARENT") parentCompareModeRow = dr;
                            }
                        }
                        // Update the 'current' compare_mode record...
                        if (currentCompareModeRow == null)
                        {
                            // For some reason there is no existing 'current' compare_mode permission_field record yet - so create one...
                            DataRow newPermissionFieldCurrent = _sysPermissionField.NewRow();
                            newPermissionFieldCurrent["sys_permission_id"] = sysPermissionRow["sys_permission_id"];
                            newPermissionFieldCurrent["sys_table_field_id"] = sys_table_field_id;
                            newPermissionFieldCurrent["field_type"] = "INTEGER";
                            newPermissionFieldCurrent["compare_operator"] = "in";
                            newPermissionFieldCurrent["compare_value"] = compare_value;
                            newPermissionFieldCurrent["compare_mode"] = "current";
                            _sysPermissionField.Rows.Add(newPermissionFieldCurrent);
                        }
                        else
                        {
                            currentCompareModeRow["sys_table_field_id"] = sys_table_field_id;
                            currentCompareModeRow["compare_value"] = compare_value;
                        }

                        // Determine what to do with the parent compare_mode record...
                        if (ux_checkboxIncludeChildTables.Checked)
                        {
                            if (parentCompareModeRow == null)
                            {
                                // The user wants to include child tables in the security policy but there 
                                // is no existing 'parent' compare_mode permission_field record yet - so create one...
                                DataRow newPermissionFieldParent = _sysPermissionField.NewRow();
                                newPermissionFieldParent["sys_permission_id"] = sysPermissionRow["sys_permission_id"];
                                newPermissionFieldParent["parent_table_field_id"] = sys_table_field_id;
                                newPermissionFieldParent["parent_field_type"] = "INTEGER";
                                newPermissionFieldParent["parent_compare_operator"] = "in";
                                newPermissionFieldParent["parent_compare_value"] = compare_value;
                                newPermissionFieldParent["compare_mode"] = "parent";
                                _sysPermissionField.Rows.Add(newPermissionFieldParent);
                            }
                            else
                            {
                                parentCompareModeRow["parent_table_field_id"] = sys_table_field_id;
                                parentCompareModeRow["parent_compare_value"] = compare_value;
                            }
                        }
                        else if (parentCompareModeRow != null)
                        {
                            // The user does not want child table included, but the permission_field record
                            // exist - so delete the record...
                            parentCompareModeRow.Delete();
                        }
                    }
                }
            }
        }

        private void UpdateSysGroupPermissionMapTable(string groupID)
        {
            // I'm not sure what needs to be updated here - right now this is just a place holder...
        }

        private void UpdateSysGroupUserMapTable(string groupID)
        {
            if (!string.IsNullOrEmpty(groupID))
            {
                // Get all users mapped to this group policy...
                DataRow[] sysGroupUserMapRows = _sysGroupUserMap.Select("sys_group_id=" + groupID);

                // First - Iterate through all of the users associated with this policy for ones that
                // need to be deleted from the map table...
                foreach (DataRow sysGroupUserMapRow in sysGroupUserMapRows)
                {
                    // Get the cooperator_name from the _sysUser table...
                    string cooperatorName = _sysUser.Rows.Find(sysGroupUserMapRow["sys_user_id"])["cooperator_name"].ToString();
                    if (!string.IsNullOrEmpty(cooperatorName) &&
                        !ux_listboxUsers.Items.Contains(cooperatorName))
                    {
                        // There is a record in the sysGroupUserMap table that has no match in the current 
                        // list of users in the ux_listboxUsers control - so delete it from sysGroupUserMap...
                        sysGroupUserMapRow.Delete();
                    }
                }

                // Now - Iterate through all of the users in the listbox to see if any need to be are missing
                // in the current group_user mapping - thus need to be added to the map table...
                foreach (string user in ux_listboxUsers.Items)
                {
                    bool foundUser = false;
                    // Get the user_id from a search of the _sysUser table...
                    DataRow[] sysUserRows = _sysUser.Select("cooperator_name='" + user.Replace("'", "''") + "'");
                    foreach (DataRow sysUserRow in sysUserRows)
                    {
                        foreach (DataRow sysGroupUserMapRow in sysGroupUserMapRows)
                        {
                            if (sysGroupUserMapRow.RowState != DataRowState.Deleted &&
                                sysUserRow["sys_user_id"].ToString() == sysGroupUserMapRow["sys_user_id"].ToString()) foundUser = true;
                        }
                    }
                    // If the user was not found in the group_user map table - create one now...
                    if (!foundUser && sysUserRows.Length > 0)
                    {
                        DataRow newSysGroupUserMapRow = _sysGroupUserMap.NewRow();
                        newSysGroupUserMapRow["sys_group_id"] = groupID;
                        newSysGroupUserMapRow["sys_user_id"] = sysUserRows[0]["sys_user_id"];
                        _sysGroupUserMap.Rows.Add(newSysGroupUserMapRow);
                    }
                }
            }
        }
    }
}
