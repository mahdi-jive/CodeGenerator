using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum
{
    public interface ITableEnum : ISchemaObject
    {
        public Task<IEnumerable<IEnumItem>> Items { get; }

    }


}
