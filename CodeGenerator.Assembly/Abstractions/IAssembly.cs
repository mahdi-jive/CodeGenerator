using CodeGenerator.FileSystem.Abstractions;
using System.Xml.Linq;

namespace CodeGenerator.Assembly.Abstractions
{
    public interface IAssembly
    {
        public IDirectory Directory { get; }
        public string AssemblyName { get; }
        public XDocument CsprojContent { get; }
        public IFile CsprojFile { get; }
    }
}
