create procedure [HostedApplicationLayerLog].[Save]
(
    @HostedApplicationLayerLog HostedApplicationLayerLog READONLY,
    @HostedApplicationLayerLogNullableColumn HostedApplicationLayerLogNullableColumn READONLY,
    @UserGuid uniqueidentifier = NULL,
    @UserName varchar(50) = NULL
)
as
begin
    -- Create
    declare @InsertedRecords ListOfLong
    insert into dbo.HostedApplicationLayerLog
    (
        [ApplicationLayerName],
        [DomainName],
        [Guid],
        [LoggedEntityBusinessGuid],
        [LoggedEntityBusinessStringRepresentation],
        [LoggedEntityCreatedByUserGuid],
        [LoggedEntityCreatedByUserName],
        [LoggedEntityDateOfCreation],
        [LoggedEntityDateOfModification],
        [LoggedEntityGuid],
        [LoggedEntityId],
        [LoggedEntityIsArchived],
        [LoggedEntityIsDeleted],
        [LoggedEntityModifiedByUserGuid],
        [LoggedEntityModifiedByUserName],
        [LoggedEntityStringRepresentation],
        [StringRepresentation],
        [Url],
        [UseCaseName],
        [BusinessGuid],
        [BusinessStringRepresentation],
        [CreatedByUserGuid],
        [CreatedByUserName],
        [DateOfCreation],
        [DateOfModification],
        [IsArchived],
        [IsDeleted],
        [ModifiedByUserGuid],
        [ModifiedByUserName]
    ) output inserted.Id, inserted.CreatedByUserName, inserted.DateOfCreation, inserted.Id into @InsertedRecords

    select 
        [ApplicationLayerName],
        [DomainName],
        [Guid],
        [LoggedEntityBusinessGuid],
        [LoggedEntityBusinessStringRepresentation],
        [LoggedEntityCreatedByUserGuid],
        [LoggedEntityCreatedByUserName],
        [LoggedEntityDateOfCreation],
        [LoggedEntityDateOfModification],
        [LoggedEntityGuid],
        [LoggedEntityId],
        [LoggedEntityIsArchived],
        [LoggedEntityIsDeleted],
        [LoggedEntityModifiedByUserGuid],
        [LoggedEntityModifiedByUserName],
        [LoggedEntityStringRepresentation],
        [StringRepresentation],
        [Url],
        [UseCaseName],
        [BusinessGuid],
        [BusinessStringRepresentation],
        case when @UserGuid is null then [CreatedByUserGuid] else @UserGuid end,
        case when @UserName is null then [CreatedByUserName] else @UserName end,
        case when @UserName is null then [DateOfCreation] else getdate() end,
        case when @UserName is null then [DateOfModification] else getdate() end,
        [IsArchived],
        [IsDeleted],
        case when @UserGuid is null then [ModifiedByUserGuid] else @UserGuid end,
        case when @UserName is null then [ModifiedByUserName] else @UserName end
    from @HostedApplicationLayerLog
    where
        IsDeleted = 0 and
        Id < 0

    -- Update
    update destination set
        destination.[ApplicationLayerName]=case when source.[ApplicationLayerName] is null then case when condition.[ApplicationLayerName]=1 then null else destination.[ApplicationLayerName] end else source.[ApplicationLayerName] end,
        destination.[DomainName]=case when source.[DomainName] is null then case when condition.[DomainName]=1 then null else destination.[DomainName] end else source.[DomainName] end,
        destination.[Guid]=case when source.[Guid] is null then case when condition.[Guid]=1 then null else destination.[Guid] end else source.[Guid] end,
        destination.[LoggedEntityBusinessGuid]=case when source.[LoggedEntityBusinessGuid] is null then case when condition.[LoggedEntityBusinessGuid]=1 then null else destination.[LoggedEntityBusinessGuid] end else source.[LoggedEntityBusinessGuid] end,
        destination.[LoggedEntityBusinessStringRepresentation]=case when source.[LoggedEntityBusinessStringRepresentation] is null then case when condition.[LoggedEntityBusinessStringRepresentation]=1 then null else destination.[LoggedEntityBusinessStringRepresentation] end else source.[LoggedEntityBusinessStringRepresentation] end,
        destination.[LoggedEntityCreatedByUserGuid]=case when source.[LoggedEntityCreatedByUserGuid] is null then case when condition.[LoggedEntityCreatedByUserGuid]=1 then null else destination.[LoggedEntityCreatedByUserGuid] end else source.[LoggedEntityCreatedByUserGuid] end,
        destination.[LoggedEntityCreatedByUserName]=case when source.[LoggedEntityCreatedByUserName] is null then case when condition.[LoggedEntityCreatedByUserName]=1 then null else destination.[LoggedEntityCreatedByUserName] end else source.[LoggedEntityCreatedByUserName] end,
        destination.[LoggedEntityDateOfCreation]=case when source.[LoggedEntityDateOfCreation] is null then case when condition.[LoggedEntityDateOfCreation]=1 then null else destination.[LoggedEntityDateOfCreation] end else source.[LoggedEntityDateOfCreation] end,
        destination.[LoggedEntityDateOfModification]=case when source.[LoggedEntityDateOfModification] is null then case when condition.[LoggedEntityDateOfModification]=1 then null else destination.[LoggedEntityDateOfModification] end else source.[LoggedEntityDateOfModification] end,
        destination.[LoggedEntityGuid]=case when source.[LoggedEntityGuid] is null then case when condition.[LoggedEntityGuid]=1 then null else destination.[LoggedEntityGuid] end else source.[LoggedEntityGuid] end,
        destination.[LoggedEntityId]=case when source.[LoggedEntityId] is null then case when condition.[LoggedEntityId]=1 then null else destination.[LoggedEntityId] end else source.[LoggedEntityId] end,
        destination.[LoggedEntityIsArchived]=case when source.[LoggedEntityIsArchived] is null then case when condition.[LoggedEntityIsArchived]=1 then null else destination.[LoggedEntityIsArchived] end else source.[LoggedEntityIsArchived] end,
        destination.[LoggedEntityIsDeleted]=case when source.[LoggedEntityIsDeleted] is null then case when condition.[LoggedEntityIsDeleted]=1 then null else destination.[LoggedEntityIsDeleted] end else source.[LoggedEntityIsDeleted] end,
        destination.[LoggedEntityModifiedByUserGuid]=case when source.[LoggedEntityModifiedByUserGuid] is null then case when condition.[LoggedEntityModifiedByUserGuid]=1 then null else destination.[LoggedEntityModifiedByUserGuid] end else source.[LoggedEntityModifiedByUserGuid] end,
        destination.[LoggedEntityModifiedByUserName]=case when source.[LoggedEntityModifiedByUserName] is null then case when condition.[LoggedEntityModifiedByUserName]=1 then null else destination.[LoggedEntityModifiedByUserName] end else source.[LoggedEntityModifiedByUserName] end,
        destination.[LoggedEntityStringRepresentation]=case when source.[LoggedEntityStringRepresentation] is null then case when condition.[LoggedEntityStringRepresentation]=1 then null else destination.[LoggedEntityStringRepresentation] end else source.[LoggedEntityStringRepresentation] end,
        destination.[StringRepresentation]=case when source.[StringRepresentation] is null then case when condition.[StringRepresentation]=1 then null else destination.[StringRepresentation] end else source.[StringRepresentation] end,
        destination.[Url]=case when source.[Url] is null then case when condition.[Url]=1 then null else destination.[Url] end else source.[Url] end,
        destination.[UseCaseName]=case when source.[UseCaseName] is null then case when condition.[UseCaseName]=1 then null else destination.[UseCaseName] end else source.[UseCaseName] end,
        destination.BusinessGuid = source.BusinessGuid,
        destination.BusinessStringRepresentation = source.BusinessStringRepresentation,
        destination.CreatedByUserGuid = source.CreatedByUserGuid,
        destination.DateOfModification = case when @UserName is null then source.[DateOfModification] else getdate() end,
        destination.IsArchived = source.IsArchived,
        destination.IsDeleted = source.IsDeleted,
        destination.ModifiedByUserGuid = case when @UserGuid is null then source.[ModifiedByUserGuid] else @UserGuid end,
        destination.ModifiedByUserName  = case when @UserName is null then source.[ModifiedByUserName] else @UserName end
    from HostedApplicationLayerLog destination, @HostedApplicationLayerLog source, @HostedApplicationLayerLogNullableColumn condition
    where
        source.IsDeleted = 0 and
        source.Id > 0 and
        destination.Id = source.Id

    -- Delete
    update destination set
        destination.DateOfModification = case when @UserName is null then source.[DateOfModification] else getdate() end,
        destination.IsDeleted = 1,
        destination.ModifiedByUserGuid = case when @UserGuid is null then source.[ModifiedByUserGuid] else @UserGuid end,
        destination.ModifiedByUserName  = case when @UserName is null then source.[ModifiedByUserName] else @UserName end
    from HostedApplicationLayerLog destination, @HostedApplicationLayerLog source
    where
        source.IsDeleted = 1 and
        source.Id > 0 and
        destination.Id = source.Id

    -- Return identifiers of created records
    select * from @InsertedRecords
end

GO

