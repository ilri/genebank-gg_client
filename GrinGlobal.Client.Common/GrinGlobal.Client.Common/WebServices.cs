using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GRINGlobal.Client.Common
{
    public class WebServices
    {
        private GrinGlobalGUIWebServices.GUI _GUIWebServices;
        private string _username = "";
        private string _password = "";
        private string _passwordClearText = "";
        private string _site = "";

        public string Url
        {
            get
            {
                return _GUIWebServices.Url;
            }
            set
            {
                _GUIWebServices.Url = value;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string Password_ClearText
        {
            get
            {
                return _passwordClearText;
            }
            set
            {
                _passwordClearText = value;
            }
        }

        public string Site
        {
            get
            {
                return _site;
            }
        }
        
        public WebServices(string webServiceURL, string webServiceUsername, string webServicePasswordEncrypted, string webServicePasswordClearText, string site)
        {
            _GUIWebServices = new GrinGlobalGUIWebServices.GUI();
            if(!string.IsNullOrEmpty(webServiceURL)) _GUIWebServices.Url = webServiceURL;
            _username = webServiceUsername;
            _password = webServicePasswordEncrypted;
            _passwordClearText = webServicePasswordClearText;
            _site = site;
        }

        public DataSet GetAllLookupTableStats()
        {
            DataSet returnDataSet;

            try
            {
                returnDataSet = _GUIWebServices.GetAllLookupTableStats(true, _username, _password);
                return returnDataSet;
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet GetData(string dataviewName, string delimitedParameterList, int offset, int limit)
        {
            DataSet dataViewParams;
            DataSet returnDataSet;
            string fullParamList;

            try
            {
                dataViewParams = _GUIWebServices.GetData(true, _username, _password, "get_dataview_parameters", ":dataview=" + dataviewName, 0, 0, null);
                if (dataViewParams.Tables.Contains("get_dataview_parameters") &&
                    dataViewParams.Tables["get_dataview_parameters"].Rows.Count > 0)
                {
                    fullParamList = "";
                    string[] paramKeyValueList = delimitedParameterList.Split(new char[] { ';' });
                    foreach (DataRow dr in dataViewParams.Tables["get_dataview_parameters"].Rows)
                    {
                        string paramName = dr["param_name"].ToString();
                        fullParamList += paramName + "=; ";
                        foreach (string paramKeyValue in paramKeyValueList)
                        {
                            if (paramKeyValue.Contains(paramName))
                            {
                                fullParamList = fullParamList.Replace(paramName + "=; ", paramName + "=" + paramKeyValue.Substring(paramKeyValue.IndexOf('=') + 1).Trim() + "; ");
                            }
                        }
                    }
                }
                else
                {
                    if (delimitedParameterList.Length > 0)
                    {
                        fullParamList = delimitedParameterList;
                    }
                    else
                    {
                        fullParamList = ":accessionid=; :inventoryid=; :orderrequestid=; :cooperatorid=; :createddate=; :modifieddate=; :startpkey=; :stoppkey=; :displaymember=; :valuemember=; :dataview=; :tablename=; pkfieldname=; :seclangid=; :name=;";
                    }
                }
                returnDataSet = _GUIWebServices.GetData(true, _username, _password, dataviewName, fullParamList, offset, limit, null);
                if (returnDataSet.Tables.Contains(dataviewName))
                {
                    ApplyColumnConstraints(returnDataSet.Tables[dataviewName]);
                }
                return returnDataSet;
            }
            catch(Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet SaveData(DataSet modifiedDataSet)
        {
            try
            {
                return _GUIWebServices.SaveData(true, _username, _password, modifiedDataSet, null);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        private DataSet BuildExceptionDataSet(Exception err)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable("ExceptionTable");
            dt.Columns.Add("ExceptionIndex", typeof(Int32));
            dt.Columns.Add("ExceptionType", typeof(string));
            dt.Columns.Add("Data", typeof(string));
            dt.Columns.Add("Message", typeof(string));
            dt.Columns.Add("Source", typeof(string));
            dt.Columns.Add("StackTrace", typeof(string));
            dt.Columns.Add("InnerException", typeof(string));
            DataRow dr = dt.NewRow();
            dr["ExceptionIndex"] = 1;
            dr["ExceptionType"] = err.GetType().FullName;
            dr["Data"] = err.Data.ToString();
            dr["Message"] = err.Message;
            dr["Source"] = err.Source;
            dr["StackTrace"] = err.StackTrace;
            if(err.InnerException != null && err.InnerException.Message != null) dr["InnerException"] = err.InnerException.Message;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            return ds;
        }

        public DataSet Search(string query, bool ignoreCase, bool andTermsTogether, string indexList, string resolverName, int offset, int limit, string searchOptions)
        {
            try
            {
                return _GUIWebServices.Search(true, _username, _password, query, ignoreCase, andTermsTogether, indexList, resolverName, offset, limit, searchOptions);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ValidateLogin()
        {
            try
            {
                return _GUIWebServices.ValidateLogin(true, _username, _password);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ChangeLanguage(int newLanguage)
        {
            try
            {
                return _GUIWebServices.ChangeLanguage(true, _username, _password, newLanguage);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ChangePassword(string newPassword)
        {
            try
            {
//return _GUIWebServices.ChangePassword(true, _username, _password, _username, newPassword);
return _GUIWebServices.ChangePassword(true, _username, _passwordClearText, _username, newPassword);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public DataSet ChangeOwnership(DataSet ownedDataset, int newCNO, bool includeChildren)
        {
            try
            {
                return _GUIWebServices.TransferOwnership(true, _username, _password, ownedDataset, newCNO, includeChildren);
            }
            catch (Exception err)
            {
                return BuildExceptionDataSet(err);
            }
        }

        public string GetVersion()
        {
            try
            {
                return _GUIWebServices.GetVersion();
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public string UploadImage(string destinationFilePath, byte[] imageBytes, bool createThumbnail, bool overWriteIfExists)
        {
            try
            {
                return _GUIWebServices.UploadImage(_username, _password, destinationFilePath, imageBytes, createThumbnail, overWriteIfExists);
            }
            catch (Exception err)
            {
                return err.Message;
            }
        }

        public byte[] DownloadImage(string remoteFilePath)
        {
            try
            {
                return _GUIWebServices.DownloadImage(_username, _password, remoteFilePath);
            }
            catch (Exception err)
            {
                return new byte[0];
            }
        }

        public void ApplyColumnConstraints(DataTable dataviewTable)
        {
            if (dataviewTable != null &&
                dataviewTable.Columns != null &&
                dataviewTable.Columns.Count > 0)
            {
                foreach (DataColumn dc in dataviewTable.Columns)
                {
                    if (dc.ExtendedProperties.Contains("is_primary_key") &&
                        dc.ExtendedProperties["is_primary_key"].ToString() == "Y")
                    {
                        if (dc.DataType == typeof(int))
                        {
                            dc.AllowDBNull = false;
                            dc.AutoIncrement = true;
                            dc.AutoIncrementSeed = -1;
                            dc.AutoIncrementStep = -1;
                            dataviewTable.PrimaryKey = new DataColumn[1] { dc };
                        }
                        else
                        {
                            dc.AllowDBNull = false;
                            dataviewTable.PrimaryKey = new DataColumn[1] { dc };
                        }
                    }
                }
            }
        }
    }
}
