const dotenv = require('dotenv');
const path = require('path');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

const { ServiceBusClient, ReceiveMode } = require('@azure/service-bus');
const sbClient = ServiceBusClient.createFromConnectionString(process.env.ConnectionString);
const queueClient = sbClient.createQueueClient('demo-queue');
const receiver = queueClient.createReceiver(ReceiveMode.receiveAndDelete);

async function main() {
    try {
        const messages = await receiver.receiveMessages(10)
        console.log('Received messages:');
        console.log(messages.map(message => message.body));

        await queueClient.close();
    } finally {
        await sbClient.close();
    }
}

main().catch((err) => {
    console.log("Error occurred: ", err);
});
