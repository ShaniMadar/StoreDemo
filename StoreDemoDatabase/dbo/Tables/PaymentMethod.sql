CREATE TABLE [dbo].[PaymentMethod] (
    [id]   INT           IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (120) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

