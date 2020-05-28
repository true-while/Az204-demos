using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using StackExchange.Redis;


namespace RadisDemo
{
    
    class Program
    {

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

            var cString = configuration.GetSection("connectionString").Value;
            using (var radis = ConnectionMultiplexer.Connect(cString))
            {

                // Connection refers to a property that returns a ConnectionMultiplexer
                // as shown in the previous example.
                var cache = radis.GetDatabase();

                // Perform cache operations using the cache object...

                // Simple PING command
                string cacheCommand = "PING";
                Console.WriteLine("\nCache command  : " + cacheCommand);
                Console.WriteLine("Cache response : " + cache.Execute(cacheCommand).ToString());

                // Simple get and put of integral data types into the cache
                cacheCommand = "GET Message";
                Console.WriteLine("\nCache command  : " + cacheCommand + " or StringGet()");
                Console.WriteLine("Cache response : " + cache.StringGet("Message").ToString());

                cacheCommand = "SET Message \"Hello! The cache is working from a .NET Core console app!\"";
                Console.WriteLine("\nCache command  : " + cacheCommand + " or StringSet()");
                Console.WriteLine("Cache response : " + cache.StringSet("Message", "Hello! The cache is working from a .NET Core console app!").ToString());

                // Demonstrate "SET Message" executed as expected...
                cacheCommand = "GET Message";
                Console.WriteLine("\nCache command  : " + cacheCommand + " or StringGet()");
                Console.WriteLine("Cache response : " + cache.StringGet("Message").ToString());

                // Get the client list, useful to see if connection list is growing...
                cacheCommand = "CLIENT LIST";
                Console.WriteLine("\nCache command  : " + cacheCommand);
                Console.WriteLine("Cache response : \n" + cache.Execute("CLIENT", "LIST").ToString().Replace("id=", "id="));

            }

            Console.ReadLine();
        }
    }
}
