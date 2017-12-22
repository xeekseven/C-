using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvConsole
{
    public class CsvTool
    {

        public static string[] GetContainCommaLineArray(string originStr)
        {
            string regStr = @"""[\w\n\d\u4e00-\u9fa5\(\)\（\）&\*#%+-]+,+[,\w\n\d\u4e00-\u9fa5\(\)\（\）&\*#%+-]+""";
            List<string> reStrList = new List<string>();
            foreach (var item in Regex.Matches(originStr, regStr))
            {
                reStrList.Add(item.ToString());
            }
            string newStr = Regex.Replace(originStr, regStr, "==占位符==");
            string[] lineArray = newStr.Split(',');
            int lineCount = 0;
            for (int arrayIndex = 0; arrayIndex < lineArray.Length; arrayIndex++)
            {
                string lineStr = lineArray[arrayIndex];
                if (lineStr.Equals("==占位符=="))
                {
                    lineArray[arrayIndex] = reStrList[lineCount].Replace("\"",string.Empty);
                    lineCount++;
                }
            }

            return lineArray;
        }
        public static DataTable readCsv(string fileFullName,bool isFirstRow = true)
        {
            DataTable datatable = new DataTable();

            try
            {
                using (FileStream fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Read))
                {
                    string readLineStr = string.Empty;
                    string[] lineStrArray = null;
                    int rowCount = 0;
                    using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                    {

                        while ((readLineStr = sr.ReadLine()) != null)
                        {
                            rowCount++;
                            bool isAllEmpty = true;
                            lineStrArray = readLineStr.Split(',');
                            if (isFirstRow)
                            {
                                for (int index = 0; index < lineStrArray.Length; index++)
                                {
                                    DataColumn dc = new DataColumn(lineStrArray[index].ToString());
                                    datatable.Columns.Add(dc);
                                }
                                isFirstRow = false;
                            }
                            else
                            {
                                if (lineStrArray.Length > datatable.Columns.Count)
                                {
                                    lineStrArray = GetContainCommaLineArray(readLineStr);

                                    //throw (new Exception("第" + rowCount + "行此行内容中含有逗号！不合法,分隔符请用【|】"));
                                }
                                if (lineStrArray.Length != datatable.Columns.Count)
                                {
                                    throw (new Exception("第" + rowCount + "行此行内容中不合法。"));

                                }
                                foreach (string lineStr in lineStrArray)
                                {
                                    if (lineStr.Contains("==占位符=="))
                                    {
                                        throw (new Exception("第" + rowCount + "行此行内容中不合法,可能某列既含有逗号【,】有含有引号【\"\"】"));
                                    }
                                }
                                for (int arrayIndex = 0; arrayIndex < lineStrArray.Length; arrayIndex++)
                                {
                                    if (lineStrArray[arrayIndex].ToString() != string.Empty && Regex.IsMatch(lineStrArray[arrayIndex].ToString(), "[nN][uU][lL][lL]") == false)
                                    {
                                        isAllEmpty = false;
                                    }
                                    else
                                    {
                                        lineStrArray[arrayIndex] = null;
                                    }
                                }
                                if (isAllEmpty)
                                {
                                    continue;
                                }
                                DataRow newRow = datatable.NewRow();
                                for (int rowIndex = 0; rowIndex < datatable.Columns.Count; rowIndex++)
                                {

                                    newRow[rowIndex] = lineStrArray[rowIndex];
                                }
                                datatable.Rows.Add(newRow);
                            }

                        }
                    }
                }
            }catch(Exception exMsg)
            {
                throw (exMsg);
            }

            return datatable;
        }

        public static bool writeCsv(string fileFullName,DataTable datatable,bool isFirstRow = true)
        {
            try
            {

                using (FileStream fs = new FileStream(fileFullName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("gb2312")))
                    {
                        string lineStr = string.Empty;
                        if (isFirstRow)
                        {
                            foreach (DataColumn headItem in datatable.Columns)
                            {
                                lineStr += String.Format(",{0}", headItem.ColumnName.ToString());
                            }
                            lineStr.Substring(0, 1);
                            sw.WriteLine(lineStr);
                            isFirstRow = false;
                        }

                        foreach (DataRow row in datatable.Rows)
                        {
                            lineStr = string.Empty;
                            for (int columnIndex = 0; columnIndex < datatable.Columns.Count; columnIndex++)
                            {
                                string colStr = row[columnIndex].ToString().Replace("\"", "\"\"");
                                if (colStr.Contains(",") || colStr.Contains("\n") || colStr.Contains("\r"))
                                {
                                    colStr = String.Format("\"{0}\"", colStr);
                                }
                                lineStr += String.Format(",{0}", colStr);
                            }
                            lineStr.Substring(0, 1);
                            sw.WriteLine(lineStr);
                        }
                        
                    }
                }
            }catch(Exception exMsg)
            {
                throw (exMsg);
            }
            
            return true;
        }
    }
}
