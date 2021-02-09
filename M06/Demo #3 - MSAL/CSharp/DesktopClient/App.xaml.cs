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
        private static string Tenant = "...";

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
