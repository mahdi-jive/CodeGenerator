SELECT 
    [Procedures].[name] AS [ProcedureName],
    [Procedures].[object_id] AS [ProcedureId],
    [Parameters].[name] AS [Name],
    [Parameters].[parameter_id] AS [ObjectId],
    ISNULL([SysType].[name] ,[Types].[name])AS [ParameterType],
    [Types].[is_table_type] AS [IsTableType],
    [Parameters].[max_length] AS [MaxLength],
    [Parameters].[is_output] AS [IsOutput],
    [Parameters].[parameter_id] AS [ParameterOrder],
    [ExtendedProperties].[value] AS [Description]
FROM [sys].[procedures] AS [Procedures]
JOIN [sys].[parameters] AS [Parameters]
    ON [Procedures].[object_id] = [Parameters].[object_id]
JOIN [sys].[types] AS [Types]
    ON [Parameters].[user_type_id] = [Types].[user_type_id]
LEFT JOIN [sys].[types] [SysType] ON [SysType].[user_type_id] = [types].[system_type_id]
LEFT JOIN [sys].[extended_properties] AS [ExtendedProperties]
    ON [Parameters].[object_id] = [ExtendedProperties].[major_id]
    AND [Parameters].[parameter_id] = [ExtendedProperties].[minor_id]
    AND [ExtendedProperties].[class] = 2 -- class 2 = parameters
    AND [ExtendedProperties].[name] = 'MS_Description'
ORDER BY 
    [ProcedureName],
    [Parameters].[parameter_id];

