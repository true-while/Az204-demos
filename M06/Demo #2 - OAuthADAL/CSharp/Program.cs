using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace OAuthBasic
{
    class Program
    {
        static void Main(string[] args)
        {
            AccessToken();

            Console.Read();
        }

         static async void AccessToken()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

            string resourceUri = "https://datacatalog.azure.com";
            string authorityUri = "https://login.windows.net/common/oauth2/" + configuration.GetSection("tenant").Value;

            AuthenticationContext authContext = new AuthenticationContext(authorityUri);

            var cred = new ClientCredential(configuration.GetSection("applicationId").Value, configuration.GetSection("clientSecret").Value);

            AuthenticationResult token = await authContext.AcquireTokenAsync(
                resourceUri, cred);

            Console.WriteLine(token.AccessToken);
        }
    }
}
