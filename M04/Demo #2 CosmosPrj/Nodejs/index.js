const dotenv = require('dotenv');
const path = require('path');
const { v1: uuidv1 } = require('uuid');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

const CosmosClient = require('@azure/cosmos').CosmosClient;
const client = new CosmosClient(process.env.connectionStr);

var savedItem;
var database;

async function init(client, databaseId, containerId, partitionKey) {

    database = await client.databases.createIfNotExists({
        id: databaseId
    });

    console.log(`Created db: ${ databaseId }\n`);

    const { container } = await client
        .database(databaseId)
        .containers.createIfNotExists(
            { id: containerId, partitionKey },
            { offerThroughput: 400 }
        );

    console.log(`Created container: ${ container.id }\n`);

    return container;
}

async function createItem(container) {

    var newItem = {
        id: uuidv1(),
        category: 'fun',
        name: 'Cosmos DB',
        description: 'Complete Cosmos DB Node.js Quickstart',
        isComplete: false
    };

    const { resource: createdItem } = await container.items.create(newItem);

    console.log(`Created new item: ${ createdItem.id } - ${ createdItem.description }\r\n`);

    return container;
}

async function queryItems(container) {

    console.log('Querying container: Items');

    // query to return all items
    const querySpec = {
        query: 'SELECT * from c'
    };

    // read all items in the Items container
    const { resources: items } = await container.items
        .query(querySpec)
        .fetchAll();

    items.forEach(item => {
        console.log(`${ item.id } - ${ item.description }`);
    });

    return { item: items.shift(), container: container };
}

async function updateItem(container, item) {

    savedItem = item;

    item.isComplete = true;

    try {
        var { resource: updatedItem } = await container
            .item(item.id)
            .replace(item, { accessCondition: { type: 'IfMatch', condition: item._etag } });

        console.log(`Updated item: ${ updatedItem.id } - ${ updatedItem.description }`); 
        console.log(`Updated isComplete to ${ updatedItem.isComplete }\r\n`);
    } catch (er) {
        console.log('Update fail');
        console.log(er.message);
    }

    return container;
}
async function cleanup(container, databaseId) {
    await container.delete();
    await client.database(databaseId).delete();
    console.log('The database is deleted');
}

init(client, process.env.databaseId, process.env.containerId, process.env.partitionKey)
    .then((container) => createItem(container)).then((container) => createItem(container)).then((container) => createItem(container))
    .then((container) => queryItems(container))
    .then((result) => updateItem(result.container, result.item))
    .then((container) => updateItem(container, savedItem))
    .then((container) => cleanup(container, process.env.databaseId));