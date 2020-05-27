DECLARE @p0 NVarChar(58);
SET @p0 = 'EXEC sp_set_session_context @key=N''UserId'', @value=@UserId';
DECLARE @p1 NVarChar(20);
SET @p1 = '@UserId nvarchar(36)';
DECLARE @p2 NVarChar(36);
SET @p2 = '2ba087ec-d8c3-4955-9a9d-c6719dc29ec2';   
EXECUTE [sp_executesql] @p0, @p1, @UserId=@p2


SELECT 
    [Extent1].[VisitID] AS [VisitID], 
    [Extent1].[PatientID] AS [PatientID], 
    [Extent1].[Date] AS [Date], 
    [Extent1].[Reason] AS [Reason], 
    [Extent1].[Treatment] AS [Treatment], 
    [Extent1].[FollowUpDate] AS [FollowUpDate], 
    [Extent2].[PatientID] AS [PatientID1], 
    [Extent2].[SSN] AS [SSN], 
    [Extent2].[FirstName] AS [FirstName], 
    [Extent2].[LastName] AS [LastName], 
    [Extent2].[MiddleName] AS [MiddleName], 
    [Extent2].[StreetAddress] AS [StreetAddress], 
    [Extent2].[City] AS [City], 
    [Extent2].[ZipCode] AS [ZipCode], 
    [Extent2].[State] AS [State], 
    [Extent2].[BirthDate] AS [BirthDate]
    FROM  [dbo].[Visits] AS [Extent1]
    INNER JOIN [dbo].[Patients] AS [Extent2] ON [Extent1].[PatientID] = [Extent2].[PatientID]