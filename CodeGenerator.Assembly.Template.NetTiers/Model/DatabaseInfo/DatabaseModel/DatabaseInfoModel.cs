using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Tables;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public class DatabaseInfoModel : IDatabaseInfoModel
    {
        public DatabaseInfoModel(ITableCollection tables, IViewCollection views, ITableEnumCollection tableEnums)
        {
            Tables = tables;
            Views = views;
            TableEnums = tableEnums;
        }

        public ITableCollection Tables { get; private set; }

        public IViewCollection Views { get; private set; }

        public ITableEnumCollection TableEnums { get; private set; }

        public IStoredProcedureCollection StoredProcedures { get; private set; }

        public IReadOnlyCollection<string> SelectedTables => throw new NotImplementedException();

        public IReadOnlyCollection<string> SelectedViews => throw new NotImplementedException();

        public IReadOnlyCollection<string> SelectedTableEnums => throw new NotImplementedException();
    }
}
