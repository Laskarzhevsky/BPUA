CREATE TYPE [dbo].[ListOfString] AS TABLE (
    [Id]                BIGINT         NOT NULL,
    [CreatedByUserName] VARCHAR (50)   NULL,
    [DateOfCreation]    DATETIME2 (7)  NULL,
    [value]             VARCHAR (1024) NOT NULL);

