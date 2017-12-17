public interface IFileContentTool
{
    void Readfile_not_create();
    void Readfile_create();
    void Writefile_not_create();
    void Writefile_create();
    void Readfile_n2m();
}

//StreamReader:以固定编码中从字节流中读出字符
//StreamWriter:把字符写为字节流中的固定编码
//操作的是字符

//FileStream:是对文件中字节流来进行处理，
//操作字节与字节数组
//随机文件访问(访问文件中间某点的数据)，就必须由FileStream对象执行.