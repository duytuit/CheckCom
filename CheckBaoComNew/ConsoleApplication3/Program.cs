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
             string info = @"D:\du an 2019\CheckCom\CheckBaoComNew\CheckCom_Version2\bin\Debug\Dulieuxuatan\CheckCom\06-07-2019 Chieu.txt";
            FileStream fs = new FileStream(info, FileMode.Open, FileAccess.Read,FileShare.Read);
            using (StreamReader sr = new StreamReader(fs))
            {
                string[] lines = sr.ReadToEnd().Split('\n');
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i].Split('-')[0].Contains("000405"))
                    {
                        Console.WriteLine(lines[i].Split('-')[1]);
                    }
                }
            }
           
          //  File.ReadAllLines(fs.ToString());
           // string[] lines = File.ReadAllLines(fs.ToString());
            //if (lines.Count() > 0)
            //{
            //    for (int i = 0; i < lines.Count(); i++)
            //    {
            //        if (lines[i].Split('-')[0].Contains("000405"))
            //        {
                      
            //          Console.WriteLine(lines[i].Split('-')[1]);
                   
            //        }
            //    }
            //}


            //   string lines = File.ReadAllLines(info)[0];
            //  Console.WriteLine(lines);
            //if (lines.Count() > 0)
            //{
            //    for (int i = 0; i < lines.Count(); i++)
            //    {
            //        Console.WriteLine(lines[i].ToString());
            //    }

            //}
            //string info = @"\\192.84.100.39\d";
            //string[] filesPaths = Directory.GetFiles(info);
            //foreach(string f in filesPaths)
            //{
            //    Console.WriteLine(f);
            //}

            //string[] lines = File.ReadAllLines(info);
            //if (lines.Count() > 0)
            //{
            //    for (int i = 0; i < lines.Count(); i++)
            //    {
            //        Console.WriteLine(lines[i].ToString());
            //    }

            //}
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
