IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    )
END


IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20180309190231_InitialMigration')
BEGIN

    BEGIN TRANSACTION
    
    
    CREATE TABLE [AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    )
    

    CREATE TABLE [AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
        [AccessFailedCount] int NOT NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [Email] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [FacebookId] bigint NULL,
        [FirstName] nvarchar(max) NULL,
        [Gender] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [Locale] nvarchar(max) NULL,
        [LockoutEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [PasswordHash] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [PictureUrl] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [UserName] nvarchar(256) NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    )
    

    CREATE TABLE [Asset] (
        [Id] uniqueidentifier NOT NULL,
        [Description] nvarchar(max) NULL,
        [IsEnabled] bit NOT NULL,
        [IsExcluded] bit NOT NULL,
        [Name] nvarchar(max) NULL,
        [RegistryDate] datetime2 NOT NULL,
        [Symbol] nvarchar(max) NULL,
        CONSTRAINT [PK_Asset] PRIMARY KEY ([Id])
    )
    

    CREATE TABLE [Exchange] (
        [Id] uniqueidentifier NOT NULL,
        [APIUrl] nvarchar(max) NULL,
        [IsEnabled] bit NOT NULL,
        [IsExcluded] bit NOT NULL,
        [Name] nvarchar(max) NULL,
        [RegistryDate] datetime2 NOT NULL,
        [Website] nvarchar(max) NULL,
        CONSTRAINT [PK_Exchange] PRIMARY KEY ([Id])
    )
    

    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id])
    )
    

    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    )
    

    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    )
    

    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    )
    

    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id])
    )
    

    CREATE TABLE [Order] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicationUserId] uniqueidentifier NOT NULL,
        [BaseAssetId] uniqueidentifier NOT NULL,
        [Date] datetime2 NOT NULL,
        [ExchangeId] uniqueidentifier NOT NULL,
        [IsEnabled] bit NOT NULL,
        [IsExcluded] bit NOT NULL,
        [QuoteAssetId] uniqueidentifier NOT NULL,
        [RegistryDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Order] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Order_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
        CONSTRAINT [FK_Order_Asset_BaseAssetId] FOREIGN KEY ([BaseAssetId]) REFERENCES [Asset] ([Id]),
        CONSTRAINT [FK_Order_Exchange_ExchangeId] FOREIGN KEY ([ExchangeId]) REFERENCES [Exchange] ([Id]),
        CONSTRAINT [FK_Order_Asset_QuoteAssetId] FOREIGN KEY ([QuoteAssetId]) REFERENCES [Asset] ([Id])
    )
    

    CREATE TABLE [OrderItem] (
        [Id] uniqueidentifier NOT NULL,
        [Fee] decimal(18,2) NOT NULL,
        [FeeAssetId] uniqueidentifier NOT NULL,
        [IsEnabled] bit NOT NULL,
        [IsExcluded] bit NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Quantity] decimal(18,2) NOT NULL,
        [RegistryDate] datetime2 NOT NULL,
        CONSTRAINT [PK_OrderItem] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItem_Asset_FeeAssetId] FOREIGN KEY ([FeeAssetId]) REFERENCES [Asset] ([Id]),
        CONSTRAINT [FK_OrderItem_Order_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Order] ([Id])
    )
    

    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId])
    

    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL
    

    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId])
    

    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId])
    

    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId])
    

    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail])
    

    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL
    

    CREATE INDEX [IX_Order_ApplicationUserId] ON [Order] ([ApplicationUserId])
    

    CREATE INDEX [IX_Order_BaseAssetId] ON [Order] ([BaseAssetId])
    

    CREATE INDEX [IX_Order_ExchangeId] ON [Order] ([ExchangeId])
    

    CREATE INDEX [IX_Order_QuoteAssetId] ON [Order] ([QuoteAssetId])
    

    CREATE INDEX [IX_OrderItem_FeeAssetId] ON [OrderItem] ([FeeAssetId])
    

    CREATE INDEX [IX_OrderItem_OrderId] ON [OrderItem] ([OrderId])
    

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20180309190231_InitialMigration', N'8.0.13')
    

    COMMIT
END


IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20210716142045_dotnet3migration')
BEGIN
    BEGIN TRANSACTION
    

    ALTER TABLE [Exchange] ADD [ExchangeType] int NOT NULL DEFAULT 0
    

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210716142045_dotnet3migration', N'8.0.13')
    

    COMMIT
    
END


IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20230329141039_OrderRelationship')
BEGIN

    BEGIN TRANSACTION
    

    DROP INDEX [UserNameIndex] ON [AspNetUsers]
    

    DROP INDEX [RoleNameIndex] ON [AspNetRoles]
    

    DECLARE @var0 sysname
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'RegistryDate')
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var0 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [RegistryDate] datetime2 NOT NULL
    

    DECLARE @var1 sysname
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Quantity')
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var1 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [Quantity] decimal(18,2) NOT NULL
    

    DECLARE @var2 sysname
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Price')
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var2 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [Price] decimal(18,2) NOT NULL
    

    DROP INDEX [IX_OrderItem_OrderId] ON [OrderItem]
    DECLARE @var3 sysname
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'OrderId')
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var3 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [OrderId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_OrderItem_OrderId] ON [OrderItem] ([OrderId])
    

    DECLARE @var4 sysname
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'IsExcluded')
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var4 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [IsExcluded] bit NOT NULL
    

    DECLARE @var5 sysname
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'IsEnabled')
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var5 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [IsEnabled] bit NOT NULL
    

    DROP INDEX [IX_OrderItem_FeeAssetId] ON [OrderItem]
    DECLARE @var6 sysname
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'FeeAssetId')
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var6 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [FeeAssetId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_OrderItem_FeeAssetId] ON [OrderItem] ([FeeAssetId])
    

    DECLARE @var7 sysname
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Fee')
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var7 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [Fee] decimal(18,2) NOT NULL
    

    DECLARE @var8 sysname
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Id')
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var8 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [Id] uniqueidentifier NOT NULL
    

    DECLARE @var9 sysname
    SELECT @var9 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'RegistryDate')
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var9 + ']')
    ALTER TABLE [Order] ALTER COLUMN [RegistryDate] datetime2 NOT NULL
    

    DROP INDEX [IX_Order_QuoteAssetId] ON [Order]
    DECLARE @var10 sysname
    SELECT @var10 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'QuoteAssetId')
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var10 + ']')
    ALTER TABLE [Order] ALTER COLUMN [QuoteAssetId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_Order_QuoteAssetId] ON [Order] ([QuoteAssetId])
    

    DECLARE @var11 sysname
    SELECT @var11 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'IsExcluded')
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var11 + ']')
    ALTER TABLE [Order] ALTER COLUMN [IsExcluded] bit NOT NULL
    

    DECLARE @var12 sysname
    SELECT @var12 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'IsEnabled')
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var12 + ']')
    ALTER TABLE [Order] ALTER COLUMN [IsEnabled] bit NOT NULL
    

    DROP INDEX [IX_Order_ExchangeId] ON [Order]
    DECLARE @var13 sysname
    SELECT @var13 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'ExchangeId')
    IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var13 + ']')
    ALTER TABLE [Order] ALTER COLUMN [ExchangeId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_Order_ExchangeId] ON [Order] ([ExchangeId])
    

    DECLARE @var14 sysname
    SELECT @var14 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'Date')
    IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var14 + ']')
    ALTER TABLE [Order] ALTER COLUMN [Date] datetime2 NOT NULL
    

    DROP INDEX [IX_Order_BaseAssetId] ON [Order]
    DECLARE @var15 sysname
    SELECT @var15 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'BaseAssetId')
    IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var15 + ']')
    ALTER TABLE [Order] ALTER COLUMN [BaseAssetId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_Order_BaseAssetId] ON [Order] ([BaseAssetId])
    

    DROP INDEX [IX_Order_ApplicationUserId] ON [Order]
    DECLARE @var16 sysname
    SELECT @var16 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'ApplicationUserId')
    IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var16 + ']')
    ALTER TABLE [Order] ALTER COLUMN [ApplicationUserId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_Order_ApplicationUserId] ON [Order] ([ApplicationUserId])
    

    DECLARE @var17 sysname
    SELECT @var17 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Order]') AND [c].[name] = N'Id')
    IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Order] DROP CONSTRAINT [' + @var17 + ']')
    ALTER TABLE [Order] ALTER COLUMN [Id] uniqueidentifier NOT NULL
    

    ALTER TABLE [Order] ADD [IsViaRoboTrader] bit NOT NULL DEFAULT 0
    

    ALTER TABLE [Order] ADD [OrderType] bit NOT NULL DEFAULT 0
    

    ALTER TABLE [Order] ADD [RelatedOrderId] uniqueidentifier NULL
    

    DECLARE @var18 sysname
    SELECT @var18 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'Website')
    IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var18 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [Website] nvarchar(max) NULL
    

    DECLARE @var19 sysname
    SELECT @var19 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'RegistryDate')
    IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var19 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [RegistryDate] datetime2 NOT NULL
    

    DECLARE @var20 sysname
    SELECT @var20 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'Name')
    IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var20 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [Name] nvarchar(max) NULL
    

    DECLARE @var21 sysname
    SELECT @var21 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'IsExcluded')
    IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var21 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [IsExcluded] bit NOT NULL
    

    DECLARE @var22 sysname
    SELECT @var22 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'IsEnabled')
    IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var22 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [IsEnabled] bit NOT NULL
    

    DECLARE @var23 sysname
    SELECT @var23 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'ExchangeType')
    IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var23 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [ExchangeType] bit NOT NULL
    

    DECLARE @var24 sysname
    SELECT @var24 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'APIUrl')
    IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var24 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [APIUrl] nvarchar(max) NULL
    

    DECLARE @var25 sysname
    SELECT @var25 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Exchange]') AND [c].[name] = N'Id')
    IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [Exchange] DROP CONSTRAINT [' + @var25 + ']')
    ALTER TABLE [Exchange] ALTER COLUMN [Id] uniqueidentifier NOT NULL
    

    DECLARE @var26 sysname
    SELECT @var26 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Asset]') AND [c].[name] = N'Symbol')
    IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [Asset] DROP CONSTRAINT [' + @var26 + ']')
    ALTER TABLE [Asset] ALTER COLUMN [Symbol] nvarchar(max) NULL
    

    DECLARE @var27 sysname
    SELECT @var27 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Asset]') AND [c].[name] = N'RegistryDate')
    IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [Asset] DROP CONSTRAINT [' + @var27 + ']')
    ALTER TABLE [Asset] ALTER COLUMN [RegistryDate] datetime2 NOT NULL
    

    DECLARE @var28 sysname
    SELECT @var28 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Asset]') AND [c].[name] = N'Name')
    IF @var28 IS NOT NULL EXEC(N'ALTER TABLE [Asset] DROP CONSTRAINT [' + @var28 + ']')
    ALTER TABLE [Asset] ALTER COLUMN [Name] nvarchar(max) NULL
    

    DECLARE @var29 sysname
    SELECT @var29 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Asset]') AND [c].[name] = N'IsExcluded')
    IF @var29 IS NOT NULL EXEC(N'ALTER TABLE [Asset] DROP CONSTRAINT [' + @var29 + ']')
    ALTER TABLE [Asset] ALTER COLUMN [IsExcluded] bit NOT NULL
    

    DECLARE @var30 sysname
    SELECT @var30 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Asset]') AND [c].[name] = N'IsEnabled')
    IF @var30 IS NOT NULL EXEC(N'ALTER TABLE [Asset] DROP CONSTRAINT [' + @var30 + ']')
    ALTER TABLE [Asset] ALTER COLUMN [IsEnabled] bit NOT NULL
    

    DECLARE @var31 sysname
    SELECT @var31 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Asset]') AND [c].[name] = N'Description')
    IF @var31 IS NOT NULL EXEC(N'ALTER TABLE [Asset] DROP CONSTRAINT [' + @var31 + ']')
    ALTER TABLE [Asset] ALTER COLUMN [Description] nvarchar(max) NULL
    

    DECLARE @var32 sysname
    SELECT @var32 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Asset]') AND [c].[name] = N'Id')
    IF @var32 IS NOT NULL EXEC(N'ALTER TABLE [Asset] DROP CONSTRAINT [' + @var32 + ']')
    ALTER TABLE [Asset] ALTER COLUMN [Id] uniqueidentifier NOT NULL
    

    DECLARE @var33 sysname
    SELECT @var33 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'Value')
    IF @var33 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var33 + ']')
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [Value] nvarchar(max) NULL
    

    DECLARE @var34 sysname
    SELECT @var34 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'Name')
    IF @var34 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var34 + ']')
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [Name] nvarchar(450) NOT NULL
    

    DECLARE @var35 sysname
    SELECT @var35 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'LoginProvider')
    IF @var35 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var35 + ']')
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [LoginProvider] nvarchar(450) NOT NULL
    

    DECLARE @var36 sysname
    SELECT @var36 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserTokens]') AND [c].[name] = N'UserId')
    IF @var36 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserTokens] DROP CONSTRAINT [' + @var36 + ']')
    ALTER TABLE [AspNetUserTokens] ALTER COLUMN [UserId] uniqueidentifier NOT NULL
    

    DECLARE @var37 sysname
    SELECT @var37 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'UserName')
    IF @var37 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var37 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [UserName] nvarchar(256) NULL
    

    DECLARE @var38 sysname
    SELECT @var38 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'TwoFactorEnabled')
    IF @var38 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var38 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [TwoFactorEnabled] bit NOT NULL
    

    DECLARE @var39 sysname
    SELECT @var39 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'SecurityStamp')
    IF @var39 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var39 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [SecurityStamp] nvarchar(max) NULL
    

    DECLARE @var40 sysname
    SELECT @var40 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'PictureUrl')
    IF @var40 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var40 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [PictureUrl] nvarchar(max) NULL
    

    DECLARE @var41 sysname
    SELECT @var41 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'PhoneNumberConfirmed')
    IF @var41 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var41 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [PhoneNumberConfirmed] bit NOT NULL
    

    DECLARE @var42 sysname
    SELECT @var42 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'PhoneNumber')
    IF @var42 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var42 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [PhoneNumber] nvarchar(max) NULL
    

    DECLARE @var43 sysname
    SELECT @var43 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'PasswordHash')
    IF @var43 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var43 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [PasswordHash] nvarchar(max) NULL
    

    DECLARE @var44 sysname
    SELECT @var44 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'NormalizedUserName')
    IF @var44 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var44 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [NormalizedUserName] nvarchar(256) NULL
    

    DROP INDEX [EmailIndex] ON [AspNetUsers]
    DECLARE @var45 sysname
    SELECT @var45 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'NormalizedEmail')
    IF @var45 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var45 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [NormalizedEmail] nvarchar(256) NULL
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail])
    

    DECLARE @var46 sysname
    SELECT @var46 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'LockoutEnd')
    IF @var46 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var46 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [LockoutEnd] datetimeoffset NULL
    

    DECLARE @var47 sysname
    SELECT @var47 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'LockoutEnabled')
    IF @var47 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var47 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [LockoutEnabled] bit NOT NULL
    

    DECLARE @var48 sysname
    SELECT @var48 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Locale')
    IF @var48 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var48 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Locale] nvarchar(max) NULL
    

    DECLARE @var49 sysname
    SELECT @var49 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'LastName')
    IF @var49 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var49 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [LastName] nvarchar(max) NULL
    

    DECLARE @var50 sysname
    SELECT @var50 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Gender')
    IF @var50 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var50 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Gender] nvarchar(max) NULL
    

    DECLARE @var51 sysname
    SELECT @var51 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'FirstName')
    IF @var51 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var51 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [FirstName] nvarchar(max) NULL
    

    DECLARE @var52 sysname
    SELECT @var52 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'FacebookId')
    IF @var52 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var52 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [FacebookId] bigint NULL
    

    DECLARE @var53 sysname
    SELECT @var53 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'EmailConfirmed')
    IF @var53 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var53 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [EmailConfirmed] bit NOT NULL
    

    DECLARE @var54 sysname
    SELECT @var54 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Email')
    IF @var54 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var54 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Email] nvarchar(256) NULL
    

    DECLARE @var55 sysname
    SELECT @var55 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'ConcurrencyStamp')
    IF @var55 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var55 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [ConcurrencyStamp] nvarchar(max) NULL
    

    DECLARE @var56 sysname
    SELECT @var56 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'AccessFailedCount')
    IF @var56 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var56 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [AccessFailedCount] int NOT NULL
    

    DECLARE @var57 sysname
    SELECT @var57 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'Id')
    IF @var57 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var57 + ']')
    ALTER TABLE [AspNetUsers] ALTER COLUMN [Id] uniqueidentifier NOT NULL
    

    DROP INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles]
    DECLARE @var58 sysname
    SELECT @var58 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserRoles]') AND [c].[name] = N'RoleId')
    IF @var58 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [' + @var58 + ']')
    ALTER TABLE [AspNetUserRoles] ALTER COLUMN [RoleId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId])
    

    DECLARE @var59 sysname
    SELECT @var59 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserRoles]') AND [c].[name] = N'UserId')
    IF @var59 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [' + @var59 + ']')
    ALTER TABLE [AspNetUserRoles] ALTER COLUMN [UserId] uniqueidentifier NOT NULL
    

    DROP INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins]
    DECLARE @var60 sysname
    SELECT @var60 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'UserId')
    IF @var60 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var60 + ']')
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [UserId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId])
    

    DECLARE @var61 sysname
    SELECT @var61 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'ProviderDisplayName')
    IF @var61 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var61 + ']')
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [ProviderDisplayName] nvarchar(max) NULL
    

    DECLARE @var62 sysname
    SELECT @var62 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'ProviderKey')
    IF @var62 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var62 + ']')
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [ProviderKey] nvarchar(450) NOT NULL
    

    DECLARE @var63 sysname
    SELECT @var63 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserLogins]') AND [c].[name] = N'LoginProvider')
    IF @var63 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [' + @var63 + ']')
    ALTER TABLE [AspNetUserLogins] ALTER COLUMN [LoginProvider] nvarchar(450) NOT NULL
    

    DROP INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims]
    DECLARE @var64 sysname
    SELECT @var64 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserClaims]') AND [c].[name] = N'UserId')
    IF @var64 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [' + @var64 + ']')
    ALTER TABLE [AspNetUserClaims] ALTER COLUMN [UserId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId])
    

    DECLARE @var65 sysname
    SELECT @var65 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserClaims]') AND [c].[name] = N'ClaimValue')
    IF @var65 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [' + @var65 + ']')
    ALTER TABLE [AspNetUserClaims] ALTER COLUMN [ClaimValue] nvarchar(max) NULL
    

    DECLARE @var66 sysname
    SELECT @var66 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserClaims]') AND [c].[name] = N'ClaimType')
    IF @var66 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [' + @var66 + ']')
    ALTER TABLE [AspNetUserClaims] ALTER COLUMN [ClaimType] nvarchar(max) NULL
    

    DECLARE @var67 sysname
    SELECT @var67 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUserClaims]') AND [c].[name] = N'Id')
    IF @var67 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [' + @var67 + ']')
    ALTER TABLE [AspNetUserClaims] ALTER COLUMN [Id] int NOT NULL
    

    DECLARE @var68 sysname
    SELECT @var68 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoles]') AND [c].[name] = N'NormalizedName')
    IF @var68 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoles] DROP CONSTRAINT [' + @var68 + ']')
    ALTER TABLE [AspNetRoles] ALTER COLUMN [NormalizedName] nvarchar(256) NULL
    

    DECLARE @var69 sysname
    SELECT @var69 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoles]') AND [c].[name] = N'Name')
    IF @var69 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoles] DROP CONSTRAINT [' + @var69 + ']')
    ALTER TABLE [AspNetRoles] ALTER COLUMN [Name] nvarchar(256) NULL
    

    DECLARE @var70 sysname
    SELECT @var70 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoles]') AND [c].[name] = N'ConcurrencyStamp')
    IF @var70 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoles] DROP CONSTRAINT [' + @var70 + ']')
    ALTER TABLE [AspNetRoles] ALTER COLUMN [ConcurrencyStamp] nvarchar(max) NULL
    

    DECLARE @var71 sysname
    SELECT @var71 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoles]') AND [c].[name] = N'Id')
    IF @var71 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoles] DROP CONSTRAINT [' + @var71 + ']')
    ALTER TABLE [AspNetRoles] ALTER COLUMN [Id] uniqueidentifier NOT NULL
    

    DROP INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims]
    DECLARE @var72 sysname
    SELECT @var72 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoleClaims]') AND [c].[name] = N'RoleId')
    IF @var72 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [' + @var72 + ']')
    ALTER TABLE [AspNetRoleClaims] ALTER COLUMN [RoleId] uniqueidentifier NOT NULL
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId])
    

    DECLARE @var73 sysname
    SELECT @var73 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoleClaims]') AND [c].[name] = N'ClaimValue')
    IF @var73 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [' + @var73 + ']')
    ALTER TABLE [AspNetRoleClaims] ALTER COLUMN [ClaimValue] nvarchar(max) NULL
    

    DECLARE @var74 sysname
    SELECT @var74 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoleClaims]') AND [c].[name] = N'ClaimType')
    IF @var74 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [' + @var74 + ']')
    ALTER TABLE [AspNetRoleClaims] ALTER COLUMN [ClaimType] nvarchar(max) NULL
    

    DECLARE @var75 sysname
    SELECT @var75 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetRoleClaims]') AND [c].[name] = N'Id')
    IF @var75 IS NOT NULL EXEC(N'ALTER TABLE [AspNetRoleClaims] DROP CONSTRAINT [' + @var75 + ']')
    ALTER TABLE [AspNetRoleClaims] ALTER COLUMN [Id] int NOT NULL
    

    CREATE INDEX [IX_Order_RelatedOrderId] ON [Order] ([RelatedOrderId])
    

    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName])
    

    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName])
    

    ALTER TABLE [Order] ADD CONSTRAINT [FK_Order_Order_RelatedOrderId] FOREIGN KEY ([RelatedOrderId]) REFERENCES [Order] ([Id])
    

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230329141039_OrderRelationship', N'8.0.13')
    

    COMMIT
    
END


IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20230907092332_AddSetupTraderToOrder')
BEGIN

    BEGIN TRANSACTION
    

    DECLARE @var76 sysname
    SELECT @var76 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Quantity')
    IF @var76 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var76 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [Quantity] decimal(18, 8) NOT NULL
    

    DECLARE @var77 sysname
    SELECT @var77 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Price')
    IF @var77 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var77 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [Price] decimal(18, 8) NOT NULL
    

    DECLARE @var78 sysname
    SELECT @var78 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[OrderItem]') AND [c].[name] = N'Fee')
    IF @var78 IS NOT NULL EXEC(N'ALTER TABLE [OrderItem] DROP CONSTRAINT [' + @var78 + ']')
    ALTER TABLE [OrderItem] ALTER COLUMN [Fee] decimal(18, 8) NOT NULL
    

    ALTER TABLE [Order] ADD [SetupTraderId] uniqueidentifier NULL
    

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230907092332_AddSetupTraderToOrder', N'8.0.13')
    

    COMMIT
    
END



IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20231128141824_UpdateOrderForBackTest')
BEGIN

    BEGIN TRANSACTION
    

    ALTER TABLE [Order] ADD [IsBackTest] bit NOT NULL DEFAULT 0
    

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231128141824_UpdateOrderForBackTest', N'8.0.13')
    

    COMMIT
    
END

IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20240926103918_GoogleSignImplementation')
BEGIN

    BEGIN TRANSACTION
    

    ALTER TABLE [AspNetUsers] ADD [GoogleId] varchar(max) NULL
    

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240926103918_GoogleSignImplementation', N'8.0.13')
    

    COMMIT
    
END


IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20250220150020_AddingSolanaWalletAndAirdropColumnsToUser')
BEGIN

    BEGIN TRANSACTION
    

    ALTER TABLE [AspNetUsers] ADD [HasReceivedAirdrop] bit NOT NULL DEFAULT 0
    

    ALTER TABLE [AspNetUsers] ADD [SolanaWalletAddress] varchar(max) NULL
    

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250220150020_AddingSolanaWalletAndAirdropColumnsToUser', N'8.0.13')
    

    COMMIT
    

END
