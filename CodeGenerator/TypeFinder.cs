namespace CodeGenerator
{
    public static class TypeFinder
    {

        public static IEnumerable<Type> FindWithGenericTypes(System.Reflection.Assembly assembly, Type tGeneric, params Type[] arguments)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => InheritsFromTargetGeneric(t, tGeneric, arguments));
        }
        public static IEnumerable<Type> FindWithTypes(System.Reflection.Assembly assembly, Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));


            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(SafeGetTypes)
                .Where(t => t != null && t.IsClass && !t.IsAbstract)
                .Where(t => type.IsAssignableFrom(t) && t != type);
        }
        private static bool InheritsFromTargetGeneric(Type type, Type genericBaseDefinition, params Type[] arguments)
        {
            // بررسی BaseTypeها
            var currentType = type;
            while (currentType != null && currentType != typeof(object))
            {
                if (currentType.IsGenericType &&
                    currentType.GetGenericTypeDefinition() == genericBaseDefinition)
                {
                    var args = currentType.GetGenericArguments();
                    if (args.Length == arguments.Length && args.SequenceEqual(arguments))
                        return true;
                }
                currentType = currentType.BaseType;
            }

            // بررسی Interfaceها
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == genericBaseDefinition)
                {
                    var args = interfaceType.GetGenericArguments();
                    if (args.Length == arguments.Length && args.SequenceEqual(arguments))
                        return true;
                }
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
