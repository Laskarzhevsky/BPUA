CREATE TYPE [dbo].[ListOfInt] AS TABLE (
    [Id]                INT           NOT NULL,
    [CreatedByUserName] VARCHAR(50)   NULL,
    [DateOfCreation]    DATETIME2(7)  NULL,
    [Value]             INT        NOT NULL,
    [ClientKey]         VARCHAR(50)   NULL
);