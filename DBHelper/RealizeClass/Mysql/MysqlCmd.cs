using DBHelper.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.RealizeClass.Mysql
{
    public class MysqlCmd : IDataCmdOperator
    {
        private string SearchMysqlCmd(bool IsAt)
        {
            if (File.Exists(@"C:\Program Files\MySQL\MySQL Server 5.6\bin\mysql.exe"))
            {
                if (IsAt)
                {
                    return @"C:\Program Files\MySQL\MySQL Server 5.6\bin\mysql.exe";
                }
                return "C:\\Program Files\\MySQL\\MySQL Server 5.6\\bin\\mysql.exe";
            }
            else
            {
                if (IsAt)
                {
                    return @"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe";
                }
                return "C:\\Program Files\\MySQL\\MySQL Server 5.7\\bin\\mysql.exe";
            }
        }
        private string SearchMysqlDumpCmd(bool IsAt)
        {
            if (File.Exists(@"C:\Program Files\MySQL\MySQL Server 5.6\bin\mysqldump.exe"))
            {
                if (IsAt)
                {
                    return @"C:\Program Files\MySQL\MySQL Server 5.6\bin\mysqldump.exe";
                }
                return "C:\\Program Files\\MySQL\\MySQL Server 5.6\\bin\\mysqldump.exe";
            }
            else
            {
                if (IsAt)
                {
                    return @"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysqldump.exe";
                }
                return "C:\\Program Files\\MySQL\\MySQL Server 5.7\\bin\\mysqldump.exe";
            }
        }
        public string GetTypeCmdStr(string cmdStr, string newStrFilePath, string sqlName)
        {
            try
            {
                Dictionary<string, List<string>> resulltDic = new Dictionary<string, List<string>>();
                resulltDic.Add("Table", new List<string>());
                resulltDic.Add("View", new List<string>());
                resulltDic.Add("ProcTable", new List<string>());
                resulltDic.Add("ProcDure", new List<string>());
                resulltDic.Add("Func", new List<string>());
                string CmdTypeStr = string.Empty;
                using (StreamReader sr = new StreamReader(newStrFilePath, Encoding.GetEncoding("utf-8")))
                {
                    string line = sr.ReadToEnd();

                    if (line.StartsWith("CREATE TABLE"))
                    {
                        CmdTypeStr = String.Format("{0}|{1}", "Table", cmdStr);
                        //resulltDic["Table"].Add(cmdStr);
                        //tables.Add(cmd);
                    }
                    else if (line.IndexOf("OBJECTPROPERTY(id, N'IsView') = 1", StringComparison.CurrentCultureIgnoreCase) > 0)
                    {
                        string flag = "drop view [dbo].[";
                        int indexStart = line.IndexOf(flag, StringComparison.CurrentCultureIgnoreCase) + flag.Length;

                        int indexEnd = line.IndexOf("]", indexStart);
                        string viewName = line.Substring(indexStart, indexEnd - indexStart);

                        string viewStr = String.Format("{0}|{1},{2}", cmdStr, viewName, line);
                        CmdTypeStr = String.Format("{0}|{1}", "View", viewStr);
                        //resulltDic["View"].Add(viewStr);
                    }
                    else if (line.IndexOf("PROCEDURE", StringComparison.CurrentCultureIgnoreCase) > 0)
                    {
                        if (sqlName.IndexOf("_表升级_") > 0)
                        {
                            CmdTypeStr = String.Format("{0}|{1}", "ProcTable", cmdStr);
                            //resulltDic["ProcTable"].Add(cmdStr);
                        }
                        else
                        {
                            CmdTypeStr = String.Format("{0}|{1}", "ProcDure", cmdStr);
                            //resulltDic["ProcDure"].Add(cmdStr);
                        }
                    }
                    else if (line.IndexOf("FUNCTION", StringComparison.CurrentCultureIgnoreCase) > 0)
                    {
                        CmdTypeStr = String.Format("{0}|{1}", "Func", cmdStr);
                        //resulltDic["Func"].Add(cmdStr);
                    }

                }

                return CmdTypeStr;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public string GetImportScriptCmdText(string serverName, string port, string dbName, string userName, string password, string newStrFilePath, string sLogName)
        {
            try
            {
                string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format("-P{0}", port);
                string cmd = string.Format("echo +++ {0} +++ {1} {2}.log", newStrFilePath, ">>", sLogName)
                + "\r\n" +
                    "CHCP 65001 \r\n"+
                    string.Format(@"""{7}"" -u{0} -p{1} -h{2} {6} -D{3}<""{4}"" >> {5}.log"
                    , userName
                    , password
                    , serverName
                    , dbName
                    , newStrFilePath
                    , sLogName
                    , portString
                    ,SearchMysqlCmd(true)
                    );
                return cmd;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public string GetNewScriptFilePath(string filePath, string sqlName, string newDBName)
        {
            return filePath;
        }
        public string GetClearCmdText(string cmdPath, string userName, string password, string serviceName,string port, string fileName, string dbName = null)
        {
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format("-P{0}", port);
            string clearCmdText = "\r\nCHCP 65001\r\n" +string.Format(@" ""{0}"" -u{1} -p{2} -h{3} {7} -D{4}<""{5}"" >> {6}.log", SearchMysqlCmd(true), userName, password, serviceName, dbName, fileName, Path.GetFileNameWithoutExtension(fileName), portString);
            return clearCmdText;
        }
        public string GetResumeCmdText(string bcpCmdPath, string dbName, string tbName, string dirName, string userName, string password, string serviceName,string port)
        {
            //"C:\\ProgramData\\MySQL\\MySQL Server 5.6\\Uploads\\tb_cfg_cell.bcp"
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format("-P{0}", port);
            //string resumeCmdText = string.Format("\"C:\\Program Files\\MySQL\\MySQL Server 5.6\\bin\\mysql.exe\" -u{0} -p{1} -h{2} {6} -D{3} -e \"LOAD DATA INFILE '{4}' INTO TABLE {5} CHARACTER SET gbk FIELDS TERMINATED BY '\\t' lines terminated by '\\r\\n';\"", userName, password, serviceName, dbName, String.Format("{0}\\{1}.bcp", dirName, tbName).Replace(@"\", @"\\"), tbName, portString);
            string resumeCmdText = string.Format("\r\nCHCP 65001 \r\n \"{7}\" -u{0} -p{1} -h{2} {6} {3} < \"{4}\" ", userName, password, serviceName, dbName, String.Format("{0}\\{1}.bcp", dirName, tbName).Replace(@"\", @"\\"), tbName, portString, SearchMysqlCmd(false));

            resumeCmdText += "\r\n";
            return resumeCmdText;
        }
        public string CopyInfilePath(string sourceFilePath,string dbKeyName)
        {
            string destFilePath = String.Format("{0}\\{1}\\{2}", "C:\\ProgramData\\MySQL\\MySQL Server 5.6\\Uploads",dbKeyName, Path.GetFileName(sourceFilePath));
            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, destFilePath, true);
            }
            return destFilePath;

        }
        public string GetOutDataCmdText(string bcpCmdPath, string dbName, string tbName, string dirName, string userName, string password, string serviceName, string port, string strTop, string dbKeyName)
        {
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format("-P{0}", port);
            //string mysqlTmpFileName = String.Format("C:\\ProgramData\\MySQL\\MySQL Server 5.6\\Uploads\\{0}\\{1}.bcp", dbKeyName, tbName).Replace(@"\", @"\\");
            string mysqlTmpFileName = String.Format("{0}\\{1}", dirName, dbKeyName);
            if (!Directory.Exists((mysqlTmpFileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(mysqlTmpFileName));
            }
            string outDataCmdText = String.Format("\r\nCHCP 65001 \r\n \"{9}\"  -u{0} -p{1} -h{2} {8} {3} {4} > \"{5}\\{6}\\{7}.bcp\"", userName, password, serviceName, dbName, tbName, dirName, dbKeyName, tbName, portString, SearchMysqlDumpCmd(false));

            outDataCmdText += "\r\n";
            return outDataCmdText;
        }
        public void FixOutfile(List<string> fileNameList)
        {
            foreach (string fileItem in fileNameList)
            {
                if (File.Exists(fileItem))
                {
                    List<string> lineList = File.ReadAllLines(fileItem, Encoding.GetEncoding("utf-8")).ToList();
                    lineList.RemoveAt(0);
                    File.WriteAllLines(fileItem,lineList, Encoding.GetEncoding("utf-8"));
                }

            }
        }
        public string GetExecCmdText(string sqlCmdPath, string execFilePath, string logPath, string userName, string password, string serviceName, string port, string dbName)
        {
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format("-P{0}", port);
            string execCmdText = string.Format("\r\nCHCP 65001 \r\n \"{7}\"  -u{0} -p{1} -h{2} {3} -D{4} <{5} >> {6}", userName, password, serviceName, portString,dbName , execFilePath, logPath, SearchMysqlCmd(false));
            execCmdText += "\r\n";
            return execCmdText;
        }
    }
}
