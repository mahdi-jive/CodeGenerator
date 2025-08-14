using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.Table;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.View;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DatabaseModel
{
    public class DatabaseInfoModel : IDatabaseInfoModel
    {
        public DatabaseInfoModel(
            string customProcedureStartsWith,
            string companyName,
            string companyURL,
            string rootNameSpace,
            Lazy<IEnumerable<ITable>> tables,
            Lazy<IEnumerable<IView>> views,
            Lazy<IEnumerable<ITableEnum>> tableEnums)
        {
            CustomProcedureStartsWith = customProcedureStartsWith;
            CompanyName = companyName;
            CompanyURL = companyURL;
            RootNameSpace = rootNameSpace;
            _Tables = tables;
            _Views = views;
            _TableEnums = tableEnums;
        }
        private Lazy<IEnumerable<ITable>> _Tables { get; set; }
        private Lazy<IEnumerable<IView>> _Views { get; set; }
        private Lazy<IEnumerable<ITableEnum>> _TableEnums { get; set; }

        public string CustomProcedureStartsWith { get; private set; }
        public string CompanyName { get; private set; }
        public string CompanyURL { get; private set; }
        public string RootNameSpace { get; private set; }
        public IEnumerable<ITable> Tables => _Tables.Value;
        public IEnumerable<IView> Views => _Views.Value;
        public IEnumerable<ITableEnum> TableEnums => _TableEnums.Value;
    }
}
