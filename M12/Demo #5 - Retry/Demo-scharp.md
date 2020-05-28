# AZ-204 Demo: Retry Policy

In the script you will demonstrate code to get access to storage account with using retry attempt.

## Before delivery:

- update `Program.cs` file with connection string to your storage account

## In class:

1. Open in VS code folder `CSharp`
1. Demonstrate code and explain the recursion.
1. Use following commands to update firewall settings in Azure Storage Account.

```CMD
az storage account update  --name "<your account name>" --default-action Deny
```

1. Run the code and demonstrate it CANNOT access to storage account.

1. Run following command to enable access and run the code again

```CMD
az storage account update  --name "<your account name>" --default-action Allow
```
1. Now the you should get access from first attempt.