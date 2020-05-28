// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

const dotenv = require('dotenv');
const path = require('path');
const redis = require("redis");
const bluebird = require("bluebird");

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

bluebird.promisifyAll(redis.RedisClient.prototype);
bluebird.promisifyAll(redis.Multi.prototype);

async function testCache() {
    // Add your cache name and access key.
    var cacheConnection = redis.createClient(process.env.PORT, process.env.REDISCACHEHOSTNAME,
    {auth_pass: process.env.REDISCACHEKEY, tls: {servername: process.env.REDISCACHEHOSTNAME}});

     // Simple PING command
     console.log("\nCache command: PING");
     console.log("Cache response : " + await cacheConnection.pingAsync());
 
    //  // Simple get and put of integral data types into the cache
    //  console.log("\nCache command: GET Message");
    //  console.log("Cache response : " + await cacheConnection.getAsync("Message"));    
 
     console.log("\nCache command: SET Message");
     console.log("Cache response : " + await cacheConnection.setAsync("Message",
         "Hello! The cache is working from Node.js!"));    
 
     // Demonstrate "SET Message" executed as expected...
     console.log("\nCache command: GET Message");
     console.log("Cache response : " + await cacheConnection.getAsync("Message"));    
 
     // Get the client list, useful to see if connection list is growing...
     console.log("\nCache command: CLIENT LIST");
     console.log("Cache response : " + await cacheConnection.clientAsync("LIST"));    
 }

 testCache();