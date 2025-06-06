
IF NOT EXISTS (SELECT * FROM __EFMigrationsHistory WHERE MigrationId = '20250530155307_AddingReferralToUserTable')
BEGIN

    BEGIN TRANSACTION;

    ALTER TABLE [AspNetUsers] ADD [ReferredById] varchar(255) NULL;

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20250530155307_AddingReferralToUserTable', '8.0.13');

    COMMIT;
    
END