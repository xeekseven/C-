using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiConsole
{
    class Program
    {
        static void Main(string[] args)
        {
           
            //List<string> list = new List<string>();
            //Grid_ColumnName gcn = new Grid_ColumnName();
            //DataTable table = OperExcel.readExcel(@"D:\download\17年7月测试时间统计v1.xlsx",gcn.ColumnNameList,0,2); 
            //Grid_Volte_ColumnName gvcn = new Grid_Volte_ColumnName();
            //DataTable tableVolte = OperExcel.readExcel(@"D:\download\17年7月测试时间统计v1.xlsx", gvcn.ColumnNameList, 1, 3);
            //MainLine_Volte_ColumnName mvcn = new MainLine_Volte_ColumnName();
            //DataTable tableMainLine = OperExcel.readExcel(@"D:\download\17年7月测试时间统计v1.xlsx", mvcn.ColumnNameList, 2, 3);
            //HighSubway_Volte_ColumnName hvcn = new HighSubway_Volte_ColumnName();
            //DataTable tableHighSubway = OperExcel.readExcel(@"D:\download\17年7月测试时间统计v1.xlsx", hvcn.ColumnNameList, 3, 3);

            //foreach (DataColumn item in tableVolte.Columns)
            //{
            //    Console.Write(item.ColumnName + "\t");
            //}
            //Console.WriteLine();

            //for (int rowIndex = 0; rowIndex < tableVolte.Rows.Count; rowIndex++)
            //{
            //    for (int columnIndex = 0; columnIndex < tableVolte.Columns.Count; columnIndex++)
            //    {
            //        Console.Write(tableVolte.Rows[rowIndex][columnIndex] + "\t");
            //    }
            //    Console.WriteLine();
            //}

            DataTable table = ExcelTools.readExcel(@"D:\APP\APP_DATA\excel\重客投诉跟踪20171128V1-软件开发用.xlsx", "Sheet1", 1);
            for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                {
                    Console.Write(table.Rows[rowIndex][columnIndex] + "\t");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
