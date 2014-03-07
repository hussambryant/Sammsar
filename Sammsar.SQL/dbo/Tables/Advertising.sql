---------------------------------
create table Advertising
(
[ID] UNIQUEIDENTIFIER primary key ,
[UserID] UNIQUEIDENTIFIER not null ,
[Price] money null ,
[Mobile] VARCHAR(50) null ,
[CommentsEnabled] bit not null ,
[Date] DATETIME not null ,
[CountryID] int not null ,
[Approve] bit not null DEFAULT 0 ,
constraint FK_Adver_country foreign key (CountryID) references Country(ID),
constraint FK_Adver_ID foreign key (ID) references Item(ID),
constraint FK_Adver_UserID foreign key (UserID) references AspNetUsers(ID)
)