SELECT 
    [Procedures].[name] AS [Name], 
    [Procedures].[object_id] AS [ObjectId],
    [ExtendedProperties].[value] AS [Description],
    [SqlModules].[definition] AS [Body]
FROM 
    [sys].[procedures] AS [Procedures]
LEFT JOIN 
    [sys].[extended_properties]  [ExtendedProperties]
        ON [ExtendedProperties].[major_id] = [Procedures].[object_id] 
        AND [ExtendedProperties].[minor_id] = 0 
        AND [ExtendedProperties].[name] = 'MS_Description'
 JOIN 
    [sys].[sql_modules] AS [SqlModules]
        ON [SqlModules].[object_id] = [Procedures].[object_id]
WHERE 
    [Procedures].[schema_id] = SCHEMA_ID('dbo')
ORDER BY 
    [Procedures].[name];