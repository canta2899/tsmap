using System.Reflection;
using System.Text;

namespace TypescriptMapper;

public class Formatter
{
    private const string _indent = "  ";
    public static string ToTsInterface(TypeMapper typeMapper, bool export = true)
    {
        var sb = new StringBuilder();

        if (export) sb.Append("export ");
        
        sb.Append($"interface {typeMapper.Name} {{\n");
        MapProps(sb, typeMapper.Fields);
        sb.Append("}");

        return sb.ToString();
    }

    private static void MapProps(StringBuilder sb, IEnumerable<PropertyMapper> entries)
    {
        foreach (var property in entries)
        {
            var propName = $"{property.Name}?";
            var propTsType = property.TsType;
            sb.Append($"{_indent}{propName}: {propTsType};\n");
        }
    }
}