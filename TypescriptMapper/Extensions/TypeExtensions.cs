using System.Collections;

namespace TypescriptMapper.Extensions;

public static class TypeExtensions
{
    private static Type[] CollectionTypes = { typeof(IEnumerable), typeof(ICollection<>), typeof(IEnumerator) };

    public static bool IsNumeric(this Type type)
    {
        var typeCode = Type.GetTypeCode(type); 
        return typeCode == TypeCode.Byte ||
               typeCode == TypeCode.SByte ||
               typeCode == TypeCode.UInt16 ||
               typeCode == TypeCode.UInt32 ||
               typeCode == TypeCode.UInt64 ||
               typeCode == TypeCode.Int16 ||
               typeCode == TypeCode.Int32 ||
               typeCode == TypeCode.Int64 ||
               typeCode == TypeCode.Decimal ||
               typeCode == TypeCode.Double ||
               typeCode == TypeCode.Single;
    }

    public static bool IsCollection(this Type type)
    {
        return type.GetInterfaces().Any(x => 
            CollectionTypes.Contains(x) || (x.IsGenericType && CollectionTypes.Contains(x.GetGenericTypeDefinition())));
    }

    public static bool IsString(this Type type)
    {
        var t = Type.GetTypeCode(type);
        return t == TypeCode.String || t == TypeCode.Char;
    }

    public static bool IsBool(this Type type) => Type.GetTypeCode(type) == TypeCode.Boolean;

    public static bool IsDate(this Type type) => Type.GetTypeCode(type) == TypeCode.DateTime;

    public static bool IsArray(this Type type) => type.IsArray;

    public static bool IsObject(this Type type) => type.Name == "Object";

    public static Type? GetArrayType(this Type type)
    {
        if (!type.IsArray) throw new Exception("Type is not an array, therefore inner cannot be retrieved");
        return type?.GetElementType();
    }
}