param components_app_insight_name string
param workspaces_name string
param location string

resource workspaces_name_resource 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: workspaces_name
  location: location
}

resource components_app_insight_name_resource 'microsoft.insights/components@2020-02-02' = {
  name: components_app_insight_name
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Flow_Type: 'Redfield'
    Request_Source: 'IbizaWebAppExtensionCreate'
    RetentionInDays: 90
    WorkspaceResourceId: workspaces_name_resource.id
    IngestionMode: 'Disabled'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}
