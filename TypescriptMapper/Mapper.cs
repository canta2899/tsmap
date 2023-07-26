using System.Reflection;
using TypescriptMapper.Attributes;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("TypescriptMapper.Test")]
namespace TypescriptMapper;
public class Mapper 
{
    public string MapInterface<T>()
    {
        var typeEntry = new TypeMapper(typeof(T));

        return Formatter.ToTsInterface(typeEntry);
    }

    public string MapInterface(Type t)
    {
        var typeEntry = new TypeMapper(t);
        return Formatter.ToTsInterface(typeEntry);
    }

    public string MapType<T>()
    {
        var typeEntry = new TypeMapper(typeof(T));
        return Formatter.ToTsType(typeEntry);
    }

    public string MapType(Type t)
    {
        var typeEntry = new TypeMapper(t);
        return Formatter.ToTsType(typeEntry);
    }

    public IEnumerable<Type> GetMappableTypes(Assembly assembly)
    {
        var attributeType = typeof(TsMapAttribute);
        return assembly.GetTypes().Where(x => Attribute.IsDefined(x, attributeType));
    }

}