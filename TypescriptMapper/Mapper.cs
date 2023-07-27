using System.Reflection;
using TypescriptMapper.Annotations;
using System.Runtime.CompilerServices;

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
        return Map(typeof(T), new Converter());
    }
    
    public string Map<T>(Converter c)
    {
        return Map(typeof(T), c);
    }

    public string Map(Type t)
    {
        return Map(t, new Converter());
    }

    public string Map(Type t, Converter c)
    {
        if (t.IsEnum) return Formatter.WriteTsEnum(new TsEnum()
        {
            Name = t.Name,
            Fields = t.GetFields().Where(x => x.Name != "value__").Select(x => new TsEnumField() { Name = x.Name })
        }, _configuration);
        
        var properties = GetMappableProperties(t).Select(x => new TsInterfaceField()
        {
            Name = x.Name,
            Type = c.MapToTsType(x.PropertyType)
        });
        
        var name = c.NormalizeTypeName(t);
        var extends = c.GetExtendedType(t) ?? "";
        
        return Formatter.WriteTsInterface(new TsInterface()
        {
            Name = name,
            Extends = extends,
            Fields = properties
        }, _configuration);
    }

    public void MapAssembly(Assembly assembly, TextWriter tw)
    {
        var mappableTypes = GetMappableTypes(assembly);
        var converter = new Converter(mappableTypes);
        
        foreach (var t in mappableTypes)
        {
            var mappedType = Map(t, converter);
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