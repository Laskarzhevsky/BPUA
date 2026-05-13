CREATE TABLE [dbo].[EntityRelation]
(
	[Id] [bigint] IDENTITY(1,1) NOT NULL,

	[EntityGuid] [uniqueidentifier] NULL,
	[EntityStringRepresentation] [nvarchar](1024) NULL,
	[EntitySubtypeGuid] [uniqueidentifier] NULL,
	[EntitySubtypeStringRepresentation] [nvarchar](1024) NULL,
	[EntityTypeGuid] [uniqueidentifier] NULL,
	[EntityTypeStringRepresentation] [nvarchar](1024) NULL,
	[RelatedEntityGuid] [uniqueidentifier] NULL,
	[RelatedEntityStringRepresentation] [nvarchar](1024) NULL,
	[RelatedEntitySubtypeGuid] [uniqueidentifier] NULL,
	[RelatedEntitySubtypeStringRepresentation] [nvarchar](1024) NULL,
	[RelatedEntityTypeGuid] [uniqueidentifier] NULL,
	[RelatedEntityTypeStringRepresentation] [nvarchar](1024) NULL,
	
	[BusinessGuid] [uniqueidentifier] NULL,
	[BusinessStringRepresentation] [nvarchar](1024) NULL,
	[CreatedByUserGuid] [uniqueidentifier] NULL,
	[CreatedByUserName] [varchar](50) NOT NULL,
	[DateOfCreation] [datetime] NOT NULL,
	[DateOfModification] [datetime] NOT NULL,
	[Guid] [uniqueidentifier] NULL,
	[IsArchived] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ModifiedByUserGuid] [uniqueidentifier] NULL,
	[ModifiedByUserName] [varchar](50) NOT NULL,
	[StringRepresentation] [nvarchar](1024) NULL,

    CONSTRAINT [PK_EntityRelation] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
