using System;
using System.Threading;

namespace Lab2.IncrementInThreads
{
    unsafe class Program
    {
        static int* ptr;
        static object locker = new object { };
        static unsafe void Main(string[] args)
        {
            var i = 0;
            ptr = &i;
            var thread1 = new Thread(DoWork);
            var thread2 = new Thread(DoWork);

            thread1.Start();
            thread2.Start();

            Console.WriteLine(startTime);
            thread1.Join();
            thread2.Join();
            Console.WriteLine(i);
        }

        static DateTime startTime = DateTime.Now.AddSeconds(1);

        public static unsafe void DoWork()
        {
            //Console.WriteLine(Thread.GetCurrentProcessorId());
            while (DateTime.Now < startTime)
            { } // Критичний сегмент (mutex)
            //lock (locker)
                for (int j = 0; j < 1000; j++) (*ptr)++; 
        }
    }
}
