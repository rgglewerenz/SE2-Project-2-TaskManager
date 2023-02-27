CREATE TABLE [dbo].[TaskRecurranceMap]
(
	[TaskRecurranceMapID] INT NOT NULL Identity(1,1) Primary Key,
	[TaskID] int FOREIGN KEY References Tasks(TaskID),
	[IsRecurring] bit not Null,
	[IsRecurringWeekly] bit not Null,
	[IsRecurringBiWeekly] bit not Null,
	[IsRecurringMonthly] bit not Null,
	[MonthlyRecurringDays] varchar(31),
	[WeeklyRecurringDays] varchar(7),
	[FirstOccurrance] DateTime not Null,
)
