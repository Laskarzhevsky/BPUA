CREATE TYPE [dbo].[ListOfLong] AS TABLE (
    [Id]                BIGINT        NOT NULL,
    [CreatedByUserName] VARCHAR (50)  NULL,
    [DateOfCreation]    DATETIME2 (7) NULL,
    [Value]             BIGINT        NOT NULL);

