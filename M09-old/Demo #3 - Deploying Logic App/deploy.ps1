#Install-Module -Name LogicAppTemplate -RequiredVersion 1.0.10 

#Login-AzAccount
$ExistedTemplateName = "NewApp"
$ExistedTemplateNameRG = "LogicAppDemo"
$SubscriptionId = (Get-AzSubscription)[0].Id
$TemplateFile = "template-new.json"
#generate template
Get-LogicAppTemplate -LogicApp $ExistedTemplateName -ResourceGroup $ExistedTemplateNameRG -SubscriptionId $SubscriptionId -Verbose `
    | Out-File $TemplateFile

# Create new Resource Group
Login-AzAccount
New-AzureRmResourceGroup -Location "eastus2" -Name "demo2" -Force
   
New-AzureRmResourceGroupDeployment -Name "newdeployment" -ResourceGroupName "demo2" -TemplateFile $TemplateFile 
                                      
Get-AzureRmSubscription
    