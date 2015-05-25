CREATE TABLE [dbo].[Clients]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(20) NOT NULL, 
    [Phone] NCHAR(12) NOT NULL, 
    [CardNumber] NVARCHAR(MAX) NOT NULL, 
    [PassportNumber] NVARCHAR(MAX) NOT NULL
		
)
