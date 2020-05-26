using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace PartitioningDemoConsole
{
    partial class Program
    {
        #region Fields

        private static readonly string endpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly string databaseId = "PartDemo";
         
        private static readonly ConnectionPolicy connectionPolicy = new ConnectionPolicy();

        private static Database _database;
        private static DocumentCollection _collection;

        #endregion Fields

        private static void Main(string[] args)
        {

            try
            {
                using (DocumentClient client = new DocumentClient(new Uri(endpointUrl), authorizationKey, connectionPolicy))
                {
                    /*set up DB and initial data*/
                    CreateDB(client).Wait();
                    Console.ForegroundColor = ConsoleColor.Green;

                    /* upload items in collections*/
                    var partList = new[] { "A","B","C","D"};
                    CreateCollection(client, "Partitioned", partList,true).Wait();
                    CreateCollection(client, "NotPartitioned", partList, false).Wait();


                    foreach (var r in new[] { 1, 2, 3 })
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"Iteration #{r}");
                        
                        Stopwatch sp = new Stopwatch();

                        /*run 3 request over NON-partitioned collection*/
                        
                        sp.Restart(); Console.ForegroundColor = ConsoleColor.Yellow;
                        EnumDocuments(client, "NotPartitioned", null, "A");

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Non-Partition queering takes {0} ms", sp.ElapsedMilliseconds);

                        /*run 3 request over partitioned collection*/
                        sp.Restart(); Console.ForegroundColor = ConsoleColor.Yellow;
                        EnumDocuments(client, "Partitioned", "A", "A");

                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Partition queering takes {0} ms", sp.ElapsedMilliseconds);

                    }
                    /*Remove DB*/
                    Console.ForegroundColor = ConsoleColor.White;
                    CleanDB(client).Wait();
                }

            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }

        }

        private static void EnumDocuments(DocumentClient client, string collectionid, string partKey, string StartLetter)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            string continuation = string.Empty;

            var resultsIDNumQ = "SELECT * FROM Names WHERE Names.id = 'Abby-2013'";
            var resultsIDNum = client.CreateDocumentQuery<Document>(
                  UriFactory.CreateDocumentCollectionUri(databaseId, collectionid),
                  resultsIDNumQ,
                  new FeedOptions
                  {
                      EnableCrossPartitionQuery = true, // partKey == null ? true : false,
                      PartitionKey = partKey != null ? new PartitionKey(partKey) : null
                  });

            //Console.WriteLine(resultsIDNumQ);
            Console.WriteLine("'byID' query found {0} document in {1} ms", resultsIDNum.AsEnumerable().Count(), sp.Elapsed.Milliseconds);

            sp.Restart();

            var resultsContainsNumQ = $"SELECT * FROM Names WHERE CONTAINS(Names.id, 'a') and Names.StartLetter ='{StartLetter}' ";
            var resultsContainsNum = client.CreateDocumentQuery<Document>(
                  UriFactory.CreateDocumentCollectionUri(databaseId, collectionid), resultsContainsNumQ,
                  new FeedOptions
                  {
                      //RequestContinuation = continuation,
                      EnableCrossPartitionQuery = true, //partKey == null ? true : false,
                      PartitionKey = partKey != null ? new PartitionKey(partKey) : null
                  });

            //Console.WriteLine(resultsContainsNumQ);
            Console.WriteLine("'Contains' query found {0} document in {1} ms", resultsContainsNum.AsEnumerable().Count(), sp.Elapsed.Milliseconds);

            sp.Restart();

            var resultsByPropNumQ = $"SELECT * FROM Names WHERE  Names.StartLetter ='{StartLetter}' and Names.Gender='FEMALE' ";
            var resultsByPropNum = client.CreateDocumentQuery<Document>(
                  UriFactory.CreateDocumentCollectionUri(databaseId, collectionid),
                  resultsByPropNumQ,
                    new FeedOptions
                    {
                        EnableCrossPartitionQuery = true, //partKey == null ? true : false,
                        PartitionKey = partKey != null ? new PartitionKey(partKey) : null
                    });

            //Console.WriteLine(resultsByPropNumQ);
            Console.WriteLine("'ByProperty' query found {0} document in {1} ms", resultsByPropNum.AsEnumerable().Count(), sp.Elapsed.Milliseconds);
        }


        public class BabyName
        {
            public string id { get; set; }
            public string ChildFirstName { get; set; }
            public int Count { get; set; }
            public string Ethnicity { get; set; }
            public string Gender { get; set; }
            public int Rank { get; set; }
            public int YearofBirth { get; set; }
        }
    }
}
