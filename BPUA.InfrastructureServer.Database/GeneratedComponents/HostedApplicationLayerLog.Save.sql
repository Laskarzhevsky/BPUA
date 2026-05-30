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
    declare @InsertedRecords [dbo].[ListOfLong]
    merge into dbo.HostedApplicationLayerLog as destination
    using
    (
        select
            [Id],
            [ApplicationLayerName],
            [DomainName],
            [Url],
            [UseCaseName],
            [LoggedEntityId],
            [LoggedEntityBusinessGuid],
            [LoggedEntityBusinessStringRepresentation],
            [LoggedEntityCreatedByUserGuid],
            [LoggedEntityCreatedByUserName],
            [LoggedEntityDateOfCreation],
            [LoggedEntityDateOfModification],
            [LoggedEntityGuid],
            [LoggedEntityIsArchived],
            [LoggedEntityIsDeleted],
            [LoggedEntityModifiedByUserGuid],
            [LoggedEntityModifiedByUserName],
            [LoggedEntityStringRepresentation],
            [BusinessGuid],
            [BusinessStringRepresentation],
            [__ClientKey]
        from @HostedApplicationLayerLog
        where __ChangeState = 1
    ) as source
    on 1 = 0
    when not matched then
    insert
    (
        [ApplicationLayerName],
        [DomainName],
        [Url],
        [UseCaseName],
        [LoggedEntityId],
        [LoggedEntityBusinessGuid],
        [LoggedEntityBusinessStringRepresentation],
        [LoggedEntityCreatedByUserGuid],
        [LoggedEntityCreatedByUserName],
        [LoggedEntityDateOfCreation],
        [LoggedEntityDateOfModification],
        [LoggedEntityGuid],
        [LoggedEntityIsArchived],
        [LoggedEntityIsDeleted],
        [LoggedEntityModifiedByUserGuid],
        [LoggedEntityModifiedByUserName],
        [LoggedEntityStringRepresentation],
        [BusinessGuid],
        [BusinessStringRepresentation]    )
    values
    (
        source.[ApplicationLayerName],
        source.[DomainName],
        source.[Url],
        source.[UseCaseName],
        source.[LoggedEntityId],
        source.[LoggedEntityBusinessGuid],
        source.[LoggedEntityBusinessStringRepresentation],
        source.[LoggedEntityCreatedByUserGuid],
        source.[LoggedEntityCreatedByUserName],
        source.[LoggedEntityDateOfCreation],
        source.[LoggedEntityDateOfModification],
        source.[LoggedEntityGuid],
        source.[LoggedEntityIsArchived],
        source.[LoggedEntityIsDeleted],
        source.[LoggedEntityModifiedByUserGuid],
        source.[LoggedEntityModifiedByUserName],
        source.[LoggedEntityStringRepresentation],
        source.[BusinessGuid],
        source.[BusinessStringRepresentation]
    )
    output inserted.Id, inserted.CreatedByUserName, inserted.DateOfCreation, source.Id, convert(varchar(50), source.__ClientKey) into @InsertedRecords;

    -- Update
    update destination set
        destination.[ApplicationLayerName]=case when source.[ApplicationLayerName] is null then destination.[ApplicationLayerName] else source.[ApplicationLayerName] end,
        destination.[DomainName]=case when source.[DomainName] is null then destination.[DomainName] else source.[DomainName] end,
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
        destination.[Url]=case when source.[Url] is null then destination.[Url] else source.[Url] end,
        destination.[UseCaseName]=case when source.[UseCaseName] is null then destination.[UseCaseName] else source.[UseCaseName] end,
        destination.BusinessGuid = source.BusinessGuid,
        destination.BusinessStringRepresentation = source.BusinessStringRepresentation,
        destination.DateOfModification = case when @UserName is null then source.[DateOfModification] else SYSUTCDATETIME() end,
        destination.IsArchived = source.IsArchived,
        destination.IsDeleted = source.IsDeleted,
        destination.ModifiedByUserGuid = case when @UserGuid is null then source.[ModifiedByUserGuid] else @UserGuid end,
        destination.ModifiedByUserName  = case when @UserName is null then source.[ModifiedByUserName] else @UserName end
    from [dbo].[HostedApplicationLayerLog] destination, @HostedApplicationLayerLog source, @HostedApplicationLayerLogNullableColumn condition
    where
        source.__ChangeState = 2 and
        destination.Id = source.Id

    -- Delete
    update destination set
        destination.DateOfModification = case when @UserName is null then source.[DateOfModification] else SYSUTCDATETIME() end,
        destination.IsDeleted = 1,
        destination.ModifiedByUserGuid = case when @UserGuid is null then source.[ModifiedByUserGuid] else @UserGuid end,
        destination.ModifiedByUserName  = case when @UserName is null then source.[ModifiedByUserName] else @UserName end
    from [dbo].[HostedApplicationLayerLog] destination, @HostedApplicationLayerLog source
    where
        source.__ChangeState = 3 and
        destination.Id = source.Id

    -- Return identifiers of created records
    select * from @InsertedRecords
end

GO

