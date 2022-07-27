#bash

#values form previous run for Event Hub
$eventhub="your hub name";  # <<<<<--- please provide short name of eventhub from previous run
$groupname="Demo-AZ204-EH"

# to avoid name collisions generate unique name for your account
$account="eventgrid"+(Get-Random); 

#Create a resource group
New-AzResourceGroup -location eastus -name Demo-AZ204-EH;

#Enable the Event Grid resource provider
Register-AzResourceProvider -ProviderNamespace Microsoft.EventGrid;

#Create a resource group to monitor
New-AzResourceGroup -location eastus -name Demo-AZ204-monitor;

#Pull azure subscription id
$subid=(az account show --query id -o tsv)

#Сonfigure event subscription endpoint
$endpoint="/subscriptions/$subid/resourceGroups/$groupname/providers/Microsoft.EventHub/namespaces/$eventhub/eventhubs/$eventhub";

#Create a subscription for the events from Resourece group
New-AzEventGridSubscription -EventSubscriptionName "group-monitor-sub"  -EndpointType "eventhub" -Endpoint $endpoint -ResourceGroup "Demo-AZ204-monitor";

