﻿USE [Ojala]
GO
/****** Object:  StoredProcedure [dbo].[ProviderRating_Insert]    Script Date: 8/14/2024 11:07:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Daniel Gomez
-- Create date: 08/12/24
-- Description:	Inserts a Provider Rating from a Consumer in [dbo].[ProviderRating]
-- Code Reviewer: Alec Snowball 8/14/24

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE proc [dbo].[ProviderRating_Insert]
					@ProviderId int,
					@Rating int,
					@ConsumerId int
AS

/*--TEST CODE--

DECLARE @ProviderId int = --0,
	@Rating int = 5 ,
	@ConsumerId int = --0

EXECUTE [dbo].[ProviderRating_Insert]
					@ProviderId,
					@Rating,
					@ConsumerId

*/

BEGIN

INSERT INTO [dbo].[ProviderRating]
           ([ProviderId]
           ,[Rating]
           ,[ConsumerId])
     VALUES
           (@ProviderId,
	    @Rating,
	    @ConsumerId)
END


GO
