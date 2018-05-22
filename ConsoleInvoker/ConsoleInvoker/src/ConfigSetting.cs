using ConsoleInvoker.src.DataConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInvoker.src
{
    public class ConfigSetting
    {
        private static ConfigSetting Instance = null;
        public ConsoleInvoker.src.DataConfig.configClass.ConfigToolClass ConfigJson = new ConsoleInvoker.src.DataConfig.configClass.ConfigToolClass();

        public static ConfigSetting GetInstance()
        {
            if (Instance == null)
            {
                Instance = new ConfigSetting();
            }
            return Instance;
        }

        public bool InitConfig(string fileName)
        {
            if(!File.Exists(fileName)){
                return false;
            }
            string jsonText = new StreamReader(fileName, Encoding.GetEncoding("gb2312")).ReadToEnd();
            GetInstance().ConfigJson = JsonConvert.DeserializeObject<ConsoleInvoker.src.DataConfig.configClass.ConfigToolClass>(jsonText);
            int count = GetInstance().ConfigJson.configList.Count;
            return true;
        }
    }
}
