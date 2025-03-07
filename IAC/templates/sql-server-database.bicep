param server_sql_name string
param db_sql_name string
param location string
param sku object
param autoPauseDelay int = -1

resource servers_sql_name_sqldb 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  name: '${server_sql_name}/${db_sql_name}'
  location: location
  sku: sku
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 34359738368
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    autoPauseDelay: autoPauseDelay
    requestedBackupStorageRedundancy: 'Local'
    minCapacity: json('0.5')
    isLedgerOn: false
    useFreeLimit: true
    freeLimitExhaustionBehavior: 'AutoPause'
    availabilityZone: 'NoPreference'
  }
}
