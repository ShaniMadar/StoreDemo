CREATE TABLE [dbo].[PurchaseDetails] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [PurchaseId] INT NOT NULL,
    [Item]       INT NOT NULL,
    [Quantity]   INT NOT NULL,
    [Status]     INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Item]) REFERENCES [dbo].[Items] ([id]),
    FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[Purchase] ([Id]),
    FOREIGN KEY ([Status]) REFERENCES [dbo].[PurchaseStatusType] ([Id])
);

