# Deploy infrastructure using Azure Powershell

## Requisites

1. [Install the Azure PowerShell module](https://docs.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-1.7.0)

## Deploy

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fgithub.com%2Ffelipecruz91%2Ftweet-sentiment-analysis%2Ftree%2Fmaster%2Farm-template%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>

or deploy this template using the following commands:

Connect to Azure with a browser sign in token:

    Connect-AzAccount

Validate the template

    Test-AzResourceGroupDeployment -ResourceGroupName rg-azure-bootcamp -TemplateFile azuredeploy.json -TemplateParameterFile azuredeploy.parameters.json -v

    VERBOSE: 11:43:20 - Template is valid.

Deploy the template
    New-AzResourceGroupDeployment -ResourceGroupName rg-azure-bootcamp -TemplateFile azuredeploy.json -TemplateParameterFile azuredeploy.parameters.json -v
