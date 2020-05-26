using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace ConsistencyDemoConsole
{
    partial class Program
    {
        #region Fields
        private static readonly string EndpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string AuthorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly string databaseId = "ConsDemo";
        private static readonly string collectionId = "Strong";
        private static readonly ConnectionPolicy ConnectionPolicy = new ConnectionPolicy(); 
        private static Database _database;
        private static DocumentCollection _collection;
        #endregion Fields


        private static void Main()
        {

            try
            {
                using (DocumentClient setUpClient = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey,ConnectionPolicy))
                {

                    /* 
                     * Here is a way to set ConsistencyLevel 
                     * 1. For DB Account (from portal)
                     * 2. For Document Client (from constructor)
                     * 3. For each CURD Request (with RequestOptions())
                     */

                    ReadDBConsistencyLevel(setUpClient).Wait();

                    CreateDB(setUpClient).Wait();
                    CreateCollection(setUpClient).Wait();

                    TryToCreateClintWithConsistencyLevel(ConsistencyLevel.Strong);   // ConsistencyLevel.Session => ConsistencyLevel.Strong

                    TryToCreateClintWithConsistencyLevel(ConsistencyLevel.Eventual);   // ConsistencyLevel.Session => ConsistencyLevel.Eventual

                    Tuple<string, string> UpdateDocAresult, UpdateDocBresult = null;
                    string DocID = "14";

                    using (var clientA = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey))
                    using (var clientB = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey))
                    {
                        UpdateDocAresult = UpdateDocument(clientA, DocID).Result;
                        UpdateDocBresult = UpdateDocument(clientB, DocID).Result;

                        //retrieve the SessionToken from then use it on a different instance of DocumentClient within RequestOptions 
                        var sharedSessionToken = UpdateDocBresult.Item2; 

                        QueryDocuemnt(clientB, UpdateDocAresult.Item1 /* DocLink */, sharedSessionToken /*SessionToken*/).Wait();
                        QueryDocuemnt(clientA, UpdateDocBresult.Item1 /* DocLink */, sharedSessionToken /*SessionToken*/).Wait();
                    }

                    /* src https://msdn.microsoft.com/en-us/library/microsoft.azure.documents.client.requestoptions.sessiontoken.aspx */

                    /*Remove DB*/
                   CleanDB(setUpClient).Wait();
                }
            }
            catch (Exception e)
            {
                LogException(e);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }

        private static void TryToCreateClintWithConsistencyLevel(ConsistencyLevel level)
        {
            try
            {
                using (var client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey, desiredConsistencyLevel: level))
                {
                   var cl =  client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId)).Result;
                }
                Console.WriteLine("\r\nClient create with level: {0} \r\n",level);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

        }


        private static async Task<Tuple<string,string>> UpdateDocument(DocumentClient client, string id)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            var docid = UriFactory.CreateDocumentUri(databaseId, collectionId, id);

            /* get document */
            ResourceResponse<Document> readResponse = await client.ReadDocumentAsync(docid); //
            var readDocument = readResponse.Resource;

            /* update value */
            var val = rnd.Next(1000).ToString();
            readDocument.SetPropertyValue("Value",val);

            
            /*update document*/
            var updateDocuemnt = await client.ReplaceDocumentAsync(readDocument, new RequestOptions());

            Console.WriteLine("DocID {0} value {1} was crested in session {2}", id, val, updateDocuemnt.SessionToken);

            //return session 
            return new Tuple<string, string>(readDocument.SelfLink, updateDocuemnt.SessionToken);

        }

        private static async Task QueryDocuemnt(DocumentClient client, string DocLink, String SessionToken)
        {
            ResourceResponse<Document> response = await client.ReadDocumentAsync(DocLink, 
                new RequestOptions() { SessionToken = SessionToken });

            Document created = response.Resource;

            Console.WriteLine("DocID {0} val {1} was read from session: {2} ", created.Id, created.GetPropertyValue<String>("Value"), SessionToken);
        }

        private static async Task ReadDBConsistencyLevel(DocumentClient client)
        {
           var  acc = await client.GetDatabaseAccountAsync();
           Console.WriteLine("Data Base Account ConsistencyLevel is {0}", acc.ConsistencyPolicy.DefaultConsistencyLevel.ToString());
           Console.WriteLine("Client ConsistencyLevel is {0}",client.ConsistencyLevel.ToString());
        }

    }
}
