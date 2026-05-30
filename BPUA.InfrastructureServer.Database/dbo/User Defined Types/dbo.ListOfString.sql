CREATE TYPE [dbo].[ListOfString] AS TABLE (
    [Id]                VARCHAR(50)  NOT NULL,
    [CreatedByUserName] VARCHAR(50)  NULL,
    [DateOfCreation]    DATETIME2(7) NULL,
    [Value]             VARCHAR(50)  NOT NULL,
    [ClientKey]         VARCHAR(50)  NULL
);