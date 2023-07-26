using System.Text.RegularExpressions;

namespace TypescriptMapper.Extensions;

public static class StringExtensions 
{
    public static string ToCamelCase(this string name)
    {
        return Regex.Replace(name, @"(^[A-Z])|(_[a-z]?)|(_[A-Z]?)", x =>
        {
            if (!string.IsNullOrEmpty(x.Groups[1].Value))
                return x.Groups[1].Value.ToLower();
            
            return x.Groups[2].Value.Replace("_", "").ToUpper();
        });
    }
}