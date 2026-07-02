using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column;
using System.Text.RegularExpressions;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View
{
    public class View : SchemaObject, IView
    {
        public Task<IEnumerable<IColumnView>> Columns { get => _Columns.Value; }
        private Lazy<Task<IEnumerable<IColumnView>>> _Columns { get; set; }
        public Task<IEnumerable<IStoredProcedure>> StoredProcedures { get => _StoredProcedures.Value; }
        private Lazy<Task<IEnumerable<IStoredProcedure>>> _StoredProcedures { get; set; }
        public View(string name, int objectId, string? description, Lazy<Task<IEnumerable<IColumnView>>> columns, Lazy<Task<IEnumerable<IStoredProcedure>>> storedProcedures)
            : base(name, objectId, description)
        {
            _Columns = columns;
            _StoredProcedures = storedProcedures;
        }
        public async Task<IOutputProcedure> GetOutputStoredProceduresAsync(IStoredProcedure storedProcedure)
        {
            IOutputProcedure result;
            var outputProcedures = await storedProcedure.OutputColumn;
            if (outputProcedures.Any())
            {
                var columns = (await Columns);
                var outputNameList = outputProcedures.Select(q => q.Name.ToLower());
                var columnsNameList = columns.Select(q => q.Name.ToLower());
                var exceptOutputColumns = outputNameList.Except(columnsNameList);
                if (exceptOutputColumns.Any())
                {
                    result = OutputProcedure.DataSet();
                }
                else
                {
                    result = OutputProcedure.VList(this);
                }
            }
            else
            {
                result = OutputProcedure.Void();
            }
            return result;
        }
        public string GetMethodNameStoredProcedures(IStoredProcedure storedProcedure)
        {
            return Regex.Replace(storedProcedure.Name, $"sp_{this.Name}_", string.Empty);
        }
    }
}
