# AZ-204 Demo: Create Resource Manager templates by using Visual Studio Code

In the demo you will create new template in VS code and deploy template in Azure.

## Before delivery:

- Install VS Code, Install extension Tools CLI
- Follow the [guidelines](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/quickstart-create-templates-use-visual-studio-code?tabs=CLI) for VS Code


## In class:

1. Open the folder from VS Code 
2. Load templates **template.json** and  **parameters.json**. This is template for building storage account.  
3. Load the CLI script from the same folder and execute line by line **template.azcli**
4. Clean up RG after deployment to avoid name collision next time
5. If time permits demonstrate template development with intellisense by modifying template. 
6. If time permits demonstrate template deployment in Visual Studio from the [guidelines](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/create-visual-studio-deployment-project)