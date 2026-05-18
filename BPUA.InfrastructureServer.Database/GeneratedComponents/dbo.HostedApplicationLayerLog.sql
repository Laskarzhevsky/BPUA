create type [dbo].[HostedApplicationLayerLog] AS TABLE 
(
    [Id] [bigint] NULL,
    [ApplicationLayerName] [varchar] (32) NULL,
    [DomainName] [varchar] (128) NULL,
    [Guid] [uniqueidentifier] NULL,
    [LoggedEntityBusinessGuid] [uniqueidentifier] NULL,
    [LoggedEntityBusinessStringRepresentation] [nvarchar] (1024) NULL,
    [LoggedEntityCreatedByUserGuid] [uniqueidentifier] NULL,
    [LoggedEntityCreatedByUserName] [varchar] (50) NULL,
    [LoggedEntityDateOfCreation] [datetime2] NULL,
    [LoggedEntityDateOfModification] [datetime2] NULL,
    [LoggedEntityGuid] [uniqueidentifier] NULL,
    [LoggedEntityId] [bigint] NULL,
    [LoggedEntityIsArchived] [bit] NULL,
    [LoggedEntityIsDeleted] [bit] NULL,
    [LoggedEntityModifiedByUserGuid] [uniqueidentifier] NULL,
    [LoggedEntityModifiedByUserName] [varchar] (50) NULL,
    [LoggedEntityStringRepresentation] [varchar] (290) NULL,
    [Url] [varchar] (1024) NULL,
    [UseCaseName] [varchar] (128) NULL,
    [BusinessGuid] [uniqueidentifier] NULL,
    [BusinessStringRepresentation] [nvarchar] (1024) NULL,
    [CreatedByUserGuid] [uniqueidentifier] NULL,
    [CreatedByUserName] [varchar] (50) NULL,
    [DateOfCreation] [datetime2] NULL,
    [DateOfModification] [datetime2] NULL,
    [IsArchived] [bit] NULL,
    [IsDeleted] [bit] NULL,
    [ModifiedByUserGuid] [uniqueidentifier] NULL,
    [ModifiedByUserName] [varchar] (50) NULL
)


GO

