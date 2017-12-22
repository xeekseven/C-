using SqlBulCopyConsole;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace OledDBOpExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 创建表格
            if(false)
            {
                string filePath = @"D:\新建文件夹\新建文件夹\creatData.xlsx";
                //if(!File.Exists(filePath))
                //{
                //    File.Create(filePath);
                //}
                string createText = String.Format("create table userInfo ([ID] VarChar,[USERNAME] VarChar)");
                bool chCreateStr = OleDBOPExcel.createExcel(filePath, createText);
                Console.WriteLine(chCreateStr);
            }
            
            #endregion

            #region 向excel插入数据
            if (false)
            {
                bool chStr = OleDBOPExcel.writeExcel(@"D:\新建文件夹\新建文件夹\TestData.xlsx", "sheet1", new List<string> { "ID", "NAME" }, new List<List<string>> { new List<string> { "1001", "21" }, new List<string> { "1002", "1221" } });

            }
            #endregion

            #region 获取所有的sheet名
            if (false)
            {
                List<string> sheetList = OleDBOPExcel.GetSheetList(@"D:\新建文件夹\新建文件夹\新建 Microsoft Office Excel 工作表.xlsx");

                foreach (string item in sheetList)
                {
                    Console.WriteLine(item);
                }
            }
            #endregion
            
            #region 读入excel并写入数据库
            if(true)
            {
                DataTable datatable = OleDBOPExcel.readExcel(@"D:\新建文件夹\新建文件夹\4G工参数据.xlsx");
                for (int rowIndex = 0; rowIndex < datatable.Rows.Count; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < datatable.Columns.Count; columnIndex++)
                    {
                        if (datatable.Rows[rowIndex][columnIndex].ToString() == string.Empty || datatable.Rows[rowIndex][columnIndex].ToString() == "NULL")
                        {
                            datatable.Rows[rowIndex][columnIndex] = DBNull.Value;
                        }
                    }
                }
                Console.WriteLine("read ok!");
                SqlBulCopyClass.SqlBulCopyDataTable(@"Data Source=CCHCWNBSFEJ1JFZ\SQLEXPRESS;Initial Catalog=importdb;Persist Security Info=True;User ID=dtauser;Password=dtauser", "tb_cfg_cell_lte", datatable);
                Console.WriteLine("write ok!");
            }
            
            #endregion
            Console.ReadKey();
        }
    }
}
