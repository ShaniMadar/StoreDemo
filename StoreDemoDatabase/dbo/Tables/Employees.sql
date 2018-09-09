CREATE TABLE [dbo].[Employees] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (120) NOT NULL,
    [LastName]  VARCHAR (120) NOT NULL,
    [StartDate] DATETIME      NOT NULL,
    [EndDate]   DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

