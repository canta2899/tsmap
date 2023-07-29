using System.Reflection;
using TypescriptMapper.Annotations;

namespace TypescriptMapper.Extensions;

public static class PropertyExtensions
{
    public static string GetNameOrDefault(this MemberInfo member)
    {
        var attr = member.GetCustomAttributes<TsNameAttribute>().FirstOrDefault();
        if (attr is null) return member.Name;

        return string.IsNullOrEmpty(attr.Name) ? member.Name : attr.Name;
    }
}