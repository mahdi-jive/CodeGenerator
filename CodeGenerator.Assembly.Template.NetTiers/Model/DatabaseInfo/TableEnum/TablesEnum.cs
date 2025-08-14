using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum
{
    public class TablesEnum : SchemaObject, ITableEnum
    {
        public IEnumerable<IEnumItem> Items { get => _Items.Value; }
        private Lazy<IEnumerable<IEnumItem>> _Items { get; set; }

        public TablesEnum(string name, int objectId, string? description, Lazy<IEnumerable<IEnumItem>> items)
            : base(name, objectId, description)
        {
            _Items = items;
        }
    }
}
