CREATE TABLE [dbo].[Orders]
(
	[Number] INT NOT NULL PRIMARY KEY, 
    [DateTime] DATETIME NOT NULL, 
    [Adress] NCHAR(10) NOT NULL, 
    [PaymentType] INT NOT NULL, 
    [Price] INT NOT NULL, 
    [ClientID] INT NOT NULL
	CONSTRAINT [FK_Orders_FlightNumber] FOREIGN KEY ([Adress]) REFERENCES [Planes]([Id])
	CONSTRAINT [FK_Orders_Clients] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id]), 
    [CarNumber] NCHAR(10) NULL
)
