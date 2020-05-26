using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PartitioningDemoConsole
{
    partial class  Program
    {
        #region MISC

        /* src https://github.com/Azure/azure-documentdb-dotnet */

        private static async Task CreateDB(DocumentClient client)
        {
            try
            {
                _database = await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
            }
            catch (Exception)
            {
                _database = client.CreateDatabaseAsync(new Database {Id = databaseId}).Result;
            }
        }

        private static async Task CleanDB(DocumentClient client)
        {

            Console.WriteLine("\r\nU ready to delete DB?");
            Console.ReadKey();
            await client.DeleteDatabaseAsync(_database.SelfLink);
        }

        private static async Task CreateCollection(DocumentClient client, string  collectionId, string [] partitions, bool wihtPartition)
        {
            DocumentCollection collectionDefinition = new DocumentCollection { Id = collectionId };
            if (wihtPartition) collectionDefinition.PartitionKey.Paths.Add("/StartLetter");

            _collection = await client.CreateDocumentCollectionAsync(_database.SelfLink,
                collectionDefinition, new RequestOptions { OfferThroughput = 400 });

                foreach (var part in partitions)
                    RunBulkImport(client, _collection.SelfLink, wihtPartition ? part : null, part, @".\Data\" + part).Wait();

        }


        private static async Task RunBulkImport(DocumentClient client, string collectionLink, string partitionName = null, string PartiononLabel = null, string inputDirectory= @".\Data\")
        {
            string inputFileMask = "*.json";
            int maxFiles = 2000;
            int maxScriptSize = 50000;
            

            var sproc = await CreateSP(client, collectionLink);

            string[] fileNames = Directory.GetFiles(inputDirectory, inputFileMask);

            DirectoryInfo di = new DirectoryInfo(inputDirectory);
            FileInfo[] fileInfos = di.GetFiles(inputFileMask);

            int currentCount = 0;
            int fileCount = maxFiles != 0 ? Math.Min(maxFiles, fileNames.Length) : fileNames.Length;

                Stopwatch sp = new Stopwatch();
                sp.Start();
                while (currentCount < fileCount)
                {
                    string argsJson = CreateBulkInsertScriptArguments(fileNames, currentCount, fileCount, maxScriptSize,
                        PartiononLabel);
                    var args = new dynamic[] {JsonConvert.DeserializeObject<dynamic>(argsJson)};

                    StoredProcedureResponse<int> scriptResult = await client.ExecuteStoredProcedureAsync<int>(
                        sproc.SelfLink,
                        partitionName == null
                            ? new RequestOptions()
                            : new RequestOptions {PartitionKey = new PartitionKey(partitionName)},
                        args);
                    int currentlyInserted = scriptResult.Response;
                    currentCount += currentlyInserted;
                }

                
                Console.WriteLine("Upload {0} documents in the collection in {1}ms {2}", currentCount,
                    sp.Elapsed.Milliseconds, partitionName == null ? "partition " + partitionName : "no-partition");
           
        }

        private static async Task<StoredProcedure> CreateSP(DocumentClient client, string collectionLink)
        {
            string body = File.ReadAllText(@".\JS\BulkImport.js");
            StoredProcedure sproc = new StoredProcedure
            {
                Id = "BulkImport",
                Body = body
            };

            await TryDeleteStoredProcedure(client, collectionLink, sproc.Id);
            sproc = await client.CreateStoredProcedureAsync(collectionLink, sproc);
            return sproc;
        }

        private static async Task TryDeleteStoredProcedure(DocumentClient client, string collectionLink, string sprocId)
        {
            StoredProcedure sproc = client.CreateStoredProcedureQuery(collectionLink).Where(s => s.Id == sprocId).AsEnumerable().FirstOrDefault();
            if (sproc != null)
            {
                await client.DeleteStoredProcedureAsync(sproc.SelfLink);
            }
        }

        private static string CreateBulkInsertScriptArguments(string[] docFileNames, int currentIndex, int maxCount, int maxScriptSize, string partitionName)
        {
            var jsonDocumentArray = new StringBuilder();
            jsonDocumentArray.Append("[");

            if (currentIndex >= maxCount) return string.Empty;
            jsonDocumentArray.Append(UpdateContent(docFileNames, currentIndex, partitionName, 0));

            int scriptCapacityRemaining = maxScriptSize;
            string separator = string.Empty;

            int i = 1;
            while (jsonDocumentArray.Length < scriptCapacityRemaining && (currentIndex + i) < maxCount)
            {

                jsonDocumentArray.Append(", " + UpdateContent(docFileNames, currentIndex, partitionName, i));
                i++;
            }

            jsonDocumentArray.Append("]");
            return jsonDocumentArray.ToString();
        }

        private static string UpdateContent(string[] docFileNames, int currentIndex, string partitionName, int i)
        {
            string content = File.ReadAllText(docFileNames[currentIndex + i]);
            //content = Regex.Replace(content, " \"id\" ?: ?\"[^\"]*\"", "\"id\": \"" + Guid.NewGuid() + "\" ");
            var newcontent = content.Substring(0, content.LastIndexOf("}")) + ",\r\n\"StartLetter\" : \"" +
                             partitionName + "\"\r\n}";
            return newcontent;
        }

        private static void LogException(Exception e)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            if (e is DocumentClientException)
            {
                DocumentClientException de = (DocumentClientException)e;
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            else
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }

            Console.ForegroundColor = color;
        }

        #endregion MISC

    }
}
