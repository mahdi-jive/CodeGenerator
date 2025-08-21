using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum
{
    public class TablesEnum : SchemaObject, ITableEnum
    {
        public Task<IEnumerable<IEnumItem>> Items { get => _Items.Value; }
        private Lazy<Task<IEnumerable<IEnumItem>>> _Items { get; set; }

        public TablesEnum(string name, int objectId, string? description, Lazy<Task<IEnumerable<IEnumItem>>> items)
            : base(name, objectId, description)
        {
            _Items = items;
        }
    }
}
