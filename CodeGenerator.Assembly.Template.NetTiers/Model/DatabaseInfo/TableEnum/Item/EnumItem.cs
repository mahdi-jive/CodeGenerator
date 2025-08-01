namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item
{
    public class EnumItem : IEnumItem
    {
        public EnumItem(int id, string name, string title)
        {
            Id = id;
            Name = name;
            Title = title;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
