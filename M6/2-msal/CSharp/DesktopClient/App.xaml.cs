using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Identity.Client;

namespace DesktopClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Below are the clientId (Application Id) of your app registration and the tenant information.
        // You have to replace:
        // - the content of ClientID with the Application Id for your app registration
        // - the content of Tenant by the information about the accounts allowed to sign-in in your application:
        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use `organizations`
        //   - for any Work or School accounts, or Microsoft personal account, use `common`
        //   - for Microsoft Personal account, use consumers

        private static string ClientId = "<your-app-account-guid>";
        private static string ClientKey = "<your-app-account-secret>";
        private static string Tenant = "<your-tenant-guid>";

        static App()
        {
            _clientApp =
                    ConfidentialClientApplicationBuilder.Create(ClientId)
                    .WithClientSecret(ClientKey)
                    .WithTenantId(Tenant)
                    .WithRedirectUri("https://graph.microsoft.com")
                    .Build();
        }

        //check on https://jwt.ms/ 


        private static IConfidentialClientApplication _clientApp;

        public static IConfidentialClientApplication PublicClientApp { get { return _clientApp; } }
    }
}
