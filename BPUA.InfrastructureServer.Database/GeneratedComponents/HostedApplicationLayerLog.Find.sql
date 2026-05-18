create procedure [HostedApplicationLayerLog].[Find]
    @TopExpression varchar(20) = null,
    @ListOfNullableColumns varchar(max) = null,
    @OrderByClause varchar(max) = null,
    @IdCollection ListOfId READONLY,
    @ApplicationLayerName varchar(32) = null,
    @DomainName varchar(128) = null,
    @Guid uniqueidentifier = null,
    @LoggedEntityBusinessGuid uniqueidentifier = null,
    @LoggedEntityBusinessStringRepresentation nvarchar(1024) = null,
    @LoggedEntityCreatedByUserGuid uniqueidentifier = null,
    @LoggedEntityCreatedByUserName varchar(50) = null,
    @LoggedEntityDateOfCreationFrom datetime2 = null,
    @LoggedEntityDateOfCreationTo datetime2 = null,
    @LoggedEntityDateOfModificationFrom datetime2 = null,
    @LoggedEntityDateOfModificationTo datetime2 = null,
    @LoggedEntityGuid uniqueidentifier = null,
    @LoggedEntityIdCollection ListOfId READONLY,
    @LoggedEntityIsArchived bit = null,
    @LoggedEntityIsDeleted bit = null,
    @LoggedEntityModifiedByUserGuid uniqueidentifier = null,
    @LoggedEntityModifiedByUserName varchar(50) = null,
    @LoggedEntityStringRepresentation varchar(290) = null,
    @StringRepresentation varchar(290) = null,
    @Url varchar(1024) = null,
    @UseCaseName varchar(128) = null,
    @BusinessGuid uniqueidentifier = null,
    @BusinessStringRepresentation nvarchar(1024) = null,
    @CreatedByUserGuid uniqueidentifier = null,
    @CreatedByUserName varchar(50) = null,
    @DateOfCreationFrom datetime2 = null,
    @DateOfCreationTo datetime2 = null,
    @DateOfModificationFrom datetime2 = null,
    @DateOfModificationTo datetime2 = null,
    @IsArchived bit = null,
    @IsDeleted bit = null,
    @ModifiedByUserGuid uniqueidentifier = null,
    @ModifiedByUserName varchar(50) = null
as
begin
    declare @sql nvarchar(max) = 'select '

    if (@TopExpression is not null and try_convert(int, @TopExpression) is null)
        raiserror('Invalid TopExpression. Integer value is expected.', 16, 1)

    if (@TopExpression is not null)
        set @sql = @sql + 'top (' + convert(varchar(20), try_convert(int, @TopExpression)) + ') '

    set @sql = @sql + ' 
        *
    from
        [dbo].[HostedApplicationLayerLog]
    where '

        IF EXISTS (Select 1 from @IdCollection)
            set @sql = @sql + '[Id] in (SELECT Id FROM @xIdCollection) and '

        if (@ApplicationLayerName is not null)
            set @sql = @sql + '[ApplicationLayerName] = @xApplicationLayerName and '

        if (@DomainName is not null)
            set @sql = @sql + '[DomainName] = @xDomainName and '

        if (@Guid is not null)
            set @sql = @sql + '[Guid] = @xGuid and '

        if (@LoggedEntityBusinessGuid is not null)
            set @sql = @sql + '[LoggedEntityBusinessGuid] = @xLoggedEntityBusinessGuid and '
        else if (CHARINDEX(',LoggedEntityBusinessGuid,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityBusinessGuid] is null and '

        if (@LoggedEntityBusinessStringRepresentation is not null)
            set @sql = @sql + '[LoggedEntityBusinessStringRepresentation] = @xLoggedEntityBusinessStringRepresentation and '
        else if (CHARINDEX(',LoggedEntityBusinessStringRepresentation,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityBusinessStringRepresentation] is null and '

        if (@LoggedEntityCreatedByUserGuid is not null)
            set @sql = @sql + '[LoggedEntityCreatedByUserGuid] = @xLoggedEntityCreatedByUserGuid and '
        else if (CHARINDEX(',LoggedEntityCreatedByUserGuid,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityCreatedByUserGuid] is null and '

        if (@LoggedEntityCreatedByUserName is not null)
            set @sql = @sql + '[LoggedEntityCreatedByUserName] = @xLoggedEntityCreatedByUserName and '
        else if (CHARINDEX(',LoggedEntityCreatedByUserName,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityCreatedByUserName] is null and '

        if (CHARINDEX(',LoggedEntityDateOfCreation,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityDateOfCreation] is null and '
        else if (@LoggedEntityDateOfCreationFrom is not null and @LoggedEntityDateOfCreationTo is not null)
            set @sql = @sql + '[LoggedEntityDateOfCreation] >= @xLoggedEntityDateOfCreationFrom and [LoggedEntityDateOfCreation] < @xLoggedEntityDateOfCreationTo and '
        else if (@LoggedEntityDateOfCreationFrom is not null and @LoggedEntityDateOfCreationTo is null)
            set @sql = @sql + '[LoggedEntityDateOfCreation] >= @xLoggedEntityDateOfCreationFrom and '
        else if (@LoggedEntityDateOfCreationFrom is null and @LoggedEntityDateOfCreationTo is not null)
            set @sql = @sql + '[LoggedEntityDateOfCreation] < @xLoggedEntityDateOfCreationTo and '

        if (CHARINDEX(',LoggedEntityDateOfModification,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityDateOfModification] is null and '
        else if (@LoggedEntityDateOfModificationFrom is not null and @LoggedEntityDateOfModificationTo is not null)
            set @sql = @sql + '[LoggedEntityDateOfModification] >= @xLoggedEntityDateOfModificationFrom and [LoggedEntityDateOfModification] < @xLoggedEntityDateOfModificationTo and '
        else if (@LoggedEntityDateOfModificationFrom is not null and @LoggedEntityDateOfModificationTo is null)
            set @sql = @sql + '[LoggedEntityDateOfModification] >= @xLoggedEntityDateOfModificationFrom and '
        else if (@LoggedEntityDateOfModificationFrom is null and @LoggedEntityDateOfModificationTo is not null)
            set @sql = @sql + '[LoggedEntityDateOfModification] < @xLoggedEntityDateOfModificationTo and '

        if (@LoggedEntityGuid is not null)
            set @sql = @sql + '[LoggedEntityGuid] = @xLoggedEntityGuid and '
        else if (CHARINDEX(',LoggedEntityGuid,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityGuid] is null and '

        IF EXISTS (Select 1 from @LoggedEntityIdCollection)
            set @sql = @sql + '[LoggedEntityId] in (SELECT Id FROM @xLoggedEntityIdCollection) and '
        else if (CHARINDEX(',LoggedEntityId,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityId] is null and '

        if (@LoggedEntityIsArchived is not null)
            set @sql = @sql + '[LoggedEntityIsArchived] = @xLoggedEntityIsArchived and '
        else if (CHARINDEX(',LoggedEntityIsArchived,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityIsArchived] is null and '

        if (@LoggedEntityIsDeleted is not null)
            set @sql = @sql + '[LoggedEntityIsDeleted] = @xLoggedEntityIsDeleted and '
        else if (CHARINDEX(',LoggedEntityIsDeleted,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityIsDeleted] is null and '

        if (@LoggedEntityModifiedByUserGuid is not null)
            set @sql = @sql + '[LoggedEntityModifiedByUserGuid] = @xLoggedEntityModifiedByUserGuid and '
        else if (CHARINDEX(',LoggedEntityModifiedByUserGuid,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityModifiedByUserGuid] is null and '

        if (@LoggedEntityModifiedByUserName is not null)
            set @sql = @sql + '[LoggedEntityModifiedByUserName] = @xLoggedEntityModifiedByUserName and '
        else if (CHARINDEX(',LoggedEntityModifiedByUserName,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityModifiedByUserName] is null and '

        if (@LoggedEntityStringRepresentation is not null)
            set @sql = @sql + '[LoggedEntityStringRepresentation] = @xLoggedEntityStringRepresentation and '
        else if (CHARINDEX(',LoggedEntityStringRepresentation,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[LoggedEntityStringRepresentation] is null and '

        if (@StringRepresentation is not null)
            set @sql = @sql + '[StringRepresentation] = @xStringRepresentation and '

        if (@Url is not null)
            set @sql = @sql + '[Url] = @xUrl and '

        if (@UseCaseName is not null)
            set @sql = @sql + '[UseCaseName] = @xUseCaseName and '

        if (@BusinessGuid is not null)
            set @sql = @sql + '[BusinessGuid] = @xBusinessGuid and '
        else if (CHARINDEX(',BusinessGuid,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[BusinessGuid] is null and '

        if (@BusinessStringRepresentation is not null)
            set @sql = @sql + '[BusinessStringRepresentation] = @xBusinessStringRepresentation and '
        else if (CHARINDEX(',BusinessStringRepresentation,', @ListOfNullableColumns) <> 0)
            set @sql = @sql + '[BusinessStringRepresentation] is null and '

        if (@CreatedByUserGuid is not null)
            set @sql = @sql + '[CreatedByUserGuid] = @xCreatedByUserGuid and '

        if (@CreatedByUserName is not null)
            set @sql = @sql + '[CreatedByUserName] = @xCreatedByUserName and '

        if (@DateOfCreationFrom is not null and @DateOfCreationTo is not null)
            set @sql = @sql + '[DateOfCreation] >= @xDateOfCreationFrom and [DateOfCreation] < @xDateOfCreationTo and '
        else if (@DateOfCreationFrom is not null and @DateOfCreationTo is null)
            set @sql = @sql + '[DateOfCreation] >= @xDateOfCreationFrom and '
        else if (@DateOfCreationFrom is null and @DateOfCreationTo is not null)
            set @sql = @sql + '[DateOfCreation] < @xDateOfCreationTo and '

        if (@DateOfModificationFrom is not null and @DateOfModificationTo is not null)
            set @sql = @sql + '[DateOfModification] >= @xDateOfModificationFrom and [DateOfModification] < @xDateOfModificationTo and '
        else if (@DateOfModificationFrom is not null and @DateOfModificationTo is null)
            set @sql = @sql + '[DateOfModification] >= @xDateOfModificationFrom and '
        else if (@DateOfModificationFrom is null and @DateOfModificationTo is not null)
            set @sql = @sql + '[DateOfModification] < @xDateOfModificationTo and '

        if (@IsArchived is not null)
            set @sql = @sql + '[IsArchived] = @xIsArchived and '

        if (@IsDeleted is not null)
            set @sql = @sql + '[IsDeleted] = @xIsDeleted and '

        if (@ModifiedByUserGuid is not null)
            set @sql = @sql + '[ModifiedByUserGuid] = @xModifiedByUserGuid and '

        if (@ModifiedByUserName is not null)
            set @sql = @sql + '[ModifiedByUserName] = @xModifiedByUserName and '

        if (@IsArchived is null)
            set @sql = @sql + '[IsArchived] = 0 and '

        if (@IsDeleted is null)
            set @sql = @sql + '[IsDeleted] = 0 and '

        set @sql = left(@sql, len(@sql) - 3)

        if (@OrderByClause is not null)
        begin
            declare @xAllowedOrderByColumns table ([ColumnName] varchar(128) not null primary key)
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('ApplicationLayerName')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('DomainName')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('Url')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('UseCaseName')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('CreatedByUserGuid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('CreatedByUserName')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('DateOfCreation')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('DateOfModification')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('Guid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('IsArchived')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('IsDeleted')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('ModifiedByUserGuid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('ModifiedByUserName')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('Id')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityId')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('StringRepresentation')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityBusinessGuid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityBusinessStringRepresentation')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityCreatedByUserGuid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityCreatedByUserName')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityDateOfCreation')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityDateOfModification')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityGuid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityIsArchived')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityIsDeleted')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityModifiedByUserGuid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityModifiedByUserName')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('LoggedEntityStringRepresentation')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('BusinessGuid')
            insert into @xAllowedOrderByColumns ([ColumnName]) values ('BusinessStringRepresentation')

            declare @xSafeOrderByClause varchar(max) = ''
            declare @xRemainingOrderByClause varchar(max) = @OrderByClause
            declare @xOrderByItem varchar(512)
            declare @xCommaIndex int
            declare @xOrderByColumn varchar(128)
            declare @xOrderByDirection varchar(4)

            while (len(ltrim(rtrim(@xRemainingOrderByClause))) > 0)
            begin
                set @xCommaIndex = charindex(',', @xRemainingOrderByClause)

                if (@xCommaIndex = 0)
                begin
                    set @xOrderByItem = ltrim(rtrim(@xRemainingOrderByClause))
                    set @xRemainingOrderByClause = ''
                end
                else
                begin
                    set @xOrderByItem = ltrim(rtrim(left(@xRemainingOrderByClause, @xCommaIndex - 1)))
                    set @xRemainingOrderByClause = substring(@xRemainingOrderByClause, @xCommaIndex + 1, len(@xRemainingOrderByClause))
                end

                if (@xOrderByItem = '')
                    raiserror('Invalid OrderByClause.', 16, 1)

                set @xOrderByDirection = ''

                if (len(@xOrderByItem) > 5 and upper(right(@xOrderByItem, 5)) = ' DESC')
                begin
                    set @xOrderByDirection = 'DESC'
                    set @xOrderByColumn = ltrim(rtrim(left(@xOrderByItem, len(@xOrderByItem) - 5)))
                end
                else if (len(@xOrderByItem) > 4 and upper(right(@xOrderByItem, 4)) = ' ASC')
                begin
                    set @xOrderByDirection = 'ASC'
                    set @xOrderByColumn = ltrim(rtrim(left(@xOrderByItem, len(@xOrderByItem) - 4)))
                end
                else
                begin
                    set @xOrderByColumn = @xOrderByItem
                end

                if (left(@xOrderByColumn, 1) = '[' and right(@xOrderByColumn, 1) = ']')
                    set @xOrderByColumn = substring(@xOrderByColumn, 2, len(@xOrderByColumn) - 2)

                if (@xOrderByColumn like '%[^A-Za-z0-9_]%' or not exists (select 1 from @xAllowedOrderByColumns where [ColumnName] = @xOrderByColumn))
                    raiserror('Invalid OrderByClause.', 16, 1)

                if (len(@xSafeOrderByClause) > 0)
                    set @xSafeOrderByClause = @xSafeOrderByClause + ', '

                set @xSafeOrderByClause = @xSafeOrderByClause + quotename(@xOrderByColumn)

                if (@xOrderByDirection <> '')
                    set @xSafeOrderByClause = @xSafeOrderByClause + ' ' + @xOrderByDirection
            end

            set @sql = @sql + ' order by ' + @xSafeOrderByClause
        end

    declare @paramlist nvarchar(max)
    set @paramlist = '
    @xIdCollection ListOfId READONLY,
    @xApplicationLayerName varchar(32),
    @xDomainName varchar(128),
    @xGuid uniqueidentifier,
    @xLoggedEntityBusinessGuid uniqueidentifier,
    @xLoggedEntityBusinessStringRepresentation nvarchar(1024),
    @xLoggedEntityCreatedByUserGuid uniqueidentifier,
    @xLoggedEntityCreatedByUserName varchar(50),
    @xLoggedEntityDateOfCreationFrom datetime2,
    @xLoggedEntityDateOfCreationTo datetime2,
    @xLoggedEntityDateOfModificationFrom datetime2,
    @xLoggedEntityDateOfModificationTo datetime2,
    @xLoggedEntityGuid uniqueidentifier,
    @xLoggedEntityIdCollection ListOfId READONLY,
    @xLoggedEntityIsArchived bit,
    @xLoggedEntityIsDeleted bit,
    @xLoggedEntityModifiedByUserGuid uniqueidentifier,
    @xLoggedEntityModifiedByUserName varchar(50),
    @xLoggedEntityStringRepresentation varchar(290),
    @xStringRepresentation varchar(290),
    @xUrl varchar(1024),
    @xUseCaseName varchar(128),
    @xBusinessGuid uniqueidentifier,
    @xBusinessStringRepresentation nvarchar(1024),
    @xCreatedByUserGuid uniqueidentifier,
    @xCreatedByUserName varchar(50),
    @xDateOfCreationFrom datetime2,
    @xDateOfCreationTo datetime2,
    @xDateOfModificationFrom datetime2,
    @xDateOfModificationTo datetime2,
    @xIsArchived bit,
    @xIsDeleted bit,
    @xModifiedByUserGuid uniqueidentifier,
    @xModifiedByUserName varchar(50)
'
    exec sp_executesql @sql, @paramlist,
    @IdCollection,
    @ApplicationLayerName,
    @DomainName,
    @Guid,
    @LoggedEntityBusinessGuid,
    @LoggedEntityBusinessStringRepresentation,
    @LoggedEntityCreatedByUserGuid,
    @LoggedEntityCreatedByUserName,
    @LoggedEntityDateOfCreationFrom,
    @LoggedEntityDateOfCreationTo,
    @LoggedEntityDateOfModificationFrom,
    @LoggedEntityDateOfModificationTo,
    @LoggedEntityGuid,
    @LoggedEntityIdCollection,
    @LoggedEntityIsArchived,
    @LoggedEntityIsDeleted,
    @LoggedEntityModifiedByUserGuid,
    @LoggedEntityModifiedByUserName,
    @LoggedEntityStringRepresentation,
    @StringRepresentation,
    @Url,
    @UseCaseName,
    @BusinessGuid,
    @BusinessStringRepresentation,
    @CreatedByUserGuid,
    @CreatedByUserName,
    @DateOfCreationFrom,
    @DateOfCreationTo,
    @DateOfModificationFrom,
    @DateOfModificationTo,
    @IsArchived,
    @IsDeleted,
    @ModifiedByUserGuid,
    @ModifiedByUserName
end

GO

