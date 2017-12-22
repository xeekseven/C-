using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBulCopyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable datatable = OperExcel.readExcel("D:\\ex.xlsx");
            string connectStr = @"Data Source=CCHCWNBSFEJ1JFZ\SQLEXPRESS;Initial Catalog=importdb;User ID=dtauser;Password=dtauser";
            SqlBulCopyClass.SqlBulCopyDataTable(connectStr,"usertb",datatable);
            Console.ReadKey();
        }
    }
}
