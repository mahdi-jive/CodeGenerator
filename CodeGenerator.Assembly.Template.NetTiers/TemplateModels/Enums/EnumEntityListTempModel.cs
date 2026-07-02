using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum.Item;
using CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.TemplateModels.Enums
{
    public class EnumEntityListTempModel : ITemplateModel
    {
        public static async Task<EnumEntityListTempModel> Create(ITableEnum tableEnum)
        {
            var enumEntityList = new EnumEntityListTempModel(tableEnum);
            enumEntityList.Items = await tableEnum.Items;
            return enumEntityList;
        }
        private EnumEntityListTempModel(ITableEnum tableEnum)
        {
            TableEnum = tableEnum;
        }

        public ITableEnum TableEnum { get; private set; }
        public IEnumerable<IEnumItem> Items { get; private set; }

    }
}
