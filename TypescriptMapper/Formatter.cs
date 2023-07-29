using System.Text;
using TypescriptMapper.Annotations;
using TypescriptMapper.Extensions;
using TypescriptMapper.Model;

namespace TypescriptMapper;

public class Formatter
{
    private const string _indent = "  ";
    public static string WriteTsInterface(TsInterface tsInterface, IMappingConfiguration configuration)
    {
        var sb = new StringBuilder();

        sb.Append("export ");

        sb.Append($"interface {tsInterface.Name} ");
        if (!string.IsNullOrEmpty(tsInterface.Extends)) sb.Append($"extends {tsInterface.Extends} ");
        sb.Append("{\n");
        MapProps(sb, tsInterface.Fields, configuration);
        sb.Append("}");

        return sb.ToString();
    }


    private static string ApplyCasingPolicy(string argName, IMappingConfiguration configuration)
    {
        return configuration.CasingPolicy switch
        {
            CasingPolicy.CamelCase => argName.ToCamelCase(),
            _ => argName
        };
    }
    
    private static void MapProps(StringBuilder sb,
                                 IEnumerable<TsInterfaceField> fields,
                                 IMappingConfiguration configuration)
    {
        foreach (var field in fields)
            sb.Append($"{_indent}{ApplyCasingPolicy(field.Name, configuration)}?: {field.Type};\n");
    }

    public static string WriteTsEnum(TsEnum tsEnum, IMappingConfiguration configuration)
    {
        var sb = new StringBuilder();

        sb.Append("export ");

        sb.Append($"enum {tsEnum.Name} {{\n");
        foreach (var field in tsEnum.Fields)
            sb.Append($"{_indent}{field.Name},\n");
        sb.Append("}");

        return sb.ToString();
    }
}