using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace GRINGlobal.Client.Common
{
    public class LocalDatabase
    {
        private string _connectionString;
        private Dictionary<string, bool> _tableExistsStatus;

        public LocalDatabase(string DatabaseName)
        {
            _tableExistsStatus = new Dictionary<string, bool>();

            if (string.IsNullOrEmpty(DatabaseName))
            {
                _connectionString = @"Data Source=localhost\SQLExpress; Integrated Security=True;";
            }
            else
            {
                if (!DatabaseExists(DatabaseName))
                {
                    CreateDatabase(DatabaseName);
                }
                _connectionString = @"Data Source=localhost\SQLExpress; Integrated Security=True; Initial Catalog=" + DatabaseName;
            }
        }

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public DataTable GetData(string SQLSelect, string[] SQLparms)
        {
            SqlConnection dbConnection = new SqlConnection(_connectionString);
            SqlCommand dbCommand = new SqlCommand(SQLSelect, dbConnection);
            foreach (string parmEquation in SQLparms)
            {
                string[] parm = parmEquation.Split('=');
                if (parm.Length == 2)
                {
                    dbCommand.Parameters.AddWithValue(parm[0].Trim(), parm[1].TrimStart(' '));
                }
            }
            SqlDataAdapter dbDataAdapter = new SqlDataAdapter(dbCommand);
            dbDataAdapter.MissingMappingAction = MissingMappingAction.Passthrough;
            dbDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            DataTable dt = new DataTable();
            try
            {
                dbDataAdapter.Fill(dt);
                foreach (DataColumn dc in dt.PrimaryKey)
                {
                    dc.AllowDBNull = false;
                    if (dc.DataType == typeof(int) ||
                        dc.DataType == typeof(Int16) ||
                        dc.DataType == typeof(Int32) ||
                        dc.DataType == typeof(Int64))
                    {
                        dc.AutoIncrement = true;
                        dc.AutoIncrementSeed = -1;
                        dc.AutoIncrementStep = -1;
                    }
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Error retrieving local data in GrinGlobal.Client.Data library.", e);
            }

            return dt;
        }

        public bool SaveData(DataTable DataTable)
        {
            bool success = false;
            try
            {
                SqlConnection dbConnection = new SqlConnection(_connectionString);
                SqlDataAdapter dbDataAdapter = new SqlDataAdapter("SELECT * FROM " + DataTable.TableName, dbConnection);
                SqlCommandBuilder dbCommandBuilder = new SqlCommandBuilder(dbDataAdapter);

                dbDataAdapter.Update(DataTable);
                
                dbCommandBuilder.Dispose();
                dbCommandBuilder = null;
                dbDataAdapter.Dispose();
                dbDataAdapter = null;
                dbConnection.Close();
                dbConnection.Dispose();
                dbConnection = null;
                success = true;
            }
            catch
            {
                success = false;
            }
            return success;
        }

        public void CreateTable(DataTable NewTable, bool CreateSchemaOnly, bool OverwriteTable)
        {
            SqlConnection dbConnection = new SqlConnection(_connectionString);
            SqlCommand dbCommand = new SqlCommand("", dbConnection);
            SqlDataAdapter dbDataAdapter = new SqlDataAdapter("SELECT * FROM " + NewTable.TableName, dbConnection);
            SqlCommandBuilder dbCommandBuilder = new SqlCommandBuilder(dbDataAdapter);

            dbConnection.Open();

            if (TableExists(NewTable.TableName))
            {
                if (OverwriteTable)
                {
                    dbCommand.CommandText = "DROP TABLE " + NewTable.TableName;
                    dbCommand.ExecuteNonQuery();
                }
                else
                {
                    return;
                }
            }

            // Build the SQL create table command...
            string createTableCommand = "CREATE TABLE " + NewTable.TableName + " (";
            foreach (DataColumn dc in NewTable.Columns)
            {
                createTableCommand += BuildColumnSQL(dc) + ",";
            }
            createTableCommand = createTableCommand.TrimEnd(',') + ")";
            dbCommand.CommandText = createTableCommand;
            dbCommand.ExecuteNonQuery();

            // Add the data if requested...
            if (!CreateSchemaOnly && NewTable.Rows.Count > 0)
            {
                foreach (DataRow dr in NewTable.Rows)
                {
                    if(dr.RowState != DataRowState.Added) dr.SetAdded();
                }
                dbDataAdapter.Update(NewTable);
            }

            dbDataAdapter.Dispose();
            dbDataAdapter = null;
            dbCommand.Dispose();
            dbCommand = null;
            dbConnection.Close();
            dbConnection.Dispose();
            dbConnection = null;
        }

        private string BuildColumnSQL(DataColumn dc)
        {
            string columnSQL = dc.ColumnName + " ";
            string columnName = dc.DataType.Name.ToString().ToUpper();
            switch (columnName)
            {
                case "INT":
                case "INT32":
                case "INT64":
                    columnSQL += "INT ";
                    break;
                case "STRING":
                    if (dc.ExtendedProperties.Contains("is_primary_key") && dc.ExtendedProperties["is_primary_key"].ToString() == "Y")
                    {
                        columnSQL += "NVARCHAR(255) ";
                    }
                    else
                    {
                        columnSQL += "NVARCHAR(MAX) ";
                    }
                    break;
                case "DATETIME":
                    columnSQL += "DATETIME ";
                    break;
                default:
                    columnSQL += "NVARCHAR(255) ";
                    break;
            }
            //if (dc.ExtendedProperties["is_autoincrement"].ToString() == "Y")
            //{
            //    columnSQL += "IDENTITY ";
            //}
            if (dc.ExtendedProperties.Contains("is_primary_key") && dc.ExtendedProperties["is_primary_key"].ToString() == "Y")
            {
                columnSQL += "PRIMARY KEY ";
            }
            return columnSQL;
        }

        public bool DatabaseExists(string DatabaseName)
        {
            bool databaseExists = false;
            try
            {
                SqlConnection dbConnection = new SqlConnection(@"Data Source=localhost\SQLExpress; Integrated Security=True;");
                SqlCommand dbCommand = new SqlCommand("SELECT count(1) from sysdatabases where name = '" + DatabaseName + "'", dbConnection);
                dbConnection.Open();
                if ((int)dbCommand.ExecuteScalar() == 1)
                {
                    databaseExists = true;
                }
                else
                {
                    databaseExists = false;
                }
            }
            catch
            {
                databaseExists = false;
            }
            return databaseExists;
        }

        public bool CreateDatabase(string DatabaseName)
        {
            bool success = false;
            try
            {
                SqlConnection dbConnection = new SqlConnection(@"Data Source=localhost\SQLExpress; Integrated Security=True;");
                SqlCommand dbCommand = new SqlCommand("CREATE DATABASE " + DatabaseName, dbConnection);
                dbConnection.Open();
                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
                dbCommand = null;
                dbConnection.Close();
                dbConnection.Dispose();
                dbConnection = null;
                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        public bool TableExists(string TableName)
        {
            bool tableExists = false;
            // Check the cache first for table existence...
            if (_tableExistsStatus.ContainsKey(TableName))
            {
                tableExists = _tableExistsStatus[TableName];
            }
            // The cache above may have returned false (but another thread may be building 
            // the table - so check again...
            if(!tableExists)
            {
                try
                {
                    SqlConnection dbConnection = new SqlConnection(_connectionString);
                    SqlDataAdapter dbDataAdapter = new SqlDataAdapter("SELECT COUNT(*) FROM " + TableName, dbConnection);
                    DataTable table = new DataTable();
                    dbConnection.Open();
                    dbDataAdapter.Fill(table);
                    table.Dispose();
                    table = null;
                    dbDataAdapter.Dispose();
                    dbDataAdapter = null;
                    dbConnection.Close();
                    dbConnection.Dispose();
                    dbConnection = null;

                    tableExists = true;
                }
                catch
                {
                    tableExists = false;
                }

                if (_tableExistsStatus.ContainsKey(TableName))
                {
                    _tableExistsStatus[TableName] = tableExists;
                }
                else
                {
                    _tableExistsStatus.Add(TableName, tableExists);
                }
            }
            return tableExists;
        }

        public void Remove(string tableName)
        {
            if (TableExists(tableName))
            {
                SqlConnection dbConnection = new SqlConnection(_connectionString);
                SqlCommand dbCommand = new SqlCommand("DROP TABLE " + tableName, dbConnection);
                dbConnection.Open();
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
                dbConnection.Dispose();
                dbConnection = null;
                if (_tableExistsStatus.ContainsKey(tableName)) _tableExistsStatus[tableName] = false;
            }
        }

        public bool MakeAccessibleToAllUsers()
        {
            bool success = false;
            try
            {
                SqlConnection dbConnection = new SqlConnection(@"Data Source=localhost\SQLExpress; Integrated Security=True;");
                SqlCommand dbCommand = new SqlCommand("sp_addsrvrolemember", dbConnection); // @loginame = N'BUILTIN\Users', @rolename = N'sysadmin'");
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@loginame", "BUILTIN\\Users");
                dbCommand.Parameters.AddWithValue("@rolename", "sysadmin");
                dbConnection.Open();
                dbCommand.ExecuteNonQuery();
                dbCommand.Dispose();
                dbCommand = null;
                dbConnection.Close();
                dbConnection.Dispose();
                dbConnection = null;
                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }
    }
}
