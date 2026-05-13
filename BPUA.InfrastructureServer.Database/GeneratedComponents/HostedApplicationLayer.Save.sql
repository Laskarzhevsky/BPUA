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
        [Guid],
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
    from @HostedApplicationLayer
    where
        IsDeleted = 0 and
        Id < 0

    -- Update
    update destination set
        destination.[ApplicationLayerName]=case when source.[ApplicationLayerName] is null then case when condition.[ApplicationLayerName]=1 then null else destination.[ApplicationLayerName] end else source.[ApplicationLayerName] end,
        destination.[DomainName]=case when source.[DomainName] is null then case when condition.[DomainName]=1 then null else destination.[DomainName] end else source.[DomainName] end,
        destination.[Guid]=case when source.[Guid] is null then case when condition.[Guid]=1 then null else destination.[Guid] end else source.[Guid] end,
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
    from HostedApplicationLayer destination, @HostedApplicationLayer source, @HostedApplicationLayerNullableColumn condition
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
    from HostedApplicationLayer destination, @HostedApplicationLayer source
    where
        source.IsDeleted = 1 and
        source.Id > 0 and
        destination.Id = source.Id

    -- Return identifiers of created records
    select * from @InsertedRecords
end

GO

