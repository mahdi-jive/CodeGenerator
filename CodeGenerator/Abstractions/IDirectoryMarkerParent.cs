using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator.Abstractions
{
    /// ۱) یک interfaceِ کمکی که هر “والدِ” معتبر باید آن را پیاده‌سازی کند
    public interface IDirectoryMarkerParent
    {
        Task Generate(IDirectory directory, IAssembly assembly, IContextModel contextModel);
        Task SaveFileAsync(IDirectory directory, IAssembly assembly, ICodeFile codeFile);
    }
}
