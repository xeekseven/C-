using System;
using System.Collections.Generic;
using System.Text;

namespace OledDBOpExcel
{
    public class WorkParamColumnNames
    {
        private static WorkParamColumnNames Instance = null;
        public Dictionary<int, string> columnDic = new Dictionary<int, string>();

        public WorkParamColumnNames()
        { }

        public void InitColumnDic()
        {
            columnDic.Clear();
            columnDic.Add(0, "ID");
            columnDic.Add(1, "地市ID");
            columnDic.Add(2, "地市名称");
            columnDic.Add(3, "区县ID");
            columnDic.Add(4, "区县名称");
            columnDic.Add(5, "设备厂家ID");
            columnDic.Add(6, "设备厂家名称");
            columnDic.Add(7, "基站名称");
            columnDic.Add(8, "基站经度");
            columnDic.Add(9, "基站纬度");
            columnDic.Add(10, "覆盖类型");
            columnDic.Add(11, "小区名称");
            columnDic.Add(12, "小区号");
            columnDic.Add(13, "小区经度");
            columnDic.Add(14, "小区纬度");
            columnDic.Add(15, "ENodeBID");
            columnDic.Add(16, "CellID");
            columnDic.Add(17, "TAC");
            columnDic.Add(18, "ECI");
            columnDic.Add(19, "EARFCN");
            columnDic.Add(20, "PCI");
            columnDic.Add(21, "频段");
            columnDic.Add(22, "天线型号");
            columnDic.Add(23, "天线方向角");
            columnDic.Add(24, "天线下倾角");
            columnDic.Add(25, "天线挂高");
            columnDic.Add(26, "天线发射功率");
            columnDic.Add(27, "理想覆盖半径");
        }
        public static WorkParamColumnNames GetInstance()
        {
            if(Instance ==null)
            {
                Instance = new WorkParamColumnNames();
            }
            return Instance;
        }
    }
}
