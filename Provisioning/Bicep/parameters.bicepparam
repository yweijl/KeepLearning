using './main.bicep'

param name = 'keeplearning'
param environment = 'Development'
param aspKind = 'linux'
param aspSku = 'F1'
param location = 'westeurope'
param tags = {
  App: 'keeplearning'
}
param containerName = 'resources'
