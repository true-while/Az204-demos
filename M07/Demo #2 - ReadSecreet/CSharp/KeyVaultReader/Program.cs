using System;
using System.IO;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace KeyVaultReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", false)
            .Build();

            //build credential
            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", configuration.GetSection("AZURE_CLIENT_ID").Value);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", configuration.GetSection("AZURE_CLIENT_SECRET").Value);
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", configuration.GetSection("AZURE_TENANT_ID").Value);

            var keyVaultName = configuration.GetSection("keyVaultName").Value;
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";

            //build keyvault client
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
                 
            //retrieve the secret
            KeyVaultSecret secret = client.GetSecret(configuration.GetSection("secretName").Value);

            Console.WriteLine($"Your secret value is: {secret.Value}");

            Console.ReadKey();
        }
    }
}
