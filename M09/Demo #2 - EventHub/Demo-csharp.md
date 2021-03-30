# AZ-204 Demo: Event Hubs in Action

In the demo you will send and receive events from the EventGrid in Nodejs code.

## Before delivery:

- Create Azure EventHub Environment. You can use following [code sample](https://github.com/Azure/azure-event-hubs/tree/master/samples/DotNet). `Quickstart_PSsample1.ps1` will help you to set up demo and obtain the keys.
- Alternatively you can create own Event Hub and update hardcoded keys.
- Open two projects `SampleSender.sln` and `SampleEphReceiver.sln`
- Update hardcoded connection values in `Program.cs`

## In class:

1. Open project `SampleSender` and run by paly button.
2. Open project `SampleEphReceiver` and run by paly button.
1. Demonstrate output and explain eg. Why the receiving done by partition while sending does not?

![Sub](CSharp/screen.png)
