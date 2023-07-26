using System.Text.RegularExpressions;

namespace TypescriptMapper;

public static class Extensions
{
    public static string ToCamelCase(this string name)
    {
        return Regex.Replace(name, @"(^[A-Z])|(_[a-z]?)", x =>
        {
            if (!string.IsNullOrEmpty(x.Groups[1].Value))
            {
                return x.Groups[1].Value.ToLower();
            }

            return x.Groups[2].Value.Replace("_", "").ToUpper();
        });
    }

    private static string RemoveGenericSymbol(string name)
    {
        return name.Split("`")[0];
    }
}