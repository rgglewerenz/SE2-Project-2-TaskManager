CREATE TABLE [dbo].[ApiAuthCodeTable]
(
	[ApiAuthCodeTableID] INT NOT NULL Identity(1,1) Primary Key,
	[UserID] Int FOREIGN KEY REFERENCES Users(UserID),
	[Code] varchar(30) NOT NULL,
	[CreationDate] datetime not Null
)
