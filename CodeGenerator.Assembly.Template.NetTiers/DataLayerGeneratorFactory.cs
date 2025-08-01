﻿using CodeGenerator.Assembly.Template.NetTiers.Configuration;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel;

namespace CodeGenerator.Assembly.Template.NetTiers
{
    public class DataLayerGeneratorFactory
    {
        private DataLayerGeneratorFactory(DatabaseInfoModel databaseInformation)
        {

        }
        public DatabaseInfoModel DatabaseInfoModel { get; private set; }
        string connectionString = @"Data Source=DESKTOP-O7IL2PH;Initial Catalog=BehsazTestLastVersionMain;User ID=sa;Password=123;TrustServerCertificate=True;";

        public static void Generate(string connectionString, IEnumerable<string> selectedTables)
        {
            var userConfiguration = new UserConfiguration(selectedTables.ToList(), connectionString);


            var databaseInformation = GetDataInfo(userConfiguration);
            new DataLayerGeneratorFactory(databaseInformation);
        }
        private static DatabaseInfoModel GetDataInfo(IUserConfiguration userConfiguration)
        {

            var databaseModelBuilder = new DatabaseModelBuilder(userConfiguration);
            var databaseInfoModel = databaseModelBuilder
                .LoadStoredProcedures()
                .LoadTable()
                .LoadViews()
                .LoadTableEnums()
                .Build();
            return databaseInfoModel;
        }




        List<string> selectedTables = new List<string>()
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
