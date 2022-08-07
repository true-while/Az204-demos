# AZ-204 Demo: Send and receive messages from a Service Bus Queue

In the demo, you will use console applications to send and receive messages from queues and topics by leveraging transactions, sessions and dead-letter queue. 

## Technical requirements:

- Azure CLI or Cloud Shell
- .Net 6.0

## Demonstration:

1. Open **simple-messaging/publisher**  project and locate `Program.cs`.

1. In the code of the `Program.cs` update the connection string with your connection info from the Azure Service Bus Queue created from previous demo.

1. Build and run the `publisher` that send a few messages in the queue. You can observer the messages from Azure Portal by using Service Bus explorer.

    ![publisher](/4-sdk-sb/simple-messaging/publisher.png)

1. Repeat the same steps with updating `Program.cs` for **/simple-messaging/consumer** project.

1. Build and run the `consumer` that receive the messages sent by `publisher`. 

1. [1] option demonstrate you receiving 1 message and lock it with PickLock option and then delete it; [2] option demonstrate you receiving and explicit delete it later. [3] option will peak messages to observe without deletion.

    ![consumer](/4-sdk-sb/simple-messaging/consumer.png)
