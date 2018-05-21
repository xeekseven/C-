using DBHelper.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBHelper.RealizeClass.SqlServer
{
    public class SqlServerCmd : IDataCmdOperator
    {
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
                using (StreamReader sr = new StreamReader(newStrFilePath, Encoding.Default))
                {
                    string line = sr.ReadToEnd();

                    if (line.IndexOf("EXEC MTNOH_AAA_DB.dbo.proc_表升级_升级表脚本生成", StringComparison.CurrentCultureIgnoreCase) > 0)
                    {
                        CmdTypeStr = String.Format("{0}|{1}", "Table", line);
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
                    else if (line.IndexOf("OBJECTPROPERTY(id, N'IsProcedure') = 1", StringComparison.CurrentCultureIgnoreCase) > 0)
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
                    else
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
                string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format(",{0}", port);
      
                string cmd = string.Format("echo +++ {0} +++ {1} {2}.log", newStrFilePath, ">>", sLogName)
                + "\r\n" +"chcp 936\r\n"+
                    string.Format("\"SQLCMD.EXE\"  -U{0} -P{1} -S{2}{6} -d{3} -i\"{4}\" >> {5}.log\r\n"
                    , userName
                    , password
                    , serverName
                    , dbName
                    , newStrFilePath
                    , sLogName
                    , portString
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
            sqlName = sqlName.Replace(".sql", "-");

            string newSqlPath = AppDomain.CurrentDomain.BaseDirectory + "newSql";
            if (!Directory.Exists(newSqlPath))
            {
                Directory.CreateDirectory(newSqlPath);
            }
            string time = DateTime.Now.Year + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0');
            newSqlPath = newSqlPath + "\\" + time;
            if (!Directory.Exists(newSqlPath))
            {
                Directory.CreateDirectory(newSqlPath);
            }
            string newFilePath = newSqlPath + "\\" + sqlName + newDBName + ".sql";
            string newDbStr = newDBName.ToUpper();
            string contents = System.IO.File.ReadAllText(filePath, Encoding.GetEncoding("gb2312"));
            string reStrOne = @"^EXEC\s[a-zA-Z0-9_]+\.dbo\.proc.+\s'[a-zA-Z0-9_]+'";
            string reStrTwo = @"^USE\s\[[a-zA-Z0-9_]+\]";
            string newContents = contents;
            string needChangeDB = "====@@@@**特殊符号**@@@@====";
            if (Regex.IsMatch(contents, reStrOne))
            {
                string originStr = Regex.Match(contents, reStrOne).Groups[0].Value;
                needChangeDB = Regex.Match(originStr, @"'[a-zA-Z0-9_]+'$").Groups[0].Value.Replace("'", "");
            }
            else if (Regex.IsMatch(contents, reStrTwo))
            {
                string originStr = Regex.Match(contents, reStrTwo).Groups[0].Value;
                needChangeDB = Regex.Match(originStr, @"\[[a-zA-Z0-9_]+\]").Groups[0].Value.Replace("[", "").Replace("]", "");
            }
            newContents = contents.Replace(needChangeDB, newDbStr);
            File.WriteAllText(newFilePath, newContents, Encoding.GetEncoding("gb2312"));

            return newFilePath;
        }
        public string GetClearCmdText(string cmdPath, string userName, string password, string serviceName,string port, string fileName,string dbName = null)
        {
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format(",{0}", port);
            string clearCmdText = String.Format("\r\nchcp 936\r\n \"{0}\"  -U{1} -P{2} -S{3}{5} -i\"{4}\"\r\n", cmdPath, userName, password, serviceName, fileName, portString);
            return clearCmdText;
        }
        public string GetResumeCmdText(string bcpCmdPath, string dbName, string tbName,string dirPath, string userName, string password, string serviceName,string port)
        {
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format(",{0}", port);
            string resumeCmdText = String.Format("\r\nchcp 936\r\n \"{0}\" [{1}].[dbo].[{2}] in \"{3}\\{2}.bcp\" -b1000 -c -U{4} -P{5} -S{6}{7} -o \"{3}\\{2}.log\" \r\n\r\n", bcpCmdPath, dbName, tbName, dirPath, userName, password, serviceName, portString);
            return resumeCmdText;
        }
        public string CopyInfilePath(string sourceFilePath,string dbKeyName)
        {
            return sourceFilePath;
        }
        public string GetOutDataCmdText(string bcpCmdPath, string dbName, string tbName, string dirName, string userName, string password, string serviceName, string port, string strTop, string dbKeyName)
        {
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format(",{0}", port);
            string outDataCmdText = String.Format("\r\nchcp 936\r\n\r\n \"{0}\" {1}..{2} out \"{3}\\{8}\\{2}.bcp\" -b1000 -c  -U{4} -P{5} -S{6}{9} {7} -o \"{3}\\{8}\\{2}.log\" \r\n", bcpCmdPath, dbName, tbName, dirName, userName, password, serviceName, strTop, dbKeyName, portString);
            //"\r\n\"" + bcpPath + "\" " + db + ".." + tbName + " out \"" + dirPath + "\\" + dbName + "\\" + tbName + ".bcp\" -b1000 -c  -U" + userName +
              //                                    " -P" + password + " -S" + serverName + strTop + " -o \"" + dirPath + "\\" + dbName + "\\" + tbName +".log\"\r\n";
            return outDataCmdText;

        }
        public void FixOutfile(List<string> fileNameList)
        {
            return;
        }
        public string GetExecCmdText(string sqlCmdPath, string execFilePath, string logPath, string userName, string password, string serviceName, string port, string dbName)
        {
            string portString = String.IsNullOrEmpty(port) ? string.Empty : String.Format(",{0}", port);

            string execCmdText ="\r\nchcp 936\r\n"+ String.Format(@" {0} -U {1} -P {2} -d {3} -S {4}{5} -i {6} >> {7}", sqlCmdPath, userName, password, dbName, serviceName,portString, execFilePath, logPath);

            return execCmdText;

        }
    }
}
