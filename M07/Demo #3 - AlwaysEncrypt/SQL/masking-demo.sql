DECLARE @p0 NVarChar(58);
SET @p0 = 'EXEC sp_set_session_context @key=N''UserId'', @value=@UserId';
DECLARE @p1 NVarChar(20);
SET @p1 = '@UserId nvarchar(36)';
DECLARE @p2 NVarChar(36);
SET @p2 = '2ba087ec-d8c3-4955-9a9d-c6719dc29ec2';   
EXECUTE [sp_executesql] @p0, @p1, @UserId=@p2


SELECT * FROM Patients