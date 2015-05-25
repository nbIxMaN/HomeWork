CREATE TABLE [dbo].[Clients]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(20) NOT NULL, 
    [Phone] NCHAR(12) NOT NULL, 
    [Adress] NVARCHAR(MAX) NOT NULL, 
    [Info] NVARCHAR(MAX) NOT NULL
		
)
