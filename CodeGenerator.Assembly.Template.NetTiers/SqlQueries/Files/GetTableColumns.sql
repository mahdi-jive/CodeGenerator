WITH FilteredExtendedProperties AS
(
    SELECT
        major_id,
        minor_id,
        value
    FROM sys.extended_properties
    WHERE name = 'MS_Description'
      AND major_id IN (SELECT object_id FROM sys.tables WHERE name IN (SELECT value FROM STRING_SPLIT(@SelectedTables, ',')))
)
SELECT
    [Tables].[name] AS [TableName],
    [Tables].[object_id] AS [TableObjectId],
    [Columns].[name] AS [Name],
    [Columns].[column_id] AS [ObjectId],
    [DataTypes].[name] AS [DataType],
    [Columns].[max_length] AS [MaxLength],
    [Columns].[precision] AS [Precision],
    [Columns].[scale] AS [Scale],
    [Columns].[collation_name] AS [Collation],
    [Columns].[is_nullable] AS [IsNullable],
    [Columns].[is_identity] AS [IsIdentity],
    [Columns].[is_computed] AS [IsComputed],
    [Columns].[is_rowguidcol] AS [IsRowGuid],
    [Columns].[is_sparse] AS [IsSparse],
    [Columns].[generated_always_type_desc] AS [GeneratedAlwaysType],
    ISNULL([Indexes].[is_primary_key], 0) AS [IsPrimaryKey],
    [DefaultConstraints].[definition] AS [DefaultValue],
    [ExtendedProperties].[value] AS [Description]

FROM [sys].[tables] AS [Tables]
JOIN [sys].[columns] AS [Columns] ON [Tables].[object_id] = [Columns].[object_id]
JOIN [sys].[types] AS [DataTypes] ON   [Columns].[system_type_id]  = [DataTypes].[user_type_id]
LEFT JOIN [sys].[default_constraints] AS [DefaultConstraints] ON [Columns].[default_object_id] = [DefaultConstraints].[object_id]
LEFT JOIN [FilteredExtendedProperties] AS [ExtendedProperties]
    ON [ExtendedProperties].[major_id] = [Columns].[object_id]
    AND [ExtendedProperties].[minor_id] = [Columns].[column_id]
LEFT JOIN [sys].[index_columns] AS [IndexColumns]
    ON [IndexColumns].[object_id] = [Columns].[object_id]
    AND [IndexColumns].[column_id] = [Columns].[column_id]
LEFT JOIN [sys].[indexes] AS [Indexes]
    ON [Indexes].[object_id] = [IndexColumns].[object_id]
    AND [Indexes].[index_id] = [IndexColumns].[index_id]
    AND [Indexes].[is_primary_key] = 1

WHERE [Tables].[name] IN (SELECT [value] FROM STRING_SPLIT(@SelectedTables, ','))

ORDER BY [Tables].[name], [Columns].[column_id]
