create type [dbo].[HostedApplicationLayerLogNullableColumn] AS TABLE 
(
    [LoggedEntityBusinessGuid]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityBusinessStringRepresentation]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityCreatedByUserGuid]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityCreatedByUserName]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityDateOfCreation]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityDateOfModification]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityGuid]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityId]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityIsArchived]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityIsDeleted]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityModifiedByUserGuid]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityModifiedByUserName]  BIT DEFAULT ((0)) NULL,
    [LoggedEntityStringRepresentation]  BIT DEFAULT ((0)) NULL
)


GO

