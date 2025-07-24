using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class TablesEnumInfo : SchemaObject
    {
        public IReadOnlyCollection<EnumItemsInfo> EnumItemsInfos { get; set; }
    }
}
