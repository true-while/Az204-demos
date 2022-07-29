# AZ-204 Demo: Azure Cache for Redis in Action

In the demo you will build a Redis in Azure and get connected to the Redis service from console.

## Technical Requirement

- VS Code
- Node.JS 16
- Azure Cache for Azure instance provisioned from previous demo

## Demonstration:

1. Open in VS Code folder `ConsoleClient\NodeJS` and update file `.env` with your connection string to Azure Cache for Redis.

```
REDISCACHEKEY=<redis key>
REDISCACHEHOSTNAME=<redis FQDN>
PORT=6380
```

1. Run the project and observe the output.

![redis](ConsoleClient\NodeJS\screen.png)