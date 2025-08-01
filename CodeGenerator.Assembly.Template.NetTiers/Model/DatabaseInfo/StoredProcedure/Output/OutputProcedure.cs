namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.StoredProcedure.Output
{
    public class OutputProcedure : IOutputProcedure
    {
        public int? MaxLength { get; private set; }
        public string Name { get; private set; }
        public string DataType { get; private set; }
        public bool IsNullable { get; private set; }

        public OutputProcedure(string name, string dataType, bool isNullable, int? maxLength)
        {
            Name = name;
            DataType = dataType;
            IsNullable = isNullable;
            MaxLength = maxLength;
        }
    }
}
