﻿USE [Ojala]
GO
/****** Object:  StoredProcedure [dbo].[ProviderRating_SelectByProviderId]    Script Date: 8/14/2024 11:07:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Daniel Gomez
-- Create date: 08/12/24
-- Description: Selects Provider's basic info and lists of all their ratings from [dbo].[ProviderRating] by ProviderId
-- Code Reviewer: Alec Snowball 8/14/24

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE proc [dbo].[ProviderRating_SelectByProviderId]
						    @ProviderId INT
AS

/*--TEST CODE--

DECLARE @ProviderId INT = --00
EXECUTE [dbo].[ProviderRating_SelectByProviderId]
						@ProviderId

*/

BEGIN

SELECT 
    dbo.fn_GetUserJSON(P.[ProviderId]) AS [Provider],
    (
        SELECT 
            PR.[Rating],
            PR.[ConsumerId]
        FROM 
            [dbo].[ProviderRating] PR
        WHERE 
            PR.[ProviderId] = P.[ProviderId]
        FOR JSON PATH
    ) AS [Ratings]
FROM 
    [dbo].[ProviderRating] P
WHERE 
    P.[ProviderId] = @ProviderId
GROUP BY 
    P.[ProviderId]

END
GO
