using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Column
{
    public interface IColumn : ISchemaObject
    {
        public string TableName { get; }
        public int TableObjectId { get; }
        public string DataType { get; }
        public short MaxLength { get; }
        public bool IsNullable { get; }
    }


}
