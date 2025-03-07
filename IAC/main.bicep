@description('Log Analytics Workspace Name')
param logAnalyticsWorkspaceName string

@description('App Insights Name')
param appInsightsName string

@description('Key Vault Name')
param keyVaultName string

@description('App Service Plan Name')
param appServicePlanName string

@description('App Service Plan Sku')
param appServicePlanSku object

@description('Sql Server Name')
param sqlServerName string

@description('Databases SKU')
param databasesPlanSku object

@description('Sql Server CryptoManager Database Name')
param sqlServerCryptoManagerDBName string

@description('Sql Server Robot Trader DB Name')
param sqlServerRobotTraderDBName string

@description('Sql Server History Prices DB Name')
param sqlServerHistoryPricesDBName string

@description('Crypto Manager Server Web App Name')
param cryptoManagerServerWebAppName string

@description('Crypto Manager Client Web App Name')
param cryptoManagerClientWebAppName string

@description('Robot Trader API Web App Name')
param robotTraderApiWebAppName string

@description('Deploy Log Analytics Workspace')
module logAnalyticsWorkspace 'templates/log-analytics-workspace.bicep' = {
  name: 'LogAnalyticsWorkspaceDeployment'
  params: {
    workspaces_name: logAnalyticsWorkspaceName
    location: resourceGroup().location
  }
}

@description('Deploy App Insights')
module appInsights 'templates/app-insights.bicep' = {
  name: 'AppInsightsDeployment'
  params: {
    components_app_insight_name: appInsightsName
    workspaces_name: logAnalyticsWorkspaceName
    location: resourceGroup().location
  }
  dependsOn: [
    logAnalyticsWorkspace
  ]
}

@description('Deploy Key Vault')
module keyVault 'templates/keyvault.bicep' = {
  name: 'KeyVaultDeployment'
  params: {
    vaults_name: keyVaultName
    location: resourceGroup().location
    tenantId: tenant().tenantId
  }
}

@description('Deploy SQL Server')
module sqlServer 'templates/sql-server.bicep' = {
  name: 'SqlServerDeployment'
  params: {
    servers_sql_name: sqlServerName
    location: resourceGroup().location
  }
}

@description('Deploy SQL Server Crypto Manager DB')
module sqlServerCryptoManagerDB 'templates/sql-server-database.bicep' = {
  name: 'SqlServerCryptoManagerDBDeployment'
  params: {
    server_sql_name: sqlServerName
    location: resourceGroup().location
    db_sql_name: sqlServerCryptoManagerDBName
    autoPauseDelay: 60
    sku: databasesPlanSku
  }
  dependsOn: [sqlServer]
}

@description('Deploy SQL Server Robot Trader DB')
module sqlServerRobotTraderDB 'templates/sql-server-database.bicep' = {
  name: 'SqlServerRobotTraderDBDeployment'
  params: {
    server_sql_name: sqlServerName
    location: resourceGroup().location
    db_sql_name: sqlServerRobotTraderDBName
    autoPauseDelay: 60
    sku: databasesPlanSku
  }
  dependsOn: [sqlServer]
}

@description('Deploy SQL Server History Prices DB')
module sqlServerHistoryPricesDB 'templates/sql-server-database.bicep' = {
  name: 'SqlServerHistoryPricesDBDeployment'
  params: {
    server_sql_name: sqlServerName
    location: resourceGroup().location
    db_sql_name: sqlServerHistoryPricesDBName
    autoPauseDelay: 60
    sku: databasesPlanSku
  }
  dependsOn: [sqlServer]
}

@description('Deploy App Service Plan')
module appServicePlan 'templates/app-service-plan.bicep' = {
  name: 'AppServicePlanDeployment'
  params: {
    location: resourceGroup().location
    serverfarms_asp_name: appServicePlanName
    sku: appServicePlanSku
    kind: 'linux'
  }
}

@description('Deploy Crypto Manager API App Service')
module appServiceCryptoManagerServer 'templates/app-service.bicep' = {
  name: 'CryptoManagerServerAppServiceDeployment'
  params: {
    location: resourceGroup().location
    sites_app_name: cryptoManagerServerWebAppName
    appServicePlanName: appServicePlanName
    kind: 'app,linux'
    runtime_stack: 'DOTNETCORE|8.0'
    appInsightsName: appInsightsName
  }
  dependsOn:[appServicePlan, appInsights]
}

@description('Deploy Crypto Manager Client App Service')
module appServiceCryptoManagerClient 'templates/app-service.bicep' = {
  name: 'CryptoManagerClientAppServiceDeployment'
  params: {
    location: resourceGroup().location
    sites_app_name: cryptoManagerClientWebAppName
    appServicePlanName: appServicePlanName
    kind: 'app,linux'
    runtime_stack: 'NODE|20-lts'
    appCommandLine: 'pm2 serve /home/site/wwwroot/public --no-daemon --spa'
    appInsightsName: appInsightsName
  }
  dependsOn:[appServicePlan, appInsights]
}

@description('Deploy Robot Trader API App Service')
module appServiceRobotTraderAPI 'templates/app-service.bicep' = {
  name: 'RobotTraderAPIAppServiceDeployment'
  params: {
    location: resourceGroup().location
    sites_app_name: robotTraderApiWebAppName
    appServicePlanName: appServicePlanName
    kind: 'app,linux'
    runtime_stack: 'DOTNETCORE|8.0'
    appInsightsName: appInsightsName
  }
  dependsOn:[appServicePlan, appInsights]
}
