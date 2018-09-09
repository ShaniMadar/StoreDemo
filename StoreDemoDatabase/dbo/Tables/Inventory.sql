CREATE TABLE [dbo].[Inventory] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Item]        INT      NOT NULL,
    [Quantity]    INT      NOT NULL,
    [LastUpdated] DATETIME NOT NULL,
    [Employee]    INT      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Employee]) REFERENCES [dbo].[Employees] ([Id]),
    FOREIGN KEY ([Item]) REFERENCES [dbo].[Items] ([id])
);

