param serverfarms_asp_name string
param location string
param sku object
param kind string

resource serverfarms_asp_cryptomanager_server_test_name_resource 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: serverfarms_asp_name
  location: location
  sku: sku
  kind: kind
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: true
    isXenon: false
    hyperV: false
    zoneRedundant: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
  }
}
