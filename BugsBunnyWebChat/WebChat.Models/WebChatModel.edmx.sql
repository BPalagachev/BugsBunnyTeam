
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/14/2013 14:41:58
-- Generated from EDMX file: C:\Users\Boyan\Documents\GitHub\BugsBunnyTeam\BugsBunnyWebChat\WebChat.Models\WebChatModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WebChatDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Channels_Users_Channel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Channels_Users] DROP CONSTRAINT [FK_Channels_Users_Channel];
GO
IF OBJECT_ID(N'[dbo].[FK_Channels_Users_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Channels_Users] DROP CONSTRAINT [FK_Channels_Users_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Messages_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Messages_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Sessions_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sessions] DROP CONSTRAINT [FK_Sessions_Users];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Channel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Channel];
GO
IF OBJECT_ID(N'[dbo].[Channels_Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Channels_Users];
GO
IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[Sessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sessions];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Channels'
CREATE TABLE [dbo].[Channels] (
    [ChannelId] int  NOT NULL,
    [channel1] nchar(10)  NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [MessageId] int  NOT NULL,
    [Message1] nvarchar(max)  NOT NULL,
    [SendTime] datetime  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Sessions'
CREATE TABLE [dbo].[Sessions] (
    [SessionId] int  NOT NULL,
    [IP] varchar(50)  NULL,
    [UserID] int  NOT NULL,
    [SessionUID] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [SessionId] int  NOT NULL,
    [CheckInTime] datetime  NOT NULL,
    [CheckOutTime] datetime  NOT NULL,
    [ChannelId] nchar(10)  NOT NULL
);
GO

-- Creating table 'Channels_Users'
CREATE TABLE [dbo].[Channels_Users] (
    [Channels_ChannelId] int  NOT NULL,
    [Users_UserId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ChannelId] in table 'Channels'
ALTER TABLE [dbo].[Channels]
ADD CONSTRAINT [PK_Channels]
    PRIMARY KEY CLUSTERED ([ChannelId] ASC);
GO

-- Creating primary key on [MessageId] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([MessageId] ASC);
GO

-- Creating primary key on [SessionId] in table 'Sessions'
ALTER TABLE [dbo].[Sessions]
ADD CONSTRAINT [PK_Sessions]
    PRIMARY KEY CLUSTERED ([SessionId] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [UserId] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserId] ASC);
GO

-- Creating primary key on [Channels_ChannelId], [Users_UserId] in table 'Channels_Users'
ALTER TABLE [dbo].[Channels_Users]
ADD CONSTRAINT [PK_Channels_Users]
    PRIMARY KEY NONCLUSTERED ([Channels_ChannelId], [Users_UserId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_Messages_Users]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Messages_Users'
CREATE INDEX [IX_FK_Messages_Users]
ON [dbo].[Messages]
    ([UserId]);
GO

-- Creating foreign key on [UserID] in table 'Sessions'
ALTER TABLE [dbo].[Sessions]
ADD CONSTRAINT [FK_Sessions_Users]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Sessions_Users'
CREATE INDEX [IX_FK_Sessions_Users]
ON [dbo].[Sessions]
    ([UserID]);
GO

-- Creating foreign key on [Channels_ChannelId] in table 'Channels_Users'
ALTER TABLE [dbo].[Channels_Users]
ADD CONSTRAINT [FK_Channels_Users_Channel]
    FOREIGN KEY ([Channels_ChannelId])
    REFERENCES [dbo].[Channels]
        ([ChannelId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Users_UserId] in table 'Channels_Users'
ALTER TABLE [dbo].[Channels_Users]
ADD CONSTRAINT [FK_Channels_Users_Users]
    FOREIGN KEY ([Users_UserId])
    REFERENCES [dbo].[Users]
        ([UserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Channels_Users_Users'
CREATE INDEX [IX_FK_Channels_Users_Users]
ON [dbo].[Channels_Users]
    ([Users_UserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------