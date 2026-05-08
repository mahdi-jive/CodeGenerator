namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure
{
    public interface IOutputProcedure
    {
        public bool IsEnumerable { get; }
        public ISchemaObject Table { get; }
        public OutputProcedureState State { get; }
        public string OutputString { get; }

    }
}
