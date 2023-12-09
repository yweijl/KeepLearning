param name string
@allowed([ 'westeurope' ])
param location string
param tags object = {}

param environment string

param aspKind string
param aspSku string
param containerName string

module appServicePlan 'Modules/appServicePlan.bicep' = {
  name: '${name}-asp'
  params: {
    aspKind: aspKind
    aspName: '${name}-asp'
    aspSku: aspSku
    location: location
    tags: tags
  }
}

module appService 'Modules/appService.bicep' = {
  name: '${name}-as'
  params: {
    asName: '${name}-as'
    location: location
    serverFarmId: appServicePlan.outputs.id
    dbName: '${name}-db'
    containerName: containerName
    environment: environment
    tags: tags
  }
}

module cosmosDb 'Modules/cosmos.bicep' = {
  name: '${name}-db'
  params: {
    dbName: '${name}-db'
    containerName: containerName
    location: location
    appPrincipalId: appService.outputs.appPrincipalId
  }
}
