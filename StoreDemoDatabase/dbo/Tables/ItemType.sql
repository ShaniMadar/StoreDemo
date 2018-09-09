CREATE TABLE [dbo].[ItemType] (
    [id]           INT           IDENTITY (1, 1) NOT NULL,
    [name]         VARCHAR (120) NOT NULL,
    [returnPeriod] INT           CONSTRAINT [DF_Constraint] DEFAULT ((15)) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

