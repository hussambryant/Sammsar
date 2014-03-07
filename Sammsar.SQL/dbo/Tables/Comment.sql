-----------------------------------
create table Comment
(
	[ID] UNIQUEIDENTIFIER primary key ,
	[AdverID] UNIQUEIDENTIFIER not null ,
	[UserID] UNIQUEIDENTIFIER not null , 
	[Date] date not null ,
	[Comment] varchar (750) NOT NULL ,
	[IsSeen] BIT NOT NULL DEFAULT 0, 
    constraint FK_Comment_AdverID foreign key (adverID) references Advertising(ID),
	constraint FK_Comment_UserID foreign key (UserID) references AspNetUsers(Id)
)