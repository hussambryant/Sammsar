CREATE TABLE [dbo].[AspNetUsers] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [UserName]      NVARCHAR (MAX) NOT NULL,
    [PasswordHash]  NVARCHAR (MAX) NOT NULL,
	[Email] NVARCHAR(50) NOT NULL,
	[Phone] NVARCHAR(30) NULL,
    [SecurityStamp] NVARCHAR (MAX) NULL,
    [Discriminator] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE (UserName),
	UNIQUE (Email)
);

