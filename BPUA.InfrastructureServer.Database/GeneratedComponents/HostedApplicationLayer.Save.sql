create procedure [HostedApplicationLayer].[Save]
(
    @HostedApplicationLayer HostedApplicationLayer READONLY,
    @HostedApplicationLayerNullableColumn HostedApplicationLayerNullableColumn READONLY,
    @UserGuid uniqueidentifier = NULL,
    @UserName varchar(50) = NULL
)
as
begin
    -- Create
    declare @InsertedRecords [dbo].[ListOfLong]
    merge into dbo.HostedApplicationLayer as destination
    using
    (
        select
            [Id],
            [ApplicationLayerName],
            [DomainName],
            [Url],
            [UseCaseName],
            [BusinessGuid],
            [BusinessStringRepresentation],
            [__ClientKey]
        from @HostedApplicationLayer
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
        [BusinessGuid],
        [BusinessStringRepresentation]    )
    values
    (
        source.[ApplicationLayerName],
        source.[DomainName],
        source.[Url],
        source.[UseCaseName],
        source.[BusinessGuid],
        source.[BusinessStringRepresentation]
    )
    output inserted.Id, inserted.CreatedByUserName, inserted.DateOfCreation, source.Id, convert(varchar(50), source.__ClientKey) into @InsertedRecords;

    -- Update
    update destination set
        destination.[ApplicationLayerName]=case when source.[ApplicationLayerName] is null then destination.[ApplicationLayerName] else source.[ApplicationLayerName] end,
        destination.[DomainName]=case when source.[DomainName] is null then destination.[DomainName] else source.[DomainName] end,
        destination.[Url]=case when source.[Url] is null then destination.[Url] else source.[Url] end,
        destination.[UseCaseName]=case when source.[UseCaseName] is null then destination.[UseCaseName] else source.[UseCaseName] end,
        destination.BusinessGuid = source.BusinessGuid,
        destination.BusinessStringRepresentation = source.BusinessStringRepresentation,
        destination.DateOfModification = case when @UserName is null then source.[DateOfModification] else SYSUTCDATETIME() end,
        destination.IsArchived = source.IsArchived,
        destination.IsDeleted = source.IsDeleted,
        destination.ModifiedByUserGuid = case when @UserGuid is null then source.[ModifiedByUserGuid] else @UserGuid end,
        destination.ModifiedByUserName  = case when @UserName is null then source.[ModifiedByUserName] else @UserName end
    from [dbo].[HostedApplicationLayer] destination, @HostedApplicationLayer source
    where
        source.__ChangeState = 2 and
        destination.Id = source.Id

    -- Delete
    update destination set
        destination.DateOfModification = case when @UserName is null then source.[DateOfModification] else SYSUTCDATETIME() end,
        destination.IsDeleted = 1,
        destination.ModifiedByUserGuid = case when @UserGuid is null then source.[ModifiedByUserGuid] else @UserGuid end,
        destination.ModifiedByUserName  = case when @UserName is null then source.[ModifiedByUserName] else @UserName end
    from [dbo].[HostedApplicationLayer] destination, @HostedApplicationLayer source
    where
        source.__ChangeState = 3 and
        destination.Id = source.Id

    -- Return identifiers of created records
    select * from @InsertedRecords
end

GO

