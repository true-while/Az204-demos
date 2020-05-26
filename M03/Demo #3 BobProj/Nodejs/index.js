/* eslint-disable no-irregular-whitespace */
const dotenv = require('dotenv');
const path = require('path');
const { BlobServiceClient } = require('@azure/storage-blob');
const { v1: uuidv1 } = require('uuid');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

async function main() {
    // Create the BlobServiceClient object which will be used to create a container client
    const blobServiceClient = await BlobServiceClient.fromConnectionString(process.env.connectionString);

    // Create a unique name for the container.
    const containerName = 'demo' + uuidv1();

    // Get a reference to a container
    const containerClient = await blobServiceClient.getContainerClient(containerName);

    // Create the container
    const createContainerResponse = await containerClient.create();
    console.log(`Create container ${ containerName } successfully`, createContainerResponse.requestId);

    // Create a blob
    const content = 'hello world';
    const blobName = 'newblob' + new Date().getTime();
    const blockBlobClient = containerClient.getBlockBlobClient(blobName);
    const uploadBlobResponse = await blockBlobClient.upload(content, Buffer.byteLength(content));
    console.log(`Upload block blob ${ blobName } successfully`, uploadBlobResponse.requestId);

    // set blob metadata
    const metadata = {
        owner: 'me',
        approved: 'no'
    };
    await blockBlobClient.setMetadata(metadata);

    // List blobs
    var i = 1;
    for await (const blob of containerClient.listBlobsFlat()) {
        console.log(`Blob ${ i++ }: ${ blob.name }`);
    }

    // Get blob content from position 0 to the end
    const downloadBlockBlobResponse = await blockBlobClient.download(0);
    const filecontent = await streamToString(downloadBlockBlobResponse.readableStreamBody);
    console.log(`Downloaded blob content: '${ filecontent }'`);

    // Delete container
    await containerClient.delete();
    console.log('Delete the container');
};

// A helper method used to read a Node.js readable stream into string
async function streamToString(readableStream) {
    return new Promise((resolve, reject) => {
        const chunks = [];
        readableStream.on('data', (data) => {
            chunks.push(data.toString());
        });
        readableStream.on('end', () => {
            resolve(chunks.join(''));
        });
        readableStream.on('error', reject);
    });
}

main().catch((err) => {
    console.error('Error running sample:', err.message);
});


