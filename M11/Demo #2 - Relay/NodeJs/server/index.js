const https = require('hyco-https');
var os = require("os");
const dotenv = require('dotenv');
const path = require('path');
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

var uri = https.createRelayListenUri(process.env.ns, process.env.nspath);

var server = https.createRelayedServer(
    {
        server : uri,
        token : () => https.createRelayToken(uri, process.env.keyrule, process.env.key)
    },
    (req, res) => {
        console.log(`request accepted from "${ req.headers.host }"`);
        res.setHeader('Content-Type', 'text/json');
        res.end(`{ "msg": "Hello from ${ os.hostname() }" }`);
    })
    .on('error', (err) => {
        console.log('error: ' + err);
    }).listen((err) => {
        if (err) {
            return console.log('something bad happened', err);
        }
    });

console.log(`Server running at http://${os.hostname()}:443/`);
