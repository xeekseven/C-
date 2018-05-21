using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace DBHelper.Interface
{
    public interface IDataCommonOperator
    {
        string GetDefaultPort();
        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <param name="serviceName">实例名||IP</param>
        /// <param name="port">端口</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="loginName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>bool</returns>
        bool TestConnect(string serviceName,string port,string dbName,string loginName,string password);

        #region 数据库基础对象
        /// <summary>
        /// 返回一个数据库基础连接类型,可以在外部进行关闭连接以释放资源
        /// </summary>
        /// <param name="connectString"></param>
        /// <returns>IDbConnection</returns>
        IDbConnection GetDbConnection(string connectString);
        /// <summary>
        /// 返回一个数据DataReader,可以流式的输出数据库查询结果
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sqlText"></param>
        /// <param name="conn"></param>
        /// <returns>IDataReader</returns>
        IDataReader ExecuteDataReader(string connectString, string sqlText, IDbConnection conn);
        #endregion

        #region 单个操作
        /// <summary>
        /// 执行单个无返回值的Sql语句
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sqlText"></param>
        /// <returns>void</returns>
        bool ExecuteNoQuery(string connectString, string sqlText);
        /// <summary>
        /// 执行某个Sql语句,返回第一行第一列的结果
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sqlText"></param>
        /// <returns>string</returns>
        string ExecuteScalar(string connectString, string sqlText);
        /// <summary>
        /// 执行某个Sql语句,结果以DataTable格式返回
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="sqlText"></param>
        /// <returns>DataTable</returns>
        DataTable ExecuteQueryDatatable(string connectString, string sqlText);   
        #endregion

        #region 批量操作
        /// <summary>
        /// 把一个bcp文件的内容批量插入到指定的表中
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="columnNameList">列名</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="tableName">表名</param>
        /// <returns>bool</returns>
        bool SqlBulkInsertFile(string connectionString, List<string> columnNameList, string filePath, string tableName);
        /// <summary>
        /// 把一个DataTable的内容批量插入到指定的表中
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="datatable"></param>
        /// <param name="tableName"></param>
        /// <returns>bool</returns>
        bool SqlBulkInsertDataTable(string connectString, DataTable datatable, string tableName);
        bool BatchUpdateData(string connectString, List<string> sqlList, string tableName, string whereColumnName);
        #endregion
        List<string> GetTableColumns(string connectString, string tableName);
        List<string> GetAllTableNames(string connectString, string dbName, string tbLikeName, string typeName = null);
        Dictionary<string, long> GetTableRowNum(string tbName, string connectString, string dbName);
    }
}
