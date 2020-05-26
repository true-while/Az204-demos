//HelloSequence

const df = require("durable-functions");

module.exports = df.orchestrator(function*(context){
    context.log("Starting chain sample");
    const output = [];
    //call SyaHello function in parallel. 
    output.push(yield context.df.callActivity("SayHello", "Tokyo"));
    output.push(yield context.df.callActivity("SayHello", "Seattle"));
    output.push(yield context.df.callActivity("SayHello", "London"));

    return output;
});
