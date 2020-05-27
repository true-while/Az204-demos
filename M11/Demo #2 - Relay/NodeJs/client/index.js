const https = require('hyco-https');

const dotenv = require('dotenv');
const path = require('path');
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });


https.get({
    hostname : process.env.ns,
    path : (!process.env.nspath || process.env.nspath.length == 0 || process.env.nspath[0] !== '/'?'/':'') + process.env.nspath,
    port : 443,
    headers : {
        'ServiceBusAuthorization' : 
            https.createRelayToken(https.createRelayHttpsUri(process.env.ns, process.env.nspath), process.env.keyrule, process.env.key)
    }
}, (res) => {
    let error;
    if (res.statusCode !== 200) {
        console.error('Request Failed.\n Status Code: ${statusCode}');
        res.resume();
    } 
    else {
        res.setEncoding('utf8');
        res.on('data', (chunk) => {
            console.log(`Receive from ${ res.headers.via }:`)
            console.log(chunk);
        });
        res.on('end', () => {
            console.log('No more data in response.');
        });
    };
}).on('error', (e) => {
    console.error(`Got error: ${e.message}`);
});