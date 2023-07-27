using System.Reflection;
using TypescriptMapper.Attributes;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("TypescriptMapper.Test")]
namespace TypescriptMapper;
public class Mapper
{
    public string Map<T>()
    {
        var typeEntry = new TypeMapper(typeof(T));

        return Formatter.ToTsInterface(typeEntry);
    }

    public string Map(Type t)
    {
        var typeEntry = new TypeMapper(t);
        return Formatter.ToTsInterface(typeEntry);
    }

    public void MapAssembly(Assembly assembly, TextWriter tw)
    {
        var mappableTypes = GetMappableTypes(assembly);
        
        foreach (var t in mappableTypes)
        {
            var mappedType = Map(t);
            tw.WriteLine(mappedType + "\n");
        }
    }

    public IEnumerable<Type> GetMappableTypes(Assembly assembly)
    {
        var attributeType = typeof(TsMapAttribute);
        return assembly.GetTypes().Where(x => Attribute.IsDefined(x, attributeType));
    }

}