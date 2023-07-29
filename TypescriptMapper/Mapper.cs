using System.Reflection;
using TypescriptMapper.Annotations;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using TypescriptMapper.Extensions;

[assembly:InternalsVisibleTo("TypescriptMapper.Test")]
namespace TypescriptMapper;
public class Mapper
{
    private readonly IMappingConfiguration _configuration;

    public Mapper(IMappingConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Mapper()
    {
        _configuration = new DefaultMappingConfiguration();
    }
    
    public string Map<T>()
    {
        return Map(typeof(T));
    }

    private IEnumerable<TsEnumField> GetEnumFields(Type t)
    {
        return
            t
            .GetFields()
            .Where(x => x.Name != "value__")
            .Select(x => new TsEnumField() { Name = x.GetNameOrDefault() });
    }

    private IEnumerable<TsInterfaceField> GetInterfaceFields(IEnumerable<PropertyInfo> mappableProps)
    {
        return mappableProps.Select(x => new TsInterfaceField()
        {
            Name = x.GetNameOrDefault(),
            Type = Converter.MapToTsType(x.PropertyType)
        });
    }

    private TsInterface GetTsInterface(Type t)
    {
        var name = Converter.NormalizeTypeName(t);
        var extends = Converter.GetExtendedType(t) ?? "";
        var properties = GetInterfaceFields(GetMappableProperties(t));

        return new TsInterface()
        {
            Name = name,
            Extends = extends,
            Fields = properties,
        };
    }

    public string Map(Type t)
    {
        if (t.IsEnum)
        {
            var fields = GetEnumFields(t);
            var entry = new TsEnum()
            {
                Name = t.Name,
                Fields = fields
            };

            return Formatter.WriteTsEnum(entry, _configuration);
        }

        var tsInterface = GetTsInterface(t);
        return Formatter.WriteTsInterface(tsInterface, _configuration);
    }

    public void MapAssembly(Assembly assembly, TextWriter tw)
    {
        var mappableTypes = GetMappableTypes(assembly);
        var expandableTypes = mappableTypes.ToDictionary(x => x.GUID, x => x.Name);
        
        foreach (var t in mappableTypes)
        {
            var mappedType = Map(t);
            tw.WriteLine(mappedType + "\n");
        }
    }

    public void MapAssemblyToFile(Assembly assembly, string filePath)
    {
        using var fw = new StreamWriter(filePath);
        MapAssembly(assembly, fw);
    }

    public IEnumerable<Type> GetMappableTypes(Assembly assembly)
    {
        var attributeType = typeof(TsMapAttribute);
        return assembly.GetTypes().Where(x => Attribute.IsDefined(x, attributeType));
    }
    
    private IEnumerable<PropertyInfo> GetMappableProperties(Type t)
    {
        return t
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(x => !IsExcludedProperty(x));
    }

    private bool IsExcludedProperty(PropertyInfo propertyInfo)
    {
        return propertyInfo.GetCustomAttributes().OfType<TsExcludeAttribute>().Any();
    }

}