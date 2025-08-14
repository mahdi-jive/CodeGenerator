using CodeGenerator.Assembly.Template.NetTiers.Configuration;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;

namespace CodeGenerator.Assembly.Template.NetTiers
{
    public class DataLayerGeneratorFactory
    {
        private static string connectionString = @"Data Source=DESKTOP-O7IL2PH;Initial Catalog=BehsazTestLastVersionMain;User ID=sa;Password=123;TrustServerCertificate=True;";

        public static DatabaseInfoModel Generate(IUserConfiguration userConfiguration)
        {
            var userConfigurationTest = new UserConfiguration(connectionString, "sp_", "Rapco", "www.Rapco-rpk.com", "Behsaz", selectedTables.ToList(), selectedTables.ToList(), selectedTables.ToList());

            var databaseInformation = GetDataInfo(userConfigurationTest);
            return databaseInformation;
        }
        private static DatabaseInfoModel GetDataInfo(IUserConfiguration userConfiguration)
        {

            var databaseModelBuilder = new DatabaseModelBuilder(userConfiguration);
            var databaseInfoModel = databaseModelBuilder
                .LoadTable()
                .LoadViews()
                .LoadTableEnums()
                .Build();
            return databaseInfoModel;
        }




        private static List<string> selectedTables = new List<string>()
            {
"TAccYear",
"TBASActivity",
"TBASActivityDataType",
"TBASAnalysisZone",
"TBASApplicantType",
"TBASArea",
"TBASBank",
"TBASBankDetail",
"TBASBlock",
"TBASBlockShape",
"TBASBookletBusiness",
"TBASBookletRules",
"TBASBookletState",
"TBASBookletStep",
"TBASBuildingDirection",
"TBASBuildingGroup",
"TBASBuildingProperty",
"TBASBuildingPropertyOptions",
"TBASBuildingPropertyState",
"TBASBuildingState",
"TBASBuildingUsage",
"TBASBusiness",
"TBASBusinessCycle",
"TBASBusinessFactor",
"TBASBusinessGroup"
            };
    }
}
