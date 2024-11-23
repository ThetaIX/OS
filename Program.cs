using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{

    class Program
    {
        private static Mutex mutex = new Mutex();
        private static string eventData = null; 
        private static bool eventOccurred = false;

        static void Main(string[] args)
        {
            Thread producer = new Thread(Producer);
            Thread consumer = new Thread(Consumer);

            producer.Start();
            consumer.Start();

            producer.Join();
            consumer.Join();
        }

        static void Producer()
        {
            for (int i = 1; i <= 10; i++) 
            {
                Thread.Sleep(1000); 
                mutex.WaitOne();

                eventData = $"Event {i}";
                eventOccurred = true;
                Console.WriteLine($"Producer: {eventData} generated");

                mutex.ReleaseMutex();
            }
        }

        static void Consumer()
        {
            while (true)
            {
                mutex.WaitOne();

                if (eventOccurred)
                {
                    Thread.Sleep(500);
                    Console.WriteLine($"Consumer: {eventData} processed");
                    eventOccurred = false;

                    if (eventData == "Event 10")
                    {
                        mutex.ReleaseMutex();
                        break;
                    }
                }

                mutex.ReleaseMutex();
            }
        }
    }

}
