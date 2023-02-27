CREATE TABLE [dbo].[Tasks]
(
	[TaskID] INT NOT NULL Identity(1,1) Primary Key,
	[Description] Varchar(150) not NULL,
	[Title] Varchar(50) not NULL
)