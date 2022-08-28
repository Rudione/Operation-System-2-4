using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Lab4.MatrixMultiply
{
    class Program
    {
        public static int[,] generate_rand_matrix(int rows, int columns)
        {
            var r = new Random();
            var result = new int[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] = r.Next(0, 100);

            return result;
        }

        public static void print_matrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write(matrix[i, j] + " ");

                Console.WriteLine();
            }
        }

        public static int Read()
        {
            StringBuilder sb = new StringBuilder();
            int c = Console.Read();
            while (char.IsWhiteSpace((char)c))
                c = Console.Read();

            while (!char.IsWhiteSpace((char)c))
            {
                sb.Append((char)c);
                c = Console.Read();
            }

            return Convert.ToInt32(sb.ToString());
        }

        public static int[,] read_matrix_from_file(string path, int rows, int columns)
        {
            var stream = File.OpenRead(path);
            var result = new int[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    result[i, j] = Read();

            return result;
        }

        public static void compute_el(out int result, int[,] a, int[,] b, int i, int j)
        {
            if (a.GetLength(1) != b.GetLength(0))
            {
                throw new Exception();
            }

            result = 0;
            int m = a.GetLength(1);

            for (int t = 0; t < m; t++)
                result += a[i, t] * b[t, j];

            Console.WriteLine($"[{i}, {j}] computed.");
        }

        static void Main(string[] args)
        {
           // args = new string[] { "4", "3", "5" }; // Матриці
            int argc = args.Length;

            if (!(argc == 3 || (argc == 6 && Convert.ToInt32(args[3]) == 0)))
            {
                return;
            }
            int[,] first = new int[1, 1];
            int[,] second = new int[1, 1];

            int n = Convert.ToInt32(args[0]);
            int m = Convert.ToInt32(args[1]);
            int k = Convert.ToInt32(args[2]);

            if (argc == 3)
            {
                first = generate_rand_matrix(n, m);
                second = generate_rand_matrix(m, k);
            }

            if (argc == 6)
            {
                first = read_matrix_from_file(args[4], n, m);
                second = read_matrix_from_file(args[5], m, k);
            }


            print_matrix(first);
            print_matrix(second);

            List<Thread> threads = new List<Thread>();

            int[,] result = new int[n, k];

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < k; ++j)
                {
                    int a = i;
                    int b = j;
                    var thread = new Thread(() => compute_el(out result[a, b], first, second, a, b));
                    thread.Start();
                    threads.Add(thread);
                }
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            print_matrix(result);
        }
    }
}
