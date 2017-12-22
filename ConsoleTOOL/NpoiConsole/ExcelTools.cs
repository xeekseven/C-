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
    public class ExcelTools
    {
        public static DataTable readExcel(string filePath, string sheetName, int headIndex = 0)
        {
            DataTable table = new DataTable();
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    XSSFSheet sheet = (XSSFSheet)workbook.GetSheet(sheetName);
                    XSSFRow headRow = (XSSFRow)sheet.GetRow(headIndex);

                    int columnCount = headRow.Cells.Count;
                    //获取标题名
                    for (int index = 0; index < headRow.Cells.Count; index++)
                    {
                        DataColumn dc = new DataColumn(headRow.GetCell(index).ToString());
                        table.Columns.Add(dc);
                    }

                    for (int i = sheet.FirstRowNum + 1 + headIndex; i <= sheet.LastRowNum; i++)
                    {
                        DataRow newRow = table.NewRow();
                        XSSFRow row = (XSSFRow)sheet.GetRow(i);
                        int isEmpty = 0;
                        if (row != null)
                        {

                            //复制整行数据
                            for (int j = 0; j < columnCount-1; j++)
                            {
                                //if (row.GetCell(j).ToString()==string.Empty)
                                //{
                                //    isEmpty++;
                                //}
                                newRow[j] = row.GetCell(j);

                                if (newRow[j].ToString() == string.Empty || newRow[j].ToString() == "NULL")
                                {
                                    newRow[j] = null;
                                    isEmpty++;
                                }
                            }
                            if (isEmpty == columnCount)
                            {
                                continue;
                            }
                            //添加到数据表中
                            table.Rows.Add(newRow);

                        }
                    }
                }
            }
            catch (Exception exMsg)
            {
                throw (exMsg);
                //throw (exMsg);
            }

            return table;
        }

    }
}
