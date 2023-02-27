CREATE TABLE [dbo].[Users]
(
	[UserID] INT NOT NULL Identity(1,1) Primary Key,
	[UserName] Varchar(150) not NULL Unique, 
	[Email] Varchar(150) not NULL Unique, 
	[PasswordHash] Varchar(150) not NULL, 
	[Age] INT not NUll
)