IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Blogs] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Blogs] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240121150528_InitialCreate', N'6.0.14');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Blogs] ADD [Discriminator] nvarchar(max) NOT NULL DEFAULT N'';
GO

ALTER TABLE [Blogs] ADD [GithubUrl] nvarchar(max) NULL;
GO

ALTER TABLE [Blogs] ADD [LinkedInUrl] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240306015748_AddHierarchyBlogs', N'6.0.14');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [BlogAttributes] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Type] int NOT NULL,
    CONSTRAINT [PK_BlogAttributes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [BlogAttributeValues] (
    [Id] bigint NOT NULL IDENTITY,
    [BlogId] bigint NOT NULL,
    [BlogAttributeId] bigint NOT NULL,
    [Value] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_BlogAttributeValues] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BlogAttributeValues_BlogAttributes_BlogAttributeId] FOREIGN KEY ([BlogAttributeId]) REFERENCES [BlogAttributes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BlogAttributeValues_Blogs_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Blogs] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_BlogAttributeValues_BlogAttributeId] ON [BlogAttributeValues] ([BlogAttributeId]);
GO

CREATE INDEX [IX_BlogAttributeValues_BlogId] ON [BlogAttributeValues] ([BlogId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240521170321_EAV', N'6.0.14');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [FK_BlogAttributeValues_BlogAttributes_BlogAttributeId];
GO

DROP INDEX [IX_BlogAttributeValues_BlogAttributeId] ON [BlogAttributeValues];
GO

ALTER TABLE [BlogAttributes] DROP CONSTRAINT [PK_BlogAttributes];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BlogAttributeValues]') AND [c].[name] = N'BlogAttributeId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [BlogAttributeValues] DROP COLUMN [BlogAttributeId];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BlogAttributes]') AND [c].[name] = N'Id');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [BlogAttributes] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [BlogAttributes] DROP COLUMN [Id];
GO

ALTER TABLE [BlogAttributeValues] ADD [Name] nvarchar(450) NOT NULL DEFAULT N'';
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BlogAttributes]') AND [c].[name] = N'Name');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [BlogAttributes] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [BlogAttributes] ALTER COLUMN [Name] nvarchar(450) NOT NULL;
GO

ALTER TABLE [BlogAttributes] ADD CONSTRAINT [PK_BlogAttributes] PRIMARY KEY ([Name]);
GO

CREATE INDEX [IX_BlogAttributeValues_Name] ON [BlogAttributeValues] ([Name]);
GO

ALTER TABLE [BlogAttributeValues] ADD CONSTRAINT [FK_BlogAttributeValues_BlogAttributes_Name] FOREIGN KEY ([Name]) REFERENCES [BlogAttributes] ([Name]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240521173210_ChangeKey', N'6.0.14');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [FK_BlogAttributeValues_BlogAttributes_Name];
GO

DROP INDEX [IX_BlogAttributeValues_Name] ON [BlogAttributeValues];
GO

ALTER TABLE [BlogAttributes] DROP CONSTRAINT [PK_BlogAttributes];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Blogs]') AND [c].[name] = N'GithubUrl');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Blogs] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Blogs] DROP COLUMN [GithubUrl];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Blogs]') AND [c].[name] = N'LinkedInUrl');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Blogs] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Blogs] DROP COLUMN [LinkedInUrl];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BlogAttributeValues]') AND [c].[name] = N'Name');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [BlogAttributeValues] DROP COLUMN [Name];
GO

ALTER TABLE [BlogAttributeValues] ADD [AttributeId] bigint NOT NULL DEFAULT CAST(0 AS bigint);
GO

ALTER TABLE [BlogAttributes] ADD [Id] bigint NOT NULL IDENTITY;
GO

ALTER TABLE [BlogAttributes] ADD CONSTRAINT [AK_BlogAttributes_Name] UNIQUE ([Name]);
GO

ALTER TABLE [BlogAttributes] ADD CONSTRAINT [PK_BlogAttributes] PRIMARY KEY ([Id]);
GO

CREATE INDEX [IX_BlogAttributeValues_AttributeId] ON [BlogAttributeValues] ([AttributeId]);
GO

ALTER TABLE [BlogAttributeValues] ADD CONSTRAINT [FK_BlogAttributeValues_BlogAttributes_AttributeId] FOREIGN KEY ([AttributeId]) REFERENCES [BlogAttributes] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240522031430_EAV_V2', N'6.0.14');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Blogs]') AND [c].[name] = N'Discriminator');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Blogs] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Blogs] DROP COLUMN [Discriminator];
GO

ALTER TABLE [BlogAttributes] ADD [TypeId] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240522051844_AddTypeId', N'6.0.14');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [FK_BlogAttributeValues_BlogAttributes_AttributeId];
GO

ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [FK_BlogAttributeValues_Blogs_BlogId];
GO

DROP TABLE [BlogAttributes];
GO

ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [PK_BlogAttributeValues];
GO

DROP INDEX [IX_BlogAttributeValues_AttributeId] ON [BlogAttributeValues];
GO

DROP INDEX [IX_BlogAttributeValues_BlogId] ON [BlogAttributeValues];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BlogAttributeValues]') AND [c].[name] = N'AttributeId');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [BlogAttributeValues] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [BlogAttributeValues] DROP COLUMN [AttributeId];
GO

EXEC sp_rename N'[BlogAttributeValues]', N'BlogConfigs';
GO

ALTER TABLE [BlogConfigs] ADD [Key] nvarchar(450) NOT NULL DEFAULT N'';
GO

ALTER TABLE [BlogConfigs] ADD [ValueTypeId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [BlogConfigs] ADD CONSTRAINT [PK_BlogConfigs] PRIMARY KEY ([Id]);
GO

CREATE TABLE [ValueTypes] (
    [TypeId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ValueTypes] PRIMARY KEY ([TypeId])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TypeId', N'Name') AND [object_id] = OBJECT_ID(N'[ValueTypes]'))
    SET IDENTITY_INSERT [ValueTypes] ON;
INSERT INTO [ValueTypes] ([TypeId], [Name])
VALUES (5, N'True/False'),
(4, N'DateTime'),
(3, N'Real Number'),
(2, N'Number'),
(1, N'String'),
(6, N'User');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'TypeId', N'Name') AND [object_id] = OBJECT_ID(N'[ValueTypes]'))
    SET IDENTITY_INSERT [ValueTypes] OFF;
GO

CREATE UNIQUE INDEX [IX_BlogConfigs_BlogId_Key] ON [BlogConfigs] ([BlogId], [Key]);
GO

CREATE INDEX [IX_BlogConfigs_ValueTypeId] ON [BlogConfigs] ([ValueTypeId]);
GO

ALTER TABLE [BlogConfigs] ADD CONSTRAINT [FK_BlogConfigs_Blogs_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Blogs] ([Id]) ON DELETE CASCADE;
GO

ALTER TABLE [BlogConfigs] ADD CONSTRAINT [FK_BlogConfigs_ValueTypes_ValueTypeId] FOREIGN KEY ([ValueTypeId]) REFERENCES [ValueTypes] ([TypeId]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240522091636_RemoveAttributeAndAddValueType', N'6.0.14');
GO

COMMIT;
GO

