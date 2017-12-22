using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskConsole
{
    public class TaskTools
    {
        static ManualResetEventSlim _mainEvent = new ManualResetEventSlim(false);
        public static void TravelThroughGates(string threadName, int senconds)
        {
            Console.WriteLine("{0} falls to sleep", threadName);
            Thread.Sleep(TimeSpan.FromSeconds(senconds));
            Console.WriteLine("{0} waits for the gates to open!", threadName);
            _mainEvent.Wait();
            Console.WriteLine("{0} enters the gates!", threadName);
        }
        public static void ManualResetEventTh()
        {
            var t1 = new Thread(() => TravelThroughGates("Thread 1", 5));
            var t2 = new Thread(() => TravelThroughGates("Thread 2", 6));
            var t3 = new Thread(() => TravelThroughGates("Thread 3", 12));
            t1.Start();
            t2.Start();
            t3.Start();
            Thread.Sleep(TimeSpan.FromSeconds(6));
            Console.WriteLine("The gates are now open!");
            _mainEvent.Set();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _mainEvent.Reset();
            Console.WriteLine("The gates has been closed");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            Console.WriteLine("The gates are open for the sencond time!");
            _mainEvent.Set();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("The gates have been closed!");
            _mainEvent.Reset();
        }
        public static void TaskRun()
        {
            Task t = Task.Run(() =>
            {
                int ctr = 0;
                for (ctr = 0; ctr <= 1000000; ctr++)
                { }
                Console.WriteLine("Finished {0} loop iterations",ctr);
            });
            t.Wait();
        }
        public static void TaskStartNew()
        {
            Task t = Task.Factory.StartNew(() =>
            {
                int ctr = 0;
                for (ctr = 0; ctr <= 1000000; ctr++)
                { }
                Thread.Sleep(2000);
                Console.WriteLine("Finished {0} loop iterations", ctr);
            });
            Console.WriteLine("Finished {0} loop iterations", 0);
            t.Wait();
        }
        public static void TaskWait()
        {
            Task taskA = Task.Run(() => Thread.Sleep(2000));

            Console.WriteLine("taskA start :{0}", taskA.Status);
            try
            {
                taskA.Wait();
                Console.WriteLine("taskA Status: {0}", taskA.Status);
            }
            catch(AggregateException e)
            {
                throw (e);
            }
        }
        public static void TaskWaitB()
        {
            Task taskA = Task.Run(() => Thread.Sleep(2000));
            try
            {
                taskA.Wait(1000);
                bool completed = taskA.IsCompleted;
                Console.WriteLine("TaskA completed:{0},status:{1}", completed, taskA.Status);
                if(!completed)
                {
                    Console.WriteLine("Timed out be task completed");
                }
            }catch(AggregateException ex)
            {
                throw (ex);
            }
        }
        public static void CreateTask()
        {
            Action<object> action = (object obj) =>
            {
                Console.WriteLine("Task={0},obj = {1},Thread = {2}", Task.CurrentId, obj, Thread.CurrentThread.ManagedThreadId);
            };
            string paramStr = "Hello Task";
            Task t1 = new Task(action, paramStr);

            Task t2 = Task.Factory.StartNew(action, paramStr);
            t2.Wait();
            t1.Start();
            Task t3 = Task.Run(() =>
            {
                Console.WriteLine("Task={0},obj = {1},Thread = {2}", Task.CurrentId, paramStr, Thread.CurrentThread.ManagedThreadId);
            });
        }
        public static Task<int> GetSum(int n)
        {
            Task<int> result = new Task<int>(e=>Sum((int)e),n);
            result.Start();
            return result;
        }

        public static async  Task<int> GetSumS()
        {
            int result = 0;
            return result;
        }
        public static int Sum(int n)
        {
            int resultSum = 0;
            for(int index=1;index<=n;index++)
            {
                resultSum += index;
            }
            return resultSum;
        }
        public static int SumB()
        {
            int resultSum = 0;
            for (int index = 1; index <= 456566541; index++)
            {
                resultSum += index;
            }
            return resultSum;
        }

        public static Task<int> CreateOneTask(string name)
        {
            return new Task<int>(() => TaskMethod(name));
        }
        private static int TaskMethod(string name)
        {
            Console.WriteLine("Task {0} is running on a thread id{1}. Is thread pool thread:{2}", name,Thread.CurrentThread.ManagedThreadId,Thread.CurrentThread.IsThreadPoolThread);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return 42;
        }
        public static async void InvodeCreateTask()
        {
            TaskMethod("Main Thread Task");
            Task<int> task = CreateOneTask("Task 1");
            task.Start();
            int result = await task;
            Console.WriteLine("Result is:{0}", result);

            task = CreateOneTask("Task 2");
            task.RunSynchronously();
            result = task.Result;
            Console.WriteLine("Result is {0}", result);

            task = CreateOneTask("Task 3");
            Console.WriteLine(task.Status);
            task.Start();
            while(!task.IsCompleted)
            {
                Console.WriteLine(task.Status);
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            Console.WriteLine(task.Status);
            result = task.Result;
            Console.WriteLine("Result is {0}",result);
        }

        //取消任务
        private static int TaskMethod(string name,int seconds,CancellationToken token)
        {
            Console.WriteLine("Task {0} is running on a thread id{1}. Is thread pool thread:{2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            for (int i = 0; i < seconds;i++ )
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                if(token.IsCancellationRequested)
                {
                    return -1;
                }
            }
                
            return 42 * seconds;
        }
        public static void InvokeTask2()
        {
            var cts = new CancellationTokenSource();
            var longTask = new Task<int>(() => TaskMethod("task1 ", 10, cts.Token), cts.Token);
            Console.WriteLine(longTask.Status);
            cts.Cancel();
            Console.WriteLine(longTask.Status);
            Console.WriteLine("First task has been cancelled before execution");
            cts = new CancellationTokenSource();
            longTask = new Task<int>(() => TaskMethod("task2 ", 10, cts.Token), cts.Token);
            longTask.Start();
            for(int i = 0;i<5;i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine(longTask.Status);
            }
            Console.WriteLine("A TASK has been complete with result {0}", longTask.Result);

        }

        //并行运行任务
        private static int TaskMethod(string name,int seconds)
        {
            Console.WriteLine("Task {0} is running on a thread id{1}. Is thread pool thread:{2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
            return 42 * seconds;
        }

        public static void PanellRunTask()
        {
            var firstTask = new Task<int>(() =>TaskMethod("one Task",3));
            var secondTask = new Task<int>(() => TaskMethod("two Task", 2));
            var whenAllTask = Task.WhenAll(firstTask, secondTask);
            whenAllTask.ContinueWith(t =>
            {
                Console.WriteLine("The firset answer is {0},the second is{1}", t.Result[0], t.Result[1]);
            },TaskContinuationOptions.OnlyOnRanToCompletion);
            firstTask.Start();
            secondTask.Start();

            Thread.Sleep(TimeSpan.FromSeconds(4));
            var tasks = new List<Task<int>>();
            for (int i = 1; i < 4; i++)
            {
                int counter = i;
                var task = new Task<int>(() => TaskMethod(String.Format("Task {0}", counter), counter));
                tasks.Add(task);
                task.Start();
            }
            while (tasks.Count > 0)
            {
                var completedTask = Task.WhenAny(tasks).Result;
                tasks.Remove(completedTask);
                Console.WriteLine("A Task has been completed with result P{0}", completedTask.Result);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        //async与await
        private static async Task<string> GetInfoAsync(string name)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            return String.Format("Task {0} is running on a thread id{1} Is thread Poool thread {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);

        }
        public static Task AsynchronyWithTPL()
        {
            Task<string> t = GetInfoAsync("Task 1");
            Task t2 = t.ContinueWith(task => Console.WriteLine(t.Result), TaskContinuationOptions.NotOnFaulted);
            Task t3 = t.ContinueWith(task => Console.WriteLine(t.Exception.InnerException+"00"), TaskContinuationOptions.OnlyOnFaulted);
            return Task.WhenAny(t2, t3);
        }

        public async static Task AsynchroyWithAwait()
        {
            try
            {
                string result = await GetInfoAsync("Task 2");
                Console.WriteLine(result);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
            }
        }
        public static void invoke()
        {
            Task t = AsynchronyWithTPL();
            t.Wait();

            Task t2 = AsynchroyWithAwait();
            t2.Wait();
        }

        //async lambda
        private async static Task AsynchronousProcessing()
        {
            Func<string, Task<string>> asyncLambda = async name =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return String.Format("Task {0} is running on a thread id{1} Is thread Poool thread {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);

            };
            string result = await asyncLambda("async lambda");
            Console.WriteLine(result);
        }
        public static void AsyncLabda()
        {
            Task t = AsynchronousProcessing();
            t.Wait();
        }
        //连续 顺序 async
        public async static Task  AsynchronyWithAwait()
        {
            try
            {
                string result = await GetInfoAsync2("Async 1");
                Console.WriteLine(result);
                result = await GetInfoAsync2("Async 2");
                Console.WriteLine(result);
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public async static Task<string> GetInfoAsync2(string name)
        {
            Console.WriteLine("Task {0} started!", name);
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (name == "TPL2")
                throw new Exception("ex bbb");
            return String.Format("Task {0} is running on a thread id{1} Is thread Poool thread {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);
        }
        public static void ContinueAsync()
        {
            Task t = AsynchronyWithAwait();
            t.Wait();
        }
        //并行 执行
        public async static Task AsynchronousProcessingAll()
        {
            Task<string> t1 = GetInfoAsyncAll("Task 1", 3);
            Task<string> t2 = GetInfoAsyncAll("Task 2", 5);
            
            //等两个任务运行完了后才运行
            string[] results = await Task.WhenAll(t1, t2);
            foreach(string result in results)
            {
                Console.WriteLine(result);
            }
        }
        public async static Task<string> GetInfoAsyncAll(string name,int senconds)
        {
            await Task.Delay(TimeSpan.FromSeconds(senconds));
            //throw new Exception("bom");
            return String.Format("Task {0} is running on a thread id{1} Is thread Poool thread {2}", name, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsThreadPoolThread);

        }
        public static void TaskWhenAll()
        {
            Task t = AsynchronousProcessingAll();
            t.Wait();
        }
        //异步中的异常处理
        private async static Task AsyncHronousProcessingEx()
        {
            Console.WriteLine("1 Single exception");
            try
            {
                string result = await GetInfoAsyncAll("Task 1", 2);
                Console.WriteLine(result);

            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception detail {0}",ex);
            }
            Console.WriteLine();
            Console.WriteLine("2 . Multiple exception ");

            Task<string> t1 = GetInfoAsyncAll("Task 1", 3);
            Task<string> t2 = GetInfoAsyncAll("Task 2", 5);
            try
            {
                string[] results = await Task.WhenAll(t1, t2);
                Console.WriteLine(results.Length);
            }catch(Exception ex)
            {
                Console.WriteLine("Exception detail {0}", ex);
            }
            Console.WriteLine();
            Console.WriteLine("2 multiple exception with aggregateException");

            t1 = GetInfoAsyncAll("Task 1", 3);
            t2 = GetInfoAsyncAll("Task 2", 2);
            Task<string[]> t3 = Task.WhenAll(t1, t2);
            try
            {
                string[] results = await t3;
                Console.WriteLine(results.Length);              
            }
            catch
            {
                var ae = t3.Exception.Flatten();
                var exceptions = ae.InnerExceptions;
                Console.WriteLine("Exceptions caught {0}", exceptions.Count);
                foreach(var e in exceptions)
                {
                    Console.WriteLine("Exception details :{0}",e);
                    Console.WriteLine();
                }
            }
        }
        public static void InvokeAsyncException()
        {
            Task t = AsyncHronousProcessingEx();
            t.Wait();
        }
    }
}
