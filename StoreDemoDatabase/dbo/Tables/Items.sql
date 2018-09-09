CREATE TABLE [dbo].[Items] (
    [id]       INT            IDENTITY (1, 1) NOT NULL,
    [itemType] INT            NOT NULL,
    [name]     VARCHAR (250)  NOT NULL,
    [price]    DECIMAL (7, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([itemType]) REFERENCES [dbo].[ItemType] ([id])
);

