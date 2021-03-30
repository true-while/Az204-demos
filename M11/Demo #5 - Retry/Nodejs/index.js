const dotenv = require('dotenv');
const path = require('path');
const { RetryPolicy } = require('./retryPolicy');
var azure = require('azure-storage');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

const retryPolicy = new RetryPolicy(process.env);

async function connectWithRetry() {
    var blobService = azure.createBlobService(process.env.connectionString);
    blobService.createContainerIfNotExists('democontainer', {
        publicAccessLevel: 'blob'
    }, function(error, result, response) {
        if (error) {
            if (retryPolicy.checkRetries(error.statusCode)) {
                connectWithRetry(); // start recursion
            } else {
                console.log('No luck today!');
            }
        } else {
            console.log('You are connected!');
        }
    });
}

connectWithRetry();

/*
//run the following command in termintal to deny and allow connections

az storage account update  --name "streamdemo" --default-action Deny
az storage account update  --name "streamdemo" --default-action Allow

*/