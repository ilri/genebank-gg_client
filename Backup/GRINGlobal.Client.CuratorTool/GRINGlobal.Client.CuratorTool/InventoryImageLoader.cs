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
    public partial class InventoryImageLoader : Form
    {
        string[] _filePaths;
        //WebServices _webServices;
        SharedUtils _sharedUtils;
        DataGridView _imageDGV;
        string _inventoryIDs;
        string _commonFilePath;

        public InventoryImageLoader(SharedUtils sharedUtils, DataGridView imageDGV, string[] filePaths, string inventoryIDs)
        {
            InitializeComponent();

            _sharedUtils = sharedUtils;
            _imageDGV = imageDGV;
            _filePaths = filePaths;
            _inventoryIDs = inventoryIDs;
        }

        private void InventoryImageLoader_Load(object sender, EventArgs e)
        {
//////DataSet ds = _webServices.GetData("get_inventory_attach", ":inventoryid=" + _inventoryIDs.ToString() + "; :accessionid=; :orderrequestid=", 0, 0);
////DataSet ds = _webServices.GetData("get_inventory_attach", ":inventoryid=0; :accessionid=; :orderrequestid=", 0, 0);
//DataSet ds = _sharedUtils.GetWebServiceData("get_inventory_attach", ":inventoryid=" + _inventoryIDs.ToString() + "; :accessionid=; :orderrequestid=", 0, 0);
//DataSet ds = _sharedUtils.GetWebServiceData("get_inventory_attach", ":inventoryid=0; :accessionid=; :orderrequestid=", 0, 0);
//if (ds.Tables.Contains("get_inventory_attach"))
            if (_imageDGV.DataSource.GetType() == typeof(BindingSource))
            {
//DataTable dt = ds.Tables["get_inventory_attach"];
                DataTable dt = (DataTable)((BindingSource)_imageDGV.DataSource).DataSource;
                dt.Columns.Add("image_file_physical_path");
                dt.PrimaryKey[0].AutoIncrement = true;
                dt.PrimaryKey[0].AutoIncrementSeed = -1;
                dt.PrimaryKey[0].AutoIncrementStep = -1;

                // Process each selected row in the DataGridView...
                foreach (string inventoryID in _inventoryIDs.Split(','))
                {
                    // Process each filePath from the FolderDrop...
                    foreach (string filePath in _filePaths)
                    {
                        // Build the list of images in each filePath passed in (the filePath could be
                        // a single image file or a directory of image files...
                        System.Collections.Specialized.StringDictionary imageDictionary = new System.Collections.Specialized.StringDictionary();
                        BuildImageList(imageDictionary, filePath);
                        foreach (string physicalPath in imageDictionary.Keys)
                        {
                            DataRow newInventoryImageRow = dt.NewRow();
                            newInventoryImageRow["inventory_id"] = inventoryID;
//newInventoryImageRow["virtual_path"] = virtualPath;
//newInventoryImageRow["thumbnail_virtual_path"] = virtualPath.Insert(virtualPath.LastIndexOf('.'), "_thumbnail");
                            newInventoryImageRow["virtual_path"] = imageDictionary[physicalPath];
                            newInventoryImageRow["thumbnail_virtual_path"] = imageDictionary[physicalPath].Insert(imageDictionary[physicalPath].LastIndexOf('.'), "_thumbnail");
                            //newInventoryImageRow["category"] = "";
                            //newInventoryImageRow["status_code"] = "";
                            //newInventoryImageRow["note"] = "";
                            newInventoryImageRow["image_file_physical_path"] = physicalPath;
                            dt.Rows.Add(newInventoryImageRow);
                        }
                    }
                }

// Wire up the DGV to the datatable...
//ux_datagridviewImages.DataSource = dt;
                _imageDGV.Location = ux_datagridviewImages.Location;
                _imageDGV.Size = ux_datagridviewImages.Size;
                _imageDGV.Anchor = ux_datagridviewImages.Anchor;
                _imageDGV.AllowUserToAddRows = false;
                ux_datagridviewImages.Dispose();
                this.Controls.Add(_imageDGV);
                
                // Hide all of the ReadOnly DGV columns...
                foreach (DataGridViewColumn dgvc in _imageDGV.Columns)
                {
                    if (dgvc.ReadOnly) dgvc.Visible = false;
                }
                // Set the thumbnail directory path to ReadOnly (but leave it visible)...
                _imageDGV.Columns["thumbnail_virtual_path"].ReadOnly = true;
                _imageDGV.Columns["thumbnail_virtual_path"].DefaultCellStyle.BackColor = Color.LightGray;

                // Add the new column for the image file's physical path...
                _imageDGV.Columns.Add("image_file_physical_path", "Local Directory");
                _imageDGV.Columns["image_file_physical_path"].DataPropertyName = "image_file_physical_path";
                _imageDGV.Columns["image_file_physical_path"].Visible = false;
                
                _commonFilePath = FindCommonFilePath(dt);
                ux_textboxCommonFilePath.Text = _commonFilePath;

            }
_sharedUtils.UpdateControls(this.Controls, this.Name);
        }

        private string FindCommonFilePath(DataTable dt)
        {
            bool foundCommonFilePath = false;
            int max = dt.Rows[0]["virtual_path"].ToString().Length;
            string commonFilePath = dt.Rows[0]["virtual_path"].ToString();
            while (!foundCommonFilePath && max > 0)
            {
                // Start out assuming the common path has been found...
                foundCommonFilePath = true;
                max--;
                foreach (DataRow dr in dt.Rows)
                {
                    string junk = commonFilePath.Substring(0, max);
                    // Unless proven wrong...
                    //if (!dgvr.Cells["virtual_path"].Value.ToString().Contains(ux_datagridviewImages.Rows[0].Cells["virtual_path"].Value.ToString().Substring(0, max))) foundCommonFilePath = false;
                    //if (!dr["virtual_path"].ToString().Contains(commonFilePath.Substring(0, max))) foundCommonFilePath = false;
                    if (!dr["virtual_path"].ToString().Contains(junk))
                    {
                        foundCommonFilePath = false;
                        break;
                    }
                }
            }
            max = commonFilePath.Substring(0, max).LastIndexOf("\\");
            return commonFilePath.Substring(0, max);
        }

        private void BuildImageList(System.Collections.Specialized.StringDictionary imageList, string fullPath)
        {

            // Check to see if this is a directory and if so recursively process all objects in it...
            if (System.IO.Directory.Exists(fullPath))
            {
                // First process all of the files in this directory...
                foreach (string fp in System.IO.Directory.GetFiles(fullPath)) BuildImageList(imageList, fp);
                // Now process all of the subdirectories in this directory...
                foreach (string fp in System.IO.Directory.GetDirectories(fullPath)) BuildImageList(imageList, fp);
            }
            else if(System.IO.File.Exists(fullPath))
            {
                // Make sure this file is a valid image file and if so process it...
                try
                {
                    // If the image has dimension add it to the list...
                    if (!Image.FromFile(fullPath).PhysicalDimension.IsEmpty)
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(fullPath);
                        string virtualPath = fi.FullName.Replace(fi.Directory.Root.FullName, "");
                        if (!virtualPath.StartsWith("\\")) virtualPath = virtualPath.Insert(0, "\\");
//if (!imageList.ContainsKey(virtualPath)) imageList.Add(virtualPath, fi.FullName);
                        if (!imageList.ContainsKey(virtualPath)) imageList.Add(fi.FullName, virtualPath);
                        //filePath = filePath.Insert(0, "~\\uploads\\images");
                        //string junk = _webService.UploadImage(username, password, filePath, System.IO.File.ReadAllBytes(fullPath), true, true);
                        //byte[] imgBytes = webService.DownloadImage(filePath);
                        //System.IO.File.WriteAllBytes(@"C:\temp\test.jpg", imgBytes);
                    }
                }
                catch
                {
                    // This doesn't look like a valid image file so quietly skip it...
                }
            }
        }

        private void ux_buttonLoad_Click(object sender, EventArgs e)
        {
            _imageDGV.ReadOnly = true;
            ux_progressbarLoading.Show();
            ux_progressbarLoading.Minimum = 0;
            ux_progressbarLoading.Maximum = _imageDGV.Rows.Count;
            ux_progressbarLoading.Step = 1;

            foreach (DataGridViewRow dgvr in _imageDGV.Rows)
            {
                string remotePath = "";
                // Get the byte array for the image...
                byte[] imageBytes = System.IO.File.ReadAllBytes(dgvr.Cells["image_file_physical_path"].Value.ToString());
                // Attempt to upload the image to the remote server...
                if (imageBytes != null &&
                    !string.IsNullOrEmpty(dgvr.Cells["virtual_path"].Value.ToString()))
                {
                    remotePath = _sharedUtils.SaveImage(dgvr.Cells["virtual_path"].Value.ToString(), imageBytes, true, true);
                }
                // If the upload was successful the remotePath will contain the destination path (so now we can insert the record)...
                if (!string.IsNullOrEmpty(remotePath))
                {
                    // See if any records already exist for this inventory_id and if so update it 
                    // otherwise insert a new record...
                    DataSet ds = _sharedUtils.GetWebServiceData("get_accession_inv_attach", ":inventoryid=" + ((DataRowView)dgvr.DataBoundItem).Row["inventory_id"].ToString() + "; :accessionid=; :orderrequestid=; :cooperatorid=;", 0, 0);
                    if (ds.Tables.Contains("get_accession_inv_attach"))
                    {
                        DataRow[] inventoryImageRows = ds.Tables["get_accession_inv_attach"].Select("virtual_path='" + dgvr.Cells["virtual_path"].Value.ToString() + "'");
                        // This row does not exist - so create a new record...
                        if (inventoryImageRows != null && inventoryImageRows.Length == 0)
                        {
                            DataRow newInventoryImageRow = ds.Tables["get_accession_inv_attach"].NewRow();
                            newInventoryImageRow["accession_inv_attach_id"] = -1;
                            newInventoryImageRow["inventory_id"] = ((DataRowView)dgvr.DataBoundItem).Row["inventory_id"];
                            newInventoryImageRow["virtual_path"] = dgvr.Cells["virtual_path"].Value;
                            newInventoryImageRow["thumbnail_virtual_path"] = dgvr.Cells["thumbnail_virtual_path"].Value;
//newInventoryImageRow["sort_order"] = dgvr.Cells["sort_order"].Value;
//newInventoryImageRow["title"] = dgvr.Cells["title"].Value;
//newInventoryImageRow["description"] = dgvr.Cells["description"].Value;
//newInventoryImageRow["content_type"] = dgvr.Cells["content_type"].Value;
                            newInventoryImageRow["category_code"] = dgvr.Cells["category_code"].Value;
                            newInventoryImageRow["is_web_visible"] = dgvr.Cells["is_web_visible"].Value;
                            newInventoryImageRow["note"] = dgvr.Cells["note"].Value;
                            ds.Tables["get_accession_inv_attach"].Rows.Add(newInventoryImageRow);
                        }
                        // The row exists already (probably because it was previously uploaded) - so update the first existing record...
                        else if (inventoryImageRows != null && inventoryImageRows.Length > 0)
                        {
                            inventoryImageRows[0]["virtual_path"] = dgvr.Cells["virtual_path"].Value;
                            inventoryImageRows[0]["thumbnail_virtual_path"] = dgvr.Cells["thumbnail_virtual_path"].Value;
//newInventoryImageRow["sort_order"] = dgvr.Cells["sort_order"].Value;
//newInventoryImageRow["title"] = dgvr.Cells["title"].Value;
//newInventoryImageRow["description"] = dgvr.Cells["description"].Value;
//newInventoryImageRow["content_type"] = dgvr.Cells["content_type"].Value;
                            inventoryImageRows[0]["category_code"] = dgvr.Cells["category_code"].Value;
                            inventoryImageRows[0]["is_web_visible"] = dgvr.Cells["is_web_visible"].Value;
                            inventoryImageRows[0]["note"] = dgvr.Cells["note"].Value;
                        }
                        // Get the changes that need to be committed to the remote database...
                        DataSet modifiedData = new DataSet();
                        modifiedData.Tables.Add(ds.Tables["get_accession_inv_attach"].GetChanges());
                        if (modifiedData.Tables.Contains("get_accession_inv_attach"))
                        {
                            _sharedUtils.SaveWebServiceData(modifiedData);
                        }
                    }
                }
                ux_progressbarLoading.PerformStep();
            }
            // Close the form...
            this.Close();
        }

        private void ux_buttonCancel_Click(object sender, EventArgs e)
        {
int intRowEdits = ((DataTable)((BindingSource)_imageDGV.DataSource).DataSource).GetChanges().Rows.Count;
GRINGlobal.Client.Common.GGMessageBox ggMessageBox = new GRINGlobal.Client.Common.GGMessageBox("You have {0} unsaved row change(s), are you sure you want to cancel?", "Cancel Edits", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1);
ggMessageBox.Name = "ux_buttonCancelEditDataMessage1";
_sharedUtils.UpdateControls(ggMessageBox.Controls, ggMessageBox.Name);
if (ggMessageBox.MessageText.Contains("{0}")) ggMessageBox.MessageText = string.Format(ggMessageBox.MessageText, intRowEdits);
if (DialogResult.Yes == ggMessageBox.ShowDialog())
{
    // Close the form...
    this.Close();
}
        }

        private void ux_textboxCommonFilePath_TextChanged(object sender, EventArgs e)
        {
            if (_commonFilePath.Length == 0)
            {
                foreach (DataGridViewRow dgvr in _imageDGV.Rows)
                {
                    dgvr.Cells["virtual_path"].Value = dgvr.Cells["virtual_path"].Value.ToString().Insert(0, ux_textboxCommonFilePath.Text.Trim());
                    dgvr.Cells["thumbnail_virtual_path"].Value = dgvr.Cells["thumbnail_virtual_path"].Value.ToString().Insert(0, ux_textboxCommonFilePath.Text.Trim());
                }
            }
            else
            {
                foreach (DataGridViewRow dgvr in _imageDGV.Rows)
                {
                    dgvr.Cells["virtual_path"].Value = dgvr.Cells["virtual_path"].Value.ToString().Replace(_commonFilePath, ux_textboxCommonFilePath.Text.Trim());
                    dgvr.Cells["thumbnail_virtual_path"].Value = dgvr.Cells["thumbnail_virtual_path"].Value.ToString().Replace(_commonFilePath, ux_textboxCommonFilePath.Text.Trim());
                }
            }
            _commonFilePath = ux_textboxCommonFilePath.Text.Trim();
        }
    }
}
