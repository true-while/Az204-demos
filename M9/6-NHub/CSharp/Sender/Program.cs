using Microsoft.Azure.NotificationHubs;
using System;
using System.Threading.Tasks;

namespace Sender
{
    class Program
    {
        static string ConnectionString = "<your connection string>";
        static string HubName = "nhubalex";
        static void Main(string[] args)
        {
            Send().Wait();
            Console.WriteLine("Notification went through");
        }

        static async Task<NotificationOutcome> Send()
        {
            NotificationHubClient hub = 
                NotificationHubClient.CreateClientFromConnectionString(ConnectionString, HubName);

            string toast = @"<toast>
                 <visual>
                  <binding template=""ToastGeneric"">
                    <text hint-maxLines=""1"">Azure Rocks!</text>
                    <text>voice from the fields...</text>
                    <image placement=""hero"" src=""https://pbs.twimg.com/media/CKfLdEcWIAA3k2o?format=png"" />
                    <text placement=""attribution"">Via Azure</text>
                    <image placement=""appLogoOverride"" hint-crop=""circle"" src=""https://avatars2.githubusercontent.com/u/25492227"" />
                  </binding>
                 </visual>
                </toast>";

             return await hub.SendWindowsNativeNotificationAsync(toast);
        }
    }
}
