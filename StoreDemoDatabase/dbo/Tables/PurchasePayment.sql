CREATE TABLE [dbo].[PurchasePayment] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [PurchaseId]    INT            NOT NULL,
    [PaymentMethod] INT            NOT NULL,
    [Sum]           DECIMAL (7, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([PaymentMethod]) REFERENCES [dbo].[PaymentMethod] ([id]),
    FOREIGN KEY ([PurchaseId]) REFERENCES [dbo].[Purchase] ([Id])
);

