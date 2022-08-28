using System;
using System.Threading;

namespace Lab4.F
{
    class Program
    {
        public static int CalculationTime { get; set; }

        static void Main(string[] args)
        {
            CalculationTime = Convert.ToInt32(Console.ReadLine());
            int x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(F(x));
        }

        public static int F(int x)
        {
            Thread.Sleep(1000 * CalculationTime);
            return x;
        }
    }
}
