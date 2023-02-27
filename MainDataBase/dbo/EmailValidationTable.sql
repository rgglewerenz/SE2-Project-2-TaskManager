CREATE TABLE [dbo].[EmailValidationTable]
(
	[EmailValidationID] INT NOT NULL Identity(1,1) Primary Key,
	[UserID] Int FOREIGN KEY REFERENCES Users(UserID),
	[IsValid] bit Not NULL,
	[ActivationCode] varchar(6),
	[ActivationCodeCreation] DateTime,
)
