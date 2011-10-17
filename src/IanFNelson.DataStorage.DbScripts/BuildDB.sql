if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IanFNelson_DataStorage]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[IanFNelson_DataStorage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IanFNelson_DataStorage_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IanFNelson_DataStorage_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IanFNelson_DataStorage_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IanFNelson_DataStorage_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IanFNelson_DataStorage_GetObject]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IanFNelson_DataStorage_GetObject]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IanFNelson_DataStorage_GetObjectList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IanFNelson_DataStorage_GetObjectList]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



CREATE TABLE [dbo].[IanFNelson_DataStorage] (
	[application] [nvarchar] (100) NOT NULL,
	[user] [nvarchar] (100) NOT NULL,
	[key] [nvarchar] (100) NOT NULL,
	[locked] [bit] NOT NULL ,
	[createdTime] [datetime] NOT NULL ,
	[updatedTime] [datetime] NOT NULL ,
	[type] [nvarchar] (250) NOT NULL ,
	[object] [image] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



CREATE  PROCEDURE dbo.IanFNelson_DataStorage_Add
(
	@application	nvarchar(100),
	@user			nvarchar(100),
	@key			nvarchar(100),
	@locked			bit,
	@type			nvarchar(250),
	@object			image
)

AS

SET NOCOUNT ON

if exists
(
	select [user] from IanFNelson_datastorage where [user]=@user and [key]=@key and [application]=@application
)
begin
	update
		dbo.IanFNelson_datastorage
	set
		locked=@locked,
		updatedTime=getdate(),
		type=@type,
		object=@object
	where
		[application]=@application
    and	[user]=@user
	and	[key]=@key
end
else
begin
	insert into
		dbo.IanFNelson_datastorage
	(
		[application],
		[user],
		[key],
		locked,
		createdTime,
		updatedTime,
		type,
		[object]
	)
	values
	(
		@application,
		@user,
		@key,
		@locked,
		getdate(),
		getdate(),
		@type,
		@object
	)
end




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO






CREATE   PROCEDURE dbo.IanFNelson_DataStorage_Delete

(
	@application	nvarchar(100) = null,
	@user			nvarchar(100) = null,
	@key			nvarchar(100) = null
)

AS

SET NOCOUNT ON

delete from
	dbo.IanFNelson_DataStorage
where
	(([application]=@application) or @application is null)
and	(([user]=@user) or @user is null)
  and	(([key]=@key) or @key is null)





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO





CREATE  PROCEDURE dbo.IanFNelson_DataStorage_GetObject
(
	@application	nvarchar(100),
	@user			nvarchar(100),
	@key			nvarchar(100)
)

AS

SET NOCOUNT ON

select
	[object]
from
	dbo.IanFNelson_DataStorage
where
	[application]=@application
	and	[user]=@user
  and   [key]=@key



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO







CREATE    PROCEDURE dbo.IanFNelson_DataStorage_GetObjectList

(
	@application	nvarchar(100) = null,
	@user		nvarchar(100) = null,
	@key		nvarchar(100) = null
)

AS

SET NOCOUNT ON

select
	[application],
	[user],
	[key],
	locked,
	createdTime,
	updatedTime,
	type
from
	dbo.IanFNelson_DataStorage
where
(([application]=@application) or @application is null)
and	(([user]=@user) or @user is null)
  and	(([key]=@key) or @key is null)






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

