CREATE TABLE [dbo].[PasswordResetTable]
(
	[PasswordResetTableID] INT NOT NULL Identity(1,1) Primary Key,
	[UserID] Int FOREIGN KEY REFERENCES Users(UserID),
	[Code] varchar(20) not NUll,
	[CreationDate] datetime not NULL
)
