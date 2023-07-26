using System.Reflection;
using TypescriptMapper.Attributes;

namespace TypescriptMapper;

public class TypeEntry
{
    private readonly Type _t;
    public TypeEntry(Type type)
    {
        _t = type;
    }

    public string Name => _t.Name;
    public IEnumerable<PropertyEntry> Fields => GetMappableProperties(_t);
    
    private IEnumerable<PropertyEntry> GetMappableProperties(Type type)
    {
        return type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x =>
                {
                    return !x
                        .GetCustomAttributes()
                        .Select(x => x.GetType()).Contains(typeof(TsExcludeAttribute));
                })
            .Select(x => new PropertyEntry(x));
    }
}