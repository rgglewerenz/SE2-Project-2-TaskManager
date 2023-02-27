CREATE TABLE [dbo].[UserTasksMap]
(
	[UserTaskMapID] INT NOT NULL Identity(1,1) Primary Key,
	[UserID] Int FOREIGN KEY REFERENCES Users(UserID),
	[TaskID] int FOREIGN KEY References Tasks(TaskID)
)
