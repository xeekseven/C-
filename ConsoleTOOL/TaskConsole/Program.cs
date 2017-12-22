using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskConsole
{
    class Program
    {
        static CountdownEvent _countdown = new CountdownEvent(2);
        static void PerformOperation(string message,int seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine(message);
            _countdown.Signal();
        }

        static  void Main(string[] args)
        {
            TaskTools.InvokeAsyncException();
            //TaskTools.ManualResetEventTh();
            dynamic a = 1;
            //TaskTools.TaskStartNew();
            //TaskTools.CreateTask();
            //ThreadPool.QueueUserWorkItem(dosth, 5);
            //Console.WriteLine("主线程运行至此！");
            //dosth(45);
            Console.ReadKey();
        }

        public static async void dosth(object param)
        {
            int result = await TaskTools.GetSum(100000);
            Console.WriteLine(result);
        }
       
    }
}
