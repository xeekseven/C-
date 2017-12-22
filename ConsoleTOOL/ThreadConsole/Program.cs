using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadConsole
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            //ThreadTools.TestThreadPool();

            ThreadPool.QueueUserWorkItem( (e)=>
            {
                Console.WriteLine(e);
            },100);

            Console.ReadKey();
        }
    }
}
