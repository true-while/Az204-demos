using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace IndexDemoConsole
{
    partial class Program
    {
        #region Fields
        private static readonly string endpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string authorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly string databaseId = "BulkTest";
        private static readonly ConnectionPolicy connectionPolicy = new ConnectionPolicy { UserAgentSuffix = " samples-net/3" };
        private static Database _database;
        #endregion Fields


        private static void Main()
        {

            try
            {
                using (var client = new DocumentClient(new Uri(endpointUrl), authorizationKey, connectionPolicy))
                {
                    /* Create DB*/
                    CreateDB(client).Wait();

                    /*Bulk insert into Lazy DB*/
                    CreateCollection(client,"Lazy",
                        new IndexingPolicy(
                        new RangeIndex(DataType.String) { Precision = -1 }) 
                            { IndexingMode = IndexingMode.Lazy }
                    ).Wait();

                    /*Bulk insert into default Consistent DB*/
                    CreateCollection(client,"Consistent",
                        new IndexingPolicy(
                        new RangeIndex(DataType.String) {Precision = -1})
                            {IndexingMode = IndexingMode.Consistent}
                    ).Wait();
                    
                    /* NoIndex Query*/
                    RunQuery(client, "Consistent").Wait();

                    /* Set Indexes */
                    CreateIndex(client, "Consistent").Wait();

                    /*Index Query*/
                    RunQuery(client, "Consistent").Wait();

                    /*Remove DB*/
                    CleanDB(client).Wait();
                }
            }
//            catch (Exception e)
//            {
//                LogException(e);
//            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }


        private static async Task CreateCollection(DocumentClient client, string collectionName, IndexingPolicy indexingPolicy)
        {
            DocumentCollection collectionDefinition = new DocumentCollection { Id = collectionName };
            collectionDefinition.IndexingPolicy = indexingPolicy;

            DocumentCollection collection = await client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(databaseId),
                collectionDefinition, new RequestOptions { OfferThroughput = 400 });

            Console.WriteLine("Collection: {0} Index Policy: {1}", collectionName,
                indexingPolicy.IndexingMode);

            /* do insert 100 docs */
            await RunBulkImport(client, collection.SelfLink);
        }

        private static async Task RunQuery(DocumentClient client, string collectionName)
        {

            RunQueryInternal(client, "SELECT * FROM root r WHERE r.FamilyId > 1000", collectionName);

            RunQueryInternal(client, "SELECT * FROM root r WHERE r.Address.City <> 'Seattle'", collectionName);

            RunQueryInternal(client, @"SELECT c.FamilyID,children.FirstName FROM c
                    JOIN children IN c.Children Where children.Gender = 'female'", collectionName);

        }
        
        private static async  Task CreateIndex(DocumentClient client,string collectionName)
        {
            Console.WriteLine("Set up Indexes");
            DocumentCollection collection =
                await
                    client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionName));

            /*
             * Range over /prop/? (or /*) can be used to serve the following queries efficiently: 
             * SELECT * FROM collection c WHERE c.prop = "value" 
             * SELECT * FROM collection c WHERE c.prop > 5 
             * SELECT * FROM collection c ORDER BY c.prop 
             */
            Index indexNum = new RangeIndex(DataType.Number);
            collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath()
            {
                Indexes = new Collection<Index>() {indexNum},
                Path = @"/FamilyId/?"
            });

            /*
             * Hash over /prop/? (or /*) can be used to serve the following queries efficiently: 
             * SELECT * FROM collection c WHERE c.prop = "value" 
            */
            Index indexArray = new HashIndex(DataType.String);
            collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath()
            {
                Indexes = new Collection<Index>() { indexArray },
                Path = @"/Address/*"
            });

             /*
             * Hash over /props/[]/? (or /* or /props/*) can be used to serve the following queries efficiently: 
             * SELECT tag FROM collection c JOIN tag IN c.props WHERE tag = 5  
             */
            Index indexArr = new HashIndex(DataType.String);
            collection.IndexingPolicy.IncludedPaths.Add(new IncludedPath()
            {
                Indexes = new Collection<Index>() { indexArr },
                Path = @"/Children/[]/?"
            });


            /* exclude from index Parents */
            collection.IndexingPolicy.ExcludedPaths.Add(new ExcludedPath(){ Path = @"/Parents/*"});


            await client.ReplaceDocumentCollectionAsync(collection);

        }

    }
}
