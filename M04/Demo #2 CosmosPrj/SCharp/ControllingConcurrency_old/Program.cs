using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;


namespace ConsistencyDemoConsole.csproj
{
    public class OptimtimisticConcurrencyTests
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

        private readonly DocumentClient _client;
        private const string EndpointUrl = "https://localhost:8081";
        private const string AuthorizationKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DatabaseId = "ConcurrencyDemo";
        private const string CollectionId = "Customers";

        public OptimtimisticConcurrencyTests()
        {
            _client = new DocumentClient(new Uri(EndpointUrl), AuthorizationKey);
        }

        [Fact]
        public async Task Should_Throw_With_PreconditionFailed()
        {
            // Setup our Database and add a new Customer
            var dbSetup = new DatabaseSetup(_client);
            await dbSetup.Init(DatabaseId, CollectionId);
            var addCustomer = new Customer(Guid.NewGuid().ToString(), "Demo");
            await dbSetup.AddCustomer(addCustomer);

            // Fetch out the Document (Customer)
            var document = (from f in dbSetup.Client.CreateDocumentQuery(dbSetup.Collection.SelfLink)
                            where f.Id == addCustomer.Id
                            select f).AsEnumerable().FirstOrDefault();

            // Cast the Document to our Customer & make a data change
            var editCustomer = (Customer)(dynamic)document;
            editCustomer.Name = "Changed";

            // Using Access Conditions gives us the ability to use the ETag from our fetched document for optimistic concurrency.
            var ac = new AccessCondition { Condition = document.ETag, Type = AccessConditionType.IfMatch };

            // Replace our document, which will succeed with the correct ETag 
            await dbSetup.Client.ReplaceDocumentAsync(document.SelfLink, editCustomer,
                new RequestOptions { AccessCondition = ac });

            // Replace again, which will fail since our (same) ETag is now invalid
            var ex = await dbSetup.Client.ReplaceDocumentAsync(document.SelfLink, editCustomer,
                        new RequestOptions { AccessCondition = ac }).ShouldThrowAsync<DocumentClientException>();

            ex.StatusCode.ShouldBe(HttpStatusCode.PreconditionFailed);
        }
    }
}