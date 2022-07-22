# AZ-204 Demo: Stored Proc and Triggers

In this demo you will connect to provisioned Cosmos DB.
You will create a new SP from the portal and test it from the portal.
You will create a new Trigger and run the code to test the trigger.

## Technical Requirements:
- Visual Studio Code
- net 5.0

## Demonstration

1. From the previously created Cosmos DB pull the key and endpoint from the Azure Portal.

1. In the same Cosmos DB find container with name **TheDemo**

1. Create new Stored Procedure from file:  [sp](sp.js)

1. From the portal execute SP and received the output: "Hello World"

1. Create new **PRE** **INSERT** trigger from file:  [trigger](trigger.js)

1. Open folder from Visual Studio Code and updated `Program.cs`

1. From the console run commands `dotnet build` and `dotnet run`

1. Monitor console for the code output. The first insert should be accepted an the second insert should be rejected.