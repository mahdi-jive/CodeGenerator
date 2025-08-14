using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.FileSystem.Abstractions;

namespace CodeGenerator.Assembly.NetFx48.Extensions
{
    public static class AssemblyExtensions
    {
        //public static async Task<IAssembly> CreateAssemblyNetFx48Async(
        //    this IDirectory parent,
        //    IMediator mediator,
        //    string assemblyName,
        //    string targetFramework = "net48")
        //{
        //    var directory = await parent.AddDirectoryAsync(assemblyName);
        //    var project = await AssemblyProject.CreateAsync(mediator, directory, assemblyName);
        //    return project;
        //}
        public static async Task AddSourceFileAsync(this IDirectory directory, IAssembly assembly, ICodeFile codeFile)
        {
            //if (!assembly.Directory.IsPathInFolder(directory))
            //{
            //    throw new Exception($"Folder Path :{directory.FullPath} Not In Path {assembly.Directory.FullPath}");
            //}
            // ساخت فضای نام پایه: AssemblyName + فولدرهای زیرین (اگر بود)
            //string namespaceName = assembly.AssemblyName;

            // اصلاح فضای نام در CompilationUnitSyntax (فرض بر این است که فضای نام در AST باید جایگزین شود)
            //var newNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(namespaceName))
            //    .WithMembers(codeFile.CompilationUnit.Members)
            //    .NormalizeWhitespace();

            //var newCompilationUnit = SyntaxFactory.CompilationUnit()
            //    .AddMembers(newNamespace)
            //    .NormalizeWhitespace();

            string code = codeFile.CompilationUnit.ToFullString();

            // مسیر فایل در سیستم فایل:

            string fileName = codeFile.FileName;
            var file = await directory.AddFileAsync(fileName, code);
        }
        public static bool IsPathInFolder(this IFullPath folderPath, IFullPath pathInFolder)
        {
            // مسیر فولدر رو به مسیر کامل و استاندارد تبدیل می‌کنیم
            string folderFullPath = Path.GetFullPath(folderPath.FullPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;

            // مسیر فایل رو به مسیر کامل و استاندارد تبدیل می‌کنیم
            string fileFullPath = Path.GetFullPath(pathInFolder.FullPath);

            // بررسی می‌کنیم که آیا مسیر فایل با مسیر فولدر شروع می‌شود یا نه
            return fileFullPath.StartsWith(folderFullPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}
