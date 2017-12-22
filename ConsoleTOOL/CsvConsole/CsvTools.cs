using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

namespace CsvConsole
{
    public class CsvTools
    {
        public static DataTable readCsv(string filePath,ref string errorInfo)
        {
            List<List<String>> csvList = new List<List<String>>();
            DataTable datatable = new DataTable();
            //从csv中读出数据;
            try
            {
                StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312"));
                String readLineStr = null;
                int readIndex = 1;
                int rowIndex = 1;
                int headIndex = 1;
                while ((readLineStr = sr.ReadLine()) != null)
                {
                    if(rowIndex==headIndex)
                    {
                        string[] lineStrArray = readLineStr.Split(',');
                        for (int index = 0; index < lineStrArray.Length; index++)
                        {
                            DataColumn dc = new DataColumn(lineStrArray[index].ToString());
                            datatable.Columns.Add(dc);
                        }
                        rowIndex++;
                        continue;
                    }
                    rowIndex++;
                    if(rowIndex < 2)
                    {
                        continue;
                    }
                    
                    List<string> resultList = new List<string>();
                    string[] contentList = Regex.Split(readLineStr, ",\"", RegexOptions.IgnoreCase);
                    
                    for (int contentIndex = 0; contentIndex < contentList.Length; contentIndex++)
                    {
                        if (contentIndex == 0)
                        {
                            String[] oneList = contentList[contentIndex].Split(',');
                            foreach (string item in oneList)
                            {
                                resultList.Add(item);
                            }
                        }
                        else if (contentIndex == contentList.Length - 1)
                        {
                            string[] listTwo = Regex.Split(contentList[contentList.Length - 1], "\",", RegexOptions.IgnoreCase);
                            resultList.Add(listTwo[0]);
                            string[] realListTwo = listTwo[1].Split(',');
                            foreach (string item in realListTwo)
                            {
                                resultList.Add(item);
                            }
                        }
                        else
                        {
                            resultList.Add(contentList[contentIndex]);
                        }
                    }
                    if (resultList.Count != datatable.Columns.Count)
                    {
                        errorInfo += String.Format("第{0}行内容不合法，列数量不对，发生了错误，请查看!\n", rowIndex - 1);
                        continue;
                    }
                    try
                    {
                        DataRow newRow = datatable.NewRow();
                        for (int newRowIndex = 0; newRowIndex < datatable.Columns.Count; newRowIndex++)
                        {
                            newRow[newRowIndex] = resultList[newRowIndex];
                        }
                        datatable.Rows.Add(newRow);
                    }
                    catch(Exception ex)
                    {
                        throw (ex);
                    }
                    
                    csvList.Add(resultList);
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return datatable;
            //return csvList;
        }
    }
}
