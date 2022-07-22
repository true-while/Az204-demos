using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace TheCloudShopsSelector
{
    public class Program
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "<your endpoint here>";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "<your primary key>";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "AZ204Demo";
        private string containerId = "TheDemo";

        public static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Beginning operations...\n");
                Program p = new Program();
                await p.GetStartedDemoAsync();

            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        public async Task GetStartedDemoAsync()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            //create db and containers
            await this.CreateDatabaseAsync();
            await this.CreateContainerAsync();

            //run seed method to load initial data
            await this.QueryItemsAsync();
            await this.ReplaceFamilyItemAsync("o3", "Redmond");
        }

        /// <summary>
        /// Create the container if it does not exist. 
        /// Specify "/OrderAddress/City" as the partition key since we're storing different types of documents.
        /// </summary>
        /// <returns></returns>
        private async Task CreateContainerAsync()
        {
            // Create a new container
            this.container = (Container)await this.database.CreateContainerIfNotExistsAsync(containerId, "/OrderAddress/City");
            Console.WriteLine("Created Container: {0}\n", this.container.Id);
        }

        /// <summary>
        /// Create the database if it does not exist
        /// </summary>
        private async Task CreateDatabaseAsync()
        {
            // Create a new database
            this.database = (Database) await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", this.database.Id);
        }

        private async Task CreateDocumentsIfNotExists(Order order) 
        {
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<Order> readResponse = await this.container.ReadItemAsync<Order>(order.id, new PartitionKey(order.OrderAddress.City));
                Console.WriteLine("Item in database with id: {0} already exists\n", readResponse.Resource.id);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Create an item in the container.
                ItemResponse<Order> createResponse = await this.container.CreateItemAsync<Order>(order, new PartitionKey(order.OrderAddress.City));

                // Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse. We can also access the RequestCharge property to see the amount of RUs consumed on this request.
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", createResponse.Resource.id, createResponse.RequestCharge);
            }

        }
        /// <summary>
        /// Add Family items to the container
        /// </summary>
        private async Task AddItemsToContainerAsync()
        {
            // Create a objects
            Customer customer1 = new Customer() { IsActive = true, Name= "Level4you" };
            Customer customer2 = new Customer() { IsActive = true, Name = "UpperLevel" };
            Customer customer3 = new Customer() { IsActive = false, Name = "Channel-9" };

            Product product1 = new Product() { ProductName= "Book" };
            Product product2 = new Product() { ProductName = "Food" };
            Product product3 = new Product() { ProductName = "Coffee" };

            Order order1 = new Order()
            {
                id = "o1",
                OrderNumber = "NL-21",
                OrderCustomer = customer1,
                OrderAddress = new Address { State = "WA", County = "King", City = "Seattle" },
                OrderItems = new[] {
                    new OrderItem() { ProductItem  = product1, Count = 7 },
                    new OrderItem() { ProductItem  = product3, Count = 1 } 
                }
            };
            Order order2 = new Order()
            {
                id = "o2",
                OrderNumber = "NL-22",
                OrderCustomer = customer2,
                OrderAddress = new Address { State = "WA", County = "King", City = "Redmond" },
                OrderItems = new[] {
                    new OrderItem() { ProductItem = product3, Count = 99 },
                    new OrderItem() { ProductItem = product2, Count = 5 },
                    new OrderItem() { ProductItem = product1, Count = 1 }
                }
            };
            Order order3 = new Order()
            {
                id = "o3",
                OrderNumber = "NL-23",
                OrderCustomer = customer2,
                OrderAddress = new Address { State = "WA", County = "King", City = "Redmond" },
                OrderItems = new[] {
                    new OrderItem() { ProductItem = product2, Count = 2 }
                }
            };

            //create orders
            await CreateDocumentsIfNotExists(order1);
            await CreateDocumentsIfNotExists(order2);
            await CreateDocumentsIfNotExists(order3);
        }

        private async Task QueryItemsAsync()
        {

            var sqlQueryText = "SELECT * FROM c";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Order> queryResultSetIterator = this.container.GetItemQueryIterator<Order>(queryDefinition);

            List<Order> families = new List<Order>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Order> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Order order in currentResultSet)
                {
                    families.Add(order);
                    Console.WriteLine("\tRead {0}", order.OrderNumber);
                }
                Console.WriteLine("Select orders. Operation consumed {0} RUs.\n", currentResultSet.RequestCharge);
            }
        }

        private async Task ReplaceFamilyItemAsync(string id, string partition)
        {
            ItemResponse<Order> OrderResponse = await this.container.ReadItemAsync<Order>(id, new PartitionKey(partition));
            var itemBody = OrderResponse.Resource;

            // update registration status from false to true
            var oldNumber = itemBody.OrderNumber;
            itemBody.OrderNumber = "NL-77";

            // replace the item with the updated content
            OrderResponse = await this.container.ReplaceItemAsync<Order>(itemBody, itemBody.id, new PartitionKey(itemBody.OrderAddress.City));
            Console.WriteLine("Updated Order Number {0}.\n Number is now: {1}\n Operation consumed {2} RUs", oldNumber, itemBody.OrderNumber, OrderResponse.RequestCharge);
        }

    }
}
