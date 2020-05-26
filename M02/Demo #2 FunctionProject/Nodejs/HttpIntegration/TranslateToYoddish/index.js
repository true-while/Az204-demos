const rq = require('request-promise');

module.exports = function (context, req) {
  var text;
  if (req.query && req.query.text) {
    text = req.query.text;
  }
  else if (req.body && req.body.text) {
    text = req.body.text;
  }

  var options = {
    method: 'POST',
    uri: 'http://api.funtranslations.com/translate/yoda.json',
    body: {
        text: text
    },
    json: true 
  };

  rq(options)
    .then(
      function (response) {
        context.res = {
          status: 200,
            body: response.contents.translated
          };
          context.done();
      }
    )
    .catch((error) => {
        context.res = {
          status: 500,
            body: error.message
          };
          context.done();      
    }
    );
}

