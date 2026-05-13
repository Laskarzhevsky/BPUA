create type [dbo].[HostedApplicationLayerNullableColumn] AS TABLE 
(
    [ApplicationLayerName]  BIT DEFAULT ((0)) NULL,
    [DomainName]  BIT DEFAULT ((0)) NULL,
    [Guid]  BIT DEFAULT ((0)) NULL,
    [StringRepresentation]  BIT DEFAULT ((0)) NULL,
    [Url]  BIT DEFAULT ((0)) NULL,
    [UseCaseName]  BIT DEFAULT ((0)) NULL
)


GO

