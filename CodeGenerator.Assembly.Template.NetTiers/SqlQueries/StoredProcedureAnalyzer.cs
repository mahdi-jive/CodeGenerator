using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries
{
    public class StoredProcedureAnalyzer
    {
        private readonly SqlConnection _connection;

        public StoredProcedureAnalyzer(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<OutputProcedure> AnalyzeAsync(string spText, List<SqlParameter> parameters)
        {
            string preparedText = PrepareText(spText, parameters);
            return ExtractOutputSchema(preparedText, parameters);
        }
        public async Task<List<OutputProcedure>> AnalyzeBasicAsync(string storedProcedureName, List<SqlParameter> parameters)
        {
            var output = new List<OutputProcedure>();
            Console.WriteLine(storedProcedureName);
            try
            {
                _connection.Open();
                using var transaction = _connection.BeginTransaction();

                string paramList = string.Join(", ", parameters
                    .Where(parameter => parameter.Direction != ParameterDirection.Output
                                        && parameter.Direction != ParameterDirection.InputOutput
                                        && parameter.SqlDbType != SqlDbType.Structured)
                    .Select(p => $"{SetParameter(p)}"));

                var paramOutputList = parameters
                    .Where(parameter => parameter.Direction == ParameterDirection.Output
                                        || parameter.Direction == ParameterDirection.InputOutput);
                string paramOutputVariableList = string.Join("\n", paramOutputList
                    .Select(p => $"DECLARE {SetParameter(p, true)}"));
                var paramOutputValueList = string.Join(',', paramOutputList
                                             .Select(p => $"{p.ParameterName} = {p.ParameterName}"));

                string sql =
                    $@"{paramOutputVariableList}  EXEC [{storedProcedureName}] {paramList}
{(paramOutputList.Any() ? "," + paramOutputValueList : "")}";

                //using (var cmd = _connection.CreateCommand())
                //{
                //    cmd.Transaction = transaction;
                //    cmd.CommandText = sql;
                //    cmd.ExecuteNonQuery(); // اجرای اولیه برای ایجاد جدول موقت و ...
                //}

                using (var schemaCmd = _connection.CreateCommand())
                {
                    schemaCmd.Transaction = transaction;
                    schemaCmd.CommandText = sql;
                    using var reader = await schemaCmd.ExecuteReaderAsync();

                    var schemaTable = reader.GetSchemaTable();
                    if (schemaTable != null)
                    {
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string name = row["ColumnName"].ToString()!;
                            string dataType = row["DataTypeName"].ToString()!;
                            bool isNullable = (bool)row["AllowDBNull"];
                            int maxLength = Convert.ToInt32(row["ColumnSize"]);
                            output.Add(new OutputProcedure(name, dataType, isNullable, maxLength));
                        }
                    }
                }

                transaction.Rollback(); // 🔁 در انتها همه تغییرات را لغو کن
            }
            catch (Exception ex)
            {
                // اختیاری: لاگ یا مدیریت خطا
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return output;

        }

        private string PrepareText(string text, List<SqlParameter> parameters)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(text))
            {
                text = text.Replace("\r\n", "\n");

                // حذف Header: تا خط AS
                var matchHeader = Regex.Match(text, @"(?im)^AS\s*$");
                if (matchHeader.Success)
                {
                    text = text.Substring(matchHeader.Index + matchHeader.Length).Trim();
                }

                // مقداردهی اولیه به پارامترها (DECLARE @param TYPE → DECLARE @param TYPE = NULL)
                text = Regex.Replace(
                    text,
                    @"(?i)\bDECLARE\s+(@\w+)\s+([\w\[\]\.]+)(\s*;)?",
                    m => $"{m.Groups[1].Value} {m.Groups[2].Value} = NULL",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase
                );

                // حذف آخرین END که بیرون از CASE باشد
                var endMatches = Regex.Matches(text, @"(?i)^\s*END\s*$", RegexOptions.Multiline);
                if (endMatches.Count > 0)
                {
                    var lastEnd = endMatches[endMatches.Count - 1];
                    text = text.Remove(lastEnd.Index, lastEnd.Length).Trim();
                }

                // باز کردن IF (...) BEGIN ... END
                text = Regex.Replace(
                    text,
                    @"(?is)\bIF\s*\([^\)]*\)\s*BEGIN\s*(.*?)\s*END\s*",
                    m => $"{m.Groups[1].Value.Trim()}\n",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase
                );

                // باز کردن ELSE BEGIN ... END
                text = Regex.Replace(
                    text,
                    @"(?is)\bELSE\s*BEGIN\s*(.*?)\s*END\s*",
                    m => $"{m.Groups[1].Value.Trim()}\n",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase
                );

                // حذف BEGIN/END غیرمربوط به CASE WHEN
                text = Regex.Replace(
                    text,
                    @"(?im)^\s*BEGIN\s*$",
                    "",
                    RegexOptions.Multiline
                );
                text = Regex.Replace(
                    text,
                    @"(?im)^\s*END\s*$",
                    m =>
                    {
                        // اگر خط قبلی شامل CASE باشد، این END را نگه دار
                        var lines = text.Substring(0, m.Index).Split('\n');
                        if (lines.Length > 0 && lines.Last().IndexOf("CASE", StringComparison.OrdinalIgnoreCase) >= 0)
                            return m.Value;
                        return "";
                    },
                    RegexOptions.Multiline
                );

                // حذف RETURNها
                text = Regex.Replace(
                    text,
                    @"(?im)^\s*RETURN\s+(@\w+)?\s*;?\s*$",
                    "",
                    RegexOptions.Multiline
                );

                // SELECT DISTINCT بدون TOP → SELECT DISTINCT TOP 1
                text = Regex.Replace(
                    text,
                    @"(?i)\bSELECT\s+DISTINCT\b(?!\s+TOP\s+\d+)",
                    "SELECT DISTINCT TOP 1"
                );

                // SELECT ساده → SELECT TOP 1 (اگر بعدش TOP یا DISTINCT یا مقداردهی نباشد)
                text = Regex.Replace(
                    text,
                    @"(?i)(?<=\bSELECT\s)(?!TOP\b|DISTINCT\b|@\w+\s*=|\s*@\w+\s*=)",
                    "TOP 1 ",
                    RegexOptions.IgnoreCase
                );
            }

            // مقداردهی به پارامترها
            foreach (var parameter in parameters)
            {
                sb.AppendLine($"DECLARE {SetParameter(parameter)}");

            }

            sb.AppendLine(text);
            return sb.ToString();
        }
        private string SetParameter(SqlParameter parameter, bool withType = false)
        {
            var sb = new StringBuilder();
            string type;
            string value;

            if (parameter.SqlDbType == SqlDbType.Structured)
            {
                type = parameter.TypeName;
                value = parameter.Value switch
                {
                    null => "",
                    _ => $"'{parameter.Value}'"
                };
            }
            else
            {
                type = withType ? parameter.SqlDbType.ToString() : "";
                value = parameter.Value switch
                {
                    null => "NULL",
                    string s => $"N'{s.Replace("'", "''")}'",
                    int or long or short => parameter.Value.ToString()!,
                    bool b => b ? "1" : "0",
                    _ => $"'{parameter.Value}'"
                };
            }

            value = string.IsNullOrWhiteSpace(value) ? "" : $" = {value}";
            sb.AppendLine($" {parameter.ParameterName} {type} {value} ");

            return sb.ToString();
        }
        private List<OutputProcedure> ExtractOutputSchema(string sqlText, List<SqlParameter> parameters)
        {
            var output = new List<OutputProcedure>();

            try
            {
                _connection.Open();
                using var transaction = _connection.BeginTransaction();

                using (var cmd = _connection.CreateCommand())
                {
                    cmd.Transaction = transaction;
                    cmd.CommandText = sqlText;
                    cmd.ExecuteNonQuery(); // اجرای اولیه برای ایجاد جدول موقت و ...
                }

                using (var schemaCmd = _connection.CreateCommand())
                {
                    schemaCmd.Transaction = transaction;
                    schemaCmd.CommandText = "SET FMTONLY ON; " + sqlText + " SET FMTONLY OFF;";
                    using var reader = schemaCmd.ExecuteReader(CommandBehavior.SchemaOnly);

                    var schemaTable = reader.GetSchemaTable();
                    if (schemaTable != null)
                    {
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string name = row["ColumnName"].ToString()!;
                            string dataType = row["DataTypeName"].ToString()!;
                            bool isNullable = (bool)row["AllowDBNull"];
                            int maxLength = Convert.ToInt32(row["ColumnSize"]);
                            output.Add(new OutputProcedure(name, dataType, isNullable, maxLength));
                        }
                    }
                }

                transaction.Rollback(); // 🔁 در انتها همه تغییرات را لغو کن
            }
            catch (Exception ex)
            {
                // اختیاری: لاگ یا مدیریت خطا
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return output;
        }
    }
}
