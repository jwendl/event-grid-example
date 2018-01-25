#!/bin/bash

LOCATION="westus2"
STORAGE_ACCOUNT_NAME="jwfunstorage"
PLAN_NAME="egexplan"
FUNCTION_APP_NAME="jwegexapp"
NAMESPACE_NAME="jwegehns"
EVENT_HUB_NAME="jwegeh"
CONSUMER_GROUP_NAME="eventcg"
RESOURCE_GROUP_NAME="EventGridExample"
SUBSCRIPTION_NAME="jwegcapture"

az group create --name $RESOURCE_GROUP_NAME --location $LOCATION

az group deployment create --resource-group $RESOURCE_GROUP_NAME --template-uri "https://raw.githubusercontent.com/azure/azure-quickstart-templates/master/201-event-hubs-create-event-hub-and-consumer-group/azuredeploy.json" --parameters "{ \"namespaceName\": \"$NAMESPACE_NAME\", \"eventHubName\": \"$EVENT_HUB_NAME\", \"consumerGroupName\": \"$CONSUMER_GROUP_NAME\" }"

az storage account create --resource-group $RESOURCE_GROUP_NAME --name $STORAGE_ACCOUNT_NAME --sku STANDARD_LRS

az appservice plan create --resource-group $RESOURCE_GROUP_NAME --location $LOCATION --name $PLAN_NAME

az functionapp create --resource-group $RESOURCE_GROUP_NAME --name $FUNCTION_APP_NAME --storage-account $STORAGE_ACCOUNT_NAME --plan $PLAN_NAME

NAMESPACE_ID=$(az resource show --namespace Microsoft.EventHub --resource-type namespaces --name $NAMESPACE_NAME --resource-group $RESOURCE_GROUP_NAME --query id --output tsv)
FUNCTION_APP_ENDPOINT=$(az functionapp show --resource-group $RESOURCE_GROUP_NAME --name $FUNCTION_APP_NAME).endpoint
az eventgrid event-subscription create --resource-id $NAMESPACE_ID --name $SUBSCRIPTION_NAME --endpoint $FUNCTION_APP_ENDPOINT

