namespace CodeGenerator.Abstractions
{
    public interface ITemplateContext<TContextModel> where TContextModel : IContextModel
    {
        TContextModel GetContextModel();
    }
}
