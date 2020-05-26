using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataExporter
{
    partial class Program
    {
        public class BabyNameTable: TableEntity
        {
            public string ChildFirstName { get; set; }
            public int Count { get; set; }
            public string Ethnicity { get; set; }
            public string Gender { get; set; }
            public int Rank { get; set; }
            public int YearofBirth { get; set; }
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

        private static void Main(string[] args)
        {

            try
            {
                string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=aquotedevaf3aalex;AccountKey=ohX+mzxtV5gcjUAAtfOZERJZJY2ZHGm/5v/TlPjRgUi+jLCe6FPvYKI5UuPlOrQbRwCIbr4P0W+b3Di4t7huEw==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                
                var table = tableClient.GetTableReference("Baby");
                table.CreateIfNotExists();

                TableQuery<BabyNameTable> DataTableQuery = new TableQuery<BabyNameTable>();
                var query = new TableQuery<BabyNameTable>();
                       
                IEnumerable <BabyNameTable> list = table.ExecuteQuery(query);

                if (!Directory.Exists(Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, "DataExport")))
                            Directory.CreateDirectory(Path.Combine(
                            AppDomain.CurrentDomain.BaseDirectory, "DataExport"));

                foreach (var baby in list)
                {
                    var path = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, "DataExport",
                        String.Format("{0}-{1}.json", baby.RowKey, baby.PartitionKey));
                    if (File.Exists(path)) File.Delete(path);

                    File.WriteAllText(
                        path, JsonConvert.SerializeObject(new BabyName()
                        {
                            ChildFirstName = baby.ChildFirstName,
                            Count = baby.Count,
                            Ethnicity = baby.Ethnicity,
                            Gender = baby.Gender,
                            id = baby.RowKey + "-" + baby.PartitionKey,
                            Rank =baby.Rank,
                            YearofBirth = baby.YearofBirth
                        }
                        ));
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


    }
}
