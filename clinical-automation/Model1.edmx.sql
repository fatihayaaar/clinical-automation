
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/01/2022 19:33:29
-- Generated from EDMX file: C:\clinical-automation\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DbPoliklinik];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TBLHASTA_TBLDOKTOR]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TBLHASTA] DROP CONSTRAINT [FK_TBLHASTA_TBLDOKTOR];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[TBLDOKTOR]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBLDOKTOR];
GO
IF OBJECT_ID(N'[dbo].[TBLHASTA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TBLHASTA];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'TBLDOKTOR'
CREATE TABLE [dbo].[TBLDOKTOR] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [KimlikNo] varchar(50)  NULL,
    [AdiSoyadi] varchar(50)  NULL,
    [TelefonNo] varchar(50)  NULL,
    [Cinsiyeti] varchar(50)  NULL,
    [MailAdresi] varchar(50)  NULL,
    [DogumTarihi] datetime  NULL
);
GO

-- Creating table 'TBLHASTA'
CREATE TABLE [dbo].[TBLHASTA] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdiSoyadi] varchar(50)  NULL,
    [KimlikNo] varchar(50)  NULL,
    [TelefonNo] varchar(50)  NULL,
    [DogumTarihi] datetime  NULL,
    [Cinsiyeti] varchar(50)  NULL,
    [RandevuTarihi] datetime  NULL,
    [KayitTipi] varchar(50)  NULL,
    [DoktorId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'TBLDOKTOR'
ALTER TABLE [dbo].[TBLDOKTOR]
ADD CONSTRAINT [PK_TBLDOKTOR]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TBLHASTA'
ALTER TABLE [dbo].[TBLHASTA]
ADD CONSTRAINT [PK_TBLHASTA]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DoktorId] in table 'TBLHASTA'
ALTER TABLE [dbo].[TBLHASTA]
ADD CONSTRAINT [FK_TBLHASTA_TBLDOKTOR]
    FOREIGN KEY ([DoktorId])
    REFERENCES [dbo].[TBLDOKTOR]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TBLHASTA_TBLDOKTOR'
CREATE INDEX [IX_FK_TBLHASTA_TBLDOKTOR]
ON [dbo].[TBLHASTA]
    ([DoktorId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------