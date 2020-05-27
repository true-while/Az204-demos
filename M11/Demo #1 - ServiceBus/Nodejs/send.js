const dotenv = require('dotenv');
const path = require('path');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

const { ServiceBusClient } = require("@azure/service-bus");
const sbClient = ServiceBusClient.createFromConnectionString(process.env.ConnectionString);
const queueClient = sbClient.createQueueClient('demo-queue');
const sender = queueClient.createSender();
async function main() {
    try {
        for (let i = 0; i < 10; i++) {
            const message = {
                body: `Hello world! ${i}`,
                label: `test`,
                userProperties: {
                    myCustomPropertyName: `my custom property value ${i}`
                }
            };
            console.log(`Sending message: ${message.body}`);
            await sender.send(message);
        }

        await queueClient.close();
    } finally {
        await sbClient.close();
    }
}

main().catch((err) => {
    console.log("Error occurred: ", err);
});