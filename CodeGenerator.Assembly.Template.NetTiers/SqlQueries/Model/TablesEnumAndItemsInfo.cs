namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class TablesEnumAndItemsInfo : EnumItemsInfo
    {
        public string TableName { get; set; } = null!;
        public int ObjectId { get; set; }
        public string? Description { get; set; }
    }
}
