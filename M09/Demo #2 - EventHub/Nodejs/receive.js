const dotenv = require('dotenv');
const path = require('path');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

const { EventHubClient, delay } = require('@azure/event-hubs');
const client = EventHubClient.createFromConnectionString(
    process.env.connectionString, process.env.eventHubsName);

async function main() {
    const allPartitionIds = await client.getPartitionIds();
    for (var i = 0; i < allPartitionIds.length; i++) {
        var receiveHandler = client.receive(allPartitionIds[i], eventData => {
            console.log(`Received event: ${ eventData.body } from partition ${ allPartitionIds[i] }`);
        }, error => {
            console.log('Error when receiving message: ', error);
        });

        // Sleep for a while before stopping the receive operation.
        await delay(500);
        await receiveHandler.stop();
    }

    await client.close();
}

main().catch(err => {
    console.log('Error occurred:', err);
});
