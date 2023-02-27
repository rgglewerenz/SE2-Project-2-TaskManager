CREATE TABLE [dbo].[Users] (
	Personid int NOT NULL,
    LastName varchar(255) NOT NULL,
    FirstName varchar(255),
    Email varchar(MAX) NOT NULL,
    Age int,
    PRIMARY KEY (Personid)
)