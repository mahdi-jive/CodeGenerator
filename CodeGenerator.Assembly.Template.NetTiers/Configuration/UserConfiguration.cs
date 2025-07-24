namespace CodeGenerator.Assembly.Template.NetTiers.Configuration
{
    public class UserConfiguration : IUserConfiguration
    {
        public IReadOnlyCollection<string> SelectedTables { get; private set; }
        public string ConnectionString { get; private set; }


        public UserConfiguration(IReadOnlyCollection<string> selectedTables, string connectionString)
        {
            SelectedTables = selectedTables;
            ConnectionString = connectionString;
        }

        public string StartCustomSp { get; private set; }



        public IReadOnlyCollection<string> SelectedTableEnums { get; private set; }

        public IReadOnlyCollection<string> SelectedViews { get; private set; }
    }
}
