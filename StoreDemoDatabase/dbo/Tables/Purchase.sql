CREATE TABLE [dbo].[Purchase] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME       NOT NULL,
    [Employee]   INT            NOT NULL,
    [Status]     INT            NOT NULL,
    [TotalSum]   DECIMAL (7, 2) NOT NULL,
    [PaidAmount] DECIMAL (7, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Employee]) REFERENCES [dbo].[Employees] ([Id]),
    FOREIGN KEY ([Status]) REFERENCES [dbo].[PurchaseStatusType] ([Id])
);

