using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace StorageToCosmos
{
    public static class StorageToCosmos
    {


        [FunctionName("F1")]
        public static void FlightDataTransfer(
            [BlobTrigger("flightdata/{name}", 
            Connection = "StorageConneciton")]Stream myBlob, string name,
            [CosmosDB(databaseName: "DatabaseName", 
            collectionName: "CollectionName", 
            ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            //********************** Read App Settings*******************
            var CosmosDBName = System.Environment.GetEnvironmentVariable("DatabaseName", EnvironmentVariableTarget.Process);
            var CosmosCollectionName = System.Environment.GetEnvironmentVariable("CollectionName", EnvironmentVariableTarget.Process);
            var ttlDocuemnt = int.Parse(System.Environment.GetEnvironmentVariable("TTLDocuemntMinute", EnvironmentVariableTarget.Process) ?? "-1");
            //***********************************************************


            //check time to live on collection level.
            var collLink = UriFactory.CreateCollectionUri(CosmosDBName, CosmosCollectionName);
            DocumentCollection collection = client.ReadDocumentCollectionAsync(collLink).Result;
            if (ttlDocuemnt == -1 || collection.DefaultTimeToLive != ttlDocuemnt*60 )  //time-to-live in seconds
            {
                collection.DefaultTimeToLive = ttlDocuemnt*60;
                client.ReplaceDocumentCollectionAsync(collection);
            }

            log.LogInformation($"Function was triggered by blob Name:{name}  Size: {myBlob.Length} Bytes");

            //******************Read File content updated in storage****************************
            string json_content;
            var tiks = DateTime.Now.Ticks;

            FlightRecords msg;
            using (TextReader tr = new StreamReader(myBlob))
            {
                json_content = tr.ReadToEnd();
                tr.Close();
            }

            //*******************Validate File Content*****************************************
            try
            {
                msg = JsonConvert.DeserializeObject<FlightRecords>(json_content);
            }
            catch (Exception ex)
            {
                log.LogInformation($"File Content can not be deserialized. It seems to be wrong formated JSON\n");
                log.LogError(ex.ToString());
                return;
            }

            //********************Create separate documents by airline code***************************
            var gpByAirline = msg.Flight_Record.GroupBy(x => x.AIRLINE_CODE).ToDictionary(k => k.Key, k => k.ToList());
            var collectionLink = UriFactory.CreateDocumentCollectionUri(CosmosDBName, CosmosCollectionName);
            var option = new FeedOptions { EnableCrossPartitionQuery = true };

            foreach (var airline in gpByAirline.Keys)
            {
                try
                {

                    var documentId = $"FlightRecords-{airline}"; //unique Document ID

                    //select document by ID
                    var docExists = client.CreateDocumentQuery(collectionLink, option)
                        .Where(doc => doc.Id == documentId)
                        .Select(doc => doc.Id)
                        .AsEnumerable()
                        .Any();

                    //create new document. id - filed must be exists and represent the key
                    var docSpec = new { id = documentId, Flight_Record = gpByAirline[airline] };


                    if (docExists)
                    {
                        //if the document exists, document will be replaced 
                        Uri docUri = UriFactory.CreateDocumentUri("boomi-msg", "boomi", documentId);
                        var result = client.ReplaceDocumentAsync(docUri, docSpec).Result;
                        log.LogInformation($"Document with id:'{result.Resource.Id}' updated");
                    }
                    else
                    {
                        //if there is no document found - create new one.
                        var result = client.CreateDocumentAsync(collectionLink, docSpec).Result;
                        log.LogInformation($"Document with id:'{result.Resource.Id}' created");
                    }

                }
                catch (Exception ex)
                {
                    log.LogInformation($"File Content can not be deserialized. It Seems to be wrong formated JSON\n{ex}");
                    return;
                }
            }



        }
    }
}

/*
Query Example   
SELECT value f FROM documents d JOIN f IN d.Flight_Record where f.FLIGHT_STATUS in ( "Delayed" , "Canceled")

 */
