WITH FilteredExtendedProperties
AS (SELECT [extended_properties].[major_id],
           [extended_properties].[minor_id],
           [extended_properties].[value]
    FROM [sys].[extended_properties]
    WHERE name = 'MS_Description'
          AND [extended_properties].[major_id] IN
              (
                  SELECT [views].[object_id]
                  FROM [sys].[views]
                  WHERE name IN
                        (
                            SELECT value FROM STRING_SPLIT(@SelectedViews, ',')
                        )
              ))
SELECT [Views].[name] AS [ViewName],
       [Views].[object_id] AS [ViewObjectId],
       [Columns].[name] AS [Name],
       [Columns].[column_id] AS [ObjectId],
       [Types].[name] AS [DataType],
       [Columns].[collation_name] AS [Collation],
       [Columns].[is_nullable] AS [IsNullable],
       [ExtendedProperties].[value] AS [Description]
FROM [sys].[views] [Views]
    JOIN [sys].[columns] [Columns]
        ON [Views].[object_id] = [Columns].[object_id]
    JOIN [sys].[types] [Types]
        ON [Columns].[system_type_id] = [Types].[user_type_id]
    LEFT JOIN FilteredExtendedProperties [ExtendedProperties]
        ON [ExtendedProperties].[major_id] = [Columns].[object_id]
           AND [ExtendedProperties].[minor_id] = [Columns].[column_id]
WHERE [Views].[name] IN
      (
          SELECT [value] FROM STRING_SPLIT(@SelectedViews, ',')
      )
ORDER BY [Views].[name],
         [Columns].[column_id];
