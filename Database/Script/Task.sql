Create table Task (
Id  int  primary Key  identity(1,1) not null,
[Name]  varchar(255) not null,
[Description]  varchar(255)  null,
Deadline  datetime not null,
[Status] int not null,
CreateDate datetime not null,
UpdateDate datetime null
)