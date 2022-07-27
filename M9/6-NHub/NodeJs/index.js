var azure = require('azure');

const hubname = 'nhubalex';
const connectionstring = '<your connection string>';

var notificationHubService = azure.createNotificationHubService( hubname, connectionstring);

async function main() {
    var payload = `<toast>
    <visual>
     <binding template="ToastGeneric">
       <text hint-maxLines="1">Azure Rocks!</text>
       <text>voice from the fields...</text>
       <image placement="hero" src="https://pbs.twimg.com/media/CKfLdEcWIAA3k2o?format=png"/>
       <text placement="attribution">Via Azure</text>
       <image placement="appLogoOverride" hint-crop="circle" src="https://avatars2.githubusercontent.com/u/25492227" />
     </binding>
    </visual>
   </toast>`;

    return await notificationHubService.wns.send(null, payload, 'wns/toast', function(error){
        if(!error) {
            console.log('notification sent');
        }
    });
}

main();