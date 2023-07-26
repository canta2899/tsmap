using System.Reflection;
using TypescriptMapper.Extensions;
using TypeExtensions = TypescriptMapper.Extensions.TypeExtensions;

namespace TypescriptMapper;

public class Converter
{
    internal static string FallbackType => "any";
    
    internal static TsType ToTsType(Type type)
    {
        if (type.IsNumeric()) return TsType.Number;

        if (type.IsString()) return TsType.String;

        if (type.IsBool()) return TsType.Boolean;

        if (type.IsObject()) return TsType.Any;

        if (type.IsGenericType && type.Name is "Nullable`1") return TsType.Nullable;

        if (type.IsArray()) return TsType.Array;

        if (type.IsCollection()) return TsType.Collection;

        if (type.IsGenericType) return TsType.Generic;

        if (type.IsDate()) return TsType.Date;
        
        if (type.IsClass || type.IsInterface) return TsType.Object;

        throw new TypeMapException();;
    }

    internal static string ToCamelCasePropertyName(PropertyInfo propertyInfo)
    {
        return propertyInfo.Name.ToCamelCase();
    }

    private static string NormalizeGenericTypeName(string typeName) => typeName.Split("`")[0];

    internal static string GetGenericDefinition(Type type)
    {
        var genericTypes = type.GetGenericArguments().Select(x => MapToTsType(x));
        return $"{NormalizeGenericTypeName(type.Name)}<{string.Join(", ", genericTypes)}>";
    }

    internal static string GetCollectionDefinition(Type type)
    {
        var genericArg = type.GetGenericArguments().FirstOrDefault();
        var safeTypeName = GetSafeTypeName(genericArg.Name);
        return $"{safeTypeName}[]";
    }

    internal static string GetArrayDefinition(Type type)
    {
        var arrayType = type.GetArrayType();
        var mapType = arrayType is not null ? MapToTsType(arrayType) : "";
        return $"{mapType}[]";
    }

    internal static string GetNullableDefinition(Type type)
    {
        
        var generic = type.GetGenericArguments().FirstOrDefault();
        var genericName = generic is not null ? MapToTsType(generic) : "";
        var safeTypeName = GetSafeTypeName(genericName);
        return $"{safeTypeName} | null";
    }

    private static string GetSafeTypeName(string typeName) => string.IsNullOrEmpty(typeName) ? "any" : typeName;
    
    internal static string MapToTsType(Type type)
    {
        var tsType = ToTsType(type);
        
        switch (tsType)
        {
            case TsType.Date:
            case TsType.Number:
            case TsType.String:
            case TsType.Boolean:
            case TsType.Any:
                return tsType.GetAttribute<TsTypeNameAttribute>().Name;
            
            case TsType.Nullable:
                return GetNullableDefinition(type);
            
            case TsType.Array:
                return GetArrayDefinition(type);
            
            case TsType.Collection:
                return Converter.GetCollectionDefinition(type);
            
            case TsType.Generic:
                return GetGenericDefinition(type);
            
            case TsType.Object:
                return type.Name;
            
            default:
                return FallbackType;
        }
    }
}