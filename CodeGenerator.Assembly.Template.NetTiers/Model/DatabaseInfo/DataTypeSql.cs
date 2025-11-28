namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo
{
    public enum DataTypeSqlEnum
    {
        tinyint,
        smallint,
        intSql,
        bigint,
        bit,
        decimalSql,
        numeric,
        money,
        smallmoney,
        floatSql,
        real,
        charSql,
        varchar,
        text,
        nchar,
        nvarchar,
        ntext,
        date,
        datetime,
        datetime2,
        smalldatetime,
        time,
        datetimeoffset,
        binary,
        varbinary,
        image,
        rowversion,
        timestamp,
        uniqueidentifier,
        xml,
        sql_variant,
        hierarchyid,
        geography,
        geometry,
    }

    public class DataTypeSql
    {
        // ========================== MAPPING TABLE ==============================

        private class DataTypeInfo
        {
            public required string Sql { get; init; }
            public required string CSharpType { get; init; }
            public required string SystemType { get; init; }
            public required string DefaultValue { get; init; }
            public required string CastCode { get; init; }
            public required string DbTypeCode { get; init; }
        }

        private static readonly Dictionary<DataTypeSqlEnum, DataTypeInfo> Map =
            new()
            {
                // -------------------------- عددی --------------------------
                [DataTypeSqlEnum.tinyint] = new()
                {
                    Sql = "tinyint",
                    CSharpType = "byte",
                    SystemType = "System.Byte",
                    DefaultValue = "0",
                    CastCode = "(byte)",
                    DbTypeCode = "DbType.Byte"
                },
                [DataTypeSqlEnum.smallint] = new()
                {
                    Sql = "smallint",
                    CSharpType = "short",
                    SystemType = "System.Int16",
                    DefaultValue = "0",
                    CastCode = "(short)",
                    DbTypeCode = "DbType.Int16"
                },
                [DataTypeSqlEnum.intSql] = new()
                {
                    Sql = "int",
                    CSharpType = "int",
                    SystemType = "System.Int32",
                    DefaultValue = "0",
                    CastCode = "(int)",
                    DbTypeCode = "DbType.Int32"
                },
                [DataTypeSqlEnum.bigint] = new()
                {
                    Sql = "bigint",
                    CSharpType = "long",
                    SystemType = "System.Int64",
                    DefaultValue = "0",
                    CastCode = "(long)",
                    DbTypeCode = "DbType.Int64"
                },
                [DataTypeSqlEnum.bit] = new()
                {
                    Sql = "bit",
                    CSharpType = "bool",
                    SystemType = "System.Boolean",
                    DefaultValue = "false",
                    CastCode = "(bool)",
                    DbTypeCode = "DbType.Boolean"
                },
                [DataTypeSqlEnum.decimalSql] = new()
                {
                    Sql = "decimal",
                    CSharpType = "decimal",
                    SystemType = "System.Decimal",
                    DefaultValue = "0m",
                    CastCode = "(decimal)",
                    DbTypeCode = "DbType.Decimal"
                },
                [DataTypeSqlEnum.numeric] = new()
                {
                    Sql = "numeric",
                    CSharpType = "decimal",
                    SystemType = "System.Decimal",
                    DefaultValue = "0m",
                    CastCode = "(decimal)",
                    DbTypeCode = "DbType.Decimal"
                },
                [DataTypeSqlEnum.money] = new()
                {
                    Sql = "money",
                    CSharpType = "decimal",
                    SystemType = "System.Decimal",
                    DefaultValue = "0m",
                    CastCode = "(decimal)",
                    DbTypeCode = "DbType.Currency"
                },
                [DataTypeSqlEnum.smallmoney] = new()
                {
                    Sql = "smallmoney",
                    CSharpType = "decimal",
                    SystemType = "System.Decimal",
                    DefaultValue = "0m",
                    CastCode = "(decimal)",
                    DbTypeCode = "DbType.Currency"
                },
                [DataTypeSqlEnum.floatSql] = new()
                {
                    Sql = "float",
                    CSharpType = "double",
                    SystemType = "System.Double",
                    DefaultValue = "0d",
                    CastCode = "(double)",
                    DbTypeCode = "DbType.Double"
                },
                [DataTypeSqlEnum.real] = new()
                {
                    Sql = "real",
                    CSharpType = "float",
                    SystemType = "System.Single",
                    DefaultValue = "0f",
                    CastCode = "(float)",
                    DbTypeCode = "DbType.Single"
                },

                // -------------------------- متنی --------------------------
                [DataTypeSqlEnum.charSql] = new()
                {
                    Sql = "char",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.AnsiStringFixedLength"
                },
                [DataTypeSqlEnum.varchar] = new()
                {
                    Sql = "varchar",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.AnsiString"
                },
                [DataTypeSqlEnum.text] = new()
                {
                    Sql = "text",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.AnsiString"
                },
                [DataTypeSqlEnum.nchar] = new()
                {
                    Sql = "nchar",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.StringFixedLength"
                },
                [DataTypeSqlEnum.nvarchar] = new()
                {
                    Sql = "nvarchar",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.String"
                },
                [DataTypeSqlEnum.ntext] = new()
                {
                    Sql = "ntext",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.String"
                },
                [DataTypeSqlEnum.xml] = new()
                {
                    Sql = "xml",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.Xml"
                },

                // -------------------------- زمانی --------------------------
                [DataTypeSqlEnum.date] = new()
                {
                    Sql = "date",
                    CSharpType = "DateTime",
                    SystemType = "System.DateTime",
                    DefaultValue = "DateTime.MinValue",
                    CastCode = "(DateTime)",
                    DbTypeCode = "DbType.Date"
                },
                [DataTypeSqlEnum.datetime] = new()
                {
                    Sql = "datetime",
                    CSharpType = "DateTime",
                    SystemType = "System.DateTime",
                    DefaultValue = "DateTime.MinValue",
                    CastCode = "(DateTime)",
                    DbTypeCode = "DbType.DateTime"
                },
                [DataTypeSqlEnum.datetime2] = new()
                {
                    Sql = "datetime2",
                    CSharpType = "DateTime",
                    SystemType = "System.DateTime",
                    DefaultValue = "DateTime.MinValue",
                    CastCode = "(DateTime)",
                    DbTypeCode = "DbType.DateTime2"
                },
                [DataTypeSqlEnum.smalldatetime] = new()
                {
                    Sql = "smalldatetime",
                    CSharpType = "DateTime",
                    SystemType = "System.DateTime",
                    DefaultValue = "DateTime.MinValue",
                    CastCode = "(DateTime)",
                    DbTypeCode = "DbType.DateTime"
                },
                [DataTypeSqlEnum.time] = new()
                {
                    Sql = "time",
                    CSharpType = "TimeSpan",
                    SystemType = "System.TimeSpan",
                    DefaultValue = "TimeSpan.Zero",
                    CastCode = "(TimeSpan)",
                    DbTypeCode = "DbType.Time"
                },
                [DataTypeSqlEnum.datetimeoffset] = new()
                {
                    Sql = "datetimeoffset",
                    CSharpType = "DateTimeOffset",
                    SystemType = "System.DateTimeOffset",
                    DefaultValue = "DateTimeOffset.MinValue",
                    CastCode = "(DateTimeOffset)",
                    DbTypeCode = "DbType.DateTimeOffset"
                },

                // -------------------------- باینری --------------------------
                [DataTypeSqlEnum.binary] = new()
                {
                    Sql = "binary",
                    CSharpType = "byte[]",
                    SystemType = "System.Byte[]",
                    DefaultValue = "Array.Empty<byte>()",
                    CastCode = "(byte[])",
                    DbTypeCode = "DbType.Binary"
                },
                [DataTypeSqlEnum.varbinary] = new()
                {
                    Sql = "varbinary",
                    CSharpType = "byte[]",
                    SystemType = "System.Byte[]",
                    DefaultValue = "Array.Empty<byte>()",
                    CastCode = "(byte[])",
                    DbTypeCode = "DbType.Binary"
                },
                [DataTypeSqlEnum.image] = new()
                {
                    Sql = "image",
                    CSharpType = "byte[]",
                    SystemType = "System.Byte[]",
                    DefaultValue = "Array.Empty<byte>()",
                    CastCode = "(byte[])",
                    DbTypeCode = "DbType.Binary"
                },
                [DataTypeSqlEnum.rowversion] = new()
                {
                    Sql = "rowversion",
                    CSharpType = "byte[]",
                    SystemType = "System.Byte[]",
                    DefaultValue = "Array.Empty<byte>()",
                    CastCode = "(byte[])",
                    DbTypeCode = "DbType.Binary"
                },
                [DataTypeSqlEnum.timestamp] = new()
                {
                    Sql = "timestamp",
                    CSharpType = "byte[]",
                    SystemType = "System.Byte[]",
                    DefaultValue = "Array.Empty<byte>()",
                    CastCode = "(byte[])",
                    DbTypeCode = "DbType.Binary"
                },

                // -------------------------- سایر --------------------------
                [DataTypeSqlEnum.uniqueidentifier] = new()
                {
                    Sql = "uniqueidentifier",
                    CSharpType = "Guid",
                    SystemType = "System.Guid",
                    DefaultValue = "Guid.Empty",
                    CastCode = "(Guid)",
                    DbTypeCode = "DbType.Guid"
                },
                [DataTypeSqlEnum.sql_variant] = new()
                {
                    Sql = "sql_variant",
                    CSharpType = "object",
                    SystemType = "System.Object",
                    DefaultValue = "null",
                    CastCode = "(object)",
                    DbTypeCode = "DbType.Object"
                },
                [DataTypeSqlEnum.hierarchyid] = new()
                {
                    Sql = "hierarchyid",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.String"
                },
                [DataTypeSqlEnum.geography] = new()
                {
                    Sql = "geography",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.String"
                },
                [DataTypeSqlEnum.geometry] = new()
                {
                    Sql = "geometry",
                    CSharpType = "string",
                    SystemType = "System.String",
                    DefaultValue = "string.Empty",
                    CastCode = "(string)",
                    DbTypeCode = "DbType.String"
                },
            };


        // ======================================================================

        public DataTypeSqlEnum DataTypeSqlEnum { get; private set; }

        public DataTypeSql(string sqlType)
        {
            var item = Map.FirstOrDefault(x =>
                x.Value.Sql.Equals(sqlType, StringComparison.OrdinalIgnoreCase));

            if (item.Value == null)
                throw new ArgumentException($"Unsupported SQL type: {sqlType}");

            DataTypeSqlEnum = item.Key;
        }

        public DataTypeSql(DataTypeSqlEnum type)
        {
            DataTypeSqlEnum = type;
        }

        private DataTypeInfo Info => Map[DataTypeSqlEnum];

        public string Sql => Info.Sql;
        public string CSharpType => Info.CSharpType;
        public string SystemType => Info.SystemType;
        public string DefaultValue => Info.DefaultValue;
        public string CastCode => Info.CastCode;
        public string DbTypeCode => Info.DbTypeCode;
    }
}
