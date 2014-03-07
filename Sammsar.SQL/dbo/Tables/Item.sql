-----------------------------------
create table Item
(
	[ID] UNIQUEIDENTIFIER not null primary key,
	[Name] varchar (50) not null ,
	[Describtion] varchar (1000) null ,
	[CategoryID] int not null ,
	[Image1] varchar (1000) null , -- varbinary(3000) Use varbinary(max) when the column data entries exceed 8,000 bytes http://technet.microsoft.com/en-us/library/ms188362.aspx
	[Image2] varchar (1000) null ,
	[Image3] varchar (1000) null ,
	[Image4] varchar (1000) null ,
	constraint FK_Item_Category foreign key (CategoryID) references category(ID) ,
	constraint FK_Item_Advertis_num foreign key ([ID]) references Advertising (ID)
)