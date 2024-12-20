USE [Ojala]
GO
/****** Object:  StoredProcedure [dbo].[ProviderRating_SelectAll]    Script Date: 8/14/2024 11:07:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Gomez
-- Create date: 8/12/24
-- Description:	Selects basicUser info of ProviderId and lists of all their ratings from [dbo].[ProviderRating] 
-- Code Reviewer: Alec Snowball 8/14/24

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE proc [dbo].[ProviderRating_SelectAll]

AS

/*--TEST CODE--

EXECUTE [dbo].[ProviderRating_SelectAll]

*/

BEGIN

SELECT 
    dbo.fn_GetUserJSON(P.[ProviderId]) AS Provider,
    (
        SELECT 
            PR.[Rating],
            PR.[ConsumerId]
        FROM 
            [dbo].[ProviderRating] PR
        WHERE 
            PR.[ProviderId] = P.[ProviderId]
        FOR JSON PATH
    ) AS Ratings
FROM 
    (SELECT DISTINCT ProviderId FROM [dbo].[ProviderRating]) P

END


GO
