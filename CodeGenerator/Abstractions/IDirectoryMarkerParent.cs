using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.Infrastructure;

namespace CodeGenerator.Abstractions
{
    /// ۱) یک interfaceِ کمکی که هر “والدِ” معتبر باید آن را پیاده‌سازی کند
    public interface IDirectoryMarkerParent
    {
        Task Generate(IDirectory directory, IAssembly assembly, ITemplateRenderer Renderer, IContextModel contextModel);
        Task SaveFileAsync(IDirectory directory, IAssembly assembly, ICodeFile codeFile);
    }
}
