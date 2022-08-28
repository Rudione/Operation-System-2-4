using System;
using System.Threading;

namespace Lab4.G
{
    class Program
    {
        public static int CalculationTime { get; set; }

        static void Main(string[] args)
        {
            CalculationTime = Convert.ToInt32(Console.ReadLine());
            int x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(G(x));
        }

        public static int G(int x)
        {
            Thread.Sleep(1000 * CalculationTime);
            return x;
        }
    }
}