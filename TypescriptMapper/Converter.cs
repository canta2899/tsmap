using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using TypescriptMapper.Extensions;
using TypeExtensions = TypescriptMapper.Extensions.TypeExtensions;

namespace TypescriptMapper;

public class Converter
{
    private readonly Dictionary<Guid, string>? _allowedTypes;

    public Converter(IEnumerable<Type> expandTypes)
    {
        _allowedTypes = new();
        expandTypes.Aggregate(_allowedTypes, (acc, next) =>
        {
            acc.TryAdd(next.GUID, next.Name);
            return acc;
        });
    }

    public Converter()
    {
        _allowedTypes = null;
    }

    internal string FallbackType => "any";
    
    internal TsType ToTsType(Type type)
    {
        if (type.IsNumeric()) return TsType.Number;

        if (type.IsString()) return TsType.String;

        if (type.IsBool()) return TsType.Boolean;

        if (type.IsObject()) return TsType.Any;

        if (type is { IsGenericType: true, Name: "Nullable`1" }) return TsType.Nullable;

        if (type.IsArray()) return TsType.Array;

        if (type.IsCollection()) return TsType.Collection;

        if (type.IsGenericType) return TsType.Generic;

        if (type.IsDate()) return TsType.Date;

        if (type.IsClass || type.IsInterface)
            return IsTypeExpansionAllowed(type) ? TsType.Object : TsType.Any;

        throw new TypeMapException($"Unable to map {type.Name} to typescript");;
    }

    private bool IsTypeExpansionAllowed(Type t)
    {
        return _allowedTypes is null || _allowedTypes.TryGetValue(t.GUID, out var _);
    }

    internal string NormalizeTypeName(Type t) => t.IsGenericType ? t.Name.Split("`")[0] : t.Name;

    internal string GetGenericDefinition(Type type)
    {
        var genericTypes = type.GetGenericArguments().Select(x => MapToTsType(x));
        return $"{NormalizeTypeName(type)}<{string.Join(", ", genericTypes)}>";
    }

    internal string GetCollectionDefinition(Type type)
    {
        var genericArg = type.GetGenericArguments().FirstOrDefault();
        var mappedGenericArg = MapToTsType(genericArg);
        var safeTypeName = GetSafeTypeName(mappedGenericArg);
        return $"{safeTypeName}[]";
    }

    internal string GetArrayDefinition(Type type)
    {
        var arrayType = type.GetArrayType();
        var mapType = arrayType is not null ? MapToTsType(arrayType) : "";
        return $"{mapType}[]";
    }

    internal string GetNullableDefinition(Type type)
    {
        
        var generic = type.GetGenericArguments().FirstOrDefault();
        var genericName = generic is not null ? MapToTsType(generic) : "";
        var safeTypeName = GetSafeTypeName(genericName);
        return $"{safeTypeName} | null";
    }

    private string GetSafeTypeName(string typeName) => string.IsNullOrEmpty(typeName) ? "any" : typeName;

    internal string? GetExtendedType(Type t)
    {
        if (t.BaseType is null || t.BaseType == typeof(Object)) return null;

        if (_allowedTypes is null)
            return MapToTsType(t.BaseType);
        
        if (_allowedTypes.TryGetValue(t.BaseType.GUID, out var _))
            return MapToTsType(t.BaseType);
        
        return null;
    }
    
    internal string MapToTsType(Type type)
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
                return GetCollectionDefinition(type);
            
            case TsType.Generic:
                return GetGenericDefinition(type);
            
            case TsType.Object:
                return NormalizeTypeName(type);
            
            default:
                return FallbackType;
        }
    }
}