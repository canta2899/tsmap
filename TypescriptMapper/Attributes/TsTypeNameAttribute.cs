namespace TypescriptMapper.Attributes;

internal class TsTypeNameAttribute : Attribute
{
    public string Name { get; }
    public TsTypeNameAttribute(string name) : base()
    {
        Name = name;
    }
}
