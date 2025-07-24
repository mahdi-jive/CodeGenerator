using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;
using CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.DataEnumItem;

namespace CodeGenerator.Assembly.Template.NetTiers.Model.DatabaseInfo.TableEnum
{
    public interface ITableEnum : ISchemaObject
    {

        IDataEnumItemCollection DataEnumItems { get; }

    }


}
