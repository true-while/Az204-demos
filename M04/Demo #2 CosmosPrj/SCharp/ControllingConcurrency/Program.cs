using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;


namespace ControllingConcurrency
{
    partial class Program
    {
        #region Fields
        private static readonly string EndpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string AuthorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly string databaseId = "ConcurDemo";
        private static readonly string collectionId = "ConcurDemo";
        #endregion Fields


        private static void Main()
        {
            Console.WriteLine("Demonstrate optimistic Concurrency!");

            var task = new OptimtimisticConcurrencyTests(EndpointUrl, AuthorizationKey);
            task.DemoPrecondition(databaseId, collectionId).Wait();

            task.CleanDB().Wait();

            Console.WriteLine("End of demo, press any key to exit.");
            Console.ReadKey();
            
        }
    }

    public class OptimtimisticConcurrencyTests
    {

        private readonly DocumentClient _client;
        private DatabaseSetup _dbSetup;
        private string _databaseId;

        public OptimtimisticConcurrencyTests(string EndpointUrl, string AuthorizationKey)
        {
            _client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
        }

        public async Task DemoPrecondition(string DatabaseId, string CollectionId)
        {
            // Setup our Database and add a new Customer
            _databaseId = DatabaseId;
            _dbSetup = new DatabaseSetup(_client);
            await _dbSetup.Init(_databaseId, CollectionId);
            var addCustomer = new Customer(Guid.NewGuid().ToString(), "Demo");
            await _dbSetup.AddCustomer(addCustomer).ContinueWith((a)=> Console.WriteLine($"Customer Doc has {(a.IsCompleted? "created" : "fail to create" )}" ));

            ;

            // Fetch out the Document (Customer)
            var document = (from f in _dbSetup.Client.CreateDocumentQuery(_dbSetup.Collection.SelfLink)
                            where f.Id == addCustomer.Id
                            select f).AsEnumerable().FirstOrDefault();

            Console.WriteLine("Customer Doc has loaded from DB");

            // Cast the Document to our Customer & make a data change
            var editCustomer = (Customer)(dynamic)document;
            editCustomer.Name = "Changed";

            // Using Access Conditions gives us the ability to use the ETag from our fetched document for optimistic concurrency.
            var ac = new AccessCondition {
                Condition = document.ETag,
                Type = AccessConditionType.IfMatch
            };

            // Replace our document, which will succeed with the correct ETag 
            await _dbSetup.Client.ReplaceDocumentAsync(document.SelfLink, editCustomer,
                new RequestOptions { AccessCondition = ac });

            Console.WriteLine("Customer Doc has been modified in DB");

            try
            {
                Console.WriteLine("The same Doc try to be modified again and saved....");

                // Replace again, which will fail since our (same) ETag is now invalid
                await _dbSetup.Client
                         .ReplaceDocumentAsync(document.SelfLink, editCustomer,
                             new RequestOptions { AccessCondition = ac });
                        //.ContinueWith((a) => Console.WriteLine($"Customer Doc modification {(a.IsCompleted ? "completed" : "failed")}")); ;
            }
            catch (DocumentClientException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Customer Doc cannot be modified in DB");
                Console.ForegroundColor = ConsoleColor.Red;
                if (ex.StatusCode == HttpStatusCode.PreconditionFailed)
                {
                    Console.WriteLine($"Precondition exception: {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"Another Exception: {ex.Message}");
                }
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }            
        }

        internal async Task CleanDB()
        {

            Console.WriteLine("\r\nU ready to delete DB?");
            Console.ReadKey();
            await _dbSetup.DeleteDBAsync(UriFactory.CreateDatabaseUri(_databaseId));
        }
    }
}