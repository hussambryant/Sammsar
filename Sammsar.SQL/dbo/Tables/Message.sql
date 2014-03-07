-----------------------------------
create table Message
(
	[ID] UNIQUEIDENTIFIER not null PRIMARY KEY ,
	[SenderUserID] UNIQUEIDENTIFIER not null ,
	[ReciverUserID] UNIQUEIDENTIFIER not null ,
	[Message] nvarchar(MAX) not null ,
	[Date] DATETIME not null ,
	[IsSeen] bit not null DEFAULT 0, 
    CONSTRAINT [FK_Message_Sender] FOREIGN KEY ([SenderUserID]) REFERENCES [AspNetUsers]([Id]),
	CONSTRAINT [FK_Message_Reciever] FOREIGN KEY ([ReciverUserID]) REFERENCES [AspNetUsers]([Id])
)