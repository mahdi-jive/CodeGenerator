using CodeGenerator.Assembly.Template.NetTiers.Model.Abstractions;

namespace CodeGenerator.Assembly.Template.NetTiers.SqlQueries.Model
{
    public class ViewInfo : SchemaObject
    {
        public IReadOnlyCollection<ColumnViewInfo> ColumnsInfoView { get; private set; }

        public ViewInfo(IReadOnlyCollection<ColumnViewInfo> columnsInfoView)
        {
            ColumnsInfoView = columnsInfoView;
        }
    }
}
