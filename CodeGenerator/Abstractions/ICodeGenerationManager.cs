namespace CodeGenerator.Abstractions
{
    public interface ICodeGenerationManager
    {
        Task GenerateAllAsync();
        System.Reflection.Assembly Assembly { get; }
    }
}
