using CodeGenerator.Abstractions;
using CodeGenerator.Assembly.Abstractions;

namespace CodeGenerator
{
    public abstract class GeneratorBase<T> : IGenerator where T : IGenerator
    {
        public IAssembly Assembly { get; private set; }

        protected GeneratorBase(IAssembly assembly)
        {
            Assembly = assembly;
        }

        public IEnumerable<ICodeFile> Generate()
        {
            var templates = GetTemplatesFor();

            foreach (var template in templates)
            {
                foreach (var file in template.Generate())
                {
                    yield return file;
                }
            }
        }
        public abstract Task SaveFile();
        private static IEnumerable<ICodeTemplate<T>> GetTemplatesFor()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(ICodeTemplate<>) &&
                    i.GetGenericArguments()[0] == typeof(T)))
                .Select(t => (ICodeTemplate<T>)Activator.CreateInstance(t));
        }
    }

}
