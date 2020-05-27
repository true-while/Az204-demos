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
        private static string ClientId = "client id";
        private static string ClientKey = "client key";

        static App()
        {
            _clientApp =
                    ConfidentialClientApplicationBuilder.Create(ClientId)
                    .WithClientSecret(ClientKey)
                    .Build();
        }

        // Below are the clientId (Application Id) of your app registration and the tenant information.
        // You have to replace:
        // - the content of ClientID with the Application Id for your app registration
        // - the content of Tenant by the information about the accounts allowed to sign-in in your application:
        //   - For Work or School account in your org, use your tenant ID, or domain
        //   - for any Work or School accounts, use `organizations`
        //   - for any Work or School accounts, or Microsoft personal account, use `common`
        //   - for Microsoft Personal account, use consumers

        private static string Tenant = "57abfb88-2c2a-4e22-ad26-a2a845843584";

        private static IConfidentialClientApplication _clientApp;

        public static IConfidentialClientApplication PublicClientApp { get { return _clientApp; } }
    }
}
