using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum
{
    public class TablesEnum : SchemaObject, ITableEnum
    {
        public IReadOnlyCollection<IEnumItem> Items { get; private set; }

        public TablesEnum(string name, int objectId, string? description, IReadOnlyCollection<IEnumItem> items)
            : base(name, objectId, description)
        {
            Items = items;
        }
    }
}
