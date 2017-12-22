using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CsvConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable datatable = CsvTool.readCsv(@"D:\10.98.15.115_2GAlarm_accur_20171205072100.csv");

            //if(CsvTool.writeCsv(@"C:\Users\Administrator\Desktop\BigData\EXCEL\csv\写入.csv",datatable))
            //{
            //    Console.WriteLine("写入成功");
            //}
            //DataTable csvList = CsvTools.readCsv(@"D:\KPI数据\网优平台-重客APP-gsm-12月05日6点.csv");
            //DataTable csvList = null;
            //String errorInfo = null;
            //try
            //{
            //     csvList = CsvTools.readCsv(@"D:\KPI数据\网优平台-重客APP-gsm-12月05日6点.csv",ref errorInfo);
            //}catch(Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //Console.WriteLine(errorInfo);

            //foreach (List<string> csvL in csvList)
            //{
            //    Console.WriteLine(csvL.Count);
            //}

            //foreach (DataRow itemList in csvList.Rows)
            //{

            //    Console.WriteLine();
            //}


            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));

            Console.ReadLine();
        }
    }
}
