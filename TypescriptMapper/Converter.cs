namespace TypescriptMapper;

public class Converter
{
    internal static string RemoveGenericSymbol(string name) => name.Split("`")[0];
    
    internal static string MapType(Type type)
    {
        if (type.IsNumeric()) return "number";

        if (type.IsString()) return "string";

        if (type.IsObject()) return "any";

        if (type.IsGenericType && type.Name is "Nullable`1")
        {
            return $"{MapType(type.GetGenericArguments().FirstOrDefault() ?? typeof(object))} | null";
        }

        if (type.IsGenericType)
        {
            var genericTypes = type.GetGenericArguments().Select(x => MapType(x));
            return $"{RemoveGenericSymbol(type.Name)}<{string.Join(", ", genericTypes)}>";
        }

        if (type.IsArray())
        {
            var arrayType = type.GetArrayType();
            return arrayType is null ? "any[]" : $"{MapType(arrayType)}[]";
        }

        if (type.IsDate()) return "Date";
        
        if (type.IsClass || type.IsInterface) return type.Name;

        throw new TypeMapException();;
    }
}