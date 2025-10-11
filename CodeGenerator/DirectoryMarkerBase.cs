using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;
using CodeGenerator.FileSystem.Abstractions;
using CodeGenerator.Infrastructure;

namespace CodeGenerator
{
    public abstract class DirectoryMarkerBase<TDirectoryParent, TContextModel> : IDirectoryMarker<TDirectoryParent>
        where TDirectoryParent : IDirectoryMarkerParent, new()
        where TContextModel : IContextModel
    {

        public async Task Generate(IDirectory directory, IAssembly assembly, ITemplateRenderer renderer, IContextModel contextModel)
        {

            var childFolderTypes = TypeFinder.FindDerivedMarkerTypes(this.GetType(), typeof(TContextModel));

            foreach (var folderType in childFolderTypes)
            {
                if (Activator.CreateInstance(folderType) is IDirectoryMarkerParent folderInstance)
                {
                    var subDirectory = await directory.AddDirectoryAsync(folderType.Name);
                    await folderInstance.Generate(subDirectory, assembly, renderer, (TContextModel)contextModel);
                }
            }
            var templates = TypeFinder.FindFileMarkerTypes(this.GetType(), typeof(TContextModel));

            foreach (var template in templates)
            {
                if (Activator.CreateInstance(template) is ICodeTemplateBase fileInstance)
                {
                    foreach (var file in await fileInstance.Generate(renderer, (TContextModel)contextModel))
                    {
                        await SaveFileAsync(directory, assembly, file);
                    }
                }

            }
        }

        public abstract Task SaveFileAsync(IDirectory directory, IAssembly assembly, ICodeFile codeFile);
        private static IEnumerable<ICodeTemplate<TDirectoryParent, TContextModel>> GetTemplatesFor()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICodeTemplate<TDirectoryParent, TContextModel>) &&
                    i.GetGenericArguments()[0] == typeof(TDirectoryParent)))
                .Select(t => (ICodeTemplate<TDirectoryParent, TContextModel>)Activator.CreateInstance(t));
        }
    }
    public static class TypeFinder
    {
        public static IEnumerable<Type> FindDerivedMarkerTypes(Type tGenericArg1, Type tGenericArg2)
        {
            var excludeType = tGenericArg1;
            var targetGenericDefinition = typeof(DirectoryMarkerBase<,>);

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t != excludeType)
                .Where(t => InheritsFromTargetGeneric(t, targetGenericDefinition, tGenericArg1, tGenericArg2));
        }
        public static IEnumerable<Type> FindFileMarkerTypes(Type tGenericArg1, Type tGenericArg2)
        {
            var excludeType = tGenericArg1;
            var expectedInterface = typeof(ICodeTemplate<,>).MakeGenericType(tGenericArg1, tGenericArg2);

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t != excludeType)
                .Where(t => t.GetInterfaces().Any(i => i == expectedInterface));
        }

        private static bool InheritsFromTargetGeneric(Type type, Type genericBaseDefinition, Type arg1, Type arg2)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType &&
                    type.GetGenericTypeDefinition() == genericBaseDefinition)
                {
                    var args = type.GetGenericArguments();
                    if (args.Length == 2 && args[0] == arg1 && args[1] == arg2)
                        return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        private static IEnumerable<Type> SafeGetTypes(System.Reflection.Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }
        }
    }
}
