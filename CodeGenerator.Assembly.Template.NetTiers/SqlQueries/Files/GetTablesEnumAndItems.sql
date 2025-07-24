
-- ابتدا جدول‌هایی که شرایط دارند را شناسایی کن
WITH TablesWithRequiredColumns AS (
    SELECT
        [Tables].[object_id] [ObjectId],
        [Tables].[name] AS [TableName],
        [Columns].[name] AS [ColumnName]
    FROM [sys].[tables]  [Tables]
    JOIN [sys].[columns]  [Columns] ON [Tables].[object_id] = [Columns].[object_id]
    WHERE [Columns].[name] IN ('Id', 'Name', 'Title')
      AND [Tables].[name] IN (SELECT [value] FROM STRING_SPLIT(@SelectedTables, ','))
),
GroupedTables AS (
    SELECT
        [TablesWithRequiredColumns].[ObjectId],
        [TablesWithRequiredColumns].[TableName]
    FROM [TablesWithRequiredColumns]
    GROUP BY [TablesWithRequiredColumns].[ObjectId], [TablesWithRequiredColumns].[TableName]
    HAVING COUNT(DISTINCT ColumnName) = 3
),
TablesWithUniqueNameIndex AS (
    SELECT
        [Indexes].[object_id]
    FROM [sys].[indexes] [Indexes]
    JOIN [sys].[index_columns] [IndexColumns] ON [Indexes].[object_id] = [IndexColumns].[object_id] AND [Indexes].[index_id] = [IndexColumns].[index_id]
    JOIN [sys].[columns] [Columns] ON [IndexColumns].[object_id] = [Columns].[object_id] AND [IndexColumns].[column_id] = [Columns].[column_id]
    WHERE [Columns].[name] = 'Name'
      AND [Indexes].[is_unique] = 1
)
-- جدول نهایی مورد نظر
SELECT 
    [Tables].[TableName],[Tables].[ObjectId]
INTO [#ValidTables]
FROM [GroupedTables] [Tables]
JOIN [TablesWithUniqueNameIndex]  ON [Tables].[ObjectId] = [TablesWithUniqueNameIndex].[object_id];

-- حالا ساخت کوئری داینامیک برای گرفتن داده‌ها
DECLARE @sql NVARCHAR(MAX) = '';
SELECT @sql += '
SELECT 
    CAST([Id] AS NVARCHAR(MAX)) AS [Id], 
    CAST([Name] AS NVARCHAR(MAX)) AS [Name], 
    CAST([Title] AS NVARCHAR(MAX)) AS [Title], 
    ''' + [#ValidTables].[TableName] + ''' AS [TableName]
	''' + [#ValidTables].[ObjectId] + ''' AS [ObjectId]
FROM [' + [#ValidTables].[TableName] + ']
UNION ALL
'
FROM #ValidTables;

-- حذف آخرین UNION ALL
SET @sql = LEFT(@sql, LEN(@sql) - LEN('UNION ALL' + CHAR(10)));

-- اجرای کوئری نهایی
EXEC sp_executesql @sql;

-- پاکسازی
DROP TABLE #ValidTables;
