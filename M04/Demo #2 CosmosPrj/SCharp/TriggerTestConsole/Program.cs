using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using System.Net;
using Newtonsoft.Json;

namespace DocumentDBDemoConsole
{
    class Program
    {
        private static DocumentClient client;
        private static readonly string endpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly string databaseId = ConfigurationManager.AppSettings["DatabaseId"];
        private static readonly string collectionId = ConfigurationManager.AppSettings["CollectionId"];


        // first document  properly formed with crew 
        static string _testTrigerDocument1 = 
@"{
   'id': 'Apollo 8',
   'Operator': 'NASA',
   'Crew' : { 
     'Members': [ 'Frank F. Borman, II', 'James A. Lovell, Jr.', 'William A. Anders'] 
         }
}";


        //second document should return exception
        static string _testTrigerDocument2 =
 @"{
   'id': 'Apollo 0',
   'Operator': 'NASA',
   'Crew' : 'Members'
}";

        private static void Main(string[] args)
        {

            

            (new[] { _testTrigerDocument1, _testTrigerDocument2}).
                ToList().ForEach(doc =>
                {
                    try
                    {
                        

                        Console.WriteLine("Creating New Document....");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(doc);

                        using (client = new DocumentClient(new Uri(endpointUrl), authorizationKey))
                        {
                            CheckIfDocCreated("Apollo 8").Wait();
                            RunDocumentsDemo(doc).Wait();
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Document successfully created.");
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    catch (DocumentClientException de)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Exception baseException = de.GetBaseException();
                        Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message,
                            baseException.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Exception baseException = e.GetBaseException();
                        Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                });

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nEnd of demo, press any key to exit.");
            Console.ReadKey();
        }

        private static async Task CheckIfDocCreated(string docid)
        {
            try
            {
                var docLink = UriFactory.CreateDocumentUri(databaseId, collectionId, docid);

                await client.DeleteDocumentAsync(docLink,new RequestOptions() { PartitionKey = new PartitionKey("NASA") });
            }
            catch (DocumentClientException de)
            {
                //doc not existed
            }
        }

        private static async Task RunDocumentsDemo(string DocText)
        {
            var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(DocText)))
            {

                await client.CreateDocumentAsync(
                    collectionLink, 
                    Document.LoadFrom<Document>(ms), 
                new RequestOptions
                {
                    PreTriggerInclude = new List<string> { "EnsureHaveCrew" },  //the trigger name need to be provided
                });
            }
        }
    }
}
