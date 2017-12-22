using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TimerConsole
{
    class Program
    {
        public static void doTickSth(object data)
        {
            Console.WriteLine(data.ToString());
        }
        static void Main(string[] args)
        {
            Timer tmr = new Timer(doTickSth,"outoo",long.Parse("2000".ToString()),long.Parse("2000".ToString()));

            Console.ReadLine();
        }
    }
}
