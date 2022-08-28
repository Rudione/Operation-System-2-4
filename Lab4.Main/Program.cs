using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lab4.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var fTime = GetNumber("f calculation time");
            var gTime = GetNumber("g calculation time");
            var fArg = GetNumber("f argument");
            var gArg = GetNumber("g argument");
            var askTime = GetNumber("ask time (in seconds)");

            var pf = StartProcess(@"C:\Users\User\source\repos\OS\Lab4.F\bin\Debug\net5.0\Lab4.F.exe", new List<object> { fTime, fArg });
            var pg = StartProcess(@"C:\Users\User\source\repos\OS\Lab4.G\bin\Debug\net5.0\Lab4.G.exe", new List<object> { gTime, gArg });
            Console.WriteLine(Calculate(pf, pg, askTime));
        }

        public static Process StartProcess(string command, List<object> args)
        {
            var startInfo = new ProcessStartInfo(command)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            var process = Process.Start(startInfo);

            foreach (var arg in args)
                process.StandardInput.WriteLine(arg);

            return process;
        }

        public static int? Calculate(Process fp, Process gp, int askTime)
        {
            bool hasFinished = fp.HasExited && gp.HasExited;
            bool printedF = false; // чи готовий результат
            bool printedG = false;
            int fResult = 0;
            int gResult = 0;


            var stopwatch = new Stopwatch();

            while (!hasFinished)
            {
                if (!stopwatch.IsRunning)
                    stopwatch.Start();

                if (stopwatch.ElapsedMilliseconds >= 1000 * askTime)
                {

                    var shouldContinue = GetBoolAnswer("Canculation runs too long, do you want to continue [y] or abort the calculation [n]?");

                    if (!shouldContinue)
                        break;

                    stopwatch.Restart();
                }

                if (fp.HasExited && !printedF) // Якщо функція закінчила рахувати але результат ще не виведено
                {
                    fResult = GetNumberFromProcess(fp);
                    Console.WriteLine($"f finished: {fResult}");
                    printedF = true;
                }

                if (gp.HasExited && !printedG)
                {
                    gResult = GetNumberFromProcess(gp);
                    Console.WriteLine($"g finished: {gResult}");
                    printedG = true;
                }

                hasFinished = fp.HasExited && gp.HasExited; // Якщо функції закінчили рахувати і готовий результат
            }

            if(hasFinished)
            {
                return fResult * gResult;
            }

            return null;
        }

        public static bool GetBoolAnswer(string question)
        {
            Console.WriteLine(question);
            Console.Write("Answer: ");
            var answer = Console.ReadLine();
            while (answer != "y" && answer != "n")
            {
                Console.Write("Enter a valid answer [y] or [n]: ");
                answer = Console.ReadLine();
            }

            return answer == "y";
        }

        public static int GetNumberFromProcess(Process process)
        {
            var str = process.StandardOutput.ReadToEnd();
            return Convert.ToInt32(str);
        }

        public static int GetNumber(string what)
        {
            Console.Write($"Enter the {what}: ");
            string str = Console.ReadLine();
            while(str.Any(x => !char.IsDigit(x)) || str == "") // Перевіряємо чи є це числом
            {
                Console.Write($"Enter the integer number for {what}: ");
                str = Console.ReadLine();
            }

            return Convert.ToInt32(str);
        }
    }
}
