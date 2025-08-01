namespace CodeGenerator.Assembly.Template.NetTiers.Configuration
{
    public class UserConfiguration : IUserConfiguration
    {
        public UserConfiguration(string customProcedureStartsWith, string companyName, string companyURL, string rootNameSpace, IReadOnlyCollection<string> selectedTables, IReadOnlyCollection<string> selectedTableEnums, IReadOnlyCollection<string> selectedViews)
        {
            CustomProcedureStartsWith = customProcedureStartsWith;
            CompanyName = companyName;
            CompanyURL = companyURL;
            RootNameSpace = rootNameSpace;
            SelectedTables = selectedTables;
            SelectedTableEnums = selectedTableEnums;
            SelectedViews = selectedViews;
        }

        public string ConnectionString { get; private set; }
        public string CustomProcedureStartsWith { get; private set; }
        public string CompanyName { get; private set; }
        public string CompanyURL { get; private set; }
        public string RootNameSpace { get; private set; }
        public IReadOnlyCollection<string> SelectedTables { get; private set; }

        public IReadOnlyCollection<string> SelectedTableEnums { get; private set; }

        public IReadOnlyCollection<string> SelectedViews { get; private set; }
    }
}
