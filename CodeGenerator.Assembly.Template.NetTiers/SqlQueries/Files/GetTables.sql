SELECT 
    [Tables].[name] AS [Name], 
    [Tables].[object_id] AS [ObjectId],
    CAST([ExtendedProperties].[value] AS NVARCHAR(MAX)) AS [Description]
FROM [sys].[tables] AS [Tables]
LEFT JOIN [sys].[extended_properties] AS [ExtendedProperties]
    ON [Tables].[object_id] = [ExtendedProperties].[major_id]
    AND [ExtendedProperties].[minor_id] = 0
    AND [ExtendedProperties].[name] = 'MS_Description'
WHERE [Tables].[schema_id] = SCHEMA_ID('dbo')
  AND [Tables].[name] IN (SELECT [value] FROM STRING_SPLIT(@SelectedTables, ','))
ORDER BY [Tables].[name]