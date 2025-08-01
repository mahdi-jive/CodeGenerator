using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public class DatabaseInfoModel : IDatabaseInfoModel
    {
        public DatabaseInfoModel(IReadOnlyCollection<ITable> tables, IReadOnlyCollection<IView> views, IReadOnlyCollection<ITableEnum> tableEnums, IReadOnlyCollection<IStoredProcedure> storedProcedures)
        {
            Tables = tables;
            Views = views;
            TableEnums = tableEnums;
            StoredProcedures = storedProcedures;
        }

        public IReadOnlyCollection<ITable> Tables { get; private set; }

        public IReadOnlyCollection<IView> Views { get; private set; }

        public IReadOnlyCollection<ITableEnum> TableEnums { get; private set; }

        public IReadOnlyCollection<IStoredProcedure> StoredProcedures { get; private set; }
    }
}
