CREATE TABLE [dbo].[TaskRecurranceMap]
(
	[TaskRecurranceMapID] INT NOT NULL Identity(1,1) Primary Key,
	[TaskID] int FOREIGN KEY References Tasks(TaskID),
	[RecurringType] int Not NULL,
	[RecurringDays] varchar(31),
	[FirstOccurrance] DateTime not Null,
)
