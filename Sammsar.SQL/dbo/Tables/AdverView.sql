-----------------------------------
create table AdverView
(
[ID] UNIQUEIDENTIFIER NOT NULL,
[AdverID] UNIQUEIDENTIFIER NOT NULL  ,
[UserID] UNIQUEIDENTIFIER not null ,
[Date] DATETIME not null ,
CONSTRAINT FK_AdverView_AdverID foreign KEY (AdverID) references Advertising(ID),
CONSTRAINT FK_AdverView_UserID foreign KEY (UserID) references AspNetUsers(ID), 
    CONSTRAINT [PK_AdverView] PRIMARY KEY ([ID])
)