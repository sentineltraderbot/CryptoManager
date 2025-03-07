param servers_sql_name string = 'sql-sentineltrader-test-eun'
param location string

resource servers_sql_name_resource 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: servers_sql_name
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Disabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'User'
      login: 'support_sentineltraderbot.com#EXT#@supportsentineltraderbot.onmicrosoft.com'
      sid: 'ac015f3b-adde-4d46-acfe-4daa23879295'
      tenantId: '92e390e7-d44b-4985-82fb-bf95db948e50'
      azureADOnlyAuthentication: true
    }
    restrictOutboundNetworkAccess: 'Disabled'
  }
}

resource servers_sql_name_ActiveDirectory 'Microsoft.Sql/servers/administrators@2024-05-01-preview' = {
  parent: servers_sql_name_resource
  name: 'ActiveDirectory'
  properties: {
    administratorType: 'ActiveDirectory'
    login: 'support_sentineltraderbot.com#EXT#@supportsentineltraderbot.onmicrosoft.com'
    sid: 'ac015f3b-adde-4d46-acfe-4daa23879295'
    tenantId: '92e390e7-d44b-4985-82fb-bf95db948e50'
  }
}
