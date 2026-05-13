create procedure [HostedApplicationLayerLog].[Get]
    @Id bigint
as
begin
    select
        *
    from
        [dbo].[HostedApplicationLayerLog]
    where
        [Id]=@Id
        and [IsArchived]=0
        and [IsDeleted]=0
end

GO

