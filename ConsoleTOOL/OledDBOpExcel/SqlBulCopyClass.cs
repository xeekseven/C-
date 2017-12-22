using OledDBOpExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SqlBulCopyConsole
{
    public class SqlBulCopyClass
    {
        public static void SqlBulCopyDataTable(string connectionString, string TableName, DataTable dt)
        {
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy sqlbulkcopy =new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        //WorkParamColumnNames.GetInstance().InitColumnDic();
                        sqlbulkcopy.DestinationTableName = TableName;
                        //int correctVlue = dt.Columns.Count > 27 ? 0 : 1;
                        for (int index =0; index < dt.Columns.Count; index++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[index].ColumnName, dt.Columns[index].ColumnName);
                        }
                        //dt.Rows.RemoveAt(0);
                        sqlbulkcopy.WriteToServer(dt);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
