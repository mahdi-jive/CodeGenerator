namespace CodeGenerator.Abstractions
{
    public interface ICodeGenerationManager
    {
        Task GenerateAllAsync();
        IReadOnlyCollection<IGenerator> Generators { get; }
    }
}
