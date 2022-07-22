# AZ-204 Demo: Swapping Slots of Azure Web App

In the demo you will create and deploy NodeJS web App. 
Then create a slot and deploy another version of the Web App. 
Then you test the swap process.

## Technical Requirements:

- Install VSCode and Azure CLI Tools.

## Demonstration:

1. Provision new LINUX (NodeJS) Web App with S1 SKU.

1. Deploy WebApp by use VS Code from folder `\myNodeJsApp\`. 

![Deploy](deploy.png)

1. Run the script **deploy.azcli** and make sure that archive works for you. 

1. Check the slot settings it also should be NodeJs web site.

1. Your main web site should be already published:

![FixMe](fixme.png)

1. Open VSCode and run the script **deploy.azcli**. Create new slot and do the deployment from zip archive. 

1. Your slot with new version should looks like following:

![Fixed](fixed.png)

1. Now you can return to the portal and set up Canary Deployment by provide 5% of traffic to new slot.

![Fixed](swap.png)

1. Now you can swap slots from portal or by use scrip **deploy.azcli**

1. After swapping your MAIN site should looks fixed.

