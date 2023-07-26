using System.Reflection;
using System.Text;

namespace TypescriptMapper;

public class Formatter
{
    public static string ToTsInterface(TypeEntry typeEntry, bool export = true)
    {
        var sb = new StringBuilder();

        if (export) sb.Append("export ");
        
        sb.Append($"interface {typeEntry.Name} {{\n");
        MapProps(sb, typeEntry.Fields);
        sb.Append("}");

        return sb.ToString();
    }

    public static string ToTsType(TypeEntry typeEntry, bool export = true)
    {
        var sb = new StringBuilder();

        if (export) sb.Append($"export ");

        sb.Append($"type {typeEntry.Name} = {{\n");
        MapProps(sb, typeEntry.Fields);
        sb.Append("}");

        return sb.ToString();
    }

    private static void MapProps(StringBuilder sb, IEnumerable<PropertyEntry> entries)
    {
        foreach (var property in entries)
        {
            var propName = $"{property.Name}?";
            var propTsType = property.TsType;
            sb.Append($"\t{propName}: {propTsType};\n");
        }
    }
}