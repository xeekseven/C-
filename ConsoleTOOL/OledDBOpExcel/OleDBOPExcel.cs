using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace OledDBOpExcel
{
    public class OleDBOPExcel
    {
        public static DataTable readExcel(string filePath,string sheetName = null)
        {
            try
            {
                
                string connectString = string.Empty;
                switch (Path.GetExtension(filePath))
                {
                    case ".xls":
                        connectString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=1;'", filePath);
                        break;
                    case ".xlsx":
                        connectString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'", filePath);
                        break;
                    default:
                        return null;
                        break;
                }
                DataSet ds = new DataSet();
                using (OleDbConnection conn = new OleDbConnection(connectString))
                {
                    conn.Open();
                    if (sheetName == null)
                    {
                        DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        //获取Excel的第一个Sheet表名
                        sheetName = schemaTable.Rows[0][2].ToString().Trim();
                    }

                    string commandText = String.Format("select * from [{0}]", sheetName);
                    using (OleDbCommand command = new OleDbCommand(commandText,conn))
                    {
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);

                        adapter.Fill(ds, sheetName);

                    }
                    conn.Close();
                }

                return ds.Tables[sheetName];
            }
            catch(Exception exMsg)
            {
                return null;
            }
        }

        public static bool writeExcel(string filePath,string sheetName,List<string> columnList,List<List<string>> columnValueList)
        {
            try
            {
                string connectString = string.Empty;
                switch (Path.GetExtension(filePath))
                {
                    case ".xls":
                        connectString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;IMEX=0;'", filePath);
                        break;
                    case ".xlsx":
                        connectString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=YES;IMEX=0;'", filePath);
                        break;
                    default:
                        return false;
                        break;
                }
                string columnName = string.Empty;
                string columnInsertValue = string.Empty;
                List<string> columnInsertValues = new List<string>();
                if (columnValueList !=null && columnList.Count != 0)
                {
                    columnName += "(";
                    for (int listIndex = 0; listIndex < columnList.Count; listIndex++)
                    {
                        if (listIndex == 0)
                        {
                            columnName += columnList[listIndex];
                        }
                        else
                        {
                            columnName = columnName + "," + columnList[listIndex];
                        }
                    }
                    columnName += ")";
                }
                if (columnValueList.Count != 0)
                {

                    for (int rowIndex = 0; rowIndex < columnValueList.Count; rowIndex++)
                    {
                        columnInsertValue = string.Empty;
                        columnInsertValue += "(";
                        for (int columnIndex = 0; columnIndex < columnValueList[rowIndex].Count; columnIndex++)
                        {
                            if (columnIndex == 0)
                            {
                                columnInsertValue += "'" + columnValueList[rowIndex][columnIndex] + "'";
                            }
                            else
                            {
                                columnInsertValue = columnInsertValue + "," + "'" + columnValueList[rowIndex][columnIndex].ToString() + "'";
                            }
                        }
                        columnInsertValue += ")";
                        string sqlText = String.Format("insert into [{0}$]{1} values{2}", sheetName, columnName, columnInsertValue);
                        columnInsertValues.Add(sqlText);
                    }
                    
                }
                using (OleDbConnection conn = new OleDbConnection(connectString))
                {
                    conn.Open();
                    using (OleDbCommand command = conn.CreateCommand())
                    {

                       
                        //string sqlText = String.Format("insert into [{0}$]{1} values{2}", sheetName, columnName, columnInsertValue);
                        foreach(string sqlItem in columnInsertValues)
                        {
                            command.CommandText = sqlItem;
                            command.ExecuteNonQuery();
                        }
                        
                    }
                    conn.Close();
                    
                }
                return true;
            }catch(Exception exMsg)
            {
                return false;
            }
        }

        public static bool createExcel(string filePath,string creatSqlText)
        {
            try
            {
                string connectString = string.Empty;
                switch (Path.GetExtension(filePath))
                {
                    case ".xls":
                        connectString = String.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0 XML;'", filePath);
                        break;
                    case ".xlsx":
                        connectString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 XML';", filePath);
                        break;
                    default:
                        return false;
                        break;
                }
                using(OleDbConnection conn = new OleDbConnection(connectString))
                {
                    conn.Open();
                    using(OleDbCommand command = conn.CreateCommand())
                    {
                        command.CommandText = creatSqlText;
                        command.ExecuteNonQuery();

                        command.CommandText = "insert into userInfo (ID,USERNAME) values('1001','username')";
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }catch(Exception exMsg)
            {
                return false;
            }
        }
        public static List<string> GetSheetList(string filePath)
        {
            List<string> sheetList = new List<string>();
            string connectString = string.Empty;
            switch (Path.GetExtension(filePath))
            {
                case ".xls":
                    connectString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=NO;IMEX=1;'", filePath);
                    break;
                case ".xlsx":
                    connectString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=NO;IMEX=1;'", filePath);
                    break;
                default:
                    return null;
                    break;
            }
            using(OleDbConnection conn = new OleDbConnection(connectString))
            {
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach(DataRow rowItem in schemaTable.Rows)
                {
                    sheetList.Add(rowItem["TABLE_NAME"].ToString());
                }
                conn.Close();
            }

            return sheetList;
        }
    }
}
