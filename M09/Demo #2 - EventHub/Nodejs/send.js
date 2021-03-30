const dotenv = require('dotenv');
const path = require('path');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

const { EventHubClient } = require('@azure/event-hubs');

const client = EventHubClient.createFromConnectionString(
    process.env.connectionString, process.env.eventHubsName);

async function main() {
    for (let i = 0; i < 10; i++) {
        const eventData = { body: `Event ${ i }` };
        console.log(`Sending event: ${ eventData.body }`);
        await client.send(eventData);
    }
    await client.close();
}

main().catch(err => {
    console.log('Error occurred: ', err);
});
