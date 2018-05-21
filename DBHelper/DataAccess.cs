using System;
using System.Text;
using System.Reflection;
using DBHelper.Interface;

namespace DBHelper
{
    

    public class DataAccess
    {
        //private static readonly string AssemlyName = "DBHelperConsole";

        //private static readonly string db = "DataOperator.RealizeClass.SqlServer.SqlServerCommon";
        public static IDataCommonOperator CreateDataOperator(string DBTypeName)
        {
            string AssemlyName = "DBHelper";
            string className = String.Format("DBHelper.RealizeClass.{0}.{0}Common", DBTypeName);
            return (IDataCommonOperator)Assembly.Load(AssemlyName).CreateInstance(className);
        }
        public static IDataCmdOperator CreateCmdOperator(string DBTypeName)
        {
            string AssemlyName = "DBHelper";
            string className = String.Format("DBHelper.RealizeClass.{0}.{0}Cmd", DBTypeName);
            return (IDataCmdOperator)Assembly.Load(AssemlyName).CreateInstance(className);
        }
        
    }
}
