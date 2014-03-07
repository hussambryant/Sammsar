create table Category 
(
	[ID] int not null primary key,
	[Name] varchar (100) not null ,
	[ParentID] int null,
	CONSTRAINT [FK_Categort_ParentID] FOREIGN KEY (ParentID) REFERENCES [Category]([ID])
)