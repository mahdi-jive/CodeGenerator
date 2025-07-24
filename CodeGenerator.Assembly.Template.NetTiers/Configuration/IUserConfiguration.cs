namespace CodeGenerator.Assembly.Template.NetTiers.Configuration
{
    public interface IUserConfiguration
    {
        public string ConnectionString { get; }
        IReadOnlyCollection<string> SelectedTables { get; }
        IReadOnlyCollection<string> SelectedTableEnums { get; }
        IReadOnlyCollection<string> SelectedViews { get; }
        string StartCustomSp { get; }
    }
}
