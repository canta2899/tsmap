namespace TypescriptMapper.Extensions;

public static class EnumExtensions
{
    public static T GetAttribute<T>(this Enum e)
    {
        var fi = e.GetType().GetField(e.ToString());
        return fi.GetCustomAttributes(false).OfType<T>().FirstOrDefault();
    }
}