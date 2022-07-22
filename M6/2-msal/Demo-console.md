## AZ-204 Demo: Interactive authentication by using MSAL

In the demo you will run console client that use MSAL SDK to request token to Microsoft Graph.
You will validate created token. 

## Technical Requirements:

- Visual Studio Code
- Net 5.0

## Demonstration

1. Open **CSharp\Console** in VS Code.

1. Update `CSharp\Console\appsettings.json` file with values from your tenant and App you registered in previous demo.

1. Run the project locally to pull the token info

    ![TokenMSAL](CSharp/Console/screen.png)

1. Verify your token on [`https://jwt.ms/`](https://jwt.ms/)
