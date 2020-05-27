const dotenv = require('dotenv');
const path = require('path');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

var AuthenticationContext = require('adal-node').AuthenticationContext;

var authorityHostUrl = 'https://login.windows.net';
var tenantid = process.env.tenant; // AAD Tenant name.
var authorityUrl = authorityHostUrl + '/' + tenantid;
var applicationId = process.env.applicationId; // Application Id of app registered under AAD.
var clientSecret = process.env.clientSecret; // Secret generated for app. Read this environment variable.
var resource = process.env.resource;

var context = new AuthenticationContext(authorityUrl);

context.acquireTokenWithClientCredentials(resource, applicationId, clientSecret, 
    function(err, tokenResponse) {
        if (err) {
            console.log('well that didn\'t work: ' + err.stack);
        } else {
            console.log(tokenResponse.accessToken);
        }
    });
