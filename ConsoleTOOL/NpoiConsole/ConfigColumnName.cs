using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpoiConsole
{
    public class Grid_ColumnName
    {
        public  List<string> ColumnNameList = new List<string>();
        
        public Grid_ColumnName()
        {
            ColumnNameList.Clear();
            ColumnNameList.Add("编号");
            ColumnNameList.Add("地市");
            ColumnNameList.Add("区县");
            ColumnNameList.Add("网格");
            ColumnNameList.Add("类别");
            ColumnNameList.Add("扫频日期");
            ColumnNameList.Add("扫频时间");
            ColumnNameList.Add("LTE日期");
            ColumnNameList.Add("LTE时间");
            ColumnNameList.Add("LTE设备号");
            ColumnNameList.Add("FDD日期");
            ColumnNameList.Add("FDD时间");
            ColumnNameList.Add("FDD设备号");
            ColumnNameList.Add("GSM日期");
            ColumnNameList.Add("GSM时间");
            ColumnNameList.Add("GSM设备号");
            ColumnNameList.Add("CSFB日期");
            ColumnNameList.Add("CSFB时间");
            ColumnNameList.Add("CSFB主叫");
            ColumnNameList.Add("CSFB被叫");
            ColumnNameList.Add("VOLTE日期");
            ColumnNameList.Add("VOLTE时间");
            ColumnNameList.Add("VOLTE主叫");
            ColumnNameList.Add("VOLTE被叫");
        }
    }

    public class Grid_Volte_ColumnName
    {
        public List<string> ColumnNameList = new List<string>();

        public Grid_Volte_ColumnName()
        {
            ColumnNameList.Clear();
            ColumnNameList.Add("编号");
            ColumnNameList.Add("测试日期");
            ColumnNameList.Add("测试时间");
            ColumnNameList.Add("地市");
            ColumnNameList.Add("区县");
            ColumnNameList.Add("网格");
            ColumnNameList.Add("类别");
            ColumnNameList.Add("主叫");
            ColumnNameList.Add("被叫");
            ColumnNameList.Add("log个数");
            ColumnNameList.Add("全程成功率");
            ColumnNameList.Add("MOS3.0占比");
        }
    }

    public class MainLine_Volte_ColumnName
    {
        public List<string> ColumnNameList = new List<string>();

        public MainLine_Volte_ColumnName()
        {
            ColumnNameList.Clear();
            ColumnNameList.Add("测试日期");
            ColumnNameList.Add("测试时间");
            ColumnNameList.Add("编号");
            ColumnNameList.Add("地市");
            ColumnNameList.Add("测试道路");
            ColumnNameList.Add("主叫");
            ColumnNameList.Add("被叫");
            ColumnNameList.Add("log个数");
        }
    }


    public class HighSubway_Volte_ColumnName
    {
        public List<string> ColumnNameList = new List<string>();

        public HighSubway_Volte_ColumnName()
        {
            ColumnNameList.Clear();
            ColumnNameList.Add("测试时间");
            ColumnNameList.Add("测试计划名称");
            ColumnNameList.Add("高速道路");   
            ColumnNameList.Add("主叫");       
            ColumnNameList.Add("被叫");       
            ColumnNameList.Add("GSM");        
            ColumnNameList.Add("TDD");        
            ColumnNameList.Add("FDD");        
        }
    }

    
}
