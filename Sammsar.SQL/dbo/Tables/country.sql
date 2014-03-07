---------------------------------
create table Country
(
	[ID] int not null primary key,
	[Name] varchar (30) not null,
	[ParentID] int null,
	CONSTRAINT [FK_Country_ParentID] FOREIGN KEY (ParentID) REFERENCES [Country]([ID])
)