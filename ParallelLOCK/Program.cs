using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLOCK
{
    class Program
    {
        static void Main(string[] args)
        {
            const Int32 nMax = 10000;
            Int32 n = 0, m = 0;
            DateTime start;
            TimeSpan dauerN, dauerM;

            start = DateTime.Now;

            /*
             *  Task.Run setzt .net ab Version 4.5 voraus!
             *  in .net 4.0 gibt es zwar Task, nicht jedoch dessen Methode Run!
             */

            var up = Task.Run(() =>
            {
                for (Int32 i = 0; i < nMax; i++)
                {
                    //n++;
                    n = n + 1;
                }
            });

            for (Int32 i = 0; i < nMax; i++)
            {
                //n--;
                n = n - 1;
                Console.Write(n);
            }

            up.Wait();
            dauerN = DateTime.Now.Subtract(start);


            //return;


            object lockB = new object();
            object lockA = new object();

            start = DateTime.Now;
            up = Task.Run(() =>
                {
                    for (Int32 i = 0; i < nMax; i++)
                    {
                        lock (lockB)
                        {
                            m++;
                        }
                    }
                });

            for (Int32 i = 0; i < nMax; i++)
            {
                lock (lockB)
                {
                    m--;
                }
                Console.Write(m);
            }

            up.Wait();
            dauerM = DateTime.Now.Subtract(start);

            Console.Clear();
            Console.WriteLine($"{dauerN} ohne lock: {n}");
            Console.WriteLine($"{dauerM} mit  lock: {m}");

            //return;


            // Console.ReadLine();


            start = DateTime.Now;
            up = Task.Run(() =>
                {
                    lock (lockA)
                    {
                        Console.WriteLine($"{DateTime.Now.Subtract(start)} up Locked A");
                        System.Threading.Thread.Sleep(3000);
                        lock (lockB)
                            Console.WriteLine($"{DateTime.Now.Subtract(start)} Locked A and B");
                    }
                });


            
            lock (lockB)
            {
                Console.WriteLine($"{DateTime.Now.Subtract(start)} Locked B");
                lock (lockA)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine($"{DateTime.Now.Subtract(start)} Locked B and A");
                }
            }

            up.Wait();


            Console.WriteLine($"{DateTime.Now.Subtract(start)} Ende");
            Console.ReadLine();


        }
    }

}