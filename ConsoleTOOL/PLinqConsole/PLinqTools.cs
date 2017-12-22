using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PLinqConsole
{
    public class PLinqTools
    {
        public static string EmulateProcessingOld(string taskName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));
            Console.WriteLine("{0} task was processed on a thread id {1}", taskName, Thread.CurrentThread.ManagedThreadId);
            return taskName;
        }
        public static void InvokeParallel()
        {
            Parallel.Invoke(
                () => EmulateProcessingOld("Task1"),
                () => EmulateProcessingOld("Task2"),
                () => EmulateProcessingOld("Task3"));
            var cts = new CancellationTokenSource();
            var result = Parallel.ForEach(
                Enumerable.Range(1, 30),
                new ParallelOptions
                {
                    //CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount,
                    //TaskScheduler = TaskScheduler.Default
                },
                (i, state) =>
                {
                    
                    Console.WriteLine(i);
                    if(i>2)
                    {
                        state.Break();
                        Console.WriteLine("Loop is stopped : {0},{1}", state.IsStopped,i);
                    }
                });
            Console.WriteLine("-------");
            Console.WriteLine("IsCompleted: {0}", result.IsCompleted);
            Console.WriteLine("Lowest break iteration: {0}", result.LowestBreakIteration);
        }

        //并行查询
        public static void PrintInfo(string typeName)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));
            Console.WriteLine("{0} type was processed on a thread id {1}", typeName, Thread.CurrentThread.ManagedThreadId);

        }
        public static string EmulateProcessing(string typeName)
        {
            //Thread.Sleep(TimeSpan.FromMilliseconds(new Random(DateTime.Now.Millisecond).Next(250, 350)));
            Console.WriteLine("{0} type was processed on a thread id {1}", typeName, Thread.CurrentThread.ManagedThreadId);
            return typeName;
        }
        public static IEnumerable<string> GetTypes()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from type in assembly.GetExportedTypes()
                   where type.Name.StartsWith("S")
                   select type.Name;
        }
        public static void InvokeQuery()
        {
            var sw = new Stopwatch();
            sw.Start();
            var query = from t in GetTypes()
                        select EmulateProcessing(t);
            
            sw.Stop();
            //foreach (string typeName in query)
            //{
            //    PrintInfo(typeName);
            //}
            Console.WriteLine("----");
            Console.WriteLine("1Sequential LINQ query");
            Console.WriteLine("Time elapsed:{0}", sw.Elapsed);
            Console.WriteLine("Press enter to continue ....");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();

            sw.Start();
            var parallelQuery = from t in
                                    ParallelEnumerable.AsParallel(GetTypes())
                                select EmulateProcessing(t);
            
            sw.Stop();
            //foreach (string typeName in parallelQuery)
            //{
            //    PrintInfo(typeName);
            //}
            Console.WriteLine("---");
            Console.WriteLine("2Parallel LINQ query The results are being merged on a single thread");
            Console.WriteLine("Time elapsed:{0}", sw.Elapsed);
            Console.WriteLine("Press ENTER to continue ...");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();

            //耗时最短
            sw.Start();
            parallelQuery = from t in GetTypes().AsParallel()
                            select EmulateProcessing(t);
            //var list = parallelQuery.ToList();
            sw.Stop();
            //parallelQuery.ForAll(PrintInfo);
            Console.WriteLine("----");
            Console.WriteLine("3Parallel LINQ query result being processed in parallel");
            Console.WriteLine("Time elapsed :{0}", sw.Elapsed);
            Console.WriteLine("Press ENTER to continue ..");
            Console.ReadLine();
            Console.Clear();
            sw.Reset();
            sw.Start();
            query = from t in GetTypes().AsParallel().AsSequential()
                    select EmulateProcessing(t);
            
            sw.Stop();
            //foreach (var typeName in query)
            //{
            //    PrintInfo(typeName);
            //}
            Console.WriteLine("----");
            Console.WriteLine("4Parallel LINQ query, transformed into sequential");
            Console.WriteLine("Time elapsedL:{0}",sw.Elapsed);
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Console.Clear();
        }
        //AsParallel Execption
        public static void InvokeParallelException()
        {
            IEnumerable<int> numbers = Enumerable.Range(-5, 10);
            var query = from number in numbers select 100 / number;

            try
            {
                foreach(var item in query)
                {
                    Console.WriteLine(item);
                }
            }catch(DivideByZeroException ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("-----");

            var parallelQuery = from number in numbers.AsParallel() select 100 / number;
            try
            {
                
                parallelQuery.ForAll(Console.WriteLine);
            }
            catch(DivideByZeroException ex)
            {
                Console.WriteLine(ex);
            }
            catch(AggregateException e)
            {
                e.Flatten().Handle(ex =>
                    {
                        if(ex is DivideByZeroException)
                        {
                            Console.WriteLine("Divided by zero - aggregate");
                        }
                        return true;
                    });
            }
            Console.WriteLine("-----");
            Console.WriteLine("Parallel Linq query processing results merging");
        }
        //聚合功能
        public static ConcurrentDictionary<char,int> AccumulateLettersInfomation(ConcurrentDictionary<char,int> taskTotal,string item)
        {
            foreach(var c in item)
            {
                if(taskTotal.ContainsKey(c))
                {
                    taskTotal[c] = taskTotal[c] + 1;
                }
                else
                {
                    taskTotal[c] = 1;
                }
            }
            //Console.WriteLine("{0} type was aggregated on thread id:{1}", item, Thread.CurrentThread.ManagedThreadId);
            return taskTotal;
        }

        public static ConcurrentDictionary<char,int> MergeAccumulators(ConcurrentDictionary<char,int> taskTotal)
        {
            ConcurrentDictionary<char, int> total = new ConcurrentDictionary<char, int>();
            foreach(var key in taskTotal.Keys)
            {
                if(total.ContainsKey(key))
                {
                    total[key] = total[key] + taskTotal[key];
                }
                else
                {
                    total[key] = taskTotal[key];
                }
            }
           // Console.WriteLine("Total aggregate value was thread id {0}", Thread.CurrentThread.ManagedThreadId);
            return total;

        }
        public static void InvokeJH()
        {
            List<int> numList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            ParallelQuery<int> pList = numList.AsParallel();
            var pResult = pList.Aggregate(
                (result, item) => 
                { 
                    result += item; 
                    return result; 
                }
                );
            Console.WriteLine(pResult);
            var parallelQuery = from t in GetTypes().AsParallel() select t;

            //第一个参数种子值为累加器的初始值
            var parallelAggregator = parallelQuery.Aggregate(
                () =>
                {
                    ConcurrentDictionary<char, int> cdic = new ConcurrentDictionary<char, int>();
                    cdic.TryAdd('#', 9595);
                    return cdic;
                },
                //对单个分区的处理 返回单个分区结果
                (taskTotal, item) => AccumulateLettersInfomation(taskTotal, item),
                //对单个分区结果进行聚合，返回最终累加器结果
                (total, taskTotal) => MergeAccumulators(taskTotal),
                //对最后结果的的转换
                total =>
                {
                    total.TryAdd('-', 10000);
                    return total;
                });
            Console.WriteLine();
            Console.WriteLine("There were the names");
            var orderedKeys = from k in parallelAggregator.Keys
                              orderby parallelAggregator[k] descending
                              select k;
            foreach (var c in orderedKeys)
            {
                Console.WriteLine("letter {0} --- {1}times", c, parallelAggregator[c]);
            }
        }
    }
}
