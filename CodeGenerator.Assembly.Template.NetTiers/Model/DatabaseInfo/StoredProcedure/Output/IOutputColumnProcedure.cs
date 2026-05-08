namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output
{
    public interface IOutputColumnProcedure
    {
        public int? MaxLength { get; }
        public string Name { get; }
        public string DataType { get; }
        public bool IsNullable { get; }
    }
}
