using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiConsole
{
    public class OperExcel
    {
        //Excel第一行为标题名
        public static DataTable readExcel(string filePath, ref List<string> columnNameArray)
        {
            DataTable table = new DataTable();
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fs);
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                XSSFRow headRow = (XSSFRow)sheet.GetRow(0);

                int columnCount = headRow.Cells.Count;
                //获取标题名
                for (int index = 0; index < headRow.Cells.Count; index++)
                {
                    DataColumn dc = new DataColumn(headRow.GetCell(index).StringCellValue.ToString());

                    table.Columns.Add(dc);
                    columnNameArray.Add(headRow.GetCell(index).ToString());
                }

                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow newRow = table.NewRow();
                    XSSFRow row = (XSSFRow)sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.GetCell(1) != null)
                        {
                            //复制整行数据
                            for (int j = 0; j < columnCount; j++)
                            {
                                newRow[j] = row.GetCell(j);
                            }
                            //添加到数据表中
                            table.Rows.Add(newRow);
                        }
                    }
                }
            }
            return table;
        }

        public static DataTable readExcel(string filePath,List<string> columnNameList,int sheetNum,int startRow)
        {
            DataTable table = new DataTable();
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fs);
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(sheetNum);
                XSSFRow headRow = (XSSFRow)sheet.GetRow(0);

                int columnCount = columnNameList.Count;
                DataColumn[] dcArray = new DataColumn[columnCount];
               
                //获取标题名
                for (int index = 0; index < columnCount; index++)
                {
                    dcArray[index] = new DataColumn(columnNameList[index]);
                }
                table.Columns.AddRange(dcArray);

                for (int i = sheet.FirstRowNum + startRow; i <= sheet.LastRowNum; i++)
                {
                    DataRow newRow = table.NewRow();
                    XSSFRow row = (XSSFRow)sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.GetCell(1) != null)
                        {
                            //复制整行数据
                            for (int j = 0; j < columnCount; j++)
                            {
                                newRow[j] = row.GetCell(j);
                            }
                            //添加到数据表中
                            table.Rows.Add(newRow);
                        }
                    }
                }
            }
            return table;
        }
    }
}
