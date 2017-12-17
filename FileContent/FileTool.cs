using IFileTool;
using System;
using System.IO;
public class  FileTool : IFileTool
{
    public void Readfile_not_create()
    {
        string filePath = "C:\\content.txt";
        StreamReader sr = new StreamReader(filePath,Encoding.GetCode("utf-8"));
        string readLineStr = string.Empty;
        while ((readLineStr=sr.readLine())!=null)
        {
            //save readLineStr and doSth;
        }
        sr.close();
    }
    public void Readfile_create()
    {
        string filePath = "C:\\content.txt";
        List<string> fileContentList = new List<string>();
        using(FileStream fs = new FileStream(filePath,FileMode.OpenOrCreate,FileAccess.Read))
        {
            //fs为字节
            using(StreamReader sr = new StreamReader(fs,Encoding.GetEncoding("utf-8")))
            {
                string readLineStr = string.Empty;
                while ((readLineStr=sr.readLine())!=null)
                {
                    fileContentList.Add(readLineStr);
                    //save readLineStr and doSth;
                }
            }
        }
    }
    //字符写入到流中固定编码
    public void Writefile_not_appent()
    {
        string filePath = "C:\\content.txt";
        List<string> fileContentList = new List<string>();
        using(StreamWriter sw = new StreamWriter(filePath,false,Encoding.GetEncoding("utf-8")))
        {
            foreach (var lineStr in fileContentList)
            {
                sw.WriteLine(lineStr);
            }
        }
    }
    public void Writefile_appent()
    {
        string filePath = "C:\\content.txt";
        List<string> fileContentList = new List<string>();
        using(StreamWriter sw = new StreamWriter(filePath,true,Encoding.GetEncoding("utf-8")))
        {
            foreach (var lineStr in fileContentList)
            {
                sw.WriteLine(lineStr);
            }
        }
    }
    public void Readfile_n2m()
    {
        string filePath = "C:\\content.txt";
        byte[] byteArray = new byte[100];
        char[] charArray = new char[100];
        string resultStr = string.Empty;
        using(FileStream fs = new FileStream(filePath,FileMode.OpenOrCreate,FileAccess.Read))
        {
            fs.Seek(0,SeekOrigin.Begin);
            fs.Read(byteArray,0,200);
            Decoder dec = Encoding.UTF8.GetDecoder();
            dec.GetChars(byteArray, 0, byteArray.Length, charArray, 0);
        }
        resultStr = Convert.ToString(charArray);
    }
}