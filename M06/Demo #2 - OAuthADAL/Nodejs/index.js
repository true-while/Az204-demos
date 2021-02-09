const express = require('express');
const logger = require('connect-logger');
const cookieParser = require('cookie-parser');
const session = require('cookie-session');
const crypto = require('crypto');
const dotenv = require('dotenv');
const path = require('path');

// Import required configuration.
const ENV_FILE = path.join(__dirname, '.env');
dotenv.config({ path: ENV_FILE });

const AuthenticationContext = require('adal-node').AuthenticationContext;

var app = express();
app.use(logger());
app.use(cookieParser('a deep secret'));
app.use(session({ secret: '1234567890QWERTY' }));

app.get('/', function(req, res) {
    res.redirect('login');
});

const authorityHostUrl = 'https://login.windows.net';

var authorityUrl = authorityHostUrl + '/' + process.env.tenant;
var redirectUri = 'http://localhost:3000/getAToken';
var resource = process.env.resource;

var templateAuthzUrl = 'https://login.windows.net/' + process.env.tenant + '/oauth2/authorize?response_type=code&client_id=<client_id>&redirect_uri=<redirect_uri>&state=<state>&resource=<resource>';

app.get('/', function(req, res) {
    res.redirect('/login');
});

app.get('/login', function(req, res) {
    console.log(req.cookies);
    res.cookie('acookie', 'this is a cookie');
    res.send(`<head>
        <title>test</title>
        </head>
        <body>
        <a href="./auth">Login</a>
        </body>
            `);
});

function createAuthorizationUrl(state) {
    var authorizationUrl = templateAuthzUrl.replace('<client_id>', process.env.applicationId);
    authorizationUrl = authorizationUrl.replace('<redirect_uri>', redirectUri);
    authorizationUrl = authorizationUrl.replace('<state>', state);
    authorizationUrl = authorizationUrl.replace('<resource>', resource);
    return authorizationUrl;
}

// Clients get redirected here in order to create an OAuth authorize url and redirect them to AAD.
// There they will authenticate and give their consent to allow this app access to
// some resource they own.
app.get('/auth', function(req, res) {
    crypto.randomBytes(48, function(ex, buf) {
        var token = buf.toString('base64').replace(/\//g,'_').replace(/\+/g,'-');

        res.cookie('authstate', token);
        var authorizationUrl = createAuthorizationUrl(token);

        res.redirect(authorizationUrl);
    });
});

// After consent is granted AAD redirects here.  The ADAL library is invoked via the
// AuthenticationContext and retrieves an access token that can be used to access the
// user owned resource.
app.get('/getAToken', function(req, res) {
    if (req.cookies.authstate !== req.query.state) {
        res.send('error: state does not match');
    }
    var authenticationContext = new AuthenticationContext(authorityUrl);
    authenticationContext.acquireTokenWithAuthorizationCode(req.query.code, redirectUri, resource, process.env.applicationId, process.env.clientSecret, 
        function(err, response) {
            var message = '';
            if (err) {
                message = 'error: ' + err.message + '\n';
            }
            message += '<b>'+response.accessToken +'</b><br\>';
            message += 'response: ' + JSON.stringify(response);

            if (err) {
                res.send(message);
                return;
            }

            // Later, if the access token is expired it can be refreshed.
            authenticationContext.acquireTokenWithRefreshToken(response.refreshToken, process.env.applicationId, process.env.clientSecret, resource, function(refreshErr, refreshResponse) {
                if (refreshErr) {
                    message += 'refreshError: ' + refreshErr.message + '\n';
                }
                message += 'refreshResponse: ' + JSON.stringify(refreshResponse);

                res.send(message);
            });
        });
});

app.listen(3000);
console.log('listening on http://localhost:3000 (access in private browser)');
