using System.Reflection;

namespace TypescriptMapper;

public class PropertyEntry
{
    private readonly PropertyInfo _propertyInfo;

    public string Name => _propertyInfo.Name.ToCamelCase();
    public string TsType => Converter.MapType(_propertyInfo.PropertyType);
    
    public PropertyEntry(PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;
    }
    
    
}