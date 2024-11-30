using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{

    class Program
    {
        private static object _lock = new object();
        private static bool _eventOccurred = false;
        private static string _eventData;

        static void Main()
        {
            Thread producer = new Thread(Producer);
            Thread consumer = new Thread(Consumer);

            producer.Start();
            consumer.Start();

        }

        static void Producer()
        {
            for (int i = 0; i < 10; i++) 
            {
                Thread.Sleep(1000); 
                lock (_lock)
                {
                    _eventData = $"Event {i + 1}";
                    _eventOccurred = true;
                    Console.WriteLine($"Producer: {_eventData} generated");
                    Monitor.Pulse(_lock);
                }
            }
        }

        static void Consumer()
        {
            while (true)
            {
                lock (_lock)
                {
                    while (!_eventOccurred)
                    {
                        Monitor.Wait(_lock);
                    }

                    Thread.Sleep(500);
                    Console.WriteLine($"Consumer: {_eventData} processed\n");
                    _eventOccurred = false;

                    if (_eventData == "Event 10") 
                    {
                        break;
                    }
                }
            }
        }
    }

}
