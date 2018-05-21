using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using System.Transactions;
using System.Threading.Tasks;
using DBHelper.Interface;
using System.Text.RegularExpressions;

namespace DBHelper.RealizeClass.Mysql
{
    public class MysqlCommon : IDataCommonOperator
    {
        public string GetDefaultPort()
        {
            string defaultPort = "";
            return defaultPort;
        }
        public string AddEncodingConnectString(string connectString)
        {
            if (!connectString.Contains("Charset"))
            {
                connectString = connectString + ";Charset=utf8;";
            }
            return connectString;
        }
        public bool TestConnect(string serviceName, string port, string dbName, string loginName, string password)
        {
            try
            {
                string connectString = String.Format("Server={0};Database={1};Uid={2};Pwd={3};Port={4};", serviceName, dbName, loginName, password, port);
                using (MySqlConnection conn = new MySqlConnection(connectString))
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
        public IDbConnection GetDbConnection(string connectString)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(AddEncodingConnectString(connectString));
                return conn;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public IDataReader ExecuteDataReader(string connectString, string sqlText, IDbConnection conn)
        {
            MySqlDataReader reader = null;
            MySqlConnection connect = null;
            try
            {
                connect = new MySqlConnection(AddEncodingConnectString(connectString));
                connect.Open();
                MySqlCommand command = new MySqlCommand(sqlText, connect);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                throw (ex);
            }
        }
        public bool ExecuteNoQuery(string connectString, string sqlText)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(AddEncodingConnectString(connectString)))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(sqlText, conn))
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
                using (MySqlConnection conn = new MySqlConnection(AddEncodingConnectString(connectString)))
                {
                    conn.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(sqlText, conn))
                    {
                        adapter.SelectCommand.CommandTimeout = 0;
                        adapter.Fill(dTable);
                    }
                    conn.Dispose();
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
                using (MySqlConnection conn = new MySqlConnection(AddEncodingConnectString(connectString)))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(sqlText, conn))
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
                string selectColumnText = String.Format("select column_name from information_schema.columns  where table_name='{0}';", tableName);
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
        public bool SqlBulkInsertDataTable(string connectString, DataTable datatable, string tableName)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\MysqlBcp\\" + tableName + Guid.NewGuid().ToString() + ".txt";
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
            try
            {
                List<string> columnList = GetTableColumns(AddEncodingConnectString(connectString), tableName);
                List<string> dTableList = new List<string>();
                string fieldTerminator = "\t";

                foreach (DataRow rowItem in datatable.Rows)
                {
                    string lineText = string.Empty;
                    for (int index = 0; index < rowItem.ItemArray.Length; index++)
                    {
                        if (index != 0) lineText += fieldTerminator;
                        lineText += (rowItem.ItemArray[index].ToString() == string.Empty ? "NULL" : rowItem.ItemArray[index].ToString());
                    }
                    dTableList.Add(lineText);
                }
                File.WriteAllLines(filePath, dTableList.ToArray(), Encoding.GetEncoding("utf-8"));
                using (MySqlConnection conn = new MySqlConnection(AddEncodingConnectString(connectString)))
                {
                    conn.Open();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = fieldTerminator,
                        LineTerminator = "\n",
                        FileName = filePath,
                        TableName = tableName,
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        NumberOfLinesToSkip = 0,
                        CharacterSet = "utf8",
                    };
                    List<string> columnNameList = new List<string>();
                    foreach (DataColumn column in datatable.Columns)
                    {
                        columnNameList.Add(column.ColumnName);
                    }
                    bulk.Columns.AddRange(columnNameList.ToArray());
                    int insertCount = bulk.Load();
                }
                File.Delete(filePath);
                return true;
            }
            catch (Exception exMsg)
            {
                throw (exMsg);
            }
        }
        public bool SqlBulkInsertFile(string connectionString, List<string> columnNameList, string filePath, string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) throw new Exception("请给DataTable的TableName属性附上表名称");
            if (columnNameList.Count == 0) return false;
            int insertCount = 0;
            MySqlTransaction tran = null;

            using (MySqlConnection conn = new MySqlConnection(AddEncodingConnectString(connectionString)))
            {
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = "\t",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\n",
                        FileName = filePath,
                        NumberOfLinesToSkip = 0,
                        TableName = tableName,
                        CharacterSet = "utf8",
                    };
                    //List<string> columnList =new List<string>();
                    //for(int columnIndex=0;columnIndex<table.Columns.Count;columnIndex++){
                    //    columnList.Add(table.Columns[columnIndex].ColumnName);
                    //}
                    bulk.Columns.AddRange(columnNameList.ToArray());
                    insertCount = bulk.Load();
                    tran.Commit();
                }
                catch (MySqlException ex)
                {
                    if (tran != null) tran.Rollback();
                    throw ex;
                }
            }
            return true;
        }
        public bool BatchUpdateData(string connectString, List<string> sqlList, string tableName, string whereColumnName)
        {
            try
            {
                string indexName = String.Format("{0}_index_{1}", whereColumnName, Guid.NewGuid().ToString());
                string createIndexText = String.Format("if not exists (select * from sysindexes where id=object_id('{0}') and name='{1}') CREATE UNIQUE NONCLUSTERED INDEX [{1}]ON dbo.{0}({2})", tableName, indexName, whereColumnName);
                string dropIndexText = String.Format("if exists (select * from sysindexes where id=object_id('{0}') and name='{1}') drop INDEX [{1}] ON dbo.{0}", tableName, indexName);

                ExecuteNoQuery(connectString, createIndexText);
                using (TransactionScope tScope = new TransactionScope())
                {
                    using (MySqlConnection conn = new MySqlConnection(AddEncodingConnectString(connectString)))
                    {
                        using (MySqlCommand command = new MySqlCommand())
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
                using (MySqlConnection conn = new MySqlConnection(connectString))
                {
                    string sqlText = string.Format("select table_name from information_schema.tables where table_schema='{0}' and table_name like '%{1}%'", dbName, tbLikeName);

                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand(sqlText, conn))
                    {
                        MySqlDataReader reader = command.ExecuteReader();
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
            string realDbName = Regex.Replace(dbName, @"\(.+\)", "");
            using (MySqlConnection conn = new MySqlConnection(connectString))
            {
                string sqlText = String.Format("use information_schema;select table_name,table_rows from tables where TABLE_SCHEMA = '{0}'order by table_rows desc;", realDbName);
                conn.Open();
                using (MySqlCommand sqlCommand = new MySqlCommand(sqlText, conn))
                {
                    MySqlDataReader reader = null;
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
    }
}
