using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelModelConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //LazyTools.InvokeProcess();
            ParallelPipingTools.InvokePipe();
           
            
            Console.ReadLine();
        }
    }
}
