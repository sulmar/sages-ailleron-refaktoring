﻿using System;
using System.Threading.Tasks;

namespace SingletonPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Singleton Pattern!");

           // ConfigManagerTest();


             // LoggerTest();

            LoadBalancerTest();

            Console.ReadKey();
        }

        private static void ConfigManagerTest()
        {
            // Module #1
            ConfigManager configManager = ConfigManager.Instance;
            configManager.Set("name", "Marcin");

            // Module #2
            ConfigManager other = ConfigManager.Instance;
            object result = other.Get("name");
            Console.WriteLine(result);

            if (ReferenceEquals(configManager, other))
            {
                Console.WriteLine("The same instances");
            }
            else
            {
                Console.WriteLine("Not the same instances");
            }


        }

        private static void LoggerTest()
        {
            MessageService messageService = new MessageService(Logger.Instance);
            PrintService printService = new PrintService(Logger.Instance);
            messageService.Send("Hello World!");
            printService.Print("Hello World!", 3);

            if (ReferenceEquals(messageService.logger, printService.logger))
            {
                Console.WriteLine("The same instances");
            }
            else
            {
                Console.WriteLine("Different instances");
            }
        }

        private static void LoadBalancerTest()
        {
            Task.Run(() => LoadBalanceRequestTest(15));
            Task.Run(() => LoadBalanceRequestTest(15));
        }

        private static void LoadBalanceRequestTest(int numberOfRequests)
        {
            LoadBalancer loadBalancer = LoadBalancer.Instance;

            for (int i = 0; i < numberOfRequests; i++)
            {
                Server server = loadBalancer.NextServer;
                Console.WriteLine($"Send request to: {server.Name} {server.IP}");
            }
        }

        

        
    }




  
}
