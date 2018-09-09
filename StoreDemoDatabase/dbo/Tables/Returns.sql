CREATE TABLE [dbo].[Returns] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [TotalAmount]      DECIMAL (7, 2) NOT NULL,
    [CreditType]       INT            NOT NULL,
    [Employee]         INT            NOT NULL,
    [Date]             DATETIME       NOT NULL,
    [PurchaseDetailId] INT            NULL,
    [Quantity]         INT            NOT NULL,
    [PurchaseId]       INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CreditType]) REFERENCES [dbo].[PaymentMethod] ([id]),
    FOREIGN KEY ([Employee]) REFERENCES [dbo].[Employees] ([Id]),
    FOREIGN KEY ([PurchaseDetailId]) REFERENCES [dbo].[PurchaseDetails] ([Id]),
    FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[Purchase] ([Id])
);

