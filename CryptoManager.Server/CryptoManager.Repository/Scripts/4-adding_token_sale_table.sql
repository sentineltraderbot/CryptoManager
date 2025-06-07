
IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20250605172154_AddingTokenSaleTable')
BEGIN

    BEGIN TRANSACTION;

    CREATE TABLE [TokenSale] (
        [Id] uniqueidentifier NOT NULL CONSTRAINT "PK_TokenSale" PRIMARY KEY,
        [ApplicationUserId] uniqueidentifier NULL,
        [OrderType] int NOT NULL,
        [UserEmail] nvarchar(max) NULL,
        [UserName] nvarchar(max) NULL,
        [UserWalletAddress] nvarchar(max) NULL,
        [BlockchainTx] nvarchar(max) NULL,
        [SOLQuantity] decimal(18, 8) NOT NULL,
        [SENTBOTQuantity] decimal(18, 8) NOT NULL,
        [ReferralQuantity] decimal(18, 8) NOT NULL,
        [ReferralWalletAddress] nvarchar(max) NULL,
        [ReferralUserId] uniqueidentifier NULL,
        [IsExcluded] bit NOT NULL,
        [IsEnabled] bit NOT NULL,
        [RegistryDate] datetime2 NOT NULL,
        CONSTRAINT "FK_TokenSale_AspNetUsers_ApplicationUserId" FOREIGN KEY ("ApplicationUserId") REFERENCES "AspNetUsers" ("Id"),
        CONSTRAINT "FK_TokenSale_AspNetUsers_ReferralUserId" FOREIGN KEY ("ReferralUserId") REFERENCES "AspNetUsers" ("Id")
    );

    CREATE INDEX "IX_TokenSale_ApplicationUserId" ON "TokenSale" ("ApplicationUserId");

    CREATE INDEX "IX_TokenSale_ReferralUserId" ON "TokenSale" ("ReferralUserId");

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20250605172154_AddingTokenSaleTable', '8.0.13');

    COMMIT;

END