@description('Log Analytics Workspace Name')
param logAnalyticsWorkspaceName string

@description('App Insights Name')
param appInsightsName string

@description('Key Vault Name')
param keyVaultName string

@description('Sql Server Name')
param sqlServerName string

@description('Database CryptoManager SKU')
param databaseCryptoManagerPlanSku object

@description('Database CryptoManager MaxSizeBytes')
param databaseCryptoManagerMaxSizeBytes int

@description('Database CryptoManager use Free Limit')
param databaseCryptoManagerUseFreeLimit bool

@description('Database CryptoManager SKU Free Limit Exhaustion Behavior')
param databaseCryptoManagerFreeLimitExhaustionBehavior string

@description('Sql Server CryptoManager Database Name')
param sqlServerCryptoManagerDBName string

@description('Database Robot Trader SKU')
param databaseRobotTraderPlanSku object

@description('Sql Server Robot Trader DB Name')
param sqlServerRobotTraderDBName string

@description('Databases History Prices SKU')
param databaseHistoryPricesPlanSku object

@description('Sql Server History Prices DB Name')
param sqlServerHistoryPricesDBName string

@description('Crypto Manager Server App Service Plan Name')
param cryptoManagerServerAppServicePlanName string

@description('Crypto Manager Server App Service Plan Sku')
param cryptoManagerServerAppServicePlanSku object

@description('Crypto Manager Server Web App Name')
param cryptoManagerServerWebAppName string

@description('Crypto Manager Client App Service Plan Name')
param cryptoManagerClientAppServicePlanName string

@description('Crypto Manager Client App Service Plan Sku')
param cryptoManagerClientAppServicePlanSku object

@description('Crypto Manager Client Web App Name')
param cryptoManagerClientWebAppName string

@description('Robot Trader API App Service Plan Name')
param robotTraderApiAppServicePlanName string

@description('Robot Trader API App Service Plan Sku')
param robotTraderApiAppServicePlanSku object

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
    sku: databaseCryptoManagerPlanSku
    maxSizeBytes: databaseCryptoManagerMaxSizeBytes
    freeLimitExhaustionBehavior: databaseCryptoManagerFreeLimitExhaustionBehavior
    useFreeLimit: databaseCryptoManagerUseFreeLimit
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
    sku: databaseRobotTraderPlanSku
    freeLimitExhaustionBehavior: 'AutoPause'
    useFreeLimit: true
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
    sku: databaseHistoryPricesPlanSku
    freeLimitExhaustionBehavior: 'AutoPause'
    useFreeLimit: true
  }
  dependsOn: [sqlServer]
}

@description('Deploy Crypto Manager API App Service')
module appServiceCryptoManagerServer 'templates/app-service.bicep' = {
  name: 'CryptoManagerServerAppServiceDeployment'
  params: {
    location: resourceGroup().location
    sites_app_name: cryptoManagerServerWebAppName
    appServicePlanName: cryptoManagerServerAppServicePlanName
    appServicePlanSku: cryptoManagerServerAppServicePlanSku
    kind: 'app,linux'
    runtime_stack: 'DOTNETCORE|8.0'
    appInsightsName: appInsightsName
  }
  dependsOn:[appInsights]
}

@description('Deploy Crypto Manager Client App Service')
module appServiceCryptoManagerClient 'templates/app-service.bicep' = {
  name: 'CryptoManagerClientAppServiceDeployment'
  params: {
    location: resourceGroup().location
    sites_app_name: cryptoManagerClientWebAppName
    appServicePlanName: cryptoManagerClientAppServicePlanName
    appServicePlanSku: cryptoManagerClientAppServicePlanSku
    kind: 'app,linux'
    runtime_stack: 'NODE|20-lts'
    appCommandLine: 'pm2 serve /home/site/wwwroot/public --no-daemon --spa'
    appInsightsName: appInsightsName
  }
  dependsOn:[appInsights]
}

@description('Deploy Robot Trader API App Service')
module appServiceRobotTraderAPI 'templates/app-service.bicep' = {
  name: 'RobotTraderAPIAppServiceDeployment'
  params: {
    location: resourceGroup().location
    sites_app_name: robotTraderApiWebAppName
    appServicePlanName: robotTraderApiAppServicePlanName
    appServicePlanSku: robotTraderApiAppServicePlanSku
    kind: 'app,linux'
    runtime_stack: 'DOTNETCORE|8.0'
    appInsightsName: appInsightsName
  }
  dependsOn:[appInsights]
}
