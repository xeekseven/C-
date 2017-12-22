using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataTableOperator
{
    class Program
    {
        static void Main(string[] args)
        {
            DataView dv = new DataView();
            float lo =0;
            float la =0;
            string filterStr = string.Format("基站经度 < {0} and 基站经度 > {1}",lo+0.5,lo-0.5);
            dv.RowFilter = filterStr;
        }
    }
}
