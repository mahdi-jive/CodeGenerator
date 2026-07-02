namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table.StoredProcedure
{
    internal class OutputProcedure : IOutputProcedure
    {
        public bool IsEnumerable { get; private set; }
        public ISchemaObject Table { get; private set; }
        public OutputProcedureState State { get; private set; }
        public string OutputString
        {
            get
            {
                var result = "void";
                switch (State)
                {
                    case OutputProcedureState.Void:
                        result = "void";
                        break;
                    case OutputProcedureState.DataSet:
                        result = "DataSet";
                        break;
                    case OutputProcedureState.Entity:
                        result = Table.NamePascal;
                        break;
                    case OutputProcedureState.Enumerable:
                        result = $"IEnumerable<{Table.NamePascal}>";
                        break;
                    case OutputProcedureState.TList:
                        result = $"TList<{Table.NamePascal}>";
                        break;
                    case OutputProcedureState.VList:
                        result = $"VList<{Table.NamePascal}>";
                        break;
                }
                return result;
            }
        }
        public static IOutputProcedure Enumerable(ISchemaObject table)
        {
            return new OutputProcedure(OutputProcedureState.Enumerable, table, true);
        }
        public static IOutputProcedure TList(ISchemaObject table)
        {
            return new OutputProcedure(OutputProcedureState.TList, table, true);
        }
        public static IOutputProcedure VList(ISchemaObject table)
        {
            return new OutputProcedure(OutputProcedureState.VList, table, true);
        }
        public static IOutputProcedure Entity(ISchemaObject table)
        {
            return new OutputProcedure(OutputProcedureState.Entity, table, false);
        }
        public static IOutputProcedure Void()
        {
            return new OutputProcedure(OutputProcedureState.Void);
        }
        public static IOutputProcedure DataSet()
        {
            return new OutputProcedure(OutputProcedureState.DataSet);
        }
        private OutputProcedure(OutputProcedureState state)
        {
            State = state;
            IsEnumerable = false;
        }
        private OutputProcedure(OutputProcedureState state, ISchemaObject table, bool isEnumerable)
        {
            State = state;
            IsEnumerable = isEnumerable;
            Table = table;
        }
    }
}
