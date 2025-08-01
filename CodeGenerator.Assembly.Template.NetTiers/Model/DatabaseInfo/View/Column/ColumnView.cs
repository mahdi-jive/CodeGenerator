namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View.Column
{
    public class ColumnView : SchemaObject, IColumnView
    {
        public ColumnView(string name, int objectId, string? description, string viewName, int viewObjectId, string dataType, string? collation, bool isNullable)
            : base(name, objectId, description)
        {
            ViewName = viewName;
            ViewObjectId = viewObjectId;
            DataType = dataType;
            Collation = collation;
            IsNullable = isNullable;
        }

        public string ViewName { get; private set; } = null!;
        public int ViewObjectId { get; private set; }
        public string DataType { get; private set; } = null!;
        public string? Collation { get; private set; }
        public bool IsNullable { get; private set; }
    }
}
