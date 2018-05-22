using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInvoker.src.DataConfig
{
    public class configClass
    {
        public class Param{
            public string paramName;
            public string paramValue;
        }
        public class configChild
        {
            public string ClassName;
            public string ConsoleName;
            public string ExecFilePath;
            public string ParamName;
            public string ParamValue;
        }
        public class ConfigClass
        {
            public string ToolBarName;
            public List<configChild> ToolChildArray;
        }
        public class ConfigToolClass
        {
            public List<ConfigClass> configList;
        }
    }
}
