SELECT 
    [Views].[name] AS [Name], 
    [Views].[object_id] AS [ObjectId],
    CAST([ExtendedProperties].[value] AS NVARCHAR(MAX)) AS [Description]
FROM [sys].[views] AS [Views]
LEFT JOIN [sys].[extended_properties] AS [ExtendedProperties]
    ON [Views].[object_id] = [ExtendedProperties].[major_id]
    AND [ExtendedProperties].[minor_id] = 0
    AND [ExtendedProperties].[name] = 'MS_Description'
WHERE [Views].[schema_id] = SCHEMA_ID('dbo')
  AND [Views].[name] IN (SELECT [value] FROM STRING_SPLIT(@SelectedTables, ','))
ORDER BY [Views].[name]