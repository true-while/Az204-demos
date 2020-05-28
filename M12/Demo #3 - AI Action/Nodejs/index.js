var apikey='<your key>';
var appInsights = require("applicationinsights");
//contosoadsweb
appInsights.setup(apikey).setInternalLogging(true, true).start(); 
appInsights.defaultClient.config.maxBatchSize = 0;
var client = appInsights.defaultClient;

//track custom event 
client.trackEvent({name: "Market performance", properties: { CowsSold: 5, HorsSold: 12 }});

//tack metric
client.trackMetric({name: "Cows sold", value: 3});

//Track exception
client.trackException({exception: new Error("handled exceptions can be logged with this method")});

//trace message
client.trackTrace({message: "trace the message"});

//track dependency
client.trackDependency({target:"http://dbname", name:"select customers", 
          data:"SELECT * FROM Customers", duration:231, 
              resultCode:0, success: true, dependencyTypeName: "ZSQL"});

//track request
client.trackRequest({name:"GET /customers", 
          url:"http://myserver/customers", 
              duration:309, resultCode:200, success:true});


let http = require("http");
console.log("Check your the server on http://localhost:8080")
http.createServer( (req, res) => {
  res.writeHead(200, {'Content-Type': 'text/html'});
  res.write(`<!DOCTYPE html>
  <html>
  <head>
      <meta charset="utf-8" />
      <meta name="viewport" content="width=device-width, initial-scale=1.0">
      <title>The Web Server Test</title>
      <script type = 'text/javascript' >
          var appInsights=window.appInsights||function(config)
          {
              function r(config){ t[config] = function(){ var i = arguments; t.queue.push(function(){ t[config].apply(t, i)})} }
              var t = { config:config},u=document,e=window,o='script',s=u.createElement(o),i,f;for(s.src=config.url||'//az416426.vo.msecnd.net/scripts/a/ai.0.js',u.getElementsByTagName(o)[0].parentNode.appendChild(s),t.cookie=u.cookie,t.queue=[],i=['Event','Exception','Metric','PageView','Trace','Ajax'];i.length;)r('track'+i.pop());return r('setAuthenticatedUserContext'),r('clearAuthenticatedUserContext'),config.disableExceptionTracking||(i='onerror',r('_'+i),f=e[i],e[i]=function(config, r, u, e, o) { var s = f && f(config, r, u, e, o); return s !== !0 && t['_' + i](config, r, u, e, o),s}),t
          }({
              instrumentationKey:'${ apikey }'
          });
          
          window.appInsights=appInsights;
          appInsights.trackPageView();
      </script>
  </head>
  <body>Hello World!</body>
</html>
  `);
  res.end();
  client.trackNodeHttpRequest({request: req, response: res}); 
}).listen(8080);



