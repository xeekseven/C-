using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBulCopyConsole
{
    public class OperExcel
    {
        //Excel第一行为标题名
        public static DataTable readExcel(string filePath)
        {
            DataTable table = new DataTable();
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                XSSFWorkbook workbook = new XSSFWorkbook(fs);
                XSSFSheet sheet = (XSSFSheet)workbook.GetSheetAt(0);
                XSSFRow headRow = (XSSFRow)sheet.GetRow(0);

                //获取标题名
                for (int index = 0; index < headRow.Cells.Count; index++)
                {
                    DataColumn dc = new DataColumn(headRow.GetCell(index).ToString());
                    table.Columns.Add(dc); 
                }

                for (int i = sheet.FirstRowNum+1; i <= sheet.LastRowNum; i++)
                {
                    DataRow newRow = table.NewRow();
                    XSSFRow row = (XSSFRow)sheet.GetRow(i);
                    if (row != null)
                    {
                        if (row.GetCell(0) != null)
                        {
                            //复制整行数据
                            for (int j = row.FirstCellNum; j < row.Cells.Count; j++)
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
