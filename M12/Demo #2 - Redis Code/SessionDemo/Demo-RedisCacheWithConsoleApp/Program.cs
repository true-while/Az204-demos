using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;
namespace Demo_RedisCacheWithConsoleApp
{
    class Program
    {
        static string redisEndpoint;
        static string redisAccessKey;
        static IDatabase cache;
        static IServer server;

        static void Main(string[] args)
        {
            redisEndpoint = ConfigurationManager.AppSettings["RedisCacheName"].ToString();
            redisAccessKey = ConfigurationManager.AppSettings["RedisCachePassword"].ToString();
            // Connect to the Redis Cache Endpoint
            var redisConnection = ConnectionMultiplexer.Connect(string.Format("{0},ssl=true,password={1}", redisEndpoint, redisAccessKey));
            // Get a reference to the cache
            cache = redisConnection.GetDatabase();
            server = redisConnection.GetServer(redisConnection.GetEndPoints().First());
            Prompt();

        }

        private static void Prompt()
        {
            Console.WriteLine("Please select an option below...");
            Console.WriteLine("1 : Add Items to Azure Redis Cache Instance");
            Console.WriteLine("2 : Retrieve Items from Azure Redis Cache Instance");
            Console.WriteLine("3 : Add 1000s of Items to Azure Redis Cache Instance");
            Console.WriteLine("4 : Get Current Keys count");
            Console.WriteLine("5 : Quit the application");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddItemsToAzureRedisCache();
                    Prompt();
                    break;
                case "2":
                    RetrievItemsFromAzureRedisCache();
                    Prompt();
                    break;
                case "3":
                    AddMultipleItemsToAzureRedisCache();
                    Prompt();
                    break;
                case "4":
                     Console.WriteLine("Get Server Count {0}", server.Keys().Count());
                    Prompt();
                    break;
                case "5":
                    Environment.Exit(1);
                    break;
            };
        }


        private static void ConnectToAzureRedisCache()
        {
            try
            {
                Console.WriteLine("*****");
                Console.WriteLine("Successfully connected to AzureRedis Cache");
                Console.WriteLine("*****");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async void AddItemsToAzureRedisCache()
        {
            try
            {

                await cache.StringSetAsync("ModuleName", "Azure Redis Cache");
                await cache.StringSetAsync("Popularity", "Very High");
               
                // Update cache data (without retrieving it first)
                await cache.StringIncrementAsync("NumberOfHits", 125);

                // Add a Hash to the cache
                HashEntry[] userCreds = new HashEntry[]
                {
                    new HashEntry("username", "user1"),
                    new HashEntry("password", "p@ssword")
                };
                await cache.HashSetAsync("User", userCreds);
         
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void RetrievItemsFromAzureRedisCache()
        {
            try
            {
                // Retrieve simple data from cache
                Console.WriteLine("Module Name: {0}", cache.StringGet("ModuleName"));
                Console.WriteLine("Popularity: {0}", cache.StringGet("Popularity"));
                Console.WriteLine("NumberOfHits: {0}", cache.StringGet("NumberOfHits"));
                Console.WriteLine();

                // Retrieve Hash data from cache
                Console.WriteLine("User Credentials:");
                Console.WriteLine("\tUsername: {0}", cache.HashGet("User", "username"));
                Console.WriteLine("\tPassword: {0}", cache.HashGet("User", "password"));
                Console.WriteLine();

                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private static async void AddMultipleItemsToAzureRedisCache()
        {
            try
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < 2000; i++)
                {
                    await cache.StringSetAsync("Module " + rnd.Next(0,10000000), i + " : Azure Redis Cache");
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
