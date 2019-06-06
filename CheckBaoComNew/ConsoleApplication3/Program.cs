using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        static Thread thread1, thread2;
        static int sum = 0;
        static void Main(string[] args)
        {

            string info = @"D:\du an 2019\CheckCom\CheckBaoComNew\CheckCom_Version2\bin\Debug\CheckCom\06-05-2019 Chieu.txt";
            string[] lines = File.ReadAllLines(info);
            if (lines.Count() > 0)
            {
                for (int i = 0; i < lines.Count(); i++)
                {
                    Console.WriteLine(lines[i].ToString());
                }
                   
            }
            Console.ReadKey();
        }
        private static async void Sample() { await Task.Run(() => sum = sum + 1); }
        private static async void Sample2() { await Task.Run(() => sum = sum + 10); }

        private static void start()
        {
            thread1 = new Thread(new ThreadStart(Sample));
            thread2 = new Thread(new ThreadStart(Sample2));
            thread1.Start();
            // thread2.Start();
            thread1.Join();
            // thread2.Join();

            Console.WriteLine(sum);
            Console.WriteLine();
        }
        // static Thread thread1, thread2;
        // //static int sum = 0;
        // static void Main(string[] args)
        // {
        //     start();
        //     Console.ReadKey();

        // }
        //private static void A()
        // {
        //     for (int i = 0; i <= 100; i++)
        //     {
        //         Console.WriteLine(i.ToString());
        //     }
        //     Console.WriteLine("A đã đọc xong");   // Báo cáo đã đọc xong
        // }

        // private static void B()
        // {
        //     for (int i = 0; i <= 100; i++)
        //     {
        //         Console.WriteLine(i.ToString());
        //     }
        //     Console.WriteLine("B đã đọc xong");   // Báo cáo đã đọc xong
        // }

        // private static void start()
        // {
        //     thread1 = new Thread(new ThreadStart(A));
        //     thread2 = new Thread(new ThreadStart(B));
        //     thread1.Start();
        //     thread2.Start();
        //     thread1.Join();
        //     thread2.Join();

        //     Console.WriteLine("Cuộc thi kết thúc");
        //     Console.WriteLine();
        // }
    }
}
