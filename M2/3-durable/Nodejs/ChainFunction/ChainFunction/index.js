//ChainFunction

const df = require("durable-functions");

module.exports = df.orchestrator(function*(context){
    context.log("Starting chain sample");

    var  f1  = yield context.df.callActivity("F1", "start");
    var  f2  = yield context.df.callActivity("F2", f1);
    var  f3  = yield context.df.callActivity("F3", f2);

    return f3;
});
