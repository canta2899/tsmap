using System.Reflection;
using System.Reflection.Metadata;
using TypescriptMapper.Annotations;
using TypescriptMapper.Extensions;
using TypescriptMapper.Attributes;

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

        if (type is { IsGenericType: true, Name: "Nullable`1" }) return TsType.Nullable;

        if (type.IsArray()) return TsType.Array;

        if (type.IsCollection()) return TsType.Collection;

        if (type.IsGenericType)
            return TsType.Generic;

        if (type.IsDate()) return TsType.Date;

        if (type.IsClass || type.IsInterface)
            return TsType.Object;

        throw new TypeMapException($"Unable to map {type.Name} to typescript");;
    }

    internal static string NormalizePropertyName(Type t)
    {
        return t.IsGenericType ? t.Name.Split("`")[0] : t.Name;    
    }

    internal static string NormalizeTypeName(Type t)
    {
        if (t.IsGenericType)
            return $"{NormalizePropertyName(t)}<{string.Join(", ", t.GetGenericTypeDefinition().GetGenericArguments().Select(x => x.Name))}>";

        return t.Name;
    }

    internal static string GetGenericDefinition(Type type)
    {
        var genericTypes = type.GetGenericArguments().Select(x => MapToTsType(x));
        return $"{NormalizePropertyName(type)}<{string.Join(", ", genericTypes)}>";
    }

    internal static string GetCollectionDefinition(Type type)
    {
        var genericArg = type.GetGenericArguments().FirstOrDefault();
        var mappedGenericArg = MapToTsType(genericArg);
        var safeTypeName = GetSafeTypeName(mappedGenericArg);
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

    internal static string? GetExtendedType(Type t)
    {
        if (t.BaseType is null || t.BaseType == typeof(Object)) return null;
        return MapToTsType(t.BaseType);
    }

    private static bool IsTypeMapped(Type type) =>
        type.GetCustomAttributes(false).OfType<TsMapAttribute>().Any();
    
    internal static string MapToTsType(Type type)
    {
        var tsType = ToTsType(type);
        
        if ((tsType == TsType.Generic || tsType == TsType.Object) && !type.IsGenericParameter && !IsTypeMapped(type))
            return FallbackType;
        
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
                return GetCollectionDefinition(type);
            
            case TsType.Generic:
                return GetGenericDefinition(type);
            
            case TsType.Object:
                return NormalizePropertyName(type);
            
            default:
                return FallbackType;
        }
    }
}