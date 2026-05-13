create procedure [HostedApplicationLayer].[Get]
    @Id bigint
as
begin
    select
        *
    from
        [dbo].[HostedApplicationLayer]
    where
        [Id]=@Id
        and [IsArchived]=0
        and [IsDeleted]=0
end

GO

