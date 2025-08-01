namespace CodeGenerator.Assembly.Template.NetTiers.Configuration
{
    public interface IUserConfiguration
    {
        string ConnectionString { get; }
        string CustomProcedureStartsWith { get; }
        string CompanyName { get; }
        string CompanyURL { get; }
        string RootNameSpace { get; }
        IReadOnlyCollection<string> SelectedTables { get; }
        IReadOnlyCollection<string> SelectedTableEnums { get; }
        IReadOnlyCollection<string> SelectedViews { get; }

    }
}
