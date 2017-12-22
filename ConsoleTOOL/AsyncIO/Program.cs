using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncIO
{
    class Program
    {
        static void Main(string[] args)
        {
            //AsyncIOTools.InvokeFileIO();
            
            //OwnHttpServerTools.InvokeHttpS();
            Task t = AsyncDB.InvodeAsyncDB();
            t.GetAwaiter().GetResult();
            Console.ReadLine();
        }
    }
}
