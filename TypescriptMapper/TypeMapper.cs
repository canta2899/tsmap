using System.Reflection;
using TypescriptMapper.Attributes;

namespace TypescriptMapper;

public class TypeMapper
{
    private readonly Type _t;
    public TypeMapper(Type type)
    {
        _t = type;
    }

    public string Name => GetTypeName();
    public IEnumerable<PropertyMapper> Fields => GetMappableProperties();

    private string GetTypeName()
    {
        return _t.IsGenericType ? Converter.GetGenericDefinition(_t) : _t.Name;
    }
    
    private IEnumerable<PropertyMapper> GetMappableProperties()
    {
        return _t 
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x =>
                {
                    return !x
                        .GetCustomAttributes()
                        .Select(x => x.GetType()).Contains(typeof(TsExcludeAttribute));
                })
            .Select(x => new PropertyMapper(x));
    }
}