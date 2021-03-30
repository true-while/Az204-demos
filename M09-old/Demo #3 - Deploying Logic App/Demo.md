# AZ-204 Demo: Deploy Logic app from Template

In the demo you will design new logic app from portal and run the test.

## Before delivery:

- Build your demo from following [tutorial](https://docs.microsoft.com/en-us/learn/modules/create-deploy-logic-apps-using-arm-templates/5-exercise-deploy-a-logic-app-using-a-parameterized-arm-template)

## In class:

1. By using following command deploy your web app from template:

```cmd
az deployment group create \
--resource-group [sandbox resource group name] \
--template-file template-new.json\
--parameters '{ "logicAppName": {"value":"MyLogicApp2"}, "location": {"value":"East US"}}'
```

You also can use the Powershell script **deploy.ps1**

![Deploy PS](deploying.jpg)
