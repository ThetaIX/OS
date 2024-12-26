using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{

    class Program
    {
        private static object Lock = new object();
        private static bool EventOccurred = false;
        private static string EventData;

        static void Main()
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
            for (int i = 0; i < 10; i++) 
            {
                Thread.Sleep(1000); 
                lock (Lock)
                {
                    EventData = $"Event {i + 1}";
                    EventOccurred = true;
                    Console.WriteLine($"Producer: {EventData} generated");
                    Monitor.Pulse(Lock);
                }
            }
        }

        static void Consumer()
        {
            while (true)
            {
                lock (Lock)
                {
                    while (!EventOccurred)
                    {
                        Monitor.Wait(Lock);
                    }

                    Thread.Sleep(500);
                    Console.WriteLine($"Consumer: {EventData} processed\n");
                    EventOccurred = false;

                    if (EventData == "Event 10") 
                    {
                        break;
                    }
                }
            }
        }
    }

}
