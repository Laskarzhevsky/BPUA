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
    declare @InsertedRecords ListOfLong
    insert into dbo.HostedApplicationLayer
    (
        [ApplicationLayerName],
        [DomainName],
        [Url],
        [UseCaseName],
        [BusinessGuid],
        [BusinessStringRepresentation]
    ) output inserted.Id, inserted.CreatedByUserName, inserted.DateOfCreation, inserted.Id into @InsertedRecords

    select 
        [ApplicationLayerName],
        [DomainName],
        [Url],
        [UseCaseName],
        [BusinessGuid],
        [BusinessStringRepresentation]
    from @HostedApplicationLayer
    where
        IsDeleted = 0 and
        Id < 0

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
        source.IsDeleted = 0 and
        source.Id > 0 and
        destination.Id = source.Id

    -- Delete
    update destination set
        destination.DateOfModification = case when @UserName is null then source.[DateOfModification] else SYSUTCDATETIME() end,
        destination.IsDeleted = 1,
        destination.ModifiedByUserGuid = case when @UserGuid is null then source.[ModifiedByUserGuid] else @UserGuid end,
        destination.ModifiedByUserName  = case when @UserName is null then source.[ModifiedByUserName] else @UserName end
    from [dbo].[HostedApplicationLayer] destination, @HostedApplicationLayer source
    where
        source.IsDeleted = 1 and
        source.Id > 0 and
        destination.Id = source.Id

    -- Return identifiers of created records
    select * from @InsertedRecords
end

GO

