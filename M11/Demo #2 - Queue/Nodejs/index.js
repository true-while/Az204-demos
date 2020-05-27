/* eslint-disable handle-callback-err */

const dotenv = require('dotenv');
const path = require('path');

const azure = require('azure-storage');
// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });


async function main() {

// create client
var queueClient = azure.createQueueService(process.env.ConnectionString);

// create a queue
await queueClient.createQueueIfNotExists('demo-queue', 
        function (error){ if(error) console.log(`can not create queue ${error}`)});

// create message queue
await queueClient.createMessage('demo-queue', 'Hello world! #1', 
            function (error, results, response) {
                console.log('Message #1 is created');
});

// check message queue length
await queueClient.getQueueMetadata('demo-queue', 
            function (error, mdata) {
                console.log(`message count after create: ${mdata.approximateMessageCount}`);
});

// take first message from queue without deleting
await queueClient.peekMessages('demo-queue', 
            function (error, results, response) {
                console.log(`first message peeked: ${results[0].messageText}`);
});

// check message queue length
await queueClient.getQueueMetadata('demo-queue', 
            function (error, mdata) {
                console.log(`message count after peek: ${mdata.approximateMessageCount}`);
});

// clearing queue
await queueClient.clearMessages('demo-queue', 
        function (error, response) {
            console.log('all messages cleared');
});

// create a message
await queueClient.createMessage('demo-queue', 'Hello world! #2', 
    function (error, results, response) {
        console.log('Messages #2 is created');
});

// get message 
queueClient.getMessages('demo-queue', function (error, results, response) {
    this.message = results[0];
    console.log(`get message text: ${this.message.messageText}`);

    //update message
    queueClient.updateMessage('demo-queue', this.message.messageId, this.message.popReceipt, 10, { messageText: 'new text' }, 
            function (error, response) {
                console.log('message updated')
            });
    
    //delete message
    queueClient.deleteMessage('demo-queue', this.message.messageId, this.message.popReceipt, 
            function (error, response) {
                console.log('message deleted');
            });
});


await queueClient.getQueueMetadata('demo-queue', 
        function (error, mdata) {
            console.log(`message count after deleting: ${mdata.approximateMessageCount}`);
        });

}

main();

/*
queueClient.createQueueIfNotExists('demo-queue', function (error) {
    queueClient.createMessage('demo-queue', 'Hello world! #1', function (error, results, response) {
        console.log('message #1 created');
        queueClient.getQueueMetadata('demo-queue', function (error, mdata) {
            console.log(`message count after create: ${mdata.approximateMessageCount}`);

            queueClient.peekMessages('demo-queue', function (error, results, response) {
                console.log(`message text: ${results[0].messageText}`);
                queueClient.getQueueMetadata('demo-queue', function (error, mdata) {
                    console.log(`message count after peek: ${mdata.approximateMessageCount}`);
                    queueClient.clearMessages('demo-queue', function (error, response) {
                        console.log('messages cleared');

                        queueClient.createMessage('demo-queue', 'Hello world! #2', function (error, results, response) {
                            console.log('messages created');

                            queueClient.getQueueMetadata('demo-queue', function (error, mdata) {
                                console.log(`message count after create: ${mdata.approximateMessageCount}`);

                                queueClient.getMessages('demo-queue', function (error, results, response) {
                                    this.message = results[0];
                                    console.log(`message text: ${this.message.messageText}`);
                                    queueClient.getQueueMetadata('demo-queue', function (error, mdata) {
                                        console.log(`message count after get: ${mdata.approximateMessageCount}`);

                                        queueClient.updateMessage('demo-queue', this.message.messageId, this.message.popReceipt, 10, { messageText: 'new text' }, function (error, response) {
                                            queueClient.deleteMessage('demo-queue', this.message.messageId, this.message.popReceipt, function (error, response) {
                                                console.log('message deleted');

                                                queueClient.getQueueMetadata('demo-queue', function (error, mdata) {
                                                    console.log(`message count after deleting: ${mdata.approximateMessageCount}`);
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    });
});
*/