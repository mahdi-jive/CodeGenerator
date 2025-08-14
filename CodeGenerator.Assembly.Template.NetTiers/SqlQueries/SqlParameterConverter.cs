using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Parameter;
using Microsoft.Data.SqlClient;
using System.Data;

public static class SqlParameterConverter
{
    public static SqlParameter ToSqlParameter(this IParameterProcedure paramInfo)
    {
        var sqlParam = new SqlParameter
        {
            ParameterName = paramInfo.Name,
            Direction = paramInfo.IsOutput ? ParameterDirection.Output : ParameterDirection.Input
        };

        if (paramInfo.IsTableType)
        {
            sqlParam.SqlDbType = SqlDbType.Structured;
            sqlParam.TypeName = $"dbo.{paramInfo.ParameterType}";
            // اصلاح: استفاده از TypeName واقعی به جای ParameterType

        }
        else
        {
            sqlParam.SqlDbType = MapStringToSqlDbType(paramInfo.ParameterType);
            if (paramInfo.MaxLength > 0)
            {
                sqlParam.Size = paramInfo.MaxLength;
            }
        }

        return sqlParam;
    }

    private static SqlDbType MapStringToSqlDbType(string typeName)
    {
        // نگاشت اولیه معمول، کامل‌تر می‌تونی بسازی
        return typeName.ToLower() switch
        {
            "int" => SqlDbType.Int,
            "bigint" => SqlDbType.BigInt,
            "smallint" => SqlDbType.SmallInt,
            "tinyint" => SqlDbType.TinyInt,
            "bit" => SqlDbType.Bit,
            "nvarchar" => SqlDbType.NVarChar,
            "varchar" => SqlDbType.VarChar,
            "char" => SqlDbType.Char,
            "nchar" => SqlDbType.NChar,
            "text" => SqlDbType.Text,
            "ntext" => SqlDbType.NText,
            "datetime" => SqlDbType.DateTime,
            "smalldatetime" => SqlDbType.SmallDateTime,
            "date" => SqlDbType.Date,
            "time" => SqlDbType.Time,
            "decimal" => SqlDbType.Decimal,
            "numeric" => SqlDbType.Decimal,
            "float" => SqlDbType.Float,
            "real" => SqlDbType.Real,
            "uniqueidentifier" => SqlDbType.UniqueIdentifier,
            "xml" => SqlDbType.Xml,
            "varbinary" => SqlDbType.VarBinary,
            "binary" => SqlDbType.Binary,
            _ => SqlDbType.VarChar // پیش فرض به عنوان رشته
        };
    }
}