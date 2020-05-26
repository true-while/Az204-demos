using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TransactionDemoConsole
{
    partial class Program
    {
        #region Fields
        private static readonly string EndpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string AuthorizationKey = ConfigurationManager.AppSettings["AuthorizationKey"];
        private static readonly string databaseId = "TransDemo";
        private static readonly string collectionId = "Players";
        private static readonly ConnectionPolicy ConnectionPolicy = new ConnectionPolicy();
        private static Database _database;
        private static DocumentCollection _collection;
        #endregion Fields



        static void Main(string[] args)
        {
            try
            {
                using (DocumentClient client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey, ConnectionPolicy))
                {
                    /*set up DB and inital data*/
                    CreateDB(client).Wait();
                    CreateCollection(client).Wait();


                    ReviewPlayers(client).Wait();
                    /* Alex <=> Max */
                    RunExcahngeTransaction(client, "Alex","Max").Wait();

                    ReviewPlayers(client).Wait();
                    /* Scott <=X=> Alex */
                    RunExcahngeTransaction(client, "Scott", "Alex").Wait();

                    ReviewPlayers(client).Wait();

                    /*Remove DB*/
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

        private static async Task RunExcahngeTransaction(DocumentClient client, string player1, string player2)
        {
            try
            {
                Console.WriteLine("\r\n'{0}' == Money ==> '{1}'", player1, player2);
                Console.WriteLine("'{0}' <= Items ==  '{1}'", player1, player2);

                await client.ExecuteStoredProcedureAsync<string>(UriFactory.CreateStoredProcedureUri(databaseId, collectionId, "BuyItems"), player1, player2);

                Console.WriteLine("Player '{0}' successful bought items from Player '{1}'\r\n", player1, player2);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }


        }

        private static async Task ReviewPlayers(DocumentClient client)
        {
            var query = client.CreateDocumentQuery<Dictionary<string, object>>(_collection.SelfLink);

            var players = query.AsEnumerable();
            foreach (var player in players)
            {
                Dictionary<string, object> documentAsDictionary = player;
                Console.WriteLine("Player '{0}' has ${1}", documentAsDictionary.FirstOrDefault(x => x.Key == "id").Value,documentAsDictionary.FirstOrDefault(x=>x.Key =="Money").Value);

                var items = (documentAsDictionary["Items"] as JArray);
                if (items == null || items.Count <= 0) 
                    Console.WriteLine("\tNo Items");
                else
                items.Where(i=>i.HasValues).ToList().ForEach(i =>Console.WriteLine("\tItem: '{0}' cost ${1}", i.Value<string>("Name"), i.Value<string>("Price")));

                Console.WriteLine("---------------------------");
            }
           
           
        }
    }
}
