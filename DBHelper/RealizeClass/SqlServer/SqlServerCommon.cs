using DBHelper.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace DBHelper.RealizeClass.SqlServer
{
    public class SqlServerCommon : IDataCommonOperator
    {
        public string GetDefaultPort()
        {
            string defaultPort = "";
            return defaultPort;
        }
        public bool TestConnect(string serviceName,string port,string dbName,string loginName,string password)
        {
            try
            {
                port = port == string.Empty ? string.Empty : String.Format("{0}{1}", ",", port);
                string connectString = String.Format(@"Data Source={0}{1};Initial Catalog={2};Persist Security Info=True;User ID={3};Password={4};Connect Timeout=3", serviceName, port, dbName, loginName, password);
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public bool ExecuteNoQuery(string connectString, string sqlText)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sqlText, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            
            return true;
        }
        public DataTable ExecuteQueryDatatable(string connectString, string sqlText)
        {
            DataTable dTable = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, conn))
                    {
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.Fill(dTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            
            return dTable;
        }
        public string ExecuteScalar(string connectString, string sqlText)
        {
            string resText = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sqlText, conn))
                    {
                        command.CommandTimeout = 0;
                        resText = command.ExecuteScalar().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return resText;
        }
        public List<string> GetTableColumns(string connectString, string tableName)
        {
            List<string> columnList = new List<string>();
            try
            {
                string selectColumnText = String.Format("select name from syscolumns where id=(select max(id) from sysobjects where xtype='u' and name='{0}') ", tableName);
                DataTable columnDt = ExecuteQueryDatatable(connectString, selectColumnText);
                foreach (DataRow rowItem in columnDt.Rows)
                {
                    columnList.Add(rowItem.ItemArray[0].ToString());
                }
                return columnList;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public bool SqlBulkInsertDataTable(string connectString, DataTable datatable,string tableName)
        {
            try
            {
                List<string> columnList = GetTableColumns(connectString, tableName);
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectString, SqlBulkCopyOptions.KeepNulls))
                    {
                        sqlbulkcopy.DestinationTableName = tableName;
                        sqlbulkcopy.BulkCopyTimeout = 600;
                        for (int i = 0; i < datatable.Columns.Count; i++)
                        {
                            if (!columnList.Contains(datatable.Columns[i].ColumnName))
                            {
                                continue;
                            }
                            sqlbulkcopy.ColumnMappings.Add(datatable.Columns[i].ColumnName, datatable.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(datatable);
                    }
                }
                return true;
            }
            catch (Exception exMsg)
            {
                throw (exMsg);
            }
        }
        public bool BatchUpdateData(string connectString, List<string> sqlList,string tableName,string whereColumnName)
        {
            try
            {
                string indexName = String.Format("{0}_index_{1}", whereColumnName, Guid.NewGuid().ToString());
                string createIndexText = String.Format("if not exists (select * from sysindexes where id=object_id('{0}') and name='{1}') CREATE UNIQUE NONCLUSTERED INDEX [{1}]ON dbo.{0}({2})", tableName, indexName, whereColumnName);
                string dropIndexText = String.Format("if exists (select * from sysindexes where id=object_id('{0}') and name='{1}') drop INDEX [{1}] ON dbo.{0}",tableName,indexName);

                ExecuteNoQuery(connectString, createIndexText);
                using (TransactionScope tScope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {

                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = conn;
                            conn.Open();
                            foreach (string sqlText in sqlList)
                            {
                                command.CommandText = sqlText;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    tScope.Complete();
                    tScope.Dispose();
                }
                ExecuteNoQuery(connectString, dropIndexText);

                return true;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }
        public List<string> GetAllTableNames(string connectString, string dbName, string tbLikeName, string typeName = null)
        {
            List<string> list = new List<string>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    string sqlText = string.Format("select name from sysobjects where type='{0}' order by name", typeName);
                    if (tbLikeName != string.Empty)
                    {
                        sqlText = string.Format("select name from sysobjects where name like '%{0}%' and type='{1}' order by name", tbLikeName, typeName);
                    }
                    conn.Open();
                    using (SqlCommand command = new SqlCommand(sqlText, conn))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            list.Add(Convert.ToString(reader[0]));
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
                //MessageBox.Show("[错误]：\r\n" + exp.Message, "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return list;
        }
        public Dictionary<string, long> GetTableRowNum(string tbName, string connectString, string dbName)
        {
            Dictionary<string, long> tableRowNumDic = new Dictionary<string, long>();
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                string sqlText = String.Format("select b.name ,SUM(CASE WHEN (a.indid < 2) THEN a.rows ELSE 0 END) as colnumber FROM sysindexes  a, sysobjects  b WHERE a.id = b.id and b.type = 'U' and b.name in {0} group by b.name order by b.name", tbName);
                //string sqlText = "\r\nselect b.name ,\r\nSUM(CASE WHEN (a.indid < 2) THEN a.rows ELSE 0 END) as colnumber" +
                // "\r\nFROM sysindexes  a, sysobjects  b \r\nWHERE a.id = b.id and b.type = 'U' and b.name in " + tbName +
                // "\r\ngroup by b.name \r\norder by b.name";
                conn.Open();
                using (SqlCommand sqlCommand = new SqlCommand(sqlText, conn))
                {
                    SqlDataReader reader = null;
                    try
                    {
                        reader = sqlCommand.ExecuteReader();

                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                if (!tableRowNumDic.ContainsKey(dbName + "-" + reader[0].ToString()))
                                {
                                    tableRowNumDic.Add(dbName + "-" + reader[0].ToString(), -1);
                                }
                                tableRowNumDic[dbName + "-" + reader[0].ToString()] = Convert.ToInt64(reader[1].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                    conn.Close();
                    return tableRowNumDic;
                }
            }
        }
        public bool SqlBulkInsertFile(string connectionString, List<string> columnNameList, string filePath, string tableName)
        {
            throw new NotImplementedException();
        }
        public IDataReader ExecuteDataReader(string connectString, string sqlText, IDbConnection conn)
        {
            throw new NotImplementedException();
        }


        public IDbConnection GetDbConnection(string connectString)
        {
            throw new NotImplementedException();
        }
    }
}
