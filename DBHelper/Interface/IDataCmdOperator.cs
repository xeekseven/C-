using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.Interface
{
    public interface IDataCmdOperator
    {
        string GetTypeCmdStr(string cmdStr, string newStrFilePath, string sqlName);
        string GetImportScriptCmdText(string serverName,string port, string dbName, string userName, string password, string newStrFilePath, string sLogName);

        string GetNewScriptFilePath(string filePath, string sqlName, string newDBName);
        string GetClearCmdText(string cmdPath, string userName, string password, string serviceName,string port, string fileName,string dbName = null);
        string GetResumeCmdText(string bcpCmdPath,string dbName,string tbName,string dirName,string userName,string password,string serviceName,string port);

        string CopyInfilePath(string sourceFilePath, string dbKeyName);

        string GetOutDataCmdText(string bcpCmdPath, string dbName, string tbName, string dirName, string userName, string password, string serviceName,string port, string strTop, string dbKeyName);

        string GetExecCmdText(string sqlCmdPath, string execFilePath,string logPath, string userName, string password, string serviceName, string port, string dbName);
        void FixOutfile(List<string> fileNameList);
    }
}
