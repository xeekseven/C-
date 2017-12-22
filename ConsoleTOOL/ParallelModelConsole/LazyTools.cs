using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelModelConsole
{
   
    public class LazyTools
    {
        public interface IHasValue
        {
            ValueToAccess Value { get; }
        }
        public class ValueToAccess
        {
            private readonly string _text;
            public ValueToAccess(string text)
            {
                _text = text;
            }
            public string Text
            {
                get { return _text; }
            }
        }

        public class BCLThreadSafeFactory : IHasValue
        {
            private ValueToAccess _value;
            public ValueToAccess Value
            {
                get 
                {
                    return LazyInitializer.EnsureInitialized(ref _value, Compute);
                }
            }
        }
        public class UnsafeState : IHasValue
        {
            private ValueToAccess _value;
            public ValueToAccess Value
            {
                get
                {
                    if (_value == null)
                    {
                        _value = Compute();
                    }
                    return _value;
                }
            }
        }
        public class DoubleCheckedLocking : IHasValue
        {
            private object _syncRoot = new object();
            private volatile ValueToAccess _value;
            public ValueToAccess Value
            {
                get
                {
                    if (_value == null)
                    {
                        lock (_syncRoot)
                        {
                            if (_value == null) _value = Compute();
                        }
                    }
                    return _value;
                }
            }
        }

        public class BCLDoubleChecked : IHasValue
        {
            private object _syncRoot = new object();
            private ValueToAccess _value;
            private bool _initialized = false;

            public ValueToAccess Value
            {
                get
                {
                    return LazyInitializer.EnsureInitialized(ref _value, ref _initialized, ref _syncRoot, Compute);
                }
            }
        }
        public static ValueToAccess Compute()
        {
            Console.WriteLine("The Value is being construced on thread id {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return new ValueToAccess(string.Format("Construected on thread id {0}", Thread.CurrentThread.ManagedThreadId));
        }
        public static void Worker(IHasValue state)
        {
            Console.WriteLine("Worker runs on thread id{0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("State value:{0}", state.Value.Text);
        }
        public static void Worker(Lazy<ValueToAccess> state)
        {
            Console.WriteLine("Worker runs on thread id {0}",Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("State value :{0}",state.Value.Text);
        }
        public static async Task ProcessAsynchronously()
        {
            var uNsafeState = new UnsafeState();
            Task[] tasks = new Task[4];

            for(int i = 0;i<4;i++)
            {
                tasks[i] = Task.Run(() => Worker(uNsafeState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("--------------");

            var firstState = new DoubleCheckedLocking();
            for(int i =0;i<4;i++)
            {
                tasks[i] = Task.Run(() => Worker(firstState));
            }
            await Task.WhenAll(tasks);

            Console.WriteLine("-------------");
            var secondState = new BCLDoubleChecked();
            for(int i=0;i<4;i++)
            {
                tasks[i] = Task.Run(() => Worker(secondState));
            }
            await Task.WhenAll(tasks);

            Console.WriteLine("-------------");
            var thirdState = new Lazy<ValueToAccess>(Compute);
            for (int i = 0; i < 4; i++)
            {
                tasks[i] = Task.Run(() => Worker(thirdState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("-----------------");

            var fourthState = new BCLThreadSafeFactory();
            for(int i=0;i<4;i++)
            {
                tasks[i] = Task.Run(() => Worker(fourthState));
            }
            await Task.WhenAll(tasks);
            Console.WriteLine("-----------------");
        }

        public static void InvokeProcess()
        {
            var t = ProcessAsynchronously();
            t.GetAwaiter().GetResult();
        }
    }
}
