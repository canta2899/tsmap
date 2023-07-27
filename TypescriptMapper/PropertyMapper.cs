using System.Diagnostics;
using System.Reflection;
using TypescriptMapper.Extensions;
using TypeExtensions = TypescriptMapper.Extensions.TypeExtensions;

namespace TypescriptMapper;

public class PropertyMapper
{
    private readonly PropertyInfo _propertyInfo;

    public string Name => Converter.ToCamelCasePropertyName(_propertyInfo);
    public string TsType => Converter.MapToTsType(_propertyInfo.PropertyType);
    
    public PropertyMapper(PropertyInfo propertyInfo)
    {
        _propertyInfo = propertyInfo;
    }
}